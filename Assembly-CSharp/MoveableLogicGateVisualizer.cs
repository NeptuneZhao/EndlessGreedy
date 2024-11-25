using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200070B RID: 1803
[SkipSaveFileSerialization]
public class MoveableLogicGateVisualizer : LogicGateBase
{
	// Token: 0x06002EB6 RID: 11958 RVA: 0x001065E4 File Offset: 0x001047E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cell = -1;
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
		base.Subscribe<MoveableLogicGateVisualizer>(-1643076535, MoveableLogicGateVisualizer.OnRotatedDelegate);
	}

	// Token: 0x06002EB7 RID: 11959 RVA: 0x00106645 File Offset: 0x00104845
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x00106679 File Offset: 0x00104879
	private void OnOverlayChanged(HashedString mode)
	{
		if (mode == OverlayModes.Logic.ID)
		{
			this.Register();
			return;
		}
		this.Unregister();
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x00106695 File Offset: 0x00104895
	private void OnRotated(object data)
	{
		this.Unregister();
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x001066B0 File Offset: 0x001048B0
	private void Update()
	{
		if (this.visChildren.Count <= 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num == this.cell)
		{
			return;
		}
		this.cell = num;
		this.Unregister();
		this.Register();
	}

	// Token: 0x06002EBB RID: 11963 RVA: 0x001066FC File Offset: 0x001048FC
	private GameObject CreateUIElem(int cell, bool is_input)
	{
		GameObject gameObject = Util.KInstantiate(LogicGateBase.uiSrcData.prefab, Grid.CellToPosCCC(cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, null, true, 0);
		Image component = gameObject.GetComponent<Image>();
		component.sprite = (is_input ? LogicGateBase.uiSrcData.inputSprite : LogicGateBase.uiSrcData.outputSprite);
		component.raycastTarget = false;
		return gameObject;
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x00106760 File Offset: 0x00104960
	private void Register()
	{
		if (this.visChildren.Count > 0)
		{
			return;
		}
		base.enabled = true;
		this.visChildren.Add(this.CreateUIElem(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.OutputCellTwo, false));
			this.visChildren.Add(this.CreateUIElem(base.OutputCellThree, false));
			this.visChildren.Add(this.CreateUIElem(base.OutputCellFour, false));
		}
		this.visChildren.Add(this.CreateUIElem(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.InputCellTwo, true));
			this.visChildren.Add(this.CreateUIElem(base.InputCellThree, true));
			this.visChildren.Add(this.CreateUIElem(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.ControlCellOne, true));
			this.visChildren.Add(this.CreateUIElem(base.ControlCellTwo, true));
		}
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x001068B0 File Offset: 0x00104AB0
	private void Unregister()
	{
		if (this.visChildren.Count <= 0)
		{
			return;
		}
		base.enabled = false;
		this.cell = -1;
		foreach (GameObject original in this.visChildren)
		{
			Util.KDestroyGameObject(original);
		}
		this.visChildren.Clear();
	}

	// Token: 0x04001B91 RID: 7057
	private int cell;

	// Token: 0x04001B92 RID: 7058
	protected List<GameObject> visChildren = new List<GameObject>();

	// Token: 0x04001B93 RID: 7059
	private static readonly EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer>(delegate(MoveableLogicGateVisualizer component, object data)
	{
		component.OnRotated(data);
	});
}
