using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.ApplicationServices.Services
{
    public class OMDbServices : IOMDbServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = Filminurk.Data.Environment.omdbApiKey;

        public OMDbServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OMDbDTO> GetMovieByTitle(string title)
        {
            var url = $"https://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&apikey={_apiKey}";
            var response = await _httpClient.GetFromJsonAsync<OMDbDTO>(url);
            return response;
        }
    }
}
