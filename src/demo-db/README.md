# DEMO DB

## Run Demo PostgreSQL Database locally

```bash
docker run -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=demo -p 5432:5432 -d postgres:16.3
```

## Apply Migrations Locally

| Step                             | Command                |
|----------------------------------|------------------------|
| Get the Migration Status         | `liquibase status`     |
| Inspect the SQL that will be run | `liquibase update-sql` |
| Execute the Migration            | `liquibase update`     |

## Applying Migrations to AWS Db

> Note: Password is placeholder. Update with the actual password.

### Status

```bash
liquibase --url="jdbc:postgresql://int-demo-dev-db.dev-equifax.acs.millimanintelliscript.com:5432/demo" \
          --username="master" \
          --password="9)?Q*K0vt5ZWOTfm" \
          status
```

### Update

```bash
liquibase --url="jdbc:postgresql://int-demo-dev-db.dev-equifax.acs.millimanintelliscript.com:5432/demo" \
          --username="master" \
          --password="9)?Q*K0vt5ZWOTfm" \
          update
```

