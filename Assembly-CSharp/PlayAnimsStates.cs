using System;

// Token: 0x020000C4 RID: 196
public class PlayAnimsStates : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>
{
	// Token: 0x06000383 RID: 899 RVA: 0x0001D1BC File Offset: 0x0001B3BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.animating;
		GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State root = this.root;
		string name = "Unused";
		string tooltip = "Unused";
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemName, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemTooltip, main);
		this.animating.Enter("PlayAnims", delegate(PlayAnimsStates.Instance smi)
		{
			smi.PlayAnims();
		}).OnAnimQueueComplete(this.done).EventHandler(GameHashes.TagsChanged, delegate(PlayAnimsStates.Instance smi, object obj)
		{
			smi.HandleTagsChanged(obj);
		});
		this.done.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((PlayAnimsStates.Instance smi) => smi.def.tag, false);
	}

	// Token: 0x04000274 RID: 628
	public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State animating;

	// Token: 0x04000275 RID: 629
	public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State done;

	// Token: 0x02001009 RID: 4105
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06007B0C RID: 31500 RVA: 0x00302FF4 File Offset: 0x003011F4
		public Def(Tag tag, bool loop, string anim, string status_item_name, string status_item_tooltip) : this(tag, loop, new string[]
		{
			anim
		}, status_item_name, status_item_tooltip)
		{
		}

		// Token: 0x06007B0D RID: 31501 RVA: 0x0030300C File Offset: 0x0030120C
		public Def(Tag tag, bool loop, string[] anims, string status_item_name, string status_item_tooltip)
		{
			this.tag = tag;
			this.loop = loop;
			this.anims = anims;
			this.statusItemName = status_item_name;
			this.statusItemTooltip = status_item_tooltip;
		}

		// Token: 0x06007B0E RID: 31502 RVA: 0x00303039 File Offset: 0x00301239
		public override string ToString()
		{
			return this.tag.ToString() + "(PlayAnimsStates)";
		}

		// Token: 0x04005BFF RID: 23551
		public Tag tag;

		// Token: 0x04005C00 RID: 23552
		public string[] anims;

		// Token: 0x04005C01 RID: 23553
		public bool loop;

		// Token: 0x04005C02 RID: 23554
		public string statusItemName;

		// Token: 0x04005C03 RID: 23555
		public string statusItemTooltip;
	}

	// Token: 0x0200100A RID: 4106
	public new class Instance : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.GameInstance
	{
		// Token: 0x06007B0F RID: 31503 RVA: 0x00303056 File Offset: 0x00301256
		public Instance(Chore<PlayAnimsStates.Instance> chore, PlayAnimsStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.tag);
		}

		// Token: 0x06007B10 RID: 31504 RVA: 0x0030307C File Offset: 0x0030127C
		public void PlayAnims()
		{
			if (base.def.anims == null || base.def.anims.Length == 0)
			{
				return;
			}
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < base.def.anims.Length; i++)
			{
				KAnim.PlayMode mode = KAnim.PlayMode.Once;
				if (base.def.loop && i == base.def.anims.Length - 1)
				{
					mode = KAnim.PlayMode.Loop;
				}
				if (i == 0)
				{
					component.Play(base.def.anims[i], mode, 1f, 0f);
				}
				else
				{
					component.Queue(base.def.anims[i], mode, 1f, 0f);
				}
			}
		}

		// Token: 0x06007B11 RID: 31505 RVA: 0x00303135 File Offset: 0x00301335
		public void HandleTagsChanged(object obj)
		{
			if (!base.smi.HasTag(base.smi.def.tag))
			{
				base.smi.GoTo(null);
			}
		}
	}
}
