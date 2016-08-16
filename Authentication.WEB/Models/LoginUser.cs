﻿using System.ComponentModel.DataAnnotations;

namespace InsuredTraveling.Models
{
    public class LoginUser
    {
        [Required]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string grant_type { get; set; }
    }
}