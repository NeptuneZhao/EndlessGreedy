using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000922 RID: 2338
public class SandboxStressTool : BrushTool
{
	// Token: 0x060043F7 RID: 17399 RVA: 0x00181E84 File Offset: 0x00180084
	public static void DestroyInstance()
	{
		SandboxStressTool.instance = null;
	}

	// Token: 0x170004EA RID: 1258
	// (get) Token: 0x060043F8 RID: 17400 RVA: 0x00181E8C File Offset: 0x0018008C
	public override string[] DlcIDs
	{
		get
		{
			return DlcManager.AVAILABLE_ALL_VERSIONS;
		}
	}

	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x060043F9 RID: 17401 RVA: 0x00181E93 File Offset: 0x00180093
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x060043FA RID: 17402 RVA: 0x00181E9F File Offset: 0x0018009F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxStressTool.instance = this;
	}

	// Token: 0x060043FB RID: 17403 RVA: 0x00181EAD File Offset: 0x001800AD
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x060043FC RID: 17404 RVA: 0x00181EB4 File Offset: 0x001800B4
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060043FD RID: 17405 RVA: 0x00181EC4 File Offset: 0x001800C4
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.SetValue(5f, true);
		SandboxToolParameterMenu.instance.moraleSlider.SetValue(0f, true);
		if (DebugHandler.InstantBuildMode)
		{
			SandboxToolParameterMenu.instance.moraleSlider.row.SetActive(true);
		}
	}

	// Token: 0x060043FE RID: 17406 RVA: 0x00181F61 File Offset: 0x00180161
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.StopSound();
	}

	// Token: 0x060043FF RID: 17407 RVA: 0x00181F80 File Offset: 0x00180180
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06004400 RID: 17408 RVA: 0x00182038 File Offset: 0x00180238
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06004401 RID: 17409 RVA: 0x00182041 File Offset: 0x00180241
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06004402 RID: 17410 RVA: 0x0018205C File Offset: 0x0018025C
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			GameObject gameObject = Components.LiveMinionIdentities[i].gameObject;
			if (Grid.PosToCell(gameObject) == cell)
			{
				float num = -1f * SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.StressAdditive");
				Db.Get().Amounts.Stress.Lookup(Components.LiveMinionIdentities[i].gameObject).ApplyDelta(num);
				if (num >= 0f)
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_angry"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				else
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_happy"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				this.PlaySound(num, gameObject.transform.GetPosition());
				int intSetting = SandboxToolParameterMenu.instance.settings.GetIntSetting("SandbosTools.MoraleAdjustment");
				AttributeInstance attributeInstance = gameObject.GetAttributes().Get(Db.Get().Attributes.QualityOfLife);
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (this.moraleAdjustments.ContainsKey(component))
				{
					attributeInstance.Remove(this.moraleAdjustments[component]);
					this.moraleAdjustments.Remove(component);
				}
				if (intSetting != 0)
				{
					AttributeModifier attributeModifier = new AttributeModifier(attributeInstance.Id, (float)intSetting, () => DUPLICANTS.MODIFIERS.SANDBOXMORALEADJUSTMENT.NAME, false, false);
					attributeModifier.SetValue((float)intSetting);
					attributeInstance.Add(attributeModifier);
					this.moraleAdjustments.Add(component, attributeModifier);
				}
			}
		}
	}

	// Token: 0x06004403 RID: 17411 RVA: 0x00182224 File Offset: 0x00180424
	private void PlaySound(float sliderValue, Vector3 position)
	{
		this.ev = KFMOD.CreateInstance(this.UISoundPath);
		ATTRIBUTES_3D attributes = position.To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByNameWithLabel("SanboxTool_Effect", (sliderValue >= 0f) ? "Decrease" : "Increase", false);
		this.ev.start();
	}

	// Token: 0x06004404 RID: 17412 RVA: 0x00182288 File Offset: 0x00180488
	private void StopSound()
	{
		this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x04002C84 RID: 11396
	public static SandboxStressTool instance;

	// Token: 0x04002C85 RID: 11397
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04002C86 RID: 11398
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x04002C87 RID: 11399
	private string UISoundPath = GlobalAssets.GetSound("SandboxTool_Happy", false);

	// Token: 0x04002C88 RID: 11400
	private EventInstance ev;

	// Token: 0x04002C89 RID: 11401
	private Dictionary<MinionIdentity, AttributeModifier> moraleAdjustments = new Dictionary<MinionIdentity, AttributeModifier>();
}
