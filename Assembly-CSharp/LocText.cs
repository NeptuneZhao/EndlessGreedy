using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000C98 RID: 3224
public class LocText : TextMeshProUGUI
{
	// Token: 0x0600631F RID: 25375 RVA: 0x0024EE90 File Offset: 0x0024D090
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x06006320 RID: 25376 RVA: 0x0024EE98 File Offset: 0x0024D098
	// (set) Token: 0x06006321 RID: 25377 RVA: 0x0024EEA0 File Offset: 0x0024D0A0
	public bool AllowLinks
	{
		get
		{
			return this.allowLinksInternal;
		}
		set
		{
			this.allowLinksInternal = value;
			this.RefreshLinkHandler();
			this.raycastTarget = (this.raycastTarget || this.allowLinksInternal);
		}
	}

	// Token: 0x06006322 RID: 25378 RVA: 0x0024EEC8 File Offset: 0x0024D0C8
	[ContextMenu("Apply Settings")]
	public void ApplySettings()
	{
		if (this.key != "" && Application.isPlaying)
		{
			StringKey key = new StringKey(this.key);
			this.text = Strings.Get(key);
		}
		if (this.textStyleSetting != null)
		{
			SetTextStyleSetting.ApplyStyle(this, this.textStyleSetting);
		}
	}

