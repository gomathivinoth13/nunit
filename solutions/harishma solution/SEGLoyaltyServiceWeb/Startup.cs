////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	startup.cs
//
// summary:	Implements the startup class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace SEGLoyaltyServiceWeb
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A startup. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Startup
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="configuration">    The configuration. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            SEG.AzureLoyaltyDatabase.DataAccess.DapperDalBase.ConnectionString = configuration["Settings:LoyaltyConnection"];
            SEG.AzureLoyaltyDatabase.DataAccess.DapperDalBase.ConnectionStringSurvey = configuration["Settings:SurveyConnection"];
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the configuration. </summary>
        ///
        /// <value> The configuration. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public IConfiguration Configuration { get; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="services"> The services. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddJsonOptions(options =>
            {

                var settings = options.SerializerSettings;
                settings.ContractResolver = new NoCamelCaseResolver();
                settings.Formatting = Formatting.Indented;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Azure Loyalty Services", Version = "v2" });

                ///********* add below code to all the APIs ***********//
                c.CustomSchemaIds((type) => type.FullName);
                //var xmlDocs = createCombinedXmlDocumentationFile();

                //if (File.Exists(xmlDocs))
                //    c.IncludeXmlComments(xmlDocs);

            });

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["Settings:CacheConnectionString"];
            });

        }

        string createCombinedXmlDocumentationFile()
        {

            var basePath = AppContext.BaseDirectory;
            var swaggerFile = Path.Combine(basePath, "combinedSwaggerXml.xml");
            try
            {

                XElement xml = null;
                XElement dependentXml = null;

                DirectoryInfo di = new DirectoryInfo(basePath);
                var fi = di.GetFiles("*.xml", SearchOption.AllDirectories);
                foreach (var file in fi)
                {
                    if (xml == null)
                    {
                        xml = XElement.Load(file.FullName);
                    }
                    else
                    {
                        dependentXml = XElement.Load(file.FullName);
                        foreach (XElement ele in dependentXml.Descendants())
                        {
                            xml.Add(ele);
                        }
                    }
                }
                if (xml != null)
                {
                    xml.Save(swaggerFile);

                }

            }
            catch (Exception)
            {
                if (File.Exists(swaggerFile))
                    File.Delete(swaggerFile);

            }
            return swaggerFile;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request
        /// pipeline.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="app">  The application. </param>
        /// <param name="env">  The environment. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Azure Loyalty Services");

            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A no camel case resolver. </summary>
    ///
    /// <remarks>   Mcdand, 2/20/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class NoCamelCaseResolver : CamelCasePropertyNamesContractResolver
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public NoCamelCaseResolver()
        {
            NamingStrategy = new DefaultNamingStrategy();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonDictionaryContract" /> for the given
        /// type.
        /// </summary>
        ///
        /// <remarks>   Mcdand, 2/20/2018. </remarks>
        ///
        /// <param name="objectType">   Type of the object. </param>
        ///
        /// <returns>
        /// A <see cref="T:Newtonsoft.Json.Serialization.JsonDictionaryContract" /> for the given type.
        /// </returns>
        ///
        /// <seealso cref="M:Newtonsoft.Json.Serialization.DefaultContractResolver.CreateDictionaryContract(Type)"/>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);

            contract.DictionaryKeyResolver = propertyName => propertyName;

            return contract;
        }
    }
}
