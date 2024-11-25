using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DD1 RID: 3537
public class SubSpeciesInfoScreen : KModalScreen
{
	// Token: 0x06007070 RID: 28784 RVA: 0x002A8CF0 File Offset: 0x002A6EF0
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06007071 RID: 28785 RVA: 0x002A8CF3 File Offset: 0x002A6EF3
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06007072 RID: 28786 RVA: 0x002A8CFC File Offset: 0x002A6EFC
	private void ClearMutations()
	{
		for (int i = this.mutationLineItems.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.mutationLineItems[i]);
		}
		this.mutationLineItems.Clear();
	}

	// Token: 0x06007073 RID: 28787 RVA: 0x002A8D3D File Offset: 0x002A6F3D
	public void DisplayDiscovery(Tag speciesID, Tag subSpeciesID, GeneticAnalysisStation station)
	{
		this.SetSubspecies(speciesID, subSpeciesID);
		this.targetStation = station;
	}

	// Token: 0x06007074 RID: 28788 RVA: 0x002A8D50 File Offset: 0x002A6F50
	private void SetSubspecies(Tag speciesID, Tag subSpeciesID)
	{
		this.ClearMutations();
		ref PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(speciesID, subSpeciesID);
		this.plantIcon.sprite = Def.GetUISprite(Assets.GetPrefab(speciesID), "ui", false).first;
		foreach (string id in subSpecies.mutationIDs)
		{
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			GameObject gameObject = Util.KInstantiateUI(this.mutationsItemPrefab, this.mutationsList.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("nameLabel").text = plantMutation.Name;
			component.GetReference<LocText>("descriptionLabel").text = plantMutation.description;
			this.mutationLineItems.Add(gameObject);
		}
	}

	// Token: 0x04004D47 RID: 19783
	[SerializeField]
	private KButton renameButton;

	// Token: 0x04004D48 RID: 19784
	[SerializeField]
	private KButton saveButton;

	// Token: 0x04004D49 RID: 19785
	[SerializeField]
	private KButton discardButton;

	// Token: 0x04004D4A RID: 19786
	[SerializeField]
	private RectTransform mutationsList;

	// Token: 0x04004D4B RID: 19787
	[SerializeField]
	private Image plantIcon;

	// Token: 0x04004D4C RID: 19788
	[SerializeField]
	private GameObject mutationsItemPrefab;

	// Token: 0x04004D4D RID: 19789
	private List<GameObject> mutationLineItems = new List<GameObject>();

	// Token: 0x04004D4E RID: 19790
	private GeneticAnalysisStation targetStation;
}
