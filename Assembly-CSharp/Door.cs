using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020006C0 RID: 1728
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Door")]
public class Door : Workable, ISaveLoadable, ISim200ms, INavDoor
{
	// Token: 0x06002B90 RID: 11152 RVA: 0x000F46AC File Offset: 0x000F28AC
	private void OnCopySettings(object data)
	{
		Door component = ((GameObject)data).GetComponent<Door>();
		if (component != null)
		{
			this.QueueStateChange(component.requestedState);
		}
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x000F46DA File Offset: 0x000F28DA
	public Door()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06002B92 RID: 11154 RVA: 0x000F470A File Offset: 0x000F290A
	public Door.ControlState CurrentState
	{
		get
		{
			return this.controlState;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06002B93 RID: 11155 RVA: 0x000F4712 File Offset: 0x000F2912
	public Door.ControlState RequestedState
	{
		get
		{
			return this.requestedState;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06002B94 RID: 11156 RVA: 0x000F471A File Offset: 0x000F291A
	public bool ShouldBlockFallingSand
	{
		get
		{
			return this.rotatable.GetOrientation() != this.verticalOrientation;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06002B95 RID: 11157 RVA: 0x000F4732 File Offset: 0x000F2932
	public bool isSealed
	{
		get
		{
			return this.controller != null && this.controller.sm.isSealed.Get(this.controller);
		}
	}

	// Token: 0x06002B96 RID: 11158 RVA: 0x000F475C File Offset: 0x000F295C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = Door.OVERRIDE_ANIMS;
		this.synchronizeAnims = false;
		base.SetWorkTime(3f);
		if (!string.IsNullOrEmpty(this.doorClosingSoundEventName))
		{
			this.doorClosingSound = GlobalAssets.GetSound(this.doorClosingSoundEventName, false);
		}
		if (!string.IsNullOrEmpty(this.doorOpeningSoundEventName))
		{
			this.doorOpeningSound = GlobalAssets.GetSound(this.doorOpeningSoundEventName, false);
		}
		base.Subscribe<Door>(-905833192, Door.OnCopySettingsDelegate);
	}

	// Token: 0x06002B97 RID: 11159 RVA: 0x000F47DB File Offset: 0x000F29DB
	private Door.ControlState GetNextState(Door.ControlState wantedState)
	{
		return (wantedState + 1) % Door.ControlState.NumStates;
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000F47E2 File Offset: 0x000F29E2
	private static bool DisplacesGas(Door.DoorType type)
	{
		return type != Door.DoorType.Internal;
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000F47EC File Offset: 0x000F29EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<KPrefabID>() != null)
		{
			this.log = new LoggerFSS("Door", 35);
		}
		if (!this.allowAutoControl && this.controlState == Door.ControlState.Auto)
		{
			this.controlState = Door.ControlState.Locked;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		if (Door.DisplacesGas(this.doorType))
		{
			structureTemperatures.Bypass(handle);
		}
		this.controller = new Door.Controller.Instance(this);
		this.controller.StartSM();
		if (this.doorType == Door.DoorType.Sealed && !this.hasBeenUnsealed)
		{
			this.Seal();
		}
		this.UpdateDoorSpeed(this.operational.IsOperational);
		base.Subscribe<Door>(-592767678, Door.OnOperationalChangedDelegate);
		base.Subscribe<Door>(824508782, Door.OnOperationalChangedDelegate);
		base.Subscribe<Door>(-801688580, Door.OnLogicValueChangedDelegate);
		this.requestedState = this.CurrentState;
		this.ApplyRequestedControlState(true);
		int num = (this.rotatable.GetOrientation() == Orientation.Neutral) ? (this.building.Def.WidthInCells * (this.building.Def.HeightInCells - 1)) : 0;
		int num2 = (this.rotatable.GetOrientation() == Orientation.Neutral) ? this.building.Def.WidthInCells : this.building.Def.HeightInCells;
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = this.building.PlacementCells[num + num3];
			Grid.FakeFloor.Add(num4);
			Pathfinding.Instance.AddDirtyNavGridCell(num4);
		}
		List<int> list = new List<int>();
		foreach (int num5 in this.building.PlacementCells)
		{
			Grid.HasDoor[num5] = true;
			if (this.rotatable.IsRotated)
			{
				list.Add(Grid.CellAbove(num5));
				list.Add(Grid.CellBelow(num5));
			}
			else
			{
				list.Add(Grid.CellLeft(num5));
				list.Add(Grid.CellRight(num5));
			}
			SimMessages.SetCellProperties(num5, 8);
			if (Door.DisplacesGas(this.doorType))
			{
				Grid.RenderedByWorld[num5] = false;
			}
		}
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x000F4A2C File Offset: 0x000F2C2C
	protected override void OnCleanUp()
	{
		this.UpdateDoorState(true);
		List<int> list = new List<int>();
		foreach (int num in this.building.PlacementCells)
		{
			SimMessages.ClearCellProperties(num, 12);
			Grid.RenderedByWorld[num] = Grid.Element[num].substance.renderedByWorld;
			Grid.FakeFloor.Remove(num);
			if (Grid.Element[num].IsSolid)
			{
				SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0f, -1f, byte.MaxValue, 0, -1);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
			if (this.rotatable.IsRotated)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellBelow(num));
			}
			else
			{
				list.Add(Grid.CellLeft(num));
				list.Add(Grid.CellRight(num));
			}
		}
		foreach (int num2 in this.building.PlacementCells)
		{
			Grid.HasDoor[num2] = false;
			Game.Instance.SetDupePassableSolid(num2, false, Grid.Solid[num2]);
			Grid.CritterImpassable[num2] = false;
			Grid.DupeImpassable[num2] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(num2);
		}
		base.OnCleanUp();
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x000F4B88 File Offset: 0x000F2D88
	public void Seal()
	{
		this.controller.sm.isSealed.Set(true, this.controller, false);
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x000F4BA8 File Offset: 0x000F2DA8
	public void OrderUnseal()
	{
		this.controller.GoTo(this.controller.sm.Sealed.awaiting_unlock);
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x000F4BCC File Offset: 0x000F2DCC
	private void RefreshControlState()
	{
		switch (this.controlState)
		{
		case Door.ControlState.Auto:
			this.controller.sm.isLocked.Set(false, this.controller, false);
			break;
		case Door.ControlState.Opened:
			this.controller.sm.isLocked.Set(false, this.controller, false);
			break;
		case Door.ControlState.Locked:
			this.controller.sm.isLocked.Set(true, this.controller, false);
			break;
		}
		base.Trigger(279163026, this.controlState);
		this.SetWorldState();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CurrentDoorControlState, this);
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x000F4C9C File Offset: 0x000F2E9C
	private void OnOperationalChanged(object data)
	{
		bool isOperational = this.operational.IsOperational;
		if (isOperational != this.on)
		{
			this.UpdateDoorSpeed(isOperational);
			if (this.on && base.GetComponent<KPrefabID>().HasTag(GameTags.Transition))
			{
				this.SetActive(true);
				return;
			}
			this.SetActive(false);
		}
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x000F4CF0 File Offset: 0x000F2EF0
	private void UpdateDoorSpeed(bool powered)
	{
		this.on = powered;
		this.UpdateAnimAndSoundParams(powered);
		float positionPercent = this.animController.GetPositionPercent();
		this.animController.Play(this.animController.CurrentAnim.hash, this.animController.PlayMode, 1f, 0f);
		this.animController.SetPositionPercent(positionPercent);
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x000F4D54 File Offset: 0x000F2F54
	private void UpdateAnimAndSoundParams(bool powered)
	{
		if (powered)
		{
			this.animController.PlaySpeedMultiplier = this.poweredAnimSpeed;
			if (this.doorClosingSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorClosingSound, Door.SOUND_POWERED_PARAMETER, 1f);
			}
			if (this.doorOpeningSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorOpeningSound, Door.SOUND_POWERED_PARAMETER, 1f);
				return;
			}
		}
		else
		{
			this.animController.PlaySpeedMultiplier = this.unpoweredAnimSpeed;
			if (this.doorClosingSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorClosingSound, Door.SOUND_POWERED_PARAMETER, 0f);
			}
			if (this.doorOpeningSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorOpeningSound, Door.SOUND_POWERED_PARAMETER, 0f);
			}
		}
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000F4E13 File Offset: 0x000F3013
	private void SetActive(bool active)
	{
		if (this.operational.IsOperational)
		{
			this.operational.SetActive(active, false);
		}
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x000F4E30 File Offset: 0x000F3030
	private void SetWorldState()
	{
		int[] placementCells = this.building.PlacementCells;
		bool is_door_open = this.IsOpen();
		this.SetPassableState(is_door_open, placementCells);
		this.SetSimState(is_door_open, placementCells);
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x000F4E60 File Offset: 0x000F3060
	private void SetPassableState(bool is_door_open, IList<int> cells)
	{
		for (int i = 0; i < cells.Count; i++)
		{
			int num = cells[i];
			switch (this.doorType)
			{
			case Door.DoorType.Pressure:
			case Door.DoorType.ManualPressure:
			case Door.DoorType.Sealed:
			{
				Grid.CritterImpassable[num] = (this.controlState != Door.ControlState.Opened);
				bool solid = !is_door_open;
				bool passable = this.controlState != Door.ControlState.Locked;
				Game.Instance.SetDupePassableSolid(num, passable, solid);
				if (this.controlState == Door.ControlState.Opened)
				{
					this.doorOpenLiquidRefreshHack = true;
					this.doorOpenLiquidRefreshTime = 1f;
				}
				break;
			}
			case Door.DoorType.Internal:
				Grid.CritterImpassable[num] = (this.controlState != Door.ControlState.Opened);
				Grid.DupeImpassable[num] = (this.controlState == Door.ControlState.Locked);
				break;
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
		}
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x000F4F38 File Offset: 0x000F3138
	private void SetSimState(bool is_door_open, IList<int> cells)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		float mass = component.Mass / (float)cells.Count;
		for (int i = 0; i < cells.Count; i++)
		{
			int num = cells[i];
			Door.DoorType doorType = this.doorType;
			if (doorType <= Door.DoorType.ManualPressure || doorType == Door.DoorType.Sealed)
			{
				World.Instance.groundRenderer.MarkDirty(num);
				if (is_door_open)
				{
					SimMessages.Dig(num, Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimDoorOpened), false)).index, true);
					if (this.ShouldBlockFallingSand)
					{
						SimMessages.ClearCellProperties(num, 4);
					}
					else
					{
						SimMessages.SetCellProperties(num, 4);
					}
				}
				else
				{
					HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimDoorClosed), false));
					float temperature = component.Temperature;
					if (temperature <= 0f)
					{
						temperature = component.Temperature;
					}
					SimMessages.ReplaceAndDisplaceElement(num, component.ElementID, CellEventLogger.Instance.DoorClose, mass, temperature, byte.MaxValue, 0, handle.index);
					SimMessages.SetCellProperties(num, 4);
				}
			}
		}
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x000F5058 File Offset: 0x000F3258
	private void UpdateDoorState(bool cleaningUp)
	{
		foreach (int num in this.building.PlacementCells)
		{
			if (Grid.IsValidCell(num))
			{
				Grid.Foundation[num] = !cleaningUp;
			}
		}
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x000F509C File Offset: 0x000F329C
	public void QueueStateChange(Door.ControlState nextState)
	{
		if (this.requestedState != nextState)
		{
			this.requestedState = nextState;
		}
		else
		{
			this.requestedState = this.controlState;
		}
		if (this.requestedState == this.controlState)
		{
			if (this.changeStateChore != null)
			{
				this.changeStateChore.Cancel("Change state");
				this.changeStateChore = null;
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
			}
			return;
		}
		if (DebugHandler.InstantBuildMode)
		{
			this.controlState = this.requestedState;
			this.RefreshControlState();
			this.OnOperationalChanged(null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
			this.Open();
			this.Close();
			return;
		}
		if (this.changeStateChore != null)
		{
			this.changeStateChore.Cancel("Change state");
		}
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, this);
		this.changeStateChore = new WorkChore<Door>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x000F51BC File Offset: 0x000F33BC
	private void OnSimDoorOpened()
	{
		if (this == null || !Door.DisplacesGas(this.doorType))
		{
			return;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		structureTemperatures.UnBypass(handle);
		this.do_melt_check = false;
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x000F5200 File Offset: 0x000F3400
	private void OnSimDoorClosed()
	{
		if (this == null || !Door.DisplacesGas(this.doorType))
		{
			return;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		structureTemperatures.Bypass(handle);
		this.do_melt_check = true;
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x000F5243 File Offset: 0x000F3443
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.changeStateChore = null;
		this.ApplyRequestedControlState(false);
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x000F525C File Offset: 0x000F345C
	public void Open()
	{
		if (this.openCount == 0 && Door.DisplacesGas(this.doorType))
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
			{
				int[] placementCells = this.building.PlacementCells;
				float num = 0f;
				int num2 = 0;
				foreach (int i2 in placementCells)
				{
					if (Grid.Mass[i2] > 0f)
					{
						num2++;
						num += Grid.Temperature[i2];
					}
				}
				if (num2 > 0)
				{
					num /= (float)placementCells.Length;
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					KCrashReporter.Assert(num > 0f, "Door has calculated an invalid temperature", null);
					component.Temperature = num;
				}
			}
		}
		this.openCount++;
		Door.ControlState controlState = this.controlState;
		if (controlState > Door.ControlState.Opened)
		{
			return;
		}
		this.controller.sm.isOpen.Set(true, this.controller, false);
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x000F5370 File Offset: 0x000F3570
	public void Close()
	{
		this.openCount = Mathf.Max(0, this.openCount - 1);
		if (this.openCount == 0 && Door.DisplacesGas(this.doorType))
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (handle.IsValid() && !structureTemperatures.IsBypassed(handle))
			{
				float temperature = structureTemperatures.GetPayload(handle).Temperature;
				component.Temperature = temperature;
			}
		}
		switch (this.controlState)
		{
		case Door.ControlState.Auto:
			if (this.openCount == 0)
			{
				this.controller.sm.isOpen.Set(false, this.controller, false);
				Game.Instance.userMenu.Refresh(base.gameObject);
			}
			break;
		case Door.ControlState.Opened:
			break;
		case Door.ControlState.Locked:
			this.controller.sm.isOpen.Set(false, this.controller, false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x000F5461 File Offset: 0x000F3661
	public bool IsPendingClose()
	{
		return this.controller.IsInsideState(this.controller.sm.closedelay);
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x000F5480 File Offset: 0x000F3680
	public bool IsOpen()
	{
		return this.controller.IsInsideState(this.controller.sm.open) || this.controller.IsInsideState(this.controller.sm.closedelay) || this.controller.IsInsideState(this.controller.sm.closeblocked);
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x000F54E4 File Offset: 0x000F36E4
	private void ApplyRequestedControlState(bool force = false)
	{
		if (this.requestedState == this.controlState && !force)
		{
			return;
		}
		this.controlState = this.requestedState;
		this.RefreshControlState();
		this.OnOperationalChanged(null);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
		base.Trigger(1734268753, this);
		if (!force)
		{
			this.Open();
			this.Close();
		}
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x000F5554 File Offset: 0x000F3754
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != Door.OPEN_CLOSE_PORT_ID)
		{
			return;
		}
		int newValue = logicValueChanged.newValue;
		if (this.changeStateChore != null)
		{
			this.changeStateChore.Cancel("Change state");
			this.changeStateChore = null;
		}
		this.requestedState = (LogicCircuitNetwork.IsBitActive(0, newValue) ? Door.ControlState.Opened : Door.ControlState.Locked);
		this.applyLogicChange = true;
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x000F55C0 File Offset: 0x000F37C0
	public void Sim200ms(float dt)
	{
		if (this == null)
		{
			return;
		}
		if (this.doorOpenLiquidRefreshHack)
		{
			this.doorOpenLiquidRefreshTime -= dt;
			if (this.doorOpenLiquidRefreshTime <= 0f)
			{
				this.doorOpenLiquidRefreshHack = false;
				foreach (int cell in this.building.PlacementCells)
				{
					Pathfinding.Instance.AddDirtyNavGridCell(cell);
				}
			}
		}
		if (this.applyLogicChange)
		{
			this.applyLogicChange = false;
			this.ApplyRequestedControlState(false);
		}
		if (this.do_melt_check)
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
			{
				foreach (int i2 in this.building.PlacementCells)
				{
					if (!Grid.Solid[i2])
					{
						Util.KDestroyGameObject(this);
						return;
					}
				}
			}
		}
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x000F5749 File Offset: 0x000F3949
	bool INavDoor.get_isSpawned()
	{
		return base.isSpawned;
	}

	// Token: 0x040018FD RID: 6397
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040018FE RID: 6398
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x040018FF RID: 6399
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04001900 RID: 6400
	[MyCmpReq]
	public Building building;

	// Token: 0x04001901 RID: 6401
	[MyCmpGet]
	private EnergyConsumer consumer;

	// Token: 0x04001902 RID: 6402
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x04001903 RID: 6403
	public Orientation verticalOrientation;

	// Token: 0x04001904 RID: 6404
	[SerializeField]
	public bool hasComplexUserControls;

	// Token: 0x04001905 RID: 6405
	[SerializeField]
	public float unpoweredAnimSpeed = 0.25f;

	// Token: 0x04001906 RID: 6406
	[SerializeField]
	public float poweredAnimSpeed = 1f;

	// Token: 0x04001907 RID: 6407
	[SerializeField]
	public Door.DoorType doorType;

	// Token: 0x04001908 RID: 6408
	[SerializeField]
	public bool allowAutoControl = true;

	// Token: 0x04001909 RID: 6409
	[SerializeField]
	public string doorClosingSoundEventName;

	// Token: 0x0400190A RID: 6410
	[SerializeField]
	public string doorOpeningSoundEventName;

	// Token: 0x0400190B RID: 6411
	private string doorClosingSound;

	// Token: 0x0400190C RID: 6412
	private string doorOpeningSound;

	// Token: 0x0400190D RID: 6413
	private static readonly HashedString SOUND_POWERED_PARAMETER = "doorPowered";

	// Token: 0x0400190E RID: 6414
	private static readonly HashedString SOUND_PROGRESS_PARAMETER = "doorProgress";

	// Token: 0x0400190F RID: 6415
	[Serialize]
	private bool hasBeenUnsealed;

	// Token: 0x04001910 RID: 6416
	[Serialize]
	private Door.ControlState controlState;

	// Token: 0x04001911 RID: 6417
	private bool on;

	// Token: 0x04001912 RID: 6418
	private bool do_melt_check;

	// Token: 0x04001913 RID: 6419
	private int openCount;

	// Token: 0x04001914 RID: 6420
	private Door.ControlState requestedState;

	// Token: 0x04001915 RID: 6421
	private Chore changeStateChore;

	// Token: 0x04001916 RID: 6422
	private Door.Controller.Instance controller;

	// Token: 0x04001917 RID: 6423
	private LoggerFSS log;

	// Token: 0x04001918 RID: 6424
	private const float REFRESH_HACK_DELAY = 1f;

	// Token: 0x04001919 RID: 6425
	private bool doorOpenLiquidRefreshHack;

	// Token: 0x0400191A RID: 6426
	private float doorOpenLiquidRefreshTime;

	// Token: 0x0400191B RID: 6427
	private static readonly EventSystem.IntraObjectHandler<Door> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400191C RID: 6428
	public static readonly HashedString OPEN_CLOSE_PORT_ID = new HashedString("DoorOpenClose");

	// Token: 0x0400191D RID: 6429
	private static readonly KAnimFile[] OVERRIDE_ANIMS = new KAnimFile[]
	{
		Assets.GetAnim("anim_use_remote_kanim")
	};

	// Token: 0x0400191E RID: 6430
	private static readonly EventSystem.IntraObjectHandler<Door> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0400191F RID: 6431
	private static readonly EventSystem.IntraObjectHandler<Door> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001920 RID: 6432
	private bool applyLogicChange;

	// Token: 0x020014BC RID: 5308
	public enum DoorType
	{
		// Token: 0x04006AC5 RID: 27333
		Pressure,
		// Token: 0x04006AC6 RID: 27334
		ManualPressure,
		// Token: 0x04006AC7 RID: 27335
		Internal,
		// Token: 0x04006AC8 RID: 27336
		Sealed
	}

	// Token: 0x020014BD RID: 5309
	public enum ControlState
	{
		// Token: 0x04006ACA RID: 27338
		Auto,
		// Token: 0x04006ACB RID: 27339
		Opened,
		// Token: 0x04006ACC RID: 27340
		Locked,
		// Token: 0x04006ACD RID: 27341
		NumStates
	}

	// Token: 0x020014BE RID: 5310
	public class Controller : GameStateMachine<Door.Controller, Door.Controller.Instance, Door>
	{
		// Token: 0x06008BF5 RID: 35829 RVA: 0x003384B4 File Offset: 0x003366B4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.closed;
			this.root.Update("RefreshIsBlocked", delegate(Door.Controller.Instance smi, float dt)
			{
				smi.RefreshIsBlocked();
			}, UpdateRate.SIM_200ms, false).ParamTransition<bool>(this.isSealed, this.Sealed.closed, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue);
			this.closeblocked.PlayAnim("open").ParamTransition<bool>(this.isOpen, this.open, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isBlocked, this.closedelay, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse);
			this.closedelay.PlayAnim("open").ScheduleGoTo(0.5f, this.closing).ParamTransition<bool>(this.isOpen, this.open, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isBlocked, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue);
			this.closing.ParamTransition<bool>(this.isBlocked, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ToggleTag(GameTags.Transition).ToggleLoopingSound("Closing loop", (Door.Controller.Instance smi) => smi.master.doorClosingSound, (Door.Controller.Instance smi) => !string.IsNullOrEmpty(smi.master.doorClosingSound)).Enter("SetParams", delegate(Door.Controller.Instance smi)
			{
				smi.master.UpdateAnimAndSoundParams(smi.master.on);
			}).Update(delegate(Door.Controller.Instance smi, float dt)
			{
				if (smi.master.doorClosingSound != null)
				{
					smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorClosingSound, Door.SOUND_PROGRESS_PARAMETER, smi.Get<KBatchedAnimController>().GetPositionPercent());
				}
			}, UpdateRate.SIM_33ms, false).Enter("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(true);
			}).Exit("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(false);
			}).PlayAnim("closing").OnAnimQueueComplete(this.closed);
			this.open.PlayAnim("open").ParamTransition<bool>(this.isOpen, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse).Enter("SetWorldStateOpen", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.closed.PlayAnim("closed").ParamTransition<bool>(this.isOpen, this.opening, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isLocked, this.locking, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.locking.PlayAnim("locked_pre").OnAnimQueueComplete(this.locked).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.locked.PlayAnim("locked").ParamTransition<bool>(this.isLocked, this.unlocking, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse);
			this.unlocking.PlayAnim("locked_pst").OnAnimQueueComplete(this.closed);
			this.opening.ToggleTag(GameTags.Transition).ToggleLoopingSound("Opening loop", (Door.Controller.Instance smi) => smi.master.doorOpeningSound, (Door.Controller.Instance smi) => !string.IsNullOrEmpty(smi.master.doorOpeningSound)).Enter("SetParams", delegate(Door.Controller.Instance smi)
			{
				smi.master.UpdateAnimAndSoundParams(smi.master.on);
			}).Update(delegate(Door.Controller.Instance smi, float dt)
			{
				if (smi.master.doorOpeningSound != null)
				{
					smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorOpeningSound, Door.SOUND_PROGRESS_PARAMETER, smi.Get<KBatchedAnimController>().GetPositionPercent());
				}
			}, UpdateRate.SIM_33ms, false).Enter("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(true);
			}).Exit("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(false);
			}).PlayAnim("opening").OnAnimQueueComplete(this.open);
			this.Sealed.Enter(delegate(Door.Controller.Instance smi)
			{
				OccupyArea component = smi.master.GetComponent<OccupyArea>();
				for (int i = 0; i < component.OccupiedCellsOffsets.Length; i++)
				{
					Grid.PreventFogOfWarReveal[Grid.OffsetCell(Grid.PosToCell(smi.master.gameObject), component.OccupiedCellsOffsets[i])] = false;
				}
				smi.sm.isLocked.Set(true, smi, false);
				smi.master.controlState = Door.ControlState.Locked;
				smi.master.RefreshControlState();
				if (smi.master.GetComponent<Unsealable>().facingRight)
				{
					smi.master.GetComponent<KBatchedAnimController>().FlipX = true;
				}
			}).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			}).Exit(delegate(Door.Controller.Instance smi)
			{
				smi.sm.isLocked.Set(false, smi, false);
				smi.master.GetComponent<AccessControl>().controlEnabled = true;
				smi.master.controlState = Door.ControlState.Opened;
				smi.master.RefreshControlState();
				smi.sm.isOpen.Set(true, smi, false);
				smi.sm.isLocked.Set(false, smi, false);
				smi.sm.isSealed.Set(false, smi, false);
			});
			this.Sealed.closed.PlayAnim("sealed", KAnim.PlayMode.Once);
			this.Sealed.awaiting_unlock.ToggleChore((Door.Controller.Instance smi) => this.CreateUnsealChore(smi, true), this.Sealed.chore_pst);
			this.Sealed.chore_pst.Enter(delegate(Door.Controller.Instance smi)
			{
				smi.master.hasBeenUnsealed = true;
				if (smi.master.GetComponent<Unsealable>().unsealed)
				{
					smi.GoTo(this.opening);
					FogOfWarMask.ClearMask(Grid.CellRight(Grid.PosToCell(smi.master.gameObject)));
					FogOfWarMask.ClearMask(Grid.CellLeft(Grid.PosToCell(smi.master.gameObject)));
					return;
				}
				smi.GoTo(this.Sealed.closed);
			});
		}

		// Token: 0x06008BF6 RID: 35830 RVA: 0x003389F0 File Offset: 0x00336BF0
		private Chore CreateUnsealChore(Door.Controller.Instance smi, bool approach_right)
		{
			return new WorkChore<Unsealable>(Db.Get().ChoreTypes.Toggle, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04006ACE RID: 27342
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State open;

		// Token: 0x04006ACF RID: 27343
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State opening;

		// Token: 0x04006AD0 RID: 27344
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;

		// Token: 0x04006AD1 RID: 27345
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closing;

		// Token: 0x04006AD2 RID: 27346
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closedelay;

		// Token: 0x04006AD3 RID: 27347
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closeblocked;

		// Token: 0x04006AD4 RID: 27348
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locking;

		// Token: 0x04006AD5 RID: 27349
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locked;

		// Token: 0x04006AD6 RID: 27350
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;

		// Token: 0x04006AD7 RID: 27351
		public Door.Controller.SealedStates Sealed;

		// Token: 0x04006AD8 RID: 27352
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isOpen;

		// Token: 0x04006AD9 RID: 27353
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isLocked;

		// Token: 0x04006ADA RID: 27354
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isBlocked;

		// Token: 0x04006ADB RID: 27355
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isSealed;

		// Token: 0x04006ADC RID: 27356
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter sealDirectionRight;

		// Token: 0x020024CE RID: 9422
		public class SealedStates : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
		{
			// Token: 0x0400A35A RID: 41818
			public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;

			// Token: 0x0400A35B RID: 41819
			public Door.Controller.SealedStates.AwaitingUnlock awaiting_unlock;

			// Token: 0x0400A35C RID: 41820
			public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State chore_pst;

			// Token: 0x02003533 RID: 13619
			public class AwaitingUnlock : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
			{
				// Token: 0x0400D7A3 RID: 55203
				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State awaiting_arrival;

				// Token: 0x0400D7A4 RID: 55204
				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;
			}
		}

		// Token: 0x020024CF RID: 9423
		public new class Instance : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.GameInstance
		{
			// Token: 0x0600BB6B RID: 47979 RVA: 0x003D502B File Offset: 0x003D322B
			public Instance(Door door) : base(door)
			{
			}

			// Token: 0x0600BB6C RID: 47980 RVA: 0x003D5034 File Offset: 0x003D3234
			public void RefreshIsBlocked()
			{
				bool value = false;
				foreach (int cell in this.building.PlacementCells)
				{
					if (Grid.Objects[cell, 40] != null)
					{
						value = true;
						break;
					}
				}
				base.sm.isBlocked.Set(value, base.smi, false);
			}

			// Token: 0x0400A35D RID: 41821
			[MyCmpReq]
			public Building building;
		}
	}
}
