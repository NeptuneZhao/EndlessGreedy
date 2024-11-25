using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D4C RID: 3404
public class BionicSideScreen : SideScreenContent
{
	// Token: 0x06006B1E RID: 27422 RVA: 0x002853DC File Offset: 0x002835DC
	private void OnBionicUpgradeSlotClicked(BionicSideScreenUpgradeSlot slotClicked)
	{
		bool flag = slotClicked == null || this.lastSlotSelected == slotClicked.upgradeSlot.GetAssignableSlotInstance();
		this.lastSlotSelected = (flag ? null : slotClicked.upgradeSlot.GetAssignableSlotInstance());
		this.RefreshSelectedStateInSlots();
		AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
		AssignableSlotInstance assignableSlotInstance = flag ? null : slotClicked.upgradeSlot.GetAssignableSlotInstance();
		if (this.ownableSidescreen != null)
		{
			this.ownableSidescreen.SetSelectedSlot(assignableSlotInstance);
			return;
		}
		if (flag)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			return;
		}
		((OwnablesSecondSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.ownableSecondSideScreenPrefab, bionicUpgrade.Name)).SetSlot(assignableSlotInstance);
	}

	// Token: 0x06006B1F RID: 27423 RVA: 0x00285494 File Offset: 0x00283694
	private void RefreshSelectedStateInSlots()
	{
		for (int i = 0; i < this.bionicSlots.Count; i++)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
		}
	}

	// Token: 0x06006B20 RID: 27424 RVA: 0x002854DC File Offset: 0x002836DC
	public void RecreateBionicSlots()
	{
		int num = (this.upgradeMonitor != null) ? this.upgradeMonitor.def.SlotCount : 0;
		for (int i = 0; i < Mathf.Max(num, this.bionicSlots.Count); i++)
		{
			if (i >= this.bionicSlots.Count)
			{
				BionicSideScreenUpgradeSlot item = this.CreateBionicSlot();
				this.bionicSlots.Add(item);
			}
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			if (i < num)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot = this.upgradeMonitor.upgradeComponentSlots[i];
				bionicSideScreenUpgradeSlot.gameObject.SetActive(true);
				bionicSideScreenUpgradeSlot.Setup(upgradeSlot);
				bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
			}
			else
			{
				bionicSideScreenUpgradeSlot.Setup(null);
				bionicSideScreenUpgradeSlot.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06006B21 RID: 27425 RVA: 0x002855AC File Offset: 0x002837AC
	private BionicSideScreenUpgradeSlot CreateBionicSlot()
	{
		BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = Util.KInstantiateUI<BionicSideScreenUpgradeSlot>(this.originalBionicSlot.gameObject, this.originalBionicSlot.transform.parent.gameObject, false);
		bionicSideScreenUpgradeSlot.OnClick = (Action<BionicSideScreenUpgradeSlot>)Delegate.Combine(bionicSideScreenUpgradeSlot.OnClick, new Action<BionicSideScreenUpgradeSlot>(this.OnBionicUpgradeSlotClicked));
		return bionicSideScreenUpgradeSlot;
	}

	// Token: 0x06006B22 RID: 27426 RVA: 0x00285601 File Offset: 0x00283801
	private void OnBionicUpgradeChanged(object o)
	{
		this.RecreateBionicSlots();
	}

	// Token: 0x06006B23 RID: 27427 RVA: 0x00285609 File Offset: 0x00283809
	private void OnBionicBecameOnline(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x06006B24 RID: 27428 RVA: 0x00285611 File Offset: 0x00283811
	private void OnBionicBecameOffline(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x06006B25 RID: 27429 RVA: 0x00285619 File Offset: 0x00283819
	private void OnBionicWattageChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x06006B26 RID: 27430 RVA: 0x00285621 File Offset: 0x00283821
	private void OnBionicBatterySaveModeChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x06006B27 RID: 27431 RVA: 0x0028562C File Offset: 0x0028382C
	private void RefreshSlots()
	{
		for (int i = 0; i < this.bionicSlots.Count; i++)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			if (bionicSideScreenUpgradeSlot != null)
			{
				bionicSideScreenUpgradeSlot.Refresh();
			}
		}
	}

	// Token: 0x06006B28 RID: 27432 RVA: 0x0028566C File Offset: 0x0028386C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.originalBionicSlot.gameObject.SetActive(false);
		this.ownableSidescreen = base.transform.parent.GetComponentInChildren<OwnablesSidescreen>();
		if (this.ownableSidescreen != null)
		{
			OwnablesSidescreen ownablesSidescreen = this.ownableSidescreen;
			ownablesSidescreen.OnSlotInstanceSelected = (Action<AssignableSlotInstance>)Delegate.Combine(ownablesSidescreen.OnSlotInstanceSelected, new Action<AssignableSlotInstance>(this.OnOwnableSidescreenRowSelected));
		}
	}

	// Token: 0x06006B29 RID: 27433 RVA: 0x002856DB File Offset: 0x002838DB
	private void OnOwnableSidescreenRowSelected(AssignableSlotInstance slot)
	{
		this.lastSlotSelected = slot;
		this.RefreshSelectedStateInSlots();
	}

	// Token: 0x06006B2A RID: 27434 RVA: 0x002856EC File Offset: 0x002838EC
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.lastSlotSelected = null;
		if (this.upgradeMonitor != null)
		{
			this.upgradeMonitor.Unsubscribe(160824499, new Action<object>(this.OnBionicBecameOnline));
			this.upgradeMonitor.Unsubscribe(-1730800797, new Action<object>(this.OnBionicBecameOffline));
			this.upgradeMonitor.Unsubscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		}
		if (this.batteryMonitor != null)
		{
			this.batteryMonitor.Unsubscribe(1361471071, new Action<object>(this.OnBionicWattageChanged));
			this.batteryMonitor.Unsubscribe(-426516281, new Action<object>(this.OnBionicBatterySaveModeChanged));
		}
		this.batteryMonitor = target.GetSMI<BionicBatteryMonitor.Instance>();
		this.upgradeMonitor = target.GetSMI<BionicUpgradesMonitor.Instance>();
		this.upgradeMonitor.Subscribe(160824499, new Action<object>(this.OnBionicBecameOnline));
		this.upgradeMonitor.Subscribe(-1730800797, new Action<object>(this.OnBionicBecameOffline));
		this.upgradeMonitor.Subscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		this.batteryMonitor.Subscribe(1361471071, new Action<object>(this.OnBionicWattageChanged));
		this.batteryMonitor.Subscribe(-426516281, new Action<object>(this.OnBionicBatterySaveModeChanged));
		this.RecreateBionicSlots();
	}

	// Token: 0x06006B2B RID: 27435 RVA: 0x0028584D File Offset: 0x00283A4D
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshSlots();
		}
	}

	// Token: 0x06006B2C RID: 27436 RVA: 0x0028585F File Offset: 0x00283A5F
	public override void ClearTarget()
	{
		base.ClearTarget();
		if (this.upgradeMonitor != null)
		{
			this.upgradeMonitor.Unsubscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		}
		this.upgradeMonitor = null;
		this.lastSlotSelected = null;
	}

	// Token: 0x06006B2D RID: 27437 RVA: 0x00285899 File Offset: 0x00283A99
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<BionicBatteryMonitor.Instance>() != null;
	}

	// Token: 0x06006B2E RID: 27438 RVA: 0x002858A4 File Offset: 0x00283AA4
	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	// Token: 0x04004907 RID: 18695
	public OwnablesSecondSideScreen ownableSecondSideScreenPrefab;

	// Token: 0x04004908 RID: 18696
	public BionicSideScreenUpgradeSlot originalBionicSlot;

	// Token: 0x04004909 RID: 18697
	private BionicUpgradesMonitor.Instance upgradeMonitor;

	// Token: 0x0400490A RID: 18698
	private BionicBatteryMonitor.Instance batteryMonitor;

	// Token: 0x0400490B RID: 18699
	private List<BionicSideScreenUpgradeSlot> bionicSlots = new List<BionicSideScreenUpgradeSlot>();

	// Token: 0x0400490C RID: 18700
	private OwnablesSidescreen ownableSidescreen;

	// Token: 0x0400490D RID: 18701
	private AssignableSlotInstance lastSlotSelected;
}
