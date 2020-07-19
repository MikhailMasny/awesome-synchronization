# Scheduled synchronization

![.NET Core](https://github.com/MikhailMasny/awesome-synchronization/workflows/.NET%20Core/badge.svg)

A web-application developed on the .NET 3.1 (LTS). The main idea of a web application is to develop a system for data synchronization based on scheduled tasks. This repository can also serve as a template for creating the necessary data synchronization with a specific database or resource on the Internet.

## Getting Started

Many applications face a problem where, when rewriting an application to a newer technology stack, it is necessary to interact with old data, as they can be used by other applications. In this case, data synchronization is used, so the presented example in this repository can solve this problem. An example is synchronizing data from another database where data may change periodically.

### EF Migrations

For EF migrations, use the following commands from Package Manager Console:

```
add-migration -context context -outputdir "Folder/Migrations"
remove-migration -context Context
update-database -context Context
```

## Built with
- [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/);
- [Clean architecture](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures);
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/);
- [Serilog](https://serilog.net/);
- [Automapper](https://automapper.org/);
- [Coravel](https://github.com/jamesmh/coravel);
- [xUnit](https://xunit.net/);

## Author
[Mikhail M.](https://mikhailmasny.github.io/) - Software Engineer;

## License
This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/MikhailMasny/awesome-synchronization/blob/master/LICENSE) file for details.
