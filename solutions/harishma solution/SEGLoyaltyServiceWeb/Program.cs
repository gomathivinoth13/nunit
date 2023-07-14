////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Program.cs
//
// summary:	Implements the program class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SEGLoyaltyServiceWeb
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A program. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Program
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Main entry-point for this application. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="args"> The arguments. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void Main(string[] args)
        {
            
            //var fb = new Firebase.FireBasePush("AIzaSyBZJOEVsRzDSjW9cenUAV_DCKmPNFRfOZw");
            //var results = fb.SendPush(new Firebase.PushMessage() { to = "", notification= new Firebase.PushMessageData() { text="test", title="Test Push" } }).Result;

            BuildWebHost(args).Run();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Builds web host. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="args"> The arguments. </param>
        ///
        /// <returns>   An IWebHost. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .ConfigureAppConfiguration((buildercontext, config) =>
                {
                    IHostingEnvironment env = buildercontext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                })
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
        //Test
    }
}
