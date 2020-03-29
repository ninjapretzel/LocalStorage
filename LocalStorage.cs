using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

/// <summary> Helper class to access LocalStorage on WebGL, and as files outside of WebGL </summary>
public sealed class LocalStorage {
#if UNITY_WEBGL
	[DllImport("__Internal")]
	private static extern string GetData(string key);
	[DllImport("__Internal")]
	private static extern void SetData(string key, string data);
#endif
	/// <summary> Instance to access localStorage. </summary>
	public static readonly LocalStorage instance = new LocalStorage();
	/// <summary> Instance to access localStorage for byte[] data using base64 encoding. </summary>
	public static readonly WithBase64 base64 = new WithBase64(); 
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
				} else {
					return null;
				}
			} catch (Exception e) {
				Debug.LogWarning($"Error loading file {key} from {path}");
				Debug.LogWarning(e);
			}
			return null;
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
	
	/// <summary> Class to wrap LocalStorage with Base64 conversion for byte[] data. </summary>
	public class WithBase64 {
		/// <summary> Internal constructor for use by same assembly (Plugins) only. </summary>
		internal WithBase64() { }
		/// <summary> Accesses 'localStorage', passing data through Base64 conversion. </summary>
		/// <param name="key"> Key to access in localStorage. </param>
		/// <returns> Value of key in localStorage, as a Base64 encoded byte[] </returns>
		public byte[] this[string key] {
			get { 
				string value = instance[key];
				if (value == null) { return null; }

				try {
					return Convert.FromBase64String(value); 
				} catch (Exception e) {
					Debug.LogWarning($"Could not parse contents of file {key}, was not Base64 encoded.");
					Debug.LogWarning(e);
					return null;
				}
			}
			set { instance[key] = Convert.ToBase64String(value); }
		}
	}
}
