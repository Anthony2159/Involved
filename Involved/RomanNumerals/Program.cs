using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomanNumerals
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Swagger
            // https://app-htf-2022.azurewebsites.net/swagger/index.html
            // De httpclient die we gebruiken om http calls te maken
            var client = new HttpClient();

            // De base url die voor alle calls hetzelfde is
            client.BaseAddress = new Uri("https://app-htf-2022.azurewebsites.net");

            // De token die je gebruikt om je team te authenticeren, deze kan je via de swagger ophalen met je teamname + password
            var token = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMTIiLCJuYmYiOjE2Njg1MTU3NDMsImV4cCI6MTY2ODYwMjE0MywiaWF0IjoxNjY4NTE1NzQzfQ.H3uEeM0UNgUj7EN11gVwXe_eq1HkATxdMRJvAyVAw_xQyiBm9jrRrXjNMurRUz0O2p6J3SDG_whSCENG2D98zw";

            // We stellen de token in zodat die wordt meegestuurd bij alle calls, anders krijgen we een 401 Unauthorized response op onze calls
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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
            var sampleGetResponse = await client.GetFromJsonAsync<List<string>>(sampleUrl);

            // Je zoekt het antwoord
            var sampleAnswer = GetAnswer(sampleGetResponse);

            // We sturen het antwoord met een POST request
            // Het antwoord dat we moeten versturen is een getal dus gebruiken we int
            // De response die we krijgen zal ons zeggen of ons antwoord juist was
            var samplePostResponse = await client.PostAsJsonAsync<string>(sampleUrl, sampleAnswer);

            // Om te zien of ons antwoord juist was moeten we de response uitlezen
            // Een 200 status code betekent dus niet dat je antwoord juist was!
            var samplePostResponseValue = await samplePostResponse.Content.ReadAsStringAsync();

            Console.WriteLine(samplePostResponseValue);

            //De url om de puzzle challenge data op te halen
            var puzzleUrl = "api/path/1/easy/Puzzle";
            // We doen de GET request en wachten op de het antwoord
            // De response die we verwachten is een lijst van getallen dus gebruiken we List<int>
            var puzzleGetResponse = await client.GetFromJsonAsync<List<string>>(puzzleUrl);

            // Je zoekt het antwoord
            var puzzleAnswer = GetAnswer(puzzleGetResponse);

            // We sturen het antwoord met een POST request
            // Het antwoord dat we moeten versturen is een getal dus gebruiken we int
            // De response die we krijgen zal ons zeggen of ons antwoord juist was
            var puzzlePostResponse = await client.PostAsJsonAsync<string>(puzzleUrl, puzzleAnswer);

            // Om te zien of ons antwoord juist was moeten we de response uitlezen
            // Een 200 status code betekent dus niet dat je antwoord juist was!
            var puzzlePostResponseValue = await puzzlePostResponse.Content.ReadAsStringAsync();

            Console.WriteLine(puzzlePostResponseValue);
        }
        
        public static string GetAnswer(List<string> romanLetters)
        {
            // roman letters to list of ints
            List<int> intFromRomanList = new List<int>();

            foreach (var romanLetter in romanLetters)
            {
                var response = roman_to_int(romanLetter);
                intFromRomanList.Add(response);
            }

            // sum of ints
            var sumInts = intFromRomanList.Sum();

            // sum to roman letter
           var romein =  IntToRoman(sumInts);

            return romein; 
        }

        // omzetten romans cijfers naar int 
        public static int roman_to_int(string str1)
        {
            var num = 0;
            for (int i = 0; i < str1.Length; i++)
            {
                if (i > 0 && Value(str1[i]) > Value(str1[i - 1]))
                {
                    num += Value(str1[i]) - Value(str1[i - 1]) * 2;
                }
                else
                {
                    num += Value(str1[i]);
                }
            }
            return num;
        }

        // De value's van roman cijfers en integers 
        public static int Value(char chr)
        {
            switch (chr)
            {
                case 'I': return 1;
                case 'V': return 5;
                case 'X': return 10;
                case 'L': return 50;
                case 'C': return 100;
                case 'D': return 500;
                case 'M': return 1000;
                default: return 0;
            }
        }

        // het omzetten van een Int naar Roman cijfers 
        public static string IntToRoman(int num)
        {
            int[] values = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            String[] romanLetters = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            StringBuilder roman = new StringBuilder();
            //string roman = "";
            for (int i = 0; i < values.Length; i++)
            {
                while (num >= values[i])
                {
                    num = num - values[i];
                    roman.Append(romanLetters[i]);
                }
            }
            return roman.ToString();
        }
    }
}
