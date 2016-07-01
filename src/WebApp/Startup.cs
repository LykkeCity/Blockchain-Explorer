using System;
using AzureRepositories;
using Common.Log;
using Core.Bitcoin;
using Core.BitcoinNinja;
using Core.CoinprismApi;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sevices.Bitcoin;
using Sevices.BitcoinNinja;
using Sevices.CoinprismApi;
using Core.Enums;

namespace BitcoinChainExplorerForAspNet5
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            var storageConnectionString = Configuration.Get<string>("ConnectionString");

            if (string.IsNullOrWhiteSpace(storageConnectionString))
            {
                storageConnectionString = "UseDevelopmentStorage=true";
            }

            var bitcoinRpcSettings = new BitcoinRpcSettings
            {
                Url = new Uri(Configuration.Get<string>("RpcHostPort")),
                Password = Configuration.Get<string>("RpcPassword"),
                User = Configuration.Get<string>("RpcUser")
            };

            services.AddInstance(new BitcoinRpcClient(bitcoinRpcSettings));

            var bitcoinNinjaSettings = new BitcoinNinjaSettings
            {
                //UrlMain = new Uri(Configuration.Get<string>("UrlMainNinja")),
                UrlTest = new Uri(Configuration.Get<string>("UrlTestNetNinja")),
                Network = (Network)Enum.Parse(typeof(Network), Configuration.Get<string>("Network"))
            };

            services.AddInstance<IBitcoinNinjaClient>(new BitcoinNinjaClient(bitcoinNinjaSettings));

            var coniprismApiSettings = new CoinprismApiSettings
            {
                UrlMain = new Uri(Configuration.Get<string>("UrlCoinprismApi")),
                UrlTest = new Uri(Configuration.Get<string>("UrlCoinprismApiTestnet")),
                Network = (Network)Enum.Parse(typeof(Network), Configuration.Get<string>("Network"))
            };

            services.AddInstance<ICoinprismClient>(new CoinprismClient(coniprismApiSettings));

            var log = new LogToConsole();
            services.AddInstance<ILog>(log);

            AppInfo.Version = Configuration.Get<string>("version");

            services.BindAzureRepositories(storageConnectionString, log);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
             
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Block_details",
                    template: "block/{id}",
                    defaults: new { controller = "Block", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}