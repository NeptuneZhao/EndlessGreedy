using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using KMod;
using Steamworks;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C96 RID: 3222
public class LanguageOptionsScreen : KModalScreen, SteamUGCService.IClient
{
	// Token: 0x060062FC RID: 25340 RVA: 0x0024E2BC File Offset: 0x0024C4BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.dismissButton.onClick += this.Deactivate;
		this.dismissButton.GetComponent<HierarchyReferences>().GetReference<LocText>("Title").SetText(UI.FRONTEND.OPTIONS_SCREEN.BACK);
		this.closeButton.onClick += this.Deactivate;
		this.workshopButton.onClick += delegate()
		{
			this.OnClickOpenWorkshop();
		};
		this.uninstallButton.onClick += delegate()
		{
			this.OnClickUninstall();
		};
		this.uninstallButton.gameObject.SetActive(false);
		this.RebuildScreen();
	}

	// Token: 0x060062FD RID: 25341 RVA: 0x0024E368 File Offset: 0x0024C568
	private void RebuildScreen()
	{
		foreach (GameObject obj in this.buttons)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.buttons.Clear();
		this.uninstallButton.isInteractable = (KPlayerPrefs.GetString(Localization.SELECTED_LANGUAGE_TYPE_KEY, Localization.SelectedLanguageType.None.ToString()) != Localization.SelectedLanguageType.None.ToString());
		this.RebuildPreinstalledButtons();
		this.RebuildUGCButtons();
	}

	// Token: 0x060062FE RID: 25342 RVA: 0x0024E408 File Offset: 0x0024C608
	private void RebuildPreinstalledButtons()
	{
		using (List<string>.Enumerator enumerator = Localization.PreinstalledLanguages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string code = enumerator.Current;
				if (!(code != Localization.DEFAULT_LANGUAGE_CODE) || File.Exists(Localization.GetPreinstalledLocalizationFilePath(code)))
				{
					GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.preinstalledLanguagesContainer, false);
					gameObject.name = code + "_button";
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					LocText reference = component.GetReference<LocText>("Title");
					reference.text = Localization.GetPreinstalledLocalizationTitle(code);
					reference.enabled = false;
					reference.enabled = true;
					Texture2D preinstalledLocalizationImage = Localization.GetPreinstalledLocalizationImage(code);
					if (preinstalledLocalizationImage != null)
					{
						component.GetReference<Image>("Image").sprite = Sprite.Create(preinstalledLocalizationImage, new Rect(Vector2.zero, new Vector2((float)preinstalledLocalizationImage.width, (float)preinstalledLocalizationImage.height)), Vector2.one * 0.5f);
					}
					gameObject.GetComponent<KButton>().onClick += delegate()
					{
						this.ConfirmLanguagePreinstalledOrMod((code != Localization.DEFAULT_LANGUAGE_CODE) ? code : string.Empty, null);
					};
					this.buttons.Add(gameObject);
				}
			}
		}
	}

	// Token: 0x060062FF RID: 25343 RVA: 0x0024E578 File Offset: 0x0024C778
	protected override void OnActivate()
	{
		base.OnActivate();
		Global.Instance.modManager.Sanitize(base.gameObject);
		if (SteamUGCService.Instance != null)
		{
			SteamUGCService.Instance.AddClient(this);
		}
	}

	// Token: 0x06006300 RID: 25344 RVA: 0x0024E5AD File Offset: 0x0024C7AD
	protected override void OnDeactivate()
	{
		base.OnDeactivate();
		if (SteamUGCService.Instance != null)
		{
			SteamUGCService.Instance.RemoveClient(this);
		}
	}

	// Token: 0x06006301 RID: 25345 RVA: 0x0024E5D0 File Offset: 0x0024C7D0
	private void ConfirmLanguageChoiceDialog(string[] lines, bool is_template, System.Action install_language)
	{
		Localization.Locale locale = Localization.GetLocale(lines);
		Dictionary<string, string> translated_strings = Localization.ExtractTranslatedStrings(lines, is_template);
		TMP_FontAsset font = Localization.GetFont(locale.FontName);
		ConfirmDialogScreen screen = this.GetConfirmDialog();
		HashSet<MemberInfo> excluded_members = new HashSet<MemberInfo>(typeof(ConfirmDialogScreen).GetMember("cancelButton", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy));
		Localization.SetFont<ConfirmDialogScreen>(screen, font, locale.IsRightToLeft, excluded_members);
		Func<LocString, string> func = delegate(LocString loc_string)
		{
			string result;
			if (!translated_strings.TryGetValue(loc_string.key.String, out result))
			{
				return loc_string;
			}
			return result;
		};
		ConfirmDialogScreen screen2 = screen;
		string title_text = func(UI.CONFIRMDIALOG.DIALOG_HEADER);
		screen2.PopupConfirmDialog(func(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT), delegate
		{
			LanguageOptionsScreen.CleanUpSavedLanguageMod();
			install_language();
			App.instance.Restart();
		}, delegate
		{
			Localization.SetFont<ConfirmDialogScreen>(screen, Localization.FontAsset, Localization.IsRightToLeft, excluded_members);
		}, null, null, title_text, func(UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART), UI.FRONTEND.TRANSLATIONS_SCREEN.CANCEL, null);
	}

	// Token: 0x06006302 RID: 25346 RVA: 0x0024E6B2 File Offset: 0x0024C8B2
	private void ConfirmPreinstalledLanguage(string selected_preinstalled_translation)
	{
		Localization.GetSelectedLanguageType();
	}

	// Token: 0x06006303 RID: 25347 RVA: 0x0024E6BC File Offset: 0x0024C8BC
	private void ConfirmLanguagePreinstalledOrMod(string selected_preinstalled_translation, string mod_id)
	{
		Localization.SelectedLanguageType selectedLanguageType = Localization.GetSelectedLanguageType();
		if (mod_id != null)
		{
			if (selectedLanguageType == Localization.SelectedLanguageType.UGC && mod_id == this.currentLanguageModId)
			{
				this.Deactivate();
				return;
			}
			string[] languageLinesForMod = LanguageOptionsScreen.GetLanguageLinesForMod(mod_id);
			this.ConfirmLanguageChoiceDialog(languageLinesForMod, false, delegate
			{
				LanguageOptionsScreen.SetCurrentLanguage(mod_id);
			});
			return;
		}
		else if (!string.IsNullOrEmpty(selected_preinstalled_translation))
		{
			string currentLanguageCode = Localization.GetCurrentLanguageCode();
			if (selectedLanguageType == Localization.SelectedLanguageType.Preinstalled && currentLanguageCode == selected_preinstalled_translation)
			{
				this.Deactivate();
				return;
			}
			string[] lines = File.ReadAllLines(Localization.GetPreinstalledLocalizationFilePath(selected_preinstalled_translation), Encoding.UTF8);
			this.ConfirmLanguageChoiceDialog(lines, false, delegate
			{
				Localization.LoadPreinstalledTranslation(selected_preinstalled_translation);
			});
			return;
		}
		else
		{
			if (selectedLanguageType == Localization.SelectedLanguageType.None)
			{
				this.Deactivate();
				return;
			}
			string[] lines2 = File.ReadAllLines(Localization.GetDefaultLocalizationFilePath(), Encoding.UTF8);
			this.ConfirmLanguageChoiceDialog(lines2, true, delegate
			{
				Localization.ClearLanguage();
			});
			return;
		}
	}

	// Token: 0x06006304 RID: 25348 RVA: 0x0024E7C6 File Offset: 0x0024C9C6
	private ConfirmDialogScreen GetConfirmDialog()
	{
		KScreen component = KScreenManager.AddChild(base.transform.parent.gameObject, ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<KScreen>();
		component.Activate();
		return component.GetComponent<ConfirmDialogScreen>();
	}

	// Token: 0x06006305 RID: 25349 RVA: 0x0024E7FC File Offset: 0x0024C9FC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.Consumed)
		{
			return;
		}
		if (e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06006306 RID: 25350 RVA: 0x0024E820 File Offset: 0x0024CA20
	private void RebuildUGCButtons()
	{
		foreach (Mod mod in Global.Instance.modManager.mods)
		{
			if ((mod.available_content & Content.Translation) != (Content)0 && mod.status == Mod.Status.Installed)
			{
				GameObject gameObject = Util.KInstantiateUI(this.languageButtonPrefab, this.ugcLanguagesContainer, false);
				gameObject.name = mod.title + "_button";
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				TMP_FontAsset font = Localization.GetFont(Localization.GetFontName(LanguageOptionsScreen.GetLanguageLinesForMod(mod)));
				LocText reference = component.GetReference<LocText>("Title");
				reference.SetText(string.Format(UI.FRONTEND.TRANSLATIONS_SCREEN.UGC_MOD_TITLE_FORMAT, mod.title));
				reference.font = font;
				Texture2D previewImage = mod.GetPreviewImage();
				if (previewImage != null)
				{
					component.GetReference<Image>("Image").sprite = Sprite.Create(previewImage, new Rect(Vector2.zero, new Vector2((float)previewImage.width, (float)previewImage.height)), Vector2.one * 0.5f);
				}
				string mod_id = mod.label.id;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.ConfirmLanguagePreinstalledOrMod(string.Empty, mod_id);
				};
				this.buttons.Add(gameObject);
			}
		}
	}

	// Token: 0x06006307 RID: 25351 RVA: 0x0024E9AC File Offset: 0x0024CBAC
	private void Uninstall()
	{
		this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.ARE_YOU_SURE, delegate
		{
			Localization.ClearLanguage();
			this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, new System.Action(App.instance.Restart), new System.Action(this.Deactivate), null, null, null, null, null, null);
		}, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x06006308 RID: 25352 RVA: 0x0024E9FF File Offset: 0x0024CBFF
	private void OnClickUninstall()
	{
		this.Uninstall();
	}

	// Token: 0x06006309 RID: 25353 RVA: 0x0024EA07 File Offset: 0x0024CC07
	private void OnClickOpenWorkshop()
	{
		App.OpenWebURL("http://steamcommunity.com/workshop/browse/?appid=457140&requiredtags[]=language");
	}

	// Token: 0x0600630A RID: 25354 RVA: 0x0024EA14 File Offset: 0x0024CC14
	public void UpdateMods(IEnumerable<PublishedFileId_t> added, IEnumerable<PublishedFileId_t> updated, IEnumerable<PublishedFileId_t> removed, IEnumerable<SteamUGCService.Mod> loaded_previews)
	{
		string savedLanguageMod = LanguageOptionsScreen.GetSavedLanguageMod();
		ulong value;
		if (ulong.TryParse(savedLanguageMod, out value))
		{
			PublishedFileId_t value2 = (PublishedFileId_t)value;
			if (removed.Contains(value2))
			{
				global::Debug.Log("Unsubscribe detected for currently installed language mod [" + savedLanguageMod + "]");
				this.GetConfirmDialog().PopupConfirmDialog(UI.FRONTEND.TRANSLATIONS_SCREEN.PLEASE_REBOOT, delegate
				{
					Localization.ClearLanguage();
					App.instance.Restart();
				}, null, null, null, null, UI.FRONTEND.TRANSLATIONS_SCREEN.RESTART, null, null);
			}
			if (updated.Contains(value2))
			{
				global::Debug.Log("Download complete for currently installed language [" + savedLanguageMod + "] updating in background. Changes will happen next restart.");
			}
		}
		this.RebuildScreen();
	}

	// Token: 0x0600630B RID: 25355 RVA: 0x0024EAC1 File Offset: 0x0024CCC1
	public static string GetSavedLanguageMod()
	{
		return KPlayerPrefs.GetString("InstalledLanguage");
	}

	// Token: 0x0600630C RID: 25356 RVA: 0x0024EACD File Offset: 0x0024CCCD
	public static void SetSavedLanguageMod(string mod_id)
	{
		KPlayerPrefs.SetString("InstalledLanguage", mod_id);
	}

	// Token: 0x0600630D RID: 25357 RVA: 0x0024EADA File Offset: 0x0024CCDA
	public static void CleanUpSavedLanguageMod()
	{
		KPlayerPrefs.SetString("InstalledLanguage", null);
	}

	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x0600630E RID: 25358 RVA: 0x0024EAE7 File Offset: 0x0024CCE7
	// (set) Token: 0x0600630F RID: 25359 RVA: 0x0024EAEF File Offset: 0x0024CCEF
	public string currentLanguageModId
	{
		get
		{
			return this._currentLanguageModId;
		}
		private set
		{
			this._currentLanguageModId = value;
			LanguageOptionsScreen.SetSavedLanguageMod(this._currentLanguageModId);
		}
	}

	// Token: 0x06006310 RID: 25360 RVA: 0x0024EB03 File Offset: 0x0024CD03
	public static bool SetCurrentLanguage(string mod_id)
	{
		LanguageOptionsScreen.CleanUpSavedLanguageMod();
		if (LanguageOptionsScreen.LoadTranslation(mod_id))
		{
			LanguageOptionsScreen.SetSavedLanguageMod(mod_id);
			return true;
		}
		return false;
	}

	// Token: 0x06006311 RID: 25361 RVA: 0x0024EB1C File Offset: 0x0024CD1C
	public static bool HasInstalledLanguage()
	{
		string currentModId = LanguageOptionsScreen.GetSavedLanguageMod();
		if (currentModId == null)
		{
			return false;
		}
		if (Global.Instance.modManager.mods.Find((Mod m) => m.label.id == currentModId) == null)
		{
			LanguageOptionsScreen.CleanUpSavedLanguageMod();
			return false;
		}
		return true;
	}

	// Token: 0x06006312 RID: 25362 RVA: 0x0024EB70 File Offset: 0x0024CD70
	public static string GetInstalledLanguageCode()
	{
		string result = "";
		string[] languageLinesForMod = LanguageOptionsScreen.GetLanguageLinesForMod(LanguageOptionsScreen.GetSavedLanguageMod());
		if (languageLinesForMod != null)
		{
			Localization.Locale locale = Localization.GetLocale(languageLinesForMod);
			if (locale != null)
			{
				result = locale.Code;
			}
		}
		return result;
	}

	// Token: 0x06006313 RID: 25363 RVA: 0x0024EBA4 File Offset: 0x0024CDA4
	private static bool LoadTranslation(string mod_id)
	{
		Mod mod = Global.Instance.modManager.mods.Find((Mod m) => m.label.id == mod_id);
		if (mod == null)
		{
			global::Debug.LogWarning("Tried loading a translation from a non-existent mod id: " + mod_id);
			return false;
		}
		string languageFilename = LanguageOptionsScreen.GetLanguageFilename(mod);
		return languageFilename != null && Localization.LoadLocalTranslationFile(Localization.SelectedLanguageType.UGC, languageFilename);
	}

	// Token: 0x06006314 RID: 25364 RVA: 0x0024EC0C File Offset: 0x0024CE0C
	private static string GetLanguageFilename(Mod mod)
	{
		global::Debug.Assert(mod.content_source.GetType() == typeof(KMod.Directory), "Can only load translations from extracted mods.");
		string text = Path.Combine(mod.ContentPath, "strings.po");
		if (!File.Exists(text))
		{
			global::Debug.LogWarning("GetLanguagFile: " + text + " missing for mod " + mod.label.title);
			return null;
		}
		return text;
	}

	// Token: 0x06006315 RID: 25365 RVA: 0x0024EC7C File Offset: 0x0024CE7C
	private static string[] GetLanguageLinesForMod(string mod_id)
	{
		return LanguageOptionsScreen.GetLanguageLinesForMod(Global.Instance.modManager.mods.Find((Mod m) => m.label.id == mod_id));
	}

	// Token: 0x06006316 RID: 25366 RVA: 0x0024ECBC File Offset: 0x0024CEBC
	private static string[] GetLanguageLinesForMod(Mod mod)
	{
		string languageFilename = LanguageOptionsScreen.GetLanguageFilename(mod);
		if (languageFilename == null)
		{
			return null;
		}
		string[] array = File.ReadAllLines(languageFilename, Encoding.UTF8);
		if (array == null || array.Length == 0)
		{
			global::Debug.LogWarning("Couldn't find any strings in the translation mod " + mod.label.title);
			return null;
		}
		return array;
	}

	// Token: 0x04004321 RID: 17185
	private static readonly string[] poFile = new string[]
	{
		"strings.po"
	};

	// Token: 0x04004322 RID: 17186
	public const string KPLAYER_PREFS_LANGUAGE_KEY = "InstalledLanguage";

	// Token: 0x04004323 RID: 17187
	public const string TAG_LANGUAGE = "language";

	// Token: 0x04004324 RID: 17188
	public KButton textButton;

	// Token: 0x04004325 RID: 17189
	public KButton dismissButton;

	// Token: 0x04004326 RID: 17190
	public KButton closeButton;

	// Token: 0x04004327 RID: 17191
	public KButton workshopButton;

	// Token: 0x04004328 RID: 17192
	public KButton uninstallButton;

	// Token: 0x04004329 RID: 17193
	[Space]
	public GameObject languageButtonPrefab;

	// Token: 0x0400432A RID: 17194
	public GameObject preinstalledLanguagesTitle;

	// Token: 0x0400432B RID: 17195
	public GameObject preinstalledLanguagesContainer;

	// Token: 0x0400432C RID: 17196
	public GameObject ugcLanguagesTitle;

	// Token: 0x0400432D RID: 17197
	public GameObject ugcLanguagesContainer;

	// Token: 0x0400432E RID: 17198
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x0400432F RID: 17199
	private string _currentLanguageModId;

	// Token: 0x04004330 RID: 17200
	private System.DateTime currentLastModified;
}
