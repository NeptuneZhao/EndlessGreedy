using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02000B9B RID: 2971
public class ImmigrantScreen : CharacterSelectionController
{
	// Token: 0x0600599E RID: 22942 RVA: 0x002069EC File Offset: 0x00204BEC
	public static void DestroyInstance()
	{
		ImmigrantScreen.instance = null;
	}

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x0600599F RID: 22943 RVA: 0x002069F4 File Offset: 0x00204BF4
	public Telepad Telepad
	{
		get
		{
			return this.telepad;
		}
	}

	// Token: 0x060059A0 RID: 22944 RVA: 0x002069FC File Offset: 0x00204BFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060059A1 RID: 22945 RVA: 0x00206A04 File Offset: 0x00204C04
	protected override void OnSpawn()
	{
		this.activateOnSpawn = false;
		base.ConsumeMouseScroll = false;
		base.OnSpawn();
		base.IsStarterMinion = false;
		this.rejectButton.onClick += this.OnRejectAll;
		this.confirmRejectionBtn.onClick += this.OnRejectionConfirmed;
		this.cancelRejectionBtn.onClick += this.OnRejectionCancelled;
		ImmigrantScreen.instance = this;
		this.title.text = UI.IMMIGRANTSCREEN.IMMIGRANTSCREENTITLE;
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.PROCEEDBUTTON;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.Show(false);
	}

	// Token: 0x060059A2 RID: 22946 RVA: 0x00206AC4 File Offset: 0x00204CC4
	protected override void OnShow(bool show)
	{
		if (show)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("Dialog_Popup", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
			MusicManager.instance.PlaySong("Music_SelectDuplicant", false);
			this.hasShown = true;
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.StopSong("Music_SelectDuplicant", true, STOP_MODE.ALLOWFADEOUT);
			}
			if (Immigration.Instance.ImmigrantsAvailable && this.hasShown)
			{
				AudioMixer.instance.Start(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot);
			}
		}
		base.OnShow(show);
	}

	// Token: 0x060059A3 RID: 22947 RVA: 0x00206B7A File Offset: 0x00204D7A
	public void DebugShuffleOptions()
	{
		this.OnRejectionConfirmed();
		Immigration.Instance.timeBeforeSpawn = 0f;
	}

	// Token: 0x060059A4 RID: 22948 RVA: 0x00206B91 File Offset: 0x00204D91
	public override void OnPressBack()
	{
		if (this.rejectConfirmationScreen.activeSelf)
		{
			this.OnRejectionCancelled();
			return;
		}
		base.OnPressBack();
	}

	// Token: 0x060059A5 RID: 22949 RVA: 0x00206BAD File Offset: 0x00204DAD
	public override void Deactivate()
	{
		this.Show(false);
	}

	// Token: 0x060059A6 RID: 22950 RVA: 0x00206BB6 File Offset: 0x00204DB6
	public static void InitializeImmigrantScreen(Telepad telepad)
	{
		ImmigrantScreen.instance.Initialize(telepad);
		ImmigrantScreen.instance.Show(true);
	}

	// Token: 0x060059A7 RID: 22951 RVA: 0x00206BD0 File Offset: 0x00204DD0
	private void Initialize(Telepad telepad)
	{
		this.InitializeContainers();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.SetReshufflingState(false);
			}
		}
		this.telepad = telepad;
	}

	// Token: 0x060059A8 RID: 22952 RVA: 0x00206C40 File Offset: 0x00204E40
	protected override void OnProceed()
	{
		this.telepad.OnAcceptDelivery(this.selectedDeliverables[0]);
		this.Show(false);
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.PlaySong("Stinger_NewDuplicant", false);
	}

	// Token: 0x060059A9 RID: 22953 RVA: 0x00206CDC File Offset: 0x00204EDC
	private void OnRejectAll()
	{
		this.rejectConfirmationScreen.transform.SetAsLastSibling();
		this.rejectConfirmationScreen.SetActive(true);
	}

	// Token: 0x060059AA RID: 22954 RVA: 0x00206CFA File Offset: 0x00204EFA
	private void OnRejectionCancelled()
	{
		this.rejectConfirmationScreen.SetActive(false);
	}

	// Token: 0x060059AB RID: 22955 RVA: 0x00206D08 File Offset: 0x00204F08
	private void OnRejectionConfirmed()
	{
		this.telepad.RejectAll();
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		this.rejectConfirmationScreen.SetActive(false);
		this.Show(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x04003AEF RID: 15087
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04003AF0 RID: 15088
	[SerializeField]
	private KButton rejectButton;

	// Token: 0x04003AF1 RID: 15089
	[SerializeField]
	private LocText title;

	// Token: 0x04003AF2 RID: 15090
	[SerializeField]
	private GameObject rejectConfirmationScreen;

	// Token: 0x04003AF3 RID: 15091
	[SerializeField]
	private KButton confirmRejectionBtn;

	// Token: 0x04003AF4 RID: 15092
	[SerializeField]
	private KButton cancelRejectionBtn;

	// Token: 0x04003AF5 RID: 15093
	public static ImmigrantScreen instance;

	// Token: 0x04003AF6 RID: 15094
	private Telepad telepad;

	// Token: 0x04003AF7 RID: 15095
	private bool hasShown;
}
