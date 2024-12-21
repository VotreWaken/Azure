using Microsoft.AspNetCore.Mvc;

namespace AzureProject.Models.Translator
{
    public class TranslatorFormModel
    {
        [FromQuery(Name = "text")]
        public String Text { get; set; } = null!;

		[FromQuery(Name = "lang-from")]
		public String LangFrom { get; set; } = null!;

		[FromQuery(Name = "lang-to")]
		public String LangTo { get; set; } = null!;
	}
}
