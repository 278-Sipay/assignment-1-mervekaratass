using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
using SipayApi.Models;
using SipayApi.Validators;

namespace SipayApi.Controllers
{
        [ApiController]
        [Route("sipy/api/[controller]")]
        public class PersonController : ControllerBase
        {    

            //PersonValidator türünde bir değişken oluşturuldu 
             PersonValidator validator;

           //Constructor içinde modelin validasyonlarını gerçekleşmesi için PersonValidator nesnesi oluşturdum
            public PersonController()
            {
               validator = new PersonValidator();
            }

         
            [HttpPost]
            public IActionResult Post([FromBody] Person person)
            {
            //post metodu ile aldığımız person nesnesinin validator ile validate ederek kurallara  uyup uymadığına bakıyoruz
                var result = validator.Validate(person);
                if (!result.IsValid)
                {
                //validasyonumda hata varsa hatayı çekerek error e atıyoruz ve geriye bad request yani kötü istek ile beraber hatayı veriyoruz
                    var error = result.Errors.Select(e => e.ErrorMessage);     
                    return BadRequest(new { Errors = error });

                }
                //hata yoksa zaten personu başarılı bir şekilde döndürüyoruz
                return Ok(person);
            }
        }
    }

