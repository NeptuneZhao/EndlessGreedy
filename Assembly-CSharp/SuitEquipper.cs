using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000B2A RID: 2858
[AddComponentMenu("KMonoBehaviour/scripts/SuitEquipper")]
public class SuitEquipper : KMonoBehaviour
{
	// Token: 0x0600553E RID: 21822 RVA: 0x001E739D File Offset: 0x001E559D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SuitEquipper>(493375141, SuitEquipper.OnRefreshUserMenuDelegate);
	}

	// Token: 0x0600553F RID: 21823 RVA: 0x001E73B8 File Offset: 0x001E55B8
	private void OnRefreshUserMenu(object data)
	{
		foreach (AssignableSlotInstance assignableSlotInstance in base.GetComponent<MinionIdentity>().GetEquipment().Slots)
		{
			EquipmentSlotInstance equipmentSlotInstance = (EquipmentSlotInstance)assignableSlotInstance;
			Equippable equippable = equipmentSlotInstance.assignable as Equippable;
			if (equippable && equippable.unequippable)
			{
				string text = string.Format(UI.USERMENUACTIONS.UNEQUIP.NAME, equippable.def.GenericName);
				Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("iconDown", text, delegate()
				{
					equippable.Unassign();
				}, global::Action.NumActions, null, null, null, "", true), 2f);
			}
		}
	}

	// Token: 0x06005540 RID: 21824 RVA: 0x001E74AC File Offset: 0x001E56AC
	public Equippable IsWearingAirtightSuit()
	{
		Equippable result = null;
		foreach (AssignableSlotInstance assignableSlotInstance in base.GetComponent<MinionIdentity>().GetEquipment().Slots)
		{
			Equippable equippable = ((EquipmentSlotInstance)assignableSlotInstance).assignable as Equippable;
			if (equippable && equippable.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit) && equippable.isEquipped)
			{
				result = equippable;
				break;
			}
		}
		return result;
	}

	// Token: 0x040037D3 RID: 14291
	private static readonly EventSystem.IntraObjectHandler<SuitEquipper> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitEquipper>(delegate(SuitEquipper component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
