using System;
using UnityEngine;

// Token: 0x02000A86 RID: 2694
[AddComponentMenu("KMonoBehaviour/scripts/ScreenPrefabs")]
public class ScreenPrefabs : KMonoBehaviour
{
	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06004F28 RID: 20264 RVA: 0x001C7C11 File Offset: 0x001C5E11
	// (set) Token: 0x06004F29 RID: 20265 RVA: 0x001C7C18 File Offset: 0x001C5E18
	public static ScreenPrefabs Instance { get; private set; }

	// Token: 0x06004F2A RID: 20266 RVA: 0x001C7C20 File Offset: 0x001C5E20
	protected override void OnPrefabInit()
	{
		ScreenPrefabs.Instance = this;
	}

	// Token: 0x06004F2B RID: 20267 RVA: 0x001C7C28 File Offset: 0x001C5E28
	public void ConfirmDoAction(string message, System.Action action, Transform parent)
	{
		((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent.gameObject)).PopupConfirmDialog(message, action, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x04003488 RID: 13448
	public ControlsScreen ControlsScreen;

	// Token: 0x04003489 RID: 13449
	public Hud HudScreen;

	// Token: 0x0400348A RID: 13450
	public HoverTextScreen HoverTextScreen;

	// Token: 0x0400348B RID: 13451
	public OverlayScreen OverlayScreen;

	// Token: 0x0400348C RID: 13452
	public TileScreen TileScreen;

	// Token: 0x0400348D RID: 13453
	public SpeedControlScreen SpeedControlScreen;

	// Token: 0x0400348E RID: 13454
	public ManagementMenu ManagementMenu;

	// Token: 0x0400348F RID: 13455
	public ToolTipScreen ToolTipScreen;

	// Token: 0x04003490 RID: 13456
	public DebugPaintElementScreen DebugPaintElementScreen;

	// Token: 0x04003491 RID: 13457
	public UserMenuScreen UserMenuScreen;

	// Token: 0x04003492 RID: 13458
	public KButtonMenu OwnerScreen;

	// Token: 0x04003493 RID: 13459
	public KButtonMenu ButtonGrid;

	// Token: 0x04003494 RID: 13460
	public NameDisplayScreen NameDisplayScreen;

	// Token: 0x04003495 RID: 13461
	public ConfirmDialogScreen ConfirmDialogScreen;

	// Token: 0x04003496 RID: 13462
	public CustomizableDialogScreen CustomizableDialogScreen;

	// Token: 0x04003497 RID: 13463
	public SpriteListDialogScreen SpriteListDialogScreen;

	// Token: 0x04003498 RID: 13464
	public InfoDialogScreen InfoDialogScreen;

	// Token: 0x04003499 RID: 13465
	public StoryMessageScreen StoryMessageScreen;

	// Token: 0x0400349A RID: 13466
	public SubSpeciesInfoScreen SubSpeciesInfoScreen;

	// Token: 0x0400349B RID: 13467
	public EventInfoScreen eventInfoScreen;

	// Token: 0x0400349C RID: 13468
	public FileNameDialog FileNameDialog;

	// Token: 0x0400349D RID: 13469
	public TagFilterScreen TagFilterScreen;

	// Token: 0x0400349E RID: 13470
	public ResearchScreen ResearchScreen;

	// Token: 0x0400349F RID: 13471
	public MessageDialogFrame MessageDialogFrame;

	// Token: 0x040034A0 RID: 13472
	public ResourceCategoryScreen ResourceCategoryScreen;

	// Token: 0x040034A1 RID: 13473
	public ColonyDiagnosticScreen ColonyDiagnosticScreen;

	// Token: 0x040034A2 RID: 13474
	public LanguageOptionsScreen languageOptionsScreen;

	// Token: 0x040034A3 RID: 13475
	public ModsScreen modsMenu;

	// Token: 0x040034A4 RID: 13476
	public RailModUploadScreen RailModUploadMenu;

	// Token: 0x040034A5 RID: 13477
	public GameObject GameOverScreen;

	// Token: 0x040034A6 RID: 13478
	public GameObject VictoryScreen;

	// Token: 0x040034A7 RID: 13479
	public GameObject StatusItemIndicatorScreen;

	// Token: 0x040034A8 RID: 13480
	public GameObject CollapsableContentPanel;

	// Token: 0x040034A9 RID: 13481
	public GameObject DescriptionLabel;

	// Token: 0x040034AA RID: 13482
	public LoadingOverlay loadingOverlay;

	// Token: 0x040034AB RID: 13483
	public LoadScreen LoadScreen;

	// Token: 0x040034AC RID: 13484
	public InspectSaveScreen InspectSaveScreen;

	// Token: 0x040034AD RID: 13485
	public OptionsMenuScreen OptionsScreen;

	// Token: 0x040034AE RID: 13486
	public WorldGenScreen WorldGenScreen;

	// Token: 0x040034AF RID: 13487
	public ModeSelectScreen ModeSelectScreen;

	// Token: 0x040034B0 RID: 13488
	public ColonyDestinationSelectScreen ColonyDestinationSelectScreen;

	// Token: 0x040034B1 RID: 13489
	public RetiredColonyInfoScreen RetiredColonyInfoScreen;

	// Token: 0x040034B2 RID: 13490
	public VideoScreen VideoScreen;

	// Token: 0x040034B3 RID: 13491
	public ComicViewer ComicViewer;

	// Token: 0x040034B4 RID: 13492
	public GameObject OldVersionWarningScreen;

	// Token: 0x040034B5 RID: 13493
	public GameObject DLCBetaWarningScreen;

	// Token: 0x040034B6 RID: 13494
	[Header("Klei Items")]
	public GameObject KleiItemDropScreen;

	// Token: 0x040034B7 RID: 13495
	public GameObject LockerMenuScreen;

	// Token: 0x040034B8 RID: 13496
	public GameObject LockerNavigator;

	// Token: 0x040034B9 RID: 13497
	[Header("Main Menu")]
	public GameObject MainMenuForVanilla;

	// Token: 0x040034BA RID: 13498
	public GameObject MainMenuForSpacedOut;

	// Token: 0x040034BB RID: 13499
	public GameObject MainMenuIntroShort;

	// Token: 0x040034BC RID: 13500
	public GameObject MainMenuHealthyGameMessage;
}
