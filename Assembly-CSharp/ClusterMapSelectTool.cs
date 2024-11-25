using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000905 RID: 2309
public class ClusterMapSelectTool : InterfaceTool
{
	// Token: 0x0600428D RID: 17037 RVA: 0x0017AB66 File Offset: 0x00178D66
	public static void DestroyInstance()
	{
		ClusterMapSelectTool.Instance = null;
	}

	// Token: 0x0600428E RID: 17038 RVA: 0x0017AB6E File Offset: 0x00178D6E
	protected override void OnPrefabInit()
	{
		ClusterMapSelectTool.Instance = this;
	}

	// Token: 0x0600428F RID: 17039 RVA: 0x0017AB76 File Offset: 0x00178D76
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x06004290 RID: 17040 RVA: 0x0017AB9A File Offset: 0x00178D9A
	public KSelectable GetSelected()
	{
		return this.m_selected;
	}

	// Token: 0x06004291 RID: 17041 RVA: 0x0017ABA2 File Offset: 0x00178DA2
	public override bool ShowHoverUI()
	{
		return ClusterMapScreen.Instance.HasCurrentHover();
	}

	// Token: 0x06004292 RID: 17042 RVA: 0x0017ABAE File Offset: 0x00178DAE
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x06004293 RID: 17043 RVA: 0x0017ABC8 File Offset: 0x00178DC8
	private void UpdateHoveredSelectables()
	{
		this.m_hoveredSelectables.Clear();
		if (ClusterMapScreen.Instance.HasCurrentHover())
		{
			AxialI currentHoverLocation = ClusterMapScreen.Instance.GetCurrentHoverLocation();
			List<KSelectable> collection = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(currentHoverLocation)
			select entity.GetComponent<KSelectable>() into selectable
			where selectable != null && selectable.IsSelectable
			select selectable).ToList<KSelectable>();
			this.m_hoveredSelectables.AddRange(collection);
		}
	}

	// Token: 0x06004294 RID: 17044 RVA: 0x0017AC5C File Offset: 0x00178E5C
	public override void LateUpdate()
	{
		this.UpdateHoveredSelectables();
		KSelectable kselectable = (this.m_hoveredSelectables.Count > 0) ? this.m_hoveredSelectables[0] : null;
		base.UpdateHoverElements(this.m_hoveredSelectables);
		if (!this.hasFocus)
		{
			base.ClearHover();
		}
		else if (kselectable != this.hover)
		{
			base.ClearHover();
			this.hover = kselectable;
			if (kselectable != null)
			{
				Game.Instance.Trigger(2095258329, kselectable.gameObject);
				kselectable.Hover(!this.playedSoundThisFrame);
				this.playedSoundThisFrame = true;
			}
		}
		this.playedSoundThisFrame = false;
	}

	// Token: 0x06004295 RID: 17045 RVA: 0x0017ACFF File Offset: 0x00178EFF
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x06004296 RID: 17046 RVA: 0x0017AD2D File Offset: 0x00178F2D
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06004297 RID: 17047 RVA: 0x0017AD48 File Offset: 0x00178F48
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.m_selected)
		{
			return;
		}
		if (this.m_selected != null)
		{
			this.m_selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == -1)
		{
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
		}
		this.m_selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x04002BFD RID: 11261
	private List<KSelectable> m_hoveredSelectables = new List<KSelectable>();

	// Token: 0x04002BFE RID: 11262
	private KSelectable m_selected;

	// Token: 0x04002BFF RID: 11263
	public static ClusterMapSelectTool Instance;

	// Token: 0x04002C00 RID: 11264
	private KSelectable delayedNextSelection;

	// Token: 0x04002C01 RID: 11265
	private bool delayedSkipSound;
}
