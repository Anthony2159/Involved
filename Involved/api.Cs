using System;

public class Class1
{
	public Class1()
	{
		// De url om de challenge te starten
		var startUrl = "api/path/1/easy/Start";

		// We voeren de call uit en wachten op de response
		// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/await
		// De start endpoint moet je 1 keer oproepen voordat je aan een challenge kan beginnen
		// Krijg je een 403 Forbidden response op je Sample of Puzzle calls? Dat betekent dat de challenge niet gestart is
		var startResponse = await client.GetAsync(startUrl);

		// De url om de sample challenge data op te halen
		var sampleUrl = "api/path/1/easy/Sample";

		// We doen de GET request en wachten op de het antwoord
		// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
		var sampleGetResponse = await client.GetFromJsonAsync<List<int>>(sampleUrl);

		// Je zoekt het antwoord
		var sampleAnswer = GetAnswer(sampleGetResponse);

		// We sturen het antwoord met een POST request
		// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
		// De response die we krijgen zal ons zeggen of ons antwoord juist was
		var samplePostResponse = await client.PostAsJsonAsync<int>(sampleUrl, sampleAnswer);

		// Om te zien of ons antwoord juist was moeten we de response uitlezen
		// Een 200 status code betekent dus niet dat je antwoord juist was!
		var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();

		// De url om de puzzle challenge data op te halen
		var puzzleUrl = "api/path/1/easy/Puzzle";
		// We doen de GET request en wachten op de het antwoord
		// De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
		var puzzleGetResponse = await client.GetFromJsonAsync<List<int>>(puzzleUrl);

		// Je zoekt het antwoord
		var puzzleAnswer = GetAnswer(puzzleGetResponse);

		// We sturen het antwoord met een POST request
		// Het antwoord dat we moeten versturen is een getal dus gebruiken we int
		// De response die we krijgen zal ons zeggen of ons antwoord juist was
		var puzzlePostResponse = await client.PostAsJsonAsync<int>(puzzleUrl, puzzleAnswer);

		// Om te zien of ons antwoord juist was moeten we de response uitlezen
		// Een 200 status code betekent dus niet dat je antwoord juist was!
		var puzzlePostResponseValue = await puzzlePostResponse.Content.ReadAsStringAsync();
	}
}
