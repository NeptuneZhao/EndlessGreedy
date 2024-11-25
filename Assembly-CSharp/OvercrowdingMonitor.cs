using System;
using System.Diagnostics;
using Klei.AI;
using STRINGS;

// Token: 0x02000817 RID: 2071
public class OvercrowdingMonitor : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>
{
	// Token: 0x06003942 RID: 14658 RVA: 0x001383B4 File Offset: 0x001365B4
	[Conditional("DETAILED_OVERCROWDING_MONITOR_PROFILE")]
	private static void BeginDetailedSample(string regionName)
	{
	}

	// Token: 0x06003943 RID: 14659 RVA: 0x001383B6 File Offset: 0x001365B6
	[Conditional("DETAILED_OVERCROWDING_MONITOR_PROFILE")]
	private static void EndDetailedSample(string regionName)
	{
	}

	// Token: 0x06003944 RID: 14660 RVA: 0x001383B8 File Offset: 0x001365B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<OvercrowdingMonitor.Instance, float>(OvercrowdingMonitor.UpdateState), UpdateRate.SIM_1000ms, true);
	}

	// Token: 0x06003945 RID: 14661 RVA: 0x001383DC File Offset: 0x001365DC
	private static bool IsConfined(OvercrowdingMonitor.Instance smi)
	{
		if (smi.kpid.HasAnyTags(OvercrowdingMonitor.confinementImmunity))
		{
			return false;
		}
		if (smi.isFish)
		{
			int cell = Grid.PosToCell(smi);
			if (Grid.IsValidCell(cell) && !Grid.IsLiquid(cell))
			{
				return true;
			}
			if (smi.fishOvercrowdingMonitor.cellCount < smi.def.spaceRequiredPerCreature)
			{
				return true;
			}
		}
		else
		{
			if (smi.cavity == null)
			{
				return true;
			}
			if (smi.cavity.numCells < smi.def.spaceRequiredPerCreature)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003946 RID: 14662 RVA: 0x00138460 File Offset: 0x00136660
	private static bool IsFutureOvercrowded(OvercrowdingMonitor.Instance smi)
	{
		if (smi.cavity != null)
		{
			int num = smi.cavity.creatures.Count + smi.cavity.eggs.Count;
			return num != 0 && smi.cavity.eggs.Count != 0 && smi.cavity.numCells / num < smi.def.spaceRequiredPerCreature;
		}
		return false;
	}

	// Token: 0x06003947 RID: 14663 RVA: 0x001384CC File Offset: 0x001366CC
	private static int CalculateOvercrowdedModifer(OvercrowdingMonitor.Instance smi)
	{
		if (smi.fishOvercrowdingMonitor != null)
		{
			int fishCount = smi.fishOvercrowdingMonitor.fishCount;
			if (fishCount <= 0)
			{
				return 0;
			}
			int num = smi.fishOvercrowdingMonitor.cellCount / smi.def.spaceRequiredPerCreature;
			if (num < smi.fishOvercrowdingMonitor.fishCount)
			{
				return -(fishCount - num);
			}
			return 0;
		}
		else
		{
			if (smi.cavity == null)
			{
				return 0;
			}
			if (smi.cavity.creatures.Count <= 1)
			{
				return 0;
			}
			int num2 = smi.cavity.numCells / smi.def.spaceRequiredPerCreature;
			if (num2 < smi.cavity.creatures.Count)
			{
				return -(smi.cavity.creatures.Count - num2);
			}
			return 0;
		}
	}

	// Token: 0x06003948 RID: 14664 RVA: 0x00138580 File Offset: 0x00136780
	private static bool IsOvercrowded(OvercrowdingMonitor.Instance smi)
	{
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return false;
		}
		if (smi.fishOvercrowdingMonitor == null)
		{
			return smi.cavity != null && smi.cavity.creatures.Count > 1 && smi.cavity.numCells / smi.cavity.creatures.Count < smi.def.spaceRequiredPerCreature;
		}
		int fishCount = smi.fishOvercrowdingMonitor.fishCount;
		if (fishCount > 0)
		{
			return smi.fishOvercrowdingMonitor.cellCount / fishCount < smi.def.spaceRequiredPerCreature;
		}
		int cell = Grid.PosToCell(smi);
		return Grid.IsValidCell(cell) && !Grid.IsLiquid(cell);
	}

	// Token: 0x06003949 RID: 14665 RVA: 0x00138630 File Offset: 0x00136830
	private static void UpdateState(OvercrowdingMonitor.Instance smi, float dt)
	{
		bool flag = smi.kpid.HasTag(GameTags.Creatures.Confined);
		bool flag2 = smi.kpid.HasTag(GameTags.Creatures.Expecting);
		bool flag3 = smi.kpid.HasTag(GameTags.Creatures.Overcrowded);
		OvercrowdingMonitor.UpdateCavity(smi, dt);
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return;
		}
		bool flag4 = OvercrowdingMonitor.IsConfined(smi);
		bool flag5 = OvercrowdingMonitor.IsOvercrowded(smi);
		if (flag5)
		{
			if (!smi.isFish)
			{
				smi.overcrowdedModifier.SetValue((float)OvercrowdingMonitor.CalculateOvercrowdedModifer(smi));
			}
			else
			{
				smi.fishOvercrowdedModifier.SetValue((float)OvercrowdingMonitor.CalculateOvercrowdedModifer(smi));
			}
		}
		bool flag6 = !smi.isBaby && OvercrowdingMonitor.IsFutureOvercrowded(smi);
		if (flag != flag4 || flag2 != flag6 || flag3 != flag5)
		{
			KPrefabID kpid = smi.kpid;
			Effect effect = smi.isFish ? smi.fishOvercrowdedEffect : smi.overcrowdedEffect;
			kpid.SetTag(GameTags.Creatures.Confined, flag4);
			kpid.SetTag(GameTags.Creatures.Overcrowded, flag5);
			kpid.SetTag(GameTags.Creatures.Expecting, flag6);
			OvercrowdingMonitor.SetEffect(smi, smi.stuckEffect, flag4);
			OvercrowdingMonitor.SetEffect(smi, effect, !flag4 && flag5);
			OvercrowdingMonitor.SetEffect(smi, smi.futureOvercrowdedEffect, !flag4 && flag6);
		}
	}

	// Token: 0x0600394A RID: 14666 RVA: 0x00138759 File Offset: 0x00136959
	private static void SetEffect(OvercrowdingMonitor.Instance smi, Effect effect, bool set)
	{
		if (set)
		{
			smi.effects.Add(effect, false);
			return;
		}
		smi.effects.Remove(effect);
	}

	// Token: 0x0600394B RID: 14667 RVA: 0x0013877C File Offset: 0x0013697C
	private static void UpdateCavity(OvercrowdingMonitor.Instance smi, float dt)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi));
		if (cavityForCell != smi.cavity)
		{
			if (smi.cavity != null)
			{
				if (smi.kpid.HasTag(GameTags.Egg))
				{
					smi.cavity.RemoveFromCavity(smi.kpid, smi.cavity.eggs);
				}
				else
				{
					smi.cavity.RemoveFromCavity(smi.kpid, smi.cavity.creatures);
				}
				Game.Instance.roomProber.UpdateRoom(cavityForCell);
			}
			smi.cavity = cavityForCell;
			if (smi.cavity != null)
			{
				if (smi.kpid.HasTag(GameTags.Egg))
				{
					smi.cavity.eggs.Add(smi.kpid);
				}
				else
				{
					smi.cavity.creatures.Add(smi.kpid);
				}
				Game.Instance.roomProber.UpdateRoom(smi.cavity);
			}
		}
	}

	// Token: 0x04002275 RID: 8821
	public const float OVERCROWDED_FERTILITY_DEBUFF = -1f;

	// Token: 0x04002276 RID: 8822
	public static Tag[] confinementImmunity = new Tag[]
	{
		GameTags.Creatures.Burrowed,
		GameTags.Creatures.Digger
	};

	// Token: 0x02001722 RID: 5922
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040071D5 RID: 29141
		public int spaceRequiredPerCreature;
	}

	// Token: 0x02001723 RID: 5923
	public new class Instance : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x060094CB RID: 38091 RVA: 0x0035DA84 File Offset: 0x0035BC84
		public Instance(IStateMachineTarget master, OvercrowdingMonitor.Def def) : base(master, def)
		{
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			this.isBaby = (def2 != null);
			FishOvercrowdingMonitor.Def def3 = master.gameObject.GetDef<FishOvercrowdingMonitor.Def>();
			this.isFish = (def3 != null);
			this.futureOvercrowdedEffect = new Effect("FutureOvercrowded", CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, CREATURES.MODIFIERS.FUTURE_OVERCROWDED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.futureOvercrowdedEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, true, false, true));
			this.overcrowdedEffect = new Effect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.NAME, CREATURES.MODIFIERS.OVERCROWDED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.overcrowdedModifier = new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 0f, CREATURES.MODIFIERS.OVERCROWDED.NAME, false, false, false);
			this.overcrowdedEffect.Add(this.overcrowdedModifier);
			this.fishOvercrowdedEffect = new Effect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.NAME, CREATURES.MODIFIERS.OVERCROWDED.FISHTOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.fishOvercrowdedModifier = new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -5f, CREATURES.MODIFIERS.OVERCROWDED.NAME, false, false, false);
			this.fishOvercrowdedEffect.Add(this.fishOvercrowdedModifier);
			this.stuckEffect = new Effect("Confined", CREATURES.MODIFIERS.CONFINED.NAME, CREATURES.MODIFIERS.CONFINED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.stuckEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.CONFINED.NAME, false, false, true));
			this.stuckEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.CONFINED.NAME, true, false, true));
			OvercrowdingMonitor.UpdateState(this, 0f);
		}

		// Token: 0x060094CC RID: 38092 RVA: 0x0035DCF4 File Offset: 0x0035BEF4
		protected override void OnCleanUp()
		{
			if (this.cavity == null)
			{
				return;
			}
			if (this.kpid.HasTag(GameTags.Egg))
			{
				this.cavity.RemoveFromCavity(this.kpid, this.cavity.eggs);
				return;
			}
			this.cavity.RemoveFromCavity(this.kpid, this.cavity.creatures);
		}

		// Token: 0x060094CD RID: 38093 RVA: 0x0035DD55 File Offset: 0x0035BF55
		public void RoomRefreshUpdateCavity()
		{
			OvercrowdingMonitor.UpdateState(this, 0f);
		}

		// Token: 0x040071D6 RID: 29142
		public CavityInfo cavity;

		// Token: 0x040071D7 RID: 29143
		public bool isBaby;

		// Token: 0x040071D8 RID: 29144
		public bool isFish;

		// Token: 0x040071D9 RID: 29145
		public Effect futureOvercrowdedEffect;

		// Token: 0x040071DA RID: 29146
		public Effect overcrowdedEffect;

		// Token: 0x040071DB RID: 29147
		public AttributeModifier overcrowdedModifier;

		// Token: 0x040071DC RID: 29148
		public Effect fishOvercrowdedEffect;

		// Token: 0x040071DD RID: 29149
		public AttributeModifier fishOvercrowdedModifier;

		// Token: 0x040071DE RID: 29150
		public Effect stuckEffect;

		// Token: 0x040071DF RID: 29151
		[MyCmpReq]
		public KPrefabID kpid;

		// Token: 0x040071E0 RID: 29152
		[MyCmpReq]
		public Effects effects;

		// Token: 0x040071E1 RID: 29153
		[MySmiGet]
		public FishOvercrowdingMonitor.Instance fishOvercrowdingMonitor;
	}
}
