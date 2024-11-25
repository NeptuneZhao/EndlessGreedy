using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000AF6 RID: 2806
public class ClusterMapVisualizer : KMonoBehaviour
{
	// Token: 0x060053BE RID: 21438 RVA: 0x001E028C File Offset: 0x001DE48C
	public void Init(ClusterGridEntity entity, ClusterMapPathDrawer pathDrawer)
	{
		this.entity = entity;
		this.pathDrawer = pathDrawer;
		this.animControllers = new List<KBatchedAnimController>();
		if (this.animContainer == null)
		{
			GameObject gameObject = new GameObject("AnimContainer", new Type[]
			{
				typeof(RectTransform)
			});
			RectTransform component = base.GetComponent<RectTransform>();
			RectTransform component2 = gameObject.GetComponent<RectTransform>();
			component2.SetParent(component, false);
			component2.SetLocalPosition(new Vector3(0f, 0f, 0f));
			component2.sizeDelta = component.sizeDelta;
			component2.localScale = Vector3.one;
			this.animContainer = component2;
		}
		Vector3 position = ClusterGrid.Instance.GetPosition(entity);
		this.rectTransform().SetLocalPosition(position);
		this.RefreshPathDrawing();
		entity.Subscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
	}

	// Token: 0x060053BF RID: 21439 RVA: 0x001E0362 File Offset: 0x001DE562
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.doesTransitionAnimation)
		{
			new ClusterMapTravelAnimator.StatesInstance(this, this.entity).StartSM();
		}
	}

	// Token: 0x060053C0 RID: 21440 RVA: 0x001E0384 File Offset: 0x001DE584
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.entity != null)
		{
			if (this.doesTransitionAnimation)
			{
				base.gameObject.GetSMI<ClusterMapTravelAnimator.StatesInstance>().keepRotationOnIdle = this.entity.KeepRotationWhenSpacingOutInHex();
			}
			if (this.entity is Clustercraft)
			{
				new ClusterMapRocketAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity is BallisticClusterGridEntity)
			{
				new ClusterMapBallisticAnimator.StatesInstance(this, this.entity).StartSM();
				return;
			}
			if (this.entity.Layer == EntityLayer.FX)
			{
				new ClusterMapFXAnimator.StatesInstance(this, this.entity).StartSM();
			}
		}
	}

	// Token: 0x060053C1 RID: 21441 RVA: 0x001E0428 File Offset: 0x001DE628
	protected override void OnCleanUp()
	{
		if (this.entity != null)
		{
			this.entity.Unsubscribe(543433792, new Action<object>(this.OnClusterDestinationChanged));
		}
		base.OnCleanUp();
	}

	// Token: 0x060053C2 RID: 21442 RVA: 0x001E045A File Offset: 0x001DE65A
	private void OnClusterDestinationChanged(object data)
	{
		this.RefreshPathDrawing();
	}

	// Token: 0x060053C3 RID: 21443 RVA: 0x001E0464 File Offset: 0x001DE664
	public void Select(bool selected)
	{
		if (this.animControllers == null || this.animControllers.Count == 0)
		{
			return;
		}
		if (!selected == this.isSelected)
		{
			this.isSelected = selected;
			this.RefreshPathDrawing();
		}
		this.GetFirstAnimController().SetSymbolVisiblity("selected", selected);
	}

	// Token: 0x060053C4 RID: 21444 RVA: 0x001E04B6 File Offset: 0x001DE6B6
	public void PlayAnim(string animName, KAnim.PlayMode playMode)
	{
		if (this.animControllers.Count > 0)
		{
			this.GetFirstAnimController().Play(animName, playMode, 1f, 0f);
		}
	}

	// Token: 0x060053C5 RID: 21445 RVA: 0x001E04E2 File Offset: 0x001DE6E2
	public KBatchedAnimController GetFirstAnimController()
	{
		return this.GetAnimController(0);
	}

	// Token: 0x060053C6 RID: 21446 RVA: 0x001E04EB File Offset: 0x001DE6EB
	public KBatchedAnimController GetAnimController(int index)
	{
		if (index < this.animControllers.Count)
		{
			return this.animControllers[index];
		}
		return null;
	}

	// Token: 0x060053C7 RID: 21447 RVA: 0x001E0509 File Offset: 0x001DE709
	public void ManualAddAnimController(KBatchedAnimController externalAnimController)
	{
		this.animControllers.Add(externalAnimController);
	}

	// Token: 0x060053C8 RID: 21448 RVA: 0x001E0518 File Offset: 0x001DE718
	public void Show(ClusterRevealLevel level)
	{
		if (!this.entity.IsVisible)
		{
			level = ClusterRevealLevel.Hidden;
		}
		if (level == this.lastRevealLevel)
		{
			return;
		}
		this.lastRevealLevel = level;
		switch (level)
		{
		case ClusterRevealLevel.Hidden:
			base.gameObject.SetActive(false);
			break;
		case ClusterRevealLevel.Peeked:
		{
			this.ClearAnimControllers();
			KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.peekControllerPrefab, this.animContainer);
			kbatchedAnimController.gameObject.SetActive(true);
			this.animControllers.Add(kbatchedAnimController);
			base.gameObject.SetActive(true);
			break;
		}
		case ClusterRevealLevel.Visible:
			this.ClearAnimControllers();
			if (this.animControllerPrefab != null && this.entity.AnimConfigs != null)
			{
				foreach (ClusterGridEntity.AnimConfig animConfig in this.entity.AnimConfigs)
				{
					KBatchedAnimController kbatchedAnimController2 = UnityEngine.Object.Instantiate<KBatchedAnimController>(this.animControllerPrefab, this.animContainer);
					kbatchedAnimController2.AnimFiles = new KAnimFile[]
					{
						animConfig.animFile
					};
					kbatchedAnimController2.initialMode = animConfig.playMode;
					kbatchedAnimController2.initialAnim = animConfig.initialAnim;
					kbatchedAnimController2.Offset = animConfig.animOffset;
					kbatchedAnimController2.gameObject.AddComponent<LoopingSounds>();
					if (animConfig.animPlaySpeedModifier != 0f)
					{
						kbatchedAnimController2.PlaySpeedMultiplier = animConfig.animPlaySpeedModifier;
					}
					if (!string.IsNullOrEmpty(animConfig.symbolSwapTarget) && !string.IsNullOrEmpty(animConfig.symbolSwapSymbol))
					{
						SymbolOverrideController component = kbatchedAnimController2.GetComponent<SymbolOverrideController>();
						KAnim.Build.Symbol symbol = kbatchedAnimController2.AnimFiles[0].GetData().build.GetSymbol(animConfig.symbolSwapSymbol);
						component.AddSymbolOverride(animConfig.symbolSwapTarget, symbol, 0);
					}
					kbatchedAnimController2.gameObject.SetActive(true);
					this.animControllers.Add(kbatchedAnimController2);
				}
			}
			base.gameObject.SetActive(true);
			break;
		}
		this.entity.OnClusterMapIconShown(level);
	}

	// Token: 0x060053C9 RID: 21449 RVA: 0x001E0718 File Offset: 0x001DE918
	public void RefreshPathDrawing()
	{
		if (this.entity == null)
		{
			return;
		}
		ClusterTraveler component = this.entity.GetComponent<ClusterTraveler>();
		if (component == null)
		{
			return;
		}
		List<AxialI> list = (this.entity.IsVisible && component.IsTraveling()) ? component.CurrentPath : null;
		if (list != null && list.Count > 0)
		{
			if (this.mapPath == null)
			{
				this.mapPath = this.pathDrawer.AddPath();
			}
			this.mapPath.SetPoints(ClusterMapPathDrawer.GetDrawPathList(base.transform.GetLocalPosition(), list));
			Color color;
			if (this.isSelected)
			{
				color = ClusterMapScreen.Instance.rocketSelectedPathColor;
			}
			else if (this.entity.ShowPath())
			{
				color = ClusterMapScreen.Instance.rocketPathColor;
			}
			else
			{
				color = new Color(0f, 0f, 0f, 0f);
			}
			this.mapPath.SetColor(color);
			return;
		}
		if (this.mapPath != null)
		{
			global::Util.KDestroyGameObject(this.mapPath);
			this.mapPath = null;
		}
	}

	// Token: 0x060053CA RID: 21450 RVA: 0x001E0832 File Offset: 0x001DEA32
	public void SetAnimRotation(float rotation)
	{
		this.animContainer.localRotation = Quaternion.Euler(0f, 0f, rotation);
	}

	// Token: 0x060053CB RID: 21451 RVA: 0x001E084F File Offset: 0x001DEA4F
	public float GetPathAngle()
	{
		if (this.mapPath == null)
		{
			return 0f;
		}
		return this.mapPath.GetRotationForNextSegment();
	}

	// Token: 0x060053CC RID: 21452 RVA: 0x001E0870 File Offset: 0x001DEA70
	private void ClearAnimControllers()
	{
		if (this.animControllers == null)
		{
			return;
		}
		foreach (KBatchedAnimController kbatchedAnimController in this.animControllers)
		{
			global::Util.KDestroyGameObject(kbatchedAnimController.gameObject);
		}
		this.animControllers.Clear();
	}

	// Token: 0x0400371C RID: 14108
	public KBatchedAnimController animControllerPrefab;

	// Token: 0x0400371D RID: 14109
	public KBatchedAnimController peekControllerPrefab;

	// Token: 0x0400371E RID: 14110
	public Transform nameTarget;

	// Token: 0x0400371F RID: 14111
	public AlertVignette alertVignette;

	// Token: 0x04003720 RID: 14112
	public bool doesTransitionAnimation;

	// Token: 0x04003721 RID: 14113
	[HideInInspector]
	public Transform animContainer;

	// Token: 0x04003722 RID: 14114
	private ClusterGridEntity entity;

	// Token: 0x04003723 RID: 14115
	private ClusterMapPathDrawer pathDrawer;

	// Token: 0x04003724 RID: 14116
	private ClusterMapPath mapPath;

	// Token: 0x04003725 RID: 14117
	private List<KBatchedAnimController> animControllers;

	// Token: 0x04003726 RID: 14118
	private bool isSelected;

	// Token: 0x04003727 RID: 14119
	private ClusterRevealLevel lastRevealLevel;

	// Token: 0x02001B59 RID: 7001
	private class UpdateXPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x0600A33E RID: 41790 RVA: 0x00389392 File Offset: 0x00387592
		public UpdateXPositionParameter() : base("Starmap_Position_X")
		{
		}

		// Token: 0x0600A33F RID: 41791 RVA: 0x003893B0 File Offset: 0x003875B0
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateXPositionParameter.Entry item = new ClusterMapVisualizer.UpdateXPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x0600A340 RID: 41792 RVA: 0x00389408 File Offset: 0x00387608
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateXPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().x / (float)Screen.width, false);
				}
			}
		}

		// Token: 0x0600A341 RID: 41793 RVA: 0x00389490 File Offset: 0x00387690
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04007F7E RID: 32638
		private List<ClusterMapVisualizer.UpdateXPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateXPositionParameter.Entry>();

		// Token: 0x0200261E RID: 9758
		private struct Entry
		{
			// Token: 0x0400A999 RID: 43417
			public Transform transform;

			// Token: 0x0400A99A RID: 43418
			public EventInstance ev;

			// Token: 0x0400A99B RID: 43419
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x02001B5A RID: 7002
	private class UpdateYPositionParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x0600A342 RID: 41794 RVA: 0x003894E8 File Offset: 0x003876E8
		public UpdateYPositionParameter() : base("Starmap_Position_Y")
		{
		}

		// Token: 0x0600A343 RID: 41795 RVA: 0x00389508 File Offset: 0x00387708
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateYPositionParameter.Entry item = new ClusterMapVisualizer.UpdateYPositionParameter.Entry
			{
				transform = sound.transform,
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x0600A344 RID: 41796 RVA: 0x00389560 File Offset: 0x00387760
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateYPositionParameter.Entry entry in this.entries)
			{
				if (!(entry.transform == null))
				{
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, entry.transform.GetPosition().y / (float)Screen.height, false);
				}
			}
		}

		// Token: 0x0600A345 RID: 41797 RVA: 0x003895E8 File Offset: 0x003877E8
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04007F7F RID: 32639
		private List<ClusterMapVisualizer.UpdateYPositionParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateYPositionParameter.Entry>();

		// Token: 0x0200261F RID: 9759
		private struct Entry
		{
			// Token: 0x0400A99C RID: 43420
			public Transform transform;

			// Token: 0x0400A99D RID: 43421
			public EventInstance ev;

			// Token: 0x0400A99E RID: 43422
			public PARAMETER_ID parameterId;
		}
	}

	// Token: 0x02001B5B RID: 7003
	private class UpdateZoomPercentageParameter : LoopingSoundParameterUpdater
	{
		// Token: 0x0600A346 RID: 41798 RVA: 0x00389640 File Offset: 0x00387840
		public UpdateZoomPercentageParameter() : base("Starmap_Zoom_Percentage")
		{
		}

		// Token: 0x0600A347 RID: 41799 RVA: 0x00389660 File Offset: 0x00387860
		public override void Add(LoopingSoundParameterUpdater.Sound sound)
		{
			ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry item = new ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry
			{
				ev = sound.ev,
				parameterId = sound.description.GetParameterId(base.parameter)
			};
			this.entries.Add(item);
		}

		// Token: 0x0600A348 RID: 41800 RVA: 0x003896AC File Offset: 0x003878AC
		public override void Update(float dt)
		{
			foreach (ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry entry in this.entries)
			{
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			}
		}

		// Token: 0x0600A349 RID: 41801 RVA: 0x00389718 File Offset: 0x00387918
		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].ev.handle == sound.ev.handle)
				{
					this.entries.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x04007F80 RID: 32640
		private List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry> entries = new List<ClusterMapVisualizer.UpdateZoomPercentageParameter.Entry>();

		// Token: 0x02002620 RID: 9760
		private struct Entry
		{
			// Token: 0x0400A99F RID: 43423
			public Transform transform;

			// Token: 0x0400A9A0 RID: 43424
			public EventInstance ev;

			// Token: 0x0400A9A1 RID: 43425
			public PARAMETER_ID parameterId;
		}
	}
}
