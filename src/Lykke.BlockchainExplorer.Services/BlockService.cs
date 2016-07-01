using Lykke.BlockchainExplorer.Core.Contracts;
using Lykke.BlockchainExplorer.Core.Contracts.Services;
using Lykke.BlockchainExplorer.Core.Domain;
using Lykke.BlockchainExplorer.Core.Log;
using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Services
{
    public class BlockService : IBlockService
    {
        public IBlockchainDataProvider BlockchainDataProvider { get; set; }
        public IBlockRepository BlockRepository { get; set; }
        public IAssetRepository AssetRepository { get; set; }
        public ILog Log { get; set; }

        public BlockService(IBlockchainDataProvider blockchainProvider,
                            IBlockRepository blockRepository,
                            ILog log)
        {
            BlockchainDataProvider = blockchainProvider;
            BlockRepository = blockRepository;
            Log = log;
        } 

        public async Task<Block> GetBlock(string id)
        {
            Block block;

            try
            {
                block = await BlockRepository.GetById(id);

                if(block == null) 
                { 
                    block = await BlockchainDataProvider.GetBlock(id);

                    if(block != null)
                    {
                        await BlockRepository.Save(block);
                    }
                }
            }
            catch (Exception ex)
            {
                await Log.WriteError(this.GetType().ToString(), "GetBlock", $"block_id:{id}", ex, DateTime.Now);
                return null;
            }

            return block;
        }

        public async Task<Block> GetLastBlock()
        {
            Block block;

            try
            {
                block = await BlockchainDataProvider.GetLastBlock();
            }
            catch(Exception ex)
            {
                await Log.WriteError(this.GetType().ToString(), "GetLastBlock", $"last_block", ex, DateTime.Now);
                return null;
            }

            return block;
        }
    } 
}