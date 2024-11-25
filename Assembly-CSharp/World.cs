using System;
using System.Collections.Generic;
using Klei;
using Rendering;
using UnityEngine;

// Token: 0x02000B61 RID: 2913
[AddComponentMenu("KMonoBehaviour/scripts/World")]
public class World : KMonoBehaviour
{
	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06005718 RID: 22296 RVA: 0x001F1834 File Offset: 0x001EFA34
	// (set) Token: 0x06005719 RID: 22297 RVA: 0x001F183B File Offset: 0x001EFA3B
	public static World Instance { get; private set; }

	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x0600571A RID: 22298 RVA: 0x001F1843 File Offset: 0x001EFA43
	// (set) Token: 0x0600571B RID: 22299 RVA: 0x001F184B File Offset: 0x001EFA4B
	public SubworldZoneRenderData zoneRenderData { get; private set; }

	// Token: 0x0600571C RID: 22300 RVA: 0x001F1854 File Offset: 0x001EFA54
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(World.Instance == null);
		World.Instance = this;
		this.blockTileRenderer = base.GetComponent<BlockTileRenderer>();
	}

	// Token: 0x0600571D RID: 22301 RVA: 0x001F1878 File Offset: 0x001EFA78
	protected override void OnSpawn()
	{
		base.GetComponent<SimDebugView>().OnReset();
		base.GetComponent<PropertyTextures>().OnReset(null);
		this.zoneRenderData = base.GetComponent<SubworldZoneRenderData>();
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(this.OnReveal));
	}

	// Token: 0x0600571E RID: 22302 RVA: 0x001F18C8 File Offset: 0x001EFAC8
	protected override void OnLoadLevel()
	{
		World.Instance = null;
		if (this.blockTileRenderer != null)
		{
			this.blockTileRenderer.FreeResources();
		}
		this.blockTileRenderer = null;
		if (this.groundRenderer != null)
		{
			this.groundRenderer.FreeResources();
		}
		this.groundRenderer = null;
		this.revealedCells.Clear();
		this.revealedCells = null;
		base.OnLoadLevel();
	}

	// Token: 0x0600571F RID: 22303 RVA: 0x001F1934 File Offset: 0x001EFB34
	public unsafe void UpdateCellInfo(List<SolidInfo> solidInfo, List<CallbackInfo> callbackInfo, int num_solid_substance_change_info, Sim.SolidSubstanceChangeInfo* solid_substance_change_info, int num_liquid_change_info, Sim.LiquidChangeInfo* liquid_change_info)
	{
		int count = solidInfo.Count;
		this.changedCells.Clear();
		for (int i = 0; i < count; i++)
		{
			int cellIdx = solidInfo[i].cellIdx;
			if (!this.changedCells.Contains(cellIdx))
			{
				this.changedCells.Add(cellIdx);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(cellIdx);
			WorldDamage.Instance.OnSolidStateChanged(cellIdx);
			if (this.OnSolidChanged != null)
			{
				this.OnSolidChanged(cellIdx);
			}
		}
		if (this.changedCells.Count != 0)
		{
			SaveGame.Instance.entombedItemManager.OnSolidChanged(this.changedCells);
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
		int count2 = callbackInfo.Count;
		for (int j = 0; j < count2; j++)
		{
			callbackInfo[j].Release();
		}
		for (int k = 0; k < num_solid_substance_change_info; k++)
		{
			int cellIdx2 = solid_substance_change_info[k].cellIdx;
			if (!Grid.IsValidCell(cellIdx2))
			{
				global::Debug.LogError(cellIdx2);
			}
			else
			{
				Grid.RenderedByWorld[cellIdx2] = (Grid.Element[cellIdx2].substance.renderedByWorld && Grid.Objects[cellIdx2, 9] == null);
				this.groundRenderer.MarkDirty(cellIdx2);
			}
		}
		GameScenePartitioner instance = GameScenePartitioner.Instance;
		this.changedCells.Clear();
		for (int l = 0; l < num_liquid_change_info; l++)
		{
			int cellIdx3 = liquid_change_info[l].cellIdx;
			this.changedCells.Add(cellIdx3);
			if (this.OnLiquidChanged != null)
			{
				this.OnLiquidChanged(cellIdx3);
			}
		}
		instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.liquidChangedLayer, null);
	}

	// Token: 0x06005720 RID: 22304 RVA: 0x001F1B09 File Offset: 0x001EFD09
	private void OnReveal(int cell)
	{
		this.revealedCells.Add(cell);
	}

	// Token: 0x06005721 RID: 22305 RVA: 0x001F1B18 File Offset: 0x001EFD18
	private void LateUpdate()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		if (GameUtil.IsCapturingTimeLapse())
		{
			this.groundRenderer.RenderAll();
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
			KAnimBatchManager.Instance().Render();
		}
		else
		{
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			this.groundRenderer.Render(visibleArea.Min, visibleArea.Max, false);
			Vector2I vis_chunk_min;
			Vector2I vis_chunk_max;
			Singleton<KBatchedAnimUpdater>.Instance.GetVisibleArea(out vis_chunk_min, out vis_chunk_max);
			KAnimBatchManager.Instance().UpdateActiveArea(vis_chunk_min, vis_chunk_max);
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
			KAnimBatchManager.Instance().Render();
		}
		if (Camera.main != null)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -Camera.main.transform.GetPosition().z));
			Shader.SetGlobalVector("_CursorPos", new Vector4(vector.x, vector.y, vector.z, 0f));
		}
		FallingWater.instance.UpdateParticles(Time.deltaTime);
		FallingWater.instance.Render();
		if (this.revealedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.revealedCells, GameScenePartitioner.Instance.fogOfWarChangedLayer, null);
			this.revealedCells.Clear();
		}
	}

	// Token: 0x04003903 RID: 14595
	public Action<int> OnSolidChanged;

	// Token: 0x04003904 RID: 14596
	public Action<int> OnLiquidChanged;

	// Token: 0x04003906 RID: 14598
	public BlockTileRenderer blockTileRenderer;

	// Token: 0x04003907 RID: 14599
	[MyCmpGet]
	[NonSerialized]
	public GroundRenderer groundRenderer;

	// Token: 0x04003908 RID: 14600
	private List<int> revealedCells = new List<int>();

	// Token: 0x04003909 RID: 14601
	public static int DebugCellID = -1;

	// Token: 0x0400390A RID: 14602
	private List<int> changedCells = new List<int>();
}
