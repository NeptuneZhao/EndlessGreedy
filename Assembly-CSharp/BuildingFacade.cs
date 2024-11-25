using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using UnityEngine;

// Token: 0x0200068D RID: 1677
[SerializationConfig(MemberSerialization.OptIn)]
public class BuildingFacade : KMonoBehaviour
{
	// Token: 0x1700022D RID: 557
	// (get) Token: 0x060029D1 RID: 10705 RVA: 0x000EBBB6 File Offset: 0x000E9DB6
	public string CurrentFacade
	{
		get
		{
			return this.currentFacade;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x060029D2 RID: 10706 RVA: 0x000EBBBE File Offset: 0x000E9DBE
	public bool IsOriginal
	{
		get
		{
			return this.currentFacade.IsNullOrWhiteSpace();
		}
	}

	// Token: 0x060029D3 RID: 10707 RVA: 0x000EBBCB File Offset: 0x000E9DCB
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x060029D4 RID: 10708 RVA: 0x000EBBCD File Offset: 0x000E9DCD
	protected override void OnSpawn()
	{
		if (!this.IsOriginal)
		{
			this.ApplyBuildingFacade(Db.GetBuildingFacades().TryGet(this.currentFacade), false);
		}
	}

	// Token: 0x060029D5 RID: 10709 RVA: 0x000EBBEE File Offset: 0x000E9DEE
	public void ApplyDefaultFacade(bool shouldTryAnimate = false)
	{
		this.currentFacade = "DEFAULT_FACADE";
		this.ClearFacade(shouldTryAnimate);
	}

	// Token: 0x060029D6 RID: 10710 RVA: 0x000EBC04 File Offset: 0x000E9E04
	public void ApplyBuildingFacade(BuildingFacadeResource facade, bool shouldTryAnimate = false)
	{
		if (facade == null)
		{
			this.ClearFacade(false);
			return;
		}
		this.currentFacade = facade.Id;
		KAnimFile[] array = new KAnimFile[]
		{
			Assets.GetAnim(facade.AnimFile)
		};
		this.ChangeBuilding(array, facade.Name, facade.Description, facade.InteractFile, shouldTryAnimate);
	}

	// Token: 0x060029D7 RID: 10711 RVA: 0x000EBC5C File Offset: 0x000E9E5C
	private void ClearFacade(bool shouldTryAnimate = false)
	{
		Building component = base.GetComponent<Building>();
		this.ChangeBuilding(component.Def.AnimFiles, component.Def.Name, component.Def.Desc, null, shouldTryAnimate);
	}

	// Token: 0x060029D8 RID: 10712 RVA: 0x000EBC9C File Offset: 0x000E9E9C
	private void ChangeBuilding(KAnimFile[] animFiles, string displayName, string desc, Dictionary<string, string> interactAnimsNames = null, bool shouldTryAnimate = false)
	{
		this.interactAnims.Clear();
		if (interactAnimsNames != null && interactAnimsNames.Count > 0)
		{
			this.interactAnims = new Dictionary<string, KAnimFile[]>();
			foreach (KeyValuePair<string, string> keyValuePair in interactAnimsNames)
			{
				this.interactAnims.Add(keyValuePair.Key, new KAnimFile[]
				{
					Assets.GetAnim(keyValuePair.Value)
				});
			}
		}
		Building[] components = base.GetComponents<Building>();
		foreach (Building building in components)
		{
			building.SetDescriptionFlavour(desc);
			KBatchedAnimController component = building.GetComponent<KBatchedAnimController>();
			HashedString batchGroupID = component.batchGroupID;
			component.SwapAnims(animFiles);
			foreach (KBatchedAnimController kbatchedAnimController in building.GetComponentsInChildren<KBatchedAnimController>(true))
			{
				if (kbatchedAnimController.batchGroupID == batchGroupID)
				{
					kbatchedAnimController.SwapAnims(animFiles);
				}
			}
			if (!this.animateIn.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(this.animateIn);
				this.animateIn = null;
			}
			if (shouldTryAnimate)
			{
				this.animateIn = BuildingFacadeAnimateIn.MakeFor(component);
				string parameter = "Unlocked";
				float parameterValue = 1f;
				KFMOD.PlayUISoundWithParameter(GlobalAssets.GetSound(KleiInventoryScreen.GetFacadeItemSoundName(Db.Get().Permits.TryGet(this.currentFacade)) + "_Click", false), parameter, parameterValue);
			}
		}
		base.GetComponent<KSelectable>().SetName(displayName);
		if (base.GetComponent<AnimTileable>() != null && components.Length != 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(components[0].GetExtents(), GameScenePartitioner.Instance.objectLayers[1], null);
		}
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x000EBE64 File Offset: 0x000EA064
	public string GetNextFacade()
	{
		BuildingDef def = base.GetComponent<Building>().Def;
		int num = def.AvailableFacades.FindIndex((string s) => s == this.currentFacade) + 1;
		if (num >= def.AvailableFacades.Count)
		{
			num = 0;
		}
		return def.AvailableFacades[num];
	}

	// Token: 0x04001817 RID: 6167
	[Serialize]
	private string currentFacade;

	// Token: 0x04001818 RID: 6168
	public KAnimFile[] animFiles;

	// Token: 0x04001819 RID: 6169
	public Dictionary<string, KAnimFile[]> interactAnims = new Dictionary<string, KAnimFile[]>();

	// Token: 0x0400181A RID: 6170
	private BuildingFacadeAnimateIn animateIn;
}
