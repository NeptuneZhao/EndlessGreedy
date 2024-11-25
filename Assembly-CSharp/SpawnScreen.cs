using System;
using UnityEngine;

// Token: 0x02000DC7 RID: 3527
[AddComponentMenu("KMonoBehaviour/scripts/SpawnScreen")]
public class SpawnScreen : KMonoBehaviour
{
	// Token: 0x06006FEA RID: 28650 RVA: 0x002A389C File Offset: 0x002A1A9C
	protected override void OnPrefabInit()
	{
		Util.KInstantiateUI(this.Screen, base.gameObject, false);
	}

	// Token: 0x04004CB5 RID: 19637
	public GameObject Screen;
}
