namespace METADATABASE.Models
{
    public class Feedback
    {
        public int ID { get; set; }
        public string pfp { get; set; }
        public int userID { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }

    }
}
