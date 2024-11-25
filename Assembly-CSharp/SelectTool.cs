using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000924 RID: 2340
public class SelectTool : InterfaceTool
{
	// Token: 0x06004409 RID: 17417 RVA: 0x0018240F File Offset: 0x0018060F
	public static void DestroyInstance()
	{
		SelectTool.Instance = null;
	}

	// Token: 0x0600440A RID: 17418 RVA: 0x00182418 File Offset: 0x00180618
	protected override void OnPrefabInit()
	{
		this.defaultLayerMask = (1 | LayerMask.GetMask(new string[]
		{
			"World",
			"Pickupable",
			"Place",
			"PlaceWithDepth",
			"BlockSelection",
			"Construction",
			"Selection"
		}));
		this.layerMask = this.defaultLayerMask;
		this.selectMarker = global::Util.KInstantiateUI<SelectMarker>(EntityPrefabs.Instance.SelectMarker, GameScreenManager.Instance.worldSpaceCanvas, false);
		this.selectMarker.gameObject.SetActive(false);
		this.populateHitsList = true;
		SelectTool.Instance = this;
	}

	// Token: 0x0600440B RID: 17419 RVA: 0x001824BA File Offset: 0x001806BA
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
		ToolMenu.Instance.PriorityScreen.ResetPriority();
		this.Select(null, false);
	}

	// Token: 0x0600440C RID: 17420 RVA: 0x001824DE File Offset: 0x001806DE
	public void SetLayerMask(int mask)
	{
		this.layerMask = mask;
		base.ClearHover();
		this.LateUpdate();
	}

	// Token: 0x0600440D RID: 17421 RVA: 0x001824F3 File Offset: 0x001806F3
	public void ClearLayerMask()
	{
		this.layerMask = this.defaultLayerMask;
	}

	// Token: 0x0600440E RID: 17422 RVA: 0x00182501 File Offset: 0x00180701
	public int GetDefaultLayerMask()
	{
		return this.defaultLayerMask;
	}

	// Token: 0x0600440F RID: 17423 RVA: 0x00182509 File Offset: 0x00180709
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		base.ClearHover();
		this.Select(null, false);
	}

	// Token: 0x06004410 RID: 17424 RVA: 0x00182520 File Offset: 0x00180720
	public void Focus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		if (selectable != null)
		{
			pos = selectable.transform.GetPosition();
		}
		pos.z = -40f;
		pos += offset;
		WorldContainer worldFromPosition = ClusterManager.Instance.GetWorldFromPosition(pos);
		if (worldFromPosition != null)
		{
			CameraController.Instance.ActiveWorldStarWipe(worldFromPosition.id, pos, 10f, null);
			return;
		}
		DebugUtil.DevLogError("DevError: specified camera focus position has null world - possible out of bounds location");
	}

	// Token: 0x06004411 RID: 17425 RVA: 0x0018258F File Offset: 0x0018078F
	public void SelectAndFocus(Vector3 pos, KSelectable selectable, Vector3 offset)
	{
		this.Focus(pos, selectable, offset);
		this.Select(selectable, false);
	}

	// Token: 0x06004412 RID: 17426 RVA: 0x001825A2 File Offset: 0x001807A2
	public void SelectAndFocus(Vector3 pos, KSelectable selectable)
	{
		this.SelectAndFocus(pos, selectable, Vector3.zero);
	}

	// Token: 0x06004413 RID: 17427 RVA: 0x001825B1 File Offset: 0x001807B1
	public void SelectNextFrame(KSelectable new_selected, bool skipSound = false)
	{
		this.delayedNextSelection = new_selected;
		this.delayedSkipSound = skipSound;
		UIScheduler.Instance.ScheduleNextFrame("DelayedSelect", new Action<object>(this.DoSelectNextFrame), null, null);
	}

	// Token: 0x06004414 RID: 17428 RVA: 0x001825DF File Offset: 0x001807DF
	private void DoSelectNextFrame(object data)
	{
		this.Select(this.delayedNextSelection, this.delayedSkipSound);
		this.delayedNextSelection = null;
	}

	// Token: 0x06004415 RID: 17429 RVA: 0x001825FC File Offset: 0x001807FC
	public void Select(KSelectable new_selected, bool skipSound = false)
	{
		if (new_selected == this.previousSelection)
		{
			return;
		}
		this.previousSelection = new_selected;
		if (this.selected != null)
		{
			this.selected.Unselect();
		}
		GameObject gameObject = null;
		if (new_selected != null && new_selected.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
		{
			SelectToolHoverTextCard component = base.GetComponent<SelectToolHoverTextCard>();
			if (component != null)
			{
				int num = component.currentSelectedSelectableIndex;
				int recentNumberOfDisplayedSelectables = component.recentNumberOfDisplayedSelectables;
				if (recentNumberOfDisplayedSelectables != 0)
				{
					num = (num + 1) % recentNumberOfDisplayedSelectables;
					if (!skipSound)
					{
						if (recentNumberOfDisplayedSelectables == 1)
						{
							KFMOD.PlayUISound(GlobalAssets.GetSound("Select_empty", false));
						}
						else
						{
							EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("Select_full", false), Vector3.zero, 1f);
							instance.setParameterByName("selection", (float)num, false);
							SoundEvent.EndOneShot(instance);
						}
						this.playedSoundThisFrame = true;
					}
				}
			}
			if (new_selected == this.hover)
			{
				base.ClearHover();
			}
			new_selected.Select();
			gameObject = new_selected.gameObject;
			this.selectMarker.SetTargetTransform(gameObject.transform);
			this.selectMarker.gameObject.SetActive(!new_selected.DisableSelectMarker);
		}
		else if (this.selectMarker != null)
		{
			this.selectMarker.gameObject.SetActive(false);
		}
		this.selected = ((gameObject == null) ? null : new_selected);
		Game.Instance.Trigger(-1503271301, gameObject);
	}

	// Token: 0x06004416 RID: 17430 RVA: 0x00182768 File Offset: 0x00180968
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		KSelectable objectUnderCursor = base.GetObjectUnderCursor<KSelectable>(true, (KSelectable s) => s.GetComponent<KSelectable>().IsSelectable, this.selected);
		this.selectedCell = Grid.PosToCell(cursor_pos);
		this.Select(objectUnderCursor, false);
		if (DevToolSimDebug.Instance != null)
		{
			DevToolSimDebug.Instance.SetCell(this.selectedCell);
		}
		if (DevToolNavGrid.Instance != null)
		{
			DevToolNavGrid.Instance.SetCell(this.selectedCell);
		}
	}

	// Token: 0x06004417 RID: 17431 RVA: 0x001827E4 File Offset: 0x001809E4
	public int GetSelectedCell()
	{
		return this.selectedCell;
	}

	// Token: 0x04002C8C RID: 11404
	public KSelectable selected;

	// Token: 0x04002C8D RID: 11405
	protected int cell_new;

	// Token: 0x04002C8E RID: 11406
	private int selectedCell;

	// Token: 0x04002C8F RID: 11407
	protected int defaultLayerMask;

	// Token: 0x04002C90 RID: 11408
	public static SelectTool Instance;

	// Token: 0x04002C91 RID: 11409
	private KSelectable delayedNextSelection;

	// Token: 0x04002C92 RID: 11410
	private bool delayedSkipSound;

	// Token: 0x04002C93 RID: 11411
	private KSelectable previousSelection;
}
