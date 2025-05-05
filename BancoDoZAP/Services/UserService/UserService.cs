using BancoDoZAP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Services.UserService
{
    public class UserService
    {
        public void Registrar()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
              _____            _     _             
             |  __ \          (_)   | |            
             | |__) |___  __ _ _ ___| |_ _ __ ___  
             |  _  // _ \/ _` | / __| __| '__/ _ \ 
             | | \ \  __/ (_| | \__ \ |_| | | (_) |
             |_|  \_\___|\__, |_|___/\__|_|  \___/ 
                          __/ |                    
                         |___/                     
            ");
            Console.ResetColor();

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("CPF: ");
            string cpf = Console.ReadLine();

            Console.Write("Telefone: ");
            string telefone = Console.ReadLine();

            Console.Write("Senha: ");
            string senha = LerSenha();

            Conta conta = new Conta(new Random().Next(1000, 9999), "0091", 0);
            Database.Database.Contas.Add(conta);

            Usuario novoUsuario = new Usuario(nome, cpf, telefone, senha, conta);
            Database.Database.Usuarios.Add(novoUsuario);


            Console.WriteLine("\nUsuário registrado com sucesso!");
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        public Usuario Logar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
              _                 _       
             | |               (_)      
             | |     ___   __ _ _ _ __  
             | |    / _ \ / _` | | '_ \ 
             | |___| (_) | (_| | | | | |
             |______\___/ \__, |_|_| |_|
                           __/ |        
                          |___/         
            ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("CPF: ");
            Console.ResetColor();
            string cpf = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Senha: ");
            Console.ResetColor();
            string senha = LerSenha();

            foreach (var usuario in Database.Database.Usuarios)
            {
                if (usuario.CPF == cpf && usuario.ValidarSenha(senha))
                {
                    LogadoComSucesso();

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Aguarde...");
                    Thread.Sleep(1000); // Espera 1 segundo

                    Console.WriteLine("Estamos te encaminhando...");
                    Thread.Sleep(1000);

                    Console.WriteLine("Preparando seu ambiente...");
                    Thread.Sleep(1000);

                    Console.ResetColor();
                    return usuario;
                }
            }

            Console.WriteLine("\nEmail ou senha incorretos!");
            Console.WriteLine("Pressione qualquer tecla para ir ao Painel...");
            Console.ReadKey();
            return null;
        }

        public void ListarUsuarios()
        {
            Console.Clear();
            Console.WriteLine("=== Lista de Usuários ===");

            if (Database.Database.Usuarios.Count == 0)
            {
                Console.WriteLine("Nenhum usuário cadastrado.");
            }
            else
            {
                foreach (var usuario in Database.Database.Usuarios)
                {
                    Console.WriteLine($"Nome: {usuario.Nome} | CPF: {usuario.CPF} | Numero da Conta: {usuario.Conta.NumeroConta}");
                }
            }

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }

        static string LerSenha()
        {
            string senha = "";
            ConsoleKeyInfo tecla;

            do
            {
                tecla = Console.ReadKey(intercept: true);

                if (tecla.Key == ConsoleKey.Backspace && senha.Length > 0)
                {
                    senha = senha.Substring(0, senha.Length - 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(tecla.KeyChar))
                {
                    senha += tecla.KeyChar;
                    Console.Write("*");
                }
            }
            while (tecla.Key != ConsoleKey.Enter);

            return senha;
        }

        static void LogadoComSucesso()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
              _                 _          _____            _ _              _        _  
             | |               (_)        |  __ \          | (_)            | |      | | 
             | |     ___   __ _ _ _ __    | |__) |___  __ _| |_ ______ _  __| | ___  | | 
             | |    / _ \ / _` | | '_ \   |  _  // _ \/ _` | | |_  / _` |/ _` |/ _ \ | | 
             | |___| (_) | (_| | | | | |  | | \ \  __/ (_| | | |/ / (_| | (_| | (_)  |_|
             |______\___/ \__, |_|_| |_|  |_|  \_\___|\__,_|_|_/___\__,_|\__,_|\___/ (_|
                           __/ |                                                         
                          |___/                                                          
            ");
            Console.ResetColor();
        }
    }
}
