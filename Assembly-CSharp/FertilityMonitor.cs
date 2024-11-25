using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000808 RID: 2056
public class FertilityMonitor : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>
{
	// Token: 0x060038D5 RID: 14549 RVA: 0x0013605C File Offset: 0x0013425C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.fertile;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.DefaultState(this.fertile);
		this.fertile.ToggleBehaviour(GameTags.Creatures.Fertile, (FertilityMonitor.Instance smi) => smi.IsReadyToLayEgg(), null).ToggleEffect((FertilityMonitor.Instance smi) => smi.fertileEffect).Transition(this.infertile, GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Not(new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile)), UpdateRate.SIM_1000ms);
		this.infertile.Transition(this.fertile, new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile), UpdateRate.SIM_1000ms);
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x0013611B File Offset: 0x0013431B
	public static bool IsFertile(FertilityMonitor.Instance smi)
	{
		return !smi.HasTag(GameTags.Creatures.PausedReproduction) && !smi.HasTag(GameTags.Creatures.Confined) && !smi.HasTag(GameTags.Creatures.Expecting);
	}

	// Token: 0x060038D7 RID: 14551 RVA: 0x0013614C File Offset: 0x0013434C
	public static Tag EggBreedingRoll(List<FertilityMonitor.BreedingChance> breedingChances, bool excludeOriginalCreature = false)
	{
		float num = UnityEngine.Random.value;
		if (excludeOriginalCreature)
		{
			num *= 1f - breedingChances[0].weight;
		}
		foreach (FertilityMonitor.BreedingChance breedingChance in breedingChances)
		{
			if (excludeOriginalCreature)
			{
				excludeOriginalCreature = false;
			}
			else
			{
				num -= breedingChance.weight;
				if (num <= 0f)
				{
					return breedingChance.egg;
				}
			}
		}
		return Tag.Invalid;
	}

	// Token: 0x0400222E RID: 8750
	private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State fertile;

	// Token: 0x0400222F RID: 8751
	private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State infertile;

	// Token: 0x020016FB RID: 5883
	[Serializable]
	public class BreedingChance
	{
		// Token: 0x0400715D RID: 29021
		public Tag egg;

		// Token: 0x0400715E RID: 29022
		public float weight;
	}

	// Token: 0x020016FC RID: 5884
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06009432 RID: 37938 RVA: 0x0035B4CD File Offset: 0x003596CD
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Fertility.Id);
		}

		// Token: 0x0400715F RID: 29023
		public Tag eggPrefab;

		// Token: 0x04007160 RID: 29024
		public List<FertilityMonitor.BreedingChance> initialBreedingWeights;

		// Token: 0x04007161 RID: 29025
		public float baseFertileCycles;
	}

	// Token: 0x020016FD RID: 5885
	public new class Instance : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.GameInstance
	{
		// Token: 0x06009434 RID: 37940 RVA: 0x0035B4FC File Offset: 0x003596FC
		public Instance(IStateMachineTarget master, FertilityMonitor.Def def) : base(master, def)
		{
			this.fertility = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				this.fertility.deltaAttribute.Add(new AttributeModifier(this.fertility.deltaAttribute.Id, 33.333332f, "Accelerated Lifecycle", false, false, true));
			}
			float value = 100f / (def.baseFertileCycles * 600f);
			this.fertileEffect = new Effect("Fertile", CREATURES.MODIFIERS.BASE_FERTILITY.NAME, CREATURES.MODIFIERS.BASE_FERTILITY.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
			this.fertileEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, value, CREATURES.MODIFIERS.BASE_FERTILITY.NAME, false, false, true));
			this.InitializeBreedingChances();
		}

		// Token: 0x06009435 RID: 37941 RVA: 0x0035B5FC File Offset: 0x003597FC
		[OnDeserialized]
		private void OnDeserialized()
		{
			int num = (base.def.initialBreedingWeights != null) ? base.def.initialBreedingWeights.Count : 0;
			if (this.breedingChances.Count != num)
			{
				this.InitializeBreedingChances();
			}
		}

		// Token: 0x06009436 RID: 37942 RVA: 0x0035B640 File Offset: 0x00359840
		private void InitializeBreedingChances()
		{
			this.breedingChances = new List<FertilityMonitor.BreedingChance>();
			if (base.def.initialBreedingWeights != null)
			{
				foreach (FertilityMonitor.BreedingChance breedingChance in base.def.initialBreedingWeights)
				{
					this.breedingChances.Add(new FertilityMonitor.BreedingChance
					{
						egg = breedingChance.egg,
						weight = breedingChance.weight
					});
					foreach (FertilityModifier fertilityModifier in Db.Get().FertilityModifiers.GetForTag(breedingChance.egg))
					{
						fertilityModifier.ApplyFunction(this, breedingChance.egg);
					}
				}
				this.NormalizeBreedingChances();
			}
		}

		// Token: 0x06009437 RID: 37943 RVA: 0x0035B738 File Offset: 0x00359938
		public void ShowEgg()
		{
			if (this.egg != null)
			{
				bool flag;
				Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(FertilityMonitor.Instance.targetEggSymbol, out flag).MultiplyPoint3x4(Vector3.zero);
				if (flag)
				{
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					int num = Grid.PosToCell(vector);
					if (Grid.IsValidCell(num) && !Grid.Solid[num])
					{
						this.egg.transform.SetPosition(vector);
					}
				}
				this.egg.SetActive(true);
				Db.Get().Amounts.Wildness.Copy(this.egg, base.gameObject);
				this.egg = null;
			}
		}

		// Token: 0x06009438 RID: 37944 RVA: 0x0035B7E8 File Offset: 0x003599E8
		public void LayEgg()
		{
			this.fertility.value = 0f;
			Vector3 position = base.smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Tag tag = FertilityMonitor.EggBreedingRoll(this.breedingChances, false);
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				float num = 0f;
				foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
				{
					if (breedingChance.weight > num)
					{
						num = breedingChance.weight;
						tag = breedingChance.egg;
					}
				}
			}
			global::Debug.Assert(tag != Tag.Invalid, "Didn't pick an egg to lay. Weights weren't normalized?");
			GameObject prefab = Assets.GetPrefab(tag);
			GameObject gameObject = Util.KInstantiate(prefab, position);
			this.egg = gameObject;
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			string str = "egg01";
			CreatureBrain component2 = Assets.GetPrefab(prefab.GetDef<IncubationMonitor.Def>().spawnedCreature).GetComponent<CreatureBrain>();
			if (!string.IsNullOrEmpty(component2.symbolPrefix))
			{
				str = component2.symbolPrefix + "egg01";
			}
			KAnim.Build.Symbol symbol = this.egg.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(str);
			if (symbol != null)
			{
				component.AddSymbolOverride(FertilityMonitor.Instance.targetEggSymbol, symbol, 0);
			}
			base.Trigger(1193600993, this.egg);
		}

		// Token: 0x06009439 RID: 37945 RVA: 0x0035B960 File Offset: 0x00359B60
		public bool IsReadyToLayEgg()
		{
			return base.smi.fertility.value >= base.smi.fertility.GetMax();
		}

		// Token: 0x0600943A RID: 37946 RVA: 0x0035B988 File Offset: 0x00359B88
		public void AddBreedingChance(Tag type, float addedPercentChance)
		{
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				if (breedingChance.egg == type)
				{
					float num = Mathf.Min(1f - breedingChance.weight, Mathf.Max(0f - breedingChance.weight, addedPercentChance));
					breedingChance.weight += num;
				}
			}
			this.NormalizeBreedingChances();
			base.master.Trigger(1059811075, this.breedingChances);
		}

		// Token: 0x0600943B RID: 37947 RVA: 0x0035BA30 File Offset: 0x00359C30
		public float GetBreedingChance(Tag type)
		{
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				if (breedingChance.egg == type)
				{
					return breedingChance.weight;
				}
			}
			return -1f;
		}

		// Token: 0x0600943C RID: 37948 RVA: 0x0035BA9C File Offset: 0x00359C9C
		public void NormalizeBreedingChances()
		{
			float num = 0f;
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				num += breedingChance.weight;
			}
			foreach (FertilityMonitor.BreedingChance breedingChance2 in this.breedingChances)
			{
				breedingChance2.weight /= num;
			}
		}

		// Token: 0x0600943D RID: 37949 RVA: 0x0035BB40 File Offset: 0x00359D40
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.egg != null)
			{
				UnityEngine.Object.Destroy(this.egg);
				this.egg = null;
			}
		}

		// Token: 0x04007162 RID: 29026
		public AmountInstance fertility;

		// Token: 0x04007163 RID: 29027
		private GameObject egg;

		// Token: 0x04007164 RID: 29028
		[Serialize]
		public List<FertilityMonitor.BreedingChance> breedingChances;

		// Token: 0x04007165 RID: 29029
		public Effect fertileEffect;

		// Token: 0x04007166 RID: 29030
		private static HashedString targetEggSymbol = "snapto_egg";
	}
}
