# Kill Postgres with Container in a loop

Kill container some-postgress-11. 
Container restart´s successfully and Postgress DB keeps working well.

```powershell
Start-Process docker -ArgumentList "kill some-postgres-11"
```

# Kill Postgres from Linux shell

After the third kill -9 Postgres runs in Recovery mode.

```powershell

docker exec -it some-postgres-11 /bin/bash -c 'kill -9 $(pidof postgres)'

```

# Full Powershell Script

```powershell

for ($i = 0; $i -lt 9999; $i++)
{
    docker exec -it some-postgres-11 /bin/bash -c 'pidof postgres'

    for ($i = 0; $i -lt 100; $i++) {
        Start-Process -FilePath 'C:\Bens\Code\Postgress\bin\Debug\netcoreapp3.0\PostgreSQL.exe'  
    }
    
    
    #Start-Process docker -ArgumentList "exec -it some-postgres-11 /bin/bash -c 'pidof postgres && kill -9 `$(pidof postgres) && pidof postgres'"
    
    docker exec -it some-postgres-11 /bin/bash -c 'kill -9 $(pidof postgres)'
    
    Stop-Process -Name PostgreSQL
    
    docker exec -it some-postgres-11 /bin/bash -c 'pidof postgres'
    
}

```