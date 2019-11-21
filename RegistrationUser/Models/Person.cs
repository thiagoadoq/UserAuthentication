using System;

namespace RegistrationUser.Models
{
    public class Person
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Cpf { get; set; }

        public string Uf { get; set; }

        public DateTime DataNascimento { get; set; }        
    }
}
