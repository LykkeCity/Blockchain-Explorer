using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.BitcoinNinja;
using Newtonsoft.Json;


namespace Sevices.BitcoinNinja.Models
{
    public class TransactionNinjaModel: ITransactionNinja
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
        public IDictionary<string, Asset> Asset => Create(DeserializeInputs, DeserializeOutputs);

        public long TotalOut
        {
            get
            {
                return DeserializeOutputs.Sum(x => x.Value);
            }
        }


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

        public void CleanupOutputs()
        {
            var map = new Dictionary<string, DeserializeOutputsNinja>();

            foreach (var i in DeserializeOutputs)
            {
                if (i.Value == 0 || String.IsNullOrWhiteSpace(i.Address)) continue;

                if(map.ContainsKey(i.Address))
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
}


