using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000768 RID: 1896
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduit")]
public class SolidConduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr
{
	// Token: 0x06003306 RID: 13062 RVA: 0x001185B4 File Offset: 0x001167B4
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06003307 RID: 13063 RVA: 0x001185CA File Offset: 0x001167CA
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003308 RID: 13064 RVA: 0x001185D9 File Offset: 0x001167D9
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.solidConduitSystem;
	}

	// Token: 0x06003309 RID: 13065 RVA: 0x001185E5 File Offset: 0x001167E5
	public UtilityNetwork GetNetwork()
	{
		return this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
	}

	// Token: 0x0600330A RID: 13066 RVA: 0x001185F8 File Offset: 0x001167F8
	public static SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x0600330B RID: 13067 RVA: 0x00118604 File Offset: 0x00116804
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x0600330C RID: 13068 RVA: 0x00118611 File Offset: 0x00116811
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Conveyor, this);
	}

	// Token: 0x0600330D RID: 13069 RVA: 0x00118644 File Offset: 0x00116844
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(this);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			SolidConduit.GetFlowManager().EmptyConduit(cell);
		}
		base.OnCleanUp();
	}

	// Token: 0x04001E22 RID: 7714
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04001E23 RID: 7715
	private System.Action firstFrameCallback;
}
