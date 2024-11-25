using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BEF RID: 3055
public class ClusterMapHex : MultiToggle, ICanvasRaycastFilter
{
	// Token: 0x170006D1 RID: 1745
	// (get) Token: 0x06005D0C RID: 23820 RVA: 0x00223438 File Offset: 0x00221638
	// (set) Token: 0x06005D0D RID: 23821 RVA: 0x00223440 File Offset: 0x00221640
	public AxialI location { get; private set; }

	// Token: 0x06005D0E RID: 23822 RVA: 0x0022344C File Offset: 0x0022164C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.rectTransform = base.GetComponent<RectTransform>();
		this.onClick = new System.Action(this.TrySelect);
		this.onDoubleClick = new Func<bool>(this.TryGoTo);
		this.onEnter = new System.Action(this.OnHover);
		this.onExit = new System.Action(this.OnUnhover);
	}

	// Token: 0x06005D0F RID: 23823 RVA: 0x002234B3 File Offset: 0x002216B3
	public void SetLocation(AxialI location)
	{
		this.location = location;
	}

	// Token: 0x06005D10 RID: 23824 RVA: 0x002234BC File Offset: 0x002216BC
	public void SetRevealed(ClusterRevealLevel level)
	{
		this._revealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			this.fogOfWar.gameObject.SetActive(true);
			this.peekedTile.gameObject.SetActive(false);
			return;
		case ClusterRevealLevel.Peeked:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(true);
			return;
		case ClusterRevealLevel.Visible:
			this.fogOfWar.gameObject.SetActive(false);
			this.peekedTile.gameObject.SetActive(false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06005D11 RID: 23825 RVA: 0x0022354B File Offset: 0x0022174B
	public void SetDestinationStatus(string fail_reason)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x06005D12 RID: 23826 RVA: 0x00223580 File Offset: 0x00221780
	public void SetDestinationStatus(string fail_reason, int pathLength, int rocketRange, bool repeat)
	{
		this.m_tooltip.ClearMultiStringTooltip();
		if (pathLength > 0)
		{
			string text = repeat ? UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH_RETURN : UI.CLUSTERMAP.TOOLTIP_PATH_LENGTH;
			if (repeat)
			{
				pathLength *= 2;
			}
			text = string.Format(text, pathLength, GameUtil.GetFormattedRocketRange(rocketRange, true));
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(string.IsNullOrEmpty(fail_reason));
		if (!string.IsNullOrEmpty(fail_reason))
		{
			this.m_tooltip.AddMultiStringTooltip(fail_reason, this.invalidDestinationTooltipStyle);
		}
	}

	// Token: 0x06005D13 RID: 23827 RVA: 0x00223608 File Offset: 0x00221808
	public void UpdateToggleState(ClusterMapHex.ToggleState state)
	{
		int new_state_index = -1;
		switch (state)
		{
		case ClusterMapHex.ToggleState.Unselected:
			new_state_index = 0;
			break;
		case ClusterMapHex.ToggleState.Selected:
			new_state_index = 1;
			break;
		case ClusterMapHex.ToggleState.OrbitHighlight:
			new_state_index = 2;
			break;
		}
		base.ChangeState(new_state_index);
	}

	// Token: 0x06005D14 RID: 23828 RVA: 0x0022363C File Offset: 0x0022183C
	private void TrySelect()
	{
		if (DebugHandler.InstantBuildMode)
		{
			SaveGame.Instance.GetSMI<ClusterFogOfWarManager.Instance>().RevealLocation(this.location, 0);
		}
		ClusterMapScreen.Instance.SelectHex(this);
	}

	// Token: 0x06005D15 RID: 23829 RVA: 0x00223668 File Offset: 0x00221868
	private bool TryGoTo()
	{
		List<WorldContainer> list = (from entity in ClusterGrid.Instance.GetVisibleEntitiesAtCell(this.location)
		select entity.GetComponent<WorldContainer>() into x
		where x != null
		select x).ToList<WorldContainer>();
		if (list.Count == 1)
		{
			CameraController.Instance.ActiveWorldStarWipe(list[0].id, null);
			return true;
		}
		return false;
	}

	// Token: 0x06005D16 RID: 23830 RVA: 0x002236F8 File Offset: 0x002218F8
	private void OnHover()
	{
		this.m_tooltip.ClearMultiStringTooltip();
		string text = "";
		switch (this._revealLevel)
		{
		case ClusterRevealLevel.Hidden:
			text = UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX;
			break;
		case ClusterRevealLevel.Peeked:
		{
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.Asteroid);
			List<ClusterGridEntity> hiddenEntitiesOfLayerAtCell2 = ClusterGrid.Instance.GetHiddenEntitiesOfLayerAtCell(this.location, EntityLayer.POI);
			text = ((hiddenEntitiesOfLayerAtCell.Count > 0 || hiddenEntitiesOfLayerAtCell2.Count > 0) ? UI.CLUSTERMAP.TOOLTIP_PEEKED_HEX_WITH_OBJECT : UI.CLUSTERMAP.TOOLTIP_HIDDEN_HEX);
			break;
		}
		case ClusterRevealLevel.Visible:
			if (ClusterGrid.Instance.GetEntitiesOnCell(this.location).Count == 0)
			{
				text = UI.CLUSTERMAP.TOOLTIP_EMPTY_HEX;
			}
			break;
		}
		if (!text.IsNullOrWhiteSpace())
		{
			this.m_tooltip.AddMultiStringTooltip(text, this.informationTooltipStyle);
		}
		this.UpdateHoverColors(true);
		ClusterMapScreen.Instance.OnHoverHex(this);
	}

	// Token: 0x06005D17 RID: 23831 RVA: 0x002237D4 File Offset: 0x002219D4
	private void OnUnhover()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapScreen.Instance.OnUnhoverHex(this);
		}
	}

	// Token: 0x06005D18 RID: 23832 RVA: 0x002237F0 File Offset: 0x002219F0
	private void UpdateHoverColors(bool validDestination)
	{
		Color color_on_hover = validDestination ? this.hoverColorValid : this.hoverColorInvalid;
		for (int i = 0; i < this.states.Length; i++)
		{
			this.states[i].color_on_hover = color_on_hover;
			for (int j = 0; j < this.states[i].additional_display_settings.Length; j++)
			{
				this.states[i].additional_display_settings[j].color_on_hover = color_on_hover;
			}
		}
		base.RefreshHoverColor();
	}

	// Token: 0x06005D19 RID: 23833 RVA: 0x00223878 File Offset: 0x00221A78
	public bool IsRaycastLocationValid(Vector2 inputPoint, Camera eventCamera)
	{
		Vector2 vector = this.rectTransform.position;
		float num = Mathf.Abs(inputPoint.x - vector.x);
		float num2 = Mathf.Abs(inputPoint.y - vector.y);
		Vector2 vector2 = this.rectTransform.lossyScale;
		return num <= vector2.x && num2 <= vector2.y && vector2.y * vector2.x - vector2.y / 2f * num - vector2.x * num2 >= 0f;
	}

	// Token: 0x04003E4F RID: 15951
	private RectTransform rectTransform;

	// Token: 0x04003E50 RID: 15952
	public Color hoverColorValid;

	// Token: 0x04003E51 RID: 15953
	public Color hoverColorInvalid;

	// Token: 0x04003E52 RID: 15954
	public Image fogOfWar;

	// Token: 0x04003E53 RID: 15955
	public Image peekedTile;

	// Token: 0x04003E54 RID: 15956
	public TextStyleSetting invalidDestinationTooltipStyle;

	// Token: 0x04003E55 RID: 15957
	public TextStyleSetting informationTooltipStyle;

	// Token: 0x04003E56 RID: 15958
	[MyCmpGet]
	private ToolTip m_tooltip;

	// Token: 0x04003E57 RID: 15959
	private ClusterRevealLevel _revealLevel;

	// Token: 0x02001CD4 RID: 7380
	public enum ToggleState
	{
		// Token: 0x0400853C RID: 34108
		Unselected,
		// Token: 0x0400853D RID: 34109
		Selected,
		// Token: 0x0400853E RID: 34110
		OrbitHighlight
	}
}
