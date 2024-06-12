using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LifeStyle.Application.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
    }

    public class FileService : IFileService
    {
        private readonly string _uploadPath;
        private readonly ILogger<FileService> _logger;

        public FileService(IConfiguration configuration, ILogger<FileService> logger)
        {
            _uploadPath = configuration.GetValue<string>("UploadPath") ?? throw new ArgumentNullException("UploadPath configuration is missing");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                throw new ArgumentNullException(nameof(folder));
            }

            _logger.LogInformation("Saving file to {Folder}", folder);

          
            var uploadPath = Path.Combine(_uploadPath, folder);
            if (!Directory.Exists(uploadPath))
            {
                _logger.LogInformation("Creating directory at {UploadPath}", uploadPath);
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation("File saved to {FilePath}", filePath);

           
            var relativeFilePath = Path.Combine(folder, fileName).Replace("\\", "/");
            return $"/{relativeFilePath}";
        }
    }
}

