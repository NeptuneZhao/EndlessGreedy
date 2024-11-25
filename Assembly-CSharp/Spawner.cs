using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B16 RID: 2838
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Spawner")]
public class Spawner : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x0600548B RID: 21643 RVA: 0x001E3D77 File Offset: 0x001E1F77
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SaveGame.Instance.worldGenSpawner.AddLegacySpawner(this.prefabTag, Grid.PosToCell(this));
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x04003763 RID: 14179
	[Serialize]
	public Tag prefabTag;

	// Token: 0x04003764 RID: 14180
	[Serialize]
	public int units = 1;
}
