

CREATE TABLE Core.Block
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Hash] NVARCHAR(50) NOT NULL,
	Height BIGINT NOT NULL,
	[Time] DATETIME NOT NULL,
	Confirmations BIGINT NOT NULL,
	Difficulty FLOAT NOT NULL,
	MerkleRoot NVARCHAR(100) NOT NULL,
	Nonce BIGINT NOT NULL,
	TotalTransactions INT NOT NULL,
	IsImported BIT NOT NULL DEFAULT 0,
	SerializedData NVARCHAR(MAX),
	PreviousBlockHash NVARCHAR(50) NOT NULL,
	PreviousBlockId INT FOREIGN KEY REFERENCES Core.Block(Id)
)

GO

CREATE TABLE Core.Asset
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Hash] NVARCHAR(50) NOT NULL,
	Name NVARCHAR(150) NOT NULL,
	NameShort NVARCHAR(150) NOT NULL,
	[Description] NVARCHAR(MAX),
	DescriptionMime NVARCHAR(150),
	[Type] NVARCHAR(150),
	ContractUrl NVARCHAR(MAX) NOT NULL,
	MetadataUrl NVARCHAR(MAX) NOT NULL,
	FinalMetadataUrl NVARCHAR(MAX) NOT NULL,
	Issuer NVARCHAR(200) NOT NULL,
	VerifiedIssuer BIT DEFAULT 0,
	Divisibility INT,
	IconUrl NVARCHAR(MAX),
	ImageUrl NVARCHAR(MAX),
	[Version] NVARCHAR(150)
)

GO

CREATE TABLE Core.[Transaction]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Hash] NVARCHAR(50) NOT NULL,
	Confirmations BIGINT NOT NULL,
	[Time] DATETIME NOT NULL,
	IsColor BIT NOT NULL DEFAULT 0,
	IsCoinbase BIT NOT NULL DEFAULT 0,
	Hex NVARCHAR(MAX) NOT NULL,
	Fees BIGINT NOT NULL,
	BlockHash NVARCHAR(50) NOT NULL,
	SerializedData NVARCHAR(MAX),
	IsImported BIT NOT NULL DEFAULT 0,
	BlockId INT NOT NULL FOREIGN KEY REFERENCES Core.Block(Id)
)

GO

CREATE TABLE Core.[Address]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Hash] NVARCHAR(100) NOT NULL,
	ColoredAddress NVARCHAR(100),
	UncoloredAddress NVARCHAR(100),
	Balance DECIMAL DEFAULT 0
)

GO

CREATE TABLE Core.TransactionItem
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	[Type] INT NOT NULL FOREIGN KEY REFERENCES [Core.Codebook].TransactionItemType,
	AddressId INT FOREIGN KEY REFERENCES Core.[Address](Id),
	[Index] INT NOT NULL,
	Value BIGINT NOT NULL,
	AssetHash NVARCHAR(50),
	Quantity BIGINT DEFAULT 0,
	TransactionId INT NOT NULL FOREIGN KEY REFERENCES Core.[Transaction](Id),
	AssetId INT FOREIGN KEY REFERENCES Core.Asset(Id)
)

GO

CREATE TABLE Core.AssetOwner
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	AssetId INT NOT NULL FOREIGN KEY REFERENCES Core.Asset(Id),
	AddressId INT NOT NULL FOREIGN KEY REFERENCES Core.[Address](Id),
	Quantity INT DEFAULT 0
)

GO

CREATE TABLE Core.[Log]
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	LogType NVARCHAR(30),
	Component NVARCHAR(MAX),
	Process NVARCHAR(MAX),
	Context NVARCHAR(MAX),
	Data NVARCHAR(MAX),
	[Time] DATETIME
)

GO

CREATE TABLE [Core.Codebook].TransactionItemType
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(30) NOT NULL
)

GO



INSERT INTO [Core.Codebook].TransactionItemType VALUES ('In')
INSERT INTO [Core.Codebook].TransactionItemType VALUES ('Out')

GO

CREATE PROC Core.InsertBlock @hash NVARCHAR(50), @height BIGINT, @time DATETIME, @confirmations BIGINT, @difficulty FLOAT, @merkleRoot NVARCHAR(100), 
						     @nonce BIGINT, @totalTransactions INT, @isImported BIT, @previousBlockHash NVARCHAR(50), @serializedData NVARCHAR(MAX)
