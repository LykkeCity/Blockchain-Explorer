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

            var connectString = Configuration.Get<string>("ConnectionString");
            if (string.IsNullOrEmpty(connectString))
            {
                connectString = "UseDevelopmentStorage=true";
            }
            var bitcoinRpcSettings = new SrvBitcoinRpcSettings
            {
                HostPort = Configuration.Get<string>("RpcHostPort"),
                Password = Configuration.Get<string>("RpcPassword"),
                User = Configuration.Get<string>("RpcUser")
            };
            services.AddInstance(new SrvBitcoinRpc(bitcoinRpcSettings));
            services.AddTransient<ISrvRpcReader, SrvRpcReader>();


            var bitcoinNinjaSettings = new SrvBitcoinNinjaSettings
            {
                UrlMainNinja = Configuration.Get<string>("UrlMainNinja"),
                UrlTestNetNinja = Configuration.Get<string>("UrlTestNetNinja"),
                Network = Configuration.Get<string>("Network")
            };

            services.AddInstance<IBitcoinNinjaReaderRepository>(new SrvBitcoinNinjaReader(bitcoinNinjaSettings));

            var coniprismApiSettings = new CoinprismApiSettings
            {
                Network = Configuration.Get<string>("Network"),
                UrlCoinprismApiTestnet = Configuration.Get<string>("UrlCoinprismApiTestnet"),
                UrlCoinprismApi = Configuration.Get<string>("UrlCoinprismApi")
            };

            services.AddInstance<ISrvCoinprismReader>(new SrvCoinprismReader(coniprismApiSettings));

            var log = new LogToConsole();
            services.AddInstance<ILog>(log);

            services.BindAzureRepositories(connectString, log);



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
