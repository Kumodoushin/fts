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

	public override int GetHashCode() => Id.GetHashCode();

	public override bool Equals(object? obj) => 
		obj is not null && obj is Flight otherFlight && this == otherFlight;

	public static bool operator ==(Flight a, Flight b) => a.Id == b.Id;
	public static bool operator !=(Flight a, Flight b) => !(a == b);
}