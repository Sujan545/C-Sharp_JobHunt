public class JobDto
{
    public string Url { get; set; }="";
    public string JobSlug { get; set; }="";
    public string JobTitle { get; set; }="";

    public string CompanyName { get; set; }="";
    public string CompanyLogo { get; set; }="";

    public string[] JobIndustry { get; set; }=[];
    public string[] JobType { get; set; }=[];

    public string JobGeo { get; set; }="";
    public string JobLevel { get; set; }="";

    public string JobExcerpt { get; set; }="";
    public string JobDescription { get; set; }="";

    public DateTime PubDate { get; set; }
}
