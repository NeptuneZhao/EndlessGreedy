using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using UnityEngine;

// Token: 0x02000B8B RID: 2955
public class CharacterSelectionController : KModalScreen
{
	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x0600592B RID: 22827 RVA: 0x00204001 File Offset: 0x00202201
	// (set) Token: 0x0600592C RID: 22828 RVA: 0x00204009 File Offset: 0x00202209
	public bool IsStarterMinion { get; set; }

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x0600592D RID: 22829 RVA: 0x00204012 File Offset: 0x00202212
	public bool AllowsReplacing
	{
		get
		{
			return this.allowsReplacing;
		}
	}

	// Token: 0x0600592E RID: 22830 RVA: 0x0020401A File Offset: 0x0020221A
	protected virtual void OnProceed()
	{
	}

	// Token: 0x0600592F RID: 22831 RVA: 0x0020401C File Offset: 0x0020221C
	protected virtual void OnDeliverableAdded()
	{
	}

	// Token: 0x06005930 RID: 22832 RVA: 0x0020401E File Offset: 0x0020221E
	protected virtual void OnDeliverableRemoved()
	{
	}

	// Token: 0x06005931 RID: 22833 RVA: 0x00204020 File Offset: 0x00202220
	protected virtual void OnLimitReached()
	{
	}

	// Token: 0x06005932 RID: 22834 RVA: 0x00204022 File Offset: 0x00202222
	protected virtual void OnLimitUnreached()
	{
	}

