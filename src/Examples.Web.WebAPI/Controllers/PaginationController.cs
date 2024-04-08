using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Examples.Web.Infrastructure.DataAnnotations;
using Examples.Web.Infrastructure.Pagination;
using Swashbuckle.AspNetCore.Annotations;

namespace Examples.Web.WebAPI.Controllers;

[ApiController]
[SwaggerTag("Examples")]
[Route("[controller]")]
public class PaginationController(ILogger<PaginationController> logger) : ControllerBase
{
    private readonly Repository _repository = new();

    [HttpGet("/limits")]
    public async Task<ActionResult<IEnumerable<PaginationModel>>> GetByOffsetLimitAsync(
        LimitPaginationParameter param, CancellationToken cancellationToken)
    {
        logger.LogDebug("{param}", param);

        var total = await _repository.GetTotalCountAsync(cancellationToken);

        var records = ((total > 0) && (param.Offset < total))
            ? (await _repository.FindByOffsetAsync(param.Offset, param.Limit, cancellationToken)).ToArray()
            : [];
        var count = records.Length;

        Response.Headers.Append("x-total", $"{total}");
        Response.Headers.Append("link",
            LinkHeaderBuilder.Create(Request.GetEncodedUrl())
                .WithOffset(param.Offset, param.Limit, total)
                .Build());

        logger.LogDebug("range: {start}-{end}({count})/{total}",
                param.Offset, param.Offset + count - 1, count, total);

        return Ok(records);
    }

    public class LimitPaginationParameter
    {
        [FromQuery(Name = "offset")]
        [LongRange(0L, long.MaxValue)]
        public long Offset { get; init; } = 0L;

        [FromQuery(Name = "limit")]
        [Range(1, int.MaxValue)]
        public int Limit { get; init; } = 20;
    }


    [HttpGet("/pages")]
    public async Task<ActionResult<IEnumerable<PaginationModel>>> GetByPagesAsync(
        PagePaginationParameter param, CancellationToken cancellationToken)
    {
        logger.LogDebug("{param}", param);

        var offset = PaginationUtility.ToOffset(param.Page, param.Size);
        var limit = param.Size;

        var total = await _repository.GetTotalCountAsync(cancellationToken);
        var pages = PaginationUtility.ToPage(total, param.Size);

        var records = ((total > 0) && (param.Page <= pages))
            ? (await _repository.FindByOffsetAsync(offset, limit, cancellationToken)).ToArray()
            : [];
        var count = records.Length;

        Response.Headers.AppendXPagination(param.Page, param.Size, pages);
        Response.Headers.Append("x-total", $"{total}");
        Response.Headers.Append("link",
            LinkHeaderBuilder.Create(Request.GetEncodedUrl())
                .WithPages(param.Page, param.Size, pages)
                .UseQueryFormat((size, page) => $"page={page}&size={size}&customize")
                .Build());

        logger.LogDebug("pages: {page}-({count})/{total}",
                param.Page, count, pages);

        return Ok(records);
    }

    public class PagePaginationParameter
    {
        [FromQuery(Name = "page")]
        [MinValue<int>(1)]
        public int Page { get; init; } = 1;

        [FromQuery(Name = "size")]
        [MinValue<int>(1)]
        public int Size { get; init; } = 20;
    }


    [HttpGet("/cursors")]
    public async Task<IActionResult> GetByCursorsAsync(
        CursorPaginationParameter param, CancellationToken cancellationToken)
    {
        logger.LogDebug("{param}", param);

        var cursor = CursorConvert.FromBase64String<PaginationModel>(param.Cursor);
        var limit = param.Size;

        var total = await _repository.GetTotalCountAsync(cancellationToken);

        var records = (total > 0)
            ? (await _repository.FindByCursorAsync(cursor, limit, cancellationToken)).ToArray()
            : [];
        var count = records.Length;

        string? nextCursor = null;
        if (count == param.Size + 1)
        {
            nextCursor = CursorConvert.ToBase64String(records.Last());
            records = records[..^1];

            Response.Headers.Append("x-next-cursor", nextCursor);
        }

        Response.Headers.Append("x-total", $"{total}");

        logger.LogDebug("cursor: {start}-{end}({count})/{total}",
                records[0].Id, records[^1].Id, count, total);

        return Ok(new
        {
            Total = total,
            param.Size,
            Items = records,
            NextCursor = nextCursor,
        });
    }

    public class CursorPaginationParameter
    {
        [FromQuery(Name = "cursor")]
        public string? Cursor { get; init; }

        [FromQuery(Name = "size")]
        [Range(1, int.MaxValue)]
        public int Size { get; init; } = 20;
    }

    public record PaginationModel(
        long Id,
        string Name,
        DateTime Timestamp
    );

    private static class CursorConvert
    {
        public static T? FromBase64String<T>(string? query)
            where T : class
        {
            if (query is null) { return null; }

            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(Convert.FromBase64String(query)));
        }

        public static string ToBase64String<T>(T record)
            where T : class
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize<T>(record)));
        }
    }

    private class Repository
    {
        public static readonly long MaxValue = 1000L;

#pragma warning disable CA1822 // Mark members as static

        public Task<long> GetTotalCountAsync(CancellationToken _)
            => Task.FromResult(MaxValue);

        public Task<IEnumerable<PaginationModel>> FindByOffsetAsync(long offset, int limit, CancellationToken _)
        {
            // SELECT ... FROM ... ORDER BY ...
            // OFFSET {offset} ROWS FETCH FIRST {limit} ROWS ONLY;

            var records = Enumerable.Range(1, limit)
                            .Select(i => offset + i)
                            .Where(l => l <= MaxValue)
                            .Select(l => new PaginationModel(l, $"data-{l:D10}", DateTime.UtcNow));
            return Task.FromResult(records);
        }

        public Task<IEnumerable<PaginationModel>> FindByCursorAsync(
            PaginationModel? cursor, int size, CancellationToken _)
        {
            var cursor1 = cursor?.Id ?? 1L;

            // cursor SQL is:
            //  SELECT ... 
            //  FROM ... 
            //  WHERE 
            //     (sort_column1 > cursor1)
            //     OR (sort_column1 = cursor1 AND sort_column2 > cursor2)
            //     OR (sort_column1 = cursor1 AND sort_column2 0 cursor2 AND sort_column3 > cursor3)
            //  ORDER BY
            //     sort_column1 ASC, sort_column2 ASC, sort_column3 ASC
            //  ;

            var records = Enumerable.Range(0, size + 1)
                .Select(i => cursor1 + i)
                .Where(l => l >= cursor1)
                .Where(l => l <= MaxValue)
                .Select(l => new PaginationModel(l, $"data-{l:D10}", DateTime.UtcNow));

            return Task.FromResult(records);
        }

#pragma warning restore CA1822

    }

}
