using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200077B RID: 1915
[AddComponentMenu("KMonoBehaviour/scripts/SweepBotStation")]
public class SweepBotStation : KMonoBehaviour
{
	// Token: 0x060033F0 RID: 13296 RVA: 0x0011C3CB File Offset: 0x0011A5CB
	public void SetStorages(Storage botMaterialStorage, Storage sweepStorage)
	{
		this.botMaterialStorage = botMaterialStorage;
		this.sweepStorage = sweepStorage;
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x0011C3DB File Offset: 0x0011A5DB
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
		base.Subscribe<SweepBotStation>(-592767678, SweepBotStation.OnOperationalChangedDelegate);
	}

	// Token: 0x060033F2 RID: 13298 RVA: 0x0011C3F5 File Offset: 0x0011A5F5
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		base.GetComponent<Operational>().SetFlag(SweepBotStation.dockedRobot, false);
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x0011C410 File Offset: 0x0011A610
	protected override void OnSpawn()
	{
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
		else
		{
			StorageUnloadMonitor.Instance smi = this.sweepBot.Get().GetSMI<StorageUnloadMonitor.Instance>();
			smi.sm.sweepLocker.Set(this.sweepStorage, smi, false);
			this.RefreshSweepBotSubscription();
		}
		this.UpdateMeter();
		this.UpdateNameDisplay();
	}

