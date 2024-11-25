using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000B46 RID: 2886
public class ElectricalUtilityNetwork : UtilityNetwork
{
	// Token: 0x06005629 RID: 22057 RVA: 0x001ECE94 File Offset: 0x001EB094
	public override void AddItem(object item)
	{
		if (item.GetType() == typeof(Wire))
		{
			Wire wire = (Wire)item;
			Wire.WattageRating maxWattageRating = wire.MaxWattageRating;
			List<Wire> list = this.wireGroups[(int)maxWattageRating];
			if (list == null)
			{
				list = new List<Wire>();
				this.wireGroups[(int)maxWattageRating] = list;
			}
			list.Add(wire);
			this.allWires.Add(wire);
			this.timeOverloaded = Mathf.Max(this.timeOverloaded, wire.circuitOverloadTime);
		}
	}

	// Token: 0x0600562A RID: 22058 RVA: 0x001ECF0C File Offset: 0x001EB10C
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		for (int i = 0; i < 5; i++)
		{
			List<Wire> list = this.wireGroups[i];
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					Wire wire = list[j];
					if (wire != null)
					{
						wire.circuitOverloadTime = this.timeOverloaded;
						int num = Grid.PosToCell(wire.transform.GetPosition());
						UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
						utilityNetworkGridNode.networkIdx = -1;
						grid[num] = utilityNetworkGridNode;
					}
				}
				list.Clear();
			}
		}
		this.allWires.Clear();
		this.RemoveOverloadedNotification();
	}

	// Token: 0x0600562B RID: 22059 RVA: 0x001ECFA4 File Offset: 0x001EB1A4
	public void UpdateOverloadTime(float dt, float watts_used, List<WireUtilityNetworkLink>[] bridgeGroups)
	{
		bool flag = false;
		List<Wire> list = null;
		List<WireUtilityNetworkLink> list2 = null;
		for (int i = 0; i < 5; i++)
		{
			List<Wire> list3 = this.wireGroups[i];
			List<WireUtilityNetworkLink> list4 = bridgeGroups[i];
			float num = Wire.GetMaxWattageAsFloat((Wire.WattageRating)i);
			num += POWER.FLOAT_FUDGE_FACTOR;
			if (watts_used > num && ((list4 != null && list4.Count > 0) || (list3 != null && list3.Count > 0)))
			{
				flag = true;
				list = list3;
				list2 = list4;
				break;
			}
		}
		if (list != null)
		{
			list.RemoveAll((Wire x) => x == null);
		}
		if (list2 != null)
		{
			list2.RemoveAll((WireUtilityNetworkLink x) => x == null);
		}
		if (flag)
		{
			this.timeOverloaded += dt;
			if (this.timeOverloaded > 6f)
			{
				this.timeOverloaded = 0f;
				if (this.targetOverloadedWire == null)
				{
					if (list2 != null && list2.Count > 0)
					{
						int index = UnityEngine.Random.Range(0, list2.Count);
						this.targetOverloadedWire = list2[index].gameObject;
					}
					else if (list != null && list.Count > 0)
					{
						int index2 = UnityEngine.Random.Range(0, list.Count);
						this.targetOverloadedWire = list[index2].gameObject;
					}
				}
				if (this.targetOverloadedWire != null)
				{
					this.targetOverloadedWire.Trigger(-794517298, new BuildingHP.DamageSourceInfo
					{
						damage = 1,
						source = STRINGS.BUILDINGS.DAMAGESOURCES.CIRCUIT_OVERLOADED,
						popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CIRCUIT_OVERLOADED,
						takeDamageEffect = SpawnFXHashes.BuildingSpark,
						fullDamageEffectName = "spark_damage_kanim",
						statusItemID = Db.Get().BuildingStatusItems.Overloaded.Id
					});
				}
				if (this.overloadedNotification == null)
				{
					this.timeOverloadNotificationDisplayed = 0f;
					this.overloadedNotification = new Notification(MISC.NOTIFICATIONS.CIRCUIT_OVERLOADED.NAME, NotificationType.BadMinor, null, null, true, 0f, null, null, this.targetOverloadedWire.transform, true, false, false);
					GameScheduler.Instance.Schedule("Power Tutorial", 2f, delegate(object obj)
					{
						Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Power, true);
					}, null, null);
					Game.Instance.FindOrAdd<Notifier>().Add(this.overloadedNotification, "");
					return;
				}
			}
		}
		else
		{
			this.timeOverloaded = Mathf.Max(0f, this.timeOverloaded - dt * 0.95f);
			this.timeOverloadNotificationDisplayed += dt;
			if (this.timeOverloadNotificationDisplayed > 5f)
			{
				this.RemoveOverloadedNotification();
			}
		}
	}

	// Token: 0x0600562C RID: 22060 RVA: 0x001ED258 File Offset: 0x001EB458
	private void RemoveOverloadedNotification()
	{
		if (this.overloadedNotification != null)
		{
			Game.Instance.FindOrAdd<Notifier>().Remove(this.overloadedNotification);
			this.overloadedNotification = null;
		}
	}

	// Token: 0x0600562D RID: 22061 RVA: 0x001ED280 File Offset: 0x001EB480
	public float GetMaxSafeWattage()
	{
		for (int i = 0; i < this.wireGroups.Length; i++)
		{
			List<Wire> list = this.wireGroups[i];
			if (list != null && list.Count > 0)
			{
				return Wire.GetMaxWattageAsFloat((Wire.WattageRating)i);
			}
		}
		return 0f;
	}

	// Token: 0x0600562E RID: 22062 RVA: 0x001ED2C8 File Offset: 0x001EB4C8
	public override void RemoveItem(object item)
	{
		if (item.GetType() == typeof(Wire))
		{
			Wire wire = (Wire)item;
			wire.circuitOverloadTime = 0f;
			this.allWires.Remove(wire);
		}
	}

	// Token: 0x0400386B RID: 14443
	private Notification overloadedNotification;

	// Token: 0x0400386C RID: 14444
	private List<Wire>[] wireGroups = new List<Wire>[5];

	// Token: 0x0400386D RID: 14445
	public List<Wire> allWires = new List<Wire>();

	// Token: 0x0400386E RID: 14446
	private const float MIN_OVERLOAD_TIME_FOR_DAMAGE = 6f;

	// Token: 0x0400386F RID: 14447
	private const float MIN_OVERLOAD_NOTIFICATION_DISPLAY_TIME = 5f;

	// Token: 0x04003870 RID: 14448
	private GameObject targetOverloadedWire;

	// Token: 0x04003871 RID: 14449
	private float timeOverloaded;

	// Token: 0x04003872 RID: 14450
	private float timeOverloadNotificationDisplayed;
}
