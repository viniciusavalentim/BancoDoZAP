using BancoDoZAP.Enums;
using BancoDoZAP.Models;
using NAudio.Wave;
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

            Log log = new Log();
            log.CriarLog("Saque", Enums.TypeLog.Saque, usuario, valor, "Saque");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ResetColor();
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


            Log log = new Log("Deposito", Enums.TypeLog.Saque, usuario);
            log.CriarLog("Deposito", Enums.TypeLog.Deposito, usuario, valor, "Deposito");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ResetColor();
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

            Log log = new Log("Transferencia", Enums.TypeLog.Transferencia, usuario);
            log.CriarLog("Transferencia", Enums.TypeLog.Transferencia, usuario, valor, "Transferencia", contaDestino);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ResetColor();
        }


        public void VisualizarRelatório()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(@"
    ██████╗ ███████╗██╗      █████╗ ████████╗ ██████╗ ██████╗ ██╗ ██████╗ 
    ██╔══██╗██╔════╝██║     ██╔══██╗╚══██╔══╝██╔═══██╗██╔══██╗██║██╔═══██╗
    ██████╔╝█████╗  ██║     ███████║   ██║   ██║   ██║██████╔╝██║██║   ██║
    ██╔══██╗██╔══╝  ██║     ██╔══██║   ██║   ██║   ██║██╔══██╗██║██║   ██║
    ██║  ██║███████╗███████╗██║  ██║   ██║   ╚██████╔╝██║  ██║██║╚██████╔╝
    ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝  ╚═╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚═╝ ╚═════╝ 
    ");
            Console.ResetColor();

            var usuariosComuns = Database.Database.Usuarios.Where(u => u.TypeUser != "adm").ToList();

            if (!usuariosComuns.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNão há usuários comuns cadastrados para visualizar relatórios.");
                Console.ResetColor();
                Console.WriteLine("\nPressione qualquer tecla para voltar...");
                Console.ReadKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("\n══════════════════════════════════════════════════");
            Console.WriteLine("             USUÁRIOS DISPONÍVEIS");
            Console.WriteLine("══════════════════════════════════════════════════");
            Console.ResetColor();

            // Lista os usuários disponíveis
            foreach (var usuario in usuariosComuns)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"ID: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{usuario.Conta.Id} ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("| Nome: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{usuario.Nome} ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("| Conta: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{usuario.Conta.NumeroConta} ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("| Saldo: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"R$ {usuario.Conta.Saldo.ToString("N2")}");
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nDigite a conta do usuário para ver o relatório (ou 0 para cancelar):");
            Console.ResetColor();

            int idUsuario;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("► ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nOperação cancelada pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("\nPressione qualquer tecla para voltar...");
                    Console.ReadKey();
                    return;
                }

                if (!int.TryParse(input, out idUsuario))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nID inválido! Digite apenas números.");
                    Console.ResetColor();
                    continue;
                }

                var usuarioSelecionado = usuariosComuns.FirstOrDefault(u => u.Conta.NumeroConta == idUsuario);
                if (usuarioSelecionado == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nID não encontrado! Digite um ID válido da lista.");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            var usuarioRelatorio = usuariosComuns.First(u => u.Conta.NumeroConta == idUsuario);
            var logsUsuario = Database.Database.Logs.Where(l => l.Usuario?.Conta?.NumeroConta == idUsuario).ToList();

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("════════════════════════════════════════════════════════════════════");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n        RELATÓRIO DO USUÁRIO: {usuarioRelatorio.Nome.ToUpper()}");
            Console.WriteLine($"        CONTA: {usuarioRelatorio.Conta.NumeroConta}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("        SALDO ATUAL: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"R$ {usuarioRelatorio.Conta.Saldo.ToString("N2")}");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("════════════════════════════════════════════════════════════════════");
            Console.ResetColor();

            if (!logsUsuario.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nNenhuma operação registrada para este usuário.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("{0,-5} {1,-25} {2,-20} {3,-15} {4,-15}",
                                  "ID", "Data/Hora", "Operação", "Valor", "Destinatário");
                Console.WriteLine("════════════════════════════════════════════════════════════════════════════════════════════");
                Console.ResetColor();

                foreach (var log in logsUsuario.OrderByDescending(l => l.DataHora))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"{log.Id,-5} ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{log.DataHora,-25:dd/MM/yyyy HH:mm:ss} ");

                    // Cor baseada no tipo de operação
                    switch (log.Tipo)
                    {
                        case TypeLog.Deposito:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case TypeLog.Saque:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case TypeLog.Transferencia:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }

                    Console.Write($"{log.Tipo,-20} ");
                    Console.Write($"R$ {log.Value.ToString("N2"),-13} ");

                    if (log.UsuarioRecebido != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write($" {log.UsuarioRecebido.Nome}");
                    }

                    Console.WriteLine();
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n════════════════════════════════════════════════════════════════════════════════════════════");
            Console.WriteLine($"Total de operações: {logsUsuario.Count}");
            Console.WriteLine("\nPressione qualquer tecla para voltar...");
            Console.ResetColor();
            Console.ReadKey();
        }

    }
}