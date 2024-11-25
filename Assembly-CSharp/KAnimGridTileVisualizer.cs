using System;
using Rendering;
using UnityEngine;

// Token: 0x02000574 RID: 1396
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGridTileVisualizer")]
public class KAnimGridTileVisualizer : KMonoBehaviour, IBlockTileInfo
{
	// Token: 0x06002056 RID: 8278 RVA: 0x000B563B File Offset: 0x000B383B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<KAnimGridTileVisualizer>(-1503271301, KAnimGridTileVisualizer.OnSelectionChangedDelegate);
		base.Subscribe<KAnimGridTileVisualizer>(-1201923725, KAnimGridTileVisualizer.OnHighlightChangedDelegate);
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x000B5668 File Offset: 0x000B3868
	protected override void OnCleanUp()
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ObjectLayer tileLayer = component.Def.TileLayer;
			if (Grid.Objects[cell, (int)tileLayer] == base.gameObject)
			{
				Grid.Objects[cell, (int)tileLayer] = null;
			}
			TileVisualizer.RefreshCell(cell, tileLayer, component.Def.ReplacementLayer);
		}
		base.OnCleanUp();
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x000B56E0 File Offset: 0x000B38E0
	private void OnSelectionChanged(object data)
	{
		bool enabled = (bool)data;
		World.Instance.blockTileRenderer.SelectCell(Grid.PosToCell(base.transform.GetPosition()), enabled);
	}

	// Token: 0x06002059 RID: 8281 RVA: 0x000B5714 File Offset: 0x000B3914
	private void OnHighlightChanged(object data)
	{
		bool enabled = (bool)data;
		World.Instance.blockTileRenderer.HighlightCell(Grid.PosToCell(base.transform.GetPosition()), enabled);
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x000B5748 File Offset: 0x000B3948
	public int GetBlockTileConnectorID()
	{
		return this.blockTileConnectorID;
	}

	// Token: 0x04001241 RID: 4673
	[SerializeField]
	public int blockTileConnectorID;

	// Token: 0x04001242 RID: 4674
	private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnSelectionChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>(delegate(KAnimGridTileVisualizer component, object data)
	{
		component.OnSelectionChanged(data);
	});

	// Token: 0x04001243 RID: 4675
	private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnHighlightChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>(delegate(KAnimGridTileVisualizer component, object data)
	{
		component.OnHighlightChanged(data);
	});
}
