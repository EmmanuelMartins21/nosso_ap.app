using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace nosso_apartamento.Models
{
    [Table("compras")]
    public class Compra : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("concluido")]
        public bool Concluido { get; set; } = false;
        
        [JsonPropertyName("compra_itens")]
        public List<CompraItem> Itens { get; set; } = new List<CompraItem>();
    }
}
