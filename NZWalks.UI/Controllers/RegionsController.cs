using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Net.Http.Headers;
using System.Text;
using System.Reflection;
using System.Text.Json;
using System.Net.Http.Json;
namespace NZWalks.UI.Controllers
{


    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            try
            {
                //Get All Regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7158/api/Regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<List<RegionDto>>());

            }
            catch (Exception ex)
            {
                //Log the exception
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7158/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")

            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            //var response = await httpResponseMessage.Content.ReadFromJsonAsync<List<RegionDto>>();


            //if (response is not null) {
            //    return RedirectToAction("Index", "Regions");
            //}

            //return View();
            return RedirectToAction("Index", "Regions");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>(
                $"https://localhost:7158/api/Regions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }

            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7158/api/Regions/{request.Id}"),

                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")

            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Regions");
            }

            var errorBody =
                await httpResponseMessage.Content.ReadAsStringAsync();

            ViewBag.Error =
                $"Status: {(int)httpResponseMessage.StatusCode} " +
                $"{httpResponseMessage.ReasonPhrase}. {errorBody}";

            return View(request);
        }



        //[HttpPost]
        //public async Task<IActionResult> Edit(RegionDto request)
        //{
        //    var client = httpClientFactory.CreateClient();

        //    var httpRequestMessage = new HttpRequestMessage()
        //    {
        //        Method = HttpMethod.Put,
        //        RequestUri = new Uri(
        //            $"https://localhost:7158/api/Regions/{request.Id}"),
        //        Content = new StringContent(
        //            JsonSerializer.Serialize(request),
        //            Encoding.UTF8,
        //            "application/json")
        //    };

        //    var httpResponseMessage =
        //        await client.SendAsync(httpRequestMessage);
 
        //    var respose = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

        //    if (respose is not null)
        //    {
        //        return RedirectToAction("Edit", "Regions");
        //    }

        //    return View();
        //}

        [HttpPost]

        public async Task<IActionResult> Delete(RegionDto request)
        {

            try
            {
                var client = httpClientFactory.CreateClient();

                //Delete no need of body just need a resource url along with the Id in it

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7158/api/Regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {

            }
            return View("Edit");
        }
    }
}
