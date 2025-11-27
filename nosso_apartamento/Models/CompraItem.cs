using System;
using System.Collections.Generic;
using System.Text;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace nosso_apartamento.Models
{
    [Table("compra_itens")]
    public class CompraItem : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("compra_id")]
        public Guid CompraId { get; set; }

        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Column("quantidade")]
        public int Quantidade { get; set; } = 1;

        [Column("comprado")]
        public bool Comprado { get; set; } = false;

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
