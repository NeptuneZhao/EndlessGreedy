using System;
using System.Runtime.CompilerServices;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200092D RID: 2349
public class StampToolPreview_SolidLiquidGas : IStampToolPreviewPlugin
{
	// Token: 0x0600443A RID: 17466 RVA: 0x00183BA0 File Offset: 0x00181DA0
	public void Setup(StampToolPreviewContext context)
	{
		this.SetupMaterials(context);
		using (HashSetPool<int, StampToolPreview_SolidLiquidGas>.PooledHashSet pooledHashSet = PoolsFor<StampToolPreview_SolidLiquidGas>.AllocateHashSet<int>())
		{
			if (context.stampTemplate.buildings != null)
			{
				foreach (Prefab prefab in context.stampTemplate.buildings)
				{
					if (!prefab.IsNullOrDestroyed())
					{
						GameObject prefab2 = Assets.GetPrefab(prefab.id);
						if (!prefab2.IsNullOrDestroyed())
						{
							Building component = prefab2.GetComponent<Building>();
							if (!component.IsNullOrDestroyed() && component.Def.IsTilePiece)
							{
								pooledHashSet.Add(StampToolPreview_SolidLiquidGas.CellHash(prefab.location_x, prefab.location_y));
							}
							MakeBaseSolid.Def def = prefab2.GetDef<MakeBaseSolid.Def>();
							if (!def.IsNullOrDestroyed())
							{
								foreach (CellOffset cellOffset in def.solidOffsets)
								{
									pooledHashSet.Add(StampToolPreview_SolidLiquidGas.CellHash(prefab.location_x + cellOffset.x, prefab.location_y + cellOffset.y));
								}
							}
						}
					}
				}
			}
			if (context.stampTemplate.cells != null)
			{
				for (int j = 0; j < context.stampTemplate.cells.Count; j++)
				{
					Cell cell = context.stampTemplate.cells[j];
					if (!cell.IsNullOrDestroyed() && !pooledHashSet.Contains(StampToolPreview_SolidLiquidGas.CellHash(cell.location_x, cell.location_y)))
					{
						Element element = ElementLoader.FindElementByHash(cell.element);
						Element.State? state;
						if (element == null)
						{
							state = null;
						}
						else
						{
							state = new Element.State?(element.state & Element.State.Solid);
						}
						if (state != null)
						{
							Material material;
							string str;
							switch (state.GetValueOrDefault())
							{
							case Element.State.Vacuum:
								material = StampToolPreview_SolidLiquidGas.gasMaterial;
								str = "Vacuum";
								break;
							case Element.State.Gas:
								material = StampToolPreview_SolidLiquidGas.gasMaterial;
								str = "Gas";
								break;
							case Element.State.Liquid:
								material = StampToolPreview_SolidLiquidGas.liquidMaterial;
								str = "Liquid";
								break;
							case Element.State.Solid:
								material = StampToolPreview_SolidLiquidGas.solidMaterial;
								str = "Solid";
								break;
							default:
								goto IL_2B7;
							}
							MeshRenderer meshRenderer;
							GameObject gameObject;
							StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1f, null);
							gameObject.transform.SetParent(context.previewParent, false);
							gameObject.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y + Grid.HalfCellSizeInMeters);
							context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
							{
								if (gameObject.IsNullOrDestroyed())
								{
									return;
								}
								UnityEngine.Object.Destroy(gameObject);
							}));
							gameObject.name = "TilePlacer (" + str + ")";
							meshRenderer.material = material;
						}
					}
					IL_2B7:;
				}
			}
		}
	}

	// Token: 0x0600443B RID: 17467 RVA: 0x00183EC4 File Offset: 0x001820C4
	private void SetupMaterials(StampToolPreviewContext context)
	{
		if (StampToolPreview_SolidLiquidGas.solidMaterial.IsNullOrDestroyed())
		{
			StampToolPreview_SolidLiquidGas.solidMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_solid"));
			StampToolPreview_SolidLiquidGas.solidMaterial.name = "Solid (" + StampToolPreview_SolidLiquidGas.solidMaterial.name + ")";
		}
		if (StampToolPreview_SolidLiquidGas.liquidMaterial.IsNullOrDestroyed())
		{
			StampToolPreview_SolidLiquidGas.liquidMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_liquid"));
			StampToolPreview_SolidLiquidGas.liquidMaterial.name = "Liquid (" + StampToolPreview_SolidLiquidGas.liquidMaterial.name + ")";
		}
		if (StampToolPreview_SolidLiquidGas.gasMaterial.IsNullOrDestroyed())
		{
			StampToolPreview_SolidLiquidGas.gasMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_gas"));
			StampToolPreview_SolidLiquidGas.gasMaterial.name = "Gas (" + StampToolPreview_SolidLiquidGas.gasMaterial.name + ")";
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			Color c = (error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
			if (!StampToolPreview_SolidLiquidGas.solidMaterial.IsNullOrDestroyed())
			{
				StampToolPreview_SolidLiquidGas.solidMaterial.color = StampToolPreview_SolidLiquidGas.<SetupMaterials>g__WithAlpha|4_1(c, 1f);
			}
			if (!StampToolPreview_SolidLiquidGas.liquidMaterial.IsNullOrDestroyed())
			{
				StampToolPreview_SolidLiquidGas.liquidMaterial.color = StampToolPreview_SolidLiquidGas.<SetupMaterials>g__WithAlpha|4_1(c, 1f);
			}
			if (!StampToolPreview_SolidLiquidGas.gasMaterial.IsNullOrDestroyed())
			{
				StampToolPreview_SolidLiquidGas.gasMaterial.color = StampToolPreview_SolidLiquidGas.<SetupMaterials>g__WithAlpha|4_1(c, 1f);
			}
		}));
	}

	// Token: 0x0600443C RID: 17468 RVA: 0x00183FCF File Offset: 0x001821CF
	private static int CellHash(int x, int y)
	{
		return x + y * 10000;
	}

	// Token: 0x0600443E RID: 17470 RVA: 0x00183FE2 File Offset: 0x001821E2
	[CompilerGenerated]
	internal static Color <SetupMaterials>g__WithAlpha|4_1(Color c, float a)
	{
		return new Color(c.r, c.g, c.b, a);
	}

	// Token: 0x04002CB0 RID: 11440
	public static Material solidMaterial;

	// Token: 0x04002CB1 RID: 11441
	public static Material liquidMaterial;

	// Token: 0x04002CB2 RID: 11442
	public static Material gasMaterial;
}
