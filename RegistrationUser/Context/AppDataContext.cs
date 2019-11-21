using LiteDB;
using RegistrationUser.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RegistrationUser.Context
{
    public class AppDataContext
    {
        //Criando o arquivo Register.db no diretório especificado.
        private static string LOG_LITE_DB_PATH => Path.Combine(GetAppDataPath(), "Register.db");

        //Criando inserção de dados no banco.
        public static Person  Insert(Person person)
        {
            //Instanciando objeto para inserir no banco de dados LiteDb.
            var personDTO = new Person
            {
                Id = Guid.NewGuid(),
                UserName = person.UserName,
                Cpf = person.Cpf,
                DataNascimento = person.DataNascimento.Date,
                Uf = person.Uf
            };

            //Conexão com o banco de dados e inserindo o (objeto) pessoa.
            using (var db = new LiteDatabase(LOG_LITE_DB_PATH))
            {
                var context = db.GetCollection<Person>(nameof(Person));
                    context.Insert(personDTO);

                return personDTO;
            }
        }

        //Trazendo do banco de dados uma pessoa pelo seu ID.
        public static Person GetPersonId(Guid id)
        {
            using (var db = new LiteDatabase(LOG_LITE_DB_PATH))
            {
                 var context = db.GetCollection<Person>(nameof(Person));
                 return context.FindById(id); 
            }
        }

        //Trazendo do banco de dados uma pessoa pelo UF.
        public static Person GetPesonUf(string Uf)
        {
            using (var db = new LiteDatabase(LOG_LITE_DB_PATH))
            {
                var context = db.GetCollection<Person>(nameof(Person));
                return context.FindAll().FirstOrDefault(c => c.Uf == Uf);
            }
        }

        //Trazendo do bando de dados uma lista de pessoas cadastrada no banco de dados.
        public static List<Person> GetPersons()
        {
            using (var db = new LiteDatabase(LOG_LITE_DB_PATH))
            {
                var context = db.GetCollection<Person>(nameof(Person));
                return context.FindAll().ToList();
            }           
        }

        //Alterando no banco de dados um (objeto) pessoa e retornando os dados alterados.
        public static Person UpdatePerson(Person person)
        {
            using (var db = new LiteDatabase(LOG_LITE_DB_PATH))
            {
                try
                {
                    var context = db.GetCollection<Person>(nameof(Person));
                    var data = context.Update(person);
                    return person;
                }
                catch (Exception)
                {
                    throw new Exception();
                }              

            }
        }

        //Excluído uma pessoa do banco de dados pelo ID.
        public static bool  Delete(Guid id)
        {
            using (var db = new LiteDatabase(LOG_LITE_DB_PATH))
            {
                var context = db.GetCollection<Person>(nameof(Person));
                context.Delete(id);

                return true;
            }            
        }

        //Criando o diretório na pasta Data do projeto.
        private static string GetAppDataPath()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Data");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
