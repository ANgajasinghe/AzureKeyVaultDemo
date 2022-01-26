
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVaultFunction.Config
{
    public class AppConfiguration
    {
        public string Name { get; set; }
        public int AmountOfRetries { get; set; }
    }
}