	// Token: 0x06005933 RID: 22835 RVA: 0x00204024 File Offset: 0x00202224
	protected virtual void InitializeContainers()
	{
		this.DisableProceedButton();
		if (this.containers != null && this.containers.Count > 0)
		{
			return;
		}
		this.OnReplacedEvent = null;
		this.containers = new List<ITelepadDeliverableContainer>();
		if (this.IsStarterMinion || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CarePackages).id != "Enabled")
		{
			this.numberOfDuplicantOptions = 3;
			this.numberOfCarePackageOptions = 0;
		}
		else
		{
			this.numberOfCarePackageOptions = ((UnityEngine.Random.Range(0, 101) > 70) ? 2 : 1);
			this.numberOfDuplicantOptions = 4 - this.numberOfCarePackageOptions;
		}
		for (int i = 0; i < this.numberOfDuplicantOptions; i++)
		{
			CharacterContainer characterContainer = Util.KInstantiateUI<CharacterContainer>(this.containerPrefab.gameObject, this.containerParent, false);
			characterContainer.SetController(this);
			characterContainer.SetReshufflingState(true);
			this.containers.Add(characterContainer);
		}
		for (int j = 0; j < this.numberOfCarePackageOptions; j++)
		{
			CarePackageContainer carePackageContainer = Util.KInstantiateUI<CarePackageContainer>(this.carePackageContainerPrefab.gameObject, this.containerParent, false);
			carePackageContainer.SetController(this);
			this.containers.Add(carePackageContainer);
			carePackageContainer.gameObject.transform.SetSiblingIndex(UnityEngine.Random.Range(0, carePackageContainer.transform.parent.childCount));
		}
		this.selectedDeliverables = new List<ITelepadDeliverable>();
	}

	// Token: 0x06005934 RID: 22836 RVA: 0x0020416C File Offset: 0x0020236C
	public virtual void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
		this.Show(false);
	}

	// Token: 0x06005935 RID: 22837 RVA: 0x002041D4 File Offset: 0x002023D4
	public void RemoveLast()
	{
		if (this.selectedDeliverables == null || this.selectedDeliverables.Count == 0)
		{
			return;
		}
		ITelepadDeliverable obj = this.selectedDeliverables[this.selectedDeliverables.Count - 1];
		if (this.OnReplacedEvent != null)
		{
			this.OnReplacedEvent(obj);
		}
	}

	// Token: 0x06005936 RID: 22838 RVA: 0x00204224 File Offset: 0x00202424
	public void AddDeliverable(ITelepadDeliverable deliverable)
	{
		if (this.selectedDeliverables.Contains(deliverable))
		{
			global::Debug.Log("Tried to add the same minion twice.");
			return;
		}
		if (this.selectedDeliverables.Count >= this.selectableCount)
		{
			global::Debug.LogError("Tried to add minions beyond the allowed limit");
			return;
		}
		this.selectedDeliverables.Add(deliverable);
		this.OnDeliverableAdded();
		if (this.selectedDeliverables.Count == this.selectableCount)
		{
			this.EnableProceedButton();
			if (this.OnLimitReachedEvent != null)
			{
				this.OnLimitReachedEvent();
			}
			this.OnLimitReached();
		}
	}

	// Token: 0x06005937 RID: 22839 RVA: 0x002042AC File Offset: 0x002024AC
	public void RemoveDeliverable(ITelepadDeliverable deliverable)
	{
		bool flag = this.selectedDeliverables.Count >= this.selectableCount;
		this.selectedDeliverables.Remove(deliverable);
		this.OnDeliverableRemoved();
		if (flag && this.selectedDeliverables.Count < this.selectableCount)
		{
			this.DisableProceedButton();
			if (this.OnLimitUnreachedEvent != null)
			{
				this.OnLimitUnreachedEvent();
			}
			this.OnLimitUnreached();
		}
	}

	// Token: 0x06005938 RID: 22840 RVA: 0x00204316 File Offset: 0x00202516
	public bool IsSelected(ITelepadDeliverable deliverable)
	{
		return this.selectedDeliverables.Contains(deliverable);
	}

	// Token: 0x06005939 RID: 22841 RVA: 0x00204324 File Offset: 0x00202524
	protected void EnableProceedButton()
	{
		this.proceedButton.isInteractable = true;
		this.proceedButton.ClearOnClick();
		this.proceedButton.onClick += delegate()
		{
			this.OnProceed();
		};
	}

	// Token: 0x0600593A RID: 22842 RVA: 0x00204354 File Offset: 0x00202554
	protected void DisableProceedButton()
	{
		this.proceedButton.ClearOnClick();
		this.proceedButton.isInteractable = false;
		this.proceedButton.onClick += delegate()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		};
	}

	// Token: 0x04003A91 RID: 14993
	[SerializeField]
	private CharacterContainer containerPrefab;

	// Token: 0x04003A92 RID: 14994
	[SerializeField]
	private CarePackageContainer carePackageContainerPrefab;

	// Token: 0x04003A93 RID: 14995
	[SerializeField]
	private GameObject containerParent;

	// Token: 0x04003A94 RID: 14996
	[SerializeField]
	protected KButton proceedButton;

	// Token: 0x04003A95 RID: 14997
	protected int numberOfDuplicantOptions = 3;

	// Token: 0x04003A96 RID: 14998
	protected int numberOfCarePackageOptions;

	// Token: 0x04003A97 RID: 14999
	[SerializeField]
	protected int selectableCount;

	// Token: 0x04003A98 RID: 15000
	[SerializeField]
	private bool allowsReplacing;

	// Token: 0x04003A9A RID: 15002
	protected List<ITelepadDeliverable> selectedDeliverables;

	// Token: 0x04003A9B RID: 15003
	protected List<ITelepadDeliverableContainer> containers;

	// Token: 0x04003A9C RID: 15004
	public System.Action OnLimitReachedEvent;

	// Token: 0x04003A9D RID: 15005
	public System.Action OnLimitUnreachedEvent;

	// Token: 0x04003A9E RID: 15006
	public Action<bool> OnReshuffleEvent;

	// Token: 0x04003A9F RID: 15007
	public Action<ITelepadDeliverable> OnReplacedEvent;

	// Token: 0x04003AA0 RID: 15008
	public System.Action OnProceedEvent;
}
