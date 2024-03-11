using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace METADATABASE.Models
{
    public class Users
    {
        public string ID { get; set; }
        public string username { get; set; }
        public string email {  get; set; }
        public int projID { get; set; }
        public string pfp {  get; set; }
        public string projName { get; set; }
        public string projImg { get; set; }
        public string projDesc { get; set; }

    }
}
