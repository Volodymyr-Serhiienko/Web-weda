namespace Astro
{
    public class PlanetsInfo
    {
        private DateTime Datetime { get; }
        private double Coord_correction
        {
            get
            {
                double result;
                DateTime millenium = new(2011, 9, 21);
                TimeSpan timeSpan;
                double day_correction = 22.5 / (1620 * 365.25);
                if (Datetime < millenium)
                {
                    timeSpan = millenium - Datetime;
                    result = timeSpan.Days * day_correction;
                }
                else
                {
                    timeSpan = Datetime - millenium;
                    result = -(timeSpan.Days * day_correction);
                }
                return result;
            }
        }
        private Coords Coords { get => new(new AstroTime(Datetime), Coord_correction); }

        public PlanetsInfo(DateTime datetime) => Datetime = datetime;

        public PlanetInfo Sun { get => new(Coords.Sun); }
        public PlanetInfo Earth { get => new(Coords.Earth); }
        public PlanetInfo Moon { get => new(Coords.Moon); }
        public PlanetInfo Mercury { get => new(Coords.Mercury.Helio, Coords.Mercury.Geo); }
        public PlanetInfo Venus { get => new(Coords.Venus.Helio, Coords.Venus.Geo); }
        public PlanetInfo Mars { get => new(Coords.Mars.Helio, Coords.Mars.Geo); }
        public PlanetInfo Jupiter { get => new(Coords.Jupiter.Helio, Coords.Jupiter.Geo); }
        public PlanetInfo Saturn { get => new(Coords.Saturn.Helio, Coords.Saturn.Geo); }
        public PlanetInfo Uranus { get => new(Coords.Uranus.Helio, Coords.Uranus.Geo); }
        public PlanetInfo Neptune { get => new(Coords.Neptune.Helio, Coords.Neptune.Geo); }
        public PlanetInfo Pluto { get => new(Coords.Pluto.Helio, Coords.Pluto.Geo); }
    }
}