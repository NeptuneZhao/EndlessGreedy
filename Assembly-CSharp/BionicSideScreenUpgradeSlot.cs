using System;
using System.Collections;
using STRINGS;
using UnityEngine;

// Token: 0x02000D4D RID: 3405
public class BionicSideScreenUpgradeSlot : KMonoBehaviour
{
	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x06006B31 RID: 27441 RVA: 0x002858C7 File Offset: 0x00283AC7
	// (set) Token: 0x06006B30 RID: 27440 RVA: 0x002858BE File Offset: 0x00283ABE
	public BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot { get; private set; }

	// Token: 0x06006B32 RID: 27442 RVA: 0x002858CF File Offset: 0x00283ACF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnSlotClicked));
	}

	// Token: 0x06006B33 RID: 27443 RVA: 0x00285900 File Offset: 0x00283B00
	public void Setup(BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot)
	{
		if (this.upgradeSlot != null)
		{
			BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot2 = this.upgradeSlot;
			upgradeSlot2.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Remove(upgradeSlot2.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnAssignedUpgradeChanged));
		}
		this.upgradeSlot = upgradeSlot;
		if (upgradeSlot != null)
		{
			upgradeSlot.OnAssignedUpgradeChanged = (Action<BionicUpgradesMonitor.UpgradeComponentSlot>)Delegate.Combine(upgradeSlot.OnAssignedUpgradeChanged, new Action<BionicUpgradesMonitor.UpgradeComponentSlot>(this.OnAssignedUpgradeChanged));
		}
		this.Refresh();
	}

	// Token: 0x06006B34 RID: 27444 RVA: 0x0028596E File Offset: 0x00283B6E
	private void OnAssignedUpgradeChanged(BionicUpgradesMonitor.UpgradeComponentSlot slot)
	{
		this.Refresh();
	}

	// Token: 0x06006B35 RID: 27445 RVA: 0x00285978 File Offset: 0x00283B78
	public void Refresh()
	{
		if (this.wattageLabelEffectCoroutine != null)
		{
			base.StopCoroutine(this.wattageLabelEffectCoroutine);
			this.wattageLabelEffectCoroutine = null;
		}
		this.label.color = this.standardColor;
		if (this.upgradeSlot != null && this.upgradeSlot.HasUpgradeInstalled)
		{
			this.icon.sprite = Def.GetUISprite(this.upgradeSlot.installedUpgradeComponent.gameObject, "ui", false).first;
			this.icon.Opacity(1f);
			float currentWattage = this.upgradeSlot.installedUpgradeComponent.CurrentWattage;
			float potentialWattage = this.upgradeSlot.installedUpgradeComponent.PotentialWattage;
			bool flag = currentWattage != 0f;
			string str = flag ? ("<b>" + ((currentWattage >= 0f) ? "+" : "-") + "</b>") : "";
			this.label.SetText(string.Format(BionicSideScreenUpgradeSlot.TEXT_UPGRADE_WATTS, str + GameUtil.GetFormattedWattage((currentWattage != 0f) ? currentWattage : potentialWattage, GameUtil.WattageFormatterUnit.Automatic, true)));
			this.label.Opacity((currentWattage != 0f) ? 1f : 0.5f);
			this.icon.gameObject.SetActive(true);
			if (flag && base.gameObject.activeInHierarchy)
			{
				this.wattageLabelEffectCoroutine = base.StartCoroutine(this.UpgradeInUse_WattageLabelEffects());
			}
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
			if (flag)
			{
				string text = this.activeColorTooltip.ToHexString();
				str = string.Concat(new string[]
				{
					"<color=#",
					text,
					"><b>",
					(currentWattage >= 0f) ? "+" : "-",
					"</b>"
				});
				this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_INSTALLED_IN_USE, this.upgradeSlot.installedUpgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattage, GameUtil.WattageFormatterUnit.Automatic, true) + "</color>", this.upgradeSlot.installedUpgradeComponent.GetComponent<InfoDescription>().description));
			}
			else
			{
				this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_INSTALLED_NOT_IN_USE, this.upgradeSlot.installedUpgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(potentialWattage, GameUtil.WattageFormatterUnit.Automatic, true), this.upgradeSlot.installedUpgradeComponent.GetComponent<InfoDescription>().description));
			}
		}
		else if (this.upgradeSlot != null && this.upgradeSlot.HasUpgradeComponentAssigned && !this.upgradeSlot.GetAssignableSlotInstance().IsUnassigning())
		{
			this.icon.sprite = Def.GetUISprite(this.upgradeSlot.assignedUpgradeComponent.gameObject, "ui", false).first;
			this.icon.Opacity(0.5f);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_UPGRADE_ASSIGNED_NOT_INSTALLED);
			this.label.Opacity(1f);
			this.icon.gameObject.SetActive(true);
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
			this.tooltip.SetSimpleTooltip(string.Format(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_ASSIGNED, this.upgradeSlot.assignedUpgradeComponent.GetProperName()));
		}
		else
		{
			this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap;
			this.tooltip.SetSimpleTooltip(BionicSideScreenUpgradeSlot.TEXT_TOOLTIP_EMPTY);
			this.label.SetText(BionicSideScreenUpgradeSlot.TEXT_NO_UPGRADE_INSTALLED);
			this.label.Opacity(1f);
			this.icon.gameObject.SetActive(false);
		}
		this.SetSelected(this._isSelected);
	}

	// Token: 0x06006B36 RID: 27446 RVA: 0x00285CFF File Offset: 0x00283EFF
	private void OnSlotClicked()
	{
		Action<BionicSideScreenUpgradeSlot> onClick = this.OnClick;
		if (onClick == null)
		{
			return;
		}
		onClick(this);
	}

	// Token: 0x06006B37 RID: 27447 RVA: 0x00285D14 File Offset: 0x00283F14
	public void SetSelected(bool isSelected)
	{
		this._isSelected = isSelected;
		bool flag = this.upgradeSlot != null && this.upgradeSlot.HasUpgradeComponentAssigned && !this.upgradeSlot.GetAssignableSlotInstance().IsUnassigning();
		bool flag2 = flag && this.upgradeSlot.assignedUpgradeComponent.Rarity == BionicUpgradeComponentConfig.RarityType.Special;
		this.toggle.ChangeState((isSelected ? 1 : 0) + (flag ? 2 : 0) + ((flag && flag2) ? 2 : 0));
	}

	// Token: 0x06006B38 RID: 27448 RVA: 0x00285D92 File Offset: 0x00283F92
	private IEnumerator UpgradeInUse_WattageLabelEffects()
	{
		while (base.gameObject.activeInHierarchy)
		{
			float t = (Mathf.Sin(((this.inUseAnimationDuration <= 0f) ? 0f : (Time.time / this.inUseAnimationDuration * 2f * 3.1415927f)) - 1.5707964f) + 1f) * 0.5f;
			Color color = Color.Lerp(this.standardColor, this.activeColor, t);
			this.label.color = color;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400490E RID: 18702
	public static string TEXT_NO_UPGRADE_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_EMPTY;

	// Token: 0x0400490F RID: 18703
	public static string TEXT_UPGRADE_ASSIGNED_NOT_INSTALLED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_ASSIGNED;

	// Token: 0x04004910 RID: 18704
	public static string TEXT_UPGRADE_WATTS = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.UPGRADE_SLOT_WATTAGE;

	// Token: 0x04004911 RID: 18705
	public static string TEXT_TOOLTIP_EMPTY = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_EMPTY;

	// Token: 0x04004912 RID: 18706
	public static string TEXT_TOOLTIP_ASSIGNED = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_ASSIGNED;

	// Token: 0x04004913 RID: 18707
	public static string TEXT_TOOLTIP_INSTALLED_NOT_IN_USE = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_INSTALLED_NOT_IN_USE;

	// Token: 0x04004914 RID: 18708
	public static string TEXT_TOOLTIP_INSTALLED_IN_USE = UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.TOOLTIP.SLOT_INSTALLED_IN_USE;

	// Token: 0x04004915 RID: 18709
	public MultiToggle toggle;

	// Token: 0x04004916 RID: 18710
	public KImage icon;

	// Token: 0x04004917 RID: 18711
	public LocText label;

	// Token: 0x04004918 RID: 18712
	public ToolTip tooltip;

	// Token: 0x04004919 RID: 18713
	[Header("Effects settings")]
	public float inUseAnimationDuration = 0.5f;

	// Token: 0x0400491A RID: 18714
	public Color standardColor = Color.black;

	// Token: 0x0400491B RID: 18715
	public Color activeColor = Color.blue;

	// Token: 0x0400491C RID: 18716
	public Color activeColorTooltip = Color.blue;

	// Token: 0x0400491D RID: 18717
	public Action<BionicSideScreenUpgradeSlot> OnClick;

	// Token: 0x0400491F RID: 18719
	private bool _isSelected;

	// Token: 0x04004920 RID: 18720
	private Coroutine wattageLabelEffectCoroutine;
}
