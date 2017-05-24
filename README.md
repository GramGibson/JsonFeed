# JsonFeed
C# parsing for JsonFeed.org feeds

## Nuget

JsonFeed is available on [NuGet](https://www.nuget.org):

```
Install-Package JsonFeed
```

## Usage

Load a JSON string from an HTTP call and use the static `JsonFeed.Parse` method to get an instance of `Feed`.

```csharp
var feedUrl = "https://jsonfeed.org/feed.json";
var client = new HttpClient();
var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, feedUrl);
var httpResponseMessage = await client.SendAsync(httpRequestMessage);
var byteArray = await httpResponseMessage.Content.ReadAsByteArrayAsync();
var response = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

// parse an instance of Feed from the HTTP response
var feed = JsonFeed.Parse(response);
```

#### More examples and documentation coming soon
