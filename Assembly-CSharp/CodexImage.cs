using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C05 RID: 3077
public class CodexImage : CodexWidget<CodexImage>
{
	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x06005E64 RID: 24164 RVA: 0x00231354 File Offset: 0x0022F554
	// (set) Token: 0x06005E65 RID: 24165 RVA: 0x0023135C File Offset: 0x0022F55C
	public Sprite sprite { get; set; }

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x06005E66 RID: 24166 RVA: 0x00231365 File Offset: 0x0022F565
	// (set) Token: 0x06005E67 RID: 24167 RVA: 0x0023136D File Offset: 0x0022F56D
	public Color color { get; set; }

	// Token: 0x17000709 RID: 1801
	// (get) Token: 0x06005E69 RID: 24169 RVA: 0x00231389 File Offset: 0x0022F589
	// (set) Token: 0x06005E68 RID: 24168 RVA: 0x00231376 File Offset: 0x0022F576
	public string spriteName
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			this.sprite = Assets.GetSprite(value);
		}
	}

	// Token: 0x1700070A RID: 1802
	// (get) Token: 0x06005E6B RID: 24171 RVA: 0x0023141C File Offset: 0x0022F61C
	// (set) Token: 0x06005E6A RID: 24170 RVA: 0x002313B8 File Offset: 0x0022F5B8
	public string batchedAnimPrefabSourceID
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			GameObject prefab = Assets.GetPrefab(value);
			KBatchedAnimController kbatchedAnimController = (prefab != null) ? prefab.GetComponent<KBatchedAnimController>() : null;
			KAnimFile kanimFile = (kbatchedAnimController != null) ? kbatchedAnimController.AnimFiles[0] : null;
			this.sprite = ((kanimFile != null) ? Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "") : null);
		}
	}

	// Token: 0x1700070B RID: 1803
	// (get) Token: 0x06005E6D RID: 24173 RVA: 0x00231484 File Offset: 0x0022F684
	// (set) Token: 0x06005E6C RID: 24172 RVA: 0x00231448 File Offset: 0x0022F648
	public string elementIcon
	{
		get
		{
			return "";
		}
		set
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(value.ToTag(), "ui", false);
			this.sprite = uisprite.first;
			this.color = uisprite.second;
		}
	}

	// Token: 0x06005E6E RID: 24174 RVA: 0x0023148B File Offset: 0x0022F68B
	public CodexImage()
	{
		this.color = Color.white;
	}

	// Token: 0x06005E6F RID: 24175 RVA: 0x0023149E File Offset: 0x0022F69E
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite, Color color) : base(preferredWidth, preferredHeight)
	{
		this.sprite = sprite;
		this.color = color;
	}

	// Token: 0x06005E70 RID: 24176 RVA: 0x002314B7 File Offset: 0x0022F6B7
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite) : this(preferredWidth, preferredHeight, sprite, Color.white)
	{
	}

	// Token: 0x06005E71 RID: 24177 RVA: 0x002314C7 File Offset: 0x0022F6C7
	public CodexImage(int preferredWidth, int preferredHeight, global::Tuple<Sprite, Color> coloredSprite) : this(preferredWidth, preferredHeight, coloredSprite.first, coloredSprite.second)
	{
	}

	// Token: 0x06005E72 RID: 24178 RVA: 0x002314DD File Offset: 0x0022F6DD
	public CodexImage(global::Tuple<Sprite, Color> coloredSprite) : this(-1, -1, coloredSprite)
	{
	}

	// Token: 0x06005E73 RID: 24179 RVA: 0x002314E8 File Offset: 0x0022F6E8
	public void ConfigureImage(Image image)
	{
		image.sprite = this.sprite;
		image.color = this.color;
	}

	// Token: 0x06005E74 RID: 24180 RVA: 0x00231502 File Offset: 0x0022F702
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureImage(contentGameObject.GetComponent<Image>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
