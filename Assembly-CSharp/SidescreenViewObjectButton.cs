using System;
using UnityEngine;

// Token: 0x020003AD RID: 941
public class SidescreenViewObjectButton : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x0600138A RID: 5002 RVA: 0x0006BE28 File Offset: 0x0006A028
	public bool IsValid()
	{
		SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
		if (trackMode != SidescreenViewObjectButton.Mode.Target)
		{
			return trackMode == SidescreenViewObjectButton.Mode.Cell && Grid.IsValidCell(this.TargetCell);
		}
		return this.Target != null;
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600138B RID: 5003 RVA: 0x0006BE5D File Offset: 0x0006A05D
	public string SidescreenButtonText
	{
		get
		{
			return this.Text;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600138C RID: 5004 RVA: 0x0006BE65 File Offset: 0x0006A065
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.Tooltip;
		}
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x0006BE6D File Offset: 0x0006A06D
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x0006BE74 File Offset: 0x0006A074
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x0006BE77 File Offset: 0x0006A077
	public bool SidescreenButtonInteractable()
	{
		return this.IsValid();
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x0006BE7F File Offset: 0x0006A07F
	public int HorizontalGroupID()
	{
		return this.horizontalGroupID;
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x0006BE88 File Offset: 0x0006A088
	public void OnSidescreenButtonPressed()
	{
		if (this.IsValid())
		{
			SidescreenViewObjectButton.Mode trackMode = this.TrackMode;
			if (trackMode == SidescreenViewObjectButton.Mode.Target)
			{
				CameraController.Instance.CameraGoTo(this.Target.transform.GetPosition(), 2f, true);
				return;
			}
			if (trackMode == SidescreenViewObjectButton.Mode.Cell)
			{
				CameraController.Instance.CameraGoTo(Grid.CellToPos(this.TargetCell), 2f, true);
				return;
			}
		}
		else
		{
			base.gameObject.Trigger(1980521255, null);
		}
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x0006BEF9 File Offset: 0x0006A0F9
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04000B3A RID: 2874
	public string Text;

	// Token: 0x04000B3B RID: 2875
	public string Tooltip;

	// Token: 0x04000B3C RID: 2876
	public SidescreenViewObjectButton.Mode TrackMode;

	// Token: 0x04000B3D RID: 2877
	public GameObject Target;

	// Token: 0x04000B3E RID: 2878
	public int TargetCell;

	// Token: 0x04000B3F RID: 2879
	public int horizontalGroupID = -1;

	// Token: 0x0200114E RID: 4430
	public enum Mode
	{
		// Token: 0x04005FB7 RID: 24503
		Target,
		// Token: 0x04005FB8 RID: 24504
		Cell
	}
}
