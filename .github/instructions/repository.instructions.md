---
description: Repository-specific constraints for examples-dotnet-web.
applyTo: "**"
---

# Repository Instructions

- Keep framework versions centralized. In `src/**/*.csproj`, use `$(LatestFramework)` or `$(LTSFrameworks)` from `src/Directory.Build.props`. Do not hardcode `netX.Y` unless intentionally diverging.
- Do not upgrade `Grpc.Tools` in `src/Examples.Web.Infrastructure.GrpcClient/Examples.Web.Infrastructure.GrpcClient.csproj` from `2.67.0` without explicit confirmation. This pin exists for Apple Silicon compatibility.
- For gRPC client contract changes, update `src/Examples.Web.WebApi.Grpc/Protos/greet.proto` and keep `Examples.Web.Infrastructure.GrpcClient` consuming it via `SharedProtoRoot`. Do not fork or copy the same proto into multiple projects.
- Preserve certificate-auth integration points in `Examples.Web.Authentication.Certificate`: `Authentication:Certificate:CustomTrustStore`, certificate loading via `CertificateLoader`, and forwarded header `X-Client-Cert`.
- Keep shared web concerns in `Examples.Web.Infrastructure*` projects and reference them from sample apps. Avoid duplicating the same cross-cutting logic in each `Examples.Web.*` app.
- Preserve `src/Examples.Web.WebApp` references to `src/fixtures/Examples.Web.HostingStartup1` and `src/fixtures/Examples.Web.HostingStartup2` unless hosting-startup sample behavior is intentionally being changed.
