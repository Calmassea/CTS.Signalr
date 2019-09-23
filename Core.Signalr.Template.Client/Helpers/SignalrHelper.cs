﻿using Core.Signalr.Template.Client.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.Signalr.Template.Client.Helpers
{
    public class SignalrHelper
    {
        private readonly IConfiguration _configuration;

        public SignalrHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PushNotifyAsync(Send send)
        {
            var signalrAddress = _configuration.GetSection("SignalrAddress").Value;
            var sendUrl =$"{signalrAddress}api/Notify";
            var stringContent = new StringContent(JsonConvert.SerializeObject(send), Encoding.UTF8, "application/json");
            await new HttpClient().PostAsync(sendUrl, stringContent);
        }
    }
}
