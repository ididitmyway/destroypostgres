## Nuget Paket 1 Npqsql Versions History
https://www.nuget.org/packages/Npgsql/4.1.2

## Npsql Bugs
> https://github.com/npgsql/npgsql/issues
> https://github.com/npgsql/efcore.pg/issues
> https://github.com/npgsql/EntityFramework6.Npgsql/issues


## Quick Guide start PostgreSQL in Docker
In Shell type
`docker pull postgres:11`

Connection String in C# Sample is
`string connectionParams = String.Format("Server=127.0.0.1;Username=postgres;Database=postgres;Port=5432;Password=mysecretpassword;SSLMode=Prefer");`

Letz do a quick test -p <hostport=5432>:<dockerport=5432>
`$ docker run --name some-postgres -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -d postgres:11`

Run the VS Sample Projekt on your Windows host.

Result should look like

```console
Harry
Bobby

C:\....\PostgreSQL.exe (process 20016) exited with code 0.
To automatically close the console when debugging stops, enable Tools->Options->Debugging->Automatically close the console when debugging stops.
Press any key to close this window . . .
```
> If the database is unavailable you get error 

### PG Admin 4 in Docker
`docker pull dpage/pgadmin4:4.27`
docker run -p 80:80 -e 'PGADMIN_DEFAULT_EMAIL=user@domain.com' -e 'PGADMIN_DEFAULT_PASSWORD=mysecretpassword' -d dpage/pgadmin4:4.27


## Postgres in Docker CLI
### Check Posgres Version 
It is given that the Docker Console is opened.
Type
`postgres -V`
```shell
# postgres -V
postgres (PostgreSQL) 13.0 (Debian 13.0-1.pgdg100+1)
```

### Connect to Database
You not need the Password otherwise add -W or --password
`psql -d postgres -U  postgres`
```shell
# psql -d postgres -U  postgres
psql (13.0 (Debian 13.0-1.pgdg100+1))
Type "help" for help.

postgres=#
```

### Look table definition DemoTable
The "DemoTable" was created by the sample

Type `\d DemoTable`
```shell
postgres-# \d DemoTable
                                     Table "public.demotable"
  Column   |         Type          | Collation | Nullable |                Default
-----------+-----------------------+-----------+----------+---------------------------------------
 id        | integer               |           | not null | nextval('demotable_id_seq'::regclass)
 firstname | character varying(50) |           |          |
 lastname  | character varying(50) |           |          |
Indexes:
    "demotable_pkey" PRIMARY KEY, btree (id)
```

### Show "DemoTable" values
`SELECT * FROM DemoTable;`
```shell
postgres=# SELECT * FROM DemoTable;
 id | firstname | lastname
----+-----------+----------
  1 | Harry     | Potter
  2 | Sahra     | Dumess
  3 | Bobby     | Potter
(3 rows)
```

### psql command´s
#### Check psql is installed
`psql -V`

```shell
# psql -V
psql (PostgreSQL) 13.0 (Debian 13.0-1.pgdg100+1)
```

#### Connect to database postgres with user postgres

`psql -d postgres -U  postgres -W`

```shell
psql (PostgreSQL) 13.0 (Debian 13.0-1.pgdg100+1)
# psql -d postgres -U  postgres -W
Password:
psql (13.0 (Debian 13.0-1.pgdg100+1))
Type "help" for help.
```
You should now connected

```shell
postgres=#
```
#### List available databases
`\l`
```shell
postgres-# \l
                                 List of databases
   Name    |  Owner   | Encoding |  Collate   |   Ctype    |   Access privileges
-----------+----------+----------+------------+------------+-----------------------
 postgres  | postgres | UTF8     | en_US.utf8 | en_US.utf8 |
 template0 | postgres | UTF8     | en_US.utf8 | en_US.utf8 | =c/postgres          +
           |          |          |            |            | postgres=CTc/postgres
 template1 | postgres | UTF8     | en_US.utf8 | en_US.utf8 | =c/postgres          +
           |          |          |            |            | postgres=CTc/postgres
(3 rows)
```

List table
`\dt`
```shell
postgres-# \dt
           List of relations
 Schema |   Name    | Type  |  Owner
--------+-----------+-------+----------
 public | demotable | table | postgres
(1 row)
```

Quit psql
`\q`
```shell
postgres-# \q
#
```


## Optional Grafana
https://grafana.com/grafana/dashboards/455
### Prometheus
https://prometheus.io/docs/prometheus/latest/installation/
docker run -p 9090:9090 prom/prometheus

