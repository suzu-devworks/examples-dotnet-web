using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Examples.Web.Infrastructure.Localization;

public class StringLocalizerAggregator : IStringLocalizer
{
    private readonly IEnumerable<IStringLocalizer> _localizers;

    private StringLocalizerAggregator(IEnumerable<IStringLocalizer> localizers)
    {
        _localizers = localizers;
    }

    public LocalizedString this[string name]
        => this[name, Array.Empty<object>()];

    public LocalizedString this[string name, params object[] arguments]
        => _localizers.Select(localizer => localizer[name, arguments])
            .FirstOrDefault(s => !s.ResourceNotFound)
            ?? new LocalizedString(name, name, resourceNotFound: true);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => _localizers.SelectMany(x => x.GetAllStrings(includeParentCultures));

    public static IStringLocalizer Create(Action<ICollection<IStringLocalizer>> providerAction)
    {
        var localizers = new LinkedList<IStringLocalizer>();
        providerAction.Invoke(localizers);
        var instance = new StringLocalizerAggregator(localizers);

        return instance;
    }
}
