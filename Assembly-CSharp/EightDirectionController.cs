using System;
using UnityEngine;

// Token: 0x020006C5 RID: 1733
public class EightDirectionController
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06002BD5 RID: 11221 RVA: 0x000F635A File Offset: 0x000F455A
	// (set) Token: 0x06002BD6 RID: 11222 RVA: 0x000F6362 File Offset: 0x000F4562
	public KBatchedAnimController controller { get; private set; }

	// Token: 0x06002BD7 RID: 11223 RVA: 0x000F636B File Offset: 0x000F456B
	public EightDirectionController(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBank)
	{
		this.Initialize(buildingController, targetSymbol, defaultAnim, frontBank, Grid.SceneLayer.NoLayer);
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x000F6380 File Offset: 0x000F4580
	private void Initialize(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBack, Grid.SceneLayer userSpecifiedRenderLayer)
	{
		string name = buildingController.name + ".eight_direction";
		this.gameObject = new GameObject(name);
		this.gameObject.SetActive(false);
		this.gameObject.transform.parent = buildingController.transform;
		this.gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.defaultAnim = defaultAnim;
		this.controller = this.gameObject.AddOrGet<KBatchedAnimController>();
		this.controller.AnimFiles = new KAnimFile[]
		{
			buildingController.AnimFiles[0]
		};
		this.controller.initialAnim = defaultAnim;
		this.controller.isMovable = true;
		this.controller.sceneLayer = Grid.SceneLayer.NoLayer;
		if (EightDirectionController.Offset.UserSpecified == frontBack)
		{
			this.controller.sceneLayer = userSpecifiedRenderLayer;
		}
		buildingController.SetSymbolVisiblity(targetSymbol, false);
		bool flag;
		Vector3 position = buildingController.GetSymbolTransform(new HashedString(targetSymbol), out flag).GetColumn(3);
		switch (frontBack)
		{
		case EightDirectionController.Offset.Infront:
			position.z = buildingController.transform.GetPosition().z - 0.1f;
			break;
		case EightDirectionController.Offset.Behind:
			position.z = buildingController.transform.GetPosition().z + 0.1f;
			break;
		case EightDirectionController.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(userSpecifiedRenderLayer);
			break;
		}
		this.gameObject.transform.SetPosition(position);
		this.gameObject.SetActive(true);
		this.link = new KAnimLink(buildingController, this.controller);
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x000F6508 File Offset: 0x000F4708
	public void SetPositionPercent(float percent_full)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.SetPositionPercent(percent_full);
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x000F6525 File Offset: 0x000F4725
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.controller != null)
		{
			this.controller.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x000F6547 File Offset: 0x000F4747
	public void SetRotation(float rot)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.Rotation = rot;
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x000F6564 File Offset: 0x000F4764
	public void PlayAnim(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		this.controller.Play(anim, mode, 1f, 0f);
	}

	// Token: 0x04001934 RID: 6452
	public GameObject gameObject;

	// Token: 0x04001935 RID: 6453
	private string defaultAnim;

	// Token: 0x04001936 RID: 6454
	private KAnimLink link;

	// Token: 0x020014C5 RID: 5317
	public enum Offset
	{
		// Token: 0x04006AE6 RID: 27366
		Infront,
		// Token: 0x04006AE7 RID: 27367
		Behind,
		// Token: 0x04006AE8 RID: 27368
		UserSpecified
	}
}
