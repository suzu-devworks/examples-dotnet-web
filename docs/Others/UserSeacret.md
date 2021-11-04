# User secrets 

## See also

* https://docs.microsoft.com/ja-jp/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=linux


## Setup User secrets

```shell
cd src/Examples.WebApi
dotnet user-secrets init

dotnet user-secrets set "Movies:ServiceApiKey" "12345"
dotnet user-secrets remove "Movies:ServiceApiKey"

cd ../../

```
