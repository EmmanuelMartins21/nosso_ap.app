using System;
using System.Collections.Generic;
using System.Text;

namespace nosso_apartamento.Models
{
    public class Compra
    {
        public string Titulo { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public List<CompraItem> Itens { get; set; } = new List<CompraItem>();
        public bool Concluido { get; set; } = false;
        
    }
}
