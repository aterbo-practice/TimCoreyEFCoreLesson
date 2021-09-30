using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimCoreyEFCoreLesson.Models
{
    public class Email
    {
        public int Id { get; set; }

        //To map by name and ensure that orphaned records aren't left when doing the phone delete
        //The original RemovePhoneNumber method won't work with this
        [Required]
        public int ContactId { get; set; }

        [MaxLength(100)]   //Note you need to set max length twice for the C# code to recognize it
        [Column(TypeName = "varchar(100")] 
        [Required]
        public string EmailAddress { get; set; }
    }
}
