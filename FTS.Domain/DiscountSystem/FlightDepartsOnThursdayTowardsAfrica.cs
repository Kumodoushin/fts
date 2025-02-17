namespace FTS.Domain;

public partial class Discount
{
	public static readonly Discount FlightDepartsOnThursdayTowardsAfrica = new FlightDepartsOnThursdayTowardsAfricaDefinition(); 
	public sealed class FlightDepartsOnThursdayTowardsAfricaDefinition : Discount
	{
		public FlightDepartsOnThursdayTowardsAfricaDefinition() : 
			base("Flight destination is in Africa and departure is on Thursday", 2)
		{
		}

		public override bool IsApplicableTo(Tenant tenant, Flight.Instance flight) 
			=> flight.DepartsOnThursday()
			   && flight.FliesToAfrica();
	}
}