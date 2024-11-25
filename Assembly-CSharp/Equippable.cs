using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000893 RID: 2195
[SerializationConfig(MemberSerialization.OptIn)]
public class Equippable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor, IQuality
{
	// Token: 0x06003D7D RID: 15741 RVA: 0x001541B2 File Offset: 0x001523B2
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x06003D7E RID: 15742 RVA: 0x001541BA File Offset: 0x001523BA
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06003D7F RID: 15743 RVA: 0x001541C3 File Offset: 0x001523C3
	// (set) Token: 0x06003D80 RID: 15744 RVA: 0x001541D0 File Offset: 0x001523D0
	public EquipmentDef def
	{
		get
		{
			return this.defHandle.Get<EquipmentDef>();
		}
		set
		{
			this.defHandle.Set<EquipmentDef>(value);
		}
	}

	// Token: 0x06003D81 RID: 15745 RVA: 0x001541E0 File Offset: 0x001523E0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.def.AdditionalTags != null)
		{
			foreach (Tag tag in this.def.AdditionalTags)
			{
				base.GetComponent<KPrefabID>().AddTag(tag, false);
			}
		}
	}

	// Token: 0x06003D82 RID: 15746 RVA: 0x00154230 File Offset: 0x00152430
	protected override void OnSpawn()
	{
		Components.AssignableItems.Add(this);
		if (this.isEquipped)
		{
			if (this.assignee != null && this.assignee is MinionIdentity)
			{
				this.assignee = (this.assignee as MinionIdentity).assignableProxy.Get();
				this.assignee_identityRef.Set(this.assignee as KMonoBehaviour);
			}
			if (this.assignee == null && this.assignee_identityRef.Get() != null)
			{
				this.assignee = this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
			}
			if (this.assignee != null)
			{
				this.assignee.GetSoleOwner().GetComponent<Equipment>().Equip(this);
			}
			else
			{
				global::Debug.LogWarning("Equippable trying to be equipped to missing prefab");
				this.isEquipped = false;
			}
		}
		base.Subscribe<Equippable>(1969584890, Equippable.SetDestroyedTrueDelegate);
	}

	// Token: 0x06003D83 RID: 15747 RVA: 0x0015430C File Offset: 0x0015250C
	public KAnimFile GetBuildOverride()
	{
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component == null || component.BuildOverride == null)
		{
			return this.def.BuildOverride;
		}
		return Assets.GetAnim(component.BuildOverride);
	}

	// Token: 0x06003D84 RID: 15748 RVA: 0x00154350 File Offset: 0x00152550
	public override void Assign(IAssignableIdentity new_assignee)
	{
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (base.slot != null && new_assignee is MinionIdentity)
		{
			new_assignee = (new_assignee as MinionIdentity).assignableProxy.Get();
		}
		if (base.slot != null && new_assignee is StoredMinionIdentity)
		{
			new_assignee = (new_assignee as StoredMinionIdentity).assignableProxy.Get();
		}
		if (new_assignee is MinionAssignablesProxy)
		{
			AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Equipment>().GetSlot(base.slot);
			if (slot != null)
			{
				Assignable assignable = slot.assignable;
				if (assignable != null)
				{
					assignable.Unassign();
				}
			}
		}
		base.Assign(new_assignee);
	}

	// Token: 0x06003D85 RID: 15749 RVA: 0x001543EC File Offset: 0x001525EC
	public override void Unassign()
	{
		if (this.isEquipped)
		{
			((this.assignee is MinionIdentity) ? ((MinionIdentity)this.assignee).assignableProxy.Get().GetComponent<Equipment>() : ((KMonoBehaviour)this.assignee).GetComponent<Equipment>()).Unequip(this);
			this.OnUnequip();
		}
		base.Unassign();
	}

	// Token: 0x06003D86 RID: 15750 RVA: 0x0015444C File Offset: 0x0015264C
	public void OnEquip(AssignableSlotInstance slot)
	{
		this.isEquipped = true;
		if (SelectTool.Instance.selected == this.selectable)
		{
			SelectTool.Instance.Select(null, false);
		}
		base.GetComponent<KBatchedAnimController>().enabled = false;
		base.GetComponent<KSelectable>().IsSelectable = false;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		GameObject targetGameObject = slot.gameObject.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		Effects component = targetGameObject.GetComponent<Effects>();
		if (component != null)
		{
			foreach (Effect effect in this.def.EffectImmunites)
			{
				component.AddImmunity(effect, name, true);
			}
		}
		if (this.def.OnEquipCallBack != null)
		{
			this.def.OnEquipCallBack(this);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Equipped, false);
		targetGameObject.Trigger(-210173199, this);
	}

	// Token: 0x06003D87 RID: 15751 RVA: 0x00154558 File Offset: 0x00152758
	public void OnUnequip()
	{
		this.isEquipped = false;
		if (this.destroyed)
		{
			return;
		}
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Equipped);
		base.GetComponent<KBatchedAnimController>().enabled = true;
		base.GetComponent<KSelectable>().IsSelectable = true;
		string name = base.GetComponent<KPrefabID>().PrefabTag.Name;
		if (this.assignee != null)
		{
			Ownables soleOwner = this.assignee.GetSoleOwner();
			if (soleOwner)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					Effects component = targetGameObject.GetComponent<Effects>();
					if (component != null)
					{
						foreach (Effect effect in this.def.EffectImmunites)
						{
							component.RemoveImmunity(effect, name);
						}
					}
				}
			}
		}
		if (this.def.OnUnequipCallBack != null)
		{
			this.def.OnUnequipCallBack(this);
		}
		if (this.assignee != null)
		{
			Ownables soleOwner2 = this.assignee.GetSoleOwner();
			if (soleOwner2)
			{
				GameObject targetGameObject2 = soleOwner2.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject2)
				{
					targetGameObject2.Trigger(-1841406856, this);
				}
			}
		}
	}

	// Token: 0x06003D88 RID: 15752 RVA: 0x001546A0 File Offset: 0x001528A0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.def != null)
		{
			List<Descriptor> equipmentEffects = GameUtil.GetEquipmentEffects(this.def);
			if (this.def.additionalDescriptors != null)
			{
				foreach (Descriptor item in this.def.additionalDescriptors)
				{
					equipmentEffects.Add(item);
				}
			}
			return equipmentEffects;
		}
		return new List<Descriptor>();
	}

	// Token: 0x0400258C RID: 9612
	private global::QualityLevel quality;

	// Token: 0x0400258D RID: 9613
	[MyCmpAdd]
	private EquippableWorkable equippableWorkable;

	// Token: 0x0400258E RID: 9614
	[MyCmpAdd]
	private EquippableFacade facade;

	// Token: 0x0400258F RID: 9615
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002590 RID: 9616
	public DefHandle defHandle;

	// Token: 0x04002591 RID: 9617
	[Serialize]
	public bool isEquipped;

	// Token: 0x04002592 RID: 9618
	private bool destroyed;

	// Token: 0x04002593 RID: 9619
	[Serialize]
	public bool unequippable = true;

	// Token: 0x04002594 RID: 9620
	[Serialize]
	public bool hideInCodex;

	// Token: 0x04002595 RID: 9621
	private static readonly EventSystem.IntraObjectHandler<Equippable> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equippable>(delegate(Equippable component, object data)
	{
		component.destroyed = true;
	});
}
