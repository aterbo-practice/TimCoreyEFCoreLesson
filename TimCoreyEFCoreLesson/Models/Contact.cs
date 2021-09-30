using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoreyEFCoreLesson.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
        public List<Email> EmailAddresses { get; set; } = new List<Email>();
        public List<Phone> PhoneNumbers { get; set; } = new List<Phone>();
    }
}
