# Cshrap-M1-Project
4th year project of C# at EFREI

## Development

### Prerequisite

-   Docker

### Initialisation

#### Docker compose

Before running the application you will need to run the `docker-compose.yml` file

```shell script
docker-compose run
```

The docker compose will install and run for you the project database (PostgreSQL 12)


#### Database

Once the docker-compose is running **you will need** to set up the database tables and data with the included scripts.
The database is already created with the name `postgres`, and the user `adm`, password `adm`.

1. Run `create-tables.sql` to initialize all the tables.
2. Run `fill-database.sql` for test purposes, to add data in the DB.

