using Npgsql;
using System;
using System.Data.SqlClient;

//
// 1) Before start of this app start postgres in docker
//      docker run --rm --name postgresdb -e POSTGRES_PASSWORD=mysecretpassword -p 5432:5432 -it postgres
// 2) Add NPQSQL package
//
// Sample to search text with Like operator can be found in ../SolLucene/PostgreSQLSearchDemo.cs

namespace PostgreSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            int repeat = 1;
            for (int i = 1; i <= repeat; i++)
            {
                Random rnd = new Random();
                
                SampleConnection(rnd.Next(1, 20));
            }
        
        }

        static void SampleConnection(int randomInt
            
            )
        {
            string connectionParams = String.Format("Server=127.0.0.1;Username=postgres;Database=postgres;Port=5432;Password=mysecretpassword;SSLMode=Prefer");

            using (var connection = new NpgsqlConnection(connectionParams))
            {
                /*
                Ist ok sich mit dem EF zu beschäftigen, aber das wrappt die komplette Transaktion - das kannst Du wahrscheinlich schwer unterbrechen / simulieren. 
 
                Nimm lieber das Sample:´und versuche das nachzubilden: 
 
                a) Datenbank öffnen
                b) Transaktion starten 
                c) Datenbankoperation ausführen
                d) - ReadLine - damit der Prozess anhält. 
                e) Prozess killen (am besten die Datenbank in Docker-Linux mit einem kill -9) 
                f) Resultat ? Datenbank noch mit den üblichen Tools erreichbar 
 
                Dieses Ergebnis reicht eigentlich schon für eine weitere Analyse - das wären dann die nächsten Schritte*/

                

                    try
                    {
                        //BaseSample(connection);
                        TransactionSample(connection, randomInt);

                    }
                    catch (Exception)
                    {
                        // do nothing
                        // throw;
                    }
                
            }
        }


        static void TransactionSample(NpgsqlConnection connection, int randomInt)
        {
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                String ErrorReturnMessage = "can not open connection: " + connection.ConnectionString.ToString();
                Console.WriteLine(ErrorReturnMessage);
                throw new System.ArgumentException(ErrorReturnMessage);
            }

            // Begin Transaction V1/4
            string randomTableName = "DemoTable" + randomInt.ToString(); //  " + randomTableName + "
            NpgsqlTransaction SampleTransaction = connection.BeginTransaction();
            ExecuteCommand(connection, "DROP TABLE IF EXISTS " + randomTableName);
            ExecuteCommand(connection, "CREATE TABLE " + randomTableName + "(id serial PRIMARY KEY, firstName VARCHAR(50), lastName VARCHAR(50))");
            ExecuteCommandAddValues(connection, "INSERT INTO " + randomTableName + @" (firstName, lastName)
                                                      VALUES (@firstName1, @lastName1), 
                                                             (@firstName2, @lastName2), 
                                                             (@firstName3, @lastName3)",
                                                             new[] { ("firstName1", "Tarry"), ("lastName1", "Totter"),
                                                                     ("firstName2", "Tahra"), ("lastName2", "Tumess"),
                                                                     ("firstName3", "Tobby"), ("lastName3", "Totter") });
           
            try
            {
                SampleTransaction.Save("Testsafepoint" + randomInt.ToString());
            }
            catch (Exception)
            {
               // Do nothing
               // throw;
            }
            
           

            // UserInput
            // Type your username and press enter
            Console.WriteLine("You can enter anything. e.g. Totter");

            // Create a string variable and get user input from the keyboard and store it in the variable
            string anyThing = Console.ReadLine();

            // Print the value of the variable (userName), which will display the input value
            Console.WriteLine("You entered " + anyThing);

            // Commit Transaction
            
            SampleTransaction.Commit();



            //https://www.postgresql.org/docs/current/static/sql-copy.html
            //connection.BeginTextImport
            using (var command = new NpgsqlCommand($"SELECT * FROM " + randomTableName + " WHERE lastName = (@p1)", connection)) // Commands can be batched = parallel requests
            {
                command.Parameters.AddWithValue("p1", anyThing);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read()) { Console.WriteLine(dr[1]); }
                    dr.DisposeAsync().GetAwaiter().GetResult();
                }
            }

            connection.Close();



        }


        static void BaseSample(NpgsqlConnection connection)
        {
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                String ErrorReturnMessage = "can not open connection: " + connection.ConnectionString.ToString();
                Console.WriteLine(ErrorReturnMessage);
                throw new System.ArgumentException(ErrorReturnMessage);
            }

            // Create table 
            ExecuteCommand(connection, "DROP TABLE IF EXISTS DemoTable");
                ExecuteCommand(connection, "CREATE TABLE DemoTable(id serial PRIMARY KEY, firstName VARCHAR(50), lastName VARCHAR(50))");

                ExecuteCommandAddValues(connection, @"INSERT INTO DemoTable (firstName, lastName)
                                                      VALUES (@firstName1, @lastName1), 
                                                             (@firstName2, @lastName2), 
                                                             (@firstName3, @lastName3)",
                                                             new[] { ("firstName1", "Harry"), ("lastName1", "Potter"),
                                                                     ("firstName2", "Sahra"), ("lastName2", "Dumess"),
                                                                     ("firstName3", "Bobby"), ("lastName3", "Potter") });


                using (var command = new NpgsqlCommand($"SELECT * FROM DemoTable WHERE lastName = (@p1)", connection)) // Commands can be batched = parallel requests
                {
                    command.Parameters.AddWithValue("p1", "Potter");
                    using (NpgsqlDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read()) { Console.WriteLine(dr[1]); }
                        dr.DisposeAsync().GetAwaiter().GetResult();
                    }
                }

                connection.Close();

        }

        static int ExecuteCommand(NpgsqlConnection connection, string commandString)
        {
            using (var command = new NpgsqlCommand(commandString, connection))
            {
                return command.ExecuteNonQuery();
            }
        }


        static int ExecuteCommandAddValues(NpgsqlConnection connection, string commandString, (string paramName, string paramValue)[] paramsToAdd)
        {
            return ExecuteCommandParam(connection, commandString, 
                (x => { foreach (var t in paramsToAdd) { x.AddWithValue(t.paramName, t.paramValue);  } }));
        }

        static int ExecuteCommandParam(NpgsqlConnection connection, string commandString, Action<NpgsqlParameterCollection> adder)
        {
            using (var command = new NpgsqlCommand(commandString, connection))
            {
                adder(command.Parameters);
                return command.ExecuteNonQuery();
            }
        }
    }
}
