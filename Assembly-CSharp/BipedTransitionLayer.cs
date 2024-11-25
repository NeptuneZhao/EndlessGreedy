using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020005E8 RID: 1512
public class BipedTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x060024A2 RID: 9378 RVA: 0x000CC004 File Offset: 0x000CA204
	public BipedTransitionLayer(Navigator navigator, float floor_speed, float ladder_speed) : base(navigator)
	{
		navigator.Subscribe(1773898642, delegate(object data)
		{
			this.isWalking = true;
		});
		navigator.Subscribe(1597112836, delegate(object data)
		{
			this.isWalking = false;
		});
		this.floorSpeed = floor_speed;
		this.ladderSpeed = ladder_speed;
		this.jetPackSpeed = floor_speed;
		this.movementSpeed = Db.Get().AttributeConverters.MovementSpeed.Lookup(navigator.gameObject);
		this.attributeLevels = navigator.GetComponent<AttributeLevels>();
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x000CC08C File Offset: 0x000CA28C
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		float num = 1f;
		bool flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) && transition.y < 0 && transition.x == 0;
		bool flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
		bool flag3 = transition.start == NavType.Hover || transition.end == NavType.Hover;
		if (!flag && !flag2 && !flag3)
		{
			if (this.isWalking)
			{
				return;
			}
			num = this.GetMovementSpeedMultiplier(navigator);
		}
		int cell = Grid.PosToCell(navigator);
		float num2 = 1f;
		bool flag4 = (navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
		bool flag5 = (navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
		bool flag6 = (navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
		if (!flag5 && !flag4 && !flag6 && Grid.IsSubstantialLiquid(cell, 0.35f))
		{
			num2 = 0.5f;
		}
		num *= num2;
		if (transition.x == 0 && (transition.start == NavType.Ladder || transition.start == NavType.Pole) && transition.start == transition.end)
		{
			if (flag)
			{
				transition.speed = 15f * num2;
			}
			else
			{
				transition.speed = this.ladderSpeed * num;
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					Ladder component = gameObject.GetComponent<Ladder>();
					if (component != null)
					{
						float num3 = component.upwardsMovementSpeedMultiplier;
						if (transition.y < 0)
						{
							num3 = component.downwardsMovementSpeedMultiplier;
						}
						transition.speed *= num3;
						transition.animSpeed *= num3;
					}
				}
			}
		}
		else if (flag2)
		{
			transition.speed = this.GetTubeTravellingSpeedMultiplier(navigator);
		}
		else if (flag3)
		{
			transition.speed = this.jetPackSpeed;
		}
		else
		{
			transition.speed = this.floorSpeed * num;
		}
		float num4 = num - 1f;
		transition.animSpeed += transition.animSpeed * num4 / 2f;
		if (transition.start == NavType.Floor && transition.end == NavType.Floor)
		{
			int num5 = Grid.CellBelow(cell);
			if (Grid.Foundation[num5])
			{
				GameObject gameObject2 = Grid.Objects[num5, 1];
				if (gameObject2 != null)
				{
					SimCellOccupier component2 = gameObject2.GetComponent<SimCellOccupier>();
					if (component2 != null)
					{
						transition.speed *= component2.movementSpeedMultiplier;
						transition.animSpeed *= component2.movementSpeedMultiplier;
					}
				}
			}
		}
		this.startTime = Time.time;
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x000CC30C File Offset: 0x000CA50C
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		bool flag = (transition.start == NavType.Pole || transition.end == NavType.Pole) && transition.y < 0 && transition.x == 0;
		bool flag2 = transition.start == NavType.Tube || transition.end == NavType.Tube;
		if (!this.isWalking && !flag && !flag2 && this.attributeLevels != null)
		{
			this.attributeLevels.AddExperience(Db.Get().Attributes.Athletics.Id, Time.time - this.startTime, DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE);
		}
	}

	// Token: 0x060024A5 RID: 9381 RVA: 0x000CC3AC File Offset: 0x000CA5AC
	public float GetTubeTravellingSpeedMultiplier(Navigator navigator)
	{
		AttributeInstance attributeInstance = Db.Get().Attributes.TransitTubeTravelSpeed.Lookup(navigator.gameObject);
		if (attributeInstance != null)
		{
			return attributeInstance.GetTotalValue();
		}
		return DUPLICANTSTATS.STANDARD.BaseStats.TRANSIT_TUBE_TRAVEL_SPEED;
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x000CC3F0 File Offset: 0x000CA5F0
	public float GetMovementSpeedMultiplier(Navigator navigator)
	{
		float num = 1f;
		if (this.movementSpeed != null)
		{
			num += this.movementSpeed.Evaluate();
		}
		return Mathf.Max(0.1f, num);
	}

	// Token: 0x040014C0 RID: 5312
	private bool isWalking;

	// Token: 0x040014C1 RID: 5313
	private float floorSpeed;

	// Token: 0x040014C2 RID: 5314
	private float ladderSpeed;

	// Token: 0x040014C3 RID: 5315
	private float startTime;

	// Token: 0x040014C4 RID: 5316
	private float jetPackSpeed;

	// Token: 0x040014C5 RID: 5317
	private const float downPoleSpeed = 15f;

	// Token: 0x040014C6 RID: 5318
	private const float WATER_SPEED_PENALTY = 0.5f;

	// Token: 0x040014C7 RID: 5319
	private AttributeConverterInstance movementSpeed;

	// Token: 0x040014C8 RID: 5320
	private AttributeLevels attributeLevels;
}
