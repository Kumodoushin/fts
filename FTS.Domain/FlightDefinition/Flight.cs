namespace FTS.Domain;

public class Flight
{
	public Flight(
		FlightId id, 
		Airport departureAirport,
		Airport destinationAirport,
		TimeOnly departure,
		List<DayOfWeek> departureDays)
	{
		Id = id;
		_departureAirport = departureAirport;
		_destinationAirport = destinationAirport;
		_departure = departure;
		_departureDays = departureDays;
	}
	public FlightId Id { get; }
	private Airport _departureAirport;
	private Airport _destinationAirport;
	private TimeOnly _departure;
	private List<DayOfWeek> _departureDays;

	public override string ToString() => $"{Id} from {_departureAirport} to {_destinationAirport} leaving at {_departure.ToShortTimeString()} local time on {string.Join(", ",_departureDays.Select(x=>x.ToString()))}";
}