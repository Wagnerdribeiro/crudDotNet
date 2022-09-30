using System;
using System.Threading;
using Microsoft.Data.Sqlite;

namespace crudDotNet
{
    class Program
    {
        public class Modelo
        {
            public int Id { get; set; }
            public string Descricao { get; set; }
        }
        public class Rastreador
        {
            public int Id { get; set; }
            public int Modelo { get; set; }
            public string ICCID { get; set; }
            public string IMEI { get; set; }
        }
        static string connectionString = @"Data Source=cruddotnet.db";
        static void Main(string[] args)
        {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableModelo = connection.CreateCommand();
                tableModelo.CommandText =
                @"CREATE TABLE IF NOT EXISTS Modelo (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Descricao TEXT
                )";
                tableModelo.ExecuteNonQuery();

                var tableRastreador = connection.CreateCommand();
                tableRastreador.CommandText =
                @"CREATE TABLE IF NOT EXISTS Rastreador (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Modelo INTEGER,
                    ICCID TEXT,
                    IMEI TEXT
                )";
                tableRastreador.ExecuteNonQuery();

                connection.Close();
            }

            OptionsMenu();
        }

        static void OptionsMenu()
        {
            Console.Clear();

            bool closeMenu = false;

            while (closeMenu == false)
            {
                Console.WriteLine("\n\nMenu Principal");
                Console.WriteLine("\nEscolha uma opção:");
                Console.WriteLine("\nEscolha 0 para fechar o app.");
                Console.WriteLine("1 - Inserir novo modelo");
                Console.WriteLine("2 - Ver todos modelos.");
                Console.WriteLine("3 - Deletar modelo.");
                Console.WriteLine("4 - Atualizar modelo.");
                Console.WriteLine("5 - Inserir novo rastreador");
                Console.WriteLine("6 - Ver todos rastradores.");
                Console.WriteLine("7 - Deletar rastreador.");
                Console.WriteLine("8 - Atualizar rastreador.");
                Console.WriteLine("------------------------------------------\n");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nAté mais :) !\n");
                        closeMenu = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        Insert("Modelo");
                        break;
                    case "2":
                        GetAll();
                        break;
                    case "3":
                        Delete("Modelo");
                        break;
                    case "4":
                        Update("Modelo");
                        break;
                    case "5":
                        Insert("Rastreador");
                        break;
                    case "6":
                        GetAllRastreador();
                        break;
                    case "7":
                        Delete("Rastreador");
                        break;
                    case "8":
                        Update("Rastreador");
                        break;
                    default:
                        Console.WriteLine("\nComando inválido\n");
                        break;
                }
            }

        }

        private static void Insert(string entityName)
        {
            Console.Clear();
            if (entityName == "Modelo")
            {
                Console.Write("Qual modelo ? ");
                string description = Console.ReadLine();

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText =
                    $"INSERT INTO Modelo(Descricao) VALUES('{description}')";

                    tableCmd.ExecuteNonQuery();

                    connection.Close();
                }
                Console.WriteLine($"\n\nO modelo foi cadastrado com sucesso. \n\n");
                Thread.Sleep(2000);
                Console.Clear();
                OptionsMenu();
            }

            var modelos = GetAll();

            Console.Write("\nInforme o (ID)modelo do rastreador ? ");
            string idModelo = Console.ReadLine();
            Console.Write("\nInforme o ICCID ? ");
            string ICCID = Console.ReadLine();
            Console.Write("\nInforme o IMEI ? ");
            string IMEI = Console.ReadLine();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                $"INSERT INTO Rastreador(Modelo, ICCID, IMEI) VALUES({idModelo}, '{ICCID}', '{IMEI}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
            Console.WriteLine($"\n\nO Rastreador foi cadastrado com sucesso. \n\n");
            Thread.Sleep(2000);
            Console.Clear();
        }

        private static List<Modelo> GetAll()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"SELECT * FROM Modelo";

                List<Modelo> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new Modelo
                        {
                            Id = reader.GetInt32(0),
                            Descricao = reader.GetString(1)
                        });
                    }
                }

                connection.Close();

                if (tableData.Count == 0)
                {
                    Console.WriteLine("\nNenhum modelo cadastrado");
                    Thread.Sleep(2000);
                    OptionsMenu();
                }

                Console.WriteLine("Modelos cadastrados");
                foreach (var data in tableData)
                {
                    Console.WriteLine($"\nId........: {data.Id}");
                    Console.WriteLine($"Descrição.: {data.Descricao} \n");
                }
                Console.WriteLine("\n-----------------------------------------------------------");

                Thread.Sleep(1000);
                return tableData;
            }

        }
        private static List<Rastreador> GetAllRastreador()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"SELECT * FROM Rastreador";

                List<Rastreador> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new Rastreador
                        {
                            Id = reader.GetInt32(0),
                            Modelo = reader.GetInt32(1),
                            ICCID = reader.GetString(2),
                            IMEI = reader.GetString(3),
                        });
                    }
                }

                connection.Close();
                if (tableData.Count == 0)
                {
                    Console.WriteLine("\nNenhum rastreador cadastrado");
                    Thread.Sleep(2000);
                    OptionsMenu();
                }
                foreach (var data in tableData)
                {
                    Console.WriteLine($"\nId........: {data.Id} \n");
                    Console.WriteLine($"Modelo....: {data.Modelo} \n");
                    Console.WriteLine($"ICCID.....: {data.ICCID} \n");
                    Console.WriteLine($"IMEI......: {data.IMEI} \n");
                }

                Thread.Sleep(1000);
                return tableData;
            }
        }

        private static void Delete(string entityName)
        {
            Console.Clear();

            if (entityName == "Modelo")
            {
                var modelos = GetAll();

                Console.Write("\nInforme o ID do modelo que deseja deletar: ");
                var idModel = Console.ReadLine();

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();

                    tableCmd.CommandText = $"DELETE from Modelo WHERE Id = '{idModel}'";

                    int rowCount = tableCmd.ExecuteNonQuery();

                    if (rowCount == 0)
                    {
                        Console.WriteLine($"\n\nO modelo {idModel} não existe. \n");
                        Console.WriteLine("Aguarde para informar outro ID\n");
                        Thread.Sleep(2000);
                        Delete("Modelo");
                    }

                }

                Console.WriteLine($"\n\nO modelo {idModel} foi deletado com sucesso. \n\n");
                Thread.Sleep(2000);
                Console.Clear();

                OptionsMenu();
            }

            var rastreador = GetAllRastreador();

            Console.Write("\nInforme o ID do rastrador que deseja deletar: ");
            var idRastreador = Console.ReadLine();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from Rastreador WHERE Id = '{idRastreador}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nO rastreador {idRastreador} não existe. \n");
                    Console.WriteLine("Aguarde para informar outro ID\n");
                    Thread.Sleep(2000);
                    Delete("Rastreador");
                }

            }

            Console.WriteLine($"\n\nO Rastreador {idRastreador} foi deletado com sucesso. \n\n");
            Thread.Sleep(2000);
            Console.Clear();

            OptionsMenu();
        }

        private static void Update(string entityName)
        {
            Console.Clear();

            if (entityName == "Modelo")
            {
                GetAll();
                Console.Write("\nInforme o ID do modelo que deseja alterar: ");
                var idModel = Console.ReadLine();

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    var checkCmd = connection.CreateCommand();
                    checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM Modelo WHERE Id = {idModel})";
                    int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (checkQuery == 0)
                    {
                        Console.WriteLine($"\n\nEste modelo {idModel} não existe.\n\n");
                        connection.Close();
                        Thread.Sleep(2000);
                        Update("Modelo");
                    }

                    Console.Write("Informe a nova descrição deste modelo: ");
                    var description = Console.ReadLine();

                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText = $"UPDATE Modelo SET Descricao = '{description}' WHERE Id = {idModel}";
                    tableCmd.ExecuteNonQuery();

                    Console.Write("\nModelo atualizado com sucesso!");
                    Thread.Sleep(2000);

                    connection.Close();
                }
                OptionsMenu();
            }

            GetAllRastreador();
            Console.Write("\nInforme o ID do Rastreador que deseja alterar: ");
            var idRastreador = Console.ReadLine();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM Rastreador WHERE Id = {idRastreador})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nEste modelo {idRastreador} não existe.\n\n");
                    connection.Close();
                    Thread.Sleep(2000);
                    Update("Rastreador");
                }

                Console.Write("\nInforme o (ID)modelo do rastreador ? ");
                string idModelo = Console.ReadLine();
                Console.Write("\nInforme o ICCID ? ");
                string ICCID = Console.ReadLine();
                Console.Write("\nInforme o IMEI ? ");
                string IMEI = Console.ReadLine();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = 
                $"UPDATE Modelo SET Modelo = '{idModelo}, ICCID = {ICCID}, IMEI = {IMEI}' WHERE Id = {idRastreador}";
                tableCmd.ExecuteNonQuery();

                Console.Write("\nRastreador atualizado com sucesso!");
                Thread.Sleep(2000);

                connection.Close();
            }
            OptionsMenu();

        }
        
    }
}
