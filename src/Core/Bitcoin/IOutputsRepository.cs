using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Bitcoin
{
    public interface IOutput
    {
        double Value { get; }
        string Addresses { get; }
        string BlockHash { get; }
        string Txid { get; }
        DateTime Time { get; }
    }

    public class Output : IOutput
    {
        public double Value { get; set; }
        public string Addresses { get; set; }
        public string BlockHash { get; set; }
        public string Txid { get; set; }
        public DateTime Time { get; set; }
    }

    public interface IOutputsRepository
    {
        Task SaveAsync(IOutput output);
        Task<IEnumerable<IOutput>> GetAsync(string hashBlock);
    }

}