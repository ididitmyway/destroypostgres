for ($i = 0; $i -lt 100; $i++) {
    Start-Process -FilePath '.\netcoreapp3.0\PostgreSQL.exe'  
}
Stop-Process -Name PostgreSQL
Start-Process docker -ArgumentList "kill some-postgres-11"
Start-Process docker -ArgumentList "start some-postgres-11"
for ($i = 0; $i -lt 100; $i++) {
    Start-Process -FilePath '.\netcoreapp3.0\PostgreSQL.exe'  
}
Start-Process docker -ArgumentList "kill some-postgres-11"
Start-Process docker -ArgumentList "start some-postgres-11"
Stop-Process -Name PostgreSQL