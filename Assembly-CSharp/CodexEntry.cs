using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000BF9 RID: 3065
public class CodexEntry
{
	// Token: 0x06005D98 RID: 23960 RVA: 0x00227044 File Offset: 0x00225244
	public CodexEntry()
	{
	}

	// Token: 0x06005D99 RID: 23961 RVA: 0x0022709C File Offset: 0x0022529C
	public CodexEntry(string category, List<ContentContainer> contentContainers, string name)
	{
		this.category = category;
		this.name = name;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x06005D9A RID: 23962 RVA: 0x00227120 File Offset: 0x00225320
	public CodexEntry(string category, string titleKey, List<ContentContainer> contentContainers)
	{
		this.category = category;
		this.title = titleKey;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(this.title);
		}
	}

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06005D9B RID: 23963 RVA: 0x002271A9 File Offset: 0x002253A9
	// (set) Token: 0x06005D9C RID: 23964 RVA: 0x002271B1 File Offset: 0x002253B1
	public List<ContentContainer> contentContainers
	{
		get
		{
			return this._contentContainers;
		}
		private set
		{
			this._contentContainers = value;
		}
	}

	// Token: 0x06005D9D RID: 23965 RVA: 0x002271BC File Offset: 0x002253BC
	public static List<string> ContentContainerDebug(List<ContentContainer> _contentContainers)
	{
		List<string> list = new List<string>();
		foreach (ContentContainer contentContainer in _contentContainers)
		{
			if (contentContainer != null)
			{
				string text = string.Concat(new string[]
				{
					"<b>",
					contentContainer.contentLayout.ToString(),
					" container: ",
					((contentContainer.content == null) ? 0 : contentContainer.content.Count).ToString(),
					" items</b>"
				});
				if (contentContainer.content != null)
				{
					text += "\n";
					for (int i = 0; i < contentContainer.content.Count; i++)
					{
						text = string.Concat(new string[]
						{
							text,
							"    • ",
							contentContainer.content[i].ToString(),
							": ",
							CodexEntry.GetContentWidgetDebugString(contentContainer.content[i]),
							"\n"
						});
					}
				}
				list.Add(text);
			}
			else
			{
				list.Add("null container");
			}
		}
		return list;
	}

	// Token: 0x06005D9E RID: 23966 RVA: 0x00227314 File Offset: 0x00225514
	private static string GetContentWidgetDebugString(ICodexWidget widget)
	{
		CodexText codexText = widget as CodexText;
		if (codexText != null)
		{
			return codexText.text;
		}
		CodexLabelWithIcon codexLabelWithIcon = widget as CodexLabelWithIcon;
		if (codexLabelWithIcon != null)
		{
			return codexLabelWithIcon.label.text + " / " + codexLabelWithIcon.icon.spriteName;
		}
		CodexImage codexImage = widget as CodexImage;
		if (codexImage != null)
		{
			return codexImage.spriteName;
		}
		CodexVideo codexVideo = widget as CodexVideo;
		if (codexVideo != null)
		{
			return codexVideo.name;
		}
		CodexIndentedLabelWithIcon codexIndentedLabelWithIcon = widget as CodexIndentedLabelWithIcon;
		if (codexIndentedLabelWithIcon != null)
		{
			return codexIndentedLabelWithIcon.label.text + " / " + codexIndentedLabelWithIcon.icon.spriteName;
		}
		return "";
	}

	// Token: 0x06005D9F RID: 23967 RVA: 0x002273B3 File Offset: 0x002255B3
	public void CreateContentContainerCollection()
	{
		this.contentContainers = new List<ContentContainer>();
	}

	// Token: 0x06005DA0 RID: 23968 RVA: 0x002273C0 File Offset: 0x002255C0
	public void InsertContentContainer(int index, ContentContainer container)
	{
		this.contentContainers.Insert(index, container);
	}

	// Token: 0x06005DA1 RID: 23969 RVA: 0x002273CF File Offset: 0x002255CF
	public void RemoveContentContainerAt(int index)
	{
		this.contentContainers.RemoveAt(index);
	}

	// Token: 0x06005DA2 RID: 23970 RVA: 0x002273DD File Offset: 0x002255DD
	public void AddContentContainer(ContentContainer container)
	{
		this.contentContainers.Add(container);
	}

	// Token: 0x06005DA3 RID: 23971 RVA: 0x002273EB File Offset: 0x002255EB
	public void AddContentContainerRange(IEnumerable<ContentContainer> containers)
	{
		this.contentContainers.AddRange(containers);
	}

	// Token: 0x06005DA4 RID: 23972 RVA: 0x002273F9 File Offset: 0x002255F9
	public void RemoveContentContainer(ContentContainer container)
	{
		this.contentContainers.Remove(container);
	}

