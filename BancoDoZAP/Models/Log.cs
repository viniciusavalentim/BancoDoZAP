using BancoDoZAP.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataHora { get; set; } = DateTime.Now;
        public TypeLog Tipo { get; set; }
        public Usuario Usuario { get; set; }
        public double Value { get; set; }
        public string TypeLogAccount { get; set; }

        public Log()
        {
            
        }

        public Log(string descricao, TypeLog tipo, Usuario usuario)
        {
            Descricao = descricao;
            Tipo = tipo;
            Usuario = usuario;
        }

        public void CriarLog(string descricao, TypeLog tipo, Usuario Usuario, double value = 0, string TypeLogAccount = "", Usuario UsuarioRecebido = null)
        {

        }
    }
}
