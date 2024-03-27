namespace METADATABASE.Models
{
    public class Bug
    {

        public int BugID { get; set; }
        public string pfp { get; set; }
        public int userID { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }
        public bool correct { get; set; }
    }
}
