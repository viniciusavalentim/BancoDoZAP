namespace BancoDoZAP.Models
{
    public class Pessoa
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }

        public Pessoa(string nome, string cpf, string telefone)
        {
            Nome = nome;
            CPF = cpf;
            Telefone = telefone;
        }
        public virtual void ExibirInformacoes()
        {
            Console.WriteLine($"Nome: {Nome}, CPF: {CPF}, Telefone: {Telefone}");
        }
    }
}
