﻿using Flutter.Backend.DAL.Domains;
using Flutter.Backend.Service.Models.Dtos;
using Flutter.Backend.Service.Models.Requests;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.IServices
{
    public interface IAuthenticateService
    {
        Task<AppActionResultMessage<DtoAuthent>> AuthendicateAsync(AuthendicateRequest request);

        Task<AppActionResultMessage<string>> RegisterAsync(RegisterRequest request);

        Task<AppActionResultMessage<DtoRefreshToken>> RefreshTokenAsync(string refreshToken);
    }
}
