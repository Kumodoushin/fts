using FTS.Domain;

namespace FTS.Application;

public class CalculatePriceForTenantsReservation
{
	private readonly Dictionary<FlightId, Price> _priceCatalog;
	private readonly DateOnly _currentDate;

	public CalculatePriceForTenantsReservation(Dictionary<FlightId,Price> priceCatalog, DateOnly currentDate)
	{
		_priceCatalog = priceCatalog;
		_currentDate = currentDate;
	}

	public Flight.PricedInstance Handle(Tenant tenant, Flight.Instance instance)
	{
		_priceCatalog.TryGetValue(instance.FlightId, out var basePrice);
		basePrice.TryApplyDiscounts(tenant,instance);
		var pricedInstance = instance.WithPrice(basePrice);
		return tenant.AddPricedFlight(pricedInstance, _currentDate);
	}
}