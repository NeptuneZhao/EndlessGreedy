using System;
using UnityEngine;

// Token: 0x02000C66 RID: 3174
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/InfoDescription")]
public class InfoDescription : KMonoBehaviour
{
	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x06006158 RID: 24920 RVA: 0x002442A8 File Offset: 0x002424A8
	// (set) Token: 0x06006157 RID: 24919 RVA: 0x00244281 File Offset: 0x00242481
	public string DescriptionLocString
	{
		get
		{
			return this.descriptionLocString;
		}
		set
		{
			this.descriptionLocString = value;
			if (this.descriptionLocString != null)
			{
				this.description = Strings.Get(this.descriptionLocString);
			}
		}
	}

	// Token: 0x06006159 RID: 24921 RVA: 0x002442B0 File Offset: 0x002424B0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (!string.IsNullOrEmpty(this.nameLocString))
		{
			this.displayName = Strings.Get(this.nameLocString);
		}
		if (!string.IsNullOrEmpty(this.descriptionLocString))
		{
			this.description = Strings.Get(this.descriptionLocString);
		}
	}

	// Token: 0x040041FB RID: 16891
	public string nameLocString = "";

	// Token: 0x040041FC RID: 16892
	private string descriptionLocString = "";

	// Token: 0x040041FD RID: 16893
	public string description;

	// Token: 0x040041FE RID: 16894
	public string effect = "";

	// Token: 0x040041FF RID: 16895
	public string displayName;
}
