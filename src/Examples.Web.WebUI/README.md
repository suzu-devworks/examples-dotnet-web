# Examples.Web.WebUI


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
