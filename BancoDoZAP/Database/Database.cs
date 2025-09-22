using BancoDoZAP.Enums;
using BancoDoZAP.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BancoDoZAP.Database
{
    public static class Database
    {

        public static string connectionString = "Data Source=bancodozap3.0.db";

        public static List<Usuario> Usuarios = new List<Usuario>
        {
            new Usuario(
                nome: "Paula Tejando",
                cpf: "123",
                telefone: "(11) 91234-5678",
                senha: "1234",
                conta: new Conta(1, "0091", 30, 1),
                typeUser: "adm"
            ),
            new Usuario(
                nome: "Cuca Beludo",
                cpf: "321",
                telefone: "(21) 99876-5432",
                senha: "1234",
                conta: new Conta(2, "0091", 3232, 2)
            ),
            new Usuario(
                nome: "Zeca Gado",
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


        public static int RegistrarUsuario(string nome, string cpf, string telefone, string typeUser, string senha)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"
            INSERT INTO Usuario (Nome, CPF, Telefone, TypeUser, Senha)
            VALUES ($nome, $cpf, $telefone, $typeUser, $senha);

            SELECT last_insert_rowid();";

                insertCmd.Parameters.AddWithValue("$nome", nome);
                insertCmd.Parameters.AddWithValue("$cpf", cpf);
                insertCmd.Parameters.AddWithValue("$telefone", telefone);
                insertCmd.Parameters.AddWithValue("$typeUser", typeUser);
                insertCmd.Parameters.AddWithValue("$senha", senha);

                try
                {
                    long userId = (long)insertCmd.ExecuteScalar();
                    return (int)userId;
                }
                catch (SqliteException ex)
                {
                    return 0;
                }
            }
        }


        public static bool UsuarioExiste(string cpf)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = @"
                SELECT COUNT(1)
                FROM Usuario
                WHERE CPF = $cpf";

                selectCmd.Parameters.AddWithValue("$cpf", cpf);

                try
                {
                    long count = (long)selectCmd.ExecuteScalar();
                    return count > 0;
                }
                catch (SqliteException ex)
                {
                    return false;
                }
            }
        }


        public static Usuario LogarUsuario(string cpf, string senha)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = @"
                SELECT 
                    u.Id, u.Nome, u.CPF, u.Telefone, u.TypeUser, u.Senha,
                    c.Id, c.NumeroConta, c.Agencia, c.Saldo
                FROM Usuario u
                INNER JOIN Conta c ON u.ContaId = c.Id
                WHERE u.CPF = $cpf AND u.Senha = $senha";

                selectCmd.Parameters.AddWithValue("$cpf", cpf);
                selectCmd.Parameters.AddWithValue("$senha", senha);

                using (var reader = selectCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var conta = new Conta(
                            id: reader.GetInt32(6),
                            numeroConta: reader.GetInt32(7),
                            agencia: reader.GetString(8),
                            saldo: reader.GetDouble(9)
                        );

                        var usuario = new Usuario(
                            nome: reader.GetString(1),
                            cpf: reader.GetString(2),
                            telefone: reader.IsDBNull(3) ? "" : reader.GetString(3),
                            senha: reader.GetString(5),
                            conta: conta,
                            typeUser: reader.GetString(4)
                        );

                        return usuario;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }


        public static int CriarConta(int numeroConta, string agencia, double saldoInicial = 0)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"
                INSERT INTO Conta (NumeroConta, Agencia, Saldo)
                VALUES ($numeroConta, $agencia, $saldo);
                SELECT last_insert_rowid();";

                insertCmd.Parameters.AddWithValue("$numeroConta", numeroConta);
                insertCmd.Parameters.AddWithValue("$agencia", agencia);
                insertCmd.Parameters.AddWithValue("$saldo", saldoInicial);

                long contaId = (long)insertCmd.ExecuteScalar();
                return (int)contaId;
            }
        }

        public static bool VincularContaAoUsuario(int usuarioId, int contaId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var updateCmd = connection.CreateCommand();
                updateCmd.CommandText = @"
            UPDATE Usuario
            SET ContaId = $contaId
            WHERE Id = $usuarioId";

                updateCmd.Parameters.AddWithValue("$contaId", contaId);
                updateCmd.Parameters.AddWithValue("$usuarioId", usuarioId);

                int rows = updateCmd.ExecuteNonQuery();

                if (rows > 0)
                    return true;
                else
                    return false;
            }
        }

        public static List<Usuario> ListarUsuarios()
        {
            var usuarios = new List<Usuario>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var selectCmd = connection.CreateCommand();
                selectCmd.CommandText = @"
                SELECT u.Id, u.Nome, u.CPF, u.Telefone, u.TypeUser, u.Senha, c.NumeroConta
                FROM Usuario u
                LEFT JOIN Conta c ON u.Id = c.id";

                using (var reader = selectCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new Usuario(
                            nome: reader.GetString(1),
                            cpf: reader.GetString(2),
                            telefone: reader.GetString(3),
                            senha: reader.GetString(5),
                            conta: new Conta(
                                id: 0,
                                agencia: "",
                                saldo: 0,
                                numeroConta: reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                            ),
                            typeUser: reader.GetString(4)
                        )
                        {
                            Id = reader.GetInt32(0)
                        };

                        usuarios.Add(user);
                    }
                }
            }

            return usuarios;
        }

        public static bool Sacar(int numeroConta, double valor)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var checkCmd = connection.CreateCommand();
                        checkCmd.CommandText = "SELECT Id, Saldo FROM Conta WHERE NumeroConta = $numeroConta";
                        checkCmd.Parameters.AddWithValue("$numeroConta", numeroConta);

                        int contaId;
                        double saldoAtual;

                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return false;

                            contaId = reader.GetInt32(0);
                            saldoAtual = reader.GetDouble(1);
                        }

                        if (saldoAtual < valor)
                            return false;

                        var updateCmd = connection.CreateCommand();
                        updateCmd.CommandText = "UPDATE Conta SET Saldo = Saldo - $valor WHERE NumeroConta = $numeroConta";
                        updateCmd.Parameters.AddWithValue("$valor", valor);
                        updateCmd.Parameters.AddWithValue("$numeroConta", numeroConta);
                        updateCmd.ExecuteNonQuery();

                        var logCmd = connection.CreateCommand();
                        logCmd.CommandText = @"
                    INSERT INTO Log (Descricao, DataHora, Tipo, Value, TypeLogAccount, UsuarioId)
                    VALUES ('Saque realizado', $dataHora, 'SAQUE', $valor, 'DEBITO',
                           (SELECT Id FROM Usuario WHERE ContaId = $contaId))";
                        logCmd.Parameters.AddWithValue("$dataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        logCmd.Parameters.AddWithValue("$valor", valor);
                        logCmd.Parameters.AddWithValue("$contaId", contaId);
                        logCmd.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool Depositar(int numeroConta, double valor)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updateCmd = connection.CreateCommand();
                        updateCmd.CommandText = "UPDATE Conta SET Saldo = Saldo + $valor WHERE NumeroConta = $numeroConta";
                        updateCmd.Parameters.AddWithValue("$valor", valor);
                        updateCmd.Parameters.AddWithValue("$numeroConta", numeroConta);

                        int linhas = updateCmd.ExecuteNonQuery();
                        if (linhas == 0) return false;

                        var logCmd = connection.CreateCommand();
                        logCmd.CommandText = @"
                    INSERT INTO Log (Descricao, DataHora, Tipo, Value, TypeLogAccount, UsuarioId)
                    VALUES ('Depósito realizado', $dataHora, 'DEPOSITO', $valor, 'CREDITO',
                           (SELECT Id FROM Usuario WHERE ContaId = (SELECT Id FROM Conta WHERE NumeroConta = $numeroConta)))";
                        logCmd.Parameters.AddWithValue("$dataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        logCmd.Parameters.AddWithValue("$valor", valor);
                        logCmd.Parameters.AddWithValue("$numeroConta", numeroConta);
                        logCmd.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public static bool Transferir(int numeroContaOrigem, int numeroContaDestino, double valor)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var checkCmd = connection.CreateCommand();
                        checkCmd.CommandText = "SELECT Id, Saldo FROM Conta WHERE NumeroConta = $contaOrigem";
                        checkCmd.Parameters.AddWithValue("$contaOrigem", numeroContaOrigem);

                        int contaIdOrigem;
                        double saldoOrigem;

                        using (var reader = checkCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return false;

                            contaIdOrigem = reader.GetInt32(0);
                            saldoOrigem = reader.GetDouble(1);
                        }

                        if (saldoOrigem < valor)
                            return false;

                        var updateOrigemCmd = connection.CreateCommand();
                        updateOrigemCmd.CommandText = "UPDATE Conta SET Saldo = Saldo - $valor WHERE NumeroConta = $contaOrigem";
                        updateOrigemCmd.Parameters.AddWithValue("$valor", valor);
                        updateOrigemCmd.Parameters.AddWithValue("$contaOrigem", numeroContaOrigem);
                        updateOrigemCmd.ExecuteNonQuery();

                        var updateDestinoCmd = connection.CreateCommand();
                        updateDestinoCmd.CommandText = "UPDATE Conta SET Saldo = Saldo + $valor WHERE NumeroConta = $contaDestino";
                        updateDestinoCmd.Parameters.AddWithValue("$valor", valor);
                        updateDestinoCmd.Parameters.AddWithValue("$contaDestino", numeroContaDestino);
                        int linhasDestino = updateDestinoCmd.ExecuteNonQuery();
                        if (linhasDestino == 0) throw new Exception("Conta destino não encontrada");

                        var logOrigemCmd = connection.CreateCommand();
                        logOrigemCmd.CommandText = @"
                    INSERT INTO Log (Descricao, DataHora, Tipo, Value, TypeLogAccount, UsuarioId, UsuarioRecebidoId)
                    VALUES ('Transferência enviada', $dataHora, 'TRANSFERENCIA', $valor, 'DEBITO',
                            (SELECT Id FROM Usuario WHERE ContaId = $contaIdOrigem),
                            (SELECT Id FROM Usuario WHERE ContaId = (SELECT Id FROM Conta WHERE NumeroConta = $contaDestino)))";
                        logOrigemCmd.Parameters.AddWithValue("$dataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        logOrigemCmd.Parameters.AddWithValue("$valor", valor);
                        logOrigemCmd.Parameters.AddWithValue("$contaIdOrigem", contaIdOrigem);
                        logOrigemCmd.Parameters.AddWithValue("$contaDestino", numeroContaDestino);
                        logOrigemCmd.ExecuteNonQuery();

                        var logDestinoCmd = connection.CreateCommand();
                        logDestinoCmd.CommandText = @"
                    INSERT INTO Log (Descricao, DataHora, Tipo, Value, TypeLogAccount, UsuarioId, UsuarioRecebidoId)
                    VALUES ('Transferência recebida', $dataHora, 'TRANSFERENCIA', $valor, 'CREDITO',
                            (SELECT Id FROM Usuario WHERE ContaId = (SELECT Id FROM Conta WHERE NumeroConta = $contaDestino)),
                            (SELECT Id FROM Usuario WHERE ContaId = $contaIdOrigem))";
                        logDestinoCmd.Parameters.AddWithValue("$dataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        logDestinoCmd.Parameters.AddWithValue("$valor", valor);
                        logDestinoCmd.Parameters.AddWithValue("$contaDestino", numeroContaDestino);
                        logDestinoCmd.Parameters.AddWithValue("$contaIdOrigem", contaIdOrigem);
                        logDestinoCmd.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }



    }
}
