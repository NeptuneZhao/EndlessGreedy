using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000CA5 RID: 3237
[AddComponentMenu("KMonoBehaviour/scripts/CrewPortrait")]
[Serializable]
public class CrewPortrait : KMonoBehaviour
{
	// Token: 0x17000747 RID: 1863
	// (get) Token: 0x060063C5 RID: 25541 RVA: 0x002533D1 File Offset: 0x002515D1
	// (set) Token: 0x060063C6 RID: 25542 RVA: 0x002533D9 File Offset: 0x002515D9
	public IAssignableIdentity identityObject { get; private set; }

	// Token: 0x060063C7 RID: 25543 RVA: 0x002533E2 File Offset: 0x002515E2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.startTransparent)
		{
			base.StartCoroutine(this.AlphaIn());
		}
		this.requiresRefresh = true;
	}

	// Token: 0x060063C8 RID: 25544 RVA: 0x00253406 File Offset: 0x00251606
	private IEnumerator AlphaIn()
	{
		this.SetAlpha(0f);
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime * 4f)
		{
			this.SetAlpha(i);
			yield return 0;
		}
		this.SetAlpha(1f);
		yield break;
	}

	// Token: 0x060063C9 RID: 25545 RVA: 0x00253415 File Offset: 0x00251615
	private void OnRoleChanged(object data)
	{
		if (this.controller == null)
		{
			return;
		}
		CrewPortrait.RefreshHat(this.identityObject, this.controller);
	}

	// Token: 0x060063CA RID: 25546 RVA: 0x00253438 File Offset: 0x00251638
	private void RegisterEvents()
	{
		if (this.areEventsRegistered)
		{
			return;
		}
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Subscribe(540773776, new Action<object>(this.OnRoleChanged));
		this.areEventsRegistered = true;
	}

	// Token: 0x060063CB RID: 25547 RVA: 0x00253484 File Offset: 0x00251684
	private void UnregisterEvents()
	{
		if (!this.areEventsRegistered)
		{
			return;
		}
		this.areEventsRegistered = false;
		KMonoBehaviour kmonoBehaviour = this.identityObject as KMonoBehaviour;
		if (kmonoBehaviour == null)
		{
			return;
		}
		kmonoBehaviour.Unsubscribe(540773776, new Action<object>(this.OnRoleChanged));
	}

	// Token: 0x060063CC RID: 25548 RVA: 0x002534CE File Offset: 0x002516CE
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RegisterEvents();
		this.ForceRefresh();
	}

	// Token: 0x060063CD RID: 25549 RVA: 0x002534E2 File Offset: 0x002516E2
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.UnregisterEvents();
	}

	// Token: 0x060063CE RID: 25550 RVA: 0x002534F0 File Offset: 0x002516F0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.UnregisterEvents();
	}

	// Token: 0x060063CF RID: 25551 RVA: 0x00253500 File Offset: 0x00251700
	public void SetIdentityObject(IAssignableIdentity identity, bool jobEnabled = true)
	{
		this.UnregisterEvents();
		this.identityObject = identity;
		this.RegisterEvents();
		this.targetImage.enabled = true;
		if (this.identityObject != null)
		{
			this.targetImage.enabled = false;
		}
		if (this.useLabels && (identity is MinionIdentity || identity is MinionAssignablesProxy))
		{
			this.SetDuplicantJobTitleActive(jobEnabled);
		}
		this.requiresRefresh = true;
	}

	// Token: 0x060063D0 RID: 25552 RVA: 0x00253568 File Offset: 0x00251768
	public void SetSubTitle(string newTitle)
	{
		if (this.subTitle != null)
		{
			if (string.IsNullOrEmpty(newTitle))
			{
				this.subTitle.gameObject.SetActive(false);
				return;
			}
			this.subTitle.gameObject.SetActive(true);
			this.subTitle.SetText(newTitle);
		}
	}

	// Token: 0x060063D1 RID: 25553 RVA: 0x002535BA File Offset: 0x002517BA
	public void SetDuplicantJobTitleActive(bool state)
	{
		if (this.duplicantJob != null && this.duplicantJob.gameObject.activeInHierarchy != state)
		{
			this.duplicantJob.gameObject.SetActive(state);
		}
	}

	// Token: 0x060063D2 RID: 25554 RVA: 0x002535EE File Offset: 0x002517EE
	public void ForceRefresh()
	{
		this.requiresRefresh = true;
	}

	// Token: 0x060063D3 RID: 25555 RVA: 0x002535F7 File Offset: 0x002517F7
	public void Update()
	{
		if (this.requiresRefresh && (this.controller == null || this.controller.enabled))
		{
			this.requiresRefresh = false;
			this.Rebuild();
		}
	}

	// Token: 0x060063D4 RID: 25556 RVA: 0x0025362C File Offset: 0x0025182C
	private void Rebuild()
	{
		if (this.controller == null)
		{
			this.controller = base.GetComponentInChildren<KBatchedAnimController>();
			if (this.controller == null)
			{
				if (this.targetImage != null)
				{
					this.targetImage.enabled = true;
				}
				global::Debug.LogWarning("Controller for [" + base.name + "] null");
				return;
			}
		}
		CrewPortrait.SetPortraitData(this.identityObject, this.controller, this.useDefaultExpression);
		if (this.useLabels && this.duplicantName != null)
		{
			this.duplicantName.SetText((!this.identityObject.IsNullOrDestroyed()) ? this.identityObject.GetProperName() : "");
			if (this.identityObject is MinionIdentity && this.duplicantJob != null)
			{
				this.duplicantJob.SetText((this.identityObject != null) ? (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle() : "");
				this.duplicantJob.GetComponent<ToolTip>().toolTip = (this.identityObject as MinionIdentity).GetComponent<MinionResume>().GetSkillsSubtitle();
			}
		}
	}

	// Token: 0x060063D5 RID: 25557 RVA: 0x00253764 File Offset: 0x00251964
	private static void RefreshHat(IAssignableIdentity identityObject, KBatchedAnimController controller)
	{
		string hat_id = "";
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity != null)
		{
			hat_id = minionIdentity.GetComponent<MinionResume>().CurrentHat;
		}
		else if (identityObject as StoredMinionIdentity != null)
		{
			hat_id = (identityObject as StoredMinionIdentity).currentHat;
		}
		MinionResume.ApplyHat(hat_id, controller);
	}

	// Token: 0x060063D6 RID: 25558 RVA: 0x002537B8 File Offset: 0x002519B8
	public static void SetPortraitData(IAssignableIdentity identityObject, KBatchedAnimController controller, bool useDefaultExpression = true)
	{
		if (identityObject == null)
		{
			controller.gameObject.SetActive(false);
			return;
		}
		MinionIdentity minionIdentity = identityObject as MinionIdentity;
		if (minionIdentity == null)
		{
			MinionAssignablesProxy minionAssignablesProxy = identityObject as MinionAssignablesProxy;
			if (minionAssignablesProxy != null && minionAssignablesProxy.target != null)
			{
				minionIdentity = (minionAssignablesProxy.target as MinionIdentity);
			}
		}
		controller.gameObject.SetActive(true);
		controller.Play("ui_idle", KAnim.PlayMode.Once, 1f, 0f);
		SymbolOverrideController component = controller.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		if (minionIdentity != null)
		{
			HashSet<KAnimHashedString> hashSet = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet2 = new HashSet<KAnimHashedString>();
			Accessorizer component2 = minionIdentity.GetComponent<Accessorizer>();
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory = component2.GetAccessory(accessorySlot);
				if (accessory != null)
				{
					component.AddSymbolOverride(accessorySlot.targetSymbolId, accessory.symbol, 0);
					hashSet.Add(accessorySlot.targetSymbolId);
				}
				else
				{
					hashSet2.Add(accessorySlot.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet, true);
			controller.BatchSetSymbolsVisiblity(hashSet2, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component2.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(minionIdentity, controller);
		}
		else
		{
			HashSet<KAnimHashedString> hashSet3 = new HashSet<KAnimHashedString>();
			HashSet<KAnimHashedString> hashSet4 = new HashSet<KAnimHashedString>();
			StoredMinionIdentity storedMinionIdentity = identityObject as StoredMinionIdentity;
			if (storedMinionIdentity == null)
			{
				MinionAssignablesProxy minionAssignablesProxy2 = identityObject as MinionAssignablesProxy;
				if (minionAssignablesProxy2 != null && minionAssignablesProxy2.target != null)
				{
					storedMinionIdentity = (minionAssignablesProxy2.target as StoredMinionIdentity);
				}
			}
			if (!(storedMinionIdentity != null))
			{
				controller.gameObject.SetActive(false);
				return;
			}
			foreach (AccessorySlot accessorySlot2 in Db.Get().AccessorySlots.resources)
			{
				Accessory accessory2 = storedMinionIdentity.GetAccessory(accessorySlot2);
				if (accessory2 != null)
				{
					component.AddSymbolOverride(accessorySlot2.targetSymbolId, accessory2.symbol, 0);
					hashSet3.Add(accessorySlot2.targetSymbolId);
				}
				else
				{
					hashSet4.Add(accessorySlot2.targetSymbolId);
				}
			}
			controller.BatchSetSymbolsVisiblity(hashSet3, true);
			controller.BatchSetSymbolsVisiblity(hashSet4, false);
			component.AddSymbolOverride(Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(storedMinionIdentity.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
			CrewPortrait.RefreshHat(storedMinionIdentity, controller);
		}
		float animScale = 0.25f;
		controller.animScale = animScale;
		string s = "ui_idle";
		controller.Play(s, KAnim.PlayMode.Loop, 1f, 0f);
		controller.SetSymbolVisiblity("snapTo_neck", false);
		controller.SetSymbolVisiblity("snapTo_goggles", false);
	}

	// Token: 0x060063D7 RID: 25559 RVA: 0x00253B50 File Offset: 0x00251D50
	public void SetAlpha(float value)
	{
		if (this.controller == null)
		{
			return;
		}
		if ((float)this.controller.TintColour.a != value)
		{
			this.controller.TintColour = new Color(1f, 1f, 1f, value);
		}
	}

	// Token: 0x040043C4 RID: 17348
	public Image targetImage;

	// Token: 0x040043C5 RID: 17349
	public bool startTransparent;

	// Token: 0x040043C6 RID: 17350
	public bool useLabels = true;

	// Token: 0x040043C7 RID: 17351
	[SerializeField]
	public KBatchedAnimController controller;

	// Token: 0x040043C8 RID: 17352
	public float animScaleBase = 0.2f;

	// Token: 0x040043C9 RID: 17353
	public LocText duplicantName;

	// Token: 0x040043CA RID: 17354
	public LocText duplicantJob;

	// Token: 0x040043CB RID: 17355
	public LocText subTitle;

	// Token: 0x040043CC RID: 17356
	public bool useDefaultExpression = true;

	// Token: 0x040043CD RID: 17357
	private bool requiresRefresh;

	// Token: 0x040043CE RID: 17358
	private bool areEventsRegistered;
}
