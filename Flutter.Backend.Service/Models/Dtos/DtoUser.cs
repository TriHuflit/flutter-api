using Flutter.Backend.DAL.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoUser : DtoAuditLogSystem
    {
        /// <summary>
        /// FullName.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// UserName.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// HashPassword.
        /// </summary>
        public string HashPassword { get; set; }

        /// <summary>
        ///Address.
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Birth.
        /// </summary>
        public DateTime Birth { get; set; }

        /// <summary>
        /// Avatar.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Role.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// RefreshToken.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// IsEmailConfirmed.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// IsActive.
        /// </summary>
        public int? IsActive { get; set; }

    }
}
