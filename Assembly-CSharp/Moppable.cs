using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200058D RID: 1421
[AddComponentMenu("KMonoBehaviour/Workable/Moppable")]
public class Moppable : Workable, ISim1000ms, ISim200ms
{
	// Token: 0x06002121 RID: 8481 RVA: 0x000B9B68 File Offset: 0x000B7D68
	private Moppable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x000B9BC4 File Offset: 0x000B7DC4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.childRenderer = base.GetComponentInChildren<MeshRenderer>();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000B9C48 File Offset: 0x000B7E48
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.IsThereLiquid())
		{
			base.gameObject.DeleteObject();
			return;
		}
		Grid.Objects[Grid.PosToCell(base.gameObject), 8] = base.gameObject;
		new WorkChore<Moppable>(Db.Get().ChoreTypes.Mop, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForMop, null);
		base.Subscribe<Moppable>(493375141, Moppable.OnRefreshUserMenuDelegate);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_mop_dirtywater_kanim")
		};
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Moppable.OnSpawn", base.gameObject, new Extents(Grid.PosToCell(this), new CellOffset[]
		{
			new CellOffset(0, 0)
		}), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		this.Refresh();
		base.Subscribe<Moppable>(-1432940121, Moppable.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(this).StartSM();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x000B9D94 File Offset: 0x000B7F94
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELMOP.NAME, new System.Action(this.OnCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELMOP.TOOLTIP, true), 1f);
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x000B9DEE File Offset: 0x000B7FEE
	private void OnCancel()
	{
		DetailsScreen.Instance.Show(false);
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x000B9E0C File Offset: 0x000B800C
	protected override void OnStartWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Add(this, false);
		this.Refresh();
		this.MopTick(this.amountMoppedPerTick);
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x000B9E2C File Offset: 0x000B802C
	protected override void OnStopWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06002128 RID: 8488 RVA: 0x000B9E39 File Offset: 0x000B8039
	protected override void OnCompleteWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06002129 RID: 8489 RVA: 0x000B9E46 File Offset: 0x000B8046
	public override bool InstantlyFinish(WorkerBase worker)
	{
		this.MopTick(1000f);
		return true;
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x000B9E54 File Offset: 0x000B8054
	public void Sim1000ms(float dt)
	{
		if (this.amountMopped > 0f)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, GameUtil.GetFormattedMass(-this.amountMopped, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), base.transform, 1.5f, false);
			this.amountMopped = 0f;
		}
	}

	// Token: 0x0600212B RID: 8491 RVA: 0x000B9EAE File Offset: 0x000B80AE
	public void Sim200ms(float dt)
	{
		if (base.worker != null)
		{
			this.Refresh();
			this.MopTick(this.amountMoppedPerTick);
		}
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x000B9ED0 File Offset: 0x000B80D0
	private void OnCellMopped(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		if (this == null)
		{
			return;
		}
		if (mass_cb_info.mass > 0f)
		{
			this.amountMopped += mass_cb_info.mass;
			int cell = Grid.PosToCell(this);
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(ElementLoader.elements[(int)mass_cb_info.elemIdx], mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
			substanceChunk.transform.SetPosition(substanceChunk.transform.GetPosition() + new Vector3((UnityEngine.Random.value - 0.5f) * 0.5f, 0f, 0f));
		}
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x000B9F88 File Offset: 0x000B8188
	public static void MopCell(int cell, float amount, Action<Sim.MassConsumedCallback, object> cb)
	{
		if (Grid.Element[cell].IsLiquid)
		{
			int callbackIdx = -1;
			if (cb != null)
			{
				callbackIdx = Game.Instance.massConsumedCallbackManager.Add(cb, null, "Moppable").index;
			}
			SimMessages.ConsumeMass(cell, Grid.Element[cell].id, amount, 1, callbackIdx);
		}
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x000B9FDC File Offset: 0x000B81DC
	private void MopTick(float mopAmount)
	{
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid)
			{
				Moppable.MopCell(num, mopAmount, new Action<Sim.MassConsumedCallback, object>(this.OnCellMopped));
			}
		}
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x000BA038 File Offset: 0x000B8238
	private bool IsThereLiquid()
	{
		int cell = Grid.PosToCell(this);
		bool result = false;
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid && Grid.Mass[num] <= MopTool.maxMopAmt)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x000BA098 File Offset: 0x000B8298
	private void Refresh()
	{
		if (!this.IsThereLiquid())
		{
			if (!this.destroyHandle.IsValid)
			{
				this.destroyHandle = GameScheduler.Instance.Schedule("DestroyMoppable", 1f, delegate(object moppable)
				{
					this.TryDestroy();
				}, this, null);
				return;
			}
		}
		else if (this.destroyHandle.IsValid)
		{
			this.destroyHandle.ClearScheduler();
		}
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x000BA0FB File Offset: 0x000B82FB
	private void OnLiquidChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x000BA103 File Offset: 0x000B8303
	private void TryDestroy()
	{
		if (this != null)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x000BA119 File Offset: 0x000B8319
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x000BA134 File Offset: 0x000B8334
	private void OnReachableChanged(object data)
	{
		if (this.childRenderer != null)
		{
			Material material = this.childRenderer.material;
			bool flag = (bool)data;
			if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
			{
				return;
			}
			KSelectable component = base.GetComponent<KSelectable>();
			if (flag)
			{
				material.color = Game.Instance.uiColours.Dig.validLocation;
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, false);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, this);
			GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true);
			}, null, null);
			material.color = Game.Instance.uiColours.Dig.unreachable;
		}
	}

	// Token: 0x04001299 RID: 4761
	[MyCmpReq]
	private KSelectable Selectable;

	// Token: 0x0400129A RID: 4762
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x0400129B RID: 4763
	public float amountMoppedPerTick = 1000f;

	// Token: 0x0400129C RID: 4764
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400129D RID: 4765
	private SchedulerHandle destroyHandle;

	// Token: 0x0400129E RID: 4766
	private float amountMopped;

	// Token: 0x0400129F RID: 4767
	private MeshRenderer childRenderer;

	// Token: 0x040012A0 RID: 4768
	private CellOffset[] offsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x040012A1 RID: 4769
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040012A2 RID: 4770
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnReachableChanged(data);
	});
}
