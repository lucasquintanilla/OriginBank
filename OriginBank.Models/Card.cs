using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OriginBank.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}(-\d{4}){3}$", ErrorMessage = "The card number must have the following format: 1111-1111-1111-1111")]
        [StringLength(19, MinimumLength = 19)]
        //[DataType(DataType.CreditCard)]
        public string Number { get; set; }

        [Range(1000, 9999)]
        public int Pin { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Display(Name = "Is Blocked")]
        public bool IsBlocked { get; set; }
    }
}
