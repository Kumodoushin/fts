namespace FTS.Domain;

public struct FlightId
{
	private readonly string _IATAAirlineSegment;
	private readonly int _number;
	private readonly string _postfix;
	private static bool AnyOfCharsIsNotAsciiUpperLetter(string value) =>
		!char.IsAsciiLetterUpper(value[0]) 
		|| !char.IsAsciiLetterUpper(value[1]) 
		|| !char.IsAsciiLetterUpper(value[2]);

	public FlightId(string id)
	{
		var segments = id.Split(" ");
		if (segments.Length != 3)
		{
			throw new InvalidInputIdFormat();
		}

		if (segments[0].Length != 3)
		{
			throw new InvalidIATAAirlineCodeFormat();
		}
		if (AnyOfCharsIsNotAsciiUpperLetter(segments[0]))
		{
			throw new InvalidSegmentFormat(segments[0]);
		}
		_IATAAirlineSegment = segments[0];
		
		_number = int.Parse(segments[1]);
		if (_number is > 99999 or < 1)
		{
			throw new InvalidFlightNumberValue();
		}
		
		if (segments[2].Length != 3)
		{
			throw new InvalidFinalSegmentLength();
		}

		if (AnyOfCharsIsNotAsciiUpperLetter(segments[2]))
		{
			throw new InvalidSegmentFormat(segments[2]);
		}
		_postfix = segments[2];
	}

	public override string ToString() => $"{_IATAAirlineSegment} {_number:D5} {_postfix}";

	public override int GetHashCode() => HashCode.Combine(_IATAAirlineSegment,_number,_postfix);

	public override bool Equals(object? obj) => 
		obj is not null 
		&& obj is FlightId otherFlight 
		&& this == otherFlight;

	public static bool operator ==(FlightId a, FlightId b) =>
		a._IATAAirlineSegment == b._IATAAirlineSegment 
		&& a._number == b._number 
		&& a._postfix == b._postfix;

	public static bool operator !=(FlightId a, FlightId b) => !(a == b);

	public class InvalidFlightNumberValue : Exception
	{
		public InvalidFlightNumberValue() :
			base ("Second part of FlightId, the Flight number must be greater than 0 and less than 100'000")
		{
		}
	}

	public class InvalidSegmentFormat : Exception
	{
		public InvalidSegmentFormat(string value) : 
			base($"Provided value \"{value}\" does not match the 3 letter format")
		{
		}
	}

	public class InvalidFinalSegmentLength : Exception
	{
		public InvalidFinalSegmentLength() : 
			base("Last segment of FlightId, must have 3 letters")
		{
		}
	}

	public class InvalidIATAAirlineCodeFormat : Exception
	{
		public InvalidIATAAirlineCodeFormat() : 
			base("First part of FlightId, the IATA Airline Code must have 3 letters")
		{
		}
	}
	
	public class InvalidInputIdFormat : Exception
	{
		public InvalidInputIdFormat() : 
			base("Provided FlightId string has invalid format, expected \"AAA 11111 AAA\"")
		{
		}
	}
}