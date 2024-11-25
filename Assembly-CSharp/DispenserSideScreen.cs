using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D63 RID: 3427
public class DispenserSideScreen : SideScreenContent
{
	// Token: 0x06006BF2 RID: 27634 RVA: 0x00289DD2 File Offset: 0x00287FD2
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDispenser>() != null;
	}

	// Token: 0x06006BF3 RID: 27635 RVA: 0x00289DDD File Offset: 0x00287FDD
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetDispenser = target.GetComponent<IDispenser>();
		this.Refresh();
	}

	// Token: 0x06006BF4 RID: 27636 RVA: 0x00289DF8 File Offset: 0x00287FF8
	private void Refresh()
	{
		this.dispenseButton.ClearOnClick();
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			UnityEngine.Object.Destroy(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.targetDispenser.DispensedItems())
		{
			GameObject gameObject = Util.KInstantiateUI(this.itemRowPrefab, this.itemRowContainer.gameObject, true);
			this.rows.Add(tag, gameObject);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<Image>("Icon").sprite = Def.GetUISprite(tag, "ui", false).first;
			component.GetReference<LocText>("Label").text = Assets.GetPrefab(tag).GetProperName();
			gameObject.GetComponent<MultiToggle>().ChangeState((tag == this.targetDispenser.SelectedItem()) ? 0 : 1);
		}
		if (this.targetDispenser.HasOpenChore())
		{
			this.dispenseButton.onClick += delegate()
			{
				this.targetDispenser.OnCancelDispense();
				this.Refresh();
			};
			this.dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_CANCEL;
		}
		else
		{
			this.dispenseButton.onClick += delegate()
			{
				this.targetDispenser.OnOrderDispense();
				this.Refresh();
			};
			this.dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_DISPENSE;
		}
		this.targetDispenser.OnStopWorkEvent -= this.Refresh;
		this.targetDispenser.OnStopWorkEvent += this.Refresh;
	}

	// Token: 0x06006BF5 RID: 27637 RVA: 0x00289FDC File Offset: 0x002881DC
	private void SelectTag(Tag tag)
	{
		this.targetDispenser.SelectItem(tag);
		this.Refresh();
	}

	// Token: 0x0400499E RID: 18846
	[SerializeField]
	private KButton dispenseButton;

	// Token: 0x0400499F RID: 18847
	[SerializeField]
	private RectTransform itemRowContainer;

	// Token: 0x040049A0 RID: 18848
	[SerializeField]
	private GameObject itemRowPrefab;

	// Token: 0x040049A1 RID: 18849
	private IDispenser targetDispenser;

	// Token: 0x040049A2 RID: 18850
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
