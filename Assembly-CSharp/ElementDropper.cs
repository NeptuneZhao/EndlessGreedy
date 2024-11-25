using System;
using UnityEngine;

// Token: 0x020006C9 RID: 1737
[AddComponentMenu("KMonoBehaviour/scripts/ElementDropper")]
public class ElementDropper : KMonoBehaviour
{
	// Token: 0x06002BF0 RID: 11248 RVA: 0x000F6C6E File Offset: 0x000F4E6E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ElementDropper>(-1697596308, ElementDropper.OnStorageChangedDelegate);
	}

	// Token: 0x06002BF1 RID: 11249 RVA: 0x000F6C87 File Offset: 0x000F4E87
	private void OnStorageChanged(object data)
	{
		if (this.storage.GetMassAvailable(this.emitTag) >= this.emitMass)
		{
			this.storage.DropSome(this.emitTag, this.emitMass, false, false, this.emitOffset, true, true);
		}
	}

	// Token: 0x0400194A RID: 6474
	[SerializeField]
	public Tag emitTag;

	// Token: 0x0400194B RID: 6475
	[SerializeField]
	public float emitMass;

	// Token: 0x0400194C RID: 6476
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x0400194D RID: 6477
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400194E RID: 6478
	private static readonly EventSystem.IntraObjectHandler<ElementDropper> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ElementDropper>(delegate(ElementDropper component, object data)
	{
		component.OnStorageChanged(data);
	});
}
