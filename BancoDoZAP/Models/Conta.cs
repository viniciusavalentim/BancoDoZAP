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

        public virtual void Depositar(double valor)
        {
            Saldo += valor;
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

        public bool Transferir(double valor, Conta destino)
        {
            if (Sacar(valor))
            {
                destino.Depositar(valor);
                return true;
            }
            return false;
        }
    }
}
