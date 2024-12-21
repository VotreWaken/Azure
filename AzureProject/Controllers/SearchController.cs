using AzureProject.Models.ImagesSearch;
using AzureProject.Models.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Reflection;
using System.Text.Json;

namespace AzureProject.Controllers
{
	public class SearchController(IConfiguration configuration) : Controller
	{
		private readonly IConfiguration _configuration = configuration;
		public async Task<IActionResult> IndexAsync([FromQuery] int? page, [FromQuery] int? perpage, [FromQuery(Name = "search-phrase")] String? searchPhrase)
		{
			int perPage = perpage ?? 10;

			if (perpage < 2)
			{
				perpage = 2;
			}

			if (perPage < 50)
			{
				perpage = 50;
			}

			WebSearchPageModel model = new()
			{
				PageNumber = page ?? 1,
				PerPage = perPage,
			};

			if (model.PageNumber <= 0)
			{
				model.PageNumber = 1;
			}

			int offset = (model.PageNumber - 1) * perPage;

			if (!String.IsNullOrEmpty(searchPhrase))
			{
				var azureSection = _configuration.GetSection("Azure");
				var searchSection = azureSection?.GetSection("Search");

				var key = searchSection?.GetSection("Key").Value;
				var endpoint = searchSection?.GetSection("Endpoint").Value;
				var region = searchSection?.GetSection("Region").Value;
				var webPath = searchSection?.GetSection("WebPath").Value;

				if (key == null || endpoint == null || region == null || webPath == null)
				{
					ViewData["config"] = "Error";
				}
				else
				{
					// String searched = "IT STEP";
					using HttpClient client = new();
					using HttpRequestMessage request = new();

					request.RequestUri = new Uri(endpoint + webPath +
						"?q=" + Uri.EscapeDataString(searchPhrase) + $"&count={perPage}&offset={offset}");

					request.Headers.Add("Ocp-Apim-Subscription-Key", key);
					request.Headers.Add("Ocp-Apim-Subscription-Region", region);

					// Send the request and get response.
					HttpResponseMessage response =
						await client.SendAsync(request).ConfigureAwait(false);

					// Read response as a string.
					string result = await response.Content.ReadAsStringAsync();

					model.WebSearchResponse = JsonSerializer.Deserialize<WebSearchResponse>(result);

					ViewData["config"] = result;
				}
			}
			return View(model);
		}

		public async Task<IActionResult> ImagesAsync([FromQuery] int? page, [FromQuery] int? perpage, [FromQuery(Name = "search-phrase")] String? searchPhrase)
		{
			int perPage = perpage ?? 10;

			if (perpage < 2)
			{
				perpage = 2;
			}

			if (perPage < 50)
			{
				perpage = 50;
			}

			ImagesSearchPageModel model = new()
			{
				PageNumber = page ?? 1,
				PerPage = perPage,
			};

			if (model.PageNumber <= 0)
			{
				model.PageNumber = 1;
			}

			int offset = (model.PageNumber - 1) * perPage;

			if (!String.IsNullOrEmpty(searchPhrase))
			{
				var azureSection = _configuration.GetSection("Azure");
				var searchSection = azureSection?.GetSection("Search");

				var key = searchSection?.GetSection("Key").Value;
				var endpoint = searchSection?.GetSection("Endpoint").Value;
				var region = searchSection?.GetSection("Region").Value;
				var imagePath = searchSection?.GetSection("ImagePath").Value;

				if (key == null || endpoint == null || region == null || imagePath == null)
				{
					ViewData["config"] = "Error";
				}
				else
				{
					// String searched = "IT STEP";
					using HttpClient client = new();
					using HttpRequestMessage request = new();

					request.RequestUri = new Uri(endpoint + imagePath +
						"?q=" + Uri.EscapeDataString(searchPhrase));

					request.Headers.Add("Ocp-Apim-Subscription-Key", key);
					request.Headers.Add("Ocp-Apim-Subscription-Region", region);

					// Send the request and get response.
					HttpResponseMessage response =
						await client.SendAsync(request).ConfigureAwait(false);

					// Read response as a string.
					string result = await response.Content.ReadAsStringAsync();

					model.SearchResponse = JsonSerializer.Deserialize<ImagesSearchResponse>(result);

					ViewData["config"] = result;
				}
			}

			return View(model);
		}

    }
}
