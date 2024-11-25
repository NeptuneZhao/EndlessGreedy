using System;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x0200087E RID: 2174
[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{name} {WattsUsed}W")]
[AddComponentMenu("KMonoBehaviour/scripts/EnergyConsumer")]
public class EnergyConsumer : KMonoBehaviour, ISaveLoadable, IEnergyConsumer, ICircuitConnected, IGameObjectEffectDescriptor
{
	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x06003CD4 RID: 15572 RVA: 0x0015125F File Offset: 0x0014F45F
	public int PowerSortOrder
	{
		get
		{
			return this.powerSortOrder;
		}
	}

	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06003CD5 RID: 15573 RVA: 0x00151267 File Offset: 0x0014F467
	// (set) Token: 0x06003CD6 RID: 15574 RVA: 0x0015126F File Offset: 0x0014F46F
	public int PowerCell { get; private set; }

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06003CD7 RID: 15575 RVA: 0x00151278 File Offset: 0x0014F478
	public bool HasWire
	{
		get
		{
			return Grid.Objects[this.PowerCell, 26] != null;
		}
	}

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06003CD8 RID: 15576 RVA: 0x00151292 File Offset: 0x0014F492
	// (set) Token: 0x06003CD9 RID: 15577 RVA: 0x001512A4 File Offset: 0x0014F4A4
	public virtual bool IsPowered
	{
		get
		{
			return this.operational.GetFlag(EnergyConsumer.PoweredFlag);
		}
		protected set
		{
			this.operational.SetFlag(EnergyConsumer.PoweredFlag, value);
		}
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06003CDA RID: 15578 RVA: 0x001512B7 File Offset: 0x0014F4B7
	public bool IsConnected
	{
		get
		{
			return this.CircuitID != ushort.MaxValue;
		}
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06003CDB RID: 15579 RVA: 0x001512C9 File Offset: 0x0014F4C9
	public string Name
	{
		get
		{
			return this.selectable.GetName();
		}
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06003CDC RID: 15580 RVA: 0x001512D6 File Offset: 0x0014F4D6
	// (set) Token: 0x06003CDD RID: 15581 RVA: 0x001512DE File Offset: 0x0014F4DE
	public bool IsVirtual { get; private set; }

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06003CDE RID: 15582 RVA: 0x001512E7 File Offset: 0x0014F4E7
	// (set) Token: 0x06003CDF RID: 15583 RVA: 0x001512EF File Offset: 0x0014F4EF
	public object VirtualCircuitKey { get; private set; }

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x001512F8 File Offset: 0x0014F4F8
	// (set) Token: 0x06003CE1 RID: 15585 RVA: 0x00151300 File Offset: 0x0014F500
	public ushort CircuitID { get; private set; }

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x06003CE2 RID: 15586 RVA: 0x00151309 File Offset: 0x0014F509
	// (set) Token: 0x06003CE3 RID: 15587 RVA: 0x00151311 File Offset: 0x0014F511
	public float BaseWattageRating
	{
		get
		{
			return this._BaseWattageRating;
		}
		set
		{
			this._BaseWattageRating = value;
		}
	}

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x06003CE4 RID: 15588 RVA: 0x0015131A File Offset: 0x0014F51A
	public float WattsUsed
	{
		get
		{
			if (this.operational.IsActive)
			{
				return this.BaseWattageRating;
			}
			return 0f;
		}
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x00151335 File Offset: 0x0014F535
	public float WattsNeededWhenActive
	{
		get
		{
			return this.building.Def.EnergyConsumptionWhenActive;
		}
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x00151347 File Offset: 0x0014F547
	protected override void OnPrefabInit()
	{
		this.CircuitID = ushort.MaxValue;
		this.IsPowered = false;
		this.BaseWattageRating = this.building.Def.EnergyConsumptionWhenActive;
	}

	// Token: 0x06003CE7 RID: 15591 RVA: 0x00151374 File Offset: 0x0014F574
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.EnergyConsumers.Add(this);
		Building component = base.GetComponent<Building>();
		this.PowerCell = component.GetPowerInputCell();
		Game.Instance.circuitManager.Connect(this);
		Game.Instance.energySim.AddEnergyConsumer(this);
	}

	// Token: 0x06003CE8 RID: 15592 RVA: 0x001513C5 File Offset: 0x0014F5C5
	protected override void OnCleanUp()
	{
		Game.Instance.energySim.RemoveEnergyConsumer(this);
		Game.Instance.circuitManager.Disconnect(this, true);
		Components.EnergyConsumers.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x001513F9 File Offset: 0x0014F5F9
	public virtual void EnergySim200ms(float dt)
	{
		this.CircuitID = Game.Instance.circuitManager.GetCircuitID(this);
		if (!this.IsConnected)
		{
			this.IsPowered = false;
		}
		this.circuitOverloadTime = Mathf.Max(0f, this.circuitOverloadTime - dt);
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x00151438 File Offset: 0x0014F638
	public virtual void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
	{
		switch (connection_status)
		{
		case CircuitManager.ConnectionStatus.NotConnected:
			this.IsPowered = false;
			return;
		case CircuitManager.ConnectionStatus.Unpowered:
			if (this.IsPowered && base.GetComponent<Battery>() == null)
			{
				this.IsPowered = false;
				this.circuitOverloadTime = 6f;
				this.PlayCircuitSound("overdraw");
				return;
			}
			break;
		case CircuitManager.ConnectionStatus.Powered:
			if (!this.IsPowered && this.circuitOverloadTime <= 0f)
			{
				this.IsPowered = true;
				this.PlayCircuitSound("powered");
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06003CEB RID: 15595 RVA: 0x001514BC File Offset: 0x0014F6BC
	protected void PlayCircuitSound(string state)
	{
		EventReference event_ref;
		if (state == "powered")
		{
			event_ref = Sounds.Instance.BuildingPowerOnMigrated;
		}
		else if (state == "overdraw")
		{
			event_ref = Sounds.Instance.ElectricGridOverloadMigrated;
		}
		else
		{
			event_ref = default(EventReference);
			global::Debug.Log("Invalid state for sound in EnergyConsumer.");
		}
		if (!CameraController.Instance.IsAudibleSound(base.transform.GetPosition()))
		{
			return;
		}
		float num;
		if (!this.lastTimeSoundPlayed.TryGetValue(state, out num))
		{
			num = 0f;
		}
		float value = (Time.time - num) / this.soundDecayTime;
		Vector3 position = base.transform.GetPosition();
		position.z = 0f;
		FMOD.Studio.EventInstance instance = KFMOD.BeginOneShot(event_ref, CameraController.Instance.GetVerticallyScaledPosition(position, false), 1f);
		instance.setParameterByName("timeSinceLast", value, false);
		KFMOD.EndOneShot(instance);
		this.lastTimeSoundPlayed[state] = Time.time;
	}

	// Token: 0x06003CEC RID: 15596 RVA: 0x001515AA File Offset: 0x0014F7AA
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x04002526 RID: 9510
	[MyCmpReq]
	private Building building;

	// Token: 0x04002527 RID: 9511
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04002528 RID: 9512
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04002529 RID: 9513
	[SerializeField]
	public int powerSortOrder;

	// Token: 0x0400252B RID: 9515
	[Serialize]
	protected float circuitOverloadTime;

	// Token: 0x0400252C RID: 9516
	public static readonly Operational.Flag PoweredFlag = new Operational.Flag("powered", Operational.Flag.Type.Requirement);

	// Token: 0x0400252D RID: 9517
	private Dictionary<string, float> lastTimeSoundPlayed = new Dictionary<string, float>();

	// Token: 0x0400252E RID: 9518
	private float soundDecayTime = 10f;

	// Token: 0x04002532 RID: 9522
	private float _BaseWattageRating;
}
