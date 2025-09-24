using BancoDoZAP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Services.UserService
{
    public class UserService
    {
        public string connectionString = "Data Source=bancodozap3.0.db";

        public void Registrar()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
    ██████╗ ███████╗ ██████╗ ██╗███████╗████████╗██████╗  ██████╗ 
    ██╔══██╗██╔════╝██╔════╝ ██║██╔════╝╚══██╔══╝██╔══██╗██╔═══██╗
    ██████╔╝█████╗  ██║  ███╗██║███████╗   ██║   ██████╔╝██║   ██║
    ██╔══██╗██╔══╝  ██║   ██║██║╚════██║   ██║   ██╔══██╗██║   ██║
    ██║  ██║███████╗╚██████╔╝██║███████║   ██║   ██║  ██║╚██████╔╝
    ╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝╚══════╝   ╚═╝   ╚═╝  ╚═╝ ╚═════╝ 
            ");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Digite '0' em qualquer campo para cancelar o registro.");
            Console.ResetColor();

            string nome;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Nome: ");
                Console.ForegroundColor = ConsoleColor.White;
                nome = Console.ReadLine();

                if (nome == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Registro cancelado pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O nome não pode estar vazio. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else if (nome.Any(char.IsDigit))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O nome não pode conter números. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(nome, @"[^\p{L}\sçÇ]"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O nome não pode conter caracteres especiais (exceto ç). Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else if (nome.Length < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O nome deve conter dois ou mais caracteres. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            string cpf;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("CPF (apenas números ou formato 000.000.000-00): ");
                Console.ForegroundColor = ConsoleColor.White;
                cpf = Console.ReadLine();

                if (cpf == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Registro cancelado pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                if (string.IsNullOrWhiteSpace(cpf))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O CPF não pode estar vazio. Por favor, digite novamente.");
                    Console.ResetColor();
                    continue;
                }

                bool formatoValido = System.Text.RegularExpressions.Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$");

                string cpfNumerico = new string(cpf.Where(char.IsDigit).ToArray());

                if (cpf.Length != 11 && !formatoValido)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Formato inválido. Use apenas números ou o formato 000.000.000-00.");
                    Console.ResetColor();
                }
                else if (cpfNumerico.Length != 11)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CPF deve conter 11 dígitos. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else if (!ValidarCPF(cpfNumerico))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CPF inválido. Por favor, digite um CPF válido.");
                    Console.ResetColor();
                }
                else if (Database.Database.Usuarios.Any(u => u.CPF == cpfNumerico))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CPF já cadastrado. Por favor, use outro CPF.");
                    Console.ResetColor();
                }
                else
                {
                    cpf = cpfNumerico;
                    break;
                }
            }

            var usuarioExiste = Database.Database.UsuarioExiste(cpf);

            if (usuarioExiste)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nUsuário já cadastrado!");
                Console.ResetColor();
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                return;
            }

            string telefone;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Telefone (com DDD, apenas números): ");
                Console.ForegroundColor = ConsoleColor.White;
                telefone = Console.ReadLine();

                if (telefone == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Registro cancelado pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                if (string.IsNullOrWhiteSpace(telefone))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O telefone não pode estar vazio. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else if (!ValidarTelefone(telefone))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Telefone inválido. Por favor, digite um telefone válido (10 ou 11 dígitos).");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            string senha;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Senha (mínimo 6 caracteres): ");
                Console.ForegroundColor = ConsoleColor.White;
                senha = LerSenha();

                if (senha == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nRegistro cancelado pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return;
                }

                if (string.IsNullOrWhiteSpace(senha))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nA senha não pode estar vazia. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else if (senha.Length < 6)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nA senha deve ter no mínimo 6 caracteres. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine();
                    break;
                }
            }

            Conta conta = new Conta(new Random().Next(1000, 9999), "0091", 0);
            var createAccount = Database.Database.CriarConta(conta.NumeroConta, conta.Agencia, conta.Saldo);

            Usuario novoUsuario = new Usuario(nome, cpf, telefone, senha, conta);
            var createUser = Database.Database.RegistrarUsuario(nome, cpf, telefone, novoUsuario.TypeUser, senha);

            var newUserAccount = Database.Database.VincularContaAoUsuario(createUser, createAccount);

            if (newUserAccount)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nUsuário registrado com sucesso!");
                Console.ResetColor();
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNâo foi possível cadastrar o usuário!");
                Console.ResetColor();
                Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
            }
        }

        public Usuario Logar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
    ██╗      ██████╗  ██████╗ ██╗███╗   ██╗
    ██║     ██╔═══██╗██╔════╝ ██║████╗  ██║
    ██║     ██║   ██║██║  ███╗██║██╔██╗ ██║
    ██║     ██║   ██║██║   ██║██║██║╚██╗██║
    ███████╗╚██████╔╝╚██████╔╝██║██║ ╚████║
    ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝
                                       
            ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Digite '0' em qualquer campo para cancelar o login.");
            Console.ResetColor();

            string cpf;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("CPF (apenas números): ");
                Console.ForegroundColor = ConsoleColor.White;
                cpf = Console.ReadLine();

                if (cpf == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Login cancelado pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return null;
                }

                if (string.IsNullOrWhiteSpace(cpf))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O CPF não pode estar vazio. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            string senha;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Senha: ");
                Console.ForegroundColor = ConsoleColor.White;
                senha = LerSenha();

                if (senha == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nLogin cancelado pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return null;
                }

                if (string.IsNullOrWhiteSpace(senha))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nA senha não pode estar vazia. Por favor, digite novamente.");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine();
                    break;
                }
            }

            var validarUsuario = Database.Database.UsuarioExiste(cpf);

            if (validarUsuario)
            {
                var logarUsuario = Database.Database.LogarUsuario(cpf, senha);
                if (logarUsuario != null)
                {
                    LogadoComSucesso();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nAguarde...");
                    Thread.Sleep(1000);

                    Console.WriteLine("Estamos te encaminhando...");
                    Thread.Sleep(1000);

                    Console.WriteLine("Preparando seu ambiente...");
                    Thread.Sleep(1000);

                    Console.ResetColor();
                    return logarUsuario;
                }

            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nCPF ou senha incorretos!");
            Console.ResetColor();
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            return null;
        }

        public void ListarUsuarios()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ██╗   ██╗███████╗██╗   ██╗ █████╗ ██████╗ ██╗ ██████╗ ███████╗
    ██║   ██║██╔════╝██║   ██║██╔══██╗██╔══██╗██║██╔═══██╗██╔════╝
    ██║   ██║███████╗██║   ██║███████║██████╔╝██║██║   ██║███████╗
    ██║   ██║╚════██║██║   ██║██╔══██║██╔══██╗██║██║   ██║╚════██║
    ╚██████╔╝███████║╚██████╔╝██║  ██║██║  ██║██║╚██████╔╝███████║
     ╚═════╝ ╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝ ╚═════╝ ╚══════╝
                                                                                                      
                ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("═══════════════════════════════════════════");
            Console.ResetColor();

            var users = Database.Database.ListarUsuarios();

            if (users.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n⚠  Nenhum usuário cadastrado.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n┌────────────────────────┬───────────────┬─────────────────┐");
                Console.WriteLine("│         Nome           │      CPF      │  Número Conta   │");
                Console.WriteLine("├────────────────────────┼───────────────┼─────────────────┤");

                foreach (var usuario in users)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"│ {usuario.Nome,-22} ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"│ {FormatarCPF(usuario.CPF)}");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"│ {usuario.Conta.NumeroConta.ToString().PadRight(15)} │");
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("└────────────────────────┴───────────────┴─────────────────┘");
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ResetColor();
            Console.ReadKey();
        }

        private string FormatarCPF(string cpf)
        {
            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        private string LerSenha()
        {
            string senha = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    senha += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha = senha.Substring(0, (senha.Length - 1));
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            return senha;
        }

        static void LogadoComSucesso()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(@"
    ██╗      ██████╗  ██████╗ ██╗███╗   ██╗    ██████╗ ███████╗ █████╗ ██╗     ██╗███████╗ █████╗ ██████╗  ██████╗ ██╗
    ██║     ██╔═══██╗██╔════╝ ██║████╗  ██║    ██╔══██╗██╔════╝██╔══██╗██║     ██║╚══███╔╝██╔══██╗██╔══██╗██╔═══██╗██║
    ██║     ██║   ██║██║  ███╗██║██╔██╗ ██║    ██████╔╝█████╗  ███████║██║     ██║  ███╔╝ ███████║██║  ██║██║   ██║██║
    ██║     ██║   ██║██║   ██║██║██║╚██╗██║    ██╔══██╗██╔══╝  ██╔══██║██║     ██║ ███╔╝  ██╔══██║██║  ██║██║   ██║╚═╝
    ███████╗╚██████╔╝╚██████╔╝██║██║ ╚████║    ██║  ██║███████╗██║  ██║███████╗██║███████╗██║  ██║██████╔╝╚██████╔╝██╗
    ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝    ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚══════╝╚═╝╚══════╝╚═╝  ╚═╝╚═════╝  ╚═════╝ ╚═╝
            ");
            Console.ResetColor();
        }

        private bool ValidarCPF(string cpf)
        {
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (cpf.All(c => c == cpf[0]))
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        private bool ValidarTelefone(string telefone)
        {
            telefone = new string(telefone.Where(char.IsDigit).ToArray());

            return telefone.Length == 10 || telefone.Length == 11;
        }
    }
}
