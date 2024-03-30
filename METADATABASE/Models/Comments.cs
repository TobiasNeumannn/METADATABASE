namespace METADATABASE.Models
{
    public class Comments
    {
        public int CommentsID { get; set; }
        public int PostsID { get; set; }
        public string Content { get; set; }
        public DateOnly Creation {  get; set; }
        public int UserID { get; set; }
        public string Pfp {  get; set; }
        public bool Correct { get; set; }
    }
}
