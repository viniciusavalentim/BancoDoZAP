using BancoDoZAP.Models;
using BancoDoZAP.Services.AccountService;
using BancoDoZAP.Services.UserService;
using Microsoft.Data.Sqlite;
using NAudio.Wave;
using System;

namespace BancoDoZap
{
    class Program
    {
        static string connectionString = "Data Source=bancodozap3.0.db";

        static void Main(string[] args)
        {
            CriarTabela();
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

        static void CriarTabela()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Pessoa (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    CPF TEXT NOT NULL UNIQUE,
                    Telefone TEXT
                );

                CREATE TABLE IF NOT EXISTS Administrador (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    CPF TEXT NOT NULL UNIQUE,
                    Telefone TEXT,
                    Cargo TEXT NOT NULL,
                    DataAdmissao TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Conta (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NumeroConta INTEGER NOT NULL,
                    Agencia TEXT NOT NULL,
                    Saldo REAL NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Usuario (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    CPF TEXT NOT NULL UNIQUE,
                    Telefone TEXT,
                    TypeUser TEXT NOT NULL,
                    Senha TEXT NOT NULL,
                    ContaId INTEGER,
                    FOREIGN KEY (ContaId) REFERENCES Conta(Id)
                );

                CREATE TABLE IF NOT EXISTS Log (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Descricao TEXT NOT NULL,
                    DataHora TEXT NOT NULL,
                    Tipo TEXT NOT NULL,
                    Value REAL NOT NULL,
                    TypeLogAccount TEXT,
                    UsuarioId INTEGER,
                    UsuarioRecebidoId INTEGER,
                    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
                    FOREIGN KEY (UsuarioRecebidoId) REFERENCES Usuario(Id)
                )";
                tableCmd.ExecuteNonQuery();
            }
        }

        static void InserirPessoa(string nome, int idade)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = "INSERT INTO Pessoas (Nome, Idade) VALUES ($nome, $idade)";
                insertCmd.Parameters.AddWithValue("$nome", nome);
                insertCmd.Parameters.AddWithValue("$idade", idade);

                insertCmd.ExecuteNonQuery();
            }

            Console.WriteLine("✅ Pessoa inserida com sucesso!");
        }

        static void ListarPessoas()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = "SELECT Id, Nome, Idade FROM Pessoas";

                using (var reader = selectCmd.ExecuteReader())
                {
                    Console.WriteLine("\n📋 Pessoas cadastradas:");
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var nome = reader.GetString(1);
                        var idade = reader.GetInt32(2);

                        Console.WriteLine($"{id} - {nome}, {idade} anos");
                    }
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
