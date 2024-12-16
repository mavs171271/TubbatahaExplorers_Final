namespace VisualStudio.Models.ViewModels
{
    public class UploadedFileViewModel: UploadedFile
    {
        public byte[] FileData { get; set; }
        public List<UploadedFile> SystemFiles { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10; // Default page size
    }
}
