# HTTP headers for Security

## References

* [ASP.NET Core でセキュアな レスポンスヘッダー を設定する - BEACHSIDE BLOG](https://blog.beachside.dev/entry/2020/06/10/183000)
* [ASP.Net Core: X-Frame-Options strange behavior - stackoverflow](https://stackoverflow.com/questions/40523565/asp-net-core-x-frame-options-strange-behavior)
* [The ASP.NET Core security headers guide](https://blog.elmah.io/the-asp-net-core-security-headers-guide/)
* [カスタムヘッダーを使ったJavaScriptによるCSRF対策](https://it-web-life.com/javascript_csrf_cors_preflight_xform/)


<!-- --------------------------- -->

## 気になるHTTPヘッダ

### X-Requested-With:

カスタムヘッダを利用することで、異なるオリジンからのリクエスト時に Preflight リクエストするようにします。
サーバー側でブラウザから送られてくる Preflight リクエストを解析してレスポンスを返すようにすることで、CSRF 対策用のトークンが不要になります。


### Content-Security-Policy:

* [Content-Security-Policy - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/Content-Security-Policy)

コンテンツセキュリティポリシー (CSP) ということで、クロスサイトスクリプティング対応などセキュリティ関連で色々設定できます。

```default-src 'self';```とかいるかなー

### X-Frame-Options:

* [X-Frame-Options - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/X-Frame-Options)

```<frame>、<iframe>、<embed>、<object>``` は、既に悪用目的のように思えるので、通常 ```DENY``` で必要になってから考えるで良いと思います。

```
Content-Security-Policy: frame-ancestors <source>;
```

に順次置き換わるようです。


### X-Xss-Protection:

* [X-Xss-Protection - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/X-XSS-Protection)

反射型クロスサイトスクリプティング (XSS) 攻撃を検出したときに、ページの読み込みを停止するためのものです

Content-Security-Policy で ```unsafe-inline``` を**指定しない**
に順次置き換わるようです。


### X-Content-Type-Options:

* [X-Content-Type-Options - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/X-Content-Type-Options)

実行可能なコンテンツを表す MIME タイプの一部には、セキュリティ上の懸念があります。サーバーは X-Content-Type-Options を送信することで、MIME スニッフィングを抑制できます。

### X-Powered-By:

ホスティング環境やその他のフレームワークによって設定される。
**不必要な情報を提供しているため消すべきです。**


### Server:

リクエストを扱うサーバーが使用するソフトウェアの情報を持ちます。
**不必要な情報を提供しているため消すべきです。**


<!-- --------------------------- -->

## 追加HTTPヘッダを有効にする方法

### 　IIS(IIS　Express）で有効にする方法

web.config で設定します。

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <security>
      <requestFiltering removeServerHeader="true" />
    </security>
    <httpProtocol>
      <customHeaders>
        <remove name="X-Powered-By" />
        <add name="Content-Security-Policy" value="frame-ancestors 'none'" />
        <add name="X-Frame-Options" value="DENY" />
        <add name="X-Xss-Protection" value="1; mode=block" />
        <add name="X-Content-Type-Options" value="nosniff" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
</configuration>
```

### Kestrel で有効にする方法

Kestrel は web.config を読まないのでコードで指定する必要があります。

```cs
webBuilder.ConfigureKestrel(serverOptions => serverOptions.AddServerHeader = false);

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'none'");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

    await next();
});

```
