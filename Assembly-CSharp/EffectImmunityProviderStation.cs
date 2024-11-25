using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200055E RID: 1374
public class EffectImmunityProviderStation<StateMachineInstanceType> : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def> where StateMachineInstanceType : EffectImmunityProviderStation<StateMachineInstanceType>.BaseInstance
{
	// Token: 0x06001FD0 RID: 8144 RVA: 0x000B2E58 File Offset: 0x000B1058
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.inactive;
		this.inactive.EventTransition(GameHashes.ActiveChanged, this.active, (StateMachineInstanceType smi) => smi.GetComponent<Operational>().IsActive);
		this.active.EventTransition(GameHashes.ActiveChanged, this.inactive, (StateMachineInstanceType smi) => !smi.GetComponent<Operational>().IsActive);
	}

	// Token: 0x040011EF RID: 4591
	public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State inactive;

	// Token: 0x040011F0 RID: 4592
	public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State active;

	// Token: 0x02001359 RID: 4953
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060086B6 RID: 34486 RVA: 0x0032992B File Offset: 0x00327B2B
		public virtual string[] DefaultAnims()
		{
			return new string[]
			{
				"",
				"",
				""
			};
		}

		// Token: 0x060086B7 RID: 34487 RVA: 0x0032994B File Offset: 0x00327B4B
		public virtual string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		// Token: 0x060086B8 RID: 34488 RVA: 0x00329952 File Offset: 0x00327B52
		public string[] GetAnimNames()
		{
			if (this.overrideAnims != null)
			{
				return this.overrideAnims;
			}
			return this.DefaultAnims();
		}

		// Token: 0x060086B9 RID: 34489 RVA: 0x00329969 File Offset: 0x00327B69
		public string GetAnimFileName(GameObject entity)
		{
			if (this.overrideFileName != null)
			{
				return this.overrideFileName(entity);
			}
			return this.DefaultAnimFileName();
		}

		// Token: 0x04006644 RID: 26180
		public Action<GameObject, StateMachineInstanceType> onEffectApplied;

		// Token: 0x04006645 RID: 26181
		public Func<GameObject, bool> specialRequirements;

		// Token: 0x04006646 RID: 26182
		public Func<GameObject, string> overrideFileName;

		// Token: 0x04006647 RID: 26183
		public string[] overrideAnims;

		// Token: 0x04006648 RID: 26184
		public CellOffset[][] range;
	}

	// Token: 0x0200135A RID: 4954
	public abstract class BaseInstance : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.GameInstance
	{
		// Token: 0x060086BB RID: 34491 RVA: 0x0032998E File Offset: 0x00327B8E
		public string GetAnimFileName(GameObject entity)
		{
			return base.def.GetAnimFileName(entity);
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060086BC RID: 34492 RVA: 0x0032999C File Offset: 0x00327B9C
		public string PreAnimName
		{
			get
			{
				return base.def.GetAnimNames()[0];
			}
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060086BD RID: 34493 RVA: 0x003299AB File Offset: 0x00327BAB
		public string LoopAnimName
		{
			get
			{
				return base.def.GetAnimNames()[1];
			}
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060086BE RID: 34494 RVA: 0x003299BA File Offset: 0x00327BBA
		public string PstAnimName
		{
			get
			{
				return base.def.GetAnimNames()[2];
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060086BF RID: 34495 RVA: 0x003299C9 File Offset: 0x00327BC9
		public bool CanBeUsed
		{
			get
			{
				return this.IsActive && (base.def.specialRequirements == null || base.def.specialRequirements(base.gameObject));
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060086C0 RID: 34496 RVA: 0x003299FA File Offset: 0x00327BFA
		protected bool IsActive
		{
			get
			{
				return base.IsInsideState(base.sm.active);
			}
		}

		// Token: 0x060086C1 RID: 34497 RVA: 0x00329A0D File Offset: 0x00327C0D
		public BaseInstance(IStateMachineTarget master, EffectImmunityProviderStation<StateMachineInstanceType>.Def def) : base(master, def)
		{
		}

		// Token: 0x060086C2 RID: 34498 RVA: 0x00329A18 File Offset: 0x00327C18
		public int GetBestAvailableCell(Navigator dupeLooking, out int _cost)
		{
			_cost = int.MaxValue;
			if (!this.CanBeUsed)
			{
				return Grid.InvalidCell;
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.InvalidCell;
			if (base.def.range != null)
			{
				for (int i = 0; i < base.def.range.GetLength(0); i++)
				{
					int num3 = int.MaxValue;
					for (int j = 0; j < base.def.range[i].Length; j++)
					{
						int num4 = Grid.OffsetCell(num, base.def.range[i][j]);
						if (dupeLooking.CanReach(num4))
						{
							int navigationCost = dupeLooking.GetNavigationCost(num4);
							if (navigationCost < num3)
							{
								num3 = navigationCost;
								num2 = num4;
							}
						}
					}
					if (num2 != Grid.InvalidCell)
					{
						_cost = num3;
						break;
					}
				}
				return num2;
			}
			if (dupeLooking.CanReach(num))
			{
				_cost = dupeLooking.GetNavigationCost(num);
				return num;
			}
			return Grid.InvalidCell;
		}

		// Token: 0x060086C3 RID: 34499 RVA: 0x00329AFC File Offset: 0x00327CFC
		public void ApplyImmunityEffect(GameObject target, bool triggerEvents = true)
		{
			Effects component = target.GetComponent<Effects>();
			if (component == null)
			{
				return;
			}
			this.ApplyImmunityEffect(component);
			if (triggerEvents)
			{
				Action<GameObject, StateMachineInstanceType> onEffectApplied = base.def.onEffectApplied;
				if (onEffectApplied == null)
				{
					return;
				}
				onEffectApplied(component.gameObject, (StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x060086C4 RID: 34500
		protected abstract void ApplyImmunityEffect(Effects target);

		// Token: 0x060086C5 RID: 34501 RVA: 0x00329B45 File Offset: 0x00327D45
		public override void StartSM()
		{
			Components.EffectImmunityProviderStations.Add(this);
			base.StartSM();
		}

		// Token: 0x060086C6 RID: 34502 RVA: 0x00329B58 File Offset: 0x00327D58
		protected override void OnCleanUp()
		{
			Components.EffectImmunityProviderStations.Remove(this);
			base.OnCleanUp();
		}
	}
}
