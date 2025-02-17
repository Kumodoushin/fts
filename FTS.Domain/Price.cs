namespace FTS.Domain;

public struct Price
{
	private static decimal _discountValue = 5;
	private static decimal _minPrice = 20;
	private readonly decimal _valueEuro;
	private decimal _finalPrice;

	public Price(decimal valueEuro)
	{
		if (_valueEuro < _minPrice)
		{
			throw new ArgumentException($"Min price must be {_minPrice}");
		}
		_valueEuro = valueEuro;
		_finalPrice = _valueEuro;
	}

	public decimal FinalPrice => _finalPrice;
	public List<Discount> AppliedDiscounts { get; } = new();

	public void TryApplyDiscounts(Tenant tenant,Flight.Instance instance)
	{
		var applicableDiscounts = Discount.FindApplicableDiscountsFor(tenant, instance);
		foreach (var discount in applicableDiscounts)
		{
			if (_finalPrice - _discountValue > _minPrice)
			{
				_finalPrice -= _discountValue;
				AppliedDiscounts.Add(discount);
			}
		}
	}
}