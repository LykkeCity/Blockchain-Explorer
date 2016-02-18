using Newtonsoft.Json;

namespace Sevices.Bitcoin.Models
{
        public class TransportRpcModel<T>
        {
            [JsonProperty("result")]
            public T Result { get; set; }
        }
}