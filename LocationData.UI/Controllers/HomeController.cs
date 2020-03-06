using LocationData.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace LocationData.UI.Controllers
{
    using ApiRepository.Facades;
    using AutoMapper;
    using Core.Extensions;
    using Core.Models;
    using Core.Models.City;
    using Core.Models.Weather;
    using Data.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICityDataFacade _cityApi;
        private readonly IWeatherDataFacade _weatherApi;
        private readonly IMapper _mapper;
        private readonly LocationContext _context;

        public HomeController(ILogger<HomeController> logger, ICityDataFacade cityApi, IWeatherDataFacade weatherApi,
            IMapper mapper, LocationContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cityApi = cityApi ?? throw new ArgumentNullException(nameof(cityApi));
            _weatherApi = weatherApi ?? throw new ArgumentNullException(nameof(weatherApi));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Home
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CityNameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["CountrySortParam"] = sortOrder == "Country" ? "country_desc" : "Country";
            ViewData["PopulationSortParam"] = sortOrder == "Population" ? "population_desc" : "Population";
            ViewData["CurrencySortParam"] = sortOrder == "Currency" ? "currency_desc" : "Currency";

            var cities = _context.CombinedLocationData.Select(c => c);

            switch (sortOrder)
            {
                case "name_desc":
                    cities = cities.OrderByDescending(c => c.CityName);
                    break;
                case "Country":
                    cities = cities.OrderBy(c => c.Country);
                    break;
                case "country_desc":
                    cities = cities.OrderByDescending(c => c.Country);
                    break;
                case "Population":
                    cities = cities.OrderBy(c => c.Population);
                    break;
                case "population_desc":
                    cities = cities.OrderByDescending(c => c.Population);
                    break;
                case "Currency":
                    cities = cities.OrderBy(c => c.Currency.Name);
                    break;
                case "currency_desc":
                    cities = cities.OrderByDescending(c => c.Currency.Name);
                    break;
                default:
                    cities = cities.OrderBy(c => c.CityName);
                    break;
            }

            cities = cities.Include(c => c.Currency);

            return View(await cities.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (await CityExists(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }

            var search = _cityApi.GetCityData<List<City>>(searchTerm).Result[0];
            var weather = _weatherApi.GetWeatherDataForLngLat<WeatherData>(search.Latlng[1],
                search.Latlng[0]).Result;

            var result = _mapper.Map<CombinedLocationData>(search).Map(_mapper, weather);

            if (await CurrencyExists(result))
            {
                result.Currency = await GetExistingCurrency(result);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(result);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Please try again.");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Home/Edit/f8b7a3c4-9df9-4fc2-88a9-973a4d6588c1
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.CombinedLocationData.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid? id, [Bind("Id,CityName,Country,Alpha2Code,Alpha3Code")] CombinedLocationData city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CityExists(city.Id))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Home/Delete/f8b7a3c4-9df9-4fc2-88a9-973a4d6588c1
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.CombinedLocationData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            city.Currency = await _context.Currency.FirstOrDefaultAsync(m => m.CurrencyId == city.CurrencyId);

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var city = await _context.CombinedLocationData.FindAsync(id);
            _context.CombinedLocationData.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateWeather()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Task<bool> CityExists(string searchTerm)
        {
            return _context.CombinedLocationData.AnyAsync(x => x.CityName == searchTerm);
        }

        private Task<bool> CityExists(Guid id)
        {
            return _context.CombinedLocationData.AnyAsync(x => x.Id == id);
        }

        private Task<Currency> GetExistingCurrency(CombinedLocationData result)
        {
            return _context.Currency.FirstOrDefaultAsync(c => c.Name == result.Currency.Name);
        }

        private async Task<bool> CurrencyExists(CombinedLocationData result)
        {
            return await _context.Currency.AnyAsync(c => c.Name == result.Currency.Name);
        }

    }
}