AS

	DECLARE @previousBlockId INT
	
	SELECT TOP 1 @previousBlockId = b.Id FROM Core.Block b
	WHERE b.[Hash] = @previousBlockHash

	INSERT INTO Core.Block
	VALUES (@hash, @height, @time, @confirmations, @difficulty, @merkleRoot, @nonce, @totalTransactions, @isImported, @serializedData, @previousBlockHash, @previousBlockId)

GO

CREATE PROC Core.GetBlockById @id NVARCHAR(50)
AS

	SELECT * FROM Core.Block b
	WHERE b.[Hash] = @id

GO

ALTER PROC Core.IsBlockImported @id NVARCHAR(50)
AS

	SELECT CAST(
		CASE WHEN EXISTS(SELECT * FROM Core.Block b WHERE b.[Hash] = @id AND b.IsImported = 1) THEN 1
		ELSE 0
		END
	AS BIT) AS IsImported

GO

CREATE PROC Core.BlockExists @id NVARCHAR(50)
AS

	SELECT CAST(
		CASE WHEN EXISTS(SELECT * FROM Core.Block b WHERE b.[Hash] = @id) THEN 1
		ELSE 0
		END
	AS BIT) AS [Exists]

GO

CREATE PROC Core.SetBlockAsImported @id NVARCHAR(50)
AS

	UPDATE Core.Block 
	SET IsImported = 1
	WHERE [Hash] = @id

GO

ALTER PROC Core.InsertTransaction @hash NVARCHAR(50), @time DATETIME, @confirmations BIGINT, @isColor BIT, @isCoinBase BIT, 
								   @hex NVARCHAR(MAX), @fees BIGINT, @blockHash NVARCHAR(50), @isImported BIT, @serializedData NVARCHAR(MAX)
AS

	DECLARE @blockId INT

	SELECT TOP 1 @blockId = b.Id FROM Core.Block b
	WHERE b.[Hash] = @blockHash

	INSERT INTO Core.[Transaction]
	VALUES (@hash, @confirmations, @time, @isColor, @isCoinBase, @hex, @fees, @blockHash, @serializedData, @isImported, @blockId)

GO

CREATE PROC Core.GetTransactionById @id NVARCHAR(50)
AS

	SELECT TOP 1 * FROM Core.[Transaction] t
	WHERE t.[Hash] = @id

GO

ALTER PROC Core.IsTransactionImported @id NVARCHAR(50)
AS

	SELECT CAST(
		CASE WHEN EXISTS(SELECT * FROM Core.[Transaction] t WHERE t.[Hash] = @id AND t.IsImported = 1) THEN 1
		ELSE 0
		END
	AS BIT) AS IsImported


GO

CREATE PROC Core.SetTransactionAsImported @id NVARCHAR(50)
AS

	UPDATE Core.[Transaction]
	SET IsImported = 1
	WHERE [Hash] = @id

GO

ALTER PROC Core.InsertTransactionItem @transactionHash NVARCHAR(50), @type INT, @address NVARCHAR(100), @index INT, 
									   @value BIGINT, @assetHash NVARCHAR(50), @quantity BIGINT
AS

	DECLARE @transactionId INT, @assetId INT, @addressId INT

	SELECT TOP 1 @transactionId = t.Id FROM Core.[Transaction] t
	WHERE t.[Hash] = @transactionHash

	SELECT TOP 1 @assetId = a.Id FROM Core.Asset a
	WHERE a.[Hash] = @assetHash

	SELECT TOP 1 @addressId = ad.Id FROM Core.[Address] ad
	WHERE ad.[Hash] = @address

	IF @addressId IS NULL AND @address IS NOT NULL
	BEGIN
		INSERT INTO Core.[Address] VALUES ( @address, '', @address, 0)

		SET @addressId = SCOPE_IDENTITY()
	END

	INSERT INTO Core.TransactionItem 
	VALUES (@type, @addressId, @index, @value, @assetHash, @quantity, @transactionId, @assetId)

GO

CREATE PROC Core.GetTransactionItems @transactionId INT, @itemType INT = NULL
AS 

	SELECT	a.[Hash] as [Address],
			ti.[Index],
			ti.Value,
			ast.[Hash] as [AssetId],
			ti.Quantity
	FROM Core.TransactionItem ti
	INNER JOIN Core.[Address] a ON ti.AddressId = a.Id
	INNER JOIN Core.Asset ast ON ti.AssetId = ast.Id
	WHERE ti.TransactionId = @transactionId AND
	      (ti.[Type] = @itemType OR @itemType IS NULL)

