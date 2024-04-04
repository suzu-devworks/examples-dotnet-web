# Secure HTTP Response Header

## Table of contents <!-- omit in toc -->

- [Secure HTTP Response Header](#secure-http-response-header)
  - [References](#references)
  - [HTTP headers of interest](#http-headers-of-interest)
    - [`X-Requested-With`](#x-requested-with)
    - [`Content-Security-Policy`](#content-security-policy)
    - [`X-Frame-Options`](#x-frame-options)
    - [`X-Xss-Protection`](#x-xss-protection)
    - [`X-Content-Type-Options`](#x-content-type-options)
    - [`X-Powered-By`](#x-powered-by)
    - [`Server`](#server)
  - [Configuration](#configuration)
    - [How to delete `Server`.](#how-to-delete-server)
    - [How to delete `X-Powered-By`.](#how-to-delete-x-powered-by)
    - [How to add response headers.](#how-to-add-response-headers)

<!-- --------------------------- -->

## References

* [ASP.Net Core: X-Frame-Options strange behavior - stackoverflow](https://stackoverflow.com/questions/40523565/asp-net-core-x-frame-options-strange-behavior)
- [Configure options for the ASP.NET Core Kestrel web server](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/servers/kestrel/options?view=aspnetcore-8.0)

<!-- --------------------------- -->

## HTTP headers of interest

### `X-Requested-With`

If the requests come from different origin, create a preflight request with custom headers.
By parsing Preflight requests sent from the browser on the server side and returning a response, tokens for CSRF countermeasures are no longer required.


### `Content-Security-Policy`

* [Content-Security-Policy - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/Content-Security-Policy)

Content Security Policy (CSP) allows you to configure various security-related settings, including support for cross-site scripting.

`default-src 'self';` ???


### `X-Frame-Options`

* [X-Frame-Options - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/X-Frame-Options)

It appears that `<frame>, <iframe>, <embed>, and <object>` already have a malicious purpose. Therefore, 
I think it's usually best to set it to `DENY` and think about it until you need it.

```
Content-Security-Policy: frame-ancestors <source>;
```

It seems that it will be replaced sequentially.


### `X-Xss-Protection`

* [X-Xss-Protection - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/X-XSS-Protection)

Intended for stopping a page from loading when a reflected cross-site scripting (XSS) attack is detected.

**Do not specify `unsafe-inline` in `Content-Security-Policy`**
It seems that it will be replaced sequentially.


### `X-Content-Type-Options`

* [X-Content-Type-Options - MDN](https://developer.mozilla.org/ja/docs/Web/HTTP/Headers/X-Content-Type-Options)

Some MIME types that represent executable content have security concerns.
A server can suppress her MIME sniffing by sending X-Content-Type-Options.

### `X-Powered-By`

Set by the hosting environment or other frameworks.

> **This should be deleted as it provides unnecessary information.**


### `Server`

Contains information about the software used by the server handling the request.

> **This should be deleted as it provides unnecessary information.**


<!-- --------------------------- -->

## Configuration

### How to delete `Server`.

For IIS (IIS Express):

use `web.config`

```xml:web.config
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <security>
      <requestFiltering removeServerHeader="true" />
    </security>
  </system.webServer>
</configuration>
```

For Kestrel:

> Isn't it automatically configured with ConfigureKestrel?

```cs
builder.WebHost.ConfigureKestrel(options => options.AddServerHeader = false);
```


### How to delete `X-Powered-By`.


For IIS (IIS Express):

use `web.config`

```xml:web.config
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <customHeaders>
      <remove name="X-Powered-By" />
    </customHeaders>
  </system.webServer>
</configuration>
```

For Kestrel:

kestrel is not configured in the first place.


### How to add response headers.

For IIS (IIS Express):

use `web.config`

```xml:web.config
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

For Kestrel:

```cs
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'none'");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

    await next();
});
```
