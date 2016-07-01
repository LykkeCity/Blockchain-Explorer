using Lykke.BlockchainExplorer.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.BlockchainExplorer.Core.Domain;

namespace Lykke.BlockchainExplorer.Repository.SqlServer
{
    public class AddressRepository : IAddressRepository, IDisposable
    {
        private Orm.Entities _context;

        public AddressRepository()
        {
            _context = new Orm.Entities();
        }

        public async Task<Address> GetById(string id)
        {
            return await Task.Run<Address>(() =>
            {
                return GetAddressById(id);
            }); 
        }

        private Address GetAddressById(string id)
        {
            var addressRecord = _context.GetAddressById(id).SingleOrDefault();

            if (addressRecord == null) return null;

            var address = new Address()
            {
                Hash = addressRecord.Hash,
                ColoredAddress = addressRecord.ColoredAddress,
                UncoloredAddress = addressRecord.UncoloredAddress
            };

            return address;
        }

        public async Task Save(Address entity)
        {
            await Task.Run(() =>
            {
                SaveAddress(entity);
            });
        }

        private void SaveAddress(Address entity)
        {
            _context.InsertAddress(entity.Hash, entity.ColoredAddress, entity.UncoloredAddress, entity.Balance);
        }

        public async Task UpdateAddress(Address address)
        {
            await Task.Run(() =>
            { 
                UpdateAddressData(address);
            });
        } 

        private void UpdateAddressData(Address address)
        {
            _context.UpdateAddress(address.Hash, address.ColoredAddress, address.UncoloredAddress);
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }
}
