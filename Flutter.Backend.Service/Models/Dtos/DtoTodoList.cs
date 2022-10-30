namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoTodoList : DtoAuditLogSystem
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool status { get; set; }
    }
}
