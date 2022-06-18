using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Api.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Authorize lại  !!!
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoOrder>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoOrder>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateOrderAsync(BaseOrderRequest request)
        {
            var result = await _orderService.CreateDraftOrderAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("confirm-by-user")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConfirmOrderByUserAsync(ConfirmOrderRequest request)
        {
            var result = await _orderService.ConfirmOrderByUserAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("confirm-by-staff/{OrderId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<string>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> ConfirmOrderByStaffAsync(string OrderId)
        {
            var result = await _orderService.ConfirmOrderByStaffAsync(OrderId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{OrderId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoOrder>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoOrder>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> DeleteOrderAsync(string OrderId)
        {
            var result = await _orderService.DeleteOrderAsync(OrderId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-info")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoOrder>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoOrder>), StatusCodes.Status400BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConstain.USER)]
        public async Task<IActionResult> GetInfoOrderAsync()
        {
            var result = await _orderService.GetInfoOrderDraftAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-detail-portal/{OrderId}")]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoOrder>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<DtoOrder>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetDetailsOrderPortalAsync(string OrderId)
        {
            var result = await _orderService.GetDetailsOrderPortalAsync(OrderId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all-pending-portal")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoOrder>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoOrder>>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetAllOrderPendingPortalAsync()
        {
            var result = await _orderService.GetAllOrderPendingPortalAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all-delivery-portal")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoOrder>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoOrder>>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetAllOrderDeliveryPortalAsync()
        {
            var result = await _orderService.GetAllOrderDeliveryPortalAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all-cancle-portal")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoOrder>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoOrder>>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetAllOrderCancleOrderPortalAsync()
        {
            var result = await _orderService.GetAllOrderCancleOrderPortalAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all-success-portal")]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoOrder>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AppActionResultMessage<IEnumerable<DtoOrder>>), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetAllOrderSuccessOrderPortalAsync()
        {
            var result = await _orderService.GetAllOrderSuccessOrderPortalAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
