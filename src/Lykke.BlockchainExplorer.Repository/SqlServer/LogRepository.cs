using Lykke.BlockchainExplorer.Core.Log;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.BlockchainExplorer.Repository.SqlServer
{
    public class LogRepository : ILog, IDisposable
    {
        private Orm.Entities _context;

        public LogRepository()
        {
            _context = new Orm.Entities();
        }

        public async Task WriteError(string component, string process, string context, Exception exeption, DateTime? dateTime = default(DateTime?))
        {
            await Task.Run(() =>
            {
                var exceptionJson = JsonConvert.SerializeObject(exeption);

                _context.InsertLog("Error", component, process, context, exceptionJson, dateTime);
            });
        }

        public async Task WriteFatalError(string component, string process, string context, Exception exeption, DateTime? dateTime = default(DateTime?))
        {
            await Task.Run(() =>
            {
                var exceptionJson = JsonConvert.SerializeObject(exeption);

                _context.InsertLog("FatalError", component, process, context, exceptionJson, dateTime);
            });
        }

        public async Task WriteInfo(string component, string process, string context, string info, DateTime? dateTime = default(DateTime?))
        {
            await Task.Run(() =>
            {
                _context.InsertLog("Info", component, process, context, $"Info:{info}", dateTime);
            });
        }

        public async Task WriteWarning(string component, string process, string context, string info, DateTime? dateTime = default(DateTime?))
        {
            await Task.Run(() => 
            {
                _context.InsertLog("Warning", component, process, context, $"Warning:{info}", dateTime);
            });
        }

        public void Dispose()
        {
            if(_context != null)
            {
                _context.Dispose();
            }
        }

    }
}
