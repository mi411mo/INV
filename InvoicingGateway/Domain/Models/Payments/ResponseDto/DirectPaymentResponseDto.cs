using Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Payments.ResponseDto
{
    public class DirectPaymentResponseDto
    {
        public DirectPaymentResponseDto(string transactionRef, decimal amount, string targetReference, TransactionStatusEnum transactionStatus)
        {
            this.transactionRef = transactionRef;
            this.amount = amount;
            TargetReference = targetReference;
            TransactionStatus = transactionStatus;
        }

        public string transactionRef { get; set; }
        public decimal amount { get; set; }
        public string TargetReference { get; set; }
        public TransactionStatusEnum TransactionStatus { get; set; }


    }
}
