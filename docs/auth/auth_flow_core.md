# Authentication and authorization in ASP.NET Core

## Table of Contents <!-- omit in toc -->

- [Authentication](#authentication)
  - [Authentication flow](#authentication-flow)
- [Authorization](#authorization)
  - [Authorization flow](#authorization-flow)

## Authentication

- [Overview of ASP.NET Core Authentication | Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authentication/)

### Authentication flow

```mermaid
sequenceDiagram
    participant C as Client
    box Middleware Pipeline
        participant P as Middleware Pipeline
        participant ANM as AuthenticationMiddleware
    end
    participant ANS as (IAuthenticationService)<br>AuthenticationService 
    participant IAuthenticationSchemeProvider
    participant IAuthenticationHandlerProvider
    participant ANH as (IAuthenticationHandler)<br>AuthenticationHandler<TOptions>

    C->>P: HTTP Request (Authorization)
    P->>ANM: Invoke(context)

    Note over P, ANH: Authentication phase

    ANM-->>ANS: AuthenticateAsync(context, scheme)
    ANS->>IAuthenticationSchemeProvider: GetDefaultAuthenticateScheme()
    IAuthenticationSchemeProvider -->>ANS: AuthenticationScheme 
    ANS->>IAuthenticationHandlerProvider: GetHandlerAsync(context, scheme)
    IAuthenticationHandlerProvider-->>ANS: AuthenticationHandler 
    ANS->>ANH: AuthenticateAsync()

    ANH->>ANH: Handle Authentication (validate credentials)
    
    ANH-->>ANS: AuthenticateResult (succeeded / failed)

    alt Authentication succeeded
        ANS-->>ANM: AuthenticateResult.Success (ClaimsPrincipal)
        ANM->>P: _next(context)
    else Authentication failed
        ANS-->>ANM: AuthenticateResult.Fail
        ANM-->>C: 401 Unauthorized (WWW-Authenticate: ...)
    end
```

## Authorization

- [Introduction to authorization in ASP.NET Core | Microsoft Learn](https://learn.microsoft.com/ja-jp/aspnet/core/security/authorization/introduction)

### Authorization flow

```mermaid
sequenceDiagram
    participant C as Client
    box Middleware Pipeline
        participant P as Middleware Pipeline
        participant AZM as AuthorizationMiddleware
    end

    participant AZR as (IAuthorizationMiddlewareResultHandler)<br>AuthorizationMiddlewareResultHandler
    participant IAuthorizationPolicyProvider
    participant PE as (IPolicyEvaluator)<br>PolicyEvaluator

    participant ANS as (IAuthenticationService)<br>AuthenticationService 

    participant AZS as (IAuthorizationService)<br>DefaultAuthorizationService
    participant AZE as (IAuthorizationEvaluator)<br>DefaultAuthorizationEvaluator
    participant IAuthorizationHandlerProvider
    participant AZH as (IAuthorizationHandler)<br>AuthorizationHandler<TRequirement>

    C->>P: HTTP Request
    P->>AZM: Invoke(context)

    Note over P, AZH: Authorization phase

    AZM->>IAuthorizationPolicyProvider: GetPolicyAsync(policyName)
    IAuthorizationPolicyProvider-->>AZM: AuthorizationPolicy (requirements)

    AZM->>PE: AuthenticateAsync(policy, context)
    PE->>ANS: AuthenticateAsync(context, scheme)
    ANS-->>PE: AuthenticateResult (succeeded / failed)
    PE-->AZM: AuthenticateResult (success / no result)

    AZM->>PE: AuthorizeAsync(policy, authenticateResult!, context, resource);
    PE->>AZS: AuthorizeAsync(context.User, resource, policy)
    AZS->>IAuthorizationHandlerProvider: GetHandlerAsync(context, scheme)
    IAuthorizationHandlerProvider-->>AZS: IEnumerable<IAuthorizationHandler>
    AZS->>AZH: HandleAsync(authContext)

    AZH->>AZH: Handle Authorization (roles, claims, policy )

    AZH-->>AZS: Task
    AZS->>AZE: Evaluate(context, requirements, handlers)
    AZE-->>AZS: AuthorizationResult (succeeded / failed )
    AZS-->>PE: Task

    alt Authorization succeeded
        PE-->>AZM: AuthorizationResult.Success (Ticket)
        AZM->>AZR: HandleAsync(_next, context, policy, authorizeResult);
        AZR->>P: _next(context) 
    else Authorization failed
        PE-->>AZM: AuthenticateResult.NoResult
        AZM->>AZR: HandleAsync(_next, context, policy, authorizeResult);
        AZR->>ANS: ForbidAsync(context, scheme) or ChallengeAsync(context, scheme)
        ANS-->>AZR: Task
        AZR-->>AZM: Task
        AZM-->>C: 403 Forbidden or 401 Unauthorized
    end
```
