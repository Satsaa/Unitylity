
namespace Unitylity.Systems.Lang {

	using System;
	using System.Collections.Concurrent;

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
	using Unitylity.Extensions;

	using Object = UnityEngine.Object;

#if UNITYLITY_SYSTEMS_LANG_HIDDEN
	[AddComponentMenu("")]
#else
	[AddComponentMenu("Unitylity/" + nameof(Systems.Lang) + "/" + nameof(Lang))]
#endif
	public class Lang : Singleton<Lang> {

		[field: SerializeField, Tooltip("Path to translations root folder in resources without leading or trailing slash.")]
		public string translationsPath { get; private set; } = "Lang";

		[field: SerializeField, Tooltip("Name of the language in use or to be used. Language names are C# CultureInfo names.")]
		public string language { get; private set; } = "en-US";

		[field: SerializeField, Tooltip("List of languages. Language names are C# CultureInfo names.")]
		public List<string> languages { get; private set; } = new List<string>() { "en-US" };


		Dictionary<string, string> strings;

		protected virtual Func<IFormattable, string> GetFormatter(string format) {
			return format switch {
				_ => null,
			};
		}

		protected virtual Func<string, string> GetStringFormatter(string format) {
			return format switch {
				"C" => (v) => v.ToString().Capitalize(),
				"LC" => (v) => v.ToString().ToLower().Capitalize(),
				"W" => (v) => WordCap(v.ToString()),
				"LW" => (v) => WordCap(v.ToString().ToLower()),
				"U" => (v) => v.ToString().ToUpper(),
				"L" => (v) => v.ToString(),
				_ => null,
			};
		}


		public static bool LoadLanguage(string language) {
			try {
				var ta = Resources.Load<TextAsset>($"{instance.translationsPath}/{language}");
				try {

					var strings = new Dictionary<string, string>(StringComparer.Ordinal);
					JsonConvert.PopulateObject(ta.text, strings);
					if (strings == null) {
						return false;
					}
					instance.strings = strings;
					instance.language = language;
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
					return false;

				}
			} catch (Exception) {
				return false;
			}
		}

		public static bool HasStr(string strId) {
			return instance.strings.ContainsKey(strId);
		}

		public static bool TryGetStr(string strId, out string str, IReadOnlyDictionary<string, object> context = null) {
			if (instance.strings.TryGetValue(strId, out str)) {
				str = Format(str, context);
				return true;
			}
			return false;
		}


		public static string GetStr(string strId, IReadOnlyDictionary<string, object> context = null) {
			if (instance.strings.TryGetValue(strId, out var res)) return Format(res, context);
			return strId;
		}
		public static string GetStr(string strId, string defaultStr, IReadOnlyDictionary<string, object> context = null) {
			if (instance.strings.TryGetValue(strId, out var res)) return Format(res, context);
			return defaultStr;
		}

#if UNITY_EDITOR
		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnScriptsReloaded() {
			if (instance) LoadLanguage(instance.language);
		}
#endif

		public class BuilderPool {
			static readonly ConcurrentBag<StringBuilder> builders = new();
			static public StringBuilder Get() => builders.TryTake(out var item) ? item.Clear() : new StringBuilder(512);
			static public void Return(StringBuilder item) => builders.Add(item);
			static public string StringReturn(StringBuilder item) {
				var res = item.ToString();
				builders.Add(item);
				return res;
			}
		}

		static string WordCap(string input) {
			if (input == null) return input;

			var builder = new StringBuilder(input.Length);

			bool capitalizeNext = true;
			foreach (char c in input) {
				if (char.IsWhiteSpace(c)) {
					capitalizeNext = true;
					builder.Append(c);
				} else {
					builder.Append(capitalizeNext ? char.ToUpper(c) : c);
					capitalizeNext = false;
				}
			}

			return builder.ToString();
		}

