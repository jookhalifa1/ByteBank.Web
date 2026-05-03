using Microsoft.AspNetCore.Mvc;

namespace ByteBank.WASM.Services
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }

       
    }
}
