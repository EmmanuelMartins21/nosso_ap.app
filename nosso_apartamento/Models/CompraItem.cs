using System;
using System.Collections.Generic;
using System.Text;

namespace nosso_apartamento.Models
{
    public class CompraItem
    {
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; } = 1;
        public bool Comprado { get; set; } = false;
    }
}
