using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x0200094D RID: 2381
public class LogicCircuitNetwork : UtilityNetwork
{
	// Token: 0x06004562 RID: 17762 RVA: 0x0018B260 File Offset: 0x00189460
	public override void AddItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			LogicWire.BitDepth maxBitDepth = logicWire.MaxBitDepth;
			List<LogicWire> list = this.wireGroups[(int)maxBitDepth];
			if (list == null)
			{
				list = new List<LogicWire>();
				this.wireGroups[(int)maxBitDepth] = list;
			}
			list.Add(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = (ILogicEventReceiver)item;
			this.receivers.Add(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Add(item3);
		}
	}

	// Token: 0x06004563 RID: 17763 RVA: 0x0018B2E0 File Offset: 0x001894E0
	public override void RemoveItem(object item)
	{
		if (item is LogicWire)
		{
			LogicWire logicWire = (LogicWire)item;
			this.wireGroups[(int)logicWire.MaxBitDepth].Remove(logicWire);
			return;
		}
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver item2 = item as ILogicEventReceiver;
			this.receivers.Remove(item2);
			return;
		}
		if (item is ILogicEventSender)
		{
			ILogicEventSender item3 = (ILogicEventSender)item;
			this.senders.Remove(item3);
		}
	}

	// Token: 0x06004564 RID: 17764 RVA: 0x0018B34A File Offset: 0x0018954A
	public override void ConnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			((ILogicEventReceiver)item).OnLogicNetworkConnectionChanged(true);
			return;
		}
		if (item is ILogicEventSender)
		{
			((ILogicEventSender)item).OnLogicNetworkConnectionChanged(true);
		}
	}

	// Token: 0x06004565 RID: 17765 RVA: 0x0018B375 File Offset: 0x00189575
	public override void DisconnectItem(object item)
	{
		if (item is ILogicEventReceiver)
		{
			ILogicEventReceiver logicEventReceiver = item as ILogicEventReceiver;
			logicEventReceiver.ReceiveLogicEvent(0);
			logicEventReceiver.OnLogicNetworkConnectionChanged(false);
			return;
		}
		if (item is ILogicEventSender)
		{
			(item as ILogicEventSender).OnLogicNetworkConnectionChanged(false);
		}
	}

	// Token: 0x06004566 RID: 17766 RVA: 0x0018B3A8 File Offset: 0x001895A8
	public override void Reset(UtilityNetworkGridNode[] grid)
	{
		this.resetting = true;
		this.previousValue = -1;
		this.outputValue = 0;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list = this.wireGroups[i];
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					LogicWire logicWire = list[j];
					if (logicWire != null)
					{
						int num = Grid.PosToCell(logicWire.transform.GetPosition());
						UtilityNetworkGridNode utilityNetworkGridNode = grid[num];
						utilityNetworkGridNode.networkIdx = -1;
						grid[num] = utilityNetworkGridNode;
					}
				}
				list.Clear();
			}
		}
		this.senders.Clear();
		this.receivers.Clear();
		this.resetting = false;
		this.RemoveOverloadedNotification();
	}

	// Token: 0x06004567 RID: 17767 RVA: 0x0018B45C File Offset: 0x0018965C
	public void UpdateLogicValue()
	{
		if (this.resetting)
		{
			return;
		}
		this.previousValue = this.outputValue;
		this.outputValue = 0;
		foreach (ILogicEventSender logicEventSender in this.senders)
		{
			logicEventSender.LogicTick();
		}
		foreach (ILogicEventSender logicEventSender2 in this.senders)
		{
			int logicValue = logicEventSender2.GetLogicValue();
			this.outputValue |= logicValue;
		}
	}

	// Token: 0x06004568 RID: 17768 RVA: 0x0018B518 File Offset: 0x00189718
	public int GetBitsUsed()
	{
		int result;
		if (this.outputValue > 1)
		{
			result = 4;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	// Token: 0x06004569 RID: 17769 RVA: 0x0018B537 File Offset: 0x00189737
	public bool IsBitActive(int bit)
	{
		return (this.OutputValue & 1 << bit) > 0;
	}

	// Token: 0x0600456A RID: 17770 RVA: 0x0018B549 File Offset: 0x00189749
	public static bool IsBitActive(int bit, int value)
	{
		return (value & 1 << bit) > 0;
	}

	// Token: 0x0600456B RID: 17771 RVA: 0x0018B556 File Offset: 0x00189756
	public static int GetBitValue(int bit, int value)
	{
		return value & 1 << bit;
	}

	// Token: 0x0600456C RID: 17772 RVA: 0x0018B560 File Offset: 0x00189760
	public void SendLogicEvents(bool force_send, int id)
	{
		if (this.resetting)
		{
			return;
		}
		if (this.outputValue != this.previousValue || force_send)
		{
			foreach (ILogicEventReceiver logicEventReceiver in this.receivers)
			{
				logicEventReceiver.ReceiveLogicEvent(this.outputValue);
			}
			if (!force_send)
			{
				this.TriggerAudio((this.previousValue >= 0) ? this.previousValue : 0, id);
			}
		}
	}

	// Token: 0x0600456D RID: 17773 RVA: 0x0018B5F0 File Offset: 0x001897F0
	private void TriggerAudio(int old_value, int id)
	{
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (old_value != this.outputValue && instance != null && !instance.IsPaused)
		{
			int num = 0;
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			List<LogicWire> list = new List<LogicWire>();
			for (int i = 0; i < 2; i++)
			{
				List<LogicWire> list2 = this.wireGroups[i];
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						num++;
						if (visibleArea.Min <= list2[j].transform.GetPosition() && list2[j].transform.GetPosition() <= visibleArea.Max)
						{
							list.Add(list2[j]);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				int index = Mathf.CeilToInt((float)(list.Count / 2));
				if (list[index] != null)
				{
					Vector3 position = list[index].transform.GetPosition();
					position.z = 0f;
					string name = "Logic_Circuit_Toggle";
					LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
					if (!LogicCircuitNetwork.logicSoundRegister.ContainsKey(id))
					{
						LogicCircuitNetwork.logicSoundRegister.Add(id, logicSoundPair);
					}
					else
					{
						logicSoundPair.playedIndex = LogicCircuitNetwork.logicSoundRegister[id].playedIndex;
						logicSoundPair.lastPlayed = LogicCircuitNetwork.logicSoundRegister[id].lastPlayed;
					}
					if (logicSoundPair.playedIndex < 2)
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
					}
					else
					{
						LogicCircuitNetwork.logicSoundRegister[id].playedIndex = 0;
						LogicCircuitNetwork.logicSoundRegister[id].lastPlayed = Time.time;
					}
					float value = (Time.time - logicSoundPair.lastPlayed) / 3f;
					EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), position, 1f);
					instance2.setParameterByName("logic_volumeModifer", value, false);
					instance2.setParameterByName("wireCount", (float)(num % 24), false);
					instance2.setParameterByName("enabled", (float)this.outputValue, false);
					KFMOD.EndOneShot(instance2);
				}
			}
		}
	}

	// Token: 0x0600456E RID: 17774 RVA: 0x0018B828 File Offset: 0x00189A28
	public void UpdateOverloadTime(float dt, int bits_used)
	{
		bool flag = false;
		List<LogicWire> list = null;
		List<LogicUtilityNetworkLink> list2 = null;
		for (int i = 0; i < 2; i++)
		{
			List<LogicWire> list3 = this.wireGroups[i];
			List<LogicUtilityNetworkLink> list4 = this.relevantBridges[i];
			float num = (float)LogicWire.GetBitDepthAsInt((LogicWire.BitDepth)i);
			if ((float)bits_used > num && ((list4 != null && list4.Count > 0) || (list3 != null && list3.Count > 0)))
			{
				flag = true;
				list = list3;
				list2 = list4;
				break;
			}
		}
		if (list != null)
		{
			list.RemoveAll((LogicWire x) => x == null);
		}
		if (list2 != null)
		{
			list2.RemoveAll((LogicUtilityNetworkLink x) => x == null);
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
						source = BUILDINGS.DAMAGESOURCES.LOGIC_CIRCUIT_OVERLOADED,
						popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LOGIC_CIRCUIT_OVERLOADED,
						takeDamageEffect = SpawnFXHashes.BuildingLogicOverload,
						fullDamageEffectName = "logic_ribbon_damage_kanim",
						statusItemID = Db.Get().BuildingStatusItems.LogicOverloaded.Id
					});
				}
				if (this.overloadedNotification == null)
				{
					this.timeOverloadNotificationDisplayed = 0f;
					this.overloadedNotification = new Notification(MISC.NOTIFICATIONS.LOGIC_CIRCUIT_OVERLOADED.NAME, NotificationType.BadMinor, null, null, true, 0f, null, null, this.targetOverloadedWire.transform, true, false, false);
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

	// Token: 0x0600456F RID: 17775 RVA: 0x0018BAA3 File Offset: 0x00189CA3
	private void RemoveOverloadedNotification()
	{
		if (this.overloadedNotification != null)
		{
			Game.Instance.FindOrAdd<Notifier>().Remove(this.overloadedNotification);
			this.overloadedNotification = null;
		}
	}

	// Token: 0x06004570 RID: 17776 RVA: 0x0018BACC File Offset: 0x00189CCC
	public void UpdateRelevantBridges(List<LogicUtilityNetworkLink>[] bridgeGroups)
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		for (int i = 0; i < bridgeGroups.Length; i++)
		{
			if (this.relevantBridges[i] != null)
			{
				this.relevantBridges[i].Clear();
			}
			for (int j = 0; j < bridgeGroups[i].Count; j++)
			{
				if (logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_one) == this || logicCircuitManager.GetNetworkForCell(bridgeGroups[i][j].cell_two) == this)
				{
					if (this.relevantBridges[i] == null)
					{
						this.relevantBridges[i] = new List<LogicUtilityNetworkLink>();
					}
					this.relevantBridges[i].Add(bridgeGroups[i][j]);
				}
			}
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06004571 RID: 17777 RVA: 0x0018BB7D File Offset: 0x00189D7D
	public int OutputValue
	{
		get
		{
			return this.outputValue;
		}
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06004572 RID: 17778 RVA: 0x0018BB88 File Offset: 0x00189D88
	public int WireCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (this.wireGroups[i] != null)
				{
					num += this.wireGroups[i].Count;
				}
			}
			return num;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06004573 RID: 17779 RVA: 0x0018BBBE File Offset: 0x00189DBE
	public ReadOnlyCollection<ILogicEventSender> Senders
	{
		get
		{
			return this.senders.AsReadOnly();
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06004574 RID: 17780 RVA: 0x0018BBCB File Offset: 0x00189DCB
	public ReadOnlyCollection<ILogicEventReceiver> Receivers
	{
		get
		{
			return this.receivers.AsReadOnly();
		}
	}

	// Token: 0x04002D3B RID: 11579
	private List<LogicWire>[] wireGroups = new List<LogicWire>[2];

	// Token: 0x04002D3C RID: 11580
	private List<LogicUtilityNetworkLink>[] relevantBridges = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x04002D3D RID: 11581
	private List<ILogicEventReceiver> receivers = new List<ILogicEventReceiver>();

	// Token: 0x04002D3E RID: 11582
	private List<ILogicEventSender> senders = new List<ILogicEventSender>();

	// Token: 0x04002D3F RID: 11583
	private int previousValue = -1;

	// Token: 0x04002D40 RID: 11584
	private int outputValue;

	// Token: 0x04002D41 RID: 11585
	private bool resetting;

	// Token: 0x04002D42 RID: 11586
	public static float logicSoundLastPlayedTime = 0f;

	// Token: 0x04002D43 RID: 11587
	private const float MIN_OVERLOAD_TIME_FOR_DAMAGE = 6f;

	// Token: 0x04002D44 RID: 11588
	private const float MIN_OVERLOAD_NOTIFICATION_DISPLAY_TIME = 5f;

	// Token: 0x04002D45 RID: 11589
	public const int VALID_LOGIC_SIGNAL_MASK = 15;

	// Token: 0x04002D46 RID: 11590
	public const int UNINITIALIZED_LOGIC_STATE = -16;

	// Token: 0x04002D47 RID: 11591
	private GameObject targetOverloadedWire;

	// Token: 0x04002D48 RID: 11592
	private float timeOverloaded;

	// Token: 0x04002D49 RID: 11593
	private float timeOverloadNotificationDisplayed;

	// Token: 0x04002D4A RID: 11594
	private Notification overloadedNotification;

	// Token: 0x04002D4B RID: 11595
	public static Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = new Dictionary<int, LogicCircuitNetwork.LogicSoundPair>();

	// Token: 0x020018B0 RID: 6320
	public class LogicSoundPair
	{
		// Token: 0x04007728 RID: 30504
		public int playedIndex;

		// Token: 0x04007729 RID: 30505
		public float lastPlayed;
	}
}
