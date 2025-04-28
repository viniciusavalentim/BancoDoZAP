using BancoDoZAP.Models;
using System;
using System.Collections.Generic;

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
                conta: new Conta(1, "0091", 30, 1)
            ),
            new Usuario(
                nome: "Maria Souza",
                cpf: "321",
                telefone: "(21) 99876-5432",
                senha: "1234",
                conta: new Conta(2, "0091", 0, 2)
            ),
            new Usuario(
                nome: "Pedro Santos",
                cpf: "555",
                telefone: "(31) 95555-6666",
                senha: "1234",
                conta: new Conta(3, "0091", 0, 3)
            )
        };

        public static List<Conta> Contas = new List<Conta>
        {
            new Conta(1, "0091", 300, 1),
            new Conta(2, "0091", 3300, 2),
            new Conta(3, "0091", 30, 3)
        };
    }
}
