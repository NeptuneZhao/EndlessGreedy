using System;
using System.Collections.Generic;

// Token: 0x0200046A RID: 1130
public class ChoreTable
{
	// Token: 0x06001851 RID: 6225 RVA: 0x000820BF File Offset: 0x000802BF
	public ChoreTable(ChoreTable.Entry[] entries)
	{
		this.entries = entries;
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x000820D0 File Offset: 0x000802D0
	public ref ChoreTable.Entry GetEntry<T>()
	{
		ref ChoreTable.Entry result = ref ChoreTable.InvalidEntry;
		for (int i = 0; i < this.entries.Length; i++)
		{
			if (this.entries[i].stateMachineDef is T)
			{
				result = ref this.entries[i];
				break;
			}
		}
		return ref result;
	}

	// Token: 0x06001853 RID: 6227 RVA: 0x00082120 File Offset: 0x00080320
	public int GetChorePriority<StateMachineType>(ChoreConsumer chore_consumer)
	{
		for (int i = 0; i < this.entries.Length; i++)
		{
			ChoreTable.Entry entry = this.entries[i];
			if (entry.stateMachineDef.GetStateMachineType() == typeof(StateMachineType))
			{
				return entry.choreType.priority;
			}
		}
		Debug.LogError(chore_consumer.name + "'s chore table does not have an entry for: " + typeof(StateMachineType).Name);
		return -1;
	}

	// Token: 0x04000D80 RID: 3456
	private ChoreTable.Entry[] entries;

	// Token: 0x04000D81 RID: 3457
	public static ChoreTable.Entry InvalidEntry;

	// Token: 0x02001221 RID: 4641
	public class Builder
	{
		// Token: 0x06008244 RID: 33348 RVA: 0x0031C514 File Offset: 0x0031A714
		public ChoreTable.Builder PushInterruptGroup()
		{
			this.interruptGroupId++;
			return this;
		}

		// Token: 0x06008245 RID: 33349 RVA: 0x0031C525 File Offset: 0x0031A725
		public ChoreTable.Builder PopInterruptGroup()
		{
			DebugUtil.Assert(this.interruptGroupId > 0);
			this.interruptGroupId--;
			return this;
		}

		// Token: 0x06008246 RID: 33350 RVA: 0x0031C544 File Offset: 0x0031A744
		public ChoreTable.Builder Add(StateMachine.BaseDef def, bool condition = true, int forcePriority = -1)
		{
			if (condition)
			{
				ChoreTable.Builder.Info item = new ChoreTable.Builder.Info
				{
					interruptGroupId = this.interruptGroupId,
					forcePriority = forcePriority,
					def = def
				};
				this.infos.Add(item);
			}
			return this;
		}

		// Token: 0x06008247 RID: 33351 RVA: 0x0031C588 File Offset: 0x0031A788
		public bool HasChoreType(Type choreType)
		{
			return this.infos.Exists((ChoreTable.Builder.Info info) => info.def.GetType() == choreType);
		}

		// Token: 0x06008248 RID: 33352 RVA: 0x0031C5BC File Offset: 0x0031A7BC
		public bool TryGetChoreDef<T>(out T def) where T : StateMachine.BaseDef
		{
			for (int i = 0; i < this.infos.Count; i++)
			{
				if (this.infos[i].def != null && typeof(T).IsAssignableFrom(this.infos[i].def.GetType()))
				{
					def = (T)((object)this.infos[i].def);
					return true;
				}
			}
			def = default(T);
			return false;
		}

		// Token: 0x06008249 RID: 33353 RVA: 0x0031C640 File Offset: 0x0031A840
		public ChoreTable CreateTable()
		{
			DebugUtil.Assert(this.interruptGroupId == 0);
			ChoreTable.Entry[] array = new ChoreTable.Entry[this.infos.Count];
			Stack<int> stack = new Stack<int>();
			int num = 10000;
			for (int i = 0; i < this.infos.Count; i++)
			{
				int num2 = (this.infos[i].forcePriority != -1) ? this.infos[i].forcePriority : (num - 100);
				num = num2;
				int num3 = 10000 - i * 100;
				int num4 = this.infos[i].interruptGroupId;
				if (num4 != 0)
				{
					if (stack.Count != num4)
					{
						stack.Push(num3);
					}
					else
					{
						num3 = stack.Peek();
					}
				}
				else if (stack.Count > 0)
				{
					stack.Pop();
				}
				array[i] = new ChoreTable.Entry(this.infos[i].def, num2, num3);
			}
			return new ChoreTable(array);
		}

		// Token: 0x0400627C RID: 25212
		private int interruptGroupId;

		// Token: 0x0400627D RID: 25213
		private List<ChoreTable.Builder.Info> infos = new List<ChoreTable.Builder.Info>();

		// Token: 0x0400627E RID: 25214
		private const int INVALID_PRIORITY = -1;

		// Token: 0x020023F9 RID: 9209
		private struct Info
		{
			// Token: 0x0400A0A4 RID: 41124
			public int interruptGroupId;

			// Token: 0x0400A0A5 RID: 41125
			public int forcePriority;

			// Token: 0x0400A0A6 RID: 41126
			public StateMachine.BaseDef def;
		}
	}

	// Token: 0x02001222 RID: 4642
	public class ChoreTableChore<StateMachineType, StateMachineInstanceType> : Chore<StateMachineInstanceType> where StateMachineInstanceType : StateMachine.Instance
	{
		// Token: 0x0600824B RID: 33355 RVA: 0x0031C750 File Offset: 0x0031A950
		public ChoreTableChore(StateMachine.BaseDef state_machine_def, ChoreType chore_type, KPrefabID prefab_id) : base(chore_type, prefab_id, prefab_id.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
		{
			this.showAvailabilityInHoverText = false;
			base.smi = (state_machine_def.CreateSMI(this) as StateMachineInstanceType);
		}
	}

	// Token: 0x02001223 RID: 4643
	public struct Entry
	{
		// Token: 0x0600824C RID: 33356 RVA: 0x0031C798 File Offset: 0x0031A998
		public Entry(StateMachine.BaseDef state_machine_def, int priority, int interrupt_priority)
		{
			Type stateMachineInstanceType = Singleton<StateMachineManager>.Instance.CreateStateMachine(state_machine_def.GetStateMachineType()).GetStateMachineInstanceType();
			Type[] typeArguments = new Type[]
			{
				state_machine_def.GetStateMachineType(),
				stateMachineInstanceType
			};
			this.choreClassType = typeof(ChoreTable.ChoreTableChore<, >).MakeGenericType(typeArguments);
			this.choreType = new ChoreType(state_machine_def.ToString(), null, new string[0], "", "", "", "", new Tag[0], priority, priority);
			this.choreType.interruptPriority = interrupt_priority;
			this.stateMachineDef = state_machine_def;
		}

		// Token: 0x0400627F RID: 25215
		public Type choreClassType;

		// Token: 0x04006280 RID: 25216
		public ChoreType choreType;

		// Token: 0x04006281 RID: 25217
		public StateMachine.BaseDef stateMachineDef;
	}

	// Token: 0x02001224 RID: 4644
	public class Instance
	{
		// Token: 0x0600824D RID: 33357 RVA: 0x0031C82C File Offset: 0x0031AA2C
		public static void ResetParameters()
		{
			for (int i = 0; i < ChoreTable.Instance.parameters.Length; i++)
			{
				ChoreTable.Instance.parameters[i] = null;
			}
		}

		// Token: 0x0600824E RID: 33358 RVA: 0x0031C854 File Offset: 0x0031AA54
		public Instance(ChoreTable chore_table, KPrefabID prefab_id)
		{
			this.prefabId = prefab_id;
			this.entries = ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.Allocate();
			for (int i = 0; i < chore_table.entries.Length; i++)
			{
				this.entries.Add(new ChoreTable.Instance.Entry(chore_table.entries[i], prefab_id));
			}
		}

		// Token: 0x0600824F RID: 33359 RVA: 0x0031C8AC File Offset: 0x0031AAAC
		~Instance()
		{
			this.OnCleanUp(this.prefabId);
		}

		// Token: 0x06008250 RID: 33360 RVA: 0x0031C8E0 File Offset: 0x0031AAE0
		public void OnCleanUp(KPrefabID prefab_id)
		{
			if (this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].OnCleanUp(prefab_id);
			}
			this.entries.Recycle();
			this.entries = null;
		}

		// Token: 0x04006282 RID: 25218
		private static object[] parameters = new object[3];

		// Token: 0x04006283 RID: 25219
		private KPrefabID prefabId;

		// Token: 0x04006284 RID: 25220
		private ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.PooledList entries;

		// Token: 0x020023FB RID: 9211
		private struct Entry
		{
			// Token: 0x0600B886 RID: 47238 RVA: 0x003CF170 File Offset: 0x003CD370
			public Entry(ChoreTable.Entry chore_table_entry, KPrefabID prefab_id)
			{
				ChoreTable.Instance.parameters[0] = chore_table_entry.stateMachineDef;
				ChoreTable.Instance.parameters[1] = chore_table_entry.choreType;
				ChoreTable.Instance.parameters[2] = prefab_id;
				this.chore = (Chore)Activator.CreateInstance(chore_table_entry.choreClassType, ChoreTable.Instance.parameters);
				ChoreTable.Instance.parameters[0] = null;
				ChoreTable.Instance.parameters[1] = null;
				ChoreTable.Instance.parameters[2] = null;
			}

			// Token: 0x0600B887 RID: 47239 RVA: 0x003CF1D2 File Offset: 0x003CD3D2
			public void OnCleanUp(KPrefabID prefab_id)
			{
				if (this.chore != null)
				{
					this.chore.Cancel("ChoreTable.Instance.OnCleanUp");
					this.chore = null;
				}
			}

			// Token: 0x0400A0A8 RID: 41128
			public Chore chore;
		}
	}
}
