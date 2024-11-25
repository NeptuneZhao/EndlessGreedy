using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B1B RID: 2843
public class StatusItem : Resource
{
	// Token: 0x06005495 RID: 21653 RVA: 0x001E3F34 File Offset: 0x001E2134
	private StatusItem(string id, string composed_prefix) : base(id, Strings.Get(composed_prefix + ".NAME"))
	{
		this.composedPrefix = composed_prefix;
		this.tooltipText = Strings.Get(composed_prefix + ".TOOLTIP");
	}

	// Token: 0x06005496 RID: 21654 RVA: 0x001E3F88 File Offset: 0x001E2188
	private void SetIcon(string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool show_world_icon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null)
	{
		switch (icon_type)
		{
		case StatusItem.IconType.Info:
			icon = "dash";
			break;
		case StatusItem.IconType.Exclamation:
			icon = "status_item_exclamation";
			break;
		}
		this.iconName = icon;
		this.notificationType = notification_type;
		this.sprite = Assets.GetTintedSprite(icon);
		if (this.sprite == null)
		{
			this.sprite = new TintedSprite();
			this.sprite.sprite = Assets.GetSprite(icon);
			this.sprite.color = new Color(0f, 0f, 0f, 255f);
		}
		this.iconType = icon_type;
		this.allowMultiples = allow_multiples;
		this.render_overlay = render_overlay;
		this.showShowWorldIcon = show_world_icon;
		this.status_overlays = status_overlays;
		this.resolveStringCallback = resolve_string_callback;
		if (this.sprite == null)
		{
			global::Debug.LogWarning("Status item '" + this.Id + "' references a missing icon: " + icon);
		}
	}

