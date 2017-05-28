# JsonFeed 1.0.1
C# parsing for [JsonFeed.org](https://jsonfeed.org) feeds

## Nuget

JsonFeed is available on [NuGet](https://www.nuget.org/packages/JsonFeed/):

```
Install-Package JsonFeed
```

## Usage

Load a JSON string from an HTTP call and use the static `JsonFeed.Parse` method to get an instance of `JsonFeed`.

```csharp
var feedUrl = "https://jsonfeed.org/feed.json";
var client = new HttpClient();
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, feedUrl);
var httpResponseMessage = await client.SendAsync(httpRequestMessage);
var byteArray = await httpResponseMessage.Content.ReadAsByteArrayAsync();
var response = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

// parse an instance of JsonFeed from the HTTP response
var jsonFeed = JsonFeed.Parse(response);
```

#### More examples and documentation coming soon
