namespace Flutter.Backend.Service.Models.Dtos
{
    public class DtoStatistical
    {
        public int TotalOrderWaitingConfirm{ get; set; }

        public int TotalOrderSuccess { get; set; }

        public int TotalUser { get; set; }

        public decimal TotalPriceOfMonth { get; set; }

    }
}
