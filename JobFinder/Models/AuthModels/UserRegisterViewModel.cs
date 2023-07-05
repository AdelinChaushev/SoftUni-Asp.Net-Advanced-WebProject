﻿using System.ComponentModel.DataAnnotations;

namespace JobFinder.Models.AuthModels
{
    public class UserRegisterViewModel
    {
        [MinLength(1)]
        [MaxLength(20)]
        [Required]
        public string Username { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
       

        public string ConfirmPassword { get; set; }


    }
}
