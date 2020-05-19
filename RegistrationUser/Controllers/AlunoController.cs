using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistrationUser.Context;
using RegistrationUser.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {

        /// <summary>
        /// Pegando uma lista de pessoas cadastradas no banco de dados
        /// </summary>
        /// <returns>Lista de pessoas</returns>
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {                
                var listPerson = AppDataContext.GetPersons();                

                return Ok(listPerson);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message, });
            }
        }

        /// <summary>
        /// Pegando um pessoa cadastrada passando o ID.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Um objeto pessoa</returns>
        [Authorize]
        [HttpGet("Id/{id}")]
        public IActionResult Get(Guid Id)
        {
            try
            {
                var person = AppDataContext.GetPersonId(Id);

                if (person == null)
                {
                    return BadRequest(new ValidationResult($"O Id: {Id} do cliente informado não consta na nossa base de dados"));
                }
                else
                {
                    return Ok(person);
                }               
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message, });
            }
        }

        /// <summary>
        /// Pegando um objeto pessoa pelo Uf cadastrado.
        /// </summary>
        /// <param name="Uf"></param>
        /// <returns>Um Objeto pessoa</returns>
        [Authorize]
        [HttpGet("Uf/{uf}")]
        public IActionResult Get(string Uf)
        {
            try
            {
                var person = AppDataContext.GetPesonUf(Uf);

                if (person == null)
                {
                    return BadRequest(new ValidationResult($"A uf: {Uf} do cliente informado não consta na nossa base de dados "));
                }
                else
                {
                    return Ok(person);
                }                
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message, });
            }
        }

        /// <summary>
        /// Criando um novo resgistro de pessoa.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Um objeto de pessoa cadastrada no banco de dados</returns>
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] Person entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ValidationResultModel(ModelState));
                }

                var personDTO = new Person();

                //Obtendo a lista de pessoas cadastradas.
                var listPerson = AppDataContext.GetPersons();

                //Criando validação, se o cpf que está sendo inserido já existe na base de dados.
                var personValidetion = listPerson.Find(c => c.Cpf == entity.Cpf);

                if (personValidetion == null)
                {
                   personDTO = AppDataContext.Insert(entity);
                }
                else
                {
                    return BadRequest(new ValidationResult($"Cpf já cadastrado na nossa base: {entity.Cpf}"));
                }                

                return Ok(new { sucess = true, message = "Cliente cadastrado com sucesso.", personDTO });
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResult($"Não foi possivel efetuar o cadastro do cliente: {entity.UserName}-{ex.InnerException.Message}"));
            }
        }

        /// <summary>
        /// Alterando um registro no banco de dados pelo Id.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>O objeto pessoa alterado</returns>
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] Person entity)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ValidationResultModel(ModelState));
                }

                var personDTO = AppDataContext.GetPersonId(entity.Id);

                if (personDTO != null)
                {
                    AppDataContext.UpdatePerson(entity);

                    personDTO = AppDataContext.GetPersonId(entity.Id);
                }
                else
                {
                    return BadRequest(new ValidationResult($"Não foi possivel encontrar o cadastro do cliente pelo id: {entity.Id}"));
                }

                return Ok(personDTO);

            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResult($"Não foi possivel atualizar o cadastro do cliente: {entity.UserName}-{ex.InnerException.Message}"));
            }
        }

        /// <summary>
        /// Deleta um objeto pessoa pelo seu Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
       [Authorize]
        [HttpDelete]
        public IActionResult Delete(Guid Id)
        {
            try
            {
                var listPerson = AppDataContext.GetPersons();

                var personDTO = listPerson.Find(c => c.Id == Id);

                if (personDTO == null)
                {
                    return BadRequest(new ValidationResult($"O Id: {Id} do cliente informado não consta na nossa base de dados"));
                }
                else
                {
                    var person = AppDataContext.Delete(Id);
                }                

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ValidationResult($"Não foi possivel excluir o cadastro do cliente--{ex.InnerException.Message}"));
            }
        }
    }
}