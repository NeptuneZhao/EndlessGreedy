using System;
using STRINGS;

// Token: 0x02000283 RID: 643
public class ExcavateButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000D51 RID: 3409 RVA: 0x0004C2FD File Offset: 0x0004A4FD
	public string SidescreenButtonText
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x06000D52 RID: 3410 RVA: 0x0004C329 File Offset: 0x0004A529
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.isMarkedForDig == null || !this.isMarkedForDig())
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.DIG_SITE_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x0004C355 File Offset: 0x0004A555
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x0004C358 File Offset: 0x0004A558
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0004C35F File Offset: 0x0004A55F
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x0004C362 File Offset: 0x0004A562
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x0004C365 File Offset: 0x0004A565
	public void OnSidescreenButtonPressed()
	{
		System.Action onButtonPressed = this.OnButtonPressed;
		if (onButtonPressed == null)
		{
			return;
		}
		onButtonPressed();
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x0004C377 File Offset: 0x0004A577
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000847 RID: 2119
	public Func<bool> isMarkedForDig;

	// Token: 0x04000848 RID: 2120
	public System.Action OnButtonPressed;
}