	// Token: 0x06006323 RID: 25379 RVA: 0x0024EF28 File Offset: 0x0024D128
	private new void Awake()
	{
		base.Awake();
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(this.key));
			this.text = stringEntry.String;
		}
		this.text = Localization.Fixup(this.text);
		base.isRightToLeftText = Localization.IsRightToLeft;
		KInputManager.InputChange.AddListener(new UnityAction(this.RefreshText));
		SetTextStyleSetting setTextStyleSetting = base.gameObject.GetComponent<SetTextStyleSetting>();
		if (setTextStyleSetting == null)
		{
			setTextStyleSetting = base.gameObject.AddComponent<SetTextStyleSetting>();
		}
		if (!this.allowOverride)
		{
			setTextStyleSetting.SetStyle(this.textStyleSetting);
		}
		this.textLinkHandler = base.GetComponent<TextLinkHandler>();
	}

	// Token: 0x06006324 RID: 25380 RVA: 0x0024EFE5 File Offset: 0x0024D1E5
	private new void Start()
	{
		base.Start();
		this.RefreshLinkHandler();
	}

	// Token: 0x06006325 RID: 25381 RVA: 0x0024EFF3 File Offset: 0x0024D1F3
	private new void OnDestroy()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshText));
		base.OnDestroy();
	}

	// Token: 0x06006326 RID: 25382 RVA: 0x0024F011 File Offset: 0x0024D211
	public override void SetLayoutDirty()
	{
		if (this.staticLayout)
		{
			return;
		}
		base.SetLayoutDirty();
	}

	// Token: 0x06006327 RID: 25383 RVA: 0x0024F022 File Offset: 0x0024D222
	public void SetLinkOverrideAction(Func<string, bool> action)
	{
		this.RefreshLinkHandler();
		if (this.textLinkHandler != null)
		{
			this.textLinkHandler.overrideLinkAction = action;
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x06006328 RID: 25384 RVA: 0x0024F044 File Offset: 0x0024D244
	// (set) Token: 0x06006329 RID: 25385 RVA: 0x0024F04C File Offset: 0x0024D24C
	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			base.text = this.FilterInput(value);
		}
	}

	// Token: 0x0600632A RID: 25386 RVA: 0x0024F05B File Offset: 0x0024D25B
	public override void SetText(string text)
	{
		text = this.FilterInput(text);
		base.SetText(text);
	}

	// Token: 0x0600632B RID: 25387 RVA: 0x0024F06D File Offset: 0x0024D26D
	private string FilterInput(string input)
	{
		if (input != null)
		{
			string text = LocText.ParseText(input);
			if (text != input)
			{
				this.originalString = input;
			}
			else
			{
				this.originalString = string.Empty;
			}
			input = text;
		}
		if (this.AllowLinks)
		{
			return LocText.ModifyLinkStrings(input);
		}
		return input;
	}

	// Token: 0x0600632C RID: 25388 RVA: 0x0024F0A8 File Offset: 0x0024D2A8
	public static string ParseText(string input)
	{
		string pattern = "\\{Hotkey/(\\w+)\\}";
		string input2 = Regex.Replace(input, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			global::Action action;
			if (LocText.ActionLookup.TryGetValue(value, out action))
			{
				return GameUtil.GetHotkeyString(action);
			}
			return m.Value;
		});
		pattern = "\\(ClickType/(\\w+)\\)";
		return Regex.Replace(input2, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			Pair<LocString, LocString> pair;
			if (!LocText.ClickLookup.TryGetValue(value, out pair))
			{
				return m.Value;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				return pair.first.ToString();
			}
			return pair.second.ToString();
		});
	}

	// Token: 0x0600632D RID: 25389 RVA: 0x0024F10C File Offset: 0x0024D30C
	private void RefreshText()
	{
		if (this.originalString != string.Empty)
		{
			this.SetText(this.originalString);
		}
	}

	// Token: 0x0600632E RID: 25390 RVA: 0x0024F12C File Offset: 0x0024D32C
	protected override void GenerateTextMesh()
	{
		base.GenerateTextMesh();
	}

	// Token: 0x0600632F RID: 25391 RVA: 0x0024F134 File Offset: 0x0024D334
	internal void SwapFont(TMP_FontAsset font, bool isRightToLeft)
	{
		base.font = font;
		if (this.key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(this.key));
			this.text = stringEntry.String;
		}
		this.text = Localization.Fixup(this.text);
		base.isRightToLeftText = isRightToLeft;
	}

	// Token: 0x06006330 RID: 25392 RVA: 0x0024F190 File Offset: 0x0024D390
	private static string ModifyLinkStrings(string input)
	{
		if (input == null || input.IndexOf("<b><style=\"KLink\">") != -1)
		{
			return input;
		}
		StringBuilder stringBuilder = new StringBuilder(input);
		stringBuilder.Replace("<link=\"", LocText.combinedPrefix);
		stringBuilder.Replace("</link>", LocText.combinedSuffix);
		return stringBuilder.ToString();
	}

	// Token: 0x06006331 RID: 25393 RVA: 0x0024F1E0 File Offset: 0x0024D3E0
	private void RefreshLinkHandler()
	{
		if (this.textLinkHandler == null && this.allowLinksInternal)
		{
			this.textLinkHandler = base.GetComponent<TextLinkHandler>();
			if (this.textLinkHandler == null)
			{
				this.textLinkHandler = base.gameObject.AddComponent<TextLinkHandler>();
			}
		}
		else if (!this.allowLinksInternal && this.textLinkHandler != null)
		{
			UnityEngine.Object.Destroy(this.textLinkHandler);
			this.textLinkHandler = null;
		}
		if (this.textLinkHandler != null)
		{
			this.textLinkHandler.CheckMouseOver();
		}
	}

	// Token: 0x04004337 RID: 17207
	public string key;

	// Token: 0x04004338 RID: 17208
	public TextStyleSetting textStyleSetting;

	// Token: 0x04004339 RID: 17209
	public bool allowOverride;

	// Token: 0x0400433A RID: 17210
	public bool staticLayout;

	// Token: 0x0400433B RID: 17211
	private TextLinkHandler textLinkHandler;

	// Token: 0x0400433C RID: 17212
	private string originalString = string.Empty;

	// Token: 0x0400433D RID: 17213
	[SerializeField]
	private bool allowLinksInternal;

	// Token: 0x0400433E RID: 17214
	private static readonly Dictionary<string, global::Action> ActionLookup = Enum.GetNames(typeof(global::Action)).ToDictionary((string x) => x, (string x) => (global::Action)Enum.Parse(typeof(global::Action), x), StringComparer.OrdinalIgnoreCase);

	// Token: 0x0400433F RID: 17215
	private static readonly Dictionary<string, Pair<LocString, LocString>> ClickLookup = new Dictionary<string, Pair<LocString, LocString>>
	{
		{
			UI.ClickType.Click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESS, UI.CONTROLS.CLICK)
		},
		{
			UI.ClickType.Clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLE, UI.CONTROLS.CLICKABLE)
		},
		{
			UI.ClickType.Clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSED, UI.CONTROLS.CLICKED)
		},
		{
			UI.ClickType.Clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSING, UI.CONTROLS.CLICKING)
		},
		{
			UI.ClickType.Clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSES, UI.CONTROLS.CLICKS)
		},
		{
			UI.ClickType.click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSLOWER, UI.CONTROLS.CLICKLOWER)
		},
		{
			UI.ClickType.clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLELOWER, UI.CONTROLS.CLICKABLELOWER)
		},
		{
			UI.ClickType.clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDLOWER, UI.CONTROLS.CLICKEDLOWER)
		},
		{
			UI.ClickType.clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGLOWER, UI.CONTROLS.CLICKINGLOWER)
		},
		{
			UI.ClickType.clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESLOWER, UI.CONTROLS.CLICKSLOWER)
		},
		{
			UI.ClickType.CLICK.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSUPPER, UI.CONTROLS.CLICKUPPER)
		},
		{
			UI.ClickType.CLICKABLE.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLEUPPER, UI.CONTROLS.CLICKABLEUPPER)
		},
		{
			UI.ClickType.CLICKED.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDUPPER, UI.CONTROLS.CLICKEDUPPER)
		},
		{
			UI.ClickType.CLICKING.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGUPPER, UI.CONTROLS.CLICKINGUPPER)
		},
		{
			UI.ClickType.CLICKS.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESUPPER, UI.CONTROLS.CLICKSUPPER)
		}
	};

	// Token: 0x04004340 RID: 17216
	private const string linkPrefix_open = "<link=\"";

	// Token: 0x04004341 RID: 17217
	private const string linkSuffix = "</link>";

	// Token: 0x04004342 RID: 17218
	private const string linkColorPrefix = "<b><style=\"KLink\">";

	// Token: 0x04004343 RID: 17219
	private const string linkColorSuffix = "</style></b>";

	// Token: 0x04004344 RID: 17220
	private static readonly string combinedPrefix = "<b><style=\"KLink\"><link=\"";

	// Token: 0x04004345 RID: 17221
	private static readonly string combinedSuffix = "</style></b></link>";
}
