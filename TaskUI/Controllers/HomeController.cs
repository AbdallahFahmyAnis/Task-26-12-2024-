using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using TaskUI.Models;
using TaskUI.Services;

namespace TaskUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettingsService _apiSettingsService;

        public HomeController(HttpClient httpClient, ApiSettingsService apiSettingsService = null)
        {
            _httpClient = httpClient;
            _apiSettingsService = apiSettingsService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    
        [HttpPost]
        public async Task<IActionResult> UploadJsonFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");
           
            var formData = new MultipartFormDataContent();
            using (var stream = file.OpenReadStream())
            {
                formData.Add(new StreamContent(stream), "file", file.FileName);
                string apiBaseUrl = _apiSettingsService.GetApiBaseUrl();
                string apiUrl = $"{apiBaseUrl}words";
                var response = await _httpClient.PostAsync(apiUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();

                    // Using ViewData to pass the result
                    ViewData["Result"] = result;

                    return View("index");
                }

                return BadRequest("Failed to process the file");
            }
        }

    }
}
