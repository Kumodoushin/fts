using Ardalis.SmartEnum;

namespace FTS.Domain;

public sealed class Continent:SmartEnum<Continent,string>
{
	private Continent(string name, string value) : base(name, value)
	{
	}

	public static Continent Oceania = new("Oceania", "OC");
	public static Continent NorthAmerica = new ("North America","NA");
	public static Continent Europe = new ("Europe","EU");
	public static Continent Africa = new ("Africa","AF");
	public static Continent SouthAmerica = new ("South America","SA");
	public static Continent Asia = new ("Asia","AS");
}