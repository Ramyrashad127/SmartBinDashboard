namespace SmartBin.Api.DTOs
{
    public class TransactionDto
    {
        public DateTime Timestamp { get; set; }
        public string MaterialName { get; set; }
        public float? AiConfidence { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
