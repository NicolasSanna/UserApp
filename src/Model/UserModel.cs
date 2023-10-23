// Espace de nom correspondant à l'application et au dossier src/Model
namespace App.src.Model
{
    // Classe UserModel qui étend de Database
    public class UserModel : AbstractModel
    {
        // Propriétés
        private int Id = 0;
        private string Firstname = "";
        private string Lastname = "";
        private int Age = 0;

        // Getters
        public int GetId()
        {
            return this.Id;
        }

        public string GetFirstname()
        {
            return this.Firstname;
        }

        public string GetLastname()
        {
            return this.Lastname;
        }

        public int GetAge()
        {
            return this.Age;
        }

        // Setters
        public void SetId(int value)
        {
            this.Id = value;
        }

        public void SetFirstname(string value)
        {
            this.Firstname = value;
        }

        public void SetLastname(string value)
        {
            this.Lastname = value;
        }

        public void SetAge(int val)
        {
            this.Age = val;
        }

        public string GetFullName()
        {
            return _ = $"Je m'appelle {this.GetFirstname()} {this.GetLastname()} et j'ai {this.GetAge()} ans";
        }

        public string CheckAge()
        {
            string message = this.GetAge() >= 18 ? "Vous êtes majeur" : "Vous êtes mineur";

            return message;
        }

        // Ajout d'un utilisateur à la base de données
        public void Insert(string firstname, string lastname, int age)
        {
            string sql = @" INSERT INTO user (firstname, lastname, age, created_at) 
                            VALUES (@firstname, @lastname, @age, NOW())";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@firstname", firstname },
                { "@lastname", lastname },
                { "@age", age }
            };

            this.Database.ExecuteQuery(sql, parameters);
        }

        // Récupération de la liste des utilisateurs
        public List<Dictionary<string, object>> Index()
        {
            string sql = @" SELECT id, firstname, lastname, age, DATE_FORMAT(created_at,'Le %d/%m/%Y à %H:%i') AS created_at 
                            FROM user";

            List<Dictionary<string, object>> results = this.Database.GetAllResults(sql);

            return results;
        }

        // Récupération d'un utilisateur
        public Dictionary<string, object> Show(string id)
        {
            string sql = @" SELECT id, firstname, lastname, age, DATE_FORMAT(created_at,'Le %d/%m/%Y à %H:%i') AS created_at 
                            FROM user 
                            WHERE id = @id";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@id", id}
            };

            Dictionary<string, object> result = this.Database.GetOneResult(sql, parameters);

            return result;
        }

        // Suppression d'un utilisateur
        public void Delete(string id)
        {
            string sql = @" DELETE 
                            FROM user 
                            WHERE id = @id";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@id", id}
            };

            this.Database.ExecuteQuery(sql, parameters);
        }

        // Modification d'un utilisateur dans la base de données
        public void Update(string firstname, string lastname, int age, int id)
        {
            string sql = @" UPDATE user 
                            SET 
                            firstname = @firstname, 
                            lastname = @lastname, 
                            age = @age 
                            WHERE id = @id";

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@firstname", firstname },
                { "@lastname", lastname },
                { "@age", age },
                { "@id", id }
            };

            this.Database.ExecuteQuery(sql, parameters);
        }
    }
}