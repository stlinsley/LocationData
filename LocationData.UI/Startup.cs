using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LocationData.UI
{
    using ApiRepository;
    using ApiRepository.Facades;
    using AutoMapper;
    using Core.Mappings;
    using Core.Models.City;
    using Core.Models.Weather;
    using Data.Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.IO;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLogging()
                .AddTransient<IWeatherData, WeatherData>()
                .AddTransient<IClient, Client>()
                .AddTransient<ICity, City>()
                .AddTransient<IWeatherDataFacade, WeatherApiFacade>()
                .AddTransient<ICityDataFacade, CityApiFacade>()
                .AddAutoMapper(Assembly.GetAssembly(typeof(LocationProfile)))
                .AddTransient<ISerialization, Serialization>()
                .AddSingleton<JsonSerializer>()
                .AddControllersWithViews();

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            services.AddHttpClient<WeatherApiFacade>();
            services.AddHttpClient<CityApiFacade>();

            var config = configBuilder.Build();

            services.Configure<WeatherDataFacadeOptions>(myOptions =>
            {
                myOptions.ApiKey = config.GetSection("WeatherApiFacade")["apiKey"];
                myOptions.BaseUri = config.GetSection("WeatherApiFacade")["baseUri"];
            });

            services.Configure<CityDataFacadeOptions>(myOptions =>
            {
                myOptions.BaseUri = config.GetSection("CityApiFacade")["baseUri"];
            });

            services.AddDbContext<LocationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LocationContext"), x => x.MigrationsAssembly("LocationData.Data")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
