using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DF7 RID: 3575
[AddComponentMenu("KMonoBehaviour/scripts/MinMaxSlider")]
public class MinMaxSlider : KMonoBehaviour
{
	// Token: 0x170007DB RID: 2011
	// (get) Token: 0x06007173 RID: 29043 RVA: 0x002AE8C5 File Offset: 0x002ACAC5
	// (set) Token: 0x06007174 RID: 29044 RVA: 0x002AE8CD File Offset: 0x002ACACD
	public MinMaxSlider.Mode mode { get; private set; }

	// Token: 0x06007175 RID: 29045 RVA: 0x002AE8D8 File Offset: 0x002ACAD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ToolTip component = base.transform.parent.gameObject.GetComponent<ToolTip>();
		if (component != null)
		{
			UnityEngine.Object.DestroyImmediate(this.toolTip);
			this.toolTip = component;
		}
		this.minSlider.value = this.currentMinValue;
		this.maxSlider.value = this.currentMaxValue;
		this.minSlider.interactable = this.interactable;
		this.maxSlider.interactable = this.interactable;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.direction = (this.maxSlider.direction = this.direction);
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = false;
		}
		this.minSlider.gameObject.SetActive(false);
		if (this.mode != MinMaxSlider.Mode.Single)
		{
			this.minSlider.gameObject.SetActive(true);
		}
		if (this.extraSlider != null)
		{
			this.extraSlider.value = this.currentExtraValue;
			this.extraSlider.wholeNumbers = (this.minSlider.wholeNumbers = (this.maxSlider.wholeNumbers = this.wholeNumbers));
			this.extraSlider.direction = this.direction;
			this.extraSlider.interactable = this.interactable;
			this.extraSlider.maxValue = this.maxLimit;
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.gameObject.SetActive(false);
			if (this.mode == MinMaxSlider.Mode.Triple)
			{
				this.extraSlider.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06007176 RID: 29046 RVA: 0x002AEAC8 File Offset: 0x002ACCC8
	public void SetIcon(Image newIcon)
	{
		this.icon = newIcon;
		this.icon.gameObject.transform.SetParent(base.transform);
		this.icon.gameObject.transform.SetAsFirstSibling();
		this.icon.rectTransform().anchoredPosition = Vector2.zero;
	}

	// Token: 0x06007177 RID: 29047 RVA: 0x002AEB24 File Offset: 0x002ACD24
	public void SetMode(MinMaxSlider.Mode mode)
	{
		this.mode = mode;
		if (mode == MinMaxSlider.Mode.Single && this.extraSlider != null)
		{
			this.extraSlider.gameObject.SetActive(false);
			this.extraSlider.handleRect.gameObject.SetActive(false);
		}
	}

	// Token: 0x06007178 RID: 29048 RVA: 0x002AEB70 File Offset: 0x002ACD70
	private void SetAnchor(RectTransform trans, Vector2 min, Vector2 max)
	{
		trans.anchorMin = min;
		trans.anchorMax = max;
	}

	// Token: 0x06007179 RID: 29049 RVA: 0x002AEB80 File Offset: 0x002ACD80
	public void SetMinMaxValue(float currentMin, float currentMax, float min, float max)
	{
		this.minSlider.value = currentMin;
		this.currentMinValue = currentMin;
		this.maxSlider.value = currentMax;
		this.currentMaxValue = currentMax;
		this.minLimit = min;
		this.maxLimit = max;
		this.minSlider.minValue = this.minLimit;
		this.maxSlider.minValue = this.minLimit;
		this.minSlider.maxValue = this.maxLimit;
		this.maxSlider.maxValue = this.maxLimit;
		if (this.extraSlider != null)
		{
			this.extraSlider.minValue = this.minLimit;
			this.extraSlider.maxValue = this.maxLimit;
		}
	}

	// Token: 0x0600717A RID: 29050 RVA: 0x002AEC3A File Offset: 0x002ACE3A
	public void SetExtraValue(float current)
	{
		this.extraSlider.value = current;
		this.toolTip.toolTip = base.transform.parent.name + ": " + current.ToString("F2");
	}

	// Token: 0x0600717B RID: 29051 RVA: 0x002AEC7C File Offset: 0x002ACE7C
	public void SetMaxValue(float current, float max)
	{
		float num = current / max * 100f;
		if (this.isOverPowered != null)
		{
			this.isOverPowered.enabled = (num > 100f);
		}
		this.maxSlider.value = Mathf.Min(100f, num);
		if (this.toolTip != null)
		{
			this.toolTip.toolTip = string.Concat(new string[]
			{
				base.transform.parent.name,
				": ",
				current.ToString("F2"),
				"/",
				max.ToString("F2")
			});
		}
	}

	// Token: 0x0600717C RID: 29052 RVA: 0x002AED30 File Offset: 0x002ACF30
	private void Update()
	{
		if (!this.interactable)
		{
			return;
		}
		this.minSlider.value = Mathf.Clamp(this.currentMinValue, this.minLimit, this.currentMinValue);
		this.maxSlider.value = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.currentMaxValue, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		if (this.direction == Slider.Direction.LeftToRight || this.direction == Slider.Direction.RightToLeft)
		{
			this.minRect.anchorMax = new Vector2(this.minSlider.value / this.maxLimit, this.minRect.anchorMax.y);
			this.maxRect.anchorMax = new Vector2(this.maxSlider.value / this.maxLimit, this.maxRect.anchorMax.y);
			this.maxRect.anchorMin = new Vector2(this.minSlider.value / this.maxLimit, this.maxRect.anchorMin.y);
			return;
		}
		this.minRect.anchorMax = new Vector2(this.minRect.anchorMin.x, this.minSlider.value / this.maxLimit);
		this.maxRect.anchorMin = new Vector2(this.maxRect.anchorMin.x, this.minSlider.value / this.maxLimit);
	}

	// Token: 0x0600717D RID: 29053 RVA: 0x002AEEBC File Offset: 0x002AD0BC
	public void OnMinValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMaxValue = Mathf.Min(Mathf.Max(this.minLimit, this.minSlider.value) + this.range, this.maxLimit);
			this.currentMinValue = Mathf.Max(this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue - this.range));
		}
		else
		{
			this.currentMinValue = Mathf.Clamp(this.minSlider.value, this.minLimit, Mathf.Min(this.maxSlider.value, this.currentMaxValue));
		}
		if (this.onMinChange != null)
		{
			this.onMinChange(this);
		}
	}

	// Token: 0x0600717E RID: 29054 RVA: 0x002AEF80 File Offset: 0x002AD180
	public void OnMaxValueChanged(float ignoreThis)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange)
		{
			this.currentMinValue = Mathf.Max(this.maxSlider.value - this.range, this.minLimit);
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.currentMinValue + this.range, this.minLimit), this.maxLimit));
		}
		else
		{
			this.currentMaxValue = Mathf.Max(this.minSlider.value, Mathf.Clamp(this.maxSlider.value, Mathf.Max(this.minSlider.value, this.minLimit), this.maxLimit));
		}
		if (this.onMaxChange != null)
		{
			this.onMaxChange(this);
		}
	}

	// Token: 0x0600717F RID: 29055 RVA: 0x002AF060 File Offset: 0x002AD260
	public void Lock(bool shouldLock)
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Drag)
		{
			this.lockRange = shouldLock;
			this.range = this.maxSlider.value - this.minSlider.value;
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x06007180 RID: 29056 RVA: 0x002AF0B0 File Offset: 0x002AD2B0
	public void ToggleLock()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockType == MinMaxSlider.LockingType.Toggle)
		{
			this.lockRange = !this.lockRange;
			if (this.lockRange)
			{
				this.range = this.maxSlider.value - this.minSlider.value;
			}
		}
	}

	// Token: 0x06007181 RID: 29057 RVA: 0x002AF104 File Offset: 0x002AD304
	public void OnDrag()
	{
		if (!this.interactable)
		{
			return;
		}
		if (this.lockRange && this.lockType == MinMaxSlider.LockingType.Drag)
		{
			float num = KInputManager.GetMousePos().x - this.mousePos.x;
			if (this.direction == Slider.Direction.TopToBottom || this.direction == Slider.Direction.BottomToTop)
			{
				num = KInputManager.GetMousePos().y - this.mousePos.y;
			}
			this.currentMinValue = Mathf.Max(this.currentMinValue + num, this.minLimit);
			this.mousePos = KInputManager.GetMousePos();
		}
	}

	// Token: 0x04004E21 RID: 20001
	public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

	// Token: 0x04004E23 RID: 20003
	public bool lockRange;

	// Token: 0x04004E24 RID: 20004
	public bool interactable = true;

	// Token: 0x04004E25 RID: 20005
	public float minLimit;

	// Token: 0x04004E26 RID: 20006
	public float maxLimit = 100f;

	// Token: 0x04004E27 RID: 20007
	public float range = 50f;

	// Token: 0x04004E28 RID: 20008
	public float barWidth = 10f;

	// Token: 0x04004E29 RID: 20009
	public float barHeight = 100f;

	// Token: 0x04004E2A RID: 20010
	public float currentMinValue = 10f;

	// Token: 0x04004E2B RID: 20011
	public float currentMaxValue = 90f;

	// Token: 0x04004E2C RID: 20012
	public float currentExtraValue = 50f;

	// Token: 0x04004E2D RID: 20013
	public Slider.Direction direction;

	// Token: 0x04004E2E RID: 20014
	public bool wholeNumbers = true;

	// Token: 0x04004E2F RID: 20015
	public Action<MinMaxSlider> onMinChange;

	// Token: 0x04004E30 RID: 20016
	public Action<MinMaxSlider> onMaxChange;

	// Token: 0x04004E31 RID: 20017
	public Slider minSlider;

	// Token: 0x04004E32 RID: 20018
	public Slider maxSlider;

	// Token: 0x04004E33 RID: 20019
	public Slider extraSlider;

	// Token: 0x04004E34 RID: 20020
	public RectTransform minRect;

	// Token: 0x04004E35 RID: 20021
	public RectTransform maxRect;

	// Token: 0x04004E36 RID: 20022
	public RectTransform bgFill;

	// Token: 0x04004E37 RID: 20023
	public RectTransform mgFill;

	// Token: 0x04004E38 RID: 20024
	public RectTransform fgFill;

	// Token: 0x04004E39 RID: 20025
	public Text title;

	// Token: 0x04004E3A RID: 20026
	[MyCmpGet]
	public ToolTip toolTip;

	// Token: 0x04004E3B RID: 20027
	public Image icon;

	// Token: 0x04004E3C RID: 20028
	public Image isOverPowered;

	// Token: 0x04004E3D RID: 20029
	private Vector3 mousePos;

	// Token: 0x02001EFD RID: 7933
	public enum LockingType
	{
		// Token: 0x04008C47 RID: 35911
		Toggle,
		// Token: 0x04008C48 RID: 35912
		Drag
	}

	// Token: 0x02001EFE RID: 7934
	public enum Mode
	{
		// Token: 0x04008C4A RID: 35914
		Single,
		// Token: 0x04008C4B RID: 35915
		Double,
		// Token: 0x04008C4C RID: 35916
		Triple
	}
}
