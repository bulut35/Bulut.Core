using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;

namespace BulutBusinessCore.Core.Localization;
public static class ApplicationBuilderLocalizationMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseLocalization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LocalizationMiddleware>();
    }
}
public interface ILocalizationService
{
    public ICollection<string>? AcceptLocales { get; set; }

    /// <summary>
    /// Gets the localized string for the given key by <see cref="AcceptLocales"/>.
    /// </summary>
    public Task<string> GetLocalizedAsync(string key, string? keySection = null);

    public Task<string> GetLocalizedAsync(string key, ICollection<string> acceptLocales, string? keySection = null);
}
public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context, ILocalizationService localizationService)
    {
        IList<StringWithQualityHeaderValue> acceptLanguages = context.Request.GetTypedHeaders().AcceptLanguage;
        if (acceptLanguages.Count > 0)
            localizationService.AcceptLocales = acceptLanguages
                .OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .ToImmutableArray();

        await _next(context);
    }
}
public class ResourceLocalizationManager : ILocalizationService
{
    private const string _defaultLocale = "en";
    private const string _defaultKeySection = "index";
    public ICollection<string>? AcceptLocales { get; set; }

    // <locale, <section, <path, content>>>
    private readonly Dictionary<string, Dictionary<string, (string path, YamlMappingNode? content)>> _resourceData = [];

    public ResourceLocalizationManager(Dictionary<string, Dictionary<string, string>> resources)
    {
        foreach ((string locale, Dictionary<string, string> sectionResources) in resources)
        {
            if (!_resourceData.ContainsKey(locale))
                _resourceData.Add(locale, new Dictionary<string, (string path, YamlMappingNode? value)>());

            foreach ((string sectionName, string sectionResourcePath) in sectionResources)
                _resourceData[locale].Add(sectionName, (sectionResourcePath, null));
        }
    }

    public Task<string> GetLocalizedAsync(string key, string? keySection = null)
    {
        return GetLocalizedAsync(key, AcceptLocales ?? throw new NoNullAllowedException(nameof(AcceptLocales)), keySection);
    }

    public Task<string> GetLocalizedAsync(string key, ICollection<string> acceptLocales, string? keySection = null)
    {
        string? localization;
        if (acceptLocales is not null)
            foreach (string locale in acceptLocales)
            {
                localization = getLocalizationFromResource(key, locale, keySection);
                if (localization is not null)
                    return Task.FromResult(localization);
            }

        localization = getLocalizationFromResource(key, _defaultLocale, keySection);
        if (localization is not null)
            return Task.FromResult(localization);

        return Task.FromResult(key);
    }

    private string? getLocalizationFromResource(string key, string locale, string? keySection = _defaultKeySection)
    {
        if (string.IsNullOrWhiteSpace(keySection))
            keySection = _defaultKeySection;

        if (
            _resourceData.TryGetValue(locale, out Dictionary<string, (string path, YamlMappingNode? content)>? cultureNode)
            && cultureNode.TryGetValue(keySection, out (string path, YamlMappingNode? content) sectionNode)
        )
        {
            if (sectionNode.content is null)
                lazyLoadResource(sectionNode.path, out sectionNode.content);

            if (sectionNode.content!.Children.TryGetValue(new YamlScalarNode(key), out YamlNode? cultureValueNode))
                return cultureValueNode.ToString();
        }

        return null;
    }

    private void lazyLoadResource(string path, out YamlMappingNode? content)
    {
        using StreamReader reader = new(path);
        YamlStream yamlStream = [];
        yamlStream.Load(reader);
        content = (YamlMappingNode)yamlStream.Documents[0].RootNode;
    }
}
public static class ServiceCollectionResourceLocalizationManagerExtension
{
    /// <summary>
    /// Adds <see cref="ResourceLocalizationManager"/> as <see cref="ILocalizationService"/> to <see cref="IServiceCollection"/>.
    /// <list type="bullet">
    ///    <item>
    ///        <description>Reads all yaml files in the "<see cref="Assembly.GetExecutingAssembly()"/>/Features/{featureName}/Resources/Locales/". Yaml file names must be like {uniqueKeySectionName}.{culture}.yaml.</description>
    ///    </item>
    ///    <item>
    ///        <description>If you don't want separate locale files with sections, create "<see cref="Assembly.GetExecutingAssembly()"/>/Features/Index/Resources/Locales/index.{culture}.yaml".</description>
    ///    </item>
    /// </list>
    /// </summary>
    public static IServiceCollection AddYamlResourceLocalization(this IServiceCollection services)
    {
        services.AddScoped<ILocalizationService, ResourceLocalizationManager>(_ =>
        {
            // <locale, <featureName, resourceDir>>
            Dictionary<string, Dictionary<string, string>> resources = [];

            string[] featureDirs = Directory.GetDirectories(
                Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Features")
            );
            foreach (string featureDir in featureDirs)
            {
                string featureName = Path.GetFileName(featureDir);
                string localeDir = Path.Combine(featureDir, "Resources", "Locales");
                if (Directory.Exists(localeDir))
                {
                    string[] localeFiles = Directory.GetFiles(localeDir);
                    foreach (string localeFile in localeFiles)
                    {
                        string localeName = Path.GetFileNameWithoutExtension(localeFile);
                        int separatorIndex = localeName.IndexOf('.');
                        string localeCulture = localeName[(separatorIndex + 1)..];

                        if (File.Exists(localeFile))
                        {
                            if (!resources.ContainsKey(localeCulture))
                                resources.Add(localeCulture, []);
                            resources[localeCulture].Add(featureName, localeFile);
                        }
                    }
                }
            }

            return new ResourceLocalizationManager(resources);
        });

        return services;
    }
}
public class TranslateLocalizationManager : ILocalizationService
{
    private const string _defaultLocale = "en";
    public ICollection<string>? AcceptLocales { get; set; }

    private readonly ITranslationService _translationService;

    public TranslateLocalizationManager(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public Task<string> GetLocalizedAsync(string key, string? keySection = null)
    {
        return GetLocalizedAsync(key, AcceptLocales ?? throw new NoNullAllowedException(nameof(AcceptLocales)));
    }

    public async Task<string> GetLocalizedAsync(string key, ICollection<string> acceptLocales, string? keySection = null)
    {
        string? localization;

        if (acceptLocales is not null)
            foreach (string locale in acceptLocales)
            {
                localization = await _translationService.TranslateAsync(key, locale);
                if (!string.IsNullOrWhiteSpace(localization))
                    return localization;
            }

        localization = await _translationService.TranslateAsync(key, _defaultLocale);
        if (!string.IsNullOrWhiteSpace(localization))
            return localization;

        return key;
    }
}