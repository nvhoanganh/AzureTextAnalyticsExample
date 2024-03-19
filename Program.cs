using System.Globalization;
using Azure;
using Azure.AI.TextAnalytics;
using Newtonsoft.Json;

// https://www.linkedin.com/pulse/entity-recognition-azure-cognitive-services-c-matt-eland

// These values should come from a config file and should NOT be stored in source control
string key = "EnterKey";
Uri endpoint = new Uri("https://ENTER_SERVICE.cognitiveservices.azure.com/");


// Create the TextAnalyticsClient and set its endpoint
AzureKeyCredential credentials = new AzureKeyCredential(key);
TextAnalyticsClient textClient = new TextAnalyticsClient(endpoint, credentials);

// var text = new[] {
//     "Sub-Roman Britain is the period of late antiquity in Great Britain between the end of Roman rule and the Anglo-Saxon settlement. The term was originally used to describe archaeological remains found in 5th- and 6th-century AD sites that hinted at the decay of locally made wares from a previous higher standard under the Roman Empire. It is now used to describe the period that commenced with the recall of Roman troops to Gaul by Constantine III in 407 and to have concluded with the Battle of Deorham in 577.",
//     "Transcript of conversation obtained in relation to this case:\n\nSpeaker 1: Her family and friends have gathered to farewell Andrew Simons almost a fortnight after his tragic passing. Former teammates spoke of his brilliance both on and off the pitch as loved ones mourn the cricket legend. Lane Calcutt has more.\n  Speaker 2: The pain so raw for a mother, wife, children and siblings.\n  Speaker 3: A life well lived, albeit cut short far too.\n  Speaker 2: Early saying goodbye to a cricket legend, a public hero farewelled in private on a Townsville cricket field by family and invited guests.\n  Speaker 4: The service was just simply beautiful. It was. I've not seen a congregation be so moved, full of sadness, but just such wonderful, beautiful memories of a guy that just\n  Speaker 4: just gave him so much of himself to so many people.\n  Speaker 2: Later, a public memorial to celebrate the life, the times, the career of the man they called Roy.\n  Speaker 2: They came in the hundreds, partner Laura, those who didn't know him and those who did. Some who played beside him in his 26 Test and two World Cup wins.\n  Speaker 5: The memories of Roy\n  Speaker 5: A fond\n  Speaker 5: He had Impact\n  Speaker 5: and A.\n  Speaker 2: Lot of mates grades themselves. Darren Lehman, Adam Gilchrist, his former captain, Ricky Ponting's lifelong friend and childhood teammate. Jimmy Marr, where he's fiercely loyal, suspicious, creative, humorous. They spoke, of course, about Roy's on field heroics, a run scored, the catches and wickets taken.\n  Speaker 6: From picking A-Team tomorrow in a Test match one day or or a T20 team, he's in my team every day of the week.\n  Speaker 2: But they also spoke of the off field Andrew Simons, the heart and soul of\n  Speaker 2: any team farewell. The way Roy would have wanted by his cricket mate Andrew Simons was 46 Lane Calcutt 9 News."
// };

// var toReturn = new List<CategorizedEntity>();
// Response<RecognizeEntitiesResultCollection> entities = await textClient.RecognizeEntitiesBatchAsync(text);
// foreach (var entity in entities.Value)
// {
//     foreach (CategorizedEntity categorizedEntity in entity.Entities)
//     {
//         toReturn.Add(categorizedEntity);
//     }
// }

// foreach (CategorizedEntity entity in toReturn)
// {
//     Console.WriteLine($"\t{entity.Text} (Category: {entity.Category}, subCategory: {entity.SubCategory}) with {entity.ConfidenceScore:P} confidence");
// }


var listOfTexts = new[] {
  "I didn't kill my wife",
  "I really enjoy killing people",
  "I really enjoy not killing people",
};
// var sentiments = await textClient.AnalyzeSentimentBatchAsync(listOfTexts);
// Console.WriteLine("--------------------------------------------------------");
// Console.WriteLine("Sentiments:");
// // Console.WriteLine(JsonConvert.SerializeObject(sentiments.Value, Formatting.Indented));

