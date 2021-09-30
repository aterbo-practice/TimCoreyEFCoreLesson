using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TimCoreyEFCoreLesson.Models
{
    public class Phone
    {
        public int Id { get; set; }

        //To map by name and ensure that orphaned records aren't left
        public int ContactId { get; set; }

        [MaxLength(20)]   //Note you need to set max length twice for the C# code to recognize it
        [Column(TypeName = "varchar(20")]
        [Required]
        public string PhoneNumber { get; set; }
    }
}
