using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ABD RID: 2749
public class ClusterMapMeteorShowerVisualizer : ClusterGridEntity
{
	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06005108 RID: 20744 RVA: 0x001D1969 File Offset: 0x001CFB69
	public override string Name
	{
		get
		{
			return this.p_name;
		}
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x06005109 RID: 20745 RVA: 0x001D1971 File Offset: 0x001CFB71
	public override EntityLayer Layer
	{
		get
		{
			return EntityLayer.Craft;
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x0600510A RID: 20746 RVA: 0x001D1974 File Offset: 0x001CFB74
	public override bool IsVisible
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x0600510B RID: 20747 RVA: 0x001D1977 File Offset: 0x001CFB77
	public override ClusterRevealLevel IsVisibleInFOW
	{
		get
		{
			return ClusterRevealLevel.Peeked;
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x0600510C RID: 20748 RVA: 0x001D197C File Offset: 0x001CFB7C
	public override List<ClusterGridEntity.AnimConfig> AnimConfigs
	{
		get
		{
			return new List<ClusterGridEntity.AnimConfig>
			{
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim(this.clusterAnimName),
					initialAnim = this.AnimName,
					animPlaySpeedModifier = 0.5f
				},
				new ClusterGridEntity.AnimConfig
				{
					animFile = Assets.GetAnim("shower_identify_kanim"),
					initialAnim = "identify_off",
					playMode = KAnim.PlayMode.Once
				},
				this.questionMarkAnimConfig
			};
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x0600510D RID: 20749 RVA: 0x001D1A12 File Offset: 0x001CFC12
	public ClusterRevealLevel clusterCellRevealLevel
	{
		get
		{
			return ClusterGrid.Instance.GetCellRevealLevel(base.Location);
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x0600510E RID: 20750 RVA: 0x001D1A24 File Offset: 0x001CFC24
	public string AnimName
	{
		get
		{
			if (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return "unknown";
			}
			return "idle_loop";
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x0600510F RID: 20751 RVA: 0x001D1A42 File Offset: 0x001CFC42
	public string QuestionMarkAnimName
	{
		get
		{
			if (!this.revealed || this.clusterCellRevealLevel != ClusterRevealLevel.Visible)
			{
				return this.questionMarkAnimConfig.initialAnim;
			}
			return "off";
		}
	}

	// Token: 0x06005110 RID: 20752 RVA: 0x001D1A68 File Offset: 0x001CFC68
	public KBatchedAnimController CreateQuestionMarkInstance(KBatchedAnimController origin, Transform parent)
	{
		KBatchedAnimController kbatchedAnimController = UnityEngine.Object.Instantiate<KBatchedAnimController>(origin, parent);
		kbatchedAnimController.gameObject.SetActive(true);
		kbatchedAnimController.SwapAnims(new KAnimFile[]
		{
			this.questionMarkAnimConfig.animFile
		});
		kbatchedAnimController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		return kbatchedAnimController;
	}

	// Token: 0x06005111 RID: 20753 RVA: 0x001D1ACC File Offset: 0x001CFCCC
	protected override void OnCleanUp()
	{
		if (ClusterMapScreen.Instance != null)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			if (entityVisAnim != null)
			{
				entityVisAnim.gameObject.SetActive(false);
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06005112 RID: 20754 RVA: 0x001D1B0D File Offset: 0x001CFD0D
	public void SetInitialLocation(AxialI startLocation)
	{
		this.m_location = startLocation;
		this.RefreshVisuals();
	}

	// Token: 0x06005113 RID: 20755 RVA: 0x001D1B1C File Offset: 0x001CFD1C
	public override bool SpaceOutInSameHex()
	{
		return true;
	}

	// Token: 0x06005114 RID: 20756 RVA: 0x001D1B1F File Offset: 0x001CFD1F
	public override bool KeepRotationWhenSpacingOutInHex()
	{
		return true;
	}

	// Token: 0x06005115 RID: 20757 RVA: 0x001D1B22 File Offset: 0x001CFD22
	public override bool ShowPath()
	{
		return this.m_selectable.IsSelected;
	}

	// Token: 0x06005116 RID: 20758 RVA: 0x001D1B30 File Offset: 0x001CFD30
	public override void OnClusterMapIconShown(ClusterRevealLevel levelUsed)
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		switch (levelUsed)
		{
		case ClusterRevealLevel.Hidden:
			this.Deselect();
			break;
		case ClusterRevealLevel.Peeked:
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.SwapAnims(new KAnimFile[]
				{
					this.AnimConfigs[0].animFile
				});
				KBatchedAnimController externalAnimController = this.CreateQuestionMarkInstance(entityVisAnim.peekControllerPrefab, firstAnimController.transform.parent);
				entityVisAnim.ManualAddAnimController(externalAnimController);
			}
			this.RefreshVisuals();
			this.Deselect();
			break;
		}
		case ClusterRevealLevel.Visible:
			this.RefreshVisuals();
			break;
		}
		KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
		if (animController != null && !this.revealed)
		{
			animController.gameObject.AddOrGet<ClusterMapIconFixRotation>();
		}
	}

	// Token: 0x06005117 RID: 20759 RVA: 0x001D1BED File Offset: 0x001CFDED
	public void Deselect()
	{
		if (this.m_selectable.IsSelected)
		{
			this.m_selectable.Unselect();
		}
	}

	// Token: 0x06005118 RID: 20760 RVA: 0x001D1C08 File Offset: 0x001CFE08
	public void RefreshVisuals()
	{
		ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
		if (entityVisAnim != null)
		{
			KBatchedAnimController firstAnimController = entityVisAnim.GetFirstAnimController();
			if (firstAnimController != null)
			{
				firstAnimController.Play(this.AnimName, KAnim.PlayMode.Loop, 1f, 0f);
			}
			KBatchedAnimController animController = entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play(this.QuestionMarkAnimName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x06005119 RID: 20761 RVA: 0x001D1C84 File Offset: 0x001CFE84
	public void PlayRevealAnimation(bool playIdentifyAnimationIfVisible)
	{
		this.revealed = true;
		this.RefreshVisuals();
		if (playIdentifyAnimationIfVisible)
		{
			ClusterMapVisualizer entityVisAnim = ClusterMapScreen.Instance.GetEntityVisAnim(this);
			KBatchedAnimController animController = entityVisAnim.GetAnimController(1);
			entityVisAnim.GetAnimController(2);
			if (animController != null)
			{
				animController.Play("identify", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x0600511A RID: 20762 RVA: 0x001D1CDF File Offset: 0x001CFEDF
	public void PlayHideAnimation()
	{
		this.revealed = false;
		if (ClusterMapScreen.Instance.GetEntityVisAnim(this) != null)
		{
			this.RefreshVisuals();
		}
	}

	// Token: 0x040035D9 RID: 13785
	private ClusterGridEntity.AnimConfig questionMarkAnimConfig = new ClusterGridEntity.AnimConfig
	{
		animFile = Assets.GetAnim("shower_question_mark_kanim"),
		initialAnim = "idle",
		playMode = KAnim.PlayMode.Once
	};

	// Token: 0x040035DA RID: 13786
	public string p_name;

	// Token: 0x040035DB RID: 13787
	public string clusterAnimName;

	// Token: 0x040035DC RID: 13788
	public bool revealed;
}
