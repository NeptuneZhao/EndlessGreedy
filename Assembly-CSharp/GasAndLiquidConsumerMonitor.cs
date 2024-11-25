using System;
using UnityEngine;

// Token: 0x0200054F RID: 1359
public class GasAndLiquidConsumerMonitor : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>
{
	// Token: 0x06001F35 RID: 7989 RVA: 0x000AEE94 File Offset: 0x000AD094
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).ScheduleGoTo((GasAndLiquidConsumerMonitor.Instance smi) => UnityEngine.Random.Range(smi.def.minCooldown, smi.def.maxCooldown), this.satisfied);
		this.satisfied.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.looking, false);
		this.looking.ToggleBehaviour((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.behaviourTag, (GasAndLiquidConsumerMonitor.Instance smi) => smi.targetCell != -1, delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.satisfied, true).PreBrainUpdate(delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.FindElement();
		});
	}

	// Token: 0x04001193 RID: 4499
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State cooldown;

	// Token: 0x04001194 RID: 4500
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State satisfied;

	// Token: 0x04001195 RID: 4501
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State looking;

	// Token: 0x02001337 RID: 4919
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040065DE RID: 26078
		public Tag[] transitionTag = new Tag[]
		{
			GameTags.Creatures.Hungry
		};

		// Token: 0x040065DF RID: 26079
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x040065E0 RID: 26080
		public float minCooldown = 5f;

		// Token: 0x040065E1 RID: 26081
		public float maxCooldown = 5f;

		// Token: 0x040065E2 RID: 26082
		public Diet diet;

		// Token: 0x040065E3 RID: 26083
		public float consumptionRate = 0.5f;

		// Token: 0x040065E4 RID: 26084
		public Tag consumableElementTag = Tag.Invalid;
	}

	// Token: 0x02001338 RID: 4920
	public new class Instance : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.GameInstance
	{
		// Token: 0x06008637 RID: 34359 RVA: 0x003288A0 File Offset: 0x00326AA0
		public Instance(IStateMachineTarget master, GasAndLiquidConsumerMonitor.Def def) : base(master, def)
		{
			this.navigator = base.smi.GetComponent<Navigator>();
			DebugUtil.Assert(base.smi.def.diet != null || this.storage != null, "GasAndLiquidConsumerMonitor needs either a diet or a storage");
		}

		// Token: 0x06008638 RID: 34360 RVA: 0x003288F8 File Offset: 0x00326AF8
		public void ClearTargetCell()
		{
			this.targetCell = -1;
			this.massUnavailableFrameCount = 0;
		}

		// Token: 0x06008639 RID: 34361 RVA: 0x00328908 File Offset: 0x00326B08
		public void FindElement()
		{
			this.targetCell = -1;
			this.FindTargetCell();
		}

		// Token: 0x0600863A RID: 34362 RVA: 0x00328917 File Offset: 0x00326B17
		public Element GetTargetElement()
		{
			return this.targetElement;
		}

		// Token: 0x0600863B RID: 34363 RVA: 0x00328920 File Offset: 0x00326B20
		public bool IsConsumableCell(int cell, out Element element)
		{
			element = Grid.Element[cell];
			bool flag = true;
			bool flag2 = true;
			if (base.smi.def.consumableElementTag != Tag.Invalid)
			{
				flag = element.HasTag(base.smi.def.consumableElementTag);
			}
			if (base.smi.def.diet != null)
			{
				flag2 = false;
				Diet.Info[] infos = base.smi.def.diet.infos;
				for (int i = 0; i < infos.Length; i++)
				{
					if (infos[i].IsMatch(element.tag))
					{
						flag2 = true;
						break;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x0600863C RID: 34364 RVA: 0x003289C0 File Offset: 0x00326BC0
		public void FindTargetCell()
		{
			GasAndLiquidConsumerMonitor.ConsumableCellQuery consumableCellQuery = new GasAndLiquidConsumerMonitor.ConsumableCellQuery(base.smi, 25);
			this.navigator.RunQuery(consumableCellQuery);
			if (consumableCellQuery.success)
			{
				this.targetCell = consumableCellQuery.GetResultCell();
				this.targetElement = consumableCellQuery.targetElement;
			}
		}

		// Token: 0x0600863D RID: 34365 RVA: 0x00328A08 File Offset: 0x00326C08
		public void Consume(float dt)
		{
			int index = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedCallback), this, "GasAndLiquidConsumerMonitor").index;
			SimMessages.ConsumeMass(Grid.PosToCell(this), this.targetElement.id, base.def.consumptionRate * dt, 3, index);
		}

		// Token: 0x0600863E RID: 34366 RVA: 0x00328A64 File Offset: 0x00326C64
		private static void OnMassConsumedCallback(Sim.MassConsumedCallback mcd, object data)
		{
			((GasAndLiquidConsumerMonitor.Instance)data).OnMassConsumed(mcd);
		}

		// Token: 0x0600863F RID: 34367 RVA: 0x00328A74 File Offset: 0x00326C74
		private void OnMassConsumed(Sim.MassConsumedCallback mcd)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (mcd.mass > 0f)
			{
				if (base.def.diet != null)
				{
					this.massUnavailableFrameCount = 0;
					Diet.Info dietInfo = base.def.diet.GetDietInfo(this.targetElement.tag);
					if (dietInfo == null)
					{
						return;
					}
					float calories = dietInfo.ConvertConsumptionMassToCalories(mcd.mass);
					CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
					{
						tag = this.targetElement.tag,
						calories = calories
					};
					base.Trigger(-2038961714, caloriesConsumedEvent);
					return;
				}
				else if (this.storage != null)
				{
					this.storage.AddElement(this.targetElement.id, mcd.mass, mcd.temperature, mcd.diseaseIdx, mcd.diseaseCount, false, true);
					return;
				}
			}
			else
			{
				this.massUnavailableFrameCount++;
				if (this.massUnavailableFrameCount >= 2)
				{
					base.Trigger(801383139, null);
				}
			}
		}

		// Token: 0x040065E5 RID: 26085
		public int targetCell = -1;

		// Token: 0x040065E6 RID: 26086
		private Element targetElement;

		// Token: 0x040065E7 RID: 26087
		private Navigator navigator;

		// Token: 0x040065E8 RID: 26088
		private int massUnavailableFrameCount;

		// Token: 0x040065E9 RID: 26089
		[MyCmpGet]
		private Storage storage;
	}

	// Token: 0x02001339 RID: 4921
	public class ConsumableCellQuery : PathFinderQuery
	{
		// Token: 0x06008640 RID: 34368 RVA: 0x00328B74 File Offset: 0x00326D74
		public ConsumableCellQuery(GasAndLiquidConsumerMonitor.Instance smi, int maxIterations)
		{
			this.smi = smi;
			this.maxIterations = maxIterations;
		}

		// Token: 0x06008641 RID: 34369 RVA: 0x00328B8C File Offset: 0x00326D8C
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			int cell2 = Grid.CellAbove(cell);
			this.success = (this.smi.IsConsumableCell(cell, out this.targetElement) || (Grid.IsValidCell(cell2) && this.smi.IsConsumableCell(cell2, out this.targetElement)));
			if (!this.success)
			{
				int num = this.maxIterations - 1;
				this.maxIterations = num;
				return num <= 0;
			}
			return true;
		}

		// Token: 0x040065EA RID: 26090
		public bool success;

		// Token: 0x040065EB RID: 26091
		public Element targetElement;

		// Token: 0x040065EC RID: 26092
		private GasAndLiquidConsumerMonitor.Instance smi;

		// Token: 0x040065ED RID: 26093
		private int maxIterations;
	}
}
