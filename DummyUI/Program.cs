﻿using System;

namespace DummyUI
{
    using AutoMapper;
    using LocationData.ApiRepository;
    using LocationData.ApiRepository.Facades;
    using LocationData.Core.Extensions;
    using LocationData.Core.Mappings;
    using LocationData.Core.Models;
    using LocationData.Core.Models.City;
    using LocationData.Core.Models.Weather;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddLogging()
                .AddTransient<IWeatherData, WeatherData>()
                .AddTransient<ICity, City>()
                .AddTransient<IClient, Client>()
                .AddTransient<IWeatherDataFacade, WeatherApiFacade>()
                .AddTransient<ICityDataFacade, CityApiFacade>()
                .AddAutoMapper(Assembly.GetAssembly(typeof(LocationProfile)))
                .AddSingleton<JsonSerializer>();

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

            serviceCollection.AddHttpClient<WeatherApiFacade>();
            serviceCollection.AddHttpClient<CityApiFacade>();

            var config = configBuilder.Build();

            serviceCollection.Configure<WeatherDataFacadeOptions>(myOptions =>
                {
                    myOptions.ApiKey = config.GetSection("WeatherApiFacade")["apiKey"];
                    myOptions.BaseUri = config.GetSection("WeatherApiFacade")["baseUri"];
                });

            serviceCollection.Configure<CityDataFacadeOptions>(myOptions =>
            {
                myOptions.BaseUri = config.GetSection("CityApiFacade")["baseUri"];
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mapper = serviceProvider.GetService<IMapper>();

            var cityData = serviceProvider.GetService<ICityDataFacade>();

            var weatherData = serviceProvider.GetService<IWeatherDataFacade>();

            Console.WriteLine("Please enter a city");
            var cityInput = Console.ReadLine();

            var city = cityData.GetCityData(cityInput).Result;
            var weather = weatherData.GetWeatherDataForLngLat(25, 54).Result;

            var result = mapper.Map<CombinedLocationData>(city).Map(mapper, weather);

            var cityList = new List<CombinedLocationData>();

            if (File.Exists("citydata.json"))
            {
                using StreamReader file = File.OpenText("citydata.json");
                JsonSerializer serializer = new JsonSerializer();
                cityList = (List<CombinedLocationData>) serializer.Deserialize(file, typeof(List<CombinedLocationData>));
            }

            cityList.Add(result);

            using (StreamWriter file = File.CreateText("citydata.json"))
            {
                using JsonTextWriter writer = new JsonTextWriter(file);
                var jsonSerializer = new JsonSerializer();
                jsonSerializer.Serialize(writer, cityList);
            }

            foreach (var property in result.GetType().GetProperties())
            {
                Console.WriteLine($"{property.Name} : {JsonConvert.SerializeObject(property.GetValue(result))}");
            }

            Console.ReadLine();
        }
    }
}
