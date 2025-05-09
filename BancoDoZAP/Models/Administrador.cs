using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Models
{
    public class Administrador : Pessoa
    {
        public string Cargo { get; set; }
        public DateTime DataAdmissao { get; set; }

        public Administrador(string nome, string cpf, string telefone, string cargo, DateTime dataAdmissao)
            : base(nome, cpf, telefone)
        {
            Cargo = cargo;
            DataAdmissao = dataAdmissao;
        }

        public override void ExibirInformacoes()
        {
            Console.WriteLine($"[ADMIN] Nome: {Nome}, Cargo: {Cargo}, Admitido em: {DataAdmissao.ToShortDateString()}");
        }
    }
}
