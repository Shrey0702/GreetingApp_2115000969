﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<GreetingEntity> Greetings { get; set; } = new List<GreetingEntity>();
    }
}
