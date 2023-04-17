namespace TextEditor.Models
{
    public class ContentModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
		public string FileOwner { get; set; }

		public string Content { get; set; }
    }
}
