using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BE3 RID: 3043
public struct AsteroidDescriptor
{
	// Token: 0x06005C8F RID: 23695 RVA: 0x0021DAFF File Offset: 0x0021BCFF
	public AsteroidDescriptor(string text, string tooltip, Color associatedColor, List<global::Tuple<string, Color, float>> bands = null, string associatedIcon = null)
	{
		this.text = text;
		this.tooltip = tooltip;
		this.associatedColor = associatedColor;
		this.bands = bands;
		this.associatedIcon = associatedIcon;
	}

	// Token: 0x04003DC5 RID: 15813
	public string text;

	// Token: 0x04003DC6 RID: 15814
	public string tooltip;

	// Token: 0x04003DC7 RID: 15815
	public List<global::Tuple<string, Color, float>> bands;

	// Token: 0x04003DC8 RID: 15816
	public Color associatedColor;

	// Token: 0x04003DC9 RID: 15817
	public string associatedIcon;
}
