using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000F5A RID: 3930
	public class PeriodicEmoteSickness : Sickness.SicknessComponent
	{
		// Token: 0x060078B6 RID: 30902 RVA: 0x002FC698 File Offset: 0x002FA898
		public PeriodicEmoteSickness(Emote emote, float cooldown)
		{
			this.emote = emote;
			this.cooldown = cooldown;
		}

		// Token: 0x060078B7 RID: 30903 RVA: 0x002FC6AE File Offset: 0x002FA8AE
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			PeriodicEmoteSickness.StatesInstance statesInstance = new PeriodicEmoteSickness.StatesInstance(diseaseInstance, this);
			statesInstance.StartSM();
			return statesInstance;
		}

		// Token: 0x060078B8 RID: 30904 RVA: 0x002FC6BD File Offset: 0x002FA8BD
		public override void OnCure(GameObject go, object instance_data)
		{
			((PeriodicEmoteSickness.StatesInstance)instance_data).StopSM("Cured");
		}

		// Token: 0x04005A2D RID: 23085
		private Emote emote;

		// Token: 0x04005A2E RID: 23086
		private float cooldown;

		// Token: 0x02002341 RID: 9025
		public class StatesInstance : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600B60C RID: 46604 RVA: 0x003C9240 File Offset: 0x003C7440
			public StatesInstance(SicknessInstance master, PeriodicEmoteSickness periodicEmoteSickness) : base(master)
			{
				this.periodicEmoteSickness = periodicEmoteSickness;
			}

			// Token: 0x0600B60D RID: 46605 RVA: 0x003C9250 File Offset: 0x003C7450
			public Reactable GetReactable()
			{
				return new SelfEmoteReactable(base.master.gameObject, "PeriodicEmoteSickness", Db.Get().ChoreTypes.Emote, 0f, this.periodicEmoteSickness.cooldown, float.PositiveInfinity, 0f).SetEmote(this.periodicEmoteSickness.emote).SetOverideAnimSet("anim_sneeze_kanim");
			}

			// Token: 0x04009E2D RID: 40493
			public PeriodicEmoteSickness periodicEmoteSickness;
		}

		// Token: 0x02002342 RID: 9026
		public class States : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600B60E RID: 46606 RVA: 0x003C92BA File Offset: 0x003C74BA
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.root.ToggleReactable((PeriodicEmoteSickness.StatesInstance smi) => smi.GetReactable());
			}
		}
	}
}
