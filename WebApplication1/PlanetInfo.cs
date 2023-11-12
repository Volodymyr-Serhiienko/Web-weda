namespace Astro
{
    public class Zodiac
    {
        public string Name { get; protected set; }
        public double Ratio { get; protected set; }

        public Zodiac(double Coord)
        {
            var tuple = SlavAstronomy.ZodiacPos(Coord);
            Name = tuple.Item1;
            Ratio = tuple.Item2;
        }
    }
    public class Chertog : Zodiac
    {
        public string Hall { get; }
        public string Rune { get; }
		public string Sphere { get; }

		public Chertog(double Coord)
            :base(Coord)
        {
            var tuple = SlavAstronomy.ChertogPos(Coord);
            Name = tuple.Item1;
            Hall = tuple.Item2;
            Ratio = tuple.Item3;
            Rune = tuple.Item4;
            Sphere = tuple.Item5;
        }
    }
    
    public class PlanetPosition
    {
        public Chertog Chertog { get => new(Coord); }
        public Zodiac Zodiac { get => new(Coord); }
        public double Coord { get; }

        public PlanetPosition(double coord)
        {
            this.Coord = coord;
        }
    }

    public class PlanetInfo
    {
        public PlanetPosition Helio { get; private set; }
        public PlanetPosition Geo { get; private set; }

        public PlanetInfo(double helioCoord)
        {
            Helio = new PlanetPosition(helioCoord);
            Geo = new PlanetPosition(helioCoord);
        }
        public PlanetInfo(double helioCoord, double geoCoord)
        {
            Helio = new PlanetPosition(helioCoord);
            Geo = new PlanetPosition(geoCoord);
        }
    }
}