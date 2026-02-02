namespace Library.Core.Domain
{
    public class MediaFile
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public bool IsPlaying { get; set; }
    }
}
