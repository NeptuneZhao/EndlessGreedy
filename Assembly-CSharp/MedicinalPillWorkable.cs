using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200095B RID: 2395
[AddComponentMenu("KMonoBehaviour/Workable/MedicinalPillWorkable")]
public class MedicinalPillWorkable : Workable, IConsumableUIItem
{
	// Token: 0x060045F1 RID: 17905 RVA: 0x0018DF40 File Offset: 0x0018C140
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		this.showProgressBar = false;
		this.synchronizeAnims = false;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		this.CreateChore();
	}

	// Token: 0x060045F2 RID: 17906 RVA: 0x0018DFA0 File Offset: 0x0018C1A0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = worker.GetComponent<Effects>();
			EffectInstance effectInstance = component.Get(this.pill.info.effect);
			if (effectInstance != null)
			{
				effectInstance.timeRemaining = effectInstance.effect.duration;
			}
			else
			{
				component.Add(this.pill.info.effect, true);
			}
		}
		Sicknesses sicknesses = worker.GetSicknesses();
		foreach (string id in this.pill.info.curedSicknesses)
		{
			SicknessInstance sicknessInstance = sicknesses.Get(id);
			if (sicknessInstance != null)
			{
				Game.Instance.savedInfo.curedDisease = true;
				sicknessInstance.Cure();
			}
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x060045F3 RID: 17907 RVA: 0x0018E094 File Offset: 0x0018C294
	private void CreateChore()
	{
		new TakeMedicineChore(this);
	}

	// Token: 0x060045F4 RID: 17908 RVA: 0x0018E0A0 File Offset: 0x0018C2A0
	public bool CanBeTakenBy(GameObject consumer)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = consumer.GetComponent<Effects>();
			if (component == null || component.HasEffect(this.pill.info.effect))
			{
				return false;
			}
		}
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.Booster)
		{
			return true;
		}
		Sicknesses sicknesses = consumer.GetSicknesses();
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.CureAny && sicknesses.Count > 0)
		{
			return true;
		}
		foreach (SicknessInstance sicknessInstance in sicknesses)
		{
			if (this.pill.info.curedSicknesses.Contains(sicknessInstance.modifier.Id))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x060045F5 RID: 17909 RVA: 0x0018E188 File Offset: 0x0018C388
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x060045F6 RID: 17910 RVA: 0x0018E1A3 File Offset: 0x0018C3A3
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x060045F7 RID: 17911 RVA: 0x0018E1AB File Offset: 0x0018C3AB
	public int MajorOrder
	{
		get
		{
			return (int)(this.pill.info.medicineType + 1000);
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x060045F8 RID: 17912 RVA: 0x0018E1C3 File Offset: 0x0018C3C3
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x060045F9 RID: 17913 RVA: 0x0018E1C6 File Offset: 0x0018C3C6
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04002D80 RID: 11648
	public MedicinalPill pill;
}
