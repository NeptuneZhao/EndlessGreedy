using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020006A0 RID: 1696
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Conduit")]
public class Conduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IDisconnectable, FlowUtilityNetwork.IItem
{
	// Token: 0x06002A8E RID: 10894 RVA: 0x000F048C File Offset: 0x000EE68C
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06002A8F RID: 10895 RVA: 0x000F04A2 File Offset: 0x000EE6A2
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

	// Token: 0x06002A90 RID: 10896 RVA: 0x000F04B4 File Offset: 0x000EE6B4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Conduit>(-1201923725, Conduit.OnHighlightedDelegate);
		base.Subscribe<Conduit>(-700727624, Conduit.OnConduitFrozenDelegate);
		base.Subscribe<Conduit>(-1152799878, Conduit.OnConduitBoilingDelegate);
		base.Subscribe<Conduit>(-1555603773, Conduit.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x000F050B File Offset: 0x000EE70B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate);
		base.Subscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x000F0538 File Offset: 0x000EE738
	protected virtual void OnStructureTemperatureRegistered(object data)
	{
		int cell = Grid.PosToCell(this);
		this.GetNetworkManager().AddToNetworks(cell, this, false);
		this.Connect();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Pipe, this);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().AddThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x000F05D0 File Offset: 0x000EE7D0
	protected override void OnCleanUp()
	{
		base.Unsubscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate, false);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().RemoveThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
		}
		base.OnCleanUp();
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x000F06C4 File Offset: 0x000EE8C4
	protected ConduitFlowVisualizer GetFlowVisualizer()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidFlowVisualizer;
		}
		return Game.Instance.gasFlowVisualizer;
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x000F06E4 File Offset: 0x000EE8E4
	public IUtilityNetworkMgr GetNetworkManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x000F0704 File Offset: 0x000EE904
	public ConduitFlow GetFlowManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x000F0724 File Offset: 0x000EE924
	public static ConduitFlow GetFlowManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x000F073F File Offset: 0x000EE93F
	public static IUtilityNetworkMgr GetNetworkManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x000F075C File Offset: 0x000EE95C
	public virtual void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x000F0788 File Offset: 0x000EE988
	public virtual bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		return networks.Contains(networkForCell);
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x000F07AE File Offset: 0x000EE9AE
	public virtual int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x000F07B8 File Offset: 0x000EE9B8
	private void OnHighlighted(object data)
	{
		int highlightedCell = ((bool)data) ? Grid.PosToCell(base.transform.GetPosition()) : -1;
		this.GetFlowVisualizer().SetHighlightedCell(highlightedCell);
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x000F07F0 File Offset: 0x000EE9F0
	private void OnConduitFrozen(object data)
	{
		base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_FROZE,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_FROZE,
			takeDamageEffect = ((this.ConduitType == ConduitType.Gas) ? SpawnFXHashes.BuildingLeakLiquid : SpawnFXHashes.BuildingFreeze),
			fullDamageEffectName = ((this.ConduitType == ConduitType.Gas) ? "water_damage_kanim" : "ice_damage_kanim")
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x000F0894 File Offset: 0x000EEA94
	private void OnConduitBoiling(object data)
	{
		base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_BOILED,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_BOILED,
			takeDamageEffect = SpawnFXHashes.BuildingLeakGas,
			fullDamageEffectName = "gas_damage_kanim"
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x000F0917 File Offset: 0x000EEB17
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x000F091F File Offset: 0x000EEB1F
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x000F0928 File Offset: 0x000EEB28
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x000F0930 File Offset: 0x000EEB30
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			this.GetNetworkManager().ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000F0971 File Offset: 0x000EEB71
	public void Disconnect()
	{
		this.disconnected = true;
		this.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x17000240 RID: 576
	// (set) Token: 0x06002AA4 RID: 10916 RVA: 0x000F0985 File Offset: 0x000EEB85
	public FlowUtilityNetwork Network
	{
		set
		{
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06002AA5 RID: 10917 RVA: 0x000F0987 File Offset: 0x000EEB87
	public int Cell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06002AA6 RID: 10918 RVA: 0x000F098F File Offset: 0x000EEB8F
	public Endpoint EndpointType
	{
		get
		{
			return Endpoint.Conduit;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06002AA7 RID: 10919 RVA: 0x000F0992 File Offset: 0x000EEB92
	public ConduitType ConduitType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06002AA8 RID: 10920 RVA: 0x000F099A File Offset: 0x000EEB9A
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x04001884 RID: 6276
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04001885 RID: 6277
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04001886 RID: 6278
	public ConduitType type;

	// Token: 0x04001887 RID: 6279
	private System.Action firstFrameCallback;

	// Token: 0x04001888 RID: 6280
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnHighlightedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnHighlighted(data);
	});

	// Token: 0x04001889 RID: 6281
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitFrozenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitFrozen(data);
	});

	// Token: 0x0400188A RID: 6282
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitBoilingDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitBoiling(data);
	});

	// Token: 0x0400188B RID: 6283
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x0400188C RID: 6284
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x0400188D RID: 6285
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
