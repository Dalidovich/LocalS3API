using LocalS3API.Model;

namespace LocalS3API.Service
{
    public class S3FileService
    {
        private readonly IWebHostEnvironment _appEnvironment;

        public S3FileService(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        private string GetBucketPath(string bucketName) => Path.Combine(_appEnvironment.WebRootPath, bucketName);

        public async Task<List<string>> UploadFiles(List<IFormFile> files, string bucketName)
        {
            var idList = new List<string>();

            if (Directory.Exists(GetBucketPath(bucketName)))
            {
                foreach (var file in files)
                {
                    idList.Add(await SaveFile(file, bucketName));
                }
            }
            else
            {
                CreateBucket(bucketName);
                foreach (var file in files)
                {
                    idList.Add(await SaveFile(file, bucketName));
                }
            }

            return idList;
        }

        private async Task<string> SaveFile(IFormFile file, string bucketName)
        {
            var fileExtension= file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);
            var saveName = $"{file.Name}{Guid.NewGuid()}.{fileExtension}";
            using (var fileStream = new FileStream(GetBucketPath(bucketName) + $"\\{saveName}", FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return saveName;
        }

        private void CreateBucket(string bucketName) => Directory.CreateDirectory(GetBucketPath(bucketName));

        public LoadFile GetFile(string id, string bucketName)
        {
            var filePath = GetBucketPath(bucketName) + @"\" + id;
            if (File.Exists(filePath))
            {
                return new LoadFile()
                {
                    ContentType = GetContentType(filePath),
                    Path = filePath,
                    FileStream = File.OpenRead(filePath)
                };
            }

            return new LoadFile()
            {
                Path = ""
            };
        }

        private string GetContentType(string filePath)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
