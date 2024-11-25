using System;
using System.Collections.Generic;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D6E RID: 3438
public class GeneticAnalysisStationSideScreen : SideScreenContent
{
	// Token: 0x06006C2C RID: 27692 RVA: 0x0028B1F0 File Offset: 0x002893F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Refresh();
	}

	// Token: 0x06006C2D RID: 27693 RVA: 0x0028B1FE File Offset: 0x002893FE
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<GeneticAnalysisStation.StatesInstance>() != null;
	}

	// Token: 0x06006C2E RID: 27694 RVA: 0x0028B209 File Offset: 0x00289409
	public override void SetTarget(GameObject target)
	{
		this.target = target.GetSMI<GeneticAnalysisStation.StatesInstance>();
		target.GetComponent<GeneticAnalysisStationWorkable>();
		this.Refresh();
	}

	// Token: 0x06006C2F RID: 27695 RVA: 0x0028B224 File Offset: 0x00289424
	private void Refresh()
	{
		if (this.target == null)
		{
			return;
		}
		this.DrawPickerMenu();
	}

	// Token: 0x06006C30 RID: 27696 RVA: 0x0028B238 File Offset: 0x00289438
	private void DrawPickerMenu()
	{
		Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> dictionary = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();
		foreach (Tag tag in PlantSubSpeciesCatalog.Instance.GetAllDiscoveredSpecies())
		{
			dictionary[tag] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>(PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(tag));
		}
		int num = 0;
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in dictionary)
		{
			if (PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(keyValuePair.Key).Count > 1)
			{
				GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
				if (!(prefab == null))
				{
					SeedProducer component = prefab.GetComponent<SeedProducer>();
					if (!(component == null))
					{
						Tag tag2 = component.seedInfo.seedId.ToTag();
						if (DiscoveredResources.Instance.IsDiscovered(tag2))
						{
							HierarchyReferences hierarchyReferences;
							if (num < this.rows.Count)
							{
								hierarchyReferences = this.rows[num];
							}
							else
							{
								hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.rowPrefab.gameObject, this.rowContainer, false);
								this.rows.Add(hierarchyReferences);
							}
							this.ConfigureButton(hierarchyReferences, keyValuePair.Key);
							this.rows[num].gameObject.SetActive(true);
							num++;
						}
					}
				}
			}
		}
		for (int i = num; i < this.rows.Count; i++)
		{
			this.rows[i].gameObject.SetActive(false);
		}
		if (num == 0)
		{
			this.message.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.NONE_DISCOVERED;
			this.contents.gameObject.SetActive(false);
			return;
		}
		this.message.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SELECT_SEEDS;
		this.contents.gameObject.SetActive(true);
	}

	// Token: 0x06006C31 RID: 27697 RVA: 0x0028B444 File Offset: 0x00289644
	private void ConfigureButton(HierarchyReferences button, Tag speciesID)
	{
		TMP_Text reference = button.GetReference<LocText>("Label");
		Image reference2 = button.GetReference<Image>("Icon");
		LocText reference3 = button.GetReference<LocText>("ProgressLabel");
		button.GetReference<ToolTip>("ToolTip");
		Tag seedID = this.GetSeedIDFromPlantID(speciesID);
		bool isForbidden = this.target.GetSeedForbidden(seedID);
		reference.text = seedID.ProperName();
		reference2.sprite = Def.GetUISprite(seedID, "ui", false).first;
		if (PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(speciesID).Count > 0)
		{
			reference3.text = (isForbidden ? UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_FORBIDDEN : UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_ALLOWED);
		}
		else
		{
			reference3.text = UI.UISIDESCREENS.GENETICANALYSISSIDESCREEN.SEED_NO_MUTANTS;
		}
		KToggle component = button.GetComponent<KToggle>();
		component.isOn = !isForbidden;
		component.ClearOnClick();
		component.onClick += delegate()
		{
			this.target.SetSeedForbidden(seedID, !isForbidden);
			this.Refresh();
		};
	}

	// Token: 0x06006C32 RID: 27698 RVA: 0x0028B552 File Offset: 0x00289752
	private Tag GetSeedIDFromPlantID(Tag speciesID)
	{
		return Assets.GetPrefab(speciesID).GetComponent<SeedProducer>().seedInfo.seedId;
	}

	// Token: 0x040049C5 RID: 18885
	[SerializeField]
	private LocText message;

	// Token: 0x040049C6 RID: 18886
	[SerializeField]
	private GameObject contents;

	// Token: 0x040049C7 RID: 18887
	[SerializeField]
	private GameObject rowContainer;

	// Token: 0x040049C8 RID: 18888
	[SerializeField]
	private HierarchyReferences rowPrefab;

	// Token: 0x040049C9 RID: 18889
	private List<HierarchyReferences> rows = new List<HierarchyReferences>();

	// Token: 0x040049CA RID: 18890
	private GeneticAnalysisStation.StatesInstance target;

	// Token: 0x040049CB RID: 18891
	private Dictionary<Tag, bool> expandedSeeds = new Dictionary<Tag, bool>();
}
