using System;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000D2A RID: 3370
[AddComponentMenu("KMonoBehaviour/scripts/RocketThrustWidget")]
public class RocketThrustWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060069C0 RID: 27072 RVA: 0x0027CB66 File Offset: 0x0027AD66
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x060069C1 RID: 27073 RVA: 0x0027CB68 File Offset: 0x0027AD68
	public void Draw(CommandModule commandModule)
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
		}
		this.commandModule = commandModule;
		this.totalWidth = this.rectTransform.rect.width;
		this.UpdateGraphDotPos(commandModule);
	}

	// Token: 0x060069C2 RID: 27074 RVA: 0x0027CBC0 File Offset: 0x0027ADC0
	private void UpdateGraphDotPos(CommandModule rocket)
	{
		this.totalWidth = this.rectTransform.rect.width;
		float num = Mathf.Lerp(0f, this.totalWidth, rocket.rocketStats.GetTotalMass() / this.maxMass);
		num = Mathf.Clamp(num, 0f, this.totalWidth);
		this.graphDot.rectTransform.SetLocalPosition(new Vector3(num, 0f, 0f));
		this.graphDotText.text = "-" + Util.FormatWholeNumber(rocket.rocketStats.GetTotalThrust() - rocket.rocketStats.GetRocketMaxDistance()) + "km";
	}

	// Token: 0x060069C3 RID: 27075 RVA: 0x0027CC74 File Offset: 0x0027AE74
	private void Update()
	{
		if (this.mouseOver)
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
			}
			Vector3 position = this.rectTransform.GetPosition();
			Vector2 size = this.rectTransform.rect.size;
			float num = KInputManager.GetMousePos().x - position.x + size.x / 2f;
			num = Mathf.Clamp(num, 0f, this.totalWidth);
			this.hoverMarker.rectTransform.SetLocalPosition(new Vector3(num, 0f, 0f));
			float num2 = Mathf.Lerp(0f, this.maxMass, num / this.totalWidth);
			float totalThrust = this.commandModule.rocketStats.GetTotalThrust();
			float rocketMaxDistance = this.commandModule.rocketStats.GetRocketMaxDistance();
			this.hoverTooltip.SetSimpleTooltip(string.Concat(new string[]
			{
				UI.STARMAP.ROCKETWEIGHT.MASS,
				GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"),
				"\n",
				UI.STARMAP.ROCKETWEIGHT.MASSPENALTY,
				Util.FormatWholeNumber(ROCKETRY.CalculateMassWithPenalty(num2)),
				UI.UNITSUFFIXES.DISTANCE.KILOMETER,
				"\n\n",
				UI.STARMAP.ROCKETWEIGHT.CURRENTMASS,
				GameUtil.GetFormattedMass(this.commandModule.rocketStats.GetTotalMass(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"),
				"\n",
				UI.STARMAP.ROCKETWEIGHT.CURRENTMASSPENALTY,
				Util.FormatWholeNumber(totalThrust - rocketMaxDistance),
				UI.UNITSUFFIXES.DISTANCE.KILOMETER
			}));
		}
	}

	// Token: 0x060069C4 RID: 27076 RVA: 0x0027CE2D File Offset: 0x0027B02D
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.hoverMarker.SetAlpha(1f);
	}

	// Token: 0x060069C5 RID: 27077 RVA: 0x0027CE46 File Offset: 0x0027B046
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.hoverMarker.SetAlpha(0f);
	}

	// Token: 0x04004804 RID: 18436
	public Image graphBar;

	// Token: 0x04004805 RID: 18437
	public Image graphDot;

	// Token: 0x04004806 RID: 18438
	public LocText graphDotText;

	// Token: 0x04004807 RID: 18439
	public Image hoverMarker;

	// Token: 0x04004808 RID: 18440
	public ToolTip hoverTooltip;

	// Token: 0x04004809 RID: 18441
	public RectTransform markersContainer;

	// Token: 0x0400480A RID: 18442
	public Image markerTemplate;

	// Token: 0x0400480B RID: 18443
	private RectTransform rectTransform;

	// Token: 0x0400480C RID: 18444
	private float maxMass = 20000f;

	// Token: 0x0400480D RID: 18445
	private float totalWidth = 5f;

	// Token: 0x0400480E RID: 18446
	private bool mouseOver;

	// Token: 0x0400480F RID: 18447
	public CommandModule commandModule;
}
