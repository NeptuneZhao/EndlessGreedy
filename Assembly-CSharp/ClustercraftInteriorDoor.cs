using System;
using STRINGS;

// Token: 0x02000AC1 RID: 2753
public class ClustercraftInteriorDoor : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x0600518C RID: 20876 RVA: 0x001D40D6 File Offset: 0x001D22D6
	public string SidescreenButtonText
	{
		get
		{
			return UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL;
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x0600518D RID: 20877 RVA: 0x001D40E2 File Offset: 0x001D22E2
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.SidescreenButtonInteractable() ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.INVALID;
		}
	}

	// Token: 0x0600518E RID: 20878 RVA: 0x001D40FD File Offset: 0x001D22FD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterCraftInteriorDoors.Add(this);
	}

	// Token: 0x0600518F RID: 20879 RVA: 0x001D4110 File Offset: 0x001D2310
	protected override void OnCleanUp()
	{
		Components.ClusterCraftInteriorDoors.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06005190 RID: 20880 RVA: 0x001D4123 File Offset: 0x001D2323
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06005191 RID: 20881 RVA: 0x001D4128 File Offset: 0x001D2328
	public bool SidescreenButtonInteractable()
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		return myWorld.ParentWorldId != 255 && myWorld.ParentWorldId != myWorld.id;
	}

	// Token: 0x06005192 RID: 20882 RVA: 0x001D4161 File Offset: 0x001D2361
	public void OnSidescreenButtonPressed()
	{
		ClusterManager.Instance.SetActiveWorld(base.gameObject.GetMyWorld().ParentWorldId);
	}

	// Token: 0x06005193 RID: 20883 RVA: 0x001D417D File Offset: 0x001D237D
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06005194 RID: 20884 RVA: 0x001D4181 File Offset: 0x001D2381
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005195 RID: 20885 RVA: 0x001D4188 File Offset: 0x001D2388
	public int HorizontalGroupID()
	{
		return -1;
	}
}
