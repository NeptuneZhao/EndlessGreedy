using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000537 RID: 1335
public class CancellableMove : Cancellable
{
	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06001E67 RID: 7783 RVA: 0x000A95F3 File Offset: 0x000A77F3
	public List<Ref<Movable>> movingObjects
	{
		get
		{
			return this.movables;
		}
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x000A95FC File Offset: 0x000A77FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (!component.IsPrioritizable())
		{
			component.AddRef();
		}
		if (this.fetchChore == null)
		{
			GameObject nextTarget = this.GetNextTarget();
			if (!(nextTarget != null) || nextTarget.IsNullOrDestroyed())
			{
				global::Debug.LogWarning("MovePickupable spawned with no objects to move. Destroying placer.");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
		}
		base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		base.Subscribe(2127324410, new Action<object>(this.OnCancel));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = base.gameObject;
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x000A96D4 File Offset: 0x000A78D4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = Grid.PosToCell(this);
		Grid.Objects[cell, 44] = null;
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x000A9707 File Offset: 0x000A7907
	public void CancelAll()
	{
		this.OnCancel(null);
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x000A9710 File Offset: 0x000A7910
	public void OnCancel(Movable cancel_movable = null)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			Ref<Movable> @ref = this.movables[i];
			if (@ref != null)
			{
				Movable movable = @ref.Get();
				if (cancel_movable == null || movable == cancel_movable)
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("CancelMove");
			if (this.fetchChore.driver == null && this.movables.Count <= 0)
			{
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x000A97B4 File Offset: 0x000A79B4
	protected override void OnCancel(object data)
	{
		this.OnCancel(null);
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x000A97C0 File Offset: 0x000A79C0
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_control", UI.USERMENUACTIONS.PICKUPABLEMOVE.NAME_OFF, new System.Action(this.CancelAll), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.PICKUPABLEMOVE.TOOLTIP_OFF, true), 1f);
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x000A981C File Offset: 0x000A7A1C
	public void SetMovable(Movable movable)
	{
		if (this.fetchChore == null)
		{
			this.fetchChore = new MovePickupableChore(this, movable.gameObject, new Action<Chore>(this.OnChoreEnd));
		}
		if (this.movables.Find((Ref<Movable> move) => move.Get() == movable) == null)
		{
			this.movables.Add(new Ref<Movable>(movable));
		}
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x000A9890 File Offset: 0x000A7A90
	public void OnChoreEnd(Chore chore)
	{
		GameObject nextTarget = this.GetNextTarget();
		if (nextTarget == null)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.fetchChore = new MovePickupableChore(this, nextTarget, new Action<Chore>(this.OnChoreEnd));
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x000A98D2 File Offset: 0x000A7AD2
	public bool IsDeliveryComplete()
	{
		this.ValidateMovables();
		return this.movables.Count <= 0;
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x000A98EC File Offset: 0x000A7AEC
	public void RemoveMovable(Movable moved)
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i].Get() == null || this.movables[i].Get() == moved)
			{
				this.movables.RemoveAt(i);
			}
		}
		if (this.movables.Count <= 0)
		{
			this.OnCancel(null);
		}
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x000A9964 File Offset: 0x000A7B64
	public GameObject GetNextTarget()
	{
		this.ValidateMovables();
		if (this.movables.Count > 0)
		{
			return this.movables[0].Get().gameObject;
		}
		return null;
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x000A9994 File Offset: 0x000A7B94
	private void ValidateMovables()
	{
		for (int i = this.movables.Count - 1; i >= 0; i--)
		{
			if (this.movables[i] == null)
			{
				this.movables.RemoveAt(i);
			}
			else
			{
				Movable movable = this.movables[i].Get();
				if (movable == null)
				{
					this.movables.RemoveAt(i);
				}
				else if (Grid.PosToCell(movable) == Grid.PosToCell(this))
				{
					movable.ClearMove();
					this.movables.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x04001123 RID: 4387
	[Serialize]
	private List<Ref<Movable>> movables = new List<Ref<Movable>>();

	// Token: 0x04001124 RID: 4388
	private MovePickupableChore fetchChore;
}
