$MyServer = "127.0.0.1"
$MyPort  = "5432"
$MyDB = "postgres"
$MyUid = "postgres"
$MyPass = "mysecretpassword"
while (1) {
    docker run --name external-db-postgres -e POSTGRES_PASSWORD=mysecretpassword -v C:/TEMP/postgres:/var/lib/postgresql/data -p 5432:5432 -d postgres:11
    Start-Sleep -Seconds 6
    #Write some stuff
    $DBConnectionString = "Driver={PostgreSQL UNICODE(x64)};Server=$MyServer;Port=$MyPort;Database=$MyDB;Uid=$MyUid;Pwd=$MyPass;"
    $DBConn = New-Object System.Data.Odbc.OdbcConnection;
    $DBConn.ConnectionString = $DBConnectionString;
    $DBConn.Open();
    $DBCmd = $DBConn.CreateCommand();
    $date = Get-Date
    $DBCmd.CommandText = "insert into test.test (coltest) values ('$date');";
    $DBCmd.ExecuteReader();
    $DBConn.Close();
    Start-Sleep -Seconds 3
    #Show logs
    docker logs external-db-postgres
    #Kill container
    docker rm external-db-postgres -f
    Start-Sleep -Seconds 1
}