using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OriginBank.Web.Models
{
    public class WithdrawalResultViewModel
    {
        [Required]
        [RegularExpression(@"^\d{4}(-\d{4}){3}$", ErrorMessage = "The card number must have the following format: 1111-1111-1111-1111")]
        public string Number { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Operation timestamp")]
        public DateTime CurrentTime { get => DateTime.Now; }

        [DataType(DataType.Currency)]
        [Display(Name = "Withdrawal amount")]
        public decimal WithdrawalAmount { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Remaining balance")]
        public decimal RemainingBalance { get; set; }
    }
}
