using System;
using UnityEngine;

// Token: 0x02000558 RID: 1368
[AddComponentMenu("KMonoBehaviour/scripts/DestroyAfter")]
public class DestroyAfter : KMonoBehaviour
{
	// Token: 0x06001F78 RID: 8056 RVA: 0x000B0BFF File Offset: 0x000AEDFF
	protected override void OnSpawn()
	{
		this.particleSystems = base.gameObject.GetComponentsInChildren<ParticleSystem>(true);
	}

	// Token: 0x06001F79 RID: 8057 RVA: 0x000B0C14 File Offset: 0x000AEE14
	private bool IsAlive()
	{
		for (int i = 0; i < this.particleSystems.Length; i++)
		{
			if (this.particleSystems[i].IsAlive(false))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001F7A RID: 8058 RVA: 0x000B0C47 File Offset: 0x000AEE47
	private void Update()
	{
		if (this.particleSystems != null && !this.IsAlive())
		{
			this.DeleteObject();
		}
	}

	// Token: 0x040011BB RID: 4539
	private ParticleSystem[] particleSystems;
}
