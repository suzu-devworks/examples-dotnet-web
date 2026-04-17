# Examples.Web.Infrastructure.Assets

## Table of Contents <!-- omit in toc -->

## Overview

This project is for sharing front-end static assets, such as JavaScript files, across projects.

I created it because the number of similar files was increasing.

## How to use

Because it is shared in lib as follows,
you can replace `wwwroot` in this project with `lib` and reference it.

```xml
    <StaticWebAssetBasePath>lib</StaticWebAssetBasePath>
```

e.g.

```html
 <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
```

## Development

### How the project was initialized

This project was initialized with the following commands:

```shell
## Solution
dotnet new sln -o .

## Examples.Web.Infrastructure.Assets
dotnet new razorclasslib -o src/Examples.Web.Infrastructure.Assets
dotnet sln add src/Examples.Web.Infrastructure.Assets/
cd src/Examples.Web.Infrastructure.Assets
cd ../../

# Check outdated packages
dotnet list package --outdated

# Tools
dotnet new tool-manifest
dotnet tool install microsoft.web.librarymanager.cli
dotnet tool restore

# JavaScript libraries
cd src/Examples.Web.Infrastructure.Assets
dotnet libman init --default-provider jsdelivr 
dotnet libman install bootstrap -d wwwroot/bootstrap
dotnet libman install jquery -d wwwroot/jquery
dotnet libman install jquery-validation -d wwwroot/jquery-validation
dotnet libman install jquery-validation-unobtrusive -d wwwroot/jquery-validation-unobtrusive

dotnet libman restore
cd ../../
```
