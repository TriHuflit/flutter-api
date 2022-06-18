using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IOrderService
    {

        /// <summary>
        /// Admin Order
        /// </summary>
        /// <returns></returns>
        Task<AppActionResultMessage<string>> ConfirmOrderByStaffAsync(string OrderId);

        Task<AppActionResultMessage<string>> ComfirmOrderCancelByStaffAsync(string OrderId);

        Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderPendingPortalAsync();

        Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderDeliveryPortalAsync();

        Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderSuccessOrderPortalAsync();

        Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderCancleOrderPortalAsync();

        Task<AppActionResultMessage<DtoOrder>> GetDetailsOrderPortalAsync(string OrderId);
        /// <summary>
        /// User Order
        /// </summary>
        /// <returns></returns>
        Task<AppActionResultMessage<DtoOrder>> CreateDraftOrderAsync(BaseOrderRequest request);

        Task<AppActionResultMessage<string>> DeleteOrderAsync(string OrderId);

        Task<AppActionResultMessage<DtoOrder>> GetInfoOrderAsync();

        Task<AppActionResultMessage<string>> ConfirmOrderByUserAsync(ConfirmOrderRequest request);

        Task<AppActionResultMessage<string>> CancelOrderByUserAsync(string OrderId);

        Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderPendingAsync();

        Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderDeliveryAsync();

        Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderSuccessOrderAsync();

        Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderCancleOrderAsync();
    }
}
