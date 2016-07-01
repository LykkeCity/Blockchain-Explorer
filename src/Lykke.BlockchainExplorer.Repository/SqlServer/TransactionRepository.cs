using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;
using Newtonsoft.Json;

namespace Lykke.BlockchainExplorer.Repository.SqlServer
{
    public class TransactionRepository : ITransactionRepository, IDisposable
    {
        private Orm.Entities _context;

        public TransactionRepository()
        {
            _context = new Orm.Entities();
        }

        public async Task<Transaction> GetById(string id)
        {
            return await Task.Run<Transaction>(() =>
            {
                return GetTransactionById(id);
            });
        }

        private Transaction GetTransactionById(string id) 
        {
            Transaction transaction = null;

            var transactionRecord = _context.GetTransactionById(id).SingleOrDefault();

            if (transactionRecord != null)
            {
                transaction = JsonConvert.DeserializeObject<Transaction>(transactionRecord.SerializedData);
            }

            return transaction;
        }

        public async Task<bool> IsImported(string id)
        {
            return await Task.Run(() =>
            {
                return IsTransactionImported(id);
            });
        }

        private bool IsTransactionImported(string id)
        {
            var res = _context.IsTransactionImported(id);

            if (res == null) return false;
            return res.Single().GetValueOrDefault();
        }

        public async Task SetAsImported(string id)
        {
            await Task.Run(() =>
            {
                SetTransactionAsImported(id);
            });
        }

        private void SetTransactionAsImported(string id)
        {
            _context.SetTransactionAsImported(id);
        } 

        public async Task Save(Transaction entity)
        {
            await Task.Run(() =>
            {
                SaveTransaction(entity);
            });
        }

        public async Task SaveAsImport(Transaction entity)
        {
            await Task.Run(() =>
            {
                SaveTransaction(entity, isImport: true);
            });
        }

        private void SaveTransaction(Transaction entity, bool isImport = false)
        {
            var serializedEntity = JsonConvert.SerializeObject(entity);

            _context.InsertTransaction(entity.TransactionId, entity.Time, entity.Confirmations, entity.IsColor,
                        entity.IsCoinBase, entity.Hex, entity.Fees, entity.Blockhash, isImport, serializedEntity);

            foreach (var transactionIn in entity.TransactionIn)
            {
                _context.InsertTransactionItem(entity.TransactionId, (int)TransactionItemType.In, transactionIn.Address,
                        transactionIn.Index, transactionIn.Value, transactionIn.AssetId, transactionIn.Quantity);
            }
            
            foreach (var transactionOut in entity.TransactionsOut)
            {
                _context.InsertTransactionItem(entity.TransactionId, (int)TransactionItemType.Out, transactionOut.Address,
                        transactionOut.Index, transactionOut.Value, transactionOut.AssetId, transactionOut.Quantity);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
