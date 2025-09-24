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

            var saque = Database.Database.Sacar(usuario.Conta.Id, valor);

            if (saque && usuario.Conta.Sacar(valor))
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

            var deposito = Database.Database.Depositar(usuario.Conta.NumeroConta, valor);

            if (deposito && usuario.Conta.Depositar(valor))
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

            foreach (var usuarioData in Database.Database.ListarUsuarios().Where(u => u.CPF != usuario.CPF))
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

            Usuario contaDestino = Database.Database.ListarUsuarios().FirstOrDefault(c => c.Conta.NumeroConta == numeroContaDestino);

            if (contaDestino != null)
            {
                var transferencia = Database.Database.Transferir(usuario.Conta.NumeroConta, contaDestino.Conta.NumeroConta, valor);
                if (transferencia && usuario.Conta.Transferir(valor, contaDestino.Conta))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nTransferência de R$ {valor.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
                    Console.WriteLine($"para {contaDestino.Nome} realizada com sucesso!");
                    Console.WriteLine($"\nNovo saldo: R$ {usuario.Conta.Saldo.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
                    Log log = new Log("Transferencia", Enums.TypeLog.Transferencia, usuario);
                    log.CriarLog("Transferencia", Enums.TypeLog.Transferencia, usuario, valor, "Transferencia", contaDestino);

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                    Console.ResetColor();
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

            var usuariosComuns = Database.Database.ListarUsuarios().Where(u => u.TypeUser != "adm").ToList();

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
                Console.Write($"{usuario.Id} ");
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

                var usuarioSelecionado = usuariosComuns.FirstOrDefault(u => u.Id == idUsuario);
                if (usuarioSelecionado == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nID não encontrado! Digite um ID válido da lista.");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            var usuarioRelatorio = usuariosComuns.First(u => u.Id == idUsuario);
            var logsUsuario = Database.Database.ObterLogsPorUsuario(idUsuario);

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


        public void SuasChaves(Usuario usuario)
        {
            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(@"
    ███████╗██╗   ██╗ █████╗ ███████╗     ██████╗██╗  ██╗ █████╗ ██╗   ██╗███████╗███████╗
    ██╔════╝██║   ██║██╔══██╗██╔════╝    ██╔════╝██║  ██║██╔══██╗██║   ██║██╔════╝██╔════╝
    ███████╗██║   ██║███████║███████╗    ██║     ███████║███████║██║   ██║█████╗  ███████╗
    ╚════██║██║   ██║██╔══██║╚════██║    ██║     ██╔══██║██╔══██║╚██╗ ██╔╝██╔══╝  ╚════██║
    ███████║╚██████╔╝██║  ██║███████║    ╚██████╗██║  ██║██║  ██║ ╚████╔╝ ███████╗███████║
    ╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝     ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝╚══════╝
            ");
                Console.ResetColor();


                if (usuario.Conta.PixChaves.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n Nenhuma Chave cadastrada.");
                    Console.ResetColor();
                }
                else
                {
                    int maxTipoLength = "Tipo".Length;
                    int maxValorLength = "Valor".Length;


                    foreach (var chave in usuario.Conta.PixChaves)
                    {
                        if (chave.Tipo.Length > maxTipoLength)
                            maxTipoLength = chave.Tipo.Length;

                        if (chave.Valor.Length > maxValorLength)
                            maxValorLength = chave.Valor.Length;
                    }

                    maxTipoLength += 2;
                    maxValorLength += 2;

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine();
                    Console.WriteLine($"┌{new string('─', maxTipoLength)}┬{new string('─', maxValorLength)}┐");
                    Console.WriteLine($"│ {"Tipo".PadRight(maxTipoLength - 1)}│ {"Valor".PadRight(maxValorLength - 1)}│");
                    Console.WriteLine($"├{new string('─', maxTipoLength)}┼{new string('─', maxValorLength)}┤");

                    // Linhas com os dados
                    foreach (var chave in usuario.Conta.PixChaves)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("│ ");
                        Console.Write(chave.Tipo.PadRight(maxTipoLength - 1));

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("│ ");
                        Console.WriteLine(chave.Valor.PadRight(maxValorLength - 1) + "│");
                    }

                    // Fecha a tabela
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"└{new string('─', maxTipoLength)}┴{new string('─', maxValorLength)}┘");

                }

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n═════════════════════════════════════════════════════════════");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n\n   [1] » Cadastrar nova chave");
                Console.WriteLine("   [2] » Editar chave");
                Console.WriteLine("   [3] » Excluir chave");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("   [0] » Sair");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("   ► Escolha uma opção: ");
                Console.ForegroundColor = ConsoleColor.White;
                string opcao = Console.ReadLine();
                Console.ResetColor();

                switch (opcao)
                {
                    case "1":
                        cadastrarNovaChave(usuario);
                        break;
                    case "2":
                        EditarChave(usuario);
                        Console.ReadKey();
                        break;
                    case "3":
                        ExcluirChave(usuario);
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

        public void cadastrarNovaChave(Usuario usuario)
        {
            while (true)
            {

                Console.Clear();

                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine(@"
    ███╗   ██╗ ██████╗ ██╗   ██╗ █████╗      ██████╗██╗  ██╗ █████╗ ██╗   ██╗███████╗
    ████╗  ██║██╔═══██╗██║   ██║██╔══██╗    ██╔════╝██║  ██║██╔══██╗██║   ██║██╔════╝
    ██╔██╗ ██║██║   ██║██║   ██║███████║    ██║     ███████║███████║██║   ██║█████╗  
    ██║╚██╗██║██║   ██║╚██╗ ██╔╝██╔══██║    ██║     ██╔══██║██╔══██║╚██╗ ██╔╝██╔══╝  
    ██║ ╚████║╚██████╔╝ ╚████╔╝ ██║  ██║    ╚██████╗██║  ██║██║  ██║ ╚████╔╝ ███████╗
    ╚═╝  ╚═══╝ ╚═════╝   ╚═══╝  ╚═╝  ╚═╝     ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝
        ");
                Console.ResetColor();


                // Verifica se as chaves estão em uso
                bool cpfEmUso = usuario.Conta.PixChaves.Exists(c => c.Tipo == "CPF");
                bool telefoneEmUso = usuario.Conta.PixChaves.Exists(c => c.Tipo == "Telefone");
                bool emailEmUso = usuario.Conta.PixChaves.Exists(c => c.Tipo == "E-mail");

                // Opção 1 - CPF
                if (cpfEmUso)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" [1] » CPF");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" (Em uso)");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" [1] » CPF");
                }
                Console.ResetColor();
                Console.WriteLine();

                // Opção 2 - Telefone
                if (telefoneEmUso)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" [2] » Telefone");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" (Em uso)");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" [2] » Telefone");
                }
                Console.ResetColor();
                Console.WriteLine();

                // Opção 3 - E-mail
                if (emailEmUso)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" [3] » E-mail");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" (Em uso)");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(" [3] » E-mail");
                }
                Console.ResetColor();
                Console.WriteLine();

                // Opção 4 - Chave Aleatória (sempre disponível)
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(" [4] » Chave Aleatória");

                // Opção 0 - Sair
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(" [0] » Sair");

                Console.ResetColor();


                Console.Write("\nEscolha uma opção: ");
                string opcao = Console.ReadLine();

                string tipoChave = "";
                string valorChave = "";
                bool emUso = false;

                switch (opcao)
                {
                    case "1":
                        if (cpfEmUso)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("CPF já está em uso como chave PIX!");
                            Console.ResetColor();
                            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                            Console.ReadKey();
                            return;
                        }

                        tipoChave = "CPF";
                        valorChave = ValidarCPF();

                        if (valorChave == "0") return;

                        break;

                    case "2":
                        if (telefoneEmUso)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Telefone já está em uso como chave PIX!");
                            Console.ResetColor();
                            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                            Console.ReadKey();
                            return;
                        }

                        tipoChave = "Telefone";
                        valorChave = validateTelefone();

                        if (valorChave == "0") return;

                        break;

                    case "3":
                        if (emailEmUso)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("E-mail já está em uso como chave PIX!");
                            Console.ResetColor();
                            Console.ResetColor();
                            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                            Console.ReadKey();
                            return;
                        }

                        tipoChave = "E-mail";
                        valorChave = ValidarEmail();

                        if (valorChave == "0") return;
                        break;

                    case "4":
                        tipoChave = "Chave Aleatória";
                        valorChave = Guid.NewGuid().ToString("N").ToUpper();
                        Console.WriteLine($"Chave aleatória gerada: {valorChave}");
                        break;

                    case "0":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opção inválida!");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }

                if (!string.IsNullOrEmpty(valorChave))
                {
                    var cadastrarChave = Database.Database.AdicionarChavePix(usuario.Conta.Id, tipoChave, valorChave);
                    var atualizarChavePix = usuario.Conta.CadastrarChavePix(tipoChave, valorChave);
                    if (cadastrarChave && atualizarChavePix)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Chave PIX adicionada com sucesso!");
                        Console.ResetColor();
                        Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Erro ao adicionar chave PIX. Tente novamente.");
                        Console.ResetColor();
                        return;
                    }
                }
            }

        }
        public void EditarChave(Usuario usuario)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(@"
    ███████╗██████╗ ██╗████████╗ █████╗ ██████╗      ██████╗██╗  ██╗ █████╗ ██╗   ██╗███████╗███████╗
    ██╔════╝██╔══██╗██║╚══██╔══╝██╔══██╗██╔══██╗    ██╔════╝██║  ██║██╔══██╗██║   ██║██╔════╝██╔════╝
    █████╗  ██║  ██║██║   ██║   ███████║██████╔╝    ██║     ███████║███████║██║   ██║█████╗  ███████╗
    ██╔══╝  ██║  ██║██║   ██║   ██╔══██║██╔══██╗    ██║     ██╔══██║██╔══██║╚██╗ ██╔╝██╔══╝  ╚════██║
    ███████╗██████╔╝██║   ██║   ██║  ██║██║  ██║    ╚██████╗██║  ██║██║  ██║ ╚████╔╝ ███████╗███████║
    ╚══════╝╚═════╝ ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝     ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝╚══════╝
        ");
            Console.ResetColor();

            var chaves = Database.Database.BuscarChavesPix(usuario.Conta.Id);

            if (chaves == null || !chaves.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Você não possui nenhuma chave Pix cadastrada.");
                Console.ResetColor();
                Console.WriteLine("Pressione qualquer tecla para voltar...");
                Console.ReadKey();
                return;
            }

            int maxTipo = Math.Max("Tipo".Length, chaves.Max(c => c.Tipo.Length)) + 2;
            int maxValor = Math.Max("Valor".Length, chaves.Max(c => c.Valor.Length)) + 2;

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n┌────┬{new string('─', maxTipo)}┬{new string('─', maxValor)}┐");
            Console.WriteLine($"│ #  │ {"Tipo".PadRight(maxTipo - 1)}│ {"Valor".PadRight(maxValor - 1)}│");
            Console.WriteLine($"├────┼{new string('─', maxTipo)}┼{new string('─', maxValor)}┤");

            for (int i = 0; i < chaves.Count; i++)
            {
                var chave = chaves[i];
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"│ {i + 1,-2} ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"│ {chave.Tipo.PadRight(maxTipo - 1)}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"│ {chave.Valor.PadRight(maxValor - 1)}│");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"└────┴{new string('─', maxTipo)}┴{new string('─', maxValor)}┘");
            Console.ResetColor();

            int escolha = -1;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\nDigite o número da chave que deseja editar (ou 0 para voltar): ");
                Console.ResetColor();

                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada vazia. Por favor, digite um número válido.");
                    Console.ResetColor();
                    continue;
                }

                if (!int.TryParse(input, out escolha))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inválida. Digite apenas números.");
                    Console.ResetColor();
                    continue;
                }

                if (escolha == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Operação cancelada. Retornando ao menu...");
                    Console.ResetColor();
                    return;
                }

                if (escolha < 1 || escolha > chaves.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Número fora do intervalo. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            var chaveSelecionada = chaves[escolha - 1];

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"\nVocê selecionou a chave: {chaveSelecionada.Tipo}: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(chaveSelecionada.Valor);
            Console.ResetColor();

            string novaChave = "";

            switch (chaveSelecionada.Tipo)
            {
                case "CPF":
                    novaChave = ValidarCPF();
                    break;
                case "Telefone":
                    novaChave = validateTelefone();
                    break;
                case "E-mail":
                    novaChave = ValidarEmail();
                    break;
            }


            if (!string.IsNullOrEmpty(novaChave))
            {
                var editarChave = Database.Database.EditarChavePix(chaveSelecionada.Id, chaveSelecionada.Tipo, novaChave, usuario.Conta.Id);
                var atualizarChavePix = usuario.Conta.EditarChavePix(chaveSelecionada.Id, chaveSelecionada.Tipo, novaChave);
                if (editarChave && atualizarChavePix)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Chave PIX editada com sucesso!");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    return;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Erro ao adicionar chave PIX. Tente novamente.");
                    Console.ResetColor();
                    return;
                }
            }
        }



        public void ExcluirChave(Usuario usuario)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
    ███████╗██╗  ██╗ ██████╗██╗     ██╗   ██╗██╗██████╗      ██████╗██╗  ██╗ █████╗ ██╗   ██╗███████╗
    ██╔════╝╚██╗██╔╝██╔════╝██║     ██║   ██║██║██╔══██╗    ██╔════╝██║  ██║██╔══██╗██║   ██║██╔════╝
    █████╗   ╚███╔╝ ██║     ██║     ██║   ██║██║██████╔╝    ██║     ███████║███████║██║   ██║█████╗  
    ██╔══╝   ██╔██╗ ██║     ██║     ██║   ██║██║██╔══██╗    ██║     ██╔══██║██╔══██║╚██╗ ██╔╝██╔══╝  
    ███████╗██╔╝ ██╗╚██████╗███████╗╚██████╔╝██║██║  ██║    ╚██████╗██║  ██║██║  ██║ ╚████╔╝ ███████╗
    ╚══════╝╚═╝  ╚═╝ ╚═════╝╚══════╝ ╚═════╝ ╚═╝╚═╝  ╚═╝     ╚═════╝╚═╝  ╚═╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝
        ");
            Console.ResetColor();

            var chaves = Database.Database.BuscarChavesPix(usuario.Conta.Id);

            if (chaves == null || !chaves.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Você não possui nenhuma chave Pix cadastrada.");
                Console.ResetColor();
                Console.WriteLine("Pressione qualquer tecla para voltar...");
                Console.ReadKey();
                return;
            }

            int maxTipo = Math.Max("Tipo".Length, chaves.Max(c => c.Tipo.Length)) + 2;
            int maxValor = Math.Max("Valor".Length, chaves.Max(c => c.Valor.Length)) + 2;

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n┌────┬{new string('─', maxTipo)}┬{new string('─', maxValor)}┐");
            Console.WriteLine($"│ #  │ {"Tipo".PadRight(maxTipo - 1)}│ {"Valor".PadRight(maxValor - 1)}│");
            Console.WriteLine($"├────┼{new string('─', maxTipo)}┼{new string('─', maxValor)}┤");

            for (int i = 0; i < chaves.Count; i++)
            {
                var chave = chaves[i];
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"│ {i + 1,-2} ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"│ {chave.Tipo.PadRight(maxTipo - 1)}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"│ {chave.Valor.PadRight(maxValor - 1)}│");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"└────┴{new string('─', maxTipo)}┴{new string('─', maxValor)}┘");
            Console.ResetColor();

            int escolha = -1;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\nDigite o número da chave que deseja excluir (ou 0 para voltar): ");
                Console.ResetColor();

                string input = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada vazia. Por favor, digite um número válido.");
                    Console.ResetColor();
                    continue;
                }

                if (!int.TryParse(input, out escolha))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inválida. Digite apenas números.");
                    Console.ResetColor();
                    continue;
                }

                if (escolha == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Operação cancelada. Retornando ao menu...");
                    Console.ResetColor();
                    return;
                }

                if (escolha < 1 || escolha > chaves.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Número fora do intervalo. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            var chaveSelecionada = chaves[escolha - 1];

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"\nA sua chave Pix do tipo {chaveSelecionada.Tipo} é: ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(chaveSelecionada.Valor);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" [1] Sim ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[2] Não");
                Console.ResetColor();

                Console.Write("> ");
                string opcao = Console.ReadLine()?.Trim();

                if (opcao == "1")
                {
                    var excluir = Database.Database.ExcluirChavePix(chaveSelecionada.Id);
                    if (excluir)
                    {
                        usuario.Conta.PixChaves.RemoveAll(c => c.Id == chaveSelecionada.Id);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Chave Pix excluída com sucesso!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Erro ao excluir chave Pix. Tente novamente.");
                        Console.ResetColor();
                    }

                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    return;
                }
                else if (opcao == "2")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Operação cancelada. Nenhuma chave foi excluída.");
                    Console.ResetColor();
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opção inválida. Digite 1 para Sim ou 2 para Não.");
                Console.ResetColor();
            }
        }

        private string ValidarCPFPix()
        {
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
                    return "0";
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
                else if (!ValidateCPF(cpfNumerico))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CPF inválido. Por favor, digite um CPF válido.");
                    Console.ResetColor();
                }
                else
                {
                    cpf = cpfNumerico;
                    if (!ConfirmarChavePix("CPF", cpf))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Vamos tentar novamente.\n");
                        Console.ResetColor();
                        continue;
                    }

                    break;
                }
            }

            return cpf;
        }


        private string ValidarCPF()
        {
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
                    return "0";
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
                else if (!ValidateCPF(cpfNumerico))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CPF inválido. Por favor, digite um CPF válido.");
                    Console.ResetColor();
                }
                else if (Database.Database.ListarTodasAsChavesPix().Any(u => u.Tipo == "CPF" && u.Valor == cpfNumerico))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CPF já cadastrado. Por favor, use outro CPF.");
                    Console.ResetColor();
                }
                else
                {
                    cpf = cpfNumerico;
                    if (!ConfirmarChavePix("CPF", cpf))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Vamos tentar novamente.\n");
                        Console.ResetColor();
                        continue;
                    }

                    break;
                }
            }

            return cpf;
        }

        private bool ValidateCPF(string cpf)
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

        private string validateTelefone()
        {
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
                    return "0";
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
                else if (Database.Database.ListarTodasAsChavesPix().Any(u => u.Tipo == "Telefone" && u.Valor == telefone))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Telefone já cadastrado em nossa base. Por favor, use outro telefone.");
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    if (!ConfirmarChavePix("Telefone", telefone))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Vamos tentar novamente.\n");
                        Console.ResetColor();
                        continue;
                    }

                    break;
                }
            }

            return telefone;
        }

        private string validateTelefonePix()
        {
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
                    return "0";
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
                    if (!ConfirmarChavePix("Telefone", telefone))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Vamos tentar novamente.\n");
                        Console.ResetColor();
                        continue;
                    }

                    break;
                }
            }

            return telefone;
        }
        private string ValidarEmail()
        {
            string email;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Email (ex: usuario123@dominio.com ou usuario1@dominio.com.br): ");
                Console.ForegroundColor = ConsoleColor.White;
                email = Console.ReadLine();

                if (email == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Registro cancelado pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return "0";
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O email não pode estar vazio. Por favor, digite novamente.");
                    Console.ResetColor();
                    continue;
                }

                string pattern = @"^(?=[^@]*[a-zA-Z])[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                bool formatoValido = System.Text.RegularExpressions.Regex.IsMatch(email, pattern);

                if (!formatoValido)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Formato de email inválido.");
                    Console.ResetColor();
                    continue;
                }

                if (Database.Database.ListarTodasAsChavesPix().Any(u => u.Tipo == "E-mail" && u.Valor == email.ToLower()))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Email já cadastrado em nossa base. Por favor, use outro email.");
                    Console.ResetColor();
                    continue;
                }

                if (!ConfirmarChavePix("E-mail", email))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Vamos tentar novamente.\n");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            return email;
        }

        private string ValidarEmailPix()
        {
            string email;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Email (ex: usuario123@dominio.com ou usuario1@dominio.com.br): ");
                Console.ForegroundColor = ConsoleColor.White;
                email = Console.ReadLine();

                if (email == "0")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Registro cancelado pelo usuário.");
                    Console.ResetColor();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    Console.ReadKey();
                    return "0";
                }

                if (string.IsNullOrWhiteSpace(email))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("O email não pode estar vazio. Por favor, digite novamente.");
                    Console.ResetColor();
                    continue;
                }

                string pattern = @"^(?=[^@]*[a-zA-Z])[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                bool formatoValido = System.Text.RegularExpressions.Regex.IsMatch(email, pattern);

                if (!formatoValido)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Formato de email inválido.");
                    Console.ResetColor();
                    continue;
                }

                if (!ConfirmarChavePix("E-mail", email))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Vamos tentar novamente.\n");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            return email;
        }

        private bool ConfirmarChavePix(string tipo, string valor)
        {
            if (tipo.Equals("CPF", StringComparison.OrdinalIgnoreCase))
            {
                if (valor.All(char.IsDigit) && valor.Length == 11)
                {
                    valor = Convert.ToUInt64(valor).ToString(@"000\.000\.000\-00");
                }
            }

            else if (tipo.Equals("Telefone", StringComparison.OrdinalIgnoreCase))
            {
                if (valor.All(char.IsDigit) && valor.Length == 11)
                {
                    valor = Convert.ToUInt64(valor).ToString(@"\(00\) 00000\-0000");
                }
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"\nA sua chave Pix do tipo {tipo} é: ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(valor);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(" [1] Sim ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[2] Não");
                Console.ResetColor();

                Console.Write("> ");
                string opcao = Console.ReadLine()?.Trim();

                if (opcao == "1")
                    return true;
                else if (opcao == "2")
                    return false;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opção inválida. Digite 1 para Sim ou 2 para Não.");
                Console.ResetColor();
            }
        }



        private bool ValidarTelefone(string telefone)
        {
            telefone = new string(telefone.Where(char.IsDigit).ToArray());

            return telefone.Length == 10 || telefone.Length == 11;
        }


        public void FazerPix(Usuario usuario)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
    ███████╗ █████╗ ███████╗███████╗██████╗     ██████╗ ██╗██╗  ██╗
    ██╔════╝██╔══██╗╚══███╔╝██╔════╝██╔══██╗    ██╔══██╗██║╚██╗██╔╝
    █████╗  ███████║  ███╔╝ █████╗  ██████╔╝    ██████╔╝██║ ╚███╔╝ 
    ██╔══╝  ██╔══██║ ███╔╝  ██╔══╝  ██╔══██╗    ██╔═══╝ ██║ ██╔██╗ 
    ██║     ██║  ██║███████╗███████╗██║  ██║    ██║     ██║██╔╝ ██╗
    ╚═╝     ╚═╝  ╚═╝╚══════╝╚══════╝╚═╝  ╚═╝    ╚═╝     ╚═╝╚═╝  ╚═╝
        ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\nSaldo atual: R$ {usuario.Conta.Saldo.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
            Console.WriteLine("Digite 0 em qualquer campo para cancelar a operação\n");
            Console.ResetColor();

            double valorPix;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Valor do PIX: R$ ");
                Console.ForegroundColor = ConsoleColor.White;
                string valorInput = Console.ReadLine();

                if (valorInput == "0")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nOperação de pix cancelada pelo usuário.");
                    Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
                    Console.ResetColor();
                    Console.ReadKey();
                    return;
                }

                if (!double.TryParse(valorInput, NumberStyles.Any, CultureInfo.CreateSpecificCulture("pt-BR"), out valorPix))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nValor inválido! Use números no formato brasileiro (ex: 1500,50).");
                    continue;
                }

                if (valorPix <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nO valor do pix deve ser maior que zero.");
                    continue;
                }

                if (valorPix > usuario.Conta.Saldo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nSaldo insuficiente. Seu saldo atual é R$ {usuario.Conta.Saldo.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR"))}");
                    continue;
                }

                break;
            }


            var tipos = new List<string> { "CPF", "Telefone", "E-mail", "Chave Aleatória" };

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nEscolha o tipo de chave para o destinatário:");
            Console.ResetColor();

            for (int i = 0; i < tipos.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write($" [{i + 1}] » {tipos[i]}");
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" [0] » Cancelar");
            Console.ResetColor();

            int escolhaTipo = -1;
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("\nDigite o número da opção: ");
                Console.ResetColor();
                string inputTipo = Console.ReadLine()?.Trim();

                if (escolhaTipo == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Operação cancelada.");
                    Console.ResetColor();
                    return;
                }
                if (string.IsNullOrWhiteSpace(inputTipo))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada vazia. Digite uma opção válida.");
                    Console.ResetColor();
                    continue;
                }

                if (!int.TryParse(inputTipo, out escolhaTipo))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inválida. Digite apenas números.");
                    Console.ResetColor();
                    continue;
                }


                if (escolhaTipo < 1 || escolhaTipo > tipos.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção fora do intervalo. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            string tipoChaveSelecionada = tipos[escolhaTipo - 1];

            string chaveDestinatario = "";

            switch (tipoChaveSelecionada)
            {
                case "CPF":
                    chaveDestinatario = ValidarCPFPix();
                    break;
                case "Telefone":
                    chaveDestinatario = validateTelefonePix();
                    break;
                case "E-mail":
                    chaveDestinatario = ValidarEmailPix();
                    break;
                case "Chave Aleatória":
                    chaveDestinatario = Console.ReadLine(); ;
                    break;
            }

            if (string.IsNullOrEmpty(chaveDestinatario) || chaveDestinatario == "0")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Operação de Pix cancelada ou chave inválida.");
                Console.ResetColor();
                return;
            }

            var usuarioDestino = Database.Database.BuscarUsuarioPorChavePix(tipoChaveSelecionada, chaveDestinatario);
            
            if (usuarioDestino == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Chave Pix não encontrada. Verifique os dados e tente novamente.");
                Console.ResetColor();
                return;
            }

            bool confirmado = ConfirmarOperacao(tipoChaveSelecionada, chaveDestinatario, valorPix, usuarioDestino.CPF, usuarioDestino);

            if (!confirmado)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Operação cancelada pelo usuário.");
                Console.ResetColor();
                return;
            }

            bool sucesso = Database.Database.TransferirPorChavePix(
                usuarioDestino.Conta.Id,
                tipoChaveSelecionada,
                chaveDestinatario,
                valorPix
            );

            if (sucesso)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pix realizado com sucesso!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro ao realizar Pix. Tente novamente mais tarde.");
            }
            Console.ResetColor();
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
        }


        private string MascaraCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11 || !cpf.All(char.IsDigit))
                return cpf;

            string parte1 = cpf.Substring(0, 3);
            return $"{parte1}.***.***‑{cpf.Substring(9, 2)}";
        }

        private bool ConfirmarOperacao(string tipoChave, string chave, double valor, string cpfUsuario, Usuario user)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Você está prestes a fazer um Pix para:\n");

                Console.Write("Nome: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(user.Nome);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Tipo de chave: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(tipoChave);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Chave: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(chave);
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Valor: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"R$ {valor:N2}");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("CPF do remetente: ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(MascaraCPF(cpfUsuario));
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n [1] Sim ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[2] Não");
                Console.ResetColor();

                Console.Write("> ");
                string opcao = Console.ReadLine()?.Trim();

                if (opcao == "1") return true;
                else if (opcao == "2") return false;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opção inválida. Digite 1 para Sim ou 2 para Não.");
                Console.ResetColor();
            }
        }

    }
}