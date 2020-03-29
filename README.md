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