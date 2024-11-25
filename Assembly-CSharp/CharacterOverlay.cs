using System;
using UnityEngine;

// Token: 0x02000BED RID: 3053
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/CharacterOverlay")]
public class CharacterOverlay : KMonoBehaviour
{
	// Token: 0x06005D02 RID: 23810 RVA: 0x00223101 File Offset: 0x00221301
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x06005D03 RID: 23811 RVA: 0x0022310F File Offset: 0x0022130F
	public void Register()
	{
		if (this.registered)
		{
			return;
		}
		this.registered = true;
		NameDisplayScreen.Instance.AddNewEntry(base.gameObject);
	}

	// Token: 0x04003E45 RID: 15941
	public bool shouldShowName;

	// Token: 0x04003E46 RID: 15942
	private bool registered;
}
