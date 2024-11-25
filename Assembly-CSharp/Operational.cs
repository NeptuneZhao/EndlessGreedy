using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x0200059C RID: 1436
[AddComponentMenu("KMonoBehaviour/scripts/Operational")]
public class Operational : KMonoBehaviour
{
	// Token: 0x17000174 RID: 372
	// (get) Token: 0x060021CA RID: 8650 RVA: 0x000BC3D7 File Offset: 0x000BA5D7
	// (set) Token: 0x060021CB RID: 8651 RVA: 0x000BC3DF File Offset: 0x000BA5DF
	public bool IsFunctional { get; private set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x060021CC RID: 8652 RVA: 0x000BC3E8 File Offset: 0x000BA5E8
	// (set) Token: 0x060021CD RID: 8653 RVA: 0x000BC3F0 File Offset: 0x000BA5F0
	public bool IsOperational { get; private set; }

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x060021CE RID: 8654 RVA: 0x000BC3F9 File Offset: 0x000BA5F9
	// (set) Token: 0x060021CF RID: 8655 RVA: 0x000BC401 File Offset: 0x000BA601
	public bool IsActive { get; private set; }

	// Token: 0x060021D0 RID: 8656 RVA: 0x000BC40A File Offset: 0x000BA60A
	[OnSerializing]
	private void OnSerializing()
	{
		this.AddTimeData(this.IsActive);
		this.activeStartTime = GameClock.Instance.GetTime();
		this.inactiveStartTime = GameClock.Instance.GetTime();
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x000BC438 File Offset: 0x000BA638
	protected override void OnPrefabInit()
	{
		this.UpdateFunctional();
		this.UpdateOperational();
		base.Subscribe<Operational>(-1661515756, Operational.OnNewBuildingDelegate);
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x000BC474 File Offset: 0x000BA674
	public void OnNewBuilding(object data)
	{
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.creationTime > 0f)
		{
			this.inactiveStartTime = component.creationTime;
			this.activeStartTime = component.creationTime;
		}
	}

	// Token: 0x060021D3 RID: 8659 RVA: 0x000BC4AD File Offset: 0x000BA6AD
	public bool IsOperationalType(Operational.Flag.Type type)
	{
		if (type == Operational.Flag.Type.Functional)
		{
			return this.IsFunctional;
		}
		return this.IsOperational;
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000BC4C0 File Offset: 0x000BA6C0
	public void SetFlag(Operational.Flag flag, bool value)
	{
		bool flag2 = false;
		if (this.Flags.TryGetValue(flag, out flag2))
		{
			if (flag2 != value)
			{
				this.Flags[flag] = value;
				base.Trigger(187661686, flag);
			}
		}
		else
		{
			this.Flags[flag] = value;
			base.Trigger(187661686, flag);
		}
		if (flag.FlagType == Operational.Flag.Type.Functional && value != this.IsFunctional)
		{
			this.UpdateFunctional();
		}
		if (value != this.IsOperational)
		{
			this.UpdateOperational();
		}
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x000BC540 File Offset: 0x000BA740
	public bool GetFlag(Operational.Flag flag)
	{
		bool result = false;
		this.Flags.TryGetValue(flag, out result);
		return result;
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x000BC560 File Offset: 0x000BA760
	private void UpdateFunctional()
	{
		bool isFunctional = true;
		foreach (KeyValuePair<Operational.Flag, bool> keyValuePair in this.Flags)
		{
			if (keyValuePair.Key.FlagType == Operational.Flag.Type.Functional && !keyValuePair.Value)
			{
				isFunctional = false;
				break;
			}
		}
		this.IsFunctional = isFunctional;
		base.Trigger(-1852328367, this.IsFunctional);
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x000BC5E8 File Offset: 0x000BA7E8
	private void UpdateOperational()
	{
		Dictionary<Operational.Flag, bool>.Enumerator enumerator = this.Flags.GetEnumerator();
		bool flag = true;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Operational.Flag, bool> keyValuePair = enumerator.Current;
			if (!keyValuePair.Value)
			{
				flag = false;
				break;
			}
		}
		if (flag != this.IsOperational)
		{
			this.IsOperational = flag;
			if (!this.IsOperational)
			{
				this.SetActive(false, false);
			}
			if (this.IsOperational)
			{
				base.GetComponent<KPrefabID>().AddTag(GameTags.Operational, false);
			}
			else
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Operational);
			}
			base.Trigger(-592767678, this.IsOperational);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000BC699 File Offset: 0x000BA899
	public void SetActive(bool value, bool force_ignore = false)
	{
		if (this.IsActive != value)
		{
			this.AddTimeData(value);
			base.Trigger(824508782, this);
			Game.Instance.Trigger(-809948329, base.gameObject);
		}
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000BC6CC File Offset: 0x000BA8CC
	private void AddTimeData(bool value)
	{
		float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
		float time = GameClock.Instance.GetTime();
		float num2 = time - num;
		if (this.IsActive)
		{
			this.activeTime += num2;
		}
		else
		{
			this.inactiveTime += num2;
		}
		this.IsActive = value;
		if (this.IsActive)
		{
			this.activeStartTime = time;
			return;
		}
		this.inactiveStartTime = time;
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000BC744 File Offset: 0x000BA944
	public void OnNewDay(object data)
	{
		this.AddTimeData(this.IsActive);
		this.uptimeData.Add(this.activeTime / 600f);
		while (this.uptimeData.Count > this.MAX_DATA_POINTS)
		{
			this.uptimeData.RemoveAt(0);
		}
		this.activeTime = 0f;
		this.inactiveTime = 0f;
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000BC7AC File Offset: 0x000BA9AC
	public float GetCurrentCycleUptime()
	{
		if (this.IsActive)
		{
			float num = this.IsActive ? this.activeStartTime : this.inactiveStartTime;
			float num2 = GameClock.Instance.GetTime() - num;
			return (this.activeTime + num2) / GameClock.Instance.GetTimeSinceStartOfCycle();
		}
		return this.activeTime / GameClock.Instance.GetTimeSinceStartOfCycle();
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x000BC80A File Offset: 0x000BAA0A
	public float GetLastCycleUptime()
	{
		if (this.uptimeData.Count > 0)
		{
			return this.uptimeData[this.uptimeData.Count - 1];
		}
		return 0f;
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000BC838 File Offset: 0x000BAA38
	public float GetUptimeOverCycles(int num_cycles)
	{
		if (this.uptimeData.Count > 0)
		{
			int num = Mathf.Min(this.uptimeData.Count, num_cycles);
			float num2 = 0f;
			for (int i = num - 1; i >= 0; i--)
			{
				num2 += this.uptimeData[i];
			}
			return num2 / (float)num;
		}
		return 0f;
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000BC892 File Offset: 0x000BAA92
	public bool MeetsRequirements(Operational.State stateRequirement)
	{
		switch (stateRequirement)
		{
		case Operational.State.Operational:
			return this.IsOperational;
		case Operational.State.Functional:
			return this.IsFunctional;
		case Operational.State.Active:
			return this.IsActive;
		}
		return true;
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000BC8C2 File Offset: 0x000BAAC2
	public static GameHashes GetEventForState(Operational.State state)
	{
		if (state == Operational.State.Operational)
		{
			return GameHashes.OperationalChanged;
		}
		if (state == Operational.State.Functional)
		{
			return GameHashes.FunctionalChanged;
		}
		return GameHashes.ActiveChanged;
	}

	// Token: 0x04001308 RID: 4872
	[Serialize]
	public float inactiveStartTime;

	// Token: 0x04001309 RID: 4873
	[Serialize]
	public float activeStartTime;

	// Token: 0x0400130A RID: 4874
	[Serialize]
	private List<float> uptimeData = new List<float>();

	// Token: 0x0400130B RID: 4875
	[Serialize]
	private float activeTime;

	// Token: 0x0400130C RID: 4876
	[Serialize]
	private float inactiveTime;

	// Token: 0x0400130D RID: 4877
	private int MAX_DATA_POINTS = 5;

	// Token: 0x0400130E RID: 4878
	public Dictionary<Operational.Flag, bool> Flags = new Dictionary<Operational.Flag, bool>();

	// Token: 0x0400130F RID: 4879
	private static readonly EventSystem.IntraObjectHandler<Operational> OnNewBuildingDelegate = new EventSystem.IntraObjectHandler<Operational>(delegate(Operational component, object data)
	{
		component.OnNewBuilding(data);
	});

	// Token: 0x0200138B RID: 5003
	public enum State
	{
		// Token: 0x040066F7 RID: 26359
		Operational,
		// Token: 0x040066F8 RID: 26360
		Functional,
		// Token: 0x040066F9 RID: 26361
		Active,
		// Token: 0x040066FA RID: 26362
		None
	}

	// Token: 0x0200138C RID: 5004
	public class Flag
	{
		// Token: 0x0600877A RID: 34682 RVA: 0x0032BC95 File Offset: 0x00329E95
		public Flag(string name, Operational.Flag.Type type)
		{
			this.Name = name;
			this.FlagType = type;
		}

		// Token: 0x0600877B RID: 34683 RVA: 0x0032BCAB File Offset: 0x00329EAB
		public static Operational.Flag.Type GetFlagType(Operational.State operationalState)
		{
			switch (operationalState)
			{
			case Operational.State.Operational:
			case Operational.State.Active:
				return Operational.Flag.Type.Requirement;
			case Operational.State.Functional:
				return Operational.Flag.Type.Functional;
			}
			throw new InvalidOperationException("Can not convert NONE state to an Operational Flag Type");
		}

		// Token: 0x040066FB RID: 26363
		public string Name;

		// Token: 0x040066FC RID: 26364
		public Operational.Flag.Type FlagType;

		// Token: 0x0200249C RID: 9372
		public enum Type
		{
			// Token: 0x0400A253 RID: 41555
			Requirement,
			// Token: 0x0400A254 RID: 41556
			Functional
		}
	}
}
