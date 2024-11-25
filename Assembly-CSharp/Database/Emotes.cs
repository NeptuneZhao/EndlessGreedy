using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000E60 RID: 3680
	public class Emotes : ResourceSet<Resource>
	{
		// Token: 0x0600747B RID: 29819 RVA: 0x002D456D File Offset: 0x002D276D
		public Emotes(ResourceSet parent) : base("Emotes", parent)
		{
			this.Minion = new Emotes.MinionEmotes(this);
			this.Critter = new Emotes.CritterEmotes(this);
		}

		// Token: 0x0600747C RID: 29820 RVA: 0x002D4594 File Offset: 0x002D2794
		public void ResetProblematicReferences()
		{
			for (int i = 0; i < this.Minion.resources.Count; i++)
			{
				Emote emote = this.Minion.resources[i];
				for (int j = 0; j < emote.StepCount; j++)
				{
					emote[j].UnregisterAllCallbacks();
				}
			}
			for (int k = 0; k < this.Critter.resources.Count; k++)
			{
				Emote emote2 = this.Critter.resources[k];
				for (int l = 0; l < emote2.StepCount; l++)
				{
					emote2[l].UnregisterAllCallbacks();
				}
			}
		}

		// Token: 0x04005341 RID: 21313
		public Emotes.MinionEmotes Minion;

		// Token: 0x04005342 RID: 21314
		public Emotes.CritterEmotes Critter;

		// Token: 0x02001F67 RID: 8039
		public class MinionEmotes : ResourceSet<Emote>
		{
			// Token: 0x0600AEFB RID: 44795 RVA: 0x003AFF35 File Offset: 0x003AE135
			public MinionEmotes(ResourceSet parent) : base("Minion", parent)
			{
				this.InitializeCelebrations();
				this.InitializePhysicalStatus();
				this.InitializeEmotionalStatus();
				this.InitializeGreetings();
			}

			// Token: 0x0600AEFC RID: 44796 RVA: 0x003AFF5C File Offset: 0x003AE15C
			public void InitializeCelebrations()
			{
				this.ClapCheer = new Emote(this, "ClapCheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "clapcheer_pre"
					},
					new EmoteStep
					{
						anim = "clapcheer_loop"
					},
					new EmoteStep
					{
						anim = "clapcheer_pst"
					}
				}, "anim_clapcheer_kanim");
				this.Cheer = new Emote(this, "Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cheer_pre"
					},
					new EmoteStep
					{
						anim = "cheer_loop"
					},
					new EmoteStep
					{
						anim = "cheer_pst"
					}
				}, "anim_cheer_kanim");
				this.ProductiveCheer = new Emote(this, "Productive Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "productive"
					}
				}, "anim_productive_kanim");
				this.ResearchComplete = new Emote(this, "ResearchComplete", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_research_complete_kanim");
				this.ThumbsUp = new Emote(this, "ThumbsUp", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_thumbsup_kanim");
			}

			// Token: 0x0600AEFD RID: 44797 RVA: 0x003B009C File Offset: 0x003AE29C
			private void InitializePhysicalStatus()
			{
				this.CloseCall_Fall = new Emote(this, "Near Fall", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_floor_missing_kanim");
				this.Cold = new Emote(this, "Cold", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_cold_kanim");
				this.Cough = new Emote(this, "Cough", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_slimelungcough_kanim");
				this.Cough_Small = new Emote(this, "Small Cough", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_small"
					}
				}, "anim_slimelungcough_kanim");
				this.FoodPoisoning = new Emote(this, "Food Poisoning", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_contaminated_food_kanim");
				this.Hot = new Emote(this, "Hot", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_hot_kanim");
				this.IritatedEyes = new Emote(this, "Irritated Eyes", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "irritated_eyes"
					}
				}, "anim_irritated_eyes_kanim");
				this.MorningStretch = new Emote(this, "Morning Stretch", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_morning_stretch_kanim");
				this.Radiation_Glare = new Emote(this, "Radiation Glare", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_glare"
					}
				}, "anim_react_radiation_kanim");
				this.Radiation_Itch = new Emote(this, "Radiation Itch", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_itch"
					}
				}, "anim_react_radiation_kanim");
				this.Sick = new Emote(this, "Sick", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_sick_kanim");
				this.Sneeze = new Emote(this, "Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze"
					},
					new EmoteStep
					{
						anim = "sneeze_pst"
					}
				}, "anim_sneeze_kanim");
				this.WaterDamage = new Emote(this, "WaterDamage", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "zapped"
					}
				}, "anim_bionic_kanim");
				this.Sneeze_Short = new Emote(this, "Short Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze_short"
					},
					new EmoteStep
					{
						anim = "sneeze_short_pst"
					}
				}, "anim_sneeze_kanim");
			}

			// Token: 0x0600AEFE RID: 44798 RVA: 0x003B0304 File Offset: 0x003AE504
			private void InitializeEmotionalStatus()
			{
				this.Concern = new Emote(this, "Concern", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_concern_kanim");
				this.Cringe = new Emote(this, "Cringe", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cringe_pre"
					},
					new EmoteStep
					{
						anim = "cringe_loop"
					},
					new EmoteStep
					{
						anim = "cringe_pst"
					}
				}, "anim_cringe_kanim");
				this.Disappointed = new Emote(this, "Disappointed", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_disappointed_kanim");
				this.Shock = new Emote(this, "Shock", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_shock_kanim");
				this.Sing = new Emote(this, "Sing", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_singer_kanim");
			}

			// Token: 0x0600AEFF RID: 44799 RVA: 0x003B03E4 File Offset: 0x003AE5E4
			private void InitializeGreetings()
			{
				this.FingerGuns = new Emote(this, "Finger Guns", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_fingerguns_kanim");
				this.Wave = new Emote(this, "Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_kanim");
				this.Wave_Shy = new Emote(this, "Shy Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_shy_kanim");
			}

			// Token: 0x04008E43 RID: 36419
			private static EmoteStep[] DEFAULT_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			};

			// Token: 0x04008E44 RID: 36420
			private static EmoteStep[] DEFAULT_IDLE_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "idle_pre"
				},
				new EmoteStep
				{
					anim = "idle_default"
				},
				new EmoteStep
				{
					anim = "idle_pst"
				}
			};

			// Token: 0x04008E45 RID: 36421
			public Emote ClapCheer;

			// Token: 0x04008E46 RID: 36422
			public Emote Cheer;

			// Token: 0x04008E47 RID: 36423
			public Emote ProductiveCheer;

			// Token: 0x04008E48 RID: 36424
			public Emote ResearchComplete;

			// Token: 0x04008E49 RID: 36425
			public Emote ThumbsUp;

			// Token: 0x04008E4A RID: 36426
			public Emote CloseCall_Fall;

			// Token: 0x04008E4B RID: 36427
			public Emote Cold;

			// Token: 0x04008E4C RID: 36428
			public Emote Cough;

			// Token: 0x04008E4D RID: 36429
			public Emote Cough_Small;

			// Token: 0x04008E4E RID: 36430
			public Emote FoodPoisoning;

			// Token: 0x04008E4F RID: 36431
			public Emote Hot;

			// Token: 0x04008E50 RID: 36432
			public Emote IritatedEyes;

			// Token: 0x04008E51 RID: 36433
			public Emote MorningStretch;

			// Token: 0x04008E52 RID: 36434
			public Emote Radiation_Glare;

			// Token: 0x04008E53 RID: 36435
			public Emote Radiation_Itch;

			// Token: 0x04008E54 RID: 36436
			public Emote Sick;

			// Token: 0x04008E55 RID: 36437
			public Emote Sneeze;

			// Token: 0x04008E56 RID: 36438
			public Emote WaterDamage;

			// Token: 0x04008E57 RID: 36439
			public Emote Sneeze_Short;

			// Token: 0x04008E58 RID: 36440
			public Emote Concern;

			// Token: 0x04008E59 RID: 36441
			public Emote Cringe;

			// Token: 0x04008E5A RID: 36442
			public Emote Disappointed;

			// Token: 0x04008E5B RID: 36443
			public Emote Shock;

			// Token: 0x04008E5C RID: 36444
			public Emote Sing;

			// Token: 0x04008E5D RID: 36445
			public Emote FingerGuns;

			// Token: 0x04008E5E RID: 36446
			public Emote Wave;

			// Token: 0x04008E5F RID: 36447
			public Emote Wave_Shy;
		}

		// Token: 0x02001F68 RID: 8040
		public class CritterEmotes : ResourceSet<Emote>
		{
			// Token: 0x0600AF01 RID: 44801 RVA: 0x003B04C7 File Offset: 0x003AE6C7
			public CritterEmotes(ResourceSet parent) : base("Critter", parent)
			{
				this.InitializePhysicalState();
				this.InitializeEmotionalState();
			}

			// Token: 0x0600AF02 RID: 44802 RVA: 0x003B04E4 File Offset: 0x003AE6E4
			private void InitializePhysicalState()
			{
				this.Hungry = new Emote(this, "Hungry", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_hungry"
					}
				}, null);
			}

			// Token: 0x0600AF03 RID: 44803 RVA: 0x003B0524 File Offset: 0x003AE724
			private void InitializeEmotionalState()
			{
				this.Angry = new Emote(this, "Angry", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_angry"
					}
				}, null);
				this.Happy = new Emote(this, "Happy", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_happy"
					}
				}, null);
				this.Idle = new Emote(this, "Idle", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_idle"
					}
				}, null);
				this.Sad = new Emote(this, "Sad", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_sad"
					}
				}, null);
			}

			// Token: 0x04008E60 RID: 36448
			public Emote Hungry;

			// Token: 0x04008E61 RID: 36449
			public Emote Angry;

			// Token: 0x04008E62 RID: 36450
			public Emote Happy;

			// Token: 0x04008E63 RID: 36451
			public Emote Idle;

			// Token: 0x04008E64 RID: 36452
			public Emote Sad;
		}
	}
}
