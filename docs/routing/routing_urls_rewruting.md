# Urls Path base rewriting

## Table of contents <!-- omit in toc -->

- [Urls Path base rewriting](#urls-path-base-rewriting)
  - [Situation](#situation)
  - [Configuration](#configuration)

## Situation

This is required if you want to add a prefix to the URL.

```mermaid
flowchart LR
    OU1[User-Agent]

    subgraph GC[containers]
        subgraph GS1[web1]
            NW1{{"NGINX"}}
        end
        subgraph GS2[dev1]
            CP1("kestrel<br>webapi")
        end
        subgraph GS3[dev2]
            CP2("kestrel<br>webapi")
        end
    end

    %%サービス同士の関係
    OU1 --> |https://*:443| NW1
    NW1 --> |http://*:8081/api1| CP1
    NW1 --> |http://*:8082/api2| CP2

classDef SGC fill:none,color:#345,stroke:#345
class GC SGC

classDef SOU fill:#aaa,color:#fff,stroke:#fff
class OU1 SOU

classDef SNW fill:#84d,color:#fff,stroke:none
class NW1 SNW

classDef SCP fill:#e83,color:#fff,stroke:none
class CP1 SCP
class CP2 SCP
```

In such a configuration, it is difficult to set the prefix in advance on the application side, so I would like to build the application with the prefix ignored.

## Configuration

Use [`UsePathBase`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.builder.usepathbaseextensions.usepathbase?view=aspnetcore-8.0) to achieve this.

The order in which you set this method is important.

Basically before `UseRouting`. Add as needed.

```cs
app.UsePathBase("/api1"); // here
app.UseRouting();
```

However, if you have swagger while debugging, PathBase will only be valid for swagger if you don't put it before `UseSwagger`.

```cs
app.UsePathBase("/api1"); // here

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
```

If you use a reverse proxy, you may need to disable UseHttpsRedirection.

```cs
//app.UseHttpsRedirection();
```
