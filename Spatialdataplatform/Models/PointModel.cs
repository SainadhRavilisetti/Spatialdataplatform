namespace Spatialdataplatform.Models
{
    public class PointModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }  // Store GeoJSON representation
    }
}
