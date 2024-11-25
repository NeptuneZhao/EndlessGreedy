using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x02000C41 RID: 3137
public class EntryDevLog
{
	// Token: 0x0600606C RID: 24684 RVA: 0x0023DA64 File Offset: 0x0023BC64
	[Conditional("UNITY_EDITOR")]
	public void AddModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Add(new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x0600606D RID: 24685 RVA: 0x0023DA8C File Offset: 0x0023BC8C
	[Conditional("UNITY_EDITOR")]
	public void InsertModificationRecord(int index, EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Insert(index, new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x0600606E RID: 24686 RVA: 0x0023DAB8 File Offset: 0x0023BCB8
	private string TrimAuthor()
	{
		string text = "";
		string[] array = new string[]
		{
			"Invoke",
			"CreateInstance",
			"AwakeInternal",
			"Internal",
			"<>",
			"YamlDotNet",
			"Deserialize"
		};
		string[] array2 = new string[]
		{
			".ctor",
			"Trigger",
			"AddContentContainerRange",
			"AddContentContainer",
			"InsertContentContainer",
			"KInstantiateUI",
			"Start",
			"InitializeComponentAwake",
			"TrimAuthor",
			"InsertModificationRecord",
			"AddModificationRecord",
			"SetValue",
			"Write"
		};
		StackTrace stackTrace = new StackTrace();
		int i = 0;
		int num = 0;
		int num2 = 3;
		while (i < num2)
		{
			num++;
			if (stackTrace.FrameCount <= num)
			{
				break;
			}
			MethodBase method = stackTrace.GetFrame(num).GetMethod();
			bool flag = false;
			for (int j = 0; j < array.Length; j++)
			{
				flag = (flag || method.Name.Contains(array[j]));
			}
			for (int k = 0; k < array2.Length; k++)
			{
				flag = (flag || method.Name.Contains(array2[k]));
			}
			if (!flag && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("set_") && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("Instantiate"))
			{
				if (i != 0)
				{
					text += " < ";
				}
				i++;
				text += stackTrace.GetFrame(num).GetMethod().Name;
			}
		}
		return text;
	}

	// Token: 0x04004123 RID: 16675
	[SerializeField]
	public List<EntryDevLog.ModificationRecord> modificationRecords = new List<EntryDevLog.ModificationRecord>();

	// Token: 0x02001D24 RID: 7460
	public class ModificationRecord
	{
		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x0600A7D2 RID: 42962 RVA: 0x0039B5DB File Offset: 0x003997DB
		// (set) Token: 0x0600A7D3 RID: 42963 RVA: 0x0039B5E3 File Offset: 0x003997E3
		public EntryDevLog.ModificationRecord.ActionType actionType { get; private set; }

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x0600A7D4 RID: 42964 RVA: 0x0039B5EC File Offset: 0x003997EC
		// (set) Token: 0x0600A7D5 RID: 42965 RVA: 0x0039B5F4 File Offset: 0x003997F4
		public string target { get; private set; }

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x0600A7D6 RID: 42966 RVA: 0x0039B5FD File Offset: 0x003997FD
		// (set) Token: 0x0600A7D7 RID: 42967 RVA: 0x0039B605 File Offset: 0x00399805
		public object newValue { get; private set; }

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x0600A7D8 RID: 42968 RVA: 0x0039B60E File Offset: 0x0039980E
		// (set) Token: 0x0600A7D9 RID: 42969 RVA: 0x0039B616 File Offset: 0x00399816
		public string author { get; private set; }

		// Token: 0x0600A7DA RID: 42970 RVA: 0x0039B61F File Offset: 0x0039981F
		public ModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue, string author)
		{
			this.target = target;
			this.newValue = newValue;
			this.author = author;
			this.actionType = actionType;
		}

		// Token: 0x02002652 RID: 9810
		public enum ActionType
		{
			// Token: 0x0400AA60 RID: 43616
			Created,
			// Token: 0x0400AA61 RID: 43617
			ChangeSubEntry,
			// Token: 0x0400AA62 RID: 43618
			ChangeContent,
			// Token: 0x0400AA63 RID: 43619
			ValueChange,
			// Token: 0x0400AA64 RID: 43620
			YAMLData
		}
	}
}
