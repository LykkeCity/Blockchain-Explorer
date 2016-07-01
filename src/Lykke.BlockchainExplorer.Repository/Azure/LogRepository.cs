using Lykke.BlockchainExplorer.Core.Log;
using Lykke.BlockchainExplorer.Repository.AzureEntities;
using Lykke.BlockchainExplorer.Repository.AzureTables;
using Lykke.BlockchainExplorer.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Repository.Azure
{
    public class LogRepository : ILog
    {
        private const string TableName = "Log";
        private IAzureTableStorage<LogEntity> _logTable;

        public LogRepository()
        {
            var connectionString = AppSettings.AzureStorageConnectionString;
            _logTable = new AzureTableStorage<LogEntity>(connectionString, TableName, this);
        }

        public async Task WriteError(string component, string process, string context, Exception exeption, DateTime? dateTime = default(DateTime?))
        {
            var jsonData = serializeErrorData(component, process, context, exeption, dateTime);

            var log = LogEntity.Create("Error", jsonData);

            await _logTable.InsertOrReplaceAsync(log);
        }

        public async Task WriteFatalError(string component, string process, string context, Exception exeption, DateTime? dateTime = default(DateTime?))
        {
            var jsonData = serializeErrorData(component, process, context, exeption, dateTime);

            var log = LogEntity.Create("FatalError", jsonData);

            await _logTable.InsertOrReplaceAsync(log);
        }

        public async Task WriteInfo(string component, string process, string context, string info, DateTime? dateTime = default(DateTime?))
        {
            var jsonData = serializeErrorData(component, process, context, null, dateTime);

            var log = LogEntity.Create("Info", jsonData);

            await _logTable.InsertOrReplaceAsync(log);
        }

        public async Task WriteWarning(string component, string process, string context, string info, DateTime? dateTime = default(DateTime?))
        {
            var jsonData = serializeErrorData(component, process, context, null, dateTime);

            var log = LogEntity.Create("Warning", jsonData);

            await _logTable.InsertOrReplaceAsync(log);
        }

        private string serializeErrorData(string component, string process, string context, Exception exeption, DateTime? dateTime = default(DateTime?))
        {
            var errorData = new
            {
                Component = component,
                Process = process,
                Context = context,
                Exception = exeption,
                DateTime = dateTime
            };

            return JsonConvert.SerializeObject(errorData);
        }
    }
}