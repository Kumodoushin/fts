namespace FTS.Domain;

public abstract class Tenant
{
	private readonly DateOnly _currentDate;
	private Dictionary<DateOnly,Flight.Instance> _reservations;
	private Dictionary<DateOnly, Flight.PricedInstance> _pricedReservations;
	private List<Flight.PricedInstance> _boughtTickets;

	protected Tenant(DateOnly currentDate, Guid id, DateOnly dateOfBirth, Dictionary<DateOnly,Flight.Instance> reservations)
	{
		_currentDate = currentDate;
		_reservations = reservations;
		Id = id;
		DateOfBirth = dateOfBirth;
	}

	public Guid Id { get; }

	public DateOnly DateOfBirth { get; }

	public Flight.Instance AddReservationFor(Flight.Instance flightInstance)
	{
		_reservations.Add(_currentDate,flightInstance);

		return flightInstance;
	}
	public Flight.PricedInstance AddPricedFlight(Flight.PricedInstance pricedFlight, DateOnly currentDate)
	{
		_pricedReservations.Add(_currentDate,pricedFlight);

		return pricedFlight;
	}

	public void FinalizeBuy(Flight.PricedInstance pricedInstance)
	{
		_boughtTickets.Add(pricedInstance);
		_pricedReservations.Remove(_pricedReservations.First(x => x.Value == pricedInstance).Key);
	}

	protected abstract void LogDiscounts(Flight.Instance flightInstance, Price price); 

	public sealed class A : Tenant
	{
		private Dictionary<Flight.Instance, List<Discount>> _discounts;

		public A(DateOnly currentDate, Guid id, DateOnly dateOfBirth, Dictionary<DateOnly,Flight.Instance> reservations) : base(currentDate, id,dateOfBirth,reservations)
		{
			
		}

		protected override void LogDiscounts(Flight.Instance instance, Price price)
		{
			_discounts.Add(instance,price.AppliedDiscounts);
		}
	}
	
	public sealed class B : Tenant
	{
		public B(DateOnly currentDate, Guid id, DateOnly dateOfBirth, Dictionary<DateOnly,Flight.Instance> reservations) : base(currentDate, id,dateOfBirth,reservations)
		{
			
		}

		protected override void LogDiscounts(Flight.Instance instance, Price price)
		{
		}
	}
}