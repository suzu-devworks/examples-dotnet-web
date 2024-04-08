# CSRF (Cross-Site Request Forgery, Anti Forgery)

CSRF (Cross-Site Request Forgery) is an attack that impersonates a trusted user and sends a website unwanted commands.

## Table of contents <!-- omit in toc -->

- [CSRF (Cross-Site Request Forgery, Anti Forgery)](#csrf-cross-site-request-forgery-anti-forgery)
  - [References](#references)
  - [Cross-Site Request Forgery Prevention](#cross-site-request-forgery-prevention)
    - [Token-Based Mitigation:](#token-based-mitigation)
    - [Disallowing non-simple requests](#disallowing-non-simple-requests)

## References

- [Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0)
- [Cross-Site Request Forgery Prevention - OWASP Cheat Sheet Series](https://cheatsheetseries.owasp.org/cheatsheets/Cross-Site_Request_Forgery_Prevention_Cheat_Sheet.html#synchronizer-token-pattern)


## Cross-Site Request Forgery Prevention

### Token-Based Mitigation:

Synchronizer Token Pattern:

> "X-CSRF-Token": csrf_token

### Disallowing non-simple requests

> "X-Requested-With": "XMLHttpRequest"
