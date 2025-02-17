namespace FTS.Domain;

internal static class FlightInstanceExtensions
{
	internal static bool DepartsOnThursday(this Flight.Instance flightReservation) =>
		flightReservation.DepartureDate.DayOfWeek == DayOfWeek.Thursday;

	internal static bool FliesToAfrica(this Flight.Instance flightReservation) =>
		flightReservation.DestinationAirport.Continent == Continent.Africa;
}