using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Core.Repository
{
    public interface IRepository<T>
    {
        Task<T> GetById(string id);
        Task Save(T entity);
    }
}
