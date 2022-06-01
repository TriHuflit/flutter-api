﻿using AutoMapper;
using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.IServices;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Flutter.Backend.Common.Constains.MessageResConstain;

namespace Flutter.Backend.Service.Services
{
    public class OrderService : GenericErrorTextService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IClassifyProductRepository _classifyProductRepository;
        private readonly ITemplateSendMailRepository _templateSendMailRepository;

        private readonly IMapper _mapper;

        private readonly ICurrentUserService _currentUserService;
        private readonly ISendMailService _sendMailService;

        public OrderService(IOrderRepository orderRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IAppUserRepository appUserRepository,
            IMessageService messageService,
            ISendMailService sendMailService,
            IVoucherRepository voucherRepository,
            IClassifyProductRepository classifyProductRepository,
            ITemplateSendMailRepository templateSendMailRepository) : base(messageService)
        {

            _orderRepository = orderRepository;
            _appUserRepository = appUserRepository;
            _currentUserService = currentUserService;
            _sendMailService = sendMailService;
            _mapper = mapper;
            _voucherRepository = voucherRepository;
            _classifyProductRepository = classifyProductRepository;
            _templateSendMailRepository = templateSendMailRepository;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoOrder>> CreateDraftOrderAsync(BaseOrderRequest request)
        {
            var result = new AppActionResultMessage<DtoOrder>();

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(_currentUserService.UserId));
            }

            var user = await _appUserRepository.GetAsync(u => u.Id == objUser);

            var order = await _orderRepository.GetAsync(o => o.UserId == user.Id && o.Status == StatusOrderConstain.DRAFT);

            if (order == null)
            {
                order = new Order
                {
                    UserOrder = user.FullName,
                    UserId = user.Id,
                    Status = StatusOrderConstain.DRAFT,
                    AddressReceive = user.Location,
                    PhoneReceive = user.Phone,
                    Email = user.Email,
                    OrderDetails = request.OrderDetails
                };
            }
            else
            {
                order.OrderDetails = request.OrderDetails;

            }

            foreach (var item in order.OrderDetails)
            {
                order.TotalPrice += item.Price * item.Count;
            }

            order.SetFullInfor(user.Id.ToString(), user.UserName);
            _orderRepository.Add(order);

            var dtoOrder = _mapper.Map<Order, DtoOrder>(order);

