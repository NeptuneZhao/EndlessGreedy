using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000907 RID: 2311
public class DebugTool : DragTool
{
	// Token: 0x060042A1 RID: 17057 RVA: 0x0017AE5A File Offset: 0x0017905A
	public static void DestroyInstance()
	{
		DebugTool.Instance = null;
	}

	// Token: 0x060042A2 RID: 17058 RVA: 0x0017AE62 File Offset: 0x00179062
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugTool.Instance = this;
	}

	// Token: 0x060042A3 RID: 17059 RVA: 0x0017AE70 File Offset: 0x00179070
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x060042A4 RID: 17060 RVA: 0x0017AE7D File Offset: 0x0017907D
	public void Activate(DebugTool.Type type)
	{
		this.type = type;
		this.Activate();
	}

	// Token: 0x060042A5 RID: 17061 RVA: 0x0017AE8C File Offset: 0x0017908C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		PlayerController.Instance.ToolDeactivated(this);
	}

	// Token: 0x060042A6 RID: 17062 RVA: 0x0017AEA0 File Offset: 0x001790A0
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			switch (this.type)
			{
			case DebugTool.Type.ReplaceSubstance:
				this.DoReplaceSubstance(cell);
				return;
			case DebugTool.Type.FillReplaceSubstance:
			{
				GameUtil.FloodFillNext.Value.Clear();
				GameUtil.FloodFillVisited.Value.Clear();
				SimHashes elem_hash = Grid.Element[cell].id;
				GameUtil.FloodFillConditional(cell, delegate(int check_cell)
				{
					bool result = false;
					if (Grid.Element[check_cell].id == elem_hash)
					{
						result = true;
						this.DoReplaceSubstance(check_cell);
					}
					return result;
				}, GameUtil.FloodFillVisited.Value, null);
				return;
			}
			case DebugTool.Type.Clear:
				this.ClearCell(cell);
				return;
			case DebugTool.Type.AddSelection:
				DebugBaseTemplateButton.Instance.AddToSelection(cell);
				return;
			case DebugTool.Type.RemoveSelection:
				DebugBaseTemplateButton.Instance.RemoveFromSelection(cell);
				return;
			case DebugTool.Type.Deconstruct:
				this.DeconstructCell(cell);
				return;
			case DebugTool.Type.Destroy:
				this.DestroyCell(cell);
				return;
			case DebugTool.Type.Sample:
				DebugPaintElementScreen.Instance.SampleCell(cell);
				return;
			case DebugTool.Type.StoreSubstance:
				this.DoStoreSubstance(cell);
				return;
			case DebugTool.Type.Dig:
				SimMessages.Dig(cell, -1, false);
				return;
			case DebugTool.Type.Heat:
				SimMessages.ModifyEnergy(cell, 10000f, 10000f, SimMessages.EnergySourceID.DebugHeat);
				return;
			case DebugTool.Type.Cool:
				SimMessages.ModifyEnergy(cell, -10000f, 10000f, SimMessages.EnergySourceID.DebugCool);
				return;
			case DebugTool.Type.AddPressure:
				SimMessages.ModifyMass(cell, 10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 293f, SimHashes.Oxygen);
				return;
			case DebugTool.Type.RemovePressure:
				SimMessages.ModifyMass(cell, -10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 0f, SimHashes.Oxygen);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060042A7 RID: 17063 RVA: 0x0017B028 File Offset: 0x00179228
	public void DoReplaceSubstance(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		Element element = DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element) : ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element == null)
		{
			element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
		}
		byte b = DebugPaintElementScreen.Instance.paintDisease.isOn ? DebugPaintElementScreen.Instance.diseaseIdx : Grid.DiseaseIdx[cell];
		float num = DebugPaintElementScreen.Instance.paintTemperature.isOn ? DebugPaintElementScreen.Instance.temperature : Grid.Temperature[cell];
		float num2 = DebugPaintElementScreen.Instance.paintMass.isOn ? DebugPaintElementScreen.Instance.mass : Grid.Mass[cell];
		int num3 = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? DebugPaintElementScreen.Instance.diseaseCount : Grid.DiseaseCount[cell];
		if (num == -1f)
		{
			num = element.defaultValues.temperature;
		}
		if (num2 == -1f)
		{
			num2 = element.defaultValues.mass;
		}
		if (DebugPaintElementScreen.Instance.affectCells.isOn)
		{
			SimMessages.ReplaceElement(cell, element.id, CellEventLogger.Instance.DebugTool, num2, num, b, num3, -1);
			if (DebugPaintElementScreen.Instance.set_prevent_fow_reveal)
			{
				Grid.Visible[cell] = 0;
				Grid.PreventFogOfWarReveal[cell] = true;
			}
			else if (DebugPaintElementScreen.Instance.set_allow_fow_reveal && Grid.PreventFogOfWarReveal[cell])
			{
				Grid.PreventFogOfWarReveal[cell] = false;
			}
		}
		if (DebugPaintElementScreen.Instance.affectBuildings.isOn)
		{
			foreach (GameObject gameObject in new List<GameObject>
			{
				Grid.Objects[cell, 1],
				Grid.Objects[cell, 2],
				Grid.Objects[cell, 9],
				Grid.Objects[cell, 16],
				Grid.Objects[cell, 12],
				Grid.Objects[cell, 16],
				Grid.Objects[cell, 26]
			})
			{
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (num > 0f)
					{
						component.Temperature = num;
					}
					if (num3 > 0 && b != 255)
					{
						component.ModifyDiseaseCount(int.MinValue, "DebugTool.DoReplaceSubstance");
						component.AddDisease(b, num3, "DebugTool.DoReplaceSubstance");
					}
				}
			}
		}
	}

	// Token: 0x060042A8 RID: 17064 RVA: 0x0017B2EC File Offset: 0x001794EC
	public void DeconstructCell(int cell)
	{
		bool instantBuildMode = DebugHandler.InstantBuildMode;
		DebugHandler.InstantBuildMode = true;
		DeconstructTool.Instance.DeconstructCell(cell);
		if (!instantBuildMode)
		{
			DebugHandler.InstantBuildMode = false;
		}
	}

	// Token: 0x060042A9 RID: 17065 RVA: 0x0017B30C File Offset: 0x0017950C
	public void DestroyCell(int cell)
	{
		foreach (GameObject gameObject in new List<GameObject>
		{
			Grid.Objects[cell, 2],
			Grid.Objects[cell, 1],
			Grid.Objects[cell, 12],
			Grid.Objects[cell, 16],
			Grid.Objects[cell, 20],
			Grid.Objects[cell, 0],
			Grid.Objects[cell, 26],
			Grid.Objects[cell, 31],
			Grid.Objects[cell, 30]
		})
		{
			if (gameObject != null)
			{
				Util.KDestroyGameObject(gameObject);
			}
		}
		this.ClearCell(cell);
		if (ElementLoader.elements[(int)Grid.ElementIdx[cell]].id == SimHashes.Void)
		{
			SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
			return;
		}
		SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
	}

	// Token: 0x060042AA RID: 17066 RVA: 0x0017B484 File Offset: 0x00179684
	public void ClearCell(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		ListPool<ScenePartitionerEntry, DebugTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, DebugTool>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (pickupable != null && pickupable.GetComponent<MinionBrain>() == null)
			{
				Util.KDestroyGameObject(pickupable.gameObject);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x060042AB RID: 17067 RVA: 0x0017B50C File Offset: 0x0017970C
	public void DoStoreSubstance(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject == null)
		{
			return;
		}
		Storage component = gameObject.GetComponent<Storage>();
		if (component == null)
		{
			return;
		}
		Element element = DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element) : ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element == null)
		{
			element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
		}
		byte disease_idx = DebugPaintElementScreen.Instance.paintDisease.isOn ? DebugPaintElementScreen.Instance.diseaseIdx : Grid.DiseaseIdx[cell];
		float num = DebugPaintElementScreen.Instance.paintTemperature.isOn ? DebugPaintElementScreen.Instance.temperature : element.defaultValues.temperature;
		float num2 = DebugPaintElementScreen.Instance.paintMass.isOn ? DebugPaintElementScreen.Instance.mass : element.defaultValues.mass;
		if (num == -1f)
		{
			num = element.defaultValues.temperature;
		}
		if (num2 == -1f)
		{
			num2 = element.defaultValues.mass;
		}
		int disease_count = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? DebugPaintElementScreen.Instance.diseaseCount : 0;
		if (element.IsGas)
		{
			component.AddGasChunk(element.id, num2, num, disease_idx, disease_count, false, true);
			return;
		}
		if (element.IsLiquid)
		{
			component.AddLiquid(element.id, num2, num, disease_idx, disease_count, false, true);
			return;
		}
		if (element.IsSolid)
		{
			component.AddOre(element.id, num2, num, disease_idx, disease_count, false, true);
		}
	}

	// Token: 0x04002C05 RID: 11269
	public static DebugTool Instance;

	// Token: 0x04002C06 RID: 11270
	public DebugTool.Type type;

	// Token: 0x0200186C RID: 6252
	public enum Type
	{
		// Token: 0x0400762C RID: 30252
		ReplaceSubstance,
		// Token: 0x0400762D RID: 30253
		FillReplaceSubstance,
		// Token: 0x0400762E RID: 30254
		Clear,
		// Token: 0x0400762F RID: 30255
		AddSelection,
		// Token: 0x04007630 RID: 30256
		RemoveSelection,
		// Token: 0x04007631 RID: 30257
		Deconstruct,
		// Token: 0x04007632 RID: 30258
		Destroy,
		// Token: 0x04007633 RID: 30259
		Sample,
		// Token: 0x04007634 RID: 30260
		StoreSubstance,
		// Token: 0x04007635 RID: 30261
		Dig,
		// Token: 0x04007636 RID: 30262
		Heat,
		// Token: 0x04007637 RID: 30263
		Cool,
		// Token: 0x04007638 RID: 30264
		AddPressure,
		// Token: 0x04007639 RID: 30265
		RemovePressure,
		// Token: 0x0400763A RID: 30266
		PaintPlant
	}
}
