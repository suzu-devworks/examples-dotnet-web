using System.Net.Http.Headers;
using Examples.Web.Infrastructure;
using Examples.Web.WebApi.Grpc.Inspection;
using Google.Api;
using Google.Protobuf;
using Grpc.Core;

namespace Examples.Web.WebApi.Grpc.Services.Inspection;

public partial class InspectorService
{
    public override async Task<HttpBody> Download(DownloadRequest request, ServerCallContext context)
    {
        using var stream = await GetPdfStreamAsync(request.Id, context.CancellationToken);

        // 'attachment' means the file will be downloaded, not displayed in the browser.
        // 'inline' means the file will be displayed in the browser, not downloaded.
        var contentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = GetContentDispositionFileName(request.Filename),
            FileNameStar = request.Filename?.Sanitize(),
        };

        var httpContext = context.GetHttpContext();
        httpContext.Response.Headers.ContentDisposition = contentDisposition.ToString();
        httpContext.Response.Headers.ContentLength = stream.Length;

        var httpBody = new HttpBody
        {
            ContentType = System.Net.Mime.MediaTypeNames.Application.Pdf,
            Data = ByteString.FromStream(stream)
        };

        return httpBody;
    }

    private static Task<Stream> GetPdfStreamAsync(string id, CancellationToken cancellationToken)
    {
        _ = id; // ignore IDE0060
        _ = cancellationToken; // ignore IDE0060

        var stream = new MemoryStream(Convert.FromBase64String(ForStudyPdfBase64));
        return Task.FromResult<Stream>(stream);
    }

    private static string GetContentDispositionFileName(string? fileName)
    {
        const string defaultDownloadFileName = "download.pdf";

        if (string.IsNullOrWhiteSpace(fileName))
        {
            return defaultDownloadFileName;
        }

        if (fileName.Any(c => c > 0x7F))
        {
            return defaultDownloadFileName;
        }

        return fileName;
    }

    // spell-checker: disable
    public const string ForStudyPdfBase64 =
        "JVBERi0xLjQKJcOkw7zDtsOfCjEgMCBvYmoKPDwvVHlwZS9DYXRhbG9nL1BhZ2VzIDIgMCBSPj4KZW5k" +
        "b2JqCjIgMCBvYmoKPDwvVHlwZS9QYWdlcy9Db3VudCAxL0tpZHNbMyAwIFJdPj4KZW5kb2JqCjMgMCBv" +
        "YmoKPDwvVHlwZS9QYWdlL1BhcmVudCAyIDAgUi9SZXNvdXJjZXM8PC9Gb250PDwvRjEgNCAwIFI+Pj4+" +
        "L01lZGlhQm94WzAgMCA1OTUuMjc4IDg0MS44OV0vQ29udGVudHMgNSAwIFI+PgplbmRvYmoKNCAwIG9i" +
        "ago8PC9UeXBlL0ZvbnQvU3VidHlwZS9UeXBlMS9CYXNlRm9udC9IZWx2ZXRpY2EtQm9sZD4+CmVuZG9i" +
        "ago1IDAgb2JqCjw8L0xlbmd0aCA3OD4+c3RyZWFtCkJUCi9GMSA2MCBUZgoxIDAgMCAxIDgwIDQyMCBU" +
        "bQooRk9SIFNUVURZKSBUagpFVAplbmRzdHJlYW0KZW5kb2JqCnRyYWlsZXIKPDwvU2l6ZSA2L1Jvb3Qg" +
        "MSAwIFI+PgpzdGFydHhyZWYKNDQ4CiUlRU9GCg==";
    // spell-checker: enable
}
