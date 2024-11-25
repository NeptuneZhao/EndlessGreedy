using System;

// Token: 0x0200092F RID: 2351
public class UtilityBuildTool : BaseUtilityBuildTool
{
	// Token: 0x0600444F RID: 17487 RVA: 0x00185193 File Offset: 0x00183393
	public static void DestroyInstance()
	{
		UtilityBuildTool.Instance = null;
	}

	// Token: 0x06004450 RID: 17488 RVA: 0x0018519B File Offset: 0x0018339B
	protected override void OnPrefabInit()
	{
		UtilityBuildTool.Instance = this;
		base.OnPrefabInit();
		this.populateHitsList = true;
		this.canChangeDragAxis = false;
	}

	// Token: 0x06004451 RID: 17489 RVA: 0x001851B8 File Offset: 0x001833B8
	protected override void ApplyPathToConduitSystem()
	{
		if (this.path.Count < 2)
		{
			return;
		}
		for (int i = 1; i < this.path.Count; i++)
		{
			if (this.path[i - 1].valid && this.path[i].valid)
			{
				int cell = this.path[i - 1].cell;
				int cell2 = this.path[i].cell;
				UtilityConnections utilityConnections = UtilityConnectionsExtensions.DirectionFromToCell(cell, cell2);
				if (utilityConnections != (UtilityConnections)0)
				{
					UtilityConnections new_connection = utilityConnections.InverseDirection();
					string text;
					if (this.conduitMgr.CanAddConnection(utilityConnections, cell, false, out text) && this.conduitMgr.CanAddConnection(new_connection, cell2, false, out text))
					{
						this.conduitMgr.AddConnection(utilityConnections, cell, false);
						this.conduitMgr.AddConnection(new_connection, cell2, false);
					}
					else if (i == this.path.Count - 1 && this.lastPathHead != i)
					{
						PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, text, null, Grid.CellToPosCCC(cell2, (Grid.SceneLayer)0), 1.5f, false, false);
					}
				}
			}
		}
		this.lastPathHead = this.path.Count - 1;
	}

	// Token: 0x04002CB4 RID: 11444
	public static UtilityBuildTool Instance;

	// Token: 0x04002CB5 RID: 11445
	private int lastPathHead = -1;
}
