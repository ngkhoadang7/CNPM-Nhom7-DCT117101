using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Models.TheaterViewModels
{
    public class UserRoleViewModel 
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsSelected { get; set; }
    }
}