// for (int i = 0; i < sentiments.Value.Count; i++)
// {
//     Console.WriteLine(listOfTexts[i]);
//     var docSentiment = sentiments.Value[i];
//     Console.WriteLine($"\t Overall Sentiment: {docSentiment.DocumentSentiment.Sentiment}");
//     foreach (var sentence in docSentiment.DocumentSentiment.Sentences)
//     {
//         Console.WriteLine($"\t \"{sentence.Text}\": {sentence.Sentiment}");
//     }
// }

var kingArthur = @"Transcript of conversation obtained in relation to this case:\n\nSpeaker 1: Her family and friends have gathered to farewell Andrew Simons almost a fortnight after his tragic passing. Former teammates spoke of his brilliance both on and off the pitch as loved ones mourn the cricket legend. Lane Calcutt has more.\n  Speaker 2: The pain so raw for a mother, wife, children and siblings.\n  Speaker 3: A life well lived, albeit cut short far too.\n  Speaker 2: Early saying goodbye to a cricket legend, a public hero farewelled in private on a Townsville cricket field by family and invited guests.\n  Speaker 4: The service was just simply beautiful. It was. I've not seen a congregation be so moved, full of sadness, but just such wonderful, beautiful memories of a guy that just\n  Speaker 4: just gave him so much of himself to so many people.\n  Speaker 2: Later, a public memorial to celebrate the life, the times, the career of the man they called Roy.\n  Speaker 2: They came in the hundreds, partner Laura, those who didn't know him and those who did. Some who played beside him in his 26 Test and two World Cup wins.\n  Speaker 5: The memories of Roy\n  Speaker 5: A fond\n  Speaker 5: He had Impact\n  Speaker 5: and A.\n  Speaker 2: Lot of mates grades themselves. Darren Lehman, Adam Gilchrist, his former captain, Ricky Ponting's lifelong friend and childhood teammate. Jimmy Marr, where he's fiercely loyal, suspicious, creative, humorous. They spoke, of course, about Roy's on field heroics, a run scored, the catches and wickets taken.\n  Speaker 6: From picking A-Team tomorrow in a Test match one day or or a T20 team, he's in my team every day of the week.\n  Speaker 2: But they also spoke of the off field Andrew Simons, the heart and soul of\n  Speaker 2: any team farewell. The way Roy would have wanted by his cricket mate Andrew Simons was 46 Lane Calcutt 9 News.";

var stringInfo = new StringInfo(kingArthur);
Console.WriteLine($"Number of characters in this document is {stringInfo.LengthInTextElements}");
if (stringInfo.LengthInTextElements > 125000)
  // we can't send more than 125,000 chacracters to the service, need to trim it first
  kingArthur = kingArthur[..125000];

AbstractiveSummarizeOperation summary = await textClient.AbstractiveSummarizeAsync(WaitUntil.Completed, new[] { kingArthur });

Console.WriteLine("--------------------------------------------------------");


Console.WriteLine("Abstrac Summary: ");
var results = summary.GetValuesAsync();

await foreach (AbstractiveSummarizeResultCollection result in results)
{
    foreach (AbstractiveSummarizeResult item in result)
    {
        foreach (AbstractiveSummary s in item.Summaries)
        {
            Console.WriteLine(s.Text);
        }
    }
}


// var keyPhrases = new[] {
//   "I had a terrible time at the hotel. The staff was rude and the food was awful.",
//   "Her family and friends have gathered to farewell Andrew Simons almost a fortnight after his tragic passing. Former teammates spoke of his brilliance both on and off the pitch as loved ones mourn the cricket legend. Lane Calcutt has more.",
// };
// Response<ExtractKeyPhrasesResultCollection> keyPhrasesRslt = await textClient.ExtractKeyPhrasesBatchAsync(listOfTexts);
// Console.WriteLine("--------------------------------------------------------");
// Console.WriteLine("Key Phrases:");
// Console.WriteLine(JsonConvert.SerializeObject(keyPhrasesRslt.Value, Formatting.Indented));

// for (int i = 0; i < keyPhrasesRslt.Value.Count; i++)
// {
//     Console.WriteLine(keyPhrases[i]);
//     ExtractKeyPhrasesResult keyPhrase = keyPhrasesRslt.Value[i];
//     foreach (var sentence in keyPhrase.KeyPhrases)
//     {
//         Console.WriteLine($"\t - {sentence}");
//     }
// }