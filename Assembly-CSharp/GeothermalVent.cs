using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020006E3 RID: 1763
public class GeothermalVent : StateMachineComponent<GeothermalVent.StatesInstance>, ISim200ms
{
	// Token: 0x06002CCD RID: 11469 RVA: 0x000FB9EC File Offset: 0x000F9BEC
	public bool IsQuestEntombed()
	{
		return this.progress == GeothermalVent.QuestProgress.Entombed;
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x000FB9F8 File Offset: 0x000F9BF8
	public void SetQuestComplete()
	{
		this.progress = GeothermalVent.QuestProgress.Complete;
		this.connectedToggler.showButton = true;
		base.GetComponent<InfoDescription>().description = BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + BUILDINGS.PREFABS.GEOTHERMALVENT.DESC;
		base.Trigger(-1514841199, null);
	}

	// Token: 0x06002CCF RID: 11471 RVA: 0x000FBA50 File Offset: 0x000F9C50
	public static string GenerateName()
	{
		string text = "";
		for (int i = 0; i < 2; i++)
		{
			text += "0123456789"[UnityEngine.Random.Range(0, "0123456789".Length)].ToString();
		}
		return BUILDINGS.PREFABS.GEOTHERMALVENT.NAME_FMT.Replace("{ID}", text);
	}

	// Token: 0x06002CD0 RID: 11472 RVA: 0x000FBAA8 File Offset: 0x000F9CA8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.entombVulnerable.SetStatusItem(Db.Get().BuildingStatusItems.Entombed);
		base.GetComponent<PrimaryElement>().SetElement(SimHashes.Katairite, true);
		this.emitterInfo = default(GeothermalVent.EmitterInfo);
		this.emitterInfo.cell = Grid.PosToCell(base.gameObject) + Grid.WidthInCells * 3;
		this.emitterInfo.element = default(GeothermalVent.ElementInfo);
		this.emitterInfo.simHandle = -1;
		Components.GeothermalVents.Add(base.gameObject.GetMyWorldId(), this);
		if (this.progress == GeothermalVent.QuestProgress.Uninitialized)
		{
			if (Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId()).Count == 3)
			{
				this.progress = GeothermalVent.QuestProgress.Entombed;
			}
			else
			{
				this.progress = GeothermalVent.QuestProgress.Complete;
			}
		}
		if (this.progress == GeothermalVent.QuestProgress.Complete)
		{
			this.connectedToggler.showButton = true;
		}
		else
		{
			base.GetComponent<InfoDescription>().description = BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + BUILDINGS.PREFABS.GEOTHERMALVENT.BLOCKED_DESC;
			base.Trigger(-1514841199, null);
		}
		this.massMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.BAROMETER_SYMBOLS);
		UserNameable component = base.GetComponent<UserNameable>();
		if (component.savedName == "" || component.savedName == BUILDINGS.PREFABS.GEOTHERMALVENT.NAME)
		{
			component.SetName(GeothermalVent.GenerateName());
		}
		this.SimRegister();
		base.smi.StartSM();
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x000FBC34 File Offset: 0x000F9E34
	protected void SimRegister()
	{
		this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimBlockedCallback), true));
		this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimUnblockedCallback), true));
		SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(GeothermalVent.OnSimRegisteredCallback), this, "GeothermalVentElementEmitter").index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x000FBCD8 File Offset: 0x000F9ED8
	protected void OnSimBlockedCallback()
	{
		this.overpressure = true;
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x000FBCE1 File Offset: 0x000F9EE1
	protected void OnSimUnblockedCallback()
	{
		this.overpressure = false;
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x000FBCEA File Offset: 0x000F9EEA
	protected static void OnSimRegisteredCallback(int handle, object data)
	{
		((GeothermalVent)data).OnSimRegisteredImpl(handle);
	}

	// Token: 0x06002CD5 RID: 11477 RVA: 0x000FBCF8 File Offset: 0x000F9EF8
	protected void OnSimRegisteredImpl(int handle)
	{
		global::Debug.Assert(this.emitterInfo.simHandle == -1, "?! too many handles registered");
		this.emitterInfo.simHandle = handle;
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x000FBD1E File Offset: 0x000F9F1E
	protected void SimUnregister()
	{
		if (Sim.IsValidHandle(this.emitterInfo.simHandle))
		{
			SimMessages.RemoveElementEmitter(-1, this.emitterInfo.simHandle);
		}
		this.emitterInfo.simHandle = -1;
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x000FBD4F File Offset: 0x000F9F4F
	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
		Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
		Components.GeothermalVents.Remove(base.gameObject.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x000FBD90 File Offset: 0x000F9F90
	protected void OnMassEmitted(ushort element, float mass)
	{
		bool flag = false;
		for (int i = 0; i < this.availableMaterial.Count; i++)
		{
			if (this.availableMaterial[i].elementIdx == element)
			{
				GeothermalVent.ElementInfo elementInfo = this.availableMaterial[i];
				elementInfo.mass -= mass;
				flag |= (elementInfo.mass <= 0f);
				this.availableMaterial[i] = elementInfo;
				break;
			}
		}
		if (flag)
		{
			this.RecomputeEmissions();
		}
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x000FBE10 File Offset: 0x000FA010
	public void SpawnKeepsake()
	{
		GameObject keepsakePrefab = Assets.GetPrefab("keepsake_geothermalplant");
		if (keepsakePrefab != null)
		{
			base.GetComponent<KBatchedAnimController>().Play("pooped", KAnim.PlayMode.Once, 1f, 0f);
			GameScheduler.Instance.Schedule("UncorkPoopAnim", 1.5f, delegate(object data)
			{
				this.GetComponent<KBatchedAnimController>().Play("uncork", KAnim.PlayMode.Once, 1f, 0f);
			}, null, null);
			GameScheduler.Instance.Schedule("UncorkPoopFX", 2f, delegate(object data)
			{
				Game.Instance.SpawnFX(SpawnFXHashes.MissileExplosion, this.transform.GetPosition() + Vector3.up * 3f, 0f);
			}, null, null);
			GameScheduler.Instance.Schedule("SpawnGeothermalKeepsake", 3.75f, delegate(object data)
			{
				Vector3 position = this.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
				GameObject gameObject = Util.KInstantiate(keepsakePrefab, position);
				gameObject.SetActive(true);
				new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
			}, null, null);
		}
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x000FBED9 File Offset: 0x000FA0D9
	public bool IsOverPressure()
	{
		return this.overpressure;
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x000FBEE4 File Offset: 0x000FA0E4
	protected void RecomputeEmissions()
	{
		this.availableMaterial.Sort();
		while (this.availableMaterial.Count > 0 && this.availableMaterial[this.availableMaterial.Count - 1].mass <= 0f)
		{
			this.availableMaterial.RemoveAt(this.availableMaterial.Count - 1);
		}
		int num = 0;
		using (List<GeothermalVent.ElementInfo>.Enumerator enumerator = this.availableMaterial.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.isSolid)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			int num2 = UnityEngine.Random.Range(0, this.availableMaterial.Count);
			while (this.availableMaterial[num2].isSolid)
			{
				num2 = (num2 + 1) % this.availableMaterial.Count;
			}
			this.emitterInfo.element = this.availableMaterial[num2];
			this.emitterInfo.element.diseaseCount = (int)((float)this.availableMaterial[num2].diseaseCount * this.emitterInfo.element.mass / this.availableMaterial[num2].mass);
		}
		else
		{
			this.emitterInfo.element.elementIdx = 0;
			this.emitterInfo.element.mass = 0f;
		}
		this.emitterInfo.dirty = true;
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x000FC064 File Offset: 0x000FA264
	public void addMaterial(GeothermalVent.ElementInfo info)
	{
		this.availableMaterial.Add(info);
		this.recentMass = this.MaterialAvailable();
	}

	// Token: 0x06002CDD RID: 11485 RVA: 0x000FC080 File Offset: 0x000FA280
	public bool HasMaterial()
	{
		bool flag = this.availableMaterial.Count != 0;
		if (flag != this.logicPorts.GetOutputValue("GEOTHERMAL_VENT_STATUS_PORT") > 0)
		{
			this.logicPorts.SendSignal("GEOTHERMAL_VENT_STATUS_PORT", flag ? 1 : 0);
		}
		return flag;
	}

	// Token: 0x06002CDE RID: 11486 RVA: 0x000FC0D4 File Offset: 0x000FA2D4
	public float MaterialAvailable()
	{
		float num = 0f;
		foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
		{
			num += elementInfo.mass;
		}
		return num;
	}

	// Token: 0x06002CDF RID: 11487 RVA: 0x000FC130 File Offset: 0x000FA330
	public bool IsEntombed()
	{
		return this.entombVulnerable.GetEntombed;
	}

	// Token: 0x06002CE0 RID: 11488 RVA: 0x000FC13D File Offset: 0x000FA33D
	public bool CanVent()
	{
		return !this.HasMaterial() && !this.IsEntombed();
	}

	// Token: 0x06002CE1 RID: 11489 RVA: 0x000FC152 File Offset: 0x000FA352
	public bool IsVentConnected()
	{
		return !(this.connectedToggler == null) && this.connectedToggler.IsConnected;
	}

	// Token: 0x06002CE2 RID: 11490 RVA: 0x000FC170 File Offset: 0x000FA370
	public void EmitSolidChunk()
	{
		int num = 0;
		foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
		{
			if (elementInfo.isSolid && elementInfo.mass > 0f)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		int num2 = UnityEngine.Random.Range(0, this.availableMaterial.Count);
		while (!this.availableMaterial[num2].isSolid)
		{
			num2 = (num2 + 1) % this.availableMaterial.Count;
		}
		GeothermalVent.ElementInfo elementInfo2 = this.availableMaterial[num2];
		if (ElementLoader.elements[(int)this.availableMaterial[num2].elementIdx] == null)
		{
			return;
		}
		bool flag = UnityEngine.Random.value >= 0.5f;
		float f = GeothermalVentConfig.INITIAL_DEBRIS_ANGLE.Get() * 3.1415927f / 180f;
		Vector2 normalized = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
		if (flag)
		{
			normalized.x = -normalized.x;
		}
		normalized = normalized.normalized;
		normalized * GeothermalVentConfig.INITIAL_DEBRIS_VELOCIOTY.Get();
		float num3 = Math.Min(GeothermalVentConfig.DEBRIS_MASS_KG.Get(), elementInfo2.mass);
		if (elementInfo2.mass - num3 < GeothermalVentConfig.DEBRIS_MASS_KG.min)
		{
			num3 = elementInfo2.mass;
		}
		if (num3 < 0.01f)
		{
			elementInfo2.mass = 0f;
			this.availableMaterial[num2] = elementInfo2;
			return;
		}
		int num4 = (int)((float)elementInfo2.diseaseCount * num3 / elementInfo2.mass);
		Vector3 vector = Grid.CellToPos(this.emitterInfo.cell, CellAlignment.Top, Grid.SceneLayer.BuildingFront);
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, vector, 0f);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("MiniComet"), vector);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(ElementLoader.elements[(int)elementInfo2.elementIdx].id, true);
		component.Mass = num3;
		component.Temperature = elementInfo2.temperature;
		MiniComet component2 = gameObject.GetComponent<MiniComet>();
		component2.diseaseIdx = elementInfo2.diseaseIdx;
		component2.addDiseaseCount = num4;
		gameObject.SetActive(true);
		elementInfo2.diseaseCount -= num4;
		elementInfo2.mass -= num3;
		this.availableMaterial[num2] = elementInfo2;
	}

	// Token: 0x06002CE3 RID: 11491 RVA: 0x000FC3D8 File Offset: 0x000FA5D8
	public void Sim200ms(float dt)
	{
		if (dt > 0f)
		{
			this.unsafeSim200ms(dt);
		}
	}

	// Token: 0x06002CE4 RID: 11492 RVA: 0x000FC3EC File Offset: 0x000FA5EC
	private unsafe void unsafeSim200ms(float dt)
	{
		if (Sim.IsValidHandle(this.emitterInfo.simHandle))
		{
			if (this.emitterInfo.dirty)
			{
				SimMessages.ModifyElementEmitter(this.emitterInfo.simHandle, this.emitterInfo.cell, 1, ElementLoader.elements[(int)this.emitterInfo.element.elementIdx].id, 0.2f, Math.Min(3f, this.emitterInfo.element.mass), this.emitterInfo.element.temperature, 120f, this.emitterInfo.element.diseaseIdx, this.emitterInfo.element.diseaseCount);
				this.emitterInfo.dirty = false;
			}
			int handleIndex = Sim.GetHandleIndex(this.emitterInfo.simHandle);
			Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
			if (emittedMassInfo.mass > 0f)
			{
				this.OnMassEmitted(emittedMassInfo.elemIdx, emittedMassInfo.mass);
			}
		}
		this.massMeter.SetPositionPercent(this.MaterialAvailable() / this.recentMass);
	}

	// Token: 0x06002CE5 RID: 11493 RVA: 0x000FC520 File Offset: 0x000FA720
	protected static bool HasProblem(GeothermalVent.StatesInstance smi)
	{
		return smi.master.IsEntombed() || smi.master.IsOverPressure();
	}

	// Token: 0x040019DA RID: 6618
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040019DB RID: 6619
	[MyCmpAdd]
	private ConnectionManager connectedToggler;

	// Token: 0x040019DC RID: 6620
	[MyCmpAdd]
	private EntombVulnerable entombVulnerable;

	// Token: 0x040019DD RID: 6621
	[MyCmpReq]
	private LogicPorts logicPorts;

	// Token: 0x040019DE RID: 6622
	[Serialize]
	private float recentMass = 1f;

	// Token: 0x040019DF RID: 6623
	private MeterController massMeter;

	// Token: 0x040019E0 RID: 6624
	[Serialize]
	private GeothermalVent.QuestProgress progress;

	// Token: 0x040019E1 RID: 6625
	protected GeothermalVent.EmitterInfo emitterInfo;

	// Token: 0x040019E2 RID: 6626
	[Serialize]
	protected List<GeothermalVent.ElementInfo> availableMaterial = new List<GeothermalVent.ElementInfo>();

	// Token: 0x040019E3 RID: 6627
	protected bool overpressure;

	// Token: 0x040019E4 RID: 6628
	protected int debrisEmissionCell;

	// Token: 0x040019E5 RID: 6629
	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x040019E6 RID: 6630
	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x020014FD RID: 5373
	private enum QuestProgress
	{
		// Token: 0x04006BA4 RID: 27556
		Uninitialized,
		// Token: 0x04006BA5 RID: 27557
		Entombed,
		// Token: 0x04006BA6 RID: 27558
		Complete
	}

	// Token: 0x020014FE RID: 5374
	public struct ElementInfo : IComparable
	{
		// Token: 0x06008CDC RID: 36060 RVA: 0x0033C941 File Offset: 0x0033AB41
		public int CompareTo(object obj)
		{
			return -this.mass.CompareTo(((GeothermalVent.ElementInfo)obj).mass);
		}

		// Token: 0x04006BA7 RID: 27559
		public bool isSolid;

		// Token: 0x04006BA8 RID: 27560
		public ushort elementIdx;

		// Token: 0x04006BA9 RID: 27561
		public float mass;

		// Token: 0x04006BAA RID: 27562
		public float temperature;

		// Token: 0x04006BAB RID: 27563
		public byte diseaseIdx;

		// Token: 0x04006BAC RID: 27564
		public int diseaseCount;
	}

	// Token: 0x020014FF RID: 5375
	public struct EmitterInfo
	{
		// Token: 0x04006BAD RID: 27565
		public int simHandle;

		// Token: 0x04006BAE RID: 27566
		public int cell;

		// Token: 0x04006BAF RID: 27567
		public GeothermalVent.ElementInfo element;

		// Token: 0x04006BB0 RID: 27568
		public bool dirty;
	}

	// Token: 0x02001500 RID: 5376
	public class States : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent>
	{
		// Token: 0x06008CDD RID: 36061 RVA: 0x0033C95C File Offset: 0x0033AB5C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EnterTransition(this.questEntombed, (GeothermalVent.StatesInstance smi) => smi.master.IsQuestEntombed()).EnterTransition(this.online, (GeothermalVent.StatesInstance smi) => !smi.master.IsQuestEntombed());
			this.questEntombed.PlayAnim("pooped").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentQuestBlockage, (GeothermalVent.StatesInstance smi) => smi.master).Transition(this.online, (GeothermalVent.StatesInstance smi) => smi.master.progress == GeothermalVent.QuestProgress.Complete, UpdateRate.SIM_200ms);
			this.online.PlayAnim("on", KAnim.PlayMode.Once).defaultState = this.online.identify;
			this.online.identify.EnterTransition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem)).EnterTransition(this.online.active, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && smi.master.HasMaterial()).EnterTransition(this.online.ready, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && smi.master.IsVentConnected()).EnterTransition(this.online.disconnected, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && !smi.master.IsVentConnected());
			this.online.active.defaultState = this.online.active.preVent;
			this.online.active.preVent.PlayAnim("working_pre").OnAnimQueueComplete(this.online.active.loopVent);
			this.online.active.loopVent.Enter(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Exit(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Transition(this.online.active.postVent, (GeothermalVent.StatesInstance smi) => !smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive.identify, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsVenting, (GeothermalVent.StatesInstance smi) => smi.master).Update(delegate(GeothermalVent.StatesInstance smi, float dt)
			{
				if (dt > 0f)
				{
					smi.master.RecomputeEmissions();
				}
			}, UpdateRate.SIM_4000ms, false).defaultState = this.online.active.loopVent.start;
			this.online.active.loopVent.start.PlayAnim("working1").OnAnimQueueComplete(this.online.active.loopVent.finish);
			this.online.active.loopVent.finish.Enter(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.EmitSolidChunk();
			}).PlayAnim("working2").OnAnimQueueComplete(this.online.active.loopVent.start);
			this.online.active.postVent.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.online.ready);
			this.online.ready.PlayAnim("on", KAnim.PlayMode.Once).Transition(this.online.active, (GeothermalVent.StatesInstance smi) => smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).Transition(this.online.disconnected, (GeothermalVent.StatesInstance smi) => !smi.master.IsVentConnected(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsReady, (GeothermalVent.StatesInstance smi) => smi.master);
			this.online.disconnected.PlayAnim("on", KAnim.PlayMode.Once).Transition(this.online.active, (GeothermalVent.StatesInstance smi) => smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).Transition(this.online.ready, (GeothermalVent.StatesInstance smi) => smi.master.IsVentConnected(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsDisconnected, (GeothermalVent.StatesInstance smi) => smi.master);
			this.online.inactive.PlayAnim("over_pressure", KAnim.PlayMode.Once).Transition(this.online.identify, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi), UpdateRate.SIM_200ms).defaultState = this.online.inactive.identify;
			this.online.inactive.identify.EnterTransition(this.online.inactive.entombed, (GeothermalVent.StatesInstance smi) => smi.master.IsEntombed()).EnterTransition(this.online.inactive.overpressure, (GeothermalVent.StatesInstance smi) => smi.master.IsOverPressure());
			this.online.inactive.entombed.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Entombed, null).Transition(this.online.inactive.identify, (GeothermalVent.StatesInstance smi) => !smi.master.IsEntombed(), UpdateRate.SIM_200ms);
			this.online.inactive.overpressure.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsOverpressure, null).EnterTransition(this.online.inactive.identify, (GeothermalVent.StatesInstance smi) => !smi.master.IsOverPressure());
		}

		// Token: 0x04006BB1 RID: 27569
		public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State questEntombed;

		// Token: 0x04006BB2 RID: 27570
		public GeothermalVent.States.OnlineStates online;

		// Token: 0x020024E3 RID: 9443
		public class ActiveStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x0400A3FE RID: 41982
			public GeothermalVent.States.ActiveStates.LoopStates loopVent;

			// Token: 0x0400A3FF RID: 41983
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State preVent;

			// Token: 0x0400A400 RID: 41984
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State postVent;

			// Token: 0x02003536 RID: 13622
			public class LoopStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
			{
				// Token: 0x0400D7AA RID: 55210
				public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State start;

				// Token: 0x0400D7AB RID: 55211
				public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State finish;
			}
		}

		// Token: 0x020024E4 RID: 9444
		public class ProblemStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x0400A401 RID: 41985
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;

			// Token: 0x0400A402 RID: 41986
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State entombed;

			// Token: 0x0400A403 RID: 41987
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State overpressure;
		}

		// Token: 0x020024E5 RID: 9445
		public class OnlineStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x0400A404 RID: 41988
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;

			// Token: 0x0400A405 RID: 41989
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State ready;

			// Token: 0x0400A406 RID: 41990
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State disconnected;

			// Token: 0x0400A407 RID: 41991
			public GeothermalVent.States.ActiveStates active;

			// Token: 0x0400A408 RID: 41992
			public GeothermalVent.States.ProblemStates inactive;
		}
	}

	// Token: 0x02001501 RID: 5377
	public class StatesInstance : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.GameInstance
	{
		// Token: 0x06008CDF RID: 36063 RVA: 0x0033D067 File Offset: 0x0033B267
		public StatesInstance(GeothermalVent smi) : base(smi)
		{
		}
	}
}
