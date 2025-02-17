using Bogus;
using FsCheck;
using FsCheck.Fluent;

namespace FTS.Domain.Tests;

public class FlightIdTests
{
	
	[Theory]
	[ClassData(typeof(ValidInputStringsForFlightId))]
	public void ProperInputStringCreatesValidInstance(string inputString)
	{
		var flightId = new FlightId(inputString);
	}

	[Theory]
	[ClassData(typeof(InvalidInputStringsForFlightIdAndExpectedExceptions))]
	public void InvalidInputStringThrowsSpecificExceptions(Type expectedException, string inputString)
	{
		Assert.Throws(
			expectedException,
			testCode: () =>
				{
					var id = new FlightId(inputString);
				});
	}

	public static Gen<string> _iataGen = Gen.Elements((new List<string> { "ASD", "BSD", "CSD" }).AsEnumerable());
	public static Gen<string> _lastBitGen = Gen.Elements((new List<string> { "ASD", "BSD", "CSD" }).AsEnumerable());
	
	[Fact]
	public void IntancesCreatedFromSameDataAreEquivalent()
	{
		Prop.ForAll(
			_iataGen.ToArbitrary(),
			Gen.Choose(1, 99_999).ToArbitrary(),
			_lastBitGen.ToArbitrary(),
			(
				iata,
				num,
				lastSeg) =>
			{
				var inst1 = new FlightId($"{iata} {num:D5} {lastSeg}");
				var inst2 = new FlightId($"{iata} {num} {lastSeg}");

				return inst1 == inst2;
			})
			.VerboseCheckThrowOnFailure();
	}

	public class InvalidInputStringsForFlightIdAndExpectedExceptions : TheoryData<Type,string>
	{
		public InvalidInputStringsForFlightIdAndExpectedExceptions()
		{
			Add(typeof(FlightId.InvalidInputIdFormat), "1");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLM1");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLM1BCA");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLM 1");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLM 12");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLM 123");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLM 1234");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLM 12345");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLMA 123456");
			Add(typeof(FlightId.InvalidInputIdFormat), "1 BCA");
			Add(typeof(FlightId.InvalidInputIdFormat), "12 BCA");
			Add(typeof(FlightId.InvalidInputIdFormat), "123 BCA");
			Add(typeof(FlightId.InvalidInputIdFormat), "1234 BCA");
			Add(typeof(FlightId.InvalidInputIdFormat), "12345 BCA");
			Add(typeof(FlightId.InvalidInputIdFormat), "123456 BCAA");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLMA BCAA");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLA BAA");
			Add(typeof(FlightId.InvalidInputIdFormat), "KA BA");
			Add(typeof(FlightId.InvalidInputIdFormat), "K B");
			Add(typeof(FlightId.InvalidInputIdFormat), "KLM 123456 BCAA extrabits");

			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"KLMA 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"KLMBA 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"KA 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"A 00001 BCA");
			
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"KLm 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"KlM 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"kLM 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"1LM 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"K2M 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"KL3 00001 BCA");
			Add(typeof(FlightId.InvalidICAOAirlineCodeFormat), $"KLą 00001 BCA");
			
			Add(typeof(FlightId.InvalidFlightNumberValue), $"KLM wrongValue BCA");
			Add(typeof(FlightId.InvalidFlightNumberValue), $"KLM 00000 BCA");
			Add(typeof(FlightId.InvalidFlightNumberValue), $"KLM 100001 BCA");
			var f = new Faker();
			var rnd = f.Random;
			for (int i = 0; i < 100; i++)
			{
				Add(typeof(FlightId.InvalidFlightNumberValue), $"KLM {rnd.Int(100_000, Int32.MaxValue)} BCA");
			}
			for (int i = 0; i < 100; i++)
			{
				Add(typeof(FlightId.InvalidFlightNumberValue), $"KLM {rnd.Int(Int32.MinValue, 0)} BCA");
			}
			
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 BCAC");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 BCABC");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 BC");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 B");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 bCA");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 BcA");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 BCa");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 1CA");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 B2A");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 BC3");
			Add(typeof(FlightId.InvalidFinalSegmentValue), $"KLM 00001 BCą");
			
		}
	}

	public class ValidInputStringsForFlightId : TheoryData<string>
	{
		private static string _characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public ValidInputStringsForFlightId()
		{
			var f = new Faker();
			var rnd = f.Random;
			
			for (int i = 0; i < 10000; i++)
			{
				Add($"{rnd.String2(3, _characterSet)} {rnd.Int(1, 99999)} {rnd.String2(3, _characterSet)}");
			}
		}
	}
}