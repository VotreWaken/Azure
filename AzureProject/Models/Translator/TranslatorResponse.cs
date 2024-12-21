namespace AzureProject.Models.Translator
{
    public record TranslatorResponse
    {
        public List<Translation> translations { get; set; } = [];
    }

    public record Translation
    {
        public String text { get; set; } = null!;
        public String to { get; set; } = null!;

    }
}
