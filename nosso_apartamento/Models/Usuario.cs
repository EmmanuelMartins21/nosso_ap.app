using System;
using System.Collections.Generic;
using System.Text;
using static nosso_apartamento.Utils.Enums;

namespace nosso_apartamento.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public PerfilUsuario PerfilUsuario { get; set; } = PerfilUsuario.Administrador;
    }
}
