﻿using System;
using Database;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200092C RID: 2348
public class StampToolPreview_Prefabs : IStampToolPreviewPlugin
{
	// Token: 0x06004434 RID: 17460 RVA: 0x001833B0 File Offset: 0x001815B0
	public void Setup(StampToolPreviewContext context)
	{
		if (!context.stampTemplate.elementalOres.IsNullOrDestroyed())
		{
			foreach (Prefab prefabInfo in context.stampTemplate.elementalOres)
			{
				StampToolPreview_Prefabs.SpawnPrefab(context, prefabInfo);
			}
		}
		if (!context.stampTemplate.otherEntities.IsNullOrDestroyed())
		{
			foreach (Prefab prefabInfo2 in context.stampTemplate.otherEntities)
			{
				StampToolPreview_Prefabs.SpawnPrefab(context, prefabInfo2);
			}
		}
		if (!context.stampTemplate.buildings.IsNullOrDestroyed())
		{
			foreach (Prefab prefabInfo3 in context.stampTemplate.buildings)
			{
				StampToolPreview_Prefabs.SpawnPrefab(context, prefabInfo3);
			}
		}
		if (!context.stampTemplate.elementalOres.IsNullOrDestroyed())
		{
			foreach (Prefab prefabInfo4 in context.stampTemplate.elementalOres)
			{
				StampToolPreview_Prefabs.SpawnPrefab(context, prefabInfo4);
			}
		}
	}

	// Token: 0x06004435 RID: 17461 RVA: 0x00183528 File Offset: 0x00181728
	public static void SpawnPrefab(StampToolPreviewContext context, Prefab prefabInfo)
	{
		GameObject gameObject = Assets.TryGetPrefab(prefabInfo.id);
		if (gameObject.IsNullOrDestroyed())
		{
			return;
		}
		if (gameObject.GetComponent<Building>().IsNullOrDestroyed())
		{
			StampToolPreview_Prefabs.SpawnPrefab_Default(context, prefabInfo, gameObject);
			return;
		}
		Building component = gameObject.GetComponent<Building>();
		if (component.Def.IsTilePiece)
		{
			StampToolPreview_Prefabs.SpawnPrefab_Tile(context, prefabInfo, component);
			return;
		}
		StampToolPreview_Prefabs.SpawnPrefab_Building(context, prefabInfo, component);
	}

