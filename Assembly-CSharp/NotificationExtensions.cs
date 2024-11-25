﻿using System;
using System.Collections.Generic;

// Token: 0x02000599 RID: 1433
public static class NotificationExtensions
{
	// Token: 0x060021C2 RID: 8642 RVA: 0x000BC118 File Offset: 0x000BA318
	public static string ReduceMessages(this List<Notification> notifications, bool countNames = true)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (Notification notification in notifications)
		{
			int num = 0;
			if (!dictionary.TryGetValue(notification.NotifierName, out num))
			{
				dictionary[notification.NotifierName] = 0;
			}
			dictionary[notification.NotifierName] = num + 1;
		}
		string text = "";
		foreach (KeyValuePair<string, int> keyValuePair in dictionary)
		{
			if (countNames)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					keyValuePair.Key,
					"(",
					keyValuePair.Value.ToString(),
					")"
				});
			}
			else
			{
				text = text + "\n" + keyValuePair.Key;
			}
		}
		return text;
	}
}
