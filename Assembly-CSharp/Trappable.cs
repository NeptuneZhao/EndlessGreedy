using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000B39 RID: 2873
[AddComponentMenu("KMonoBehaviour/scripts/Trappable")]
public class Trappable : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x060055BC RID: 21948 RVA: 0x001EA294 File Offset: 0x001E8494
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x060055BD RID: 21949 RVA: 0x001EA2A8 File Offset: 0x001E84A8
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x060055BE RID: 21950 RVA: 0x001EA2B8 File Offset: 0x001E84B8
	private void OnCellChange()
	{
		int cell = Grid.PosToCell(this);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.trapsLayer, this);
	}

	// Token: 0x060055BF RID: 21951 RVA: 0x001EA2E2 File Offset: 0x001E84E2
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x060055C0 RID: 21952 RVA: 0x001EA2F0 File Offset: 0x001E84F0
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x060055C1 RID: 21953 RVA: 0x001EA300 File Offset: 0x001E8500
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		base.Subscribe<Trappable>(856640610, Trappable.OnStoreDelegate);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "Trappable.Register");
		this.registered = true;
	}

	// Token: 0x060055C2 RID: 21954 RVA: 0x001EA350 File Offset: 0x001E8550
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		base.Unsubscribe<Trappable>(856640610, Trappable.OnStoreDelegate, false);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		this.registered = false;
	}

	// Token: 0x060055C3 RID: 21955 RVA: 0x001EA38F File Offset: 0x001E858F
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_TRAP, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_TRAP, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x060055C4 RID: 21956 RVA: 0x001EA3B8 File Offset: 0x001E85B8
	public void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage && (storage.GetComponent<Trap>() != null || storage.GetSMI<ReusableTrap.Instance>() != null))
		{
			base.gameObject.AddTag(GameTags.Trapped);
			return;
		}
		base.gameObject.RemoveTag(GameTags.Trapped);
	}

	// Token: 0x04003835 RID: 14389
	private bool registered;

	// Token: 0x04003836 RID: 14390
	private static readonly EventSystem.IntraObjectHandler<Trappable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Trappable>(delegate(Trappable component, object data)
	{
		component.OnStore(data);
	});
}
