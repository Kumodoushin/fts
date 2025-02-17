using FTS.Domain;
using OneOf;

namespace FTS.Application;

public class ReserveFlightForTenant
{
	private readonly Dictionary<FlightId, Flight> _flightDefinitions;

	public ReserveFlightForTenant(Dictionary<FlightId,Flight> flightDefinitions)
	{
		_flightDefinitions = flightDefinitions;
	}

	public OneOf<Flight.Instance,CouldNotReserveFlight> Handle(Tenant tenant, Flight.Instance flightInstance)
	{
		if (_flightDefinitions.TryGetValue(flightInstance.FlightId, out _))
		{
			return tenant.AddReservationFor(flightInstance);
		}

		return new CouldNotReserveFlight();
	}

	public class CouldNotReserveFlight : Exception
	{
	}
}