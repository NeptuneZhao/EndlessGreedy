using System;
using Database;
using UnityEngine;

// Token: 0x02000C6E RID: 3182
public readonly struct JoyResponseScreenConfig
{
	// Token: 0x0600619F RID: 24991 RVA: 0x002475A3 File Offset: 0x002457A3
	private JoyResponseScreenConfig(JoyResponseOutfitTarget target, Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem)
	{
		this.target = target;
		this.initalSelectedItem = initalSelectedItem;
		this.isValid = true;
	}

	// Token: 0x060061A0 RID: 24992 RVA: 0x002475BA File Offset: 0x002457BA
	public JoyResponseScreenConfig WithInitialSelection(Option<BalloonArtistFacadeResource> initialSelectedItem)
	{
		return new JoyResponseScreenConfig(this.target, JoyResponseDesignerScreen.GalleryItem.Of(initialSelectedItem));
	}

	// Token: 0x060061A1 RID: 24993 RVA: 0x002475D2 File Offset: 0x002457D2
	public static JoyResponseScreenConfig Minion(GameObject minionInstance)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromMinion(minionInstance), Option.None);
	}

	// Token: 0x060061A2 RID: 24994 RVA: 0x002475E9 File Offset: 0x002457E9
	public static JoyResponseScreenConfig Personality(Personality personality)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromPersonality(personality), Option.None);
	}

	// Token: 0x060061A3 RID: 24995 RVA: 0x00247600 File Offset: 0x00245800
	public static JoyResponseScreenConfig From(MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return JoyResponseScreenConfig.Personality(personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return JoyResponseScreenConfig.Minion(minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x060061A4 RID: 24996 RVA: 0x0024763E File Offset: 0x0024583E
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.joyResponseDesignerScreen.GetComponent<JoyResponseDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.joyResponseDesignerScreen, null);
	}

	// Token: 0x04004231 RID: 16945
	public readonly JoyResponseOutfitTarget target;

	// Token: 0x04004232 RID: 16946
	public readonly Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem;

	// Token: 0x04004233 RID: 16947
	public readonly bool isValid;
}
