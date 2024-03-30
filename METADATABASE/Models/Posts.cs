namespace METADATABASE.Models
{
    public class Posts
    {
        public int PostsID { get; set; }
        public int UsersID { get; set; }
        public string Description { get; set; }
        public DateOnly Creation {  get; set; }
        public string Title { get; set; }
        public string Pfp { get; set; }
        public bool Locked { get; set; }

    }
}
