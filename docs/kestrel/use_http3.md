# Use HTTP/3 with Kestrel web server.

for .NET 7.0+

## References

- [Use HTTP/3 with the ASP.NET Core Kestrel web server](https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/servers/kestrel/http3?view=aspnetcore-7.0)

## Settings

### Preparation

check os version.

```shell
cat /etc/issue
cat /etc/debian_version
```

`add-apt-repository command not found`

```shell
sudo apt install -y software-properties-common
```

`E: gnupg, gnupg2 and gnupg1 do not seem to be installed, but one of them is required for this operation`

```shell
sudo apt install -y gnupg2
```

### Configure repository

On `mcr.microsoft.com/dotnet/sdk:6.0` (Debian 11.5)

```shell
curl -sSL https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -

sudo apt-add-repository https://packages.microsoft.com/debian/11/prod

sudo apt update
```

```console
Warning: apt-key is deprecated. Manage keyring files in trusted.gpg.d instead (see apt-key(8)).
OK
```

### Install `libmsquic 1.9.*`

```shell
sudo install -y libmsquic=1.9*
```

```console
Listing... Done
vscode@821beb6c44f0:~$ apt list libmsquic
Listing... Done
libmsquic/bullseye 2.1.4 amd64 [upgradable from: 1.9.1]
N: There are 6 additional versions. Please use the '-a' switch to see them.
```

### Configuration ASP.NET Core

In `Program.cs`

```cs
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        listenOptions.UseHttps();
    });
});
```
