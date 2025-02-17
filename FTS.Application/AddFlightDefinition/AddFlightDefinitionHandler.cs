using FTS.Domain;
using OneOf;
namespace FTS.Application.AddFlightDefinition;

public class AddFlightDefinitionHandler
{
	private readonly Dictionary<FlightId,Flight> _data;
	public AddFlightDefinitionHandler(Dictionary<FlightId,Flight> data)
	{
		_data = data;
	}
	
	public OneOf<bool,FlightDefinitionAlreadyExists> Handle(Flight flightToAdd)
	{
		if (_data.ContainsKey(flightToAdd.Id))
		{
			return new FlightDefinitionAlreadyExists();
		}

		_data.Add(flightToAdd.Id, flightToAdd);

		return true;
	}

	public class FlightDefinitionAlreadyExists : Exception
	{
		
	}
}