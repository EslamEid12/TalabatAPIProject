namespace TalabatAPIProject.Error
{
    public class ApiResponse
    {
        public int _StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message =null)
        {
            _StatusCode = statusCode;
            Message = message?? GetDefaultMessageForStautusCode(statusCode);
        }

        private string? GetDefaultMessageForStautusCode(int statusCode)
        {
            var message = string.Empty;
            switch (statusCode)
            {
                case 400 :
                    message = "A bad request, you have made";
                        break;
                case 401:
                    message = "Authorized, you are not";
                    break;
                case 404:
                    message= "Resource found, it was not";
                    break;
                case 500:
                    message = "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change";
                    break;
                default:
                    break;
            }
            return message;
            //return statusCode switch
            //{
            //    400 => "A bad request, you have made",
            //    401 => "Authorized, you are not",
            //    404 => "Resource found, it was not",
            //    500 => "Errors are the path to the dark side. Errors lead to anger.  Anger leads to hate.  Hate leads to career change",
            //    _ => null
            //};
        }
    }
}
