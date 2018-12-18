# JsonStreamLogger

JSON Stream logger provider implementation for Microsoft.Extensions.Logging.

## Usage

```csharp
serviceCollection.AddLogging(options =>
{
    options.AddConsole();
    options.AddJsonStream();
});
```

```csharp
logger.Log(LogLevel.Error, "[{Id}] is {Hello}", 12345, "Konnnichiwa");
logger.Log(LogLevel.Error, "[{Id}] is {Hello}", 67890, "Nya-n");
```

### Outputs
```json
{"Category":"Test","LogLevel":4,"EventId":{"Id":0,"Name":null},"State":{"Id":12345,"Hello":"Konnnichiwa"},"Exception":null,"Message":"[12345] is Konnnichiwa"}
{"Category":"Test","LogLevel":4,"EventId":{"Id":0,"Name":null},"State":{"Id":67890,"Hello":"Nya-n"},"Exception":null,"Message":"[67890] is Nya-n"}
```
