using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000686 RID: 1670
[AddComponentMenu("KMonoBehaviour/Workable/Bed")]
public class Bed : Workable, IGameObjectEffectDescriptor, IBasicBuilding
{
	// Token: 0x0600298F RID: 10639 RVA: 0x000EA668 File Offset: 0x000E8868
	private bool CanSleepOwnablePrecondition(MinionAssignablesProxy worker)
	{
		bool result = false;
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			result = (Db.Get().Amounts.Stamina.Lookup(minionIdentity) != null);
		}
		return result;
	}

	// Token: 0x06002990 RID: 10640 RVA: 0x000EA6A6 File Offset: 0x000E88A6
	protected override void OnPrefabInit()
	{
		this.ownable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanSleepOwnablePrecondition));
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x06002991 RID: 10641 RVA: 0x000EA6CC File Offset: 0x000E88CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.BasicBuildings.Add(this);
		this.sleepable = base.GetComponent<Sleepable>();
		Sleepable sleepable = this.sleepable;
		sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
	}

	// Token: 0x06002992 RID: 10642 RVA: 0x000EA71D File Offset: 0x000E891D
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent workable_event)
	{
		if (workable_event == Workable.WorkableEvent.WorkStarted)
		{
			this.AddEffects();
			return;
		}
		if (workable_event == Workable.WorkableEvent.WorkStopped)
		{
			this.RemoveEffects();
		}
	}

	// Token: 0x06002993 RID: 10643 RVA: 0x000EA734 File Offset: 0x000E8934
	private void AddEffects()
	{
		this.targetWorker = this.sleepable.worker;
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Add(effect_id, false);
			}
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject == null)
		{
			return;
		}
		RoomType roomType = roomOfGameObject.roomType;
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			if (keyValuePair.Key == roomType.Id)
			{
				this.targetWorker.GetComponent<Effects>().Add(keyValuePair.Value, false);
			}
		}
		roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), this.targetWorker.GetComponent<Effects>());
	}

	// Token: 0x06002994 RID: 10644 RVA: 0x000EA830 File Offset: 0x000E8A30
	private void RemoveEffects()
	{
		if (this.targetWorker == null)
		{
			return;
		}
		if (this.effects != null)
		{
			foreach (string effect_id in this.effects)
			{
				this.targetWorker.GetComponent<Effects>().Remove(effect_id);
			}
		}
		foreach (KeyValuePair<string, string> keyValuePair in Bed.roomSleepingEffects)
		{
			this.targetWorker.GetComponent<Effects>().Remove(keyValuePair.Value);
		}
		this.targetWorker = null;
	}

	// Token: 0x06002995 RID: 10645 RVA: 0x000EA8DC File Offset: 0x000E8ADC
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.effects != null)
		{
			foreach (string text in this.effects)
			{
				if (text != null && text != "")
				{
					Effect.AddModifierDescriptions(base.gameObject, list, text, false);
				}
			}
		}
		return list;
	}

	// Token: 0x06002996 RID: 10646 RVA: 0x000EA930 File Offset: 0x000E8B30
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BasicBuildings.Remove(this);
		if (this.sleepable != null)
		{
			Sleepable sleepable = this.sleepable;
			sleepable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(sleepable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		}
	}

	// Token: 0x040017F7 RID: 6135
	[MyCmpReq]
	private Ownable ownable;

	// Token: 0x040017F8 RID: 6136
	[MyCmpReq]
	private Sleepable sleepable;

	// Token: 0x040017F9 RID: 6137
	private WorkerBase targetWorker;

	// Token: 0x040017FA RID: 6138
	public string[] effects;

	// Token: 0x040017FB RID: 6139
	private static Dictionary<string, string> roomSleepingEffects = new Dictionary<string, string>
	{
		{
			"Barracks",
			"BarracksStamina"
		},
		{
			"Luxury Barracks",
			"BarracksStamina"
		},
		{
			"Private Bedroom",
			"BedroomStamina"
		}
	};
}
