namespace AzureProject.Models.Search
{
	public class WebSearchPageModel
	{
        public WebSearchResponse? WebSearchResponse { get; set; }
        public int PageNumber { get; set; } = 1;

        public int PerPage { get; set; } = 10;
    }
}
