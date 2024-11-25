using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200082A RID: 2090
[AddComponentMenu("KMonoBehaviour/scripts/WiltCondition")]
public class WiltCondition : KMonoBehaviour
{
	// Token: 0x060039CF RID: 14799 RVA: 0x0013AF55 File Offset: 0x00139155
	public bool IsWilting()
	{
		return this.wilting;
	}

	// Token: 0x060039D0 RID: 14800 RVA: 0x0013AF60 File Offset: 0x00139160
	public List<WiltCondition.Condition> CurrentWiltSources()
	{
		List<WiltCondition.Condition> list = new List<WiltCondition.Condition>();
		foreach (KeyValuePair<int, bool> keyValuePair in this.WiltConditions)
		{
			if (!keyValuePair.Value)
			{
				list.Add((WiltCondition.Condition)keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x060039D1 RID: 14801 RVA: 0x0013AFCC File Offset: 0x001391CC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.WiltConditions.Add(0, true);
		this.WiltConditions.Add(1, true);
		this.WiltConditions.Add(2, true);
		this.WiltConditions.Add(3, true);
		this.WiltConditions.Add(4, true);
		this.WiltConditions.Add(5, true);
		this.WiltConditions.Add(6, true);
		this.WiltConditions.Add(7, true);
		this.WiltConditions.Add(9, true);
		this.WiltConditions.Add(10, true);
		this.WiltConditions.Add(11, true);
		this.WiltConditions.Add(12, true);
		base.Subscribe<WiltCondition>(-107174716, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-1758196852, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-1234705021, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(-55477301, WiltCondition.SetTemperatureFalseDelegate);
		base.Subscribe<WiltCondition>(115888613, WiltCondition.SetTemperatureTrueDelegate);
		base.Subscribe<WiltCondition>(-593125877, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(-1175525437, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(-907106982, WiltCondition.SetPressureTrueDelegate);
		base.Subscribe<WiltCondition>(103243573, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(646131325, WiltCondition.SetPressureFalseDelegate);
		base.Subscribe<WiltCondition>(221594799, WiltCondition.SetAtmosphereElementFalseDelegate);
		base.Subscribe<WiltCondition>(777259436, WiltCondition.SetAtmosphereElementTrueDelegate);
		base.Subscribe<WiltCondition>(1949704522, WiltCondition.SetDrowningFalseDelegate);
		base.Subscribe<WiltCondition>(99949694, WiltCondition.SetDrowningTrueDelegate);
		base.Subscribe<WiltCondition>(-2057657673, WiltCondition.SetDryingOutFalseDelegate);
		base.Subscribe<WiltCondition>(1555379996, WiltCondition.SetDryingOutTrueDelegate);
		base.Subscribe<WiltCondition>(-370379773, WiltCondition.SetIrrigationFalseDelegate);
		base.Subscribe<WiltCondition>(207387507, WiltCondition.SetIrrigationTrueDelegate);
		base.Subscribe<WiltCondition>(-1073674739, WiltCondition.SetFertilizedFalseDelegate);
		base.Subscribe<WiltCondition>(-1396791468, WiltCondition.SetFertilizedTrueDelegate);
		base.Subscribe<WiltCondition>(1113102781, WiltCondition.SetIlluminationComfortTrueDelegate);
		base.Subscribe<WiltCondition>(1387626797, WiltCondition.SetIlluminationComfortFalseDelegate);
		base.Subscribe<WiltCondition>(1628751838, WiltCondition.SetReceptacleTrueDelegate);
		base.Subscribe<WiltCondition>(960378201, WiltCondition.SetReceptacleFalseDelegate);
		base.Subscribe<WiltCondition>(-1089732772, WiltCondition.SetEntombedDelegate);
		base.Subscribe<WiltCondition>(912965142, WiltCondition.SetRootHealthDelegate);
		base.Subscribe<WiltCondition>(874353739, WiltCondition.SetRadiationComfortTrueDelegate);
		base.Subscribe<WiltCondition>(1788072223, WiltCondition.SetRadiationComfortFalseDelegate);
	}

	// Token: 0x060039D2 RID: 14802 RVA: 0x0013B25C File Offset: 0x0013945C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.CheckShouldWilt();
		if (this.wilting)
		{
			this.DoWilt();
			if (!this.goingToWilt)
			{
				this.goingToWilt = true;
				this.Recover();
				return;
			}
		}
		else
		{
			this.DoRecover();
			if (this.goingToWilt)
			{
				this.goingToWilt = false;
				this.Wilt();
			}
		}
	}

	// Token: 0x060039D3 RID: 14803 RVA: 0x0013B2B4 File Offset: 0x001394B4
	protected override void OnCleanUp()
	{
		this.wiltSchedulerHandler.ClearScheduler();
		this.recoverSchedulerHandler.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x060039D4 RID: 14804 RVA: 0x0013B2D2 File Offset: 0x001394D2
	private void SetCondition(WiltCondition.Condition condition, bool satisfiedState)
	{
		if (!this.WiltConditions.ContainsKey((int)condition))
		{
			return;
		}
		this.WiltConditions[(int)condition] = satisfiedState;
		this.CheckShouldWilt();
	}

	// Token: 0x060039D5 RID: 14805 RVA: 0x0013B2F8 File Offset: 0x001394F8
	private void CheckShouldWilt()
	{
		bool flag = false;
		foreach (KeyValuePair<int, bool> keyValuePair in this.WiltConditions)
		{
			if (!keyValuePair.Value)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (!this.goingToWilt)
			{
				this.Wilt();
				return;
			}
		}
		else if (this.goingToWilt)
		{
			this.Recover();
		}
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x0013B374 File Offset: 0x00139574
	private void Wilt()
	{
		if (!this.goingToWilt)
		{
			this.goingToWilt = true;
			this.recoverSchedulerHandler.ClearScheduler();
			if (!this.wiltSchedulerHandler.IsValid)
			{
				this.wiltSchedulerHandler = GameScheduler.Instance.Schedule("Wilt", this.WiltDelay, new Action<object>(WiltCondition.DoWiltCallback), this, null);
			}
		}
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x0013B3D4 File Offset: 0x001395D4
	private void Recover()
	{
		if (this.goingToWilt)
		{
			this.goingToWilt = false;
			this.wiltSchedulerHandler.ClearScheduler();
			if (!this.recoverSchedulerHandler.IsValid)
			{
				this.recoverSchedulerHandler = GameScheduler.Instance.Schedule("Recover", this.RecoveryDelay, new Action<object>(WiltCondition.DoRecoverCallback), this, null);
			}
		}
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x0013B431 File Offset: 0x00139631
	private static void DoWiltCallback(object data)
	{
		((WiltCondition)data).DoWilt();
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x0013B440 File Offset: 0x00139640
	private void DoWilt()
	{
		this.wiltSchedulerHandler.ClearScheduler();
		KSelectable component = base.GetComponent<KSelectable>();
		component.GetComponent<KPrefabID>().AddTag(GameTags.Wilting, false);
		if (!this.wilting)
		{
			this.wilting = true;
			base.Trigger(-724860998, null);
		}
		if (this.rm != null)
		{
			if (this.rm.Replanted)
			{
				component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, base.GetComponent<ReceptacleMonitor>());
				return;
			}
			component.AddStatusItem(Db.Get().CreatureStatusItems.Wilting, base.GetComponent<ReceptacleMonitor>());
			return;
		}
		else
		{
			ReceptacleMonitor.StatesInstance smi = component.GetSMI<ReceptacleMonitor.StatesInstance>();
			if (smi != null && !smi.IsInsideState(smi.sm.wild))
			{
				component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, this);
				return;
			}
			component.AddStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, this);
			return;
		}
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x0013B52C File Offset: 0x0013972C
	public string WiltCausesString()
	{
		string text = "";
		List<IWiltCause> allSMI = this.GetAllSMI<IWiltCause>();
		allSMI.AddRange(base.GetComponents<IWiltCause>());
		foreach (IWiltCause wiltCause in allSMI)
		{
			foreach (WiltCondition.Condition key in wiltCause.Conditions)
			{
				if (this.WiltConditions.ContainsKey((int)key) && !this.WiltConditions[(int)key])
				{
					text += "\n";
					text += wiltCause.WiltStateString;
					break;
				}
			}
		}
		return text;
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x0013B5E4 File Offset: 0x001397E4
	private static void DoRecoverCallback(object data)
	{
		((WiltCondition)data).DoRecover();
	}

	// Token: 0x060039DC RID: 14812 RVA: 0x0013B5F4 File Offset: 0x001397F4
	private void DoRecover()
	{
		this.recoverSchedulerHandler.ClearScheduler();
		KSelectable component = base.GetComponent<KSelectable>();
		this.wilting = false;
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingDomestic, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.Wilting, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowing, false);
		component.RemoveStatusItem(Db.Get().CreatureStatusItems.WiltingNonGrowingDomestic, false);
		component.GetComponent<KPrefabID>().RemoveTag(GameTags.Wilting);
		base.Trigger(712767498, null);
	}

	// Token: 0x040022C6 RID: 8902
	[MyCmpGet]
	private ReceptacleMonitor rm;

	// Token: 0x040022C7 RID: 8903
	[Serialize]
	private bool goingToWilt;

	// Token: 0x040022C8 RID: 8904
	[Serialize]
	private bool wilting;

	// Token: 0x040022C9 RID: 8905
	private Dictionary<int, bool> WiltConditions = new Dictionary<int, bool>();

	// Token: 0x040022CA RID: 8906
	public float WiltDelay = 1f;

	// Token: 0x040022CB RID: 8907
	public float RecoveryDelay = 1f;

	// Token: 0x040022CC RID: 8908
	private SchedulerHandle wiltSchedulerHandler;

	// Token: 0x040022CD RID: 8909
	private SchedulerHandle recoverSchedulerHandler;

	// Token: 0x040022CE RID: 8910
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Temperature, false);
	});

	// Token: 0x040022CF RID: 8911
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetTemperatureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Temperature, true);
	});

	// Token: 0x040022D0 RID: 8912
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pressure, false);
	});

