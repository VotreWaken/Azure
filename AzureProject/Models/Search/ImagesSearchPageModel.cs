using AzureProject.Models.ImagesSearch;

namespace AzureProject.Models.Search
{
	public class ImagesSearchPageModel
	{

        public int PageNumber { get; set; } = 1;
        public int PerPage { get; set; } = 10;
        public ImagesSearchResponse? SearchResponse { get; set; }
    }
}
