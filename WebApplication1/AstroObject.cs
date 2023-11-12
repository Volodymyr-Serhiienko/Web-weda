using System.Text;

namespace Astro
{
    public class AstroObject
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
        public Coords Coords { get => new(new AstroTime(Datetime), Coord_correction); }
        public DateInfo DateInfo { get => new(Datetime); }
        public PlanetsInfo PlanetInfo { get => new(Datetime); }
        
        public AstroObject(DateTime datetime) => Datetime = datetime;

        public string ShowHelioInfo(PlanetInfo planetInfo)
        {
            return $"Экл. долг.: {Math.Round(planetInfo.Helio.Coord, 2)}\u00B0 - Чертог {planetInfo.Helio.Chertog.Name}, {planetInfo.Helio.Chertog.Hall} ({planetInfo.Helio.Chertog.Ratio}%),{planetInfo.Helio.Chertog.Rune}, {planetInfo.Helio.Zodiac.Name} ({planetInfo.Helio.Zodiac.Ratio}%).";
        }
        public string ShowGeoInfo(PlanetInfo planetInfo)
        {
            return $"Экл. долг.: {Math.Round(planetInfo.Geo.Coord, 2)}\u00B0 - Чертог {planetInfo.Geo.Chertog.Name}, {planetInfo.Geo.Chertog.Hall} ({planetInfo.Geo.Chertog.Ratio}%),{planetInfo.Geo.Chertog.Rune}, {planetInfo.Geo.Zodiac.Name} ({planetInfo.Geo.Zodiac.Ratio}%).";
        }
        public string ShowSpheres()
        {
			string[] spheres = new string[] { "Душевная", "Духовная", "Земная", "Божественная" };
			List<string> allSpheres = new()
            {   PlanetInfo.Sun.Helio.Chertog.Sphere, PlanetInfo.Sun.Helio.Chertog.Sphere,
                PlanetInfo.Earth.Helio.Chertog.Sphere, PlanetInfo.Earth.Helio.Chertog.Sphere,
                PlanetInfo.Moon.Helio.Chertog.Sphere, PlanetInfo.Moon.Helio.Chertog.Sphere,
                PlanetInfo.Mercury.Helio.Chertog.Sphere, PlanetInfo.Mercury.Geo.Chertog.Sphere,
                PlanetInfo.Venus.Helio.Chertog.Sphere, PlanetInfo.Venus.Geo.Chertog.Sphere,
                PlanetInfo.Mars.Helio.Chertog.Sphere, PlanetInfo.Mars.Geo.Chertog.Sphere,
                PlanetInfo.Jupiter.Helio.Chertog.Sphere, PlanetInfo.Jupiter.Geo.Chertog.Sphere,
                PlanetInfo.Saturn.Helio.Chertog.Sphere, PlanetInfo.Saturn.Geo.Chertog.Sphere,
                PlanetInfo.Uranus.Helio.Chertog.Sphere, PlanetInfo.Uranus.Geo.Chertog.Sphere,
                PlanetInfo.Neptune.Helio.Chertog.Sphere, PlanetInfo.Neptune.Geo.Chertog.Sphere,
                PlanetInfo.Pluto.Helio.Chertog.Sphere, PlanetInfo.Pluto.Geo.Chertog.Sphere
            };
            List<string> activeSpheres = allSpheres.Distinct().ToList();

            int[] number = new int[4];
            foreach (var item in allSpheres)
			{
                switch (item)
                {
                    case "Душевная": number[0]++; break;
					case "Духовная": number[1]++; break;
					case "Земная": number[2]++; break;
					case "Божественная": number[3]++; break;
				}
			}

			StringBuilder result = new();
            for (int i = 0; i < activeSpheres.Count; i++)
            {
                for (int j = 0; j < spheres.Length; j++)
                {
                    if (activeSpheres[i] == spheres[j]) result.Append($"{activeSpheres[i]} - {number[j]}, ");
				}
            }
			return result.Remove(result.Length - 2, 2).Append('.').ToString();
        }

