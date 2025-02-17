var client = new HttpClient()
{
	BaseAddress = new Uri("https://airportdb.io/api/v1/airport/"),
};

string apiToken = string.Empty; //get yours from airportdb.io

if (string.IsNullOrWhiteSpace(apiToken))
{
	return;
}

var dir = new DirectoryInfo("icaoData");
if (!dir.Exists)
{
	dir.Create();
}

using var textReader = new StreamReader("ICAO.txt");

while (!textReader.EndOfStream)
{
	var ICAO = (await textReader.ReadLineAsync() ?? string.Empty).Trim();

	if (ICAO.StartsWith('#') || dir.GetFiles($"{ICAO}.*").Any())
	{
		Console.WriteLine($"Skipping {ICAO}");
		continue;
	}
	
	Console.Write($"Downloading {ICAO}...");
	var response = await client.GetAsync($"{ICAO}?apiToken={apiToken}");
	
	if (response.IsSuccessStatusCode)
	{
		var data = await response.Content.ReadAsStringAsync();
		
		Console.Write($"\tSaving {ICAO}.json...");
		File.WriteAllText(
			Path.Combine("icaoData", $"{ICAO}.json"),
			data);
	}
	else
	{
		Console.Write($"\tFailed, details saved to {ICAO}.error...");
		
		File.WriteAllText(
			Path.Combine("icaoData", $"{ICAO}.error"),
			 response.ToString());
	}
	Console.WriteLine($"\tDone");
}