	// Token: 0x060033F4 RID: 13300 RVA: 0x0011C4D0 File Offset: 0x0011A6D0
	private void RequestNewSweepBot(object data = null)
	{
		if (this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS) == null)
		{
			FetchList2 fetchList = new FetchList2(this.botMaterialStorage, Db.Get().ChoreTypes.Fetch);
			fetchList.Add(GameTags.RefinedMetal, null, SweepBotConfig.MASS, Operational.State.None);
			fetchList.Submit(null, true);
			return;
		}
		this.MakeNewSweepBot(null);
	}

	// Token: 0x060033F5 RID: 13301 RVA: 0x0011C538 File Offset: 0x0011A738
	private void MakeNewSweepBot(object data = null)
	{
		if (this.newSweepyHandle.IsValid)
		{
			return;
		}
		if (this.botMaterialStorage.GetAmountAvailable(GameTags.RefinedMetal) < SweepBotConfig.MASS)
		{
			return;
		}
		PrimaryElement primaryElement = this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS);
		if (primaryElement == null)
		{
			return;
		}
		SimHashes sweepBotMaterial = primaryElement.ElementID;
		float temperature;
		SimUtil.DiseaseInfo disease;
		float num;
		this.botMaterialStorage.ConsumeAndGetDisease(sweepBotMaterial.CreateTag(), SweepBotConfig.MASS, out num, out disease, out temperature);
		this.UpdateMeter();
		this.newSweepyHandle = GameScheduler.Instance.Schedule("MakeSweepy", 2f, delegate(object obj)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("SweepBot"), Grid.CellToPos(Grid.CellRight(Grid.PosToCell(this.gameObject))), Grid.SceneLayer.Creatures, null, 0);
			gameObject.SetActive(true);
			this.sweepBot = new Ref<KSelectable>(gameObject.GetComponent<KSelectable>());
			if (!string.IsNullOrEmpty(this.storedName))
			{
				this.sweepBot.Get().GetComponent<UserNameable>().SetName(this.storedName);
			}
			this.UpdateNameDisplay();
			StorageUnloadMonitor.Instance smi = gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			smi.sm.sweepLocker.Set(this.sweepStorage, smi, false);
			PrimaryElement component = this.sweepBot.Get().GetComponent<PrimaryElement>();
			component.ElementID = sweepBotMaterial;
			component.Temperature = temperature;
			if (disease.idx != 255)
			{
				component.AddDisease(disease.idx, disease.count, "Inherited from the material used for its creation");
			}
			this.RefreshSweepBotSubscription();
			this.newSweepyHandle.ClearScheduler();
		}, null, null);
		base.GetComponent<KBatchedAnimController>().Play("newsweepy", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060033F6 RID: 13302 RVA: 0x0011C61C File Offset: 0x0011A81C
	private void RefreshSweepBotSubscription()
	{
		if (this.refreshSweepbotHandle != -1)
		{
			this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
			this.sweepBot.Get().Unsubscribe(this.sweepBotNameChangeHandle);
		}
		this.refreshSweepbotHandle = this.sweepBot.Get().Subscribe(1969584890, new Action<object>(this.RequestNewSweepBot));
		this.sweepBotNameChangeHandle = this.sweepBot.Get().Subscribe(1102426921, new Action<object>(this.UpdateStoredName));
	}

	// Token: 0x060033F7 RID: 13303 RVA: 0x0011C6AC File Offset: 0x0011A8AC
	private void UpdateStoredName(object data)
	{
		this.storedName = (string)data;
		this.UpdateNameDisplay();
	}

	// Token: 0x060033F8 RID: 13304 RVA: 0x0011C6C0 File Offset: 0x0011A8C0
	private void UpdateNameDisplay()
	{
		if (string.IsNullOrEmpty(this.storedName))
		{
			base.GetComponent<KSelectable>().SetName(string.Format(BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, ROBOTS.MODELS.SWEEPBOT.NAME));
		}
		else
		{
			base.GetComponent<KSelectable>().SetName(string.Format(BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, this.storedName));
		}
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x060033F9 RID: 13305 RVA: 0x0011C72B File Offset: 0x0011A92B
	public void DockRobot(bool docked)
	{
		base.GetComponent<Operational>().SetFlag(SweepBotStation.dockedRobot, docked);
	}

	// Token: 0x060033FA RID: 13306 RVA: 0x0011C740 File Offset: 0x0011A940
	public void StartCharging()
	{
		base.GetComponent<KBatchedAnimController>().Queue("sleep_pre", KAnim.PlayMode.Once, 1f, 0f);
		base.GetComponent<KBatchedAnimController>().Queue("sleep_idle", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x060033FB RID: 13307 RVA: 0x0011C78D File Offset: 0x0011A98D
	public void StopCharging()
	{
		base.GetComponent<KBatchedAnimController>().Play("sleep_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.UpdateNameDisplay();
	}

	// Token: 0x060033FC RID: 13308 RVA: 0x0011C7B8 File Offset: 0x0011A9B8
	protected override void OnCleanUp()
	{
		if (this.newSweepyHandle.IsValid)
		{
			this.newSweepyHandle.ClearScheduler();
		}
		if (this.refreshSweepbotHandle != -1 && this.sweepBot.Get() != null)
		{
			this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
		}
	}

	// Token: 0x060033FD RID: 13309 RVA: 0x0011C810 File Offset: 0x0011AA10
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x060033FE RID: 13310 RVA: 0x0011C848 File Offset: 0x0011AA48
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component.currentFrame >= component.GetCurrentNumFrames())
		{
			base.GetComponent<KBatchedAnimController>().Play("remove", KAnim.PlayMode.Once, 1f, 0f);
		}
		for (int i = 0; i < this.sweepStorage.Count; i++)
		{
			this.sweepStorage[i].GetComponent<Clearable>().MarkForClear(false, true);
		}
	}

	// Token: 0x060033FF RID: 13311 RVA: 0x0011C8E0 File Offset: 0x0011AAE0
	private void OnOperationalChanged(object data)
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(!component.Flags.ContainsValue(false), false);
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x0011C91F File Offset: 0x0011AB1F
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.sweepStorage.Capacity() - this.sweepStorage.storageFullMargin;
	}

	// Token: 0x06003401 RID: 13313 RVA: 0x0011C938 File Offset: 0x0011AB38
	private float GetAmountStored()
	{
		return this.sweepStorage.MassStored();
	}

	// Token: 0x04001EC0 RID: 7872
	[Serialize]
	public Ref<KSelectable> sweepBot;

	// Token: 0x04001EC1 RID: 7873
	[Serialize]
	public string storedName;

	// Token: 0x04001EC2 RID: 7874
	private static readonly Operational.Flag dockedRobot = new Operational.Flag("dockedRobot", Operational.Flag.Type.Functional);

	// Token: 0x04001EC3 RID: 7875
	private MeterController meter;

	// Token: 0x04001EC4 RID: 7876
	[SerializeField]
	private Storage botMaterialStorage;

	// Token: 0x04001EC5 RID: 7877
	[SerializeField]
	private Storage sweepStorage;

	// Token: 0x04001EC6 RID: 7878
	private SchedulerHandle newSweepyHandle;

	// Token: 0x04001EC7 RID: 7879
	private static readonly EventSystem.IntraObjectHandler<SweepBotStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SweepBotStation>(delegate(SweepBotStation component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04001EC8 RID: 7880
	private int refreshSweepbotHandle = -1;

	// Token: 0x04001EC9 RID: 7881
	private int sweepBotNameChangeHandle = -1;
}
