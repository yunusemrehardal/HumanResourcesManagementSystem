# HumanResourcesManagementSystem

A Sample N-layered .NET CORE Project Demonstrating Clean Architecture and the Generic Repository Pattern.

## Migrations

### Infrastructure
Firstly,Set the project "Web" as startup project.
Secondly, choose infrastructure on Package Manager Console .
```
Add-Migration InitialCreate -o  Data/Migrations 
Update-Database 

```


## Packages Installed

### Web
```
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -v 6.0.14
Install-Package Microsoft.AspNetCore.Identity.UI -v 6.0.14
Install-Package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore -v 6.0.14
Install-Package Microsoft.EntityFrameworkCore.Design -v 6.0.14

```

### Application Core
```
Install-Package Ardalis.Specification 
```

### Infrastructure
```
Install-Package Microsoft.EntityFrameworkCore -v 6.0.14
Install-Package Microsoft.EntityFrameworkCore.Tools -v 6.0.14
Install-Package Npgsql.EntityFrameworkCore.PostgreSQL -v 6.0.8
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -v 6.0.14
Install-Package Ardalis.Specification.EntityFrameworkCore -v 6.1.0
```




