namespace LocalS3API.Model
{
    public class ErrorDTO
    {
        public string Message { get; set; } = null!;
        public int StatusCode { get; set; }
    }
}
