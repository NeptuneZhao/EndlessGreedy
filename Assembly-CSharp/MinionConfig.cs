using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020002DF RID: 735
public class MinionConfig : IEntityConfig
{
	// Token: 0x06000F63 RID: 3939 RVA: 0x00058B3F File Offset: 0x00056D3F
	public static string[] GetAttributes()
	{
		return BaseMinionConfig.BaseMinionAttributes().Append(new string[]
		{
			Db.Get().Attributes.FoodExpectation.Id,
			Db.Get().Attributes.ToiletEfficiency.Id
		});
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00058B80 File Offset: 0x00056D80
	public static string[] GetAmounts()
	{
		return BaseMinionConfig.BaseMinionAmounts().Append(new string[]
		{
			Db.Get().Amounts.Bladder.Id,
			Db.Get().Amounts.Stamina.Id,
			Db.Get().Amounts.Calories.Id
		});
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x00058BE4 File Offset: 0x00056DE4
	public static AttributeModifier[] GetTraits()
	{
		return BaseMinionConfig.BaseMinionTraits(MinionConfig.MODEL).Append(new AttributeModifier[]
		{
			new AttributeModifier(Db.Get().Attributes.FoodExpectation.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.FOOD_QUALITY_EXPECTATION, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.MAX_CALORIES, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.CALORIES_BURNED_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.STAMINA_USED_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.BLADDER_INCREASE_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Attributes.ToiletEfficiency.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.TOILET_EFFICIENCY, MinionConfig.NAME, false, false, true)
		});
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00058D6A File Offset: 0x00056F6A
	public GameObject CreatePrefab()
	{
		return BaseMinionConfig.BaseMinion(MinionConfig.MODEL, MinionConfig.GetAttributes(), MinionConfig.GetAmounts(), MinionConfig.GetTraits());
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x00058D88 File Offset: 0x00056F88
	public void OnPrefabInit(GameObject go)
	{
		BaseMinionConfig.BasePrefabInit(go, MinionConfig.MODEL);
		DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL);
		Db.Get().Amounts.Bladder.Lookup(go).value = UnityEngine.Random.Range(0f, 10f);
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(go);
		amountInstance.value = (statsFor.BaseStats.HUNGRY_THRESHOLD + statsFor.BaseStats.SATISFIED_THRESHOLD) * 0.5f * amountInstance.GetMax();
		AmountInstance amountInstance2 = Db.Get().Amounts.Stamina.Lookup(go);
		amountInstance2.value = amountInstance2.GetMax();
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00058E34 File Offset: 0x00057034
	public void OnSpawn(GameObject go)
	{
		Sensors component = go.GetComponent<Sensors>();
		component.Add(new ToiletSensor(component));
		BaseMinionConfig.BaseOnSpawn(go, MinionConfig.MODEL, this.RATIONAL_AI_STATE_MACHINES);
		if (go.GetComponent<OxygenBreather>().GetGasProvider() == null)
		{
			go.GetComponent<OxygenBreather>().SetGasProvider(new GasBreatherFromWorldProvider());
		}
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x00058E80 File Offset: 0x00057080
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x00058E88 File Offset: 0x00057088
	public MinionConfig()
	{
		Func<RationalAi.Instance, StateMachine.Instance>[] array = BaseMinionConfig.BaseRationalAiStateMachines();
		Func<RationalAi.Instance, StateMachine.Instance>[] array2 = new Func<RationalAi.Instance, StateMachine.Instance>[9];
		array2[0] = ((RationalAi.Instance smi) => new BreathMonitor.Instance(smi.master));
		array2[1] = ((RationalAi.Instance smi) => new SteppedInMonitor.Instance(smi.master));
		array2[2] = ((RationalAi.Instance smi) => new Dreamer.Instance(smi.master));
		array2[3] = ((RationalAi.Instance smi) => new StaminaMonitor.Instance(smi.master));
		array2[4] = ((RationalAi.Instance smi) => new RationMonitor.Instance(smi.master));
		array2[5] = ((RationalAi.Instance smi) => new CalorieMonitor.Instance(smi.master));
		array2[6] = ((RationalAi.Instance smi) => new BladderMonitor.Instance(smi.master));
		array2[7] = ((RationalAi.Instance smi) => new HygieneMonitor.Instance(smi.master));
		array2[8] = ((RationalAi.Instance smi) => new TiredMonitor.Instance(smi.master));
		this.RATIONAL_AI_STATE_MACHINES = array.Append(array2);
		base..ctor();
	}

	// Token: 0x0400097A RID: 2426
	public static Tag MODEL = GameTags.Minions.Models.Standard;

	// Token: 0x0400097B RID: 2427
	public static string NAME = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x0400097C RID: 2428
	public static string ID = MinionConfig.MODEL.ToString();

	// Token: 0x0400097D RID: 2429
	public Func<RationalAi.Instance, StateMachine.Instance>[] RATIONAL_AI_STATE_MACHINES;
}
