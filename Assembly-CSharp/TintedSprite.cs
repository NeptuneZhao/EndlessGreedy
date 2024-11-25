using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000650 RID: 1616
[DebuggerDisplay("{name}")]
[Serializable]
public class TintedSprite : ISerializationCallbackReceiver
{
	// Token: 0x0600278C RID: 10124 RVA: 0x000E10DA File Offset: 0x000DF2DA
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x0600278D RID: 10125 RVA: 0x000E10DC File Offset: 0x000DF2DC
	public void OnBeforeSerialize()
	{
		if (this.sprite != null)
		{
			this.name = this.sprite.name;
		}
	}

	// Token: 0x040016CC RID: 5836
	[ReadOnly]
	public string name;

	// Token: 0x040016CD RID: 5837
	public Sprite sprite;

	// Token: 0x040016CE RID: 5838
	public Color color;
}
