using ManeroBackend.Enums;
using Microsoft.AspNetCore.Http;

namespace ManeroBackend.Models
{
    public class ServiceResponse<T>
    {
        public StatusCode StatusCode { get; set; }
        public T? Content { get; set; }
        public string Message { get; set; } = null!;
    }
}
