using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;
using Klei;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x02000943 RID: 2371
public static class Localization
{
	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x060044FF RID: 17663 RVA: 0x00189317 File Offset: 0x00187517
	public static TMP_FontAsset FontAsset
	{
		get
		{
			return Localization.sFontAsset;
		}
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06004500 RID: 17664 RVA: 0x0018931E File Offset: 0x0018751E
	public static bool IsRightToLeft
	{
		get
		{
			return Localization.sLocale != null && Localization.sLocale.IsRightToLeft;
		}
	}

	// Token: 0x06004501 RID: 17665 RVA: 0x00189334 File Offset: 0x00187534
	private static IEnumerable<Type> CollectLocStringTreeRoots(string locstrings_namespace, Assembly assembly)
	{
		return from type in assembly.GetTypes()
		where type.IsClass && type.Namespace == locstrings_namespace && !type.IsNested
		select type;
	}

	// Token: 0x06004502 RID: 17666 RVA: 0x00189368 File Offset: 0x00187568
	private static Dictionary<string, object> MakeRuntimeLocStringTree(Type locstring_tree_root)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (FieldInfo fieldInfo in locstring_tree_root.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + fieldInfo.Name);
				}
				else
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					if (locString == null)
					{
						global::Debug.LogError("Tried to generate LocString for " + fieldInfo.Name + " but it is null so skipping");
					}
					else
					{
						dictionary[fieldInfo.Name] = locString.text;
					}
				}
			}
		}
		foreach (Type type in locstring_tree_root.GetNestedTypes())
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		return dictionary;
	}

	// Token: 0x06004503 RID: 17667 RVA: 0x00189450 File Offset: 0x00187650
	private static void WriteStringsTemplate(string path, StreamWriter writer, Dictionary<string, object> runtime_locstring_tree)
	{
		List<string> list = new List<string>(runtime_locstring_tree.Keys);
		list.Sort();
		foreach (string text in list)
		{
			string text2 = path + "." + text;
			object obj = runtime_locstring_tree[text];
			if (obj.GetType() != typeof(string))
			{
				Localization.WriteStringsTemplate(text2, writer, obj as Dictionary<string, object>);
			}
			else
			{
				string text3 = obj as string;
				text3 = text3.Replace("\\", "\\\\");
				text3 = text3.Replace("\"", "\\\"");
				text3 = text3.Replace("\n", "\\n");
				text3 = text3.Replace("’", "'");
				text3 = text3.Replace("“", "\\\"");
				text3 = text3.Replace("”", "\\\"");
				text3 = text3.Replace("…", "...");
				writer.WriteLine("#. " + text2);
				writer.WriteLine("msgctxt \"{0}\"", text2);
				writer.WriteLine("msgid \"" + text3 + "\"");
				writer.WriteLine("msgstr \"\"");
				writer.WriteLine("");
			}
		}
	}

	// Token: 0x06004504 RID: 17668 RVA: 0x001895D0 File Offset: 0x001877D0
	public static void GenerateStringsTemplate(string locstrings_namespace, Assembly assembly, string output_filename, Dictionary<string, object> current_runtime_locstring_forest)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstrings_namespace, assembly))
		{
			Dictionary<string, object> dictionary2 = Localization.MakeRuntimeLocStringTree(type);
			if (dictionary2.Count > 0)
			{
				dictionary[type.Name] = dictionary2;
			}
		}
		if (current_runtime_locstring_forest != null)
		{
			dictionary.Concat(current_runtime_locstring_forest);
		}
		using (StreamWriter streamWriter = new StreamWriter(output_filename, false, new UTF8Encoding(false)))
		{
			streamWriter.WriteLine("msgid \"\"");
			streamWriter.WriteLine("msgstr \"\"");
			streamWriter.WriteLine("\"Application: Oxygen Not Included\"");
			streamWriter.WriteLine("\"POT Version: 2.0\"");
			streamWriter.WriteLine("");
			Localization.WriteStringsTemplate(locstrings_namespace, streamWriter, dictionary);
		}
		DebugUtil.LogArgs(new object[]
		{
			"Generated " + output_filename
		});
	}

	// Token: 0x06004505 RID: 17669 RVA: 0x001896CC File Offset: 0x001878CC
	public static void GenerateStringsTemplate(Type locstring_tree_root, string output_folder)
	{
		output_folder = FileSystem.Normalize(output_folder);
		if (!FileUtil.CreateDirectory(output_folder, 5))
		{
			return;
		}
		Localization.GenerateStringsTemplate(locstring_tree_root.Namespace, Assembly.GetAssembly(locstring_tree_root), FileSystem.Normalize(Path.Combine(output_folder, string.Format("{0}_template.pot", locstring_tree_root.Namespace.ToLower()))), null);
	}

	// Token: 0x06004506 RID: 17670 RVA: 0x00189720 File Offset: 0x00187920
	public static void Initialize()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Localization.Initialize!"
		});
		bool flag = false;
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			Localization.sFontAsset = Localization.GetFont(Localization.GetDefaultLocale().FontName);
			break;
		case Localization.SelectedLanguageType.Preinstalled:
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (!string.IsNullOrEmpty(currentLanguageCode))
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Preinstalled localization"
				});
				DebugUtil.LogArgs(new object[]
				{
					" -> ",
					currentLanguageCode
				});
				Localization.LoadPreinstalledTranslation(currentLanguageCode);
			}
			else
			{
				flag = true;
			}
			break;
		}
		case Localization.SelectedLanguageType.UGC:
			if (LanguageOptionsScreen.HasInstalledLanguage())
			{
				DebugUtil.LogArgs(new object[]
				{
					"Localization Initialize... Mod-based localization"
				});
				string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
				if (LanguageOptionsScreen.SetCurrentLanguage(savedLanguageMod))
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Loaded language from mod: " + savedLanguageMod
					});
				}
				else
				{
					DebugUtil.LogArgs(new object[]
					{
						" -> Failed to load language from mod: " + savedLanguageMod
					});
				}
			}
			else
			{
				flag = true;
			}
			break;
		}
		if (flag)
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x06004507 RID: 17671 RVA: 0x00189824 File Offset: 0x00187A24
	public static void VerifyTranslationModSubscription(GameObject context)
	{
		if (Localization.GetSelectedLanguageType() != Localization.SelectedLanguageType.UGC)
		{
			return;
		}
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (LanguageOptionsScreen.HasInstalledLanguage())
		{
			return;
		}
		PublishedFileId_t publishedFileId_t = new PublishedFileId_t((ulong)KPlayerPrefs.GetInt("InstalledLanguage", (int)PublishedFileId_t.Invalid.m_PublishedFileId));
		Label rhs = new Label
		{
			distribution_platform = Label.DistributionPlatform.Steam,
			id = publishedFileId_t.ToString()
		};
		string arg = UI.FRONTEND.TRANSLATIONS_SCREEN.UNKNOWN;
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if (mod.label.Match(rhs))
			{
				arg = mod.title;
				break;
			}
		}
		Localization.ClearLanguage();
		KScreen component = KScreenManager.AddChild(context, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		ConfirmDialogScreen component2 = component.GetComponent<ConfirmDialogScreen>();
		string title_text = UI.CONFIRMDIALOG.DIALOG_HEADER;
		string text = string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.MISSING_LANGUAGE_PACK, arg);
		string confirm_text = UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART;
		component2.PopupConfirmDialog(text, new System.Action(App.instance.Restart), null, null, null, title_text, confirm_text, null, null);
	}

	// Token: 0x06004508 RID: 17672 RVA: 0x0018996C File Offset: 0x00187B6C
	public static void LoadPreinstalledTranslation(string code)
	{
		if (!string.IsNullOrEmpty(code) && code != Localization.DEFAULT_LANGUAGE_CODE)
		{
			string preinstalledLocalizationFilePath = Localization.GetPreinstalledLocalizationFilePath(code);
			if (Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.Preinstalled, preinstalledLocalizationFilePath))
			{
				KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, code);
				return;
			}
		}
		else
		{
			Localization.ClearLanguage();
		}
	}

	// Token: 0x06004509 RID: 17673 RVA: 0x001899AF File Offset: 0x00187BAF
	public static bool LoadLocalTranslationFile(Localization.SelectedLanguageType source, string path)
	{
		if (!File.Exists(path))
		{
			return false;
		}
		bool flag = Localization.LoadTranslationFromLines(File.ReadAllLines(path, Encoding.UTF8));
		if (flag)
		{
			KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, source.ToString());
			return flag;
		}
		Localization.ClearLanguage();
		return flag;
	}

	// Token: 0x0600450A RID: 17674 RVA: 0x001899EC File Offset: 0x00187BEC
	private static bool LoadTranslationFromLines(string[] lines)
	{
		if (lines == null || lines.Length == 0)
		{
			return false;
		}
		Localization.sLocale = Localization.GetLocale(lines);
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
		bool flag = Localization.LoadTranslation(lines, false);
		if (flag)
		{
			Localization.currentFontName = Localization.GetFontName(lines);
			Localization.SwapToLocalizedFont(Localization.currentFontName);
		}
		return flag;
	}

	// Token: 0x0600450B RID: 17675 RVA: 0x00189A50 File Offset: 0x00187C50
	public static bool LoadTranslation(string[] lines, bool isTemplate = false)
	{
		bool result;
		try
		{
			Localization.OverloadStrings(Localization.ExtractTranslatedStrings(lines, isTemplate));
			result = true;
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex
			});
			result = false;
		}
		return result;
	}

	// Token: 0x0600450C RID: 17676 RVA: 0x00189A94 File Offset: 0x00187C94
	public static Dictionary<string, string> LoadStringsFile(string path, bool isTemplate)
	{
		return Localization.ExtractTranslatedStrings(File.ReadAllLines(path, Encoding.UTF8), isTemplate);
	}

	// Token: 0x0600450D RID: 17677 RVA: 0x00189AA8 File Offset: 0x00187CA8
	public static Dictionary<string, string> ExtractTranslatedStrings(string[] lines, bool isTemplate = false)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Localization.Entry entry = default(Localization.Entry);
		string key = isTemplate ? "msgid" : "msgstr";
		for (int i = 0; i < lines.Length; i++)
		{
			string text = lines[i];
			if (text == null || text.Length == 0)
			{
				entry = default(Localization.Entry);
			}
			else
			{
				string parameter = Localization.GetParameter("msgctxt", i, lines);
				if (parameter != null)
				{
					entry.msgctxt = parameter;
				}
				parameter = Localization.GetParameter(key, i, lines);
				if (parameter != null)
				{
					entry.msgstr = parameter;
				}
			}
			if (entry.IsPopulated)
			{
				dictionary[entry.msgctxt] = entry.msgstr;
				entry = default(Localization.Entry);
			}
		}
		return dictionary;
	}

	// Token: 0x0600450E RID: 17678 RVA: 0x00189B54 File Offset: 0x00187D54
	private static string FixupString(string result)
	{
		result = result.Replace("\\n", "\n");
		result = result.Replace("\\\"", "\"");
		result = result.Replace("<style=“", "<style=\"");
		result = result.Replace("”>", "\">");
		result = result.Replace("<color=^p", "<color=#");
		return result;
	}

	// Token: 0x0600450F RID: 17679 RVA: 0x00189BBC File Offset: 0x00187DBC
	private static string GetParameter(string key, int idx, string[] all_lines)
	{
		if (!all_lines[idx].StartsWith(key))
		{
			return null;
		}
		List<string> list = new List<string>();
		string text = all_lines[idx];
		text = text.Substring(key.Length + 1, text.Length - key.Length - 1);
		list.Add(text);
		for (int i = idx + 1; i < all_lines.Length; i++)
		{
			string text2 = all_lines[i];
			if (!text2.StartsWith("\""))
			{
				break;
			}
			list.Add(text2);
		}
		string text3 = "";
		foreach (string text4 in list)
		{
			if (text4.EndsWith("\r"))
			{
				text4 = text4.Substring(0, text4.Length - 1);
			}
			text4 = text4.Substring(1, text4.Length - 2);
			text4 = Localization.FixupString(text4);
			text3 += text4;
		}
		return text3;
	}

	// Token: 0x06004510 RID: 17680 RVA: 0x00189CBC File Offset: 0x00187EBC
	private static void AddAssembly(string locstrings_namespace, Assembly assembly)
	{
		List<Assembly> list;
		if (!Localization.translatable_assemblies.TryGetValue(locstrings_namespace, out list))
		{
			list = new List<Assembly>();
			Localization.translatable_assemblies.Add(locstrings_namespace, list);
		}
		list.Add(assembly);
	}

	// Token: 0x06004511 RID: 17681 RVA: 0x00189CF1 File Offset: 0x00187EF1
	public static void AddAssembly(Assembly assembly)
	{
		Localization.AddAssembly("STRINGS", assembly);
	}

	// Token: 0x06004512 RID: 17682 RVA: 0x00189D00 File Offset: 0x00187F00
	public static void RegisterForTranslation(Type locstring_tree_root)
	{
		Assembly assembly = Assembly.GetAssembly(locstring_tree_root);
		Localization.AddAssembly(locstring_tree_root.Namespace, assembly);
		string parent_path = locstring_tree_root.Namespace + ".";
		foreach (Type type in Localization.CollectLocStringTreeRoots(locstring_tree_root.Namespace, assembly))
		{
			LocString.CreateLocStringKeys(type, parent_path);
		}
	}

	// Token: 0x06004513 RID: 17683 RVA: 0x00189D78 File Offset: 0x00187F78
	public static void OverloadStrings(Dictionary<string, string> translated_strings)
	{
		string text = "";
		string text2 = "";
		string text3 = "";
		foreach (KeyValuePair<string, List<Assembly>> keyValuePair in Localization.translatable_assemblies)
		{
			foreach (Assembly assembly in keyValuePair.Value)
			{
				foreach (Type type in Localization.CollectLocStringTreeRoots(keyValuePair.Key, assembly))
				{
					string path = keyValuePair.Key + "." + type.Name;
					Localization.OverloadStrings(translated_strings, path, type, ref text, ref text2, ref text3);
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have missing or mismatched parameters:\n" + text
			});
		}
		if (!string.IsNullOrEmpty(text2))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following have mismatched <link> tags:\n" + text2
			});
		}
		if (!string.IsNullOrEmpty(text3))
		{
			DebugUtil.LogArgs(new object[]
			{
				"TRANSLATION ERROR! The following do not have the same amount of <link> tags as the english string which can cause nested link errors:\n" + text3
			});
		}
	}

	// Token: 0x06004514 RID: 17684 RVA: 0x00189EEC File Offset: 0x001880EC
	public static void OverloadStrings(Dictionary<string, string> translated_strings, string path, Type locstring_hierarchy, ref string parameter_errors, ref string link_errors, ref string link_count_errors)
	{
		foreach (FieldInfo fieldInfo in locstring_hierarchy.GetFields())
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				string text = path + "." + fieldInfo.Name;
				string text2 = null;
				if (translated_strings.TryGetValue(text, out text2))
				{
					LocString locString = (LocString)fieldInfo.GetValue(null);
					LocString value = new LocString(text2, text);
					if (!Localization.AreParametersPreserved(locString.text, text2))
					{
						parameter_errors = parameter_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasSameOrLessLinkCountAsEnglish(locString.text, text2))
					{
						link_count_errors = link_count_errors + "\t" + text + "\n";
					}
					else if (!Localization.HasMatchingLinkTags(text2, 0))
					{
						link_errors = link_errors + "\t" + text + "\n";
					}
					else
					{
						fieldInfo.SetValue(null, value);
					}
				}
			}
		}
		foreach (Type type in locstring_hierarchy.GetNestedTypes())
		{
			string path2 = path + "." + type.Name;
			Localization.OverloadStrings(translated_strings, path2, type, ref parameter_errors, ref link_errors, ref link_count_errors);
		}
	}

	// Token: 0x06004515 RID: 17685 RVA: 0x0018A026 File Offset: 0x00188226
	public static string GetDefaultLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings_template.pot");
	}

	// Token: 0x06004516 RID: 17686 RVA: 0x0018A037 File Offset: 0x00188237
	public static string GetModLocalizationFilePath()
	{
		return Path.Combine(Application.streamingAssetsPath, "strings/strings.po");
	}

	// Token: 0x06004517 RID: 17687 RVA: 0x0018A048 File Offset: 0x00188248
	public static string GetPreinstalledLocalizationFilePath(string code)
	{
		string path = "strings/strings_preinstalled_" + code + ".po";
		return Path.Combine(Application.streamingAssetsPath, path);
	}

	// Token: 0x06004518 RID: 17688 RVA: 0x0018A071 File Offset: 0x00188271
	public static string GetPreinstalledLocalizationTitle(string code)
	{
		return Strings.Get("STRINGS.UI.FRONTEND.TRANSLATIONS_SCREEN.PREINSTALLED_LANGUAGES." + code.ToUpper());
	}

	// Token: 0x06004519 RID: 17689 RVA: 0x0018A090 File Offset: 0x00188290
	public static Texture2D GetPreinstalledLocalizationImage(string code)
	{
		string path = Path.Combine(Application.streamingAssetsPath, "strings/preinstalled_icon_" + code + ".png");
		if (File.Exists(path))
		{
			byte[] data = File.ReadAllBytes(path);
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
			return texture2D;
		}
		return null;
	}

	// Token: 0x0600451A RID: 17690 RVA: 0x0018A0D8 File Offset: 0x001882D8
	public static void SetLocale(Localization.Locale locale)
	{
		Localization.sLocale = locale;
		DebugUtil.LogArgs(new object[]
		{
			" -> Locale is now ",
			Localization.sLocale.ToString()
		});
	}

	// Token: 0x0600451B RID: 17691 RVA: 0x0018A100 File Offset: 0x00188300
	public static Localization.Locale GetLocale()
	{
		return Localization.sLocale;
	}

	// Token: 0x0600451C RID: 17692 RVA: 0x0018A108 File Offset: 0x00188308
	private static string GetFontParam(string line)
	{
		string text = null;
		if (line.StartsWith("\"Font:"))
		{
			text = line.Substring("\"Font:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x0600451D RID: 17693 RVA: 0x0018A160 File Offset: 0x00188360
	public static string GetCurrentLanguageCode()
	{
		switch (Localization.GetSelectedLanguageType())
		{
		case Localization.SelectedLanguageType.None:
			return Localization.DEFAULT_LANGUAGE_CODE;
		case Localization.SelectedLanguageType.Preinstalled:
			return KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_CODE_KEY);
		case Localization.SelectedLanguageType.UGC:
			return LanguageOptionsScreen.GetInstalledLanguageCode();
		default:
			return "";
		}
	}

	// Token: 0x0600451E RID: 17694 RVA: 0x0018A1A4 File Offset: 0x001883A4
	public static Localization.SelectedLanguageType GetSelectedLanguageType()
	{
		return (Localization.SelectedLanguageType)Enum.Parse(typeof(Localization.SelectedLanguageType), KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()), true);
	}

	// Token: 0x0600451F RID: 17695 RVA: 0x0018A1E0 File Offset: 0x001883E0
	private static string GetLanguageCode(string line)
	{
		string text = null;
		if (line.StartsWith("\"Language:"))
		{
			text = line.Substring("\"Language:".Length).Trim();
			text = text.Replace("\\n", "");
			text = text.Replace("\"", "");
		}
		return text;
	}

	// Token: 0x06004520 RID: 17696 RVA: 0x0018A238 File Offset: 0x00188438
	private static Localization.Locale GetLocaleForCode(string code)
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesCode(code))
			{
				result = locale;
				break;
			}
		}
		return result;
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x0018A294 File Offset: 0x00188494
	public static Localization.Locale GetLocale(string[] lines)
	{
		Localization.Locale locale = null;
		string text = null;
		if (lines != null && lines.Length != 0)
		{
			foreach (string text2 in lines)
			{
				if (text2 != null && text2.Length != 0)
				{
					text = Localization.GetLanguageCode(text2);
					if (text != null)
					{
						locale = Localization.GetLocaleForCode(text);
					}
					if (text != null)
					{
						break;
					}
				}
			}
		}
		if (locale == null)
		{
			locale = Localization.GetDefaultLocale();
		}
		if (text != null && locale.Code == "")
		{
			locale.SetCode(text);
		}
		return locale;
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x0018A309 File Offset: 0x00188509
	private static string GetFontName(string filename)
	{
		return Localization.GetFontName(File.ReadAllLines(filename, Encoding.UTF8));
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x0018A31C File Offset: 0x0018851C
	public static Localization.Locale GetDefaultLocale()
	{
		Localization.Locale result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = new Localization.Locale(locale);
				break;
			}
		}
		return result;
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x0018A37C File Offset: 0x0018857C
	public static string GetDefaultFontName()
	{
		string result = null;
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.Lang == Localization.Language.Unspecified)
			{
				result = locale.FontName;
				break;
			}
		}
		return result;
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x0018A3DC File Offset: 0x001885DC
	public static string ValidateFontName(string fontName)
	{
		foreach (Localization.Locale locale in Localization.Locales)
		{
			if (locale.MatchesFont(fontName))
			{
				return locale.FontName;
			}
		}
		return null;
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x0018A43C File Offset: 0x0018863C
	public static string GetFontName(string[] lines)
	{
		string text = null;
		if (lines != null)
		{
			foreach (string text2 in lines)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					string fontParam = Localization.GetFontParam(text2);
					if (fontParam != null)
					{
						text = Localization.ValidateFontName(fontParam);
					}
				}
				if (text != null)
				{
					break;
				}
			}
		}
		if (text == null)
		{
			if (Localization.sLocale != null)
			{
				text = Localization.sLocale.FontName;
			}
			else
			{
				text = Localization.GetDefaultFontName();
			}
		}
		return text;
	}

	// Token: 0x06004527 RID: 17703 RVA: 0x0018A49F File Offset: 0x0018869F
	public static void SwapToLocalizedFont()
	{
		Localization.SwapToLocalizedFont(Localization.currentFontName);
	}

	// Token: 0x06004528 RID: 17704 RVA: 0x0018A4AC File Offset: 0x001886AC
	public static bool SwapToLocalizedFont(string fontname)
	{
		if (string.IsNullOrEmpty(fontname))
		{
			return false;
		}
		Localization.sFontAsset = Localization.GetFont(fontname);
		foreach (TextStyleSetting textStyleSetting in Resources.FindObjectsOfTypeAll<TextStyleSetting>())
		{
			if (textStyleSetting != null)
			{
				textStyleSetting.sdfFont = Localization.sFontAsset;
			}
		}
		bool isRightToLeft = Localization.IsRightToLeft;
		foreach (LocText locText in Resources.FindObjectsOfTypeAll<LocText>())
		{
			if (locText != null)
			{
				locText.SwapFont(Localization.sFontAsset, isRightToLeft);
			}
		}
		return true;
	}

	// Token: 0x06004529 RID: 17705 RVA: 0x0018A534 File Offset: 0x00188734
	private static bool SetFont(Type target_type, object target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		if (target_type == null || target == null || font == null)
		{
			return false;
		}
		foreach (FieldInfo fieldInfo in target_type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (!excluded_members.Contains(fieldInfo))
			{
				if (fieldInfo.FieldType == typeof(TextStyleSetting))
				{
					((TextStyleSetting)fieldInfo.GetValue(target)).sdfFont = font;
				}
				else if (fieldInfo.FieldType == typeof(LocText))
				{
					((LocText)fieldInfo.GetValue(target)).SwapFont(font, is_right_to_left);
				}
				else if (fieldInfo.FieldType == typeof(GameObject))
				{
					foreach (Component component in ((GameObject)fieldInfo.GetValue(target)).GetComponents<Component>())
					{
						Localization.SetFont(component.GetType(), component, font, is_right_to_left, excluded_members);
					}
				}
				else if (fieldInfo.MemberType == MemberTypes.Field && fieldInfo.FieldType != fieldInfo.DeclaringType)
				{
					Localization.SetFont(fieldInfo.FieldType, fieldInfo.GetValue(target), font, is_right_to_left, excluded_members);
				}
			}
		}
		return true;
	}

	// Token: 0x0600452A RID: 17706 RVA: 0x0018A66D File Offset: 0x0018886D
	public static bool SetFont<T>(T target, TMP_FontAsset font, bool is_right_to_left, HashSet<MemberInfo> excluded_members)
	{
		return Localization.SetFont(typeof(T), target, font, is_right_to_left, excluded_members);
	}

	// Token: 0x0600452B RID: 17707 RVA: 0x0018A688 File Offset: 0x00188888
	public static TMP_FontAsset GetFont(string fontname)
	{
		foreach (TMP_FontAsset tmp_FontAsset in Resources.FindObjectsOfTypeAll<TMP_FontAsset>())
		{
			if (tmp_FontAsset.name == fontname)
			{
				return tmp_FontAsset;
			}
		}
		return null;
	}

	// Token: 0x0600452C RID: 17708 RVA: 0x0018A6C0 File Offset: 0x001888C0
	private static bool HasSameOrLessTokenCount(string english_string, string translated_string, string token)
	{
		int num = english_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		int num2 = translated_string.Split(new string[]
		{
			token
		}, StringSplitOptions.None).Length;
		return num >= num2;
	}

	// Token: 0x0600452D RID: 17709 RVA: 0x0018A6FC File Offset: 0x001888FC
	private static bool HasSameOrLessLinkCountAsEnglish(string english_string, string translated_string)
	{
		return Localization.HasSameOrLessTokenCount(english_string, translated_string, "<link") && Localization.HasSameOrLessTokenCount(english_string, translated_string, "</link");
	}

	// Token: 0x0600452E RID: 17710 RVA: 0x0018A71C File Offset: 0x0018891C
	private static bool HasMatchingLinkTags(string str, int idx = 0)
	{
		int num = str.IndexOf("<link", idx);
		int num2 = str.IndexOf("</link", idx);
		if (num == -1 && num2 == -1)
		{
			return true;
		}
		if (num == -1 && num2 != -1)
		{
			return false;
		}
		if (num != -1 && num2 == -1)
		{
			return false;
		}
		if (num2 < num)
		{
			return false;
		}
		int num3 = str.IndexOf("<link", num + 1);
		return (num < 0 || num3 == -1 || num3 >= num2) && Localization.HasMatchingLinkTags(str, num2 + 1);
	}

	// Token: 0x0600452F RID: 17711 RVA: 0x0018A790 File Offset: 0x00188990
	private static bool AreParametersPreserved(string old_string, string new_string)
	{
		MatchCollection matchCollection = Regex.Matches(old_string, "({.[^}]*?})(?!.*\\1)");
		MatchCollection matchCollection2 = Regex.Matches(new_string, "({.[^}]*?})(?!.*\\1)");
		bool result = false;
		if (matchCollection == null && matchCollection2 == null)
		{
			result = true;
		}
		else if (matchCollection != null && matchCollection2 != null && matchCollection.Count == matchCollection2.Count)
		{
			result = true;
			foreach (object obj in matchCollection)
			{
				string a = obj.ToString();
				bool flag = false;
				foreach (object obj2 in matchCollection2)
				{
					string b = obj2.ToString();
					if (a == b)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x06004530 RID: 17712 RVA: 0x0018A888 File Offset: 0x00188A88
	public static bool HasDirtyWords(string str)
	{
		return Localization.FilterDirtyWords(str) != str;
	}

	// Token: 0x06004531 RID: 17713 RVA: 0x0018A896 File Offset: 0x00188A96
	public static string FilterDirtyWords(string str)
	{
		return DistributionPlatform.Inst.ApplyWordFilter(str);
	}

	// Token: 0x06004532 RID: 17714 RVA: 0x0018A8A3 File Offset: 0x00188AA3
	public static string GetFileDateFormat(int format_idx)
	{
		return "{" + format_idx.ToString() + ":dd / MMM / yyyy}";
	}

	// Token: 0x06004533 RID: 17715 RVA: 0x0018A8BC File Offset: 0x00188ABC
	public static void ClearLanguage()
	{
		DebugUtil.LogArgs(new object[]
		{
			" -> Clearing selected language! Either it didn't load correct or returning to english by menu."
		});
		Localization.sFontAsset = null;
		Localization.sLocale = null;
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString());
		KPlayerPrefs.SetString(Localization.SELECTED_LANGUAGE_CODE_KEY, "");
		Localization.SwapToLocalizedFont(Localization.GetDefaultLocale().FontName);
		string defaultLocalizationFilePath = Localization.GetDefaultLocalizationFilePath();
		if (File.Exists(defaultLocalizationFilePath))
		{
			Localization.LoadTranslation(File.ReadAllLines(defaultLocalizationFilePath, Encoding.UTF8), true);
		}
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
	}

	// Token: 0x06004534 RID: 17716 RVA: 0x0018A948 File Offset: 0x00188B48
	private static string ReverseText(string source)
	{
		char[] separator = new char[]
		{
			'\n'
		};
		string[] array = source.Split(separator);
		string text = "";
		int num = 0;
		foreach (string text2 in array)
		{
			num++;
			char[] array3 = new char[text2.Length];
			for (int j = 0; j < text2.Length; j++)
			{
				array3[array3.Length - 1 - j] = text2[j];
			}
			text += new string(array3);
			if (num < array.Length)
			{
				text += "\n";
			}
		}
		return text;
	}

	// Token: 0x06004535 RID: 17717 RVA: 0x0018A9EC File Offset: 0x00188BEC
	public static string Fixup(string text)
	{
		if (Localization.sLocale != null && text != null && text != "" && Localization.sLocale.Lang == Localization.Language.Arabic)
		{
			return Localization.ReverseText(ArabicFixer.Fix(text));
		}
		return text;
	}

	// Token: 0x04002D0F RID: 11535
	private static TMP_FontAsset sFontAsset = null;

	// Token: 0x04002D10 RID: 11536
	private static readonly List<Localization.Locale> Locales = new List<Localization.Locale>
	{
		new Localization.Locale(Localization.Language.Chinese, Localization.Direction.LeftToRight, "zh", "NotoSansCJKsc-Regular"),
		new Localization.Locale(Localization.Language.Japanese, Localization.Direction.LeftToRight, "ja", "NotoSansCJKjp-Regular"),
		new Localization.Locale(Localization.Language.Korean, Localization.Direction.LeftToRight, "ko", "NotoSansCJKkr-Regular"),
		new Localization.Locale(Localization.Language.Russian, Localization.Direction.LeftToRight, "ru", "RobotoCondensed-Regular"),
		new Localization.Locale(Localization.Language.Thai, Localization.Direction.LeftToRight, "th", "NotoSansThai-Regular"),
		new Localization.Locale(Localization.Language.Arabic, Localization.Direction.RightToLeft, "ar", "NotoNaskhArabic-Regular"),
		new Localization.Locale(Localization.Language.Hebrew, Localization.Direction.RightToLeft, "he", "NotoSansHebrew-Regular"),
		new Localization.Locale(Localization.Language.Unspecified, Localization.Direction.LeftToRight, "", "RobotoCondensed-Regular")
	};

	// Token: 0x04002D11 RID: 11537
	private static Localization.Locale sLocale = null;

	// Token: 0x04002D12 RID: 11538
	private static string currentFontName = null;

	// Token: 0x04002D13 RID: 11539
	public static string DEFAULT_LANGUAGE_CODE = "en";

	// Token: 0x04002D14 RID: 11540
	public static readonly List<string> PreinstalledLanguages = new List<string>
	{
		Localization.DEFAULT_LANGUAGE_CODE,
		"zh_klei",
		"ko_klei",
		"ru_klei"
	};

	// Token: 0x04002D15 RID: 11541
	public static string SELECTED_LANGUAGE_TYPE_KEY = "SelectedLanguageType";

	// Token: 0x04002D16 RID: 11542
	public static string SELECTED_LANGUAGE_CODE_KEY = "SelectedLanguageCode";

	// Token: 0x04002D17 RID: 11543
	private static Dictionary<string, List<Assembly>> translatable_assemblies = new Dictionary<string, List<Assembly>>();

	// Token: 0x04002D18 RID: 11544
	public const BindingFlags non_static_data_member_fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

	// Token: 0x04002D19 RID: 11545
	private const string start_link_token = "<link";

	// Token: 0x04002D1A RID: 11546
	private const string end_link_token = "</link";

	// Token: 0x020018A9 RID: 6313
	public enum Language
	{
		// Token: 0x04007710 RID: 30480
		Chinese,
		// Token: 0x04007711 RID: 30481
		Japanese,
		// Token: 0x04007712 RID: 30482
		Korean,
		// Token: 0x04007713 RID: 30483
		Russian,
		// Token: 0x04007714 RID: 30484
		Thai,
		// Token: 0x04007715 RID: 30485
		Arabic,
		// Token: 0x04007716 RID: 30486
		Hebrew,
		// Token: 0x04007717 RID: 30487
		Unspecified
	}

	// Token: 0x020018AA RID: 6314
	public enum Direction
	{
		// Token: 0x04007719 RID: 30489
		LeftToRight,
		// Token: 0x0400771A RID: 30490
		RightToLeft
	}

	// Token: 0x020018AB RID: 6315
	public class Locale
	{
		// Token: 0x0600998C RID: 39308 RVA: 0x0036A8EE File Offset: 0x00368AEE
		public Locale(Localization.Locale other)
		{
			this.mLanguage = other.mLanguage;
			this.mDirection = other.mDirection;
			this.mCode = other.mCode;
			this.mFontName = other.mFontName;
		}

		// Token: 0x0600998D RID: 39309 RVA: 0x0036A926 File Offset: 0x00368B26
		public Locale(Localization.Language language, Localization.Direction direction, string code, string fontName)
		{
			this.mLanguage = language;
			this.mDirection = direction;
			this.mCode = code.ToLower();
			this.mFontName = fontName;
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x0600998E RID: 39310 RVA: 0x0036A950 File Offset: 0x00368B50
		public Localization.Language Lang
		{
			get
			{
				return this.mLanguage;
			}
		}

		// Token: 0x0600998F RID: 39311 RVA: 0x0036A958 File Offset: 0x00368B58
		public void SetCode(string code)
		{
			this.mCode = code;
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06009990 RID: 39312 RVA: 0x0036A961 File Offset: 0x00368B61
		public string Code
		{
			get
			{
				return this.mCode;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06009991 RID: 39313 RVA: 0x0036A969 File Offset: 0x00368B69
		public string FontName
		{
			get
			{
				return this.mFontName;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06009992 RID: 39314 RVA: 0x0036A971 File Offset: 0x00368B71
		public bool IsRightToLeft
		{
			get
			{
				return this.mDirection == Localization.Direction.RightToLeft;
			}
		}

		// Token: 0x06009993 RID: 39315 RVA: 0x0036A97C File Offset: 0x00368B7C
		public bool MatchesCode(string language_code)
		{
			return language_code.ToLower().Contains(this.mCode);
		}

		// Token: 0x06009994 RID: 39316 RVA: 0x0036A98F File Offset: 0x00368B8F
		public bool MatchesFont(string fontname)
		{
			return fontname.ToLower() == this.mFontName.ToLower();
		}

		// Token: 0x06009995 RID: 39317 RVA: 0x0036A9A8 File Offset: 0x00368BA8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.mCode,
				":",
				this.mLanguage.ToString(),
				":",
				this.mDirection.ToString(),
				":",
				this.mFontName
			});
		}

		// Token: 0x0400771B RID: 30491
		private Localization.Language mLanguage;

		// Token: 0x0400771C RID: 30492
		private string mCode;

		// Token: 0x0400771D RID: 30493
		private string mFontName;

		// Token: 0x0400771E RID: 30494
		private Localization.Direction mDirection;
	}

	// Token: 0x020018AC RID: 6316
	private struct Entry
	{
		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06009996 RID: 39318 RVA: 0x0036AA12 File Offset: 0x00368C12
		public bool IsPopulated
		{
			get
			{
				return this.msgctxt != null && this.msgstr != null && this.msgstr.Length > 0;
			}
		}

		// Token: 0x0400771F RID: 30495
		public string msgctxt;

		// Token: 0x04007720 RID: 30496
		public string msgstr;
	}

	// Token: 0x020018AD RID: 6317
	public enum SelectedLanguageType
	{
		// Token: 0x04007722 RID: 30498
		None,
		// Token: 0x04007723 RID: 30499
		Preinstalled,
		// Token: 0x04007724 RID: 30500
		UGC
	}
}
