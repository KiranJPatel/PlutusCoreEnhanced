﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Plutocrat.Core.Interfaces;

namespace Plutocrat.Core.Helpers
{
    public class SettingsLoader : ISettingsLoader
    {
        public int BuyInterval { get; }

        public int PlacedOrderManagementInterval { get; }

        public int DownTrendNotificationJobInterval { get; }

        public string TelegramBotApiKey { get; }

        public bool Test { get; }
        
        public string RedisUrl { get; }

        public List<ExhangeConfig> ExhangeConfigurations { get; }

        public SettingsLoader(string path)
        {
            var json = File.ReadAllText(path);
            var obj = JObject.Parse(json);

            var exchanges = obj["Exchanges"];

            ExhangeConfigurations = new List<ExhangeConfig>();

            foreach (var exchange in exchanges)
            {
                var name = exchange["Name"].Value<string>();
                var apiKey = exchange["ApiKey"].Value<string>();
                var secretKey = exchange["SecretKey"].Value<string>();

                ExhangeConfigurations.Add(new ExhangeConfig()
                {
                    ExhangeName = name,
                    ApiKey = apiKey,
                    SecretKey = secretKey
                });
            }

            BuyInterval = GetAppConfig<int>(obj, "BuyInterval");

            PlacedOrderManagementInterval = GetAppConfig<int>(obj, "PlacedOrderManagementInterval");

            DownTrendNotificationJobInterval = GetAppConfig<int>(obj, "DownTrendNotificationJobInterval");

            TelegramBotApiKey = GetAppConfig<string>(obj, "TelegramBotApiKey");

            Test = GetAppConfig<bool>(obj, "Test");
            
            RedisUrl = GetAppConfig<string>(obj, "RedisUrl");
        }

        private static T GetAppConfig<T>(JObject config, string key)
        {
            return config["AppConfig"][key].Value<T>();
        }
    }
}