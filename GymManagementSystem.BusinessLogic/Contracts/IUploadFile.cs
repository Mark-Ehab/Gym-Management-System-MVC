using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BusinessLogic.Contracts;

public interface IUploadedFile
{
    string FileName { get; }
    long Length { get; }
    string? ContentType { get; }

    Stream OpenReadStream();
}