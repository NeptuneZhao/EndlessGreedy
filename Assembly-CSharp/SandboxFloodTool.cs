using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

// Token: 0x0200091C RID: 2332
public class SandboxFloodTool : FloodTool
{
	// Token: 0x060043B4 RID: 17332 RVA: 0x00180147 File Offset: 0x0017E347
	public static void DestroyInstance()
	{
		SandboxFloodTool.instance = null;
	}

	// Token: 0x060043B5 RID: 17333 RVA: 0x0018014F File Offset: 0x0017E34F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFloodTool.instance = this;
		this.floodCriteria = ((int cell) => Grid.IsValidCell(cell) && Grid.Element[cell] == Grid.Element[this.mouseCell] && Grid.WorldIdx[cell] == Grid.WorldIdx[this.mouseCell]);
		this.paintArea = delegate(HashSet<int> cells)
		{
			foreach (int cell in cells)
			{
				this.PaintCell(cell);
			}
		};
	}

	// Token: 0x060043B6 RID: 17334 RVA: 0x00180184 File Offset: 0x0017E384
	private void PaintCell(int cell)
	{
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index = Db.Get().Diseases.GetIndex(disease.id);
		}
		int index2 = Game.Instance.callbackManager.Add(item).index;
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index2;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x060043B7 RID: 17335 RVA: 0x001802B8 File Offset: 0x0017E4B8
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x060043B8 RID: 17336 RVA: 0x001802C4 File Offset: 0x0017E4C4
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060043B9 RID: 17337 RVA: 0x001802D4 File Offset: 0x0017E4D4
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
	}

	// Token: 0x060043BA RID: 17338 RVA: 0x0018036A File Offset: 0x0017E56A
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x060043BB RID: 17339 RVA: 0x00180390 File Offset: 0x0017E590
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsToAffect)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.areaColour));
		}
	}

	// Token: 0x060043BC RID: 17340 RVA: 0x0018044C File Offset: 0x0017E64C
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.cellsToAffect = base.Flood(Grid.PosToCell(cursorPos));
	}

	// Token: 0x060043BD RID: 17341 RVA: 0x00180468 File Offset: 0x0017E668
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		if (element.IsSolid)
		{
			sound = GlobalAssets.GetSound("Break_" + element.substance.GetMiningBreakSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Break_Rock", false);
			}
		}
		else if (element.IsGas)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Gas", false);
		}
		else if (element.IsLiquid)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Liquid", false);
		}
		else
		{
			sound = GlobalAssets.GetSound("Break_Rock", false);
		}
		this.ev = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByName("SandboxToggle", 1f, false);
		this.ev.start();
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Bucket", false));
	}

	// Token: 0x060043BE RID: 17342 RVA: 0x00180568 File Offset: 0x0017E768
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

	// Token: 0x04002C6C RID: 11372
	public static SandboxFloodTool instance;

	// Token: 0x04002C6D RID: 11373
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04002C6E RID: 11374
	protected HashSet<int> cellsToAffect = new HashSet<int>();

	// Token: 0x04002C6F RID: 11375
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x04002C70 RID: 11376
	private EventInstance ev;
}
