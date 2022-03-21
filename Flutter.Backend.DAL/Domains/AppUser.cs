using MongoDB.Bson;
using System;

namespace Flutter.Backend.DAL.Domains
{
    public class AppUser : AuditLogSystem
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
        public string Address { get; set; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gender.
        /// </summary>
        public bool Gender { get; set; }

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
        public ObjectId Role { get; set; }

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
