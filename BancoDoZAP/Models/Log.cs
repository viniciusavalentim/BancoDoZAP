using BancoDoZAP.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoDoZAP.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataHora { get; set; } = DateTime.Now;
        public TypeLog Tipo { get; set; }
        public Usuario Usuario { get; set; }
        public double Value { get; set; }
        public string TypeLogAccount { get; set; }
        public Usuario UsuarioRecebido { get; set; } = null;

        public Log()
        {

        }

        public Log(string descricao, TypeLog tipo, Usuario usuario)
        {
            Descricao = descricao;
            Tipo = tipo;
            Usuario = usuario;
        }

        public bool CriarLog(string descricao, TypeLog tipo, Usuario usuario, double value = 0, string typeLogAccount = "", Usuario usuarioRecebido = null)
        {
            try
            {
                var novoLog = new Log
                {
                    Id = Database.Database.Logs.Count > 0 ? Database.Database.Logs.Max(l => l.Id) + 1 : 1,
                    Descricao = descricao,
                    DataHora = DateTime.Now,
                    Tipo = tipo,
                    Usuario = usuario,
                    Value = value,
                    TypeLogAccount = typeLogAccount,
                    UsuarioRecebido = usuarioRecebido
                };

                Database.Database.Logs.Add(novoLog);

                SalvarLogNoCSV(novoLog);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar log: {ex.Message}");
                return false;
            }
        }

        private static void SalvarLogNoCSV(Log log)
        {
            string caminhoArquivo = @"c:\dev\zap_log.csv";
            bool arquivoExiste = File.Exists(caminhoArquivo);

            try
            {
                using (var sw = new StreamWriter(caminhoArquivo, append: true))
                {
                    if (!arquivoExiste)
                    {
                        sw.WriteLine("Id;Descricao;DataHora;Tipo;UsuarioId;Value;TypeLogAccount;UsuarioRecebidoId");
                    }

                    string linha = $"{log.Id};{log.Descricao};{log.DataHora};{log.Tipo};{log.Usuario.Conta.Id};" +
                                  $"{log.Value.ToString(CultureInfo.InvariantCulture)};{log.TypeLogAccount};" +
                                  $"{(log.UsuarioRecebido != null ? log.UsuarioRecebido.Conta.Id.ToString() : "")}";

                    sw.WriteLine(linha);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar log no CSV: {ex.Message}");
            }
        }

    }
}
