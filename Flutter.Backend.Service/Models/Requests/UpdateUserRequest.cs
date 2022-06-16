using Flutter.Backend.DAL.Domains;
using System;

namespace Flutter.Backend.Service.Models.Requests
{
    public class UpdateUserRequest
    {
        /// <summary>
        /// FullName.
        /// </summary>
        public string FullName { get; set; }

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

    }
}
