using System;
using UnityEngine;

// Token: 0x0200068E RID: 1678
public class BuildingFacadeAnimateIn : MonoBehaviour
{
	// Token: 0x060029DC RID: 10716 RVA: 0x000EBED4 File Offset: 0x000EA0D4
	private void Awake()
	{
		this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.updater = Updater.Series(new Updater[]
		{
			KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController),
			Updater.Do(delegate()
			{
				UnityEngine.Object.Destroy(base.gameObject);
			})
		});
	}

	// Token: 0x060029DD RID: 10717 RVA: 0x000EBF68 File Offset: 0x000EA168
	private void Update()
	{
		if (this.sourceAnimController.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, false);
		this.updater.Internal_Update(Time.unscaledDeltaTime);
	}

	// Token: 0x060029DE RID: 10718 RVA: 0x000EBFA0 File Offset: 0x000EA1A0
	private void OnDisable()
	{
		if (!this.sourceAnimController.IsNullOrDestroyed())
		{
			BuildingFacadeAnimateIn.SetVisibilityOn(this.sourceAnimController, true);
		}
		UnityEngine.Object.Destroy(this.placeAnimController.gameObject);
		UnityEngine.Object.Destroy(this.colorAnimController.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060029DF RID: 10719 RVA: 0x000EBFF4 File Offset: 0x000EA1F4
	public static BuildingFacadeAnimateIn MakeFor(KBatchedAnimController sourceAnimController)
	{
		BuildingFacadeAnimateIn.SetVisibilityOn(sourceAnimController, false);
		KBatchedAnimController kbatchedAnimController = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
		kbatchedAnimController.gameObject.name = "BuildingFacadeAnimateIn.placeAnimController";
		kbatchedAnimController.initialAnim = "place";
		KBatchedAnimController kbatchedAnimController2 = BuildingFacadeAnimateIn.SpawnAnimFrom(sourceAnimController);
		kbatchedAnimController2.gameObject.name = "BuildingFacadeAnimateIn.colorAnimController";
		kbatchedAnimController2.initialAnim = ((sourceAnimController.CurrentAnim != null) ? sourceAnimController.CurrentAnim.name : sourceAnimController.AnimFiles[0].GetData().GetAnim(0).name);
		GameObject gameObject = new GameObject("BuildingFacadeAnimateIn");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		BuildingFacadeAnimateIn buildingFacadeAnimateIn = gameObject.AddComponent<BuildingFacadeAnimateIn>();
		buildingFacadeAnimateIn.sourceAnimController = sourceAnimController;
		buildingFacadeAnimateIn.placeAnimController = kbatchedAnimController;
		buildingFacadeAnimateIn.colorAnimController = kbatchedAnimController2;
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController2.gameObject.SetActive(true);
		gameObject.SetActive(true);
		return buildingFacadeAnimateIn;
	}

	// Token: 0x060029E0 RID: 10720 RVA: 0x000EC0D8 File Offset: 0x000EA2D8
	private static void SetVisibilityOn(KBatchedAnimController animController, bool isVisible)
	{
		animController.SetVisiblity(isVisible);
		foreach (KBatchedAnimController kbatchedAnimController in animController.GetComponentsInChildren<KBatchedAnimController>(true))
		{
			if (kbatchedAnimController.batchGroupID == animController.batchGroupID)
			{
				kbatchedAnimController.SetVisiblity(isVisible);
			}
		}
	}

	// Token: 0x060029E1 RID: 10721 RVA: 0x000EC120 File Offset: 0x000EA320
	private static KBatchedAnimController SpawnAnimFrom(KBatchedAnimController sourceAnimController)
	{
		GameObject gameObject = new GameObject();
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		gameObject.transform.localPosition = sourceAnimController.transform.localPosition;
		gameObject.transform.localRotation = sourceAnimController.transform.localRotation;
		gameObject.transform.localScale = sourceAnimController.transform.localScale;
		gameObject.layer = sourceAnimController.gameObject.layer;
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.materialType = sourceAnimController.materialType;
		kbatchedAnimController.initialMode = sourceAnimController.initialMode;
		kbatchedAnimController.AnimFiles = sourceAnimController.AnimFiles;
		kbatchedAnimController.Offset = sourceAnimController.Offset;
		kbatchedAnimController.animWidth = sourceAnimController.animWidth;
		kbatchedAnimController.animHeight = sourceAnimController.animHeight;
		kbatchedAnimController.animScale = sourceAnimController.animScale;
		kbatchedAnimController.sceneLayer = sourceAnimController.sceneLayer;
		kbatchedAnimController.fgLayer = sourceAnimController.fgLayer;
		kbatchedAnimController.FlipX = sourceAnimController.FlipX;
		kbatchedAnimController.FlipY = sourceAnimController.FlipY;
		kbatchedAnimController.Rotation = sourceAnimController.Rotation;
		kbatchedAnimController.Pivot = sourceAnimController.Pivot;
		return kbatchedAnimController;
	}

	// Token: 0x0400181B RID: 6171
	private KBatchedAnimController sourceAnimController;

	// Token: 0x0400181C RID: 6172
	private KBatchedAnimController placeAnimController;

	// Token: 0x0400181D RID: 6173
	private KBatchedAnimController colorAnimController;

	// Token: 0x0400181E RID: 6174
	private Updater updater;
}
