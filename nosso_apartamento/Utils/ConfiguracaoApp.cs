using nosso_apartamento.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace nosso_apartamento.Utils
{
    public class ConfiguracaoApp
    {
        public ConfiguracaoApp()
        {
            CarregarConfiguracaoAsync();
        }

        public static string SenhaAdmin { get; private set; } = string.Empty;

        public static async Task CarregarConfiguracaoAsync()
        {
            var config = await LerConfigAsync();
            SenhaAdmin = config.SenhaAdmin;
        }

        private static async Task<ConfigModel> LerConfigAsync()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("assets/config.json");
            using var reader = new StreamReader(stream);

            var json = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<ConfigModel>(json);
        }
    }
}
