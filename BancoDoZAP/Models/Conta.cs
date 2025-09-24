using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public int NumeroConta { get; protected set; }
        public string Agencia { get; set; }
        public double Saldo { get; protected set; }
        public List<PixChave> PixChaves { get; set; }

        public Conta(int id, string agencia, double saldo, int numeroConta = 0)
        {
            Id = id;
            Agencia = agencia;
            if(numeroConta != 0)
            {
                NumeroConta = numeroConta;
            }
            else
            {
                NumeroConta = new Random().Next(1000, 9999);
            }
            Saldo = saldo;
        }

        public virtual bool Depositar(double valor)
        {
            Saldo += valor;
            return true;
        }

        public virtual bool Sacar(double valor)
        {
            if (valor <= 0)
            {
                return false;
            }

            if (Saldo >= valor)
            {
                Saldo -= valor;
                return true;
            }
            return false;
        }

        public bool CadastrarChavePix(string tipo, string valor)
        {
            if (PixChaves == null)
            {
                PixChaves = new List<PixChave>();
            }
            if (PixChaves.Any(c => c.Tipo == tipo && c.Valor == valor))
            {
                return false;
            }
            var novaChave = new PixChave
            {
                Id = PixChaves.Count > 0 ? PixChaves.Max(c => c.Id) + 1 : 1,
                Tipo = tipo,
                Valor = valor,
                ContaId = this.Id
            };
            PixChaves.Add(novaChave);
            return true;
        }

        public bool EditarChavePix(int id, string novoTipo, string novoValor)
        {
            var chaveExistente = PixChaves.FirstOrDefault(c => c.Id == id);
            if (chaveExistente == null)
            {
                return false;
            }
            if (PixChaves.Any(c => c.Tipo == novoTipo && c.Valor == novoValor && c.Id != id))
            {
                return false;
            }
            chaveExistente.Tipo = novoTipo;
            chaveExistente.Valor = novoValor;
            return true;
        }

        public bool ExcluirChavePix(int id)
        {
            var chaveExistente = PixChaves.FirstOrDefault(c => c.Id == id);
            if (chaveExistente == null)
            {
                return false;
            }
            PixChaves.Remove(chaveExistente);
            return true;
        }



        public bool Transferir(double valor, Conta destino)
        {
            if (Sacar(valor))
            {
                Console.WriteLine(destino.NumeroConta);
                destino.Depositar(valor);
                return true;
            }
            return false;
        }
    }
}
