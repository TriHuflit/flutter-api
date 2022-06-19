namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoStaff : DtoAuditLogSystem
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }

        public string RoleName { get; set; }

        public int IsActive { get; set; }
    }
}
