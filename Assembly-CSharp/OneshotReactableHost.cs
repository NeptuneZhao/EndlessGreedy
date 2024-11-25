using System;
using UnityEngine;

// Token: 0x020009D6 RID: 2518
[AddComponentMenu("KMonoBehaviour/scripts/OneshotReactableHost")]
public class OneshotReactableHost : KMonoBehaviour
{
	// Token: 0x06004920 RID: 18720 RVA: 0x001A2CC9 File Offset: 0x001A0EC9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", this.lifetime, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x001A2CF5 File Offset: 0x001A0EF5
	public void SetReactable(Reactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x06004922 RID: 18722 RVA: 0x001A2D00 File Offset: 0x001A0F00
	private void OnExpire(object obj)
	{
		if (!this.reactable.IsReacting)
		{
			this.reactable.Cleanup();
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameScheduler.Instance.Schedule("CleanupOneshotReactable", 0.5f, new Action<object>(this.OnExpire), null, null);
	}

	// Token: 0x04002FD9 RID: 12249
	private Reactable reactable;

	// Token: 0x04002FDA RID: 12250
	public float lifetime = 1f;
}
