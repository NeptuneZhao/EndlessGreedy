using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200079E RID: 1950
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Wire")]
public class Wire : KMonoBehaviour, IDisconnectable, IFirstFrameCallback, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x06003550 RID: 13648 RVA: 0x001223BC File Offset: 0x001205BC
	public static float GetMaxWattageAsFloat(Wire.WattageRating rating)
	{
		switch (rating)
		{
		case Wire.WattageRating.Max500:
			return 500f;
		case Wire.WattageRating.Max1000:
			return 1000f;
		case Wire.WattageRating.Max2000:
			return 2000f;
		case Wire.WattageRating.Max20000:
			return 20000f;
		case Wire.WattageRating.Max50000:
			return 50000f;
		default:
			return 0f;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06003551 RID: 13649 RVA: 0x00122408 File Offset: 0x00120608
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) is ElectricalUtilityNetwork;
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06003552 RID: 13650 RVA: 0x00122440 File Offset: 0x00120640
	public ushort NetworkID
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ElectricalUtilityNetwork electricalUtilityNetwork = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) as ElectricalUtilityNetwork;
			if (electricalUtilityNetwork == null)
			{
				return ushort.MaxValue;
			}
			return (ushort)electricalUtilityNetwork.id;
		}
	}

	// Token: 0x06003553 RID: 13651 RVA: 0x00122484 File Offset: 0x00120684
	protected override void OnSpawn()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.electricalConduitSystem.AddToNetworks(cell, this, false);
		this.InitializeSwitchState();
		base.Subscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate);
		base.Subscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireCircuitStatus, this);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireMaxWattageStatus, this);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(Wire.OutlineSymbol, false);
	}

	// Token: 0x06003554 RID: 13652 RVA: 0x00122514 File Offset: 0x00120714
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.electricalConduitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06003555 RID: 13653 RVA: 0x001225A0 File Offset: 0x001207A0
	private void InitializeSwitchState()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		bool flag = false;
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			CircuitSwitch component = gameObject.GetComponent<CircuitSwitch>();
			if (component != null)
			{
				flag = true;
				component.AttachWire(this);
			}
		}
		if (!flag)
		{
			this.Connect();
		}
	}

	// Token: 0x06003556 RID: 13654 RVA: 0x001225FC File Offset: 0x001207FC
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.electricalConduitSystem.GetConnections(cell, true);
	}

	// Token: 0x06003557 RID: 13655 RVA: 0x0012262C File Offset: 0x0012082C
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.electricalConduitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x00122650 File Offset: 0x00120850
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x00122658 File Offset: 0x00120858
	private void OnBuildingFullyRepaired(object data)
	{
		this.InitializeSwitchState();
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x00122660 File Offset: 0x00120860
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.Wires, false);
		if (Wire.WireCircuitStatus == null)
		{
			Wire.WireCircuitStatus = new StatusItem("WireCircuitStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(circuitID);
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				string wireLoadColor = GameUtil.GetWireLoadColor(wattsUsedByCircuit, maxWattageAsFloat, wattsNeededWhenActive);
				str = str.Replace("{CurrentLoadAndColor}", (wireLoadColor == Color.white.ToHexString()) ? GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit, true) : string.Concat(new string[]
				{
					"<color=#",
					wireLoadColor,
					">",
					GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit, true),
					"</color>"
				}));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				str = str.Replace("{WireType}", this.GetProperName());
				return str;
			});
		}
		if (Wire.WireMaxWattageStatus == null)
		{
			Wire.WireMaxWattageStatus = new StatusItem("WireMaxWattageStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				str = str.Replace("{TotalPotentialLoadAndColor}", (wattsNeededWhenActive > maxWattageAsFloat) ? string.Concat(new string[]
				{
					"<color=#",
					new Color(0.9843137f, 0.6901961f, 0.23137255f).ToHexString(),
					">",
					GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true),
					"</color>"
				}) : GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				return str;
			});
		}
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x00122717 File Offset: 0x00120917
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.MaxWattageRating;
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x0012271F File Offset: 0x0012091F
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x00122728 File Offset: 0x00120928
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x00122770 File Offset: 0x00120970
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x001227BE File Offset: 0x001209BE
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x001227D4 File Offset: 0x001209D4
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

	// Token: 0x06003561 RID: 13665 RVA: 0x001227E3 File Offset: 0x001209E3
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x06003562 RID: 13666 RVA: 0x001227F0 File Offset: 0x001209F0
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x0012282C File Offset: 0x00120A2C
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x00122862 File Offset: 0x00120A62
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04001FAB RID: 8107
	[SerializeField]
	public Wire.WattageRating MaxWattageRating;

	// Token: 0x04001FAC RID: 8108
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04001FAD RID: 8109
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x04001FAE RID: 8110
	public float circuitOverloadTime;

	// Token: 0x04001FAF RID: 8111
	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001FB0 RID: 8112
	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x04001FB1 RID: 8113
	private static StatusItem WireCircuitStatus = null;

	// Token: 0x04001FB2 RID: 8114
	private static StatusItem WireMaxWattageStatus = null;

	// Token: 0x04001FB3 RID: 8115
	private System.Action firstFrameCallback;

	// Token: 0x02001657 RID: 5719
	public enum WattageRating
	{
		// Token: 0x04006F64 RID: 28516
		Max500,
		// Token: 0x04006F65 RID: 28517
		Max1000,
		// Token: 0x04006F66 RID: 28518
		Max2000,
		// Token: 0x04006F67 RID: 28519
		Max20000,
		// Token: 0x04006F68 RID: 28520
		Max50000,
		// Token: 0x04006F69 RID: 28521
		NumRatings
	}
}