		public static string Format(in string str, IReadOnlyDictionary<string, object> context = null) => Format(in str, context, 0);
		private static string Format(in string str, IReadOnlyDictionary<string, object> context, int depth) {

			if (instance.strings == null) {
				if (string.IsNullOrWhiteSpace(instance.language)) {
					return str;
				}
				LoadLanguage(instance.language);
			}

			var builder = BuilderPool.Get();
			for (int i = 0; i < str.Length; i++) {
				var c = str[i];
				switch (c) {
					case '/':
						if (i + 1 >= str.Length) break; // Escaped end of string
						var next = str[i + 1];
						if (next == '{' || next == '/') {
							builder.Append(next);
							i++;
						} else {
							builder.Append(c);
						}
						break;
					case '{':
						i++;
						builder.Append(FormatToken(in str, true, false, depth, i, out i));
						break;
					default:
						builder.Append(c);
						break;
				}
			}
			return BuilderPool.StringReturn(builder);

			string FormatToken(in string str, bool isEntry, bool isParam, int depth, int start, out int end) {
				char prevSpecial = default;
				int wordStart = start;
				var builder = BuilderPool.Get();
				string selector = null;
				string branch1 = null;
				string branch2 = null;

				object Evaluate() {
					if (selector == null) return "[NO SELECTOR]";
					if (selector.Length > 0 && selector[0] == '@') {
						if (context != null && context.TryGetValue(selector[1..], out var val)) {
							if (branch1 != null) {
								if (branch2 == null) return selector; // Fail
								return val switch {
									bool v => v ? branch2 : branch1,
									IComparable v => v.CompareTo(1) == 0 ? branch2 : branch1,
									_ => val,
								};
							}
							if (val is string stringVal) {
								if (depth > 10) return "[RECURSIVE]";
								return isParam || isEntry ? Format(GetStr(stringVal), context, depth + 1) : stringVal;
							}
							return val;
						}
						return selector; // Fail find from context
					} else {
						if (depth > 10) return "[RECURSIVE]";
						return isParam || isEntry ? Format(GetStr(selector), context, depth + 1) : selector;
						string GetStr(string strId) {
							if (instance.strings != null && instance.strings.TryGetValue(strId, out var res)) return res;
							return strId;
						}
					}
				}

				for (int i = start; i < str.Length; i++) {
					var c = str[i];
					switch (c) {
						case '/':
							if (i + 1 >= str.Length) {
								// Early end
								BuilderPool.Return(builder);
								end = start + str.Length;
								return str;
							}
							var next = str[i + 1];
							switch (next) {
								case '{':
								case '}':
								case '|':
								case ':':
								case '?':
								case '/':
									builder.Append(next);
									i++;
									break;
								default:
									builder.Append(c);
									break;
							}
							break;
						case '{':
							i++;
							builder.Append(FormatToken(in str, false, prevSpecial != default, depth + 1, i, out i));
							break;
						case '}':
							if (prevSpecial == default) {
								selector = builder.ToString();
							} else if (prevSpecial == '|') {
								branch2 = builder.ToString();
							}
							end = i;
							BuilderPool.Return(builder);
							return Evaluate().ToString();
						case '|':
							if (prevSpecial != '?') {
								builder.Append(c);
								break;
							}
							prevSpecial = c;
							branch1 = builder.ToString();
							builder.Clear();
							break;
						case ':':
							if (prevSpecial == '?') {
								builder.Append(c);
								break;
							}
							if (prevSpecial == default) {
								selector = builder.ToString();
							} else if (prevSpecial == '|') {
								branch2 = builder.ToString();
							}
							end = i;
							i++;
							var format = "";
							for (; i < str.Length; i++) {
								c = str[i];
								switch (c) {
									case '/':
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
										BuilderPool.Return(builder);
										if (eval is IFormattable formattable) {
											var formatter = instance.GetFormatter(format);
											if (formatter != null) return formatter(formattable);
											return formattable.ToString(format, null);
										} else {
											var strFormatter = instance.GetStringFormatter(format);
											return strFormatter == null ? eval.ToString() : strFormatter(eval.ToString());

										}
									default:
										format += c;
										break;
								}
							}
							// Early end
							BuilderPool.Return(builder);
							end = start + str.Length;
							return str;
						case '?':
							if (prevSpecial != default) {
								builder.Append(c);
								break;
							}
							selector = builder.ToString();
							prevSpecial = c;
							builder.Clear();
							break;
						default:
							builder.Append(c);
							break;
					}
				}
				// Early end
				BuilderPool.Return(builder);
				end = start + str.Length;
				return str;

			}
		}

	}

}
