using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000530 RID: 1328
public class Bee : KMonoBehaviour
{
	// Token: 0x06001DEF RID: 7663 RVA: 0x000A5B1C File Offset: 0x000A3D1C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Bee>(-739654666, Bee.OnAttackDelegate);
		base.Subscribe<Bee>(-1283701846, Bee.OnSleepDelegate);
		base.Subscribe<Bee>(-2090444759, Bee.OnWakeUpDelegate);
		base.Subscribe<Bee>(1623392196, Bee.OnDeathDelegate);
		base.Subscribe<Bee>(49018834, Bee.OnSatisfiedDelegate);
		base.Subscribe<Bee>(-647798969, Bee.OnUnhappyDelegate);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("tag", false);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapto_tag", false);
		this.StopSleep();
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x000A5BC8 File Offset: 0x000A3DC8
	private void OnDeath(object data)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Storage component2 = base.GetComponent<Storage>();
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
		component2.AddOre(SimHashes.NuclearWaste, BeeTuning.WASTE_DROPPED_ON_DEATH, component.Temperature, index, BeeTuning.GERMS_DROPPED_ON_DEATH, false, true);
		component2.DropAll(base.transform.position, true, true, default(Vector3), true, null);
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x000A5C42 File Offset: 0x000A3E42
	private void StartSleep()
	{
		this.RemoveRadiationMod(this.awakeRadiationModKey);
		base.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x06001DF2 RID: 7666 RVA: 0x000A5C5C File Offset: 0x000A3E5C
	private void StopSleep()
	{
		this.AddRadiationModifier(this.awakeRadiationModKey, this.awakeRadiationMod);
		base.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x06001DF3 RID: 7667 RVA: 0x000A5C7C File Offset: 0x000A3E7C
	private void AddRadiationModifier(HashedString name, float mod)
	{
		this.radiationModifiers.Add(name, mod);
		this.RefreshRadiationOutput();
	}

	// Token: 0x06001DF4 RID: 7668 RVA: 0x000A5C91 File Offset: 0x000A3E91
	private void RemoveRadiationMod(HashedString name)
	{
		this.radiationModifiers.Remove(name);
		this.RefreshRadiationOutput();
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x000A5CA8 File Offset: 0x000A3EA8
	public void RefreshRadiationOutput()
	{
		float num = this.radiationOutputAmount;
		foreach (KeyValuePair<HashedString, float> keyValuePair in this.radiationModifiers)
		{
			num *= keyValuePair.Value;
		}
		RadiationEmitter component = base.GetComponent<RadiationEmitter>();
		component.SetEmitting(true);
		component.emitRads = num;
		component.Refresh();
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x000A5D20 File Offset: 0x000A3F20
	private void OnAttack(object data)
	{
		if ((Tag)data == GameTags.Creatures.Attack)
		{
			base.GetComponent<Health>().Damage(base.GetComponent<Health>().hitPoints);
		}
	}

	// Token: 0x06001DF7 RID: 7671 RVA: 0x000A5D4C File Offset: 0x000A3F4C
	public KPrefabID FindHiveInRoom()
	{
		List<BeeHive.StatesInstance> list = new List<BeeHive.StatesInstance>();
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		foreach (BeeHive.StatesInstance statesInstance in Components.BeeHives.Items)
		{
			if (Game.Instance.roomProber.GetRoomOfGameObject(statesInstance.gameObject) == roomOfGameObject)
			{
				list.Add(statesInstance);
			}
		}
		int num = int.MaxValue;
		KPrefabID result = null;
		foreach (BeeHive.StatesInstance statesInstance2 in list)
		{
			int navigationCost = base.gameObject.GetComponent<Navigator>().GetNavigationCost(Grid.PosToCell(statesInstance2.transform.GetLocalPosition()));
			if (navigationCost < num)
			{
				num = navigationCost;
				result = statesInstance2.GetComponent<KPrefabID>();
			}
		}
		return result;
	}

	// Token: 0x040010D0 RID: 4304
	public float radiationOutputAmount;

	// Token: 0x040010D1 RID: 4305
	private Dictionary<HashedString, float> radiationModifiers = new Dictionary<HashedString, float>();

	// Token: 0x040010D2 RID: 4306
	private float unhappyRadiationMod = 0.1f;

	// Token: 0x040010D3 RID: 4307
	private float awakeRadiationMod;

	// Token: 0x040010D4 RID: 4308
	private HashedString unhappyRadiationModKey = "UNHAPPY";

	// Token: 0x040010D5 RID: 4309
	private HashedString awakeRadiationModKey = "AWAKE";

	// Token: 0x040010D6 RID: 4310
	private static readonly EventSystem.IntraObjectHandler<Bee> OnAttackDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.OnAttack(data);
	});

	// Token: 0x040010D7 RID: 4311
	private static readonly EventSystem.IntraObjectHandler<Bee> OnSleepDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.StartSleep();
	});

	// Token: 0x040010D8 RID: 4312
	private static readonly EventSystem.IntraObjectHandler<Bee> OnWakeUpDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.StopSleep();
	});

	// Token: 0x040010D9 RID: 4313
	private static readonly EventSystem.IntraObjectHandler<Bee> OnDeathDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040010DA RID: 4314
	private static readonly EventSystem.IntraObjectHandler<Bee> OnUnhappyDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.AddRadiationModifier(component.unhappyRadiationModKey, component.unhappyRadiationMod);
	});

	// Token: 0x040010DB RID: 4315
	private static readonly EventSystem.IntraObjectHandler<Bee> OnSatisfiedDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.RemoveRadiationMod(component.unhappyRadiationModKey);
	});
}
