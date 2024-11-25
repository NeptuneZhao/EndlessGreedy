using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D6B RID: 3435
[AddComponentMenu("KMonoBehaviour/scripts/FilterSideScreenRow")]
public class FilterSideScreenRow : SingleItemSelectionRow
{
	// Token: 0x17000791 RID: 1937
	// (get) Token: 0x06006C1D RID: 27677 RVA: 0x0028AC90 File Offset: 0x00288E90
	public override string InvalidTagTitle
	{
		get
		{
			return UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;
		}
	}

	// Token: 0x06006C1E RID: 27678 RVA: 0x0028AC9C File Offset: 0x00288E9C
	protected override void SetIcon(Sprite sprite, Color color)
	{
		if (this.icon != null)
		{
			this.icon.gameObject.SetActive(false);
		}
	}
}
