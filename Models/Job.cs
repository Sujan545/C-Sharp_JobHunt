

namespace MyRazorApp.Models   // 👈 ADD THIS
{
    public class Job
    {
        public int Id { get; set; }

        public string Url { get; set; } = "";
        public string JobSlug { get; set; } = "";
        public string JobTitle { get; set; } = "";

        public string CompanyName { get; set; } = "";
        public string CompanyLogo { get; set; } = "";

        public string JobIndustry { get; set; } = "";   // JSON
        public string JobType { get; set; } = "";       // JSON

        public string JobGeo { get; set; } = "";
        public string JobLevel { get; set; } = "";

        public string JobExcerpt { get; set; } = "";
        public string JobDescription { get; set; } = "";

        public DateTime PubDate { get; set; }

        public int AuthorId { get; set; }
        public AppUser? Author { get; set; }
    }
}
