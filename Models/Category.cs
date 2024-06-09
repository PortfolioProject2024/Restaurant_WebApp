namespace Restaurant_WebApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<FoodItem> FoodItems { get; set; }
    }
}
