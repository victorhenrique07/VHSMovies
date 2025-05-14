using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHSMovies.Infrastructure
{
    public class DbConfigurationManager
    {
        private static DbConfigurationManager instance;

        private DbConfigurationManager() { }

        public static DbConfigurationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbConfigurationManager();
                }
                return instance;
            }
        }

        public string GetConfigurationValue(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
