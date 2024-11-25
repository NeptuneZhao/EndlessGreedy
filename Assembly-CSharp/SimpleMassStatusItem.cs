using System;
using UnityEngine;

// Token: 0x020005B9 RID: 1465
[AddComponentMenu("KMonoBehaviour/scripts/SimpleMassStatusItem")]
public class SimpleMassStatusItem : KMonoBehaviour
{
	// Token: 0x060022F8 RID: 8952 RVA: 0x000C322A File Offset: 0x000C142A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OreMass, base.gameObject);
	}

	// Token: 0x040013D9 RID: 5081
	public string symbolPrefix = "";
}
