namespace Astro
{
    public class Position
    {
        public double Helio { get; }
        public double Geo { get; }

        public Position(double helio, double geo)
        {
            Helio = helio;
            Geo = geo;
        }
    }

    public class Coords
    {
        private AstroTime Astrotime { get; set; }
        private double Coord_correction { get; set; }

        public Coords(AstroTime astrotime, double coord_correction)
        {
            Astrotime = astrotime;
            Coord_correction = coord_correction;
        }

        public double Sun { get => Normalize(Astronomy.SunPosition(Astrotime).elon + Coord_correction); }
        public double SunTrop { get => Normalize(Astronomy.SunPosition(Astrotime).elon); }
        public double Earth { get => Sun + 180 < 360 ? Sun + 180 : Sun - 180; }
        public double Moon { get => Geo(Body.Moon); }
        public Position Mercury { get => new(Helio(Body.Mercury), Geo(Body.Mercury)); }
        public Position Venus { get => new(Helio(Body.Venus), Geo(Body.Venus)); }
        public Position Mars { get => new(Helio(Body.Mars), Geo(Body.Mars)); }
        public Position Jupiter { get => new(Helio(Body.Jupiter), Geo(Body.Jupiter)); }
        public Position Saturn { get => new(Helio(Body.Saturn), Geo(Body.Saturn)); }
        public Position Uranus { get => new(Helio(Body.Uranus), Geo(Body.Uranus)); }
        public Position Neptune { get => new(Helio(Body.Neptune), Geo(Body.Neptune)); }
        public Position Pluto { get => new(Helio(Body.Pluto), Geo(Body.Pluto)); }
        
        private double Helio(Body body) => Normalize(Astronomy.EclipticLongitude(body, Astrotime) + Coord_correction);
        private double Geo(Body body) => Normalize(Astronomy.EquatorialToEcliptic(Astronomy.GeoVector(body, Astrotime, Aberration.None)).elon + Coord_correction);
        private static double Normalize(double coord)
        {
            if (coord < 0) { coord += 360; }
            if (coord > 360) { coord -= 360; }
            return coord;
        }
    }
}