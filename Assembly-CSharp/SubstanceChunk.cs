using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000B27 RID: 2855
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SubstanceChunk")]
public class SubstanceChunk : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x0600552A RID: 21802 RVA: 0x001E7048 File Offset: 0x001E5248
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Color color = base.GetComponent<PrimaryElement>().Element.substance.colour;
		color.a = 1f;
		base.GetComponent<KBatchedAnimController>().SetSymbolTint(SubstanceChunk.symbolToTint, color);
	}

	// Token: 0x0600552B RID: 21803 RVA: 0x001E7094 File Offset: 0x001E5294
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.RELEASEELEMENT.NAME, new System.Action(this.OnRelease), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.RELEASEELEMENT.TOOLTIP, true), 1f);
	}

	// Token: 0x0600552C RID: 21804 RVA: 0x001E70F0 File Offset: 0x001E52F0
	private void OnRelease()
	{
		int gameCell = Grid.PosToCell(base.transform.GetPosition());
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component.Mass > 0f)
		{
			SimMessages.AddRemoveSubstance(gameCell, component.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x040037CD RID: 14285
	private static readonly KAnimHashedString symbolToTint = new KAnimHashedString("substance_tinter");
}
