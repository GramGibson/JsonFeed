# JsonFeed 1.0.2
C# parsing for [JsonFeed.org](https://jsonfeed.org) feeds

[![NuGet](https://img.shields.io/nuget/v/JsonFeed.svg)](https://www.nuget.org/packages/JsonFeed)

## Install

JsonFeed is available on [NuGet](https://www.nuget.org/packages/JsonFeed/):

```
Install-Package JsonFeed
```

## Usage

Load a feed by URL using `JsonFeed.Load`:

```csharp
var url = "https://jsonfeed.org/feed.json";
var jsonFeed = JsonFeed.Load(url);
```

Parse a JSON string using `JsonFeed.Parse`:

```csharp
var jsonFeed = JsonFeed.Parse(jsonString);
```

#### More examples and documentation coming soon
