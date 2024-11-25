using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000A52 RID: 2642
public class Tech : Resource
{
	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06004CA2 RID: 19618 RVA: 0x001B6194 File Offset: 0x001B4394
	public bool FoundNode
	{
		get
		{
			return this.node != null;
		}
	}

	// Token: 0x17000579 RID: 1401
	// (get) Token: 0x06004CA3 RID: 19619 RVA: 0x001B619F File Offset: 0x001B439F
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06004CA4 RID: 19620 RVA: 0x001B61AC File Offset: 0x001B43AC
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06004CA5 RID: 19621 RVA: 0x001B61B9 File Offset: 0x001B43B9
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06004CA6 RID: 19622 RVA: 0x001B61C6 File Offset: 0x001B43C6
	public List<ResourceTreeNode.Edge> edges
	{
		get
		{
			return this.node.edges;
		}
	}

	// Token: 0x06004CA7 RID: 19623 RVA: 0x001B61D4 File Offset: 0x001B43D4
	public Tech(string id, List<string> unlockedItemIDs, Techs techs, Dictionary<string, float> overrideDefaultCosts = null) : base(id, techs, Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".NAME"))
	{
		this.desc = Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".DESC");
		this.unlockedItemIDs = unlockedItemIDs;
		if (overrideDefaultCosts != null && DlcManager.IsExpansion1Active())
		{
			foreach (KeyValuePair<string, float> keyValuePair in overrideDefaultCosts)
			{
				this.costsByResearchTypeID.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06004CA8 RID: 19624 RVA: 0x001B62CC File Offset: 0x001B44CC
	public void AddUnlockedItemIDs(params string[] ids)
	{
		foreach (string item in ids)
		{
			this.unlockedItemIDs.Add(item);
		}
	}

	// Token: 0x06004CA9 RID: 19625 RVA: 0x001B62FC File Offset: 0x001B44FC
	public void RemoveUnlockedItemIDs(params string[] ids)
	{
		foreach (string text in ids)
		{
			if (!this.unlockedItemIDs.Remove(text))
			{
				DebugUtil.DevLogError("Tech item '" + text + "' does not exist to remove");
			}
		}
	}

	// Token: 0x06004CAA RID: 19626 RVA: 0x001B6340 File Offset: 0x001B4540
	public bool RequiresResearchType(string type)
	{
		return this.costsByResearchTypeID.ContainsKey(type) && this.costsByResearchTypeID[type] > 0f;
	}

	// Token: 0x06004CAB RID: 19627 RVA: 0x001B6365 File Offset: 0x001B4565
	public void SetNode(ResourceTreeNode node, string categoryID)
	{
		this.node = node;
		this.category = categoryID;
	}

	// Token: 0x06004CAC RID: 19628 RVA: 0x001B6378 File Offset: 0x001B4578
	public bool CanAfford(ResearchPointInventory pointInventory)
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			if (pointInventory.PointsByTypeID[keyValuePair.Key] < keyValuePair.Value)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06004CAD RID: 19629 RVA: 0x001B63E8 File Offset: 0x001B45E8
	public string CostString(ResearchTypes types)
	{
		string text = "";
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			text += string.Format("{0}:{1}", types.GetResearchType(keyValuePair.Key).name.ToString(), keyValuePair.Value.ToString());
			text += "\n";
		}
		return text;
	}

	// Token: 0x06004CAE RID: 19630 RVA: 0x001B6480 File Offset: 0x001B4680
	public bool IsComplete()
	{
		if (Research.Instance != null)
		{
			TechInstance techInstance = Research.Instance.Get(this);
			return techInstance != null && techInstance.IsComplete();
		}
		return false;
	}

	// Token: 0x06004CAF RID: 19631 RVA: 0x001B64B4 File Offset: 0x001B46B4
	public bool ArePrerequisitesComplete()
	{
		using (List<Tech>.Enumerator enumerator = this.requiredTech.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x040032F4 RID: 13044
	public List<Tech> requiredTech = new List<Tech>();

	// Token: 0x040032F5 RID: 13045
	public List<Tech> unlockedTech = new List<Tech>();

	// Token: 0x040032F6 RID: 13046
	public List<TechItem> unlockedItems = new List<TechItem>();

	// Token: 0x040032F7 RID: 13047
	public List<string> unlockedItemIDs = new List<string>();

	// Token: 0x040032F8 RID: 13048
	public int tier;

	// Token: 0x040032F9 RID: 13049
	public Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();

	// Token: 0x040032FA RID: 13050
	public string desc;

	// Token: 0x040032FB RID: 13051
	public string category;

	// Token: 0x040032FC RID: 13052
	public Tag[] tags;

	// Token: 0x040032FD RID: 13053
	private ResourceTreeNode node;
}
