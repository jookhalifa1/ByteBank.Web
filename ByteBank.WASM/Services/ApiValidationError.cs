namespace ByteBank.WASM.Services
{
    public class ApiValidationError
    {
        public string Title { get; set; } = string.Empty;

        public Dictionary<string, string[]> Errors { get; set; }
            = new Dictionary<string, string[]>();
    }
}
