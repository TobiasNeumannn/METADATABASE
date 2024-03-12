namespace METADATABASE.Models
{
    public class Feedback
    {
        public int ID { get; set; }
        public string pfp { get; set; }
        public int userID { get; set; }
        public string desc { get; set; }
        public DateTime date { get; set; }

    }
}
