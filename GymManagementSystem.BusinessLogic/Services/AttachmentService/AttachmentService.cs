using GymManagementSystem.BusinessLogic.Common;
using GymManagementSystem.BusinessLogic.Contracts;
using GymManagementSystem.BusinessLogic.Contracts.AttachmentService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Services.AttachmentService;

public class AttachmentService : IAttachmentService
{
    private const long _maxFileSize = 5 * 1024 * 1024;
    private readonly HashSet<string> _allowedImageExtensions = new ([".jpg", ".jpeg", ".png"],StringComparer.OrdinalIgnoreCase);
    private readonly ILogger<AttachmentService> _logger;
    private readonly IHostEnvironment _env;

    public AttachmentService(ILogger<AttachmentService> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task<Result<string>> SaveFileAsync(IUploadedFile uploadedFile, string fileDirectory, bool isImage = true, CancellationToken ct = default)
    {
        // Check file name
        if (string.IsNullOrWhiteSpace(uploadedFile.FileName))
        {
            _logger.LogWarning("Uploaded file name is invalid");
            return Result<string>.Failure(new("Upload.File.FileNameInvalid", Description: "File is invalid !"));
        }

        // Check file directory
        if (string.IsNullOrWhiteSpace(fileDirectory))
        {
            _logger.LogWarning("Directory of uploaded file {@FileName} is invalid", uploadedFile.FileName);
            return Result<string>.Failure(new("Upload.File.NotUploaded", Description: "File is not uploaded successfully !"));
        }

        // Open read stream on uploaded file
        var uploadedFileStream = uploadedFile.OpenReadStream();

        // Check if file stream is null or not
        if (uploadedFileStream is null || uploadedFileStream.Length == 0)
        {
            _logger.LogWarning("Uploaded {@FileName} is eiter null or empty", uploadedFile.FileName);
            return Result<string>.Failure(new("Upload.File.Empty", $"Uploaded {uploadedFile.FileName} is eiter null or empty."));
        }

        // Check if file exceeds maximum file size
        if(uploadedFileStream.Length > _maxFileSize)
        {
            _logger.LogWarning("Uploaded {@FileName} size exceeds 5 MB", uploadedFile.FileName);
            return Result<string>.Failure(new("Upload.File.ExceedMaxSize", $"Uploaded {uploadedFile.FileName} size exceeds 5 MB."));
        }

        // Check if the file has an extension
        var fileExtension = Path.GetExtension(uploadedFile.FileName);
        if(fileExtension is null)
        {
            _logger.LogWarning("Uploaded {@FileName} has no extension", uploadedFile.FileName);
            return Result<string>.Failure(new("Upload.File.NoExtension", $"Uploaded {uploadedFile.FileName} has no extension."));
        }

        // Check if the file has a read permission
        if (!uploadedFileStream.CanRead)
        {
            _logger.LogWarning("Uploaded {@FileName} has no permission to read from", uploadedFile.FileName);
            return Result<string>.Failure(new("Upload.File.NoReadPermission", $"Uploaded {uploadedFile.FileName} has no permission to read from."));
        }

        // Check if the uploaded file is an image
        if (isImage)
        {
            var result = ValidateImage(uploadedFileStream, uploadedFile.FileName);

            if(result.IsFailure)
                return Result<string>.Failure(result.Error!);

            // Re-open the read stream on the file
            uploadedFileStream = uploadedFile.OpenReadStream();
        }

        // Create Directory if not exists
        var directoryPath = Path.Combine(_env.ContentRootPath,"Files", fileDirectory);
        Directory.CreateDirectory(directoryPath);

        // Create file inside the directory
        var fileStorageName = $"{Guid.NewGuid():N}{uploadedFile.FileName}";
        var fileFullPath = Path.Combine(directoryPath, fileStorageName);
        using var fileStream = new FileStream(fileFullPath, FileMode.Create, FileAccess.Write);
        await uploadedFileStream.CopyToAsync(fileStream,ct);

        _logger.LogInformation("{@FileName} is uploaded successfully", uploadedFile.FileName);
        return Result<string>.Success(fileStorageName);
    }

    public Result<(Stream, string)> GetFile(string fileStorageName, string fileDirectory)
    {
        // Check file name
        if (string.IsNullOrWhiteSpace(fileStorageName))
        {
            _logger.LogWarning("Name of requested file is invalid");
            return Result<(Stream, string)>.Failure(new("Request.File.Invalid",Description: "Requested file is not found"));
        }

        // Check file directory
        if (string.IsNullOrWhiteSpace(fileDirectory))
        {
            _logger.LogWarning("Name of directory of requested file is invalid");
            return Result<(Stream, string)>.Failure(new("Request.File.Invalid", Description: "Requested file is not found"));
        }

        // Check if the requested file exists
        var fileFullPath = Path.Combine(_env.ContentRootPath, "Files", fileDirectory, fileStorageName);
        if (!File.Exists(fileFullPath))
        {
            _logger.LogWarning("Requested {@FileName} doesn't exist", fileStorageName);
            return Result<(Stream, string)>.Failure(new("Request.File.Invalid", Description: "Requested file is not found")); ;
        }

        var fileStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read);
        var extenion = Path.GetExtension(fileStorageName.ToLowerInvariant()) switch { 
            ".png" => "pmg",
            ".jpg" or ".jpeg" => "jpg",
            _ => "octet-stream"
        };
        var contentType = $"image/{extenion}";

        return Result<(Stream, string)>.Success((fileStream, contentType));
    }

    public void DeleteFile(string fileStorageName, string fileDirectory)
    {
        // Check file name
        if (string.IsNullOrWhiteSpace(fileStorageName))
        {
            _logger.LogWarning("Name of file to be deleted is invalid");
            return;
        }

        // Check file directory
        if (string.IsNullOrWhiteSpace(fileDirectory))
        {
            _logger.LogWarning("Name of directory of the file to be deleted is invalid");
            return;
        }

        // Check if the file to be deleted exists
        var fileFullPath = Path.Combine(_env.ContentRootPath, "Files", fileDirectory, fileStorageName);
        if(!File.Exists(fileFullPath))
        {
            _logger.LogWarning("{@FileName} file to be deleted doesn't exist", fileStorageName);
            return;
        }

        File.Delete(fileFullPath);
    }

    private Result ValidateImage(Stream fileStream, string fileName)
    {
        // Check if image extension is among allowed extension
        var imageExtension = Path.GetExtension(fileName);
        if(!_allowedImageExtensions.Contains(imageExtension,StringComparer.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Uploaded {@fileName} extension is not among allowed image extensions (.jpg,.jpeg.,.png)", fileName);
            return Result.Failure(new("Upload.File.Image.NotAllowedExtension",$"Uploaded {fileName} extension is not among allowed image extensions (.jpg,.jpeg.,.png)"));
        }

        // Check if the uploaded file is a real image
        var bitMap = SKBitmap.Decode(fileStream);
        if(bitMap is null)
        {
            _logger.LogWarning("Uploaded {@fileName} is invalid image", fileName);
            return Result.Failure(new("Upload.File.Image.Invalid", $"Uploaded {fileName} is invalid image."));
        }

        // Check image dimensions
        if (bitMap.Width == 0 || bitMap.Height == 0)
        {
            _logger.LogWarning("Uploaded {@fileName} has invalid image dimensions", fileName);
            return Result.Failure(new("Upload.File.Image.InvalidDimensions", $"Uploaded {fileName} has invalid image dimensions."));
        }

        return Result.Success();
    }
}
