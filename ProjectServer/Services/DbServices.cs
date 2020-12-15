using System.ComponentModel;
using Npgsql;
using ProjectServer.Models;

namespace ProjectServer.Services
{
    public class DbServices
    {
        private static DbServices _instance = null;
        private static readonly object instanceLock = new object();

        public static DbServices Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (_instance != null) return _instance;
                    _instance = new DbServices();
                    return _instance;
                }
                
            }

        }

        public ServerDbContext Context { get; }
        
        public DbServices()
        {
            Context = new ServerDbContext();
        }

        public static void Init()
        {
            _instance = new DbServices();
        }
        
    }
}