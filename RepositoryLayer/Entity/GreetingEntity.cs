using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {
        [Key]
        public int GreetingID { get; set; }
        [Required]
        public string GreetingMessage { get; set; } = string.Empty;
    }
}
