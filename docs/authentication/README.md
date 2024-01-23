# ASP.NET core Authentication and Authorization

## Table of contents

- [Basic Authentication](./basic_auth.md)
- [ASP.NET Core Identity ...](./identity/README.md)


## References

- [HTTP authentication - mdn](https://developer.mozilla.org/ja/docs/Web/HTTP/Authentication)
- [Overview of ASP.NET Core authentication - Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication)

## Overview

### Authenticate

Authentication is the process of determining a user's identity. 

In ASP.NET Core, authentication is handled by the authentication service, `IAuthenticationService`, which is used by authentication middleware. 

The authentication service uses registered authentication handlers to complete authentication-related actions:

- Authenticating a user.
- Responding when an unauthenticated user tries to access a restricted resource.

An authentication scheme is a name that corresponds to:

- An authentication handler.
- Options for configuring that specific instance of the handler.

An authentication handler:

- Is a type that implements the behavior of a scheme.
- Is derived from `IAuthenticationHandler` or `AuthenticationHandler<TOptions>`.
- Has the primary responsibility to authenticate users.

### Authorization

Authorization is the process of determining whether a user has access to a resource.
