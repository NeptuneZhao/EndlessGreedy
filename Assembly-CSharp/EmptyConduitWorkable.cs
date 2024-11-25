using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200087B RID: 2171
[AddComponentMenu("KMonoBehaviour/Workable/EmptyConduitWorkable")]
public class EmptyConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x06003CAB RID: 15531 RVA: 0x0015098C File Offset: 0x0014EB8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptyConduitWorkable>(2127324410, EmptyConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptyConduitWorkable.emptyLiquidConduitStatusItem == null)
		{
			EmptyConduitWorkable.emptyLiquidConduitStatusItem = new StatusItem("EmptyLiquidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, 66, true, null);
			EmptyConduitWorkable.emptyGasConduitStatusItem = new StatusItem("EmptyGasConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.Neutral, false, OverlayModes.GasConduits.ID, 130, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x06003CAC RID: 15532 RVA: 0x00150A80 File Offset: 0x0014EC80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x06003CAD RID: 15533 RVA: 0x00150A9C File Offset: 0x0014EC9C
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x06003CAE RID: 15534 RVA: 0x00150AD8 File Offset: 0x0014ECD8
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).mass > 0f;
	}

	// Token: 0x06003CAF RID: 15535 RVA: 0x00150B11 File Offset: 0x0014ED11
	private void CancelEmptying()
	{
		this.CleanUpVisualization();
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel");
			this.chore = null;
			this.shouldShowSkillPerkStatusItem = false;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06003CB0 RID: 15536 RVA: 0x00150B48 File Offset: 0x0014ED48
	private void CleanUpVisualization()
	{
		StatusItem statusItem = this.GetStatusItem();
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.ToggleStatusItem(statusItem, false, null);
		}
		this.elapsedTime = -1f;
		if (this.chore != null)
		{
			base.GetComponent<Prioritizable>().RemoveRef();
		}
	}

	// Token: 0x06003CB1 RID: 15537 RVA: 0x00150B94 File Offset: 0x0014ED94
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x00150BA2 File Offset: 0x0014EDA2
	private ConduitFlow GetFlowManager()
	{
		if (this.conduit.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x00150BC7 File Offset: 0x0014EDC7
	private void OnEmptyConduitCancelled(object data)
	{
		this.CancelEmptying();
	}

	// Token: 0x06003CB4 RID: 15540 RVA: 0x00150BD0 File Offset: 0x0014EDD0
	private StatusItem GetStatusItem()
	{
		ConduitType type = this.conduit.type;
		StatusItem result;
		if (type != ConduitType.Gas)
		{
			if (type != ConduitType.Liquid)
			{
				throw new ArgumentException();
			}
			result = EmptyConduitWorkable.emptyLiquidConduitStatusItem;
		}
		else
		{
			result = EmptyConduitWorkable.emptyGasConduitStatusItem;
		}
		return result;
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x00150C0C File Offset: 0x0014EE0C
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptyConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x06003CB6 RID: 15542 RVA: 0x00150C9C File Offset: 0x0014EE9C
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedTime == -1f)
		{
			return true;
		}
		bool result = false;
		this.elapsedTime += dt;
		if (!this.emptiedPipe)
		{
			if (this.elapsedTime > 4f)
			{
				this.EmptyContents();
				this.emptiedPipe = true;
				this.elapsedTime = 0f;
			}
		}
		else if (this.elapsedTime > 2f)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			if (this.GetFlowManager().GetContents(cell).mass > 0f)
			{
				this.elapsedTime = 0f;
				this.emptiedPipe = false;
			}
			else
			{
				this.CleanUpVisualization();
				this.chore = null;
				result = true;
				this.shouldShowSkillPerkStatusItem = false;
				this.UpdateStatusItem(null);
			}
		}
		return result;
	}

	// Token: 0x06003CB7 RID: 15543 RVA: 0x00150D65 File Offset: 0x0014EF65
	public override bool InstantlyFinish(WorkerBase worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x06003CB8 RID: 15544 RVA: 0x00150D74 File Offset: 0x0014EF74
	public void EmptyContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		ConduitFlow.ConduitContents conduitContents = this.GetFlowManager().RemoveElement(cell, float.PositiveInfinity);
		this.elapsedTime = 0f;
		if (conduitContents.mass > 0f && conduitContents.element != SimHashes.Vacuum)
		{
			ConduitType type = this.conduit.type;
			IChunkManager instance;
			if (type != ConduitType.Gas)
			{
				if (type != ConduitType.Liquid)
				{
					throw new ArgumentException();
				}
				instance = LiquidSourceManager.Instance;
			}
			else
			{
				instance = GasSourceManager.Instance;
			}
			instance.CreateChunk(conduitContents.element, conduitContents.mass, conduitContents.temperature, conduitContents.diseaseIdx, conduitContents.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore)).Trigger(580035959, base.worker);
		}
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x00150E33 File Offset: 0x0014F033
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x04002513 RID: 9491
	[MyCmpReq]
	private Conduit conduit;

	// Token: 0x04002514 RID: 9492
	private static StatusItem emptyLiquidConduitStatusItem;

	// Token: 0x04002515 RID: 9493
	private static StatusItem emptyGasConduitStatusItem;

	// Token: 0x04002516 RID: 9494
	private Chore chore;

	// Token: 0x04002517 RID: 9495
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x04002518 RID: 9496
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x04002519 RID: 9497
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x0400251A RID: 9498
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x0400251B RID: 9499
	private bool emptiedPipe = true;

	// Token: 0x0400251C RID: 9500
	private static readonly EventSystem.IntraObjectHandler<EmptyConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptyConduitWorkable>(delegate(EmptyConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
