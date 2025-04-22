namespace BulutBusiness.Core.Translation;

public interface ITranslationService
{
    public Task<string> TranslateAsync(string text, string to, string from = "en");
}
