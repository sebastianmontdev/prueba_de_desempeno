Sports Complex Management System
this project is  a app web build in asp.net core 9.0 for Sports Complex Management System,deportive spaces and reservations from the spaces


main functionalities

user manage: Registrer, edit and list of athletes.

spaces manage: fild's administrations, pools by filters by type..

reservations:
automatic validation for avoid peopel with same space at same time.
persistence: Conexión  data base  MySQL in VPS.

technologies used
automatic validation
Language: C#
Framework: ASP.NET Core MVC (net9.0)
ORM: Entity Framework Core
Data base: MySQL
Style: Bootstrap 5

settings

1. step one
   SDK of .NET 9.0 installed.
   vps's credentials.

2. data base
   Ejecuta el script SQL proporcionado en tu gestor de base de datos (MySQL) para crear las tablas necesarias (Usuarios, EspaciosDeportivos, Reservas).

3. run project
   open the terminal in a file of the project:

bash
dotnet run


the app would be avalible in: http://localhost:5138

project structure.

Controllers: Lógic.
Models: Clases that represent the tables in data base.
Views: User interface (HTML/Razor).
Data: Contexto de conexión a la base de datos (EF Core).
context about data base conection

repository's link : https://github.com/sebastianmontdev/prueba-de-desempe-o-c-.git
sebastian montaño ramirez
clan c#
cc:1238938411