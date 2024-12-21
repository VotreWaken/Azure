using AzureProject.Models;
using AzureProject.Models.Translator;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace AzureProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _configuration;
		public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult AppService()
		{
			return View();
		}

		public async Task<IActionResult> TranslatorAsync(TranslatorFormModel? formModel)
		{

			TranslatorPageModel model = new()
			{
				FormModel = formModel,
			};

			{
				using HttpClient client = new();
				String response = await client.GetStringAsync("https://api.cognitive.microsofttranslator.com/languages?api-version=3.0");
				model.LanguageResponse = 
					JsonSerializer.Deserialize<LanguageResponse>(response)!;
			}

			if (formModel?.Text != null)
			{
				var azureSection = _configuration.GetSection("Azure");
				var translatorSection = azureSection.GetSection("Translator");
				String? key = translatorSection.GetSection("Key").Value;
				String? endpoint = translatorSection.GetSection("Endpoint").Value;
				String? region = translatorSection.GetSection("Region").Value;

				string route = $"/translate?api-version=3.0&from={formModel!.LangFrom}&to={formModel!.LangTo}";
				object[] body = [new { Text = formModel!.Text }];
				String requestBody = JsonSerializer.Serialize(body);

				using HttpClient client = new();
				using HttpRequestMessage request = new();

				// Build the request.
				request.Method = HttpMethod.Post;
				request.RequestUri = new Uri(endpoint + route);
				request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

				// Azure Settings
				request.Headers.Add("Ocp-Apim-Subscription-Key", key);
				request.Headers.Add("Ocp-Apim-Subscription-Region", region);

				// Send the request and get response.
				HttpResponseMessage response =
					await client.SendAsync(request).ConfigureAwait(false);

				// Read response as a string.
				string result = await response.Content.ReadAsStringAsync();


				model.translatorResponse =
					JsonSerializer.Deserialize<List<TranslatorResponse>>(result);



				ViewData["result"] = result;
			}


			return View(model);
		}

		public async Task<JsonResult> Translate(TranslatorFormModel formModel)
		{
			var azureSection = _configuration.GetSection("Azure");
			var translatorSection = azureSection.GetSection("Translator");
			String? key = translatorSection.GetSection("Key").Value;
			String? endpoint = translatorSection.GetSection("Endpoint").Value;
			String? region = translatorSection.GetSection("Region").Value;

			string route = $"/translate?api-version=3.0&from={formModel!.LangFrom}&to={formModel!.LangTo}";
			object[] body = [new { Text = formModel!.Text }];
			String requestBody = JsonSerializer.Serialize(body);

			using HttpClient client = new();
			using HttpRequestMessage request = new();

			// Build the request.
			request.Method = HttpMethod.Post;
			request.RequestUri = new Uri(endpoint + route);
			request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

			// Azure Settings
			request.Headers.Add("Ocp-Apim-Subscription-Key", key);
			request.Headers.Add("Ocp-Apim-Subscription-Region", region);

			// Send the request and get response.
			HttpResponseMessage response =
				await client.SendAsync(request).ConfigureAwait(false);

			// Read response as a string.
			string result = await response.Content.ReadAsStringAsync();


			var r = JsonSerializer.Deserialize<List<TranslatorResponse>>(result);

			return Json(r[0].translations[0].text);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
