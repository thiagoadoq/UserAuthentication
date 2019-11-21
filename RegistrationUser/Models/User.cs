using System;

namespace RegistrationUser.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }
    }   
}