        private readonly List<string> planets = new() { "Ярило", "Мидгард", "Месяц", "Хорс", "Мерцана", "Орей", "Перун", "Стрибог", "Варуна", "Ний", "Вий" };
		public string ShowHalls()
        {
            List<string> halls = new()
            {
                PlanetInfo.Sun.Helio.Chertog.Hall, PlanetInfo.Sun.Helio.Chertog.Hall,
                PlanetInfo.Earth.Helio.Chertog.Hall, PlanetInfo.Earth.Helio.Chertog.Hall,
                PlanetInfo.Moon.Helio.Chertog.Hall, PlanetInfo.Moon.Helio.Chertog.Hall,
                PlanetInfo.Mercury.Helio.Chertog.Hall, PlanetInfo.Mercury.Geo.Chertog.Hall,
                PlanetInfo.Venus.Helio.Chertog.Hall, PlanetInfo.Venus.Geo.Chertog.Hall,
                PlanetInfo.Mars.Helio.Chertog.Hall, PlanetInfo.Mars.Geo.Chertog.Hall,
                PlanetInfo.Jupiter.Helio.Chertog.Hall, PlanetInfo.Jupiter.Geo.Chertog.Hall,
                PlanetInfo.Saturn.Helio.Chertog.Hall, PlanetInfo.Saturn.Geo.Chertog.Hall,
                PlanetInfo.Uranus.Helio.Chertog.Hall, PlanetInfo.Uranus.Geo.Chertog.Hall,
                PlanetInfo.Neptune.Helio.Chertog.Hall, PlanetInfo.Neptune.Geo.Chertog.Hall,
                PlanetInfo.Pluto.Helio.Chertog.Hall, PlanetInfo.Pluto.Geo.Chertog.Hall
			};
            List<string> activeHalls = halls.Distinct().ToList();
			List<int> number = new();
            foreach (var str in activeHalls)
            {
                int count = 0;
                foreach (var str2 in halls)
                    if (str == str2) count++;
                number.Add(count);
            }
            
            StringBuilder planet = new();
			StringBuilder result = new();
			for (int i = 0; i < activeHalls.Count; i++)
            {
                planet.Clear();
				if (activeHalls[i] == PlanetInfo.Sun.Helio.Chertog.Hall) planet.Append($"{planets[0]}, ");
				if (activeHalls[i] == PlanetInfo.Earth.Helio.Chertog.Hall) planet.Append($"{planets[1]}, ");
				if (activeHalls[i] == PlanetInfo.Moon.Helio.Chertog.Hall) planet.Append($"{planets[2]}, ");
				if (activeHalls[i] == PlanetInfo.Mercury.Helio.Chertog.Hall) planet.Append($"{planets[3]}, ");
				if (activeHalls[i] == PlanetInfo.Mercury.Geo.Chertog.Hall && activeHalls[i] != PlanetInfo.Mercury.Helio.Chertog.Hall) planet.Append($"{planets[3]}, ");
				if (activeHalls[i] == PlanetInfo.Venus.Helio.Chertog.Hall) planet.Append($"{planets[4]}, ");
                if (activeHalls[i] == PlanetInfo.Venus.Geo.Chertog.Hall && activeHalls[i] != PlanetInfo.Venus.Helio.Chertog.Hall) planet.Append($"{planets[4]}, ");
				if (activeHalls[i] == PlanetInfo.Mars.Helio.Chertog.Hall) planet.Append($"{planets[5]}, ");
				if (activeHalls[i] == PlanetInfo.Mars.Geo.Chertog.Hall && activeHalls[i] != PlanetInfo.Mars.Helio.Chertog.Hall) planet.Append($"{planets[5]}, ");
				if (activeHalls[i] == PlanetInfo.Jupiter.Helio.Chertog.Hall) planet.Append($"{planets[6]}, ");
				if (activeHalls[i] == PlanetInfo.Jupiter.Geo.Chertog.Hall && activeHalls[i] != PlanetInfo.Jupiter.Helio.Chertog.Hall) planet.Append($"{planets[6]}, ");
				if (activeHalls[i] == PlanetInfo.Saturn.Helio.Chertog.Hall) planet.Append($"{planets[7]}, ");
				if (activeHalls[i] == PlanetInfo.Saturn.Geo.Chertog.Hall && activeHalls[i] != PlanetInfo.Saturn.Helio.Chertog.Hall) planet.Append($"{planets[7]}, ");
                if (activeHalls[i] == PlanetInfo.Uranus.Helio.Chertog.Hall) planet.Append($"{planets[8]}, ");
				if (activeHalls[i] == PlanetInfo.Uranus.Geo.Chertog.Hall && activeHalls[i] != PlanetInfo.Uranus.Helio.Chertog.Hall) planet.Append($"{planets[8]}, ");
				if (activeHalls[i] == PlanetInfo.Neptune.Helio.Chertog.Hall) planet.Append($"{planets[9]}, ");
				if (activeHalls[i] == PlanetInfo.Neptune.Geo.Chertog.Hall && activeHalls[i] != PlanetInfo.Neptune.Helio.Chertog.Hall) planet.Append($"{planets[9]}, ");
				if (activeHalls[i] == PlanetInfo.Pluto.Helio.Chertog.Hall) planet.Append($"{planets[10]}, ");
				if (activeHalls[i] == PlanetInfo.Pluto.Geo.Chertog.Hall && activeHalls[i] != PlanetInfo.Pluto.Helio.Chertog.Hall) planet.Append($"{planets[10]}, ");
				planet.Remove(planet.Length - 2, 2);
				result.Append($"{activeHalls[i]} - {number[i]} ({planet}); ");
            }
			return result.Remove(result.Length - 2, 2).Append('.').ToString();
        }
		public string ShowChertogs()
        {
			List<string> chertogs = new()
			{
				PlanetInfo.Sun.Helio.Chertog.Name, PlanetInfo.Sun.Helio.Chertog.Name,
				PlanetInfo.Earth.Helio.Chertog.Name, PlanetInfo.Earth.Helio.Chertog.Name,
				PlanetInfo.Moon.Helio.Chertog.Name, PlanetInfo.Moon.Helio.Chertog.Name,
				PlanetInfo.Mercury.Helio.Chertog.Name, PlanetInfo.Mercury.Geo.Chertog.Name,
				PlanetInfo.Venus.Helio.Chertog.Name, PlanetInfo.Venus.Geo.Chertog.Name,
				PlanetInfo.Mars.Helio.Chertog.Name, PlanetInfo.Mars.Geo.Chertog.Name,
				PlanetInfo.Jupiter.Helio.Chertog.Name, PlanetInfo.Jupiter.Geo.Chertog.Name,
				PlanetInfo.Saturn.Helio.Chertog.Name, PlanetInfo.Saturn.Geo.Chertog.Name,
				PlanetInfo.Uranus.Helio.Chertog.Name, PlanetInfo.Uranus.Geo.Chertog.Name,
				PlanetInfo.Neptune.Helio.Chertog.Name, PlanetInfo.Neptune.Geo.Chertog.Name,
				PlanetInfo.Pluto.Helio.Chertog.Name, PlanetInfo.Pluto.Geo.Chertog.Name
			};
			List<string> activeChertogs = chertogs.Distinct().ToList();
			List<int> number = new();
			foreach (var str in activeChertogs)
			{
				int count = 0;
				foreach (var str2 in chertogs)
					if (str == str2) count++;
				number.Add(count);
			}

			StringBuilder planet = new();
			StringBuilder result = new();
			for (int i = 0; i < activeChertogs.Count; i++)
			{
				planet.Clear();
				if (activeChertogs[i] == PlanetInfo.Sun.Helio.Chertog.Name) planet.Append($"{planets[0]}, ");
				if (activeChertogs[i] == PlanetInfo.Earth.Helio.Chertog.Name) planet.Append($"{planets[1]}, ");
				if (activeChertogs[i] == PlanetInfo.Moon.Helio.Chertog.Name) planet.Append($"{planets[2]}, ");
				if (activeChertogs[i] == PlanetInfo.Mercury.Helio.Chertog.Name) planet.Append($"{planets[3]}, ");
				if (activeChertogs[i] == PlanetInfo.Mercury.Geo.Chertog.Name && activeChertogs[i] != PlanetInfo.Mercury.Helio.Chertog.Name) planet.Append($"{planets[3]}, ");
				if (activeChertogs[i] == PlanetInfo.Venus.Helio.Chertog.Name) planet.Append($"{planets[4]}, ");
				if (activeChertogs[i] == PlanetInfo.Venus.Geo.Chertog.Name && activeChertogs[i] != PlanetInfo.Venus.Helio.Chertog.Name) planet.Append($"{planets[4]}, ");
				if (activeChertogs[i] == PlanetInfo.Mars.Helio.Chertog.Name) planet.Append($"{planets[5]}, ");
				if (activeChertogs[i] == PlanetInfo.Mars.Geo.Chertog.Name && activeChertogs[i] != PlanetInfo.Mars.Helio.Chertog.Name) planet.Append($"{planets[5]}, ");
				if (activeChertogs[i] == PlanetInfo.Jupiter.Helio.Chertog.Name) planet.Append($"{planets[6]}, ");
				if (activeChertogs[i] == PlanetInfo.Jupiter.Geo.Chertog.Name && activeChertogs[i] != PlanetInfo.Jupiter.Helio.Chertog.Name) planet.Append($"{planets[6]}, ");
				if (activeChertogs[i] == PlanetInfo.Saturn.Helio.Chertog.Name) planet.Append($"{planets[7]}, ");
				if (activeChertogs[i] == PlanetInfo.Saturn.Geo.Chertog.Name && activeChertogs[i] != PlanetInfo.Saturn.Helio.Chertog.Name) planet.Append($"{planets[7]}, ");
				if (activeChertogs[i] == PlanetInfo.Uranus.Helio.Chertog.Name) planet.Append($"{planets[8]}, ");
				if (activeChertogs[i] == PlanetInfo.Uranus.Geo.Chertog.Name && activeChertogs[i] != PlanetInfo.Uranus.Helio.Chertog.Name) planet.Append($"{planets[8]}, ");
				if (activeChertogs[i] == PlanetInfo.Neptune.Helio.Chertog.Name) planet.Append($"{planets[9]}, ");
				if (activeChertogs[i] == PlanetInfo.Neptune.Geo.Chertog.Name && activeChertogs[i] != PlanetInfo.Neptune.Helio.Chertog.Name) planet.Append($"{planets[9]}, ");
				if (activeChertogs[i] == PlanetInfo.Pluto.Helio.Chertog.Name) planet.Append($"{planets[10]}, ");
				if (activeChertogs[i] == PlanetInfo.Pluto.Geo.Chertog.Name && activeChertogs[i] != PlanetInfo.Pluto.Helio.Chertog.Name) planet.Append($"{planets[10]}, ");
				planet.Remove(planet.Length - 2, 2);
				result.Append($"{activeChertogs[i]} - {number[i]} ({planet}); ");
			}
			return result.Remove(result.Length - 2, 2).Append('.').ToString();
		}
		public string ShowZodiacs()
		{
			List<string> zodiacs = new()
			{
				PlanetInfo.Sun.Helio.Zodiac.Name, PlanetInfo.Sun.Helio.Zodiac.Name,
				PlanetInfo.Earth.Helio.Zodiac.Name, PlanetInfo.Earth.Helio.Zodiac.Name,
				PlanetInfo.Moon.Helio.Zodiac.Name, PlanetInfo.Moon.Helio.Zodiac.Name,
				PlanetInfo.Mercury.Helio.Zodiac.Name, PlanetInfo.Mercury.Geo.Zodiac.Name,
				PlanetInfo.Venus.Helio.Zodiac.Name, PlanetInfo.Venus.Geo.Zodiac.Name,
				PlanetInfo.Mars.Helio.Zodiac.Name, PlanetInfo.Mars.Geo.Zodiac.Name,
				PlanetInfo.Jupiter.Helio.Zodiac.Name, PlanetInfo.Jupiter.Geo.Zodiac.Name,
				PlanetInfo.Saturn.Helio.Zodiac.Name, PlanetInfo.Saturn.Geo.Zodiac.Name,
				PlanetInfo.Uranus.Helio.Zodiac.Name, PlanetInfo.Uranus.Geo.Zodiac.Name,
				PlanetInfo.Neptune.Helio.Zodiac.Name, PlanetInfo.Neptune.Geo.Zodiac.Name,
				PlanetInfo.Pluto.Helio.Zodiac.Name, PlanetInfo.Pluto.Geo.Zodiac.Name
			};
			List<string> activeZodiacs = zodiacs.Distinct().ToList();
			List<int> number = new();
			foreach (var str in activeZodiacs)
			{
				int count = 0;
				foreach (var str2 in zodiacs)
					if (str == str2) count++;
				number.Add(count);
			}

			StringBuilder planet = new();
			StringBuilder result = new();
			for (int i = 0; i < activeZodiacs.Count; i++)
			{
				planet.Clear();
				if (activeZodiacs[i] == PlanetInfo.Sun.Helio.Zodiac.Name) planet.Append($"{planets[0]}, ");
				if (activeZodiacs[i] == PlanetInfo.Earth.Helio.Zodiac.Name) planet.Append($"{planets[1]}, ");
				if (activeZodiacs[i] == PlanetInfo.Moon.Helio.Zodiac.Name) planet.Append($"{planets[2]}, ");
				if (activeZodiacs[i] == PlanetInfo.Mercury.Helio.Zodiac.Name) planet.Append($"{planets[3]}, ");
				if (activeZodiacs[i] == PlanetInfo.Mercury.Geo.Zodiac.Name && activeZodiacs[i] != PlanetInfo.Mercury.Helio.Zodiac.Name) planet.Append($"{planets[3]}, ");
				if (activeZodiacs[i] == PlanetInfo.Venus.Helio.Zodiac.Name) planet.Append($"{planets[4]}, ");
				if (activeZodiacs[i] == PlanetInfo.Venus.Geo.Zodiac.Name && activeZodiacs[i] != PlanetInfo.Venus.Helio.Zodiac.Name) planet.Append($"{planets[4]}, ");
				if (activeZodiacs[i] == PlanetInfo.Mars.Helio.Zodiac.Name) planet.Append($"{planets[5]}, ");
				if (activeZodiacs[i] == PlanetInfo.Mars.Geo.Zodiac.Name && activeZodiacs[i] != PlanetInfo.Mars.Helio.Zodiac.Name) planet.Append($"{planets[5]}, ");
				if (activeZodiacs[i] == PlanetInfo.Jupiter.Helio.Zodiac.Name) planet.Append($"{planets[6]}, ");
				if (activeZodiacs[i] == PlanetInfo.Jupiter.Geo.Zodiac.Name && activeZodiacs[i] != PlanetInfo.Jupiter.Helio.Zodiac.Name) planet.Append($"{planets[6]}, ");
				if (activeZodiacs[i] == PlanetInfo.Saturn.Helio.Zodiac.Name) planet.Append($"{planets[7]}, ");
				if (activeZodiacs[i] == PlanetInfo.Saturn.Geo.Zodiac.Name && activeZodiacs[i] != PlanetInfo.Saturn.Helio.Zodiac.Name) planet.Append($"{planets[7]}, ");
				if (activeZodiacs[i] == PlanetInfo.Uranus.Helio.Zodiac.Name) planet.Append($"{planets[8]}, ");
				if (activeZodiacs[i] == PlanetInfo.Uranus.Geo.Zodiac.Name && activeZodiacs[i] != PlanetInfo.Uranus.Helio.Zodiac.Name) planet.Append($"{planets[8]}, ");
				if (activeZodiacs[i] == PlanetInfo.Neptune.Helio.Zodiac.Name) planet.Append($"{planets[9]}, ");
				if (activeZodiacs[i] == PlanetInfo.Neptune.Geo.Zodiac.Name && activeZodiacs[i] != PlanetInfo.Neptune.Helio.Zodiac.Name) planet.Append($"{planets[9]}, ");
				if (activeZodiacs[i] == PlanetInfo.Pluto.Helio.Zodiac.Name) planet.Append($"{planets[10]}, ");
				if (activeZodiacs[i] == PlanetInfo.Pluto.Geo.Zodiac.Name && activeZodiacs[i] != PlanetInfo.Pluto.Helio.Zodiac.Name) planet.Append($"{planets[10]}, ");
				planet.Remove(planet.Length - 2, 2);
				result.Append($"{activeZodiacs[i]} - {number[i]} ({planet}); ");
			}
			return result.Remove(result.Length - 2, 2).Append('.').ToString();
		}
        public (int, int) ShowPosition(double coord)
        {
            int x = (int)(221 + 183 * Math.Cos((-coord + 65.5) * Math.PI / 180));
            int y = (int)(223 + 183 * Math.Sin((-coord + 65.5) * Math.PI / 180));
            return (x, y);
		}
    }
}