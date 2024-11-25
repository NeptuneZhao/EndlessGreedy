using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class EquippableBalloonConfig : IEquipmentConfig
{
	// Token: 0x0600027F RID: 639 RVA: 0x00011D07 File Offset: 0x0000FF07
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000280 RID: 640 RVA: 0x00011D10 File Offset: 0x0000FF10
	public EquipmentDef CreateEquipmentDef()
	{
		List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
		EquipmentDef equipmentDef = EquipmentTemplates.CreateEquipmentDef("EquippableBalloon", EQUIPMENT.TOYS.SLOT, SimHashes.Carbon, EQUIPMENT.TOYS.BALLOON_MASS, EQUIPMENT.VESTS.WARM_VEST_ICON0, null, null, 0, attributeModifiers, null, false, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, null, null);
		equipmentDef.OnEquipCallBack = new Action<Equippable>(this.OnEquipBalloon);
		equipmentDef.OnUnequipCallBack = new Action<Equippable>(this.OnUnequipBalloon);
		return equipmentDef;
	}

	// Token: 0x06000281 RID: 641 RVA: 0x00011D78 File Offset: 0x0000FF78
	private void OnEquipBalloon(Equippable eq)
	{
		if (!eq.IsNullOrDestroyed() && !eq.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target;
			Effects component = kmonoBehaviour.GetComponent<Effects>();
			KSelectable component2 = kmonoBehaviour.GetComponent<KSelectable>();
			if (!component.IsNullOrDestroyed())
			{
				component.Add("HasBalloon", false);
				EquippableBalloon component3 = eq.GetComponent<EquippableBalloon>();
				EquippableBalloon.StatesInstance data = (EquippableBalloon.StatesInstance)component3.GetSMI();
				component2.AddStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon, data);
				this.SpawnFxInstanceFor(kmonoBehaviour);
				component3.ApplyBalloonOverrideToBalloonFx();
			}
		}
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00011E20 File Offset: 0x00010020
	private void OnUnequipBalloon(Equippable eq)
	{
		if (!eq.IsNullOrDestroyed() && !eq.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			MinionAssignablesProxy component = soleOwner.GetComponent<MinionAssignablesProxy>();
			if (!component.target.IsNullOrDestroyed())
			{
				KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)component.target;
				Effects component2 = kmonoBehaviour.GetComponent<Effects>();
				KSelectable component3 = kmonoBehaviour.GetComponent<KSelectable>();
				if (!component2.IsNullOrDestroyed())
				{
					component2.Remove("HasBalloon");
					component3.RemoveStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_HasBalloon, false);
					this.DestroyFxInstanceFor(kmonoBehaviour);
				}
			}
		}
		Util.KDestroyGameObject(eq.gameObject);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x00011EC8 File Offset: 0x000100C8
	public void DoPostConfigure(GameObject go)
	{
		go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
		Equippable equippable = go.GetComponent<Equippable>();
		if (equippable.IsNullOrDestroyed())
		{
			equippable = go.AddComponent<Equippable>();
		}
		equippable.hideInCodex = true;
		equippable.unequippable = false;
		go.AddOrGet<EquippableBalloon>();
	}

	// Token: 0x06000284 RID: 644 RVA: 0x00011F11 File Offset: 0x00010111
	private void SpawnFxInstanceFor(KMonoBehaviour target)
	{
		new BalloonFX.Instance(target.GetComponent<KMonoBehaviour>()).StartSM();
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00011F23 File Offset: 0x00010123
	private void DestroyFxInstanceFor(KMonoBehaviour target)
	{
		target.GetSMI<BalloonFX.Instance>().StopSM("Unequipped");
	}

	// Token: 0x04000177 RID: 375
	public const string ID = "EquippableBalloon";
}
