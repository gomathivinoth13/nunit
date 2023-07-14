using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAccountDataProcessorFunctionApp.Implementation
{
    public class ConfigurationDAL
    {
        protected string _connectionString = Environment.GetEnvironmentVariable("DataBaseConnectionString");

    }
}
