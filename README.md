To work with application you need to go to Initialization folder. In InitialFile there are several commands to pull a container with ms sql database and to create new database.

How to add a new migration?
1) Start a CLI for a folder with target DAL project
2) Uncomment an OnConfiguring method in your context file, add a hardcode database connection string into it (from appsettings.json)
3) Remove any calls of the Configuration from the context factory. There is no way to call our appsettings from migration process
4) Use command: dotnet ef migrations add 'migration name'
5) Use command: dotnet ef database update
6) Undo changes from context and context factory