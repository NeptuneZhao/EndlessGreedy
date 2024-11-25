using System;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200092A RID: 2346
public class StampToolPreview_Area : IStampToolPreviewPlugin
{
	// Token: 0x0600442E RID: 17454 RVA: 0x00183038 File Offset: 0x00181238
	public void Setup(StampToolPreviewContext context)
	{
		if (StampToolPreview_Area.material == null)
		{
			StampToolPreview_Area.material = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_background"));
			StampToolPreview_Area.material.name = "Area (" + StampToolPreview_Area.material.name + ")";
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			Color color = (error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
			color.a = 1f;
			StampToolPreview_Area.material.color = color;
		}));
		for (int i = 0; i < context.stampTemplate.cells.Count; i++)
		{
			Cell cell = context.stampTemplate.cells[i];
			MeshRenderer meshRenderer;
			GameObject gameObject;
			StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1f, null);
			gameObject.name = "AreaPlacer";
			gameObject.transform.SetParent(context.previewParent, false);
			gameObject.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y + Grid.HalfCellSizeInMeters);
			context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}));
			meshRenderer.sharedMaterial = StampToolPreview_Area.material;
		}
	}

	// Token: 0x04002CAC RID: 11436
	public static Material material;
}
