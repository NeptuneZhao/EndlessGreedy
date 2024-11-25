using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E6 RID: 1254
[AddComponentMenu("KMonoBehaviour/scripts/KBatchedAnimEventToggler")]
public class KBatchedAnimEventToggler : KMonoBehaviour
{
	// Token: 0x06001BD4 RID: 7124 RVA: 0x00091DD4 File Offset: 0x0008FFD4
	protected override void OnPrefabInit()
	{
		Vector3 position = this.eventSource.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
		int layer = LayerMask.NameToLayer("Default");
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.transform.SetPosition(position);
			entry.controller.SetLayer(layer);
			entry.controller.gameObject.SetActive(false);
		}
		int hash = Hash.SDBMLower(this.enableEvent);
		int hash2 = Hash.SDBMLower(this.disableEvent);
		base.Subscribe(this.eventSource, hash, new Action<object>(this.Enable));
		base.Subscribe(this.eventSource, hash2, new Action<object>(this.Disable));
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x00091EC4 File Offset: 0x000900C4
	protected override void OnSpawn()
	{
		this.animEventHandler = base.GetComponentInParent<AnimEventHandler>();
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x00091ED4 File Offset: 0x000900D4
	private void Enable(object data)
	{
		this.StopAll();
		HashedString context = this.animEventHandler.GetContext();
		if (!context.IsValid)
		{
			return;
		}
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			if (entry.context == context)
			{
				entry.controller.gameObject.SetActive(true);
				entry.controller.Play(entry.anim, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x00091F7C File Offset: 0x0009017C
	private void Disable(object data)
	{
		this.StopAll();
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x00091F84 File Offset: 0x00090184
	private void StopAll()
	{
		foreach (KBatchedAnimEventToggler.Entry entry in this.entries)
		{
			entry.controller.StopAndClear();
			entry.controller.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000FAA RID: 4010
	[SerializeField]
	public GameObject eventSource;

	// Token: 0x04000FAB RID: 4011
	[SerializeField]
	public string enableEvent;

	// Token: 0x04000FAC RID: 4012
	[SerializeField]
	public string disableEvent;

	// Token: 0x04000FAD RID: 4013
	[SerializeField]
	public List<KBatchedAnimEventToggler.Entry> entries;

	// Token: 0x04000FAE RID: 4014
	private AnimEventHandler animEventHandler;

	// Token: 0x020012BB RID: 4795
	[Serializable]
	public struct Entry
	{
		// Token: 0x04006428 RID: 25640
		public string anim;

		// Token: 0x04006429 RID: 25641
		public HashedString context;

		// Token: 0x0400642A RID: 25642
		public KBatchedAnimController controller;
	}
}
