namespace FTS.Domain;

public class Airport
{
	private readonly string _iata;
	private readonly string _icao;

	public required string IATA
	{
		get => _iata;
		init
		{
			if (HasInvalidLengthOrContainsNonAsciiUpperLetter(value))
			{
				throw new InvalidIATAAirportCodeFormat();
			}
			_iata = value;
		}
	}

	public required string ICAO
	{
		get => _icao;
		init
		{
			if (HasInvalidLengthOrContainsNonAsciiUpperLetterOrDigit(value))
			{
				throw new InvalidICAOAirportCodeFormat();
			}
			_icao = value;
		}
	}

	public required string Name { get; init; }
	public required string Country { get; init; }
	public required string CountryISO { get; init; }
	public required Continent Continent { get; init; }
	public required double Latitude { get; init; }
	public required double Longitude { get; init; }
	
	private static bool HasInvalidLengthOrContainsNonAsciiUpperLetter(string value) =>
		value.Length != 3
		|| !char.IsAsciiLetterUpper(value[0]) 
		|| !char.IsAsciiLetterUpper(value[1]) 
		|| !char.IsAsciiLetterUpper(value[2]);
	
	private static bool HasInvalidLengthOrContainsNonAsciiUpperLetterOrDigit(string value) =>
		value.Length != 4
		|| !char.IsAsciiLetterUpper(value[0]) || !char.IsAsciiDigit(value[0]) 
		|| !char.IsAsciiLetterUpper(value[1]) || !char.IsAsciiDigit(value[1]) 
		|| !char.IsAsciiLetterUpper(value[2]) || !char.IsAsciiDigit(value[2])
		|| !char.IsAsciiLetterUpper(value[3]) || !char.IsAsciiDigit(value[3]);
	
	public class InvalidIATAAirportCodeFormat : Exception
	{
		public InvalidIATAAirportCodeFormat() : 
			base("IATA airport code must have 3 uppercase letters")
		{
		}
	}
	
	public class InvalidICAOAirportCodeFormat : Exception
	{
		public InvalidICAOAirportCodeFormat() : 
			base("ICAO airport code must have 4 uppercase letters and digits")
		{
		}
	}
}