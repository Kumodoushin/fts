using FTS.Application.AddFlightDefinition;
using FTS.Domain;
using Newtonsoft.Json;

namespace FTS.Application.Tests;

public class BuyFlightByIdTests
{
	[Fact]
	public void BuyingTicketTests()
	{
		var flightDefinitionsDictionary = new Dictionary<FlightId, Flight>();
		
		var airports = LoadAirports();
		//Add a Warsaw-Bou Saada Airport in Algeria (Africa) flight definition that happens on Mondays and Thursdays 18:00
		var flightId = new FlightId("LOT 12345 BCA");
		
		var departureAirport = airports.GetByIATA("WAW").AsT0;
		var destinationAirport = airports.GetByIATA("BUJ").AsT0;

		var departureTime = TimeOnly.FromTimeSpan(
			new TimeSpan(
				18,
				0,
				0));

		var departureDays = new List<DayOfWeek>()
		{
			DayOfWeek.Monday,
			DayOfWeek.Thursday,
		};

		var verySpecificFlightDefinition = new Flight(
			flightId,
			departureAirport,
			destinationAirport,
			departureTime,
			departureDays);

		var flightDefinitions = new AddFlightDefinitionHandler(flightDefinitionsDictionary);
		flightDefinitions.Handle(verySpecificFlightDefinition);
		
		var currentTime = new DateTimeOffset(DateTime.Now);
		var nextFlightInstances = new NextFlightInstances(flightDefinitionsDictionary, currentTime);
		
		var flightInstanceSelection = nextFlightInstances.Handle(flightId,10);

		var pickedFlight = flightInstanceSelection.First(x => x.DepartureDate.DayOfWeek == DayOfWeek.Thursday);

		var reservationModule = new ReserveFlightForTenant(flightDefinitionsDictionary);

		var tenant = new Tenant.A(
			DateOnly.FromDateTime(currentTime.UtcDateTime),
			Guid.NewGuid(),
			new DateOnly(
				1980,
				1,
				1),
			new (),
			new(),
			new());
		var reservedFlight = reservationModule.Handle(tenant, pickedFlight).AsT0;

		var priceCatalog = new Dictionary<FlightId, Price>();
		priceCatalog.Add(pickedFlight.FlightId,new Price(26));
		var priceBuilder = new CalculatePriceForTenantsReservation(priceCatalog, DateOnly.FromDateTime(currentTime.DateTime));
		
		var pricedFlight = priceBuilder.Handle(tenant, reservedFlight);

		var dealMaker = new FinalizeBuyForTenant();
		dealMaker.Handle(tenant,pricedFlight);

	}

	private static Airports LoadAirports() =>
		new Airports(
			JsonConvert.DeserializeObject<List<Airport>>(
				File.OpenText("AllAirports.json").ReadToEnd(),
				new ContinentConverter()));
}