using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SEG.EagleEyeLibrary.Controllers
{
    public class RedisConnectorHelper
    {
        static string _localHost;

        public static string localHost
        {
            get
            {
                return _localHost;
            }

            set
            {
                _localHost = value;
            }
        }
        static RedisConnectorHelper()
        {
            RedisConnectorHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                //set min thread pool 
                ThreadPool.SetMinThreads(200, 200);

                ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
                return ConnectionMultiplexer.Connect(localHost);
            });
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }


        //if (RedisConn == null)
        //{ 
        //    ConfigurationOptions option = new ConfigurationOptions
        //    {
        //        AbortOnConnectFail = false,
        //        EndPoints = { redisEndpoint }
        //    };
        //RedisConn = ConnectionMultiplexer.Connect(option);
        //}

        //private static Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    string _cacheConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString", EnvironmentVariableTarget.Process);
        //    return ConnectionMultiplexer.Connect(_cacheConnectionString);
        //}, LazyThreadSafetyMode.PublicationOnly);

        //public static ConnectionMultiplexer GetConnection()
        //{
        //    return _lazyConnection.Value;
        //}


        // Redis database
        //private IDatabase redisCache;
        //private static Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        //{
        //    ThreadPool.SetMinThreads(100, 100);

        //    string _cacheConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString", EnvironmentVariableTarget.Process);
        //    var configurationOptions = ConfigurationOptions.Parse(_cacheConnectionString);

        //    configurationOptions.SyncTimeout = 10000;
        //    return ConnectionMultiplexer.Connect(configurationOptions);
        //}, LazyThreadSafetyMode.PublicationOnly);

        //public static ConnectionMultiplexer GetConnection()
        //{
        //    return _lazyConnection.Value;
        //}

    }
}
