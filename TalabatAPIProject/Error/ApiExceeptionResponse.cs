namespace TalabatAPIProject.Error
{
    public class ApiExceeptionResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiExceeptionResponse(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
        {
            Details = details;  
        }
    }
}
