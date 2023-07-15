using SipayApi.Controllers;

namespace SipayApi.Models
{
    public class Person
    {
        //Person modelimizi oluşturuyoruz
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public int AccessLevel { get; set; }
        public decimal Salary { get; set; }

    }
}