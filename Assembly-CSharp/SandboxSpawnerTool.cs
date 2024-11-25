using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200091F RID: 2335
public class SandboxSpawnerTool : InterfaceTool
{
	// Token: 0x060043D6 RID: 17366 RVA: 0x00180D35 File Offset: 0x0017EF35
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		colors.Add(new ToolMenu.CellColorData(this.currentCell, this.radiusIndicatorColor));
	}

	// Token: 0x060043D7 RID: 17367 RVA: 0x00180D57 File Offset: 0x0017EF57
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.currentCell = Grid.PosToCell(cursorPos);
	}

	// Token: 0x060043D8 RID: 17368 RVA: 0x00180D6C File Offset: 0x0017EF6C
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		this.Place(Grid.PosToCell(cursor_pos));
	}

	// Token: 0x060043D9 RID: 17369 RVA: 0x00180D7C File Offset: 0x0017EF7C
	private void Place(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		string stringSetting = SandboxToolParameterMenu.instance.settings.GetStringSetting("SandboxTools.SelectedEntity");
		GameObject prefab = Assets.GetPrefab(stringSetting);
		if (prefab.HasTag(GameTags.BaseMinion))
		{
			this.SpawnMinion(stringSetting);
		}
		else if (prefab.GetComponent<Building>() != null)
		{
			BuildingDef def = prefab.GetComponent<Building>().Def;
			def.Build(cell, Orientation.Neutral, null, def.DefaultElements(), 298.15f, true, -1f);
		}
		else
		{
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			Grid.SceneLayer sceneLayer = (component == null) ? Grid.SceneLayer.Creatures : component.sceneLayer;
			GameObject gameObject = GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(this.currentCell, sceneLayer), sceneLayer, null, 0);
			if (gameObject.GetComponent<Pickupable>() != null && !gameObject.HasTag(GameTags.Creature))
			{
				gameObject.transform.position += Vector3.up * (Grid.CellSizeInMeters / 3f);
			}
			gameObject.SetActive(true);
		}
		GameUtil.KInstantiate(this.fxPrefab, Grid.CellToPosCCC(this.currentCell, Grid.SceneLayer.FXFront), Grid.SceneLayer.FXFront, null, 0).GetComponent<KAnimControllerBase>().Play("placer", KAnim.PlayMode.Once, 1f, 0f);
		KFMOD.PlayUISound(this.soundPath);
	}

	// Token: 0x060043DA RID: 17370 RVA: 0x00180ED2 File Offset: 0x0017F0D2
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.entitySelector.row.SetActive(true);
	}

	// Token: 0x060043DB RID: 17371 RVA: 0x00180F09 File Offset: 0x0017F109
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x060043DC RID: 17372 RVA: 0x00180F24 File Offset: 0x0017F124
	private void SpawnMinion(string prefabID)
	{
		GameObject prefab = Assets.GetPrefab(prefabID);
		Tag model = prefabID;
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(this.currentCell, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		new MinionStartingStats(model, false, null, null, false).Apply(gameObject);
		gameObject.GetMyWorld().SetDupeVisited();
	}

	// Token: 0x060043DD RID: 17373 RVA: 0x00180FA0 File Offset: 0x0017F1A0
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			List<ObjectLayer> list = new List<ObjectLayer>();
			list.Add(ObjectLayer.Pickupables);
			list.Add(ObjectLayer.Plants);
			list.Add(ObjectLayer.Minion);
			list.Add(ObjectLayer.Building);
			if (Grid.IsValidCell(cell))
			{
				foreach (ObjectLayer layer in list)
				{
					GameObject gameObject = Grid.Objects[cell, (int)layer];
					if (gameObject)
					{
						SandboxToolParameterMenu.instance.settings.SetStringSetting("SandboxTools.SelectedEntity", gameObject.PrefabID().ToString());
						break;
					}
				}
			}
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x04002C77 RID: 11383
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x04002C78 RID: 11384
	private int currentCell;

	// Token: 0x04002C79 RID: 11385
	private string soundPath = GlobalAssets.GetSound("SandboxTool_Spawner", false);

	// Token: 0x04002C7A RID: 11386
	[SerializeField]
	private GameObject fxPrefab;
}
