namespace LocalS3API.Model
{
    public class LoadFile
    {
        public FileStream FileStream { get; set; }
        public string Path { get; set; }
        public string ContentType { get; set; }
    }
}
