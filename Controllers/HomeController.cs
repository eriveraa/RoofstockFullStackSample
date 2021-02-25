using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoofstockFullStackSample.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RoofstockFullStackSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IMapper mapper, ApplicationDbContext context)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ResponseRawModel rawData;
            IEnumerable<Property> dataForUI;

            try
            {
                // Get the data from the API endpoint
                var request = new HttpRequestMessage(HttpMethod.Get, "https://samplerspubcontent.blob.core.windows.net/public/properties.json");
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        // Serialize the fetched data
                        rawData = await JsonSerializer.DeserializeAsync<ResponseRawModel>(responseStream);

                        // Map the data to the User interface specific Dto (flattening)
                        dataForUI = _mapper.Map<IEnumerable<Property>>(rawData.properties);
                    };
                }
                else
                {
                    dataForUI = Array.Empty<Property>();
                    TempData["Message"] = "There was an error in the response!";
                    TempData["Error"] = true;
                }
            }
            catch (Exception)
            {
                dataForUI = Array.Empty<Property>();
                TempData["Message"] = "There was an exception when trying to fetch data!";
                TempData["Error"] = true;
            }

            return View(dataForUI);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Property itemToSave)
        {
            // Store the item in the database
            try
            {
                // Check if the item is already saved
                Property record = await _context.Properties.FindAsync(itemToSave.Id);

                // If the item does not exists, store in Database
                if (record == null)
                {
                    await _context.Properties.AddAsync(itemToSave);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "The item was saved successfully!";
                    TempData["Error"] = false;
                }
                else
                {
                    // The item is already in the database. Send a message.
                    TempData["Message"] = "The item is already in the database.";
                    TempData["Error"] = true;
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "There was an error when saving.";
                TempData["Error"] = true;
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
