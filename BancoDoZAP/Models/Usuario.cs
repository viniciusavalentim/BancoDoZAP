using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Models
{
    public class Usuario : Pessoa
    {
        public int Id { get; set; }
        public Conta Conta { get; set; }
        public string TypeUser { get; set; } = string.Empty;
        private string Senha;


        public Usuario(string nome, string cpf, string telefone, string senha, Conta conta)
            : base(nome, cpf, telefone)
        {
            this.Senha = senha;
            Conta = conta;
        }

        public Usuario(string nome, string cpf, string telefone, string senha, Conta conta, string typeUser)
            : base(nome, cpf, telefone)
        {
            this.Senha = senha;
            Conta = conta;
            TypeUser = typeUser;
        }

        public bool ValidarSenha(string senhaInformada)
        {
            return Senha == senhaInformada;
        }
    }
}
