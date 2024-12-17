namespace Spatialdataplatform.Models
{
    public class PolygonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Boundary { get; set; }  // Store GeoJSON representation
    }
}
