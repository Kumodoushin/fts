namespace FTS.Domain;

public partial class Discount
{
	public static readonly Discount FlightDepartureIsOnTenantsBirthday = new FlightDepartureIsOnTenantsBirthdayDefinition(); 
	public sealed class FlightDepartureIsOnTenantsBirthdayDefinition : Discount
	{
		public FlightDepartureIsOnTenantsBirthdayDefinition() : base("Flight departure is on tenants birthday", 1)
		{
		}

		public override bool IsApplicableTo(Tenant tenant, Flight.Instance flight) 
			=> tenant.HasBirthdayOnDepartureDateOf(flight);
	}
}