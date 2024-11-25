using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000681 RID: 1665
public class ArtifactAnalysisStationWorkable : Workable
{
	// Token: 0x0600292E RID: 10542 RVA: 0x000E8F1C File Offset: 0x000E711C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyArtifact.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingArtifact;
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_artifact_analysis_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
		Components.ArtifactAnalysisStations.Add(this);
	}

	// Token: 0x0600292F RID: 10543 RVA: 0x000E8FE5 File Offset: 0x000E71E5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.animController.SetSymbolVisiblity("snapTo_artifact", false);
	}

	// Token: 0x06002930 RID: 10544 RVA: 0x000E900F File Offset: 0x000E720F
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.ArtifactAnalysisStations.Remove(this);
	}

	// Token: 0x06002931 RID: 10545 RVA: 0x000E9022 File Offset: 0x000E7222
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		this.InitialDisplayStoredArtifact();
	}

	// Token: 0x06002932 RID: 10546 RVA: 0x000E9031 File Offset: 0x000E7231
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		this.PositionArtifact();
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002933 RID: 10547 RVA: 0x000E9044 File Offset: 0x000E7244
	private void InitialDisplayStoredArtifact()
	{
		GameObject gameObject = base.GetComponent<Storage>().GetItems()[0];
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		gameObject.transform.SetPosition(new Vector3(base.transform.position.x, base.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)));
		gameObject.SetActive(true);
		component.enabled = false;
		component.enabled = true;
		this.PositionArtifact();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, gameObject);
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x000E90F0 File Offset: 0x000E72F0
	private void ReleaseStoredArtifact()
	{
		Storage component = base.GetComponent<Storage>();
		GameObject gameObject = component.GetItems()[0];
		KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
		gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Ore)));
		component2.enabled = false;
		component2.enabled = true;
		component.Drop(gameObject, true);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, gameObject);
	}

	// Token: 0x06002935 RID: 10549 RVA: 0x000E9184 File Offset: 0x000E7384
	private void PositionArtifact()
	{
		GameObject gameObject = base.GetComponent<Storage>().GetItems()[0];
		bool flag;
		Vector3 position = this.animController.GetSymbolTransform("snapTo_artifact", out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
		gameObject.transform.SetPosition(position);
	}

	// Token: 0x06002936 RID: 10550 RVA: 0x000E91E2 File Offset: 0x000E73E2
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.ConsumeCharm();
		this.ReleaseStoredArtifact();
	}

	// Token: 0x06002937 RID: 10551 RVA: 0x000E91F8 File Offset: 0x000E73F8
	private void ConsumeCharm()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.CharmedArtifact);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"ArtifactAnalysisStation finished studying a charmed artifact but there is not one in its storage"
		});
		if (gameObject != null)
		{
			this.YieldPayload(gameObject.GetComponent<SpaceArtifact>());
			gameObject.GetComponent<SpaceArtifact>().RemoveCharm();
		}
		if (ArtifactSelector.Instance.RecordArtifactAnalyzed(gameObject.GetComponent<KPrefabID>().PrefabID().ToString()))
		{
			if (gameObject.HasTag(GameTags.TerrestrialArtifact))
			{
				ArtifactSelector.Instance.IncrementAnalyzedTerrestrialArtifacts();
				return;
			}
			ArtifactSelector.Instance.IncrementAnalyzedSpaceArtifacts();
		}
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x000E9298 File Offset: 0x000E7498
	private void YieldPayload(SpaceArtifact artifact)
	{
		if (this.nextYeildRoll == -1f)
		{
			this.nextYeildRoll = UnityEngine.Random.Range(0f, 1f);
		}
		if (this.nextYeildRoll <= artifact.GetArtifactTier().payloadDropChance)
		{
			GameUtil.KInstantiate(Assets.GetPrefab("GeneShufflerRecharge"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore, null, 0).SetActive(true);
		}
		int num = Mathf.FloorToInt(artifact.GetArtifactTier().payloadDropChance * 20f);
		for (int i = 0; i < num; i++)
		{
			GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore, null, 0).SetActive(true);
		}
		this.nextYeildRoll = UnityEngine.Random.Range(0f, 1f);
	}

	// Token: 0x040017B6 RID: 6070
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x040017B7 RID: 6071
	[MyCmpReq]
	public Storage storage;

	// Token: 0x040017B8 RID: 6072
	[SerializeField]
	public Vector3 finishedArtifactDropOffset;

	// Token: 0x040017B9 RID: 6073
	private Notification notification;

	// Token: 0x040017BA RID: 6074
	public ArtifactAnalysisStation.StatesInstance statesInstance;

	// Token: 0x040017BB RID: 6075
	private KBatchedAnimController animController;

	// Token: 0x040017BC RID: 6076
	[Serialize]
	private float nextYeildRoll = -1f;
}
