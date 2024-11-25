using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200071F RID: 1823
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LogicWire")]
public class LogicWire : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IBitRating, IDisconnectable
{
	// Token: 0x0600302F RID: 12335 RVA: 0x0010A801 File Offset: 0x00108A01
	public static int GetBitDepthAsInt(LogicWire.BitDepth rating)
	{
		if (rating == LogicWire.BitDepth.OneBit)
		{
			return 1;
		}
		if (rating != LogicWire.BitDepth.FourBit)
		{
			return 0;
		}
		return 4;
	}

	// Token: 0x06003030 RID: 12336 RVA: 0x0010A814 File Offset: 0x00108A14
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.logicCircuitSystem.AddToNetworks(cell, this, false);
		base.Subscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate);
		base.Subscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate);
		this.Connect();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(LogicWire.OutlineSymbol, false);
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x0010A884 File Offset: 0x00108A84
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.logicCircuitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06003032 RID: 12338 RVA: 0x0010A910 File Offset: 0x00108B10
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.logicCircuitSystem.GetNetworkForCell(cell) is LogicCircuitNetwork;
		}
	}

	// Token: 0x06003033 RID: 12339 RVA: 0x0010A946 File Offset: 0x00108B46
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x06003034 RID: 12340 RVA: 0x0010A950 File Offset: 0x00108B50
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x06003035 RID: 12341 RVA: 0x0010A998 File Offset: 0x00108B98
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
	}

	// Token: 0x06003036 RID: 12342 RVA: 0x0010A9E8 File Offset: 0x00108BE8
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.logicCircuitSystem.GetConnections(cell, true);
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x0010AA18 File Offset: 0x00108C18
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.logicCircuitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x0010AA3C File Offset: 0x00108C3C
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06003039 RID: 12345 RVA: 0x0010AA44 File Offset: 0x00108C44
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x0600303A RID: 12346 RVA: 0x0010AA4D File Offset: 0x00108C4D
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x0600303B RID: 12347 RVA: 0x0010AA63 File Offset: 0x00108C63
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

	// Token: 0x0600303C RID: 12348 RVA: 0x0010AA72 File Offset: 0x00108C72
	public LogicWire.BitDepth GetMaxBitRating()
	{
		return this.MaxBitDepth;
	}

	// Token: 0x0600303D RID: 12349 RVA: 0x0010AA7A File Offset: 0x00108C7A
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x0600303E RID: 12350 RVA: 0x0010AA88 File Offset: 0x00108C88
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x0600303F RID: 12351 RVA: 0x0010AAC4 File Offset: 0x00108CC4
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x06003040 RID: 12352 RVA: 0x0010AAFA File Offset: 0x00108CFA
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04001C44 RID: 7236
	[SerializeField]
	public LogicWire.BitDepth MaxBitDepth;

	// Token: 0x04001C45 RID: 7237
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04001C46 RID: 7238
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x04001C47 RID: 7239
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001C48 RID: 7240
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x04001C49 RID: 7241
	private System.Action firstFrameCallback;

	// Token: 0x0200155C RID: 5468
	public enum BitDepth
	{
		// Token: 0x04006C8E RID: 27790
		OneBit,
		// Token: 0x04006C8F RID: 27791
		FourBit,
		// Token: 0x04006C90 RID: 27792
		NumRatings
	}
}
