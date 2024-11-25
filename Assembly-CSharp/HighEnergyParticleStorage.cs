using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020008EE RID: 2286
public class HighEnergyParticleStorage : KMonoBehaviour, IStorage
{
	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x060041B0 RID: 16816 RVA: 0x00174C99 File Offset: 0x00172E99
	public float Particles
	{
		get
		{
			return this.particles;
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x060041B1 RID: 16817 RVA: 0x00174CA1 File Offset: 0x00172EA1
	// (set) Token: 0x060041B2 RID: 16818 RVA: 0x00174CA9 File Offset: 0x00172EA9
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x060041B3 RID: 16819 RVA: 0x00174CB4 File Offset: 0x00172EB4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Combine(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
			component.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Combine(component.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
		this.SetupStorageStatusItems();
	}

	// Token: 0x060041B4 RID: 16820 RVA: 0x00174D23 File Offset: 0x00172F23
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateLogicPorts();
	}

	// Token: 0x060041B5 RID: 16821 RVA: 0x00174D34 File Offset: 0x00172F34
	private void UpdateLogicPorts()
	{
		if (this._logicPorts != null)
		{
			bool value = this.IsFull();
			this._logicPorts.SendSignal(this.PORT_ID, Convert.ToInt32(value));
		}
	}

	// Token: 0x060041B6 RID: 16822 RVA: 0x00174D72 File Offset: 0x00172F72
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Remove(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
		}
	}

	// Token: 0x060041B7 RID: 16823 RVA: 0x00174DB0 File Offset: 0x00172FB0
	private void OnParticleCapture(HighEnergyParticle particle)
	{
		float num = Mathf.Min(particle.payload, this.capacity - this.particles);
		this.Store(num);
		particle.payload -= num;
		if (particle.payload > 0f)
		{
			base.gameObject.GetComponent<HighEnergyParticlePort>().Uncapture(particle);
		}
	}

	// Token: 0x060041B8 RID: 16824 RVA: 0x00174E0A File Offset: 0x0017300A
	private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
	{
		return this.particles < this.capacity && this.receiverOpen;
	}

	// Token: 0x060041B9 RID: 16825 RVA: 0x00174E24 File Offset: 0x00173024
	private void DeltaParticles(float delta)
	{
		this.particles += delta;
		if (this.particles <= 0f)
		{
			base.Trigger(155636535, base.transform.gameObject);
		}
		base.Trigger(-1837862626, base.transform.gameObject);
		this.UpdateLogicPorts();
	}

	// Token: 0x060041BA RID: 16826 RVA: 0x00174E80 File Offset: 0x00173080
	public float Store(float amount)
	{
		float num = Mathf.Min(amount, this.RemainingCapacity());
		this.DeltaParticles(num);
		return num;
	}

	// Token: 0x060041BB RID: 16827 RVA: 0x00174EA2 File Offset: 0x001730A2
	public float ConsumeAndGet(float amount)
	{
		amount = Mathf.Min(this.Particles, amount);
		this.DeltaParticles(-amount);
		return amount;
	}

	// Token: 0x060041BC RID: 16828 RVA: 0x00174EBB File Offset: 0x001730BB
	[ContextMenu("Trigger Stored Event")]
	public void DEBUG_TriggerStorageEvent()
	{
		base.Trigger(-1837862626, base.transform.gameObject);
	}

	// Token: 0x060041BD RID: 16829 RVA: 0x00174ED3 File Offset: 0x001730D3
	[ContextMenu("Trigger Zero Event")]
	public void DEBUG_TriggerZeroEvent()
	{
		this.ConsumeAndGet(this.particles + 1f);
	}

	// Token: 0x060041BE RID: 16830 RVA: 0x00174EE8 File Offset: 0x001730E8
	public float ConsumeAll()
	{
		return this.ConsumeAndGet(this.particles);
	}

	// Token: 0x060041BF RID: 16831 RVA: 0x00174EF6 File Offset: 0x001730F6
	public bool HasRadiation()
	{
		return this.Particles > 0f;
	}

	// Token: 0x060041C0 RID: 16832 RVA: 0x00174F05 File Offset: 0x00173105
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		return null;
	}

	// Token: 0x060041C1 RID: 16833 RVA: 0x00174F08 File Offset: 0x00173108
	public List<GameObject> GetItems()
	{
		return new List<GameObject>
		{
			base.gameObject
		};
	}

	// Token: 0x060041C2 RID: 16834 RVA: 0x00174F1B File Offset: 0x0017311B
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x060041C3 RID: 16835 RVA: 0x00174F2D File Offset: 0x0017312D
	public bool IsEmpty()
	{
		return this.Particles == 0f;
	}

	// Token: 0x060041C4 RID: 16836 RVA: 0x00174F3C File Offset: 0x0017313C
	public float Capacity()
	{
		return this.capacity;
	}

	// Token: 0x060041C5 RID: 16837 RVA: 0x00174F44 File Offset: 0x00173144
	public float RemainingCapacity()
	{
		return Mathf.Max(this.capacity - this.Particles, 0f);
	}

	// Token: 0x060041C6 RID: 16838 RVA: 0x00174F5D File Offset: 0x0017315D
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x060041C7 RID: 16839 RVA: 0x00174F65 File Offset: 0x00173165
	public float GetAmountAvailable(Tag tag)
	{
		if (tag != GameTags.HighEnergyParticle)
		{
			return 0f;
		}
		return this.Particles;
	}

	// Token: 0x060041C8 RID: 16840 RVA: 0x00174F80 File Offset: 0x00173180
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		DebugUtil.DevAssert(tag == GameTags.HighEnergyParticle, "Consuming non-particle tag as amount", null);
		this.ConsumeAndGet(amount);
	}

	// Token: 0x060041C9 RID: 16841 RVA: 0x00174FA0 File Offset: 0x001731A0
	private void SetupStorageStatusItems()
	{
		if (HighEnergyParticleStorage.capacityStatusItem == null)
		{
			HighEnergyParticleStorage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			HighEnergyParticleStorage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				HighEnergyParticleStorage highEnergyParticleStorage = (HighEnergyParticleStorage)data;
				string newValue = Util.FormatWholeNumber(highEnergyParticleStorage.particles);
				string newValue2 = Util.FormatWholeNumber(highEnergyParticleStorage.capacity);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				str = str.Replace("{Units}", UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES);
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, HighEnergyParticleStorage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, HighEnergyParticleStorage.capacityStatusItem, this);
		}
	}

	// Token: 0x04002B8C RID: 11148
	[Serialize]
	[SerializeField]
	private float particles;

	// Token: 0x04002B8D RID: 11149
	public float capacity = float.MaxValue;

	// Token: 0x04002B8E RID: 11150
	public bool showInUI = true;

	// Token: 0x04002B8F RID: 11151
	public bool showCapacityStatusItem;

	// Token: 0x04002B90 RID: 11152
	public bool showCapacityAsMainStatus;

	// Token: 0x04002B92 RID: 11154
	public bool autoStore;

	// Token: 0x04002B93 RID: 11155
	[Serialize]
	public bool receiverOpen = true;

	// Token: 0x04002B94 RID: 11156
	[MyCmpGet]
	private LogicPorts _logicPorts;

	// Token: 0x04002B95 RID: 11157
	public string PORT_ID = "";

	// Token: 0x04002B96 RID: 11158
	private static StatusItem capacityStatusItem;
}
