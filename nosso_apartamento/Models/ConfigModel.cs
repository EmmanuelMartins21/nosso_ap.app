using System;
using System.Collections.Generic;
using System.Text;

namespace nosso_apartamento.Models
{
    public class ConfigModel
    {
        public string SenhaAdmin { get; set; } = string.Empty;
        public string SupabaseUrl { get; set; } = string.Empty;
        public string SupabaseAnonKey { get; set; } = string.Empty;
    }
}
