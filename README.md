[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-24ddc0f5d75046c5622901739e7c5dd533143b0c8e959d652212380cedb1ea36.svg)](https://classroom.github.com/a/0mNoXTBm)
# assignment-1


## Appendix

Person class icersindeki validation lar FluentValidation kutuphanesi kullanilarak yeniden duzenlenecek.
Controller uzerindeki POST methodu attributelar yerinde FluentValidation ile calisacak sekilde duzenlenecek. 
Odev icerisinde sadece 1 controller ve 1 method teslim edilecek. 



```c#=
public class Person
{
    [DisplayName("Staff person name")]
    [Required]
    [StringLength(maximumLength: 100, MinimumLength = 5)]
    public string Name { get; set; }


    [DisplayName("Staff person lastname")]
    [Required]
    [StringLength(maximumLength: 100, MinimumLength = 5)]
    public string Lastname { get; set; }


    [DisplayName("Staff person phone number")]
    [Required]
    [Phone]
    public string Phone { get; set; }


    [DisplayName("Staff person access level to system")]
    [Range(minimum: 1, maximum: 5)]
    [Required]
    public int AccessLevel { get; set; }



    [DisplayName("Staff person salary")]
    [Required]
    [Range(minimum: 5000, maximum: 50000)]
    [SalaryAttribute]
    public decimal Salary { get; set; }
}
```

# Fluent Validation
FluentValidation’un nasıl kullanılacağına başlamadan önce FluentValidation Nedir? ve Neden FluentValidation kullanmalıyız? sorularını cevaplamaya çalışalım.

<img align="right" src="https://miro.medium.com/v2/resize:fit:1400/1*n8b0eOFxMkNEXh5MGkg_Tg.png" alt="fluent validation" width="320" height="180">

<p align="left">FluentValidation bir veri doğrulama kütüphanesidir. FluentValidation ve benzeri ürünlerin kullanılması, verilerin doğru şekilde yani verilerin oluştururken konulmuş kısıtlamaları sağlayarak kurallara uyumlu halde olmasını ve kullanıcı ya da sistem kaynaklı hataların oluşmasını engeller.</p>

Neden FluentValidation?
FluentValidation yerine if else yapısı ve data annotation gibi yapılar kullanılıyor. İf else yapısı her zaman kodun anlaşabilirliğini azaltır ve yapısal olarak en son istenen sonuçlara neden olur. Daha çok data annotation kullanılıyor ve daha pratik kullanılıyor. Örnek data annotation kullanımı şöyle oluyor:

```c#
[StringLength(20, MinimumLength =2)]

public string name { get; set; }
```
Bu kullanım ise kod yazarken çoğunluk yazılımcının kullandığı SOLID prensiplerinin Open Closed ve dolayısıyla Single-Responsibility prensiplerine ters düşmektedir. Çünkü herhangi bir kural değişikliğinde ileride değişiklik yapabilme olasılığımız artmış olur böyle bir durumda da bağımlılık oluşmuş diyebiliriz.

FluentValidation kullandığımızda ise kurallar bir metot içerisinde belirleyeceğimiz için bu durumun önüne geçmiş oluruz.
## Ödevin yapılışı
- İlk olarak models klasörü altında bir Person sınıfı oluşturuldu.Bu sınıfı ayrı bir models klasöründe tutmamız hem yönetilebilirlik hemde modülerlik yaklaşımına daha uygun olmasını sağlar.

```c#
 public class Person
    {
        //Person modelimizi oluşturuyoruz
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public int AccessLevel { get; set; }
        public decimal Salary { get; set; }

    }
```
- Projede Fluent Validation ile validasyon yapmamız için Manage.NuGetPackages dan FluentValidation.Asp.NetCore paketini indirerek projeye dahil ediyoruz.
- Bu yüklediğimiz paketi Program.cs içinde services kısmına gelip ekliyoruz.
```c#
builder.Services.AddControllers().AddFluentValidation(a =>
{
    a.RegisterValidatorsFromAssemblyContaining<Program>();
});
```

- Validators klasörünün içine validasyon işlemini yapıcağımız kuarllarımızı yazıcağımız sınıfı oluşturuyoruz ve burada validasyon işlemlerimizi gerçekleştiriyoruz.

 ```c#
  public class PersonValidator : AbstractValidator<Person>
    {// Person sınıfına ait fluent validation kurallarını validator sınıfına yazıyorum
        public PersonValidator()
        {
            RuleFor(a => a.Name)
           .NotEmpty().WithMessage("Staff person name is not empty.")
           .Length(5, 100).WithMessage("Staff person name must be between 5 and 100 characters.");

            RuleFor(a => a.Lastname)
                .NotEmpty().WithMessage("Staff person lastname is not empty.")
                .Length(5, 100).WithMessage("Staff person lastname must be between 5 and 100 characters.");

            //Regex
            RuleFor(a => a.Phone)
                .NotEmpty().WithMessage("Staff person phone number is not empty.")
                .Matches(@"^\d{10}$").WithMessage("Staff person phone number is not valid.");

            RuleFor(a => a.AccessLevel)
                .InclusiveBetween(1, 5).WithMessage("Staff person access level must be between 1 and 5.");

            RuleFor(a => a.Salary)
            .NotEmpty().WithMessage("Staff person salary is not empty.")
            .InclusiveBetween(5000, 50000).WithMessage("Staff person salary must be between 5000 and 50000.");

        }
    }
```
- Controller sınıfında bu validasyon sınıfından nesne üreterek post metodu içinde modele valide işlemleri doğrusunda validasyonlarımız gerçekleşmişse doğru person nesnemizi, uymamışsa hata mesajlarımızı yolluyoruz.

 ```c#
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
```  

## Badges

Add badges from somewhere like: [shields.io](https://shields.io/)

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)
[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](https://opensource.org/licenses/)
[![AGPL License](https://img.shields.io/badge/license-AGPL-blue.svg)](http://www.gnu.org/licenses/agpl-3.0)

