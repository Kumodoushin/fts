namespace FTS.Domain;

internal static class TenantExtensions
{
	internal static bool HasBirthdayOnDepartureDateOf(this Tenant tenant, Flight.Instance flight) =>
		tenant.DateOfBirth.Month == flight.DepartureDate.Month
		&& tenant.DateOfBirth.Day == flight.DepartureDate.Day;
}