            return await BuildResult(result, dtoOrder, MSG_FIND_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoOrder>> UpdateOrderAsync(UpdateOrderRequest request)
        {
            var result = new AppActionResultMessage<DtoOrder>();

            if (!ObjectId.TryParse(request.Id, out ObjectId objOrder))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.Id));
            }

            var order = await _orderRepository.GetAsync(o => o.Id == objOrder && (
            o.Status != StatusOrderConstain.DELIVERY || o.Status != StatusOrderConstain.CONFIRM
            || o.Status != StatusOrderConstain.SUCCESS));

            if (order == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(request.Id));
            }
            else
            {
                order.OrderDetails = request.OrderDetails;
            }
            order.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _orderRepository.Update(order, o => o.Id == objOrder);

            return await BuildResult(result, MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<string>> DeleteOrderAsync(string OrderId)
        {
            var result = new AppActionResultMessage<string>();


            if (!ObjectId.TryParse(OrderId, out ObjectId objOrder))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(OrderId));
            }

            var order = await _orderRepository.GetAsync(o => o.Id == objOrder && o.Status == StatusOrderConstain.DRAFT);

            if (order == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(order));
            }

            _orderRepository.DeleteAll(o => o.Id == objOrder);

            return await BuildResult(result, MSG_DELETE_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<DtoOrder>> GetInfoOrderAsync()
        {
            var result = new AppActionResultMessage<DtoOrder>();

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, _currentUserService.UserId, ERR_MSG_ID_ISVALID_FORMART);
            }

            var user = await _appUserRepository.GetAsync(u => u.Id == objUser);
            var order = await _orderRepository.GetAsync(o => o.UserId == user.Id && o.Status == StatusOrderConstain.DRAFT);
            var dtoOrder = _mapper.Map<Order, DtoOrder>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<AppActionResultMessage<string>> ConfirmOrderByStaffAsync(string OrderId)
        {
            var result = new AppActionResultMessage<string>();



            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<AppActionResultMessage<string>> ComfirmOrderCancelByStaffAsync(string OrderId)
        {
            // Modify stock classifyProduct
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderPendingPortalAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoOrder>>();

            var order = await _orderRepository.FindByAsync(o => o.Status == StatusOrderConstain.PENDING);
            order = order.OrderBy(o => o.CreatedByTime);

            var dtoOrder = _mapper.Map<IEnumerable<Order>, IEnumerable<DtoOrder>>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderDeliveryPortalAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoOrder>>();

            var order = await _orderRepository.FindByAsync(o => o.Status == StatusOrderConstain.DELIVERY);
            order = order.OrderBy(o => o.CreatedByTime);

            var dtoOrder = _mapper.Map<IEnumerable<Order>, IEnumerable<DtoOrder>>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderSuccessOrderPortalAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoOrder>>();

            var order = await _orderRepository.FindByAsync(o => o.Status == StatusOrderConstain.SUCCESS);
            order = order.OrderBy(o => o.CreatedByTime);

            var dtoOrder = _mapper.Map<IEnumerable<Order>, IEnumerable<DtoOrder>>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderCancleOrderPortalAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoOrder>>();

            var order = await _orderRepository.FindByAsync(o => o.Status == StatusOrderConstain.CANCEL);
            order = order.OrderBy(o => o.CreatedByTime);

            var dtoOrder = _mapper.Map<IEnumerable<Order>, IEnumerable<DtoOrder>>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public Task<AppActionResultMessage<DtoOrder>> GetDetailsOrderPortalAsync(string OrderId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public async Task<AppActionResultMessage<string>> ConfirmOrderByUserAsync(ConfirmOrderRequest request)
        {
            var result = new AppActionResultMessage<string>();


            if (!ObjectId.TryParse(request.Id, out ObjectId objOrder))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.Id));
            }

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(_currentUserService.UserId));
            }

            var order = await _orderRepository.GetAsync(o => o.Id == objOrder &&
                                                          o.UserId == objUser && o.Status == StatusOrderConstain.DRAFT);

            if (order == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(order));
            }

            if (!ObjectId.TryParse(request.VoucherId, out ObjectId objVoucher))
            {
                return await BuildError(result, ERR_MSG_ID_ISVALID_FORMART, nameof(request.VoucherId));
            }


            var voucher = await _voucherRepository.GetAsync(v => v.Id == objVoucher && v.FromDate >= DateTime.UtcNow
                                                                    && v.ToDate <= DateTime.UtcNow && v.IsShow == IsShowConstain.ACTIVE);

            if (voucher == null)
            {
                return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(request.VoucherId));
            }

            order.AddressReceive = request.AddressReceive;
            order.PhoneReceive = request.PhoneReceive;
            order.VoucherId = objVoucher;
            order.Description = request.Description;
            order.IsPayment = request.IsPayment;
            order.Status = StatusOrderConstain.PENDING;
            order.OrderDetails = request.OrderDetails;

            /// caculator total with voucher

            foreach (var item in order.OrderDetails)
            {
                var classifyProduct = await _classifyProductRepository.GetAsync(c => c.Id == item.ClassifyProductId
                && c.IsShow == IsShowConstain.ACTIVE);

                if (classifyProduct == null)
                {
                    return await BuildError(result, ERR_MSG_DATA_NOT_FOUND, nameof(classifyProduct));
                }

                classifyProduct.Stock = classifyProduct.Stock - item.Count;
                if (classifyProduct.Stock < 0)
                {
                    // Add message
                    return await BuildError(result, "Sản phẩm đã hết số lượng tồn kho", nameof(classifyProduct));
                }

                order.TotalPrice += item.Price * item.Count;

                classifyProduct.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
                _classifyProductRepository.Update(classifyProduct, c => c.Id == classifyProduct.Id);
            }

            if (voucher.FromCondition >= order.TotalPrice && order.TotalPrice <= voucher.ToCondition)
            {
                if (voucher.DisCountPercent != 0)
                {
                    order.TotalPrice = order.TotalPrice - (order.TotalPrice / voucher.DisCountPercent);
                }
                else
                {
                    order.TotalPrice = order.TotalPrice - voucher.DisCountAmount;
                }
            }

            order.SetUpdatedInFor(_currentUserService.UserId, _currentUserService.UserName);
            _orderRepository.Update(order, o => o.Id == objOrder);

            var template = await _templateSendMailRepository.GetAsync(t => t.Key == SendMailConstain.TemplateEmailConfirm);
            var requestSendMail = new MailRequest
            {
                Body =String.Format(template.TemplateHTML,order.Id),
                Subject = SendMailConstain.SubjectRegister,
                ToEmail = order.Email
            };

            var sendMailResult = await _sendMailService.SendMailRegisterAsync(requestSendMail);
            if (!sendMailResult.IsSuccess)
            {
                return await BuildError(result, ERR_MSG_EMAIL_IS_NOT_CONFIRM);
            }

            // Add Message
            return await BuildResult(result, order.Id.ToString(), "Đặt hàng thành công");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public Task<AppActionResultMessage<string>> CancelOrderByUserAsync(string OrderId)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderPendingAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoOrder>>();

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, _currentUserService.UserId, ERR_MSG_ID_ISVALID_FORMART);
            }

            var order = await _orderRepository.FindByAsync(o => o.UserId == objUser && o.Status == StatusOrderConstain.PENDING);
            order = order.OrderBy(o => o.CreatedByTime);

            var dtoOrder = _mapper.Map<IEnumerable<Order>, IEnumerable<DtoOrder>>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>       
        public async Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderDeliveryAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoOrder>>();

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, _currentUserService.UserId, ERR_MSG_ID_ISVALID_FORMART);
            }

            var order = await _orderRepository.FindByAsync(o => o.UserId == objUser && o.Status == StatusOrderConstain.DELIVERY);
            order = order.OrderBy(o => o.CreatedByTime);

            var dtoOrder = _mapper.Map<IEnumerable<Order>, IEnumerable<DtoOrder>>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>    
        public async Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderSuccessOrderAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoOrder>>();

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, _currentUserService.UserId, ERR_MSG_ID_ISVALID_FORMART);
            }

            var order = await _orderRepository.FindByAsync(o => o.UserId == objUser && o.Status == StatusOrderConstain.SUCCESS);
            order = order.OrderBy(o => o.CreatedByTime);

            var dtoOrder = _mapper.Map<IEnumerable<Order>, IEnumerable<DtoOrder>>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<AppActionResultMessage<IEnumerable<DtoOrder>>> GetAllOrderCancleOrderAsync()
        {
            var result = new AppActionResultMessage<IEnumerable<DtoOrder>>();

            if (!ObjectId.TryParse(_currentUserService.UserId, out ObjectId objUser))
            {
                return await BuildError(result, _currentUserService.UserId, ERR_MSG_ID_ISVALID_FORMART);
            }

            var order = await _orderRepository.FindByAsync(o => o.UserId == objUser && o.Status == StatusOrderConstain.CANCEL);
            order = order.OrderBy(o => o.CreatedByTime);

            var dtoOrder = _mapper.Map<IEnumerable<Order>, IEnumerable<DtoOrder>>(order);

            return await BuildResult(result, dtoOrder, MSG_SAVE_SUCCESSFULLY);
        }

        #region private method

        #endregion private method

        #region private class method

        #endregion private class method
    }
}
