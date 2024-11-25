using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200080D RID: 2061
public class Growing : StateMachineComponent<Growing.StatesInstance>, IGameObjectEffectDescriptor, IManageGrowingStates
{
	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x060038ED RID: 14573 RVA: 0x0013691B File Offset: 0x00134B1B
	private Crop crop
	{
		get
		{
			if (this._crop == null)
			{
				this._crop = base.GetComponent<Crop>();
			}
			return this._crop;
		}
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x00136940 File Offset: 0x00134B40
	protected override void OnPrefabInit()
	{
		Amounts amounts = base.gameObject.GetAmounts();
		this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
		this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, base.gameObject));
		this.oldAge.maxAttribute.ClearModifiers();
		this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, this.maxAge, null, false, false, true));
		base.OnPrefabInit();
		base.Subscribe<Growing>(1119167081, Growing.OnNewGameSpawnDelegate);
		base.Subscribe<Growing>(1272413801, Growing.ResetGrowthDelegate);
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x00136A0A File Offset: 0x00134C0A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.gameObject.AddTag(GameTags.GrowingPlant);
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x00136A30 File Offset: 0x00134C30
	private void OnNewGameSpawn(object data)
	{
		Prefab prefab = (Prefab)data;
		if (prefab.amounts != null)
		{
			foreach (Prefab.template_amount_value template_amount_value in prefab.amounts)
			{
				if (template_amount_value.id == this.maturity.amount.Id && template_amount_value.value == this.GetMaxMaturity())
				{
					return;
				}
			}
		}
		this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x00136AC0 File Offset: 0x00134CC0
	public void OverrideMaturityLevel(float percent)
	{
		float value = this.maturity.GetMax() * percent;
		this.maturity.SetValue(value);
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x00136AE8 File Offset: 0x00134CE8
	public bool ReachedNextHarvest()
	{
		return this.PercentOfCurrentHarvest() >= 1f;
	}

	// Token: 0x060038F3 RID: 14579 RVA: 0x00136AFA File Offset: 0x00134CFA
	public bool IsGrown()
	{
		return this.maturity.value == this.maturity.GetMax();
	}

	// Token: 0x060038F4 RID: 14580 RVA: 0x00136B14 File Offset: 0x00134D14
	public bool CanGrow()
	{
		return !this.IsGrown();
	}

	// Token: 0x060038F5 RID: 14581 RVA: 0x00136B1F File Offset: 0x00134D1F
	public bool IsGrowing()
	{
		return this.maturity.GetDelta() > 0f;
	}

	// Token: 0x060038F6 RID: 14582 RVA: 0x00136B33 File Offset: 0x00134D33
	public void ClampGrowthToHarvest()
	{
		this.maturity.value = this.maturity.GetMax();
	}

	// Token: 0x060038F7 RID: 14583 RVA: 0x00136B4B File Offset: 0x00134D4B
	public float GetMaxMaturity()
	{
		return this.maturity.GetMax();
	}

	// Token: 0x060038F8 RID: 14584 RVA: 0x00136B58 File Offset: 0x00134D58
	public float PercentOfCurrentHarvest()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x060038F9 RID: 14585 RVA: 0x00136B71 File Offset: 0x00134D71
	public float TimeUntilNextHarvest()
	{
		return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
	}

	// Token: 0x060038FA RID: 14586 RVA: 0x00136B96 File Offset: 0x00134D96
	public float DomesticGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.baseGrowingRate.Value;
	}

	// Token: 0x060038FB RID: 14587 RVA: 0x00136BB4 File Offset: 0x00134DB4
	public float WildGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.wildGrowingRate.Value;
	}

	// Token: 0x060038FC RID: 14588 RVA: 0x00136BD2 File Offset: 0x00134DD2
	public float PercentGrown()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x060038FD RID: 14589 RVA: 0x00136BEB File Offset: 0x00134DEB
	public void ResetGrowth(object data = null)
	{
		this.maturity.value = 0f;
	}

	// Token: 0x060038FE RID: 14590 RVA: 0x00136BFD File Offset: 0x00134DFD
	public float PercentOldAge()
	{
		if (!this.shouldGrowOld)
		{
			return 0f;
		}
		return this.oldAge.value / this.oldAge.GetMax();
	}

	// Token: 0x060038FF RID: 14591 RVA: 0x00136C24 File Offset: 0x00134E24
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Klei.AI.Attribute maxAttribute = Db.Get().Amounts.Maturity.maxAttribute;
		list.Add(new Descriptor(go.GetComponent<Modifiers>().GetPreModifiedAttributeDescription(maxAttribute), go.GetComponent<Modifiers>().GetPreModifiedAttributeToolTip(maxAttribute), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06003900 RID: 14592 RVA: 0x00136C70 File Offset: 0x00134E70
	public void ConsumeMass(float mass_to_consume)
	{
		float value = this.maturity.value;
		mass_to_consume = Mathf.Min(mass_to_consume, value);
		this.maturity.value = this.maturity.value - mass_to_consume;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06003901 RID: 14593 RVA: 0x00136CBC File Offset: 0x00134EBC
	public void ConsumeGrowthUnits(float units_to_consume, float unit_maturity_ratio)
	{
		float num = units_to_consume / unit_maturity_ratio;
		global::Debug.Assert(num <= this.maturity.value);
		this.maturity.value -= num;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x00136D07 File Offset: 0x00134F07
	public Crop GetGropComponent()
	{
		return base.GetComponent<Crop>();
	}

	// Token: 0x0400223F RID: 8767
	public float GROWTH_RATE = 0.0016666667f;

	// Token: 0x04002240 RID: 8768
	public float WILD_GROWTH_RATE = 0.00041666668f;

	// Token: 0x04002241 RID: 8769
	public bool shouldGrowOld = true;

	// Token: 0x04002242 RID: 8770
	public float maxAge = 2400f;

	// Token: 0x04002243 RID: 8771
	private AmountInstance maturity;

	// Token: 0x04002244 RID: 8772
	private AmountInstance oldAge;

	// Token: 0x04002245 RID: 8773
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x04002246 RID: 8774
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002247 RID: 8775
	[MyCmpReq]
	private Modifiers modifiers;

	// Token: 0x04002248 RID: 8776
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x04002249 RID: 8777
	private Crop _crop;

	// Token: 0x0400224A RID: 8778
	private static readonly EventSystem.IntraObjectHandler<Growing> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.OnNewGameSpawn(data);
	});

	// Token: 0x0400224B RID: 8779
	private static readonly EventSystem.IntraObjectHandler<Growing> ResetGrowthDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.ResetGrowth(data);
	});

	// Token: 0x02001707 RID: 5895
	public class StatesInstance : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.GameInstance
	{
		// Token: 0x06009465 RID: 37989 RVA: 0x0035C310 File Offset: 0x0035A510
		public StatesInstance(Growing master) : base(master)
		{
			this.baseGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			this.getOldRate = new AttributeModifier(master.oldAge.deltaAttribute.Id, master.shouldGrowOld ? 1f : 0f, null, false, false, true);
		}

		// Token: 0x06009466 RID: 37990 RVA: 0x0035C3B3 File Offset: 0x0035A5B3
		public bool IsGrown()
		{
			return base.master.IsGrown();
		}

		// Token: 0x06009467 RID: 37991 RVA: 0x0035C3C0 File Offset: 0x0035A5C0
		public bool ReachedNextHarvest()
		{
			return base.master.ReachedNextHarvest();
		}

		// Token: 0x06009468 RID: 37992 RVA: 0x0035C3CD File Offset: 0x0035A5CD
		public void ClampGrowthToHarvest()
		{
			base.master.ClampGrowthToHarvest();
		}

		// Token: 0x06009469 RID: 37993 RVA: 0x0035C3DA File Offset: 0x0035A5DA
		public bool IsWilting()
		{
			return base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}

		// Token: 0x0600946A RID: 37994 RVA: 0x0035C404 File Offset: 0x0035A604
		public bool IsSleeping()
		{
			CropSleepingMonitor.Instance smi = base.master.GetSMI<CropSleepingMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x0600946B RID: 37995 RVA: 0x0035C428 File Offset: 0x0035A628
		public bool CanExitStalled()
		{
			return !this.IsWilting() && !this.IsSleeping();
		}

		// Token: 0x04007185 RID: 29061
		public AttributeModifier baseGrowingRate;

		// Token: 0x04007186 RID: 29062
		public AttributeModifier wildGrowingRate;

		// Token: 0x04007187 RID: 29063
		public AttributeModifier getOldRate;
	}

	// Token: 0x02001708 RID: 5896
	public class States : GameStateMachine<Growing.States, Growing.StatesInstance, Growing>
	{
		// Token: 0x0600946C RID: 37996 RVA: 0x0035C440 File Offset: 0x0035A640
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.growing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.growing.EventTransition(GameHashes.Wilt, this.stalled, (Growing.StatesInstance smi) => smi.IsWilting()).EventTransition(GameHashes.CropSleep, this.stalled, (Growing.StatesInstance smi) => smi.IsSleeping()).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.planted, (Growing.StatesInstance smi) => smi.master.rm.Replanted).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.wild, (Growing.StatesInstance smi) => !smi.master.rm.Replanted).EventTransition(GameHashes.PlanterStorage, this.growing.planted, (Growing.StatesInstance smi) => smi.master.rm.Replanted).EventTransition(GameHashes.PlanterStorage, this.growing.wild, (Growing.StatesInstance smi) => !smi.master.rm.Replanted).TriggerOnEnter(GameHashes.Grow, null).Update("CheckGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.ReachedNextHarvest())
				{
					smi.GoTo(this.grown);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Growing.StatesInstance smi) => smi.master.GetComponent<IManageGrowingStates>()).Enter(delegate(Growing.StatesInstance smi)
			{
				GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State state = smi.master.rm.Replanted ? this.growing.planted : this.growing.wild;
				smi.GoTo(state);
			});
			this.growing.wild.ToggleAttributeModifier("GrowingWild", (Growing.StatesInstance smi) => smi.wildGrowingRate, null);
			this.growing.planted.ToggleAttributeModifier("Growing", (Growing.StatesInstance smi) => smi.baseGrowingRate, null);
			this.stalled.EventTransition(GameHashes.WiltRecover, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled()).EventTransition(GameHashes.CropWakeUp, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled());
			this.grown.DefaultState(this.grown.idle).TriggerOnEnter(GameHashes.Grow, null).Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (!smi.ReachedNextHarvest())
				{
					smi.GoTo(this.growing);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleAttributeModifier("GettingOld", (Growing.StatesInstance smi) => smi.getOldRate, null).Enter(delegate(Growing.StatesInstance smi)
			{
				smi.ClampGrowthToHarvest();
			}).Exit(delegate(Growing.StatesInstance smi)
			{
				smi.master.oldAge.SetValue(0f);
			});
			this.grown.idle.Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.master.shouldGrowOld && smi.master.oldAge.value >= smi.master.oldAge.GetMax())
				{
					smi.GoTo(this.grown.try_self_harvest);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.grown.try_self_harvest.Enter(delegate(Growing.StatesInstance smi)
			{
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component && component.CanBeHarvested)
				{
					bool harvestWhenReady = component.harvestDesignatable.HarvestWhenReady;
					component.ForceCancelHarvest(null);
					component.Harvest();
					if (harvestWhenReady && component != null)
					{
						component.harvestDesignatable.SetHarvestWhenReady(true);
					}
				}
				smi.master.maturity.SetValue(0f);
				smi.master.oldAge.SetValue(0f);
			}).GoTo(this.grown.idle);
		}

		// Token: 0x04007188 RID: 29064
		public Growing.States.GrowingStates growing;

		// Token: 0x04007189 RID: 29065
		public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State stalled;

		// Token: 0x0400718A RID: 29066
		public Growing.States.GrownStates grown;

		// Token: 0x0200257A RID: 9594
		public class GrowingStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x0400A6D3 RID: 42707
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State wild;

			// Token: 0x0400A6D4 RID: 42708
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State planted;
		}

		// Token: 0x0200257B RID: 9595
		public class GrownStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x0400A6D5 RID: 42709
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State idle;

			// Token: 0x0400A6D6 RID: 42710
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State try_self_harvest;
		}
	}
}
