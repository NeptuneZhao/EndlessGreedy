using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

// Token: 0x02000917 RID: 2327
public class SandboxBrushTool : BrushTool
{
	// Token: 0x06004373 RID: 17267 RVA: 0x0017EFA9 File Offset: 0x0017D1A9
	public static void DestroyInstance()
	{
		SandboxBrushTool.instance = null;
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06004374 RID: 17268 RVA: 0x0017EFB1 File Offset: 0x0017D1B1
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06004375 RID: 17269 RVA: 0x0017EFBD File Offset: 0x0017D1BD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxBrushTool.instance = this;
	}

	// Token: 0x06004376 RID: 17270 RVA: 0x0017EFCB File Offset: 0x0017D1CB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06004377 RID: 17271 RVA: 0x0017EFD8 File Offset: 0x0017D1D8
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
		SandboxToolParameterMenu.SelectorValue elementSelector = SandboxToolParameterMenu.instance.elementSelector;
		elementSelector.onValueChanged = (Action<object>)Delegate.Combine(elementSelector.onValueChanged, new Action<object>(this.OnElementChanged));
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x0017F0AE File Offset: 0x0017D2AE
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.audioEvent.release();
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x0017F0D4 File Offset: 0x0017D2D4
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int num in this.recentlyAffectedCells)
		{
			Color color = new Color(this.recentAffectedCellColor[num].r, this.recentAffectedCellColor[num].g, this.recentAffectedCellColor[num].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 10f), -1f, 1f, 0.1f, 0.2f));
			colors.Add(new ToolMenu.CellColorData(num, color));
		}
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x0017F1EC File Offset: 0x0017D3EC
	public override void SetBrushSize(int radius)
	{
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x0017F284 File Offset: 0x0017D484
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		if (!this.recentAffectedCellColor.ContainsKey(cell))
		{
			this.recentAffectedCellColor.Add(cell, element.substance.uiColour);
		}
		else
		{
			this.recentAffectedCellColor[cell] = element.substance.uiColour;
		}
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
			this.recentAffectedCellColor.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		byte index2 = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index2 = Db.Get().Diseases.GetIndex(disease.id);
		}
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index2, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
	}

	// Token: 0x0600437C RID: 17276 RVA: 0x0017F420 File Offset: 0x0017D620
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(cell))
			{
				SandboxSampleTool.Sample(cell);
			}
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x0017F467 File Offset: 0x0017D667
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x0600437E RID: 17278 RVA: 0x0017F480 File Offset: 0x0017D680
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.StopSound();
	}

	// Token: 0x0600437F RID: 17279 RVA: 0x0017F48F File Offset: 0x0017D68F
	private void OnElementChanged(object new_element)
	{
		this.clearVisitedCells();
	}

	// Token: 0x06004380 RID: 17280 RVA: 0x0017F498 File Offset: 0x0017D698
	protected override string GetDragSound()
	{
		string str = (ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")].state & Element.State.Solid).ToString();
		return "SandboxTool_Brush_" + str + "_Add";
	}

	// Token: 0x06004381 RID: 17281 RVA: 0x0017F4E8 File Offset: 0x0017D6E8
	protected override void PlaySound()
	{
		base.PlaySound();
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		switch (element.state & Element.State.Solid)
		{
		case Element.State.Vacuum:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas", false);
			break;
		case Element.State.Gas:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas", false);
			break;
		case Element.State.Liquid:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Liquid", false);
			break;
		case Element.State.Solid:
			sound = GlobalAssets.GetSound("Brush_" + element.substance.GetOreBumpSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Brush_Rock", false);
			}
			break;
		default:
			sound = GlobalAssets.GetSound("Brush_Rock", false);
			break;
		}
		this.audioEvent = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.audioEvent.set3DAttributes(attributes);
		this.audioEvent.start();
	}

	// Token: 0x06004382 RID: 17282 RVA: 0x0017F5D4 File Offset: 0x0017D7D4
	private void StopSound()
	{
		this.audioEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.audioEvent.release();
	}

	// Token: 0x04002C5D RID: 11357
	public static SandboxBrushTool instance;

	// Token: 0x04002C5E RID: 11358
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04002C5F RID: 11359
	private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();

	// Token: 0x04002C60 RID: 11360
	private EventInstance audioEvent;
}
