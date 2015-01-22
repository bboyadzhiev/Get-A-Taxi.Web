using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        public byte[] Content { get; set; }

        [StringLength(4)]
        public string FileExtension { get; set; }
    }

}
