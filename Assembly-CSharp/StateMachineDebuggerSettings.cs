using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D0 RID: 1232
public class StateMachineDebuggerSettings : ScriptableObject
{
	// Token: 0x06001A91 RID: 6801 RVA: 0x0008BF35 File Offset: 0x0008A135
	public IEnumerator<StateMachineDebuggerSettings.Entry> GetEnumerator()
	{
		return this.entries.GetEnumerator();
	}

	// Token: 0x06001A92 RID: 6802 RVA: 0x0008BF47 File Offset: 0x0008A147
	public static StateMachineDebuggerSettings Get()
	{
		if (StateMachineDebuggerSettings._Instance == null)
		{
			StateMachineDebuggerSettings._Instance = Resources.Load<StateMachineDebuggerSettings>("StateMachineDebuggerSettings");
			StateMachineDebuggerSettings._Instance.Initialize();
		}
		return StateMachineDebuggerSettings._Instance;
	}

	// Token: 0x06001A93 RID: 6803 RVA: 0x0008BF74 File Offset: 0x0008A174
	private void Initialize()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (typeof(StateMachine).IsAssignableFrom(type))
			{
				this.CreateEntry(type);
			}
		}
		this.entries.RemoveAll((StateMachineDebuggerSettings.Entry x) => x.type == null);
	}

	// Token: 0x06001A94 RID: 6804 RVA: 0x0008C004 File Offset: 0x0008A204
	public StateMachineDebuggerSettings.Entry CreateEntry(Type type)
	{
		foreach (StateMachineDebuggerSettings.Entry entry in this.entries)
		{
			if (type.FullName == entry.typeName)
			{
				entry.type = type;
				return entry;
			}
		}
		StateMachineDebuggerSettings.Entry entry2 = new StateMachineDebuggerSettings.Entry(type);
		this.entries.Add(entry2);
		return entry2;
	}

	// Token: 0x06001A95 RID: 6805 RVA: 0x0008C084 File Offset: 0x0008A284
	public void Clear()
	{
		this.entries.Clear();
		this.Initialize();
	}

	// Token: 0x04000F12 RID: 3858
	public List<StateMachineDebuggerSettings.Entry> entries = new List<StateMachineDebuggerSettings.Entry>();

	// Token: 0x04000F13 RID: 3859
	private static StateMachineDebuggerSettings _Instance;

	// Token: 0x020012AA RID: 4778
	[Serializable]
	public class Entry
	{
		// Token: 0x060084A7 RID: 33959 RVA: 0x00323DAC File Offset: 0x00321FAC
		public Entry(Type type)
		{
			this.typeName = type.FullName;
			this.type = type;
		}

		// Token: 0x040063F2 RID: 25586
		public Type type;

		// Token: 0x040063F3 RID: 25587
		public string typeName;

		// Token: 0x040063F4 RID: 25588
		public bool breakOnGoTo;

		// Token: 0x040063F5 RID: 25589
		public bool enableConsoleLogging;

		// Token: 0x040063F6 RID: 25590
		public bool saveHistory;
	}
}
