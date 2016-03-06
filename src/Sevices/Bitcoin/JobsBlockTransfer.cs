using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Core.Bitcoin;
using Sevices.Bitcoin.Models;

namespace Sevices.Bitcoin
{
    public class JobsBlockTransfer : TimerPeriod
    {
        private readonly SrvBitcoinRpc _bitcoinRpc;
    private readonly ILastImportendBlockHash _lastIportendBlockHash;
    private readonly IBitcoinBlockRepository _bitcoinBlockRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IOutputsRepository _outputsRepository;
    private readonly IInputsRepository _inputsRepository;
    private readonly ILog _log;

    private const string FirstBlock = "000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f";

    public JobsBlockTransfer(ILog log, SrvBitcoinRpc bitcoinRpc, ILastImportendBlockHash lastIportendBlockHash,
        IBitcoinBlockRepository bitcoinBlockRepository, ITransactionRepository transactionRepository, IOutputsRepository outputsRepository, IInputsRepository inputsRepository)
            : base("JobsBlockTransfer", 1, log)
        {
        _bitcoinRpc = bitcoinRpc;
        _lastIportendBlockHash = lastIportendBlockHash;
        _bitcoinBlockRepository = bitcoinBlockRepository;
        _transactionRepository = transactionRepository;
        _outputsRepository = outputsRepository;
        _inputsRepository = inputsRepository;
        _log = log;
    }

    private async Task ReadNewBlocksAsync(Func<GetBlockRpcModel, Task> saveTransactions)
    {

        var hashToImport = await _lastIportendBlockHash.GetAsync();
        if (string.IsNullOrEmpty(hashToImport))
            hashToImport = FirstBlock;

        var block = await _bitcoinRpc.GetBlockAsync(hashToImport);

        while (true)
        {
            var newBlock = new BitcoinBlock
            {
                Hash = block.Hash,
                Time = block.GetTime(),
                Height = block.Height,
                Merkleroot = block.Merkleroot,
                Nonce = block.Nonce,
                Difficulty = block.Difficulty,
                Previousblockhash = block.Previousblockhash,
                Nextblockhash = block.Nextblockhash,
                TotalTransactions = block.Tx.Length
            };
            Console.WriteLine("\n block - " + newBlock.Hash + " room block " + newBlock.Height);
            await _bitcoinBlockRepository.SaveAsync(newBlock);
            await saveTransactions(block);
            await _lastIportendBlockHash.SetAsync(block.Hash, block.Height);

            if (block.IsLastBlock())
            {
                Console.WriteLine("block - " + newBlock.Hash + " room block " + newBlock.Height);
                Console.WriteLine("It was the last block.");
                break;
            }


            block = await _bitcoinRpc.GetBlockAsync(block.Nextblockhash);
 
            }


    }

    private async Task SaveTransactionToDb(GetBlockRpcModel block)
    {
        foreach (var itemTx in block.Tx)
        {

            try
            {
                Console.WriteLine("Transaction - " + itemTx);
                var transaction = await _bitcoinRpc.GetRawTransactionAsync(itemTx);

                var newTransaction = new Transaction
                {
                    Time = transaction.GetTime(),
                    Height = block.Height,
                    Confirmations = transaction.Confirmations,
                    Txid = transaction.Txid,
                    Blockhash = transaction.Blockhash
                };

                await _transactionRepository.SaveAsync(newTransaction);

                await SaveOutputsToDb(transaction);
                await SaveInputsDb(transaction);
                }
            catch (Exception ex)
            {
                    Console.WriteLine("Error - SaveTransactionToDb");
                    await _log.WriteError("JobsBlockTransfer", "SaveTransactionToDb", itemTx, ex);
            }


        }


    }

    private async Task SaveOutputsToDb(GetRawTransactionPrcModel tx)
    {
        foreach (var itmVout in tx.Vout.Where(itm => itm.ScriptPubKey.Addresses != null))
        {

            foreach (var itmAdress in itmVout.ScriptPubKey.Addresses)
            {
                Console.WriteLine("--- Output - " + itmAdress);
                var newOutputs = new Output
                {
                    Time = tx.GetTime(),
                    Txid = tx.Txid,
                    Value = itmVout.Value,
                    Addresses = itmAdress,
                    BlockHash = tx.Blockhash
                };

                await _outputsRepository.SaveAsync(newOutputs);
                }
            }

    }

    private async Task SaveInputsDb(GetRawTransactionPrcModel tx)
    {
        foreach (var itmVin in tx.Vin.Where(itm => itm.Txid != null))
        {
            var addressVin = await _bitcoinRpc.GetRawTransactionAsync(itmVin.Txid);

            foreach (var itmAddressVin in addressVin.Vout)
            {
                if (itmAddressVin.N == itmVin.Vout)
                {

                    foreach (var address in itmAddressVin.ScriptPubKey.Addresses)
                    {
                        Console.WriteLine("------ Input - " + address);
                        var newInputs = new Input
                        {
                            Txid = tx.Txid,
                            Value = itmAddressVin.Value,
                            Addresses = address,
                            BlockHash = tx.Blockhash
                        };

                        await _inputsRepository.SaveAsync(newInputs);
                          
                        }
                }
                }

        }
    }

        protected override async Task Execute()
        {
            await ReadNewBlocksAsync(SaveTransactionToDb);
        }
    }
}
