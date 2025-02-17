using FTS.Application.AddFlightDefinition;
using FTS.Domain;
using Newtonsoft.Json;

namespace FTS.Application.Tests;

public class FlightDefinitionTests
{
	
	[Fact]
	public void ValidDataCreatesAFlightDefinition()
	{
		var flightDefinitionsDictionary = new Dictionary<FlightId, Flight>();
		var airports = new Airports(
			JsonConvert.DeserializeObject<List<Airport>>(
				File.OpenText("AllAirports.json").ReadToEnd(),
			new ContinentConverter()));
		
		var flight = new Flight(
			new ("KLM 12346 BCA"),
			airports.GetByIATA("JFK").AsT0,
			airports.GetByIATA("WAW").AsT0,
			TimeOnly.FromTimeSpan(
				new TimeSpan(
					18,
					0,
					0)),
			new()
			{
				DayOfWeek.Monday,
				DayOfWeek.Wednesday,
				DayOfWeek.Friday
			});
		
		var flightDefinitions = new AddFlightDefinitionHandler(flightDefinitionsDictionary);
		var result1 = flightDefinitions.Handle(flight);
		Assert.True(result1.IsT0);
		Assert.True(result1.AsT0);
		Assert.True(flightDefinitionsDictionary.ContainsKey(new FlightId("KLM 12346 BCA")));
		
		
	}
}