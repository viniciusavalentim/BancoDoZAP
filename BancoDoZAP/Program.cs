using System;
using BancoDoZAP.Models;
using BancoDoZAP.Services.AccountService;
using BancoDoZAP.Services.UserService;
using NAudio.Wave;

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
            try
            {
                using var audioFile = new AudioFileReader(@"c:\dev\zap.m4a");
                using var outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Play();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao tocar o áudio: " + ex.Message);
            }

            AccountService accountService = new AccountService();
            Console.Title = $"Banco ZAP - Conta de {usuario.Nome}";

            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(@"
 ██████╗  █████╗ ███╗   ██╗ ██████╗ ██████╗     ██████╗  ██████╗     ███████╗ █████╗ ██████╗ 
 ██╔══██╗██╔══██╗████╗  ██║██╔════╝██╔═══██╗    ██╔══██╗██╔═══██╗    ╚══███╔╝██╔══██╗██╔══██╗
 ██████╔╝███████║██╔██╗ ██║██║     ██║   ██║    ██║  ██║██║   ██║      ███╔╝ ███████║██████╔╝
 ██╔══██╗██╔══██║██║╚██╗██║██║     ██║   ██║    ██║  ██║██║   ██║     ███╔╝  ██╔══██║██╔═══╝ 
 ██████╔╝██║  ██║██║ ╚████║╚██████╗╚██████╔╝    ██████╔╝╚██████╔╝    ███████╗██║  ██║██║     
 ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚═════╝     ╚═════╝  ╚═════╝     ╚══════╝╚═╝  ╚═╝╚═╝   
                ");

                if (usuario.TypeUser == "adm")
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("   ┌──────────────────────────────────────────────┐");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   │           ACESSO DE ADMINISTRADOR            │");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("   └──────────────────────────────────────────────┘");
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("\n┌══════════════════════════════════════════════════════════════┐");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"   BEM-VINDO(A), {usuario.Nome.ToUpper()}!");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("└══════════════════════════════════════════════════════════════┘");



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

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("   [1] » Sacar");
                Console.WriteLine("   [2] » Depositar");
                Console.WriteLine("   [3] » Transferir");
                if (usuario.TypeUser == "adm")
                {
                    Console.WriteLine("   [4] » Ver relatório");
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("   [0] » Sair da Conta\n");

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
                    case "4" when usuario.TypeUser == "adm":
                        accountService.VisualizarRelatório();
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