	// Token: 0x06005DA5 RID: 23973 RVA: 0x00227408 File Offset: 0x00225608
	public ICodexWidget GetFirstWidget()
	{
		for (int i = 0; i < this.contentContainers.Count; i++)
		{
			if (this.contentContainers[i].content != null)
			{
				for (int j = 0; j < this.contentContainers[i].content.Count; j++)
				{
					if (this.contentContainers[i].content[j] != null)
					{
						return this.contentContainers[i].content[j];
					}
				}
			}
		}
		return null;
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x06005DA6 RID: 23974 RVA: 0x00227491 File Offset: 0x00225691
	// (set) Token: 0x06005DA7 RID: 23975 RVA: 0x00227499 File Offset: 0x00225699
	public string[] dlcIds
	{
		get
		{
			return this._dlcIds;
		}
		set
		{
			this._dlcIds = value;
		}
	}

	// Token: 0x06005DA8 RID: 23976 RVA: 0x002274A2 File Offset: 0x002256A2
	public string[] GetDlcIds()
	{
		return this._dlcIds;
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x06005DA9 RID: 23977 RVA: 0x002274AA File Offset: 0x002256AA
	// (set) Token: 0x06005DAA RID: 23978 RVA: 0x002274B4 File Offset: 0x002256B4
	public string[] forbiddenDLCIds
	{
		get
		{
			return this._forbiddenDLCIds;
		}
		set
		{
			this._forbiddenDLCIds = value;
			string str = "";
			for (int i = 0; i < value.Length; i++)
			{
				str += value[i];
				if (i != value.Length - 1)
				{
					str += "\n";
				}
			}
		}
	}

	// Token: 0x06005DAB RID: 23979 RVA: 0x002274FA File Offset: 0x002256FA
	public string[] GetForbiddenDLCs()
	{
		if (this._forbiddenDLCIds == null)
		{
			this._forbiddenDLCIds = this.NONE;
		}
		return this._forbiddenDLCIds;
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x06005DAC RID: 23980 RVA: 0x00227516 File Offset: 0x00225716
	// (set) Token: 0x06005DAD RID: 23981 RVA: 0x0022751E File Offset: 0x0022571E
	public string id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x06005DAE RID: 23982 RVA: 0x00227527 File Offset: 0x00225727
	// (set) Token: 0x06005DAF RID: 23983 RVA: 0x0022752F File Offset: 0x0022572F
	public string parentId
	{
		get
		{
			return this._parentId;
		}
		set
		{
			this._parentId = value;
		}
	}

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06005DB0 RID: 23984 RVA: 0x00227538 File Offset: 0x00225738
	// (set) Token: 0x06005DB1 RID: 23985 RVA: 0x00227540 File Offset: 0x00225740
	public string category
	{
		get
		{
			return this._category;
		}
		set
		{
			this._category = value;
		}
	}

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x06005DB2 RID: 23986 RVA: 0x00227549 File Offset: 0x00225749
	// (set) Token: 0x06005DB3 RID: 23987 RVA: 0x00227551 File Offset: 0x00225751
	public string title
	{
		get
		{
			return this._title;
		}
		set
		{
			this._title = value;
		}
	}

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x06005DB4 RID: 23988 RVA: 0x0022755A File Offset: 0x0022575A
	// (set) Token: 0x06005DB5 RID: 23989 RVA: 0x00227562 File Offset: 0x00225762
	public string name
	{
		get
		{
			return this._name;
		}
		set
		{
			this._name = value;
		}
	}

	// Token: 0x170006EB RID: 1771
	// (get) Token: 0x06005DB6 RID: 23990 RVA: 0x0022756B File Offset: 0x0022576B
	// (set) Token: 0x06005DB7 RID: 23991 RVA: 0x00227573 File Offset: 0x00225773
	public string subtitle
	{
		get
		{
			return this._subtitle;
		}
		set
		{
			this._subtitle = value;
		}
	}

	// Token: 0x170006EC RID: 1772
	// (get) Token: 0x06005DB8 RID: 23992 RVA: 0x0022757C File Offset: 0x0022577C
	// (set) Token: 0x06005DB9 RID: 23993 RVA: 0x00227584 File Offset: 0x00225784
	public List<SubEntry> subEntries
	{
		get
		{
			return this._subEntries;
		}
		set
		{
			this._subEntries = value;
		}
	}

	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x06005DBA RID: 23994 RVA: 0x0022758D File Offset: 0x0022578D
	// (set) Token: 0x06005DBB RID: 23995 RVA: 0x00227595 File Offset: 0x00225795
	public List<CodexEntry_MadeAndUsed> contentMadeAndUsed
	{
		get
		{
			return this._contentMadeAndUsed;
		}
		set
		{
			this._contentMadeAndUsed = value;
		}
	}

	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x06005DBC RID: 23996 RVA: 0x0022759E File Offset: 0x0022579E
	// (set) Token: 0x06005DBD RID: 23997 RVA: 0x002275A6 File Offset: 0x002257A6
	public Sprite icon
	{
		get
		{
			return this._icon;
		}
		set
		{
			this._icon = value;
		}
	}

	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x06005DBE RID: 23998 RVA: 0x002275AF File Offset: 0x002257AF
	// (set) Token: 0x06005DBF RID: 23999 RVA: 0x002275B7 File Offset: 0x002257B7
	public Color iconColor
	{
		get
		{
			return this._iconColor;
		}
		set
		{
			this._iconColor = value;
		}
	}

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06005DC0 RID: 24000 RVA: 0x002275C0 File Offset: 0x002257C0
	// (set) Token: 0x06005DC1 RID: 24001 RVA: 0x002275C8 File Offset: 0x002257C8
	public string iconPrefabID
	{
		get
		{
			return this._iconPrefabID;
		}
		set
		{
			this._iconPrefabID = value;
		}
	}

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06005DC2 RID: 24002 RVA: 0x002275D1 File Offset: 0x002257D1
	// (set) Token: 0x06005DC3 RID: 24003 RVA: 0x002275D9 File Offset: 0x002257D9
	public string iconLockID
	{
		get
		{
			return this._iconLockID;
		}
		set
		{
			this._iconLockID = value;
		}
	}

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06005DC4 RID: 24004 RVA: 0x002275E2 File Offset: 0x002257E2
	// (set) Token: 0x06005DC5 RID: 24005 RVA: 0x002275EA File Offset: 0x002257EA
	public string iconAssetName
	{
		get
		{
			return this._iconAssetName;
		}
		set
		{
			this._iconAssetName = value;
		}
	}

	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06005DC6 RID: 24006 RVA: 0x002275F3 File Offset: 0x002257F3
	// (set) Token: 0x06005DC7 RID: 24007 RVA: 0x002275FB File Offset: 0x002257FB
	public bool disabled
	{
		get
		{
			return this._disabled;
		}
		set
		{
			this._disabled = value;
		}
	}

	// Token: 0x170006F4 RID: 1780
	// (get) Token: 0x06005DC8 RID: 24008 RVA: 0x00227604 File Offset: 0x00225804
	// (set) Token: 0x06005DC9 RID: 24009 RVA: 0x0022760C File Offset: 0x0022580C
	public bool searchOnly
	{
		get
		{
			return this._searchOnly;
		}
		set
		{
			this._searchOnly = value;
		}
	}

	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x06005DCA RID: 24010 RVA: 0x00227615 File Offset: 0x00225815
	// (set) Token: 0x06005DCB RID: 24011 RVA: 0x0022761D File Offset: 0x0022581D
	public int customContentLength
	{
		get
		{
			return this._customContentLength;
		}
		set
		{
			this._customContentLength = value;
		}
	}

	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06005DCC RID: 24012 RVA: 0x00227626 File Offset: 0x00225826
	// (set) Token: 0x06005DCD RID: 24013 RVA: 0x0022762E File Offset: 0x0022582E
	public string sortString
	{
		get
		{
			return this._sortString;
		}
		set
		{
			this._sortString = value;
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x06005DCE RID: 24014 RVA: 0x00227637 File Offset: 0x00225837
	// (set) Token: 0x06005DCF RID: 24015 RVA: 0x0022763F File Offset: 0x0022583F
	public bool showBeforeGeneratedCategoryLinks
	{
		get
		{
			return this._showBeforeGeneratedCategoryLinks;
		}
		set
		{
			this._showBeforeGeneratedCategoryLinks = value;
		}
	}

	// Token: 0x04003EB1 RID: 16049
	public EntryDevLog log = new EntryDevLog();

	// Token: 0x04003EB2 RID: 16050
	private List<ContentContainer> _contentContainers = new List<ContentContainer>();

	// Token: 0x04003EB3 RID: 16051
	private string[] _dlcIds;

	// Token: 0x04003EB4 RID: 16052
	private string[] _forbiddenDLCIds;

	// Token: 0x04003EB5 RID: 16053
	private string[] NONE = new string[0];

	// Token: 0x04003EB6 RID: 16054
	private string _id;

	// Token: 0x04003EB7 RID: 16055
	private string _parentId;

	// Token: 0x04003EB8 RID: 16056
	private string _category;

	// Token: 0x04003EB9 RID: 16057
	private string _title;

	// Token: 0x04003EBA RID: 16058
	private string _name;

	// Token: 0x04003EBB RID: 16059
	private string _subtitle;

	// Token: 0x04003EBC RID: 16060
	private List<SubEntry> _subEntries = new List<SubEntry>();

	// Token: 0x04003EBD RID: 16061
	private List<CodexEntry_MadeAndUsed> _contentMadeAndUsed = new List<CodexEntry_MadeAndUsed>();

	// Token: 0x04003EBE RID: 16062
	private Sprite _icon;

	// Token: 0x04003EBF RID: 16063
	private Color _iconColor = Color.white;

	// Token: 0x04003EC0 RID: 16064
	private string _iconPrefabID;

	// Token: 0x04003EC1 RID: 16065
	private string _iconLockID;

	// Token: 0x04003EC2 RID: 16066
	private string _iconAssetName;

	// Token: 0x04003EC3 RID: 16067
	private bool _disabled;

	// Token: 0x04003EC4 RID: 16068
	private bool _searchOnly;

	// Token: 0x04003EC5 RID: 16069
	private int _customContentLength;

	// Token: 0x04003EC6 RID: 16070
	private string _sortString;

	// Token: 0x04003EC7 RID: 16071
	private bool _showBeforeGeneratedCategoryLinks;
}
