using BancoDoZAP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Services.AccountService
{
    class AccountService
    {
        public void Sacar(Usuario usuario)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"
               _____                      
              / ____|                     
             | (___   __ _  ___ __ _ _ __ 
              \___ \ / _` |/ __/ _` | '__|
              ____) | (_| | (_| (_| | |   
             |_____/ \__,_|\___\__,_|_|   
                              
            ");
            Console.ResetColor();
            Console.WriteLine($"Saldo atual: R$ {usuario.Conta.Saldo}");
            Console.Write("Valor: R$ ");
            double valor = double.Parse(Console.ReadLine());
            if (usuario.Conta.Sacar(valor))
            {
                Console.WriteLine($"Saque de R$ {valor} realizado com sucesso!");
            }
            else
            {
                Console.WriteLine("Saldo insuficiente ou valor inválido.");
            }
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
        }


        public void Depositar(Usuario usuario)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"
              _____                       _ _             
             |  __ \                     (_) |            
             | |  | | ___ _ __   ___  ___ _| |_ __ _ _ __ 
             | |  | |/ _ \ '_ \ / _ \/ __| | __/ _` | '__|
             | |__| |  __/ |_) | (_) \__ \ | || (_| | |   
             |_____/ \___| .__/ \___/|___/_|\__\__,_|_|   
                         | |                              
                         |_|                              
            ");
            Console.ResetColor();

            Console.WriteLine($"Saldo atual: R$ {usuario.Conta.Saldo}");
            Console.Write("Valor: R$ ");
            double valor = double.Parse(Console.ReadLine());
            if (valor > 0)
            {
                usuario.Conta.Depositar(valor);
                Console.WriteLine($"Depósito de R$ {valor} realizado com sucesso!");
            }
            else
            {
                Console.WriteLine("Valor inválido.");
            }
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
        }


        public void Transferir(Usuario usuario)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"
          _______                   __          _      
         |__   __|                 / _|        (_)     
            | |_ __ __ _ _ __  ___| |_ ___ _ __ _ _ __ 
            | | '__/ _` | '_ \/ __|  _/ _ \ '__| | '__|
            | | | | (_| | | | \__ \ ||  __/ |  | | |   
            |_|_|  \__,_|_| |_|___/_| \___|_|  |_|_|   
                                               
            ");
            Console.ResetColor();
            Console.WriteLine($"Saldo atual: R$ {usuario.Conta.Saldo}");
            Console.Write("Valor: R$ ");
            double valor = double.Parse(Console.ReadLine());


            Console.WriteLine();
            Console.WriteLine("==== Lista de usuários para transferencia =====");
            foreach (var usuarioData in Database.Database.Usuarios)
            {
                Console.WriteLine($"Nome: {usuarioData.Nome} | CPF: {usuarioData.CPF} | Numero da Conta: {usuarioData.Conta.NumeroConta}");
            }
            Console.WriteLine();

            Console.Write("Número da conta de destino: ");
            int numeroContaDestino = int.Parse(Console.ReadLine());

            if (valor <= 0)
            {
                Console.WriteLine("Valor inválido.");
                return;
            }

            Usuario contaDestino = Database.Database.Usuarios.FirstOrDefault(c => c.Conta.NumeroConta == numeroContaDestino);
            if (contaDestino != null)
            {
                Console.WriteLine($"{contaDestino.Conta.Saldo} + {contaDestino.Conta.NumeroConta}");
                if (usuario.Conta.Transferir(valor, contaDestino.Conta))
                {
                    Console.WriteLine($"Transferência de R$ {valor} realizada com sucesso!");
                }
                else
                {
                    Console.WriteLine("Saldo insuficiente ou valor inválido.");
                }
            }
            else
            {
                Console.WriteLine("Conta de destino não encontrada.");
            }
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
        }
    }
}