using FTS.Domain;

namespace FTS.Application;

public class NextFlightInstances
{
	private readonly Dictionary<FlightId,Flight> _flightDefinitions;
	private readonly DateTimeOffset _currentUtcTime;

	public NextFlightInstances(Dictionary<FlightId, Flight> flightDefinitions, DateTimeOffset currentUtcTime)
	{
		_flightDefinitions = flightDefinitions;
		_currentUtcTime = currentUtcTime;
	}

	public List<Flight.Instance> Handle(FlightId id, byte amount)
	{
		if (_flightDefinitions.TryGetValue(id, out var flightDefinition))
		{
			var currentTime = new DateTimeOffset(
				_currentUtcTime.UtcDateTime,
				new TimeSpan(
					0,
					0,
					0));
			return flightDefinition.NextFlightInstances(amount, currentTime).ToList();
		}

		return new List<Flight.Instance>();
	}
}