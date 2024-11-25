using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A94 RID: 2708
public class SetLocker : StateMachineComponent<SetLocker.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x06004F59 RID: 20313 RVA: 0x001C874F File Offset: 0x001C694F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004F5A RID: 20314 RVA: 0x001C8757 File Offset: 0x001C6957
	public void ChooseContents()
	{
		this.contents = this.possible_contents_ids[UnityEngine.Random.Range(0, this.possible_contents_ids.GetLength(0))];
	}

	// Token: 0x06004F5B RID: 20315 RVA: 0x001C8778 File Offset: 0x001C6978
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.contents == null)
		{
			this.ChooseContents();
		}
		else
		{
			string[] array = this.contents;
			for (int i = 0; i < array.Length; i++)
			{
				if (Assets.GetPrefab(array[i]) == null)
				{
					this.ChooseContents();
					break;
				}
			}
		}
		if (this.pendingRummage)
		{
			this.ActivateChore(null);
		}
	}

	// Token: 0x06004F5C RID: 20316 RVA: 0x001C87E8 File Offset: 0x001C69E8
	public void DropContents()
	{
		if (this.contents == null)
		{
			return;
		}
		if (DlcManager.IsExpansion1Active() && this.numDataBanks.Length >= 2)
		{
			int num = UnityEngine.Random.Range(this.numDataBanks[0], this.numDataBanks[1]);
			for (int i = 0; i <= num; i++)
			{
				Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, "OrbitalResearchDatabank", Grid.SceneLayer.Front).SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		for (int j = 0; j < this.contents.Length; j++)
		{
			GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(base.gameObject), this.dropOffset.x, this.dropOffset.y, this.contents[j], Grid.SceneLayer.Front);
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, Assets.GetPrefab(this.contents[j].ToTag()).GetProperName(), base.smi.master.transform, 1.5f, false);
			}
		}
		base.gameObject.Trigger(-372600542, this);
	}

	// Token: 0x06004F5D RID: 20317 RVA: 0x001C8957 File Offset: 0x001C6B57
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x06004F5E RID: 20318 RVA: 0x001C8960 File Offset: 0x001C6B60
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x06004F5F RID: 20319 RVA: 0x001C896C File Offset: 0x001C6B6C
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		base.Trigger(1980521255, null);
		this.pendingRummage = true;
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteChore();
		}, null, null, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06004F60 RID: 20320 RVA: 0x001C89F6 File Offset: 0x001C6BF6
	public void CancelChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.pendingRummage = false;
		Prioritizable.RemoveRef(base.gameObject);
		base.Trigger(1980521255, null);
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06004F61 RID: 20321 RVA: 0x001C8A38 File Offset: 0x001C6C38
	private void CompleteChore()
	{
		this.used = true;
		base.smi.GoTo(base.smi.sm.open);
		this.chore = null;
		this.pendingRummage = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06004F62 RID: 20322 RVA: 0x001C8A95 File Offset: 0x001C6C95
	public string SidescreenButtonText
	{
		get
		{
			return (this.chore == null) ? UI.USERMENUACTIONS.OPENPOI.NAME : UI.USERMENUACTIONS.OPENPOI.NAME_OFF;
		}
	}

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x06004F63 RID: 20323 RVA: 0x001C8AB0 File Offset: 0x001C6CB0
	public string SidescreenButtonTooltip
	{
		get
		{
			return (this.chore == null) ? UI.USERMENUACTIONS.OPENPOI.TOOLTIP : UI.USERMENUACTIONS.OPENPOI.TOOLTIP_OFF;
		}
	}

	// Token: 0x06004F64 RID: 20324 RVA: 0x001C8ACB File Offset: 0x001C6CCB
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06004F65 RID: 20325 RVA: 0x001C8ACE File Offset: 0x001C6CCE
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06004F66 RID: 20326 RVA: 0x001C8AD1 File Offset: 0x001C6CD1
	public void OnSidescreenButtonPressed()
	{
		if (this.chore == null)
		{
			this.OnClickOpen();
			return;
		}
		this.OnClickCancel();
	}

	// Token: 0x06004F67 RID: 20327 RVA: 0x001C8AE8 File Offset: 0x001C6CE8
	public bool SidescreenButtonInteractable()
	{
		return !this.used;
	}

	// Token: 0x06004F68 RID: 20328 RVA: 0x001C8AF3 File Offset: 0x001C6CF3
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06004F69 RID: 20329 RVA: 0x001C8AF7 File Offset: 0x001C6CF7
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x040034BF RID: 13503
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x040034C0 RID: 13504
	public string[][] possible_contents_ids;

	// Token: 0x040034C1 RID: 13505
	public string machineSound;

	// Token: 0x040034C2 RID: 13506
	public string overrideAnim;

	// Token: 0x040034C3 RID: 13507
	public Vector2I dropOffset = Vector2I.zero;

	// Token: 0x040034C4 RID: 13508
	public int[] numDataBanks;

	// Token: 0x040034C5 RID: 13509
	[Serialize]
	private string[] contents;

	// Token: 0x040034C6 RID: 13510
	public bool dropOnDeconstruct;

	// Token: 0x040034C7 RID: 13511
	[Serialize]
	private bool pendingRummage;

	// Token: 0x040034C8 RID: 13512
	[Serialize]
	private bool used;

	// Token: 0x040034C9 RID: 13513
	private Chore chore;

	// Token: 0x02001AC0 RID: 6848
	public class StatesInstance : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.GameInstance
	{
		// Token: 0x0600A10E RID: 41230 RVA: 0x00381D16 File Offset: 0x0037FF16
		public StatesInstance(SetLocker master) : base(master)
		{
		}

		// Token: 0x0600A10F RID: 41231 RVA: 0x00381D1F File Offset: 0x0037FF1F
		public override void StartSM()
		{
			base.StartSM();
			base.smi.Subscribe(-702296337, delegate(object o)
			{
				if (base.smi.master.dropOnDeconstruct && base.smi.IsInsideState(base.smi.sm.closed))
				{
					base.smi.master.DropContents();
				}
			});
		}
	}

	// Token: 0x02001AC1 RID: 6849
	public class States : GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker>
	{
		// Token: 0x0600A111 RID: 41233 RVA: 0x00381D90 File Offset: 0x0037FF90
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.open.PlayAnim("working_pre").QueueAnim("working_loop", false, null).QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.off).Exit(delegate(SetLocker.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.off.PlayAnim("off").Enter(delegate(SetLocker.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x04007D8E RID: 32142
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State closed;

		// Token: 0x04007D8F RID: 32143
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State open;

		// Token: 0x04007D90 RID: 32144
		public GameStateMachine<SetLocker.States, SetLocker.StatesInstance, SetLocker, object>.State off;
	}
}
