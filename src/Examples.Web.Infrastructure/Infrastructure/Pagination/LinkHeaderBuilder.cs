using System;
using System.Collections.Generic;
using System.Linq;

namespace Examples.Web.Infrastructure.Pagination;

/// <summary>
/// Builder for The HTTP Link entity-header field (RFC 8288).
/// </summary>
/// <seealso href="https://www.rfc-editor.org/rfc/rfc8288" />
/// <seealso href="https://docs.github.com/ja/rest/guides/traversing-with-pagination" />
public sealed class LinkHeaderBuilder
{
    public static LinkHeaderBuilder Create(string url)
    {
        return new()
        {
            Url = url,
        };
    }

    public string Url { get; init; } = default!;

    private Func<IEnumerable<Func<(Relationship rel, string? query)>>> _queries = () =>
            Enumerable.Empty<Func<(Relationship rel, string? query)>>();
    private Func<string, string, string> _formatQuery = (_, _) => "";

    public string Build()
    {
        var url = new UriBuilder(Url);
        var links = _queries!.Invoke()
                    .Select(x => x.Invoke())
                    .Where(x => x.query is not null)
                    .Select(x => FormatLink(url, x));

        return string.Join(",", links);

        static string FormatLink(UriBuilder url, (Relationship rel, string? query) x)
        {
            url.Query = x.query;
            return $"<{url}>; rel=\"{x.rel.ToString().ToLower()}\"";
        }
    }

    public LinkHeaderBuilder WithOffset(long offset, int limit, long total)
    {
        if (offset < 0L) { throw new ArgumentOutOfRangeException(nameof(offset)); }
        if (limit < 1) { throw new ArgumentOutOfRangeException(nameof(limit)); }
        if (total < 0) { throw new ArgumentOutOfRangeException(nameof(total)); }

        UseQueryFormat((limit, offset) => $"limit={limit}&offset={offset}");

        _queries = () =>
        {
            var pages = PaginationUtility.ToPage(total, limit);

            var first = 0L;
            var last = (long)(pages - 1) * limit;
            var previous = ((offset - limit) > first) ? (offset - limit) : (long?)null;
            var next = ((offset + limit) < last) ? (offset + limit) : (long?)null;

            return new List<Func<(Relationship rel, string? query)>> {
                () => (Relationship.First,
                        _formatQuery.Invoke($"{limit}", $"{first}")),
                () => (Relationship.Last,
                        _formatQuery.Invoke($"{limit}", $"{last}")),
                () => (Relationship.Previous,
                        previous is null ? null : _formatQuery.Invoke($"{limit}", $"{previous}")),
                () => (Relationship.Next,
                        next is null ? null : _formatQuery.Invoke($"{limit}", $"{next}")),
            };
        };

        return this;
    }

    public LinkHeaderBuilder WithPages(int page, int size, int pages)
    {
        if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }
        if (size < 1) { throw new ArgumentOutOfRangeException(nameof(size)); }
        if (pages < 0) { throw new ArgumentOutOfRangeException(nameof(pages)); }

        UseQueryFormat((size, page) => $"size={size}&page={page}");

        _queries = () =>
        {
            var first = 1;
            var last = pages;
            var previous = (page > first) ? (page - 1) : (int?)null;
            var next = (page < last) ? (page + 1) : (int?)null;

            return new List<Func<(Relationship rel, string? query)>> {
                () => (Relationship.First,
                        _formatQuery.Invoke($"{size}", $"{first}")),
                () => (Relationship.Last,
                        _formatQuery.Invoke($"{size}", $"{last}")),
                () => (Relationship.Previous,
                        previous is null ? null : _formatQuery.Invoke($"{size}", $"{previous}")),
                () => (Relationship.Next,
                        next is null ? null: _formatQuery.Invoke($"{size}", $"{next}")),
            };
        };

        return this;
    }

    public LinkHeaderBuilder UseQueryFormat(Func<string, string, string> action)
    {
        _formatQuery = action;
        return this;
    }

    private enum Relationship
    {
        First,
        Last,
        Next,
        Previous,
    }

}