GO

CREATE PROC Core.InsertAddress @hash NVARCHAR(100), @coloredAddress NVARCHAR(100), @uncoloredAddress NVARCHAR(100), @balance DECIMAL
AS

	INSERT INTO Core.[Address] 
	VALUES (@hash, @coloredAddress, @uncoloredAddress, @balance)

GO

CREATE PROC Core.UpdateAddress @hash NVARCHAR(100), @coloredAddress NVARCHAR(100), @uncoloredAddress NVARCHAR(100)
AS

	UPDATE Core.[Address]
	SET ColoredAddress = @coloredAddress,
		UncoloredAddress = @uncoloredAddress
	WHERE [Hash] = @hash

GO

CREATE PROC Core.GetAddressById @id NVARCHAR(100)
AS

	SELECT * FROM Core.[Address] a
	WHERE a.[Hash] = @id

GO

CREATE PROC Core.GetAddressAssets @id NVARCHAR(50)
AS

	SELECT  ast.[Hash] AS Asset,
			SUM(ti.Quantity) AS AssetQuantity
	FROM Core.TransactionItem ti
	INNER JOIN Core.[Address] a ON ti.AddressId = a.Id
	INNER JOIN Core.Asset ast ON ti.AssetId = ast.Id
	WHERE a.[Hash] = @id
	GROUP BY ast.[Hash]
	HAVING SUM(ti.Quantity) > 0

GO

CREATE PROC Core.InsertAsset @hash NVARCHAR(50), @name NVARCHAR(150), @shortName NVARCHAR(150), @description NVARCHAR(MAX), 
							 @descriptionMime NVARCHAR(150), @type NVARCHAR(150), @contractUrl NVARCHAR(MAX), @metadataUrl NVARCHAR(MAX),
							 @finalMetadataUrl NVARCHAR(MAX), @issuer NVARCHAR(200), @verifiedIssuer BIT, @divisibility INT,
							 @iconUrl NVARCHAR(MAX), @imageUrl NVARCHAR(MAX), @version NVARCHAR(150)
AS

	INSERT INTO Core.Asset
	VALUES (@hash, @name, @shortName, @description, @descriptionMime, @type, @contractUrl, @metadataUrl, @finalMetadataUrl,
			@issuer, @verifiedIssuer, @divisibility, @iconUrl, @imageUrl, @version)


GO

CREATE PROC Core.AssetExists @id NVARCHAR(50)
AS

	SELECT CAST(
			CASE WHEN EXISTS(SELECT * FROM Core.Asset a WHERE a.[Hash] = @id) THEN 1
			ELSE 0
			END
		AS BIT) AS [Exists]

GO

CREATE PROC Core.GetAssetById @id NVARCHAR(50)
AS

	SELECT * FROM Core.Asset a
	WHERE a.[Hash] = @id

GO

CREATE PROC Core.GetAssetsByTransaction @transactionId int
AS

	SELECT	ast.[Hash] as AssetId,
			ast.MetadataUrl,
			ti.[Index],
			ti.Value,
			ti.Quantity,
			ti.[Type] as TransactionItemType,
			t.Id as TransactionId
	FROM Core.Asset ast
	INNER JOIN Core.TransactionItem ti ON ast.Id = ti.AssetId
	INNER JOIN Core.[Transaction] t ON ti.Transactionid = t.Id
	WHERE t.Id = @transactionId


GO

CREATE PROC Core.GetAssets
AS

	SELECT * FROM Core.Asset

GO

ALTER PROC Core.GetAssetOwners @id NVARCHAR(50)
AS

	SELECT  a.[Hash] AS [Address],
			SUM(ti.Quantity) AS Quantity
	FROM Core.TransactionItem ti
	INNER JOIN Core.[Address] a ON ti.AddressId = a.Id
	INNER JOIN Core.[Asset] ast ON ti.AssetId = ast.Id
	WHERE ti.Quantity != 0 AND ast.[Hash] = @id
	GROUP BY a.[Hash]

GO

ALTER PROC Core.InsertLog @logType NVARCHAR(30), @component NVARCHAR(MAX), @process NVARCHAR(MAX), @context NVARCHAR(MAX), @data NVARCHAR(MAX), @time DATETIME
AS

	INSERT INTO Core.[Log] VALUES ( @logType, @component, @process, @context, @data, @time )
	
GO