using Microsoft.AspNetCore.Http;

namespace Examples.Web.Infrastructure.Pagination;

public static class HeaderDictionaryExtensions
{
    public static IHeaderDictionary AppendXPagination(this IHeaderDictionary headers,
        int page, int perPage, int totalPages)
    {
        headers.Append("x-page", $"{page}");
        headers.Append("x-per-page", $"{perPage}");
        headers.Append("x-total-pages", $"{totalPages}");

        if (page > 1)
        {
            headers.Append("x-prev-page", $"{page - 1}");
        }

        if (page >= totalPages)
        {
            headers.Append("x-next-page", $"{page + 1}");
        }

        return headers;
    }

}