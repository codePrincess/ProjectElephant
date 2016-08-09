using Newtonsoft.Json;
using ProjectElephant.Bot.Models;
using ProjectElephant.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ProjectElephant.Bot.Services
{
    public class ElephantBrainService
    {
        private HttpClient httpClient = new HttpClient();

        public async Task<KeywordAnswer[]> GetAnswersAsync(string keyword)
        {
            var url = $"http://[ENTER YOUR BOT ADDRESS HERE]:9044/api/answers?keyword={keyword}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var answers = JsonConvert.DeserializeObject<KeywordAnswer[]>(content);
            return answers;
        }
    }
}