using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000D6D RID: 3437
public class GeneShufflerSideScreen : SideScreenContent
{
	// Token: 0x06006C26 RID: 27686 RVA: 0x0028AFC9 File Offset: 0x002891C9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += this.OnButtonClick;
		this.Refresh();
	}

	// Token: 0x06006C27 RID: 27687 RVA: 0x0028AFEE File Offset: 0x002891EE
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<GeneShuffler>() != null;
	}

	// Token: 0x06006C28 RID: 27688 RVA: 0x0028AFFC File Offset: 0x002891FC
	public override void SetTarget(GameObject target)
	{
		GeneShuffler component = target.GetComponent<GeneShuffler>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a GeneShuffler associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x06006C29 RID: 27689 RVA: 0x0028B034 File Offset: 0x00289234
	private void OnButtonClick()
	{
		if (this.target.WorkComplete)
		{
			this.target.SetWorkTime(0f);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.target.RequestRecharge(!this.target.RechargeRequested);
			this.Refresh();
		}
	}

	// Token: 0x06006C2A RID: 27690 RVA: 0x0028B08C File Offset: 0x0028928C
	private void Refresh()
	{
		if (!(this.target != null))
		{
			this.contents.SetActive(false);
			return;
		}
		if (this.target.WorkComplete)
		{
			this.contents.SetActive(true);
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.COMPLETE;
			this.button.gameObject.SetActive(true);
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON;
			return;
		}
		if (this.target.IsConsumed)
		{
			this.contents.SetActive(true);
			this.button.gameObject.SetActive(true);
			if (this.target.RechargeRequested)
			{
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED_WAITING;
				this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE_CANCEL;
				return;
			}
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED;
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE;
			return;
		}
		else
		{
			if (this.target.IsWorking)
			{
				this.contents.SetActive(true);
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.UNDERWAY;
				this.button.gameObject.SetActive(false);
				return;
			}
			this.contents.SetActive(false);
			return;
		}
	}

	// Token: 0x040049C0 RID: 18880
	[SerializeField]
	private LocText label;

	// Token: 0x040049C1 RID: 18881
	[SerializeField]
	private KButton button;

	// Token: 0x040049C2 RID: 18882
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x040049C3 RID: 18883
	[SerializeField]
	private GeneShuffler target;

	// Token: 0x040049C4 RID: 18884
	[SerializeField]
	private GameObject contents;
}
