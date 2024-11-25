using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000864 RID: 2148
[AddComponentMenu("KMonoBehaviour/Workable/Dumpable")]
public class Dumpable : Workable
{
	// Token: 0x06003BDD RID: 15325 RVA: 0x001499CA File Offset: 0x00147BCA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Dumpable>(493375141, Dumpable.OnRefreshUserMenuDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x001499F8 File Offset: 0x00147BF8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForDumping)
		{
			this.CreateChore();
		}
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_dumpable_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"working"
		};
		this.synchronizeAnims = false;
		base.SetWorkTime(1f);
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x00149A68 File Offset: 0x00147C68
	public void ToggleDumping()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.isMarkedForDumping)
		{
			this.isMarkedForDumping = false;
			this.chore.Cancel("Cancel Dumping!");
			Prioritizable.RemoveRef(base.gameObject);
			this.chore = null;
			base.ShowProgressBar(false);
			return;
		}
		this.isMarkedForDumping = true;
		this.CreateChore();
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x00149ACC File Offset: 0x00147CCC
	private void CreateChore()
	{
		if (this.chore == null)
		{
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Dumpable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x06003BE1 RID: 15329 RVA: 0x00149B18 File Offset: 0x00147D18
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.isMarkedForDumping = false;
		this.chore = null;
		this.Dump();
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x00149B39 File Offset: 0x00147D39
	public void Dump()
	{
		this.Dump(base.transform.GetPosition());
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x00149B4C File Offset: 0x00147D4C
	public void Dump(Vector3 pos)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component.Mass > 0f)
		{
			if (component.Element.IsLiquid)
			{
				FallingWater.instance.AddParticle(Grid.PosToCell(pos), component.Element.idx, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, false, false, false);
			}
			else
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(pos), component.ElementID, CellEventLogger.Instance.Dumpable, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
			}
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x00149BF4 File Offset: 0x00147DF4
	private void OnRefreshUserMenu(object data)
	{
		if (this.HasTag(GameTags.Stored))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForDumping ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.DUMP.NAME_OFF, new System.Action(this.ToggleDumping), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DUMP.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.DUMP.NAME, new System.Action(this.ToggleDumping), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DUMP.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04002437 RID: 9271
	private Chore chore;

	// Token: 0x04002438 RID: 9272
	[Serialize]
	private bool isMarkedForDumping;

	// Token: 0x04002439 RID: 9273
	private static readonly EventSystem.IntraObjectHandler<Dumpable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Dumpable>(delegate(Dumpable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
