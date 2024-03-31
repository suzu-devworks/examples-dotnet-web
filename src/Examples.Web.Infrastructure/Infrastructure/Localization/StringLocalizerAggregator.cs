using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Examples.Web.Infrastructure.Localization;

public class StringLocalizerAggregator : IStringLocalizer
{
    private readonly IEnumerable<IStringLocalizer> _listOfLocalizer;

    private StringLocalizerAggregator(IEnumerable<IStringLocalizer> listOfLocalizer)
    {
        _listOfLocalizer = listOfLocalizer;
    }

    public LocalizedString this[string name]
        => this[name, Array.Empty<object>()];

    public LocalizedString this[string name, params object[] arguments]
        => _listOfLocalizer.Select(localizer => localizer[name, arguments])
            .FirstOrDefault(s => !s.ResourceNotFound)
            ?? new LocalizedString(name, name, resourceNotFound: true);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => _listOfLocalizer.SelectMany(x => x.GetAllStrings(includeParentCultures));

    public static IStringLocalizer Create(Action<ICollection<IStringLocalizer>> providerAction)
    {
        var list = new LinkedList<IStringLocalizer>();
        providerAction.Invoke(list);
        var instance = new StringLocalizerAggregator(list);

        return instance;
    }
}
