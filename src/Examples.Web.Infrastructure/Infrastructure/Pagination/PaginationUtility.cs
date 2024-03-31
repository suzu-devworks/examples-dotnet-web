using System;

namespace Examples.Web.Infrastructure.Pagination;

public static class PaginationUtility
{
    public static long ToOffset(int page, int size)
    {
        if (page < 1) { throw new ArgumentOutOfRangeException(nameof(page)); }
        if (size < 1) { throw new ArgumentOutOfRangeException(nameof(size)); }

        return ((long)page - 1) * size;
    }

    public static int ToPage(long count, int limit)
    {
        if (count < 0L) { throw new ArgumentOutOfRangeException(nameof(count)); }
        if (limit < 1) { throw new ArgumentOutOfRangeException(nameof(limit)); }

        return (int)Math.Ceiling(count / (double)limit);
    }
}
