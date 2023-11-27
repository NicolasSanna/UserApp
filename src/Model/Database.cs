// Chargement du client MySQL
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

// Espace de nom correspondant à l'application et au dossier src/Model
namespace App.src.Model
{
    // Classe Database
    public class Database
    {
        // Propriété onnection pour MySQL et Singleton
        private static Database? Instance = null;
        private readonly MySqlConnection Connection;

        // Constructeur lors de l'instanciation de l'objet
        private Database()
        {
            this.Connection = this.GetDbConnection();
        }

        // Création de la méthode getInstance() pour le singleton
        public static Database GetInstance()
        {
            if (Database.Instance == null)
            {
                Database.Instance = new Database();
            }
            return Database.Instance;
        }

        // Création de la connextion MySQL avec les informations
        private MySqlConnection GetDbConnection()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }

        private void OpenConnection()
        {
            if (this.Connection.State == System.Data.ConnectionState.Closed)
            {
                this.Connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (this.Connection.State == System.Data.ConnectionState.Open)
            {
                this.Connection.Close();
            }
        }

        // Méthode d'exécution d'une requête SQL
        public void ExecuteQuery(string sql, Dictionary<string, object>? parameters = null)
        {
            this.OpenConnection();

            using (MySqlCommand cmd = new MySqlCommand(sql, this.Connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                cmd.ExecuteNonQuery();
            }

            this.CloseConnection();
        }

        // Méthode de récupération d'un résultat
        public Dictionary<string, object> GetOneResult(string sql, Dictionary<string, object>? parameters = null)
        {
            this.OpenConnection();

            using MySqlCommand cmd = new MySqlCommand(sql, this.Connection);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Dictionary<string, object> result = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result[reader.GetName(i)] = reader[i];
                    }

                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        // Méthode de récupération de plusieurs résultats
        public List<Dictionary<string, object>> GetAllResults(string sql, Dictionary<string, object>? parameters = null)
        {
            this.OpenConnection();

            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            using (MySqlCommand cmd = new MySqlCommand(sql, this.Connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dictionary<string, object> row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader[i];
                        }

                        results.Add(row);
                    }
                }
            }

            this.CloseConnection();

            return results;
        }

        public int Insert(string sql, Dictionary<string, object>? parameters = null)
        {
            this.OpenConnection();

            using (MySqlCommand cmd = new MySqlCommand(sql, this.Connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                cmd.ExecuteNonQuery();

                int lastInsertId = (int)cmd.LastInsertedId;

                this.CloseConnection();

                return lastInsertId;
            }
        } 
    }
}