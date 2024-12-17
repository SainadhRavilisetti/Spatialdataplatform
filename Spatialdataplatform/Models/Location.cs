using NetTopologySuite.Geometries;

namespace Spatialdataplatform.Models
{
    public class Location
    {
        public int Id { get; set; }

        // Spatial column for storing Point data
        public Point? Coordinate { get; set; }  // Make it nullable if it can be null

    }
}
