using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D65 RID: 3429
public class DoorToggleSideScreen : SideScreenContent
{
	// Token: 0x06006C01 RID: 27649 RVA: 0x0028A029 File Offset: 0x00288229
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.InitButtons();
	}

	// Token: 0x06006C02 RID: 27650 RVA: 0x0028A038 File Offset: 0x00288238
	private void InitButtons()
	{
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.openButton,
			state = Door.ControlState.Opened,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN_PENDING
		});
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.autoButton,
			state = Door.ControlState.Auto,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO_PENDING
		});
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.closeButton,
			state = Door.ControlState.Locked,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE_PENDING
		});
		using (List<DoorToggleSideScreen.DoorButtonInfo>.Enumerator enumerator = this.buttonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DoorToggleSideScreen.DoorButtonInfo info = enumerator.Current;
				info.button.onClick += delegate()
				{
					this.target.QueueStateChange(info.state);
					this.Refresh();
				};
			}
		}
	}

	// Token: 0x06006C03 RID: 27651 RVA: 0x0028A194 File Offset: 0x00288394
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x0028A1A4 File Offset: 0x002883A4
	public override void SetTarget(GameObject target)
	{
		if (this.target != null)
		{
			this.ClearTarget();
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Door>();
		this.accessTarget = target.GetComponent<AccessControl>();
		if (this.target == null)
		{
			return;
		}
		target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
		target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		this.Refresh();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06006C05 RID: 27653 RVA: 0x0028A238 File Offset: 0x00288438
	public override void ClearTarget()
	{
		if (this.target != null)
		{
			this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
			this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		}
		this.target = null;
	}

	// Token: 0x06006C06 RID: 27654 RVA: 0x0028A294 File Offset: 0x00288494
	private void Refresh()
	{
		string text = null;
		string text2 = null;
		if (this.buttonList == null || this.buttonList.Count == 0)
		{
			this.InitButtons();
		}
		foreach (DoorToggleSideScreen.DoorButtonInfo doorButtonInfo in this.buttonList)
		{
			if (this.target.CurrentState == doorButtonInfo.state && this.target.RequestedState == doorButtonInfo.state)
			{
				doorButtonInfo.button.isOn = true;
				text = doorButtonInfo.currentString;
				foreach (ImageToggleState imageToggleState in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState.SetActive();
					imageToggleState.SetActive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			}
			else if (this.target.RequestedState == doorButtonInfo.state)
			{
				doorButtonInfo.button.isOn = true;
				text2 = doorButtonInfo.pendingString;
				foreach (ImageToggleState imageToggleState2 in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState2.SetActive();
					imageToggleState2.SetActive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = true;
			}
			else
			{
				doorButtonInfo.button.isOn = false;
				foreach (ImageToggleState imageToggleState3 in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState3.SetInactive();
					imageToggleState3.SetInactive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			}
		}
		string text3 = text;
		if (text2 != null)
		{
			text3 = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.PENDING_FORMAT, text3, text2);
		}
		if (this.accessTarget != null && !this.accessTarget.Online)
		{
			text3 = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_FORMAT, text3, UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_OFFLINE);
		}
		if (this.target.building.Def.PrefabID == POIDoorInternalConfig.ID)
		{
			text3 = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.POI_INTERNAL;
			using (List<DoorToggleSideScreen.DoorButtonInfo>.Enumerator enumerator = this.buttonList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DoorToggleSideScreen.DoorButtonInfo doorButtonInfo2 = enumerator.Current;
					doorButtonInfo2.button.gameObject.SetActive(false);
				}
				goto IL_2A1;
			}
		}
		foreach (DoorToggleSideScreen.DoorButtonInfo doorButtonInfo3 in this.buttonList)
		{
			bool active = doorButtonInfo3.state != Door.ControlState.Auto || this.target.allowAutoControl;
			doorButtonInfo3.button.gameObject.SetActive(active);
		}
		IL_2A1:
		this.description.text = text3;
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(text3));
		this.ContentContainer.SetActive(!this.target.isSealed);
	}

	// Token: 0x06006C07 RID: 27655 RVA: 0x0028A5CC File Offset: 0x002887CC
	private void OnDoorStateChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x06006C08 RID: 27656 RVA: 0x0028A5D4 File Offset: 0x002887D4
	private void OnAccessControlChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x040049A3 RID: 18851
	[SerializeField]
	private KToggle openButton;

	// Token: 0x040049A4 RID: 18852
	[SerializeField]
	private KToggle autoButton;

	// Token: 0x040049A5 RID: 18853
	[SerializeField]
	private KToggle closeButton;

	// Token: 0x040049A6 RID: 18854
	[SerializeField]
	private LocText description;

	// Token: 0x040049A7 RID: 18855
	private Door target;

	// Token: 0x040049A8 RID: 18856
	private AccessControl accessTarget;

	// Token: 0x040049A9 RID: 18857
	private List<DoorToggleSideScreen.DoorButtonInfo> buttonList = new List<DoorToggleSideScreen.DoorButtonInfo>();

	// Token: 0x02001E94 RID: 7828
	private struct DoorButtonInfo
	{
		// Token: 0x04008AFC RID: 35580
		public KToggle button;

		// Token: 0x04008AFD RID: 35581
		public Door.ControlState state;

		// Token: 0x04008AFE RID: 35582
		public string currentString;

		// Token: 0x04008AFF RID: 35583
		public string pendingString;
	}
}
