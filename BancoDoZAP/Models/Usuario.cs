using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public Conta Conta { get; set; }
        private string Senha;

        public Usuario(string nome, string cpf, string telefone, string senha, Conta conta)
        {
            Nome = nome;
            CPF = cpf;
            Telefone = telefone;
            this.Senha = senha;
            Conta = conta;
        }

        public bool ValidarSenha(string senhaInformada)
        {
            return Senha == senhaInformada;
        }
    }
}
