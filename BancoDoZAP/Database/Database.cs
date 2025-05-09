using BancoDoZAP.Enums;
using BancoDoZAP.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BancoDoZAP.Database
{
    public static class Database
    {
        public static List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario(
                nome: "João Silva",
                cpf: "123",
                telefone: "(11) 91234-5678",
                senha: "1234",
                conta: new Conta(1, "0091", 30, 1),
                typeUser: "adm"
            ),
            new Usuario(
                nome: "Maria Souza",
                cpf: "321",
                telefone: "(21) 99876-5432",
                senha: "1234",
                conta: new Conta(2, "0091", 3232, 2)
            ),
            new Usuario(
                nome: "Pedro Santos",
                cpf: "555",
                telefone: "(31) 95555-6666",
                senha: "1234",
                conta: new Conta(3, "0091", 2120, 3)
            )
        };

        public static List<Conta> Contas = new List<Conta>
        {
            new Conta(1, "0091", 300, 1),
            new Conta(2, "0091", 3300, 2),
            new Conta(3, "0091", 30, 3)
        };

        public static List<Log> Logs = new List<Log>();

        static Database()
        {
            CarregarLogsDoCSV();
        }

        private static void CarregarLogsDoCSV()
        {
            try
            {
                string caminhoArquivo = @"c:\dev\zap_log.csv";

                if (!File.Exists(caminhoArquivo))
                {
                    Console.WriteLine("Arquivo de logs não encontrado. Criando dados mockados...");
                    CriarDadosMockados();
                    return;
                }

                var linhas = File.ReadAllLines(caminhoArquivo);

                var dados = linhas.Skip(1).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();

                foreach (var linha in dados)
                {
                    var valores = linha.Split(';');

                    try
                    {
                        var log = new Log();

                        if (valores.Length > 0 && int.TryParse(valores[0], out int id))
                            log.Id = id;
                        else
                            log.Id = Logs.Count + 1;

                        if (valores.Length > 1)
                            log.Descricao = valores[1];

                        if (valores.Length > 2 && DateTime.TryParse(valores[2], out DateTime dataHora))
                            log.DataHora = dataHora;

                        if (valores.Length > 3 && Enum.TryParse(valores[3], out TypeLog tipo))
                            log.Tipo = tipo;

                        if (valores.Length > 4 && int.TryParse(valores[4], out int usuarioId))
                            log.Usuario = Usuarios.FirstOrDefault(u => u.Conta.Id == usuarioId);

                        if (valores.Length > 5 && double.TryParse(valores[5], NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                            log.Value = value;

                        if (valores.Length > 6)
                            log.TypeLogAccount = valores[6];

                        if (valores.Length > 7 && int.TryParse(valores[7], out int usuarioRecebidoId))
                        {
                            log.UsuarioRecebido = Usuarios.FirstOrDefault(u => u.Conta.Id == usuarioRecebidoId);
                        }

                        Logs.Add(log);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar linha do log: {linha}. Erro: {ex.Message}");
                    }
                }

                if (Logs.Count == 0)
                {
                    CriarDadosMockados();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar logs do CSV: {ex.Message}");
                CriarDadosMockados();
            }
        }

        private static void CriarDadosMockados()
        {
            Logs = new List<Log>
            {
                new Log
                {
                    Id = 1,
                    Descricao = "Depósito",
                    DataHora = DateTime.Now.AddDays(-2),
                    Tipo = TypeLog.Deposito,
                    Usuario = Usuarios[0],
                    Value = 300,
                    TypeLogAccount = "Corrente"
                },
                new Log
                {
                    Id = 2,
                    Descricao = "Saque",
                    DataHora = DateTime.Now.AddDays(-1),
                    Tipo = TypeLog.Saque,
                    Usuario = Usuarios[1],
                    Value = 150,
                    TypeLogAccount = "Poupança"
                },
                new Log
                {
                    Id = 3,
                    Descricao = "Transferência",
                    DataHora = DateTime.Now,
                    Tipo = TypeLog.Transferencia,
                    Usuario = Usuarios[0],
                    Value = 50,
                    TypeLogAccount = "Corrente"
                }
            };
        }

    }
}
