using System;
using BancoDoZAP.Models;
using BancoDoZAP.Services.AccountService;
using BancoDoZAP.Services.UserService;

namespace BancoDoZap
{
    class Program
    {
        static void Main(string[] args)
        {
            UserService usuarioService = new UserService();
            Usuario usuarioLogado = null;

            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"
                      _____       _      _       
                     |_   _|     (_)    (_)      
                       | |  _ __  _  ___ _  ___  
                       | | | '_ \| |/ __| |/ _ \ 
                      _| |_| | | | | (__| | (_) |
                     |_____|_| |_|_|\___|_|\___/ 
                            
                ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("==== Bem vindo ao Banco do ZAP ====");
                Console.ResetColor();
                Console.WriteLine("1 - Registrar");
                Console.WriteLine("2 - Logar");
                Console.WriteLine("3 - Visualizar Usuários");
                Console.WriteLine("0 - Sair");
                Console.Write("\nEscolha uma opção: ");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        usuarioService.Registrar();
                        break;
                    case "2":
                        usuarioLogado = usuarioService.Logar();
                        if (usuarioLogado != null)
                        {
                            MenuConta(usuarioLogado);
                        }
                        break;
                    case "3":
                        usuarioService.ListarUsuarios();
                        break;
                    case "0":
                        Console.WriteLine("Encerrando o programa...");
                        return;
                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MenuConta(Usuario usuario)
        {
            AccountService accountService = new AccountService();   

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"
                  _____      _            _    ____                              _         ______         _____  
                 |  __ \    (_)          | |  |  _ \                            | |       |___  /   /\   |  __ \ 
                 | |__) |_ _ _ _ __   ___| |  | |_) | __ _ _ __   ___ ___     __| | ___      / /   /  \  | |__) |
                 |  ___/ _` | | '_ \ / _ \ |  |  _ < / _` | '_ \ / __/ _ \   / _` |/ _ \    / /   / /\ \ |  ___/ 
                 | |  | (_| | | | | |  __/ |  | |_) | (_| | | | | (_| (_) | | (_| | (_) |  / /__ / ____ \| |     
                 |_|   \__,_|_|_| |_|\___|_|  |____/ \__,_|_| |_|\___\___/   \__,_|\___/  /_____/_/    \_\_|     
                                                                                                 
                            
                ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"=== Olá, {usuario.Nome}! Seu saldo é de R$ {usuario.Conta.Saldo} ===\n");
                Console.ResetColor();
                Console.ResetColor();
                Console.WriteLine("1 - Sacar");
                Console.WriteLine("2 - Depositar");
                Console.WriteLine("3 - Transferir");
                Console.WriteLine("0 - Sair da Conta");
                Console.Write("\nEscolha uma opção: ");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        accountService.Sacar(usuario);
                        Console.ReadKey();
                        break;
                    case "2":
                        accountService.Depositar(usuario);
                        Console.ReadKey();
                        break;
                    case "3":
                        accountService.Transferir(usuario);
                        Console.ReadKey();
                        break;
                    case "0":
                        return; // Sai para o menu principal
                    default:
                        Console.WriteLine("Opção inválida!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
