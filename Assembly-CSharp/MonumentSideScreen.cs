using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000D80 RID: 3456
public class MonumentSideScreen : SideScreenContent
{
	// Token: 0x06006CC6 RID: 27846 RVA: 0x0028ECCC File Offset: 0x0028CECC
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MonumentPart>() != null;
	}

	// Token: 0x06006CC7 RID: 27847 RVA: 0x0028ECDC File Offset: 0x0028CEDC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.debugVictoryButton.onClick += delegate()
		{
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Thriving.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			GameScheduler.Instance.Schedule("ForceCheckAchievements", 0.1f, delegate(object data)
			{
				Game.Instance.Trigger(395452326, null);
			}, null, null);
		};
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.flipButton.onClick += delegate()
		{
			this.target.GetComponent<Rotatable>().Rotate();
		};
	}

	// Token: 0x06006CC8 RID: 27848 RVA: 0x0028ED58 File Offset: 0x0028CF58
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<MonumentPart>();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.GenerateStateButtons();
	}

	// Token: 0x06006CC9 RID: 27849 RVA: 0x0028EDA8 File Offset: 0x0028CFA8
	public void GenerateStateButtons()
	{
		for (int i = this.buttons.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.buttons[i]);
		}
		this.buttons.Clear();
		using (List<MonumentPartResource>.Enumerator enumerator = Db.GetMonumentParts().GetParts(this.target.part).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MonumentPartResource state = enumerator.Current;
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				string state2 = state.State;
				string symbolName = state.SymbolName;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.target.SetState(state.Id);
				};
				this.buttons.Add(gameObject);
				gameObject.GetComponent<KButton>().fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(state.AnimFile, state2, false, symbolName);
			}
		}
	}

	// Token: 0x04004A2F RID: 18991
	private MonumentPart target;

	// Token: 0x04004A30 RID: 18992
	public KButton debugVictoryButton;

	// Token: 0x04004A31 RID: 18993
	public KButton flipButton;

	// Token: 0x04004A32 RID: 18994
	public GameObject stateButtonPrefab;

	// Token: 0x04004A33 RID: 18995
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x04004A34 RID: 18996
	[SerializeField]
	private RectTransform buttonContainer;
}
