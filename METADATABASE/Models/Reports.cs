namespace METADATABASE.Models
{
    public class Reports
    {
        public int ReportsID { get; set; }
        public int UsersID { get; set; }
        public int PostsID { get; set; }
        public int CommentsID { get; set; }
        public string Content { get; set; }
        public DateOnly Creation {  get; set; }
    }
}
