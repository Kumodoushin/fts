using Ardalis.SmartEnum;

namespace FTS.Domain;

public partial class Discount : SmartEnum<Discount>
{
	public virtual bool IsApplicableTo(Tenant tenant, Flight.Instance flight) => false;

	protected Discount(string name, int value) : base(name, value)
	{
	}

	internal static IEnumerable<Discount> FindApplicableDiscountsFor(Tenant tenant, Flight.Instance flight) =>
		List.Where(x => x.IsApplicableTo(tenant, flight));
}