using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200094C RID: 2380
internal class LogicEventHandler : ILogicEventReceiver, ILogicNetworkConnection, ILogicUIElement, IUniformGridObject
{
	// Token: 0x06004558 RID: 17752 RVA: 0x0018AFFA File Offset: 0x001891FA
	public LogicEventHandler(int cell, Action<int, int> on_value_changed, Action<int, bool> on_connection_changed, LogicPortSpriteType sprite_type)
	{
		this.cell = cell;
		this.onValueChanged = on_value_changed;
		this.onConnectionChanged = on_connection_changed;
		this.spriteType = sprite_type;
	}

	// Token: 0x06004559 RID: 17753 RVA: 0x0018B020 File Offset: 0x00189220
	public void ReceiveLogicEvent(int value)
	{
		this.TriggerAudio(value);
		int arg = this.value;
		this.value = value;
		this.onValueChanged(value, arg);
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x0600455A RID: 17754 RVA: 0x0018B04F File Offset: 0x0018924F
	public int Value
	{
		get
		{
			return this.value;
		}
	}

	// Token: 0x0600455B RID: 17755 RVA: 0x0018B057 File Offset: 0x00189257
	public int GetLogicUICell()
	{
		return this.cell;
	}

	// Token: 0x0600455C RID: 17756 RVA: 0x0018B05F File Offset: 0x0018925F
	public LogicPortSpriteType GetLogicPortSpriteType()
	{
		return this.spriteType;
	}

	// Token: 0x0600455D RID: 17757 RVA: 0x0018B067 File Offset: 0x00189267
	public Vector2 PosMin()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x0600455E RID: 17758 RVA: 0x0018B079 File Offset: 0x00189279
	public Vector2 PosMax()
	{
		return Grid.CellToPos2D(this.cell);
	}

	// Token: 0x0600455F RID: 17759 RVA: 0x0018B08B File Offset: 0x0018928B
	public int GetLogicCell()
	{
		return this.cell;
	}

	// Token: 0x06004560 RID: 17760 RVA: 0x0018B094 File Offset: 0x00189294
	private void TriggerAudio(int new_value)
	{
		LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.cell);
		SpeedControlScreen instance = SpeedControlScreen.Instance;
		if (networkForCell != null && new_value != this.value && instance != null && !instance.IsPaused)
		{
			if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation) && KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) != 1 && OverlayScreen.Instance.GetMode() != OverlayModes.Logic.ID)
			{
				return;
			}
			string name = "Logic_Building_Toggle";
			if (!CameraController.Instance.IsAudibleSound(Grid.CellToPosCCC(this.cell, Grid.SceneLayer.BuildingFront)))
			{
				return;
			}
			LogicCircuitNetwork.LogicSoundPair logicSoundPair = new LogicCircuitNetwork.LogicSoundPair();
			Dictionary<int, LogicCircuitNetwork.LogicSoundPair> logicSoundRegister = LogicCircuitNetwork.logicSoundRegister;
			int id = networkForCell.id;
			if (!logicSoundRegister.ContainsKey(id))
			{
				logicSoundRegister.Add(id, logicSoundPair);
			}
			else
			{
				logicSoundPair.playedIndex = logicSoundRegister[id].playedIndex;
				logicSoundPair.lastPlayed = logicSoundRegister[id].lastPlayed;
			}
			if (logicSoundPair.playedIndex < 2)
			{
				logicSoundRegister[id].playedIndex = logicSoundPair.playedIndex + 1;
			}
			else
			{
				logicSoundRegister[id].playedIndex = 0;
				logicSoundRegister[id].lastPlayed = Time.time;
			}
			float num = (Time.time - logicSoundPair.lastPlayed) / 3f;
			EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), Grid.CellToPos(this.cell), 1f);
			instance2.setParameterByName("logic_volumeModifer", num, false);
			instance2.setParameterByName("wireCount", (float)(networkForCell.WireCount % 24), false);
			instance2.setParameterByName("enabled", (float)new_value, false);
			KFMOD.EndOneShot(instance2);
		}
	}

	// Token: 0x06004561 RID: 17761 RVA: 0x0018B244 File Offset: 0x00189444
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
		if (this.onConnectionChanged != null)
		{
			this.onConnectionChanged(this.cell, connected);
		}
	}

	// Token: 0x04002D36 RID: 11574
	private int cell;

	// Token: 0x04002D37 RID: 11575
	private int value;

	// Token: 0x04002D38 RID: 11576
	private Action<int, int> onValueChanged;

	// Token: 0x04002D39 RID: 11577
	private Action<int, bool> onConnectionChanged;

	// Token: 0x04002D3A RID: 11578
	private LogicPortSpriteType spriteType;
}
