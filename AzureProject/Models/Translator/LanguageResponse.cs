namespace AzureProject.Models.Translator
{
	public class LanguageResponse
	{
        public Dictionary<String, LanguageData> translation { get; set; } = null!;
    }

	public class LanguageData
	{
        public String name { get; set; } = null!;
		public String nativeName { get; set; } = null!;
		public String dir { get; set; } = null!;
		public String? code { get; set; }
	}
}
