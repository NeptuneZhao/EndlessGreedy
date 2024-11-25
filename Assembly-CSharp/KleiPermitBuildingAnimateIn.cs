using System;
using UnityEngine;

// Token: 0x02000C83 RID: 3203
public class KleiPermitBuildingAnimateIn : MonoBehaviour
{
	// Token: 0x06006291 RID: 25233 RVA: 0x0024C9CC File Offset: 0x0024ABCC
	private void Awake()
	{
		this.placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 1);
		this.updater = Updater.Parallel(new Updater[]
		{
			KleiPermitBuildingAnimateIn.MakeAnimInUpdater(this.sourceAnimController, this.placeAnimController, this.colorAnimController),
			this.extraUpdater
		});
	}

	// Token: 0x06006292 RID: 25234 RVA: 0x0024CA55 File Offset: 0x0024AC55
	private void Update()
	{
		this.sourceAnimController.gameObject.SetActive(false);
		this.updater.Internal_Update(Time.unscaledDeltaTime);
	}

	// Token: 0x06006293 RID: 25235 RVA: 0x0024CA79 File Offset: 0x0024AC79
	private void OnDisable()
	{
		this.sourceAnimController.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(this.placeAnimController.gameObject);
		UnityEngine.Object.Destroy(this.colorAnimController.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06006294 RID: 25236 RVA: 0x0024CAB8 File Offset: 0x0024ACB8
	public static KleiPermitBuildingAnimateIn MakeFor(KBatchedAnimController sourceAnimController, Updater extraUpdater = default(Updater))
	{
		sourceAnimController.gameObject.SetActive(false);
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
		kbatchedAnimController.gameObject.name = "KleiPermitBuildingAnimateIn.placeAnimController";
		kbatchedAnimController.initialAnim = "place";
		KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(sourceAnimController, sourceAnimController.transform.parent, false);
		kbatchedAnimController2.gameObject.name = "KleiPermitBuildingAnimateIn.colorAnimController";
		KAnimFileData data = sourceAnimController.AnimFiles[0].GetData();
		KAnim.Anim anim = data.GetAnim("idle");
		if (anim == null)
		{
			anim = data.GetAnim("off");
			if (anim == null)
			{
				anim = data.GetAnim(0);
			}
		}
		kbatchedAnimController2.initialAnim = anim.name;
		GameObject gameObject = new GameObject("KleiPermitBuildingAnimateIn");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(sourceAnimController.transform.parent, false);
		KleiPermitBuildingAnimateIn kleiPermitBuildingAnimateIn = gameObject.AddComponent<KleiPermitBuildingAnimateIn>();
		kleiPermitBuildingAnimateIn.sourceAnimController = sourceAnimController;
		kleiPermitBuildingAnimateIn.placeAnimController = kbatchedAnimController;
		kleiPermitBuildingAnimateIn.colorAnimController = kbatchedAnimController2;
		kleiPermitBuildingAnimateIn.extraUpdater = ((extraUpdater.fn == null) ? Updater.None() : extraUpdater);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController2.gameObject.SetActive(true);
		gameObject.SetActive(true);
		return kleiPermitBuildingAnimateIn;
	}

	// Token: 0x06006295 RID: 25237 RVA: 0x0024CBE0 File Offset: 0x0024ADE0
	public static Updater MakeAnimInUpdater(KBatchedAnimController sourceAnimController, KBatchedAnimController placeAnimController, KBatchedAnimController colorAnimController)
	{
		return Updater.Parallel(new Updater[]
		{
			Updater.Series(new Updater[]
			{
				Updater.Ease(delegate(float alpha)
				{
					placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
				}, 1f, 255f, 0.1f, Easing.CubicOut, -1f),
				Updater.Ease(delegate(float alpha)
				{
					placeAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(255f - alpha, 1f, 255f));
					colorAnimController.TintColour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)Mathf.Clamp(alpha, 1f, 255f));
				}, 1f, 255f, 0.3f, Easing.CubicIn, -1f)
			}),
			Updater.Series(new Updater[]
			{
				Updater.Ease(delegate(float scale)
				{
					scale = sourceAnimController.transform.localScale.x * scale;
					placeAnimController.transform.localScale = Vector3.one * scale;
					colorAnimController.transform.localScale = Vector3.one * scale;
				}, 1f, 1.012f, 0.2f, Easing.CubicOut, -1f),
				Updater.Ease(delegate(float scale)
				{
					scale = sourceAnimController.transform.localScale.x * scale;
					placeAnimController.transform.localScale = Vector3.one * scale;
					colorAnimController.transform.localScale = Vector3.one * scale;
				}, 1.012f, 1f, 0.1f, Easing.CubicIn, -1f)
			})
		});
	}

	// Token: 0x040042DA RID: 17114
	private KBatchedAnimController sourceAnimController;

	// Token: 0x040042DB RID: 17115
	private KBatchedAnimController placeAnimController;

	// Token: 0x040042DC RID: 17116
	private KBatchedAnimController colorAnimController;

	// Token: 0x040042DD RID: 17117
	private Updater updater;

	// Token: 0x040042DE RID: 17118
	private Updater extraUpdater;
}
