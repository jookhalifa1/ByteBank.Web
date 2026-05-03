using System.Net;
using System.Text.Json;

namespace ByteBank.WASM.Services
{
    public class BaseServices
    {
        protected async Task<ApiResult<T>> HandleError<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            var message = ExtractMessage(content);

            // 🔴 Validation (400)
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var problem = JsonSerializer.Deserialize<ApiValidationError>(
                    content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return new ApiResult<T>
                {
                    IsSuccess = false,
                    Message = message,
                    Errors = problem?.Errors
                };
            }

            // 🔴 باقي الحالات
            return new ApiResult<T>
            {
                IsSuccess = false,
                Message = message
            };
        }
        private string ExtractMessage(string content)
        {
            try
            {
                var json = JsonDocument.Parse(content);

                // ✔ أهم حاجة: detail
                if (json.RootElement.TryGetProperty("detail", out var detail))
                {
                    return detail.GetString();
                }

                // ✔ fallback: title
                if (json.RootElement.TryGetProperty("title", out var title))
                {
                    return title.GetString();
                }
            }
            catch
            {
                // ignore
            }

            return content;
        }
    }
}