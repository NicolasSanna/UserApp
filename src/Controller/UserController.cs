using System.Text;
using App.src.Model;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using Document = iText.Layout.Document;

// Espace de nom correspondant à l'application et au dossier src/Controller
namespace App.src.Controller
{
    public class UserController
    {
        public static void Start()
        {
            string[] optionNames =
            {
                "Ajouter un utilisateur",
                "Mettre à jour un utilisateur",
                "Supprimer un utilisateur",
                "Voir la liste des utilisateurs",
                "Voir un utilisateur",
                "Revenir au programme principal",
                "Imprimer les données dans un fichier .txt",
                "Exporter les données dans un fichier .csv",
                "Exporter les données dans un fichier .pdf"
            };

            int currentOption = 1;
            int totalOptions = optionNames.Length;

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(new string('#', 25));
                Console.WriteLine("# PROGRAMME UTILISATEUR #");
                Console.WriteLine(new string('#', 25));
                Console.ForegroundColor = ConsoleColor.Gray;

                for (int i = 1; i <= totalOptions; i++)
                {
                    if (i == currentOption)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    string currentOptionName = optionNames[i - 1];

                    Console.WriteLine($"{i}. {currentOptionName}");

                    Console.ResetColor();
                }

                Console.WriteLine("Choisissez une opération :");

                ConsoleKeyInfo keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        currentOption = Math.Max(1, currentOption - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        currentOption = Math.Min(totalOptions, currentOption + 1);
                        break;

                    case ConsoleKey.Enter:

                        switch (currentOption)
                        {
                            case 1:
                                UserController.New();
                                break;

                            case 2:
                                UserController.Update();
                                break;

                            case 3:
                                UserController.Delete();
                                break;

                            case 4:
                                UserController.Index();
                                break;

                            case 5:
                                UserController.Show();
                                break;

                            case 6:
                                return;

                            case 7:
                                UserController.Print();
                                break;

                            case 8:
                                UserController.ExportToCsv();
                                break;

                            case 9:
                                UserController.ExportToPdf();
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private static void New()
        {
            string firstname = "";
            string lastname = "";

            while (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
            {
                Console.WriteLine("Quel est votre prénom ?");
                firstname = Console.ReadLine()?.Trim() ?? "";

                Console.WriteLine("Quel est votre nom ?");
                lastname = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
                {
                    Console.WriteLine("Le prénom et le nom sont obligatoires. Veuillez réessayer.");
                }
            }

            int age = 0;
            bool isAgeValid = false;

            while (!isAgeValid || age == 0)
            {
                Console.WriteLine("Quel est votre âge ? ");
                string ageInput = Console.ReadLine()?.Trim() ?? "";

                if (!string.IsNullOrEmpty(ageInput) && ageInput.All(char.IsDigit))
                {
                    age = Convert.ToInt32(ageInput);
                    isAgeValid = true;
                }
                else
                {
                    Console.WriteLine("Veuillez entrer un âge valide différent de zéro.");
                }
            }

            Console.WriteLine("Voulez-vous vraiment créer un nouvel utilisateur ? (oui/non)");
            string response = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (response == "non")
            {
                Console.WriteLine("Opération annulée.");
                return;
            }

            UserModel userModel = new UserModel();
            userModel.Insert(firstname, lastname, age);

            Console.WriteLine("Utilisateur créé avec succès !");

            UserController.Index();
        }

        private static void Index()
        {
            UserModel userModel = new UserModel();
            List<Dictionary<string, object>> results = userModel.Index();

            if (results.Count == 0)
            {
                Console.WriteLine("Aucun résultat enregistré");
            }
            else
            {
                // En-tête
                Console.WriteLine("{0,-4} | {1,-16} | {2,-20} | {3}", "ID", "Prénom", "Nom", "Âge");
                Console.WriteLine(new string('-', 54));

                foreach (var result in results)
                {
                    int id = (int) result["id"];
                    string firstname = (string) result["firstname"];
                    string lastname = (string) result["lastname"];
                    int age = (int) result["age"];

                    // L'affichage est organisé : 0 = colonne id
                    // après la virgule, le - signifie alignement à gauche, sinon il n'y aurait pas ce -.
                    // Juste ensuite, le 4 par exemple signifie qu'il y a un espace de 4 caractères vides afin de simuler le caractère en colonne.
                    // Puis vient le pipe | qui permet d'indiquer la séparation avec la colonne suivante.
                    Console.WriteLine("{0,-4} | {1,-16} | {2,-20} | {3}", id, firstname, lastname, age);
                }
            }

            Console.ReadKey();
        }

        private static void Show()
        {
            UserController.Index();

            Console.WriteLine("Choisissez le numéro d'identifiant");
            string Id = Console.ReadLine() ?? "";

            UserModel userModel = new UserModel();
            Dictionary<string, object> data = userModel.Show(Id);

            if (data != null)
            {
                int id = (int) data["id"];
                string firstname = (string) data["firstname"];
                string lastname = (string) data["lastname"];
                int age = (int) data["age"];
                string createdAt = (string) data["created_at"];

                Console.WriteLine($"ID: {id}, Prénom: {firstname}, Nom: {lastname}, Âge: {age}, Créé le : {createdAt}");
            }
            else
            {
                Console.WriteLine("Aucun utilisateur trouvé avec cet identifiant");
            }

            Console.ReadKey();
        }

        private static void Delete()
        {
            UserController.Index();

            Console.WriteLine("Choisissez le numéro afin de supprimer l'utilisateur");
            string id = Console.ReadLine() ?? "";

            Console.WriteLine("Voulez-vous vraiment supprimer un utilisateur ? (oui/non)");
            string response = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (response == "non" || string.IsNullOrWhiteSpace(response))
            {
                return;
            }

            UserModel userModel = new UserModel();
            userModel.Delete(id);

            Console.WriteLine($"Utilisateur {id} supprimé");

            UserController.Index();
        }

        private static void Update()
        {
            UserController.Index();

            Console.WriteLine("Quel est le numéro d'identifiant correspondant à votre modification ?");

            int id = 0;
            bool idIsValid = false;

            while (!idIsValid || id == 0)
            {
                Console.WriteLine("Entrez le numéro d'identifier correspondant à l'utilisateur à modifier ");
                string idInput = Console.ReadLine()?.Trim() ?? "";

                if (!string.IsNullOrEmpty(idInput) && idInput.All(char.IsDigit))
                {
                    id = Convert.ToInt32(idInput);
                    idIsValid = true;
                }
                else
                {
                    Console.WriteLine("Veuillez entrer un numéro valide différent de zéro.");
                }
            }

            string firstname = "";
            string lastname = "";

            while (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
            {
                Console.WriteLine("Quel est votre nouveau prénom ?");
                firstname = Console.ReadLine()?.Trim() ?? "";

                Console.WriteLine("Quel être votre nouveau nom ?");
                lastname = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname))
                {
                    Console.WriteLine("Le prénom et le nom sont obligatoires. Veuillez réessayer.");
                }
            }

            int age = 0;
            bool isAgeValid = false;

            while (!isAgeValid || age == 0)
            {
                Console.WriteLine("Quel est votre âge ? ");
                string ageInput = Console.ReadLine()?.Trim() ?? "";

                if (!string.IsNullOrEmpty(ageInput) && ageInput.All(char.IsDigit))
                {
                    age = Convert.ToInt32(ageInput);
                    isAgeValid = true;
                }
                else
                {
                    Console.WriteLine("Veuillez entrer un âge valide différent de zéro.");
                }
            }

            Console.WriteLine("Voulez-vous vraiment modifier cet utilisateur ? (oui/non)");
            string response = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (response == "non")
            {
                Console.WriteLine("Opération annulée.");
                return;
            }

            UserModel UserModel = new UserModel();
            UserModel.Update(firstname, lastname, age, id);

            Console.WriteLine($"Utilisateur {id} modifié avec succès !");

            UserController.Index();
        }

        private static void Print()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string directoryPath = Path.Combine(myDocumentsPath, "datas"); // Créer un chemin vers le répertoire "datas"

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath); // Créer le répertoire s'il n'existe pas
            }

            string filePath = Path.Combine(directoryPath, "data.txt"); // Mettre à jour le chemin du fichier

            UserModel userModel = new UserModel();
            List<Dictionary<string, object>> results = userModel.Index();

            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var result in results)
                {
                    sw.WriteLine($"ID: {result["id"]}, Prénom: {result["firstname"]}, Nom: {result["lastname"]}, Âge: {result["age"]}, Créé le : {result["created_at"]}");
                }
            }
        }

        private static void ExportToCsv()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string directoryPath = Path.Combine(myDocumentsPath, "datas"); // Créer un chemin vers le répertoire "datas"

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath); // Créer le répertoire s'il n'existe pas
            }

            string filePath = Path.Combine(directoryPath, "data.csv"); // Mettre à jour le chemin du fichier

            UserModel userModel = new UserModel();
            List<Dictionary<string, object>> results = userModel.Index();

            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Écrire l'en-tête du fichier CSV
                sw.WriteLine("ID;Prénom;Nom;Âge;Créé le");

                foreach (var result in results)
                {
                    // Construire la ligne CSV
                    string line = $"{result["id"]};{result["firstname"]};{result["lastname"]};{result["age"]};{result["created_at"]}";

                    // Écrire la ligne dans le fichier
                    sw.WriteLine(line);
                }
            }
        }

        private static void ExportToPdf()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string directoryPath = Path.Combine(myDocumentsPath, "datas"); // Créer un chemin vers le répertoire "datas"

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath); // Créer le répertoire s'il n'existe pas
            }

            string pdfFilePath = Path.Combine(directoryPath, "data.pdf"); // Chemin pour le fichier PDF

            // Obtenir les données à partir de UserModel
            UserModel userModel = new UserModel();
            List<Dictionary<string, object>> results = userModel.Index();

            // Utiliser FileStream pour écrire le PDF
            using (FileStream fs = new FileStream(pdfFilePath, FileMode.Create))
            {
                PdfWriter writer = new PdfWriter(fs);
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    Document doc = new Document(pdf);

                    // Titre centré
                    Paragraph title = new Paragraph("Informations")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER) // Aligner le texte au centre
                        .SetFontSize(20); // Définir la taille de la police
                    doc.Add(title); // Ajouter le titre au document PDF

                    Paragraph infos = new Paragraph("Bonjour, voici les informations utilisateurs que vous avez demandé :")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.LEFT)
                        .SetFontSize(12);
                    doc.Add(infos);

                    // Ajouter un tableau avec les données
                    Table table = new Table(5); // Créer un tableau avec 5 colonnes

                    // Ajouter les en-têtes du tableau
                    string[] headers = { "ID", "Prénom", "Nom", "Âge", "Créé le" };
                    foreach (var header in headers)
                    {
                        Cell cell = new Cell().Add(new Paragraph(header).SetBold()); // Créer une cellule avec le texte en gras
                        table.AddCell(cell); // Ajouter la cellule au tableau
                    }

                    // Ajouter les données
                    foreach (var result in results)
                    {
                        table.AddCell(result["id"].ToString());
                        table.AddCell(result["firstname"].ToString());
                        table.AddCell(result["lastname"].ToString());
                        table.AddCell(result["age"].ToString());
                        table.AddCell(result["created_at"].ToString());
                    }

                    // Ajouter un bloc pour centrer le tableau
                    Paragraph centeredTableBlock = new Paragraph();
                    centeredTableBlock.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    centeredTableBlock.Add(new Paragraph().Add(table)); // Ajouter le tableau au bloc centré
                    doc.Add(centeredTableBlock);

                    // Ajouter la ligne "Signature"
                    Paragraph signature = new Paragraph("Signature: ______________________")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT); // Aligner le texte à droite
                    doc.Add(signature); // Ajouter la ligne de signature au document PDF
                }
            }
        }
    }
}