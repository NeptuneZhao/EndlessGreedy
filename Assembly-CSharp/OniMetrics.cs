using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B6B RID: 2923
public class OniMetrics : MonoBehaviour
{
	// Token: 0x060057CD RID: 22477 RVA: 0x001F9D54 File Offset: 0x001F7F54
	private static void EnsureMetrics()
	{
		if (OniMetrics.Metrics != null)
		{
			return;
		}
		OniMetrics.Metrics = new List<Dictionary<string, object>>(2);
		for (int i = 0; i < 2; i++)
		{
			OniMetrics.Metrics.Add(null);
		}
	}

	// Token: 0x060057CE RID: 22478 RVA: 0x001F9D8B File Offset: 0x001F7F8B
	public static void LogEvent(OniMetrics.Event eventType, string key, object data)
	{
		OniMetrics.EnsureMetrics();
		if (OniMetrics.Metrics[(int)eventType] == null)
		{
			OniMetrics.Metrics[(int)eventType] = new Dictionary<string, object>();
		}
		OniMetrics.Metrics[(int)eventType][key] = data;
	}

	// Token: 0x060057CF RID: 22479 RVA: 0x001F9DC4 File Offset: 0x001F7FC4
	public static void SendEvent(OniMetrics.Event eventType, string debugName)
	{
		if (OniMetrics.Metrics[(int)eventType] == null || OniMetrics.Metrics[(int)eventType].Count == 0)
		{
			return;
		}
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(OniMetrics.Metrics[(int)eventType], debugName);
		OniMetrics.Metrics[(int)eventType].Clear();
	}

	// Token: 0x04003963 RID: 14691
	private static List<Dictionary<string, object>> Metrics;

	// Token: 0x02001BC8 RID: 7112
	public enum Event : short
	{
		// Token: 0x040080B5 RID: 32949
		NewSave,
		// Token: 0x040080B6 RID: 32950
		EndOfCycle,
		// Token: 0x040080B7 RID: 32951
		NumEvents
	}
}
