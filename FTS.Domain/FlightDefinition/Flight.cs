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
	protected readonly Airport _departureAirport;
	protected readonly Airport _destinationAirport;
	protected readonly TimeOnly _departure;
	protected readonly List<DayOfWeek> _departureDays;



	public override string ToString() => $"{Id} from {_departureAirport} to {_destinationAirport} leaving at {_departure.ToShortTimeString()} local time on {string.Join(", ",_departureDays.Select(x=>x.ToString()))}";

	public override int GetHashCode() => Id.GetHashCode();

	public override bool Equals(object? obj) => 
		obj is not null && obj is Flight otherFlight && this == otherFlight;

	public static bool operator ==(Flight a, Flight b) => a.Id == b.Id;
	public static bool operator !=(Flight a, Flight b) => !(a == b);

	public IEnumerable<Instance> NextFlightInstances(int amount, DateTimeOffset onOrAfter)
	{
		var currentDate = DateOnly.FromDateTime(onOrAfter.LocalDateTime);
		for (int i = 0; i < amount; i++)
		{
			while (!_departureDays.Contains(currentDate.DayOfWeek))
			{
				currentDate = currentDate.AddDays(1);
			}

			var flightInstance = new Instance(
				Id,
				_departureAirport,
				new DateTimeOffset(
					currentDate,
					_departure,
					onOrAfter.Offset),
				_destinationAirport);
			yield return flightInstance;
		}
		
	}
	public class Instance
	{
		public (FlightId, DateTimeOffset) InstanceId => (FlightId, DepartureDate);
		public FlightId FlightId { get; }

		public Airport DepartureAirport { get; }

		public DateTimeOffset DepartureDate { get; }
		public Airport DestinationAirport { get; }

		public Instance(
			FlightId flightId, 
			Airport departureAirport, 
			DateTimeOffset departureDate, 
			Airport destinationAirport)
		{
			
			FlightId = flightId;
			DepartureAirport = departureAirport;
			DepartureDate = departureDate;
			DestinationAirport = destinationAirport;
		}

		public PricedInstance WithPrice(Price basePrice)
		{
			return new PricedInstance(
				FlightId,
				DepartureAirport,
				DepartureDate,
				DestinationAirport,
				basePrice);
		}
	}

	public class PricedInstance
	{
		public (FlightId, DateTimeOffset) InstanceId => (FlightId, DepartureDate);

		public FlightId FlightId { get; }

		public Airport DepartureAirport { get; }

		public DateTimeOffset DepartureDate { get; }

		public Airport DestinationAirport { get; }

		public Price Price { get; }

		public PricedInstance(
			FlightId flightId,
			Airport departureAirport,
			DateTimeOffset departureDate,
			Airport destinationAirport,
			Price price)
		{

			FlightId = flightId;
			DepartureAirport = departureAirport;
			DepartureDate = departureDate;
			DestinationAirport = destinationAirport;
			Price = price;
		}
	}
}