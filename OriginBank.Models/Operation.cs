using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OriginBank.Models
{
    public class Operation
    {
        public int Id { get; set; }

        [Display(Name = "Debit card ID")]
        public int CardId { get; set; }

        [Display(Name = "Operation code")]
        public int OperationCode { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Operation timestamp")]
        public DateTime Timestamp { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Withdrawal amount")]
        public decimal WithdrawalAmount { get; set; }
    }
}