	// Token: 0x040022D1 RID: 8913
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetPressureTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Pressure, true);
	});

	// Token: 0x040022D2 RID: 8914
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.AtmosphereElement, false);
	});

	// Token: 0x040022D3 RID: 8915
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetAtmosphereElementTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.AtmosphereElement, true);
	});

	// Token: 0x040022D4 RID: 8916
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Drowning, false);
	});

	// Token: 0x040022D5 RID: 8917
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDrowningTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Drowning, true);
	});

	// Token: 0x040022D6 RID: 8918
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.DryingOut, false);
	});

	// Token: 0x040022D7 RID: 8919
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetDryingOutTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.DryingOut, true);
	});

	// Token: 0x040022D8 RID: 8920
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Irrigation, false);
	});

	// Token: 0x040022D9 RID: 8921
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIrrigationTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Irrigation, true);
	});

	// Token: 0x040022DA RID: 8922
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Fertilized, false);
	});

	// Token: 0x040022DB RID: 8923
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetFertilizedTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Fertilized, true);
	});

	// Token: 0x040022DC RID: 8924
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.IlluminationComfort, false);
	});

	// Token: 0x040022DD RID: 8925
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetIlluminationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.IlluminationComfort, true);
	});

	// Token: 0x040022DE RID: 8926
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Receptacle, false);
	});

	// Token: 0x040022DF RID: 8927
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetReceptacleTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Receptacle, true);
	});

	// Token: 0x040022E0 RID: 8928
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetEntombedDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Entombed, !(bool)data);
	});

	// Token: 0x040022E1 RID: 8929
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRootHealthDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.UnhealthyRoot, (bool)data);
	});

	// Token: 0x040022E2 RID: 8930
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRadiationComfortFalseDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Radiation, false);
	});

	// Token: 0x040022E3 RID: 8931
	private static readonly EventSystem.IntraObjectHandler<WiltCondition> SetRadiationComfortTrueDelegate = new EventSystem.IntraObjectHandler<WiltCondition>(delegate(WiltCondition component, object data)
	{
		component.SetCondition(WiltCondition.Condition.Radiation, true);
	});

	// Token: 0x0200174A RID: 5962
	public enum Condition
	{
		// Token: 0x04007252 RID: 29266
		Temperature,
		// Token: 0x04007253 RID: 29267
		Pressure,
		// Token: 0x04007254 RID: 29268
		AtmosphereElement,
		// Token: 0x04007255 RID: 29269
		Drowning,
		// Token: 0x04007256 RID: 29270
		Fertilized,
		// Token: 0x04007257 RID: 29271
		DryingOut,
		// Token: 0x04007258 RID: 29272
		Irrigation,
		// Token: 0x04007259 RID: 29273
		IlluminationComfort,
		// Token: 0x0400725A RID: 29274
		Darkness,
		// Token: 0x0400725B RID: 29275
		Receptacle,
		// Token: 0x0400725C RID: 29276
		Entombed,
		// Token: 0x0400725D RID: 29277
		UnhealthyRoot,
		// Token: 0x0400725E RID: 29278
		Radiation,
		// Token: 0x0400725F RID: 29279
		Count
	}
}
