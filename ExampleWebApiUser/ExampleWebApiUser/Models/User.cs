using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleWebApiUser.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class Address
    {
        [Key]
        public long Id { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}
