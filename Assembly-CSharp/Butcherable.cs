using System;
using STRINGS;
using UnityEngine;

// Token: 0x020007A1 RID: 1953
[AddComponentMenu("KMonoBehaviour/Workable/Butcherable")]
public class Butcherable : Workable, ISaveLoadable
{
	// Token: 0x0600356F RID: 13679 RVA: 0x00122BA4 File Offset: 0x00120DA4
	public void SetDrops(string[] drops)
	{
		this.drops = drops;
	}

	// Token: 0x06003570 RID: 13680 RVA: 0x00122BB0 File Offset: 0x00120DB0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Butcherable>(1272413801, Butcherable.SetReadyToButcherDelegate);
		base.Subscribe<Butcherable>(493375141, Butcherable.OnRefreshUserMenuDelegate);
		this.workTime = 3f;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x00122C10 File Offset: 0x00120E10
	public void SetReadyToButcher(object param)
	{
		this.readyToButcher = true;
	}

	// Token: 0x06003572 RID: 13682 RVA: 0x00122C19 File Offset: 0x00120E19
	public void SetReadyToButcher(bool ready)
	{
		this.readyToButcher = ready;
	}

	// Token: 0x06003573 RID: 13683 RVA: 0x00122C24 File Offset: 0x00120E24
	public void ActivateChore(object param)
	{
		if (this.chore != null)
		{
			return;
		}
		this.chore = new WorkChore<Butcherable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06003574 RID: 13684 RVA: 0x00122C6D File Offset: 0x00120E6D
	public void CancelChore(object param)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06003575 RID: 13685 RVA: 0x00122C8F File Offset: 0x00120E8F
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x06003576 RID: 13686 RVA: 0x00122C98 File Offset: 0x00120E98
	private void OnClickButcher()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnButcherComplete();
			return;
		}
		this.ActivateChore(null);
	}

	// Token: 0x06003577 RID: 13687 RVA: 0x00122CB0 File Offset: 0x00120EB0
	private void OnRefreshUserMenu(object data)
	{
		if (!this.readyToButcher)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_harvest", "Cancel Meatify", new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, "", true) : new KIconButtonMenu.ButtonInfo("action_harvest", "Meatify", new System.Action(this.OnClickButcher), global::Action.NumActions, null, null, null, "", true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06003578 RID: 13688 RVA: 0x00122D3E File Offset: 0x00120F3E
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.OnButcherComplete();
	}

	// Token: 0x06003579 RID: 13689 RVA: 0x00122D48 File Offset: 0x00120F48
	public GameObject[] CreateDrops()
	{
		GameObject[] array = new GameObject[this.drops.Length];
		for (int i = 0; i < this.drops.Length; i++)
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, this.drops[i], Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			Edible component = gameObject.GetComponent<Edible>();
			if (component)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
			}
			array[i] = gameObject;
		}
		return array;
	}

	// Token: 0x0600357A RID: 13690 RVA: 0x00122DE0 File Offset: 0x00120FE0
	public void OnButcherComplete()
	{
		if (this.butchered)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component && component.IsSelected)
		{
			SelectTool.Instance.Select(null, false);
		}
		Pickupable component2 = base.GetComponent<Pickupable>();
		Storage storage = (component2 != null) ? component2.storage : null;
		GameObject[] array = this.CreateDrops();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (storage != null && storage.storeDropsFromButcherables)
				{
					storage.Store(array[i], false, false, true, false);
				}
			}
		}
		this.chore = null;
		this.butchered = true;
		this.readyToButcher = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		base.Trigger(395373363, array);
	}

	// Token: 0x0600357B RID: 13691 RVA: 0x00122EA8 File Offset: 0x001210A8
	private int GetDropSpawnLocation()
	{
		int num = Grid.PosToCell(base.gameObject);
		int num2 = Grid.CellAbove(num);
		if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
		{
			return num2;
		}
		return num;
	}

	// Token: 0x04001FB9 RID: 8121
	[MyCmpGet]
	private KAnimControllerBase controller;

	// Token: 0x04001FBA RID: 8122
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x04001FBB RID: 8123
	private bool readyToButcher;

	// Token: 0x04001FBC RID: 8124
	private bool butchered;

	// Token: 0x04001FBD RID: 8125
	public string[] drops;

	// Token: 0x04001FBE RID: 8126
	private Chore chore;

	// Token: 0x04001FBF RID: 8127
	private static readonly EventSystem.IntraObjectHandler<Butcherable> SetReadyToButcherDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.SetReadyToButcher(data);
	});

	// Token: 0x04001FC0 RID: 8128
	private static readonly EventSystem.IntraObjectHandler<Butcherable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
