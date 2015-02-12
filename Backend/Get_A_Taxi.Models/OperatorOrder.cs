using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Models
{
    public class OperatorOrder
    {
        [Key, ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        [ForeignKey("Operator")]
        public string OperatorId { get; set; }
        public virtual ApplicationUser Operator { get; set; }

        [StringLength(100)]
        public string Comment { get; set; }
    }
}
