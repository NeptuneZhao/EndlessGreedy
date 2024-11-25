using System;
using UnityEngine;

// Token: 0x02000897 RID: 2199
[AddComponentMenu("KMonoBehaviour/scripts/HelmetController")]
public class HelmetController : KMonoBehaviour
{
	// Token: 0x06003DA4 RID: 15780 RVA: 0x00154B1C File Offset: 0x00152D1C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HelmetController>(-1617557748, HelmetController.OnEquippedDelegate);
		base.Subscribe<HelmetController>(-170173755, HelmetController.OnUnequippedDelegate);
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x00154B48 File Offset: 0x00152D48
	private KBatchedAnimController GetAssigneeController()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (component.assignee != null)
		{
			GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
			if (assigneeGameObject)
			{
				return assigneeGameObject.GetComponent<KBatchedAnimController>();
			}
		}
		return null;
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x00154B84 File Offset: 0x00152D84
	private GameObject GetAssigneeGameObject(IAssignableIdentity ass_id)
	{
		GameObject result = null;
		MinionAssignablesProxy minionAssignablesProxy = ass_id as MinionAssignablesProxy;
		if (minionAssignablesProxy)
		{
			result = minionAssignablesProxy.GetTargetGameObject();
		}
		else
		{
			MinionIdentity minionIdentity = ass_id as MinionIdentity;
			if (minionIdentity)
			{
				result = minionIdentity.gameObject;
			}
		}
		return result;
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x00154BC4 File Offset: 0x00152DC4
	private void OnEquipped(object data)
	{
		Equippable component = base.GetComponent<Equippable>();
		this.ShowHelmet();
		GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
		assigneeGameObject.Subscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
		assigneeGameObject.Subscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
		assigneeGameObject.Subscribe(1347184327, new Action<object>(this.OnPathAdvanced));
		this.in_tube = false;
		this.is_flying = false;
		this.owner_navigator = assigneeGameObject.GetComponent<Navigator>();
	}

	// Token: 0x06003DA8 RID: 15784 RVA: 0x00154C50 File Offset: 0x00152E50
	private void OnUnequipped(object data)
	{
		this.owner_navigator = null;
		Equippable component = base.GetComponent<Equippable>();
		if (component != null)
		{
			this.HideHelmet();
			if (component.assignee != null)
			{
				GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
				if (assigneeGameObject)
				{
					assigneeGameObject.Unsubscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
					assigneeGameObject.Unsubscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
					assigneeGameObject.Unsubscribe(1347184327, new Action<object>(this.OnPathAdvanced));
				}
			}
		}
	}

	// Token: 0x06003DA9 RID: 15785 RVA: 0x00154CDC File Offset: 0x00152EDC
	private void ShowHelmet()
	{
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return;
		}
		KAnimHashedString kanimHashedString = new KAnimHashedString("snapTo_neck");
		if (!string.IsNullOrEmpty(this.anim_file))
		{
			KAnimFile anim = Assets.GetAnim(this.anim_file);
			assigneeController.GetComponent<SymbolOverrideController>().AddSymbolOverride(kanimHashedString, anim.GetData().build.GetSymbol(kanimHashedString), 6);
		}
		assigneeController.SetSymbolVisiblity(kanimHashedString, true);
		this.is_shown = true;
		this.UpdateJets();
	}

