using System;
using UnityEngine;

// Token: 0x02000B29 RID: 2857
[AddComponentMenu("KMonoBehaviour/scripts/SuitDiseaseHandler")]
public class SuitDiseaseHandler : KMonoBehaviour
{
	// Token: 0x06005536 RID: 21814 RVA: 0x001E7285 File Offset: 0x001E5485
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitDiseaseHandler>(-1617557748, SuitDiseaseHandler.OnEquippedDelegate);
		base.Subscribe<SuitDiseaseHandler>(-170173755, SuitDiseaseHandler.OnUnequippedDelegate);
	}

	// Token: 0x06005537 RID: 21815 RVA: 0x001E72B0 File Offset: 0x001E54B0
	private PrimaryElement GetPrimaryElement(object data)
	{
		GameObject targetGameObject = ((Equipment)data).GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		if (targetGameObject)
		{
			return targetGameObject.GetComponent<PrimaryElement>();
		}
		return null;
	}

	// Token: 0x06005538 RID: 21816 RVA: 0x001E72E0 File Offset: 0x001E54E0
	private void OnEquipped(object data)
	{
		PrimaryElement primaryElement = this.GetPrimaryElement(data);
		if (primaryElement != null)
		{
			primaryElement.ForcePermanentDiseaseContainer(true);
			primaryElement.RedirectDisease(base.gameObject);
		}
	}

	// Token: 0x06005539 RID: 21817 RVA: 0x001E7314 File Offset: 0x001E5514
	private void OnUnequipped(object data)
	{
		PrimaryElement primaryElement = this.GetPrimaryElement(data);
		if (primaryElement != null)
		{
			primaryElement.ForcePermanentDiseaseContainer(false);
			primaryElement.RedirectDisease(null);
		}
	}

	// Token: 0x0600553A RID: 21818 RVA: 0x001E7340 File Offset: 0x001E5540
	private void OnModifyDiseaseCount(int delta, string reason)
	{
		base.GetComponent<PrimaryElement>().ModifyDiseaseCount(delta, reason);
	}

	// Token: 0x0600553B RID: 21819 RVA: 0x001E734F File Offset: 0x001E554F
	private void OnAddDisease(byte disease_idx, int delta, string reason)
	{
		base.GetComponent<PrimaryElement>().AddDisease(disease_idx, delta, reason);
	}

	// Token: 0x040037D1 RID: 14289
	private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>(delegate(SuitDiseaseHandler component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040037D2 RID: 14290
	private static readonly EventSystem.IntraObjectHandler<SuitDiseaseHandler> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitDiseaseHandler>(delegate(SuitDiseaseHandler component, object data)
	{
		component.OnUnequipped(data);
	});
}
