using System.CommandLine;
using Examples.Web.Generator.JwtToken.Commands;
using Examples.Web.Generator.JwtToken.Services;

await using var services = new SimpleServiceProvider();

services.Register<IConsoleService>(new StandardConsoleService());
services.Register<JwtService>(new JwtService());

RootCommand rootCommand = new("Tools for JWT tokens for authentication testing.");
rootCommand.Subcommands.Add(SignCommand.Create(services));
rootCommand.Subcommands.Add(VerifyCommand.Create(services));
rootCommand.Subcommands.Add(JwksCommand.Create(services));

ParseResult parseResult = rootCommand.Parse(args);
return await parseResult.InvokeAsync();
