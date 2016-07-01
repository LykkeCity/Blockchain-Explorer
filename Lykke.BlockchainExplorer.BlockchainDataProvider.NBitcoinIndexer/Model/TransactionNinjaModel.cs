using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.BlockchainDataProvider.NBitcoinIndexer.Model
{
    internal class TransactionNinjaModel
    {
        [JsonProperty("transactionId")]
        public string TxId { get; set; }
        [JsonProperty("block")]
        public BlockInfoForDetalisTransactionNinja Block { get; set; }
        [JsonProperty("spentCoins")]
        public DeserializeInputsNinja[] DeserializeInputs { get; set; }
        [JsonProperty("receivedCoins")]
        public DeserializeOutputsNinja[] DeserializeOutputs { get; set; }
        [JsonProperty("transaction")]
        public string Hex { get; set; }
        [JsonProperty("fees")]
        public long Fees { get; set; }

        public bool IsCoinBase { get; set; }
        public bool IsColor { get; set; }
        public IDictionary<string, Asset> Asset => GenerateAssets(DeserializeInputs, DeserializeOutputs);

        public long TotalOut
        {
            get
            {
                return DeserializeOutputs.Sum(x => x.Value);
            }
        }

        public Uri TransactionUrl { get; set; }


        private IDictionary<string, Asset> Create(DeserializeInputsNinja[] inputs, DeserializeOutputsNinja[] outputs)
        {
            var asset = new Dictionary<string, Asset>();

            if (inputs.Any(itm => itm.AssetId != null))
            {
                foreach (var itm in inputs.Where(itm => itm.AssetId != null))
                {
                    asset.Add(itm.AssetId,
                        new Asset
                        {
                            AssetDataInput = inputs.Where(itmm => itmm.AssetId == itm.AssetId),
                            AssetDataOutput = outputs.Where(itmm => itmm.AssetId == itm.AssetId)
                        });
                }
            }
            else
            {
                foreach (var itm in outputs.Where(itm => itm.AssetId != null))
                {
                    asset.Add(itm.AssetId,
                        new Asset
                        {
                            AssetDataInput = inputs.Where(itmm => itmm.AssetId == itm.AssetId),
                            AssetDataOutput = outputs.Where(itmm => itmm.AssetId == itm.AssetId)
                        });
                }
            }

            return asset;
        }

        public IDictionary<string, Asset> GenerateAssets(DeserializeInputsNinja[] inputs, DeserializeOutputsNinja[] outputs)
        {
            var dict = new Dictionary<string, Asset>();

            foreach (var input in inputs.Where(x => x.AssetId != null))
            {
                if (dict.ContainsKey(input.AssetId))
                {
                    (dict[input.AssetId].AssetDataInput as List<DeserializeInputsNinja>).Add(input);
                }
                else
                {
                    var na = new Asset()
                    {
                        AssetDataInput = new List<DeserializeInputsNinja>(),
                        AssetDataOutput = new List<DeserializeOutputsNinja>()
                    };

                    (na.AssetDataInput as List<DeserializeInputsNinja>).Add(input);

                    dict.Add(input.AssetId, na);
                }
            }

            foreach (var output in outputs.Where(x => x.AssetId != null))
            {
                if (dict.ContainsKey(output.AssetId))
                {
                    (dict[output.AssetId].AssetDataOutput as List<DeserializeOutputsNinja>).Add(output);
                }
                else
                {
                    var na = new Asset()
                    {
                        AssetDataInput = new List<DeserializeInputsNinja>(),
                        AssetDataOutput = new List<DeserializeOutputsNinja>()
                    };

                    (na.AssetDataOutput as List<DeserializeOutputsNinja>).Add(output);

                    dict.Add(output.AssetId, na);
                }
            }

            return dict;
        }

        public void CalculateInputsWithReturnedChange()
        {
            foreach (var item in DeserializeInputs)
            {
                if (DeserializeOutputs.Any(x => x.Address == item.Address))
                {
                    var outputs = DeserializeOutputs.Where(x => x.Address == item.Address && x.AssetId == item.AssetId);

                    foreach (var output in outputs)
                    {
                        item.Value -= output.Value;
                        item.Quantity -= output.Quantity;

                        var newOutputs = new List<DeserializeOutputsNinja>(DeserializeOutputs);
                        newOutputs.Remove(output);
                        DeserializeOutputs = newOutputs.ToArray();
                    }
                }
            }
        }

        public void CalculateQuantitiesWithReturnsForAssets()
        {
            foreach (var item in DeserializeInputs.Where(x => x.AssetId != null))
            {
                if (DeserializeOutputs.Any(x => x.Address == item.Address))
                {
                    var output = DeserializeOutputs.Single(x => x.Address == item.Address);
                    item.Quantity -= output.Quantity;

                    var newOutputs = new List<DeserializeOutputsNinja>(DeserializeOutputs);
                    newOutputs.Remove(output);
                    DeserializeOutputs = newOutputs.ToArray();
                }
            }
        }

        public void CleanupOutputs()
        {
            var map = new Dictionary<string, DeserializeOutputsNinja>();

            foreach (var i in DeserializeOutputs)
            {
                if (i.Value == 0 || String.IsNullOrWhiteSpace(i.Address)) continue;

                if (map.ContainsKey(i.Address))
                {
                    map[i.Address].Value += i.Value;
                }
                else
                {
                    map.Add(i.Address, i);
                }
            }

            DeserializeOutputs = map.Select(x => x.Value).ToArray();
        }
    }

    internal class DeserializeOutputsNinja
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("transactionId")]
        public string TxId { get; set; }
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("value")]
        public long Value { get; set; }
        [JsonProperty("assetId")]
        public string AssetId { get; set; }
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }

    internal class DeserializeInputsNinja
    {
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("transactionId")]
        public string TxId { get; set; }
        [JsonProperty("index")]
        public int Index { get; set; }
        [JsonProperty("value")]
        public long Value { get; set; }
        [JsonProperty("assetId")]
        public string AssetId { get; set; }
        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }

    internal class Asset
    {
        public IEnumerable<DeserializeInputsNinja> AssetDataInput { get; set; }
        public IEnumerable<DeserializeOutputsNinja> AssetDataOutput { get; set; }
    }

    internal class BlockInfoForDetalisTransactionNinja
    {
        [JsonProperty("blockId")]
        public string Hash { get; set; }
        [JsonProperty("height")]
        public long Height { get; set; }
        [JsonProperty("blockTime")]
        public DateTime Time { get; set; }
        [JsonProperty("confirmations")]
        public long Confirmations { get; set; }
    }
}
