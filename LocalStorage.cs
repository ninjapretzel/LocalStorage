using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;

/// <summary> Helper class to access LocalStorage on WebGL, and as files outside of WebGL </summary>
public sealed class LocalStorage {
#if UNITY_WEBGL
	[DllImport("__Internal")]
	private static extern string GetData(string key);
	[DllImport("__Internal")]
	private static extern void SetData(string key, string data);
#endif
	/// <summary> Instance to access localStorage. </summary>
	public static LocalStorage instance = new LocalStorage();
	/// <summary> Private constructor to disallow creation elsewhere. </summary>
	private LocalStorage() { }

	/// <summary> Accesses 'localStorage' in a platform independent way. </summary>
	/// <param name="key"> Key to access in localStorage. </param>
	/// <returns> Value of key in localStorage </returns>
	public string this[string key] {
#if UNITY_WEBGL && !UNITY_EDITOR
		// Inside web-page version 
		get { return GetData(key); }
		set { SetData(key, value); }
#else
		// Other platform version
		get {
			string path = Application.persistentDataPath + "/" + key;
			try {
				if (File.Exists(path)) {
					return File.ReadAllText(path);
				}
			} catch (Exception e) {
				Debug.LogWarning($"Error loading file {key} from {path}");
				Debug.LogWarning(e);
			}
			return "";
		}
		set {
			string path = Application.persistentDataPath + "/" + key;
			try {
				File.WriteAllText(path, value);
			} catch (Exception e) {
				Debug.LogWarning($"Error saving to file {key} at {path}");
				Debug.LogWarning(e);
			}
		}
#endif
	}
}