	// Token: 0x06003DAA RID: 15786 RVA: 0x00154D60 File Offset: 0x00152F60
	private void HideHelmet()
	{
		this.is_shown = false;
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return;
		}
		KAnimHashedString kanimHashedString = "snapTo_neck";
		if (!string.IsNullOrEmpty(this.anim_file))
		{
			SymbolOverrideController component = assigneeController.GetComponent<SymbolOverrideController>();
			if (component == null)
			{
				return;
			}
			component.RemoveSymbolOverride(kanimHashedString, 6);
		}
		assigneeController.SetSymbolVisiblity(kanimHashedString, false);
		this.UpdateJets();
	}

	// Token: 0x06003DAB RID: 15787 RVA: 0x00154DCA File Offset: 0x00152FCA
	private void UpdateJets()
	{
		if (this.is_shown && this.is_flying)
		{
			this.EnableJets();
			return;
		}
		this.DisableJets();
	}

	// Token: 0x06003DAC RID: 15788 RVA: 0x00154DEC File Offset: 0x00152FEC
	private void EnableJets()
	{
		if (!this.has_jets)
		{
			return;
		}
		if (this.jet_go)
		{
			return;
		}
		this.jet_go = this.AddTrackedAnim("jet", Assets.GetAnim("jetsuit_thruster_fx_kanim"), "loop", Grid.SceneLayer.Creatures, "snapTo_neck");
		this.glow_go = this.AddTrackedAnim("glow", Assets.GetAnim("jetsuit_thruster_glow_fx_kanim"), "loop", Grid.SceneLayer.Front, "snapTo_neck");
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00154E68 File Offset: 0x00153068
	private void DisableJets()
	{
		if (!this.has_jets)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.jet_go);
		this.jet_go = null;
		UnityEngine.Object.Destroy(this.glow_go);
		this.glow_go = null;
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00154E98 File Offset: 0x00153098
	private GameObject AddTrackedAnim(string name, KAnimFile tracked_anim_file, string anim_clip, Grid.SceneLayer layer, string symbol_name)
	{
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return null;
		}
		string name2 = assigneeController.name + "." + name;
		GameObject gameObject = new GameObject(name2);
		gameObject.SetActive(false);
		gameObject.transform.parent = assigneeController.transform;
		gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name2);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			tracked_anim_file
		};
		kbatchedAnimController.initialAnim = anim_clip;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.sceneLayer = layer;
		gameObject.AddComponent<KBatchedAnimTracker>().symbol = symbol_name;
		bool flag;
		Vector3 position = assigneeController.GetSymbolTransform(symbol_name, out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(layer);
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		kbatchedAnimController.Play(anim_clip, KAnim.PlayMode.Loop, 1f, 0f);
		return gameObject;
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x00154F93 File Offset: 0x00153193
	private void OnBeginRecoverBreath(object data)
	{
		this.HideHelmet();
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00154F9B File Offset: 0x0015319B
	private void OnEndRecoverBreath(object data)
	{
		this.ShowHelmet();
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x00154FA4 File Offset: 0x001531A4
	private void OnPathAdvanced(object data)
	{
		if (this.owner_navigator == null)
		{
			return;
		}
		bool flag = this.owner_navigator.CurrentNavType == NavType.Hover;
		bool flag2 = this.owner_navigator.CurrentNavType == NavType.Tube;
		if (flag2 != this.in_tube)
		{
			this.in_tube = flag2;
			if (this.in_tube)
			{
				this.HideHelmet();
			}
			else
			{
				this.ShowHelmet();
			}
		}
		if (flag != this.is_flying)
		{
			this.is_flying = flag;
			this.UpdateJets();
		}
	}

	// Token: 0x0400259C RID: 9628
	public string anim_file;

	// Token: 0x0400259D RID: 9629
	public bool has_jets;

	// Token: 0x0400259E RID: 9630
	private bool is_shown;

	// Token: 0x0400259F RID: 9631
	private bool in_tube;

	// Token: 0x040025A0 RID: 9632
	private bool is_flying;

	// Token: 0x040025A1 RID: 9633
	private Navigator owner_navigator;

	// Token: 0x040025A2 RID: 9634
	private GameObject jet_go;

	// Token: 0x040025A3 RID: 9635
	private GameObject glow_go;

	// Token: 0x040025A4 RID: 9636
	private static readonly EventSystem.IntraObjectHandler<HelmetController> OnEquippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>(delegate(HelmetController component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040025A5 RID: 9637
	private static readonly EventSystem.IntraObjectHandler<HelmetController> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>(delegate(HelmetController component, object data)
	{
		component.OnUnequipped(data);
	});
}
