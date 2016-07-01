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
    public class AddressService : IAddressService
    {
        public IBlockchainDataProvider BlockchainDataProvider { get; set; }
        public IBlockRepository BlockRepository { get; set; }
        public IAddressRepository AddressRepository { get; set; }
        public ILog Log { get; set; }

        public AddressService(IBlockchainDataProvider blockchainProvider,
                            IBlockRepository blockRepository,
                            IAddressRepository addressRepository, 
                            ILog log)
        {
            BlockchainDataProvider = blockchainProvider; 
            BlockRepository = blockRepository;
            AddressRepository = addressRepository;
            Log = log;
        }
         
        public async Task<Address> GetAddress(string id)
        { 
            Address address;

            try
            {
                address = await BlockchainDataProvider.GetAddress(id);

                if(address != null)
                {
                    await AddressRepository.UpdateAddress(address);
                }
            }
            catch(Exception ex)
            {
                await Log.WriteError(this.GetType().ToString(), "GetAddress", $"address_id:{id}", ex, DateTime.Now);
                return null;
            }

            return address;
        }
    }
} 