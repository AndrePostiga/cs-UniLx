namespace UniLx.Shared.Abstractions
{
    public sealed class PaginatedQueryResponse<T> where T : class
    {
        public IEnumerable<T>? Data { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }

        private PaginatedQueryResponse(IEnumerable<T>? content, int page, int perPage, int total)
        {
            Data = content;
            Page = page;
            PerPage = perPage;
            Total = total;
        }

        public static PaginatedQueryResponse<T> WithContent(IEnumerable<T>? content, int page, int perPage, int total)
            => new(content, page, perPage, total);
    }
}
