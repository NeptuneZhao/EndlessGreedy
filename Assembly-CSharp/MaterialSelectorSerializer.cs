using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000CBF RID: 3263
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/MaterialSelectorSerializer")]
public class MaterialSelectorSerializer : KMonoBehaviour
{
	// Token: 0x060064F4 RID: 25844 RVA: 0x0025D4E8 File Offset: 0x0025B6E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.previouslySelectedElementsPerWorld == null)
		{
			this.previouslySelectedElementsPerWorld = new List<Dictionary<Tag, Tag>>[255];
			if (this.previouslySelectedElements != null)
			{
				foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
				{
					List<Dictionary<Tag, Tag>> list = this.previouslySelectedElements.ConvertAll<Dictionary<Tag, Tag>>((Dictionary<Tag, Tag> input) => new Dictionary<Tag, Tag>(input));
					this.previouslySelectedElementsPerWorld[worldContainer.id] = list;
				}
				this.previouslySelectedElements = null;
			}
		}
	}

	// Token: 0x060064F5 RID: 25845 RVA: 0x0025D59C File Offset: 0x0025B79C
	public void WipeWorldSelectionData(int worldID)
	{
		this.previouslySelectedElementsPerWorld[worldID] = null;
	}

	// Token: 0x060064F6 RID: 25846 RVA: 0x0025D5A8 File Offset: 0x0025B7A8
	public void SetSelectedElement(int worldID, int selectorIndex, Tag recipe, Tag element)
	{
		if (this.previouslySelectedElementsPerWorld[worldID] == null)
		{
			this.previouslySelectedElementsPerWorld[worldID] = new List<Dictionary<Tag, Tag>>();
		}
		List<Dictionary<Tag, Tag>> list = this.previouslySelectedElementsPerWorld[worldID];
		while (list.Count <= selectorIndex)
		{
			list.Add(new Dictionary<Tag, Tag>());
		}
		list[selectorIndex][recipe] = element;
	}

	// Token: 0x060064F7 RID: 25847 RVA: 0x0025D5FC File Offset: 0x0025B7FC
	public Tag GetPreviousElement(int worldID, int selectorIndex, Tag recipe)
	{
		Tag invalid = Tag.Invalid;
		if (this.previouslySelectedElementsPerWorld[worldID] == null)
		{
			return invalid;
		}
		List<Dictionary<Tag, Tag>> list = this.previouslySelectedElementsPerWorld[worldID];
		if (list.Count <= selectorIndex)
		{
			return invalid;
		}
		list[selectorIndex].TryGetValue(recipe, out invalid);
		return invalid;
	}

	// Token: 0x0400446B RID: 17515
	[Serialize]
	private List<Dictionary<Tag, Tag>> previouslySelectedElements;

	// Token: 0x0400446C RID: 17516
	[Serialize]
	private List<Dictionary<Tag, Tag>>[] previouslySelectedElementsPerWorld;
}
