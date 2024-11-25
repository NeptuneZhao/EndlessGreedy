using System;
using UnityEngine;

// Token: 0x02000D06 RID: 3334
public class LegendEntry
{
	// Token: 0x060067A4 RID: 26532 RVA: 0x0026B2C0 File Offset: 0x002694C0
	public LegendEntry(string name, string desc, Color colour, string desc_arg = null, Sprite sprite = null, bool displaySprite = true)
	{
		this.name = name;
		this.desc = desc;
		this.colour = colour;
		this.desc_arg = desc_arg;
		this.sprite = ((sprite == null) ? Assets.instance.LegendColourBox : sprite);
		this.displaySprite = displaySprite;
	}

	// Token: 0x040045F6 RID: 17910
	public string name;

	// Token: 0x040045F7 RID: 17911
	public string desc;

	// Token: 0x040045F8 RID: 17912
	public string desc_arg;

	// Token: 0x040045F9 RID: 17913
	public Color colour;

	// Token: 0x040045FA RID: 17914
	public Sprite sprite;

	// Token: 0x040045FB RID: 17915
	public bool displaySprite;
}
