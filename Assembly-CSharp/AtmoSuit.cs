using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000659 RID: 1625
[AddComponentMenu("KMonoBehaviour/scripts/AtmoSuit")]
public class AtmoSuit : KMonoBehaviour
{
	// Token: 0x06002815 RID: 10261 RVA: 0x000E3857 File Offset: 0x000E1A57
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<AtmoSuit>(-1697596308, AtmoSuit.OnStorageChangedDelegate);
	}

	// Token: 0x06002816 RID: 10262 RVA: 0x000E3870 File Offset: 0x000E1A70
	private void RefreshStatusEffects(object data)
	{
		if (this == null)
		{
			return;
		}
		Equippable component = base.GetComponent<Equippable>();
		bool flag = base.GetComponent<Storage>().Has(GameTags.AnyWater);
		if (component.assignee != null && flag)
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					AssignableSlotInstance slot = ((KMonoBehaviour)component.assignee).GetComponent<Equipment>().GetSlot(component.slot);
					Effects component2 = targetGameObject.GetComponent<Effects>();
					if (component2 != null && !component2.HasEffect("SoiledSuit") && !slot.IsUnassigning())
					{
						component2.Add("SoiledSuit", true);
					}
				}
			}
		}
	}

	// Token: 0x0400171E RID: 5918
	private static readonly EventSystem.IntraObjectHandler<AtmoSuit> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<AtmoSuit>(delegate(AtmoSuit component, object data)
	{
		component.RefreshStatusEffects(data);
	});
}
