namespace AzureProject.Models.Translator
{
    public class TranslatorPageModel
    {
        public List<TranslatorResponse>? translatorResponse { get; set; }

        public TranslatorFormModel? FormModel { get; set; }

        public LanguageResponse LanguageResponse { get; set; } = null!;
    }
}
