using FTS.Domain;

namespace FTS.Application;

public class FinalizeBuyForTenant
{
	public void Handle(Tenant tenant, Flight.PricedInstance pricedInstance)
	{
		tenant.FinalizeBuy(pricedInstance);
	}
}