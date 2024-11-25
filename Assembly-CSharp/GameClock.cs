using System;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x020008BE RID: 2238
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GameClock")]
public class GameClock : KMonoBehaviour, ISaveLoadable, ISim33ms, IRender1000ms
{
	// Token: 0x06003EFF RID: 16127 RVA: 0x0015E38C File Offset: 0x0015C58C
	public static void DestroyInstance()
	{
		GameClock.Instance = null;
	}

	// Token: 0x06003F00 RID: 16128 RVA: 0x0015E394 File Offset: 0x0015C594
	protected override void OnPrefabInit()
	{
		GameClock.Instance = this;
		this.timeSinceStartOfCycle = 50f;
	}

	// Token: 0x06003F01 RID: 16129 RVA: 0x0015E3A8 File Offset: 0x0015C5A8
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.time != 0f)
		{
			this.cycle = (int)(this.time / 600f);
			this.timeSinceStartOfCycle = Mathf.Max(this.time - (float)this.cycle * 600f, 0f);
			this.time = 0f;
		}
	}

	// Token: 0x06003F02 RID: 16130 RVA: 0x0015E404 File Offset: 0x0015C604
	public void Sim33ms(float dt)
	{
		this.AddTime(dt);
	}

	// Token: 0x06003F03 RID: 16131 RVA: 0x0015E40D File Offset: 0x0015C60D
	public void Render1000ms(float dt)
	{
		this.timePlayed += dt;
	}

	// Token: 0x06003F04 RID: 16132 RVA: 0x0015E41D File Offset: 0x0015C61D
	private void LateUpdate()
	{
		this.frame++;
	}

	// Token: 0x06003F05 RID: 16133 RVA: 0x0015E430 File Offset: 0x0015C630
	private void AddTime(float dt)
	{
		this.timeSinceStartOfCycle += dt;
		bool flag = false;
		while (this.timeSinceStartOfCycle >= 600f)
		{
			this.cycle++;
			this.timeSinceStartOfCycle -= 600f;
			base.Trigger(631075836, null);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.Trigger(631075836, null);
			}
			flag = true;
		}
		if (!this.isNight && this.IsNighttime())
		{
			this.isNight = true;
			base.Trigger(-722330267, null);
		}
		if (this.isNight && !this.IsNighttime())
		{
			this.isNight = false;
		}
		if (flag && SaveGame.Instance.AutoSaveCycleInterval > 0 && this.cycle % SaveGame.Instance.AutoSaveCycleInterval == 0)
		{
			this.DoAutoSave(this.cycle);
		}
		int num = Mathf.FloorToInt(this.timeSinceStartOfCycle - dt / 25f);
		int num2 = Mathf.FloorToInt(this.timeSinceStartOfCycle / 25f);
		if (num != num2)
		{
			base.Trigger(-1215042067, num2);
		}
	}

	// Token: 0x06003F06 RID: 16134 RVA: 0x0015E574 File Offset: 0x0015C774
	public float GetTimeSinceStartOfReport()
	{
		if (this.IsNighttime())
		{
			return 525f - this.GetTimeSinceStartOfCycle();
		}
		return this.GetTimeSinceStartOfCycle() + 75f;
	}

	// Token: 0x06003F07 RID: 16135 RVA: 0x0015E597 File Offset: 0x0015C797
	public float GetTimeSinceStartOfCycle()
	{
		return this.timeSinceStartOfCycle;
	}

	// Token: 0x06003F08 RID: 16136 RVA: 0x0015E59F File Offset: 0x0015C79F
	public float GetCurrentCycleAsPercentage()
	{
		return this.timeSinceStartOfCycle / 600f;
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x0015E5AD File Offset: 0x0015C7AD
	public float GetTime()
	{
		return this.timeSinceStartOfCycle + (float)this.cycle * 600f;
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x0015E5C3 File Offset: 0x0015C7C3
	public float GetTimeInCycles()
	{
		return (float)this.cycle + this.GetCurrentCycleAsPercentage();
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x0015E5D3 File Offset: 0x0015C7D3
	public int GetFrame()
	{
		return this.frame;
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x0015E5DB File Offset: 0x0015C7DB
	public int GetCycle()
	{
		return this.cycle;
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x0015E5E3 File Offset: 0x0015C7E3
	public bool IsNighttime()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() >= 0.875f;
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x0015E5F9 File Offset: 0x0015C7F9
	public float GetDaytimeDurationInPercentage()
	{
		return 0.875f;
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x0015E600 File Offset: 0x0015C800
	public void SetTime(float new_time)
	{
		float dt = Mathf.Max(new_time - this.GetTime(), 0f);
		this.AddTime(dt);
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x0015E627 File Offset: 0x0015C827
	public float GetTimePlayedInSeconds()
	{
		return this.timePlayed;
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x0015E630 File Offset: 0x0015C830
	private void DoAutoSave(int day)
	{
		if (GenericGameSettings.instance.disableAutosave)
		{
			return;
		}
		day++;
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, GameClock.NewCycleKey, day);
		OniMetrics.SendEvent(OniMetrics.Event.EndOfCycle, "DoAutoSave");
		string text = SaveLoader.GetActiveSaveFilePath();
		if (text == null)
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		int num = text.LastIndexOf("\\");
		if (num > 0)
		{
			int num2 = text.IndexOf(" Cycle ", num);
			if (num2 > 0)
			{
				text = text.Substring(0, num2);
			}
		}
		text = Path.ChangeExtension(text, null);
		text = text + " Cycle " + day.ToString();
		text = SaveScreen.GetValidSaveFilename(text);
		text = Path.Combine(SaveLoader.GetActiveAutoSavePath(), Path.GetFileName(text));
		string text2 = text;
		int num3 = 1;
		while (File.Exists(text))
		{
			text = text2.Replace(".sav", "");
			text = SaveScreen.GetValidSaveFilename(text2 + " (" + num3.ToString() + ")");
			num3++;
		}
		Game.Instance.StartDelayedSave(text, true, false);
	}

	// Token: 0x04002708 RID: 9992
	public static GameClock Instance;

	// Token: 0x04002709 RID: 9993
	[Serialize]
	private int frame;

	// Token: 0x0400270A RID: 9994
	[Serialize]
	private float time;

	// Token: 0x0400270B RID: 9995
	[Serialize]
	private float timeSinceStartOfCycle;

	// Token: 0x0400270C RID: 9996
	[Serialize]
	private int cycle;

	// Token: 0x0400270D RID: 9997
	[Serialize]
	private float timePlayed;

	// Token: 0x0400270E RID: 9998
	[Serialize]
	private bool isNight;

	// Token: 0x0400270F RID: 9999
	public static readonly string NewCycleKey = "NewCycle";
}
