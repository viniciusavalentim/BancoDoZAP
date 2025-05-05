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
    ██╗███╗   ██╗██╗ ██████╗██╗ ██████╗ 
    ██║████╗  ██║██║██╔════╝██║██╔═══██╗
    ██║██╔██╗ ██║██║██║     ██║██║   ██║
    ██║██║╚██╗██║██║██║     ██║██║   ██║
    ██║██║ ╚████║██║╚██████╗██║╚██████╔╝
    ╚═╝╚═╝  ╚═══╝╚═╝ ╚═════╝╚═╝ ╚═════╝ 
                ");

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("════════════════════════════════════");
                Console.WriteLine("==== Bem vindo ao Banco do ZAP ====");
                Console.WriteLine("════════════════════════════════════");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[1] - Registrar");
                Console.WriteLine("[2] - Logar");
                Console.WriteLine("[3] - Visualizar Usuários");
                Console.WriteLine("[0] - Sair");

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("\nEscolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                string opcao = Console.ReadLine();
                Console.ResetColor();

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
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Encerrando o programa...");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Opção inválida!");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void MenuConta(Usuario usuario)
        {
            AccountService accountService = new AccountService();
            Console.Title = $"Banco ZAP - Conta de {usuario.Nome}";

            while (true)
            {
                Console.Clear();

                // 1ª COR PRINCIPAL (Verde escuro) - Banner e elementos estruturais
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(@"
 ██████╗  █████╗ ███╗   ██╗ ██████╗ ██████╗     ██████╗  ██████╗     ███████╗ █████╗ ██████╗ 
 ██╔══██╗██╔══██╗████╗  ██║██╔════╝██╔═══██╗    ██╔══██╗██╔═══██╗    ╚══███╔╝██╔══██╗██╔══██╗
 ██████╔╝███████║██╔██╗ ██║██║     ██║   ██║    ██║  ██║██║   ██║      ███╔╝ ███████║██████╔╝
 ██╔══██╗██╔══██║██║╚██╗██║██║     ██║   ██║    ██║  ██║██║   ██║     ███╔╝  ██╔══██║██╔═══╝ 
 ██████╔╝██║  ██║██║ ╚████║╚██████╗╚██████╔╝    ██████╔╝╚██████╔╝    ███████╗██║  ██║██║     
 ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚═════╝     ╚═════╝  ╚═════╝     ╚══════╝╚═╝  ╚═╝╚═╝   
                ");

                // 2ª COR (Verde claro) - Títulos e divisórias
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n┌══════════════════════════════════════════════════════════════┐");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"   BEM-VINDO(A), {usuario.Nome.ToUpper()}!");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("└══════════════════════════════════════════════════════════════┘");

                // DESTAQUE DO SALDO - 3ª COR (Amarelo) + Branco para valores
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n   ┌──────────────────────────────────────────────┐");
                Console.Write("   │ ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("SEU SALDO: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"R$ {usuario.Conta.Saldo.ToString("N2")}");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("");
                Console.WriteLine("   └──────────────────────────────────────────────┘\n");

                // MENU - Branco para opções principais
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("   [1] » Sacar");
                Console.WriteLine("   [2] » Depositar");
                Console.WriteLine("   [3] » Transferir");
                Console.ForegroundColor = ConsoleColor.DarkGray; // Reutilizando a 1ª cor para a opção de saída
                Console.WriteLine("   [0] » Sair da Conta\n");

                // INPUT - Verde claro (2ª cor) + Branco para entrada
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("   ► Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                string opcao = Console.ReadLine();
                Console.ResetColor();

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
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n   Opção inválida!");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
