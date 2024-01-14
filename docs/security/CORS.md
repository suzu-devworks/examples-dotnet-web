# CORS (Cross-Origin Resource Sharing)

オリジン間リソース共有 (Cross-Origin Resource Sharing, CORS) は、追加の HTTP ヘッダーを使用して、あるオリジンで動作しているウェブアプリケーションに、異なるオリジンにある選択されたリソースへのアクセス権を与えるようブラウザーに指示するための仕組みです。

## References

- [オリジン間リソース共有 (CORS) - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/CORS)
- [ASP.NET Core でクロスオリジン要求 (CORS) を有効にする - Microsoft Docs](https://docs.microsoft.com/ja-jp/aspnet/core/security/cors?view=aspnetcore-6.0)

<!-- --------------------------- -->

## Overview

### Preflight request

CORS のプリフライトリクエストは CORS のリクエストの一つであり、サーバーが CORS プロトコルを理解していて準備がされていることを、特定のメソッドとヘッダーを使用してチェックします。

プリフライトリクエストはブラウザーが自動的に発行するものであり、通常は、フロントエンドの開発者が自分でそのようなリクエストを作成する必要はありません。

<!-- --------------------------- -->

## ASP.NET Core CORS

```cs
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: "MyPolicyName", policy  =>
  {
    policy.WithOrigins("http://localhost:8889")
      .AllowAnyHeader()
      .AllowAnyMethod();
  });
});

...

app.UseCors();

```

```cs
[EnableCors("MyPolicyName")]
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    // OPTIONS: api/Values
    [HttpOptions]
    public IActionResult PreflightRoute()
    {
        return NoContent();
    }

    // GET: api/Values
    [HttpGet]
    public IActionResult GetTodoItems() =>
        ControllerContext.MyDisplayRouteInfo();
}
```

<!-- --------------------------- -->

## CorsOptions.AddPolicy () code corresponding to the HTTP header used in CORS

### Access-Control-Allow-Origin:

CORS レスポンスヘッダーで、指定されたオリジンからのリクエストを行うコードでレスポンスが共有できるかどうかを示します。

```cs
policy.WithOrigins("http://localhost:8889");
// -> access-control-allow-origin: http://localhost:8889
```

### Access-Control-Allow-Credentials:

CORS レスポンスヘッダーで、リクエストの資格情報モード (Request.credentials) が include である場合に、レスポンスをフロントエンドの JavaScript コードに公開するかどうかをブラウザーに指示します。レスポンス自体はブラウザに送信されているように見えます。

```cs
policy.WithOrigins("http://localhost:8889")
  .AllowCredentials();
// -> access-control-allow-credentials: true
```

### Access-Control-Expose-Headers：

CORS レスポンスヘッダーで、レスポンスの一部としてどのヘッダーを公開するかを、その名前を列挙して示します。

```cs
policy.WithOrigins("http://localhost:8889")
  .WithExposedHeaders("x-custom-header");
// -> access-control-expose-headers: x-custom-header
```

### Access-Control-Allow-Headers:

Access-Control-Request-Headers を含むプリフライトリクエストへのレスポンスで、実際のリクエストの間に使用できる HTTP ヘッダーを示すために使用されます。

```cs
policy.WithOrigins("http://localhost:8889")
  //.AllowAnyHeader()
  .WithHeaders(HeaderNames.ContentType, "x-custom-header");
// -> access-control-allow-headers: Content-Type, x-custom-header
```

### Access-Control-Allow-Methods：

プリフライトリクエストのレスポンスの中で、リソースにアクセスするときに利用できる 1 つまたは複数のメソッドを指定します。
Access-Control-Request-Method と異なるメソッドを指定しても chrome で効いていない気がします。

```cs
policy.WithOrigins("http://localhost:8889")
  //.AllowAnyMethod()
  .WithMethod("GET");
// -> access-control-allow-methods: GET
```

### Access-Control-Max-Age:

プリフライトリクエストの結果 (つまり Access-Control-Allow-Methods および Access-Control-Allow-Headers ヘッダーに含まれる情報) をキャッシュすることができる時間の長さを示します。

規定値は５秒のようです。日本語訳がちょっとアレです。（The default value is 5 seconds.）
この値が有効な間は新たなプリフライトリクエストは送信されないようです。

```cs
policy.WithOrigins("http://localhost:8889")
  .SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
// -> access-control-max-age: 2520
```

### Access-Control-Request-Headers：

プリフライトリクエストを発行する際にブラウザーが使用し、実際のリクエストが行う際にどの HTTP ヘッダーを使用するかをサーバーに知らせます。

### Access-Control-Request-Method:

プリフライトリクエストを発行する際にブラウザーが使用し、実際のリクエストが行われた際にどの HTTP メソッドが使用されるかをサーバーに知らせるために使用されます。

<!-- --------------------------- -->

## CORS Tests

### CORS Test tool web site.

- [CORS Tester](https://cors-test.codehappy.dev/)

### Rester (chrome extensions), Postman

Add Headers 

- Method: OPTIONS
- Origin: localhost:3000
- Access-Control-Request-Method: GET
- Access-Control-Request-Method: x-requested-with

### curl command

``` shell
curl -H "Origin: localhost:3001" \
     -H "Access-Control-Request-Method: GET" \
     -X OPTIONS --verbose -insecure \
        https://127.0.0.1:5001/corsresource
```

OK is:

```console
< HTTP/2 204 
< date: Wed, 30 Nov 2022 13:51:39 GMT
< access-control-allow-origin: http://localhost:3000
<
```

NG is:

```console
< HTTP/2 204 
< date: Wed, 30 Nov 2022 13:56:44 GMT
< 
```
