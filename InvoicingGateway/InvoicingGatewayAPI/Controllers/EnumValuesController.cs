using Domain.Models;
using Domain.Utils;
using Domain.Utils.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingGatewayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumValuesController : ControllerBase
    {
        private readonly ILogger<EnumValuesController> logger;

        public EnumValuesController(ILogger<EnumValuesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("InvoiceStatus")]
        public async Task<IActionResult> GetInvoiceStatus()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<InvoiceStatusEnums>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }
        [HttpGet("ProductStatus")]
        public async Task<IActionResult> GetProductStatus()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<ProductsStatusEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("OrderStatus")]
        public async Task<IActionResult> GetOrderStatus()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist <OrderStatusEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("PaymentStatus")]
        public async Task<IActionResult> GetPaymentStatus()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<PaymentStatusEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("PaymentType")]
        public async Task<IActionResult> GetPaymentType()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<PaymentTypeEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("IntegrationType")]
        public async Task<IActionResult> GetIntegrationType()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<IntegrationTypeEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("AccessChannel")]
        public async Task<IActionResult> GetAccessChannel()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<AccessChannelEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("CategoryType")]
        public async Task<IActionResult> GetCategoryType()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<CategoryTypeEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("EntityType")]
        public async Task<IActionResult> GetEntityType()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<EntityTypeEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }
        [HttpGet("InvoiceCustomParametersType")]
        public async Task<IActionResult> GetInvoiceCustomType()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<InvoiceCustomParametersTypeEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }

        [HttpGet("AuditLog")]
        public async Task<IActionResult> GetAuditLog()
        {
            try
            {
                var res = new RestAPIGenericResponseDTO<List<EnumValues>>().WithSuccess(Constants.THARWAT_SUCCESS_CODE, Constants.SUCCESS_Message, EnumUtils.GetEnumValueslist<AuditLogEnum>());
                return Ok(await Task.FromResult(res));
            }
            catch (Exception ex)
            {
                logger.LogError("\n*****   \n" + ex.StackTrace + "\n*****   \n");
                return BadRequest(ex.StackTrace);
            }
        }
    }
}
