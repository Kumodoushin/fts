using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace FTS.Domain;

public sealed class Continent : SmartEnum<Continent,string>
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

public class ContinentConverter : JsonConverter<Continent>
{
	public override void WriteJson(
		JsonWriter writer,
		Continent? value,
		JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override Continent? ReadJson(
		JsonReader reader,
		Type objectType,
		Continent? existingValue,
		bool hasExistingValue,
		JsonSerializer serializer)
	{
		do
		{
			reader.Read();
		} while ((string)reader.Value != "Value");
		reader.Read();
		string s = (string)reader.Value;
		var toReturn = 
		s switch
		{
			"OC" => Continent.Oceania,
			"NA" => Continent.NorthAmerica,
			"EU" => Continent.Europe,
			"AF" => Continent.Africa,
			"SA" => Continent.SouthAmerica,
			"AS" => Continent.Asia,
			_ => null,
		};
		while (reader.TokenType != JsonToken.EndObject) 
		{
			reader.Read();
		}

		return toReturn;
	}
}