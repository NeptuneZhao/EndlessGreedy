using System;
using System.Collections;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000559 RID: 1369
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Diggable")]
public class Diggable : Workable
{
	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06001F7C RID: 8060 RVA: 0x000B0C67 File Offset: 0x000AEE67
	public bool Reachable
	{
		get
		{
			return this.isReachable;
		}
	}

	// Token: 0x06001F7D RID: 8061 RVA: 0x000B0C70 File Offset: 0x000AEE70
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Digging;
		this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.DigRequiresSkillPerk;
		this.faceTargetWhenWorking = true;
		base.Subscribe<Diggable>(-1432940121, Diggable.OnReachableChangedDelegate);
		this.attributeConverter = Db.Get().AttributeConverters.DiggingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.multitoolContext = "dig";
		this.multitoolHitEffectTag = "fx_dig_splash";
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x000B0D43 File Offset: 0x000AEF43
	private Diggable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
	}

	// Token: 0x06001F7F RID: 8063 RVA: 0x000B0D60 File Offset: 0x000AEF60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cached_cell = Grid.PosToCell(this);
		this.originalDigElement = Grid.Element[this.cached_cell];
		if (this.originalDigElement.hardness == 255)
		{
			this.OnCancel();
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForDig, null);
		this.UpdateColor(this.isReachable);
		Grid.Objects[this.cached_cell, 7] = base.gameObject;
		ChoreType chore_type = Db.Get().ChoreTypes.Dig;
		if (this.choreTypeIdHash.IsValid)
		{
			chore_type = Db.Get().ChoreTypes.GetByHash(this.choreTypeIdHash);
		}
		this.chore = new WorkChore<Diggable>(chore_type, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Diggable.OnSpawn", base.gameObject, Grid.PosToCell(this), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.OnSolidChanged(null);
		new ReachabilityMonitor.Instance(this).StartSM();
		base.Subscribe<Diggable>(493375141, Diggable.OnRefreshUserMenuDelegate);
		this.handle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		Components.Diggables.Add(this);
	}

	// Token: 0x06001F80 RID: 8064 RVA: 0x000B0EDA File Offset: 0x000AF0DA
	public override int GetCell()
	{
		return this.cached_cell;
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x000B0EE4 File Offset: 0x000AF0E4
	public override Workable.AnimInfo GetAnim(WorkerBase worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		if (this.overrideAnims != null && this.overrideAnims.Length != 0)
		{
			result.overrideAnims = this.overrideAnims;
		}
		if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
		{
			result.smi = new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
		}
		return result;
	}

	// Token: 0x06001F82 RID: 8066 RVA: 0x000B0F54 File Offset: 0x000AF154
	private static bool IsCellBuildable(int cell)
	{
		bool result = false;
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null && gameObject.GetComponent<Constructable>() != null)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x000B0F8A File Offset: 0x000AF18A
	private IEnumerator PeriodicUnstableFallingRecheck()
	{
		yield return SequenceUtil.WaitForSeconds(2f);
		this.OnSolidChanged(null);
		yield break;
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x000B0F9C File Offset: 0x000AF19C
	private void OnSolidChanged(object data)
	{
		if (this == null || base.gameObject == null)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.unstableEntry);
		int num = -1;
		this.UpdateColor(this.isReachable);
		if (Grid.Element[this.cached_cell].hardness == 255)
		{
			this.UpdateColor(false);
			this.requiredSkillPerk = null;
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigUnobtanium);
		}
		else if (Grid.Element[this.cached_cell].hardness >= 251)
		{
			bool flag = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigRadioactiveMaterials);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigRadioactiveMaterials.Id;
			this.materialDisplay.sharedMaterial = this.materials[3];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 200)
		{
			bool flag2 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag2)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigSuperDuperHard);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigSuperDuperHard.Id;
			this.materialDisplay.sharedMaterial = this.materials[3];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 150)
		{
			bool flag3 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag3 = true;
						break;
					}
				}
			}
			if (!flag3)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigNearlyImpenetrable);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigNearlyImpenetrable.Id;
			this.materialDisplay.sharedMaterial = this.materials[2];
		}
		else if (Grid.Element[this.cached_cell].hardness >= 50)
		{
			bool flag4 = false;
			using (List<Chore.PreconditionInstance>.Enumerator enumerator = this.chore.GetPreconditions().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.condition.id == ChorePreconditions.instance.HasSkillPerk.id)
					{
						flag4 = true;
						break;
					}
				}
			}
			if (!flag4)
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDigVeryFirm);
			}
			this.requiredSkillPerk = Db.Get().SkillPerks.CanDigVeryFirm.Id;
			this.materialDisplay.sharedMaterial = this.materials[1];
		}
		else
		{
			this.requiredSkillPerk = null;
			this.chore.GetPreconditions().Remove(this.chore.GetPreconditions().Find((Chore.PreconditionInstance o) => o.condition.id == ChorePreconditions.instance.HasSkillPerk.id));
		}
		this.UpdateStatusItem(null);
		bool flag5 = false;
		if (!Grid.Solid[this.cached_cell])
		{
			num = Diggable.GetUnstableCellAbove(this.cached_cell);
			if (num == -1)
			{
				flag5 = true;
			}
			else
			{
				base.StartCoroutine("PeriodicUnstableFallingRecheck");
			}
		}
		else if (Grid.Foundation[this.cached_cell])
		{
			flag5 = true;
		}
		if (!flag5)
		{
			if (num != -1)
			{
				Extents extents = default(Extents);
				Grid.CellToXY(this.cached_cell, out extents.x, out extents.y);
				extents.width = 1;
				extents.height = (num - this.cached_cell + Grid.WidthInCells - 1) / Grid.WidthInCells + 1;
				this.unstableEntry = GameScenePartitioner.Instance.Add("Diggable.OnSolidChanged", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
			}
			return;
		}
		this.isDigComplete = true;
		if (this.chore == null || !this.chore.InProgress())
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		base.GetComponentInChildren<MeshRenderer>().enabled = false;
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x000B14E8 File Offset: 0x000AF6E8
	public Element GetTargetElement()
	{
		return Grid.Element[this.cached_cell];
	}

	// Token: 0x06001F86 RID: 8070 RVA: 0x000B14F6 File Offset: 0x000AF6F6
	public override string GetConversationTopic()
	{
		return this.originalDigElement.tag.Name;
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x000B1508 File Offset: 0x000AF708
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		Diggable.DoDigTick(this.cached_cell, dt);
		return this.isDigComplete;
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x000B151C File Offset: 0x000AF71C
	protected override void OnStopWork(WorkerBase worker)
	{
		if (this.isDigComplete)
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x000B1534 File Offset: 0x000AF734
	public override bool InstantlyFinish(WorkerBase worker)
	{
		if (Grid.Element[this.cached_cell].hardness == 255)
		{
			return false;
		}
		float approximateDigTime = Diggable.GetApproximateDigTime(this.cached_cell);
		worker.Work(approximateDigTime);
		return true;
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x000B1570 File Offset: 0x000AF770
	public static void DoDigTick(int cell, float dt)
	{
		Diggable.DoDigTick(cell, dt, WorldDamage.DamageType.Absolute);
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x000B157C File Offset: 0x000AF77C
	public static void DoDigTick(int cell, float dt, WorldDamage.DamageType damageType)
	{
		float approximateDigTime = Diggable.GetApproximateDigTime(cell);
		float amount = dt / approximateDigTime;
		WorldDamage.Instance.ApplyDamage(cell, amount, -1, damageType, null, null);
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x000B15A8 File Offset: 0x000AF7A8
	public static float GetApproximateDigTime(int cell)
	{
		float num = (float)Grid.Element[cell].hardness;
		if (num == 255f)
		{
			return float.MaxValue;
		}
		Element element = ElementLoader.FindElementByHash(SimHashes.Ice);
		float num2 = num / (float)element.hardness;
		float num3 = Mathf.Min(Grid.Mass[cell], 400f) / 400f;
		float num4 = 4f * num3;
		return num4 + num2 * num4;
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x000B1614 File Offset: 0x000AF814
	public static Diggable GetDiggable(int cell)
	{
		GameObject gameObject = Grid.Objects[cell, 7];
		if (gameObject != null)
		{
			return gameObject.GetComponent<Diggable>();
		}
		return null;
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x000B163F File Offset: 0x000AF83F
	public static bool IsDiggable(int cell)
	{
		if (Grid.Solid[cell])
		{
			return !Grid.Foundation[cell];
		}
		return Diggable.GetUnstableCellAbove(cell) != Grid.InvalidCell;
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x000B1670 File Offset: 0x000AF870
	private static int GetUnstableCellAbove(int cell)
	{
		Vector2I cellXY = Grid.CellToXY(cell);
		List<int> cellsContainingFallingAbove = World.Instance.GetComponent<UnstableGroundManager>().GetCellsContainingFallingAbove(cellXY);
		if (cellsContainingFallingAbove.Contains(cell))
		{
			return cell;
		}
		byte b = Grid.WorldIdx[cell];
		int num = Grid.CellAbove(cell);
		while (Grid.IsValidCell(num) && Grid.WorldIdx[num] == b)
		{
			if (Grid.Foundation[num])
			{
				return Grid.InvalidCell;
			}
			if (Grid.Solid[num])
			{
				if (Grid.Element[num].IsUnstable)
				{
					return num;
				}
				return Grid.InvalidCell;
			}
			else
			{
				if (cellsContainingFallingAbove.Contains(num))
				{
					return num;
				}
				num = Grid.CellAbove(num);
			}
		}
		return Grid.InvalidCell;
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x000B1710 File Offset: 0x000AF910
	public static bool RequiresTool(Element e)
	{
		return false;
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x000B1713 File Offset: 0x000AF913
	public static bool Undiggable(Element e)
	{
		return e.id == SimHashes.Unobtanium;
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x000B1724 File Offset: 0x000AF924
	private void OnReachableChanged(object data)
	{
		if (this.childRenderer == null)
		{
			this.childRenderer = base.GetComponentInChildren<MeshRenderer>();
		}
		Material material = this.childRenderer.material;
		this.isReachable = (bool)data;
		if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
		{
			return;
		}
		this.UpdateColor(this.isReachable);
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.isReachable)
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, false);
			return;
		}
		component.AddStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, this);
		GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true);
		}, null, null);
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x000B1804 File Offset: 0x000AFA04
	private void UpdateColor(bool reachable)
	{
		if (this.childRenderer != null)
		{
			Material material = this.childRenderer.material;
			if (Diggable.RequiresTool(Grid.Element[Grid.PosToCell(base.gameObject)]) || Diggable.Undiggable(Grid.Element[Grid.PosToCell(base.gameObject)]))
			{
				material.color = Game.Instance.uiColours.Dig.invalidLocation;
				return;
			}
			if (Grid.Element[Grid.PosToCell(base.gameObject)].hardness >= 50)
			{
				if (reachable)
				{
					material.color = Game.Instance.uiColours.Dig.validLocation;
				}
				else
				{
					material.color = Game.Instance.uiColours.Dig.unreachable;
				}
				this.multitoolContext = Diggable.lasersForHardness[1].first;
				this.multitoolHitEffectTag = Diggable.lasersForHardness[1].second;
				return;
			}
			if (reachable)
			{
				material.color = Game.Instance.uiColours.Dig.validLocation;
			}
			else
			{
				material.color = Game.Instance.uiColours.Dig.unreachable;
			}
			this.multitoolContext = Diggable.lasersForHardness[0].first;
			this.multitoolHitEffectTag = Diggable.lasersForHardness[0].second;
		}
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x000B1968 File Offset: 0x000AFB68
	public override float GetPercentComplete()
	{
		return Grid.Damage[Grid.PosToCell(this)];
	}

	// Token: 0x06001F95 RID: 8085 RVA: 0x000B1978 File Offset: 0x000AFB78
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.unstableEntry);
		Game.Instance.Unsubscribe(this.handle);
		int cell = Grid.PosToCell(this);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.digDestroyedLayer, null);
		Components.Diggables.Remove(this);
	}

	// Token: 0x06001F96 RID: 8086 RVA: 0x000B19E3 File Offset: 0x000AFBE3
	private void OnCancel()
	{
		if (DetailsScreen.Instance != null)
		{
			DetailsScreen.Instance.Show(false);
		}
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x000B1A10 File Offset: 0x000AFC10
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELDIG.NAME, new System.Action(this.OnCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELDIG.TOOLTIP, true), 1f);
	}

	// Token: 0x040011BC RID: 4540
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040011BD RID: 4541
	private HandleVector<int>.Handle unstableEntry;

	// Token: 0x040011BE RID: 4542
	private MeshRenderer childRenderer;

	// Token: 0x040011BF RID: 4543
	private bool isReachable;

	// Token: 0x040011C0 RID: 4544
	private int cached_cell = -1;

	// Token: 0x040011C1 RID: 4545
	private Element originalDigElement;

	// Token: 0x040011C2 RID: 4546
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x040011C3 RID: 4547
	[SerializeField]
	public HashedString choreTypeIdHash;

	// Token: 0x040011C4 RID: 4548
	[SerializeField]
	public Material[] materials;

	// Token: 0x040011C5 RID: 4549
	[SerializeField]
	public MeshRenderer materialDisplay;

	// Token: 0x040011C6 RID: 4550
	private bool isDigComplete;

	// Token: 0x040011C7 RID: 4551
	private static List<global::Tuple<string, Tag>> lasersForHardness = new List<global::Tuple<string, Tag>>
	{
		new global::Tuple<string, Tag>("dig", "fx_dig_splash"),
		new global::Tuple<string, Tag>("specialistdig", "fx_dig_splash")
	};

	// Token: 0x040011C8 RID: 4552
	private int handle;

	// Token: 0x040011C9 RID: 4553
	private static readonly EventSystem.IntraObjectHandler<Diggable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Diggable>(delegate(Diggable component, object data)
	{
		component.OnReachableChanged(data);
	});

	// Token: 0x040011CA RID: 4554
	private static readonly EventSystem.IntraObjectHandler<Diggable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Diggable>(delegate(Diggable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040011CB RID: 4555
	public Chore chore;
}
