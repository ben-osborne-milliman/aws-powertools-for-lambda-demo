# DEMO DB

## Run Demo PostgreSQL Database locally

```bash
docker run -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=demo -p 5432:5432 -d postgres:16.3
```

## Apply Migrations

| Step                             | Command                |
|----------------------------------|------------------------|
| Get the Migration Status         | `liquibase status`     |
| Inspect the SQL that will be run | `liquibase update-sql` |
| Execute the Migration            | `liquibase update`     |

