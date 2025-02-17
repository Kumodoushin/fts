// See https://aka.ms/new-console-template for more information
using FTS.Domain;
using Newtonsoft.Json;

var dir = new DirectoryInfo("icaoData");
if (!dir.Exists)
{
	Console.WriteLine("No data to process - missing icaoData directory");
}

var files = dir.EnumerateFiles("*.json");
Console.WriteLine($"Found {files.Count()} airports");
var airports = new List<Airport>();
foreach (var file in files)
{
	var airportData = JsonConvert.DeserializeObject<AirportJson>(file.OpenText().ReadToEnd());
	if (airportData.Iata_Code.Length != 3)
	{
		continue;
	}
	airports.Add(new()
	{
		IATA = airportData.Iata_Code, 
		ICAO = airportData.Ident,
		Name = airportData.Name,
		Country = airportData.Country.Name,
		CountryISO = airportData.Iso_Country,
		Continent = Continent.FromValue(airportData.Continent),
		Latitude = airportData.Latitude_deg,
		Longitude = airportData.Longitude_deg,
	});
}
Console.WriteLine($"Processed {airports.Count} airports");

File.WriteAllText("AllAirports.json", JsonConvert.SerializeObject(airports));
//OC, NA, EU, AF, SA, AS
Console.WriteLine("done");
Console.ReadLine();

public class AirportJson
{
	public string Ident { get; set; }
	public string Name { get; set; }
	public string Continent { get; set; }
	public string Iata_Code { get; set; }
	public string Iso_Country { get; set; }
	public CountryT Country { get; set; }
	
	public double Latitude_deg { get; set; }
	public double Longitude_deg { get; set; }

	public class CountryT
	{
		public string Name { get; set; }
	}
}