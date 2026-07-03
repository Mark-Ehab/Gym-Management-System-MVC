using GymManagementSystem.BusinessLogic.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts.AttachmentService;

public interface IAttachmentService
{
    Task<Result<string>> SaveFileAsync(IUploadedFile uploadedFile, string fileDirectory, bool isImage = true, CancellationToken ct = default);
    Result<(Stream, string)> GetFile(string fileStorageName, string fileDirectory);
    void DeleteFile(string fileStorageName, string fileDirectory);
}
