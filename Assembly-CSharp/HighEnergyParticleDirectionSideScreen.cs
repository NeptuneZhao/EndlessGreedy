using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D72 RID: 3442
public class HighEnergyParticleDirectionSideScreen : SideScreenContent
{
	// Token: 0x06006C4D RID: 27725 RVA: 0x0028BE37 File Offset: 0x0028A037
	public override string GetTitle()
	{
		return UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.TITLE;
	}

	// Token: 0x06006C4E RID: 27726 RVA: 0x0028BE44 File Offset: 0x0028A044
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.Buttons.Count; i++)
		{
			KButton button = this.Buttons[i];
			button.onClick += delegate()
			{
				int num = this.Buttons.IndexOf(button);
				if (this.activeButton != null)
				{
					this.activeButton.isInteractable = true;
				}
				button.isInteractable = false;
				this.activeButton = button;
				if (this.target != null)
				{
					this.target.Direction = EightDirectionUtil.AngleToDirection(num * 45);
					Game.Instance.ForceOverlayUpdate(true);
					this.Refresh();
				}
			};
		}
	}

	// Token: 0x06006C4F RID: 27727 RVA: 0x0028BEA3 File Offset: 0x0028A0A3
	public override int GetSideScreenSortOrder()
	{
		return 10;
	}

	// Token: 0x06006C50 RID: 27728 RVA: 0x0028BEA8 File Offset: 0x0028A0A8
	public override bool IsValidForTarget(GameObject target)
	{
		HighEnergyParticleRedirector component = target.GetComponent<HighEnergyParticleRedirector>();
		bool flag = component != null;
		if (flag)
		{
			flag = (flag && component.directionControllable);
		}
		bool flag2 = target.GetComponent<HighEnergyParticleSpawner>() != null || target.GetComponent<ManualHighEnergyParticleSpawner>() != null || target.GetComponent<DevHEPSpawner>() != null;
		return (flag || flag2) && target.GetComponent<IHighEnergyParticleDirection>() != null;
	}

	// Token: 0x06006C51 RID: 27729 RVA: 0x0028BF10 File Offset: 0x0028A110
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IHighEnergyParticleDirection>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain IHighEnergyParticleDirection component");
			return;
		}
		this.Refresh();
	}

	// Token: 0x06006C52 RID: 27730 RVA: 0x0028BF4C File Offset: 0x0028A14C
	private void Refresh()
	{
		int directionIndex = EightDirectionUtil.GetDirectionIndex(this.target.Direction);
		if (directionIndex >= 0 && directionIndex < this.Buttons.Count)
		{
			this.Buttons[directionIndex].SignalClick(KKeyCode.Mouse0);
		}
		else
		{
			if (this.activeButton)
			{
				this.activeButton.isInteractable = true;
			}
			this.activeButton = null;
		}
		this.directionLabel.SetText(string.Format(UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.SELECTED_DIRECTION, this.directionStrings[directionIndex]));
	}

	// Token: 0x040049D9 RID: 18905
	private IHighEnergyParticleDirection target;

	// Token: 0x040049DA RID: 18906
	public List<KButton> Buttons;

	// Token: 0x040049DB RID: 18907
	private KButton activeButton;

	// Token: 0x040049DC RID: 18908
	public LocText directionLabel;

	// Token: 0x040049DD RID: 18909
	private string[] directionStrings = new string[]
	{
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_N,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NW,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_W,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SW,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_S,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SE,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_E,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NE
	};
}
