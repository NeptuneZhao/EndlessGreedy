using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A53 RID: 2643
public class TechInstance
{
	// Token: 0x06004CB0 RID: 19632 RVA: 0x001B6510 File Offset: 0x001B4710
	public TechInstance(Tech tech)
	{
		this.tech = tech;
	}

	// Token: 0x06004CB1 RID: 19633 RVA: 0x001B6535 File Offset: 0x001B4735
	public bool IsComplete()
	{
		return this.complete;
	}

	// Token: 0x06004CB2 RID: 19634 RVA: 0x001B653D File Offset: 0x001B473D
	public void Purchased()
	{
		if (!this.complete)
		{
			this.complete = true;
		}
	}

	// Token: 0x06004CB3 RID: 19635 RVA: 0x001B6550 File Offset: 0x001B4750
	public void UnlockPOITech(string tech_id)
	{
		TechItem techItem = Db.Get().TechItems.Get(tech_id);
		if (techItem == null || !techItem.isPOIUnlock)
		{
			return;
		}
		if (!this.UnlockedPOITechIds.Contains(tech_id))
		{
			this.UnlockedPOITechIds.Add(tech_id);
			BuildingDef buildingDef = Assets.GetBuildingDef(techItem.Id);
			if (buildingDef != null)
			{
				Game.Instance.Trigger(-107300940, buildingDef);
			}
		}
	}

	// Token: 0x06004CB4 RID: 19636 RVA: 0x001B65BC File Offset: 0x001B47BC
	public float GetTotalPercentageComplete()
	{
		float num = 0f;
		int num2 = 0;
		foreach (string type in this.progressInventory.PointsByTypeID.Keys)
		{
			if (this.tech.RequiresResearchType(type))
			{
				num += this.PercentageCompleteResearchType(type);
				num2++;
			}
		}
		return num / (float)num2;
	}

	// Token: 0x06004CB5 RID: 19637 RVA: 0x001B663C File Offset: 0x001B483C
	public float PercentageCompleteResearchType(string type)
	{
		if (!this.tech.RequiresResearchType(type))
		{
			return 1f;
		}
		return Mathf.Clamp01(this.progressInventory.PointsByTypeID[type] / this.tech.costsByResearchTypeID[type]);
	}

	// Token: 0x06004CB6 RID: 19638 RVA: 0x001B667C File Offset: 0x001B487C
	public TechInstance.SaveData Save()
	{
		string[] array = new string[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Keys.CopyTo(array, 0);
		float[] array2 = new float[this.progressInventory.PointsByTypeID.Count];
		this.progressInventory.PointsByTypeID.Values.CopyTo(array2, 0);
		string[] unlockedPOIIDs = this.UnlockedPOITechIds.ToArray();
		return new TechInstance.SaveData
		{
			techId = this.tech.Id,
			complete = this.complete,
			inventoryIDs = array,
			inventoryValues = array2,
			unlockedPOIIDs = unlockedPOIIDs
		};
	}

	// Token: 0x06004CB7 RID: 19639 RVA: 0x001B6730 File Offset: 0x001B4930
	public void Load(TechInstance.SaveData save_data)
	{
		this.complete = save_data.complete;
		for (int i = 0; i < save_data.inventoryIDs.Length; i++)
		{
			this.progressInventory.AddResearchPoints(save_data.inventoryIDs[i], save_data.inventoryValues[i]);
		}
		if (save_data.unlockedPOIIDs != null)
		{
			this.UnlockedPOITechIds = new List<string>(save_data.unlockedPOIIDs);
		}
	}

	// Token: 0x040032FE RID: 13054
	public Tech tech;

	// Token: 0x040032FF RID: 13055
	private bool complete;

	// Token: 0x04003300 RID: 13056
	public ResearchPointInventory progressInventory = new ResearchPointInventory();

	// Token: 0x04003301 RID: 13057
	public List<string> UnlockedPOITechIds = new List<string>();

	// Token: 0x02001A5A RID: 6746
	public struct SaveData
	{
		// Token: 0x04007C24 RID: 31780
		public string techId;

		// Token: 0x04007C25 RID: 31781
		public bool complete;

		// Token: 0x04007C26 RID: 31782
		public string[] inventoryIDs;

		// Token: 0x04007C27 RID: 31783
		public float[] inventoryValues;

		// Token: 0x04007C28 RID: 31784
		public string[] unlockedPOIIDs;
	}
}
