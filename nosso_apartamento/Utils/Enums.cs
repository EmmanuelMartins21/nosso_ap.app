using System;
using System.Collections.Generic;
using System.Text;

namespace nosso_apartamento.Utils
{
    public class Enums
    {
        public enum TipoMovimentoFinanceiro
        {
            Receita,
            Despesa
        }

        public enum StatusCompraItem
        {
            Pendente,
            Comprado,
            Cancelado
        }
        public enum StatusCompra
        {
            Pendente,
            Realizada
        }
        public enum PerfilUsuario
        {
            Administrador,
            Morador
        }
    }
}
