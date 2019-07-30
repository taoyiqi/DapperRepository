using System.Collections.Generic;
using System.IO;
using CoreDapperRepository.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace CoreDapperRepository.Web
{
    public class DbConnConfig : IDbConnConfig
    {
        private readonly IHostingEnvironment _environment;

        public DbConnConfig(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public string GetConnectionString(string connKey)
        {
            if (string.IsNullOrEmpty(connKey))
                return string.Empty;

            string filePath = Path.Combine(_environment.ContentRootPath + "/App_Data/DbConnSettings.json");

            if (!File.Exists(filePath))
                return string.Empty;

            string text = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            Dictionary<string, string> connStrDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);

            return connStrDict[connKey];
        }
    }
}
