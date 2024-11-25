using System;
using System.Collections.Generic;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200092B RID: 2347
public class StampToolPreview_Placers : IStampToolPreviewPlugin
{
	// Token: 0x06004430 RID: 17456 RVA: 0x00183198 File Offset: 0x00181398
	public StampToolPreview_Placers(GameObject placerPrefab)
	{
		StampToolPreview_Placers <>4__this = this;
		this.pool = new GameObjectPool(delegate()
		{
			if (<>4__this.poolParent == null)
			{
				<>4__this.poolParent = new GameObject("StampToolPreview::PlacerPool").transform;
			}
			GameObject gameObject = Util.KInstantiate(placerPrefab, <>4__this.poolParent.gameObject, null);
			gameObject.SetActive(false);
			return gameObject;
		}, 0);
	}

	// Token: 0x06004431 RID: 17457 RVA: 0x001831E4 File Offset: 0x001813E4
	public void Setup(StampToolPreviewContext context)
	{
		for (int i = 0; i < context.stampTemplate.cells.Count; i++)
		{
			Cell cell = context.stampTemplate.cells[i];
			GameObject instance = this.pool.GetInstance();
			instance.transform.SetParent(context.previewParent.transform, false);
			instance.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y);
			instance.SetActive(true);
			this.inUse.Add(instance);
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			foreach (GameObject gameObject in this.inUse)
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					gameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
				}
			}
		}));
		context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
		{
			foreach (GameObject gameObject in this.inUse)
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					gameObject.SetActive(false);
					gameObject.transform.SetParent(this.poolParent);
					this.pool.ReleaseInstance(gameObject);
				}
			}
			this.inUse.Clear();
		}));
	}

	// Token: 0x04002CAD RID: 11437
	private List<GameObject> inUse = new List<GameObject>();

	// Token: 0x04002CAE RID: 11438
	private GameObjectPool pool;

	// Token: 0x04002CAF RID: 11439
	private Transform poolParent;
}
