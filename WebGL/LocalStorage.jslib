// On the C# side, add these lines into some file to use these JS functions directly:
/*
/// At the top...
using System.Runtime.InteropServices; // DllImport attribute

// In your class: 
	[DllImport("__Internal")]
	private static extern string GetData(string key);
	[DllImport("__Internal")]
	private static extern void SetData(string key, string data);
*/


/** Adds the functions into the JS Library so they may be called in the C# side. */
mergeInto(LibraryManager.library, {
	GetData: function(key){
		/** Helper to Convert a javascript string to a managed "C#" string. */
		function toManagedString(str) {
			const bufferSize = lengthBytesUTF8(str) + 1;
			const buffer = _malloc(bufferSize);
			stringToUTF8(str, buffer, bufferSize);
			return buffer;
		}

		try {
			key = Pointer_stringify(key);
			return toManagedString(localStorage[key]);
		} catch (err) {
			console.log("Error Loading data:");
			console.log(err);
			return toManagedString("");
		}
	},
	SetData: function(key, data){
		try {
			key = Pointer_stringify(key);
			data = Pointer_stringify(data);
			localStorage[key] = data;
		} catch (err) {
			console.log("Error saving data:");
			console.log(err);
		}
	}
});