using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x020008D6 RID: 2262
public class GlobalAssets : KMonoBehaviour
{
	// Token: 0x170004BA RID: 1210
	// (get) Token: 0x06004084 RID: 16516 RVA: 0x0016F427 File Offset: 0x0016D627
	// (set) Token: 0x06004085 RID: 16517 RVA: 0x0016F42E File Offset: 0x0016D62E
	public static GlobalAssets Instance { get; private set; }

	// Token: 0x06004086 RID: 16518 RVA: 0x0016F438 File Offset: 0x0016D638
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GlobalAssets.Instance = this;
		if (GlobalAssets.SoundTable.Count == 0)
		{
			Bank[] array = null;
			try
			{
				if (RuntimeManager.StudioSystem.getBankList(out array) != RESULT.OK)
				{
					array = null;
				}
			}
			catch
			{
				array = null;
			}
			if (array != null)
			{
				foreach (Bank bank in array)
				{
					EventDescription[] array3;
					RESULT eventList = bank.getEventList(out array3);
					if (eventList != RESULT.OK)
					{
						string text;
						bank.getPath(out text);
						global::Debug.LogError(string.Format("ERROR [{0}] loading FMOD events for bank [{1}]", eventList, text));
					}
					else
					{
						foreach (EventDescription eventDescription in array3)
						{
							string text;
							eventDescription.getPath(out text);
							if (text == null)
							{
								bank.getPath(out text);
								GUID guid;
								eventDescription.getID(out guid);
								global::Debug.LogError(string.Format("Got a FMOD event with a null path! {0} {1} in bank {2}", eventDescription.ToString(), guid, text));
							}
							else
							{
								string text2 = Assets.GetSimpleSoundEventName(text);
								text2 = text2.ToLowerInvariant();
								if (text2.Length > 0 && !GlobalAssets.SoundTable.ContainsKey(text2))
								{
									GlobalAssets.SoundTable[text2] = text;
									if (text.ToLower().Contains("lowpriority") || text2.Contains("lowpriority"))
									{
										GlobalAssets.LowPrioritySounds.Add(text);
									}
									else if (text.ToLower().Contains("highpriority") || text2.Contains("highpriority"))
									{
										GlobalAssets.HighPrioritySounds.Add(text);
									}
								}
							}
						}
					}
				}
			}
		}
		SetDefaults.Initialize();
		GraphicsOptionsScreen.SetColorModeFromPrefs();
		this.AddColorModeStyles();
		LocString.CreateLocStringKeys(typeof(UI), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(INPUT), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(GAMEPLAY_EVENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ROOMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(BUILDING.STATUSITEMS), "STRINGS.BUILDING.");
		LocString.CreateLocStringKeys(typeof(BUILDING.DETAILS), "STRINGS.BUILDING.");
		LocString.CreateLocStringKeys(typeof(SETITEMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(COLONY_ACHIEVEMENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CREATURES), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(RESEARCH), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(DUPLICANTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ITEMS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ROBOTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(ELEMENTS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(MISC), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(VIDEOS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(NAMEGEN), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(WORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CLUSTER_NAMES), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(SUBWORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(WORLD_TRAITS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(INPUT_BINDINGS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(LORE), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(CODEX), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(SUBWORLDS), "STRINGS.");
		LocString.CreateLocStringKeys(typeof(BLUEPRINTS), "STRINGS.");
	}

	// Token: 0x06004087 RID: 16519 RVA: 0x0016F808 File Offset: 0x0016DA08
	private void AddColorModeStyles()
	{
		TMP_Style style = new TMP_Style("logic_on", string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGB(this.colorSet.logicOn)), "</color>");
		TMP_StyleSheet.instance.AddStyle(style);
		TMP_Style style2 = new TMP_Style("logic_off", string.Format("<color=#{0}>", ColorUtility.ToHtmlStringRGB(this.colorSet.logicOff)), "</color>");
		TMP_StyleSheet.instance.AddStyle(style2);
		TMP_StyleSheet.RefreshStyles();
	}

	// Token: 0x06004088 RID: 16520 RVA: 0x0016F88E File Offset: 0x0016DA8E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GlobalAssets.Instance = null;
	}

	// Token: 0x06004089 RID: 16521 RVA: 0x0016F89C File Offset: 0x0016DA9C
	public static string GetSound(string name, bool force_no_warning = false)
	{
		if (name == null)
		{
			return null;
		}
		name = name.ToLowerInvariant();
		string result = null;
		GlobalAssets.SoundTable.TryGetValue(name, out result);
		return result;
	}

	// Token: 0x0600408A RID: 16522 RVA: 0x0016F8C7 File Offset: 0x0016DAC7
	public static bool IsLowPriority(string path)
	{
		return GlobalAssets.LowPrioritySounds.Contains(path);
	}

	// Token: 0x0600408B RID: 16523 RVA: 0x0016F8D4 File Offset: 0x0016DAD4
	public static bool IsHighPriority(string path)
	{
		return GlobalAssets.HighPrioritySounds.Contains(path);
	}

	// Token: 0x04002A8E RID: 10894
	private static Dictionary<string, string> SoundTable = new Dictionary<string, string>();

	// Token: 0x04002A8F RID: 10895
	private static HashSet<string> LowPrioritySounds = new HashSet<string>();

	// Token: 0x04002A90 RID: 10896
	private static HashSet<string> HighPrioritySounds = new HashSet<string>();

	// Token: 0x04002A92 RID: 10898
	public ColorSet colorSet;

	// Token: 0x04002A93 RID: 10899
	public ColorSet[] colorSetOptions;
}
