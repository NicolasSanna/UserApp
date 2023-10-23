namespace App.src.Model
{
    public abstract class AbstractModel
    {
        protected Database Database;

        protected AbstractModel()
        {
            this.Database = Database.GetInstance();
        }
    }
}