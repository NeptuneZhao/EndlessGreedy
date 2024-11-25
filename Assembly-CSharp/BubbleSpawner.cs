using System;
using UnityEngine;

// Token: 0x02000689 RID: 1673
[AddComponentMenu("KMonoBehaviour/scripts/BubbleSpawner")]
public class BubbleSpawner : KMonoBehaviour
{
	// Token: 0x060029B5 RID: 10677 RVA: 0x000EB4E5 File Offset: 0x000E96E5
	protected override void OnSpawn()
	{
		this.emitMass += (UnityEngine.Random.value - 0.5f) * this.emitVariance * this.emitMass;
		base.OnSpawn();
		base.Subscribe<BubbleSpawner>(-1697596308, BubbleSpawner.OnStorageChangedDelegate);
	}

	// Token: 0x060029B6 RID: 10678 RVA: 0x000EB524 File Offset: 0x000E9724
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = this.storage.FindFirst(ElementLoader.FindElementByHash(this.element).tag);
		if (gameObject == null)
		{
			return;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		if (component.Mass >= this.emitMass)
		{
			gameObject.GetComponent<PrimaryElement>().Mass -= this.emitMass;
			BubbleManager.instance.SpawnBubble(base.transform.GetPosition(), this.initialVelocity, component.ElementID, this.emitMass, component.Temperature);
		}
	}

	// Token: 0x04001808 RID: 6152
	public SimHashes element;

	// Token: 0x04001809 RID: 6153
	public float emitMass;

	// Token: 0x0400180A RID: 6154
	public float emitVariance;

	// Token: 0x0400180B RID: 6155
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x0400180C RID: 6156
	public Vector2 initialVelocity;

	// Token: 0x0400180D RID: 6157
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400180E RID: 6158
	private static readonly EventSystem.IntraObjectHandler<BubbleSpawner> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<BubbleSpawner>(delegate(BubbleSpawner component, object data)
	{
		component.OnStorageChanged(data);
	});
}
