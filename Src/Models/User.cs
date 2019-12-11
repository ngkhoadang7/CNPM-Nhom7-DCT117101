using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
 
namespace MovieTheater.Models
{
    public class User: IdentityUser
    {
        public String Name {get; set;}

        [DataType(DataType.Date)]
        public DateTime DayOfBirth {get; set;}

        public string Gender {get; set;}
    }
}