	// Token: 0x06004436 RID: 17462 RVA: 0x0018358C File Offset: 0x0018178C
	public static void SpawnPrefab_Tile(StampToolPreviewContext context, Prefab prefabInfo, Building buildingPrefab)
	{
		TextureAtlas textureAtlas = buildingPrefab.Def.BlockTilePlaceAtlas;
		if (textureAtlas == null)
		{
			textureAtlas = buildingPrefab.Def.BlockTileAtlas;
		}
		if (textureAtlas == null || textureAtlas.items == null || textureAtlas.items.Length < 0)
		{
			return;
		}
		GameObject gameObject;
		MeshRenderer meshRenderer;
		StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1.5f, new Vector4?(textureAtlas.items[0].uvBox));
		gameObject.name = string.Format("TilePlacer {0}", buildingPrefab.PrefabID());
		gameObject.transform.SetParent(context.previewParent.transform, false);
		gameObject.transform.SetLocalPosition(new Vector2((float)prefabInfo.location_x, (float)prefabInfo.location_y + Grid.HalfCellSizeInMeters));
		Material material = StampToolPreviewUtil.MakeMaterial(textureAtlas.texture);
		material.name = string.Format("Tile ({0}) ({1})", buildingPrefab.PrefabID(), material.name);
		meshRenderer.material = material;
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
		{
			if (!gameObject.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			if (!material.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(material);
			}
		}));
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			if (meshRenderer.IsNullOrDestroyed())
			{
				return;
			}
			meshRenderer.material.color = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
		}));
	}

	// Token: 0x06004437 RID: 17463 RVA: 0x00183708 File Offset: 0x00181908
	public static void SpawnPrefab_Building(StampToolPreviewContext context, Prefab prefabInfo, Building buildingPrefab)
	{
		int num = LayerMask.NameToLayer("Place");
		GameObject original;
		if (buildingPrefab.Def.BuildingPreview.IsNullOrDestroyed())
		{
			original = BuildingLoader.Instance.CreateBuildingPreview(buildingPrefab.Def);
		}
		else
		{
			original = buildingPrefab.Def.BuildingPreview;
		}
		Building spawn = GameUtil.KInstantiate(original, Vector3.zero, Grid.SceneLayer.Building, null, num).GetComponent<Building>();
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
		{
			if (spawn.IsNullOrDestroyed())
			{
				return;
			}
			UnityEngine.Object.Destroy(spawn.gameObject);
		}));
		Rotatable component = spawn.GetComponent<Rotatable>();
		if (component != null)
		{
			component.SetOrientation(prefabInfo.rotationOrientation);
		}
		KBatchedAnimController kanim = spawn.GetComponent<KBatchedAnimController>();
		if (kanim != null)
		{
			kanim.visibilityType = KAnimControllerBase.VisibilityType.Always;
			kanim.isMovable = true;
			kanim.Offset = buildingPrefab.Def.GetVisualizerOffset();
			kanim.name = kanim.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
			kanim.TintColour = StampToolPreviewUtil.COLOR_OK;
			kanim.SetLayer(num);
		}
		spawn.transform.SetParent(context.previewParent.transform, false);
		spawn.transform.SetLocalPosition(new Vector2((float)prefabInfo.location_x, (float)prefabInfo.location_y));
		context.frameAfterSetupFn = (System.Action)Delegate.Combine(context.frameAfterSetupFn, new System.Action(delegate()
		{
			if (spawn.IsNullOrDestroyed())
			{
				return;
			}
			spawn.gameObject.SetActive(false);
			spawn.gameObject.SetActive(true);
			if (kanim.IsNullOrDestroyed())
			{
				return;
			}
			string text = "";
			if ((prefabInfo.connections & 1) != 0)
			{
				text += "L";
			}
			if ((prefabInfo.connections & 2) != 0)
			{
				text += "R";
			}
			if ((prefabInfo.connections & 4) != 0)
			{
				text += "U";
			}
			if ((prefabInfo.connections & 8) != 0)
			{
				text += "D";
			}
			if (text == "")
			{
				text = "None";
			}
			if (kanim != null && kanim.HasAnimation(text))
			{
				string text2 = text + "_place";
				bool flag = kanim.HasAnimation(text2);
				kanim.Play(flag ? text2 : text, KAnim.PlayMode.Loop, 1f, 0f);
			}
		}));
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			if (kanim.IsNullOrDestroyed())
			{
				return;
			}
			Color c = (error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
			if (buildingPrefab.Def.SceneLayer == Grid.SceneLayer.Backwall)
			{
				c.a = 0.2f;
			}
			kanim.TintColour = c;
		}));
		BuildingFacade component2 = spawn.GetComponent<BuildingFacade>();
		if (component2 != null && !prefabInfo.facadeId.IsNullOrWhiteSpace())
		{
			BuildingFacadeResource buildingFacadeResource = Db.GetBuildingFacades().TryGet(prefabInfo.facadeId);
			if (buildingFacadeResource != null && buildingFacadeResource.IsUnlocked())
			{
				component2.ApplyBuildingFacade(buildingFacadeResource, false);
			}
		}
	}

	// Token: 0x06004438 RID: 17464 RVA: 0x00183958 File Offset: 0x00181B58
	public static void SpawnPrefab_Default(StampToolPreviewContext context, Prefab prefabInfo, GameObject prefab)
	{
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		if (component == null)
		{
			return;
		}
		string name = prefab.GetComponent<KPrefabID>().GetDebugName() + "_visualizer";
		int layer = LayerMask.NameToLayer("Place");
		GameObject spawn = new GameObject(name);
		spawn.SetActive(false);
		KBatchedAnimController kanim = spawn.AddComponent<KBatchedAnimController>();
		if (!component.IsNullOrDestroyed())
		{
			kanim.AnimFiles = component.AnimFiles;
			kanim.visibilityType = KAnimControllerBase.VisibilityType.Always;
			kanim.isMovable = true;
			kanim.name = name;
			kanim.TintColour = StampToolPreviewUtil.COLOR_OK;
			kanim.SetLayer(layer);
		}
		spawn.transform.SetParent(context.previewParent.transform, false);
		OccupyArea component2 = prefab.GetComponent<OccupyArea>();
		int num;
		if (component2.IsNullOrDestroyed() || component2._UnrotatedOccupiedCellsOffsets.Length == 0)
		{
			num = 0;
		}
		else
		{
			int num2 = int.MaxValue;
			int num3 = int.MinValue;
			foreach (CellOffset cellOffset in component2._UnrotatedOccupiedCellsOffsets)
			{
				if (cellOffset.x < num2)
				{
					num2 = cellOffset.x;
				}
				if (cellOffset.x > num3)
				{
					num3 = cellOffset.x;
				}
			}
			num = num3 - num2 + 1;
		}
		if (num != 0 && num % 2 == 0)
		{
			spawn.transform.SetLocalPosition(new Vector2((float)prefabInfo.location_x + Grid.HalfCellSizeInMeters, (float)prefabInfo.location_y));
		}
		else
		{
			spawn.transform.SetLocalPosition(new Vector2((float)prefabInfo.location_x, (float)prefabInfo.location_y));
		}
		context.frameAfterSetupFn = (System.Action)Delegate.Combine(context.frameAfterSetupFn, new System.Action(delegate()
		{
			if (spawn.IsNullOrDestroyed())
			{
				return;
			}
			spawn.gameObject.SetActive(false);
			spawn.gameObject.SetActive(true);
			if (kanim.IsNullOrDestroyed())
			{
				return;
			}
			kanim.Play("place", KAnim.PlayMode.Loop, 1f, 0f);
		}));
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
		{
			if (spawn.IsNullOrDestroyed())
			{
				return;
			}
			UnityEngine.Object.Destroy(spawn.gameObject);
		}));
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			if (kanim.IsNullOrDestroyed())
			{
				return;
			}
			kanim.TintColour = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
		}));
	}
}