	// Token: 0x06005497 RID: 21655 RVA: 0x001E4074 File Offset: 0x001E2274
	public StatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null) : this(id, "STRINGS." + prefix + ".STATUSITEMS." + id.ToUpper())
	{
		this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
	}

	// Token: 0x06005498 RID: 21656 RVA: 0x001E40B4 File Offset: 0x001E22B4
	public StatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022, bool showWorldIcon = true, Func<string, object, string> resolve_string_callback = null) : base(id, name)
	{
		this.tooltipText = tooltip;
		this.SetIcon(icon, icon_type, notification_type, allow_multiples, render_overlay, showWorldIcon, status_overlays, resolve_string_callback);
	}

	// Token: 0x06005499 RID: 21657 RVA: 0x001E40F0 File Offset: 0x001E22F0
	public void AddNotification(string sound_path = null, string notification_text = null, string notification_tooltip = null)
	{
		this.shouldNotify = true;
		if (sound_path == null)
		{
			if (this.notificationType == NotificationType.Bad)
			{
				this.soundPath = "Warning";
			}
			else
			{
				this.soundPath = "Notification";
			}
		}
		else
		{
			this.soundPath = sound_path;
		}
		if (notification_text != null)
		{
			this.notificationText = notification_text;
		}
		else
		{
			DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
			this.notificationText = Strings.Get(this.composedPrefix + ".NOTIFICATION_NAME");
		}
		if (notification_tooltip != null)
		{
			this.notificationTooltipText = notification_tooltip;
			return;
		}
		DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
		this.notificationTooltipText = Strings.Get(this.composedPrefix + ".NOTIFICATION_TOOLTIP");
	}

	// Token: 0x0600549A RID: 21658 RVA: 0x001E41AE File Offset: 0x001E23AE
	public virtual string GetName(object data)
	{
		return this.ResolveString(this.Name, data);
	}

	// Token: 0x0600549B RID: 21659 RVA: 0x001E41BD File Offset: 0x001E23BD
	public virtual string GetTooltip(object data)
	{
		return this.ResolveTooltip(this.tooltipText, data);
	}

	// Token: 0x0600549C RID: 21660 RVA: 0x001E41CC File Offset: 0x001E23CC
	private string ResolveString(string str, object data)
	{
		if (this.resolveStringCallback != null && (data != null || this.resolveStringCallback_shouldStillCallIfDataIsNull))
		{
			return this.resolveStringCallback(str, data);
		}
		return str;
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x001E41F0 File Offset: 0x001E23F0
	private string ResolveTooltip(string str, object data)
	{
		if (data != null)
		{
			if (this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
			if (this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
		}
		else
		{
			if (this.resolveStringCallback_shouldStillCallIfDataIsNull && this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
			if (this.resolveTooltipCallback_shouldStillCallIfDataIsNull && this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
		}
		return str;
	}

	// Token: 0x0600549E RID: 21662 RVA: 0x001E4269 File Offset: 0x001E2469
	public bool ShouldShowIcon()
	{
		return this.iconType == StatusItem.IconType.Custom && this.showShowWorldIcon;
	}

	// Token: 0x0600549F RID: 21663 RVA: 0x001E427C File Offset: 0x001E247C
	public virtual void ShowToolTip(ToolTip tooltip_widget, object data, TextStyleSetting property_style)
	{
		tooltip_widget.ClearMultiStringTooltip();
		string tooltip = this.GetTooltip(data);
		tooltip_widget.AddMultiStringTooltip(tooltip, property_style);
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x001E429F File Offset: 0x001E249F
	public void SetIcon(Image image, object data)
	{
		if (this.sprite == null)
		{
			return;
		}
		image.color = this.sprite.color;
		image.sprite = this.sprite.sprite;
	}

	// Token: 0x060054A1 RID: 21665 RVA: 0x001E42CC File Offset: 0x001E24CC
	public bool UseConditionalCallback(HashedString overlay, Transform transform)
	{
		return overlay != OverlayModes.None.ID && this.conditionalOverlayCallback != null && this.conditionalOverlayCallback(overlay, transform);
	}

	// Token: 0x060054A2 RID: 21666 RVA: 0x001E42F2 File Offset: 0x001E24F2
	public StatusItem SetResolveStringCallback(Func<string, object, string> cb)
	{
		this.resolveStringCallback = cb;
		return this;
	}

	// Token: 0x060054A3 RID: 21667 RVA: 0x001E42FC File Offset: 0x001E24FC
	public void OnClick(object data)
	{
		if (this.statusItemClickCallback != null)
		{
			this.statusItemClickCallback(data);
		}
	}

	// Token: 0x060054A4 RID: 21668 RVA: 0x001E4314 File Offset: 0x001E2514
	public static StatusItem.StatusItemOverlays GetStatusItemOverlayBySimViewMode(HashedString mode)
	{
		StatusItem.StatusItemOverlays result;
		if (!StatusItem.overlayBitfieldMap.TryGetValue(mode, out result))
		{
			string str = "ViewMode ";
			HashedString hashedString = mode;
			global::Debug.LogWarning(str + hashedString.ToString() + " has no StatusItemOverlay value");
			result = StatusItem.StatusItemOverlays.None;
		}
		return result;
	}

	// Token: 0x04003766 RID: 14182
	public string tooltipText;

	// Token: 0x04003767 RID: 14183
	public string notificationText;

	// Token: 0x04003768 RID: 14184
	public string notificationTooltipText;

	// Token: 0x04003769 RID: 14185
	public string soundPath;

	// Token: 0x0400376A RID: 14186
	public string iconName;

	// Token: 0x0400376B RID: 14187
	public bool unique;

	// Token: 0x0400376C RID: 14188
	public TintedSprite sprite;

	// Token: 0x0400376D RID: 14189
	public bool shouldNotify;

	// Token: 0x0400376E RID: 14190
	public StatusItem.IconType iconType;

	// Token: 0x0400376F RID: 14191
	public NotificationType notificationType;

	// Token: 0x04003770 RID: 14192
	public Notification.ClickCallback notificationClickCallback;

	// Token: 0x04003771 RID: 14193
	public Func<string, object, string> resolveStringCallback;

	// Token: 0x04003772 RID: 14194
	public Func<string, object, string> resolveTooltipCallback;

	// Token: 0x04003773 RID: 14195
	public bool resolveStringCallback_shouldStillCallIfDataIsNull;

	// Token: 0x04003774 RID: 14196
	public bool resolveTooltipCallback_shouldStillCallIfDataIsNull;

	// Token: 0x04003775 RID: 14197
	public bool allowMultiples;

	// Token: 0x04003776 RID: 14198
	public Func<HashedString, object, bool> conditionalOverlayCallback;

	// Token: 0x04003777 RID: 14199
	public HashedString render_overlay;

	// Token: 0x04003778 RID: 14200
	public int status_overlays;

	// Token: 0x04003779 RID: 14201
	public Action<object> statusItemClickCallback;

	// Token: 0x0400377A RID: 14202
	private string composedPrefix;

	// Token: 0x0400377B RID: 14203
	private bool showShowWorldIcon = true;

	// Token: 0x0400377C RID: 14204
	public const int ALL_OVERLAYS = 129022;

	// Token: 0x0400377D RID: 14205
	private static Dictionary<HashedString, StatusItem.StatusItemOverlays> overlayBitfieldMap = new Dictionary<HashedString, StatusItem.StatusItemOverlays>
	{
		{
			OverlayModes.None.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Power.ID,
			StatusItem.StatusItemOverlays.PowerMap
		},
		{
			OverlayModes.Temperature.ID,
			StatusItem.StatusItemOverlays.Temperature
		},
		{
			OverlayModes.ThermalConductivity.ID,
			StatusItem.StatusItemOverlays.ThermalComfort
		},
		{
			OverlayModes.Light.ID,
			StatusItem.StatusItemOverlays.Light
		},
		{
			OverlayModes.LiquidConduits.ID,
			StatusItem.StatusItemOverlays.LiquidPlumbing
		},
		{
			OverlayModes.GasConduits.ID,
			StatusItem.StatusItemOverlays.GasPlumbing
		},
		{
			OverlayModes.SolidConveyor.ID,
			StatusItem.StatusItemOverlays.Conveyor
		},
		{
			OverlayModes.Decor.ID,
			StatusItem.StatusItemOverlays.Decor
		},
		{
			OverlayModes.Disease.ID,
			StatusItem.StatusItemOverlays.Pathogens
		},
		{
			OverlayModes.Crop.ID,
			StatusItem.StatusItemOverlays.Farming
		},
		{
			OverlayModes.Rooms.ID,
			StatusItem.StatusItemOverlays.Rooms
		},
		{
			OverlayModes.Suit.ID,
			StatusItem.StatusItemOverlays.Suits
		},
		{
			OverlayModes.Logic.ID,
			StatusItem.StatusItemOverlays.Logic
		},
		{
			OverlayModes.Oxygen.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.TileMode.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Radiation.ID,
			StatusItem.StatusItemOverlays.Radiation
		}
	};

	// Token: 0x02001B67 RID: 7015
	public enum IconType
	{
		// Token: 0x04007F95 RID: 32661
		Info,
		// Token: 0x04007F96 RID: 32662
		Exclamation,
		// Token: 0x04007F97 RID: 32663
		Custom
	}

	// Token: 0x02001B68 RID: 7016
	[Flags]
	public enum StatusItemOverlays
	{
		// Token: 0x04007F99 RID: 32665
		None = 2,
		// Token: 0x04007F9A RID: 32666
		PowerMap = 4,
		// Token: 0x04007F9B RID: 32667
		Temperature = 8,
		// Token: 0x04007F9C RID: 32668
		ThermalComfort = 16,
		// Token: 0x04007F9D RID: 32669
		Light = 32,
		// Token: 0x04007F9E RID: 32670
		LiquidPlumbing = 64,
		// Token: 0x04007F9F RID: 32671
		GasPlumbing = 128,
		// Token: 0x04007FA0 RID: 32672
		Decor = 256,
		// Token: 0x04007FA1 RID: 32673
		Pathogens = 512,
		// Token: 0x04007FA2 RID: 32674
		Farming = 1024,
		// Token: 0x04007FA3 RID: 32675
		Rooms = 4096,
		// Token: 0x04007FA4 RID: 32676
		Suits = 8192,
		// Token: 0x04007FA5 RID: 32677
		Logic = 16384,
		// Token: 0x04007FA6 RID: 32678
		Conveyor = 32768,
		// Token: 0x04007FA7 RID: 32679
		Radiation = 65536
	}
}
