using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CEF RID: 3311
[AddComponentMenu("KMonoBehaviour/scripts/NewGameFlow")]
public class NewGameFlow : KMonoBehaviour
{
	// Token: 0x060066B0 RID: 26288 RVA: 0x00265F88 File Offset: 0x00264188
	public void BeginFlow()
	{
		this.currentScreenIndex = -1;
		this.Next();
	}

	// Token: 0x060066B1 RID: 26289 RVA: 0x00265F97 File Offset: 0x00264197
	private void Next()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex++;
		this.ActivateCurrentScreen();
	}

	// Token: 0x060066B2 RID: 26290 RVA: 0x00265FB3 File Offset: 0x002641B3
	private void Previous()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex--;
		this.ActivateCurrentScreen();
	}

	// Token: 0x060066B3 RID: 26291 RVA: 0x00265FCF File Offset: 0x002641CF
	private void ClearCurrentScreen()
	{
		if (this.currentScreen != null)
		{
			this.currentScreen.Deactivate();
			this.currentScreen = null;
		}
	}

	// Token: 0x060066B4 RID: 26292 RVA: 0x00265FF4 File Offset: 0x002641F4
	private void ActivateCurrentScreen()
	{
		if (this.currentScreenIndex >= 0 && this.currentScreenIndex < this.newGameFlowScreens.Count)
		{
			NewGameFlowScreen newGameFlowScreen = Util.KInstantiateUI<NewGameFlowScreen>(this.newGameFlowScreens[this.currentScreenIndex].gameObject, base.transform.parent.gameObject, true);
			newGameFlowScreen.OnNavigateForward += this.Next;
			newGameFlowScreen.OnNavigateBackward += this.Previous;
			if (!newGameFlowScreen.IsActive() && !newGameFlowScreen.activateOnSpawn)
			{
				newGameFlowScreen.Activate();
			}
			this.currentScreen = newGameFlowScreen;
		}
	}

	// Token: 0x0400453E RID: 17726
	public List<NewGameFlowScreen> newGameFlowScreens;

	// Token: 0x0400453F RID: 17727
	private int currentScreenIndex = -1;

	// Token: 0x04004540 RID: 17728
	private NewGameFlowScreen currentScreen;
}
