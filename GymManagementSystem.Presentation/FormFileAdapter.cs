using GymManagementSystem.BusinessLogic.Contracts;

public class FormFileAdapter : IUploadedFile
{
    private readonly IFormFile _file;

    public FormFileAdapter(IFormFile file)
    {
        _file = file;
    }

    public string FileName => _file.FileName;

    public long Length => _file.Length;

    public string? ContentType => _file.ContentType;

    public Stream OpenReadStream()
    {
        return _file.OpenReadStream();
    }
}