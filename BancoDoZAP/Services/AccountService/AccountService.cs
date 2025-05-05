using BancoDoZAP.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    ███████╗ █████╗  ██████╗ █████╗ ██████╗ 
    ██╔════╝██╔══██╗██╔════╝██╔══██╗██╔══██╗
    ███████╗███████║██║     ███████║██████╔╝
    ╚════██║██╔══██║██║     ██╔══██║██╔══██╗
    ███████║██║  ██║╚██████╗██║  ██║██║  ██║
    ╚══════╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝
    ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Saldo atual: ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"R$ {usuario.Conta.Saldo}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Digite 0 para cancelar a operação");
            Console.ResetColor();

            double valor;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Valor do saque: R$ ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nOperação de saque cancelada pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                if (!double.TryParse(input, out valor))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nValor inválido! Digite apenas números.");
                    Console.ResetColor();
                    continue;
                }

                if (valor <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nO valor do saque deve ser maior que zero.");
                    Console.ResetColor();
                    continue;
                }

                if (valor > usuario.Conta.Saldo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nSaldo insuficiente. Seu saldo atual é R$ {usuario.Conta.Saldo}");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            if (usuario.Conta.Sacar(valor))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nSaque de R$ {valor} realizado com sucesso!");
                Console.WriteLine($"Novo saldo: R$ {usuario.Conta.Saldo}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOcorreu um erro ao processar o saque.");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ResetColor();
            Console.ReadKey();
        }


        public void Depositar(Usuario usuario)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"
    ██████╗ ███████╗██████╗  ██████╗ ███████╗██╗████████╗ █████╗ ██████╗ 
    ██╔══██╗██╔════╝██╔══██╗██╔═══██╗██╔════╝██║╚══██╔══╝██╔══██╗██╔══██╗
    ██║  ██║█████╗  ██████╔╝██║   ██║███████╗██║   ██║   ███████║██████╔╝
    ██║  ██║██╔══╝  ██╔═══╝ ██║   ██║╚════██║██║   ██║   ██╔══██║██╔══██╗
    ██████╔╝███████╗██║     ╚██████╔╝███████║██║   ██║   ██║  ██║██║  ██║
    ╚═════╝ ╚══════╝╚═╝      ╚═════╝ ╚══════╝╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝                                                                  
    ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"Saldo atual: R$ {usuario.Conta.Saldo}");
            Console.WriteLine("Digite 0 para cancelar a operação");
            Console.ResetColor();

            double valor;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Valor do depósito: R$ ");
                Console.ForegroundColor = ConsoleColor.White;
                string input = Console.ReadLine();

                if (input == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nOperação de depósito cancelada pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                if (!double.TryParse(input, out valor))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nValor inválido! Digite apenas números.");
                    Console.ResetColor();
                    continue;
                }

                if (valor <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nO valor do depósito deve ser maior que zero.");
                    Console.ResetColor();
                    continue;
                }

                if (valor > 100000)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nValor máximo por depósito é de R$ 100.000,00");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            if (usuario.Conta.Depositar(valor))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nDepósito de {valor:C} realizado com sucesso!");
                Console.WriteLine($"Novo saldo: {usuario.Conta.Saldo:C}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nOcorreu um erro ao processar o depósito.");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ResetColor();
            Console.ReadKey();
        }

        public void Transferir(Usuario usuario)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
    ████████╗██████╗  █████╗ ███╗   ██╗███████╗███████╗███████╗██████╗ ██╗██████╗ 
    ╚══██╔══╝██╔══██╗██╔══██╗████╗  ██║██╔════╝██╔════╝██╔════╝██╔══██╗██║██╔══██╗
       ██║   ██████╔╝███████║██╔██╗ ██║███████╗█████╗  █████╗  ██████╔╝██║██████╔╝
       ██║   ██╔══██╗██╔══██║██║╚██╗██║╚════██║██╔══╝  ██╔══╝  ██╔══██╗██║██╔══██╗
       ██║   ██║  ██║██║  ██║██║ ╚████║███████║██║     ███████╗██║  ██║██║██║  ██║
       ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚══════╝╚═╝     ╚══════╝╚═╝  ╚═╝╚═╝╚═╝  ╚═╝
    ");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\nSaldo atual: R$ {usuario.Conta.Saldo.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
            Console.WriteLine("Digite 0 em qualquer campo para cancelar a operação\n");
            Console.ResetColor();

            double valor;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Valor da transferência: R$ ");
                Console.ForegroundColor = ConsoleColor.White;
                string valorInput = Console.ReadLine();

                if (valorInput == "0")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nOperação de transferência cancelada pelo usuário.");
                    Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                if (!double.TryParse(valorInput, NumberStyles.Any, CultureInfo.CreateSpecificCulture("pt-BR"), out valor))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nValor inválido! Use números no formato brasileiro (ex: 1500,50).");
                    continue;
                }

                if (valor <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nO valor da transferência deve ser maior que zero.");
                    continue;
                }

                if (valor > usuario.Conta.Saldo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nSaldo insuficiente. Seu saldo atual é R$ {usuario.Conta.Saldo.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
                    continue;
                }

                break;
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n══════════════════════════════════════════════════");
            Console.WriteLine("       LISTA DE USUÁRIOS PARA TRANSFERÊNCIA");
            Console.WriteLine("══════════════════════════════════════════════════");
            Console.ResetColor();

            foreach (var usuarioData in Database.Database.Usuarios.Where(u => u.CPF != usuario.CPF))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"Nome: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{usuarioData.Nome} ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("| Conta: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{usuarioData.Conta.NumeroConta}");
            }

            int numeroContaDestino;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("\nNúmero da conta de destino: ");
                Console.ForegroundColor = ConsoleColor.White;
                string contaInput = Console.ReadLine();

                if (contaInput == "0")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nOperação de transferência cancelada pelo usuário.");
                    Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                if (!int.TryParse(contaInput, out numeroContaDestino))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNúmero da conta inválido! Digite apenas números.");
                    continue;
                }

                if (numeroContaDestino == usuario.Conta.NumeroConta)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nVocê não pode transferir para sua própria conta.");
                    continue;
                }

                break;
            }

            Usuario contaDestino = Database.Database.Usuarios.FirstOrDefault(c => c.Conta.NumeroConta == numeroContaDestino);

            if (contaDestino != null)
            {
                if (usuario.Conta.Transferir(valor, contaDestino.Conta))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nTransferência de R$ {valor.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
                    Console.WriteLine($"para {contaDestino.Nome} realizada com sucesso!");
                    Console.WriteLine($"\nNovo saldo: R$ {usuario.Conta.Saldo.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("\nOcorreu um erro ao processar a transferência.");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nConta de destino não encontrada.");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ResetColor();
        }
    }
}