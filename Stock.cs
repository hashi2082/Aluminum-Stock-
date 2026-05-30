using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AluminumStockManager.Models
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string ItemName { get; set; } = "";
        public string Category { get; set; } = "";
        public double Weight { get; set; }
        public int Quantity { get; set; }
        public double PricePerKg { get; set; }
        public string Supplier { get; set; } = "";
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Available";
    }
}