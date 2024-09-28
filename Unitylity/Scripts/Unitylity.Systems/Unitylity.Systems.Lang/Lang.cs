
namespace Unitylity.Systems.Lang {

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Text.RegularExpressions;
	using Newtonsoft.Json;
	using UnityEngine;
	using Unitylity.Components.Extended;
	using Unitylity.Data;
	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_LANG_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Unitylity.Systems.Lang) + "/" + nameof(Lang))]
#endif
	public class Lang : Singleton<Lang> {

		[field: SerializeField, Tooltip("Path to translations root folder in resources without leading or trailing slash.")]
		public string translationsPath { get; private set; } = "Lang";

		[field: SerializeField, Tooltip("Name of the language in use or to be used. Language names are C# CultureInfo names.")]
		public string language { get; private set; } = "en-US";

		[field: SerializeField, Tooltip("List of languages. Language names are C# CultureInfo names.")]
		public List<string> languages { get; private set; } = new List<string>() { "en-US" };


		private static Dictionary<string, string> texts;


		protected override void Awake() {
			base.Awake();
			if (!LoadLanguage(language, out var msg)) {
				Debug.LogError($"Failed to load language \"{language}\" at startup: ${msg}");
			}
		}

		static Regex ends = new(@"^\/|\/$");
		public static bool LoadLanguage(string language, out string failMessage) {
			try {
				var ta = Resources.Load<TextAsset>($"{Lang.instance.translationsPath}/{language}");
				try {

					var texts = new Dictionary<string, string>(StringComparer.Ordinal);
					JsonConvert.PopulateObject(ta.text, texts);
					if (texts == null) {
						failMessage = GetStr("Lang_FileCorrupted");
						return false;
					}
					Lang.texts = texts;
					instance.language = language;
					failMessage = null;
					try {
						CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(language);
					} catch (Exception) {
						Debug.LogWarning($"Could not set specific culture of {language}");
						try {
							CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture(new Regex(@"-.*").Replace(language, ""));
						} catch (Exception) {
							Debug.LogWarning($"Could not set specific culture of {new Regex(@"-.*").Replace(language, "")}. It is set to the invariant culture.");
							CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
						}
						throw;
					}
					return true;
				} catch (Exception) {
					failMessage = GetStr("Lang_CannotLoadLanguage");
				}
			} catch (Exception) {
				failMessage = GetStr("Lang_FileCorrupted");
			}
			return false;
		}

		public static bool HasStr(string strId) {
			return texts.ContainsKey(strId);
		}

		public static bool TryGetStr(string strId, out string str) {
			if (texts.TryGetValue(strId, out str)) {
				str = Format(str);
				return true;
			}
			return false;
		}

		public static bool TryGetStrArgs(string strId, out string str, params object[] args) {
			if (texts.TryGetValue(strId, out str)) {
				str = Format(str, args);
				return true;
			}
			return false;
		}


		public static string GetStr(string strId) {
			if (texts != null && texts.TryGetValue(strId, out var res)) return Format(in res);
			return strId;
		}
		public static string GetStr(string strId, string defaultStr) {
			if (texts != null && texts.TryGetValue(strId, out var res)) return Format(in res);
			return defaultStr;
		}
		public static string GetStrArgs(string strId, params object[] args) {
			if (texts != null && texts.TryGetValue(strId, out var res)) return Format(res, args);
			return strId;
		}
		public static string GetStrArgs(string strId, string defaultStr, params object[] args) {
			if (texts != null && texts.TryGetValue(strId, out var res)) return Format(res, args);
			return defaultStr;
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnScriptsReloaded() {
			if (instance) LoadLanguage(instance.language, out var _);
		}
#endif

		[Serializable]
		struct Pair {
			[SerializeField] public string key;
			[SerializeField] public string value;
		}

		const char esc = '/';
		public static string Format(in string str, params object[] args) => Format(in str, 0, args);
		private static string Format(in string str, int depth, params object[] args) {
			var acc = new StringBuilder(str.Length);
			for (int i = 0; i < str.Length; i++) {
				var c = str[i];
				switch (c) {
					case esc:
						if (i + 1 >= str.Length) throw new SyntaxException("Unexpected end of string.");
						var next = str[i + 1];
						if (next == '{' || next == esc) {
							acc.Append(next);
							i++;
						} else {
							acc.Append(c);
						}
						break;
					case '{':
						i++;
						acc.Append(FormatToken(in str, true, false, depth, i, out i));
						break;
					default:
						acc.Append(c);
						break;
				}
			}
			return acc.ToString();

			string FormatToken(in string str, bool isEntry, bool isParam, int depth, int start, out int end) {
				char prevSpecial = default;
				int wordStart = start;
				var cur = new StringBuilder(str.Length);
				string selector = null;
				string branch1 = null;
				string branch2 = null;

				object Evaluate() {
					if (selector == null) throw new SyntaxException("No selector?");
					var splitted = selector.Split('.');
					switch (splitted.Length) {
						case 1: {
								if (splitted[0].Length == 1) {
									var val = args[Int32.Parse(splitted[0])];
									if (branch1 != null) {
										if (branch2 == null) throw new SyntaxException("No second branch.");
										return val switch {
											bool v => v ? branch2 : branch1,
											IComparable v => v.CompareTo(1) == 0 ? branch2 : branch1,
											_ => val,
										};
									}
									return val;
								} else {
									if (depth > 10) return "[RECURSIVE]";
									return isParam || isEntry ? Format(GetStr(splitted[0]), depth + 1) : splitted[0];
									string GetStr(string strId) {
										if (Lang.texts != null && Lang.texts.TryGetValue(strId, out var res)) return res;
										return strId;
									}
								}
							}
						default:
							throw new SyntaxException($"Feature '.' not supported ({selector})");
					}
				}

				for (int i = start; i < str.Length; i++) {
					var c = str[i];
					switch (c) {
						case esc:
							if (i + 1 >= str.Length) throw new SyntaxException("Unexpected end of string.");
							var next = str[i + 1];
							switch (next) {
								case '{':
								case '}':
								case '|':
								case ':':
								case '?':
								case esc:
									cur.Append(next);
									i++;
									break;
								default:
									cur.Append(c);
									break;
							}
							break;
						case '{':
							i++;
							cur.Append(FormatToken(in str, false, prevSpecial != default, depth + 1, i, out i));
							break;
						case '}':
							if (prevSpecial == default) {
								selector = cur.ToString();
							} else if (prevSpecial == '|') {
								branch2 = cur.ToString();
							}
							end = i;
							return Evaluate().ToString();
						case '|':
							if (prevSpecial != '?') {
								cur.Append(c);
								break;
							}
							prevSpecial = c;
							branch1 = cur.ToString();
							cur.Clear();
							break;
						case ':':
							if (prevSpecial == '|') {
								branch2 = cur.ToString();
								end = i;
							}
							if (prevSpecial == '?') {
								cur.Append(c);
								break;
							}
							var format = "";
							for (; i < str.Length; i++) {
								c = str[i];
								switch (c) {
									case esc:
										switch (str[i + 1]) {
											case '}':
												format += '}';
												i++;
												break;
											default:
												format += c;
												break;
										}
										break;
									case '}':
										end = i;
										var eval = Evaluate();
										if (eval is IFormattable formattable)
											return formattable.ToString(format, null);
										return eval.ToString();
									default:
										format += c;
										break;
								}
							}
							throw new SyntaxException("Unexpected end of string.");
						case '?':
							if (prevSpecial != default) {
								cur.Append(c);
								break;
							}
							selector = cur.ToString();
							prevSpecial = c;
							cur.Clear();
							break;
						default:
							cur.Append(c);
							break;
					}
				}
				throw new SyntaxException("Unexpected end of string.");
			}
		}

		public class SyntaxException : Exception {
			public SyntaxException(string message) : base(message) { }
			public SyntaxException() : base() { }
			public SyntaxException(string message, Exception innerException) : base(message, innerException) { }
		}

	}

}