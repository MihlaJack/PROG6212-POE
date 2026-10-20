# Database Migrations

This folder will contain Entity Framework Core migration files when generated.

## How to Create Migrations

1. Ensure you have the EF Core tools installed:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

2. Create a new migration:
   ```bash
   dotnet ef migrations add MigrationName --project RaceDayAPI/RaceDayAPI
   ```

3. Apply the migration to the database:
   ```bash
   dotnet ef database update --project RaceDayAPI/RaceDayAPI
   ```

## Important Notes

- Migrations are not included in the initial commit as they depend on the local database setup
- Each developer should run migrations on their local database
- Production migrations should be reviewed before applying
