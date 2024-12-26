// Controllers/WordsController.cs
using Microsoft.AspNetCore.Mvc;
using WordBoundingBoxAPI.Services;
using System.Collections.Generic;
using Task.Models;
using Newtonsoft.Json;

namespace WordBoundingBoxAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly WordService _wordService;

        public WordsController(WordService wordService)
        {
            _wordService = wordService;
        }
        [HttpPost]
        public async Task<IActionResult> ProcessWords([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            // Read the uploaded file
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = await reader.ReadToEndAsync();

                // Deserialize the JSON content to List<Word>
                List<Word> words;
                try
                {
                    words = JsonConvert.DeserializeObject<List<Word>>(fileContent);
                }
                catch (JsonException)
                {
                    return BadRequest("Invalid JSON format");
                }

                if (words == null || words.Count == 0)
                    return BadRequest("No valid words found in the file");

                // Process the words
                var result = _wordService.ProcessWords(words);

                // Return the result
                return Ok(result);
            }
        }
    }
}
