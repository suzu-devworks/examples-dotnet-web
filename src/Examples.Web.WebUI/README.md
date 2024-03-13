# Examples.Web.WebUI

## Index

- [Tiny colored console](../../docs/logging/logging_use_console.md)


## Project Initialize

```shell
## Solution
dotnet new sln -o .

## Examples.Web.WebUI
dotnet new webapp -o src/Examples.Web.WebUI
dotnet sln add src/Examples.Web.WebUI/
cd src/Examples.Web.WebUI

dotnet user-secrets init
cd ../../

# Update outdated package
dotnet list package --outdated
```
