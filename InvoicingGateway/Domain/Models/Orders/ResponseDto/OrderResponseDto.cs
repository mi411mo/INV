using Domain.Entities;
using Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Orders.ResponseDto
{
    public class OrderResponseDto
    {
        public decimal TotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string OrderReference { get; set; }
        public OrderStatusEnum Status { get; set; }


        public OrderResponseDto ToResponse (OrderModel entity)
        {
            return new OrderResponseDto()
            {
                TotalAmount = entity.TotalAmount,
                CurrencyCode = entity.CurrencyCode,
                OrderReference = entity.OrderReference,
                Status = OrderStatusEnum.Pending

            };
        }

    }
}
