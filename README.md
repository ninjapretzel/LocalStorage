# LocalStorage
Easy access to `window.localStorage` for Unity WebGL builds.

## Usage:
Add contents into `Assets/Plugins/LocalStorage`.

To store values, simply
```csharp
LocalStorage.instance["key"] = "value";
```
and to retrieve data, simply
```csharp
string value = LocalStorage.instance["key"];
```

`byte[]` Values can also be stored and retrieved.
```csharp
byte[] someByteArray;
//...
LocalStorage.base64["key"] = someByteArray;
//...
byte[] retrieved = LocalStorage.base64["key"];
```
