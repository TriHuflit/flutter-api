namespace Flutter.Backend.Service.Models.Requests
{
    public class PaginationRequest
    {
        /// <summary>
        /// Page Index of search
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page Size of search
        /// </summary>
        public int PageSize { get; set; }
    }
}
