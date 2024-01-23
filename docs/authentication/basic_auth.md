# ASP.NET Core Basic Authentication

資格情報を送信するのに、ユーザー ID とパスワードの組を base64 を使用してエンコードします。

> ユーザー ID とパスワードは、ネットワークを介してクリアテキストとして渡されるため（base64 でエンコードされますが、 base64 は可逆エンコードです）、 **Basic 認証方式は安全ではありません**。

## References

- [AspNetCore.Authentication.Basic - GitHub](https://github.com/mihirdilip/aspnetcore-authentication-basic)
- [RFC 7617 (ja)](https://tex2e.github.io/rfc-translater/html/rfc7617.html)
