namespace App.src.Services
{   
    class DotEnv
    {
        private static readonly string AppDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
        public static void EnvLoad()
        {
            string[] lines = File.ReadAllLines(DotEnv.AppDirectory);

            foreach (string line in lines)
            {
                string[] parties = line.Split('=');

                string nomVariable = parties[0].Trim();
                string valeurVariable = parties[1].Trim();
                Environment.SetEnvironmentVariable(nomVariable, valeurVariable);
            }
        }
    }
}