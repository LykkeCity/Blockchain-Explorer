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
    public class BlockRepository : IBlockRepository, IDisposable
    {
        private Orm.Entities _context;

        public BlockRepository()
        {
            _context = new Orm.Entities();
        }

        public async Task<Block> GetById(string id)
        {
            return await Task.Run<Block>(() =>
            {
                var blockRecord = _context.GetBlockById(id).SingleOrDefault();

                if (blockRecord == null) return null; 

                var block = JsonConvert.DeserializeObject<Block>(blockRecord.SerializedData);

                return block;
            });
        }

        public async Task<bool> IsImported(string id)
        {
            return await Task.Run<bool>(() =>
            {
                return IsBlockImported(id);
            });
        }

        private bool IsBlockImported(string id)
        {
            return _context.IsBlockImported(id).First().GetValueOrDefault();
        }

        public async Task SetAsImported(string id)
        {
            await Task.Run(() =>
            {
                SetBlockAsImported(id);
            });
        }

        private void SetBlockAsImported(string id)
        {
            _context.SetBlockAsImported(id);
        }

        public async Task Save(Block entity)
        {
            await Task.Run(() =>
            {
                saveEntity(entity);
            });
        }

        public async Task SaveAsImport(Block entity)
        {
            await Task.Run(() =>
            {
                saveEntity(entity, isImported: true);
            });
        }

        private void saveEntity(Block entity, bool isImported = false)
        {
            var serializedEntity = JsonConvert.SerializeObject(entity);

            _context.InsertBlock(entity.Hash, entity.Height, entity.Time, entity.Confirmations, entity.Difficulty, 
                   entity.MerkleRoot, entity.Nonce, entity.TotalTransactions, isImported, entity.PreviousBlock, serializedEntity);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
