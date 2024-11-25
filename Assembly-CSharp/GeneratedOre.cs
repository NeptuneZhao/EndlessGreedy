﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000316 RID: 790
public class GeneratedOre
{
	// Token: 0x06001091 RID: 4241 RVA: 0x0005D6B8 File Offset: 0x0005B8B8
	public static void LoadGeneratedOre(List<Type> types)
	{
		Type typeFromHandle = typeof(IOreConfig);
		HashSet<SimHashes> hashSet = new HashSet<SimHashes>();
		foreach (Type type in types)
		{
			if (typeFromHandle.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
			{
				IOreConfig oreConfig = Activator.CreateInstance(type) as IOreConfig;
				SimHashes elementID = oreConfig.ElementID;
				Element element = ElementLoader.FindElementByHash(elementID);
				if (element != null && DlcManager.IsContentSubscribed(element.dlcId))
				{
					if (elementID != SimHashes.Void)
					{
						hashSet.Add(elementID);
					}
					Assets.AddPrefab(oreConfig.CreatePrefab().GetComponent<KPrefabID>());
				}
			}
		}
		foreach (Element element2 in ElementLoader.elements)
		{
			if (element2 != null && !hashSet.Contains(element2.id) && DlcManager.IsContentSubscribed(element2.dlcId) && element2.substance != null && element2.substance.anim != null)
			{
				GameObject gameObject = null;
				if (element2.IsSolid)
				{
					gameObject = EntityTemplates.CreateSolidOreEntity(element2.id, null);
				}
				else if (element2.IsLiquid)
				{
					gameObject = EntityTemplates.CreateLiquidOreEntity(element2.id, null);
				}
				else if (element2.IsGas)
				{
					gameObject = EntityTemplates.CreateGasOreEntity(element2.id, null);
				}
				if (gameObject != null)
				{
					Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
				}
			}
		}
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x0005D86C File Offset: 0x0005BA6C
	public static SubstanceChunk CreateChunk(Element element, float mass, float temperature, byte diseaseIdx, int diseaseCount, Vector3 position)
	{
		if (temperature <= 0f)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"GeneratedOre.CreateChunk tried to create a chunk with a temperature <= 0"
			});
		}
		GameObject prefab = Assets.GetPrefab(element.tag);
		if (prefab == null)
		{
			global::Debug.LogError("Could not find prefab for element " + element.id.ToString());
		}
		SubstanceChunk component = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ore, null, 0).GetComponent<SubstanceChunk>();
		component.transform.SetPosition(position);
		component.gameObject.SetActive(true);
		PrimaryElement component2 = component.GetComponent<PrimaryElement>();
		component2.Mass = mass;
		component2.Temperature = temperature;
		component2.AddDisease(diseaseIdx, diseaseCount, "GeneratedOre.CreateChunk");
		component.GetComponent<KPrefabID>().InitializeTags(false);
		return component;
	}
}
