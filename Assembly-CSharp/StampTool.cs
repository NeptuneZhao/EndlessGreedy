using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000925 RID: 2341
public class StampTool : InterfaceTool
{
	// Token: 0x06004419 RID: 17433 RVA: 0x001827F4 File Offset: 0x001809F4
	public static void DestroyInstance()
	{
		StampTool.Instance = null;
	}

	// Token: 0x0600441A RID: 17434 RVA: 0x001827FC File Offset: 0x001809FC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		StampTool.Instance = this;
		this.preview = new StampToolPreview(this, new IStampToolPreviewPlugin[]
		{
			new StampToolPreview_Placers(this.PlacerPrefab),
			new StampToolPreview_Area(),
			new StampToolPreview_SolidLiquidGas(),
			new StampToolPreview_Prefabs()
		});
	}

	// Token: 0x0600441B RID: 17435 RVA: 0x0018284D File Offset: 0x00180A4D
	private void Update()
	{
		this.preview.Refresh(Grid.PosToCell(this.GetCursorPos()));
	}

	// Token: 0x0600441C RID: 17436 RVA: 0x00182868 File Offset: 0x00180A68
	public void Activate(TemplateContainer template, bool SelectAffected = false, bool DeactivateOnStamp = false)
	{
		this.selectAffected = SelectAffected;
		this.deactivateOnStamp = DeactivateOnStamp;
		if (this.stampTemplate == template || template == null || template.cells == null)
		{
			return;
		}
		this.stampTemplate = template;
		PlayerController.Instance.ActivateTool(this);
		base.StartCoroutine(this.preview.Setup(template));
	}

	// Token: 0x0600441D RID: 17437 RVA: 0x001828BD File Offset: 0x00180ABD
	private Vector3 GetCursorPos()
	{
		return PlayerController.GetCursorPos(KInputManager.GetMousePos());
	}

	// Token: 0x0600441E RID: 17438 RVA: 0x001828C9 File Offset: 0x00180AC9
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		this.Stamp(cursor_pos);
	}

	// Token: 0x0600441F RID: 17439 RVA: 0x001828E0 File Offset: 0x00180AE0
	private void Stamp(Vector2 pos)
	{
		if (!this.ready)
		{
			return;
		}
		int cell = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(-this.stampTemplate.info.size.X / 2f), 0);
		int cell2 = Grid.OffsetCell(Grid.PosToCell(pos), Mathf.FloorToInt(this.stampTemplate.info.size.X / 2f), 0);
		int cell3 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(-this.stampTemplate.info.size.Y / 2f));
		int cell4 = Grid.OffsetCell(Grid.PosToCell(pos), 0, 1 + Mathf.FloorToInt(this.stampTemplate.info.size.Y / 2f));
		if (!Grid.IsValidBuildingCell(cell) || !Grid.IsValidBuildingCell(cell2) || !Grid.IsValidBuildingCell(cell4) || !Grid.IsValidBuildingCell(cell3))
		{
			return;
		}
		this.ready = false;
		bool pauseOnComplete = SpeedControlScreen.Instance.IsPaused;
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(true);
		}
		if (this.stampTemplate.cells != null)
		{
			this.preview.OnPlace();
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.stampTemplate.cells.Count; i++)
			{
				for (int j = 0; j < 34; j++)
				{
					GameObject gameObject = Grid.Objects[Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[i].location_x), (int)(pos.y + (float)this.stampTemplate.cells[i].location_y)), j];
					if (gameObject != null && !list.Contains(gameObject))
					{
						list.Add(gameObject);
					}
				}
			}
			foreach (GameObject gameObject2 in list)
			{
				if (gameObject2 != null)
				{
					Util.KDestroyGameObject(gameObject2);
				}
			}
		}
		TemplateLoader.Stamp(this.stampTemplate, pos, delegate
		{
			this.CompleteStamp(pauseOnComplete);
		});
		if (this.selectAffected)
		{
			DebugBaseTemplateButton.Instance.ClearSelection();
			if (this.stampTemplate.cells != null)
			{
				for (int k = 0; k < this.stampTemplate.cells.Count; k++)
				{
					DebugBaseTemplateButton.Instance.AddToSelection(Grid.XYToCell((int)(pos.x + (float)this.stampTemplate.cells[k].location_x), (int)(pos.y + (float)this.stampTemplate.cells[k].location_y)));
				}
			}
		}
		if (this.deactivateOnStamp)
		{
			base.DeactivateTool(null);
		}
	}

	// Token: 0x06004420 RID: 17440 RVA: 0x00182BE8 File Offset: 0x00180DE8
	private void CompleteStamp(bool pause)
	{
		if (pause)
		{
			SpeedControlScreen.Instance.Pause(true, false);
		}
		this.ready = true;
		this.OnDeactivateTool(null);
	}

	// Token: 0x06004421 RID: 17441 RVA: 0x00182C07 File Offset: 0x00180E07
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		if (base.gameObject.activeSelf)
		{
			return;
		}
		this.preview.Cleanup();
		this.stampTemplate = null;
	}

	// Token: 0x04002C94 RID: 11412
	public static StampTool Instance;

	// Token: 0x04002C95 RID: 11413
	private StampToolPreview preview;

	// Token: 0x04002C96 RID: 11414
	public TemplateContainer stampTemplate;

	// Token: 0x04002C97 RID: 11415
	public GameObject PlacerPrefab;

	// Token: 0x04002C98 RID: 11416
	private bool ready = true;

	// Token: 0x04002C99 RID: 11417
	private bool selectAffected;

	// Token: 0x04002C9A RID: 11418
	private bool deactivateOnStamp;
}
