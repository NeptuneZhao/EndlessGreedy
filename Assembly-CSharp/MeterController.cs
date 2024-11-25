using System;
using UnityEngine;

// Token: 0x0200072B RID: 1835
public class MeterController
{
	// Token: 0x060030B2 RID: 12466 RVA: 0x0010C96F File Offset: 0x0010AB6F
	public static float StandardLerp(float percentage, int frames)
	{
		return percentage;
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x0010C972 File Offset: 0x0010AB72
	public static float MinMaxStepLerp(float percentage, int frames)
	{
		if ((double)percentage <= 0.0 || frames <= 1)
		{
			return 0f;
		}
		if ((double)percentage >= 1.0 || frames == 2)
		{
			return 1f;
		}
		return (1f + percentage * (float)(frames - 2)) / (float)frames;
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x060030B4 RID: 12468 RVA: 0x0010C9B1 File Offset: 0x0010ABB1
	// (set) Token: 0x060030B5 RID: 12469 RVA: 0x0010C9B9 File Offset: 0x0010ABB9
	public KBatchedAnimController meterController { get; private set; }

	// Token: 0x060030B6 RID: 12470 RVA: 0x0010C9C4 File Offset: 0x0010ABC4
	public MeterController(KMonoBehaviour target, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		string[] array = new string[symbols_to_hide.Length + 1];
		Array.Copy(symbols_to_hide, array, symbols_to_hide.Length);
		array[array.Length - 1] = "meter_target";
		KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
		this.Initialize(component, "meter_target", "meter", front_back, user_specified_render_layer, Vector3.zero, array);
	}

	// Token: 0x060030B7 RID: 12471 RVA: 0x0010CA2D File Offset: 0x0010AC2D
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, Vector3.zero, symbols_to_hide);
	}

	// Token: 0x060030B8 RID: 12472 RVA: 0x0010CA5B File Offset: 0x0010AC5B
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, tracker_offset, symbols_to_hide);
	}

	// Token: 0x060030B9 RID: 12473 RVA: 0x0010CA88 File Offset: 0x0010AC88
	private void Initialize(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		if (building_controller.HasAnimation(meter_animation + "_cb") && !GlobalAssets.Instance.colorSet.IsDefaultColorSet())
		{
			meter_animation += "_cb";
		}
		string name = building_controller.name + "." + meter_animation;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(MeterConfig.ID));
		gameObject.name = name;
		gameObject.SetActive(false);
		gameObject.transform.parent = building_controller.transform;
		this.gameObject = gameObject;
		gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(name);
		Vector3 position = building_controller.transform.GetPosition();
		switch (front_back)
		{
		case Meter.Offset.Infront:
			position.z -= 0.1f;
			break;
		case Meter.Offset.Behind:
			position.z += 0.1f;
			break;
		case Meter.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(user_specified_render_layer);
			break;
		}
		gameObject.transform.SetPosition(position);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.AnimFiles = new KAnimFile[]
		{
			building_controller.AnimFiles[0]
		};
		component.initialAnim = meter_animation;
		component.fgLayer = Grid.SceneLayer.NoLayer;
		component.initialMode = KAnim.PlayMode.Paused;
		component.isMovable = true;
		component.FlipX = building_controller.FlipX;
		component.FlipY = building_controller.FlipY;
		if (Meter.Offset.UserSpecified == front_back)
		{
			component.sceneLayer = user_specified_render_layer;
		}
		this.meterController = component;
		KBatchedAnimTracker component2 = gameObject.GetComponent<KBatchedAnimTracker>();
		component2.offset = tracker_offset;
		component2.symbol = new HashedString(meter_target);
		gameObject.SetActive(true);
		building_controller.SetSymbolVisiblity(meter_target, false);
		if (symbols_to_hide != null)
		{
			for (int i = 0; i < symbols_to_hide.Length; i++)
			{
				building_controller.SetSymbolVisiblity(symbols_to_hide[i], false);
			}
		}
		this.link = new KAnimLink(building_controller, component);
	}

	// Token: 0x060030BA RID: 12474 RVA: 0x0010CC58 File Offset: 0x0010AE58
	public MeterController(KAnimControllerBase building_controller, KBatchedAnimController meter_controller, params string[] symbol_names)
	{
		if (meter_controller == null)
		{
			return;
		}
		this.meterController = meter_controller;
		this.link = new KAnimLink(building_controller, meter_controller);
		for (int i = 0; i < symbol_names.Length; i++)
		{
			building_controller.SetSymbolVisiblity(symbol_names[i], false);
		}
		this.meterController.GetComponent<KBatchedAnimTracker>().symbol = new HashedString(symbol_names[0]);
	}

	// Token: 0x060030BB RID: 12475 RVA: 0x0010CCD0 File Offset: 0x0010AED0
	public void SetPositionPercent(float percent_full)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.SetPositionPercent(this.interpolateFunction(percent_full, this.meterController.GetCurrentNumFrames()));
	}

	// Token: 0x060030BC RID: 12476 RVA: 0x0010CD03 File Offset: 0x0010AF03
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.meterController != null)
		{
			this.meterController.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x060030BD RID: 12477 RVA: 0x0010CD25 File Offset: 0x0010AF25
	public void SetRotation(float rot)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.Rotation = rot;
	}

	// Token: 0x060030BE RID: 12478 RVA: 0x0010CD42 File Offset: 0x0010AF42
	public void Unlink()
	{
		if (this.link != null)
		{
			this.link.Unregister();
			this.link = null;
		}
	}

	// Token: 0x04001C93 RID: 7315
	public GameObject gameObject;

	// Token: 0x04001C94 RID: 7316
	public Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);

	// Token: 0x04001C95 RID: 7317
	private KAnimLink link;
}
