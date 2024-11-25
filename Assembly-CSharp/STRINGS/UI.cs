using System;
using System.Collections.Generic;

namespace STRINGS
{
	// Token: 0x02000F10 RID: 3856
	public class UI
	{
		// Token: 0x06007742 RID: 30530 RVA: 0x002F4C0E File Offset: 0x002F2E0E
		public static string FormatAsBuildMenuTab(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06007743 RID: 30531 RVA: 0x002F4C20 File Offset: 0x002F2E20
		public static string FormatAsBuildMenuTab(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06007744 RID: 30532 RVA: 0x002F4C38 File Offset: 0x002F2E38
		public static string FormatAsBuildMenuTab(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06007745 RID: 30533 RVA: 0x002F4C50 File Offset: 0x002F2E50
		public static string FormatAsOverlay(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06007746 RID: 30534 RVA: 0x002F4C62 File Offset: 0x002F2E62
		public static string FormatAsOverlay(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x06007747 RID: 30535 RVA: 0x002F4C7A File Offset: 0x002F2E7A
		public static string FormatAsOverlay(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06007748 RID: 30536 RVA: 0x002F4C92 File Offset: 0x002F2E92
		public static string FormatAsManagementMenu(string text)
		{
			return "<b>" + text + "</b>";
		}

		// Token: 0x06007749 RID: 30537 RVA: 0x002F4CA4 File Offset: 0x002F2EA4
		public static string FormatAsManagementMenu(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x0600774A RID: 30538 RVA: 0x002F4CBC File Offset: 0x002F2EBC
		public static string FormatAsManagementMenu(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x0600774B RID: 30539 RVA: 0x002F4CD4 File Offset: 0x002F2ED4
		public static string FormatAsKeyWord(string text)
		{
			return UI.PRE_KEYWORD + text + UI.PST_KEYWORD;
		}

		// Token: 0x0600774C RID: 30540 RVA: 0x002F4CE6 File Offset: 0x002F2EE6
		public static string FormatAsHotkey(string text)
		{
			return "<b><color=#F44A4A>" + text + "</b></color>";
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x002F4CF8 File Offset: 0x002F2EF8
		public static string FormatAsHotKey(global::Action a)
		{
			return "{Hotkey/" + a.ToString() + "}";
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x002F4D16 File Offset: 0x002F2F16
		public static string FormatAsTool(string text, string hotkey)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotkey(hotkey);
		}

		// Token: 0x0600774F RID: 30543 RVA: 0x002F4D2E File Offset: 0x002F2F2E
		public static string FormatAsTool(string text, global::Action a)
		{
			return "<b>" + text + "</b> " + UI.FormatAsHotKey(a);
		}

		// Token: 0x06007750 RID: 30544 RVA: 0x002F4D46 File Offset: 0x002F2F46
		public static string FormatAsLink(string text, string linkID)
		{
			text = UI.StripLinkFormatting(text);
			linkID = CodexCache.FormatLinkID(linkID);
			return string.Concat(new string[]
			{
				"<link=\"",
				linkID,
				"\">",
				text,
				"</link>"
			});
		}

		// Token: 0x06007751 RID: 30545 RVA: 0x002F4D83 File Offset: 0x002F2F83
		public static string FormatAsPositiveModifier(string text)
		{
			return UI.PRE_POS_MODIFIER + text + UI.PST_POS_MODIFIER;
		}

		// Token: 0x06007752 RID: 30546 RVA: 0x002F4D95 File Offset: 0x002F2F95
		public static string FormatAsNegativeModifier(string text)
		{
			return UI.PRE_NEG_MODIFIER + text + UI.PST_NEG_MODIFIER;
		}

		// Token: 0x06007753 RID: 30547 RVA: 0x002F4DA7 File Offset: 0x002F2FA7
		public static string FormatAsPositiveRate(string text)
		{
			return UI.PRE_RATE_POSITIVE + text + UI.PST_RATE;
		}

		// Token: 0x06007754 RID: 30548 RVA: 0x002F4DB9 File Offset: 0x002F2FB9
		public static string FormatAsNegativeRate(string text)
		{
			return UI.PRE_RATE_NEGATIVE + text + UI.PST_RATE;
		}

		// Token: 0x06007755 RID: 30549 RVA: 0x002F4DCB File Offset: 0x002F2FCB
		public static string CLICK(UI.ClickType c)
		{
			return "(ClickType/" + c.ToString() + ")";
		}

		// Token: 0x06007756 RID: 30550 RVA: 0x002F4DE9 File Offset: 0x002F2FE9
		public static string FormatAsAutomationState(string text, UI.AutomationState state)
		{
			if (state == UI.AutomationState.Active)
			{
				return UI.PRE_AUTOMATION_ACTIVE + text + UI.PST_AUTOMATION;
			}
			return UI.PRE_AUTOMATION_STANDBY + text + UI.PST_AUTOMATION;
		}

		// Token: 0x06007757 RID: 30551 RVA: 0x002F4E0F File Offset: 0x002F300F
		public static string FormatAsCaps(string text)
		{
			return text.ToUpper();
		}

		// Token: 0x06007758 RID: 30552 RVA: 0x002F4E18 File Offset: 0x002F3018
		public static string ExtractLinkID(string text)
		{
			string text2 = text;
			int num = text2.IndexOf("<link=");
			if (num != -1)
			{
				int num2 = num + 7;
				int num3 = text2.IndexOf(">") - 1;
				text2 = text.Substring(num2, num3 - num2);
			}
			return text2;
		}

		// Token: 0x06007759 RID: 30553 RVA: 0x002F4E58 File Offset: 0x002F3058
		public static string StripLinkFormatting(string text)
		{
			string text2 = text;
			try
			{
				while (text2.Contains("<link="))
				{
					int num = text2.IndexOf("</link>");
					if (num > -1)
					{
						text2 = text2.Remove(num, 7);
					}
					else
					{
						Debug.LogWarningFormat("String has no closing link tag: {0}", new object[]
						{
							text
						});
					}
					int num2 = text2.IndexOf("<link=");
					if (num2 != -1)
					{
						int num3 = text2.IndexOf("\">", num2);
						if (num3 != -1)
						{
							text2 = text2.Remove(num2, num3 - num2 + 2);
						}
						else
						{
							text2 = text2.Remove(num2, "<link=".Length);
							Debug.LogWarningFormat("String has no open link closure: {0}", new object[]
							{
								text
							});
						}
					}
					else
					{
						Debug.LogWarningFormat("String has no open link tag: {0}", new object[]
						{
							text
						});
					}
				}
			}
			catch
			{
				Debug.Log("STRIP LINK FORMATTING FAILED ON: " + text);
				text2 = text;
			}
			return text2;
		}

		// Token: 0x04005867 RID: 22631
		public static string PRE_KEYWORD = "<style=\"KKeyword\">";

		// Token: 0x04005868 RID: 22632
		public static string PST_KEYWORD = "</style>";

		// Token: 0x04005869 RID: 22633
		public static string PRE_POS_MODIFIER = "<b>";

		// Token: 0x0400586A RID: 22634
		public static string PST_POS_MODIFIER = "</b>";

		// Token: 0x0400586B RID: 22635
		public static string PRE_NEG_MODIFIER = "<b>";

		// Token: 0x0400586C RID: 22636
		public static string PST_NEG_MODIFIER = "</b>";

		// Token: 0x0400586D RID: 22637
		public static string PRE_RATE_NEGATIVE = "<style=\"consumed\">";

		// Token: 0x0400586E RID: 22638
		public static string PRE_RATE_POSITIVE = "<style=\"produced\">";

		// Token: 0x0400586F RID: 22639
		public static string PST_RATE = "</style>";

		// Token: 0x04005870 RID: 22640
		public static string CODEXLINK = "BUILDCATEGORYREQUIREMENTCLASS";

		// Token: 0x04005871 RID: 22641
		public static string PRE_AUTOMATION_ACTIVE = "<b><style=\"logic_on\">";

		// Token: 0x04005872 RID: 22642
		public static string PRE_AUTOMATION_STANDBY = "<b><style=\"logic_off\">";

		// Token: 0x04005873 RID: 22643
		public static string PST_AUTOMATION = "</style></b>";

		// Token: 0x04005874 RID: 22644
		public static string YELLOW_PREFIX = "<color=#ffff00ff>";

		// Token: 0x04005875 RID: 22645
		public static string COLOR_SUFFIX = "</color>";

		// Token: 0x04005876 RID: 22646
		public static string HORIZONTAL_RULE = "------------------";

		// Token: 0x04005877 RID: 22647
		public static string HORIZONTAL_BR_RULE = "\n" + UI.HORIZONTAL_RULE + "\n";

		// Token: 0x04005878 RID: 22648
		public static LocString POS_INFINITY = "Infinity";

		// Token: 0x04005879 RID: 22649
		public static LocString NEG_INFINITY = "-Infinity";

		// Token: 0x0400587A RID: 22650
		public static LocString PROCEED_BUTTON = "PROCEED";

		// Token: 0x0400587B RID: 22651
		public static LocString COPY_BUILDING = "Copy";

		// Token: 0x0400587C RID: 22652
		public static LocString COPY_BUILDING_TOOLTIP = "Create new build orders using the most recent building selection as a template. {Hotkey}";

		// Token: 0x0400587D RID: 22653
		public static LocString NAME_WITH_UNITS = "{0} x {1}";

		// Token: 0x0400587E RID: 22654
		public static LocString NA = "N/A";

		// Token: 0x0400587F RID: 22655
		public static LocString POSITIVE_FORMAT = "+{0}";

		// Token: 0x04005880 RID: 22656
		public static LocString NEGATIVE_FORMAT = "-{0}";

		// Token: 0x04005881 RID: 22657
		public static LocString FILTER = "Filter";

		// Token: 0x04005882 RID: 22658
		public static LocString SPEED_SLOW = "SLOW";

		// Token: 0x04005883 RID: 22659
		public static LocString SPEED_MEDIUM = "MEDIUM";

		// Token: 0x04005884 RID: 22660
		public static LocString SPEED_FAST = "FAST";

		// Token: 0x04005885 RID: 22661
		public static LocString RED_ALERT = "RED ALERT";

		// Token: 0x04005886 RID: 22662
		public static LocString JOBS = "PRIORITIES";

		// Token: 0x04005887 RID: 22663
		public static LocString CONSUMABLES = "CONSUMABLES";

		// Token: 0x04005888 RID: 22664
		public static LocString VITALS = "VITALS";

		// Token: 0x04005889 RID: 22665
		public static LocString RESEARCH = "RESEARCH";

		// Token: 0x0400588A RID: 22666
		public static LocString ROLES = "JOB ASSIGNMENTS";

		// Token: 0x0400588B RID: 22667
		public static LocString RESEARCHPOINTS = "Research points";

		// Token: 0x0400588C RID: 22668
		public static LocString SCHEDULE = "SCHEDULE";

		// Token: 0x0400588D RID: 22669
		public static LocString REPORT = "REPORTS";

		// Token: 0x0400588E RID: 22670
		public static LocString SKILLS = "SKILLS";

		// Token: 0x0400588F RID: 22671
		public static LocString OVERLAYSTITLE = "OVERLAYS";

		// Token: 0x04005890 RID: 22672
		public static LocString ALERTS = "ALERTS";

		// Token: 0x04005891 RID: 22673
		public static LocString MESSAGES = "MESSAGES";

		// Token: 0x04005892 RID: 22674
		public static LocString ACTIONS = "ACTIONS";

		// Token: 0x04005893 RID: 22675
		public static LocString QUEUE = "Queue";

		// Token: 0x04005894 RID: 22676
		public static LocString BASECOUNT = "Base {0}";

		// Token: 0x04005895 RID: 22677
		public static LocString CHARACTERCONTAINER_SKILLS_TITLE = "ATTRIBUTES";

		// Token: 0x04005896 RID: 22678
		public static LocString CHARACTERCONTAINER_TRAITS_TITLE = "TRAITS";

		// Token: 0x04005897 RID: 22679
		public static LocString CHARACTERCONTAINER_TRAITS_TITLE_BIONIC = "BIONIC SYSTEMS";

		// Token: 0x04005898 RID: 22680
		public static LocString CHARACTERCONTAINER_APTITUDES_TITLE = "INTERESTS";

		// Token: 0x04005899 RID: 22681
		public static LocString CHARACTERCONTAINER_APTITUDES_TITLE_TOOLTIP = "A Duplicant's starting Attributes are determined by their Interests\n\nLearning Skills related to their Interests will give Duplicants a Morale boost";

		// Token: 0x0400589A RID: 22682
		public static LocString CHARACTERCONTAINER_EXPECTATIONS_TITLE = "ADDITIONAL INFORMATION";

		// Token: 0x0400589B RID: 22683
		public static LocString CHARACTERCONTAINER_SKILL_VALUE = " {0} {1}";

		// Token: 0x0400589C RID: 22684
		public static LocString CHARACTERCONTAINER_NEED = "{0}: {1}";

		// Token: 0x0400589D RID: 22685
		public static LocString CHARACTERCONTAINER_STRESSTRAIT = "Stress Reaction: {0}";

		// Token: 0x0400589E RID: 22686
		public static LocString CHARACTERCONTAINER_JOYTRAIT = "Overjoyed Response: {0}";

		// Token: 0x0400589F RID: 22687
		public static LocString CHARACTERCONTAINER_CONGENITALTRAIT = "Genetic Trait: {0}";

		// Token: 0x040058A0 RID: 22688
		public static LocString CHARACTERCONTAINER_NOARCHETYPESELECTED = "Random";

		// Token: 0x040058A1 RID: 22689
		public static LocString CHARACTERCONTAINER_ARCHETYPESELECT_TOOLTIP = "Change the type of Duplicant the reroll button will produce";

		// Token: 0x040058A2 RID: 22690
		public static LocString CAREPACKAGECONTAINER_INFORMATION_TITLE = "CARE PACKAGE";

		// Token: 0x040058A3 RID: 22691
		public static LocString CHARACTERCONTAINER_ALL_MODELS = "Any";

		// Token: 0x040058A4 RID: 22692
		public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED = "Increased <b>{0}</b>";

		// Token: 0x040058A5 RID: 22693
		public static LocString CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED = "Decreased <b>{0}</b>";

		// Token: 0x040058A6 RID: 22694
		public static LocString CHARACTERCONTAINER_FILTER_STANDARD = "Check box to allow standard Duplicants";

		// Token: 0x040058A7 RID: 22695
		public static LocString CHARACTERCONTAINER_FILTER_BIONIC = "Check box to allow Bionic Duplicants";

		// Token: 0x040058A8 RID: 22696
		public static LocString PRODUCTINFO_SELECTMATERIAL = "Select {0}:";

		// Token: 0x040058A9 RID: 22697
		public static LocString PRODUCTINFO_RESEARCHREQUIRED = "Research required...";

		// Token: 0x040058AA RID: 22698
		public static LocString PRODUCTINFO_REQUIRESRESEARCHDESC = "Requires {0} Research";

		// Token: 0x040058AB RID: 22699
		public static LocString PRODUCTINFO_APPLICABLERESOURCES = "Required resources:";

		// Token: 0x040058AC RID: 22700
		public static LocString PRODUCTINFO_MISSINGRESOURCES_TITLE = "Requires {0}: {1}";

		// Token: 0x040058AD RID: 22701
		public static LocString PRODUCTINFO_MISSINGRESOURCES_HOVER = "Missing resources";

		// Token: 0x040058AE RID: 22702
		public static LocString PRODUCTINFO_MISSINGRESOURCES_DESC = "{0} has yet to be discovered";

		// Token: 0x040058AF RID: 22703
		public static LocString PRODUCTINFO_UNIQUE_PER_WORLD = "Limit one per " + UI.CLUSTERMAP.PLANETOID_KEYWORD;

		// Token: 0x040058B0 RID: 22704
		public static LocString PRODUCTINFO_ROCKET_INTERIOR = "Rocket interior only";

		// Token: 0x040058B1 RID: 22705
		public static LocString PRODUCTINFO_ROCKET_NOT_INTERIOR = "Cannot build inside rocket";

		// Token: 0x040058B2 RID: 22706
		public static LocString BUILDTOOL_ROTATE = "Rotate this building";

		// Token: 0x040058B3 RID: 22707
		public static LocString BUILDTOOL_ROTATE_CURRENT_DEGREES = "Currently rotated {Degrees} degrees";

		// Token: 0x040058B4 RID: 22708
		public static LocString BUILDTOOL_ROTATE_CURRENT_LEFT = "Currently facing left";

		// Token: 0x040058B5 RID: 22709
		public static LocString BUILDTOOL_ROTATE_CURRENT_RIGHT = "Currently facing right";

		// Token: 0x040058B6 RID: 22710
		public static LocString BUILDTOOL_ROTATE_CURRENT_UP = "Currently facing up";

		// Token: 0x040058B7 RID: 22711
		public static LocString BUILDTOOL_ROTATE_CURRENT_DOWN = "Currently facing down";

		// Token: 0x040058B8 RID: 22712
		public static LocString BUILDTOOL_ROTATE_CURRENT_UPRIGHT = "Currently upright";

		// Token: 0x040058B9 RID: 22713
		public static LocString BUILDTOOL_ROTATE_CURRENT_ON_SIDE = "Currently on its side";

		// Token: 0x040058BA RID: 22714
		public static LocString BUILDTOOL_CANT_ROTATE = "This building cannot be rotated";

		// Token: 0x040058BB RID: 22715
		public static LocString EQUIPMENTTAB_OWNED = "Owned Items";

		// Token: 0x040058BC RID: 22716
		public static LocString EQUIPMENTTAB_HELD = "Held Items";

		// Token: 0x040058BD RID: 22717
		public static LocString EQUIPMENTTAB_ROOM = "Assigned Rooms";

		// Token: 0x040058BE RID: 22718
		public static LocString JOBSCREEN_PRIORITY = "Priority";

		// Token: 0x040058BF RID: 22719
		public static LocString JOBSCREEN_HIGH = "High";

		// Token: 0x040058C0 RID: 22720
		public static LocString JOBSCREEN_LOW = "Low";

		// Token: 0x040058C1 RID: 22721
		public static LocString JOBSCREEN_EVERYONE = "Everyone";

		// Token: 0x040058C2 RID: 22722
		public static LocString JOBSCREEN_DEFAULT = "New Duplicants";

		// Token: 0x040058C3 RID: 22723
		public static LocString BUILD_REQUIRES_SKILL = "Skill: {Skill}";

		// Token: 0x040058C4 RID: 22724
		public static LocString BUILD_REQUIRES_SKILL_TOOLTIP = "At least one Duplicant must have the {Skill} Skill to construct this building";

		// Token: 0x040058C5 RID: 22725
		public static LocString VITALSSCREEN_NAME = "Name";

		// Token: 0x040058C6 RID: 22726
		public static LocString VITALSSCREEN_STRESS = "Stress";

		// Token: 0x040058C7 RID: 22727
		public static LocString VITALSSCREEN_HEALTH = "Health";

		// Token: 0x040058C8 RID: 22728
		public static LocString VITALSSCREEN_SICKNESS = "Disease";

		// Token: 0x040058C9 RID: 22729
		public static LocString VITALSSCREEN_CALORIES = "Fullness";

		// Token: 0x040058CA RID: 22730
		public static LocString VITALSSCREEN_RATIONS = "Calories / Cycle";

		// Token: 0x040058CB RID: 22731
		public static LocString VITALSSCREEN_EATENTODAY = "Eaten Today";

		// Token: 0x040058CC RID: 22732
		public static LocString VITALSSCREEN_RATIONS_TOOLTIP = "Set how many calories this Duplicant may consume daily";

		// Token: 0x040058CD RID: 22733
		public static LocString VITALSSCREEN_EATENTODAY_TOOLTIP = "The amount of food this Duplicant has eaten this cycle";

		// Token: 0x040058CE RID: 22734
		public static LocString VITALSSCREEN_UNTIL_FULL = "Until Full";

		// Token: 0x040058CF RID: 22735
		public static LocString RESEARCHSCREEN_UNLOCKSTOOLTIP = "Unlocks: {0}";

		// Token: 0x040058D0 RID: 22736
		public static LocString RESEARCHSCREEN_FILTER = "Search Tech";

		// Token: 0x040058D1 RID: 22737
		public static LocString ATTRIBUTELEVEL = "Expertise: Level {0} {1}";

		// Token: 0x040058D2 RID: 22738
		public static LocString ATTRIBUTELEVEL_SHORT = "Level {0} {1}";

		// Token: 0x040058D3 RID: 22739
		public static LocString NEUTRONIUMMASS = "Immeasurable";

		// Token: 0x040058D4 RID: 22740
		public static LocString CALCULATING = "Calculating...";

		// Token: 0x040058D5 RID: 22741
		public static LocString FORMATDAY = "{0} cycles";

		// Token: 0x040058D6 RID: 22742
		public static LocString FORMATSECONDS = "{0}s";

		// Token: 0x040058D7 RID: 22743
		public static LocString DELIVERED = "Delivered: {0} {1}";

		// Token: 0x040058D8 RID: 22744
		public static LocString PICKEDUP = "Picked Up: {0} {1}";

		// Token: 0x040058D9 RID: 22745
		public static LocString COPIED_SETTINGS = "Settings Applied";

		// Token: 0x040058DA RID: 22746
		public static LocString WELCOMEMESSAGETITLE = "- ALERT -";

		// Token: 0x040058DB RID: 22747
		public static LocString WELCOMEMESSAGEBODY = "I've awoken at the target location, but colonization efforts have already hit a hitch. I was supposed to land on the planet's surface, but became trapped many miles underground instead.\n\nAlthough the conditions are not ideal, it's imperative that I establish a colony here and begin mounting efforts to escape.";

		// Token: 0x040058DC RID: 22748
		public static LocString WELCOMEMESSAGEBODY_SPACEDOUT = "The asteroid we call home has collided with an anomalous planet, decimating our colony. Rebuilding it is of the utmost importance.\n\nI've detected a new cluster of material-rich planetoids in nearby space. If I can guide the Duplicants through the perils of space travel, we could build a colony even bigger and better than before.";

		// Token: 0x040058DD RID: 22749
		public static LocString WELCOMEMESSAGEBODY_KF23 = "This asteroid is oddly tilted, as though a powerful external force once knocked it off its axis.\n\nI'll need to recalibrate my approach to colony-building in order to make the most of this unusual distribution of resources.";

		// Token: 0x040058DE RID: 22750
		public static LocString WELCOMEMESSAGEBODY_DLC2_CERES = "The ambient temperatures of this planet are inhospitably low.\n\nI've detected the ruins of a scientifically advanced settlement buried deep beneath our landing site.\n\nIf my Duplicants can survive the journey into this frosty planet's core, we could use this newfound technology to build a colony like no other.";

		// Token: 0x040058DF RID: 22751
		public static LocString WELCOMEMESSAGEBEGIN = "BEGIN";

		// Token: 0x040058E0 RID: 22752
		public static LocString VIEWDUPLICANTS = "Choose a Blueprint";

		// Token: 0x040058E1 RID: 22753
		public static LocString DUPLICANTPRINTING = "Duplicant Printing";

		// Token: 0x040058E2 RID: 22754
		public static LocString ASSIGNDUPLICANT = "Assign Duplicant";

		// Token: 0x040058E3 RID: 22755
		public static LocString CRAFT = "ADD TO QUEUE";

		// Token: 0x040058E4 RID: 22756
		public static LocString CLEAR_COMPLETED = "CLEAR COMPLETED ORDERS";

		// Token: 0x040058E5 RID: 22757
		public static LocString CRAFT_CONTINUOUS = "CONTINUOUS";

		// Token: 0x040058E6 RID: 22758
		public static LocString INCUBATE_CONTINUOUS_TOOLTIP = "When checked, this building will continuously incubate eggs of the selected type";

		// Token: 0x040058E7 RID: 22759
		public static LocString PLACEINRECEPTACLE = "Plant";

		// Token: 0x040058E8 RID: 22760
		public static LocString REMOVEFROMRECEPTACLE = "Uproot";

		// Token: 0x040058E9 RID: 22761
		public static LocString CANCELPLACEINRECEPTACLE = "Cancel";

		// Token: 0x040058EA RID: 22762
		public static LocString CANCELREMOVALFROMRECEPTACLE = "Cancel";

		// Token: 0x040058EB RID: 22763
		public static LocString CHANGEPERSECOND = "Change per second: {0}";

		// Token: 0x040058EC RID: 22764
		public static LocString CHANGEPERCYCLE = "Total change per cycle: {0}";

		// Token: 0x040058ED RID: 22765
		public static LocString MODIFIER_ITEM_TEMPLATE = "    • {0}: {1}";

		// Token: 0x040058EE RID: 22766
		public static LocString LISTENTRYSTRING = "     {0}\n";

		// Token: 0x040058EF RID: 22767
		public static LocString LISTENTRYSTRINGNOLINEBREAK = "     {0}";

		// Token: 0x0200213C RID: 8508
		public static class PLATFORMS
		{
			// Token: 0x040094CB RID: 38091
			public static LocString UNKNOWN = "Your game client";

			// Token: 0x040094CC RID: 38092
			public static LocString STEAM = "Steam";

			// Token: 0x040094CD RID: 38093
			public static LocString EPIC = "Epic Games Store";

			// Token: 0x040094CE RID: 38094
			public static LocString WEGAME = "Wegame";
		}

		// Token: 0x0200213D RID: 8509
		private enum KeywordType
		{
			// Token: 0x040094D0 RID: 38096
			Hotkey,
			// Token: 0x040094D1 RID: 38097
			BuildMenu,
			// Token: 0x040094D2 RID: 38098
			Attribute,
			// Token: 0x040094D3 RID: 38099
			Generic
		}

		// Token: 0x0200213E RID: 8510
		public enum ClickType
		{
			// Token: 0x040094D5 RID: 38101
			Click,
			// Token: 0x040094D6 RID: 38102
			Clicked,
			// Token: 0x040094D7 RID: 38103
			Clicking,
			// Token: 0x040094D8 RID: 38104
			Clickable,
			// Token: 0x040094D9 RID: 38105
			Clicks,
			// Token: 0x040094DA RID: 38106
			click,
			// Token: 0x040094DB RID: 38107
			clicked,
			// Token: 0x040094DC RID: 38108
			clicking,
			// Token: 0x040094DD RID: 38109
			clickable,
			// Token: 0x040094DE RID: 38110
			clicks,
			// Token: 0x040094DF RID: 38111
			CLICK,
			// Token: 0x040094E0 RID: 38112
			CLICKED,
			// Token: 0x040094E1 RID: 38113
			CLICKING,
			// Token: 0x040094E2 RID: 38114
			CLICKABLE,
			// Token: 0x040094E3 RID: 38115
			CLICKS
		}

		// Token: 0x0200213F RID: 8511
		public enum AutomationState
		{
			// Token: 0x040094E5 RID: 38117
			Active,
			// Token: 0x040094E6 RID: 38118
			Standby
		}

		// Token: 0x02002140 RID: 8512
		public class VANILLA
		{
			// Token: 0x040094E7 RID: 38119
			public static LocString NAME = "Base Game";

			// Token: 0x040094E8 RID: 38120
			public static LocString NAME_ITAL = "<i>" + UI.VANILLA.NAME + "</i>";
		}

		// Token: 0x02002141 RID: 8513
		public class DLC1
		{
			// Token: 0x040094E9 RID: 38121
			public static LocString NAME = "Spaced Out!";

			// Token: 0x040094EA RID: 38122
			public static LocString NAME_ITAL = "<i>" + UI.DLC1.NAME + "</i>";
		}

		// Token: 0x02002142 RID: 8514
		public class DLC2
		{
			// Token: 0x040094EB RID: 38123
			public static LocString NAME = "The Frosty Planet Pack";

			// Token: 0x040094EC RID: 38124
			public static LocString NAME_ITAL = "<i>" + UI.DLC2.NAME + "</i>";

			// Token: 0x040094ED RID: 38125
			public static LocString MIXING_TOOLTIP = "<b><i>The Frosty Planet Pack</i></b> features frozen biomes and elements useful in thermal regulation";
		}

		// Token: 0x02002143 RID: 8515
		public class DLC3
		{
			// Token: 0x040094EE RID: 38126
			public static LocString NAME = "SUPER-SECRET DLC3 NAME";

			// Token: 0x040094EF RID: 38127
			public static LocString NAME_ITAL = "<i>" + UI.DLC3.NAME + "</i>";

			// Token: 0x040094F0 RID: 38128
			public static LocString MIXING_TOOLTIP = "";
		}

		// Token: 0x02002144 RID: 8516
		public class DIAGNOSTICS_SCREEN
		{
			// Token: 0x040094F1 RID: 38129
			public static LocString TITLE = "Diagnostics";

			// Token: 0x040094F2 RID: 38130
			public static LocString DIAGNOSTIC = "Diagnostic";

			// Token: 0x040094F3 RID: 38131
			public static LocString TOTAL = "Total";

			// Token: 0x040094F4 RID: 38132
			public static LocString RESERVED = "Reserved";

			// Token: 0x040094F5 RID: 38133
			public static LocString STATUS = "Status";

			// Token: 0x040094F6 RID: 38134
			public static LocString SEARCH = "Search";

			// Token: 0x040094F7 RID: 38135
			public static LocString CRITERIA_HEADER_TOOLTIP = "Expand or collapse diagnostic criteria panel";

			// Token: 0x040094F8 RID: 38136
			public static LocString SEE_ALL = "+ See All ({0})";

			// Token: 0x040094F9 RID: 38137
			public static LocString CRITERIA_TOOLTIP = "Toggle the <b>{0}</b> diagnostics evaluation of the <b>{1}</b> criteria";

			// Token: 0x040094FA RID: 38138
			public static LocString CRITERIA_ENABLED_COUNT = "{0}/{1} criteria enabled";

			// Token: 0x020029B2 RID: 10674
			public class CLICK_TOGGLE_MESSAGE
			{
				// Token: 0x0400B54A RID: 46410
				public static LocString ALWAYS = UI.CLICK(UI.ClickType.Click) + " to pin this diagnostic to the sidebar - Current State: <b>Visible On Alert Only</b>";

				// Token: 0x0400B54B RID: 46411
				public static LocString ALERT_ONLY = UI.CLICK(UI.ClickType.Click) + " to subscribe to this diagnostic - Current State: <b>Never Visible</b>";

				// Token: 0x0400B54C RID: 46412
				public static LocString NEVER = UI.CLICK(UI.ClickType.Click) + " to mute this diagnostic on the sidebar - Current State: <b>Always Visible</b>";

				// Token: 0x0400B54D RID: 46413
				public static LocString TUTORIAL_DISABLED = UI.CLICK(UI.ClickType.Click) + " to enable this diagnostic -  Current State: <b>Temporarily disabled</b>";
			}
		}

		// Token: 0x02002145 RID: 8517
		public class WORLD_SELECTOR_SCREEN
		{
			// Token: 0x040094FB RID: 38139
			public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;
		}

		// Token: 0x02002146 RID: 8518
		public class COLONY_DIAGNOSTICS
		{
			// Token: 0x040094FC RID: 38140
			public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants on this planetoid";

			// Token: 0x040094FD RID: 38141
			public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants aboard this rocket";

			// Token: 0x040094FE RID: 38142
			public static LocString ROCKET = "rocket";

			// Token: 0x040094FF RID: 38143
			public static LocString NO_MINIONS_REQUESTED = "    • Crew must be requested to update this diagnostic";

			// Token: 0x04009500 RID: 38144
			public static LocString NO_DATA = "    • Not enough data for evaluation";

			// Token: 0x04009501 RID: 38145
			public static LocString NO_DATA_SHORT = "    • No data";

			// Token: 0x04009502 RID: 38146
			public static LocString MUTE_TUTORIAL = "Diagnostic can be muted in the <b><color=#E5B000>See All</color></b> panel";

			// Token: 0x04009503 RID: 38147
			public static LocString GENERIC_STATUS_NORMAL = "All values nominal";

			// Token: 0x04009504 RID: 38148
			public static LocString PLACEHOLDER_CRITERIA_NAME = "Placeholder Criteria Name";

			// Token: 0x04009505 RID: 38149
			public static LocString GENERIC_CRITERIA_PASS = "Criteria met";

			// Token: 0x04009506 RID: 38150
			public static LocString GENERIC_CRITERIA_FAIL = "Criteria not met";

			// Token: 0x020029B3 RID: 10675
			public class GENERIC_CRITERIA
			{
				// Token: 0x0400B54E RID: 46414
				public static LocString CHECKWORLDHASMINIONS = "Check world has Duplicants";
			}

			// Token: 0x020029B4 RID: 10676
			public class IDLEDIAGNOSTIC
			{
				// Token: 0x0400B54F RID: 46415
				public static LocString ALL_NAME = "Idleness";

				// Token: 0x0400B550 RID: 46416
				public static LocString TOOLTIP_NAME = "<b>Idleness</b>";

				// Token: 0x0400B551 RID: 46417
				public static LocString NORMAL = "    • All Duplicants currently have tasks";

				// Token: 0x0400B552 RID: 46418
				public static LocString IDLE = "    • One or more Duplicants are idle";

				// Token: 0x020035F2 RID: 13810
				public static class CRITERIA
				{
					// Token: 0x0400D92A RID: 55594
					public static LocString CHECKIDLE = "Check idle";

					// Token: 0x0400D92B RID: 55595
					public static LocString CHECKIDLESEVERE = "Use high severity idle warning";
				}
			}

			// Token: 0x020029B5 RID: 10677
			public class CHOREGROUPDIAGNOSTIC
			{
				// Token: 0x0400B553 RID: 46419
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

				// Token: 0x020035F3 RID: 13811
				public static class CRITERIA
				{
				}
			}

			// Token: 0x020029B6 RID: 10678
			public class ALLCHORESDIAGNOSTIC
			{
				// Token: 0x0400B554 RID: 46420
				public static LocString ALL_NAME = "Errands";

				// Token: 0x0400B555 RID: 46421
				public static LocString TOOLTIP_NAME = "<b>Errands</b>";

				// Token: 0x0400B556 RID: 46422
				public static LocString NORMAL = "    • {0} errands pending or in progress";

				// Token: 0x020035F4 RID: 13812
				public static class CRITERIA
				{
				}
			}

			// Token: 0x020029B7 RID: 10679
			public class WORKTIMEDIAGNOSTIC
			{
				// Token: 0x0400B557 RID: 46423
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.ALLCHORESDIAGNOSTIC.ALL_NAME;

				// Token: 0x020035F5 RID: 13813
				public static class CRITERIA
				{
				}
			}

			// Token: 0x020029B8 RID: 10680
			public class ALLWORKTIMEDIAGNOSTIC
			{
				// Token: 0x0400B558 RID: 46424
				public static LocString ALL_NAME = "Work Time";

				// Token: 0x0400B559 RID: 46425
				public static LocString TOOLTIP_NAME = "<b>Work Time</b>";

				// Token: 0x0400B55A RID: 46426
				public static LocString NORMAL = "    • {0} of Duplicant time spent working";

				// Token: 0x020035F6 RID: 13814
				public static class CRITERIA
				{
				}
			}

			// Token: 0x020029B9 RID: 10681
			public class TRAVEL_TIME
			{
				// Token: 0x0400B55B RID: 46427
				public static LocString ALL_NAME = "Travel Time";

				// Token: 0x0400B55C RID: 46428
				public static LocString TOOLTIP_NAME = "<b>Travel Time</b>";

				// Token: 0x0400B55D RID: 46429
				public static LocString NORMAL = "    • {0} of Duplicant time spent traveling between errands";

				// Token: 0x020035F7 RID: 13815
				public static class CRITERIA
				{
				}
			}

			// Token: 0x020029BA RID: 10682
			public class TRAPPEDDUPLICANTDIAGNOSTIC
			{
				// Token: 0x0400B55E RID: 46430
				public static LocString ALL_NAME = "Trapped";

				// Token: 0x0400B55F RID: 46431
				public static LocString TOOLTIP_NAME = "<b>Trapped</b>";

				// Token: 0x0400B560 RID: 46432
				public static LocString NORMAL = "    • No Duplicants are trapped";

				// Token: 0x0400B561 RID: 46433
				public static LocString STUCK = "    • One or more Duplicants are trapped";

				// Token: 0x020035F8 RID: 13816
				public static class CRITERIA
				{
					// Token: 0x0400D92C RID: 55596
					public static LocString CHECKTRAPPED = "Check Trapped";
				}
			}

			// Token: 0x020029BB RID: 10683
			public class FLOODEDDIAGNOSTIC
			{
				// Token: 0x0400B562 RID: 46434
				public static LocString ALL_NAME = "Flooded";

				// Token: 0x0400B563 RID: 46435
				public static LocString TOOLTIP_NAME = "<b>Flooded</b>";

				// Token: 0x0400B564 RID: 46436
				public static LocString NORMAL = "    • No buildings are flooded";

				// Token: 0x0400B565 RID: 46437
				public static LocString BUILDING_FLOODED = "    • One or more buildings are flooded";

				// Token: 0x020035F9 RID: 13817
				public static class CRITERIA
				{
					// Token: 0x0400D92D RID: 55597
					public static LocString CHECKFLOODED = "Check Flooded";
				}
			}

			// Token: 0x020029BC RID: 10684
			public class BREATHABILITYDIAGNOSTIC
			{
				// Token: 0x0400B566 RID: 46438
				public static LocString ALL_NAME = "Breathability";

				// Token: 0x0400B567 RID: 46439
				public static LocString TOOLTIP_NAME = "<b>Breathability</b>";

				// Token: 0x0400B568 RID: 46440
				public static LocString NORMAL = "    • Oxygen levels are satisfactory";

				// Token: 0x0400B569 RID: 46441
				public static LocString POOR = "    • Oxygen is becoming scarce or low pressure";

				// Token: 0x0400B56A RID: 46442
				public static LocString SUFFOCATING = "    • One or more Duplicants are suffocating";

				// Token: 0x020035FA RID: 13818
				public static class CRITERIA
				{
					// Token: 0x0400D92E RID: 55598
					public static LocString CHECKSUFFOCATION = "Check suffocation";

					// Token: 0x0400D92F RID: 55599
					public static LocString CHECKLOWBREATHABILITY = "Check low breathability";
				}
			}

			// Token: 0x020029BD RID: 10685
			public class STRESSDIAGNOSTIC
			{
				// Token: 0x0400B56B RID: 46443
				public static LocString ALL_NAME = "Max Stress";

				// Token: 0x0400B56C RID: 46444
				public static LocString TOOLTIP_NAME = "<b>Max Stress</b>";

				// Token: 0x0400B56D RID: 46445
				public static LocString HIGH_STRESS = "    • One or more Duplicants is suffering high stress";

				// Token: 0x0400B56E RID: 46446
				public static LocString NORMAL = "    • Duplicants have acceptable stress levels";

				// Token: 0x020035FB RID: 13819
				public static class CRITERIA
				{
					// Token: 0x0400D930 RID: 55600
					public static LocString CHECKSTRESSED = "Check stressed";
				}
			}

			// Token: 0x020029BE RID: 10686
			public class DECORDIAGNOSTIC
			{
				// Token: 0x0400B56F RID: 46447
				public static LocString ALL_NAME = "Decor";

				// Token: 0x0400B570 RID: 46448
				public static LocString TOOLTIP_NAME = "<b>Decor</b>";

				// Token: 0x0400B571 RID: 46449
				public static LocString LOW = "    • Decor levels are low";

				// Token: 0x0400B572 RID: 46450
				public static LocString NORMAL = "    • Decor levels are satisfactory";

				// Token: 0x020035FC RID: 13820
				public static class CRITERIA
				{
					// Token: 0x0400D931 RID: 55601
					public static LocString CHECKDECOR = "Check decor";
				}
			}

			// Token: 0x020029BF RID: 10687
			public class TOILETDIAGNOSTIC
			{
				// Token: 0x0400B573 RID: 46451
				public static LocString ALL_NAME = "Toilets";

				// Token: 0x0400B574 RID: 46452
				public static LocString TOOLTIP_NAME = "<b>Toilets</b>";

				// Token: 0x0400B575 RID: 46453
				public static LocString NO_TOILETS = "    • Colony has no toilets";

				// Token: 0x0400B576 RID: 46454
				public static LocString NO_WORKING_TOILETS = "    • Colony has no working toilets";

				// Token: 0x0400B577 RID: 46455
				public static LocString TOILET_URGENT = "    • Duplicants urgently need to use a toilet";

				// Token: 0x0400B578 RID: 46456
				public static LocString FEW_TOILETS = "    • Toilet-to-Duplicant ratio is low";

				// Token: 0x0400B579 RID: 46457
				public static LocString INOPERATIONAL = "    • One or more toilets are out of order";

				// Token: 0x0400B57A RID: 46458
				public static LocString NORMAL = "    • Colony has adequate working toilets";

				// Token: 0x0400B57B RID: 46459
				public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants with a bladder on this planetoid";

				// Token: 0x0400B57C RID: 46460
				public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants with a bladder aboard this rocket";

				// Token: 0x020035FD RID: 13821
				public static class CRITERIA
				{
					// Token: 0x0400D932 RID: 55602
					public static LocString CHECKHASANYTOILETS = "Check has any toilets";

					// Token: 0x0400D933 RID: 55603
					public static LocString CHECKENOUGHTOILETS = "Check enough toilets";

					// Token: 0x0400D934 RID: 55604
					public static LocString CHECKBLADDERS = "Check Duplicants really need to use the toilet";
				}
			}

			// Token: 0x020029C0 RID: 10688
			public class BEDDIAGNOSTIC
			{
				// Token: 0x0400B57D RID: 46461
				public static LocString ALL_NAME = "Beds";

				// Token: 0x0400B57E RID: 46462
				public static LocString TOOLTIP_NAME = "<b>Beds</b>";

				// Token: 0x0400B57F RID: 46463
				public static LocString NORMAL = "    • Colony has adequate bedding";

				// Token: 0x0400B580 RID: 46464
				public static LocString NOT_ENOUGH_BEDS = "    • One or more Duplicants are missing a bed";

				// Token: 0x0400B581 RID: 46465
				public static LocString MISSING_ASSIGNMENT = "    • One or more Duplicants don't have an assigned bed";

				// Token: 0x0400B582 RID: 46466
				public static LocString CANT_REACH = "    • One or more Duplicants can't reach their bed";

				// Token: 0x0400B583 RID: 46467
				public static LocString NO_MINIONS_PLANETOID = "    • There are no Duplicants on this planetoid who need sleep";

				// Token: 0x0400B584 RID: 46468
				public static LocString NO_MINIONS_ROCKET = "    • There are no Duplicants aboard this rocket who need sleep";

				// Token: 0x020035FE RID: 13822
				public static class CRITERIA
				{
					// Token: 0x0400D935 RID: 55605
					public static LocString CHECKENOUGHBEDS = "Check enough beds";

					// Token: 0x0400D936 RID: 55606
					public static LocString CHECKREACHABILITY = "Check beds are reachable";
				}
			}

			// Token: 0x020029C1 RID: 10689
			public class FOODDIAGNOSTIC
			{
				// Token: 0x0400B585 RID: 46469
				public static LocString ALL_NAME = "Food";

				// Token: 0x0400B586 RID: 46470
				public static LocString TOOLTIP_NAME = "<b>Food</b>";

				// Token: 0x0400B587 RID: 46471
				public static LocString NORMAL = "    • Food supply is currently adequate";

				// Token: 0x0400B588 RID: 46472
				public static LocString LOW_CALORIES = "    • Food-to-Duplicant ratio is low";

				// Token: 0x0400B589 RID: 46473
				public static LocString HUNGRY = "    • One or more Duplicants are very hungry";

				// Token: 0x0400B58A RID: 46474
				public static LocString NO_FOOD = "    • Duplicants have no food";

				// Token: 0x020035FF RID: 13823
				public class CRITERIA_HAS_FOOD
				{
					// Token: 0x0400D937 RID: 55607
					public static LocString PASS = "    • Duplicants have food";

					// Token: 0x0400D938 RID: 55608
					public static LocString FAIL = "    • Duplicants have no food";
				}

				// Token: 0x02003600 RID: 13824
				public static class CRITERIA
				{
					// Token: 0x0400D939 RID: 55609
					public static LocString CHECKENOUGHFOOD = "Check enough food";

					// Token: 0x0400D93A RID: 55610
					public static LocString CHECKSTARVATION = "Check starvation";
				}
			}

			// Token: 0x020029C2 RID: 10690
			public class FARMDIAGNOSTIC
			{
				// Token: 0x0400B58B RID: 46475
				public static LocString ALL_NAME = "Crops";

				// Token: 0x0400B58C RID: 46476
				public static LocString TOOLTIP_NAME = "<b>Crops</b>";

				// Token: 0x0400B58D RID: 46477
				public static LocString NORMAL = "    • Crops are being grown in sufficient quantity";

				// Token: 0x0400B58E RID: 46478
				public static LocString NONE = "    • No farm plots";

				// Token: 0x0400B58F RID: 46479
				public static LocString NONE_PLANTED = "    • No crops planted";

				// Token: 0x0400B590 RID: 46480
				public static LocString WILTING = "    • One or more crops are wilting";

				// Token: 0x0400B591 RID: 46481
				public static LocString INOPERATIONAL = "    • One or more farm plots are inoperable";

				// Token: 0x02003601 RID: 13825
				public static class CRITERIA
				{
					// Token: 0x0400D93B RID: 55611
					public static LocString CHECKHASFARMS = "Check colony has farms";

					// Token: 0x0400D93C RID: 55612
					public static LocString CHECKPLANTED = "Check farms are planted";

					// Token: 0x0400D93D RID: 55613
					public static LocString CHECKWILTING = "Check crops wilting";

					// Token: 0x0400D93E RID: 55614
					public static LocString CHECKOPERATIONAL = "Check farm plots operational";
				}
			}

			// Token: 0x020029C3 RID: 10691
			public class POWERUSEDIAGNOSTIC
			{
				// Token: 0x0400B592 RID: 46482
				public static LocString ALL_NAME = "Power use";

				// Token: 0x0400B593 RID: 46483
				public static LocString TOOLTIP_NAME = "<b>Power use</b>";

				// Token: 0x0400B594 RID: 46484
				public static LocString NORMAL = "    • Power supply is satisfactory";

				// Token: 0x0400B595 RID: 46485
				public static LocString OVERLOADED = "    • One or more power grids are damaged";

				// Token: 0x0400B596 RID: 46486
				public static LocString SIGNIFICANT_POWER_CHANGE_DETECTED = "Significant power use change detected. (Average:{0}, Current:{1})";

				// Token: 0x0400B597 RID: 46487
				public static LocString CIRCUIT_OVER_CAPACITY = "Circuit overloaded {0}/{1}";

				// Token: 0x02003602 RID: 13826
				public static class CRITERIA
				{
					// Token: 0x0400D93F RID: 55615
					public static LocString CHECKOVERWATTAGE = "Check circuit overloaded";

					// Token: 0x0400D940 RID: 55616
					public static LocString CHECKPOWERUSECHANGE = "Check power use change";
				}
			}

			// Token: 0x020029C4 RID: 10692
			public class HEATDIAGNOSTIC
			{
				// Token: 0x0400B598 RID: 46488
				public static LocString ALL_NAME = UI.COLONY_DIAGNOSTICS.BATTERYDIAGNOSTIC.ALL_NAME;

				// Token: 0x02003603 RID: 13827
				public static class CRITERIA
				{
					// Token: 0x0400D941 RID: 55617
					public static LocString CHECKHEAT = "Check heat";
				}
			}

			// Token: 0x020029C5 RID: 10693
			public class BATTERYDIAGNOSTIC
			{
				// Token: 0x0400B599 RID: 46489
				public static LocString ALL_NAME = "Battery";

				// Token: 0x0400B59A RID: 46490
				public static LocString TOOLTIP_NAME = "<b>Battery</b>";

				// Token: 0x0400B59B RID: 46491
				public static LocString NORMAL = "    • All batteries functional";

				// Token: 0x0400B59C RID: 46492
				public static LocString NONE = "    • No batteries are connected to a power grid";

				// Token: 0x0400B59D RID: 46493
				public static LocString DEAD_BATTERY = "    • One or more batteries have died";

				// Token: 0x0400B59E RID: 46494
				public static LocString LIMITED_CAPACITY = "    • Low battery capacity relative to power use";

				// Token: 0x02003604 RID: 13828
				public class CRITERIA_CHECK_CAPACITY
				{
					// Token: 0x0400D942 RID: 55618
					public static LocString PASS = "";

					// Token: 0x0400D943 RID: 55619
					public static LocString FAIL = "";
				}

				// Token: 0x02003605 RID: 13829
				public static class CRITERIA
				{
					// Token: 0x0400D944 RID: 55620
					public static LocString CHECKCAPACITY = "Check capacity";

					// Token: 0x0400D945 RID: 55621
					public static LocString CHECKDEAD = "Check dead";
				}
			}

			// Token: 0x020029C6 RID: 10694
			public class RADIATIONDIAGNOSTIC
			{
				// Token: 0x0400B59F RID: 46495
				public static LocString ALL_NAME = "Radiation";

				// Token: 0x0400B5A0 RID: 46496
				public static LocString TOOLTIP_NAME = "<b>Radiation</b>";

				// Token: 0x0400B5A1 RID: 46497
				public static LocString NORMAL = "    • No Radiation concerns";

				// Token: 0x0400B5A2 RID: 46498
				public static LocString AVERAGE_RADS = "Avg. {0}";

				// Token: 0x02003606 RID: 13830
				public class CRITERIA_RADIATION_SICKNESS
				{
					// Token: 0x0400D946 RID: 55622
					public static LocString PASS = "Healthy";

					// Token: 0x0400D947 RID: 55623
					public static LocString FAIL = "Sick";
				}

				// Token: 0x02003607 RID: 13831
				public class CRITERIA_RADIATION_EXPOSURE
				{
					// Token: 0x0400D948 RID: 55624
					public static LocString PASS = "Safe exposure levels";

					// Token: 0x0400D949 RID: 55625
					public static LocString FAIL_CONCERN = "Exposure levels are above safe limits for one or more Duplicants";

					// Token: 0x0400D94A RID: 55626
					public static LocString FAIL_WARNING = "One or more Duplicants are being exposed to extreme levels of radiation";
				}

				// Token: 0x02003608 RID: 13832
				public static class CRITERIA
				{
					// Token: 0x0400D94B RID: 55627
					public static LocString CHECKSICK = "Check sick";

					// Token: 0x0400D94C RID: 55628
					public static LocString CHECKEXPOSED = "Check exposed";
				}
			}

			// Token: 0x020029C7 RID: 10695
			public class METEORDIAGNOSTIC
			{
				// Token: 0x0400B5A3 RID: 46499
				public static LocString ALL_NAME = "Meteor Showers";

				// Token: 0x0400B5A4 RID: 46500
				public static LocString TOOLTIP_NAME = "<b>Meteor Showers</b>";

				// Token: 0x0400B5A5 RID: 46501
				public static LocString NORMAL = "    • No meteor showers in progress";

				// Token: 0x0400B5A6 RID: 46502
				public static LocString SHOWER_UNDERWAY = "    • Meteor bombardment underway! {0} remaining";

				// Token: 0x02003609 RID: 13833
				public static class CRITERIA
				{
					// Token: 0x0400D94D RID: 55629
					public static LocString CHECKUNDERWAY = "Check meteor bombardment";
				}
			}

			// Token: 0x020029C8 RID: 10696
			public class ENTOMBEDDIAGNOSTIC
			{
				// Token: 0x0400B5A7 RID: 46503
				public static LocString ALL_NAME = "Entombed";

				// Token: 0x0400B5A8 RID: 46504
				public static LocString TOOLTIP_NAME = "<b>Entombed</b>";

				// Token: 0x0400B5A9 RID: 46505
				public static LocString NORMAL = "    • No buildings are entombed";

				// Token: 0x0400B5AA RID: 46506
				public static LocString BUILDING_ENTOMBED = "    • One or more buildings are entombed";

				// Token: 0x0200360A RID: 13834
				public static class CRITERIA
				{
					// Token: 0x0400D94E RID: 55630
					public static LocString CHECKENTOMBED = "Check entombed";
				}
			}

			// Token: 0x020029C9 RID: 10697
			public class ROCKETFUELDIAGNOSTIC
			{
				// Token: 0x0400B5AB RID: 46507
				public static LocString ALL_NAME = "Rocket Fuel";

				// Token: 0x0400B5AC RID: 46508
				public static LocString TOOLTIP_NAME = "<b>Rocket Fuel</b>";

				// Token: 0x0400B5AD RID: 46509
				public static LocString NORMAL = "    • This rocket has sufficient fuel";

				// Token: 0x0400B5AE RID: 46510
				public static LocString WARNING = "    • This rocket has no fuel";

				// Token: 0x0200360B RID: 13835
				public static class CRITERIA
				{
				}
			}

			// Token: 0x020029CA RID: 10698
			public class ROCKETOXIDIZERDIAGNOSTIC
			{
				// Token: 0x0400B5AF RID: 46511
				public static LocString ALL_NAME = "Rocket Oxidizer";

				// Token: 0x0400B5B0 RID: 46512
				public static LocString TOOLTIP_NAME = "<b>Rocket Oxidizer</b>";

				// Token: 0x0400B5B1 RID: 46513
				public static LocString NORMAL = "    • This rocket has sufficient oxidizer";

				// Token: 0x0400B5B2 RID: 46514
				public static LocString WARNING = "    • This rocket has insufficient oxidizer";

				// Token: 0x0200360C RID: 13836
				public static class CRITERIA
				{
				}
			}

			// Token: 0x020029CB RID: 10699
			public class REACTORDIAGNOSTIC
			{
				// Token: 0x0400B5B3 RID: 46515
				public static LocString ALL_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;

				// Token: 0x0400B5B4 RID: 46516
				public static LocString TOOLTIP_NAME = BUILDINGS.PREFABS.NUCLEARREACTOR.NAME;

				// Token: 0x0400B5B5 RID: 46517
				public static LocString NORMAL = "    • Safe";

				// Token: 0x0400B5B6 RID: 46518
				public static LocString CRITERIA_TEMPERATURE_WARNING = "    • Temperature dangerously high";

				// Token: 0x0400B5B7 RID: 46519
				public static LocString CRITERIA_COOLANT_WARNING = "    • Coolant tank low";

				// Token: 0x0200360D RID: 13837
				public static class CRITERIA
				{
					// Token: 0x0400D94F RID: 55631
					public static LocString CHECKTEMPERATURE = "Check temperature";

					// Token: 0x0400D950 RID: 55632
					public static LocString CHECKCOOLANT = "Check coolant";
				}
			}

			// Token: 0x020029CC RID: 10700
			public class FLOATINGROCKETDIAGNOSTIC
			{
				// Token: 0x0400B5B8 RID: 46520
				public static LocString ALL_NAME = "Flight Status";

				// Token: 0x0400B5B9 RID: 46521
				public static LocString TOOLTIP_NAME = "<b>Flight Status</b>";

				// Token: 0x0400B5BA RID: 46522
				public static LocString NORMAL_FLIGHT = "    • This rocket is in flight towards its destination";

				// Token: 0x0400B5BB RID: 46523
				public static LocString NORMAL_UTILITY = "    • This rocket is performing a task at its destination";

				// Token: 0x0400B5BC RID: 46524
				public static LocString NORMAL_LANDED = "    • This rocket is currently landed on a " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;

				// Token: 0x0400B5BD RID: 46525
				public static LocString WARNING_NO_DESTINATION = "    • This rocket is suspended in space with no set destination";

				// Token: 0x0400B5BE RID: 46526
				public static LocString WARNING_NO_SPEED = "    • This rocket's flight has been halted";

				// Token: 0x0200360E RID: 13838
				public static class CRITERIA
				{
				}
			}

			// Token: 0x020029CD RID: 10701
			public class ROCKETINORBITDIAGNOSTIC
			{
				// Token: 0x0400B5BF RID: 46527
				public static LocString ALL_NAME = "Rockets in Orbit";

				// Token: 0x0400B5C0 RID: 46528
				public static LocString TOOLTIP_NAME = "<b>Rockets in Orbit</b>";

				// Token: 0x0400B5C1 RID: 46529
				public static LocString NORMAL_ONE_IN_ORBIT = "    • {0} is in orbit waiting to land";

				// Token: 0x0400B5C2 RID: 46530
				public static LocString NORMAL_IN_ORBIT = "    • There are {0} rockets in orbit waiting to land";

				// Token: 0x0400B5C3 RID: 46531
				public static LocString WARNING_ONE_ROCKETS_STRANDED = "    • No " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " present. {0} stranded";

				// Token: 0x0400B5C4 RID: 46532
				public static LocString WARNING_ROCKETS_STRANDED = "    • No " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " present. {0} rockets stranded";

				// Token: 0x0400B5C5 RID: 46533
				public static LocString NORMAL_NO_ROCKETS = "    • No rockets waiting to land";

				// Token: 0x0200360F RID: 13839
				public static class CRITERIA
				{
					// Token: 0x0400D951 RID: 55633
					public static LocString CHECKORBIT = "Check Orbiting Rockets";
				}
			}

			// Token: 0x020029CE RID: 10702
			public class BIONICBATTERYDIAGNOSTIC
			{
				// Token: 0x0400B5C6 RID: 46534
				public static LocString ALL_NAME = "Bionic Power";

				// Token: 0x0400B5C7 RID: 46535
				public static LocString TOOLTIP_NAME = "<b>Power Banks</b>";

				// Token: 0x0400B5C8 RID: 46536
				public static LocString NORMAL = "    • Power Bank supply is currently adequate";

				// Token: 0x0400B5C9 RID: 46537
				public static LocString LOW_CALORIES = "    • Power-to-Duplicant ratio is low";

				// Token: 0x0400B5CA RID: 46538
				public static LocString HUNGRY = "    • One or more Duplicants in desparate need of power banks";

				// Token: 0x0400B5CB RID: 46539
				public static LocString NO_FOOD = "    • Duplicants have no Power Banks";

				// Token: 0x02003610 RID: 13840
				public class CRITERIA_BATTERIES
				{
					// Token: 0x0400D952 RID: 55634
					public static LocString PASS = "    • Bionics have batteries";

					// Token: 0x0400D953 RID: 55635
					public static LocString FAIL = "    • Bionics have no batteries";
				}

				// Token: 0x02003611 RID: 13841
				public static class CRITERIA
				{
					// Token: 0x0400D954 RID: 55636
					public static LocString CHECKENOUGHBATTERIES = "Check enough batteries";

					// Token: 0x0400D955 RID: 55637
					public static LocString CHECKPOWERLEVEL = "Check critical power level";
				}
			}
		}

		// Token: 0x02002147 RID: 8519
		public class TRACKERS
		{
			// Token: 0x04009507 RID: 38151
			public static LocString BREATHABILITY = "Breathability";

			// Token: 0x04009508 RID: 38152
			public static LocString FOOD = "Food";

			// Token: 0x04009509 RID: 38153
			public static LocString STRESS = "Max Stress";

			// Token: 0x0400950A RID: 38154
			public static LocString IDLE = "Idle Duplicants";
		}

		// Token: 0x02002148 RID: 8520
		public class CONTROLS
		{
			// Token: 0x0400950B RID: 38155
			public static LocString PRESS = "Press";

			// Token: 0x0400950C RID: 38156
			public static LocString PRESSLOWER = "press";

			// Token: 0x0400950D RID: 38157
			public static LocString PRESSUPPER = "PRESS";

			// Token: 0x0400950E RID: 38158
			public static LocString PRESSING = "Pressing";

			// Token: 0x0400950F RID: 38159
			public static LocString PRESSINGLOWER = "pressing";

			// Token: 0x04009510 RID: 38160
			public static LocString PRESSINGUPPER = "PRESSING";

			// Token: 0x04009511 RID: 38161
			public static LocString PRESSED = "Pressed";

			// Token: 0x04009512 RID: 38162
			public static LocString PRESSEDLOWER = "pressed";

			// Token: 0x04009513 RID: 38163
			public static LocString PRESSEDUPPER = "PRESSED";

			// Token: 0x04009514 RID: 38164
			public static LocString PRESSES = "Presses";

			// Token: 0x04009515 RID: 38165
			public static LocString PRESSESLOWER = "presses";

			// Token: 0x04009516 RID: 38166
			public static LocString PRESSESUPPER = "PRESSES";

			// Token: 0x04009517 RID: 38167
			public static LocString PRESSABLE = "Pressable";

			// Token: 0x04009518 RID: 38168
			public static LocString PRESSABLELOWER = "pressable";

			// Token: 0x04009519 RID: 38169
			public static LocString PRESSABLEUPPER = "PRESSABLE";

			// Token: 0x0400951A RID: 38170
			public static LocString CLICK = "Click";

			// Token: 0x0400951B RID: 38171
			public static LocString CLICKLOWER = "click";

			// Token: 0x0400951C RID: 38172
			public static LocString CLICKUPPER = "CLICK";

			// Token: 0x0400951D RID: 38173
			public static LocString CLICKING = "Clicking";

			// Token: 0x0400951E RID: 38174
			public static LocString CLICKINGLOWER = "clicking";

			// Token: 0x0400951F RID: 38175
			public static LocString CLICKINGUPPER = "CLICKING";

			// Token: 0x04009520 RID: 38176
			public static LocString CLICKED = "Clicked";

			// Token: 0x04009521 RID: 38177
			public static LocString CLICKEDLOWER = "clicked";

			// Token: 0x04009522 RID: 38178
			public static LocString CLICKEDUPPER = "CLICKED";

			// Token: 0x04009523 RID: 38179
			public static LocString CLICKS = "Clicks";

			// Token: 0x04009524 RID: 38180
			public static LocString CLICKSLOWER = "clicks";

			// Token: 0x04009525 RID: 38181
			public static LocString CLICKSUPPER = "CLICKS";

			// Token: 0x04009526 RID: 38182
			public static LocString CLICKABLE = "Clickable";

			// Token: 0x04009527 RID: 38183
			public static LocString CLICKABLELOWER = "clickable";

			// Token: 0x04009528 RID: 38184
			public static LocString CLICKABLEUPPER = "CLICKABLE";
		}

		// Token: 0x02002149 RID: 8521
		public class MATH_PICTURES
		{
			// Token: 0x020029CF RID: 10703
			public class AXIS_LABELS
			{
				// Token: 0x0400B5CC RID: 46540
				public static LocString CYCLES = "Cycles";
			}
		}

		// Token: 0x0200214A RID: 8522
		public class SPACEDESTINATIONS
		{
			// Token: 0x020029D0 RID: 10704
			public class WORMHOLE
			{
				// Token: 0x0400B5CD RID: 46541
				public static LocString NAME = "Temporal Tear";

				// Token: 0x0400B5CE RID: 46542
				public static LocString DESCRIPTION = "The source of our misfortune, though it may also be our shot at freedom. Traces of Neutronium are detectable in my readings.";
			}

			// Token: 0x020029D1 RID: 10705
			public class RESEARCHDESTINATION
			{
				// Token: 0x0400B5CF RID: 46543
				public static LocString NAME = "Alluring Anomaly";

				// Token: 0x0400B5D0 RID: 46544
				public static LocString DESCRIPTION = "Our researchers would have a field day with this if they could only get close enough.";
			}

			// Token: 0x020029D2 RID: 10706
			public class DEBRIS
			{
				// Token: 0x02003612 RID: 13842
				public class SATELLITE
				{
					// Token: 0x0400D956 RID: 55638
					public static LocString NAME = "Satellite";

					// Token: 0x0400D957 RID: 55639
					public static LocString DESCRIPTION = "An artificial construct that has escaped its orbit. It no longer appears to be monitored.";
				}
			}

			// Token: 0x020029D3 RID: 10707
			public class NONE
			{
				// Token: 0x0400B5D1 RID: 46545
				public static LocString NAME = "Unselected";
			}

			// Token: 0x020029D4 RID: 10708
			public class ORBIT
			{
				// Token: 0x0400B5D2 RID: 46546
				public static LocString NAME_FMT = "Orbiting {Name}";
			}

			// Token: 0x020029D5 RID: 10709
			public class EMPTY_SPACE
			{
				// Token: 0x0400B5D3 RID: 46547
				public static LocString NAME = "Empty Space";
			}

			// Token: 0x020029D6 RID: 10710
			public class FOG_OF_WAR_SPACE
			{
				// Token: 0x0400B5D4 RID: 46548
				public static LocString NAME = "Unexplored Space";
			}

			// Token: 0x020029D7 RID: 10711
			public class ARTIFACT_POI
			{
				// Token: 0x02003613 RID: 13843
				public class GRAVITASSPACESTATION1
				{
					// Token: 0x0400D958 RID: 55640
					public static LocString NAME = "Destroyed Satellite";

					// Token: 0x0400D959 RID: 55641
					public static LocString DESC = "The remnants of a bygone era, lost in time.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003614 RID: 13844
				public class GRAVITASSPACESTATION2
				{
					// Token: 0x0400D95A RID: 55642
					public static LocString NAME = "Demolished Rocket";

					// Token: 0x0400D95B RID: 55643
					public static LocString DESC = "A defunct rocket from a corporation that vanished long ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003615 RID: 13845
				public class GRAVITASSPACESTATION3
				{
					// Token: 0x0400D95C RID: 55644
					public static LocString NAME = "Ruined Rocket";

					// Token: 0x0400D95D RID: 55645
					public static LocString DESC = "The ruins of a rocket that stopped functioning ages ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003616 RID: 13846
				public class GRAVITASSPACESTATION4
				{
					// Token: 0x0400D95E RID: 55646
					public static LocString NAME = "Retired Planetary Excursion Module";

					// Token: 0x0400D95F RID: 55647
					public static LocString DESC = "A rocket part from a society that has been wiped out.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003617 RID: 13847
				public class GRAVITASSPACESTATION5
				{
					// Token: 0x0400D960 RID: 55648
					public static LocString NAME = "Destroyed Satellite";

					// Token: 0x0400D961 RID: 55649
					public static LocString DESC = "A destroyed Gravitas satellite.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003618 RID: 13848
				public class GRAVITASSPACESTATION6
				{
					// Token: 0x0400D962 RID: 55650
					public static LocString NAME = "Annihilated Satellite";

					// Token: 0x0400D963 RID: 55651
					public static LocString DESC = "The remains of a satellite made some time in the past.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x02003619 RID: 13849
				public class GRAVITASSPACESTATION7
				{
					// Token: 0x0400D964 RID: 55652
					public static LocString NAME = "Wrecked Space Shuttle";

					// Token: 0x0400D965 RID: 55653
					public static LocString DESC = "A defunct space shuttle that floats through space unattended.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x0200361A RID: 13850
				public class GRAVITASSPACESTATION8
				{
					// Token: 0x0400D966 RID: 55654
					public static LocString NAME = "Obsolete Space Station Module";

					// Token: 0x0400D967 RID: 55655
					public static LocString DESC = "The module from a space station that ceased to exist ages ago.\n\nHarvesting space junk requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x0200361B RID: 13851
				public class RUSSELLSTEAPOT
				{
					// Token: 0x0400D968 RID: 55656
					public static LocString NAME = "Russell's Teapot";

					// Token: 0x0400D969 RID: 55657
					public static LocString DESC = "Has never been disproven to not exist.";
				}
			}

			// Token: 0x020029D8 RID: 10712
			public class HARVESTABLE_POI
			{
				// Token: 0x0400B5D5 RID: 46549
				public static LocString POI_PRODUCTION = "{0}";

				// Token: 0x0400B5D6 RID: 46550
				public static LocString POI_PRODUCTION_TOOLTIP = "{0}";

				// Token: 0x0200361C RID: 13852
				public class CARBONASTEROIDFIELD
				{
					// Token: 0x0400D96A RID: 55658
					public static LocString NAME = "Carbon Asteroid Field";

					// Token: 0x0400D96B RID: 55659
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid containing ",
						UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
						" and ",
						UI.FormatAsLink("Coal", "CARBON"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200361D RID: 13853
				public class METALLICASTEROIDFIELD
				{
					// Token: 0x0400D96C RID: 55660
					public static LocString NAME = "Metallic Asteroid Field";

					// Token: 0x0400D96D RID: 55661
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Iron", "IRON"),
						", ",
						UI.FormatAsLink("Copper", "COPPER"),
						" and ",
						UI.FormatAsLink("Obsidian", "OBSIDIAN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200361E RID: 13854
				public class SATELLITEFIELD
				{
					// Token: 0x0400D96E RID: 55662
					public static LocString NAME = "Space Debris";

					// Token: 0x0400D96F RID: 55663
					public static LocString DESC = "Space junk from a forgotten age.\n\nHarvesting resources requires a rocket equipped with a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + ".";
				}

				// Token: 0x0200361F RID: 13855
				public class ROCKYASTEROIDFIELD
				{
					// Token: 0x0400D970 RID: 55664
					public static LocString NAME = "Rocky Asteroid Field";

					// Token: 0x0400D971 RID: 55665
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Copper Ore", "CUPRITE"),
						", ",
						UI.FormatAsLink("Sedimentary Rock", "SEDIMENTARYROCK"),
						" and ",
						UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003620 RID: 13856
				public class INTERSTELLARICEFIELD
				{
					// Token: 0x0400D972 RID: 55666
					public static LocString NAME = "Ice Asteroid Field";

					// Token: 0x0400D973 RID: 55667
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Oxygen", "OXYGEN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003621 RID: 13857
				public class ORGANICMASSFIELD
				{
					// Token: 0x0400D974 RID: 55668
					public static LocString NAME = "Organic Mass Field";

					// Token: 0x0400D975 RID: 55669
					public static LocString DESC = string.Concat(new string[]
					{
						"A mass of harvestable resources containing ",
						UI.FormatAsLink("Algae", "ALGAE"),
						", ",
						UI.FormatAsLink("Slime", "SLIMEMOLD"),
						", ",
						UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
						" and ",
						UI.FormatAsLink("Dirt", "DIRT"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003622 RID: 13858
				public class ICEASTEROIDFIELD
				{
					// Token: 0x0400D976 RID: 55670
					public static LocString NAME = "Exploded Ice Giant";

					// Token: 0x0400D977 RID: 55671
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of planetary remains containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						", ",
						UI.FormatAsLink("Oxygen", "OXYGEN"),
						" and ",
						UI.FormatAsLink("Natural Gas", "METHANE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003623 RID: 13859
				public class GASGIANTCLOUD
				{
					// Token: 0x0400D978 RID: 55672
					public static LocString NAME = "Exploded Gas Giant";

					// Token: 0x0400D979 RID: 55673
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable remains of a planet containing ",
						UI.FormatAsLink("Hydrogen", "HYDROGEN"),
						" in ",
						UI.FormatAsLink("gas", "ELEMENTS_GAS"),
						" form, and ",
						UI.FormatAsLink("Methane", "SOLIDMETHANE"),
						" in ",
						UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
						" and ",
						UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
						" form.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003624 RID: 13860
				public class CHLORINECLOUD
				{
					// Token: 0x0400D97A RID: 55674
					public static LocString NAME = "Chlorine Cloud";

					// Token: 0x0400D97B RID: 55675
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of harvestable debris containing ",
						UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
						" and ",
						UI.FormatAsLink("Bleach Stone", "BLEACHSTONE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003625 RID: 13861
				public class GILDEDASTEROIDFIELD
				{
					// Token: 0x0400D97C RID: 55676
					public static LocString NAME = "Gilded Asteroid Field";

					// Token: 0x0400D97D RID: 55677
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Gold", "GOLD"),
						", ",
						UI.FormatAsLink("Fullerene", "FULLERENE"),
						", ",
						UI.FormatAsLink("Regolith", "REGOLITH"),
						" and more.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003626 RID: 13862
				public class GLIMMERINGASTEROIDFIELD
				{
					// Token: 0x0400D97E RID: 55678
					public static LocString NAME = "Glimmering Asteroid Field";

					// Token: 0x0400D97F RID: 55679
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Tungsten", "TUNGSTEN"),
						", ",
						UI.FormatAsLink("Wolframite", "WOLFRAMITE"),
						" and more.\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003627 RID: 13863
				public class HELIUMCLOUD
				{
					// Token: 0x0400D980 RID: 55680
					public static LocString NAME = "Helium Cloud";

					// Token: 0x0400D981 RID: 55681
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of resources containing ",
						UI.FormatAsLink("Water", "WATER"),
						" and ",
						UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003628 RID: 13864
				public class OILYASTEROIDFIELD
				{
					// Token: 0x0400D982 RID: 55682
					public static LocString NAME = "Oily Asteroid Field";

					// Token: 0x0400D983 RID: 55683
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Solid Methane", "SOLIDMETHANE"),
						", ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003629 RID: 13865
				public class OXIDIZEDASTEROIDFIELD
				{
					// Token: 0x0400D984 RID: 55684
					public static LocString NAME = "Oxidized Asteroid Field";

					// Token: 0x0400D985 RID: 55685
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						" and ",
						UI.FormatAsLink("Rust", "RUST"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200362A RID: 13866
				public class SALTYASTEROIDFIELD
				{
					// Token: 0x0400D986 RID: 55686
					public static LocString NAME = "Salty Asteroid Field";

					// Token: 0x0400D987 RID: 55687
					public static LocString DESC = string.Concat(new string[]
					{
						"A field of harvestable resources containing ",
						UI.FormatAsLink("Salt Water", "SALTWATER"),
						",",
						UI.FormatAsLink("Brine", "BRINE"),
						" and ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200362B RID: 13867
				public class FROZENOREFIELD
				{
					// Token: 0x0400D988 RID: 55688
					public static LocString NAME = "Frozen Ore Asteroid Field";

					// Token: 0x0400D989 RID: 55689
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Polluted Ice", "DIRTYICE"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Snow", "SNOW"),
						" and ",
						UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200362C RID: 13868
				public class FORESTYOREFIELD
				{
					// Token: 0x0400D98A RID: 55690
					public static LocString NAME = "Forested Ore Field";

					// Token: 0x0400D98B RID: 55691
					public static LocString DESC = string.Concat(new string[]
					{
						"A field of harvestable resources containing ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						", ",
						UI.FormatAsLink("Igneous Rock", "IGNEOUSROCK"),
						" and ",
						UI.FormatAsLink("Aluminum Ore", "ALUMINUMORE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200362D RID: 13869
				public class SWAMPYOREFIELD
				{
					// Token: 0x0400D98C RID: 55692
					public static LocString NAME = "Swampy Ore Field";

					// Token: 0x0400D98D RID: 55693
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Mud", "MUD"),
						", ",
						UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
						" and ",
						UI.FormatAsLink("Cobalt Ore", "COBALTITE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200362E RID: 13870
				public class SANDYOREFIELD
				{
					// Token: 0x0400D98E RID: 55694
					public static LocString NAME = "Sandy Ore Field";

					// Token: 0x0400D98F RID: 55695
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Sandstone", "SANDSTONE"),
						", ",
						UI.FormatAsLink("Algae", "ALGAE"),
						", ",
						UI.FormatAsLink("Copper Ore", "CUPRITE"),
						" and ",
						UI.FormatAsLink("Sand", "SAND"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x0200362F RID: 13871
				public class RADIOACTIVEGASCLOUD
				{
					// Token: 0x0400D990 RID: 55696
					public static LocString NAME = "Radioactive Gas Cloud";

					// Token: 0x0400D991 RID: 55697
					public static LocString DESC = string.Concat(new string[]
					{
						"A cloud of resources containing ",
						UI.FormatAsLink("Chlorine Gas", "CHLORINEGAS"),
						", ",
						UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
						" and ",
						UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003630 RID: 13872
				public class RADIOACTIVEASTEROIDFIELD
				{
					// Token: 0x0400D992 RID: 55698
					public static LocString NAME = "Radioactive Asteroid Field";

					// Token: 0x0400D993 RID: 55699
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Bleach Stone", "BLEACHSTONE"),
						", ",
						UI.FormatAsLink("Rust", "RUST"),
						", ",
						UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
						" and ",
						UI.FormatAsLink("Sulfur", "SULFUR"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003631 RID: 13873
				public class OXYGENRICHASTEROIDFIELD
				{
					// Token: 0x0400D994 RID: 55700
					public static LocString NAME = "Oxygen Rich Asteroid Field";

					// Token: 0x0400D995 RID: 55701
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Ice", "ICE"),
						", ",
						UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
						" and ",
						UI.FormatAsLink("Water", "WATER"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003632 RID: 13874
				public class INTERSTELLAROCEAN
				{
					// Token: 0x0400D996 RID: 55702
					public static LocString NAME = "Interstellar Ocean";

					// Token: 0x0400D997 RID: 55703
					public static LocString DESC = string.Concat(new string[]
					{
						"An interplanetary body that consists of ",
						UI.FormatAsLink("Salt Water", "SALTWATER"),
						", ",
						UI.FormatAsLink("Brine", "BRINE"),
						", ",
						UI.FormatAsLink("Salt", "SALT"),
						" and ",
						UI.FormatAsLink("Ice", "ICE"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003633 RID: 13875
				public class DLC2CERESFIELD
				{
					// Token: 0x0400D998 RID: 55704
					public static LocString NAME = "Frozen Cinnabar Asteroid Field";

					// Token: 0x0400D999 RID: 55705
					public static LocString DESC = string.Concat(new string[]
					{
						"The harvestable remains of a planet containing ",
						UI.FormatAsLink("Cinnabar", "Cinnabar"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						" and ",
						UI.FormatAsLink("Mercury", "MERCURY"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}

				// Token: 0x02003634 RID: 13876
				public class DLC2CERESOREFIELD
				{
					// Token: 0x0400D99A RID: 55706
					public static LocString NAME = "Frozen Mercury Asteroid Field";

					// Token: 0x0400D99B RID: 55707
					public static LocString DESC = string.Concat(new string[]
					{
						"An asteroid field containing ",
						UI.FormatAsLink("Cinnabar", "Cinnabar"),
						", ",
						UI.FormatAsLink("Ice", "ICE"),
						" and ",
						UI.FormatAsLink("Mercury", "MERCURY"),
						".\n\nHarvesting resources requires a rocket equipped with a ",
						UI.FormatAsLink("Drillcone", "NOSECONEHARVEST"),
						"."
					});
				}
			}

			// Token: 0x020029D9 RID: 10713
			public class GRAVITAS_SPACE_POI
			{
				// Token: 0x0400B5D7 RID: 46551
				public static LocString STATION = "Destroyed Gravitas Space Station";
			}

			// Token: 0x020029DA RID: 10714
			public class TELESCOPE_TARGET
			{
				// Token: 0x0400B5D8 RID: 46552
				public static LocString NAME = "Telescope Target";
			}

			// Token: 0x020029DB RID: 10715
			public class ASTEROIDS
			{
				// Token: 0x02003635 RID: 13877
				public class ROCKYASTEROID
				{
					// Token: 0x0400D99C RID: 55708
					public static LocString NAME = "Rocky Asteroid";

					// Token: 0x0400D99D RID: 55709
					public static LocString DESCRIPTION = "A minor mineral planet. Unlike a comet, it does not possess a tail.";
				}

				// Token: 0x02003636 RID: 13878
				public class METALLICASTEROID
				{
					// Token: 0x0400D99E RID: 55710
					public static LocString NAME = "Metallic Asteroid";

					// Token: 0x0400D99F RID: 55711
					public static LocString DESCRIPTION = "A shimmering conglomerate of various metals.";
				}

				// Token: 0x02003637 RID: 13879
				public class CARBONACEOUSASTEROID
				{
					// Token: 0x0400D9A0 RID: 55712
					public static LocString NAME = "Carbon Asteroid";

					// Token: 0x0400D9A1 RID: 55713
					public static LocString DESCRIPTION = "A common asteroid containing several useful resources.";
				}

				// Token: 0x02003638 RID: 13880
				public class OILYASTEROID
				{
					// Token: 0x0400D9A2 RID: 55714
					public static LocString NAME = "Oily Asteroid";

					// Token: 0x0400D9A3 RID: 55715
					public static LocString DESCRIPTION = "A viscous asteroid that is only loosely held together. Contains fossil fuel resources.";
				}

				// Token: 0x02003639 RID: 13881
				public class GOLDASTEROID
				{
					// Token: 0x0400D9A4 RID: 55716
					public static LocString NAME = "Gilded Asteroid";

					// Token: 0x0400D9A5 RID: 55717
					public static LocString DESCRIPTION = "A rich asteroid with thin gold coating and veins of gold deposits throughout.";
				}
			}

			// Token: 0x020029DC RID: 10716
			public class CLUSTERMAPMETEORSHOWERS
			{
				// Token: 0x0200363A RID: 13882
				public class UNIDENTIFIED
				{
					// Token: 0x0400D9A6 RID: 55718
					public static LocString NAME = "Unidentified Object";

					// Token: 0x0400D9A7 RID: 55719
					public static LocString DESCRIPTION = "A cosmic anomaly is traveling through the galaxy.\n\nIts origins and purpose are currently unknown, though a " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " could change that.";
				}

				// Token: 0x0200363B RID: 13883
				public class SLIME
				{
					// Token: 0x0400D9A8 RID: 55720
					public static LocString NAME = "Slimy Meteor Shower";

					// Token: 0x0400D9A9 RID: 55721
					public static LocString DESCRIPTION = "A shower of slimy, biodynamic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x0200363C RID: 13884
				public class SNOW
				{
					// Token: 0x0400D9AA RID: 55722
					public static LocString NAME = "Blizzard Meteor Shower";

					// Token: 0x0400D9AB RID: 55723
					public static LocString DESCRIPTION = "A shower of cold, cold meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x0200363D RID: 13885
				public class ICE
				{
					// Token: 0x0400D9AC RID: 55724
					public static LocString NAME = "Ice Meteor Shower";

					// Token: 0x0400D9AD RID: 55725
					public static LocString DESCRIPTION = "A hailstorm of icy space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x0200363E RID: 13886
				public class ICEANDTREES
				{
					// Token: 0x0400D9AE RID: 55726
					public static LocString NAME = "Icy Nectar Meteor Shower";

					// Token: 0x0400D9AF RID: 55727
					public static LocString DESCRIPTION = "A hailstorm of sweet, icy space rocks on a collision course with the surface of an asteroid";
				}

				// Token: 0x0200363F RID: 13887
				public class COPPER
				{
					// Token: 0x0400D9B0 RID: 55728
					public static LocString NAME = "Copper Meteor Shower";

					// Token: 0x0400D9B1 RID: 55729
					public static LocString DESCRIPTION = "A shower of metallic meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003640 RID: 13888
				public class IRON
				{
					// Token: 0x0400D9B2 RID: 55730
					public static LocString NAME = "Iron Meteor Shower";

					// Token: 0x0400D9B3 RID: 55731
					public static LocString DESCRIPTION = "A shower of metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003641 RID: 13889
				public class GOLD
				{
					// Token: 0x0400D9B4 RID: 55732
					public static LocString NAME = "Gold Meteor Shower";

					// Token: 0x0400D9B5 RID: 55733
					public static LocString DESCRIPTION = "A shower of shiny metallic space rocks on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003642 RID: 13890
				public class URANIUM
				{
					// Token: 0x0400D9B6 RID: 55734
					public static LocString NAME = "Uranium Meteor Shower";

					// Token: 0x0400D9B7 RID: 55735
					public static LocString DESCRIPTION = "A toxic shower of radioactive meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003643 RID: 13891
				public class LIGHTDUST
				{
					// Token: 0x0400D9B8 RID: 55736
					public static LocString NAME = "Dust Fluff Meteor Shower";

					// Token: 0x0400D9B9 RID: 55737
					public static LocString DESCRIPTION = "A cloud-like shower of dust fluff meteors heading towards the surface of an asteroid.";
				}

				// Token: 0x02003644 RID: 13892
				public class HEAVYDUST
				{
					// Token: 0x0400D9BA RID: 55738
					public static LocString NAME = "Dense Dust Meteor Shower";

					// Token: 0x0400D9BB RID: 55739
					public static LocString DESCRIPTION = "A dark cloud of heavy dust meteors heading towards the surface of an asteroid.";
				}

				// Token: 0x02003645 RID: 13893
				public class REGOLITH
				{
					// Token: 0x0400D9BC RID: 55740
					public static LocString NAME = "Regolith Meteor Shower";

					// Token: 0x0400D9BD RID: 55741
					public static LocString DESCRIPTION = "A shower of rocky meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003646 RID: 13894
				public class OXYLITE
				{
					// Token: 0x0400D9BE RID: 55742
					public static LocString NAME = "Oxylite Meteor Shower";

					// Token: 0x0400D9BF RID: 55743
					public static LocString DESCRIPTION = "A shower of rocky, oxygen-rich meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003647 RID: 13895
				public class BLEACHSTONE
				{
					// Token: 0x0400D9C0 RID: 55744
					public static LocString NAME = "Bleach Stone Meteor Shower";

					// Token: 0x0400D9C1 RID: 55745
					public static LocString DESCRIPTION = "A shower of bleach stone meteors on a collision course with the surface of an asteroid.";
				}

				// Token: 0x02003648 RID: 13896
				public class MOO
				{
					// Token: 0x0400D9C2 RID: 55746
					public static LocString NAME = "Gassy Mooteor Shower";

					// Token: 0x0400D9C3 RID: 55747
					public static LocString DESCRIPTION = "A herd of methane-infused meteors that cause a bit of a stink, but do no actual damage.";
				}
			}

			// Token: 0x020029DD RID: 10717
			public class COMETS
			{
				// Token: 0x02003649 RID: 13897
				public class ROCKCOMET
				{
					// Token: 0x0400D9C4 RID: 55748
					public static LocString NAME = "Rock Meteor";
				}

				// Token: 0x0200364A RID: 13898
				public class DUSTCOMET
				{
					// Token: 0x0400D9C5 RID: 55749
					public static LocString NAME = "Dust Meteor";
				}

				// Token: 0x0200364B RID: 13899
				public class IRONCOMET
				{
					// Token: 0x0400D9C6 RID: 55750
					public static LocString NAME = "Iron Meteor";
				}

				// Token: 0x0200364C RID: 13900
				public class COPPERCOMET
				{
					// Token: 0x0400D9C7 RID: 55751
					public static LocString NAME = "Copper Meteor";
				}

				// Token: 0x0200364D RID: 13901
				public class GOLDCOMET
				{
					// Token: 0x0400D9C8 RID: 55752
					public static LocString NAME = "Gold Meteor";
				}

				// Token: 0x0200364E RID: 13902
				public class FULLERENECOMET
				{
					// Token: 0x0400D9C9 RID: 55753
					public static LocString NAME = "Fullerene Meteor";
				}

				// Token: 0x0200364F RID: 13903
				public class URANIUMORECOMET
				{
					// Token: 0x0400D9CA RID: 55754
					public static LocString NAME = "Uranium Meteor";
				}

				// Token: 0x02003650 RID: 13904
				public class NUCLEAR_WASTE
				{
					// Token: 0x0400D9CB RID: 55755
					public static LocString NAME = "Radioactive Meteor";
				}

				// Token: 0x02003651 RID: 13905
				public class SATELLITE
				{
					// Token: 0x0400D9CC RID: 55756
					public static LocString NAME = "Defunct Satellite";
				}

				// Token: 0x02003652 RID: 13906
				public class FOODCOMET
				{
					// Token: 0x0400D9CD RID: 55757
					public static LocString NAME = "Snack Bomb";
				}

				// Token: 0x02003653 RID: 13907
				public class GASSYMOOCOMET
				{
					// Token: 0x0400D9CE RID: 55758
					public static LocString NAME = "Gassy Mooteor";
				}

				// Token: 0x02003654 RID: 13908
				public class SLIMECOMET
				{
					// Token: 0x0400D9CF RID: 55759
					public static LocString NAME = "Slime Meteor";
				}

				// Token: 0x02003655 RID: 13909
				public class SNOWBALLCOMET
				{
					// Token: 0x0400D9D0 RID: 55760
					public static LocString NAME = "Snow Meteor";
				}

				// Token: 0x02003656 RID: 13910
				public class SPACETREESEEDCOMET
				{
					// Token: 0x0400D9D1 RID: 55761
					public static LocString NAME = "Bonbon Meteor";
				}

				// Token: 0x02003657 RID: 13911
				public class HARDICECOMET
				{
					// Token: 0x0400D9D2 RID: 55762
					public static LocString NAME = "Ice Meteor";
				}

				// Token: 0x02003658 RID: 13912
				public class LIGHTDUSTCOMET
				{
					// Token: 0x0400D9D3 RID: 55763
					public static LocString NAME = "Dust Fluff Meteor";
				}

				// Token: 0x02003659 RID: 13913
				public class ALGAECOMET
				{
					// Token: 0x0400D9D4 RID: 55764
					public static LocString NAME = "Algae Meteor";
				}

				// Token: 0x0200365A RID: 13914
				public class PHOSPHORICCOMET
				{
					// Token: 0x0400D9D5 RID: 55765
					public static LocString NAME = "Phosphoric Meteor";
				}

				// Token: 0x0200365B RID: 13915
				public class OXYLITECOMET
				{
					// Token: 0x0400D9D6 RID: 55766
					public static LocString NAME = "Oxylite Meteor";
				}

				// Token: 0x0200365C RID: 13916
				public class BLEACHSTONECOMET
				{
					// Token: 0x0400D9D7 RID: 55767
					public static LocString NAME = "Bleach Stone Meteor";
				}

				// Token: 0x0200365D RID: 13917
				public class MINICOMET
				{
					// Token: 0x0400D9D8 RID: 55768
					public static LocString NAME = "Debris Projectile";
				}
			}

			// Token: 0x020029DE RID: 10718
			public class DWARFPLANETS
			{
				// Token: 0x0200365E RID: 13918
				public class ICYDWARF
				{
					// Token: 0x0400D9D9 RID: 55769
					public static LocString NAME = "Interstellar Ice";

					// Token: 0x0400D9DA RID: 55770
					public static LocString DESCRIPTION = "A terrestrial destination, frozen completely solid.";
				}

				// Token: 0x0200365F RID: 13919
				public class ORGANICDWARF
				{
					// Token: 0x0400D9DB RID: 55771
					public static LocString NAME = "Organic Mass";

					// Token: 0x0400D9DC RID: 55772
					public static LocString DESCRIPTION = "A mass of organic material similar to the ooze used to print Duplicants. This sample is heavily degraded.";
				}

				// Token: 0x02003660 RID: 13920
				public class DUSTYDWARF
				{
					// Token: 0x0400D9DD RID: 55773
					public static LocString NAME = "Dusty Dwarf";

					// Token: 0x0400D9DE RID: 55774
					public static LocString DESCRIPTION = "A loosely held together composite of minerals.";
				}

				// Token: 0x02003661 RID: 13921
				public class SALTDWARF
				{
					// Token: 0x0400D9DF RID: 55775
					public static LocString NAME = "Salty Dwarf";

					// Token: 0x0400D9E0 RID: 55776
					public static LocString DESCRIPTION = "A dwarf planet with unusually high sodium concentrations.";
				}

				// Token: 0x02003662 RID: 13922
				public class REDDWARF
				{
					// Token: 0x0400D9E1 RID: 55777
					public static LocString NAME = "Red Dwarf";

					// Token: 0x0400D9E2 RID: 55778
					public static LocString DESCRIPTION = "An M-class star orbited by clusters of extractable aluminum and methane.";
				}
			}

			// Token: 0x020029DF RID: 10719
			public class PLANETS
			{
				// Token: 0x02003663 RID: 13923
				public class TERRAPLANET
				{
					// Token: 0x0400D9E3 RID: 55779
					public static LocString NAME = "Terrestrial Planet";

					// Token: 0x0400D9E4 RID: 55780
					public static LocString DESCRIPTION = "A planet with a walkable surface, though it does not possess the resources to sustain long-term life.";
				}

				// Token: 0x02003664 RID: 13924
				public class VOLCANOPLANET
				{
					// Token: 0x0400D9E5 RID: 55781
					public static LocString NAME = "Volcanic Planet";

					// Token: 0x0400D9E6 RID: 55782
					public static LocString DESCRIPTION = "A large terrestrial object composed mainly of molten rock.";
				}

				// Token: 0x02003665 RID: 13925
				public class SHATTEREDPLANET
				{
					// Token: 0x0400D9E7 RID: 55783
					public static LocString NAME = "Shattered Planet";

					// Token: 0x0400D9E8 RID: 55784
					public static LocString DESCRIPTION = "A once-habitable planet that has sustained massive damage.\n\nA powerful containment field prevents our rockets from traveling to its surface.";
				}

				// Token: 0x02003666 RID: 13926
				public class RUSTPLANET
				{
					// Token: 0x0400D9E9 RID: 55785
					public static LocString NAME = "Oxidized Asteroid";

					// Token: 0x0400D9EA RID: 55786
					public static LocString DESCRIPTION = "A small planet covered in large swathes of brown rust.";
				}

				// Token: 0x02003667 RID: 13927
				public class FORESTPLANET
				{
					// Token: 0x0400D9EB RID: 55787
					public static LocString NAME = "Living Planet";

					// Token: 0x0400D9EC RID: 55788
					public static LocString DESCRIPTION = "A small green planet displaying several markers of primitive life.";
				}

				// Token: 0x02003668 RID: 13928
				public class SHINYPLANET
				{
					// Token: 0x0400D9ED RID: 55789
					public static LocString NAME = "Glimmering Planet";

					// Token: 0x0400D9EE RID: 55790
					public static LocString DESCRIPTION = "A planet composed of rare, shimmering minerals. From the distance, it looks like gem in the sky.";
				}

				// Token: 0x02003669 RID: 13929
				public class CHLORINEPLANET
				{
					// Token: 0x0400D9EF RID: 55791
					public static LocString NAME = "Chlorine Planet";

					// Token: 0x0400D9F0 RID: 55792
					public static LocString DESCRIPTION = "A noxious planet permeated by unbreathable chlorine.";
				}

				// Token: 0x0200366A RID: 13930
				public class SALTDESERTPLANET
				{
					// Token: 0x0400D9F1 RID: 55793
					public static LocString NAME = "Arid Planet";

					// Token: 0x0400D9F2 RID: 55794
					public static LocString DESCRIPTION = "A sweltering, desert-like planet covered in surface salt deposits.";
				}

				// Token: 0x0200366B RID: 13931
				public class DLC2CERESSPACEDESTINATION
				{
					// Token: 0x0400D9F3 RID: 55795
					public static LocString NAME = "Ceres";

					// Token: 0x0400D9F4 RID: 55796
					public static LocString DESCRIPTION = "A frozen planet peppered with cinnabar deposits.";
				}
			}

			// Token: 0x020029E0 RID: 10720
			public class GIANTS
			{
				// Token: 0x0200366C RID: 13932
				public class GASGIANT
				{
					// Token: 0x0400D9F5 RID: 55797
					public static LocString NAME = "Gas Giant";

					// Token: 0x0400D9F6 RID: 55798
					public static LocString DESCRIPTION = "A massive volume of " + UI.FormatAsLink("Hydrogen Gas", "HYDROGEN") + " formed around a small solid center.";
				}

				// Token: 0x0200366D RID: 13933
				public class ICEGIANT
				{
					// Token: 0x0400D9F7 RID: 55799
					public static LocString NAME = "Ice Giant";

					// Token: 0x0400D9F8 RID: 55800
					public static LocString DESCRIPTION = "A massive volume of frozen material, primarily composed of " + UI.FormatAsLink("Ice", "ICE") + ".";
				}

				// Token: 0x0200366E RID: 13934
				public class HYDROGENGIANT
				{
					// Token: 0x0400D9F9 RID: 55801
					public static LocString NAME = "Helium Giant";

					// Token: 0x0400D9FA RID: 55802
					public static LocString DESCRIPTION = "A massive volume of " + UI.FormatAsLink("Helium", "HELIUM") + " formed around a small solid center.";
				}
			}
		}

		// Token: 0x0200214B RID: 8523
		public class SPACEARTIFACTS
		{
			// Token: 0x020029E1 RID: 10721
			public class ARTIFACTTIERS
			{
				// Token: 0x0400B5D9 RID: 46553
				public static LocString TIER_NONE = "Nothing";

				// Token: 0x0400B5DA RID: 46554
				public static LocString TIER0 = "Rarity 0";

				// Token: 0x0400B5DB RID: 46555
				public static LocString TIER1 = "Rarity 1";

				// Token: 0x0400B5DC RID: 46556
				public static LocString TIER2 = "Rarity 2";

				// Token: 0x0400B5DD RID: 46557
				public static LocString TIER3 = "Rarity 3";

				// Token: 0x0400B5DE RID: 46558
				public static LocString TIER4 = "Rarity 4";

				// Token: 0x0400B5DF RID: 46559
				public static LocString TIER5 = "Rarity 5";
			}

			// Token: 0x020029E2 RID: 10722
			public class PACUPERCOLATOR
			{
				// Token: 0x0400B5E0 RID: 46560
				public static LocString NAME = "Percolator";

				// Token: 0x0400B5E1 RID: 46561
				public static LocString DESCRIPTION = "Don't drink from it! There was a pacu... IN the percolator!";

				// Token: 0x0400B5E2 RID: 46562
				public static LocString ARTIFACT = "A coffee percolator with the remnants of a blend of coffee that was a personal favorite of Dr. Hassan Aydem.\n\nHe would specifically reserve the consumption of this particular blend for when he was reviewing research papers on Sunday afternoons.";
			}

			// Token: 0x020029E3 RID: 10723
			public class ROBOTARM
			{
				// Token: 0x0400B5E3 RID: 46563
				public static LocString NAME = "Robot Arm";

				// Token: 0x0400B5E4 RID: 46564
				public static LocString DESCRIPTION = "It's not functional. Just cool.";

				// Token: 0x0400B5E5 RID: 46565
				public static LocString ARTIFACT = "A commercially available robot arm that has had a significant amount of modifications made to it.\n\nThe initials B.A. appear on one of the fingers.";
			}

			// Token: 0x020029E4 RID: 10724
			public class HATCHFOSSIL
			{
				// Token: 0x0400B5E6 RID: 46566
				public static LocString NAME = "Pristine Fossil";

				// Token: 0x0400B5E7 RID: 46567
				public static LocString DESCRIPTION = "The preserved bones of an early species of Hatch.";

				// Token: 0x0400B5E8 RID: 46568
				public static LocString ARTIFACT = "The preservation of this skeleton occurred artificially using a technique called the \"The Ali Method\".\n\nIt should be noted that this fossilization technique was pioneered by one Dr. Ashkan Seyed Ali, an employee of Gravitas.";
			}

			// Token: 0x020029E5 RID: 10725
			public class MODERNART
			{
				// Token: 0x0400B5E9 RID: 46569
				public static LocString NAME = "Modern Art";

				// Token: 0x0400B5EA RID: 46570
				public static LocString DESCRIPTION = "I don't get it.";

				// Token: 0x0400B5EB RID: 46571
				public static LocString ARTIFACT = "A sculpture of the Neoplastism movement of Modern Art.\n\nGravitas records show that this piece was once used in a presentation called 'Form and Function in Corporate Aesthetic'.";
			}

			// Token: 0x020029E6 RID: 10726
			public class EGGROCK
			{
				// Token: 0x0400B5EC RID: 46572
				public static LocString NAME = "Egg-Shaped Rock";

				// Token: 0x0400B5ED RID: 46573
				public static LocString DESCRIPTION = "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.";

				// Token: 0x0400B5EE RID: 46574
				public static LocString ARTIFACT = "The words \"Happy Farters Day Dad. Love Macy\" appear on the bottom of this rock, written in a childlish scrawl.";
			}

			// Token: 0x020029E7 RID: 10727
			public class RAINBOWEGGROCK
			{
				// Token: 0x0400B5EF RID: 46575
				public static LocString NAME = "Egg-Shaped Rock";

				// Token: 0x0400B5F0 RID: 46576
				public static LocString DESCRIPTION = "It's unclear whether this is its naturally occurring shape, or if its appearance as been sculpted.\n\nThis one is rainbow colored.";

				// Token: 0x0400B5F1 RID: 46577
				public static LocString ARTIFACT = "The words \"Happy Father's Day, Dad. Love you!\" appear on the bottom of this rock, written in very neat handwriting. The words are surrounded by four hearts drawn in what appears to be a pink gel pen.";
			}

			// Token: 0x020029E8 RID: 10728
			public class OKAYXRAY
			{
				// Token: 0x0400B5F2 RID: 46578
				public static LocString NAME = "Old X-Ray";

				// Token: 0x0400B5F3 RID: 46579
				public static LocString DESCRIPTION = "Ew, weird. It has five fingers!";

				// Token: 0x0400B5F4 RID: 46580
				public static LocString ARTIFACT = "The description on this X-ray indicates that it was taken in the Gravitas Medical Facility.\n\nMost likely this X-ray was performed while investigating an injury that occurred within the facility.";
			}

			// Token: 0x020029E9 RID: 10729
			public class SHIELDGENERATOR
			{
				// Token: 0x0400B5F5 RID: 46581
				public static LocString NAME = "Shield Generator";

				// Token: 0x0400B5F6 RID: 46582
				public static LocString DESCRIPTION = "A mechanical prototype capable of producing a small section of shielding.";

				// Token: 0x0400B5F7 RID: 46583
				public static LocString ARTIFACT = "The energy field produced by this shield generator completely ignores those light behaviors which are wave-like and focuses instead on its particle behaviors.\n\nThis seemingly paradoxical state is possible when light is slowed down to the point at which it stops entirely.";
			}

			// Token: 0x020029EA RID: 10730
			public class TEAPOT
			{
				// Token: 0x0400B5F8 RID: 46584
				public static LocString NAME = "Encrusted Teapot";

				// Token: 0x0400B5F9 RID: 46585
				public static LocString DESCRIPTION = "A teapot from the depths of space, coated in a thick layer of Neutronium.";

				// Token: 0x0400B5FA RID: 46586
				public static LocString ARTIFACT = "The amount of Neutronium present in this teapot suggests that it has crossed the threshold of the spacetime continuum on countless occasions, floating through many multiple universes over a plethora of times and spaces.\n\nThough there are, theoretically, an infinite amount of outcomes to any one event over many multi-verses, the homogeneity of the still relatively young multiverse suggests that this is then not the only teapot which has crossed into multiple universes. Despite the infinite possible outcomes of infinite multiverses it appears one high probability constant is that there is, or once was, a teapot floating somewhere in space within every universe.";
			}

			// Token: 0x020029EB RID: 10731
			public class DNAMODEL
			{
				// Token: 0x0400B5FB RID: 46587
				public static LocString NAME = "Double Helix Model";

				// Token: 0x0400B5FC RID: 46588
				public static LocString DESCRIPTION = "An educational model of genetic information.";

				// Token: 0x0400B5FD RID: 46589
				public static LocString ARTIFACT = "A physical representation of the building blocks of life.\n\nThis one contains trace amounts of a Genetic Ooze prototype that was once used by Gravitas.";
			}

			// Token: 0x020029EC RID: 10732
			public class SANDSTONE
			{
				// Token: 0x0400B5FE RID: 46590
				public static LocString NAME = "Sandstone";

				// Token: 0x0400B5FF RID: 46591
				public static LocString DESCRIPTION = "A beautiful rock composed of multiple layers of sediment.";

				// Token: 0x0400B600 RID: 46592
				public static LocString ARTIFACT = "This sample of sandstone appears to have been processed by the Gravitas Mining Gun that was made available to the general public.\n\nNote: The Gravitas public Mining Gun model is different than ones used by Duplicants in its larger size, and extra precautionary features added in order to be compliant with national safety standards.";
			}

			// Token: 0x020029ED RID: 10733
			public class MAGMALAMP
			{
				// Token: 0x0400B601 RID: 46593
				public static LocString NAME = "Magma Lamp";

				// Token: 0x0400B602 RID: 46594
				public static LocString DESCRIPTION = "The sequel to \"Lava Lamp\".";

				// Token: 0x0400B603 RID: 46595
				public static LocString ARTIFACT = "Molten lava and obsidian combined in a way that allows the lava to maintain just enough heat to remain in liquid form.\n\nPlans of this lamp found in the Gravitas archives have been attributed to one Robin Nisbet, PhD.";
			}

			// Token: 0x020029EE RID: 10734
			public class OBELISK
			{
				// Token: 0x0400B604 RID: 46596
				public static LocString NAME = "Small Obelisk";

				// Token: 0x0400B605 RID: 46597
				public static LocString DESCRIPTION = "A rectangular stone piece.\n\nIts function is unclear.";

				// Token: 0x0400B606 RID: 46598
				public static LocString ARTIFACT = "On close inspection this rectangle is actually a stone box built with a covert, almost seamless, lid, housing a tiny key.\n\nIt is still unclear what the key unlocks.";
			}

			// Token: 0x020029EF RID: 10735
			public class RUBIKSCUBE
			{
				// Token: 0x0400B607 RID: 46599
				public static LocString NAME = "Rubik's Cube";

				// Token: 0x0400B608 RID: 46600
				public static LocString DESCRIPTION = "This mystery of the universe has already been solved.";

				// Token: 0x0400B609 RID: 46601
				public static LocString ARTIFACT = "A well-used, competition-compliant version of the popular puzzle cube.\n\nIt's worth noting that Dr. Dylan 'Nails' Winslow was once a regional Rubik's Cube champion.";
			}

			// Token: 0x020029F0 RID: 10736
			public class OFFICEMUG
			{
				// Token: 0x0400B60A RID: 46602
				public static LocString NAME = "Office Mug";

				// Token: 0x0400B60B RID: 46603
				public static LocString DESCRIPTION = "An intermediary place to store espresso before you move it to your mouth.";

				// Token: 0x0400B60C RID: 46604
				public static LocString ARTIFACT = "An office mug with the Gravitas logo on it. Though their office mugs were all emblazoned with the same logo, Gravitas colored their mugs differently to distinguish between their various departments.\n\nThis one is from the AI department.";
			}

			// Token: 0x020029F1 RID: 10737
			public class AMELIASWATCH
			{
				// Token: 0x0400B60D RID: 46605
				public static LocString NAME = "Wrist Watch";

				// Token: 0x0400B60E RID: 46606
				public static LocString DESCRIPTION = "It was discovered in a package labeled \"To be entrusted to Dr. Walker\".";

				// Token: 0x0400B60F RID: 46607
				public static LocString ARTIFACT = "This watch once belonged to pioneering aviator Amelia Earhart and travelled to space via astronaut Dr. Shannon Walker.\n\nHow it came to be floating in space is a matter of speculation, but perhaps the adventurous spirit of its original stewards became infused within the fabric of this timepiece and compelled the universe to launch it into the great unknown.";
			}

			// Token: 0x020029F2 RID: 10738
			public class MOONMOONMOON
			{
				// Token: 0x0400B610 RID: 46608
				public static LocString NAME = "Moonmoonmoon";

				// Token: 0x0400B611 RID: 46609
				public static LocString DESCRIPTION = "A moon's moon's moon. It's very small.";

				// Token: 0x0400B612 RID: 46610
				public static LocString ARTIFACT = "In contrast to most moons, this object's glowing properties do not come from reflecting an external source of light, but rather from an internal glow of mysterious origin.\n\nThe glow of this object also grants an extraordinary amount of Decor bonus to nearby Duplicants, almost as if it was designed that way.";
			}

			// Token: 0x020029F3 RID: 10739
			public class BIOLUMINESCENTROCK
			{
				// Token: 0x0400B613 RID: 46611
				public static LocString NAME = "Bioluminescent Rock";

				// Token: 0x0400B614 RID: 46612
				public static LocString DESCRIPTION = "A thriving colony of tiny, microscopic organisms is responsible for giving it its bluish glow.";

				// Token: 0x0400B615 RID: 46613
				public static LocString ARTIFACT = "The microscopic organisms within this rock are of a unique variety whose genetic code shows many tell-tale signs of being genetically engineered within a lab.\n\nFurther analysis reveals they share 99.999% of their genetic code with Shine Bugs.";
			}

			// Token: 0x020029F4 RID: 10740
			public class PLASMALAMP
			{
				// Token: 0x0400B616 RID: 46614
				public static LocString NAME = "Plasma Lamp";

				// Token: 0x0400B617 RID: 46615
				public static LocString DESCRIPTION = "No space colony is complete without one.";

				// Token: 0x0400B618 RID: 46616
				public static LocString ARTIFACT = "The bottom of this lamp contains the words 'Property of the Atmospheric Sciences Department'.\n\nIt's worth noting that the Gravitas Atmospheric Sciences Department once simulated an experiment testing the feasibility of survival in an environment filled with noble gasses, similar to the ones contained within this device.";
			}

			// Token: 0x020029F5 RID: 10741
			public class MOLDAVITE
			{
				// Token: 0x0400B619 RID: 46617
				public static LocString NAME = "Moldavite";

				// Token: 0x0400B61A RID: 46618
				public static LocString DESCRIPTION = "A unique green stone formed from the impact of a meteorite.";

				// Token: 0x0400B61B RID: 46619
				public static LocString ARTIFACT = "This extremely rare, museum grade moldavite once sat on the desk of Dr. Ren Sato, but it was stolen by some unknown person.\n\nDr. Sato suspected the perpetrator was none other than Director Stern, but was never able to confirm this theory.";
			}

			// Token: 0x020029F6 RID: 10742
			public class BRICKPHONE
			{
				// Token: 0x0400B61C RID: 46620
				public static LocString NAME = "Strange Brick";

				// Token: 0x0400B61D RID: 46621
				public static LocString DESCRIPTION = "It still works.";

				// Token: 0x0400B61E RID: 46622
				public static LocString ARTIFACT = "This cordless phone once held a direct line to an unknown location in which strange distant voices can be heard but not understood, nor interacted with.\n\nThough Gravitas spent a lot of money and years of study dedicated to discovering its secret, the mystery was never solved.";
			}

			// Token: 0x020029F7 RID: 10743
			public class SOLARSYSTEM
			{
				// Token: 0x0400B61F RID: 46623
				public static LocString NAME = "Self-Contained System";

				// Token: 0x0400B620 RID: 46624
				public static LocString DESCRIPTION = "A marvel of the cosmos, inside this display is an entirely self-contained solar system.";

				// Token: 0x0400B621 RID: 46625
				public static LocString ARTIFACT = "This marvel of a device was built using parts from an old Tornado-in-a-Box science fair project.\n\nVery faint, faded letters are still visible on the display bottom that read 'Camille P. Grade 5'.";
			}

			// Token: 0x020029F8 RID: 10744
			public class SINK
			{
				// Token: 0x0400B622 RID: 46626
				public static LocString NAME = "Sink";

				// Token: 0x0400B623 RID: 46627
				public static LocString DESCRIPTION = "No collection is complete without it.";

				// Token: 0x0400B624 RID: 46628
				public static LocString ARTIFACT = "A small trace of encrusted soap on this sink strongly suggests it was installed in a personal bathroom, rather than a public one which would have used a soap dispenser.\n\nThe soap sliver is light blue and contains a manufactured blueberry fragrance.";
			}

			// Token: 0x020029F9 RID: 10745
			public class ROCKTORNADO
			{
				// Token: 0x0400B625 RID: 46629
				public static LocString NAME = "Tornado Rock";

				// Token: 0x0400B626 RID: 46630
				public static LocString DESCRIPTION = "It's unclear how it formed, although I'm glad it did.";

				// Token: 0x0400B627 RID: 46631
				public static LocString ARTIFACT = "Speculations about the origin of this rock include a paper written by one Harold P. Moreson, Ph.D. in which he theorized it could be a rare form of hollow geode which failed to form any crystals inside.\n\nThis paper appears in the Gravitas archives, and in all probability, was one of the factors in the hiring of Moreson into the Geology department of the company.";
			}

			// Token: 0x020029FA RID: 10746
			public class BLENDER
			{
				// Token: 0x0400B628 RID: 46632
				public static LocString NAME = "Blender";

				// Token: 0x0400B629 RID: 46633
				public static LocString DESCRIPTION = "Equipment used to conduct experiments answering the age-old question, \"Could that blend\"?";

				// Token: 0x0400B62A RID: 46634
				public static LocString ARTIFACT = "Trace amounts of edible foodstuffs present in this blender indicate that it was probably used to emulsify the ingredients of a mush bar.\n\nIt is also very likely that it was employed at least once in the production of a peanut butter and banana smoothie.";
			}

			// Token: 0x020029FB RID: 10747
			public class SAXOPHONE
			{
				// Token: 0x0400B62B RID: 46635
				public static LocString NAME = "Mangled Saxophone";

				// Token: 0x0400B62C RID: 46636
				public static LocString DESCRIPTION = "The name \"Pesquet\" is barely legible on the inside.";

				// Token: 0x0400B62D RID: 46637
				public static LocString ARTIFACT = "Though it is often remarked that \"in space, no one can hear you scream\", Thomas Pesquet proved the same cannot be said for the smooth jazzy sounds of a saxophone.\n\nAlthough this instrument once belonged to the eminent French Astronaut its current bumped and bent shape suggests it has seen many adventures beyond that of just being used to perform an out-of-this-world saxophone solo.";
			}

			// Token: 0x020029FC RID: 10748
			public class STETHOSCOPE
			{
				// Token: 0x0400B62E RID: 46638
				public static LocString NAME = "Stethoscope";

				// Token: 0x0400B62F RID: 46639
				public static LocString DESCRIPTION = "Listens to Duplicant heartbeats, or gurgly tummies.";

				// Token: 0x0400B630 RID: 46640
				public static LocString ARTIFACT = "The size and shape of this stethescope suggests it was not intended to be used by neither a human-sized nor a Duplicant-sized person but something half-way in between the two beings.";
			}

			// Token: 0x020029FD RID: 10749
			public class VHS
			{
				// Token: 0x0400B631 RID: 46641
				public static LocString NAME = "Archaic Tech";

				// Token: 0x0400B632 RID: 46642
				public static LocString DESCRIPTION = "Be kind when you handle it. It's very fragile.";

				// Token: 0x0400B633 RID: 46643
				public static LocString ARTIFACT = "The label on this VHS tape reads \"Jackie and Olivia's House Warming Party\".\n\nUnfortunately, a device with which to play this recording no longer exists in this universe.";
			}

			// Token: 0x020029FE RID: 10750
			public class REACTORMODEL
			{
				// Token: 0x0400B634 RID: 46644
				public static LocString NAME = "Model Nuclear Power Plant";

				// Token: 0x0400B635 RID: 46645
				public static LocString DESCRIPTION = "It's pronounced nu-clear.";

				// Token: 0x0400B636 RID: 46646
				public static LocString ARTIFACT = "Though this Nuclear Power Plant was never built, this model exists as an artifact to a time early in the life of Gravitas when it was researching all alternatives to solving the global energy problem.\n\nUltimately, the idea of building a Nuclear Power Plant was abandoned in favor of the \"much safer\" alternative of developing the Temporal Bow.";
			}

			// Token: 0x020029FF RID: 10751
			public class MOODRING
			{
				// Token: 0x0400B637 RID: 46647
				public static LocString NAME = "Radiation Mood Ring";

				// Token: 0x0400B638 RID: 46648
				public static LocString DESCRIPTION = "How radioactive are you feeling?";

				// Token: 0x0400B639 RID: 46649
				public static LocString ARTIFACT = "A wholly unique ring not found anywhere outside of the Gravitas Laboratory.\n\nThough it can't be determined for sure who worked on this extraordinary curiousity it's worth noting that, for his Ph.D. thesis, Dr. Travaldo Farrington wrote a paper entitled \"Novelty Uses for Radiochromatic Dyes\".";
			}

			// Token: 0x02002A00 RID: 10752
			public class ORACLE
			{
				// Token: 0x0400B63A RID: 46650
				public static LocString NAME = "Useless Machine";

				// Token: 0x0400B63B RID: 46651
				public static LocString DESCRIPTION = "What does it do?";

				// Token: 0x0400B63C RID: 46652
				public static LocString ARTIFACT = "All of the parts for this contraption are recycled from projects abandoned by the Robotics department.\n\nThe design is very close to one published in an amateur DIY magazine that once sat in the lobby of the 'Employees Only' area of Gravitas' facilities.";
			}

			// Token: 0x02002A01 RID: 10753
			public class GRUBSTATUE
			{
				// Token: 0x0400B63D RID: 46653
				public static LocString NAME = "Grubgrub Statue";

				// Token: 0x0400B63E RID: 46654
				public static LocString DESCRIPTION = "A moving tribute to a tiny plant hugger.";

				// Token: 0x0400B63F RID: 46655
				public static LocString ARTIFACT = "It's very likely this statue was placed in a hidden, secluded place in the Gravitas laboratory since the creation of Grubgrubs was a closely held secret that the general public was not privy to.\n\nThis is a shame since the artistic quality of this statue is really quite accomplished.";
			}

			// Token: 0x02002A02 RID: 10754
			public class HONEYJAR
			{
				// Token: 0x0400B640 RID: 46656
				public static LocString NAME = "Honey Jar";

				// Token: 0x0400B641 RID: 46657
				public static LocString DESCRIPTION = "Sweet golden liquid with just a touch of uranium.";

				// Token: 0x0400B642 RID: 46658
				public static LocString ARTIFACT = "Records from the Genetics and Biology Lab of the Gravitas facility show that several early iterations of a radioactive Bee would continue to produce honey and that this honey was once accidentally stored in the employee kitchen which resulted in several incidents of minor radiation poisoning when it was erroneously labled as a sweetener for tea.\n\nEmployees who used this product reported that it was the \"sweetest honey they'd ever tasted\" and expressed no regret at the mix-up.";
			}

			// Token: 0x02002A03 RID: 10755
			public class PLASTICFLOWERS
			{
				// Token: 0x0400B643 RID: 46659
				public static LocString NAME = "Plastic Flowers";

				// Token: 0x0400B644 RID: 46660
				public static LocString DESCRIPTION = "Maintenance-free blooms that will outlast us all.";

				// Token: 0x0400B645 RID: 46661
				public static LocString ARTIFACT = "Manufactured and sold by a home staging company hired by Gravitas to \"make Space feel more like home.\"\n\nThis bouquet is designed to smell like freshly baked cookies.";
			}

			// Token: 0x02002A04 RID: 10756
			public class FOUNTAINPEN
			{
				// Token: 0x0400B646 RID: 46662
				public static LocString NAME = "Fountain Pen";

				// Token: 0x0400B647 RID: 46663
				public static LocString DESCRIPTION = "It cuts through red tape better than a sword ever could.";

				// Token: 0x0400B648 RID: 46664
				public static LocString ARTIFACT = "The handcrafted gold nib features a triangular logo with the letters V and I inside.\n\nIts owner was too proud to report it stolen, and would be shocked to learn of its whereabouts.";
			}
		}

		// Token: 0x0200214C RID: 8524
		public class KEEPSAKES
		{
			// Token: 0x02002A05 RID: 10757
			public class CRITTER_MANIPULATOR
			{
				// Token: 0x0400B649 RID: 46665
				public static LocString NAME = "Ceramic Morb";

				// Token: 0x0400B64A RID: 46666
				public static LocString DESCRIPTION = "A pottery project produced in an HR-mandated art therapy class.\n\nIt's glazed with a substance that once landed a curious lab technician in the ER.";
			}

			// Token: 0x02002A06 RID: 10758
			public class MEGA_BRAIN
			{
				// Token: 0x0400B64B RID: 46667
				public static LocString NAME = "Model Plane";

				// Token: 0x0400B64C RID: 46668
				public static LocString DESCRIPTION = "A treasured souvenir that was once a common accompaniment to children's meals during commercial flights. There's a hole in the bottom from when Dr. Holland had it mounted on a stand.";
			}

			// Token: 0x02002A07 RID: 10759
			public class LONELY_MINION
			{
				// Token: 0x0400B64D RID: 46669
				public static LocString NAME = "Rusty Toolbox";

				// Token: 0x0400B64E RID: 46670
				public static LocString DESCRIPTION = "On the inside of the lid, someone used a screwdriver to carve a drawing of a group of smiling Duplicants gathered around a massive crater.";
			}

			// Token: 0x02002A08 RID: 10760
			public class FOSSIL_HUNT
			{
				// Token: 0x0400B64F RID: 46671
				public static LocString NAME = "Critter Collar";

				// Token: 0x0400B650 RID: 46672
				public static LocString DESCRIPTION = "The tag reads \"Molly\".\n\nOn the reverse is \"Designed by B363\" stamped above what appears to be an unusually shaped pawprint.";
			}

			// Token: 0x02002A09 RID: 10761
			public class MORB_ROVER_MAKER
			{
				// Token: 0x0400B651 RID: 46673
				public static LocString NAME = "Toy Bot";

				// Token: 0x0400B652 RID: 46674
				public static LocString DESCRIPTION = "A custom-made robot programmed to deliver puns in a variety of celebrity voices.\n\nIt is also a paper shredder.";
			}

			// Token: 0x02002A0A RID: 10762
			public class GEOTHERMAL_PLANT
			{
				// Token: 0x0400B653 RID: 46675
				public static LocString NAME = "Shiny Coprolite";

				// Token: 0x0400B654 RID: 46676
				public static LocString DESCRIPTION = "A spectacular sample of organic material fossilized into lead.\n\nSome things really <i>do</i> get better with age.";
			}
		}

		// Token: 0x0200214D RID: 8525
		public class SANDBOXTOOLS
		{
			// Token: 0x02002A0B RID: 10763
			public class SETTINGS
			{
				// Token: 0x0200366F RID: 13935
				public class INSTANT_BUILD
				{
					// Token: 0x0400D9FB RID: 55803
					public static LocString NAME = "Instant build mode ON";

					// Token: 0x0400D9FC RID: 55804
					public static LocString TOOLTIP = "Toggle between placing construction plans and fully built buildings";
				}

				// Token: 0x02003670 RID: 13936
				public class BRUSH_SIZE
				{
					// Token: 0x0400D9FD RID: 55805
					public static LocString NAME = "Size";

					// Token: 0x0400D9FE RID: 55806
					public static LocString TOOLTIP = "Adjust brush size";
				}

				// Token: 0x02003671 RID: 13937
				public class BRUSH_NOISE_SCALE
				{
					// Token: 0x0400D9FF RID: 55807
					public static LocString NAME = "Noise A";

					// Token: 0x0400DA00 RID: 55808
					public static LocString TOOLTIP = "Adjust brush noisiness A";
				}

				// Token: 0x02003672 RID: 13938
				public class BRUSH_NOISE_DENSITY
				{
					// Token: 0x0400DA01 RID: 55809
					public static LocString NAME = "Noise B";

					// Token: 0x0400DA02 RID: 55810
					public static LocString TOOLTIP = "Adjust brush noisiness B";
				}

				// Token: 0x02003673 RID: 13939
				public class TEMPERATURE
				{
					// Token: 0x0400DA03 RID: 55811
					public static LocString NAME = "Temperature";

					// Token: 0x0400DA04 RID: 55812
					public static LocString TOOLTIP = "Adjust absolute temperature";
				}

				// Token: 0x02003674 RID: 13940
				public class TEMPERATURE_ADDITIVE
				{
					// Token: 0x0400DA05 RID: 55813
					public static LocString NAME = "Temperature";

					// Token: 0x0400DA06 RID: 55814
					public static LocString TOOLTIP = "Adjust additive temperature";
				}

				// Token: 0x02003675 RID: 13941
				public class RADIATION
				{
					// Token: 0x0400DA07 RID: 55815
					public static LocString NAME = "Absolute radiation";

					// Token: 0x0400DA08 RID: 55816
					public static LocString TOOLTIP = "Adjust absolute radiation";
				}

				// Token: 0x02003676 RID: 13942
				public class RADIATION_ADDITIVE
				{
					// Token: 0x0400DA09 RID: 55817
					public static LocString NAME = "Additive radiation";

					// Token: 0x0400DA0A RID: 55818
					public static LocString TOOLTIP = "Adjust additive radiation";
				}

				// Token: 0x02003677 RID: 13943
				public class STRESS_ADDITIVE
				{
					// Token: 0x0400DA0B RID: 55819
					public static LocString NAME = "Reduce Stress";

					// Token: 0x0400DA0C RID: 55820
					public static LocString TOOLTIP = "Adjust stress reduction";
				}

				// Token: 0x02003678 RID: 13944
				public class MORALE
				{
					// Token: 0x0400DA0D RID: 55821
					public static LocString NAME = "Adjust Morale";

					// Token: 0x0400DA0E RID: 55822
					public static LocString TOOLTIP = "Bonus Morale adjustment";
				}

				// Token: 0x02003679 RID: 13945
				public class MASS
				{
					// Token: 0x0400DA0F RID: 55823
					public static LocString NAME = "Mass";

					// Token: 0x0400DA10 RID: 55824
					public static LocString TOOLTIP = "Adjust mass";
				}

				// Token: 0x0200367A RID: 13946
				public class DISEASE
				{
					// Token: 0x0400DA11 RID: 55825
					public static LocString NAME = "Germ";

					// Token: 0x0400DA12 RID: 55826
					public static LocString TOOLTIP = "Adjust type of germ";
				}

				// Token: 0x0200367B RID: 13947
				public class DISEASE_COUNT
				{
					// Token: 0x0400DA13 RID: 55827
					public static LocString NAME = "Germs";

					// Token: 0x0400DA14 RID: 55828
					public static LocString TOOLTIP = "Adjust germ count";
				}

				// Token: 0x0200367C RID: 13948
				public class BRUSH
				{
					// Token: 0x0400DA15 RID: 55829
					public static LocString NAME = "Brush";

					// Token: 0x0400DA16 RID: 55830
					public static LocString TOOLTIP = "Paint elements into the world simulation {Hotkey}";
				}

				// Token: 0x0200367D RID: 13949
				public class ELEMENT
				{
					// Token: 0x0400DA17 RID: 55831
					public static LocString NAME = "Element";

					// Token: 0x0400DA18 RID: 55832
					public static LocString TOOLTIP = "Adjust type of element";
				}

				// Token: 0x0200367E RID: 13950
				public class SPRINKLE
				{
					// Token: 0x0400DA19 RID: 55833
					public static LocString NAME = "Sprinkle";

					// Token: 0x0400DA1A RID: 55834
					public static LocString TOOLTIP = "Paint elements into the simulation using noise {Hotkey}";
				}

				// Token: 0x0200367F RID: 13951
				public class FLOOD
				{
					// Token: 0x0400DA1B RID: 55835
					public static LocString NAME = "Fill";

					// Token: 0x0400DA1C RID: 55836
					public static LocString TOOLTIP = "Fill a section of the simulation with the chosen element {Hotkey}";
				}

				// Token: 0x02003680 RID: 13952
				public class SAMPLE
				{
					// Token: 0x0400DA1D RID: 55837
					public static LocString NAME = "Sample";

					// Token: 0x0400DA1E RID: 55838
					public static LocString TOOLTIP = "Copy the settings from a cell to use with brush tools {Hotkey}";
				}

				// Token: 0x02003681 RID: 13953
				public class HEATGUN
				{
					// Token: 0x0400DA1F RID: 55839
					public static LocString NAME = "Heat Gun";

					// Token: 0x0400DA20 RID: 55840
					public static LocString TOOLTIP = "Inject thermal energy into the simulation {Hotkey}";
				}

				// Token: 0x02003682 RID: 13954
				public class RADSTOOL
				{
					// Token: 0x0400DA21 RID: 55841
					public static LocString NAME = "Radiation Tool";

					// Token: 0x0400DA22 RID: 55842
					public static LocString TOOLTIP = "Inject or remove radiation from the simulation {Hotkey}";
				}

				// Token: 0x02003683 RID: 13955
				public class SPAWNER
				{
					// Token: 0x0400DA23 RID: 55843
					public static LocString NAME = "Spawner";

					// Token: 0x0400DA24 RID: 55844
					public static LocString TOOLTIP = "Spawn critters, food, equipment, and other entities {Hotkey}";
				}

				// Token: 0x02003684 RID: 13956
				public class STRESS
				{
					// Token: 0x0400DA25 RID: 55845
					public static LocString NAME = "Stress";

					// Token: 0x0400DA26 RID: 55846
					public static LocString TOOLTIP = "Manage Duplicants' stress levels {Hotkey}";
				}

				// Token: 0x02003685 RID: 13957
				public class CLEAR_FLOOR
				{
					// Token: 0x0400DA27 RID: 55847
					public static LocString NAME = "Clear Debris";

					// Token: 0x0400DA28 RID: 55848
					public static LocString TOOLTIP = "Delete loose items cluttering the floor {Hotkey}";
				}

				// Token: 0x02003686 RID: 13958
				public class DESTROY
				{
					// Token: 0x0400DA29 RID: 55849
					public static LocString NAME = "Destroy";

					// Token: 0x0400DA2A RID: 55850
					public static LocString TOOLTIP = "Delete everything in the selected cell(s) {Hotkey}";
				}

				// Token: 0x02003687 RID: 13959
				public class SPAWN_ENTITY
				{
					// Token: 0x0400DA2B RID: 55851
					public static LocString NAME = "Spawn";
				}

				// Token: 0x02003688 RID: 13960
				public class FOW
				{
					// Token: 0x0400DA2C RID: 55852
					public static LocString NAME = "Reveal";

					// Token: 0x0400DA2D RID: 55853
					public static LocString TOOLTIP = "Dispel the Fog of War shrouding the map {Hotkey}";
				}

				// Token: 0x02003689 RID: 13961
				public class CRITTER
				{
					// Token: 0x0400DA2E RID: 55854
					public static LocString NAME = "Critter Removal";

					// Token: 0x0400DA2F RID: 55855
					public static LocString TOOLTIP = "Remove critters! {Hotkey}";
				}

				// Token: 0x0200368A RID: 13962
				public class SPAWN_STORY_TRAIT
				{
					// Token: 0x0400DA30 RID: 55856
					public static LocString NAME = "Story Traits";

					// Token: 0x0400DA31 RID: 55857
					public static LocString TOOLTIP = "Spawn story traits {Hotkey}";
				}
			}

			// Token: 0x02002A0C RID: 10764
			public class FILTERS
			{
				// Token: 0x0400B655 RID: 46677
				public static LocString BACK = "Back";

				// Token: 0x0400B656 RID: 46678
				public static LocString COMMON = "Common Substances";

				// Token: 0x0400B657 RID: 46679
				public static LocString SOLID = "Solids";

				// Token: 0x0400B658 RID: 46680
				public static LocString LIQUID = "Liquids";

				// Token: 0x0400B659 RID: 46681
				public static LocString GAS = "Gases";

				// Token: 0x0200368B RID: 13963
				public class ENTITIES
				{
					// Token: 0x0400DA32 RID: 55858
					public static LocString BIONICUPGRADES = "Boosters";

					// Token: 0x0400DA33 RID: 55859
					public static LocString SPECIAL = "Special";

					// Token: 0x0400DA34 RID: 55860
					public static LocString GRAVITAS = "Gravitas";

					// Token: 0x0400DA35 RID: 55861
					public static LocString PLANTS = "Plants";

					// Token: 0x0400DA36 RID: 55862
					public static LocString SEEDS = "Seeds";

					// Token: 0x0400DA37 RID: 55863
					public static LocString CREATURE = "Critters";

					// Token: 0x0400DA38 RID: 55864
					public static LocString CREATURE_EGG = "Eggs";

					// Token: 0x0400DA39 RID: 55865
					public static LocString FOOD = "Foods";

					// Token: 0x0400DA3A RID: 55866
					public static LocString EQUIPMENT = "Equipment";

					// Token: 0x0400DA3B RID: 55867
					public static LocString GEYSERS = "Geysers";

					// Token: 0x0400DA3C RID: 55868
					public static LocString EXPERIMENTS = "Experimental";

					// Token: 0x0400DA3D RID: 55869
					public static LocString INDUSTRIAL_PRODUCTS = "Industrial";

					// Token: 0x0400DA3E RID: 55870
					public static LocString COMETS = "Meteors";

					// Token: 0x0400DA3F RID: 55871
					public static LocString ARTIFACTS = "Artifacts";

					// Token: 0x0400DA40 RID: 55872
					public static LocString STORYTRAITS = "Story Traits";
				}
			}

			// Token: 0x02002A0D RID: 10765
			public class CLEARFLOOR
			{
				// Token: 0x0400B65A RID: 46682
				public static LocString DELETED = "Deleted";
			}
		}

		// Token: 0x0200214E RID: 8526
		public class RETIRED_COLONY_INFO_SCREEN
		{
			// Token: 0x04009529 RID: 38185
			public static LocString SECONDS = "Seconds";

			// Token: 0x0400952A RID: 38186
			public static LocString CYCLES = "Cycles";

			// Token: 0x0400952B RID: 38187
			public static LocString CYCLE_COUNT = "Cycle Count: {0}";

			// Token: 0x0400952C RID: 38188
			public static LocString DUPLICANT_AGE = "Age: {0} cycles";

			// Token: 0x0400952D RID: 38189
			public static LocString SKILL_LEVEL = "Skill Level: {0}";

			// Token: 0x0400952E RID: 38190
			public static LocString BUILDING_COUNT = "Count: {0}";

			// Token: 0x0400952F RID: 38191
			public static LocString PREVIEW_UNAVAILABLE = "Preview\nUnavailable";

			// Token: 0x04009530 RID: 38192
			public static LocString TIMELAPSE_UNAVAILABLE = "Timelapse\nUnavailable";

			// Token: 0x04009531 RID: 38193
			public static LocString SEARCH = "SEARCH...";

			// Token: 0x02002A0E RID: 10766
			public class BUTTONS
			{
				// Token: 0x0400B65B RID: 46683
				public static LocString RETURN_TO_GAME = "RETURN TO GAME";

				// Token: 0x0400B65C RID: 46684
				public static LocString VIEW_OTHER_COLONIES = "BACK";

				// Token: 0x0400B65D RID: 46685
				public static LocString QUIT_TO_MENU = "QUIT TO MAIN MENU";

				// Token: 0x0400B65E RID: 46686
				public static LocString CLOSE = "CLOSE";
			}

			// Token: 0x02002A0F RID: 10767
			public class TITLES
			{
				// Token: 0x0400B65F RID: 46687
				public static LocString EXPLORER_HEADER = "COLONIES";

				// Token: 0x0400B660 RID: 46688
				public static LocString RETIRED_COLONIES = "Colony Summaries";

				// Token: 0x0400B661 RID: 46689
				public static LocString COLONY_STATISTICS = "Colony Statistics";

				// Token: 0x0400B662 RID: 46690
				public static LocString DUPLICANTS = "Duplicants";

				// Token: 0x0400B663 RID: 46691
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400B664 RID: 46692
				public static LocString CHEEVOS = "Colony Achievements";

				// Token: 0x0400B665 RID: 46693
				public static LocString ACHIEVEMENT_HEADER = "ACHIEVEMENTS";

				// Token: 0x0400B666 RID: 46694
				public static LocString TIMELAPSE = "Timelapse";
			}

			// Token: 0x02002A10 RID: 10768
			public class STATS
			{
				// Token: 0x0400B667 RID: 46695
				public static LocString OXYGEN_CREATED = "Total Oxygen Produced";

				// Token: 0x0400B668 RID: 46696
				public static LocString OXYGEN_CONSUMED = "Total Oxygen Consumed";

				// Token: 0x0400B669 RID: 46697
				public static LocString POWER_CREATED = "Average Power Produced";

				// Token: 0x0400B66A RID: 46698
				public static LocString POWER_WASTED = "Average Power Wasted";

				// Token: 0x0400B66B RID: 46699
				public static LocString TRAVEL_TIME = "Total Travel Time";

				// Token: 0x0400B66C RID: 46700
				public static LocString WORK_TIME = "Total Work Time";

				// Token: 0x0400B66D RID: 46701
				public static LocString AVERAGE_TRAVEL_TIME = "Average Travel Time";

				// Token: 0x0400B66E RID: 46702
				public static LocString AVERAGE_WORK_TIME = "Average Work Time";

				// Token: 0x0400B66F RID: 46703
				public static LocString CALORIES_CREATED = "Calorie Generation";

				// Token: 0x0400B670 RID: 46704
				public static LocString CALORIES_CONSUMED = "Calorie Consumption";

				// Token: 0x0400B671 RID: 46705
				public static LocString LIVE_DUPLICANTS = "Duplicants";

				// Token: 0x0400B672 RID: 46706
				public static LocString AVERAGE_STRESS_CREATED = "Average Stress Created";

				// Token: 0x0400B673 RID: 46707
				public static LocString AVERAGE_STRESS_REMOVED = "Average Stress Removed";

				// Token: 0x0400B674 RID: 46708
				public static LocString NUMBER_DOMESTICATED_CRITTERS = "Domesticated Critters";

				// Token: 0x0400B675 RID: 46709
				public static LocString NUMBER_WILD_CRITTERS = "Wild Critters";

				// Token: 0x0400B676 RID: 46710
				public static LocString AVERAGE_GERMS = "Average Germs";

				// Token: 0x0400B677 RID: 46711
				public static LocString ROCKET_MISSIONS = "Rocket Missions Underway";
			}
		}

		// Token: 0x0200214F RID: 8527
		public class DROPDOWN
		{
			// Token: 0x04009532 RID: 38194
			public static LocString NONE = "Unassigned";
		}

		// Token: 0x02002150 RID: 8528
		public class FRONTEND
		{
			// Token: 0x04009533 RID: 38195
			public static LocString GAME_VERSION = "Game Version: ";

			// Token: 0x04009534 RID: 38196
			public static LocString LOADING = "Loading...";

			// Token: 0x04009535 RID: 38197
			public static LocString DONE_BUTTON = "DONE";

			// Token: 0x02002A11 RID: 10769
			public class DEMO_OVER_SCREEN
			{
				// Token: 0x0400B678 RID: 46712
				public static LocString TITLE = "Thanks for playing!";

				// Token: 0x0400B679 RID: 46713
				public static LocString BODY = "Thank you for playing the demo for Oxygen Not Included!\n\nThis game is still in development.\n\nGo to kleigames.com/o2 or ask one of us if you'd like more information.";

				// Token: 0x0400B67A RID: 46714
				public static LocString BUTTON_EXIT_TO_MENU = "EXIT TO MENU";
			}

			// Token: 0x02002A12 RID: 10770
			public class CUSTOMGAMESETTINGSSCREEN
			{
				// Token: 0x0200368C RID: 13964
				public class SETTINGS
				{
					// Token: 0x02003AE6 RID: 15078
					public class SANDBOXMODE
					{
						// Token: 0x0400E4C1 RID: 58561
						public static LocString NAME = "Sandbox Mode";

						// Token: 0x0400E4C2 RID: 58562
						public static LocString TOOLTIP = "Manipulate and customize the simulation with tools that ignore regular game constraints";

						// Token: 0x02003B69 RID: 15209
						public static class LEVELS
						{
							// Token: 0x02003B82 RID: 15234
							public static class DISABLED
							{
								// Token: 0x0400E60F RID: 58895
								public static LocString NAME = "Disabled";

								// Token: 0x0400E610 RID: 58896
								public static LocString TOOLTIP = "Unchecked: Sandbox Mode is turned off (Default)";
							}

							// Token: 0x02003B83 RID: 15235
							public static class ENABLED
							{
								// Token: 0x0400E611 RID: 58897
								public static LocString NAME = "Enabled";

								// Token: 0x0400E612 RID: 58898
								public static LocString TOOLTIP = "Checked: Sandbox Mode is turned on";
							}
						}
					}

					// Token: 0x02003AE7 RID: 15079
					public class FASTWORKERSMODE
					{
						// Token: 0x0400E4C3 RID: 58563
						public static LocString NAME = "Fast Workers Mode";

						// Token: 0x0400E4C4 RID: 58564
						public static LocString TOOLTIP = "Duplicants will finish most work immediately and require little sleep";

						// Token: 0x02003B6A RID: 15210
						public static class LEVELS
						{
							// Token: 0x02003B84 RID: 15236
							public static class DISABLED
							{
								// Token: 0x0400E613 RID: 58899
								public static LocString NAME = "Disabled";

								// Token: 0x0400E614 RID: 58900
								public static LocString TOOLTIP = "Unchecked: Fast Workers Mode is turned off (Default)";
							}

							// Token: 0x02003B85 RID: 15237
							public static class ENABLED
							{
								// Token: 0x0400E615 RID: 58901
								public static LocString NAME = "Enabled";

								// Token: 0x0400E616 RID: 58902
								public static LocString TOOLTIP = "Checked: Fast Workers Mode is turned on";
							}
						}
					}

					// Token: 0x02003AE8 RID: 15080
					public class EXPANSION1ACTIVE
					{
						// Token: 0x0400E4C5 RID: 58565
						public static LocString NAME = UI.DLC1.NAME_ITAL + " Content Enabled";

						// Token: 0x0400E4C6 RID: 58566
						public static LocString TOOLTIP = "If checked, content from the " + UI.DLC1.NAME_ITAL + " Expansion will be available";

						// Token: 0x02003B6B RID: 15211
						public static class LEVELS
						{
							// Token: 0x02003B86 RID: 15238
							public static class DISABLED
							{
								// Token: 0x0400E617 RID: 58903
								public static LocString NAME = "Disabled";

								// Token: 0x0400E618 RID: 58904
								public static LocString TOOLTIP = "Unchecked: " + UI.DLC1.NAME_ITAL + " Content is turned off (Default)";
							}

							// Token: 0x02003B87 RID: 15239
							public static class ENABLED
							{
								// Token: 0x0400E619 RID: 58905
								public static LocString NAME = "Enabled";

								// Token: 0x0400E61A RID: 58906
								public static LocString TOOLTIP = "Checked: " + UI.DLC1.NAME_ITAL + " Content is turned on";
							}
						}
					}

					// Token: 0x02003AE9 RID: 15081
					public class SAVETOCLOUD
					{
						// Token: 0x0400E4C7 RID: 58567
						public static LocString NAME = "Save To Cloud";

						// Token: 0x0400E4C8 RID: 58568
						public static LocString TOOLTIP = "This colony will be created in the cloud saves folder, and synced by the game platform.";

						// Token: 0x0400E4C9 RID: 58569
						public static LocString TOOLTIP_LOCAL = "This colony will be created in the local saves folder. It will not be a cloud save and will not be synced by the game platform.";

						// Token: 0x0400E4CA RID: 58570
						public static LocString TOOLTIP_EXTRA = "This can be changed later with the colony management options in the load screen, from the main menu.";

						// Token: 0x02003B6C RID: 15212
						public static class LEVELS
						{
							// Token: 0x02003B88 RID: 15240
							public static class DISABLED
							{
								// Token: 0x0400E61B RID: 58907
								public static LocString NAME = "Disabled";

								// Token: 0x0400E61C RID: 58908
								public static LocString TOOLTIP = "Unchecked: This colony will be a local save";
							}

							// Token: 0x02003B89 RID: 15241
							public static class ENABLED
							{
								// Token: 0x0400E61D RID: 58909
								public static LocString NAME = "Enabled";

								// Token: 0x0400E61E RID: 58910
								public static LocString TOOLTIP = "Checked: This colony will be a cloud save (Default)";
							}
						}
					}

					// Token: 0x02003AEA RID: 15082
					public class CAREPACKAGES
					{
						// Token: 0x0400E4CB RID: 58571
						public static LocString NAME = "Care Packages";

						// Token: 0x0400E4CC RID: 58572
						public static LocString TOOLTIP = "Affects what resources can be printed from the Printing Pod";

						// Token: 0x02003B6D RID: 15213
						public static class LEVELS
						{
							// Token: 0x02003B8A RID: 15242
							public static class NORMAL
							{
								// Token: 0x0400E61F RID: 58911
								public static LocString NAME = "All";

								// Token: 0x0400E620 RID: 58912
								public static LocString TOOLTIP = "Checked: The Printing Pod will offer both Duplicant blueprints and care packages (Default)";
							}

							// Token: 0x02003B8B RID: 15243
							public static class DUPLICANTS_ONLY
							{
								// Token: 0x0400E621 RID: 58913
								public static LocString NAME = "Duplicants Only";

								// Token: 0x0400E622 RID: 58914
								public static LocString TOOLTIP = "Unchecked: The Printing Pod will only offer Duplicant blueprints";
							}
						}
					}

					// Token: 0x02003AEB RID: 15083
					public class IMMUNESYSTEM
					{
						// Token: 0x0400E4CD RID: 58573
						public static LocString NAME = "Disease";

						// Token: 0x0400E4CE RID: 58574
						public static LocString TOOLTIP = "Affects Duplicants' chances of contracting a disease after germ exposure";

						// Token: 0x02003B6E RID: 15214
						public static class LEVELS
						{
							// Token: 0x02003B8C RID: 15244
							public static class COMPROMISED
							{
								// Token: 0x0400E623 RID: 58915
								public static LocString NAME = "Outbreak Prone";

								// Token: 0x0400E624 RID: 58916
								public static LocString TOOLTIP = "The whole colony will be ravaged by plague if a Duplicant so much as sneezes funny";

								// Token: 0x0400E625 RID: 58917
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Outbreak Prone (Highest Difficulty)";
							}

							// Token: 0x02003B8D RID: 15245
							public static class WEAK
							{
								// Token: 0x0400E626 RID: 58918
								public static LocString NAME = "Germ Susceptible";

								// Token: 0x0400E627 RID: 58919
								public static LocString TOOLTIP = "These Duplicants have an increased chance of contracting diseases from germ exposure";

								// Token: 0x0400E628 RID: 58920
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Germ Susceptibility (Difficulty Up)";
							}

							// Token: 0x02003B8E RID: 15246
							public static class DEFAULT
							{
								// Token: 0x0400E629 RID: 58921
								public static LocString NAME = "Default";

								// Token: 0x0400E62A RID: 58922
								public static LocString TOOLTIP = "Default disease chance";
							}

							// Token: 0x02003B8F RID: 15247
							public static class STRONG
							{
								// Token: 0x0400E62B RID: 58923
								public static LocString NAME = "Germ Resistant";

								// Token: 0x0400E62C RID: 58924
								public static LocString TOOLTIP = "These Duplicants have a decreased chance of contracting diseases from germ exposure";

								// Token: 0x0400E62D RID: 58925
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Germ Resistance (Difficulty Down)";
							}

							// Token: 0x02003B90 RID: 15248
							public static class INVINCIBLE
							{
								// Token: 0x0400E62E RID: 58926
								public static LocString NAME = "Total Immunity";

								// Token: 0x0400E62F RID: 58927
								public static LocString TOOLTIP = "Like diplomatic immunity, but without the diplomacy. These Duplicants will never get sick";

								// Token: 0x0400E630 RID: 58928
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Total Immunity (No Disease)";
							}
						}
					}

					// Token: 0x02003AEC RID: 15084
					public class MORALE
					{
						// Token: 0x0400E4CF RID: 58575
						public static LocString NAME = "Morale";

						// Token: 0x0400E4D0 RID: 58576
						public static LocString TOOLTIP = "Adjusts the minimum morale Duplicants must maintain to avoid gaining stress";

						// Token: 0x02003B6F RID: 15215
						public static class LEVELS
						{
							// Token: 0x02003B91 RID: 15249
							public static class VERYHARD
							{
								// Token: 0x0400E631 RID: 58929
								public static LocString NAME = "Draconian";

								// Token: 0x0400E632 RID: 58930
								public static LocString TOOLTIP = "The finest of the finest can barely keep up with these Duplicants' stringent demands";

								// Token: 0x0400E633 RID: 58931
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Draconian (Highest Difficulty)";
							}

							// Token: 0x02003B92 RID: 15250
							public static class HARD
							{
								// Token: 0x0400E634 RID: 58932
								public static LocString NAME = "A Bit Persnickety";

								// Token: 0x0400E635 RID: 58933
								public static LocString TOOLTIP = "Duplicants require higher morale than usual to fend off stress";

								// Token: 0x0400E636 RID: 58934
								public static LocString ATTRIBUTE_MODIFIER_NAME = "A Bit Persnickety (Difficulty Up)";
							}

							// Token: 0x02003B93 RID: 15251
							public static class DEFAULT
							{
								// Token: 0x0400E637 RID: 58935
								public static LocString NAME = "Default";

								// Token: 0x0400E638 RID: 58936
								public static LocString TOOLTIP = "Default morale needs";
							}

							// Token: 0x02003B94 RID: 15252
							public static class EASY
							{
								// Token: 0x0400E639 RID: 58937
								public static LocString NAME = "Chill";

								// Token: 0x0400E63A RID: 58938
								public static LocString TOOLTIP = "Duplicants require lower morale than usual to fend off stress";

								// Token: 0x0400E63B RID: 58939
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Chill (Difficulty Down)";
							}

							// Token: 0x02003B95 RID: 15253
							public static class DISABLED
							{
								// Token: 0x0400E63C RID: 58940
								public static LocString NAME = "Totally Blasé";

								// Token: 0x0400E63D RID: 58941
								public static LocString TOOLTIP = "These Duplicants have zero standards and will never gain stress, regardless of their morale";

								// Token: 0x0400E63E RID: 58942
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Totally Blasé (No Morale)";
							}
						}
					}

					// Token: 0x02003AED RID: 15085
					public class CALORIE_BURN
					{
						// Token: 0x0400E4D1 RID: 58577
						public static LocString NAME = "Hunger";

						// Token: 0x0400E4D2 RID: 58578
						public static LocString TOOLTIP = "Affects how quickly Duplicants burn calories and become hungry";

						// Token: 0x02003B70 RID: 15216
						public static class LEVELS
						{
							// Token: 0x02003B96 RID: 15254
							public static class VERYHARD
							{
								// Token: 0x0400E63F RID: 58943
								public static LocString NAME = "Ravenous";

								// Token: 0x0400E640 RID: 58944
								public static LocString TOOLTIP = "Your Duplicants are on a see-food diet... They see food and they eat it";

								// Token: 0x0400E641 RID: 58945
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Ravenous (Highest Difficulty)";
							}

							// Token: 0x02003B97 RID: 15255
							public static class HARD
							{
								// Token: 0x0400E642 RID: 58946
								public static LocString NAME = "Rumbly Tummies";

								// Token: 0x0400E643 RID: 58947
								public static LocString TOOLTIP = "Duplicants burn calories quickly and require more feeding than usual";

								// Token: 0x0400E644 RID: 58948
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Rumbly Tummies (Difficulty Up)";
							}

							// Token: 0x02003B98 RID: 15256
							public static class DEFAULT
							{
								// Token: 0x0400E645 RID: 58949
								public static LocString NAME = "Default";

								// Token: 0x0400E646 RID: 58950
								public static LocString TOOLTIP = "Default calorie burn rate";
							}

							// Token: 0x02003B99 RID: 15257
							public static class EASY
							{
								// Token: 0x0400E647 RID: 58951
								public static LocString NAME = "Fasting";

								// Token: 0x0400E648 RID: 58952
								public static LocString TOOLTIP = "Duplicants burn calories slowly and get by with fewer meals";

								// Token: 0x0400E649 RID: 58953
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Fasting (Difficulty Down)";
							}

							// Token: 0x02003B9A RID: 15258
							public static class DISABLED
							{
								// Token: 0x0400E64A RID: 58954
								public static LocString NAME = "Tummyless";

								// Token: 0x0400E64B RID: 58955
								public static LocString TOOLTIP = "These Duplicants were printed without tummies and need no food at all";

								// Token: 0x0400E64C RID: 58956
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Tummyless (No Hunger)";
							}
						}
					}

					// Token: 0x02003AEE RID: 15086
					public class WORLD_CHOICE
					{
						// Token: 0x0400E4D3 RID: 58579
						public static LocString NAME = "World";

						// Token: 0x0400E4D4 RID: 58580
						public static LocString TOOLTIP = "New worlds added by mods can be selected here";
					}

					// Token: 0x02003AEF RID: 15087
					public class CLUSTER_CHOICE
					{
						// Token: 0x0400E4D5 RID: 58581
						public static LocString NAME = "Asteroid Belt";

						// Token: 0x0400E4D6 RID: 58582
						public static LocString TOOLTIP = "New asteroid belts added by mods can be selected here";
					}

					// Token: 0x02003AF0 RID: 15088
					public class STORY_TRAIT_COUNT
					{
						// Token: 0x0400E4D7 RID: 58583
						public static LocString NAME = "Story Traits";

						// Token: 0x0400E4D8 RID: 58584
						public static LocString TOOLTIP = "Determines the number of story traits spawned";

						// Token: 0x02003B71 RID: 15217
						public static class LEVELS
						{
							// Token: 0x02003B9B RID: 15259
							public static class NONE
							{
								// Token: 0x0400E64D RID: 58957
								public static LocString NAME = "Zilch";

								// Token: 0x0400E64E RID: 58958
								public static LocString TOOLTIP = "Zero story traits. Zip. Nada. None";
							}

							// Token: 0x02003B9C RID: 15260
							public static class FEW
							{
								// Token: 0x0400E64F RID: 58959
								public static LocString NAME = "Stingy";

								// Token: 0x0400E650 RID: 58960
								public static LocString TOOLTIP = "Not zero, but not a lot";
							}

							// Token: 0x02003B9D RID: 15261
							public static class LOTS
							{
								// Token: 0x0400E651 RID: 58961
								public static LocString NAME = "Oodles";

								// Token: 0x0400E652 RID: 58962
								public static LocString TOOLTIP = "Plenty of story traits to go around";
							}
						}
					}

					// Token: 0x02003AF1 RID: 15089
					public class DURABILITY
					{
						// Token: 0x0400E4D9 RID: 58585
						public static LocString NAME = "Durability";

						// Token: 0x0400E4DA RID: 58586
						public static LocString TOOLTIP = "Affects how quickly equippable suits wear out";

						// Token: 0x02003B72 RID: 15218
						public static class LEVELS
						{
							// Token: 0x02003B9E RID: 15262
							public static class INDESTRUCTIBLE
							{
								// Token: 0x0400E653 RID: 58963
								public static LocString NAME = "Indestructible";

								// Token: 0x0400E654 RID: 58964
								public static LocString TOOLTIP = "Duplicants have perfected clothes manufacturing and are able to make suits that last forever";

								// Token: 0x0400E655 RID: 58965
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Indestructible Suits (No Durability)";
							}

							// Token: 0x02003B9F RID: 15263
							public static class REINFORCED
							{
								// Token: 0x0400E656 RID: 58966
								public static LocString NAME = "Reinforced";

								// Token: 0x0400E657 RID: 58967
								public static LocString TOOLTIP = "Suits are more durable than usual";

								// Token: 0x0400E658 RID: 58968
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Reinforced Suits (Difficulty Down)";
							}

							// Token: 0x02003BA0 RID: 15264
							public static class DEFAULT
							{
								// Token: 0x0400E659 RID: 58969
								public static LocString NAME = "Default";

								// Token: 0x0400E65A RID: 58970
								public static LocString TOOLTIP = "Default suit durability";
							}

							// Token: 0x02003BA1 RID: 15265
							public static class FLIMSY
							{
								// Token: 0x0400E65B RID: 58971
								public static LocString NAME = "Flimsy";

								// Token: 0x0400E65C RID: 58972
								public static LocString TOOLTIP = "Suits wear out faster than usual";

								// Token: 0x0400E65D RID: 58973
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Flimsy Suits (Difficulty Up)";
							}

							// Token: 0x02003BA2 RID: 15266
							public static class THREADBARE
							{
								// Token: 0x0400E65E RID: 58974
								public static LocString NAME = "Threadbare";

								// Token: 0x0400E65F RID: 58975
								public static LocString TOOLTIP = "These Duplicants are no tailors - suits wear out much faster than usual";

								// Token: 0x0400E660 RID: 58976
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Threadbare Suits (Highest Difficulty)";
							}
						}
					}

					// Token: 0x02003AF2 RID: 15090
					public class RADIATION
					{
						// Token: 0x0400E4DB RID: 58587
						public static LocString NAME = "Radiation";

						// Token: 0x0400E4DC RID: 58588
						public static LocString TOOLTIP = "Affects how susceptible Duplicants are to radiation sickness";

						// Token: 0x02003B73 RID: 15219
						public static class LEVELS
						{
							// Token: 0x02003BA3 RID: 15267
							public static class HARDEST
							{
								// Token: 0x0400E661 RID: 58977
								public static LocString NAME = "Critical Mass";

								// Token: 0x0400E662 RID: 58978
								public static LocString TOOLTIP = "Duplicants feel ill at the merest mention of radiation...and may never truly recover";

								// Token: 0x0400E663 RID: 58979
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Super Radiation (Highest Difficulty)";
							}

							// Token: 0x02003BA4 RID: 15268
							public static class HARDER
							{
								// Token: 0x0400E664 RID: 58980
								public static LocString NAME = "Toxic Positivity";

								// Token: 0x0400E665 RID: 58981
								public static LocString TOOLTIP = "Duplicants are more sensitive to radiation exposure than usual";

								// Token: 0x0400E666 RID: 58982
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Vulnerable (Difficulty Up)";
							}

							// Token: 0x02003BA5 RID: 15269
							public static class DEFAULT
							{
								// Token: 0x0400E667 RID: 58983
								public static LocString NAME = "Default";

								// Token: 0x0400E668 RID: 58984
								public static LocString TOOLTIP = "Default radiation settings";
							}

							// Token: 0x02003BA6 RID: 15270
							public static class EASIER
							{
								// Token: 0x0400E669 RID: 58985
								public static LocString NAME = "Healthy Glow";

								// Token: 0x0400E66A RID: 58986
								public static LocString TOOLTIP = "Duplicants are more resistant to radiation exposure than usual";

								// Token: 0x0400E66B RID: 58987
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Shielded (Difficulty Down)";
							}

							// Token: 0x02003BA7 RID: 15271
							public static class EASIEST
							{
								// Token: 0x0400E66C RID: 58988
								public static LocString NAME = "Nuke-Proof";

								// Token: 0x0400E66D RID: 58989
								public static LocString TOOLTIP = "Duplicants could bathe in radioactive waste and not even notice";

								// Token: 0x0400E66E RID: 58990
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Radiation Protection (Lowest Difficulty)";
							}
						}
					}

					// Token: 0x02003AF3 RID: 15091
					public class STRESS
					{
						// Token: 0x0400E4DD RID: 58589
						public static LocString NAME = "Stress";

						// Token: 0x0400E4DE RID: 58590
						public static LocString TOOLTIP = "Affects how quickly Duplicant stress rises";

						// Token: 0x02003B74 RID: 15220
						public static class LEVELS
						{
							// Token: 0x02003BA8 RID: 15272
							public static class INDOMITABLE
							{
								// Token: 0x0400E66F RID: 58991
								public static LocString NAME = "Cloud Nine";

								// Token: 0x0400E670 RID: 58992
								public static LocString TOOLTIP = "A strong emotional support system makes these Duplicants impervious to all stress";

								// Token: 0x0400E671 RID: 58993
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Cloud Nine (No Stress)";
							}

							// Token: 0x02003BA9 RID: 15273
							public static class OPTIMISTIC
							{
								// Token: 0x0400E672 RID: 58994
								public static LocString NAME = "Chipper";

								// Token: 0x0400E673 RID: 58995
								public static LocString TOOLTIP = "Duplicants gain stress slower than usual";

								// Token: 0x0400E674 RID: 58996
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Chipper (Difficulty Down)";
							}

							// Token: 0x02003BAA RID: 15274
							public static class DEFAULT
							{
								// Token: 0x0400E675 RID: 58997
								public static LocString NAME = "Default";

								// Token: 0x0400E676 RID: 58998
								public static LocString TOOLTIP = "Default stress change rate";
							}

							// Token: 0x02003BAB RID: 15275
							public static class PESSIMISTIC
							{
								// Token: 0x0400E677 RID: 58999
								public static LocString NAME = "Glum";

								// Token: 0x0400E678 RID: 59000
								public static LocString TOOLTIP = "Duplicants gain stress more quickly than usual";

								// Token: 0x0400E679 RID: 59001
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Glum (Difficulty Up)";
							}

							// Token: 0x02003BAC RID: 15276
							public static class DOOMED
							{
								// Token: 0x0400E67A RID: 59002
								public static LocString NAME = "Frankly Depressing";

								// Token: 0x0400E67B RID: 59003
								public static LocString TOOLTIP = "These Duplicants were never taught coping mechanisms... they're devastated by stress as a result";

								// Token: 0x0400E67C RID: 59004
								public static LocString ATTRIBUTE_MODIFIER_NAME = "Frankly Depressing (Highest Difficulty)";
							}
						}
					}

					// Token: 0x02003AF4 RID: 15092
					public class STRESS_BREAKS
					{
						// Token: 0x0400E4DF RID: 58591
						public static LocString NAME = "Stress Reactions";

						// Token: 0x0400E4E0 RID: 58592
						public static LocString TOOLTIP = "Determines whether Duplicants wreak havoc on the colony when they reach maximum stress";

						// Token: 0x02003B75 RID: 15221
						public static class LEVELS
						{
							// Token: 0x02003BAD RID: 15277
							public static class DEFAULT
							{
								// Token: 0x0400E67D RID: 59005
								public static LocString NAME = "Enabled";

								// Token: 0x0400E67E RID: 59006
								public static LocString TOOLTIP = "Checked: Duplicants will wreak havoc when they reach 100% stress (Default)";
							}

							// Token: 0x02003BAE RID: 15278
							public static class DISABLED
							{
								// Token: 0x0400E67F RID: 59007
								public static LocString NAME = "Disabled";

								// Token: 0x0400E680 RID: 59008
								public static LocString TOOLTIP = "Unchecked: Duplicants will not wreak havoc at maximum stress";
							}
						}
					}

					// Token: 0x02003AF5 RID: 15093
					public class WORLDGEN_SEED
					{
						// Token: 0x0400E4E1 RID: 58593
						public static LocString NAME = "Worldgen Seed";

						// Token: 0x0400E4E2 RID: 58594
						public static LocString TOOLTIP = "This number chooses the procedural parameters that create your unique map\n\nWorldgen seeds can be copied and pasted so others can play a replica of your world configuration";

						// Token: 0x0400E4E3 RID: 58595
						public static LocString FIXEDSEED = "This is a predetermined seed, and cannot be changed";
					}

					// Token: 0x02003AF6 RID: 15094
					public class TELEPORTERS
					{
						// Token: 0x0400E4E4 RID: 58596
						public static LocString NAME = "Teleporters";

						// Token: 0x0400E4E5 RID: 58597
						public static LocString TOOLTIP = "Determines whether teleporters will be spawned during Worldgen";

						// Token: 0x02003B76 RID: 15222
						public static class LEVELS
						{
							// Token: 0x02003BAF RID: 15279
							public static class ENABLED
							{
								// Token: 0x0400E681 RID: 59009
								public static LocString NAME = "Enabled";

								// Token: 0x0400E682 RID: 59010
								public static LocString TOOLTIP = "Checked: Teleporters will spawn during Worldgen (Default)";
							}

							// Token: 0x02003BB0 RID: 15280
							public static class DISABLED
							{
								// Token: 0x0400E683 RID: 59011
								public static LocString NAME = "Disabled";

								// Token: 0x0400E684 RID: 59012
								public static LocString TOOLTIP = "Unchecked: No Teleporters will spawn during Worldgen";
							}
						}
					}

					// Token: 0x02003AF7 RID: 15095
					public class METEORSHOWERS
					{
						// Token: 0x0400E4E6 RID: 58598
						public static LocString NAME = "Meteor Showers";

						// Token: 0x0400E4E7 RID: 58599
						public static LocString TOOLTIP = "Adjusts the intensity of incoming space rocks";

						// Token: 0x02003B77 RID: 15223
						public static class LEVELS
						{
							// Token: 0x02003BB1 RID: 15281
							public static class CLEAR_SKIES
							{
								// Token: 0x0400E685 RID: 59013
								public static LocString NAME = "Clear Skies";

								// Token: 0x0400E686 RID: 59014
								public static LocString TOOLTIP = "No meteor damage, no worries";
							}

							// Token: 0x02003BB2 RID: 15282
							public static class INFREQUENT
							{
								// Token: 0x0400E687 RID: 59015
								public static LocString NAME = "Spring Showers";

								// Token: 0x0400E688 RID: 59016
								public static LocString TOOLTIP = "Meteor showers are less frequent and less intense than usual";
							}

							// Token: 0x02003BB3 RID: 15283
							public static class DEFAULT
							{
								// Token: 0x0400E689 RID: 59017
								public static LocString NAME = "Default";

								// Token: 0x0400E68A RID: 59018
								public static LocString TOOLTIP = "Default meteor shower frequency and intensity";
							}

							// Token: 0x02003BB4 RID: 15284
							public static class INTENSE
							{
								// Token: 0x0400E68B RID: 59019
								public static LocString NAME = "Cosmic Storm";

								// Token: 0x0400E68C RID: 59020
								public static LocString TOOLTIP = "Meteor showers are more frequent and more intense than usual";
							}

							// Token: 0x02003BB5 RID: 15285
							public static class DOOMED
							{
								// Token: 0x0400E68D RID: 59021
								public static LocString NAME = "Doomsday";

								// Token: 0x0400E68E RID: 59022
								public static LocString TOOLTIP = "An onslaught of apocalyptic hailstorms that feels almost personal";
							}
						}
					}

					// Token: 0x02003AF8 RID: 15096
					public class DLC_MIXING
					{
						// Token: 0x02003B78 RID: 15224
						public static class LEVELS
						{
							// Token: 0x02003BB6 RID: 15286
							public static class DISABLED
							{
								// Token: 0x0400E68F RID: 59023
								public static LocString NAME = "Disabled";

								// Token: 0x0400E690 RID: 59024
								public static LocString TOOLTIP = "Content from this DLC is currently <b>disabled</b>";
							}

							// Token: 0x02003BB7 RID: 15287
							public static class ENABLED
							{
								// Token: 0x0400E691 RID: 59025
								public static LocString NAME = "Enabled";

								// Token: 0x0400E692 RID: 59026
								public static LocString TOOLTIP = "Content from this DLC is currently <b>enabled</b>\n\nThis includes Care Packages, buildings, and space POIs";
							}
						}
					}

					// Token: 0x02003AF9 RID: 15097
					public class SUBWORLD_MIXING
					{
						// Token: 0x02003B79 RID: 15225
						public static class LEVELS
						{
							// Token: 0x02003BB8 RID: 15288
							public static class DISABLED
							{
								// Token: 0x0400E693 RID: 59027
								public static LocString NAME = "Disabled";

								// Token: 0x0400E694 RID: 59028
								public static LocString TOOLTIP = "This biome will not be mixed into any world";

								// Token: 0x0400E695 RID: 59029
								public static LocString TOOLTIP_BASEGAME = "This biome will not be mixed in";
							}

							// Token: 0x02003BB9 RID: 15289
							public static class TRY_MIXING
							{
								// Token: 0x0400E696 RID: 59030
								public static LocString NAME = "Likely";

								// Token: 0x0400E697 RID: 59031
								public static LocString TOOLTIP = "This biome is very likely to be mixed into a world";

								// Token: 0x0400E698 RID: 59032
								public static LocString TOOLTIP_BASEGAME = "This biome is very likely to be mixed in";
							}

							// Token: 0x02003BBA RID: 15290
							public static class GUARANTEE_MIXING
							{
								// Token: 0x0400E699 RID: 59033
								public static LocString NAME = "Guaranteed";

								// Token: 0x0400E69A RID: 59034
								public static LocString TOOLTIP = "This biome will be mixed into a world, even if it causes a worldgen failure";

								// Token: 0x0400E69B RID: 59035
								public static LocString TOOLTIP_BASEGAME = "This biome will be mixed in, even if it causes a worldgen failure";
							}
						}
					}

					// Token: 0x02003AFA RID: 15098
					public class WORLD_MIXING
					{
						// Token: 0x02003B7A RID: 15226
						public static class LEVELS
						{
							// Token: 0x02003BBB RID: 15291
							public static class DISABLED
							{
								// Token: 0x0400E69C RID: 59036
								public static LocString NAME = "Disabled";

								// Token: 0x0400E69D RID: 59037
								public static LocString TOOLTIP = "This asteroid will not be mixed in";
							}

							// Token: 0x02003BBC RID: 15292
							public static class TRY_MIXING
							{
								// Token: 0x0400E69E RID: 59038
								public static LocString NAME = "Likely";

								// Token: 0x0400E69F RID: 59039
								public static LocString TOOLTIP = "This asteroid is very likely to be mixed in";
							}

							// Token: 0x02003BBD RID: 15293
							public static class GUARANTEE_MIXING
							{
								// Token: 0x0400E6A0 RID: 59040
								public static LocString NAME = "Guaranteed";

								// Token: 0x0400E6A1 RID: 59041
								public static LocString TOOLTIP = "This asteroid will be mixed in, even if it causes worldgen failure";
							}
						}
					}
				}
			}

			// Token: 0x02002A13 RID: 10771
			public class MAINMENU
			{
				// Token: 0x0400B67B RID: 46715
				public static LocString STARTDEMO = "START DEMO";

				// Token: 0x0400B67C RID: 46716
				public static LocString NEWGAME = "NEW GAME";

				// Token: 0x0400B67D RID: 46717
				public static LocString RESUMEGAME = "RESUME GAME";

				// Token: 0x0400B67E RID: 46718
				public static LocString LOADGAME = "LOAD GAME";

				// Token: 0x0400B67F RID: 46719
				public static LocString RETIREDCOLONIES = "COLONY SUMMARIES";

				// Token: 0x0400B680 RID: 46720
				public static LocString KLEIINVENTORY = "KLEI INVENTORY";

				// Token: 0x0400B681 RID: 46721
				public static LocString LOCKERMENU = "SUPPLY CLOSET";

				// Token: 0x0400B682 RID: 46722
				public static LocString SCENARIOS = "SCENARIOS";

				// Token: 0x0400B683 RID: 46723
				public static LocString TRANSLATIONS = "TRANSLATIONS";

				// Token: 0x0400B684 RID: 46724
				public static LocString OPTIONS = "OPTIONS";

				// Token: 0x0400B685 RID: 46725
				public static LocString QUITTODESKTOP = "QUIT";

				// Token: 0x0400B686 RID: 46726
				public static LocString RESTARTCONFIRM = "Should I really quit?\nAll unsaved progress will be lost.";

				// Token: 0x0400B687 RID: 46727
				public static LocString QUITCONFIRM = "Should I quit to the main menu?\nAll unsaved progress will be lost.";

				// Token: 0x0400B688 RID: 46728
				public static LocString RETIRECONFIRM = "Should I surrender under the soul-crushing weight of this universe's entropy and retire my colony?";

				// Token: 0x0400B689 RID: 46729
				public static LocString DESKTOPQUITCONFIRM = "Should I really quit?\nAll unsaved progress will be lost.";

				// Token: 0x0400B68A RID: 46730
				public static LocString RESUMEBUTTON_BASENAME = "{0}: Cycle {1}";

				// Token: 0x0400B68B RID: 46731
				public static LocString QUIT = "QUIT WITHOUT SAVING";

				// Token: 0x0400B68C RID: 46732
				public static LocString SAVEANDQUITTITLE = "SAVE AND QUIT";

				// Token: 0x0400B68D RID: 46733
				public static LocString SAVEANDQUITDESKTOP = "SAVE AND QUIT";

				// Token: 0x0400B68E RID: 46734
				public static LocString WISHLIST_AD = "Available now";

				// Token: 0x0400B68F RID: 46735
				public static LocString WISHLIST_AD_TOOLTIP = "<color=#ffff00ff><b>Click to view it in the store</b></color>";

				// Token: 0x0200368D RID: 13965
				public class DLC
				{
					// Token: 0x0400DA41 RID: 55873
					public static LocString ACTIVATE_EXPANSION1 = "ENABLE DLC";

					// Token: 0x0400DA42 RID: 55874
					public static LocString ACTIVATE_EXPANSION1_TOOLTIP = "<b>This DLC is disabled</b>\n\n<color=#ffff00ff><b>Click to enable the <i>Spaced Out!</i> DLC</b></color>";

					// Token: 0x0400DA43 RID: 55875
					public static LocString ACTIVATE_EXPANSION1_DESC = "The game will need to restart in order to enable <i>Spaced Out!</i>";

					// Token: 0x0400DA44 RID: 55876
					public static LocString ACTIVATE_EXPANSION1_RAIL_DESC = "<i>Spaced Out!</i> will be enabled the next time you launch the game. The game will now close.";

					// Token: 0x0400DA45 RID: 55877
					public static LocString DEACTIVATE_EXPANSION1 = "DISABLE DLC";

					// Token: 0x0400DA46 RID: 55878
					public static LocString DEACTIVATE_EXPANSION1_TOOLTIP = "<b>This DLC is enabled</b>\n\n<color=#ffff00ff><b>Click to disable the <i>Spaced Out!</i> DLC</b></color>";

					// Token: 0x0400DA47 RID: 55879
					public static LocString DEACTIVATE_EXPANSION1_DESC = "The game will need to restart in order to enable the <i>Oxygen Not Included</i> base game.";

					// Token: 0x0400DA48 RID: 55880
					public static LocString DEACTIVATE_EXPANSION1_RAIL_DESC = "<i>Spaced Out!</i> will be disabled the next time you launch the game. The game will now close.";

					// Token: 0x0400DA49 RID: 55881
					public static LocString AD_DLC1 = "Spaced Out! DLC";

					// Token: 0x0400DA4A RID: 55882
					public static LocString CONTENT_INSTALLED_LABEL = "Installed";

					// Token: 0x0400DA4B RID: 55883
					public static LocString CONTENT_ACTIVE_TOOLTIP = "<b>This DLC is enabled</b>\n\nFind it in the destination selection screen when starting a new game, or in the Load Game screen for existing DLC-enabled saves";

					// Token: 0x0400DA4C RID: 55884
					public static LocString CONTENT_OWNED_NOTINSTALLED_LABEL = "";

					// Token: 0x0400DA4D RID: 55885
					public static LocString CONTENT_OWNED_NOTINSTALLED_TOOLTIP = "This DLC is owned but not currently installed";

					// Token: 0x0400DA4E RID: 55886
					public static LocString CONTENT_NOTOWNED_LABEL = "Available Now";

					// Token: 0x0400DA4F RID: 55887
					public static LocString CONTENT_NOTOWNED_TOOLTIP = "This DLC is available now!";
				}
			}

			// Token: 0x02002A14 RID: 10772
			public class DEVTOOLS
			{
				// Token: 0x0400B690 RID: 46736
				public static LocString TITLE = "About Dev Tools";

				// Token: 0x0400B691 RID: 46737
				public static LocString WARNING = "DANGER!!\n\nDev Tools are intended for developer use only. Using them may result in your save becoming unplayable, unstable, or severely damaged.\n\nThese tools are completely unsupported and may contain bugs. Are you sure you want to continue?";

				// Token: 0x0400B692 RID: 46738
				public static LocString DONTSHOW = "Do not show this message again";

				// Token: 0x0400B693 RID: 46739
				public static LocString BUTTON = "Show Dev Tools";
			}

			// Token: 0x02002A15 RID: 10773
			public class NEWGAMESETTINGS
			{
				// Token: 0x0400B694 RID: 46740
				public static LocString HEADER = "GAME SETTINGS";

				// Token: 0x0200368E RID: 13966
				public class BUTTONS
				{
					// Token: 0x0400DA50 RID: 55888
					public static LocString STANDARDGAME = "Standard Game";

					// Token: 0x0400DA51 RID: 55889
					public static LocString CUSTOMGAME = "Custom Game";

					// Token: 0x0400DA52 RID: 55890
					public static LocString CANCEL = "Cancel";

					// Token: 0x0400DA53 RID: 55891
					public static LocString STARTGAME = "Start Game";
				}
			}

			// Token: 0x02002A16 RID: 10774
			public class COLONYDESTINATIONSCREEN
			{
				// Token: 0x0400B695 RID: 46741
				public static LocString TITLE = "CHOOSE A DESTINATION";

				// Token: 0x0400B696 RID: 46742
				public static LocString GENTLE_ZONE = "Habitable Zone";

				// Token: 0x0400B697 RID: 46743
				public static LocString DETAILS = "Destination Details";

				// Token: 0x0400B698 RID: 46744
				public static LocString START_SITE = "Immediate Surroundings";

				// Token: 0x0400B699 RID: 46745
				public static LocString COORDINATE = "Coordinates:";

				// Token: 0x0400B69A RID: 46746
				public static LocString CANCEL = "Back";

				// Token: 0x0400B69B RID: 46747
				public static LocString CUSTOMIZE = "Game Settings";

				// Token: 0x0400B69C RID: 46748
				public static LocString START_GAME = "Start Game";

				// Token: 0x0400B69D RID: 46749
				public static LocString SHUFFLE = "Shuffle";

				// Token: 0x0400B69E RID: 46750
				public static LocString SHUFFLETOOLTIP = "Reroll World Seed\n\nThis will shuffle the layout of your world and the geographical traits listed below";

				// Token: 0x0400B69F RID: 46751
				public static LocString SHUFFLETOOLTIP_DISABLED = "This world's seed is predetermined. It cannot be changed";

				// Token: 0x0400B6A0 RID: 46752
				public static LocString HEADER_ASTEROID_STARTING = "Starting Asteroid";

				// Token: 0x0400B6A1 RID: 46753
				public static LocString HEADER_ASTEROID_NEARBY = "Nearby Asteroids";

				// Token: 0x0400B6A2 RID: 46754
				public static LocString HEADER_ASTEROID_DISTANT = "Distant Asteroids";

				// Token: 0x0400B6A3 RID: 46755
				public static LocString TRAITS_HEADER = "World Traits";

				// Token: 0x0400B6A4 RID: 46756
				public static LocString STORY_TRAITS_HEADER = "Story Traits";

				// Token: 0x0400B6A5 RID: 46757
				public static LocString MIXING_SETTINGS_HEADER = "Scramble DLCs";

				// Token: 0x0400B6A6 RID: 46758
				public static LocString MIXING_DLC_HEADER = "DLC Content";

				// Token: 0x0400B6A7 RID: 46759
				public static LocString MIXING_WORLDMIXING_HEADER = "Asteroid Remix";

				// Token: 0x0400B6A8 RID: 46760
				public static LocString MIXING_SUBWORLDMIXING_HEADER = "Biome Remix";

				// Token: 0x0400B6A9 RID: 46761
				public static LocString MIXING_NO_OPTIONS = "No additional content currently available for remixing. Don't worry, there's plenty already baked in.";

				// Token: 0x0400B6AA RID: 46762
				public static LocString MIXING_WARNING = "Choose additional content to remix into the game. Scrambling realities may cause cosmic collapse.";

				// Token: 0x0400B6AB RID: 46763
				public static LocString MIXING_TOOLTIP_DLC_MIXING = "DLC content includes buildings, Care Packages, space POIs, critters, etc\n\nEnabling DLC content allows asteroid and biome remixes from that DLC to be customized in the sections below";

				// Token: 0x0400B6AC RID: 46764
				public static LocString MIXING_TOOLTIP_ASTEROID_MIXING = "Asteroid remixing modifies which asteroids appear on the starmap\n\nRemixed asteroids will retain key features of the outer asteroids that they replace";

				// Token: 0x0400B6AD RID: 46765
				public static LocString MIXING_TOOLTIP_BIOME_MIXING = "Biome remixing modifies which biomes will be included across multiple asteroids";

				// Token: 0x0400B6AE RID: 46766
				public static LocString MIXING_TOOLTIP_TOO_MANY_GUARENTEED_ASTEROID_MIXINGS = UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_ASTEROID_MIXING + "\n\nMaximum of {1} guaranteed asteroid remixes allowed\n\nTotal currently selected: {0}";

				// Token: 0x0400B6AF RID: 46767
				public static LocString MIXING_TOOLTIP_TOO_MANY_GUARENTEED_BIOME_MIXINGS = UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_BIOME_MIXING + "\n\nMaximum of {1} guaranteed biome remixes allowed\n\nTotal currently selected: {0}";

				// Token: 0x0400B6B0 RID: 46768
				public static LocString MIXING_TOOLTIP_LOCKED_START_NOT_SUPPORTED = "This destination does not support changing this setting";

				// Token: 0x0400B6B1 RID: 46769
				public static LocString MIXING_TOOLTIP_LOCKED_REQUIRE_DLC_NOT_ENABLED = "This setting requires the following content to be enabled:\n{0}";

				// Token: 0x0400B6B2 RID: 46770
				public static LocString MIXING_TOOLTIP_DLC_CONTENT = "This content is from {0}";

				// Token: 0x0400B6B3 RID: 46771
				public static LocString MIXING_TOOLTIP_MODDED_SETTING = "<i><color=#d6d6d6>This setting was added by a mod</color></i>";

				// Token: 0x0400B6B4 RID: 46772
				public static LocString MIXING_TOOLTIP_CANNOT_START = "Cannot start a new game with current asteroid and biome remix configuration";

				// Token: 0x0400B6B5 RID: 46773
				public static LocString NO_TRAITS = "No Traits";

				// Token: 0x0400B6B6 RID: 46774
				public static LocString SINGLE_TRAIT = "1 Trait";

				// Token: 0x0400B6B7 RID: 46775
				public static LocString TRAIT_COUNT = "{0} Traits";

				// Token: 0x0400B6B8 RID: 46776
				public static LocString TOO_MANY_TRAITS_WARNING = UI.YELLOW_PREFIX + "Too many!" + UI.COLOR_SUFFIX;

				// Token: 0x0400B6B9 RID: 46777
				public static LocString TOO_MANY_TRAITS_WARNING_TOOLTIP = UI.YELLOW_PREFIX + "Squeezing this many story traits into this asteroid may cause worldgen to fail\n\nConsider lowering the number of story traits or changing the selected asteroid" + UI.COLOR_SUFFIX;

				// Token: 0x0400B6BA RID: 46778
				public static LocString SHUFFLE_STORY_TRAITS_TOOLTIP = "Randomize Story Traits\n\nThis will select a comfortable number of story traits for the starting asteroid";

				// Token: 0x0400B6BB RID: 46779
				public static LocString SELECTED_CLUSTER_TRAITS_HEADER = "Target Details";
			}

			// Token: 0x02002A17 RID: 10775
			public class MODESELECTSCREEN
			{
				// Token: 0x0400B6BC RID: 46780
				public static LocString HEADER = "GAME MODE";

				// Token: 0x0400B6BD RID: 46781
				public static LocString BLANK_DESC = "Select a playstyle...";

				// Token: 0x0400B6BE RID: 46782
				public static LocString SURVIVAL_TITLE = "SURVIVAL";

				// Token: 0x0400B6BF RID: 46783
				public static LocString SURVIVAL_DESC = "Stay on your toes and one step ahead of this unforgiving world. One slip up could bring your colony crashing down.";

				// Token: 0x0400B6C0 RID: 46784
				public static LocString NOSWEAT_TITLE = "NO SWEAT";

				// Token: 0x0400B6C1 RID: 46785
				public static LocString NOSWEAT_DESC = "When disaster strikes (and it inevitably will), take a deep breath and stay calm. You have ample time to find a solution.";

				// Token: 0x0400B6C2 RID: 46786
				public static LocString ACTIVE_CONTENT_HEADER = "ACTIVE CONTENT";
			}

			// Token: 0x02002A18 RID: 10776
			public class CLUSTERCATEGORYSELECTSCREEN
			{
				// Token: 0x0400B6C3 RID: 46787
				public static LocString HEADER = "ASTEROID STYLE";

				// Token: 0x0400B6C4 RID: 46788
				public static LocString BLANK_DESC = "Select an asteroid style...";

				// Token: 0x0400B6C5 RID: 46789
				public static LocString VANILLA_TITLE = "Standard";

				// Token: 0x0400B6C6 RID: 46790
				public static LocString VANILLA_DESC = "Scenarios designed for classic gameplay.";

				// Token: 0x0400B6C7 RID: 46791
				public static LocString CLASSIC_TITLE = "Classic";

				// Token: 0x0400B6C8 RID: 46792
				public static LocString CLASSIC_DESC = "Scenarios similar to the <b>classic Oxygen Not Included</b> experience. Large starting asteroids with many resources.\nLess emphasis on space travel.";

				// Token: 0x0400B6C9 RID: 46793
				public static LocString SPACEDOUT_TITLE = "Spaced Out!";

				// Token: 0x0400B6CA RID: 46794
				public static LocString SPACEDOUT_DESC = "Scenarios designed for the <b>Spaced Out! DLC</b>.\nSmaller starting asteroids with resources distributed across the starmap. More emphasis on space travel.";

				// Token: 0x0400B6CB RID: 46795
				public static LocString EVENT_TITLE = "The Lab";

				// Token: 0x0400B6CC RID: 46796
				public static LocString EVENT_DESC = "Alternative gameplay experiences, including experimental scenarios designed for special events.";
			}

			// Token: 0x02002A19 RID: 10777
			public class PATCHNOTESSCREEN
			{
				// Token: 0x0400B6CD RID: 46797
				public static LocString HEADER = "IMPORTANT UPDATE NOTES";

				// Token: 0x0400B6CE RID: 46798
				public static LocString OK_BUTTON = "OK";

				// Token: 0x0400B6CF RID: 46799
				public static LocString FULLPATCHNOTES_TOOLTIP = "View the full patch notes online";
			}

			// Token: 0x02002A1A RID: 10778
			public class LOADSCREEN
			{
				// Token: 0x0400B6D0 RID: 46800
				public static LocString TITLE = "LOAD GAME";

				// Token: 0x0400B6D1 RID: 46801
				public static LocString TITLE_INSPECT = "LOAD GAME";

				// Token: 0x0400B6D2 RID: 46802
				public static LocString DELETEBUTTON = "DELETE";

				// Token: 0x0400B6D3 RID: 46803
				public static LocString BACKBUTTON = "< BACK";

				// Token: 0x0400B6D4 RID: 46804
				public static LocString CONFIRMDELETE = "Are you sure you want to delete {0}?\nYou cannot undo this action.";

				// Token: 0x0400B6D5 RID: 46805
				public static LocString SAVEDETAILS = "<b>File:</b> {0}\n\n<b>Save Date:</b>\n{1}\n\n<b>Base Name:</b> {2}\n<b>Duplicants Alive:</b> {3}\n<b>Cycle(s) Survived:</b> {4}";

				// Token: 0x0400B6D6 RID: 46806
				public static LocString AUTOSAVEWARNING = "<color=#F44A47FF>Autosave: This file will get deleted as new autosaves are created</color>";

				// Token: 0x0400B6D7 RID: 46807
				public static LocString CORRUPTEDSAVE = "<b><color=#F44A47FF>Could not load file {0}. Its data may be corrupted.</color></b>";

				// Token: 0x0400B6D8 RID: 46808
				public static LocString SAVE_TOO_NEW = "<b><color=#F44A47FF>Could not load file {0}. File is using build {1}, v{2}. This build is {3}, v{4}.</color></b>";

				// Token: 0x0400B6D9 RID: 46809
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION = "This save file was created with a different DLC configuration\n\nTo load this file:";

				// Token: 0x0400B6DA RID: 46810
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_ENABLE = "    • Activate {0}";

				// Token: 0x0400B6DB RID: 46811
				public static LocString TOOLTIP_SAVE_INCOMPATABLE_DLC_CONFIGURATION_ASK_TO_DISABLE = "    • Deactivate {0}";

				// Token: 0x0400B6DC RID: 46812
				public static LocString TOOLTIP_SAVE_USES_DLC = "{0} save";

				// Token: 0x0400B6DD RID: 46813
				public static LocString UNSUPPORTED_SAVE_VERSION = "<b><color=#F44A47FF>This save file is from a previous version of the game and is no longer supported.</color></b>";

				// Token: 0x0400B6DE RID: 46814
				public static LocString MORE_INFO = "More Info";

				// Token: 0x0400B6DF RID: 46815
				public static LocString NEWEST_SAVE = "NEWEST";

				// Token: 0x0400B6E0 RID: 46816
				public static LocString BASE_NAME = "Base Name";

				// Token: 0x0400B6E1 RID: 46817
				public static LocString CYCLES_SURVIVED = "Cycles Survived";

				// Token: 0x0400B6E2 RID: 46818
				public static LocString DUPLICANTS_ALIVE = "Duplicants Alive";

				// Token: 0x0400B6E3 RID: 46819
				public static LocString WORLD_NAME = "Asteroid Type";

				// Token: 0x0400B6E4 RID: 46820
				public static LocString NO_FILE_SELECTED = "No file selected";

				// Token: 0x0400B6E5 RID: 46821
				public static LocString COLONY_INFO_FMT = "{0}: {1}";

				// Token: 0x0400B6E6 RID: 46822
				public static LocString LOAD_MORE_COLONIES_BUTTON = "Load more...";

				// Token: 0x0400B6E7 RID: 46823
				public static LocString VANILLA_RESTART = "Loading this colony will require restarting the game with " + UI.DLC1.NAME_ITAL + " content disabled";

				// Token: 0x0400B6E8 RID: 46824
				public static LocString EXPANSION1_RESTART = "Loading this colony will require restarting the game with " + UI.DLC1.NAME_ITAL + " content enabled";

				// Token: 0x0400B6E9 RID: 46825
				public static LocString UNSUPPORTED_VANILLA_TEMP = "<b><color=#F44A47FF>This save file is from the base version of the game and currently cannot be loaded while " + UI.DLC1.NAME_ITAL + " is installed.</color></b>";

				// Token: 0x0400B6EA RID: 46826
				public static LocString CONTENT = "Content";

				// Token: 0x0400B6EB RID: 46827
				public static LocString VANILLA_CONTENT = "Vanilla FIXME";

				// Token: 0x0400B6EC RID: 46828
				public static LocString EXPANSION1_CONTENT = UI.DLC1.NAME_ITAL + " Expansion FIXME";

				// Token: 0x0400B6ED RID: 46829
				public static LocString SAVE_INFO = "{0} saves  {1} autosaves  {2}";

				// Token: 0x0400B6EE RID: 46830
				public static LocString COLONIES_TITLE = "Colony View";

				// Token: 0x0400B6EF RID: 46831
				public static LocString COLONY_TITLE = "Viewing colony '{0}'";

				// Token: 0x0400B6F0 RID: 46832
				public static LocString COLONY_FILE_SIZE = "Size: {0}";

				// Token: 0x0400B6F1 RID: 46833
				public static LocString COLONY_FILE_NAME = "File: '{0}'";

				// Token: 0x0400B6F2 RID: 46834
				public static LocString NO_PREVIEW = "NO PREVIEW";

				// Token: 0x0400B6F3 RID: 46835
				public static LocString LOCAL_SAVE = "local";

				// Token: 0x0400B6F4 RID: 46836
				public static LocString CLOUD_SAVE = "cloud";

				// Token: 0x0400B6F5 RID: 46837
				public static LocString CONVERT_COLONY = "CONVERT COLONY";

				// Token: 0x0400B6F6 RID: 46838
				public static LocString CONVERT_ALL_COLONIES = "CONVERT ALL";

				// Token: 0x0400B6F7 RID: 46839
				public static LocString CONVERT_ALL_WARNING = UI.PRE_KEYWORD + "\nWarning:" + UI.PST_KEYWORD + " Converting all colonies may take some time.";

				// Token: 0x0400B6F8 RID: 46840
				public static LocString SAVE_INFO_DIALOG_TITLE = "SAVE INFORMATION";

				// Token: 0x0400B6F9 RID: 46841
				public static LocString SAVE_INFO_DIALOG_TEXT = "Access your save files using the options below.";

				// Token: 0x0400B6FA RID: 46842
				public static LocString SAVE_INFO_DIALOG_TOOLTIP = "Access your save file locations from here.";

				// Token: 0x0400B6FB RID: 46843
				public static LocString CONVERT_ERROR_TITLE = "SAVE CONVERSION UNSUCCESSFUL";

				// Token: 0x0400B6FC RID: 46844
				public static LocString CONVERT_ERROR = string.Concat(new string[]
				{
					"Converting the colony ",
					UI.PRE_KEYWORD,
					"{Colony}",
					UI.PST_KEYWORD,
					" was unsuccessful!\nThe error was:\n\n<b>{Error}</b>\n\nPlease try again, or post a bug in the forums if this problem keeps happening."
				});

				// Token: 0x0400B6FD RID: 46845
				public static LocString CONVERT_TO_CLOUD = "CONVERT TO CLOUD SAVES";

				// Token: 0x0400B6FE RID: 46846
				public static LocString CONVERT_TO_LOCAL = "CONVERT TO LOCAL SAVES";

				// Token: 0x0400B6FF RID: 46847
				public static LocString CONVERT_COLONY_TO_CLOUD = "Convert colony to use cloud saves";

				// Token: 0x0400B700 RID: 46848
				public static LocString CONVERT_COLONY_TO_LOCAL = "Convert to colony to use local saves";

				// Token: 0x0400B701 RID: 46849
				public static LocString CONVERT_ALL_TO_CLOUD = "Convert <b>all</b> colonies below to use cloud saves";

				// Token: 0x0400B702 RID: 46850
				public static LocString CONVERT_ALL_TO_LOCAL = "Convert <b>all</b> colonies below to use local saves";

				// Token: 0x0400B703 RID: 46851
				public static LocString CONVERT_ALL_TO_CLOUD_SUCCESS = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nAll existing colonies have been converted into ",
					UI.PRE_KEYWORD,
					"cloud",
					UI.PST_KEYWORD,
					" saves.\nNew colonies will use ",
					UI.PRE_KEYWORD,
					"cloud",
					UI.PST_KEYWORD,
					" saves by default.\n\n{Client} may take longer than usual to sync the next time you exit the game as a result of this change."
				});

				// Token: 0x0400B704 RID: 46852
				public static LocString CONVERT_ALL_TO_LOCAL_SUCCESS = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nAll existing colonies have been converted into ",
					UI.PRE_KEYWORD,
					"local",
					UI.PST_KEYWORD,
					" saves.\nNew colonies will use ",
					UI.PRE_KEYWORD,
					"local",
					UI.PST_KEYWORD,
					" saves by default.\n\n{Client} may take longer than usual to sync the next time you exit the game as a result of this change."
				});

				// Token: 0x0400B705 RID: 46853
				public static LocString CONVERT_TO_CLOUD_DETAILS = "Converting a colony to use cloud saves will move all of the save files for that colony into the cloud saves folder.\n\nThis allows your game platform to sync this colony to the cloud for your account, so it can be played on multiple machines.";

				// Token: 0x0400B706 RID: 46854
				public static LocString CONVERT_TO_LOCAL_DETAILS = "Converting a colony to NOT use cloud saves will move all of the save files for that colony into the local saves folder.\n\n" + UI.PRE_KEYWORD + "These save files will no longer be synced to the cloud." + UI.PST_KEYWORD;

				// Token: 0x0400B707 RID: 46855
				public static LocString OPEN_SAVE_FOLDER = "LOCAL SAVES";

				// Token: 0x0400B708 RID: 46856
				public static LocString OPEN_CLOUDSAVE_FOLDER = "CLOUD SAVES";

				// Token: 0x0400B709 RID: 46857
				public static LocString MIGRATE_TITLE = "SAVE FILE MIGRATION";

				// Token: 0x0400B70A RID: 46858
				public static LocString MIGRATE_SAVE_FILES = "MIGRATE SAVE FILES";

				// Token: 0x0400B70B RID: 46859
				public static LocString MIGRATE_COUNT = string.Concat(new string[]
				{
					"\nFound ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" autosaves that require migration."
				});

				// Token: 0x0400B70C RID: 46860
				public static LocString MIGRATE_RESULT = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"SUCCESS!",
					UI.PST_KEYWORD,
					"\nMigration moved ",
					UI.PRE_KEYWORD,
					"{0}/{1}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{2}/{3}",
					UI.PST_KEYWORD,
					" autosaves",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400B70D RID: 46861
				public static LocString MIGRATE_RESULT_FAILURES = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"<b>WARNING:</b> Not all saves could be migrated.",
					UI.PST_KEYWORD,
					"\nMigration moved ",
					UI.PRE_KEYWORD,
					"{0}/{1}",
					UI.PST_KEYWORD,
					" saves and ",
					UI.PRE_KEYWORD,
					"{2}/{3}",
					UI.PST_KEYWORD,
					" autosaves.\n\nThe file ",
					UI.PRE_KEYWORD,
					"{ErrorColony}",
					UI.PST_KEYWORD,
					" encountered this error:\n\n<b>{ErrorMessage}</b>"
				});

				// Token: 0x0400B70E RID: 46862
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_TITLE = "MIGRATION INCOMPLETE";

				// Token: 0x0400B70F RID: 46863
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_PRE = "<b>The game was unable to move all save files to their new location.\nTo fix this, please:</b>\n\n";

				// Token: 0x0400B710 RID: 46864
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM1 = "    1. Try temporarily disabling virus scanners and malware\n         protection programs.";

				// Token: 0x0400B711 RID: 46865
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM2 = "    2. Turn off file sync services such as OneDrive and DropBox.";

				// Token: 0x0400B712 RID: 46866
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_ITEM3 = "    3. Restart the game to retry file migration.";

				// Token: 0x0400B713 RID: 46867
				public static LocString MIGRATE_RESULT_FAILURES_MORE_INFO_POST = "\n<b>If this still doesn't solve the problem, please post a bug in the forums and we will attempt to assist with your issue.</b>";

				// Token: 0x0400B714 RID: 46868
				public static LocString MIGRATE_INFO = "We've changed how save files are organized!\nPlease " + UI.CLICK(UI.ClickType.click) + " the button below to automatically update your save file storage.";

				// Token: 0x0400B715 RID: 46869
				public static LocString MIGRATE_DONE = "CONTINUE";

				// Token: 0x0400B716 RID: 46870
				public static LocString MIGRATE_FAILURES_FORUM_BUTTON = "VISIT FORUMS";

				// Token: 0x0400B717 RID: 46871
				public static LocString MIGRATE_FAILURES_DONE = "MORE INFO";

				// Token: 0x0400B718 RID: 46872
				public static LocString CLOUD_TUTORIAL_BOUNCER = "Upload Saves to Cloud";
			}

			// Token: 0x02002A1B RID: 10779
			public class SAVESCREEN
			{
				// Token: 0x0400B719 RID: 46873
				public static LocString TITLE = "SAVE SLOTS";

				// Token: 0x0400B71A RID: 46874
				public static LocString NEWSAVEBUTTON = "New Save";

				// Token: 0x0400B71B RID: 46875
				public static LocString OVERWRITEMESSAGE = "Are you sure you want to overwrite {0}?";

				// Token: 0x0400B71C RID: 46876
				public static LocString SAVENAMETITLE = "SAVE NAME";

				// Token: 0x0400B71D RID: 46877
				public static LocString CONFIRMNAME = "Confirm";

				// Token: 0x0400B71E RID: 46878
				public static LocString CANCELNAME = "Cancel";

				// Token: 0x0400B71F RID: 46879
				public static LocString IO_ERROR = "An error occurred trying to save your game. Please ensure there is sufficient disk space.\n\n{0}";

				// Token: 0x0400B720 RID: 46880
				public static LocString REPORT_BUG = "Report Bug";

				// Token: 0x0400B721 RID: 46881
				public static LocString SAVE_COMPLETE_MESSAGE = "Save Complete";
			}

			// Token: 0x02002A1C RID: 10780
			public class RAILFORCEQUIT
			{
				// Token: 0x0400B722 RID: 46882
				public static LocString SAVE_EXIT = "Play time has expired and the game is exiting. Would you like to overwrite {0}?";

				// Token: 0x0400B723 RID: 46883
				public static LocString WARN_EXIT = "Play time has expired and the game will now exit.";

				// Token: 0x0400B724 RID: 46884
				public static LocString DLC_NOT_PURCHASED = "The <i>Spaced Out!</i> DLC has not yet been purchased in the WeGame store. Purchase <i>Spaced Out!</i> to support <i>Oxygen Not Included</i> and enjoy the new content!";
			}

			// Token: 0x02002A1D RID: 10781
			public class MOD_ERRORS
			{
				// Token: 0x0400B725 RID: 46885
				public static LocString TITLE = "MOD ERRORS";

				// Token: 0x0400B726 RID: 46886
				public static LocString DETAILS = "DETAILS";

				// Token: 0x0400B727 RID: 46887
				public static LocString CLOSE = "CLOSE";
			}

			// Token: 0x02002A1E RID: 10782
			public class MODS
			{
				// Token: 0x0400B728 RID: 46888
				public static LocString TITLE = "MODS";

				// Token: 0x0400B729 RID: 46889
				public static LocString MANAGE = "Subscription";

				// Token: 0x0400B72A RID: 46890
				public static LocString MANAGE_LOCAL = "Browse";

				// Token: 0x0400B72B RID: 46891
				public static LocString WORKSHOP = "STEAM WORKSHOP";

				// Token: 0x0400B72C RID: 46892
				public static LocString ENABLE_ALL = "ENABLE ALL";

				// Token: 0x0400B72D RID: 46893
				public static LocString DISABLE_ALL = "DISABLE ALL";

				// Token: 0x0400B72E RID: 46894
				public static LocString DRAG_TO_REORDER = "Drag to reorder";

				// Token: 0x0400B72F RID: 46895
				public static LocString REQUIRES_RESTART = "Mod changes require restart";

				// Token: 0x0400B730 RID: 46896
				public static LocString FAILED_TO_LOAD = "A mod failed to load and is being disabled:\n\n{0}: {1}\n\n{2}";

				// Token: 0x0400B731 RID: 46897
				public static LocString DB_CORRUPT = "An error occurred trying to load the Mod Database.\n\n{0}";

				// Token: 0x0200368F RID: 13967
				public class CONTENT_FAILURE
				{
					// Token: 0x0400DA54 RID: 55892
					public static LocString DISABLED_CONTENT = " - <b>Not compatible with <i>{Content}</i></b>";

					// Token: 0x0400DA55 RID: 55893
					public static LocString NO_CONTENT = " - <b>No compatible mod found</b>";

					// Token: 0x0400DA56 RID: 55894
					public static LocString OLD_API = " - <b>Mod out-of-date</b>";
				}

				// Token: 0x02003690 RID: 13968
				public class TOOLTIPS
				{
					// Token: 0x0400DA57 RID: 55895
					public static LocString ENABLED = "Enabled";

					// Token: 0x0400DA58 RID: 55896
					public static LocString DISABLED = "Disabled";

					// Token: 0x0400DA59 RID: 55897
					public static LocString MANAGE_STEAM_SUBSCRIPTION = "Manage Steam Subscription";

					// Token: 0x0400DA5A RID: 55898
					public static LocString MANAGE_RAIL_SUBSCRIPTION = "Manage Subscription";

					// Token: 0x0400DA5B RID: 55899
					public static LocString MANAGE_LOCAL_MOD = "Manage Local Mod";
				}

				// Token: 0x02003691 RID: 13969
				public class RAILMODUPLOAD
				{
					// Token: 0x0400DA5C RID: 55900
					public static LocString TITLE = "Upload Mod";

					// Token: 0x0400DA5D RID: 55901
					public static LocString NAME = "Mod Name";

					// Token: 0x0400DA5E RID: 55902
					public static LocString DESCRIPTION = "Mod Description";

					// Token: 0x0400DA5F RID: 55903
					public static LocString VERSION = "Version Number";

					// Token: 0x0400DA60 RID: 55904
					public static LocString PREVIEW_IMAGE = "Preview Image Path";

					// Token: 0x0400DA61 RID: 55905
					public static LocString CONTENT_FOLDER = "Content Folder Path";

					// Token: 0x0400DA62 RID: 55906
					public static LocString SHARE_TYPE = "Share Type";

					// Token: 0x0400DA63 RID: 55907
					public static LocString SUBMIT = "Submit";

					// Token: 0x0400DA64 RID: 55908
					public static LocString SUBMIT_READY = "This mod is ready to submit";

					// Token: 0x0400DA65 RID: 55909
					public static LocString SUBMIT_NOT_READY = "The mod cannot be submitted. Check that all fields are properly entered and that the paths are valid.";

					// Token: 0x02003AFB RID: 15099
					public static class MOD_SHARE_TYPE
					{
						// Token: 0x0400E4E8 RID: 58600
						public static LocString PRIVATE = "Private";

						// Token: 0x0400E4E9 RID: 58601
						public static LocString TOOLTIP_PRIVATE = "This mod will only be visible to its creator";

						// Token: 0x0400E4EA RID: 58602
						public static LocString FRIEND = "Friend";

						// Token: 0x0400E4EB RID: 58603
						public static LocString TOOLTIP_FRIEND = "Friend";

						// Token: 0x0400E4EC RID: 58604
						public static LocString PUBLIC = "Public";

						// Token: 0x0400E4ED RID: 58605
						public static LocString TOOLTIP_PUBLIC = "This mod will be available to all players after publishing. It may be subject to review before being allowed to be published.";
					}

					// Token: 0x02003AFC RID: 15100
					public static class MOD_UPLOAD_RESULT
					{
						// Token: 0x0400E4EE RID: 58606
						public static LocString SUCCESS = "Mod upload succeeded.";

						// Token: 0x0400E4EF RID: 58607
						public static LocString FAILURE = "Mod upload failed.";
					}
				}
			}

			// Token: 0x02002A1F RID: 10783
			public class MOD_EVENTS
			{
				// Token: 0x0400B732 RID: 46898
				public static LocString REQUIRED = "REQUIRED";

				// Token: 0x0400B733 RID: 46899
				public static LocString NOT_FOUND = "NOT FOUND";

				// Token: 0x0400B734 RID: 46900
				public static LocString INSTALL_INFO_INACCESSIBLE = "INACCESSIBLE";

				// Token: 0x0400B735 RID: 46901
				public static LocString OUT_OF_ORDER = "ORDERING CHANGED";

				// Token: 0x0400B736 RID: 46902
				public static LocString ACTIVE_DURING_CRASH = "ACTIVE DURING CRASH";

				// Token: 0x0400B737 RID: 46903
				public static LocString EXPECTED_ENABLED = "NEWLY DISABLED";

				// Token: 0x0400B738 RID: 46904
				public static LocString EXPECTED_DISABLED = "NEWLY ENABLED";

				// Token: 0x0400B739 RID: 46905
				public static LocString VERSION_UPDATE = "VERSION UPDATE";

				// Token: 0x0400B73A RID: 46906
				public static LocString AVAILABLE_CONTENT_CHANGED = "CONTENT CHANGED";

				// Token: 0x0400B73B RID: 46907
				public static LocString INSTALL_FAILED = "INSTALL FAILED";

				// Token: 0x0400B73C RID: 46908
				public static LocString DOWNLOAD_FAILED = "STEAM DOWNLOAD FAILED";

				// Token: 0x0400B73D RID: 46909
				public static LocString INSTALLED = "INSTALLED";

				// Token: 0x0400B73E RID: 46910
				public static LocString UNINSTALLED = "UNINSTALLED";

				// Token: 0x0400B73F RID: 46911
				public static LocString REQUIRES_RESTART = "RESTART REQUIRED";

				// Token: 0x0400B740 RID: 46912
				public static LocString BAD_WORLD_GEN = "LOAD FAILED";

				// Token: 0x0400B741 RID: 46913
				public static LocString DEACTIVATED = "DEACTIVATED";

				// Token: 0x0400B742 RID: 46914
				public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = "DEACTIVATED";

				// Token: 0x02003692 RID: 13970
				public class TOOLTIPS
				{
					// Token: 0x0400DA66 RID: 55910
					public static LocString REQUIRED = "The current save game couldn't load this mod. Unexpected things may happen!";

					// Token: 0x0400DA67 RID: 55911
					public static LocString NOT_FOUND = "This mod isn't installed";

					// Token: 0x0400DA68 RID: 55912
					public static LocString INSTALL_INFO_INACCESSIBLE = "Mod files are inaccessible";

					// Token: 0x0400DA69 RID: 55913
					public static LocString OUT_OF_ORDER = "Active mod has changed order with respect to some other active mod";

					// Token: 0x0400DA6A RID: 55914
					public static LocString ACTIVE_DURING_CRASH = "Mod was active during a crash and may be the cause";

					// Token: 0x0400DA6B RID: 55915
					public static LocString EXPECTED_ENABLED = "This mod needs to be enabled";

					// Token: 0x0400DA6C RID: 55916
					public static LocString EXPECTED_DISABLED = "This mod needs to be disabled";

					// Token: 0x0400DA6D RID: 55917
					public static LocString VERSION_UPDATE = "New version detected";

					// Token: 0x0400DA6E RID: 55918
					public static LocString AVAILABLE_CONTENT_CHANGED = "Content added or removed";

					// Token: 0x0400DA6F RID: 55919
					public static LocString INSTALL_FAILED = "Installation failed";

					// Token: 0x0400DA70 RID: 55920
					public static LocString DOWNLOAD_FAILED = "Steam failed to download the mod";

					// Token: 0x0400DA71 RID: 55921
					public static LocString INSTALLED = "Installation succeeded";

					// Token: 0x0400DA72 RID: 55922
					public static LocString UNINSTALLED = "Uninstalled";

					// Token: 0x0400DA73 RID: 55923
					public static LocString BAD_WORLD_GEN = "Encountered an error while loading file";

					// Token: 0x0400DA74 RID: 55924
					public static LocString DEACTIVATED = "Deactivated due to errors";

					// Token: 0x0400DA75 RID: 55925
					public static LocString ALL_MODS_DISABLED_EARLY_ACCESS = "Deactivated due to Early Access for " + UI.DLC1.NAME_ITAL;
				}
			}

			// Token: 0x02002A20 RID: 10784
			public class MOD_DIALOGS
			{
				// Token: 0x0400B743 RID: 46915
				public static LocString ADDITIONAL_MOD_EVENTS = "(...additional entries omitted)";

				// Token: 0x02003693 RID: 13971
				public class INSTALL_INFO_INACCESSIBLE
				{
					// Token: 0x0400DA76 RID: 55926
					public static LocString TITLE = "STEAM CONTENT ERROR";

					// Token: 0x0400DA77 RID: 55927
					public static LocString MESSAGE = "Failed to access local Steam files for mod {0}.\nTry restarting Oxygen not Included.\nIf that doesn't work, try re-subscribing to the mod via Steam.";
				}

				// Token: 0x02003694 RID: 13972
				public class STEAM_SUBSCRIBED
				{
					// Token: 0x0400DA78 RID: 55928
					public static LocString TITLE = "STEAM MOD SUBSCRIBED";

					// Token: 0x0400DA79 RID: 55929
					public static LocString MESSAGE = "Subscribed to Steam mod: {0}";
				}

				// Token: 0x02003695 RID: 13973
				public class STEAM_UPDATED
				{
					// Token: 0x0400DA7A RID: 55930
					public static LocString TITLE = "STEAM MOD UPDATE";

					// Token: 0x0400DA7B RID: 55931
					public static LocString MESSAGE = "Updating version of Steam mod: {0}";
				}

				// Token: 0x02003696 RID: 13974
				public class STEAM_UNSUBSCRIBED
				{
					// Token: 0x0400DA7C RID: 55932
					public static LocString TITLE = "STEAM MOD UNSUBSCRIBED";

					// Token: 0x0400DA7D RID: 55933
					public static LocString MESSAGE = "Unsubscribed from Steam mod: {0}";
				}

				// Token: 0x02003697 RID: 13975
				public class STEAM_REFRESH
				{
					// Token: 0x0400DA7E RID: 55934
					public static LocString TITLE = "STEAM MODS REFRESHED";

					// Token: 0x0400DA7F RID: 55935
					public static LocString MESSAGE = "Refreshed Steam mods:\n{0}";
				}

				// Token: 0x02003698 RID: 13976
				public class ALL_MODS_DISABLED_EARLY_ACCESS
				{
					// Token: 0x0400DA80 RID: 55936
					public static LocString TITLE = "ALL MODS DISABLED";

					// Token: 0x0400DA81 RID: 55937
					public static LocString MESSAGE = "Mod support is temporarily suspended for the initial launch of " + UI.DLC1.NAME_ITAL + " into Early Access:\n{0}";
				}

				// Token: 0x02003699 RID: 13977
				public class LOAD_FAILURE
				{
					// Token: 0x0400DA82 RID: 55938
					public static LocString TITLE = "LOAD FAILURE";

					// Token: 0x0400DA83 RID: 55939
					public static LocString MESSAGE = "Failed to load one or more mods:\n{0}\nThey will be re-installed when the game is restarted.\nGame may be unstable until restarted.";
				}

				// Token: 0x0200369A RID: 13978
				public class SAVE_GAME_MODS_DIFFER
				{
					// Token: 0x0400DA84 RID: 55940
					public static LocString TITLE = "MOD DIFFERENCES";

					// Token: 0x0400DA85 RID: 55941
					public static LocString MESSAGE = "Save game mods differ from currently active mods:\n{0}";
				}

				// Token: 0x0200369B RID: 13979
				public class MOD_ERRORS_ON_BOOT
				{
					// Token: 0x0400DA86 RID: 55942
					public static LocString TITLE = "MOD ERRORS";

					// Token: 0x0400DA87 RID: 55943
					public static LocString MESSAGE = "An error occurred during start-up with mods active.\nAll mods have been disabled to ensure a clean restart.\n{0}";

					// Token: 0x0400DA88 RID: 55944
					public static LocString DEV_MESSAGE = "An error occurred during start-up with mods active.\n{0}\nDisable all mods and restart, or continue in an unstable state?";
				}

				// Token: 0x0200369C RID: 13980
				public class MODS_SCREEN_CHANGES
				{
					// Token: 0x0400DA89 RID: 55945
					public static LocString TITLE = "MODS CHANGED";

					// Token: 0x0400DA8A RID: 55946
					public static LocString MESSAGE = "{0}\nRestart required to reload mods.\nGame may be unstable until restarted.";
				}

				// Token: 0x0200369D RID: 13981
				public class MOD_EVENTS
				{
					// Token: 0x0400DA8B RID: 55947
					public static LocString TITLE = "MOD EVENTS";

					// Token: 0x0400DA8C RID: 55948
					public static LocString MESSAGE = "{0}";

					// Token: 0x0400DA8D RID: 55949
					public static LocString DEV_MESSAGE = "{0}\nCheck Player.log for details.";
				}

				// Token: 0x0200369E RID: 13982
				public class RESTART
				{
					// Token: 0x0400DA8E RID: 55950
					public static LocString OK = "RESTART";

					// Token: 0x0400DA8F RID: 55951
					public static LocString CANCEL = "CONTINUE";

					// Token: 0x0400DA90 RID: 55952
					public static LocString MESSAGE = "{0}\nRestart required.";

					// Token: 0x0400DA91 RID: 55953
					public static LocString DEV_MESSAGE = "{0}\nRestart required.\nGame may be unstable until restarted.";
				}
			}

			// Token: 0x02002A21 RID: 10785
			public class PAUSE_SCREEN
			{
				// Token: 0x0400B744 RID: 46916
				public static LocString TITLE = "PAUSED";

				// Token: 0x0400B745 RID: 46917
				public static LocString RESUME = "Resume";

				// Token: 0x0400B746 RID: 46918
				public static LocString LOGBOOK = "Logbook";

				// Token: 0x0400B747 RID: 46919
				public static LocString OPTIONS = "Options";

				// Token: 0x0400B748 RID: 46920
				public static LocString SAVE = "Save";

				// Token: 0x0400B749 RID: 46921
				public static LocString ALREADY_SAVED = "<i><color=#CAC8C8>Already Saved</color></i>";

				// Token: 0x0400B74A RID: 46922
				public static LocString SAVEAS = "Save As";

				// Token: 0x0400B74B RID: 46923
				public static LocString COLONY_SUMMARY = "Colony Summary";

				// Token: 0x0400B74C RID: 46924
				public static LocString LOCKERMENU = "Supply Closet";

				// Token: 0x0400B74D RID: 46925
				public static LocString LOAD = "Load";

				// Token: 0x0400B74E RID: 46926
				public static LocString QUIT = "Main Menu";

				// Token: 0x0400B74F RID: 46927
				public static LocString DESKTOPQUIT = "Quit to Desktop";

				// Token: 0x0400B750 RID: 46928
				public static LocString WORLD_SEED = "Coordinates: {0}";

				// Token: 0x0400B751 RID: 46929
				public static LocString WORLD_SEED_TOOLTIP = "Share coordinates with a friend and they can start a colony on an identical asteroid!\n\n{0} - The asteroid\n\n{1} - The world seed\n\n{2} - Difficulty and Custom settings\n\n{3} - Story Trait settings\n\n{4} - Scramble DLC settings";

				// Token: 0x0400B752 RID: 46930
				public static LocString WORLD_SEED_COPY_TOOLTIP = "Copy Coordinates to clipboard\n\nShare coordinates with a friend and they can start a colony on an identical asteroid!";

				// Token: 0x0400B753 RID: 46931
				public static LocString MANAGEMENT_BUTTON = "Pause Menu";

				// Token: 0x0200369F RID: 13983
				public class ADD_DLC_MENU
				{
					// Token: 0x0400DA92 RID: 55954
					public static LocString ENABLE_QUESTION = "Enable DLC content on this save?\n\nThis will create a new copy of the save game. It will no longer be possible to load this copy without the DLC enabled.";

					// Token: 0x0400DA93 RID: 55955
					public static LocString CONFIRM = "CONFIRM";

					// Token: 0x0400DA94 RID: 55956
					public static LocString DLC_ENABLED_TOOLTIP = "This save has content from <b>{0}</b> DLC enabled";

					// Token: 0x0400DA95 RID: 55957
					public static LocString DLC_DISABLED_TOOLTIP = "This save does not currently have content from <b>{0}</b> DLC enabled \n\n<b>Click to enable it</b>";

					// Token: 0x0400DA96 RID: 55958
					public static LocString DLC_DISABLED_NOT_EDITABLE_TOOLTIP = "This save does not have content from the <b>{0}</b> DLC enabled";
				}
			}

			// Token: 0x02002A22 RID: 10786
			public class OPTIONS_SCREEN
			{
				// Token: 0x0400B754 RID: 46932
				public static LocString TITLE = "OPTIONS";

				// Token: 0x0400B755 RID: 46933
				public static LocString GRAPHICS = "Graphics";

				// Token: 0x0400B756 RID: 46934
				public static LocString AUDIO = "Audio";

				// Token: 0x0400B757 RID: 46935
				public static LocString GAME = "Game";

				// Token: 0x0400B758 RID: 46936
				public static LocString CONTROLS = "Controls";

				// Token: 0x0400B759 RID: 46937
				public static LocString UNITS = "Temperature Units";

				// Token: 0x0400B75A RID: 46938
				public static LocString METRICS = "Data Communication";

				// Token: 0x0400B75B RID: 46939
				public static LocString LANGUAGE = "Change Language";

				// Token: 0x0400B75C RID: 46940
				public static LocString WORLD_GEN = "World Generation Key";

				// Token: 0x0400B75D RID: 46941
				public static LocString RESET_TUTORIAL = "Reset Tutorial Messages";

				// Token: 0x0400B75E RID: 46942
				public static LocString RESET_TUTORIAL_WARNING = "All tutorial messages will be reset, and\nwill show up again the next time you play the game.";

				// Token: 0x0400B75F RID: 46943
				public static LocString FEEDBACK = "Feedback";

				// Token: 0x0400B760 RID: 46944
				public static LocString CREDITS = "Credits";

				// Token: 0x0400B761 RID: 46945
				public static LocString BACK = "Done";

				// Token: 0x0400B762 RID: 46946
				public static LocString UNLOCK_SANDBOX = "Unlock Sandbox Mode";

				// Token: 0x0400B763 RID: 46947
				public static LocString MODS = "MODS";

				// Token: 0x0400B764 RID: 46948
				public static LocString SAVE_OPTIONS = "Save Options";

				// Token: 0x020036A0 RID: 13984
				public class TOGGLE_SANDBOX_SCREEN
				{
					// Token: 0x0400DA97 RID: 55959
					public static LocString UNLOCK_SANDBOX_WARNING = "Sandbox Mode will be enabled for this save file";

					// Token: 0x0400DA98 RID: 55960
					public static LocString CONFIRM = "Enable Sandbox Mode";

					// Token: 0x0400DA99 RID: 55961
					public static LocString CANCEL = "Cancel";

					// Token: 0x0400DA9A RID: 55962
					public static LocString CONFIRM_SAVE_BACKUP = "Enable Sandbox Mode, but save a backup first";

					// Token: 0x0400DA9B RID: 55963
					public static LocString BACKUP_SAVE_GAME_APPEND = " (BACKUP)";
				}
			}

			// Token: 0x02002A23 RID: 10787
			public class INPUT_BINDINGS_SCREEN
			{
				// Token: 0x0400B765 RID: 46949
				public static LocString TITLE = "CUSTOMIZE KEYS";

				// Token: 0x0400B766 RID: 46950
				public static LocString RESET = "Reset";

				// Token: 0x0400B767 RID: 46951
				public static LocString APPLY = "Done";

				// Token: 0x0400B768 RID: 46952
				public static LocString DUPLICATE = "{0} was already bound to {1} and is now unbound.";

				// Token: 0x0400B769 RID: 46953
				public static LocString UNBOUND_ACTION = "{0} is unbound. Are you sure you want to continue?";

				// Token: 0x0400B76A RID: 46954
				public static LocString MULTIPLE_UNBOUND_ACTIONS = "You have multiple unbound actions, this may result in difficulty playing the game. Are you sure you want to continue?";

				// Token: 0x0400B76B RID: 46955
				public static LocString WAITING_FOR_INPUT = "???";
			}

			// Token: 0x02002A24 RID: 10788
			public class TRANSLATIONS_SCREEN
			{
				// Token: 0x0400B76C RID: 46956
				public static LocString TITLE = "TRANSLATIONS";

				// Token: 0x0400B76D RID: 46957
				public static LocString UNINSTALL = "Uninstall";

				// Token: 0x0400B76E RID: 46958
				public static LocString PREINSTALLED_HEADER = "Preinstalled Language Packs";

				// Token: 0x0400B76F RID: 46959
				public static LocString UGC_HEADER = "Subscribed Workshop Language Packs";

				// Token: 0x0400B770 RID: 46960
				public static LocString UGC_MOD_TITLE_FORMAT = "{0} (workshop)";

				// Token: 0x0400B771 RID: 46961
				public static LocString ARE_YOU_SURE = "Are you sure you want to uninstall this language pack?";

				// Token: 0x0400B772 RID: 46962
				public static LocString PLEASE_REBOOT = "Please restart your game for these changes to take effect.";

				// Token: 0x0400B773 RID: 46963
				public static LocString NO_PACKS = "Steam Workshop";

				// Token: 0x0400B774 RID: 46964
				public static LocString DOWNLOAD = "Start Download";

				// Token: 0x0400B775 RID: 46965
				public static LocString INSTALL = "Install";

				// Token: 0x0400B776 RID: 46966
				public static LocString INSTALLED = "Installed";

				// Token: 0x0400B777 RID: 46967
				public static LocString NO_STEAM = "Unable to retrieve language list from Steam";

				// Token: 0x0400B778 RID: 46968
				public static LocString RESTART = "RESTART";

				// Token: 0x0400B779 RID: 46969
				public static LocString CANCEL = "CANCEL";

				// Token: 0x0400B77A RID: 46970
				public static LocString MISSING_LANGUAGE_PACK = "Selected language pack ({0}) not found.\nReverting to default language.";

				// Token: 0x0400B77B RID: 46971
				public static LocString UNKNOWN = "Unknown";

				// Token: 0x020036A1 RID: 13985
				public class PREINSTALLED_LANGUAGES
				{
					// Token: 0x0400DA9C RID: 55964
					public static LocString EN = "English (Klei)";

					// Token: 0x0400DA9D RID: 55965
					public static LocString ZH_KLEI = "Chinese (Klei)";

					// Token: 0x0400DA9E RID: 55966
					public static LocString KO_KLEI = "Korean (Klei)";

					// Token: 0x0400DA9F RID: 55967
					public static LocString RU_KLEI = "Russian (Klei)";
				}
			}

			// Token: 0x02002A25 RID: 10789
			public class SCENARIOS_MENU
			{
				// Token: 0x0400B77C RID: 46972
				public static LocString TITLE = "Scenarios";

				// Token: 0x0400B77D RID: 46973
				public static LocString UNSUBSCRIBE = "Unsubscribe";

				// Token: 0x0400B77E RID: 46974
				public static LocString UNSUBSCRIBE_CONFIRM = "Are you sure you want to unsubscribe from this scenario?";

				// Token: 0x0400B77F RID: 46975
				public static LocString LOAD_SCENARIO_CONFIRM = "Load the \"{SCENARIO_NAME}\" scenario?";

				// Token: 0x0400B780 RID: 46976
				public static LocString LOAD_CONFIRM_TITLE = "LOAD";

				// Token: 0x0400B781 RID: 46977
				public static LocString SCENARIO_NAME = "Name:";

				// Token: 0x0400B782 RID: 46978
				public static LocString SCENARIO_DESCRIPTION = "Description";

				// Token: 0x0400B783 RID: 46979
				public static LocString BUTTON_DONE = "Done";

				// Token: 0x0400B784 RID: 46980
				public static LocString BUTTON_LOAD = "Load";

				// Token: 0x0400B785 RID: 46981
				public static LocString BUTTON_WORKSHOP = "Steam Workshop";

				// Token: 0x0400B786 RID: 46982
				public static LocString NO_SCENARIOS_AVAILABLE = "No scenarios available.\n\nSubscribe to some in the Steam Workshop.";
			}

			// Token: 0x02002A26 RID: 10790
			public class AUDIO_OPTIONS_SCREEN
			{
				// Token: 0x0400B787 RID: 46983
				public static LocString TITLE = "AUDIO OPTIONS";

				// Token: 0x0400B788 RID: 46984
				public static LocString HEADER_VOLUME = "VOLUME";

				// Token: 0x0400B789 RID: 46985
				public static LocString HEADER_SETTINGS = "SETTINGS";

				// Token: 0x0400B78A RID: 46986
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400B78B RID: 46987
				public static LocString MUSIC_EVERY_CYCLE = "Play background music each morning";

				// Token: 0x0400B78C RID: 46988
				public static LocString MUSIC_EVERY_CYCLE_TOOLTIP = "If enabled, background music will play every cycle instead of every few cycles";

				// Token: 0x0400B78D RID: 46989
				public static LocString AUTOMATION_SOUNDS_ALWAYS = "Always play automation sounds";

				// Token: 0x0400B78E RID: 46990
				public static LocString AUTOMATION_SOUNDS_ALWAYS_TOOLTIP = "If enabled, automation sound effects will play even when outside of the " + UI.FormatAsOverlay("Automation Overlay");

				// Token: 0x0400B78F RID: 46991
				public static LocString MUTE_ON_FOCUS_LOST = "Mute when unfocused";

				// Token: 0x0400B790 RID: 46992
				public static LocString MUTE_ON_FOCUS_LOST_TOOLTIP = "If enabled, the game will be muted while minimized or if the application loses focus";

				// Token: 0x0400B791 RID: 46993
				public static LocString AUDIO_BUS_MASTER = "Master";

				// Token: 0x0400B792 RID: 46994
				public static LocString AUDIO_BUS_SFX = "SFX";

				// Token: 0x0400B793 RID: 46995
				public static LocString AUDIO_BUS_MUSIC = "Music";

				// Token: 0x0400B794 RID: 46996
				public static LocString AUDIO_BUS_AMBIENCE = "Ambience";

				// Token: 0x0400B795 RID: 46997
				public static LocString AUDIO_BUS_UI = "UI";
			}

			// Token: 0x02002A27 RID: 10791
			public class GAME_OPTIONS_SCREEN
			{
				// Token: 0x0400B796 RID: 46998
				public static LocString TITLE = "GAME OPTIONS";

				// Token: 0x0400B797 RID: 46999
				public static LocString GENERAL_GAME_OPTIONS = "GENERAL";

				// Token: 0x0400B798 RID: 47000
				public static LocString DISABLED_WARNING = "More options available in-game";

				// Token: 0x0400B799 RID: 47001
				public static LocString DEFAULT_TO_CLOUD_SAVES = "Default to cloud saves";

				// Token: 0x0400B79A RID: 47002
				public static LocString DEFAULT_TO_CLOUD_SAVES_TOOLTIP = "When a new colony is created, this controls whether it will be saved into the cloud saves folder for syncing or not.";

				// Token: 0x0400B79B RID: 47003
				public static LocString RESET_TUTORIAL_DESCRIPTION = "Mark all tutorial messages \"unread\"";

				// Token: 0x0400B79C RID: 47004
				public static LocString SANDBOX_DESCRIPTION = "Enable sandbox tools";

				// Token: 0x0400B79D RID: 47005
				public static LocString CONTROLS_DESCRIPTION = "Change key bindings";

				// Token: 0x0400B79E RID: 47006
				public static LocString TEMPERATURE_UNITS = "TEMPERATURE UNITS";

				// Token: 0x0400B79F RID: 47007
				public static LocString SAVE_OPTIONS = "SAVE";

				// Token: 0x0400B7A0 RID: 47008
				public static LocString CAMERA_SPEED_LABEL = "Camera Pan Speed: {0}%";
			}

			// Token: 0x02002A28 RID: 10792
			public class METRIC_OPTIONS_SCREEN
			{
				// Token: 0x0400B7A1 RID: 47009
				public static LocString TITLE = "DATA COMMUNICATION";

				// Token: 0x0400B7A2 RID: 47010
				public static LocString HEADER_METRICS = "USER DATA";
			}

			// Token: 0x02002A29 RID: 10793
			public class COLONY_SAVE_OPTIONS_SCREEN
			{
				// Token: 0x0400B7A3 RID: 47011
				public static LocString TITLE = "COLONY SAVE OPTIONS";

				// Token: 0x0400B7A4 RID: 47012
				public static LocString DESCRIPTION = "Note: These values are configured per save file";

				// Token: 0x0400B7A5 RID: 47013
				public static LocString AUTOSAVE_FREQUENCY = "Autosave frequency:";

				// Token: 0x0400B7A6 RID: 47014
				public static LocString AUTOSAVE_FREQUENCY_DESCRIPTION = "Every: {0} cycle(s)";

				// Token: 0x0400B7A7 RID: 47015
				public static LocString AUTOSAVE_NEVER = "Never";

				// Token: 0x0400B7A8 RID: 47016
				public static LocString TIMELAPSE_RESOLUTION = "Timelapse resolution:";

				// Token: 0x0400B7A9 RID: 47017
				public static LocString TIMELAPSE_RESOLUTION_DESCRIPTION = "{0}x{1}";

				// Token: 0x0400B7AA RID: 47018
				public static LocString TIMELAPSE_DISABLED_DESCRIPTION = "Disabled";
			}

			// Token: 0x02002A2A RID: 10794
			public class FEEDBACK_SCREEN
			{
				// Token: 0x0400B7AB RID: 47019
				public static LocString TITLE = "FEEDBACK";

				// Token: 0x0400B7AC RID: 47020
				public static LocString HEADER = "We would love to hear from you!";

				// Token: 0x0400B7AD RID: 47021
				public static LocString DESCRIPTION = "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file. The buttons to the right will help you find those files on your local drive.\n\nThank you for being part of the Oxygen Not Included community!";

				// Token: 0x0400B7AE RID: 47022
				public static LocString ALT_DESCRIPTION = "Let us know if you encounter any problems or how we can improve your Oxygen Not Included experience.\n\nWhen reporting a bug, please include your log and colony save file.\n\nThank you for being part of the Oxygen Not Included community!";

				// Token: 0x0400B7AF RID: 47023
				public static LocString BUG_FORUMS_BUTTON = "Report a Bug";

				// Token: 0x0400B7B0 RID: 47024
				public static LocString SUGGESTION_FORUMS_BUTTON = "Suggestions Forum";

				// Token: 0x0400B7B1 RID: 47025
				public static LocString LOGS_DIRECTORY_BUTTON = "Browse Log Files";

				// Token: 0x0400B7B2 RID: 47026
				public static LocString SAVE_FILES_DIRECTORY_BUTTON = "Browse Save Files";
			}

			// Token: 0x02002A2B RID: 10795
			public class WORLD_GEN_OPTIONS_SCREEN
			{
				// Token: 0x0400B7B3 RID: 47027
				public static LocString TITLE = "WORLD GENERATION OPTIONS";

				// Token: 0x0400B7B4 RID: 47028
				public static LocString USE_SEED = "Set Worldgen Seed";

				// Token: 0x0400B7B5 RID: 47029
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400B7B6 RID: 47030
				public static LocString RANDOM_BUTTON = "Randomize";

				// Token: 0x0400B7B7 RID: 47031
				public static LocString RANDOM_BUTTON_TOOLTIP = "Randomize a new worldgen seed";

				// Token: 0x0400B7B8 RID: 47032
				public static LocString TOOLTIP = "This will override the current worldgen seed";
			}

			// Token: 0x02002A2C RID: 10796
			public class METRICS_OPTIONS_SCREEN
			{
				// Token: 0x0400B7B9 RID: 47033
				public static LocString TITLE = "DATA COMMUNICATION OPTIONS";

				// Token: 0x0400B7BA RID: 47034
				public static LocString ENABLE_BUTTON = "Enable Data Communication";

				// Token: 0x0400B7BB RID: 47035
				public static LocString DESCRIPTION = "Collecting user data helps us improve the game.\n\nPlayers who opt out of data communication will no longer send us crash reports and play data.\n\nThey will also be unable to receive new item unlocks from our servers, though existing unlocked items will continue to function.\n\nFor more details on our privacy policy and how we use the data we collect, please visit our <color=#ECA6C9><u><b>privacy center</b></u></color>.";

				// Token: 0x0400B7BC RID: 47036
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400B7BD RID: 47037
				public static LocString RESTART_BUTTON = "Restart Game";

				// Token: 0x0400B7BE RID: 47038
				public static LocString TOOLTIP = "Uncheck to disable data communication";

				// Token: 0x0400B7BF RID: 47039
				public static LocString RESTART_WARNING = "A game restart is required to apply settings.";
			}

			// Token: 0x02002A2D RID: 10797
			public class UNIT_OPTIONS_SCREEN
			{
				// Token: 0x0400B7C0 RID: 47040
				public static LocString TITLE = "TEMPERATURE UNITS";

				// Token: 0x0400B7C1 RID: 47041
				public static LocString CELSIUS = "Celsius";

				// Token: 0x0400B7C2 RID: 47042
				public static LocString CELSIUS_TOOLTIP = "Change temperature unit to Celsius (°C)";

				// Token: 0x0400B7C3 RID: 47043
				public static LocString KELVIN = "Kelvin";

				// Token: 0x0400B7C4 RID: 47044
				public static LocString KELVIN_TOOLTIP = "Change temperature unit to Kelvin (K)";

				// Token: 0x0400B7C5 RID: 47045
				public static LocString FAHRENHEIT = "Fahrenheit";

				// Token: 0x0400B7C6 RID: 47046
				public static LocString FAHRENHEIT_TOOLTIP = "Change temperature unit to Fahrenheit (°F)";
			}

			// Token: 0x02002A2E RID: 10798
			public class GRAPHICS_OPTIONS_SCREEN
			{
				// Token: 0x0400B7C7 RID: 47047
				public static LocString TITLE = "GRAPHICS OPTIONS";

				// Token: 0x0400B7C8 RID: 47048
				public static LocString FULLSCREEN = "Fullscreen";

				// Token: 0x0400B7C9 RID: 47049
				public static LocString RESOLUTION = "Resolution:";

				// Token: 0x0400B7CA RID: 47050
				public static LocString LOWRES = "Low Resolution Textures";

				// Token: 0x0400B7CB RID: 47051
				public static LocString APPLYBUTTON = "Apply";

				// Token: 0x0400B7CC RID: 47052
				public static LocString REVERTBUTTON = "Revert";

				// Token: 0x0400B7CD RID: 47053
				public static LocString DONE_BUTTON = "Done";

				// Token: 0x0400B7CE RID: 47054
				public static LocString UI_SCALE = "UI Scale";

				// Token: 0x0400B7CF RID: 47055
				public static LocString HEADER_DISPLAY = "DISPLAY";

				// Token: 0x0400B7D0 RID: 47056
				public static LocString HEADER_UI = "INTERFACE";

				// Token: 0x0400B7D1 RID: 47057
				public static LocString COLORMODE = "Color Mode:";

				// Token: 0x0400B7D2 RID: 47058
				public static LocString COLOR_MODE_DEFAULT = "Default";

				// Token: 0x0400B7D3 RID: 47059
				public static LocString COLOR_MODE_PROTANOPIA = "Protanopia";

				// Token: 0x0400B7D4 RID: 47060
				public static LocString COLOR_MODE_DEUTERANOPIA = "Deuteranopia";

				// Token: 0x0400B7D5 RID: 47061
				public static LocString COLOR_MODE_TRITANOPIA = "Tritanopia";

				// Token: 0x0400B7D6 RID: 47062
				public static LocString ACCEPT_CHANGES = "Accept Changes?";

				// Token: 0x0400B7D7 RID: 47063
				public static LocString ACCEPT_CHANGES_STRING_COLOR = "Interface changes will be visible immediately, but applying color changes to in-game text will require a restart.\n\nAccept Changes?";

				// Token: 0x0400B7D8 RID: 47064
				public static LocString COLORBLIND_FEEDBACK = "Color blindness options are currently in progress.\n\nIf you would benefit from an alternative color mode or have had difficulties with any of the default colors, please visit the forums and let us know about your experiences.\n\nYour feedback is extremely helpful to us!";

				// Token: 0x0400B7D9 RID: 47065
				public static LocString COLORBLIND_FEEDBACK_BUTTON = "Provide Feedback";
			}

			// Token: 0x02002A2F RID: 10799
			public class WORLDGENSCREEN
			{
				// Token: 0x0400B7DA RID: 47066
				public static LocString TITLE = "NEW GAME";

				// Token: 0x0400B7DB RID: 47067
				public static LocString GENERATINGWORLD = "GENERATING WORLD";

				// Token: 0x0400B7DC RID: 47068
				public static LocString SELECTSIZEPROMPT = "A new world is about to be created. Please select its size.";

				// Token: 0x0400B7DD RID: 47069
				public static LocString LOADINGGAME = "LOADING WORLD...";

				// Token: 0x020036A2 RID: 13986
				public class SIZES
				{
					// Token: 0x0400DAA0 RID: 55968
					public static LocString TINY = "Tiny";

					// Token: 0x0400DAA1 RID: 55969
					public static LocString SMALL = "Small";

					// Token: 0x0400DAA2 RID: 55970
					public static LocString STANDARD = "Standard";

					// Token: 0x0400DAA3 RID: 55971
					public static LocString LARGE = "Big";

					// Token: 0x0400DAA4 RID: 55972
					public static LocString HUGE = "Colossal";
				}
			}

			// Token: 0x02002A30 RID: 10800
			public class MINSPECSCREEN
			{
				// Token: 0x0400B7DE RID: 47070
				public static LocString TITLE = "WARNING!";

				// Token: 0x0400B7DF RID: 47071
				public static LocString SIMFAILEDTOLOAD = "A problem occurred loading Oxygen Not Included. This is usually caused by the Visual Studio C++ 2015 runtime being improperly installed on the system. Please exit the game, run Windows Update, and try re-launching Oxygen Not Included.";

				// Token: 0x0400B7E0 RID: 47072
				public static LocString BODY = "We've detected that this computer does not meet the minimum requirements to run Oxygen Not Included. While you may continue with your current specs, the game might not run smoothly for you.\n\nPlease be aware that your experience may suffer as a result.";

				// Token: 0x0400B7E1 RID: 47073
				public static LocString OKBUTTON = "Okay, thanks!";

				// Token: 0x0400B7E2 RID: 47074
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002A31 RID: 10801
			public class SUPPORTWARNINGS
			{
				// Token: 0x0400B7E3 RID: 47075
				public static LocString AUDIO_DRIVERS = "A problem occurred initializing your audio device.\nSorry about that!\n\nThis is usually caused by outdated audio drivers.\n\nPlease visit your audio device manufacturer's website to download the latest drivers.";

				// Token: 0x0400B7E4 RID: 47076
				public static LocString AUDIO_DRIVERS_MORE_INFO = "More Info";

				// Token: 0x0400B7E5 RID: 47077
				public static LocString DUPLICATE_KEY_BINDINGS = "<b>Duplicate key bindings were detected.\nThis may be because your custom key bindings conflicted with a new feature's default key.\nPlease visit the controls screen to ensure your key bindings are set how you like them.</b>\n{0}";

				// Token: 0x0400B7E6 RID: 47078
				public static LocString SAVE_DIRECTORY_READ_ONLY = "A problem occurred while accessing your save directory.\nThis may be because your directory is set to read-only.\n\nPlease ensure your save directory is readable as well as writable and re-launch the game.\n{0}";

				// Token: 0x0400B7E7 RID: 47079
				public static LocString SAVE_DIRECTORY_INSUFFICIENT_SPACE = "There is insufficient disk space to write to your save directory.\n\nPlease free at least 15 MB to give your saves some room to breathe.\n{0}";

				// Token: 0x0400B7E8 RID: 47080
				public static LocString WORLD_GEN_FILES = "A problem occurred while accessing certain game files that will prevent starting new games.\n\nPlease ensure that the directory and files are readable as well as writable and re-launch the game:\n\n{0}";

				// Token: 0x0400B7E9 RID: 47081
				public static LocString WORLD_GEN_FAILURE = "A problem occurred while generating a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with a different seed.";

				// Token: 0x0400B7EA RID: 47082
				public static LocString WORLD_GEN_FAILURE_MIXING = "A problem occurred while trying to mix a world from this seed:\n{0}.\n\nUnfortunately, not all seeds germinate. Please try again with different remix settings or a different seed.";

				// Token: 0x0400B7EB RID: 47083
				public static LocString WORLD_GEN_FAILURE_STORY = "A problem occurred while generating a world from this seed:\n{0}.\n\nNot all story traits were able to be placed. Please try again with a different seed or fewer story traits.";

				// Token: 0x0400B7EC RID: 47084
				public static LocString PLAYER_PREFS_CORRUPTED = "A problem occurred while loading your game options.\nThey have been reset to their default settings.\n\n";

				// Token: 0x0400B7ED RID: 47085
				public static LocString IO_UNAUTHORIZED = "An Unauthorized Access Error occurred when trying to write to disk.\n\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";

				// Token: 0x0400B7EE RID: 47086
				public static LocString IO_UNAUTHORIZED_ONEDRIVE = "An Unauthorized Access Error occurred when trying to write to disk.\n\nOneDrive may be interfering with the game.\n\nPlease check that you have permissions to write to:\n{0}\n\nThis may prevent the game from saving.";

				// Token: 0x0400B7EF RID: 47087
				public static LocString IO_SUFFICIENT_SPACE = "An Insufficient Space Error occurred when trying to write to disk. \n\nPlease free up some space.\n{0}";

				// Token: 0x0400B7F0 RID: 47088
				public static LocString IO_UNKNOWN = "An unknown error occurred when trying to write or access a file.\n{0}";

				// Token: 0x0400B7F1 RID: 47089
				public static LocString MORE_INFO_BUTTON = "More Info";
			}

			// Token: 0x02002A32 RID: 10802
			public class SAVEUPGRADEWARNINGS
			{
				// Token: 0x0400B7F2 RID: 47090
				public static LocString SUDDENMORALEHELPER_TITLE = "MORALE CHANGES";

				// Token: 0x0400B7F3 RID: 47091
				public static LocString SUDDENMORALEHELPER = "Welcome to the Expressive Upgrade! This update introduces a new Morale system that replaces Food and Decor Expectations that were found in previous versions of the game.\n\nThe game you are trying to load was created before this system was introduced, and will need to be updated. You may either:\n\n\n1) Enable the new Morale system in this save, removing Food and Decor Expectations. It's possible that when you load your save your old colony won't meet your Duplicants' new Morale needs, so they'll receive a 5 cycle Morale boost to give you time to adjust.\n\n2) Disable Morale in this save. The new Morale mechanics will still be visible, but won't affect your Duplicants' stress. Food and Decor expectations will no longer exist in this save.";

				// Token: 0x0400B7F4 RID: 47092
				public static LocString SUDDENMORALEHELPER_BUFF = "1) Bring on Morale!";

				// Token: 0x0400B7F5 RID: 47093
				public static LocString SUDDENMORALEHELPER_DISABLE = "2) Disable Morale";

				// Token: 0x0400B7F6 RID: 47094
				public static LocString NEWAUTOMATIONWARNING_TITLE = "AUTOMATION CHANGES";

				// Token: 0x0400B7F7 RID: 47095
				public static LocString NEWAUTOMATIONWARNING = "The following buildings have acquired new automation ports!\n\nTake a moment to check whether these buildings in your colony are now unintentionally connected to existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s.";

				// Token: 0x0400B7F8 RID: 47096
				public static LocString MERGEDOWNCHANGES_TITLE = "BREATH OF FRESH AIR UPDATE CHANGES";

				// Token: 0x0400B7F9 RID: 47097
				public static LocString MERGEDOWNCHANGES = "Oxygen Not Included has had a <b>major update</b> since this save file was created! In addition to the <b>multitude of bug fixes and quality-of-life features</b>, please pay attention to these changes which may affect your existing colony:";

				// Token: 0x0400B7FA RID: 47098
				public static LocString MERGEDOWNCHANGES_FOOD = "•<indent=20px>Fridges are more effective for early-game food storage</indent>\n•<indent=20px><b>Both</b> freezing temperatures and a sterile gas are needed for <b>total food preservation</b>.</indent>";

				// Token: 0x0400B7FB RID: 47099
				public static LocString MERGEDOWNCHANGES_AIRFILTER = "•<indent=20px>" + BUILDINGS.PREFABS.AIRFILTER.NAME + " now requires <b>5w Power</b>.</indent>\n•<indent=20px>Duplicants will get <b>Stinging Eyes</b> from gasses such as chlorine and hydrogen.</indent>";

				// Token: 0x0400B7FC RID: 47100
				public static LocString MERGEDOWNCHANGES_SIMULATION = "•<indent=20px>Many <b>simulation bugs</b> have been fixed.</indent>\n•<indent=20px>This may <b>change the effectiveness</b> of certain contraptions and " + BUILDINGS.PREFABS.STEAMTURBINE2.NAME + " setups.</indent>";

				// Token: 0x0400B7FD RID: 47101
				public static LocString MERGEDOWNCHANGES_BUILDINGS = "•<indent=20px>The <b>" + BUILDINGS.PREFABS.OXYGENMASKSTATION.NAME + "</b> has been added to aid early-game exploration.</indent>\n•<indent=20px>Use the new <b>Meter Valves</b> for precise control of resources in pipes.</indent>";

				// Token: 0x0400B7FE RID: 47102
				public static LocString SPACESCANNERANDTELESCOPECHANGES_TITLE = "JUNE 2023 QoL UPDATE CHANGES";

				// Token: 0x0400B7FF RID: 47103
				public static LocString SPACESCANNERANDTELESCOPECHANGES_SUMMARY = "There have been significant changes to <b>Space Scanners</b> and <b>Telescopes</b> since this save file was created!\n\nMeteor showers have been disabled for 20 cycles to provide time to adapt.";

				// Token: 0x0400B800 RID: 47104
				public static LocString SPACESCANNERANDTELESCOPECHANGES_WARNING = "Please note these changes which may affect your existing colony:\n\n";

				// Token: 0x0400B801 RID: 47105
				public static LocString SPACESCANNERANDTELESCOPECHANGES_SPACESCANNERS = "•<indent=20px>Automation is synced between all Space Scanners targeting the same object.</indent>\n•<indent=20px>Network quality based on the total percentage of sky covered.</indent>\n•<indent=20px>Industrial machinery no longer impacts network quality.</indent>";

				// Token: 0x0400B802 RID: 47106
				public static LocString SPACESCANNERANDTELESCOPECHANGES_TELESCOPES = "•<indent=20px>Telescopes have a symmetrical scanning range.</indent>\n•<indent=20px>Obstructions block visibility from the blocked tile out toward the outer edge of scanning range.</indent>";

				// Token: 0x0400B803 RID: 47107
				public static LocString U50_CHANGES_TITLE = "IMPORTANT CHANGES";

				// Token: 0x0400B804 RID: 47108
				public static LocString U50_CHANGES_SUMMARY = "There have been significant changes to critters since this save file was created! Please check on your ranches.";

				// Token: 0x0400B805 RID: 47109
				public static LocString U50_CHANGES_MOOD = "•<indent=20px>Critter moods have been expanded to include miserable and satisfied states: Miserable stops reproduction. Satisfied gives full metabolism and default reproduction.</indent>";

				// Token: 0x0400B806 RID: 47110
				public static LocString U50_CHANGES_PACU = "•<indent=20px>Pacus have received a number of bug fixes and changes affecting their reproduction: Now correctly Confined when flopping or in less than 8 tiles of liquid. Easier to feed due to a rebalanced diet.</indent>";

				// Token: 0x0400B807 RID: 47111
				public static LocString U50_CHANGES_SUITCHECKPOINTS = "•<indent=20px>Suit checkpoints now have an automation port to disable them so Duplicants can pass through. Some checkpoints may now be unintentionally connected to existing " + BUILDINGS.PREFABS.LOGICWIRE.NAME + "s.";

				// Token: 0x0400B808 RID: 47112
				public static LocString U50_CHANGES_METER_VALVES = "•<indent=20px>Meter valves no longer continuously reset when receiving a green signal.</indent>";
			}
		}

		// Token: 0x02002151 RID: 8529
		public class SANDBOX_TOGGLE
		{
			// Token: 0x04009536 RID: 38198
			public static LocString TOOLTIP_LOCKED = "<b>Sandbox Mode</b> must be unlocked in the options menu before it can be used. {Hotkey}";

			// Token: 0x04009537 RID: 38199
			public static LocString TOOLTIP_UNLOCKED = "Toggle <b>Sandbox Mode</b> {Hotkey}";
		}

		// Token: 0x02002152 RID: 8530
		public class SKILLS_SCREEN
		{
			// Token: 0x04009538 RID: 38200
			public static LocString CURRENT_MORALE = "Current Morale: {0}\nMorale Need: {1}";

			// Token: 0x04009539 RID: 38201
			public static LocString SORT_BY_DUPLICANT = "Duplicants";

			// Token: 0x0400953A RID: 38202
			public static LocString SORT_BY_MORALE = "Morale";

			// Token: 0x0400953B RID: 38203
			public static LocString SORT_BY_EXPERIENCE = "Skill Points";

			// Token: 0x0400953C RID: 38204
			public static LocString SORT_BY_SKILL_AVAILABLE = "Skill Points";

			// Token: 0x0400953D RID: 38205
			public static LocString SORT_BY_HAT = "Hat";

			// Token: 0x0400953E RID: 38206
			public static LocString SELECT_HAT = "<b>SELECT HAT</b>";

			// Token: 0x0400953F RID: 38207
			public static LocString POINTS_AVAILABLE = "<b>SKILL POINTS AVAILABLE</b>";

			// Token: 0x04009540 RID: 38208
			public static LocString MORALE = "<b>Morale</b>";

			// Token: 0x04009541 RID: 38209
			public static LocString MORALE_EXPECTATION = "<b>Morale Need</b>";

			// Token: 0x04009542 RID: 38210
			public static LocString EXPERIENCE = "EXPERIENCE TO NEXT LEVEL";

			// Token: 0x04009543 RID: 38211
			public static LocString EXPERIENCE_TOOLTIP = "{0}exp to next Skill Point";

			// Token: 0x04009544 RID: 38212
			public static LocString NOT_AVAILABLE = "Not available";

			// Token: 0x02002A33 RID: 10803
			public class ASSIGNMENT_REQUIREMENTS
			{
				// Token: 0x0400B809 RID: 47113
				public static LocString EXPECTATION_TARGET_SKILL = "Current Morale: {0}\nSkill Morale Needs: {1}";

				// Token: 0x0400B80A RID: 47114
				public static LocString EXPECTATION_ALERT_TARGET_SKILL = "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";

				// Token: 0x0400B80B RID: 47115
				public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

				// Token: 0x020036A3 RID: 13987
				public class SKILLGROUP_ENABLED
				{
					// Token: 0x0400DAA5 RID: 55973
					public static LocString NAME = "Can perform {0}";

					// Token: 0x0400DAA6 RID: 55974
					public static LocString DESCRIPTION = "Capable of performing <b>{0}</b> skills";
				}

				// Token: 0x020036A4 RID: 13988
				public class MASTERY
				{
					// Token: 0x0400DAA7 RID: 55975
					public static LocString CAN_MASTER = "{0} <b>can learn</b> {1}";

					// Token: 0x0400DAA8 RID: 55976
					public static LocString HAS_MASTERED = "{0} has <b>already learned</b> {1}";

					// Token: 0x0400DAA9 RID: 55977
					public static LocString CANNOT_MASTER = "{0} <b>cannot learn</b> {1}";

					// Token: 0x0400DAAA RID: 55978
					public static LocString STRESS_WARNING_MESSAGE = string.Concat(new string[]
					{
						"Learning {0} will put {1} into a ",
						UI.PRE_KEYWORD,
						"Morale",
						UI.PST_KEYWORD,
						" deficit and cause unnecessary ",
						UI.PRE_KEYWORD,
						"Stress",
						UI.PST_KEYWORD,
						"!"
					});

					// Token: 0x0400DAAB RID: 55979
					public static LocString REQUIRES_MORE_SKILL_POINTS = "    • Not enough " + UI.PRE_KEYWORD + "Skill Points" + UI.PST_KEYWORD;

					// Token: 0x0400DAAC RID: 55980
					public static LocString REQUIRES_PREVIOUS_SKILLS = "    • Missing prerequisite " + UI.PRE_KEYWORD + "Skill" + UI.PST_KEYWORD;

					// Token: 0x0400DAAD RID: 55981
					public static LocString PREVENTED_BY_TRAIT = string.Concat(new string[]
					{
						"    • This Duplicant possesses the ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" Trait and cannot learn this Skill"
					});

					// Token: 0x0400DAAE RID: 55982
					public static LocString SKILL_APTITUDE = string.Concat(new string[]
					{
						"{0} is interested in {1} and will receive a ",
						UI.PRE_KEYWORD,
						"Morale",
						UI.PST_KEYWORD,
						" bonus for learning it!"
					});

					// Token: 0x0400DAAF RID: 55983
					public static LocString SKILL_GRANTED = "{0} has been granted {1} by a Trait, but does not have increased " + UI.FormatAsKeyWord("Morale Requirements") + " from learning it";
				}
			}
		}

		// Token: 0x02002153 RID: 8531
		public class KLEI_INVENTORY_SCREEN
		{
			// Token: 0x04009545 RID: 38213
			public static LocString OPEN_INVENTORY_BUTTON = "Open Klei Inventory";

			// Token: 0x04009546 RID: 38214
			public static LocString ITEM_FACADE_FOR = "This blueprint works with any {ConfigProperName}.";

			// Token: 0x04009547 RID: 38215
			public static LocString ARTABLE_ITEM_FACADE_FOR = "This blueprint works with any {ConfigProperName} of {ArtableQuality} quality.";

			// Token: 0x04009548 RID: 38216
			public static LocString CLOTHING_ITEM_FACADE_FOR = "This blueprint can be used in any outfit.";

			// Token: 0x04009549 RID: 38217
			public static LocString BALLOON_ARTIST_FACADE_FOR = "This blueprint can be used by any Balloon Artist.";

			// Token: 0x0400954A RID: 38218
			public static LocString COLLECTION = "Part of {Collection} collection.";

			// Token: 0x0400954B RID: 38219
			public static LocString COLLECTION_COMING_SOON = "Part of {Collection} collection. Coming soon!";

			// Token: 0x0400954C RID: 38220
			public static LocString ITEM_RARITY_DETAILS = "{RarityName} quality.";

			// Token: 0x0400954D RID: 38221
			public static LocString ITEM_PLAYER_OWNED_AMOUNT = "My colony has {OwnedCount} of these blueprints.";

			// Token: 0x0400954E RID: 38222
			public static LocString ITEM_PLAYER_OWN_NONE = "My colony doesn't have any of these yet.";

			// Token: 0x0400954F RID: 38223
			public static LocString ITEM_PLAYER_OWNED_AMOUNT_ICON = "x{OwnedCount}";

			// Token: 0x04009550 RID: 38224
			public static LocString ITEM_PLAYER_UNLOCKED_BUT_UNOWNABLE = "This blueprint is part of my colony's permanent collection.";

			// Token: 0x04009551 RID: 38225
			public static LocString ITEM_DLC_REQUIRED = "This blueprint is designed for the <i>Spaced Out!</i> DLC.";

			// Token: 0x04009552 RID: 38226
			public static LocString ITEM_UNKNOWN_NAME = "Uh oh!";

			// Token: 0x04009553 RID: 38227
			public static LocString ITEM_UNKNOWN_DESCRIPTION = "Hmm. Looks like this blueprint is missing from the supply closet. Perhaps due to a temporal anomaly...";

			// Token: 0x04009554 RID: 38228
			public static LocString SEARCH_PLACEHOLDER = "Search";

			// Token: 0x04009555 RID: 38229
			public static LocString CLEAR_SEARCH_BUTTON_TOOLTIP = "Clear search";

			// Token: 0x04009556 RID: 38230
			public static LocString TOOLTIP_VIEW_ALL_ITEMS = "Filter: Showing all items\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x04009557 RID: 38231
			public static LocString TOOLTIP_VIEW_OWNED_ONLY = "Filter: Showing owned items only\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x04009558 RID: 38232
			public static LocString TOOLTIP_VIEW_DOUBLES_ONLY = "Filter: Showing multiples owned only\n\n" + UI.CLICK(UI.ClickType.Click) + " to toggle";

			// Token: 0x02002A34 RID: 10804
			public static class BARTERING
			{
				// Token: 0x0400B80C RID: 47116
				public static LocString TOOLTIP_ACTION_INVALID_OFFLINE = "Currently unavailable";

				// Token: 0x0400B80D RID: 47117
				public static LocString BUY = "PRINT";

				// Token: 0x0400B80E RID: 47118
				public static LocString TOOLTIP_BUY_ACTIVE = "This item requires {0} spools of Filament to print";

				// Token: 0x0400B80F RID: 47119
				public static LocString TOOLTIP_UNBUYABLE = "This item is unprintable";

				// Token: 0x0400B810 RID: 47120
				public static LocString TOOLTIP_UNBUYABLE_BETA = "This item may be printable after the public testing period";

				// Token: 0x0400B811 RID: 47121
				public static LocString TOOLTIP_UNBUYABLE_ALREADY_OWNED = "My colony already owns one of these blueprints";

				// Token: 0x0400B812 RID: 47122
				public static LocString TOOLTIP_BUY_CANT_AFFORD = "Filament supply is too low";

				// Token: 0x0400B813 RID: 47123
				public static LocString SELL = "RECYCLE";

				// Token: 0x0400B814 RID: 47124
				public static LocString TOOLTIP_SELL_ACTIVE = "Recycle this blueprint for {0} spools of Filament";

				// Token: 0x0400B815 RID: 47125
				public static LocString TOOLTIP_UNSELLABLE = "This item is non-recyclable";

				// Token: 0x0400B816 RID: 47126
				public static LocString TOOLTIP_NONE_TO_SELL = "My colony does not own any of these blueprints";

				// Token: 0x0400B817 RID: 47127
				public static LocString CANCEL = "CANCEL";

				// Token: 0x0400B818 RID: 47128
				public static LocString CONFIRM_RECYCLE_HEADER = "RECYCLE INTO FILAMENT?";

				// Token: 0x0400B819 RID: 47129
				public static LocString CONFIRM_PRINT_HEADER = "PRINT ITEM?";

				// Token: 0x0400B81A RID: 47130
				public static LocString OFFLINE_LABEL = "Not connected to Klei server";

				// Token: 0x0400B81B RID: 47131
				public static LocString LOADING = "Connecting to server...";

				// Token: 0x0400B81C RID: 47132
				public static LocString TRANSACTION_ERROR = "Whoops! Something's gone wrong.";

				// Token: 0x0400B81D RID: 47133
				public static LocString ACTION_DESCRIPTION_RECYCLE = "Recycling this blueprint will recover Filament that my colony can use to print other items.\n\nOne copy of this blueprint will be removed from my colony's supply closet.";

				// Token: 0x0400B81E RID: 47134
				public static LocString ACTION_DESCRIPTION_PRINT = "Producing this blueprint requires Filament from my colony's supply.\n\nOne copy of this blueprint will be extruded at a time.";

				// Token: 0x0400B81F RID: 47135
				public static LocString WALLET_TOOLTIP = "{0} spool of Filament available";

				// Token: 0x0400B820 RID: 47136
				public static LocString WALLET_PLURAL_TOOLTIP = "{0} spools of Filament available";

				// Token: 0x0400B821 RID: 47137
				public static LocString TRANSACTION_COMPLETE_HEADER = "SUCCESS!";

				// Token: 0x0400B822 RID: 47138
				public static LocString TRANSACTION_INCOMPLETE_HEADER = "ERROR";

				// Token: 0x0400B823 RID: 47139
				public static LocString PURCHASE_SUCCESS = "One copy of this blueprint has been added to my colony's supply closet.";

				// Token: 0x0400B824 RID: 47140
				public static LocString SELL_SUCCESS = "The Filament recovered from recycling this item can now be used to print other items.";
			}

			// Token: 0x02002A35 RID: 10805
			public static class CATEGORIES
			{
				// Token: 0x0400B825 RID: 47141
				public static LocString EQUIPMENT = "Equipment";

				// Token: 0x0400B826 RID: 47142
				public static LocString DUPE_TOPS = "Tops & Onesies";

				// Token: 0x0400B827 RID: 47143
				public static LocString DUPE_BOTTOMS = "Bottoms";

				// Token: 0x0400B828 RID: 47144
				public static LocString DUPE_GLOVES = "Gloves";

				// Token: 0x0400B829 RID: 47145
				public static LocString DUPE_SHOES = "Footwear";

				// Token: 0x0400B82A RID: 47146
				public static LocString DUPE_HATS = "Headgear";

				// Token: 0x0400B82B RID: 47147
				public static LocString DUPE_ACCESSORIES = "Accessories";

				// Token: 0x0400B82C RID: 47148
				public static LocString ATMO_SUIT_HELMET = "Atmo Helmets";

				// Token: 0x0400B82D RID: 47149
				public static LocString ATMO_SUIT_BODY = "Atmo Suits";

				// Token: 0x0400B82E RID: 47150
				public static LocString ATMO_SUIT_GLOVES = "Atmo Gloves";

				// Token: 0x0400B82F RID: 47151
				public static LocString ATMO_SUIT_BELT = "Atmo Belts";

				// Token: 0x0400B830 RID: 47152
				public static LocString ATMO_SUIT_SHOES = "Atmo Boots";

				// Token: 0x0400B831 RID: 47153
				public static LocString PRIMOGARB = "Primo Garb";

				// Token: 0x0400B832 RID: 47154
				public static LocString ATMOSUITS = "Atmo Suits";

				// Token: 0x0400B833 RID: 47155
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400B834 RID: 47156
				public static LocString CRITTERS = "Critters";

				// Token: 0x0400B835 RID: 47157
				public static LocString SWEEPYS = "Sweepys";

				// Token: 0x0400B836 RID: 47158
				public static LocString DUPLICANTS = "Duplicants";

				// Token: 0x0400B837 RID: 47159
				public static LocString ARTWORKS = "Artwork";

				// Token: 0x0400B838 RID: 47160
				public static LocString MONUMENTPARTS = "Monuments";

				// Token: 0x0400B839 RID: 47161
				public static LocString JOY_RESPONSE = "Overjoyed Responses";

				// Token: 0x020036A5 RID: 13989
				public static class JOY_RESPONSES
				{
					// Token: 0x0400DAB0 RID: 55984
					public static LocString BALLOON_ARTIST = "Balloon Artist";
				}
			}

			// Token: 0x02002A36 RID: 10806
			public static class TOP_LEVEL_CATEGORIES
			{
				// Token: 0x0400B83A RID: 47162
				public static LocString UNRELEASED = "DEBUG UNRELEASED";

				// Token: 0x0400B83B RID: 47163
				public static LocString CLOTHING_TOPS = "Tops & Onesies";

				// Token: 0x0400B83C RID: 47164
				public static LocString CLOTHING_BOTTOMS = "Bottoms";

				// Token: 0x0400B83D RID: 47165
				public static LocString CLOTHING_GLOVES = "Gloves";

				// Token: 0x0400B83E RID: 47166
				public static LocString CLOTHING_SHOES = "Footwear";

				// Token: 0x0400B83F RID: 47167
				public static LocString ATMOSUITS = "Atmo Suits";

				// Token: 0x0400B840 RID: 47168
				public static LocString BUILDINGS = "Buildings";

				// Token: 0x0400B841 RID: 47169
				public static LocString WALLPAPERS = "Wallpapers";

				// Token: 0x0400B842 RID: 47170
				public static LocString ARTWORK = "Artwork";

				// Token: 0x0400B843 RID: 47171
				public static LocString JOY_RESPONSES = "Joy Responses";
			}

			// Token: 0x02002A37 RID: 10807
			public static class SUBCATEGORIES
			{
				// Token: 0x0400B844 RID: 47172
				public static LocString UNRELEASED = "DEBUG UNRELEASED";

				// Token: 0x0400B845 RID: 47173
				public static LocString UNCATEGORIZED = "BUG: UNCATEGORIZED";

				// Token: 0x0400B846 RID: 47174
				public static LocString YAML = "YAML";

				// Token: 0x0400B847 RID: 47175
				public static LocString DEFAULT = "Default";

				// Token: 0x0400B848 RID: 47176
				public static LocString JOY_BALLOON = "Balloons";

				// Token: 0x0400B849 RID: 47177
				public static LocString JOY_STICKER = "Stickers";

				// Token: 0x0400B84A RID: 47178
				public static LocString PRIMO_GARB = "Primo Garb";

				// Token: 0x0400B84B RID: 47179
				public static LocString CLOTHING_TOPS_BASIC = "Standard Shirts";

				// Token: 0x0400B84C RID: 47180
				public static LocString CLOTHING_TOPS_TSHIRT = "Tees";

				// Token: 0x0400B84D RID: 47181
				public static LocString CLOTHING_TOPS_FANCY = "Specialty Tops";

				// Token: 0x0400B84E RID: 47182
				public static LocString CLOTHING_TOPS_JACKET = "Jackets";

				// Token: 0x0400B84F RID: 47183
				public static LocString CLOTHING_TOPS_UNDERSHIRT = "Undershirts";

				// Token: 0x0400B850 RID: 47184
				public static LocString CLOTHING_TOPS_DRESS = "Dresses and Onesies";

				// Token: 0x0400B851 RID: 47185
				public static LocString CLOTHING_BOTTOMS_BASIC = "Standard Pants";

				// Token: 0x0400B852 RID: 47186
				public static LocString CLOTHING_BOTTOMS_FANCY = "Fancy Pants";

				// Token: 0x0400B853 RID: 47187
				public static LocString CLOTHING_BOTTOMS_SHORTS = "Shorts";

				// Token: 0x0400B854 RID: 47188
				public static LocString CLOTHING_BOTTOMS_SKIRTS = "Skirts";

				// Token: 0x0400B855 RID: 47189
				public static LocString CLOTHING_BOTTOMS_UNDERWEAR = "Underwear";

				// Token: 0x0400B856 RID: 47190
				public static LocString CLOTHING_GLOVES_BASIC = "Standard Gloves";

				// Token: 0x0400B857 RID: 47191
				public static LocString CLOTHING_GLOVES_FORMAL = "Fancy Gloves";

				// Token: 0x0400B858 RID: 47192
				public static LocString CLOTHING_GLOVES_SHORT = "Short Gloves";

				// Token: 0x0400B859 RID: 47193
				public static LocString CLOTHING_GLOVES_PRINTS = "Specialty Gloves";

				// Token: 0x0400B85A RID: 47194
				public static LocString CLOTHING_SHOES_BASIC = "Standard Shoes";

				// Token: 0x0400B85B RID: 47195
				public static LocString CLOTHING_SHOE_SOCKS = "Socks";

				// Token: 0x0400B85C RID: 47196
				public static LocString CLOTHING_SHOES_FANCY = "Fancy Shoes";

				// Token: 0x0400B85D RID: 47197
				public static LocString ATMOSUIT_HELMETS_BASIC = "Atmo Helmets";

				// Token: 0x0400B85E RID: 47198
				public static LocString ATMOSUIT_HELMETS_FANCY = "Fancy Atmo Helmets";

				// Token: 0x0400B85F RID: 47199
				public static LocString ATMOSUIT_BODIES_BASIC = "Atmo Suits";

				// Token: 0x0400B860 RID: 47200
				public static LocString ATMOSUIT_BODIES_FANCY = "Fancy Atmo Suits";

				// Token: 0x0400B861 RID: 47201
				public static LocString ATMOSUIT_GLOVES_BASIC = "Atmo Gloves";

				// Token: 0x0400B862 RID: 47202
				public static LocString ATMOSUIT_GLOVES_FANCY = "Fancy Atmo Gloves";

				// Token: 0x0400B863 RID: 47203
				public static LocString ATMOSUIT_BELTS_BASIC = "Atmo Belts";

				// Token: 0x0400B864 RID: 47204
				public static LocString ATMOSUIT_BELTS_FANCY = "Fancy Atmo Belts";

				// Token: 0x0400B865 RID: 47205
				public static LocString ATMOSUIT_SHOES_BASIC = "Atmo Boots";

				// Token: 0x0400B866 RID: 47206
				public static LocString ATMOSUIT_SHOES_FANCY = "Fancy Atmo Boots";

				// Token: 0x0400B867 RID: 47207
				public static LocString BUILDING_WALLPAPER_BASIC = "Solid Wallpapers";

				// Token: 0x0400B868 RID: 47208
				public static LocString BUILDING_WALLPAPER_FANCY = "Geometric Wallpapers";

				// Token: 0x0400B869 RID: 47209
				public static LocString BUILDING_WALLPAPER_PRINTS = "Patterned Wallpapers";

				// Token: 0x0400B86A RID: 47210
				public static LocString BUILDING_CANVAS_STANDARD = "Standard Canvas";

				// Token: 0x0400B86B RID: 47211
				public static LocString BUILDING_CANVAS_PORTRAIT = "Portrait Canvas";

				// Token: 0x0400B86C RID: 47212
				public static LocString BUILDING_CANVAS_LANDSCAPE = "Landscape Canvas";

				// Token: 0x0400B86D RID: 47213
				public static LocString BUILDING_SCULPTURE = "Sculptures";

				// Token: 0x0400B86E RID: 47214
				public static LocString MONUMENT_PARTS = "Monuments";

				// Token: 0x0400B86F RID: 47215
				public static LocString BUILDINGS_FLOWER_VASE = "Pots and Planters";

				// Token: 0x0400B870 RID: 47216
				public static LocString BUILDINGS_BED_COT = "Cots";

				// Token: 0x0400B871 RID: 47217
				public static LocString BUILDINGS_BED_LUXURY = "Comfy Beds";

				// Token: 0x0400B872 RID: 47218
				public static LocString BUILDING_CEILING_LIGHT = "Lights";

				// Token: 0x0400B873 RID: 47219
				public static LocString BUILDINGS_STORAGE = "Storage";

				// Token: 0x0400B874 RID: 47220
				public static LocString BUILDINGS_INDUSTRIAL = "Industrial";

				// Token: 0x0400B875 RID: 47221
				public static LocString BUILDINGS_FOOD = "Cooking";

				// Token: 0x0400B876 RID: 47222
				public static LocString BUILDINGS_WASHROOM = "Sanitation";

				// Token: 0x0400B877 RID: 47223
				public static LocString BUILDINGS_RANCHING = "Agricultural";

				// Token: 0x0400B878 RID: 47224
				public static LocString BUILDINGS_RECREATION = "Recreation and Decor";

				// Token: 0x0400B879 RID: 47225
				public static LocString BUILDINGS_PRINTING_POD = "Printing Pods";
			}

			// Token: 0x02002A38 RID: 10808
			public static class COLUMN_HEADERS
			{
				// Token: 0x0400B87A RID: 47226
				public static LocString CATEGORY_HEADER = "BLUEPRINTS";

				// Token: 0x0400B87B RID: 47227
				public static LocString ITEMS_HEADER = "Items";

				// Token: 0x0400B87C RID: 47228
				public static LocString DETAILS_HEADER = "Details";
			}
		}

		// Token: 0x02002154 RID: 8532
		public class ITEM_DROP_SCREEN
		{
			// Token: 0x04009559 RID: 38233
			public static LocString THANKS_FOR_PLAYING = "New blueprints unlocked!";

			// Token: 0x0400955A RID: 38234
			public static LocString WEB_REWARDS_AVAILABLE = "Rewards available online!";

			// Token: 0x0400955B RID: 38235
			public static LocString NOTHING_AVAILABLE = "All available blueprints claimed";

			// Token: 0x0400955C RID: 38236
			public static LocString OPEN_URL_BUTTON = "CLAIM";

			// Token: 0x0400955D RID: 38237
			public static LocString PRINT_ITEM_BUTTON = "PRINT";

			// Token: 0x0400955E RID: 38238
			public static LocString DISMISS_BUTTON = "DISMISS";

			// Token: 0x0400955F RID: 38239
			public static LocString ERROR_CANNOTLOADITEM = "Whoops! Something's gone wrong.";

			// Token: 0x04009560 RID: 38240
			public static LocString UNOPENED_ITEM_COUNT = "{0} unclaimed blueprints";

			// Token: 0x04009561 RID: 38241
			public static LocString UNOPENED_ITEM = "{0} unclaimed blueprint";

			// Token: 0x02002A39 RID: 10809
			public static class IN_GAME_BUTTON
			{
				// Token: 0x0400B87D RID: 47229
				public static LocString TOOLTIP_ITEMS_AVAILABLE = "Unlock new blueprints";

				// Token: 0x0400B87E RID: 47230
				public static LocString TOOLTIP_ERROR_NO_ITEMS = "No new blueprints to unlock";
			}
		}

		// Token: 0x02002155 RID: 8533
		public class OUTFIT_BROWSER_SCREEN
		{
			// Token: 0x04009562 RID: 38242
			public static LocString BUTTON_ADD_OUTFIT = "New Outfit";

			// Token: 0x04009563 RID: 38243
			public static LocString BUTTON_PICK_OUTFIT = "Assign Outfit";

			// Token: 0x04009564 RID: 38244
			public static LocString TOOLTIP_PICK_OUTFIT_ERROR_LOCKED = "Cannot assign this outfit to {MinionName} because my colony doesn't have all of these blueprints yet";

			// Token: 0x04009565 RID: 38245
			public static LocString BUTTON_EDIT_OUTFIT = "Restyle Outfit";

			// Token: 0x04009566 RID: 38246
			public static LocString BUTTON_COPY_OUTFIT = "Copy Outfit";

			// Token: 0x04009567 RID: 38247
			public static LocString TOOLTIP_DELETE_OUTFIT = "Delete Outfit";

			// Token: 0x04009568 RID: 38248
			public static LocString TOOLTIP_DELETE_OUTFIT_ERROR_READONLY = "This outfit cannot be deleted";

			// Token: 0x04009569 RID: 38249
			public static LocString TOOLTIP_RENAME_OUTFIT = "Rename Outfit";

			// Token: 0x0400956A RID: 38250
			public static LocString TOOLTIP_RENAME_OUTFIT_ERROR_READONLY = "This outfit cannot be renamed";

			// Token: 0x0400956B RID: 38251
			public static LocString TOOLTIP_FILTER_BY_CLOTHING = "View your Clothing Outfits";

			// Token: 0x0400956C RID: 38252
			public static LocString TOOLTIP_FILTER_BY_ATMO_SUITS = "View your Atmo Suit Outfits";

			// Token: 0x02002A3A RID: 10810
			public static class COLUMN_HEADERS
			{
				// Token: 0x0400B87F RID: 47231
				public static LocString GALLERY_HEADER = "OUTFITS";

				// Token: 0x0400B880 RID: 47232
				public static LocString MINION_GALLERY_HEADER = "WARDROBE";

				// Token: 0x0400B881 RID: 47233
				public static LocString DETAILS_HEADER = "Preview";
			}

			// Token: 0x02002A3B RID: 10811
			public class DELETE_WARNING_POPUP
			{
				// Token: 0x0400B882 RID: 47234
				public static LocString HEADER = "Delete \"{OutfitName}\"?";

				// Token: 0x0400B883 RID: 47235
				public static LocString BODY = "Are you sure you want to delete \"{OutfitName}\"?\n\nAny Duplicants assigned to wear this outfit on spawn will be printed wearing their default outfit instead. Existing Duplicants in saved games won't be affected.\n\nThis <b>cannot</b> be undone.";

				// Token: 0x0400B884 RID: 47236
				public static LocString BUTTON_YES_DELETE = "Yes, delete outfit";

				// Token: 0x0400B885 RID: 47237
				public static LocString BUTTON_DONT_DELETE = "Cancel";
			}

			// Token: 0x02002A3C RID: 10812
			public class RENAME_POPUP
			{
				// Token: 0x0400B886 RID: 47238
				public static LocString HEADER = "RENAME OUTFIT";
			}
		}

		// Token: 0x02002156 RID: 8534
		public class LOCKER_MENU
		{
			// Token: 0x0400956D RID: 38253
			public static LocString TITLE = "SUPPLY CLOSET";

			// Token: 0x0400956E RID: 38254
			public static LocString BUTTON_INVENTORY = "All";

			// Token: 0x0400956F RID: 38255
			public static LocString BUTTON_INVENTORY_DESCRIPTION = "View all of my colony's blueprints";

			// Token: 0x04009570 RID: 38256
			public static LocString BUTTON_DUPLICANTS = "Duplicants";

			// Token: 0x04009571 RID: 38257
			public static LocString BUTTON_DUPLICANTS_DESCRIPTION = "Manage individual Duplicants' outfits";

			// Token: 0x04009572 RID: 38258
			public static LocString BUTTON_OUTFITS = "Wardrobe";

			// Token: 0x04009573 RID: 38259
			public static LocString BUTTON_OUTFITS_DESCRIPTION = "Manage my colony's collection of outfits";

			// Token: 0x04009574 RID: 38260
			public static LocString DEFAULT_DESCRIPTION = "Select a screen";

			// Token: 0x04009575 RID: 38261
			public static LocString BUTTON_CLAIM = "Claim Blueprints";

			// Token: 0x04009576 RID: 38262
			public static LocString BUTTON_CLAIM_DESCRIPTION = "Claim any available blueprints";

			// Token: 0x04009577 RID: 38263
			public static LocString BUTTON_CLAIM_NONE_DESCRIPTION = "All available blueprints claimed";

			// Token: 0x04009578 RID: 38264
			public static LocString UNOPENED_ITEMS_TOOLTIP = "New blueprints available";

			// Token: 0x04009579 RID: 38265
			public static LocString UNOPENED_ITEMS_NONE_TOOLTIP = "All available blueprints claimed";

			// Token: 0x0400957A RID: 38266
			public static LocString OFFLINE_ICON_TOOLTIP = "Not connected to Klei server";
		}

		// Token: 0x02002157 RID: 8535
		public class LOCKER_NAVIGATOR
		{
			// Token: 0x0400957B RID: 38267
			public static LocString BUTTON_BACK = "BACK";

			// Token: 0x0400957C RID: 38268
			public static LocString BUTTON_CLOSE = "CLOSE";

			// Token: 0x02002A3D RID: 10813
			public class DATA_COLLECTION_WARNING_POPUP
			{
				// Token: 0x0400B887 RID: 47239
				public static LocString HEADER = "Data Communication is Disabled";

				// Token: 0x0400B888 RID: 47240
				public static LocString BODY = "Data Communication must be enabled in order to access newly unlocked items. This setting can be found in the Options menu.\n\nExisting item unlocks can still be used while Data Communication is disabled.";

				// Token: 0x0400B889 RID: 47241
				public static LocString BUTTON_OK = "Continue";

				// Token: 0x0400B88A RID: 47242
				public static LocString BUTTON_OPEN_SETTINGS = "Options Menu";
			}
		}

		// Token: 0x02002158 RID: 8536
		public class JOY_RESPONSE_DESIGNER_SCREEN
		{
			// Token: 0x0400957D RID: 38269
			public static LocString CATEGORY_HEADER = "OVERJOYED RESPONSES";

			// Token: 0x0400957E RID: 38270
			public static LocString BUTTON_APPLY_TO_MINION = "Assign to {MinionName}";

			// Token: 0x0400957F RID: 38271
			public static LocString TOOLTIP_NO_FACADES_FOR_JOY_TRAIT = "There aren't any blueprints for {JoyResponseType} Duplicants yet";

			// Token: 0x04009580 RID: 38272
			public static LocString TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED = "This Overjoyed Response blueprint cannot be assigned because my colony doesn't own it yet";

			// Token: 0x02002A3E RID: 10814
			public class CHANGES_NOT_SAVED_WARNING_POPUP
			{
				// Token: 0x0400B88B RID: 47243
				public static LocString HEADER = "Discard changes to {MinionName}'s Overjoyed Response?";
			}
		}

		// Token: 0x02002159 RID: 8537
		public class OUTFIT_DESIGNER_SCREEN
		{
			// Token: 0x04009581 RID: 38273
			public static LocString CATEGORY_HEADER = "CLOTHING";

			// Token: 0x02002A3F RID: 10815
			public class MINION_INSTANCE
			{
				// Token: 0x0400B88C RID: 47244
				public static LocString BUTTON_APPLY_TO_MINION = "Assign to {MinionName}";

				// Token: 0x0400B88D RID: 47245
				public static LocString BUTTON_APPLY_TO_TEMPLATE = "Apply to Template";

				// Token: 0x020036A6 RID: 13990
				public class APPLY_TEMPLATE_POPUP
				{
					// Token: 0x0400DAB1 RID: 55985
					public static LocString HEADER = "SAVE AS TEMPLATE";

					// Token: 0x0400DAB2 RID: 55986
					public static LocString DESC_SAVE_EXISTING = "\"{OutfitName}\" will be updated and applied to {MinionName} on save.";

					// Token: 0x0400DAB3 RID: 55987
					public static LocString DESC_SAVE_NEW = "A new outfit named \"{OutfitName}\" will be created and assigned to {MinionName} on save.";

					// Token: 0x0400DAB4 RID: 55988
					public static LocString BUTTON_SAVE_EXISTING = "Update Outfit";

					// Token: 0x0400DAB5 RID: 55989
					public static LocString BUTTON_SAVE_NEW = "Save New Outfit";
				}
			}

			// Token: 0x02002A40 RID: 10816
			public class OUTFIT_TEMPLATE
			{
				// Token: 0x0400B88E RID: 47246
				public static LocString BUTTON_SAVE = "Save Template";

				// Token: 0x0400B88F RID: 47247
				public static LocString BUTTON_COPY = "Save a Copy";

				// Token: 0x0400B890 RID: 47248
				public static LocString TOOLTIP_SAVE_ERROR_LOCKED = "Cannot save this outfit because my colony doesn't own all of its blueprints yet";

				// Token: 0x0400B891 RID: 47249
				public static LocString TOOLTIP_SAVE_ERROR_READONLY = "This wardrobe staple cannot be altered\n\nMake a copy to save your changes";
			}

			// Token: 0x02002A41 RID: 10817
			public class CHANGES_NOT_SAVED_WARNING_POPUP
			{
				// Token: 0x0400B892 RID: 47250
				public static LocString HEADER = "Discard changes to \"{OutfitName}\"?";

				// Token: 0x0400B893 RID: 47251
				public static LocString BODY = "There are unsaved changes which will be lost if you exit now.\n\nAre you sure you want to discard your changes?";

				// Token: 0x0400B894 RID: 47252
				public static LocString BUTTON_DISCARD = "Yes, discard changes";

				// Token: 0x0400B895 RID: 47253
				public static LocString BUTTON_RETURN = "Cancel";
			}

			// Token: 0x02002A42 RID: 10818
			public class COPY_POPUP
			{
				// Token: 0x0400B896 RID: 47254
				public static LocString HEADER = "RENAME COPY";
			}
		}

		// Token: 0x0200215A RID: 8538
		public class OUTFIT_NAME
		{
			// Token: 0x04009582 RID: 38274
			public static LocString NEW = "Custom Outfit";

			// Token: 0x04009583 RID: 38275
			public static LocString COPY_OF = "Copy of {OutfitName}";

			// Token: 0x04009584 RID: 38276
			public static LocString RESOLVE_CONFLICT = "{OutfitName} ({ConflictNumber})";

			// Token: 0x04009585 RID: 38277
			public static LocString ERROR_NAME_EXISTS = "There's already an outfit named \"{OutfitName}\"";

			// Token: 0x04009586 RID: 38278
			public static LocString MINIONS_OUTFIT = "{MinionName}'s Current Outfit";

			// Token: 0x04009587 RID: 38279
			public static LocString NONE = "Default Outfit";

			// Token: 0x04009588 RID: 38280
			public static LocString NONE_JOY_RESPONSE = "Default Overjoyed Response";

			// Token: 0x04009589 RID: 38281
			public static LocString NONE_ATMO_SUIT = "Default Atmo Suit";
		}

		// Token: 0x0200215B RID: 8539
		public class OUTFIT_DESCRIPTION
		{
			// Token: 0x0400958A RID: 38282
			public static LocString CONTAINS_NON_OWNED_ITEMS = "This outfit can only be worn once my colony has access to all of its blueprints.";

			// Token: 0x0400958B RID: 38283
			public static LocString NO_JOY_RESPONSE_NAME = "Default Overjoyed Response";

			// Token: 0x0400958C RID: 38284
			public static LocString NO_JOY_RESPONSE_DESC = "Default response to an overjoyed state.";
		}

		// Token: 0x0200215C RID: 8540
		public class MINION_BROWSER_SCREEN
		{
			// Token: 0x0400958D RID: 38285
			public static LocString CATEGORY_HEADER = "DUPLICANTS";

			// Token: 0x0400958E RID: 38286
			public static LocString BUTTON_CHANGE_OUTFIT = "Open Wardrobe";

			// Token: 0x0400958F RID: 38287
			public static LocString BUTTON_EDIT_OUTFIT_ITEMS = "Restyle Outfit";

			// Token: 0x04009590 RID: 38288
			public static LocString BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS = "Restyle Atmo Suit";

			// Token: 0x04009591 RID: 38289
			public static LocString BUTTON_EDIT_JOY_RESPONSE = "Restyle Overjoyed Response";

			// Token: 0x04009592 RID: 38290
			public static LocString OUTFIT_TYPE_CLOTHING = "CLOTHING";

			// Token: 0x04009593 RID: 38291
			public static LocString OUTFIT_TYPE_JOY_RESPONSE = "OVERJOYED RESPONSE";

			// Token: 0x04009594 RID: 38292
			public static LocString OUTFIT_TYPE_ATMOSUIT = "ATMO SUIT";

			// Token: 0x04009595 RID: 38293
			public static LocString TOOLTIP_FROM_DLC = "This Duplicant is part of {0} DLC";
		}

		// Token: 0x0200215D RID: 8541
		public class PERMIT_RARITY
		{
			// Token: 0x04009596 RID: 38294
			public static readonly LocString UNKNOWN = "Unknown";

			// Token: 0x04009597 RID: 38295
			public static readonly LocString UNIVERSAL = "Universal";

			// Token: 0x04009598 RID: 38296
			public static readonly LocString LOYALTY = "<color=#FFB037>Loyalty</color>";

			// Token: 0x04009599 RID: 38297
			public static readonly LocString COMMON = "<color=#97B2B9>Common</color>";

			// Token: 0x0400959A RID: 38298
			public static readonly LocString DECENT = "<color=#81EBDE>Decent</color>";

			// Token: 0x0400959B RID: 38299
			public static readonly LocString NIFTY = "<color=#71E379>Nifty</color>";

			// Token: 0x0400959C RID: 38300
			public static readonly LocString SPLENDID = "<color=#FF6DE7>Splendid</color>";
		}

		// Token: 0x0200215E RID: 8542
		public class OUTFITS
		{
			// Token: 0x02002A43 RID: 10819
			public class BASIC_BLACK
			{
				// Token: 0x0400B897 RID: 47255
				public static LocString NAME = "Basic Black Outfit";
			}

			// Token: 0x02002A44 RID: 10820
			public class BASIC_WHITE
			{
				// Token: 0x0400B898 RID: 47256
				public static LocString NAME = "Basic White Outfit";
			}

			// Token: 0x02002A45 RID: 10821
			public class BASIC_RED
			{
				// Token: 0x0400B899 RID: 47257
				public static LocString NAME = "Basic Red Outfit";
			}

			// Token: 0x02002A46 RID: 10822
			public class BASIC_ORANGE
			{
				// Token: 0x0400B89A RID: 47258
				public static LocString NAME = "Basic Orange Outfit";
			}

			// Token: 0x02002A47 RID: 10823
			public class BASIC_YELLOW
			{
				// Token: 0x0400B89B RID: 47259
				public static LocString NAME = "Basic Yellow Outfit";
			}

			// Token: 0x02002A48 RID: 10824
			public class BASIC_GREEN
			{
				// Token: 0x0400B89C RID: 47260
				public static LocString NAME = "Basic Green Outfit";
			}

			// Token: 0x02002A49 RID: 10825
			public class BASIC_AQUA
			{
				// Token: 0x0400B89D RID: 47261
				public static LocString NAME = "Basic Aqua Outfit";
			}

			// Token: 0x02002A4A RID: 10826
			public class BASIC_PURPLE
			{
				// Token: 0x0400B89E RID: 47262
				public static LocString NAME = "Basic Purple Outfit";
			}

			// Token: 0x02002A4B RID: 10827
			public class BASIC_PINK_ORCHID
			{
				// Token: 0x0400B89F RID: 47263
				public static LocString NAME = "Basic Bubblegum Outfit";
			}

			// Token: 0x02002A4C RID: 10828
			public class BASIC_DEEPRED
			{
				// Token: 0x0400B8A0 RID: 47264
				public static LocString NAME = "Team Captain Outfit";
			}

			// Token: 0x02002A4D RID: 10829
			public class BASIC_BLUE_COBALT
			{
				// Token: 0x0400B8A1 RID: 47265
				public static LocString NAME = "True Blue Outfit";
			}

			// Token: 0x02002A4E RID: 10830
			public class BASIC_PINK_FLAMINGO
			{
				// Token: 0x0400B8A2 RID: 47266
				public static LocString NAME = "Pep Rally Outfit";
			}

			// Token: 0x02002A4F RID: 10831
			public class BASIC_GREEN_KELLY
			{
				// Token: 0x0400B8A3 RID: 47267
				public static LocString NAME = "Go Team! Outfit";
			}

			// Token: 0x02002A50 RID: 10832
			public class BASIC_GREY_CHARCOAL
			{
				// Token: 0x0400B8A4 RID: 47268
				public static LocString NAME = "Underdog Outfit";
			}

			// Token: 0x02002A51 RID: 10833
			public class BASIC_LEMON
			{
				// Token: 0x0400B8A5 RID: 47269
				public static LocString NAME = "Team Hype Outfit";
			}

			// Token: 0x02002A52 RID: 10834
			public class BASIC_SATSUMA
			{
				// Token: 0x0400B8A6 RID: 47270
				public static LocString NAME = "Superfan Outfit";
			}

			// Token: 0x02002A53 RID: 10835
			public class JELLYPUFF_BLUEBERRY
			{
				// Token: 0x0400B8A7 RID: 47271
				public static LocString NAME = "Blueberry Jelly Outfit";
			}

			// Token: 0x02002A54 RID: 10836
			public class JELLYPUFF_GRAPE
			{
				// Token: 0x0400B8A8 RID: 47272
				public static LocString NAME = "Grape Jelly Outfit";
			}

			// Token: 0x02002A55 RID: 10837
			public class JELLYPUFF_LEMON
			{
				// Token: 0x0400B8A9 RID: 47273
				public static LocString NAME = "Lemon Jelly Outfit";
			}

			// Token: 0x02002A56 RID: 10838
			public class JELLYPUFF_LIME
			{
				// Token: 0x0400B8AA RID: 47274
				public static LocString NAME = "Lime Jelly Outfit";
			}

			// Token: 0x02002A57 RID: 10839
			public class JELLYPUFF_SATSUMA
			{
				// Token: 0x0400B8AB RID: 47275
				public static LocString NAME = "Satsuma Jelly Outfit";
			}

			// Token: 0x02002A58 RID: 10840
			public class JELLYPUFF_STRAWBERRY
			{
				// Token: 0x0400B8AC RID: 47276
				public static LocString NAME = "Strawberry Jelly Outfit";
			}

			// Token: 0x02002A59 RID: 10841
			public class JELLYPUFF_WATERMELON
			{
				// Token: 0x0400B8AD RID: 47277
				public static LocString NAME = "Watermelon Jelly Outfit";
			}

			// Token: 0x02002A5A RID: 10842
			public class ATHLETE
			{
				// Token: 0x0400B8AE RID: 47278
				public static LocString NAME = "Racing Outfit";
			}

			// Token: 0x02002A5B RID: 10843
			public class CIRCUIT
			{
				// Token: 0x0400B8AF RID: 47279
				public static LocString NAME = "LED Party Outfit";
			}

			// Token: 0x02002A5C RID: 10844
			public class ATMOSUIT_LIMONE
			{
				// Token: 0x0400B8B0 RID: 47280
				public static LocString NAME = "Citrus Atmo Outfit";
			}

			// Token: 0x02002A5D RID: 10845
			public class ATMOSUIT_SPARKLE_RED
			{
				// Token: 0x0400B8B1 RID: 47281
				public static LocString NAME = "Red Glitter Atmo Outfit";
			}

			// Token: 0x02002A5E RID: 10846
			public class ATMOSUIT_SPARKLE_BLUE
			{
				// Token: 0x0400B8B2 RID: 47282
				public static LocString NAME = "Blue Glitter Atmo Outfit";
			}

			// Token: 0x02002A5F RID: 10847
			public class ATMOSUIT_SPARKLE_GREEN
			{
				// Token: 0x0400B8B3 RID: 47283
				public static LocString NAME = "Green Glitter Atmo Outfit";
			}

			// Token: 0x02002A60 RID: 10848
			public class ATMOSUIT_SPARKLE_LAVENDER
			{
				// Token: 0x0400B8B4 RID: 47284
				public static LocString NAME = "Violet Glitter Atmo Outfit";
			}

			// Token: 0x02002A61 RID: 10849
			public class ATMOSUIT_PUFT
			{
				// Token: 0x0400B8B5 RID: 47285
				public static LocString NAME = "Puft Atmo Outfit";
			}

			// Token: 0x02002A62 RID: 10850
			public class ATMOSUIT_CONFETTI
			{
				// Token: 0x0400B8B6 RID: 47286
				public static LocString NAME = "Confetti Atmo Outfit";
			}

			// Token: 0x02002A63 RID: 10851
			public class ATMOSUIT_BASIC_PURPLE
			{
				// Token: 0x0400B8B7 RID: 47287
				public static LocString NAME = "Eggplant Atmo Outfit";
			}

			// Token: 0x02002A64 RID: 10852
			public class ATMOSUIT_PINK_PURPLE
			{
				// Token: 0x0400B8B8 RID: 47288
				public static LocString NAME = "Pink Punch Atmo Outfit";
			}

			// Token: 0x02002A65 RID: 10853
			public class ATMOSUIT_RED_GREY
			{
				// Token: 0x0400B8B9 RID: 47289
				public static LocString NAME = "Blastoff Atmo Outfit";
			}

			// Token: 0x02002A66 RID: 10854
			public class CANUXTUX
			{
				// Token: 0x0400B8BA RID: 47290
				public static LocString NAME = "Canadian Tuxedo Outfit";
			}

			// Token: 0x02002A67 RID: 10855
			public class GONCHIES_STRAWBERRY
			{
				// Token: 0x0400B8BB RID: 47291
				public static LocString NAME = "Executive Undies Outfit";
			}

			// Token: 0x02002A68 RID: 10856
			public class GONCHIES_SATSUMA
			{
				// Token: 0x0400B8BC RID: 47292
				public static LocString NAME = "Underling Undies Outfit";
			}

			// Token: 0x02002A69 RID: 10857
			public class GONCHIES_LEMON
			{
				// Token: 0x0400B8BD RID: 47293
				public static LocString NAME = "Groupthink Undies Outfit";
			}

			// Token: 0x02002A6A RID: 10858
			public class GONCHIES_LIME
			{
				// Token: 0x0400B8BE RID: 47294
				public static LocString NAME = "Stakeholder Undies Outfit";
			}

			// Token: 0x02002A6B RID: 10859
			public class GONCHIES_BLUEBERRY
			{
				// Token: 0x0400B8BF RID: 47295
				public static LocString NAME = "Admin Undies Outfit";
			}

			// Token: 0x02002A6C RID: 10860
			public class GONCHIES_GRAPE
			{
				// Token: 0x0400B8C0 RID: 47296
				public static LocString NAME = "Buzzword Undies Outfit";
			}

			// Token: 0x02002A6D RID: 10861
			public class GONCHIES_WATERMELON
			{
				// Token: 0x0400B8C1 RID: 47297
				public static LocString NAME = "Synergy Undies Outfit";
			}

			// Token: 0x02002A6E RID: 10862
			public class NERD
			{
				// Token: 0x0400B8C2 RID: 47298
				public static LocString NAME = "Research Outfit";
			}

			// Token: 0x02002A6F RID: 10863
			public class REBELGI
			{
				// Token: 0x0400B8C3 RID: 47299
				public static LocString NAME = "Rebel Gi Outfit";
			}

			// Token: 0x02002A70 RID: 10864
			public class DONOR
			{
				// Token: 0x0400B8C4 RID: 47300
				public static LocString NAME = "Donor Outfit";
			}

			// Token: 0x02002A71 RID: 10865
			public class MECHANIC
			{
				// Token: 0x0400B8C5 RID: 47301
				public static LocString NAME = "Engineer Coveralls";
			}

			// Token: 0x02002A72 RID: 10866
			public class VELOUR_BLACK
			{
				// Token: 0x0400B8C6 RID: 47302
				public static LocString NAME = "PhD Velour Outfit";
			}

			// Token: 0x02002A73 RID: 10867
			public class SLEEVELESS_BOW_BW
			{
				// Token: 0x0400B8C7 RID: 47303
				public static LocString NAME = "PhD Dress Outfit";
			}

			// Token: 0x02002A74 RID: 10868
			public class VELOUR_BLUE
			{
				// Token: 0x0400B8C8 RID: 47304
				public static LocString NAME = "Shortwave Velour Outfit";
			}

			// Token: 0x02002A75 RID: 10869
			public class VELOUR_PINK
			{
				// Token: 0x0400B8C9 RID: 47305
				public static LocString NAME = "Gamma Velour Outfit";
			}

			// Token: 0x02002A76 RID: 10870
			public class WATER
			{
				// Token: 0x0400B8CA RID: 47306
				public static LocString NAME = "HVAC Coveralls";
			}

			// Token: 0x02002A77 RID: 10871
			public class WAISTCOAT_PINSTRIPE_SLATE
			{
				// Token: 0x0400B8CB RID: 47307
				public static LocString NAME = "Nobel Pinstripe Outfit";
			}

			// Token: 0x02002A78 RID: 10872
			public class TWEED_PINK_ORCHID
			{
				// Token: 0x0400B8CC RID: 47308
				public static LocString NAME = "Power Brunch Outfit";
			}

			// Token: 0x02002A79 RID: 10873
			public class BALLET
			{
				// Token: 0x0400B8CD RID: 47309
				public static LocString NAME = "Ballet Outfit";
			}

			// Token: 0x02002A7A RID: 10874
			public class ATMOSUIT_CANTALOUPE
			{
				// Token: 0x0400B8CE RID: 47310
				public static LocString NAME = "Rocketmelon Atmo Outfit";
			}

			// Token: 0x02002A7B RID: 10875
			public class PAJAMAS_SNOW
			{
				// Token: 0x0400B8CF RID: 47311
				public static LocString NAME = "Crystal-Iced Jammies";
			}

			// Token: 0x02002A7C RID: 10876
			public class X_SPORCHID
			{
				// Token: 0x0400B8D0 RID: 47312
				public static LocString NAME = "Sporefest Outfit";
			}

			// Token: 0x02002A7D RID: 10877
			public class X1_PINCHAPEPPERNUTBELLS
			{
				// Token: 0x0400B8D1 RID: 47313
				public static LocString NAME = "Pinchabell Outfit";
			}

			// Token: 0x02002A7E RID: 10878
			public class POMPOM_SHINEBUGS_PINK_PEPPERNUT
			{
				// Token: 0x0400B8D2 RID: 47314
				public static LocString NAME = "Pom Bug Outfit";
			}

			// Token: 0x02002A7F RID: 10879
			public class SNOWFLAKE_BLUE
			{
				// Token: 0x0400B8D3 RID: 47315
				public static LocString NAME = "Crystal-Iced Outfit";
			}

			// Token: 0x02002A80 RID: 10880
			public class POLKADOT_TRACKSUIT
			{
				// Token: 0x0400B8D4 RID: 47316
				public static LocString NAME = "Polka Dot Tracksuit";
			}

			// Token: 0x02002A81 RID: 10881
			public class SUPERSTAR
			{
				// Token: 0x0400B8D5 RID: 47317
				public static LocString NAME = "Superstar Outfit";
			}

			// Token: 0x02002A82 RID: 10882
			public class ATMOSUIT_SPIFFY
			{
				// Token: 0x0400B8D6 RID: 47318
				public static LocString NAME = "Spiffy Atmo Outfit";
			}

			// Token: 0x02002A83 RID: 10883
			public class ATMOSUIT_CUBIST
			{
				// Token: 0x0400B8D7 RID: 47319
				public static LocString NAME = "Cubist Atmo Outfit";
			}

			// Token: 0x02002A84 RID: 10884
			public class LUCKY
			{
				// Token: 0x0400B8D8 RID: 47320
				public static LocString NAME = "Lucky Jammies Outfit";
			}

			// Token: 0x02002A85 RID: 10885
			public class SWEETHEART
			{
				// Token: 0x0400B8D9 RID: 47321
				public static LocString NAME = "Sweetheart Jammies Outfit";
			}

			// Token: 0x02002A86 RID: 10886
			public class GINCH_GLUON
			{
				// Token: 0x0400B8DA RID: 47322
				public static LocString NAME = "Frilly Saltrock Outfit";
			}

			// Token: 0x02002A87 RID: 10887
			public class GINCH_CORTEX
			{
				// Token: 0x0400B8DB RID: 47323
				public static LocString NAME = "Dusk Undies Outfit";
			}

			// Token: 0x02002A88 RID: 10888
			public class GINCH_FROSTY
			{
				// Token: 0x0400B8DC RID: 47324
				public static LocString NAME = "Frostbasin Undies Outfit";
			}

			// Token: 0x02002A89 RID: 10889
			public class GINCH_LOCUS
			{
				// Token: 0x0400B8DD RID: 47325
				public static LocString NAME = "Balmy Undies Outfit";
			}

			// Token: 0x02002A8A RID: 10890
			public class GINCH_GOOP
			{
				// Token: 0x0400B8DE RID: 47326
				public static LocString NAME = "Leachy Undies Outfit";
			}

			// Token: 0x02002A8B RID: 10891
			public class GINCH_BILE
			{
				// Token: 0x0400B8DF RID: 47327
				public static LocString NAME = "Yellowcake Undies Outfit";
			}

			// Token: 0x02002A8C RID: 10892
			public class GINCH_NYBBLE
			{
				// Token: 0x0400B8E0 RID: 47328
				public static LocString NAME = "Atomic Undies Outfit";
			}

			// Token: 0x02002A8D RID: 10893
			public class GINCH_IRONBOW
			{
				// Token: 0x0400B8E1 RID: 47329
				public static LocString NAME = "Magma Undies Outfit";
			}

			// Token: 0x02002A8E RID: 10894
			public class GINCH_PHLEGM
			{
				// Token: 0x0400B8E2 RID: 47330
				public static LocString NAME = "Slate Undies Outfit";
			}

			// Token: 0x02002A8F RID: 10895
			public class GINCH_OBELUS
			{
				// Token: 0x0400B8E3 RID: 47331
				public static LocString NAME = "Charcoal Undies Outfit";
			}

			// Token: 0x02002A90 RID: 10896
			public class HIVIS
			{
				// Token: 0x0400B8E4 RID: 47332
				public static LocString NAME = "Hi-Vis Outfit";
			}

			// Token: 0x02002A91 RID: 10897
			public class DOWNTIME
			{
				// Token: 0x0400B8E5 RID: 47333
				public static LocString NAME = "Downtime Outfit";
			}

			// Token: 0x02002A92 RID: 10898
			public class FLANNEL_RED
			{
				// Token: 0x0400B8E6 RID: 47334
				public static LocString NAME = "Classic Flannel Outfit";
			}

			// Token: 0x02002A93 RID: 10899
			public class FLANNEL_ORANGE
			{
				// Token: 0x0400B8E7 RID: 47335
				public static LocString NAME = "Cadmium Flannel Outfit";
			}

			// Token: 0x02002A94 RID: 10900
			public class FLANNEL_YELLOW
			{
				// Token: 0x0400B8E8 RID: 47336
				public static LocString NAME = "Flax Flannel Outfit";
			}

			// Token: 0x02002A95 RID: 10901
			public class FLANNEL_GREEN
			{
				// Token: 0x0400B8E9 RID: 47337
				public static LocString NAME = "Swampy Flannel Outfit";
			}

			// Token: 0x02002A96 RID: 10902
			public class FLANNEL_BLUE_MIDDLE
			{
				// Token: 0x0400B8EA RID: 47338
				public static LocString NAME = "Scrub Flannel Outfit";
			}

			// Token: 0x02002A97 RID: 10903
			public class FLANNEL_PURPLE
			{
				// Token: 0x0400B8EB RID: 47339
				public static LocString NAME = "Fusion Flannel Outfit";
			}

			// Token: 0x02002A98 RID: 10904
			public class FLANNEL_PINK_ORCHID
			{
				// Token: 0x0400B8EC RID: 47340
				public static LocString NAME = "Flare Flannel Outfit";
			}

			// Token: 0x02002A99 RID: 10905
			public class FLANNEL_WHITE
			{
				// Token: 0x0400B8ED RID: 47341
				public static LocString NAME = "White Flannel Outfit";
			}

			// Token: 0x02002A9A RID: 10906
			public class FLANNEL_BLACK
			{
				// Token: 0x0400B8EE RID: 47342
				public static LocString NAME = "Monochrome Flannel Outfit";
			}
		}

		// Token: 0x0200215F RID: 8543
		public class ROLES_SCREEN
		{
			// Token: 0x0400959D RID: 38301
			public static LocString MANAGEMENT_BUTTON = "JOBS";

			// Token: 0x0400959E RID: 38302
			public static LocString ROLE_PROGRESS = "<b>Job Experience: {0}/{1}</b>\nDuplicants can become eligible for specialized jobs by maxing their current job experience";

			// Token: 0x0400959F RID: 38303
			public static LocString NO_JOB_STATION_WARNING = string.Concat(new string[]
			{
				"Build a ",
				UI.PRE_KEYWORD,
				"Printing Pod",
				UI.PST_KEYWORD,
				" to unlock this menu\n\nThe ",
				UI.PRE_KEYWORD,
				"Printing Pod",
				UI.PST_KEYWORD,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
				" of the Build Menu"
			});

			// Token: 0x040095A0 RID: 38304
			public static LocString AUTO_PRIORITIZE = "Auto-Prioritize:";

			// Token: 0x040095A1 RID: 38305
			public static LocString AUTO_PRIORITIZE_ENABLED = "Duplicant priorities are automatically reconfigured when they are assigned a new job";

			// Token: 0x040095A2 RID: 38306
			public static LocString AUTO_PRIORITIZE_DISABLED = "Duplicant priorities can only be changed manually";

			// Token: 0x040095A3 RID: 38307
			public static LocString EXPECTATION_ALERT_EXPECTATION = "Current Morale: {0}\nJob Morale Needs: {1}";

			// Token: 0x040095A4 RID: 38308
			public static LocString EXPECTATION_ALERT_JOB = "Current Morale: {0}\n{2} Minimum Morale: {1}";

			// Token: 0x040095A5 RID: 38309
			public static LocString EXPECTATION_ALERT_TARGET_JOB = "{2}'s Current: {0} Morale\n{3} Minimum Morale: {1}";

			// Token: 0x040095A6 RID: 38310
			public static LocString EXPECTATION_ALERT_DESC_EXPECTATION = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

			// Token: 0x040095A7 RID: 38311
			public static LocString EXPECTATION_ALERT_DESC_JOB = "This Duplicant's Morale is too low to handle the assigned job, which will cause them Stress over time.";

			// Token: 0x040095A8 RID: 38312
			public static LocString EXPECTATION_ALERT_DESC_TARGET_JOB = "This Duplicant's Morale is too low to handle the rigors of this position, which will cause them Stress over time.";

			// Token: 0x040095A9 RID: 38313
			public static LocString HIGHEST_EXPECTATIONS_TIER = "<b>Highest Expectations</b>";

			// Token: 0x040095AA RID: 38314
			public static LocString ADDED_EXPECTATIONS_AMOUNT = " (+{0} Expectation)";

			// Token: 0x02002A9B RID: 10907
			public class WIDGET
			{
				// Token: 0x0400B8EF RID: 47343
				public static LocString NUMBER_OF_MASTERS_TOOLTIP = "<b>Duplicants who have mastered this job:</b>{0}";

				// Token: 0x0400B8F0 RID: 47344
				public static LocString NO_MASTERS_TOOLTIP = "<b>No Duplicants have mastered this job</b>";
			}

			// Token: 0x02002A9C RID: 10908
			public class TIER_NAMES
			{
				// Token: 0x0400B8F1 RID: 47345
				public static LocString ZERO = "Tier 0";

				// Token: 0x0400B8F2 RID: 47346
				public static LocString ONE = "Tier 1";

				// Token: 0x0400B8F3 RID: 47347
				public static LocString TWO = "Tier 2";

				// Token: 0x0400B8F4 RID: 47348
				public static LocString THREE = "Tier 3";

				// Token: 0x0400B8F5 RID: 47349
				public static LocString FOUR = "Tier 4";

				// Token: 0x0400B8F6 RID: 47350
				public static LocString FIVE = "Tier 5";

				// Token: 0x0400B8F7 RID: 47351
				public static LocString SIX = "Tier 6";

				// Token: 0x0400B8F8 RID: 47352
				public static LocString SEVEN = "Tier 7";

				// Token: 0x0400B8F9 RID: 47353
				public static LocString EIGHT = "Tier 8";

				// Token: 0x0400B8FA RID: 47354
				public static LocString NINE = "Tier 9";
			}

			// Token: 0x02002A9D RID: 10909
			public class SLOTS
			{
				// Token: 0x0400B8FB RID: 47355
				public static LocString UNASSIGNED = "Vacant Position";

				// Token: 0x0400B8FC RID: 47356
				public static LocString UNASSIGNED_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to assign a Duplicant to this job opening";

				// Token: 0x0400B8FD RID: 47357
				public static LocString NOSLOTS = "No slots available";

				// Token: 0x0400B8FE RID: 47358
				public static LocString NO_ELIGIBLE_DUPLICANTS = "No Duplicants meet the requirements for this job";

				// Token: 0x0400B8FF RID: 47359
				public static LocString ASSIGNMENT_PENDING = "(Pending)";

				// Token: 0x0400B900 RID: 47360
				public static LocString PICK_JOB = "No Job";

				// Token: 0x0400B901 RID: 47361
				public static LocString PICK_DUPLICANT = "None";
			}

			// Token: 0x02002A9E RID: 10910
			public class DROPDOWN
			{
				// Token: 0x0400B902 RID: 47362
				public static LocString NAME_AND_ROLE = "{0} <color=#F44A47FF>({1})</color>";

				// Token: 0x0400B903 RID: 47363
				public static LocString ALREADY_ROLE = "(Currently {0})";
			}

			// Token: 0x02002A9F RID: 10911
			public class SIDEBAR
			{
				// Token: 0x0400B904 RID: 47364
				public static LocString ASSIGNED_DUPLICANTS = "Assigned Duplicants";

				// Token: 0x0400B905 RID: 47365
				public static LocString UNASSIGNED_DUPLICANTS = "Unassigned Duplicants";

				// Token: 0x0400B906 RID: 47366
				public static LocString UNASSIGN = "Unassign job";
			}

			// Token: 0x02002AA0 RID: 10912
			public class PRIORITY
			{
				// Token: 0x0400B907 RID: 47367
				public static LocString TITLE = "Job Priorities";

				// Token: 0x0400B908 RID: 47368
				public static LocString DESCRIPTION = "{0}s prioritize these work errands: ";

				// Token: 0x0400B909 RID: 47369
				public static LocString NO_EFFECT = "This job does not affect errand prioritization";
			}

			// Token: 0x02002AA1 RID: 10913
			public class RESUME
			{
				// Token: 0x0400B90A RID: 47370
				public static LocString TITLE = "Qualifications";

				// Token: 0x0400B90B RID: 47371
				public static LocString PREVIOUS_ROLES = "PREVIOUS DUTIES";

				// Token: 0x0400B90C RID: 47372
				public static LocString UNASSIGNED = "Unassigned";

				// Token: 0x0400B90D RID: 47373
				public static LocString NO_SELECTION = "No Duplicant selected";
			}

			// Token: 0x02002AA2 RID: 10914
			public class PERKS
			{
				// Token: 0x0400B90E RID: 47374
				public static LocString TITLE_BASICTRAINING = "Basic Job Training";

				// Token: 0x0400B90F RID: 47375
				public static LocString TITLE_MORETRAINING = "Additional Job Training";

				// Token: 0x0400B910 RID: 47376
				public static LocString NO_PERKS = "This job comes with no training";

				// Token: 0x0400B911 RID: 47377
				public static LocString ATTRIBUTE_EFFECT_FMT = "<b>{0}</b> " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

				// Token: 0x020036A7 RID: 13991
				public class CAN_DIG_VERY_FIRM
				{
					// Token: 0x0400DAB6 RID: 55990
					public static LocString DESCRIPTION = UI.FormatAsLink(ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM + " Material", "HARDNESS") + " Mining";
				}

				// Token: 0x020036A8 RID: 13992
				public class CAN_DIG_NEARLY_IMPENETRABLE
				{
					// Token: 0x0400DAB7 RID: 55991
					public static LocString DESCRIPTION = UI.FormatAsLink("Abyssalite", "KATAIRITE") + " Mining";
				}

				// Token: 0x020036A9 RID: 13993
				public class CAN_DIG_SUPER_SUPER_HARD
				{
					// Token: 0x0400DAB8 RID: 55992
					public static LocString DESCRIPTION = UI.FormatAsLink("Diamond", "DIAMOND") + " and " + UI.FormatAsLink("Obsidian", "OBSIDIAN") + " Mining";
				}

				// Token: 0x020036AA RID: 13994
				public class CAN_DIG_RADIOACTIVE_MATERIALS
				{
					// Token: 0x0400DAB9 RID: 55993
					public static LocString DESCRIPTION = UI.FormatAsLink("Corium", "CORIUM") + " Mining";
				}

				// Token: 0x020036AB RID: 13995
				public class CAN_DIG_UNOBTANIUM
				{
					// Token: 0x0400DABA RID: 55994
					public static LocString DESCRIPTION = UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " Mining";
				}

				// Token: 0x020036AC RID: 13996
				public class CAN_ART
				{
					// Token: 0x0400DABB RID: 55995
					public static LocString DESCRIPTION = "Can produce artwork using " + BUILDINGS.PREFABS.CANVAS.NAME + " and " + BUILDINGS.PREFABS.SCULPTURE.NAME;
				}

				// Token: 0x020036AD RID: 13997
				public class CAN_ART_UGLY
				{
					// Token: 0x0400DABC RID: 55996
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Crude" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x020036AE RID: 13998
				public class CAN_ART_OKAY
				{
					// Token: 0x0400DABD RID: 55997
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Mediocre" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x020036AF RID: 13999
				public class CAN_ART_GREAT
				{
					// Token: 0x0400DABE RID: 55998
					public static LocString DESCRIPTION = UI.PRE_KEYWORD + "Master" + UI.PST_KEYWORD + " artwork quality";
				}

				// Token: 0x020036B0 RID: 14000
				public class CAN_FARM_TINKER
				{
					// Token: 0x0400DABF RID: 55999
					public static LocString DESCRIPTION = UI.FormatAsLink("Crop Tending", "PLANTS") + " and " + ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME + " Crafting";
				}

				// Token: 0x020036B1 RID: 14001
				public class CAN_IDENTIFY_MUTANT_SEEDS
				{
					// Token: 0x0400DAC0 RID: 56000
					public static LocString DESCRIPTION = string.Concat(new string[]
					{
						"Can identify ",
						UI.PRE_KEYWORD,
						"Mutant Seeds",
						UI.PST_KEYWORD,
						" at the ",
						BUILDINGS.PREFABS.GENETICANALYSISSTATION.NAME
					});
				}

				// Token: 0x020036B2 RID: 14002
				public class CAN_WRANGLE_CREATURES
				{
					// Token: 0x0400DAC1 RID: 56001
					public static LocString DESCRIPTION = "Critter Wrangling";
				}

				// Token: 0x020036B3 RID: 14003
				public class CAN_USE_RANCH_STATION
				{
					// Token: 0x0400DAC2 RID: 56002
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.RANCHSTATION.NAME + " Usage";
				}

				// Token: 0x020036B4 RID: 14004
				public class CAN_USE_MILKING_STATION
				{
					// Token: 0x0400DAC3 RID: 56003
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MILKINGSTATION.NAME + " Usage";
				}

				// Token: 0x020036B5 RID: 14005
				public class CAN_POWER_TINKER
				{
					// Token: 0x0400DAC4 RID: 56004
					public static LocString DESCRIPTION = UI.FormatAsLink("Generator Tuning", "POWER") + " usage and " + ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME + " Crafting";
				}

				// Token: 0x020036B6 RID: 14006
				public class CAN_ELECTRIC_GRILL
				{
					// Token: 0x0400DAC5 RID: 56005
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COOKINGSTATION.NAME + " Usage";
				}

				// Token: 0x020036B7 RID: 14007
				public class CAN_SPICE_GRINDER
				{
					// Token: 0x0400DAC6 RID: 56006
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.SPICEGRINDER.NAME + " Usage";
				}

				// Token: 0x020036B8 RID: 14008
				public class CAN_MAKE_MISSILES
				{
					// Token: 0x0400DAC7 RID: 56007
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MISSILEFABRICATOR.NAME + " Usage";
				}

				// Token: 0x020036B9 RID: 14009
				public class CAN_CRAFT_ELECTRONICS
				{
					// Token: 0x0400DAC8 RID: 56008
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDCRAFTINGTABLE.NAME + " Usage";
				}

				// Token: 0x020036BA RID: 14010
				public class ADVANCED_RESEARCH
				{
					// Token: 0x0400DAC9 RID: 56009
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x020036BB RID: 14011
				public class INTERSTELLAR_RESEARCH
				{
					// Token: 0x0400DACA RID: 56010
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x020036BC RID: 14012
				public class NUCLEAR_RESEARCH
				{
					// Token: 0x0400DACB RID: 56011
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x020036BD RID: 14013
				public class ORBITAL_RESEARCH
				{
					// Token: 0x0400DACC RID: 56012
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME + " Usage";
				}

				// Token: 0x020036BE RID: 14014
				public class GEYSER_TUNING
				{
					// Token: 0x0400DACD RID: 56013
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.GEOTUNER.NAME + " Usage";
				}

				// Token: 0x020036BF RID: 14015
				public class CAN_CLOTHING_ALTERATION
				{
					// Token: 0x0400DACE RID: 56014
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLOTHINGALTERATIONSTATION.NAME + " Usage";
				}

				// Token: 0x020036C0 RID: 14016
				public class CAN_STUDY_WORLD_OBJECTS
				{
					// Token: 0x0400DACF RID: 56015
					public static LocString DESCRIPTION = "Geographical Analysis";
				}

				// Token: 0x020036C1 RID: 14017
				public class CAN_STUDY_ARTIFACTS
				{
					// Token: 0x0400DAD0 RID: 56016
					public static LocString DESCRIPTION = "Artifact Analysis";
				}

				// Token: 0x020036C2 RID: 14018
				public class CAN_USE_CLUSTER_TELESCOPE
				{
					// Token: 0x0400DAD1 RID: 56017
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " Usage";
				}

				// Token: 0x020036C3 RID: 14019
				public class EXOSUIT_EXPERTISE
				{
					// Token: 0x0400DAD2 RID: 56018
					public static LocString DESCRIPTION = UI.FormatAsLink("Exosuit", "EXOSUIT") + " Penalty Reduction";
				}

				// Token: 0x020036C4 RID: 14020
				public class EXOSUIT_DURABILITY
				{
					// Token: 0x0400DAD3 RID: 56019
					public static LocString DESCRIPTION = "Slows " + UI.FormatAsLink("Exosuit", "EXOSUIT") + " Durability Damage";
				}

				// Token: 0x020036C5 RID: 14021
				public class CONVEYOR_BUILD
				{
					// Token: 0x0400DAD4 RID: 56020
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " Construction";
				}

				// Token: 0x020036C6 RID: 14022
				public class CAN_DO_PLUMBING
				{
					// Token: 0x0400DAD5 RID: 56021
					public static LocString DESCRIPTION = "Pipe Emptying";
				}

				// Token: 0x020036C7 RID: 14023
				public class CAN_USE_ROCKETS
				{
					// Token: 0x0400DAD6 RID: 56022
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.COMMANDMODULE.NAME + " Usage";
				}

				// Token: 0x020036C8 RID: 14024
				public class CAN_DO_ASTRONAUT_TRAINING
				{
					// Token: 0x0400DAD7 RID: 56023
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ASTRONAUTTRAININGCENTER.NAME + " Usage";
				}

				// Token: 0x020036C9 RID: 14025
				public class CAN_MISSION_CONTROL
				{
					// Token: 0x0400DAD8 RID: 56024
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.MISSIONCONTROL.NAME + " Usage";
				}

				// Token: 0x020036CA RID: 14026
				public class CAN_PILOT_ROCKET
				{
					// Token: 0x0400DAD9 RID: 56025
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " Usage";
				}

				// Token: 0x020036CB RID: 14027
				public class CAN_COMPOUND
				{
					// Token: 0x0400DADA RID: 56026
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.APOTHECARY.NAME + " Usage";
				}

				// Token: 0x020036CC RID: 14028
				public class CAN_DOCTOR
				{
					// Token: 0x0400DADB RID: 56027
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.DOCTORSTATION.NAME + " Usage";
				}

				// Token: 0x020036CD RID: 14029
				public class CAN_ADVANCED_MEDICINE
				{
					// Token: 0x0400DADC RID: 56028
					public static LocString DESCRIPTION = BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME + " Usage";
				}

				// Token: 0x020036CE RID: 14030
				public class CAN_DEMOLISH
				{
					// Token: 0x0400DADD RID: 56029
					public static LocString DESCRIPTION = "Demolish Gravitas Buildings";
				}
			}

			// Token: 0x02002AA3 RID: 10915
			public class ASSIGNMENT_REQUIREMENTS
			{
				// Token: 0x0400B912 RID: 47378
				public static LocString TITLE = "Qualifications";

				// Token: 0x0400B913 RID: 47379
				public static LocString NONE = "This position has no qualification requirements";

				// Token: 0x0400B914 RID: 47380
				public static LocString ALREADY_IS_ROLE = "{0} <b>is already</b> assigned to the {1} position";

				// Token: 0x0400B915 RID: 47381
				public static LocString ALREADY_IS_JOBLESS = "{0} <b>is already</b> unemployed";

				// Token: 0x0400B916 RID: 47382
				public static LocString MASTERED = "{0} has mastered the {1} position";

				// Token: 0x0400B917 RID: 47383
				public static LocString WILL_BE_UNASSIGNED = "Note: Assigning {0} to {1} will <color=#F44A47FF>unassign</color> them from {2}";

				// Token: 0x0400B918 RID: 47384
				public static LocString RELEVANT_ATTRIBUTES = "Relevant skills:";

				// Token: 0x0400B919 RID: 47385
				public static LocString APTITUDES = "Interests";

				// Token: 0x0400B91A RID: 47386
				public static LocString RELEVANT_APTITUDES = "Relevant Interests:";

				// Token: 0x0400B91B RID: 47387
				public static LocString NO_APTITUDE = "None";

				// Token: 0x020036CF RID: 14031
				public class ELIGIBILITY
				{
					// Token: 0x0400DADE RID: 56030
					public static LocString ELIGIBLE = "{0} is qualified for the {1} position";

					// Token: 0x0400DADF RID: 56031
					public static LocString INELIGIBLE = "{0} is <color=#F44A47FF>not qualified</color> for the {1} position";
				}

				// Token: 0x020036D0 RID: 14032
				public class UNEMPLOYED
				{
					// Token: 0x0400DAE0 RID: 56032
					public static LocString NAME = "Unassigned";

					// Token: 0x0400DAE1 RID: 56033
					public static LocString DESCRIPTION = "Duplicant must not already have a job assignment";
				}

				// Token: 0x020036D1 RID: 14033
				public class HAS_COLONY_LEADER
				{
					// Token: 0x0400DAE2 RID: 56034
					public static LocString NAME = "Has colony leader";

					// Token: 0x0400DAE3 RID: 56035
					public static LocString DESCRIPTION = "A colony leader must be assigned";
				}

				// Token: 0x020036D2 RID: 14034
				public class HAS_ATTRIBUTE_DIGGING_BASIC
				{
					// Token: 0x0400DAE4 RID: 56036
					public static LocString NAME = "Basic Digging";

					// Token: 0x0400DAE5 RID: 56037
					public static LocString DESCRIPTION = "Must have at least {0} digging skill";
				}

				// Token: 0x020036D3 RID: 14035
				public class HAS_ATTRIBUTE_COOKING_BASIC
				{
					// Token: 0x0400DAE6 RID: 56038
					public static LocString NAME = "Basic Cooking";

					// Token: 0x0400DAE7 RID: 56039
					public static LocString DESCRIPTION = "Must have at least {0} cooking skill";
				}

				// Token: 0x020036D4 RID: 14036
				public class HAS_ATTRIBUTE_LEARNING_BASIC
				{
					// Token: 0x0400DAE8 RID: 56040
					public static LocString NAME = "Basic Learning";

					// Token: 0x0400DAE9 RID: 56041
					public static LocString DESCRIPTION = "Must have at least {0} learning skill";
				}

				// Token: 0x020036D5 RID: 14037
				public class HAS_ATTRIBUTE_LEARNING_MEDIUM
				{
					// Token: 0x0400DAEA RID: 56042
					public static LocString NAME = "Medium Learning";

					// Token: 0x0400DAEB RID: 56043
					public static LocString DESCRIPTION = "Must have at least {0} learning skill";
				}

				// Token: 0x020036D6 RID: 14038
				public class HAS_EXPERIENCE
				{
					// Token: 0x0400DAEC RID: 56044
					public static LocString NAME = "{0} Experience";

					// Token: 0x0400DAED RID: 56045
					public static LocString DESCRIPTION = "Mastery of the <b>{0}</b> job";
				}

				// Token: 0x020036D7 RID: 14039
				public class HAS_COMPLETED_ANY_OTHER_ROLE
				{
					// Token: 0x0400DAEE RID: 56046
					public static LocString NAME = "General Experience";

					// Token: 0x0400DAEF RID: 56047
					public static LocString DESCRIPTION = "Mastery of <b>at least one</b> job";
				}

				// Token: 0x020036D8 RID: 14040
				public class CHOREGROUP_ENABLED
				{
					// Token: 0x0400DAF0 RID: 56048
					public static LocString NAME = "Can perform {0}";

					// Token: 0x0400DAF1 RID: 56049
					public static LocString DESCRIPTION = "Capable of performing <b>{0}</b> jobs";
				}
			}

			// Token: 0x02002AA4 RID: 10916
			public class EXPECTATIONS
			{
				// Token: 0x0400B91C RID: 47388
				public static LocString TITLE = "Special Provisions Request";

				// Token: 0x0400B91D RID: 47389
				public static LocString NO_EXPECTATIONS = "No additional provisions are required to perform this job";

				// Token: 0x020036D9 RID: 14041
				public class PRIVATE_ROOM
				{
					// Token: 0x0400DAF2 RID: 56050
					public static LocString NAME = "Private Bedroom";

					// Token: 0x0400DAF3 RID: 56051
					public static LocString DESCRIPTION = "Duplicants in this job would appreciate their own place to unwind";
				}

				// Token: 0x020036DA RID: 14042
				public class FOOD_QUALITY
				{
					// Token: 0x02003AFD RID: 15101
					public class MINOR
					{
						// Token: 0x0400E4F0 RID: 58608
						public static LocString NAME = "Standard Food";

						// Token: 0x0400E4F1 RID: 58609
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire food that meets basic living standards";
					}

					// Token: 0x02003AFE RID: 15102
					public class MEDIUM
					{
						// Token: 0x0400E4F2 RID: 58610
						public static LocString NAME = "Good Food";

						// Token: 0x0400E4F3 RID: 58611
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire decent food for their efforts";
					}

					// Token: 0x02003AFF RID: 15103
					public class HIGH
					{
						// Token: 0x0400E4F4 RID: 58612
						public static LocString NAME = "Great Food";

						// Token: 0x0400E4F5 RID: 58613
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire better than average food";
					}

					// Token: 0x02003B00 RID: 15104
					public class VERY_HIGH
					{
						// Token: 0x0400E4F6 RID: 58614
						public static LocString NAME = "Superb Food";

						// Token: 0x0400E4F7 RID: 58615
						public static LocString DESCRIPTION = "Duplicants employed in this Tier have a refined taste for food";
					}

					// Token: 0x02003B01 RID: 15105
					public class EXCEPTIONAL
					{
						// Token: 0x0400E4F8 RID: 58616
						public static LocString NAME = "Ambrosial Food";

						// Token: 0x0400E4F9 RID: 58617
						public static LocString DESCRIPTION = "Duplicants employed in this Tier expect only the best cuisine";
					}
				}

				// Token: 0x020036DB RID: 14043
				public class DECOR
				{
					// Token: 0x02003B02 RID: 15106
					public class MINOR
					{
						// Token: 0x0400E4FA RID: 58618
						public static LocString NAME = "Minor Decor";

						// Token: 0x0400E4FB RID: 58619
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire slightly improved colony decor";
					}

					// Token: 0x02003B03 RID: 15107
					public class MEDIUM
					{
						// Token: 0x0400E4FC RID: 58620
						public static LocString NAME = "Medium Decor";

						// Token: 0x0400E4FD RID: 58621
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire reasonably improved colony decor";
					}

					// Token: 0x02003B04 RID: 15108
					public class HIGH
					{
						// Token: 0x0400E4FE RID: 58622
						public static LocString NAME = "High Decor";

						// Token: 0x0400E4FF RID: 58623
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire a decent increase in colony decor";
					}

					// Token: 0x02003B05 RID: 15109
					public class VERY_HIGH
					{
						// Token: 0x0400E500 RID: 58624
						public static LocString NAME = "Superb Decor";

						// Token: 0x0400E501 RID: 58625
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire majorly improved colony decor";
					}

					// Token: 0x02003B06 RID: 15110
					public class UNREASONABLE
					{
						// Token: 0x0400E502 RID: 58626
						public static LocString NAME = "Decadent Decor";

						// Token: 0x0400E503 RID: 58627
						public static LocString DESCRIPTION = "Duplicants employed in this Tier desire unrealistically luxurious improvements to decor";
					}
				}

				// Token: 0x020036DC RID: 14044
				public class QUALITYOFLIFE
				{
					// Token: 0x02003B07 RID: 15111
					public class TIER0
					{
						// Token: 0x0400E504 RID: 58628
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E505 RID: 58629
						public static LocString DESCRIPTION = "Tier 0";
					}

					// Token: 0x02003B08 RID: 15112
					public class TIER1
					{
						// Token: 0x0400E506 RID: 58630
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E507 RID: 58631
						public static LocString DESCRIPTION = "Tier 1";
					}

					// Token: 0x02003B09 RID: 15113
					public class TIER2
					{
						// Token: 0x0400E508 RID: 58632
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E509 RID: 58633
						public static LocString DESCRIPTION = "Tier 2";
					}

					// Token: 0x02003B0A RID: 15114
					public class TIER3
					{
						// Token: 0x0400E50A RID: 58634
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E50B RID: 58635
						public static LocString DESCRIPTION = "Tier 3";
					}

					// Token: 0x02003B0B RID: 15115
					public class TIER4
					{
						// Token: 0x0400E50C RID: 58636
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E50D RID: 58637
						public static LocString DESCRIPTION = "Tier 4";
					}

					// Token: 0x02003B0C RID: 15116
					public class TIER5
					{
						// Token: 0x0400E50E RID: 58638
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E50F RID: 58639
						public static LocString DESCRIPTION = "Tier 5";
					}

					// Token: 0x02003B0D RID: 15117
					public class TIER6
					{
						// Token: 0x0400E510 RID: 58640
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E511 RID: 58641
						public static LocString DESCRIPTION = "Tier 6";
					}

					// Token: 0x02003B0E RID: 15118
					public class TIER7
					{
						// Token: 0x0400E512 RID: 58642
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E513 RID: 58643
						public static LocString DESCRIPTION = "Tier 7";
					}

					// Token: 0x02003B0F RID: 15119
					public class TIER8
					{
						// Token: 0x0400E514 RID: 58644
						public static LocString NAME = "Morale Requirements";

						// Token: 0x0400E515 RID: 58645
						public static LocString DESCRIPTION = "Tier 8";
					}
				}
			}
		}

		// Token: 0x02002160 RID: 8544
		public class GAMEPLAY_EVENT_INFO_SCREEN
		{
			// Token: 0x040095AB RID: 38315
			public static LocString WHERE = "WHERE: {0}";

			// Token: 0x040095AC RID: 38316
			public static LocString WHEN = "WHEN: {0}";
		}

		// Token: 0x02002161 RID: 8545
		public class DEBUG_TOOLS
		{
			// Token: 0x040095AD RID: 38317
			public static LocString ENTER_TEXT = "";

			// Token: 0x040095AE RID: 38318
			public static LocString DEBUG_ACTIVE = "Debug tools active";

			// Token: 0x040095AF RID: 38319
			public static LocString INVALID_LOCATION = "Invalid Location";

			// Token: 0x02002AA5 RID: 10917
			public class PAINT_ELEMENTS_SCREEN
			{
				// Token: 0x0400B91E RID: 47390
				public static LocString TITLE = "CELL PAINTER";

				// Token: 0x0400B91F RID: 47391
				public static LocString ELEMENT = "Element";

				// Token: 0x0400B920 RID: 47392
				public static LocString MASS_KG = "Mass (kg)";

				// Token: 0x0400B921 RID: 47393
				public static LocString TEMPERATURE_KELVIN = "Temperature (K)";

				// Token: 0x0400B922 RID: 47394
				public static LocString DISEASE = "Disease";

				// Token: 0x0400B923 RID: 47395
				public static LocString DISEASE_COUNT = "Disease Count";

				// Token: 0x0400B924 RID: 47396
				public static LocString BUILDINGS = "Buildings:";

				// Token: 0x0400B925 RID: 47397
				public static LocString CELLS = "Cells:";

				// Token: 0x0400B926 RID: 47398
				public static LocString ADD_FOW_MASK = "Prevent FoW Reveal";

				// Token: 0x0400B927 RID: 47399
				public static LocString REMOVE_FOW_MASK = "Allow FoW Reveal";

				// Token: 0x0400B928 RID: 47400
				public static LocString PAINT = "Paint";

				// Token: 0x0400B929 RID: 47401
				public static LocString SAMPLE = "Sample";

				// Token: 0x0400B92A RID: 47402
				public static LocString STORE = "Store";

				// Token: 0x0400B92B RID: 47403
				public static LocString FILL = "Fill";

				// Token: 0x0400B92C RID: 47404
				public static LocString SPAWN_ALL = "Spawn All (Slow)";
			}

			// Token: 0x02002AA6 RID: 10918
			public class SAVE_BASE_TEMPLATE
			{
				// Token: 0x0400B92D RID: 47405
				public static LocString TITLE = "Base and World Tools";

				// Token: 0x0400B92E RID: 47406
				public static LocString SAVE_TITLE = "Save Selection";

				// Token: 0x0400B92F RID: 47407
				public static LocString CLEAR_BUTTON = "Clear Floor";

				// Token: 0x0400B930 RID: 47408
				public static LocString DESTROY_BUTTON = "Destroy";

				// Token: 0x0400B931 RID: 47409
				public static LocString DECONSTRUCT_BUTTON = "Deconstruct";

				// Token: 0x0400B932 RID: 47410
				public static LocString CLEAR_SELECTION_BUTTON = "Clear Selection";

				// Token: 0x0400B933 RID: 47411
				public static LocString DEFAULT_SAVE_NAME = "TemplateSaveName";

				// Token: 0x0400B934 RID: 47412
				public static LocString MORE = "More";

				// Token: 0x0400B935 RID: 47413
				public static LocString BASE_GAME_FOLDER_NAME = "Base Game";

				// Token: 0x020036DD RID: 14045
				public class SELECTION_INFO_PANEL
				{
					// Token: 0x0400DAF4 RID: 56052
					public static LocString TOTAL_MASS = "Total mass: {0}";

					// Token: 0x0400DAF5 RID: 56053
					public static LocString AVERAGE_MASS = "Average cell mass: {0}";

					// Token: 0x0400DAF6 RID: 56054
					public static LocString AVERAGE_TEMPERATURE = "Average temperature: {0}";

					// Token: 0x0400DAF7 RID: 56055
					public static LocString TOTAL_JOULES = "Total joules: {0}";

					// Token: 0x0400DAF8 RID: 56056
					public static LocString JOULES_PER_KILOGRAM = "Joules per kilogram: {0}";

					// Token: 0x0400DAF9 RID: 56057
					public static LocString TOTAL_RADS = "Total rads: {0}";

					// Token: 0x0400DAFA RID: 56058
					public static LocString AVERAGE_RADS = "Average rads: {0}";
				}
			}
		}

		// Token: 0x02002162 RID: 8546
		public class WORLDGEN
		{
			// Token: 0x040095B0 RID: 38320
			public static LocString NOHEADERS = "";

			// Token: 0x040095B1 RID: 38321
			public static LocString COMPLETE = "Success! Space adventure awaits.";

			// Token: 0x040095B2 RID: 38322
			public static LocString FAILED = "Goodness, has this ever gone terribly wrong!";

			// Token: 0x040095B3 RID: 38323
			public static LocString RESTARTING = "Rebooting...";

			// Token: 0x040095B4 RID: 38324
			public static LocString LOADING = "Loading world...";

			// Token: 0x040095B5 RID: 38325
			public static LocString GENERATINGWORLD = "The Galaxy Synthesizer";

			// Token: 0x040095B6 RID: 38326
			public static LocString CHOOSEWORLDSIZE = "Select the magnitude of your new galaxy.";

			// Token: 0x040095B7 RID: 38327
			public static LocString USING_PLAYER_SEED = "Using selected worldgen seed: {0}";

			// Token: 0x040095B8 RID: 38328
			public static LocString CLEARINGLEVEL = "Staring into the void...";

			// Token: 0x040095B9 RID: 38329
			public static LocString GENERATESOLARSYSTEM = "Catalyzing Big Bang...";

			// Token: 0x040095BA RID: 38330
			public static LocString GENERATESOLARSYSTEM1 = "Catalyzing Big Bang...";

			// Token: 0x040095BB RID: 38331
			public static LocString GENERATESOLARSYSTEM2 = "Catalyzing Big Bang...";

			// Token: 0x040095BC RID: 38332
			public static LocString GENERATESOLARSYSTEM3 = "Catalyzing Big Bang...";

			// Token: 0x040095BD RID: 38333
			public static LocString GENERATESOLARSYSTEM4 = "Catalyzing Big Bang...";

			// Token: 0x040095BE RID: 38334
			public static LocString GENERATESOLARSYSTEM5 = "Catalyzing Big Bang...";

			// Token: 0x040095BF RID: 38335
			public static LocString GENERATESOLARSYSTEM6 = "Approaching event horizon...";

			// Token: 0x040095C0 RID: 38336
			public static LocString GENERATESOLARSYSTEM7 = "Approaching event horizon...";

			// Token: 0x040095C1 RID: 38337
			public static LocString GENERATESOLARSYSTEM8 = "Approaching event horizon...";

			// Token: 0x040095C2 RID: 38338
			public static LocString GENERATESOLARSYSTEM9 = "Approaching event horizon...";

			// Token: 0x040095C3 RID: 38339
			public static LocString SETUPNOISE = "BANG!";

			// Token: 0x040095C4 RID: 38340
			public static LocString BUILDNOISESOURCE = "Sorting quadrillions of atoms...";

			// Token: 0x040095C5 RID: 38341
			public static LocString BUILDNOISESOURCE1 = "Sorting quadrillions of atoms...";

			// Token: 0x040095C6 RID: 38342
			public static LocString BUILDNOISESOURCE2 = "Sorting quadrillions of atoms...";

			// Token: 0x040095C7 RID: 38343
			public static LocString BUILDNOISESOURCE3 = "Ironing the fabric of creation...";

			// Token: 0x040095C8 RID: 38344
			public static LocString BUILDNOISESOURCE4 = "Ironing the fabric of creation...";

			// Token: 0x040095C9 RID: 38345
			public static LocString BUILDNOISESOURCE5 = "Ironing the fabric of creation...";

			// Token: 0x040095CA RID: 38346
			public static LocString BUILDNOISESOURCE6 = "Taking hot meteor shower...";

			// Token: 0x040095CB RID: 38347
			public static LocString BUILDNOISESOURCE7 = "Tightening asteroid belts...";

			// Token: 0x040095CC RID: 38348
			public static LocString BUILDNOISESOURCE8 = "Tightening asteroid belts...";

			// Token: 0x040095CD RID: 38349
			public static LocString BUILDNOISESOURCE9 = "Tightening asteroid belts...";

			// Token: 0x040095CE RID: 38350
			public static LocString GENERATENOISE = "Baking igneous rock...";

			// Token: 0x040095CF RID: 38351
			public static LocString GENERATENOISE1 = "Multilayering sediment...";

			// Token: 0x040095D0 RID: 38352
			public static LocString GENERATENOISE2 = "Multilayering sediment...";

			// Token: 0x040095D1 RID: 38353
			public static LocString GENERATENOISE3 = "Multilayering sediment...";

			// Token: 0x040095D2 RID: 38354
			public static LocString GENERATENOISE4 = "Superheating gases...";

			// Token: 0x040095D3 RID: 38355
			public static LocString GENERATENOISE5 = "Superheating gases...";

			// Token: 0x040095D4 RID: 38356
			public static LocString GENERATENOISE6 = "Superheating gases...";

			// Token: 0x040095D5 RID: 38357
			public static LocString GENERATENOISE7 = "Vacuuming out vacuums...";

			// Token: 0x040095D6 RID: 38358
			public static LocString GENERATENOISE8 = "Vacuuming out vacuums...";

			// Token: 0x040095D7 RID: 38359
			public static LocString GENERATENOISE9 = "Vacuuming out vacuums...";

			// Token: 0x040095D8 RID: 38360
			public static LocString NORMALISENOISE = "Interpolating suffocating gas...";

			// Token: 0x040095D9 RID: 38361
			public static LocString WORLDLAYOUT = "Freezing ice formations...";

			// Token: 0x040095DA RID: 38362
			public static LocString WORLDLAYOUT1 = "Freezing ice formations...";

			// Token: 0x040095DB RID: 38363
			public static LocString WORLDLAYOUT2 = "Freezing ice formations...";

			// Token: 0x040095DC RID: 38364
			public static LocString WORLDLAYOUT3 = "Freezing ice formations...";

			// Token: 0x040095DD RID: 38365
			public static LocString WORLDLAYOUT4 = "Melting magma...";

			// Token: 0x040095DE RID: 38366
			public static LocString WORLDLAYOUT5 = "Melting magma...";

			// Token: 0x040095DF RID: 38367
			public static LocString WORLDLAYOUT6 = "Melting magma...";

			// Token: 0x040095E0 RID: 38368
			public static LocString WORLDLAYOUT7 = "Sprinkling sand...";

			// Token: 0x040095E1 RID: 38369
			public static LocString WORLDLAYOUT8 = "Sprinkling sand...";

			// Token: 0x040095E2 RID: 38370
			public static LocString WORLDLAYOUT9 = "Sprinkling sand...";

			// Token: 0x040095E3 RID: 38371
			public static LocString WORLDLAYOUT10 = "Sprinkling sand...";

			// Token: 0x040095E4 RID: 38372
			public static LocString COMPLETELAYOUT = "Cooling glass...";

			// Token: 0x040095E5 RID: 38373
			public static LocString COMPLETELAYOUT1 = "Cooling glass...";

			// Token: 0x040095E6 RID: 38374
			public static LocString COMPLETELAYOUT2 = "Cooling glass...";

			// Token: 0x040095E7 RID: 38375
			public static LocString COMPLETELAYOUT3 = "Cooling glass...";

			// Token: 0x040095E8 RID: 38376
			public static LocString COMPLETELAYOUT4 = "Digging holes...";

			// Token: 0x040095E9 RID: 38377
			public static LocString COMPLETELAYOUT5 = "Digging holes...";

			// Token: 0x040095EA RID: 38378
			public static LocString COMPLETELAYOUT6 = "Digging holes...";

			// Token: 0x040095EB RID: 38379
			public static LocString COMPLETELAYOUT7 = "Adding buckets of dirt...";

			// Token: 0x040095EC RID: 38380
			public static LocString COMPLETELAYOUT8 = "Adding buckets of dirt...";

			// Token: 0x040095ED RID: 38381
			public static LocString COMPLETELAYOUT9 = "Adding buckets of dirt...";

			// Token: 0x040095EE RID: 38382
			public static LocString COMPLETELAYOUT10 = "Adding buckets of dirt...";

			// Token: 0x040095EF RID: 38383
			public static LocString PROCESSRIVERS = "Pouring rivers...";

			// Token: 0x040095F0 RID: 38384
			public static LocString CONVERTTERRAINCELLSTOEDGES = "Hardening diamonds...";

			// Token: 0x040095F1 RID: 38385
			public static LocString PROCESSING = "Embedding metals...";

			// Token: 0x040095F2 RID: 38386
			public static LocString PROCESSING1 = "Embedding metals...";

			// Token: 0x040095F3 RID: 38387
			public static LocString PROCESSING2 = "Embedding metals...";

			// Token: 0x040095F4 RID: 38388
			public static LocString PROCESSING3 = "Burying precious ore...";

			// Token: 0x040095F5 RID: 38389
			public static LocString PROCESSING4 = "Burying precious ore...";

			// Token: 0x040095F6 RID: 38390
			public static LocString PROCESSING5 = "Burying precious ore...";

			// Token: 0x040095F7 RID: 38391
			public static LocString PROCESSING6 = "Burying precious ore...";

			// Token: 0x040095F8 RID: 38392
			public static LocString PROCESSING7 = "Excavating tunnels...";

			// Token: 0x040095F9 RID: 38393
			public static LocString PROCESSING8 = "Excavating tunnels...";

			// Token: 0x040095FA RID: 38394
			public static LocString PROCESSING9 = "Excavating tunnels...";

			// Token: 0x040095FB RID: 38395
			public static LocString BORDERS = "Just adding water...";

			// Token: 0x040095FC RID: 38396
			public static LocString BORDERS1 = "Just adding water...";

			// Token: 0x040095FD RID: 38397
			public static LocString BORDERS2 = "Staring at the void...";

			// Token: 0x040095FE RID: 38398
			public static LocString BORDERS3 = "Staring at the void...";

			// Token: 0x040095FF RID: 38399
			public static LocString BORDERS4 = "Staring at the void...";

			// Token: 0x04009600 RID: 38400
			public static LocString BORDERS5 = "Avoiding awkward eye contact with the void...";

			// Token: 0x04009601 RID: 38401
			public static LocString BORDERS6 = "Avoiding awkward eye contact with the void...";

			// Token: 0x04009602 RID: 38402
			public static LocString BORDERS7 = "Avoiding awkward eye contact with the void...";

			// Token: 0x04009603 RID: 38403
			public static LocString BORDERS8 = "Avoiding awkward eye contact with the void...";

			// Token: 0x04009604 RID: 38404
			public static LocString BORDERS9 = "Avoiding awkward eye contact with the void...";

			// Token: 0x04009605 RID: 38405
			public static LocString DRAWWORLDBORDER = "Establishing personal boundaries...";

			// Token: 0x04009606 RID: 38406
			public static LocString PLACINGTEMPLATES = "Generating interest...";

			// Token: 0x04009607 RID: 38407
			public static LocString SETTLESIM = "Infusing oxygen...";

			// Token: 0x04009608 RID: 38408
			public static LocString SETTLESIM1 = "Infusing oxygen...";

			// Token: 0x04009609 RID: 38409
			public static LocString SETTLESIM2 = "Too much oxygen. Removing...";

			// Token: 0x0400960A RID: 38410
			public static LocString SETTLESIM3 = "Too much oxygen. Removing...";

			// Token: 0x0400960B RID: 38411
			public static LocString SETTLESIM4 = "Ideal oxygen levels achieved...";

			// Token: 0x0400960C RID: 38412
			public static LocString SETTLESIM5 = "Ideal oxygen levels achieved...";

			// Token: 0x0400960D RID: 38413
			public static LocString SETTLESIM6 = "Planting space flora...";

			// Token: 0x0400960E RID: 38414
			public static LocString SETTLESIM7 = "Planting space flora...";

			// Token: 0x0400960F RID: 38415
			public static LocString SETTLESIM8 = "Releasing wildlife...";

			// Token: 0x04009610 RID: 38416
			public static LocString SETTLESIM9 = "Releasing wildlife...";

			// Token: 0x04009611 RID: 38417
			public static LocString ANALYZINGWORLD = "Shuffling DNA Blueprints...";

			// Token: 0x04009612 RID: 38418
			public static LocString ANALYZINGWORLDCOMPLETE = "Tidying up for the Duplicants...";

			// Token: 0x04009613 RID: 38419
			public static LocString PLACINGCREATURES = "Building the suspense...";
		}

		// Token: 0x02002163 RID: 8547
		public class TOOLTIPS
		{
			// Token: 0x04009614 RID: 38420
			public static LocString MANAGEMENTMENU_JOBS = string.Concat(new string[]
			{
				"Manage my Duplicant Priorities {Hotkey}\n\nDuplicant Priorities",
				UI.PST_KEYWORD,
				" are calculated <i>before</i> the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by the ",
				UI.FormatAsTool("Priority Tool", global::Action.Prioritize)
			});

			// Token: 0x04009615 RID: 38421
			public static LocString MANAGEMENTMENU_CONSUMABLES = "Manage my Duplicants' diets and medications {Hotkey}";

			// Token: 0x04009616 RID: 38422
			public static LocString MANAGEMENTMENU_VITALS = "View my Duplicants' vitals {Hotkey}";

			// Token: 0x04009617 RID: 38423
			public static LocString MANAGEMENTMENU_RESEARCH = "View the Research Tree {Hotkey}";

			// Token: 0x04009618 RID: 38424
			public static LocString MANAGEMENTMENU_RESEARCH_NO_RESEARCH = "No active research projects";

			// Token: 0x04009619 RID: 38425
			public static LocString MANAGEMENTMENU_RESEARCH_CARD_NAME = "Currently researching: {0}";

			// Token: 0x0400961A RID: 38426
			public static LocString MANAGEMENTMENU_RESEARCH_ITEM_LINE = "• {0}";

			// Token: 0x0400961B RID: 38427
			public static LocString MANAGEMENTMENU_REQUIRES_RESEARCH = string.Concat(new string[]
			{
				"Build a Research Station to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.RESEARCHCENTER.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
				" of the Build Menu"
			});

			// Token: 0x0400961C RID: 38428
			public static LocString MANAGEMENTMENU_DAILYREPORT = "View each cycle's Colony Report {Hotkey}";

			// Token: 0x0400961D RID: 38429
			public static LocString MANAGEMENTMENU_CODEX = "Browse entries in my Database {Hotkey}";

			// Token: 0x0400961E RID: 38430
			public static LocString MANAGEMENTMENU_SCHEDULE = "Adjust the colony's time usage {Hotkey}";

			// Token: 0x0400961F RID: 38431
			public static LocString MANAGEMENTMENU_STARMAP = "Manage astronaut rocket missions {Hotkey}";

			// Token: 0x04009620 RID: 38432
			public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE = string.Concat(new string[]
			{
				"Build a Telescope to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.TELESCOPE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
				" of the Build Menu"
			});

			// Token: 0x04009621 RID: 38433
			public static LocString MANAGEMENTMENU_REQUIRES_TELESCOPE_CLUSTER = string.Concat(new string[]
			{
				"Build a Telescope to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.TELESCOPE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Rocketry Tab", global::Action.Plan14),
				" of the Build Menu"
			});

			// Token: 0x04009622 RID: 38434
			public static LocString MANAGEMENTMENU_SKILLS = "Manage Duplicants' Skill assignments {Hotkey}";

			// Token: 0x04009623 RID: 38435
			public static LocString MANAGEMENTMENU_REQUIRES_SKILL_STATION = string.Concat(new string[]
			{
				"Build a Printing Pod to unlock this menu\n\nThe ",
				BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME,
				" can be found in the ",
				UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
				" of the Build Menu"
			});

			// Token: 0x04009624 RID: 38436
			public static LocString MANAGEMENTMENU_PAUSEMENU = "Open the game menu {Hotkey}";

			// Token: 0x04009625 RID: 38437
			public static LocString MANAGEMENTMENU_RESOURCES = "Open the resource management screen {Hotkey}";

			// Token: 0x04009626 RID: 38438
			public static LocString OPEN_CODEX_ENTRY = "View full entry in database";

			// Token: 0x04009627 RID: 38439
			public static LocString NO_CODEX_ENTRY = "No database entry available";

			// Token: 0x04009628 RID: 38440
			public static LocString OPEN_RESOURCE_INFO = "{0} of {1} available for the Duplicants on this asteroid to use\n\nClick to open Resources menu";

			// Token: 0x04009629 RID: 38441
			public static LocString CHANGE_OUTFIT = "Change this Duplicant's outfit";

			// Token: 0x0400962A RID: 38442
			public static LocString CHANGE_MATERIAL = "Change this building's construction material";

			// Token: 0x0400962B RID: 38443
			public static LocString METERSCREEN_AVGSTRESS = "Highest Stress: {0}";

			// Token: 0x0400962C RID: 38444
			public static LocString METERSCREEN_MEALHISTORY = "Calories Available: {0}\n\nDuplicants consume a minimum of {1} calories each per cycle";

			// Token: 0x0400962D RID: 38445
			public static LocString METERSCREEN_ELECTROBANK_JOULES = "Joules Available: {0}\n\nBionic Duplicants use a minimum of X each per cycle\n\nPower Banks Available: Y\n";

			// Token: 0x0400962E RID: 38446
			public static LocString METERSCREEN_POPULATION = "Population: {0}";

			// Token: 0x0400962F RID: 38447
			public static LocString METERSCREEN_POPULATION_CLUSTER = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " Population: {1}\nTotal Population: {2}";

			// Token: 0x04009630 RID: 38448
			public static LocString METERSCREEN_SICK_DUPES = "Sick Duplicants: {0}";

			// Token: 0x04009631 RID: 38449
			public static LocString METERSCREEN_INVALID_FOOD_TYPE = "Invalid Food Type: {0}";

			// Token: 0x04009632 RID: 38450
			public static LocString METERSCREEN_INVALID_ELECTROBANK_TYPE = "Invalid Power Bank Type: {0}";

			// Token: 0x04009633 RID: 38451
			public static LocString PLAYBUTTON = "Start";

			// Token: 0x04009634 RID: 38452
			public static LocString PAUSEBUTTON = "Pause";

			// Token: 0x04009635 RID: 38453
			public static LocString PAUSE = "Pause {Hotkey}";

			// Token: 0x04009636 RID: 38454
			public static LocString UNPAUSE = "Unpause {Hotkey}";

			// Token: 0x04009637 RID: 38455
			public static LocString SPEEDBUTTON_SLOW = "Slow speed {Hotkey}";

			// Token: 0x04009638 RID: 38456
			public static LocString SPEEDBUTTON_MEDIUM = "Medium speed {Hotkey}";

			// Token: 0x04009639 RID: 38457
			public static LocString SPEEDBUTTON_FAST = "Fast speed {Hotkey}";

			// Token: 0x0400963A RID: 38458
			public static LocString RED_ALERT_TITLE = "Toggle Red Alert";

			// Token: 0x0400963B RID: 38459
			public static LocString RED_ALERT_CONTENT = "Duplicants will work, ignoring schedules and their basic needs\n\nUse in case of emergency";

			// Token: 0x0400963C RID: 38460
			public static LocString DISINFECTBUTTON = "Disinfect buildings {Hotkey}";

			// Token: 0x0400963D RID: 38461
			public static LocString MOPBUTTON = "Mop liquid spills {Hotkey}";

			// Token: 0x0400963E RID: 38462
			public static LocString DIGBUTTON = "Set dig errands {Hotkey}";

			// Token: 0x0400963F RID: 38463
			public static LocString CANCELBUTTON = "Cancel errands {Hotkey}";

			// Token: 0x04009640 RID: 38464
			public static LocString DECONSTRUCTBUTTON = "Demolish buildings {Hotkey}";

			// Token: 0x04009641 RID: 38465
			public static LocString ATTACKBUTTON = "Attack poor, wild critters {Hotkey}";

			// Token: 0x04009642 RID: 38466
			public static LocString CAPTUREBUTTON = "Capture critters {Hotkey}";

			// Token: 0x04009643 RID: 38467
			public static LocString CLEARBUTTON = "Move debris into storage {Hotkey}";

			// Token: 0x04009644 RID: 38468
			public static LocString HARVESTBUTTON = "Harvest plants {Hotkey}";

			// Token: 0x04009645 RID: 38469
			public static LocString PRIORITIZEMAINBUTTON = "";

			// Token: 0x04009646 RID: 38470
			public static LocString PRIORITIZEBUTTON = string.Concat(new string[]
			{
				"Set Building Priority {Hotkey}\n\nDuplicant Priorities",
				UI.PST_KEYWORD,
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities),
				" are calculated <i>before</i> the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by this tool"
			});

			// Token: 0x04009647 RID: 38471
			public static LocString CLEANUPMAINBUTTON = "Mop and sweep messy floors {Hotkey}";

			// Token: 0x04009648 RID: 38472
			public static LocString CANCELDECONSTRUCTIONBUTTON = "Cancel queued orders or deconstruct existing buildings {Hotkey}";

			// Token: 0x04009649 RID: 38473
			public static LocString HELP_ROTATE_KEY = "Press " + UI.FormatAsHotKey(global::Action.RotateBuilding) + " to Rotate";

			// Token: 0x0400964A RID: 38474
			public static LocString HELP_BUILDLOCATION_INVALID_CELL = "Invalid Cell";

			// Token: 0x0400964B RID: 38475
			public static LocString HELP_BUILDLOCATION_MISSING_TELEPAD = "World has no " + BUILDINGS.PREFABS.HEADQUARTERSCOMPLETE.NAME + " or " + BUILDINGS.PREFABS.EXOBASEHEADQUARTERS.NAME;

			// Token: 0x0400964C RID: 38476
			public static LocString HELP_BUILDLOCATION_FLOOR = "Must be built on solid ground";

			// Token: 0x0400964D RID: 38477
			public static LocString HELP_BUILDLOCATION_WALL = "Must be built against a wall";

			// Token: 0x0400964E RID: 38478
			public static LocString HELP_BUILDLOCATION_FLOOR_OR_ATTACHPOINT = "Must be built on solid ground or overlapping an {0}";

			// Token: 0x0400964F RID: 38479
			public static LocString HELP_BUILDLOCATION_OCCUPIED = "Must be built in unoccupied space";

			// Token: 0x04009650 RID: 38480
			public static LocString HELP_BUILDLOCATION_CEILING = "Must be built on the ceiling";

			// Token: 0x04009651 RID: 38481
			public static LocString HELP_BUILDLOCATION_INSIDEGROUND = "Must be built in the ground";

			// Token: 0x04009652 RID: 38482
			public static LocString HELP_BUILDLOCATION_ATTACHPOINT = "Must be built overlapping a {0}";

			// Token: 0x04009653 RID: 38483
			public static LocString HELP_BUILDLOCATION_SPACE = "Must be built on the surface in space";

			// Token: 0x04009654 RID: 38484
			public static LocString HELP_BUILDLOCATION_CORNER = "Must be built in a corner";

			// Token: 0x04009655 RID: 38485
			public static LocString HELP_BUILDLOCATION_CORNER_FLOOR = "Must be built in a corner on the ground";

			// Token: 0x04009656 RID: 38486
			public static LocString HELP_BUILDLOCATION_BELOWROCKETCEILING = "Must be placed further from the edge of space";

			// Token: 0x04009657 RID: 38487
			public static LocString HELP_BUILDLOCATION_ONROCKETENVELOPE = "Must be built on the interior wall of a rocket";

			// Token: 0x04009658 RID: 38488
			public static LocString HELP_BUILDLOCATION_LIQUID_CONDUIT_FORBIDDEN = "Obstructed by a building";

			// Token: 0x04009659 RID: 38489
			public static LocString HELP_BUILDLOCATION_NOT_IN_TILES = "Cannot be built inside tile";

			// Token: 0x0400965A RID: 38490
			public static LocString HELP_BUILDLOCATION_GASPORTS_OVERLAP = "Gas ports cannot overlap";

			// Token: 0x0400965B RID: 38491
			public static LocString HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP = "Liquid ports cannot overlap";

			// Token: 0x0400965C RID: 38492
			public static LocString HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP = "Solid ports cannot overlap";

			// Token: 0x0400965D RID: 38493
			public static LocString HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED = "Automation ports cannot overlap";

			// Token: 0x0400965E RID: 38494
			public static LocString HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP = "Power connectors cannot overlap";

			// Token: 0x0400965F RID: 38495
			public static LocString HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE = "Heavi-Watt connectors cannot be built inside tile";

			// Token: 0x04009660 RID: 38496
			public static LocString HELP_BUILDLOCATION_WIRE_OBSTRUCTION = "Obstructed by Heavi-Watt Wire";

			// Token: 0x04009661 RID: 38497
			public static LocString HELP_BUILDLOCATION_BACK_WALL = "Obstructed by back wall";

			// Token: 0x04009662 RID: 38498
			public static LocString HELP_TUBELOCATION_NO_UTURNS = "Can't U-Turn";

			// Token: 0x04009663 RID: 38499
			public static LocString HELP_TUBELOCATION_STRAIGHT_BRIDGES = "Can't Turn Here";

			// Token: 0x04009664 RID: 38500
			public static LocString HELP_REQUIRES_ROOM = "Must be in a " + UI.PRE_KEYWORD + "Room" + UI.PST_KEYWORD;

			// Token: 0x04009665 RID: 38501
			public static LocString OXYGENOVERLAYSTRING = "Displays ambient oxygen density {Hotkey}";

			// Token: 0x04009666 RID: 38502
			public static LocString POWEROVERLAYSTRING = "Displays power grid components {Hotkey}";

			// Token: 0x04009667 RID: 38503
			public static LocString TEMPERATUREOVERLAYSTRING = "Displays ambient temperature {Hotkey}";

			// Token: 0x04009668 RID: 38504
			public static LocString HEATFLOWOVERLAYSTRING = "Displays comfortable temperatures for Duplicants {Hotkey}";

			// Token: 0x04009669 RID: 38505
			public static LocString SUITOVERLAYSTRING = "Displays Exosuits and related buildings {Hotkey}";

			// Token: 0x0400966A RID: 38506
			public static LocString LOGICOVERLAYSTRING = "Displays automation grid components {Hotkey}";

			// Token: 0x0400966B RID: 38507
			public static LocString ROOMSOVERLAYSTRING = "Displays special purpose rooms and bonuses {Hotkey}";

			// Token: 0x0400966C RID: 38508
			public static LocString JOULESOVERLAYSTRING = "Displays the thermal energy in each cell";

			// Token: 0x0400966D RID: 38509
			public static LocString LIGHTSOVERLAYSTRING = "Displays the visibility radius of light sources {Hotkey}";

			// Token: 0x0400966E RID: 38510
			public static LocString LIQUIDVENTOVERLAYSTRING = "Displays liquid pipe system components {Hotkey}";

			// Token: 0x0400966F RID: 38511
			public static LocString GASVENTOVERLAYSTRING = "Displays gas pipe system components {Hotkey}";

			// Token: 0x04009670 RID: 38512
			public static LocString DECOROVERLAYSTRING = "Displays areas with Morale-boosting decor values {Hotkey}";

			// Token: 0x04009671 RID: 38513
			public static LocString PRIORITIESOVERLAYSTRING = "Displays work priority values {Hotkey}";

			// Token: 0x04009672 RID: 38514
			public static LocString DISEASEOVERLAYSTRING = "Displays areas of disease risk {Hotkey}";

			// Token: 0x04009673 RID: 38515
			public static LocString NOISE_POLLUTION_OVERLAY_STRING = "Displays ambient noise levels {Hotkey}";

			// Token: 0x04009674 RID: 38516
			public static LocString CROPS_OVERLAY_STRING = "Displays plant growth progress {Hotkey}";

			// Token: 0x04009675 RID: 38517
			public static LocString CONVEYOR_OVERLAY_STRING = "Displays conveyor transport components {Hotkey}";

			// Token: 0x04009676 RID: 38518
			public static LocString TILEMODE_OVERLAY_STRING = "Displays material information {Hotkey}";

			// Token: 0x04009677 RID: 38519
			public static LocString REACHABILITYOVERLAYSTRING = "Displays areas accessible by Duplicants";

			// Token: 0x04009678 RID: 38520
			public static LocString RADIATIONOVERLAYSTRING = "Displays radiation levels {Hotkey}";

			// Token: 0x04009679 RID: 38521
			public static LocString ENERGYREQUIRED = UI.FormatAsLink("Power", "POWER") + " Required";

			// Token: 0x0400967A RID: 38522
			public static LocString ENERGYGENERATED = UI.FormatAsLink("Power", "POWER") + " Produced";

			// Token: 0x0400967B RID: 38523
			public static LocString INFOPANEL = "The Info Panel contains an overview of the basic information about my Duplicant";

			// Token: 0x0400967C RID: 38524
			public static LocString VITALSPANEL = "The Vitals Panel monitors the status and well being of my Duplicant";

			// Token: 0x0400967D RID: 38525
			public static LocString STRESSPANEL = "The Stress Panel offers a detailed look at what is affecting my Duplicant psychologically";

			// Token: 0x0400967E RID: 38526
			public static LocString STATSPANEL = "The Stats Panel gives me an overview of my Duplicant's individual stats";

			// Token: 0x0400967F RID: 38527
			public static LocString ITEMSPANEL = "The Items Panel displays everything this Duplicant is in possession of";

			// Token: 0x04009680 RID: 38528
			public static LocString STRESSDESCRIPTION = string.Concat(new string[]
			{
				"Accommodate my Duplicant's needs to manage their ",
				UI.FormatAsLink("Stress", "STRESS"),
				".\n\nLow ",
				UI.FormatAsLink("Stress", "STRESS"),
				" can provide a productivity boost, while high ",
				UI.FormatAsLink("Stress", "STRESS"),
				" can impair production or even lead to a nervous breakdown."
			});

			// Token: 0x04009681 RID: 38529
			public static LocString ALERTSTOOLTIP = "Alerts provide important information about what's happening in the colony right now";

			// Token: 0x04009682 RID: 38530
			public static LocString MESSAGESTOOLTIP = "Messages are events that have happened and tips to help me manage my colony";

			// Token: 0x04009683 RID: 38531
			public static LocString NEXTMESSAGESTOOLTIP = "Next message";

			// Token: 0x04009684 RID: 38532
			public static LocString CLOSETOOLTIP = "Close";

			// Token: 0x04009685 RID: 38533
			public static LocString DISMISSMESSAGE = "Dismiss message";

			// Token: 0x04009686 RID: 38534
			public static LocString RECIPE_QUEUE = "Queue {0} for continuous fabrication";

			// Token: 0x04009687 RID: 38535
			public static LocString RED_ALERT_BUTTON_ON = "Enable Red Alert";

			// Token: 0x04009688 RID: 38536
			public static LocString RED_ALERT_BUTTON_OFF = "Disable Red Alert";

			// Token: 0x04009689 RID: 38537
			public static LocString JOBSSCREEN_PRIORITY = "High priority tasks are always performed before low priority tasks.\n\nHowever, a busy Duplicant will continue to work on their current work errand until it's complete, even if a more important errand becomes available.";

			// Token: 0x0400968A RID: 38538
			public static LocString JOBSSCREEN_ATTRIBUTES = "The following attributes affect a Duplicant's efficiency at this errand:";

			// Token: 0x0400968B RID: 38539
			public static LocString JOBSSCREEN_CANNOTPERFORMTASK = "{0} cannot perform this errand.";

			// Token: 0x0400968C RID: 38540
			public static LocString JOBSSCREEN_RELEVANT_ATTRIBUTES = "Relevant Attributes:";

			// Token: 0x0400968D RID: 38541
			public static LocString SORTCOLUMN = UI.CLICK(UI.ClickType.Click) + " to sort";

			// Token: 0x0400968E RID: 38542
			public static LocString NOMATERIAL = "Not enough materials";

			// Token: 0x0400968F RID: 38543
			public static LocString SELECTAMATERIAL = "There are insufficient materials to construct this building";

			// Token: 0x04009690 RID: 38544
			public static LocString EDITNAME = "Give this Duplicant a new name";

			// Token: 0x04009691 RID: 38545
			public static LocString RANDOMIZENAME = "Randomize this Duplicant's name";

			// Token: 0x04009692 RID: 38546
			public static LocString EDITNAMEGENERIC = "Rename {0}";

			// Token: 0x04009693 RID: 38547
			public static LocString EDITNAMEROCKET = "Rename this rocket";

			// Token: 0x04009694 RID: 38548
			public static LocString BASE_VALUE = "Base Value";

			// Token: 0x04009695 RID: 38549
			public static LocString MATIERIAL_MOD = "Made out of {0}";

			// Token: 0x04009696 RID: 38550
			public static LocString VITALS_CHECKBOX_TEMPERATURE = string.Concat(new string[]
			{
				"This plant's internal ",
				UI.PRE_KEYWORD,
				"Temperature",
				UI.PST_KEYWORD,
				" is <b>{temperature}</b>"
			});

			// Token: 0x04009697 RID: 38551
			public static LocString VITALS_CHECKBOX_PRESSURE = string.Concat(new string[]
			{
				"The current ",
				UI.PRE_KEYWORD,
				"Gas",
				UI.PST_KEYWORD,
				" pressure is <b>{pressure}</b>"
			});

			// Token: 0x04009698 RID: 38552
			public static LocString VITALS_CHECKBOX_ATMOSPHERE = "This plant is immersed in {element}";

			// Token: 0x04009699 RID: 38553
			public static LocString VITALS_CHECKBOX_ILLUMINATION_DARK = "This plant is currently in the dark";

			// Token: 0x0400969A RID: 38554
			public static LocString VITALS_CHECKBOX_ILLUMINATION_LIGHT = "This plant is currently lit";

			// Token: 0x0400969B RID: 38555
			public static LocString VITALS_CHECKBOX_SPACETREE_ILLUMINATION_DARK = "This plant must be lit in order to produce " + UI.PRE_KEYWORD + "Nectar" + UI.PST_KEYWORD;

			// Token: 0x0400969C RID: 38556
			public static LocString VITALS_CHECKBOX_SPACETREE_ILLUMINATION_LIGHT = string.Concat(new string[]
			{
				"This plant is currently lit, and will produce ",
				UI.PRE_KEYWORD,
				"Nectar",
				UI.PST_KEYWORD,
				" when fully grown"
			});

			// Token: 0x0400969D RID: 38557
			public static LocString VITALS_CHECKBOX_FERTILIZER = string.Concat(new string[]
			{
				"<b>{mass}</b> of ",
				UI.PRE_KEYWORD,
				"Fertilizer",
				UI.PST_KEYWORD,
				" is currently available"
			});

			// Token: 0x0400969E RID: 38558
			public static LocString VITALS_CHECKBOX_IRRIGATION = string.Concat(new string[]
			{
				"<b>{mass}</b> of ",
				UI.PRE_KEYWORD,
				"Liquid",
				UI.PST_KEYWORD,
				" is currently available"
			});

			// Token: 0x0400969F RID: 38559
			public static LocString VITALS_CHECKBOX_SUBMERGED_TRUE = "This plant is fully submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PRE_KEYWORD;

			// Token: 0x040096A0 RID: 38560
			public static LocString VITALS_CHECKBOX_SUBMERGED_FALSE = "This plant must be submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

			// Token: 0x040096A1 RID: 38561
			public static LocString VITALS_CHECKBOX_DROWNING_TRUE = "This plant is not drowning";

			// Token: 0x040096A2 RID: 38562
			public static LocString VITALS_CHECKBOX_DROWNING_FALSE = "This plant is drowning in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

			// Token: 0x040096A3 RID: 38563
			public static LocString VITALS_CHECKBOX_RECEPTACLE_OPERATIONAL = "This plant is housed in an operational farm plot";

			// Token: 0x040096A4 RID: 38564
			public static LocString VITALS_CHECKBOX_RECEPTACLE_INOPERATIONAL = "This plant is not housed in an operational farm plot";

			// Token: 0x040096A5 RID: 38565
			public static LocString VITALS_CHECKBOX_RADIATION = string.Concat(new string[]
			{
				"This plant is sitting in <b>{rads}</b> of ambient ",
				UI.PRE_KEYWORD,
				"Radiation",
				UI.PST_KEYWORD,
				". It needs at between {minRads} and {maxRads} to grow"
			});

			// Token: 0x040096A6 RID: 38566
			public static LocString VITALS_CHECKBOX_RADIATION_NO_MIN = string.Concat(new string[]
			{
				"This plant is sitting in <b>{rads}</b> of ambient ",
				UI.PRE_KEYWORD,
				"Radiation",
				UI.PST_KEYWORD,
				". It needs less than {maxRads} to grow"
			});
		}

		// Token: 0x02002164 RID: 8548
		public class CLUSTERMAP
		{
			// Token: 0x040096A7 RID: 38567
			public static LocString PLANETOID = "Planetoid";

			// Token: 0x040096A8 RID: 38568
			public static LocString PLANETOID_KEYWORD = UI.PRE_KEYWORD + UI.CLUSTERMAP.PLANETOID + UI.PST_KEYWORD;

			// Token: 0x040096A9 RID: 38569
			public static LocString TITLE = "STARMAP";

			// Token: 0x040096AA RID: 38570
			public static LocString LANDING_SITES = "LANDING SITES";

			// Token: 0x040096AB RID: 38571
			public static LocString DESTINATION = "DESTINATION";

			// Token: 0x040096AC RID: 38572
			public static LocString OCCUPANTS = "CREW";

			// Token: 0x040096AD RID: 38573
			public static LocString ELEMENTS = "ELEMENTS";

			// Token: 0x040096AE RID: 38574
			public static LocString UNKNOWN_DESTINATION = "Unknown";

			// Token: 0x040096AF RID: 38575
			public static LocString TILES = "Tiles";

			// Token: 0x040096B0 RID: 38576
			public static LocString TILES_PER_CYCLE = "Tiles per cycle";

			// Token: 0x040096B1 RID: 38577
			public static LocString CHANGE_DESTINATION = UI.CLICK(UI.ClickType.Click) + " to change destination";

			// Token: 0x040096B2 RID: 38578
			public static LocString SELECT_DESTINATION = "Select a new destination on the map";

			// Token: 0x040096B3 RID: 38579
			public static LocString TOOLTIP_INVALID_DESTINATION_FOG_OF_WAR = "Rockets cannot travel to this hex until it has been analyzed\n\nSpace can be analyzed with a " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME + " or " + BUILDINGS.PREFABS.SCANNERMODULE.NAME;

			// Token: 0x040096B4 RID: 38580
			public static LocString TOOLTIP_INVALID_DESTINATION_NO_PATH = string.Concat(new string[]
			{
				"There is no navigable rocket path to this ",
				UI.CLUSTERMAP.PLANETOID_KEYWORD,
				"\n\nSpace can be analyzed with a ",
				BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME,
				" or ",
				BUILDINGS.PREFABS.SCANNERMODULE.NAME,
				" to clear the way"
			});

			// Token: 0x040096B5 RID: 38581
			public static LocString TOOLTIP_INVALID_DESTINATION_NO_LAUNCH_PAD = string.Concat(new string[]
			{
				"There is no ",
				BUILDINGS.PREFABS.LAUNCHPAD.NAME,
				" on this ",
				UI.CLUSTERMAP.PLANETOID_KEYWORD,
				" for a rocket to land on\n\nUse a ",
				BUILDINGS.PREFABS.PIONEERMODULE.NAME,
				" or ",
				BUILDINGS.PREFABS.SCOUTMODULE.NAME,
				" to deploy a scout and make first contact"
			});

			// Token: 0x040096B6 RID: 38582
			public static LocString TOOLTIP_INVALID_DESTINATION_REQUIRE_ASTEROID = "Must select a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " destination";

			// Token: 0x040096B7 RID: 38583
			public static LocString TOOLTIP_INVALID_DESTINATION_OUT_OF_RANGE = "This destination is further away than the rocket's maximum range of {0}.";

			// Token: 0x040096B8 RID: 38584
			public static LocString TOOLTIP_HIDDEN_HEX = "???";

			// Token: 0x040096B9 RID: 38585
			public static LocString TOOLTIP_PEEKED_HEX_WITH_OBJECT = "UNKNOWN OBJECT DETECTED!";

			// Token: 0x040096BA RID: 38586
			public static LocString TOOLTIP_EMPTY_HEX = "EMPTY SPACE";

			// Token: 0x040096BB RID: 38587
			public static LocString TOOLTIP_PATH_LENGTH = "Trip Distance: {0}/{1}";

			// Token: 0x040096BC RID: 38588
			public static LocString TOOLTIP_PATH_LENGTH_RETURN = "Trip Distance: {0}/{1} (Return Trip)";

			// Token: 0x02002AA7 RID: 10919
			public class STATUS
			{
				// Token: 0x0400B936 RID: 47414
				public static LocString NORMAL = "Normal";

				// Token: 0x020036DE RID: 14046
				public class ROCKET
				{
					// Token: 0x0400DAFB RID: 56059
					public static LocString GROUNDED = "Normal";

					// Token: 0x0400DAFC RID: 56060
					public static LocString TRAVELING = "Traveling";

					// Token: 0x0400DAFD RID: 56061
					public static LocString STRANDED = "Stranded";

					// Token: 0x0400DAFE RID: 56062
					public static LocString IDLE = "Idle";
				}
			}

			// Token: 0x02002AA8 RID: 10920
			public class ASTEROIDS
			{
				// Token: 0x020036DF RID: 14047
				public class ELEMENT_AMOUNTS
				{
					// Token: 0x0400DAFF RID: 56063
					public static LocString LOTS = "Plentiful";

					// Token: 0x0400DB00 RID: 56064
					public static LocString SOME = "Significant amount";

					// Token: 0x0400DB01 RID: 56065
					public static LocString LITTLE = "Small amount";

					// Token: 0x0400DB02 RID: 56066
					public static LocString VERY_LITTLE = "Trace amount";
				}

				// Token: 0x020036E0 RID: 14048
				public class SURFACE_CONDITIONS
				{
					// Token: 0x0400DB03 RID: 56067
					public static LocString LIGHT = "Peak Light";

					// Token: 0x0400DB04 RID: 56068
					public static LocString RADIATION = "Cosmic Radiation";
				}
			}

			// Token: 0x02002AA9 RID: 10921
			public class POI
			{
				// Token: 0x0400B937 RID: 47415
				public static LocString TITLE = "POINT OF INTEREST";

				// Token: 0x0400B938 RID: 47416
				public static LocString MASS_REMAINING = "<b>Total Mass Remaining</b>";

				// Token: 0x0400B939 RID: 47417
				public static LocString ROCKETS_AT_THIS_LOCATION = "<b>Rockets at this location</b>";

				// Token: 0x0400B93A RID: 47418
				public static LocString ARTIFACTS = "Artifact";

				// Token: 0x0400B93B RID: 47419
				public static LocString ARTIFACTS_AVAILABLE = "Available";

				// Token: 0x0400B93C RID: 47420
				public static LocString ARTIFACTS_DEPLETED = "Collected\nRecharge: {0}";
			}

			// Token: 0x02002AAA RID: 10922
			public class ROCKETS
			{
				// Token: 0x020036E1 RID: 14049
				public class SPEED
				{
					// Token: 0x0400DB05 RID: 56069
					public static LocString NAME = "Rocket Speed: ";

					// Token: 0x0400DB06 RID: 56070
					public static LocString TOOLTIP = "<b>Rocket Speed</b> is calculated by dividing <b>Engine Power</b> by <b>Burden</b>.\n\nRockets operating on autopilot will have a reduced speed.\n\nRocket speed can be further increased by the skill of the Duplicant flying the rocket.";
				}

				// Token: 0x020036E2 RID: 14050
				public class FUEL_REMAINING
				{
					// Token: 0x0400DB07 RID: 56071
					public static LocString NAME = "Fuel Remaining: ";

					// Token: 0x0400DB08 RID: 56072
					public static LocString TOOLTIP = "This rocket has {0} fuel in its tank";
				}

				// Token: 0x020036E3 RID: 14051
				public class OXIDIZER_REMAINING
				{
					// Token: 0x0400DB09 RID: 56073
					public static LocString NAME = "Oxidizer Power Remaining: ";

					// Token: 0x0400DB0A RID: 56074
					public static LocString TOOLTIP = "This rocket has enough oxidizer in its tank for {0} of fuel";
				}

				// Token: 0x020036E4 RID: 14052
				public class RANGE
				{
					// Token: 0x0400DB0B RID: 56075
					public static LocString NAME = "Range Remaining: ";

					// Token: 0x0400DB0C RID: 56076
					public static LocString TOOLTIP = "<b>Range remaining</b> is calculated by dividing the lesser of <b>fuel remaining</b> and <b>oxidizer power remaining</b> by <b>fuel consumed per tile</b>";
				}

				// Token: 0x020036E5 RID: 14053
				public class FUEL_PER_HEX
				{
					// Token: 0x0400DB0D RID: 56077
					public static LocString NAME = "Fuel consumed per Tile: {0}";

					// Token: 0x0400DB0E RID: 56078
					public static LocString TOOLTIP = "This rocket can travel one tile per {0} of fuel";
				}

				// Token: 0x020036E6 RID: 14054
				public class BURDEN_TOTAL
				{
					// Token: 0x0400DB0F RID: 56079
					public static LocString NAME = "Rocket burden: ";

					// Token: 0x0400DB10 RID: 56080
					public static LocString TOOLTIP = "The combined burden of all the modules in this rocket";
				}

				// Token: 0x020036E7 RID: 14055
				public class BURDEN_MODULE
				{
					// Token: 0x0400DB11 RID: 56081
					public static LocString NAME = "Module Burden: ";

					// Token: 0x0400DB12 RID: 56082
					public static LocString TOOLTIP = "The selected module adds {0} to the rocket's total " + DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME;
				}

				// Token: 0x020036E8 RID: 14056
				public class POWER_TOTAL
				{
					// Token: 0x0400DB13 RID: 56083
					public static LocString NAME = "Rocket engine power: ";

					// Token: 0x0400DB14 RID: 56084
					public static LocString TOOLTIP = "The total engine power added by all the modules in this rocket";
				}

				// Token: 0x020036E9 RID: 14057
				public class POWER_MODULE
				{
					// Token: 0x0400DB15 RID: 56085
					public static LocString NAME = "Module Engine Power: ";

					// Token: 0x0400DB16 RID: 56086
					public static LocString TOOLTIP = "The selected module adds {0} to the rocket's total " + DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME;
				}

				// Token: 0x020036EA RID: 14058
				public class MODULE_STATS
				{
					// Token: 0x0400DB17 RID: 56087
					public static LocString NAME = "Module Stats: ";

					// Token: 0x0400DB18 RID: 56088
					public static LocString NAME_HEADER = "Module Stats";

					// Token: 0x0400DB19 RID: 56089
					public static LocString TOOLTIP = "Properties of the selected module";
				}

				// Token: 0x020036EB RID: 14059
				public class MAX_MODULES
				{
					// Token: 0x0400DB1A RID: 56090
					public static LocString NAME = "Max Modules: ";

					// Token: 0x0400DB1B RID: 56091
					public static LocString TOOLTIP = "The {0} can support {1} rocket modules, plus itself";
				}

				// Token: 0x020036EC RID: 14060
				public class MAX_HEIGHT
				{
					// Token: 0x0400DB1C RID: 56092
					public static LocString NAME = "Height: {0}/{1}";

					// Token: 0x0400DB1D RID: 56093
					public static LocString NAME_RAW = "Height: ";

					// Token: 0x0400DB1E RID: 56094
					public static LocString NAME_MAX_SUPPORTED = "Maximum supported rocket height: ";

					// Token: 0x0400DB1F RID: 56095
					public static LocString TOOLTIP = "The {0} can support a total rocket height {1}";
				}

				// Token: 0x020036ED RID: 14061
				public class ARTIFACT_MODULE
				{
					// Token: 0x0400DB20 RID: 56096
					public static LocString EMPTY = "Empty";
				}
			}
		}

		// Token: 0x02002165 RID: 8549
		public class STARMAP
		{
			// Token: 0x040096BD RID: 38589
			public static LocString TITLE = "STARMAP";

			// Token: 0x040096BE RID: 38590
			public static LocString MANAGEMENT_BUTTON = "STARMAP";

			// Token: 0x040096BF RID: 38591
			public static LocString SUBROW = "•  {0}";

			// Token: 0x040096C0 RID: 38592
			public static LocString UNKNOWN_DESTINATION = "Destination Unknown";

			// Token: 0x040096C1 RID: 38593
			public static LocString ANALYSIS_AMOUNT = "Analysis {0} Complete";

			// Token: 0x040096C2 RID: 38594
			public static LocString ANALYSIS_COMPLETE = "ANALYSIS COMPLETE";

			// Token: 0x040096C3 RID: 38595
			public static LocString NO_ANALYZABLE_DESTINATION_SELECTED = "No destination selected";

			// Token: 0x040096C4 RID: 38596
			public static LocString UNKNOWN_TYPE = "Type Unknown";

			// Token: 0x040096C5 RID: 38597
			public static LocString DISTANCE = "{0} km";

			// Token: 0x040096C6 RID: 38598
			public static LocString MODULE_MASS = "+ {0} t";

			// Token: 0x040096C7 RID: 38599
			public static LocString MODULE_STORAGE = "{0} / {1}";

			// Token: 0x040096C8 RID: 38600
			public static LocString ANALYSIS_DESCRIPTION = "Use a Telescope to analyze space destinations.\n\nCompleting analysis on an object will unlock rocket missions to that destination.";

			// Token: 0x040096C9 RID: 38601
			public static LocString RESEARCH_DESCRIPTION = "Gather Interstellar Research Data using Research Modules.";

			// Token: 0x040096CA RID: 38602
			public static LocString ROCKET_RENAME_BUTTON_TOOLTIP = "Rename this rocket";

			// Token: 0x040096CB RID: 38603
			public static LocString NO_ROCKETS_HELP_TEXT = "Rockets allow you to visit nearby celestial bodies.\n\nEach rocket must have a Command Module, an Engine, and Fuel.\n\nYou can also carry other modules that allow you to gather specific resources from the places you visit.\n\nRemember the more weight a rocket has, the more limited it'll be on the distance it can travel. You can add more fuel to fix that, but fuel will add weight as well.";

			// Token: 0x040096CC RID: 38604
			public static LocString CONTAINER_REQUIRED = "{0} installation required to retrieve material";

			// Token: 0x040096CD RID: 38605
			public static LocString CAN_CARRY_ELEMENT = "Gathered by: {1}";

			// Token: 0x040096CE RID: 38606
			public static LocString CANT_CARRY_ELEMENT = "{0} installation required to retrieve material";

			// Token: 0x040096CF RID: 38607
			public static LocString STATUS = "SELECTED";

			// Token: 0x040096D0 RID: 38608
			public static LocString DISTANCE_OVERLAY = "TOO FAR FOR THIS ROCKET";

			// Token: 0x040096D1 RID: 38609
			public static LocString COMPOSITION_UNDISCOVERED = "?????????";

			// Token: 0x040096D2 RID: 38610
			public static LocString COMPOSITION_UNDISCOVERED_TOOLTIP = "Further research required to identify resource\n\nSend a Research Module to this destination for more information";

			// Token: 0x040096D3 RID: 38611
			public static LocString COMPOSITION_UNDISCOVERED_AMOUNT = "???";

			// Token: 0x040096D4 RID: 38612
			public static LocString COMPOSITION_SMALL_AMOUNT = "Trace Amount";

			// Token: 0x040096D5 RID: 38613
			public static LocString CURRENT_MASS = "Current Mass";

			// Token: 0x040096D6 RID: 38614
			public static LocString CURRENT_MASS_TOOLTIP = "Warning: Missions to this destination will not return a full cargo load to avoid depleting the destination for future explorations\n\nDestination: {0} Resources Available\nRocket Capacity: {1}";

			// Token: 0x040096D7 RID: 38615
			public static LocString MAXIMUM_MASS = "Maximum Mass";

			// Token: 0x040096D8 RID: 38616
			public static LocString MINIMUM_MASS = "Minimum Mass";

			// Token: 0x040096D9 RID: 38617
			public static LocString MINIMUM_MASS_TOOLTIP = "This destination must retain at least this much mass in order to prevent depletion and allow the future regeneration of resources.\n\nDuplicants will always maintain a destination's minimum mass requirements, potentially returning with less cargo than their rocket can hold";

			// Token: 0x040096DA RID: 38618
			public static LocString REPLENISH_RATE = "Replenished/Cycle:";

			// Token: 0x040096DB RID: 38619
			public static LocString REPLENISH_RATE_TOOLTIP = "The rate at which this destination regenerates resources";

			// Token: 0x040096DC RID: 38620
			public static LocString ROCKETLIST = "Rocket Hangar";

			// Token: 0x040096DD RID: 38621
			public static LocString NO_ROCKETS_TITLE = "NO ROCKETS";

			// Token: 0x040096DE RID: 38622
			public static LocString ROCKET_COUNT = "ROCKETS: {0}";

			// Token: 0x040096DF RID: 38623
			public static LocString LAUNCH_MISSION = "LAUNCH MISSION";

			// Token: 0x040096E0 RID: 38624
			public static LocString CANT_LAUNCH_MISSION = "CANNOT LAUNCH";

			// Token: 0x040096E1 RID: 38625
			public static LocString LAUNCH_ROCKET = "Launch Rocket";

			// Token: 0x040096E2 RID: 38626
			public static LocString LAND_ROCKET = "Land Rocket";

			// Token: 0x040096E3 RID: 38627
			public static LocString SEE_ROCKETS_LIST = "See Rockets List";

			// Token: 0x040096E4 RID: 38628
			public static LocString DEFAULT_NAME = "Rocket";

			// Token: 0x040096E5 RID: 38629
			public static LocString ANALYZE_DESTINATION = "ANALYZE OBJECT";

			// Token: 0x040096E6 RID: 38630
			public static LocString SUSPEND_DESTINATION_ANALYSIS = "PAUSE ANALYSIS";

			// Token: 0x040096E7 RID: 38631
			public static LocString DESTINATIONTITLE = "Destination Status";

			// Token: 0x02002AAB RID: 10923
			public class DESTINATIONSTUDY
			{
				// Token: 0x0400B93D RID: 47421
				public static LocString UPPERATMO = "Study upper atmosphere";

				// Token: 0x0400B93E RID: 47422
				public static LocString LOWERATMO = "Study lower atmosphere";

				// Token: 0x0400B93F RID: 47423
				public static LocString MAGNETICFIELD = "Study magnetic field";

				// Token: 0x0400B940 RID: 47424
				public static LocString SURFACE = "Study surface";

				// Token: 0x0400B941 RID: 47425
				public static LocString SUBSURFACE = "Study subsurface";
			}

			// Token: 0x02002AAC RID: 10924
			public class COMPONENT
			{
				// Token: 0x0400B942 RID: 47426
				public static LocString FUEL_TANK = "Fuel Tank";

				// Token: 0x0400B943 RID: 47427
				public static LocString ROCKET_ENGINE = "Rocket Engine";

				// Token: 0x0400B944 RID: 47428
				public static LocString CARGO_BAY = "Cargo Bay";

				// Token: 0x0400B945 RID: 47429
				public static LocString OXIDIZER_TANK = "Oxidizer Tank";
			}

			// Token: 0x02002AAD RID: 10925
			public class MISSION_STATUS
			{
				// Token: 0x0400B946 RID: 47430
				public static LocString GROUNDED = "Grounded";

				// Token: 0x0400B947 RID: 47431
				public static LocString LAUNCHING = "Launching";

				// Token: 0x0400B948 RID: 47432
				public static LocString WAITING_TO_LAND = "Waiting To Land";

				// Token: 0x0400B949 RID: 47433
				public static LocString LANDING = "Landing";

				// Token: 0x0400B94A RID: 47434
				public static LocString UNDERWAY = "Underway";

				// Token: 0x0400B94B RID: 47435
				public static LocString UNDERWAY_BOOSTED = "Underway <color=#5FDB37FF>(Boosted)</color>";

				// Token: 0x0400B94C RID: 47436
				public static LocString DESTROYED = "Destroyed";

				// Token: 0x0400B94D RID: 47437
				public static LocString GO = "ALL SYSTEMS GO";
			}

			// Token: 0x02002AAE RID: 10926
			public class LISTTITLES
			{
				// Token: 0x0400B94E RID: 47438
				public static LocString MISSIONSTATUS = "Mission Status";

				// Token: 0x0400B94F RID: 47439
				public static LocString LAUNCHCHECKLIST = "Launch Checklist";

				// Token: 0x0400B950 RID: 47440
				public static LocString MAXRANGE = "Max Range";

				// Token: 0x0400B951 RID: 47441
				public static LocString MASS = "Mass";

				// Token: 0x0400B952 RID: 47442
				public static LocString STORAGE = "Storage";

				// Token: 0x0400B953 RID: 47443
				public static LocString FUEL = "Fuel";

				// Token: 0x0400B954 RID: 47444
				public static LocString OXIDIZER = "Oxidizer";

				// Token: 0x0400B955 RID: 47445
				public static LocString PASSENGERS = "Passengers";

				// Token: 0x0400B956 RID: 47446
				public static LocString RESEARCH = "Research";

				// Token: 0x0400B957 RID: 47447
				public static LocString ARTIFACTS = "Artifacts";

				// Token: 0x0400B958 RID: 47448
				public static LocString ANALYSIS = "Analysis";

				// Token: 0x0400B959 RID: 47449
				public static LocString WORLDCOMPOSITION = "World Composition";

				// Token: 0x0400B95A RID: 47450
				public static LocString RESOURCES = "Resources";

				// Token: 0x0400B95B RID: 47451
				public static LocString MODULES = "Modules";

				// Token: 0x0400B95C RID: 47452
				public static LocString TYPE = "Type";

				// Token: 0x0400B95D RID: 47453
				public static LocString DISTANCE = "Distance";

				// Token: 0x0400B95E RID: 47454
				public static LocString DESTINATION_MASS = "World Mass Available";

				// Token: 0x0400B95F RID: 47455
				public static LocString STORAGECAPACITY = "Storage Capacity";
			}

			// Token: 0x02002AAF RID: 10927
			public class ROCKETWEIGHT
			{
				// Token: 0x0400B960 RID: 47456
				public static LocString MASS = "Mass: ";

				// Token: 0x0400B961 RID: 47457
				public static LocString MASSPENALTY = "Mass Penalty: ";

				// Token: 0x0400B962 RID: 47458
				public static LocString CURRENTMASS = "Current Rocket Mass: ";

				// Token: 0x0400B963 RID: 47459
				public static LocString CURRENTMASSPENALTY = "Current Weight Penalty: ";
			}

			// Token: 0x02002AB0 RID: 10928
			public class DESTINATIONSELECTION
			{
				// Token: 0x0400B964 RID: 47460
				public static LocString REACHABLE = "Destination set";

				// Token: 0x0400B965 RID: 47461
				public static LocString UNREACHABLE = "Destination set";

				// Token: 0x0400B966 RID: 47462
				public static LocString NOTSELECTED = "Destination set";
			}

			// Token: 0x02002AB1 RID: 10929
			public class DESTINATIONSELECTION_TOOLTIP
			{
				// Token: 0x0400B967 RID: 47463
				public static LocString REACHABLE = "Viable destination selected, ready for launch";

				// Token: 0x0400B968 RID: 47464
				public static LocString UNREACHABLE = "The selected destination is beyond rocket reach";

				// Token: 0x0400B969 RID: 47465
				public static LocString NOTSELECTED = "Select the rocket's Command Module to set a destination";
			}

			// Token: 0x02002AB2 RID: 10930
			public class HASFOOD
			{
				// Token: 0x0400B96A RID: 47466
				public static LocString NAME = "Food Loaded";

				// Token: 0x0400B96B RID: 47467
				public static LocString TOOLTIP = "Sufficient food stores have been loaded, ready for launch";
			}

			// Token: 0x02002AB3 RID: 10931
			public class HASSUIT
			{
				// Token: 0x0400B96C RID: 47468
				public static LocString NAME = "Has " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400B96D RID: 47469
				public static LocString TOOLTIP = "An " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME + " has been loaded";
			}

			// Token: 0x02002AB4 RID: 10932
			public class NOSUIT
			{
				// Token: 0x0400B96E RID: 47470
				public static LocString NAME = "Missing " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME;

				// Token: 0x0400B96F RID: 47471
				public static LocString TOOLTIP = "Rocket cannot launch without an " + EQUIPMENT.PREFABS.ATMO_SUIT.NAME + " loaded";
			}

			// Token: 0x02002AB5 RID: 10933
			public class NOFOOD
			{
				// Token: 0x0400B970 RID: 47472
				public static LocString NAME = "Insufficient Food";

				// Token: 0x0400B971 RID: 47473
				public static LocString TOOLTIP = "Rocket cannot launch without adequate food stores for passengers";
			}

			// Token: 0x02002AB6 RID: 10934
			public class CARGOEMPTY
			{
				// Token: 0x0400B972 RID: 47474
				public static LocString NAME = "Emptied Cargo Bay";

				// Token: 0x0400B973 RID: 47475
				public static LocString TOOLTIP = "Cargo Bays must be emptied of all materials before launch";
			}

			// Token: 0x02002AB7 RID: 10935
			public class LAUNCHCHECKLIST
			{
				// Token: 0x0400B974 RID: 47476
				public static LocString ASTRONAUT_TITLE = "Astronaut";

				// Token: 0x0400B975 RID: 47477
				public static LocString HASASTRONAUT = "Astronaut ready for liftoff";

				// Token: 0x0400B976 RID: 47478
				public static LocString ASTRONAUGHT = "No Astronaut assigned";

				// Token: 0x0400B977 RID: 47479
				public static LocString INSTALLED = "Installed";

				// Token: 0x0400B978 RID: 47480
				public static LocString INSTALLED_TOOLTIP = "A suitable {0} has been installed";

				// Token: 0x0400B979 RID: 47481
				public static LocString REQUIRED = "Required";

				// Token: 0x0400B97A RID: 47482
				public static LocString REQUIRED_TOOLTIP = "A {0} must be installed before launch";

				// Token: 0x0400B97B RID: 47483
				public static LocString MISSING_TOOLTIP = "No {0} installed\n\nThis rocket cannot launch without a completed {0}";

				// Token: 0x0400B97C RID: 47484
				public static LocString NO_DESTINATION = "No destination selected";

				// Token: 0x0400B97D RID: 47485
				public static LocString MINIMUM_MASS = "Resources available {0}";

				// Token: 0x0400B97E RID: 47486
				public static LocString RESOURCE_MASS_TOOLTIP = "{0} has {1} resources available\nThis rocket has capacity for {2}";

				// Token: 0x0400B97F RID: 47487
				public static LocString INSUFFICENT_MASS_TOOLTIP = "Launching to this destination will not return a full cargo load";

				// Token: 0x020036EE RID: 14062
				public class CONSTRUCTION_COMPLETE
				{
					// Token: 0x02003B10 RID: 15120
					public class STATUS
					{
						// Token: 0x0400E516 RID: 58646
						public static LocString READY = "No active construction";

						// Token: 0x0400E517 RID: 58647
						public static LocString FAILURE = "No active construction";

						// Token: 0x0400E518 RID: 58648
						public static LocString WARNING = "No active construction";
					}

					// Token: 0x02003B11 RID: 15121
					public class TOOLTIP
					{
						// Token: 0x0400E519 RID: 58649
						public static LocString READY = "Construction of all modules is complete";

						// Token: 0x0400E51A RID: 58650
						public static LocString FAILURE = "In-progress module construction is preventing takeoff";

						// Token: 0x0400E51B RID: 58651
						public static LocString WARNING = "Construction warning";
					}
				}

				// Token: 0x020036EF RID: 14063
				public class PILOT_BOARDED
				{
					// Token: 0x0400DB21 RID: 56097
					public static LocString READY = "Pilot boarded";

					// Token: 0x0400DB22 RID: 56098
					public static LocString FAILURE = "Pilot boarded";

					// Token: 0x0400DB23 RID: 56099
					public static LocString WARNING = "Pilot boarded";

					// Token: 0x0400DB24 RID: 56100
					public static LocString ROBO_PILOT_WARNING = "Robo-Pilot solo flight";

					// Token: 0x02003B12 RID: 15122
					public class TOOLTIP
					{
						// Token: 0x0400E51C RID: 58652
						public static LocString READY = "A Duplicant with the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill is currently onboard";

						// Token: 0x0400E51D RID: 58653
						public static LocString FAILURE = "At least one crew member aboard the rocket must possess the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill to launch\n\nQualified Duplicants must be assigned to the rocket crew, and have access to the module's hatch";

						// Token: 0x0400E51E RID: 58654
						public static LocString WARNING = "Pilot warning";

						// Token: 0x0400E51F RID: 58655
						public static LocString ROBO_PILOT_WARNING = "This rocket is being piloted by a Robo-Pilot\n\nThere are no Duplicants with the " + DUPLICANTS.ROLES.ROCKETPILOT.NAME + " skill currently onboard";
					}
				}

				// Token: 0x020036F0 RID: 14064
				public class CREW_BOARDED
				{
					// Token: 0x0400DB25 RID: 56101
					public static LocString READY = "All crew boarded";

					// Token: 0x0400DB26 RID: 56102
					public static LocString FAILURE = "All crew boarded";

					// Token: 0x0400DB27 RID: 56103
					public static LocString WARNING = "All crew boarded";

					// Token: 0x02003B13 RID: 15123
					public class TOOLTIP
					{
						// Token: 0x0400E520 RID: 58656
						public static LocString READY = "All Duplicants assigned to the rocket crew are boarded and ready for launch\n\n    • {0}/{1} Boarded";

						// Token: 0x0400E521 RID: 58657
						public static LocString FAILURE = "No crew members have boarded this rocket\n\nDuplicants must be assigned to the rocket crew and have access to the module's hatch to board\n\n    • {0}/{1} Boarded";

						// Token: 0x0400E522 RID: 58658
						public static LocString WARNING = "Some Duplicants assigned to this rocket crew have not yet boarded\n    • {0}/{1} Boarded";

						// Token: 0x0400E523 RID: 58659
						public static LocString NONE = "There are no Duplicants assigned to this rocket crew\n    • {0}/{1} Boarded";
					}
				}

				// Token: 0x020036F1 RID: 14065
				public class NO_EXTRA_PASSENGERS
				{
					// Token: 0x0400DB28 RID: 56104
					public static LocString READY = "Non-crew exited";

					// Token: 0x0400DB29 RID: 56105
					public static LocString FAILURE = "Non-crew exited";

					// Token: 0x0400DB2A RID: 56106
					public static LocString WARNING = "Non-crew exited";

					// Token: 0x02003B14 RID: 15124
					public class TOOLTIP
					{
						// Token: 0x0400E524 RID: 58660
						public static LocString READY = "All non-crew Duplicants have disembarked";

						// Token: 0x0400E525 RID: 58661
						public static LocString FAILURE = "Non-crew Duplicants must exit the rocket before launch";

						// Token: 0x0400E526 RID: 58662
						public static LocString WARNING = "Non-crew warning";
					}
				}

				// Token: 0x020036F2 RID: 14066
				public class FLIGHT_PATH_CLEAR
				{
					// Token: 0x02003B15 RID: 15125
					public class STATUS
					{
						// Token: 0x0400E527 RID: 58663
						public static LocString READY = "Clear launch path";

						// Token: 0x0400E528 RID: 58664
						public static LocString FAILURE = "Clear launch path";

						// Token: 0x0400E529 RID: 58665
						public static LocString WARNING = "Clear launch path";
					}

					// Token: 0x02003B16 RID: 15126
					public class TOOLTIP
					{
						// Token: 0x0400E52A RID: 58666
						public static LocString READY = "The rocket's launch path is clear for takeoff";

						// Token: 0x0400E52B RID: 58667
						public static LocString FAILURE = "This rocket does not have a clear line of sight to space, preventing launch\n\nThe rocket's launch path can be cleared by excavating undug tiles and deconstructing any buildings above the rocket";

						// Token: 0x0400E52C RID: 58668
						public static LocString WARNING = "";
					}
				}

				// Token: 0x020036F3 RID: 14067
				public class HAS_FUEL_TANK
				{
					// Token: 0x02003B17 RID: 15127
					public class STATUS
					{
						// Token: 0x0400E52D RID: 58669
						public static LocString READY = "Fuel Tank";

						// Token: 0x0400E52E RID: 58670
						public static LocString FAILURE = "Fuel Tank";

						// Token: 0x0400E52F RID: 58671
						public static LocString WARNING = "Fuel Tank";
					}

					// Token: 0x02003B18 RID: 15128
					public class TOOLTIP
					{
						// Token: 0x0400E530 RID: 58672
						public static LocString READY = "A fuel tank has been installed";

						// Token: 0x0400E531 RID: 58673
						public static LocString FAILURE = "No fuel tank installed\n\nThis rocket cannot launch without a completed fuel tank";

						// Token: 0x0400E532 RID: 58674
						public static LocString WARNING = "Fuel tank warning";
					}
				}

				// Token: 0x020036F4 RID: 14068
				public class HAS_ENGINE
				{
					// Token: 0x02003B19 RID: 15129
					public class STATUS
					{
						// Token: 0x0400E533 RID: 58675
						public static LocString READY = "Engine";

						// Token: 0x0400E534 RID: 58676
						public static LocString FAILURE = "Engine";

						// Token: 0x0400E535 RID: 58677
						public static LocString WARNING = "Engine";
					}

					// Token: 0x02003B1A RID: 15130
					public class TOOLTIP
					{
						// Token: 0x0400E536 RID: 58678
						public static LocString READY = "A suitable engine has been installed";

						// Token: 0x0400E537 RID: 58679
						public static LocString FAILURE = "No engine installed\n\nThis rocket cannot launch without a completed engine";

						// Token: 0x0400E538 RID: 58680
						public static LocString WARNING = "Engine warning";
					}
				}

				// Token: 0x020036F5 RID: 14069
				public class HAS_NOSECONE
				{
					// Token: 0x02003B1B RID: 15131
					public class STATUS
					{
						// Token: 0x0400E539 RID: 58681
						public static LocString READY = "Nosecone";

						// Token: 0x0400E53A RID: 58682
						public static LocString FAILURE = "Nosecone";

						// Token: 0x0400E53B RID: 58683
						public static LocString WARNING = "Nosecone";
					}

					// Token: 0x02003B1C RID: 15132
					public class TOOLTIP
					{
						// Token: 0x0400E53C RID: 58684
						public static LocString READY = "A suitable nosecone has been installed";

						// Token: 0x0400E53D RID: 58685
						public static LocString FAILURE = "No nosecone installed\n\nThis rocket cannot launch without a completed nosecone";

						// Token: 0x0400E53E RID: 58686
						public static LocString WARNING = "Nosecone warning";
					}
				}

				// Token: 0x020036F6 RID: 14070
				public class HAS_CARGO_BAY_FOR_NOSECONE_HARVEST
				{
					// Token: 0x02003B1D RID: 15133
					public class STATUS
					{
						// Token: 0x0400E53F RID: 58687
						public static LocString READY = "Drillcone Cargo Bay";

						// Token: 0x0400E540 RID: 58688
						public static LocString FAILURE = "Drillcone Cargo Bay";

						// Token: 0x0400E541 RID: 58689
						public static LocString WARNING = "Drillcone Cargo Bay";
					}

					// Token: 0x02003B1E RID: 15134
					public class TOOLTIP
					{
						// Token: 0x0400E542 RID: 58690
						public static LocString READY = "A suitable cargo bay has been installed";

						// Token: 0x0400E543 RID: 58691
						public static LocString FAILURE = "No cargo bay installed\n\nThis rocket has a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + " installed but nowhere to store the materials";

						// Token: 0x0400E544 RID: 58692
						public static LocString WARNING = "No cargo bay installed\n\nThis rocket has a " + UI.FormatAsLink("Drillcone", "NOSECONEHARVEST") + " installed but nowhere to store the materials";
					}
				}

				// Token: 0x020036F7 RID: 14071
				public class HAS_CONTROLSTATION
				{
					// Token: 0x02003B1F RID: 15135
					public class STATUS
					{
						// Token: 0x0400E545 RID: 58693
						public static LocString READY = "Control Station";

						// Token: 0x0400E546 RID: 58694
						public static LocString FAILURE = "Control Station";

						// Token: 0x0400E547 RID: 58695
						public static LocString WARNING = "Control Station";
					}

					// Token: 0x02003B20 RID: 15136
					public class TOOLTIP
					{
						// Token: 0x0400E548 RID: 58696
						public static LocString READY = "The control station is installed and waiting for the pilot";

						// Token: 0x0400E549 RID: 58697
						public static LocString FAILURE = "No Control Station\n\nA new Rocket Control Station must be installed inside the rocket";

						// Token: 0x0400E54A RID: 58698
						public static LocString WARNING = "Control Station warning";
					}
				}

				// Token: 0x020036F8 RID: 14072
				public class LOADING_COMPLETE
				{
					// Token: 0x02003B21 RID: 15137
					public class STATUS
					{
						// Token: 0x0400E54B RID: 58699
						public static LocString READY = "Cargo Loading Complete";

						// Token: 0x0400E54C RID: 58700
						public static LocString FAILURE = "";

						// Token: 0x0400E54D RID: 58701
						public static LocString WARNING = "Cargo Loading Complete";
					}

					// Token: 0x02003B22 RID: 15138
					public class TOOLTIP
					{
						// Token: 0x0400E54E RID: 58702
						public static LocString READY = "All possible loading and unloading has been completed";

						// Token: 0x0400E54F RID: 58703
						public static LocString FAILURE = "";

						// Token: 0x0400E550 RID: 58704
						public static LocString WARNING = "The " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " could still transfer cargo to or from this rocket";
					}
				}

				// Token: 0x020036F9 RID: 14073
				public class CARGO_TRANSFER_COMPLETE
				{
					// Token: 0x02003B23 RID: 15139
					public class STATUS
					{
						// Token: 0x0400E551 RID: 58705
						public static LocString READY = "Cargo Transfer Complete";

						// Token: 0x0400E552 RID: 58706
						public static LocString FAILURE = "";

						// Token: 0x0400E553 RID: 58707
						public static LocString WARNING = "Cargo Transfer Complete";
					}

					// Token: 0x02003B24 RID: 15140
					public class TOOLTIP
					{
						// Token: 0x0400E554 RID: 58708
						public static LocString READY = "All possible loading and unloading has been completed";

						// Token: 0x0400E555 RID: 58709
						public static LocString FAILURE = "";

						// Token: 0x0400E556 RID: 58710
						public static LocString WARNING = "The " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + " could still transfer cargo to or from this rocket";
					}
				}

				// Token: 0x020036FA RID: 14074
				public class INTERNAL_CONSTRUCTION_COMPLETE
				{
					// Token: 0x02003B25 RID: 15141
					public class STATUS
					{
						// Token: 0x0400E557 RID: 58711
						public static LocString READY = "Landers Ready";

						// Token: 0x0400E558 RID: 58712
						public static LocString FAILURE = "Landers Ready";

						// Token: 0x0400E559 RID: 58713
						public static LocString WARNING = "";
					}

					// Token: 0x02003B26 RID: 15142
					public class TOOLTIP
					{
						// Token: 0x0400E55A RID: 58714
						public static LocString READY = "All requested landers have been built and are ready for deployment";

						// Token: 0x0400E55B RID: 58715
						public static LocString FAILURE = "Additional landers must be constructed to fulfill the lander requests of this rocket";

						// Token: 0x0400E55C RID: 58716
						public static LocString WARNING = "";
					}
				}

				// Token: 0x020036FB RID: 14075
				public class MAX_MODULES
				{
					// Token: 0x02003B27 RID: 15143
					public class STATUS
					{
						// Token: 0x0400E55D RID: 58717
						public static LocString READY = "Module limit";

						// Token: 0x0400E55E RID: 58718
						public static LocString FAILURE = "Module limit";

						// Token: 0x0400E55F RID: 58719
						public static LocString WARNING = "Module limit";
					}

					// Token: 0x02003B28 RID: 15144
					public class TOOLTIP
					{
						// Token: 0x0400E560 RID: 58720
						public static LocString READY = "The rocket's engine can support the number of installed rocket modules";

						// Token: 0x0400E561 RID: 58721
						public static LocString FAILURE = "The number of installed modules exceeds the engine's module limit\n\nExcess modules must be removed";

						// Token: 0x0400E562 RID: 58722
						public static LocString WARNING = "Module limit warning";
					}
				}

				// Token: 0x020036FC RID: 14076
				public class HAS_RESOURCE
				{
					// Token: 0x02003B29 RID: 15145
					public class STATUS
					{
						// Token: 0x0400E563 RID: 58723
						public static LocString READY = "{0} {1} supplied";

						// Token: 0x0400E564 RID: 58724
						public static LocString FAILURE = "{0} missing {1}";

						// Token: 0x0400E565 RID: 58725
						public static LocString WARNING = "{0} missing {1}";
					}

					// Token: 0x02003B2A RID: 15146
					public class TOOLTIP
					{
						// Token: 0x0400E566 RID: 58726
						public static LocString READY = "{0} {1} supplied";

						// Token: 0x0400E567 RID: 58727
						public static LocString FAILURE = "{0} has less than {1} {2}";

						// Token: 0x0400E568 RID: 58728
						public static LocString WARNING = "{0} has less than {1} {2}";
					}
				}

				// Token: 0x020036FD RID: 14077
				public class MAX_HEIGHT
				{
					// Token: 0x02003B2B RID: 15147
					public class STATUS
					{
						// Token: 0x0400E569 RID: 58729
						public static LocString READY = "Height limit";

						// Token: 0x0400E56A RID: 58730
						public static LocString FAILURE = "Height limit";

						// Token: 0x0400E56B RID: 58731
						public static LocString WARNING = "Height limit";
					}

					// Token: 0x02003B2C RID: 15148
					public class TOOLTIP
					{
						// Token: 0x0400E56C RID: 58732
						public static LocString READY = "The rocket's engine can support the height of the rocket";

						// Token: 0x0400E56D RID: 58733
						public static LocString FAILURE = "The height of the rocket exceeds the engine's limit\n\nExcess modules must be removed";

						// Token: 0x0400E56E RID: 58734
						public static LocString WARNING = "Height limit warning";
					}
				}

				// Token: 0x020036FE RID: 14078
				public class PROPERLY_FUELED
				{
					// Token: 0x02003B2D RID: 15149
					public class STATUS
					{
						// Token: 0x0400E56F RID: 58735
						public static LocString READY = "Fueled";

						// Token: 0x0400E570 RID: 58736
						public static LocString FAILURE = "Fueled";

						// Token: 0x0400E571 RID: 58737
						public static LocString WARNING = "Fueled";
					}

					// Token: 0x02003B2E RID: 15150
					public class TOOLTIP
					{
						// Token: 0x0400E572 RID: 58738
						public static LocString READY = "The rocket is sufficiently fueled for a roundtrip to its destination and back";

						// Token: 0x0400E573 RID: 58739
						public static LocString READY_NO_DESTINATION = "This rocket's fuel tanks have been filled to capacity, but it has no destination";

						// Token: 0x0400E574 RID: 58740
						public static LocString FAILURE = "This rocket does not have enough fuel to reach its destination\n\nIf the tanks are full, a different Fuel Tank Module may be required";

						// Token: 0x0400E575 RID: 58741
						public static LocString WARNING = "The rocket has enough fuel for a one-way trip to its destination, but will not be able to make it back";
					}
				}

				// Token: 0x020036FF RID: 14079
				public class SUFFICIENT_OXIDIZER
				{
					// Token: 0x02003B2F RID: 15151
					public class STATUS
					{
						// Token: 0x0400E576 RID: 58742
						public static LocString READY = "Sufficient Oxidizer";

						// Token: 0x0400E577 RID: 58743
						public static LocString FAILURE = "Sufficient Oxidizer";

						// Token: 0x0400E578 RID: 58744
						public static LocString WARNING = "Warning: Limited oxidizer";
					}

					// Token: 0x02003B30 RID: 15152
					public class TOOLTIP
					{
						// Token: 0x0400E579 RID: 58745
						public static LocString READY = "This rocket has sufficient oxidizer for a roundtrip to its destination and back";

						// Token: 0x0400E57A RID: 58746
						public static LocString FAILURE = "This rocket does not have enough oxidizer to reach its destination\n\nIf the oxidizer tanks are full, a different Oxidizer Tank Module may be required";

						// Token: 0x0400E57B RID: 58747
						public static LocString WARNING = "The rocket has enough oxidizer for a one-way trip to its destination, but will not be able to make it back";
					}
				}

				// Token: 0x02003700 RID: 14080
				public class ON_LAUNCHPAD
				{
					// Token: 0x02003B31 RID: 15153
					public class STATUS
					{
						// Token: 0x0400E57C RID: 58748
						public static LocString READY = "On a launch pad";

						// Token: 0x0400E57D RID: 58749
						public static LocString FAILURE = "Not on a launch pad";

						// Token: 0x0400E57E RID: 58750
						public static LocString WARNING = "No launch pad";
					}

					// Token: 0x02003B32 RID: 15154
					public class TOOLTIP
					{
						// Token: 0x0400E57F RID: 58751
						public static LocString READY = "On a launch pad";

						// Token: 0x0400E580 RID: 58752
						public static LocString FAILURE = "Not on a launch pad";

						// Token: 0x0400E581 RID: 58753
						public static LocString WARNING = "No launch pad";
					}
				}

				// Token: 0x02003701 RID: 14081
				public class ROBOT_PILOT_DATA_REQUIREMENTS
				{
					// Token: 0x02003B33 RID: 15155
					public class STATUS
					{
						// Token: 0x0400E582 RID: 58754
						public static LocString WARNING_NO_DATA_BANKS_HUMAN_PILOT = "Robo-Pilot programmed";

						// Token: 0x0400E583 RID: 58755
						public static LocString READY = "Robo-Pilot programmed";

						// Token: 0x0400E584 RID: 58756
						public static LocString FAILURE = "Robo-Pilot programmed";

						// Token: 0x0400E585 RID: 58757
						public static LocString WARNING = "Robo-Pilot programmed";
					}

					// Token: 0x02003B34 RID: 15156
					public class TOOLTIP
					{
						// Token: 0x0400E586 RID: 58758
						public static LocString READY = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has sufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" to reach its destination"
						});

						// Token: 0x0400E587 RID: 58759
						public static LocString READY_NO_DESTINATION = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has sufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							", but no destination has been set"
						});

						// Token: 0x0400E588 RID: 58760
						public static LocString FAILURE = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" requires at least one ",
							UI.PRE_KEYWORD,
							"Data Bank",
							UI.PST_KEYWORD,
							" for launch"
						});

						// Token: 0x0400E589 RID: 58761
						public static LocString WARNING = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has insufficient ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" to reach its destination\n\nTravel speed will be reduced"
						});

						// Token: 0x0400E58A RID: 58762
						public static LocString WARNING_NO_DATA_BANKS_HUMAN_PILOT = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" requires ",
							UI.PRE_KEYWORD,
							"Data Banks",
							UI.PST_KEYWORD,
							" to function\n\nThis rocket is currently operated by a Duplicant who possesses the ",
							DUPLICANTS.ROLES.ROCKETPILOT.NAME,
							" skill"
						});
					}
				}

				// Token: 0x02003702 RID: 14082
				public class ROBOT_PILOT_POWER_SOUCRE
				{
					// Token: 0x02003B35 RID: 15157
					public class STATUS
					{
						// Token: 0x0400E58B RID: 58763
						public static LocString READY = "Robo-Pilot has power";

						// Token: 0x0400E58C RID: 58764
						public static LocString WARNING = "Robo-Pilot has power";

						// Token: 0x0400E58D RID: 58765
						public static LocString FAILURE = "Robo-Pilot has power";
					}

					// Token: 0x02003B36 RID: 15158
					public class TOOLTIP
					{
						// Token: 0x0400E58E RID: 58766
						public static LocString READY = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has a ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" source"
						});

						// Token: 0x0400E58F RID: 58767
						public static LocString WARNING = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" has insufficient  ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" for a round-trip to its destination"
						});

						// Token: 0x0400E590 RID: 58768
						public static LocString FAILURE = string.Concat(new string[]
						{
							UI.PRE_KEYWORD,
							"Robo-Pilot",
							UI.PST_KEYWORD,
							" requires a ",
							UI.PRE_KEYWORD,
							"Power",
							UI.PST_KEYWORD,
							" source for launch"
						});
					}
				}
			}

			// Token: 0x02002AB8 RID: 10936
			public class FULLTANK
			{
				// Token: 0x0400B980 RID: 47488
				public static LocString NAME = "Fuel Tank full";

				// Token: 0x0400B981 RID: 47489
				public static LocString TOOLTIP = "Tank is full, ready for launch";
			}

			// Token: 0x02002AB9 RID: 10937
			public class EMPTYTANK
			{
				// Token: 0x0400B982 RID: 47490
				public static LocString NAME = "Fuel Tank not full";

				// Token: 0x0400B983 RID: 47491
				public static LocString TOOLTIP = "Fuel tank must be filled before launch";
			}

			// Token: 0x02002ABA RID: 10938
			public class FULLOXIDIZERTANK
			{
				// Token: 0x0400B984 RID: 47492
				public static LocString NAME = "Oxidizer Tank full";

				// Token: 0x0400B985 RID: 47493
				public static LocString TOOLTIP = "Tank is full, ready for launch";
			}

			// Token: 0x02002ABB RID: 10939
			public class EMPTYOXIDIZERTANK
			{
				// Token: 0x0400B986 RID: 47494
				public static LocString NAME = "Oxidizer Tank not full";

				// Token: 0x0400B987 RID: 47495
				public static LocString TOOLTIP = "Oxidizer tank must be filled before launch";
			}

			// Token: 0x02002ABC RID: 10940
			public class ROCKETSTATUS
			{
				// Token: 0x0400B988 RID: 47496
				public static LocString STATUS_TITLE = "Rocket Status";

				// Token: 0x0400B989 RID: 47497
				public static LocString NONE = "NONE";

				// Token: 0x0400B98A RID: 47498
				public static LocString SELECTED = "SELECTED";

				// Token: 0x0400B98B RID: 47499
				public static LocString LOCKEDIN = "LOCKED IN";

				// Token: 0x0400B98C RID: 47500
				public static LocString NODESTINATION = "No destination selected";

				// Token: 0x0400B98D RID: 47501
				public static LocString DESTINATIONVALUE = "None";

				// Token: 0x0400B98E RID: 47502
				public static LocString NOPASSENGERS = "No passengers";

				// Token: 0x0400B98F RID: 47503
				public static LocString STATUS = "Status";

				// Token: 0x0400B990 RID: 47504
				public static LocString TOTAL = "Total";

				// Token: 0x0400B991 RID: 47505
				public static LocString WEIGHTPENALTY = "Weight Penalty";

				// Token: 0x0400B992 RID: 47506
				public static LocString TIMEREMAINING = "Time Remaining";

				// Token: 0x0400B993 RID: 47507
				public static LocString BOOSTED_TIME_MODIFIER = "Less Than ";
			}

			// Token: 0x02002ABD RID: 10941
			public class ROCKETSTATS
			{
				// Token: 0x0400B994 RID: 47508
				public static LocString TOTAL_OXIDIZABLE_FUEL = "Total oxidizable fuel";

				// Token: 0x0400B995 RID: 47509
				public static LocString TOTAL_OXIDIZER = "Total oxidizer";

				// Token: 0x0400B996 RID: 47510
				public static LocString TOTAL_FUEL = "Total fuel";

				// Token: 0x0400B997 RID: 47511
				public static LocString NO_ENGINE = "NO ENGINE";

				// Token: 0x0400B998 RID: 47512
				public static LocString ENGINE_EFFICIENCY = "Main engine efficiency";

				// Token: 0x0400B999 RID: 47513
				public static LocString OXIDIZER_EFFICIENCY = "Average oxidizer efficiency";

				// Token: 0x0400B99A RID: 47514
				public static LocString SOLID_BOOSTER = "Solid boosters";

				// Token: 0x0400B99B RID: 47515
				public static LocString TOTAL_THRUST = "Total thrust";

				// Token: 0x0400B99C RID: 47516
				public static LocString TOTAL_RANGE = "Total range";

				// Token: 0x0400B99D RID: 47517
				public static LocString DRY_MASS = "Dry mass";

				// Token: 0x0400B99E RID: 47518
				public static LocString WET_MASS = "Wet mass";
			}

			// Token: 0x02002ABE RID: 10942
			public class STORAGESTATS
			{
				// Token: 0x0400B99F RID: 47519
				public static LocString STORAGECAPACITY = "{0} / {1}";
			}
		}

		// Token: 0x02002166 RID: 8550
		public class RESEARCHSCREEN
		{
			// Token: 0x02002ABF RID: 10943
			public class FILTER_BUTTONS
			{
				// Token: 0x0400B9A0 RID: 47520
				public static LocString HEADER = "Preset Filters";

				// Token: 0x0400B9A1 RID: 47521
				public static LocString ALL = "All";

				// Token: 0x0400B9A2 RID: 47522
				public static LocString AVAILABLE = "Next";

				// Token: 0x0400B9A3 RID: 47523
				public static LocString COMPLETED = "Completed";

				// Token: 0x0400B9A4 RID: 47524
				public static LocString OXYGEN = "Oxygen";

				// Token: 0x0400B9A5 RID: 47525
				public static LocString FOOD = "Food";

				// Token: 0x0400B9A6 RID: 47526
				public static LocString WATER = "Water";

				// Token: 0x0400B9A7 RID: 47527
				public static LocString POWER = "Power";

				// Token: 0x0400B9A8 RID: 47528
				public static LocString MORALE = "Morale";

				// Token: 0x0400B9A9 RID: 47529
				public static LocString RANCHING = "Ranching";

				// Token: 0x0400B9AA RID: 47530
				public static LocString FILTER = "Filters";

				// Token: 0x0400B9AB RID: 47531
				public static LocString TILE = "Tiles";

				// Token: 0x0400B9AC RID: 47532
				public static LocString TRANSPORT = "Transport";

				// Token: 0x0400B9AD RID: 47533
				public static LocString AUTOMATION = "Automation";

				// Token: 0x0400B9AE RID: 47534
				public static LocString MEDICINE = "Medicine";

				// Token: 0x0400B9AF RID: 47535
				public static LocString ROCKET = "Rockets";

				// Token: 0x0400B9B0 RID: 47536
				public static LocString RADIATION = "Radiation";
			}
		}

		// Token: 0x02002167 RID: 8551
		public class CODEX
		{
			// Token: 0x040096E8 RID: 38632
			public static LocString SEARCH_HEADER = "Search Database";

			// Token: 0x040096E9 RID: 38633
			public static LocString BACK_BUTTON = "Back ({0})";

			// Token: 0x040096EA RID: 38634
			public static LocString TIPS = "Tips";

			// Token: 0x040096EB RID: 38635
			public static LocString GAME_SYSTEMS = "Systems";

			// Token: 0x040096EC RID: 38636
			public static LocString DETAILS = "Details";

			// Token: 0x040096ED RID: 38637
			public static LocString RECIPE_ITEM = "{0} x {1}{2}";

			// Token: 0x040096EE RID: 38638
			public static LocString RECIPE_FABRICATOR = "{1} ({0} seconds)";

			// Token: 0x040096EF RID: 38639
			public static LocString RECIPE_FABRICATOR_HEADER = "Produced by";

			// Token: 0x040096F0 RID: 38640
			public static LocString BACK_BUTTON_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go back:\n{0}";

			// Token: 0x040096F1 RID: 38641
			public static LocString BACK_BUTTON_NO_HISTORY_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go back:\nN/A";

			// Token: 0x040096F2 RID: 38642
			public static LocString FORWARD_BUTTON_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go forward:\n{0}";

			// Token: 0x040096F3 RID: 38643
			public static LocString FORWARD_BUTTON_NO_HISTORY_TOOLTIP = UI.CLICK(UI.ClickType.Click) + " to go forward:\nN/A";

			// Token: 0x040096F4 RID: 38644
			public static LocString TITLE = "DATABASE";

			// Token: 0x040096F5 RID: 38645
			public static LocString MANAGEMENT_BUTTON = "DATABASE";

			// Token: 0x02002AC0 RID: 10944
			public class CODEX_DISCOVERED_MESSAGE
			{
				// Token: 0x0400B9B1 RID: 47537
				public static LocString TITLE = "New Log Entry";

				// Token: 0x0400B9B2 RID: 47538
				public static LocString BODY = "I've added a new entry to my log: {codex}\n";
			}

			// Token: 0x02002AC1 RID: 10945
			public class SUBWORLDS
			{
				// Token: 0x0400B9B3 RID: 47539
				public static LocString ELEMENTS = "Elements";

				// Token: 0x0400B9B4 RID: 47540
				public static LocString PLANTS = "Plants";

				// Token: 0x0400B9B5 RID: 47541
				public static LocString CRITTERS = "Critters";

				// Token: 0x0400B9B6 RID: 47542
				public static LocString NONE = "None";
			}

			// Token: 0x02002AC2 RID: 10946
			public class GEYSERS
			{
				// Token: 0x0400B9B7 RID: 47543
				public static LocString DESC = "Geysers and Fumaroles emit elements at variable intervals. They provide a sustainable source of material, albeit in typically low volumes.\n\nThe variable factors of a geyser are:\n\n    • Emission element \n    • Emission temperature \n    • Emission mass \n    • Cycle length \n    • Dormancy duration \n    • Disease emitted";
			}

			// Token: 0x02002AC3 RID: 10947
			public class EQUIPMENT
			{
				// Token: 0x0400B9B8 RID: 47544
				public static LocString DESC = "Equipment description";
			}

			// Token: 0x02002AC4 RID: 10948
			public class FOOD
			{
				// Token: 0x0400B9B9 RID: 47545
				public static LocString QUALITY = "Quality: {0}";

				// Token: 0x0400B9BA RID: 47546
				public static LocString CALORIES = "Calories: {0}";

				// Token: 0x0400B9BB RID: 47547
				public static LocString SPOILPROPERTIES = "Refrigeration temperature: {0}\nDeep Freeze temperature: {1}\nSpoil time: {2}";

				// Token: 0x0400B9BC RID: 47548
				public static LocString NON_PERISHABLE = "Spoil time: Never";
			}

			// Token: 0x02002AC5 RID: 10949
			public class CATEGORYNAMES
			{
				// Token: 0x0400B9BD RID: 47549
				public static LocString ROOT = UI.FormatAsLink("Index", "HOME");

				// Token: 0x0400B9BE RID: 47550
				public static LocString PLANTS = UI.FormatAsLink("Plants", "PLANTS");

				// Token: 0x0400B9BF RID: 47551
				public static LocString CREATURES = UI.FormatAsLink("Critters", "CREATURES");

				// Token: 0x0400B9C0 RID: 47552
				public static LocString EMAILS = UI.FormatAsLink("E-mail", "EMAILS");

				// Token: 0x0400B9C1 RID: 47553
				public static LocString JOURNALS = UI.FormatAsLink("Journals", "JOURNALS");

				// Token: 0x0400B9C2 RID: 47554
				public static LocString MYLOG = UI.FormatAsLink("My Log", "MYLOG");

				// Token: 0x0400B9C3 RID: 47555
				public static LocString INVESTIGATIONS = UI.FormatAsLink("Investigations", "Investigations");

				// Token: 0x0400B9C4 RID: 47556
				public static LocString RESEARCHNOTES = UI.FormatAsLink("Research Notes", "RESEARCHNOTES");

				// Token: 0x0400B9C5 RID: 47557
				public static LocString NOTICES = UI.FormatAsLink("Notices", "NOTICES");

				// Token: 0x0400B9C6 RID: 47558
				public static LocString FOOD = UI.FormatAsLink("Food", "FOOD");

				// Token: 0x0400B9C7 RID: 47559
				public static LocString MINION_MODIFIERS = UI.FormatAsLink("Duplicant Effects (EDITOR ONLY)", "MINION_MODIFIERS");

				// Token: 0x0400B9C8 RID: 47560
				public static LocString BUILDINGS = UI.FormatAsLink("Buildings", "BUILDINGS");

				// Token: 0x0400B9C9 RID: 47561
				public static LocString ROOMS = UI.FormatAsLink("Rooms", "ROOMS");

				// Token: 0x0400B9CA RID: 47562
				public static LocString TECH = UI.FormatAsLink("Research", "TECH");

				// Token: 0x0400B9CB RID: 47563
				public static LocString TIPS = UI.FormatAsLink("Tutorials", "LESSONS");

				// Token: 0x0400B9CC RID: 47564
				public static LocString EQUIPMENT = UI.FormatAsLink("Equipment", "EQUIPMENT");

				// Token: 0x0400B9CD RID: 47565
				public static LocString BIOMES = UI.FormatAsLink("Biomes", "BIOMES");

				// Token: 0x0400B9CE RID: 47566
				public static LocString STORYTRAITS = UI.FormatAsLink("Story Traits", "STORYTRAITS");

				// Token: 0x0400B9CF RID: 47567
				public static LocString VIDEOS = UI.FormatAsLink("Videos", "VIDEOS");

				// Token: 0x0400B9D0 RID: 47568
				public static LocString MISCELLANEOUSTIPS = UI.FormatAsLink("Tips", "MISCELLANEOUSTIPS");

				// Token: 0x0400B9D1 RID: 47569
				public static LocString MISCELLANEOUSITEMS = UI.FormatAsLink("Items", "MISCELLANEOUSITEMS");

				// Token: 0x0400B9D2 RID: 47570
				public static LocString ELEMENTS = UI.FormatAsLink("Elements", "ELEMENTS");

				// Token: 0x0400B9D3 RID: 47571
				public static LocString ELEMENTSSOLID = UI.FormatAsLink("Solids", "ELEMENTS_SOLID");

				// Token: 0x0400B9D4 RID: 47572
				public static LocString ELEMENTSGAS = UI.FormatAsLink("Gases", "ELEMENTS_GAS");

				// Token: 0x0400B9D5 RID: 47573
				public static LocString ELEMENTSLIQUID = UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID");

				// Token: 0x0400B9D6 RID: 47574
				public static LocString ELEMENTSOTHER = UI.FormatAsLink("Other", "ELEMENTS_OTHER");

				// Token: 0x0400B9D7 RID: 47575
				public static LocString BUILDINGMATERIALCLASSES = UI.FormatAsLink("Building Materials", "BUILDING_MATERIAL_CLASSES");

				// Token: 0x0400B9D8 RID: 47576
				public static LocString INDUSTRIALINGREDIENTS = UI.FormatAsLink("Industrial Ingredients", "INDUSTRIALINGREDIENTS");

				// Token: 0x0400B9D9 RID: 47577
				public static LocString GEYSERS = UI.FormatAsLink("Geysers", "GEYSERS");

				// Token: 0x0400B9DA RID: 47578
				public static LocString SYSTEMS = UI.FormatAsLink("Systems", "SYSTEMS");

				// Token: 0x0400B9DB RID: 47579
				public static LocString ROLES = UI.FormatAsLink("Duplicant Skills", "ROLES");

				// Token: 0x0400B9DC RID: 47580
				public static LocString DISEASE = UI.FormatAsLink("Disease", "DISEASE");

				// Token: 0x0400B9DD RID: 47581
				public static LocString SICKNESS = UI.FormatAsLink("Sickness", "SICKNESS");

				// Token: 0x0400B9DE RID: 47582
				public static LocString MEDIA = UI.FormatAsLink("Media", "MEDIA");
			}
		}

		// Token: 0x02002168 RID: 8552
		public class DEVELOPMENTBUILDS
		{
			// Token: 0x040096F6 RID: 38646
			public static LocString WATERMARK = "BUILD: {0}";

			// Token: 0x040096F7 RID: 38647
			public static LocString TESTING_WATERMARK = "TESTING BUILD: {0}";

			// Token: 0x040096F8 RID: 38648
			public static LocString TESTING_TOOLTIP = "This game is currently running a Test version.\n\n" + UI.CLICK(UI.ClickType.Click) + " for more info.";

			// Token: 0x040096F9 RID: 38649
			public static LocString TESTING_MESSAGE_TITLE = "TESTING BUILD";

			// Token: 0x040096FA RID: 38650
			public static LocString TESTING_MESSAGE = "This game is running a Test version of Oxygen Not Included. This means that some features may be in development or buggier than normal, and require more testing before they can be moved into the Release build.\n\nIf you encounter any bugs or strange behavior, please add a report to the bug forums. We appreciate it!";

			// Token: 0x040096FB RID: 38651
			public static LocString TESTING_MORE_INFO = "BUG FORUMS";

			// Token: 0x040096FC RID: 38652
			public static LocString FULL_PATCH_NOTES = "Full Patch Notes";

			// Token: 0x040096FD RID: 38653
			public static LocString PREVIOUS_VERSION = "Previous Version";

			// Token: 0x02002AC6 RID: 10950
			public class ALPHA
			{
				// Token: 0x02003703 RID: 14083
				public class MESSAGES
				{
					// Token: 0x0400DB2B RID: 56107
					public static LocString FORUMBUTTON = "FORUMS";

					// Token: 0x0400DB2C RID: 56108
					public static LocString MAILINGLIST = "MAILING LIST";

					// Token: 0x0400DB2D RID: 56109
					public static LocString PATCHNOTES = "PATCH NOTES";

					// Token: 0x0400DB2E RID: 56110
					public static LocString FEEDBACK = "FEEDBACK";
				}

				// Token: 0x02003704 RID: 14084
				public class LOADING
				{
					// Token: 0x0400DB2F RID: 56111
					public static LocString TITLE = "<b>Welcome to Oxygen Not Included!</b>";

					// Token: 0x0400DB30 RID: 56112
					public static LocString BODY = "This game is in the early stages of development which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\nDuring this time Oxygen Not Included will be receiving regular updates to fix bugs, add features, and introduce additional content, so if you encounter issues or just have suggestions to share, please let us know on our forums: <u>http://forums.kleientertainment.com</u>\n\nA special thanks to those who joined us during our time in Alpha. We value your feedback and thank you for joining us in the development process. We couldn't do this without you.\n\nEnjoy your time in deep space!\n\n- Klei";

					// Token: 0x0400DB31 RID: 56113
					public static LocString BODY_NOLINKS = "This DLC is currently in active development, which means you're likely to encounter strange, amusing, and occasionally just downright frustrating bugs.\n\n During this time Spaced Out! will be receiving regular updates to fix bugs, add features, and introduce additional content.\n\n We've got lots of content old and new to add to this DLC before it's ready, and we're happy to have you along with us. Enjoy your time in deep space!\n\n - The Team at Klei";

					// Token: 0x0400DB32 RID: 56114
					public static LocString FORUMBUTTON = "Visit Forums";
				}

				// Token: 0x02003705 RID: 14085
				public class HEALTHY_MESSAGE
				{
					// Token: 0x0400DB33 RID: 56115
					public static LocString CONTINUEBUTTON = "Thanks!";
				}
			}

			// Token: 0x02002AC7 RID: 10951
			public class PREVIOUS_UPDATE
			{
				// Token: 0x0400B9DF RID: 47583
				public static LocString TITLE = "<b>Welcome to Oxygen Not Included</b>";

				// Token: 0x0400B9E0 RID: 47584
				public static LocString BODY = "Whoops!\n\nYou're about to opt in to the <b>Previous Update branch</b>. That means opting out of all new features, fixes and content from the live branch.\n\nThis branch is temporary. It will be replaced when the next update is released. It's also completely unsupported: please don't report bugs or issues you find here.\n\nAre you sure you want to opt in?";

				// Token: 0x0400B9E1 RID: 47585
				public static LocString CONTINUEBUTTON = "Play Old Version";

				// Token: 0x0400B9E2 RID: 47586
				public static LocString FORUMBUTTON = "More Information";

				// Token: 0x0400B9E3 RID: 47587
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002AC8 RID: 10952
			public class DLC_BETA
			{
				// Token: 0x0400B9E4 RID: 47588
				public static LocString TITLE = "<b>Welcome to Oxygen Not Included</b>";

				// Token: 0x0400B9E5 RID: 47589
				public static LocString BODY = "You're about to opt in to the beta for <b>The Frosty Planet Pack</b> DLC.\nThis free beta is a work in progress, and will be discontinued before the paid DLC is released. \n\nAre you sure you want to opt in?";

				// Token: 0x0400B9E6 RID: 47590
				public static LocString CONTINUEBUTTON = "Play Beta";

				// Token: 0x0400B9E7 RID: 47591
				public static LocString FORUMBUTTON = "More Information";

				// Token: 0x0400B9E8 RID: 47592
				public static LocString QUITBUTTON = "Quit";
			}

			// Token: 0x02002AC9 RID: 10953
			public class UPDATES
			{
				// Token: 0x0400B9E9 RID: 47593
				public static LocString UPDATES_HEADER = "NEXT UPGRADE LIVE IN";

				// Token: 0x0400B9EA RID: 47594
				public static LocString NOW = "Less than a day";

				// Token: 0x0400B9EB RID: 47595
				public static LocString TWENTY_FOUR_HOURS = "Less than a day";

				// Token: 0x0400B9EC RID: 47596
				public static LocString FINAL_WEEK = "{0} days";

				// Token: 0x0400B9ED RID: 47597
				public static LocString BIGGER_TIMES = "{1} weeks {0} days";
			}
		}

		// Token: 0x02002169 RID: 8553
		public class UNITSUFFIXES
		{
			// Token: 0x040096FE RID: 38654
			public static LocString SECOND = " s";

			// Token: 0x040096FF RID: 38655
			public static LocString PERSECOND = "/s";

			// Token: 0x04009700 RID: 38656
			public static LocString PERCYCLE = "/cycle";

			// Token: 0x04009701 RID: 38657
			public static LocString UNIT = " unit";

			// Token: 0x04009702 RID: 38658
			public static LocString UNITS = " units";

			// Token: 0x04009703 RID: 38659
			public static LocString PERCENT = "%";

			// Token: 0x04009704 RID: 38660
			public static LocString DEGREES = " degrees";

			// Token: 0x04009705 RID: 38661
			public static LocString CRITTERS = " critters";

			// Token: 0x04009706 RID: 38662
			public static LocString GROWTH = "growth";

			// Token: 0x04009707 RID: 38663
			public static LocString SECONDS = "Seconds";

			// Token: 0x04009708 RID: 38664
			public static LocString DUPLICANTS = "Duplicants";

			// Token: 0x04009709 RID: 38665
			public static LocString GERMS = "Germs";

			// Token: 0x0400970A RID: 38666
			public static LocString ROCKET_MISSIONS = "Missions";

			// Token: 0x0400970B RID: 38667
			public static LocString TILES = "Tiles";

			// Token: 0x02002ACA RID: 10954
			public class MASS
			{
				// Token: 0x0400B9EE RID: 47598
				public static LocString TONNE = " t";

				// Token: 0x0400B9EF RID: 47599
				public static LocString KILOGRAM = " kg";

				// Token: 0x0400B9F0 RID: 47600
				public static LocString GRAM = " g";

				// Token: 0x0400B9F1 RID: 47601
				public static LocString MILLIGRAM = " mg";

				// Token: 0x0400B9F2 RID: 47602
				public static LocString MICROGRAM = " mcg";

				// Token: 0x0400B9F3 RID: 47603
				public static LocString POUND = " lb";

				// Token: 0x0400B9F4 RID: 47604
				public static LocString DRACHMA = " dr";

				// Token: 0x0400B9F5 RID: 47605
				public static LocString GRAIN = " gr";
			}

			// Token: 0x02002ACB RID: 10955
			public class TEMPERATURE
			{
				// Token: 0x0400B9F6 RID: 47606
				public static LocString CELSIUS = " " + 'º'.ToString() + "C";

				// Token: 0x0400B9F7 RID: 47607
				public static LocString FAHRENHEIT = " " + 'º'.ToString() + "F";

				// Token: 0x0400B9F8 RID: 47608
				public static LocString KELVIN = " K";
			}

			// Token: 0x02002ACC RID: 10956
			public class CALORIES
			{
				// Token: 0x0400B9F9 RID: 47609
				public static LocString CALORIE = " cal";

				// Token: 0x0400B9FA RID: 47610
				public static LocString KILOCALORIE = " kcal";
			}

			// Token: 0x02002ACD RID: 10957
			public class ELECTRICAL
			{
				// Token: 0x0400B9FB RID: 47611
				public static LocString JOULE = " J";

				// Token: 0x0400B9FC RID: 47612
				public static LocString KILOJOULE = " kJ";

				// Token: 0x0400B9FD RID: 47613
				public static LocString MEGAJOULE = " MJ";

				// Token: 0x0400B9FE RID: 47614
				public static LocString WATT = " W";

				// Token: 0x0400B9FF RID: 47615
				public static LocString KILOWATT = " kW";
			}

			// Token: 0x02002ACE RID: 10958
			public class HEAT
			{
				// Token: 0x0400BA00 RID: 47616
				public static LocString DTU = " DTU";

				// Token: 0x0400BA01 RID: 47617
				public static LocString KDTU = " kDTU";

				// Token: 0x0400BA02 RID: 47618
				public static LocString DTU_S = " DTU/s";

				// Token: 0x0400BA03 RID: 47619
				public static LocString KDTU_S = " kDTU/s";
			}

			// Token: 0x02002ACF RID: 10959
			public class DISTANCE
			{
				// Token: 0x0400BA04 RID: 47620
				public static LocString METER = " m";

				// Token: 0x0400BA05 RID: 47621
				public static LocString KILOMETER = " km";
			}

			// Token: 0x02002AD0 RID: 10960
			public class DISEASE
			{
				// Token: 0x0400BA06 RID: 47622
				public static LocString UNITS = " germs";
			}

			// Token: 0x02002AD1 RID: 10961
			public class NOISE
			{
				// Token: 0x0400BA07 RID: 47623
				public static LocString UNITS = " dB";
			}

			// Token: 0x02002AD2 RID: 10962
			public class INFORMATION
			{
				// Token: 0x0400BA08 RID: 47624
				public static LocString BYTE = "B";

				// Token: 0x0400BA09 RID: 47625
				public static LocString KILOBYTE = "kB";

				// Token: 0x0400BA0A RID: 47626
				public static LocString MEGABYTE = "MB";

				// Token: 0x0400BA0B RID: 47627
				public static LocString GIGABYTE = "GB";

				// Token: 0x0400BA0C RID: 47628
				public static LocString TERABYTE = "TB";
			}

			// Token: 0x02002AD3 RID: 10963
			public class LIGHT
			{
				// Token: 0x0400BA0D RID: 47629
				public static LocString LUX = " lux";
			}

			// Token: 0x02002AD4 RID: 10964
			public class RADIATION
			{
				// Token: 0x0400BA0E RID: 47630
				public static LocString RADS = " rads";
			}

			// Token: 0x02002AD5 RID: 10965
			public class HIGHENERGYPARTICLES
			{
				// Token: 0x0400BA0F RID: 47631
				public static LocString PARTRICLE = " Radbolt";

				// Token: 0x0400BA10 RID: 47632
				public static LocString PARTRICLES = " Radbolts";
			}
		}

		// Token: 0x0200216A RID: 8554
		public class OVERLAYS
		{
			// Token: 0x02002AD6 RID: 10966
			public class TILEMODE
			{
				// Token: 0x0400BA11 RID: 47633
				public static LocString NAME = "MATERIALS OVERLAY";

				// Token: 0x0400BA12 RID: 47634
				public static LocString BUTTON = "Materials Overlay";
			}

			// Token: 0x02002AD7 RID: 10967
			public class OXYGEN
			{
				// Token: 0x0400BA13 RID: 47635
				public static LocString NAME = "OXYGEN OVERLAY";

				// Token: 0x0400BA14 RID: 47636
				public static LocString BUTTON = "Oxygen Overlay";

				// Token: 0x0400BA15 RID: 47637
				public static LocString LEGEND1 = "Very Breathable";

				// Token: 0x0400BA16 RID: 47638
				public static LocString LEGEND2 = "Breathable";

				// Token: 0x0400BA17 RID: 47639
				public static LocString LEGEND3 = "Barely Breathable";

				// Token: 0x0400BA18 RID: 47640
				public static LocString LEGEND4 = "Unbreathable";

				// Token: 0x0400BA19 RID: 47641
				public static LocString LEGEND5 = "Barely Breathable";

				// Token: 0x0400BA1A RID: 47642
				public static LocString LEGEND6 = "Unbreathable";

				// Token: 0x02003706 RID: 14086
				public class TOOLTIPS
				{
					// Token: 0x0400DB34 RID: 56116
					public static LocString LEGEND1 = string.Concat(new string[]
					{
						"<b>Very Breathable</b>\nHigh ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400DB35 RID: 56117
					public static LocString LEGEND2 = string.Concat(new string[]
					{
						"<b>Breathable</b>\nSufficient ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400DB36 RID: 56118
					public static LocString LEGEND3 = string.Concat(new string[]
					{
						"<b>Barely Breathable</b>\nLow ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations"
					});

					// Token: 0x0400DB37 RID: 56119
					public static LocString LEGEND4 = string.Concat(new string[]
					{
						"<b>Unbreathable</b>\nExtremely low or absent ",
						UI.PRE_KEYWORD,
						"Oxygen",
						UI.PST_KEYWORD,
						" concentrations\n\nDuplicants will suffocate if trapped in these areas"
					});

					// Token: 0x0400DB38 RID: 56120
					public static LocString LEGEND5 = "<b>Slightly Toxic</b>\nHarmful gas concentration";

					// Token: 0x0400DB39 RID: 56121
					public static LocString LEGEND6 = "<b>Very Toxic</b>\nLethal gas concentration";
				}
			}

			// Token: 0x02002AD8 RID: 10968
			public class ELECTRICAL
			{
				// Token: 0x0400BA1B RID: 47643
				public static LocString NAME = "POWER OVERLAY";

				// Token: 0x0400BA1C RID: 47644
				public static LocString BUTTON = "Power Overlay";

				// Token: 0x0400BA1D RID: 47645
				public static LocString LEGEND1 = "<b>BUILDING POWER</b>";

				// Token: 0x0400BA1E RID: 47646
				public static LocString LEGEND2 = "Consumer";

				// Token: 0x0400BA1F RID: 47647
				public static LocString LEGEND3 = "Producer";

				// Token: 0x0400BA20 RID: 47648
				public static LocString LEGEND4 = "<b>CIRCUIT POWER HEALTH</b>";

				// Token: 0x0400BA21 RID: 47649
				public static LocString LEGEND5 = "Inactive";

				// Token: 0x0400BA22 RID: 47650
				public static LocString LEGEND6 = "Safe";

				// Token: 0x0400BA23 RID: 47651
				public static LocString LEGEND7 = "Strained";

				// Token: 0x0400BA24 RID: 47652
				public static LocString LEGEND8 = "Overloaded";

				// Token: 0x0400BA25 RID: 47653
				public static LocString DIAGRAM_HEADER = "Energy from the <b>Left Outlet</b> is used by the <b>Right Outlet</b>";

				// Token: 0x0400BA26 RID: 47654
				public static LocString LEGEND_SWITCH = "Switch";

				// Token: 0x02003707 RID: 14087
				public class TOOLTIPS
				{
					// Token: 0x0400DB3A RID: 56122
					public static LocString LEGEND1 = "Displays whether buildings use or generate " + UI.FormatAsLink("Power", "POWER");

					// Token: 0x0400DB3B RID: 56123
					public static LocString LEGEND2 = "<b>Consumer</b>\nThese buildings draw power from a circuit";

					// Token: 0x0400DB3C RID: 56124
					public static LocString LEGEND3 = "<b>Producer</b>\nThese buildings generate power for a circuit";

					// Token: 0x0400DB3D RID: 56125
					public static LocString LEGEND4 = "Displays the health of wire systems";

					// Token: 0x0400DB3E RID: 56126
					public static LocString LEGEND5 = "<b>Inactive</b>\nThere is no power activity on these circuits";

					// Token: 0x0400DB3F RID: 56127
					public static LocString LEGEND6 = "<b>Safe</b>\nThese circuits are not in danger of overloading";

					// Token: 0x0400DB40 RID: 56128
					public static LocString LEGEND7 = "<b>Strained</b>\nThese circuits are close to consuming more power than their wires support";

					// Token: 0x0400DB41 RID: 56129
					public static LocString LEGEND8 = "<b>Overloaded</b>\nThese circuits are consuming more power than their wires support";

					// Token: 0x0400DB42 RID: 56130
					public static LocString LEGEND_SWITCH = "<b>Switch</b>\nActivates or deactivates connected circuits";
				}
			}

			// Token: 0x02002AD9 RID: 10969
			public class TEMPERATURE
			{
				// Token: 0x0400BA27 RID: 47655
				public static LocString NAME = "TEMPERATURE OVERLAY";

				// Token: 0x0400BA28 RID: 47656
				public static LocString BUTTON = "Temperature Overlay";

				// Token: 0x0400BA29 RID: 47657
				public static LocString EXTREMECOLD = "Absolute Zero";

				// Token: 0x0400BA2A RID: 47658
				public static LocString VERYCOLD = "Cold";

				// Token: 0x0400BA2B RID: 47659
				public static LocString COLD = "Chilled";

				// Token: 0x0400BA2C RID: 47660
				public static LocString TEMPERATE = "Temperate";

				// Token: 0x0400BA2D RID: 47661
				public static LocString HOT = "Warm";

				// Token: 0x0400BA2E RID: 47662
				public static LocString VERYHOT = "Hot";

				// Token: 0x0400BA2F RID: 47663
				public static LocString EXTREMEHOT = "Scorching";

				// Token: 0x0400BA30 RID: 47664
				public static LocString MAXHOT = "Molten";

				// Token: 0x0400BA31 RID: 47665
				public static LocString HEATSOURCES = "Heat Source";

				// Token: 0x0400BA32 RID: 47666
				public static LocString HEATSINK = "Heat Sink";

				// Token: 0x0400BA33 RID: 47667
				public static LocString DEFAULT_TEMPERATURE_BUTTON = "Default";

				// Token: 0x02003708 RID: 14088
				public class TOOLTIPS
				{
					// Token: 0x0400DB43 RID: 56131
					public static LocString TEMPERATURE = "Temperatures reaching {0}";

					// Token: 0x0400DB44 RID: 56132
					public static LocString HEATSOURCES = "Elements displaying this symbol can produce heat";

					// Token: 0x0400DB45 RID: 56133
					public static LocString HEATSINK = "Elements displaying this symbol can absorb heat";
				}
			}

			// Token: 0x02002ADA RID: 10970
			public class STATECHANGE
			{
				// Token: 0x0400BA34 RID: 47668
				public static LocString LOWPOINT = "Low energy state change";

				// Token: 0x0400BA35 RID: 47669
				public static LocString STABLE = "Stable";

				// Token: 0x0400BA36 RID: 47670
				public static LocString HIGHPOINT = "High energy state change";

				// Token: 0x02003709 RID: 14089
				public class TOOLTIPS
				{
					// Token: 0x0400DB46 RID: 56134
					public static LocString LOWPOINT = "Nearing a low energy state change";

					// Token: 0x0400DB47 RID: 56135
					public static LocString STABLE = "Not near any state changes";

					// Token: 0x0400DB48 RID: 56136
					public static LocString HIGHPOINT = "Nearing high energy state change";
				}
			}

			// Token: 0x02002ADB RID: 10971
			public class HEATFLOW
			{
				// Token: 0x0400BA37 RID: 47671
				public static LocString NAME = "THERMAL TOLERANCE OVERLAY";

				// Token: 0x0400BA38 RID: 47672
				public static LocString HOVERTITLE = "THERMAL TOLERANCE";

				// Token: 0x0400BA39 RID: 47673
				public static LocString BUTTON = "Thermal Tolerance Overlay";

				// Token: 0x0400BA3A RID: 47674
				public static LocString COOLING = "Body Heat Loss";

				// Token: 0x0400BA3B RID: 47675
				public static LocString NEUTRAL = "Comfort Zone";

				// Token: 0x0400BA3C RID: 47676
				public static LocString HEATING = "Body Heat Retention";

				// Token: 0x0400BA3D RID: 47677
				public static LocString COOLING_DUPE = "Body Heat Loss {0}\n\nUncomfortably chilly surroundings";

				// Token: 0x0400BA3E RID: 47678
				public static LocString NEUTRAL_DUPE = "Comfort Zone {0}";

				// Token: 0x0400BA3F RID: 47679
				public static LocString HEATING_DUPE = "Body Heat Loss {0}\n\nUncomfortably toasty surroundings";

				// Token: 0x0200370A RID: 14090
				public class TOOLTIPS
				{
					// Token: 0x0400DB49 RID: 56137
					public static LocString COOLING = "<b>Body Heat Loss</b>\nUncomfortably cold\n\nDuplicants lose more heat in chilly surroundings than they can absorb\n    • Warm Coats help Duplicants retain body heat";

					// Token: 0x0400DB4A RID: 56138
					public static LocString NEUTRAL = "<b>Comfort Zone</b>\nComfortable area\n\nDuplicants can regulate their internal temperatures in these areas";

					// Token: 0x0400DB4B RID: 56139
					public static LocString HEATING = "<b>Body Heat Retention</b>\nUncomfortably warm\n\nDuplicants absorb more heat in toasty surroundings than they can release";
				}
			}

			// Token: 0x02002ADC RID: 10972
			public class RELATIVETEMPERATURE
			{
				// Token: 0x0400BA40 RID: 47680
				public static LocString NAME = "RELATIVE TEMPERATURE";

				// Token: 0x0400BA41 RID: 47681
				public static LocString HOVERTITLE = "RELATIVE TEMPERATURE";

				// Token: 0x0400BA42 RID: 47682
				public static LocString BUTTON = "Relative Temperature Overlay";
			}

			// Token: 0x02002ADD RID: 10973
			public class ROOMS
			{
				// Token: 0x0400BA43 RID: 47683
				public static LocString NAME = "ROOM OVERLAY";

				// Token: 0x0400BA44 RID: 47684
				public static LocString BUTTON = "Room Overlay";

				// Token: 0x0400BA45 RID: 47685
				public static LocString ROOM = "Room {0}";

				// Token: 0x0400BA46 RID: 47686
				public static LocString HOVERTITLE = "ROOMS";

				// Token: 0x0200370B RID: 14091
				public static class NOROOM
				{
					// Token: 0x0400DB4C RID: 56140
					public static LocString HEADER = "No Room";

					// Token: 0x0400DB4D RID: 56141
					public static LocString DESC = "Enclose this space with walls and doors to make a room";

					// Token: 0x0400DB4E RID: 56142
					public static LocString TOO_BIG = "<color=#F44A47FF>    • Size: {0} Tiles\n    • Maximum room size: {1} Tiles</color>";
				}

				// Token: 0x0200370C RID: 14092
				public class TOOLTIPS
				{
					// Token: 0x0400DB4F RID: 56143
					public static LocString ROOM = "Completed Duplicant bedrooms";

					// Token: 0x0400DB50 RID: 56144
					public static LocString NOROOMS = "Duplicants have nowhere to sleep";
				}
			}

			// Token: 0x02002ADE RID: 10974
			public class JOULES
			{
				// Token: 0x0400BA47 RID: 47687
				public static LocString NAME = "JOULES";

				// Token: 0x0400BA48 RID: 47688
				public static LocString HOVERTITLE = "JOULES";

				// Token: 0x0400BA49 RID: 47689
				public static LocString BUTTON = "Joules Overlay";
			}

			// Token: 0x02002ADF RID: 10975
			public class LIGHTING
			{
				// Token: 0x0400BA4A RID: 47690
				public static LocString NAME = "LIGHT OVERLAY";

				// Token: 0x0400BA4B RID: 47691
				public static LocString BUTTON = "Light Overlay";

				// Token: 0x0400BA4C RID: 47692
				public static LocString LITAREA = "Lit Area";

				// Token: 0x0400BA4D RID: 47693
				public static LocString DARK = "Unlit Area";

				// Token: 0x0400BA4E RID: 47694
				public static LocString HOVERTITLE = "LIGHT";

				// Token: 0x0400BA4F RID: 47695
				public static LocString DESC = "{0} Lux";

				// Token: 0x0200370D RID: 14093
				public class RANGES
				{
					// Token: 0x0400DB51 RID: 56145
					public static LocString NO_LIGHT = "Pitch Black";

					// Token: 0x0400DB52 RID: 56146
					public static LocString VERY_LOW_LIGHT = "Very Dim";

					// Token: 0x0400DB53 RID: 56147
					public static LocString LOW_LIGHT = "Dim";

					// Token: 0x0400DB54 RID: 56148
					public static LocString MEDIUM_LIGHT = "Well Lit";

					// Token: 0x0400DB55 RID: 56149
					public static LocString HIGH_LIGHT = "Bright";

					// Token: 0x0400DB56 RID: 56150
					public static LocString VERY_HIGH_LIGHT = "Brilliant";

					// Token: 0x0400DB57 RID: 56151
					public static LocString MAX_LIGHT = "Blinding";
				}

				// Token: 0x0200370E RID: 14094
				public class TOOLTIPS
				{
					// Token: 0x0400DB58 RID: 56152
					public static LocString NAME = "LIGHT OVERLAY";

					// Token: 0x0400DB59 RID: 56153
					public static LocString LITAREA = "<b>Lit Area</b>\nWorking in well-lit areas improves Duplicant " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;

					// Token: 0x0400DB5A RID: 56154
					public static LocString DARK = "<b>Unlit Area</b>\nWorking in the dark has no effect on Duplicants";
				}
			}

			// Token: 0x02002AE0 RID: 10976
			public class CROP
			{
				// Token: 0x0400BA50 RID: 47696
				public static LocString NAME = "FARMING OVERLAY";

				// Token: 0x0400BA51 RID: 47697
				public static LocString BUTTON = "Farming Overlay";

				// Token: 0x0400BA52 RID: 47698
				public static LocString GROWTH_HALTED = "Halted Growth";

				// Token: 0x0400BA53 RID: 47699
				public static LocString GROWING = "Growing";

				// Token: 0x0400BA54 RID: 47700
				public static LocString FULLY_GROWN = "Fully Grown";

				// Token: 0x0200370F RID: 14095
				public class TOOLTIPS
				{
					// Token: 0x0400DB5B RID: 56155
					public static LocString GROWTH_HALTED = "<b>Halted Growth</b>\nSubstandard conditions prevent these plants from growing";

					// Token: 0x0400DB5C RID: 56156
					public static LocString GROWING = "<b>Growing</b>\nThese plants are thriving in their current conditions";

					// Token: 0x0400DB5D RID: 56157
					public static LocString FULLY_GROWN = "<b>Fully Grown</b>\nThese plants have reached maturation\n\nSelect the " + UI.FormatAsTool("Harvest Tool", global::Action.Harvest) + " to batch harvest";
				}
			}

			// Token: 0x02002AE1 RID: 10977
			public class LIQUIDPLUMBING
			{
				// Token: 0x0400BA55 RID: 47701
				public static LocString NAME = "PLUMBING OVERLAY";

				// Token: 0x0400BA56 RID: 47702
				public static LocString BUTTON = "Plumbing Overlay";

				// Token: 0x0400BA57 RID: 47703
				public static LocString CONSUMER = "Output Pipe";

				// Token: 0x0400BA58 RID: 47704
				public static LocString FILTERED = "Filtered Output Pipe";

				// Token: 0x0400BA59 RID: 47705
				public static LocString PRODUCER = "Building Intake";

				// Token: 0x0400BA5A RID: 47706
				public static LocString CONNECTED = "Connected";

				// Token: 0x0400BA5B RID: 47707
				public static LocString DISCONNECTED = "Disconnected";

				// Token: 0x0400BA5C RID: 47708
				public static LocString NETWORK = "Liquid Network {0}";

				// Token: 0x0400BA5D RID: 47709
				public static LocString DIAGRAM_BEFORE_ARROW = "Liquid flows from <b>Output Pipe</b>";

				// Token: 0x0400BA5E RID: 47710
				public static LocString DIAGRAM_AFTER_ARROW = "<b>Building Intake</b>";

				// Token: 0x02003710 RID: 14096
				public class TOOLTIPS
				{
					// Token: 0x0400DB5E RID: 56158
					public static LocString CONNECTED = "Connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

					// Token: 0x0400DB5F RID: 56159
					public static LocString DISCONNECTED = "Not connected to a " + UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

					// Token: 0x0400DB60 RID: 56160
					public static LocString CONSUMER = "<b>Output Pipe</b>\nOutputs send liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING");

					// Token: 0x0400DB61 RID: 56161
					public static LocString FILTERED = "<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered liquid into pipes\n\nMust be on the same network as at least one " + UI.FormatAsLink("Intake", "LIQUIDPIPING");

					// Token: 0x0400DB62 RID: 56162
					public static LocString PRODUCER = "<b>Building Intake</b>\nIntakes send liquid into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "LIQUIDPIPING");

					// Token: 0x0400DB63 RID: 56163
					public static LocString NETWORK = "Liquid network {0}";
				}
			}

			// Token: 0x02002AE2 RID: 10978
			public class GASPLUMBING
			{
				// Token: 0x0400BA5F RID: 47711
				public static LocString NAME = "VENTILATION OVERLAY";

				// Token: 0x0400BA60 RID: 47712
				public static LocString BUTTON = "Ventilation Overlay";

				// Token: 0x0400BA61 RID: 47713
				public static LocString CONSUMER = "Output Pipe";

				// Token: 0x0400BA62 RID: 47714
				public static LocString FILTERED = "Filtered Output Pipe";

				// Token: 0x0400BA63 RID: 47715
				public static LocString PRODUCER = "Building Intake";

				// Token: 0x0400BA64 RID: 47716
				public static LocString CONNECTED = "Connected";

				// Token: 0x0400BA65 RID: 47717
				public static LocString DISCONNECTED = "Disconnected";

				// Token: 0x0400BA66 RID: 47718
				public static LocString NETWORK = "Gas Network {0}";

				// Token: 0x0400BA67 RID: 47719
				public static LocString DIAGRAM_BEFORE_ARROW = "Gas flows from <b>Output Pipe</b>";

				// Token: 0x0400BA68 RID: 47720
				public static LocString DIAGRAM_AFTER_ARROW = "<b>Building Intake</b>";

				// Token: 0x02003711 RID: 14097
				public class TOOLTIPS
				{
					// Token: 0x0400DB64 RID: 56164
					public static LocString CONNECTED = "Connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING");

					// Token: 0x0400DB65 RID: 56165
					public static LocString DISCONNECTED = "Not connected to a " + UI.FormatAsLink("Gas Pipe", "GASPIPING");

					// Token: 0x0400DB66 RID: 56166
					public static LocString CONSUMER = string.Concat(new string[]
					{
						"<b>Output Pipe</b>\nOutputs send ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" into ",
						UI.PRE_KEYWORD,
						"Pipes",
						UI.PST_KEYWORD,
						"\n\nMust be on the same network as at least one ",
						UI.FormatAsLink("Intake", "GASPIPING")
					});

					// Token: 0x0400DB67 RID: 56167
					public static LocString FILTERED = string.Concat(new string[]
					{
						"<b>Filtered Output Pipe</b>\nFiltered Outputs send filtered ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" into ",
						UI.PRE_KEYWORD,
						"Pipes",
						UI.PST_KEYWORD,
						"\n\nMust be on the same network as at least one ",
						UI.FormatAsLink("Intake", "GASPIPING")
					});

					// Token: 0x0400DB68 RID: 56168
					public static LocString PRODUCER = "<b>Building Intake</b>\nIntakes send gas into buildings\n\nMust be on the same network as at least one " + UI.FormatAsLink("Output", "GASPIPING");

					// Token: 0x0400DB69 RID: 56169
					public static LocString NETWORK = "Gas network {0}";
				}
			}

			// Token: 0x02002AE3 RID: 10979
			public class SUIT
			{
				// Token: 0x0400BA69 RID: 47721
				public static LocString NAME = "EXOSUIT OVERLAY";

				// Token: 0x0400BA6A RID: 47722
				public static LocString BUTTON = "Exosuit Overlay";

				// Token: 0x0400BA6B RID: 47723
				public static LocString SUIT_ICON = "Exosuit";

				// Token: 0x0400BA6C RID: 47724
				public static LocString SUIT_ICON_TOOLTIP = "<b>Exosuit</b>\nHighlights the current location of equippable exosuits";
			}

			// Token: 0x02002AE4 RID: 10980
			public class LOGIC
			{
				// Token: 0x0400BA6D RID: 47725
				public static LocString NAME = "AUTOMATION OVERLAY";

				// Token: 0x0400BA6E RID: 47726
				public static LocString BUTTON = "Automation Overlay";

				// Token: 0x0400BA6F RID: 47727
				public static LocString INPUT = "Input Port";

				// Token: 0x0400BA70 RID: 47728
				public static LocString OUTPUT = "Output Port";

				// Token: 0x0400BA71 RID: 47729
				public static LocString RIBBON_INPUT = "Ribbon Input Port";

				// Token: 0x0400BA72 RID: 47730
				public static LocString RIBBON_OUTPUT = "Ribbon Output Port";

				// Token: 0x0400BA73 RID: 47731
				public static LocString RESET_UPDATE = "Reset Port";

				// Token: 0x0400BA74 RID: 47732
				public static LocString CONTROL_INPUT = "Control Port";

				// Token: 0x0400BA75 RID: 47733
				public static LocString CIRCUIT_STATUS_HEADER = "GRID STATUS";

				// Token: 0x0400BA76 RID: 47734
				public static LocString ONE = "Green";

				// Token: 0x0400BA77 RID: 47735
				public static LocString ZERO = "Red";

				// Token: 0x0400BA78 RID: 47736
				public static LocString DISCONNECTED = "DISCONNECTED";

				// Token: 0x02003712 RID: 14098
				public abstract class TOOLTIPS
				{
					// Token: 0x0400DB6A RID: 56170
					public static LocString INPUT = "<b>Input Port</b>\nReceives a signal from an automation grid";

					// Token: 0x0400DB6B RID: 56171
					public static LocString OUTPUT = "<b>Output Port</b>\nSends a signal out to an automation grid";

					// Token: 0x0400DB6C RID: 56172
					public static LocString RIBBON_INPUT = "<b>Ribbon Input Port</b>\nReceives a 4-bit signal from an automation grid";

					// Token: 0x0400DB6D RID: 56173
					public static LocString RIBBON_OUTPUT = "<b>Ribbon Output Port</b>\nSends a 4-bit signal out to an automation grid";

					// Token: 0x0400DB6E RID: 56174
					public static LocString RESET_UPDATE = "<b>Reset Port</b>\nReset a " + BUILDINGS.PREFABS.LOGICMEMORY.NAME + "'s internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

					// Token: 0x0400DB6F RID: 56175
					public static LocString CONTROL_INPUT = "<b>Control Port</b>\nControl the signal selection of a " + BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.NAME + " or " + BUILDINGS.PREFABS.LOGICGATEDEMULTIPLEXER.NAME;

					// Token: 0x0400DB70 RID: 56176
					public static LocString ONE = "<b>Green</b>\nThis port is currently " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

					// Token: 0x0400DB71 RID: 56177
					public static LocString ZERO = "<b>Red</b>\nThis port is currently " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

					// Token: 0x0400DB72 RID: 56178
					public static LocString DISCONNECTED = "<b>Disconnected</b>\nThis port is not connected to an automation grid";
				}
			}

			// Token: 0x02002AE5 RID: 10981
			public class CONVEYOR
			{
				// Token: 0x0400BA79 RID: 47737
				public static LocString NAME = "CONVEYOR OVERLAY";

				// Token: 0x0400BA7A RID: 47738
				public static LocString BUTTON = "Conveyor Overlay";

				// Token: 0x0400BA7B RID: 47739
				public static LocString OUTPUT = "Loader";

				// Token: 0x0400BA7C RID: 47740
				public static LocString INPUT = "Receptacle";

				// Token: 0x02003713 RID: 14099
				public abstract class TOOLTIPS
				{
					// Token: 0x0400DB73 RID: 56179
					public static LocString OUTPUT = string.Concat(new string[]
					{
						"<b>Loader</b>\nLoads material onto a ",
						UI.PRE_KEYWORD,
						"Conveyor Rail",
						UI.PST_KEYWORD,
						" for transport to Receptacles"
					});

					// Token: 0x0400DB74 RID: 56180
					public static LocString INPUT = string.Concat(new string[]
					{
						"<b>Receptacle</b>\nReceives material from a ",
						UI.PRE_KEYWORD,
						"Conveyor Rail",
						UI.PST_KEYWORD,
						" and stores it for Duplicant use"
					});
				}
			}

			// Token: 0x02002AE6 RID: 10982
			public class DECOR
			{
				// Token: 0x0400BA7D RID: 47741
				public static LocString NAME = "DECOR OVERLAY";

				// Token: 0x0400BA7E RID: 47742
				public static LocString BUTTON = "Decor Overlay";

				// Token: 0x0400BA7F RID: 47743
				public static LocString TOTAL = "Total Decor: ";

				// Token: 0x0400BA80 RID: 47744
				public static LocString ENTRY = "{0} {1} {2}";

				// Token: 0x0400BA81 RID: 47745
				public static LocString COUNT = "({0})";

				// Token: 0x0400BA82 RID: 47746
				public static LocString VALUE = "{0}{1}";

				// Token: 0x0400BA83 RID: 47747
				public static LocString VALUE_ZERO = "{0}{1}";

				// Token: 0x0400BA84 RID: 47748
				public static LocString HEADER_POSITIVE = "Positive Value:";

				// Token: 0x0400BA85 RID: 47749
				public static LocString HEADER_NEGATIVE = "Negative Value:";

				// Token: 0x0400BA86 RID: 47750
				public static LocString LOWDECOR = "Negative Decor";

				// Token: 0x0400BA87 RID: 47751
				public static LocString HIGHDECOR = "Positive Decor";

				// Token: 0x0400BA88 RID: 47752
				public static LocString CLUTTER = "Debris";

				// Token: 0x0400BA89 RID: 47753
				public static LocString LIGHTING = "Lighting";

				// Token: 0x0400BA8A RID: 47754
				public static LocString CLOTHING = "{0}'s Outfit";

				// Token: 0x0400BA8B RID: 47755
				public static LocString CLOTHING_TRAIT_DECORUP = "{0}'s Outfit (Innately Stylish)";

				// Token: 0x0400BA8C RID: 47756
				public static LocString CLOTHING_TRAIT_DECORDOWN = "{0}'s Outfit (Shabby Dresser)";

				// Token: 0x0400BA8D RID: 47757
				public static LocString HOVERTITLE = "DECOR";

				// Token: 0x0400BA8E RID: 47758
				public static LocString MAXIMUM_DECOR = "{0}{1} (Maximum Decor)";

				// Token: 0x02003714 RID: 14100
				public class TOOLTIPS
				{
					// Token: 0x0400DB75 RID: 56181
					public static LocString LOWDECOR = string.Concat(new string[]
					{
						"<b>Negative Decor</b>\nArea with insufficient ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" values\n* Resources on the floor are considered \"debris\" and will decrease decor"
					});

					// Token: 0x0400DB76 RID: 56182
					public static LocString HIGHDECOR = string.Concat(new string[]
					{
						"<b>Positive Decor</b>\nArea with sufficient ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" values\n* Lighting and aesthetically pleasing buildings increase decor"
					});
				}
			}

			// Token: 0x02002AE7 RID: 10983
			public class PRIORITIES
			{
				// Token: 0x0400BA8F RID: 47759
				public static LocString NAME = "PRIORITY OVERLAY";

				// Token: 0x0400BA90 RID: 47760
				public static LocString BUTTON = "Priority Overlay";

				// Token: 0x0400BA91 RID: 47761
				public static LocString ONE = "1 (Low Urgency)";

				// Token: 0x0400BA92 RID: 47762
				public static LocString ONE_TOOLTIP = "Priority 1";

				// Token: 0x0400BA93 RID: 47763
				public static LocString TWO = "2";

				// Token: 0x0400BA94 RID: 47764
				public static LocString TWO_TOOLTIP = "Priority 2";

				// Token: 0x0400BA95 RID: 47765
				public static LocString THREE = "3";

				// Token: 0x0400BA96 RID: 47766
				public static LocString THREE_TOOLTIP = "Priority 3";

				// Token: 0x0400BA97 RID: 47767
				public static LocString FOUR = "4";

				// Token: 0x0400BA98 RID: 47768
				public static LocString FOUR_TOOLTIP = "Priority 4";

				// Token: 0x0400BA99 RID: 47769
				public static LocString FIVE = "5";

				// Token: 0x0400BA9A RID: 47770
				public static LocString FIVE_TOOLTIP = "Priority 5";

				// Token: 0x0400BA9B RID: 47771
				public static LocString SIX = "6";

				// Token: 0x0400BA9C RID: 47772
				public static LocString SIX_TOOLTIP = "Priority 6";

				// Token: 0x0400BA9D RID: 47773
				public static LocString SEVEN = "7";

				// Token: 0x0400BA9E RID: 47774
				public static LocString SEVEN_TOOLTIP = "Priority 7";

				// Token: 0x0400BA9F RID: 47775
				public static LocString EIGHT = "8";

				// Token: 0x0400BAA0 RID: 47776
				public static LocString EIGHT_TOOLTIP = "Priority 8";

				// Token: 0x0400BAA1 RID: 47777
				public static LocString NINE = "9 (High Urgency)";

				// Token: 0x0400BAA2 RID: 47778
				public static LocString NINE_TOOLTIP = "Priority 9";
			}

			// Token: 0x02002AE8 RID: 10984
			public class DISEASE
			{
				// Token: 0x0400BAA3 RID: 47779
				public static LocString NAME = "GERM OVERLAY";

				// Token: 0x0400BAA4 RID: 47780
				public static LocString BUTTON = "Germ Overlay";

				// Token: 0x0400BAA5 RID: 47781
				public static LocString HOVERTITLE = "Germ";

				// Token: 0x0400BAA6 RID: 47782
				public static LocString INFECTION_SOURCE = "Germ Source";

				// Token: 0x0400BAA7 RID: 47783
				public static LocString INFECTION_SOURCE_TOOLTIP = "<b>Germ Source</b>\nAreas where germs are produced\n•  Placing Wash Basins or Hand Sanitizers near these areas may prevent disease spread";

				// Token: 0x0400BAA8 RID: 47784
				public static LocString NO_DISEASE = "Zero surface germs";

				// Token: 0x0400BAA9 RID: 47785
				public static LocString DISEASE_NAME_FORMAT = "{0}<color=#{1}></color>";

				// Token: 0x0400BAAA RID: 47786
				public static LocString DISEASE_NAME_FORMAT_NO_COLOR = "{0}";

				// Token: 0x0400BAAB RID: 47787
				public static LocString DISEASE_FORMAT = "{1} [{0}]<color=#{2}></color>";

				// Token: 0x0400BAAC RID: 47788
				public static LocString DISEASE_FORMAT_NO_COLOR = "{1} [{0}]";

				// Token: 0x0400BAAD RID: 47789
				public static LocString CONTAINER_FORMAT = "\n    {0}: {1}";

				// Token: 0x02003715 RID: 14101
				public class DISINFECT_THRESHOLD_DIAGRAM
				{
					// Token: 0x0400DB77 RID: 56183
					public static LocString UNITS = "Germs";

					// Token: 0x0400DB78 RID: 56184
					public static LocString MIN_LABEL = "0";

					// Token: 0x0400DB79 RID: 56185
					public static LocString MAX_LABEL = "1m";

					// Token: 0x0400DB7A RID: 56186
					public static LocString THRESHOLD_PREFIX = "Disinfect At:";

					// Token: 0x0400DB7B RID: 56187
					public static LocString TOOLTIP = "Automatically disinfect any building with more than {NumberOfGerms} germs.";

					// Token: 0x0400DB7C RID: 56188
					public static LocString TOOLTIP_DISABLED = "Automatic building disinfection disabled.";
				}
			}

			// Token: 0x02002AE9 RID: 10985
			public class CROPS
			{
				// Token: 0x0400BAAE RID: 47790
				public static LocString NAME = "FARMING OVERLAY";

				// Token: 0x0400BAAF RID: 47791
				public static LocString BUTTON = "Farming Overlay";
			}

			// Token: 0x02002AEA RID: 10986
			public class POWER
			{
				// Token: 0x0400BAB0 RID: 47792
				public static LocString WATTS_GENERATED = "Watts Generated";

				// Token: 0x0400BAB1 RID: 47793
				public static LocString WATTS_CONSUMED = "Watts Consumed";
			}

			// Token: 0x02002AEB RID: 10987
			public class RADIATION
			{
				// Token: 0x0400BAB2 RID: 47794
				public static LocString NAME = "RADIATION";

				// Token: 0x0400BAB3 RID: 47795
				public static LocString BUTTON = "Radiation Overlay";

				// Token: 0x0400BAB4 RID: 47796
				public static LocString DESC = "{rads} per cycle ({description})";

				// Token: 0x0400BAB5 RID: 47797
				public static LocString SHIELDING_DESC = "Radiation Blocking: {radiationAbsorptionFactor}";

				// Token: 0x0400BAB6 RID: 47798
				public static LocString HOVERTITLE = "RADIATION";

				// Token: 0x02003716 RID: 14102
				public class RANGES
				{
					// Token: 0x0400DB7D RID: 56189
					public static LocString NONE = "Completely Safe";

					// Token: 0x0400DB7E RID: 56190
					public static LocString VERY_LOW = "Mostly Safe";

					// Token: 0x0400DB7F RID: 56191
					public static LocString LOW = "Barely Safe";

					// Token: 0x0400DB80 RID: 56192
					public static LocString MEDIUM = "Slight Hazard";

					// Token: 0x0400DB81 RID: 56193
					public static LocString HIGH = "Significant Hazard";

					// Token: 0x0400DB82 RID: 56194
					public static LocString VERY_HIGH = "Extreme Hazard";

					// Token: 0x0400DB83 RID: 56195
					public static LocString MAX = "Maximum Hazard";

					// Token: 0x0400DB84 RID: 56196
					public static LocString INPUTPORT = "Radbolt Input Port";

					// Token: 0x0400DB85 RID: 56197
					public static LocString OUTPUTPORT = "Radbolt Output Port";
				}

				// Token: 0x02003717 RID: 14103
				public class TOOLTIPS
				{
					// Token: 0x0400DB86 RID: 56198
					public static LocString NONE = "Completely Safe";

					// Token: 0x0400DB87 RID: 56199
					public static LocString VERY_LOW = "Mostly Safe";

					// Token: 0x0400DB88 RID: 56200
					public static LocString LOW = "Barely Safe";

					// Token: 0x0400DB89 RID: 56201
					public static LocString MEDIUM = "Slight Hazard";

					// Token: 0x0400DB8A RID: 56202
					public static LocString HIGH = "Significant Hazard";

					// Token: 0x0400DB8B RID: 56203
					public static LocString VERY_HIGH = "Extreme Hazard";

					// Token: 0x0400DB8C RID: 56204
					public static LocString MAX = "Maximum Hazard";

					// Token: 0x0400DB8D RID: 56205
					public static LocString INPUTPORT = "Radbolt Input Port";

					// Token: 0x0400DB8E RID: 56206
					public static LocString OUTPUTPORT = "Radbolt Output Port";
				}
			}
		}

		// Token: 0x0200216B RID: 8555
		public class TABLESCREENS
		{
			// Token: 0x0400970C RID: 38668
			public static LocString DUPLICANT_PROPERNAME = "<b>{0}</b>";

			// Token: 0x0400970D RID: 38669
			public static LocString SELECT_DUPLICANT_BUTTON = UI.CLICK(UI.ClickType.Click) + " to select <b>{0}</b>";

			// Token: 0x0400970E RID: 38670
			public static LocString GOTO_DUPLICANT_BUTTON = "Double-" + UI.CLICK(UI.ClickType.click) + " to go to <b>{0}</b>";

			// Token: 0x0400970F RID: 38671
			public static LocString COLUMN_SORT_BY_NAME = "Sort by <b>Name</b>";

			// Token: 0x04009710 RID: 38672
			public static LocString COLUMN_SORT_BY_STRESS = "Sort by <b>Stress</b>";

			// Token: 0x04009711 RID: 38673
			public static LocString COLUMN_SORT_BY_HITPOINTS = "Sort by <b>Health</b>";

			// Token: 0x04009712 RID: 38674
			public static LocString COLUMN_SORT_BY_SICKNESSES = "Sort by <b>Disease</b>";

			// Token: 0x04009713 RID: 38675
			public static LocString COLUMN_SORT_BY_FULLNESS = "Sort by <b>Fullness</b>";

			// Token: 0x04009714 RID: 38676
			public static LocString COLUMN_SORT_BY_EATEN_TODAY = "Sort by number of <b>Calories</b> consumed today";

			// Token: 0x04009715 RID: 38677
			public static LocString COLUMN_SORT_BY_EXPECTATIONS = "Sort by <b>Morale</b>";

			// Token: 0x04009716 RID: 38678
			public static LocString NA = "N/A";

			// Token: 0x04009717 RID: 38679
			public static LocString INFORMATION_NOT_AVAILABLE_TOOLTIP = "Information is not available because {1} is in {0}";

			// Token: 0x04009718 RID: 38680
			public static LocString NOBODY_HERE = "Nobody here...";
		}

		// Token: 0x0200216C RID: 8556
		public class CONSUMABLESSCREEN
		{
			// Token: 0x04009719 RID: 38681
			public static LocString TITLE = "CONSUMABLES";

			// Token: 0x0400971A RID: 38682
			public static LocString TOOLTIP_TOGGLE_ALL = "Toggle <b>all</b> food permissions <b>colonywide</b>";

			// Token: 0x0400971B RID: 38683
			public static LocString TOOLTIP_TOGGLE_COLUMN = "Toggle colonywide <b>{0}</b> permission";

			// Token: 0x0400971C RID: 38684
			public static LocString TOOLTIP_TOGGLE_ROW = "Toggle <b>all consumable permissions</b> for <b>{0}</b>";

			// Token: 0x0400971D RID: 38685
			public static LocString NEW_MINIONS_TOOLTIP_TOGGLE_ROW = "Toggle <b>all consumable permissions</b> for <b>New Duplicants</b>";

			// Token: 0x0400971E RID: 38686
			public static LocString NEW_MINIONS_FOOD_PERMISSION_ON = string.Concat(new string[]
			{
				"<b>New Duplicants</b> are <b>allowed</b> to eat \n",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				"</b> by default"
			});

			// Token: 0x0400971F RID: 38687
			public static LocString NEW_MINIONS_FOOD_PERMISSION_OFF = string.Concat(new string[]
			{
				"<b>New Duplicants</b> are <b>not allowed</b> to eat \n",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" by default"
			});

			// Token: 0x04009720 RID: 38688
			public static LocString FOOD_PERMISSION_ON = "<b>{0}</b> is <b>allowed</b> to eat " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x04009721 RID: 38689
			public static LocString FOOD_PERMISSION_OFF = "<b>{0}</b> is <b>not allowed</b> to eat " + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x04009722 RID: 38690
			public static LocString FOOD_CANT_CONSUME = "<b>{0}</b> <b>physically cannot</b> eat\n" + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x04009723 RID: 38691
			public static LocString FOOD_REFUSE = "<b>{0}</b> <b>refuses</b> to eat\n" + UI.PRE_KEYWORD + "{1}" + UI.PST_KEYWORD;

			// Token: 0x04009724 RID: 38692
			public static LocString FOOD_AVAILABLE = "Available: {0}";

			// Token: 0x04009725 RID: 38693
			public static LocString FOOD_MORALE = UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD + ": {0}";

			// Token: 0x04009726 RID: 38694
			public static LocString FOOD_QUALITY = UI.PRE_KEYWORD + "Quality" + UI.PST_KEYWORD + ": {0}";

			// Token: 0x04009727 RID: 38695
			public static LocString FOOD_QUALITY_VS_EXPECTATION = string.Concat(new string[]
			{
				"\nThis food will give ",
				UI.PRE_KEYWORD,
				"Morale",
				UI.PST_KEYWORD,
				" <b>{0}</b> if {1} eats it"
			});

			// Token: 0x04009728 RID: 38696
			public static LocString CANNOT_ADJUST_PERMISSIONS = "Cannot adjust consumable permissions because they're in {0}";
		}

		// Token: 0x0200216D RID: 8557
		public class JOBSSCREEN
		{
			// Token: 0x04009729 RID: 38697
			public static LocString TITLE = "MANAGE DUPLICANT PRIORITIES";

			// Token: 0x0400972A RID: 38698
			public static LocString TOOLTIP_TOGGLE_ALL = "Set priority of all Errand Types colonywide";

			// Token: 0x0400972B RID: 38699
			public static LocString HEADER_TOOLTIP = string.Concat(new string[]
			{
				"<size=16>{Job} Errand Type</size>\n\n{Details}\n\nDuplicants will first choose what ",
				UI.PRE_KEYWORD,
				"Errand Type",
				UI.PST_KEYWORD,
				" to perform based on ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				",\nthen they will choose individual tasks within that type using ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by the ",
				UI.FormatAsLink("Priority Tool", "PRIORITIES"),
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities)
			});

			// Token: 0x0400972C RID: 38700
			public static LocString HEADER_DETAILS_TOOLTIP = "{Description}\n\nAffected errands: {ChoreList}";

			// Token: 0x0400972D RID: 38701
			public static LocString HEADER_CHANGE_TOOLTIP = string.Concat(new string[]
			{
				"Set the priority for the ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type colonywide\n"
			});

			// Token: 0x0400972E RID: 38702
			public static LocString NEW_MINION_ITEM_TOOLTIP = string.Concat(new string[]
			{
				"The ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type is automatically a {Priority} ",
				UI.PRE_KEYWORD,
				"Priority",
				UI.PST_KEYWORD,
				" for <b>Arriving Duplicants</b>"
			});

			// Token: 0x0400972F RID: 38703
			public static LocString ITEM_TOOLTIP = UI.PRE_KEYWORD + "{Job}" + UI.PST_KEYWORD + " Priority for {Name}:\n<b>{Priority} Priority ({PriorityValue})</b>";

			// Token: 0x04009730 RID: 38704
			public static LocString MINION_SKILL_TOOLTIP = string.Concat(new string[]
			{
				"{Name}'s ",
				UI.PRE_KEYWORD,
				"{Attribute}",
				UI.PST_KEYWORD,
				" Skill: "
			});

			// Token: 0x04009731 RID: 38705
			public static LocString TRAIT_DISABLED = string.Concat(new string[]
			{
				"{Name} possesses the ",
				UI.PRE_KEYWORD,
				"{Trait}",
				UI.PST_KEYWORD,
				" trait and <b>cannot</b> do ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errands"
			});

			// Token: 0x04009732 RID: 38706
			public static LocString INCREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Prioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>New Duplicants</b>"
			});

			// Token: 0x04009733 RID: 38707
			public static LocString DECREASE_ROW_PRIORITY_NEW_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Deprioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>New Duplicants</b>"
			});

			// Token: 0x04009734 RID: 38708
			public static LocString INCREASE_ROW_PRIORITY_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Prioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>{Name}</b>"
			});

			// Token: 0x04009735 RID: 38709
			public static LocString DECREASE_ROW_PRIORITY_MINION_TOOLTIP = string.Concat(new string[]
			{
				"Deprioritize ",
				UI.PRE_KEYWORD,
				"All Errands",
				UI.PST_KEYWORD,
				" for <b>{Name}</b>"
			});

			// Token: 0x04009736 RID: 38710
			public static LocString INCREASE_PRIORITY_TUTORIAL = "{Hotkey} Increase Priority";

			// Token: 0x04009737 RID: 38711
			public static LocString DECREASE_PRIORITY_TUTORIAL = "{Hotkey} Decrease Priority";

			// Token: 0x04009738 RID: 38712
			public static LocString CANNOT_ADJUST_PRIORITY = string.Concat(new string[]
			{
				"Priorities for ",
				UI.PRE_KEYWORD,
				"{0}",
				UI.PST_KEYWORD,
				" cannot be adjusted currently because they're in {1}"
			});

			// Token: 0x04009739 RID: 38713
			public static LocString SORT_TOOLTIP = string.Concat(new string[]
			{
				"Sort by the ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errand Type"
			});

			// Token: 0x0400973A RID: 38714
			public static LocString DISABLED_TOOLTIP = string.Concat(new string[]
			{
				"{Name} may not perform ",
				UI.PRE_KEYWORD,
				"{Job}",
				UI.PST_KEYWORD,
				" Errands"
			});

			// Token: 0x0400973B RID: 38715
			public static LocString OPTIONS = "Options";

			// Token: 0x0400973C RID: 38716
			public static LocString TOGGLE_ADVANCED_MODE = "Enable Proximity";

			// Token: 0x0400973D RID: 38717
			public static LocString TOGGLE_ADVANCED_MODE_TOOLTIP = "<b>Errand Proximity Settings</b>\n\nEnabling Proximity settings tells my Duplicants to always choose the closest, most urgent errand to perform.\n\nWhen disabled, Duplicants will choose between two high priority errands based on a hidden priority hierarchy instead.\n\nEnabling Proximity helps cut down on travel time in areas with lots of high priority errands, and is useful for large colonies.";

			// Token: 0x0400973E RID: 38718
			public static LocString RESET_SETTINGS = "Reset Priorities";

			// Token: 0x0400973F RID: 38719
			public static LocString RESET_SETTINGS_TOOLTIP = "<b>Reset Priorities</b>\n\nReturns all priorities to their default values.\n\nProximity Enabled: Priorities will be adjusted high-to-low.\n\nProximity Disabled: All priorities will be reset to neutral.";

			// Token: 0x02002AEC RID: 10988
			public class PRIORITY
			{
				// Token: 0x0400BAB7 RID: 47799
				public static LocString VERYHIGH = "Very High";

				// Token: 0x0400BAB8 RID: 47800
				public static LocString HIGH = "High";

				// Token: 0x0400BAB9 RID: 47801
				public static LocString STANDARD = "Standard";

				// Token: 0x0400BABA RID: 47802
				public static LocString LOW = "Low";

				// Token: 0x0400BABB RID: 47803
				public static LocString VERYLOW = "Very Low";

				// Token: 0x0400BABC RID: 47804
				public static LocString DISABLED = "Disallowed";
			}

			// Token: 0x02002AED RID: 10989
			public class PRIORITY_CLASS
			{
				// Token: 0x0400BABD RID: 47805
				public static LocString IDLE = "Idle";

				// Token: 0x0400BABE RID: 47806
				public static LocString BASIC = "Normal";

				// Token: 0x0400BABF RID: 47807
				public static LocString HIGH = "Urgent";

				// Token: 0x0400BAC0 RID: 47808
				public static LocString PERSONAL_NEEDS = "Personal Needs";

				// Token: 0x0400BAC1 RID: 47809
				public static LocString EMERGENCY = "Emergency";

				// Token: 0x0400BAC2 RID: 47810
				public static LocString COMPULSORY = "Involuntary";
			}
		}

		// Token: 0x0200216E RID: 8558
		public class VITALSSCREEN
		{
			// Token: 0x04009740 RID: 38720
			public static LocString HEALTH = "Health";

			// Token: 0x04009741 RID: 38721
			public static LocString SICKNESS = "Diseases";

			// Token: 0x04009742 RID: 38722
			public static LocString NO_SICKNESSES = "No diseases";

			// Token: 0x04009743 RID: 38723
			public static LocString MULTIPLE_SICKNESSES = "Multiple diseases ({0})";

			// Token: 0x04009744 RID: 38724
			public static LocString SICKNESS_REMAINING = "{0}\n({1})";

			// Token: 0x04009745 RID: 38725
			public static LocString STRESS = "Stress";

			// Token: 0x04009746 RID: 38726
			public static LocString EXPECTATIONS = "Expectations";

			// Token: 0x04009747 RID: 38727
			public static LocString CALORIES = "Fullness";

			// Token: 0x04009748 RID: 38728
			public static LocString EATEN_TODAY = "Eaten Today";

			// Token: 0x04009749 RID: 38729
			public static LocString EATEN_TODAY_TOOLTIP = "Consumed {0} of food this cycle";

			// Token: 0x0400974A RID: 38730
			public static LocString ATMOSPHERE_CONDITION = "Atmosphere:";

			// Token: 0x0400974B RID: 38731
			public static LocString SUBMERSION = "Liquid Level";

			// Token: 0x0400974C RID: 38732
			public static LocString NOT_DROWNING = "Liquid Level";

			// Token: 0x0400974D RID: 38733
			public static LocString FOOD_EXPECTATIONS = "Food Expectation";

			// Token: 0x0400974E RID: 38734
			public static LocString FOOD_EXPECTATIONS_TOOLTIP = "This Duplicant desires food that is {0} quality or better";

			// Token: 0x0400974F RID: 38735
			public static LocString DECOR_EXPECTATIONS = "Decor Expectation";

			// Token: 0x04009750 RID: 38736
			public static LocString DECOR_EXPECTATIONS_TOOLTIP = "This Duplicant desires decor that is {0} or higher";

			// Token: 0x04009751 RID: 38737
			public static LocString QUALITYOFLIFE_EXPECTATIONS = "Morale";

			// Token: 0x04009752 RID: 38738
			public static LocString QUALITYOFLIFE_EXPECTATIONS_TOOLTIP = "This Duplicant requires " + UI.FormatAsLink("{0} Morale", "MORALE") + ".\n\nCurrent Morale:";

			// Token: 0x02002AEE RID: 10990
			public class CONDITIONS_GROWING
			{
				// Token: 0x02003718 RID: 14104
				public class WILD
				{
					// Token: 0x0400DB8F RID: 56207
					public static LocString BASE = "<b>Wild Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400DB90 RID: 56208
					public static LocString TOOLTIP = "This plant will take {0} to grow in the wild";
				}

				// Token: 0x02003719 RID: 14105
				public class DOMESTIC
				{
					// Token: 0x0400DB91 RID: 56209
					public static LocString BASE = "<b>Domestic Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400DB92 RID: 56210
					public static LocString TOOLTIP = "This plant will take {0} to grow domestically";
				}

				// Token: 0x0200371A RID: 14106
				public class ADDITIONAL_DOMESTIC
				{
					// Token: 0x0400DB93 RID: 56211
					public static LocString BASE = "<b>Additional Domestic Growth\n[Life Cycle: {0}]</b>";

					// Token: 0x0400DB94 RID: 56212
					public static LocString TOOLTIP = "This plant will take {0} to grow domestically";
				}

				// Token: 0x0200371B RID: 14107
				public class WILD_DECOR
				{
					// Token: 0x0400DB95 RID: 56213
					public static LocString BASE = "<b>Wild Growth</b>";

					// Token: 0x0400DB96 RID: 56214
					public static LocString TOOLTIP = "This plant must have these requirements met to grow in the wild";
				}

				// Token: 0x0200371C RID: 14108
				public class WILD_INSTANT
				{
					// Token: 0x0400DB97 RID: 56215
					public static LocString BASE = "<b>Wild Growth\n[{0}% Throughput]</b>";

					// Token: 0x0400DB98 RID: 56216
					public static LocString TOOLTIP = "This plant must have these requirements met to grow in the wild";
				}

				// Token: 0x0200371D RID: 14109
				public class ADDITIONAL_DOMESTIC_INSTANT
				{
					// Token: 0x0400DB99 RID: 56217
					public static LocString BASE = "<b>Domestic Growth\n[{0}% Throughput]</b>";

					// Token: 0x0400DB9A RID: 56218
					public static LocString TOOLTIP = "This plant must have these requirements met to grow domestically";
				}
			}
		}

		// Token: 0x0200216F RID: 8559
		public class SCHEDULESCREEN
		{
			// Token: 0x04009753 RID: 38739
			public static LocString SCHEDULE_EDITOR = "SCHEDULE EDITOR";

			// Token: 0x04009754 RID: 38740
			public static LocString SCHEDULE_NAME_DEFAULT = "Default Schedule";

			// Token: 0x04009755 RID: 38741
			public static LocString SCHEDULE_NAME_FORMAT = "Schedule {0}";

			// Token: 0x04009756 RID: 38742
			public static LocString SCHEDULE_DROPDOWN_ASSIGNED = "{0} (Assigned)";

			// Token: 0x04009757 RID: 38743
			public static LocString SCHEDULE_DROPDOWN_BLANK = "<i>Move Duplicant...</i>";

			// Token: 0x04009758 RID: 38744
			public static LocString SCHEDULE_DOWNTIME_MORALE = "Duplicants will receive {0} Morale from the scheduled Downtime shifts";

			// Token: 0x04009759 RID: 38745
			public static LocString RENAME_BUTTON_TOOLTIP = "Rename custom schedule";

			// Token: 0x0400975A RID: 38746
			public static LocString ALARM_BUTTON_ON_TOOLTIP = "Toggle Notifications\n\nSounds and notifications will play when shifts change for this schedule.\n\nENABLED\n" + UI.CLICK(UI.ClickType.Click) + " to disable";

			// Token: 0x0400975B RID: 38747
			public static LocString ALARM_BUTTON_OFF_TOOLTIP = "Toggle Notifications\n\nNo sounds or notifications will play for this schedule.\n\nDISABLED\n" + UI.CLICK(UI.ClickType.Click) + " to enable";

			// Token: 0x0400975C RID: 38748
			public static LocString DELETE_BUTTON_TOOLTIP = "Delete Schedule";

			// Token: 0x0400975D RID: 38749
			public static LocString PAINT_TOOLS = "Paint Tools:";

			// Token: 0x0400975E RID: 38750
			public static LocString ADD_SCHEDULE = "Add New Schedule";

			// Token: 0x0400975F RID: 38751
			public static LocString POO = "dar";

			// Token: 0x04009760 RID: 38752
			public static LocString DOWNTIME_MORALE = "Downtime Morale: {0}";

			// Token: 0x04009761 RID: 38753
			public static LocString ALARM_TITLE_ENABLED = "Alarm On";

			// Token: 0x04009762 RID: 38754
			public static LocString ALARM_TITLE_DISABLED = "Alarm Off";

			// Token: 0x04009763 RID: 38755
			public static LocString SETTINGS = "Settings";

			// Token: 0x04009764 RID: 38756
			public static LocString ALARM_BUTTON = "Shift Alarms";

			// Token: 0x04009765 RID: 38757
			public static LocString RESET_SETTINGS = "Reset Shifts";

			// Token: 0x04009766 RID: 38758
			public static LocString RESET_SETTINGS_TOOLTIP = "Restore this schedule to default shifts";

			// Token: 0x04009767 RID: 38759
			public static LocString DELETE_SCHEDULE = "Delete Schedule";

			// Token: 0x04009768 RID: 38760
			public static LocString DELETE_SCHEDULE_TOOLTIP = "Remove this schedule and unassign all Duplicants from it";

			// Token: 0x04009769 RID: 38761
			public static LocString DUPLICANT_NIGHTOWL_TOOLTIP = string.Concat(new string[]
			{
				DUPLICANTS.TRAITS.NIGHTOWL.NAME,
				"\n• All ",
				UI.PRE_KEYWORD,
				"Attributes",
				UI.PST_KEYWORD,
				" <b>+3</b> at night"
			});

			// Token: 0x0400976A RID: 38762
			public static LocString DUPLICANT_EARLYBIRD_TOOLTIP = string.Concat(new string[]
			{
				DUPLICANTS.TRAITS.EARLYBIRD.NAME,
				"\n• All ",
				UI.PRE_KEYWORD,
				"Attributes",
				UI.PST_KEYWORD,
				" <b>+2</b> in the morning"
			});

			// Token: 0x0400976B RID: 38763
			public static LocString SHIFT_SCHEDULE_LEFT_TOOLTIP = "Shift all schedule blocks left";

			// Token: 0x0400976C RID: 38764
			public static LocString SHIFT_SCHEDULE_RIGHT_TOOLTIP = "Shift all schedule blocks right";

			// Token: 0x0400976D RID: 38765
			public static LocString SHIFT_SCHEDULE_UP_TOOLTIP = "Swap this row with the one above it";

			// Token: 0x0400976E RID: 38766
			public static LocString SHIFT_SCHEDULE_DOWN_TOOLTIP = "Swap this row with the one below it";

			// Token: 0x0400976F RID: 38767
			public static LocString DUPLICATE_SCHEDULE_TIMETABLE = "Duplicate this row";

			// Token: 0x04009770 RID: 38768
			public static LocString DELETE_SCHEDULE_TIMETABLE = "Delete this row\n\nSchedules must have two or more rows in order for one row to be deleted";

			// Token: 0x04009771 RID: 38769
			public static LocString DUPLICATE_SCHEDULE = "Duplicate this schedule";
		}

		// Token: 0x02002170 RID: 8560
		public class COLONYLOSTSCREEN
		{
			// Token: 0x04009772 RID: 38770
			public static LocString COLONYLOST = "COLONY LOST";

			// Token: 0x04009773 RID: 38771
			public static LocString COLONYLOSTDESCRIPTION = "All Duplicants are dead or incapacitated.";

			// Token: 0x04009774 RID: 38772
			public static LocString RESTARTPROMPT = "Press <color=#F44A47><b>[ESC]</b></color> to return to a previous colony, or begin a new one.";

			// Token: 0x04009775 RID: 38773
			public static LocString DISMISSBUTTON = "DISMISS";

			// Token: 0x04009776 RID: 38774
			public static LocString QUITBUTTON = "MAIN MENU";
		}

		// Token: 0x02002171 RID: 8561
		public class VICTORYSCREEN
		{
			// Token: 0x04009777 RID: 38775
			public static LocString HEADER = "SUCCESS: IMPERATIVE ACHIEVED!";

			// Token: 0x04009778 RID: 38776
			public static LocString DESCRIPTION = "I have fulfilled the conditions of one of my Hardwired Imperatives";

			// Token: 0x04009779 RID: 38777
			public static LocString RESTARTPROMPT = "Press <color=#F44A47><b>[ESC]</b></color> to retire the colony and begin anew.";

			// Token: 0x0400977A RID: 38778
			public static LocString DISMISSBUTTON = "DISMISS";

			// Token: 0x0400977B RID: 38779
			public static LocString RETIREBUTTON = "RETIRE COLONY";
		}

		// Token: 0x02002172 RID: 8562
		public class GENESHUFFLERMESSAGE
		{
			// Token: 0x0400977C RID: 38780
			public static LocString HEADER = "NEURAL VACILLATION COMPLETE";

			// Token: 0x0400977D RID: 38781
			public static LocString BODY_SUCCESS = "Whew! <b>{0}'s</b> brain is still vibrating, but they've never felt better!\n\n<b>{0}</b> acquired the <b>{1}</b> trait.\n\n<b>{1}:</b>\n{2}";

			// Token: 0x0400977E RID: 38782
			public static LocString BODY_FAILURE = "The machine attempted to alter this Duplicant, but there's no improving on perfection.\n\n<b>{0}</b> already has all positive traits!";

			// Token: 0x0400977F RID: 38783
			public static LocString DISMISSBUTTON = "DISMISS";
		}

		// Token: 0x02002173 RID: 8563
		public class CRASHSCREEN
		{
			// Token: 0x04009780 RID: 38784
			public static LocString TITLE = "\"Whoops! We're sorry, but it seems your game has encountered an error. It's okay though - these errors are how we find and fix problems to make our game more fun for everyone. If you use the box below to submit a crash report to us, we can use this information to get the issue sorted out.\"";

			// Token: 0x04009781 RID: 38785
			public static LocString TITLE_MODS = "\"Oops-a-daisy! We're sorry, but it seems your game has encountered an error. If you uncheck all of the mods below, we will be able to help the next time this happens. Any mods that could be related to this error have already been unchecked.\"";

			// Token: 0x04009782 RID: 38786
			public static LocString HEADER = "OPTIONAL CRASH DESCRIPTION";

			// Token: 0x04009783 RID: 38787
			public static LocString HEADER_MODS = "ACTIVE MODS";

			// Token: 0x04009784 RID: 38788
			public static LocString BODY = "Help! A black hole ate my game!";

			// Token: 0x04009785 RID: 38789
			public static LocString THANKYOU = "Thank you!\n\nYou're making our game better, one crash at a time.";

			// Token: 0x04009786 RID: 38790
			public static LocString UPLOAD_FAILED = "There was an issue in reporting this crash.\n\nPlease submit a bug report at:\n<u>https://forums.kleientertainment.com/klei-bug-tracker/oni/</u>";

			// Token: 0x04009787 RID: 38791
			public static LocString UPLOADINFO = "UPLOAD ADDITIONAL INFO ({0})";

			// Token: 0x04009788 RID: 38792
			public static LocString REPORTBUTTON = "REPORT CRASH";

			// Token: 0x04009789 RID: 38793
			public static LocString REPORTING = "REPORTING, PLEASE WAIT...";

			// Token: 0x0400978A RID: 38794
			public static LocString CONTINUEBUTTON = "CONTINUE GAME";

			// Token: 0x0400978B RID: 38795
			public static LocString MOREINFOBUTTON = "MORE INFO";

			// Token: 0x0400978C RID: 38796
			public static LocString COPYTOCLIPBOARDBUTTON = "COPY TO CLIPBOARD";

			// Token: 0x0400978D RID: 38797
			public static LocString QUITBUTTON = "QUIT TO DESKTOP";

			// Token: 0x0400978E RID: 38798
			public static LocString SAVEFAILED = "Save Failed: {0}";

			// Token: 0x0400978F RID: 38799
			public static LocString LOADFAILED = "Load Failed: {0}\nSave Version: {1}\nExpected: {2}";

			// Token: 0x04009790 RID: 38800
			public static LocString REPORTEDERROR_SUCCESS = "Thank you for reporting this error.";

			// Token: 0x04009791 RID: 38801
			public static LocString REPORTEDERROR_FAILURE_TOO_LARGE = "Unable to report error. Save file is too large. Please contact us using the bug tracker.";

			// Token: 0x04009792 RID: 38802
			public static LocString REPORTEDERROR_FAILURE = "Unable to report error. Please contact us using the bug tracker.";

			// Token: 0x04009793 RID: 38803
			public static LocString UPLOADINPROGRESS = "Submitting {0}";
		}

		// Token: 0x02002174 RID: 8564
		public class DEMOOVERSCREEN
		{
			// Token: 0x04009794 RID: 38804
			public static LocString TIMEREMAINING = "Demo time remaining:";

			// Token: 0x04009795 RID: 38805
			public static LocString TIMERTOOLTIP = "Demo time remaining";

			// Token: 0x04009796 RID: 38806
			public static LocString TIMERINACTIVE = "Timer inactive";

			// Token: 0x04009797 RID: 38807
			public static LocString DEMOOVER = "END OF DEMO";

			// Token: 0x04009798 RID: 38808
			public static LocString DESCRIPTION = "Thank you for playing <color=#F44A47>Oxygen Not Included</color>!";

			// Token: 0x04009799 RID: 38809
			public static LocString DESCRIPTION_2 = "";

			// Token: 0x0400979A RID: 38810
			public static LocString QUITBUTTON = "RESET";
		}

		// Token: 0x02002175 RID: 8565
		public class CREDITSSCREEN
		{
			// Token: 0x0400979B RID: 38811
			public static LocString TITLE = "CREDITS";

			// Token: 0x0400979C RID: 38812
			public static LocString CLOSEBUTTON = "CLOSE";

			// Token: 0x02002AEF RID: 10991
			public class THIRD_PARTY
			{
				// Token: 0x0400BAC3 RID: 47811
				public static LocString FMOD = "FMOD Sound System\nCopyright Firelight Technologies";

				// Token: 0x0400BAC4 RID: 47812
				public static LocString HARMONY = "Harmony by Andreas Pardeike";
			}
		}

		// Token: 0x02002176 RID: 8566
		public class ALLRESOURCESSCREEN
		{
			// Token: 0x0400979D RID: 38813
			public static LocString RESOURCES_TITLE = "RESOURCES";

			// Token: 0x0400979E RID: 38814
			public static LocString RESOURCES = "Resources";

			// Token: 0x0400979F RID: 38815
			public static LocString SEARCH = "Search";

			// Token: 0x040097A0 RID: 38816
			public static LocString NAME = "Resource";

			// Token: 0x040097A1 RID: 38817
			public static LocString TOTAL = "Total";

			// Token: 0x040097A2 RID: 38818
			public static LocString AVAILABLE = "Available";

			// Token: 0x040097A3 RID: 38819
			public static LocString RESERVED = "Reserved";

			// Token: 0x040097A4 RID: 38820
			public static LocString SEARCH_PLACEHODLER = "Enter text...";

			// Token: 0x040097A5 RID: 38821
			public static LocString FIRST_FRAME_NO_DATA = "...";

			// Token: 0x040097A6 RID: 38822
			public static LocString PIN_TOOLTIP = "Check to pin resource to side panel";

			// Token: 0x040097A7 RID: 38823
			public static LocString UNPIN_TOOLTIP = "Unpin resource";
		}

		// Token: 0x02002177 RID: 8567
		public class PRIORITYSCREEN
		{
			// Token: 0x040097A8 RID: 38824
			public static LocString BASIC = "Set the order in which specific pending errands should be done\n\n1: Least Urgent\n9: Most Urgent";

			// Token: 0x040097A9 RID: 38825
			public static LocString HIGH = "";

			// Token: 0x040097AA RID: 38826
			public static LocString TOP_PRIORITY = "Top Priority\n\nThis priority will override all other priorities and set the colony on Yellow Alert until the errand is completed";

			// Token: 0x040097AB RID: 38827
			public static LocString HIGH_TOGGLE = "";

			// Token: 0x040097AC RID: 38828
			public static LocString OPEN_JOBS_SCREEN = string.Concat(new string[]
			{
				UI.CLICK(UI.ClickType.Click),
				" to open the Priorities Screen\n\nDuplicants will first decide what to work on based on their ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				", and then decide where to work based on ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD
			});

			// Token: 0x040097AD RID: 38829
			public static LocString DIAGRAM = string.Concat(new string[]
			{
				"Duplicants will first choose what ",
				UI.PRE_KEYWORD,
				"Errand Type",
				UI.PST_KEYWORD,
				" to perform using their ",
				UI.PRE_KEYWORD,
				"Duplicant Priorities",
				UI.PST_KEYWORD,
				" ",
				UI.FormatAsHotKey(global::Action.ManagePriorities),
				"\n\nThey will then choose one ",
				UI.PRE_KEYWORD,
				"Errand",
				UI.PST_KEYWORD,
				" from within that type using the ",
				UI.PRE_KEYWORD,
				"Building Priorities",
				UI.PST_KEYWORD,
				" set by this tool"
			});

			// Token: 0x040097AE RID: 38830
			public static LocString DIAGRAM_TITLE = "BUILDING PRIORITY";
		}

		// Token: 0x02002178 RID: 8568
		public class RESOURCESCREEN
		{
			// Token: 0x040097AF RID: 38831
			public static LocString HEADER = "RESOURCES";

			// Token: 0x040097B0 RID: 38832
			public static LocString CATEGORY_TOOLTIP = "Counts all unallocated resources within reach\n\n" + UI.CLICK(UI.ClickType.Click) + " to expand";

			// Token: 0x040097B1 RID: 38833
			public static LocString AVAILABLE_TOOLTIP = "Available: <b>{0}</b>\n({1} of {2} allocated to pending errands)";

			// Token: 0x040097B2 RID: 38834
			public static LocString TREND_TOOLTIP = "The available amount of this resource has {0} {1} in the last cycle";

			// Token: 0x040097B3 RID: 38835
			public static LocString TREND_TOOLTIP_NO_CHANGE = "The available amount of this resource has NOT CHANGED in the last cycle";

			// Token: 0x040097B4 RID: 38836
			public static LocString FLAT_STR = "<b>NOT CHANGED</b>";

			// Token: 0x040097B5 RID: 38837
			public static LocString INCREASING_STR = "<color=" + Constants.POSITIVE_COLOR_STR + ">INCREASED</color>";

			// Token: 0x040097B6 RID: 38838
			public static LocString DECREASING_STR = "<color=" + Constants.NEGATIVE_COLOR_STR + ">DECREASED</color>";

			// Token: 0x040097B7 RID: 38839
			public static LocString CLEAR_NEW_RESOURCES = "Clear New";

			// Token: 0x040097B8 RID: 38840
			public static LocString CLEAR_ALL = "Unpin all resources";

			// Token: 0x040097B9 RID: 38841
			public static LocString SEE_ALL = "+ See All ({0})";

			// Token: 0x040097BA RID: 38842
			public static LocString NEW_TAG = "NEW";
		}

		// Token: 0x02002179 RID: 8569
		public class CONFIRMDIALOG
		{
			// Token: 0x040097BB RID: 38843
			public static LocString OK = "OK";

			// Token: 0x040097BC RID: 38844
			public static LocString CANCEL = "CANCEL";

			// Token: 0x040097BD RID: 38845
			public static LocString DIALOG_HEADER = "MESSAGE";
		}

		// Token: 0x0200217A RID: 8570
		public class FACADE_SELECTION_PANEL
		{
			// Token: 0x040097BE RID: 38846
			public static LocString HEADER = "Select Blueprint";

			// Token: 0x040097BF RID: 38847
			public static LocString STORE_BUTTON_TOOLTIP = "See more Blueprints in the Supply Closet";
		}

		// Token: 0x0200217B RID: 8571
		public class FILE_NAME_DIALOG
		{
			// Token: 0x040097C0 RID: 38848
			public static LocString ENTER_TEXT = "Enter Text...";
		}

		// Token: 0x0200217C RID: 8572
		public class MINION_IDENTITY_SORT
		{
			// Token: 0x040097C1 RID: 38849
			public static LocString TITLE = "Sort By";

			// Token: 0x040097C2 RID: 38850
			public static LocString NAME = "Duplicant";

			// Token: 0x040097C3 RID: 38851
			public static LocString ROLE = "Role";

			// Token: 0x040097C4 RID: 38852
			public static LocString PERMISSION = "Permission";
		}

		// Token: 0x0200217D RID: 8573
		public class UISIDESCREENS
		{
			// Token: 0x02002AF0 RID: 10992
			public class TABS
			{
				// Token: 0x0400BAC5 RID: 47813
				public static LocString HEADER = "Options";

				// Token: 0x0400BAC6 RID: 47814
				public static LocString CONFIGURATION = "Config";

				// Token: 0x0400BAC7 RID: 47815
				public static LocString MATERIAL = "Material";

				// Token: 0x0400BAC8 RID: 47816
				public static LocString SKIN = "Blueprint";
			}

			// Token: 0x02002AF1 RID: 10993
			public class BLUEPRINT_TAB
			{
				// Token: 0x0400BAC9 RID: 47817
				public static LocString EDIT_OUTFIT_BUTTON = "Restyle";

				// Token: 0x0400BACA RID: 47818
				public static LocString SUBCATEGORY_OUTFIT = "Clothing";

				// Token: 0x0400BACB RID: 47819
				public static LocString SUBCATEGORY_ATMOSUIT = "Atmo Suit";

				// Token: 0x0400BACC RID: 47820
				public static LocString SUBCATEGORY_JOYRESPONSE = "Overjoyed";
			}

			// Token: 0x02002AF2 RID: 10994
			public class NOCONFIG
			{
				// Token: 0x0400BACD RID: 47821
				public static LocString TITLE = "No configuration";

				// Token: 0x0400BACE RID: 47822
				public static LocString LABEL = "There is no configuration available for this object.";
			}

			// Token: 0x02002AF3 RID: 10995
			public class ARTABLESELECTIONSIDESCREEN
			{
				// Token: 0x0400BACF RID: 47823
				public static LocString TITLE = "Style Selection";

				// Token: 0x0400BAD0 RID: 47824
				public static LocString BUTTON = "Redecorate";

				// Token: 0x0400BAD1 RID: 47825
				public static LocString BUTTON_TOOLTIP = "Clears current artwork\n\nCreates errand for a skilled Duplicant to create selected style";

				// Token: 0x0400BAD2 RID: 47826
				public static LocString CLEAR_BUTTON_TOOLTIP = "Clears current artwork\n\nAllows a skilled Duplicant to create artwork of their choice";
			}

			// Token: 0x02002AF4 RID: 10996
			public class ARTIFACTANALYSISSIDESCREEN
			{
				// Token: 0x0400BAD3 RID: 47827
				public static LocString NO_ARTIFACTS_DISCOVERED = "No artifacts analyzed";

				// Token: 0x0400BAD4 RID: 47828
				public static LocString NO_ARTIFACTS_DISCOVERED_TOOLTIP = "Analyzing artifacts requires a Duplicant with the Masterworks skill";
			}

			// Token: 0x02002AF5 RID: 10997
			public class BUTTONMENUSIDESCREEN
			{
				// Token: 0x0400BAD5 RID: 47829
				public static LocString TITLE = "Building Menu";

				// Token: 0x0400BAD6 RID: 47830
				public static LocString ALLOW_INTERNAL_CONSTRUCTOR = "Enable Auto-Delivery";

				// Token: 0x0400BAD7 RID: 47831
				public static LocString ALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = "Order Duplicants to deliver {0}" + UI.FormatAsLink("s", "NONE") + " to this building automatically when they need replacing";

				// Token: 0x0400BAD8 RID: 47832
				public static LocString DISALLOW_INTERNAL_CONSTRUCTOR = "Cancel Auto-Delivery";

				// Token: 0x0400BAD9 RID: 47833
				public static LocString DISALLOW_INTERNAL_CONSTRUCTOR_TOOLTIP = "Cancel automatic {0} deliveries to this building";
			}

			// Token: 0x02002AF6 RID: 10998
			public class CONFIGURECONSUMERSIDESCREEN
			{
				// Token: 0x0400BADA RID: 47834
				public static LocString TITLE = "Configure Building";

				// Token: 0x0400BADB RID: 47835
				public static LocString SELECTION_DESCRIPTION_HEADER = "Description";
			}

			// Token: 0x02002AF7 RID: 10999
			public class TREEFILTERABLESIDESCREEN
			{
				// Token: 0x0400BADC RID: 47836
				public static LocString TITLE = "Element Filter";

				// Token: 0x0400BADD RID: 47837
				public static LocString TITLE_CRITTER = "Critter Filter";

				// Token: 0x0400BADE RID: 47838
				public static LocString ALLBUTTON = "All Standard";

				// Token: 0x0400BADF RID: 47839
				public static LocString ALLBUTTONTOOLTIP = "Allow storage of all standard resources preferred by this building\n\nNon-standard resources must be selected manually\n\nNon-standard resources include:\n    • Clothing\n    • Critter Eggs\n    • Sublimators";

				// Token: 0x0400BAE0 RID: 47840
				public static LocString ALLBUTTON_EDIBLES = "All Edibles";

				// Token: 0x0400BAE1 RID: 47841
				public static LocString ALLBUTTON_EDIBLES_TOOLTIP = "Allow storage of all edible resources";

				// Token: 0x0400BAE2 RID: 47842
				public static LocString ALLBUTTON_CRITTERS = "All Critters";

				// Token: 0x0400BAE3 RID: 47843
				public static LocString ALLBUTTON_CRITTERS_TOOLTIP = "Allow storage of all eligible " + UI.PRE_KEYWORD + "Critters" + UI.PST_KEYWORD;

				// Token: 0x0400BAE4 RID: 47844
				public static LocString SPECIAL_RESOURCES = "Non-Standard";

				// Token: 0x0400BAE5 RID: 47845
				public static LocString SPECIAL_RESOURCES_TOOLTIP = "These objects may not be ideally suited to storage";

				// Token: 0x0400BAE6 RID: 47846
				public static LocString CATEGORYBUTTONTOOLTIP = "Allow storage of anything in the {0} resource category";

				// Token: 0x0400BAE7 RID: 47847
				public static LocString MATERIALBUTTONTOOLTIP = "Add or remove this material from storage";

				// Token: 0x0400BAE8 RID: 47848
				public static LocString ONLYALLOWTRANSPORTITEMSBUTTON = "Sweep Only";

				// Token: 0x0400BAE9 RID: 47849
				public static LocString ONLYALLOWTRANSPORTITEMSBUTTONTOOLTIP = "Only store objects marked Sweep <color=#F44A47><b>[K]</b></color> in this container";

				// Token: 0x0400BAEA RID: 47850
				public static LocString ONLYALLOWSPICEDITEMSBUTTON = "Spiced Food Only";

				// Token: 0x0400BAEB RID: 47851
				public static LocString ONLYALLOWSPICEDITEMSBUTTONTOOLTIP = "Only store foods that have been spiced at the " + UI.PRE_KEYWORD + "Spice Grinder" + UI.PST_KEYWORD;

				// Token: 0x0400BAEC RID: 47852
				public static LocString SEARCH_PLACEHOLDER = "Search";
			}

			// Token: 0x02002AF8 RID: 11000
			public class TELESCOPESIDESCREEN
			{
				// Token: 0x0400BAED RID: 47853
				public static LocString TITLE = "Telescope Configuration";

				// Token: 0x0400BAEE RID: 47854
				public static LocString NO_SELECTED_ANALYSIS_TARGET = "No analysis focus selected\nOpen the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to select a focus";

				// Token: 0x0400BAEF RID: 47855
				public static LocString ANALYSIS_TARGET_SELECTED = "Object focus selected\nAnalysis underway";

				// Token: 0x0400BAF0 RID: 47856
				public static LocString OPENSTARMAPBUTTON = "OPEN STARMAP";

				// Token: 0x0400BAF1 RID: 47857
				public static LocString ANALYSIS_TARGET_HEADER = "Object Analysis";
			}

			// Token: 0x02002AF9 RID: 11001
			public class CLUSTERTELESCOPESIDESCREEN
			{
				// Token: 0x0400BAF2 RID: 47858
				public static LocString TITLE = "Telescope Configuration";

				// Token: 0x0400BAF3 RID: 47859
				public static LocString CHECKBOX_METEORS = "Allow meteor shower identification";

				// Token: 0x0400BAF4 RID: 47860
				public static LocString CHECKBOX_TOOLTIP_METEORS = string.Concat(new string[]
				{
					"Prioritizes unidentified meteors that come within range in a previously revealed location\n\nWill interrupt a Duplicant working on revealing a new ",
					UI.PRE_KEYWORD,
					"Starmap",
					UI.PST_KEYWORD,
					" location"
				});
			}

			// Token: 0x02002AFA RID: 11002
			public class TEMPORALTEARSIDESCREEN
			{
				// Token: 0x0400BAF5 RID: 47861
				public static LocString TITLE = "Temporal Tear";

				// Token: 0x0400BAF6 RID: 47862
				public static LocString BUTTON_OPEN = "Enter Tear";

				// Token: 0x0400BAF7 RID: 47863
				public static LocString BUTTON_CLOSED = "Tear Closed";

				// Token: 0x0400BAF8 RID: 47864
				public static LocString BUTTON_LABEL = "Enter Temporal Tear";

				// Token: 0x0400BAF9 RID: 47865
				public static LocString CONFIRM_POPUP_MESSAGE = "Are you sure you want to fire this?";

				// Token: 0x0400BAFA RID: 47866
				public static LocString CONFIRM_POPUP_CONFIRM = "Yes, I'm ready for a meteor shower.";

				// Token: 0x0400BAFB RID: 47867
				public static LocString CONFIRM_POPUP_CANCEL = "No, I need more time to prepare.";

				// Token: 0x0400BAFC RID: 47868
				public static LocString CONFIRM_POPUP_TITLE = "Temporal Tear Opener";
			}

			// Token: 0x02002AFB RID: 11003
			public class RAILGUNSIDESCREEN
			{
				// Token: 0x0400BAFD RID: 47869
				public static LocString TITLE = "Launcher Configuration";

				// Token: 0x0400BAFE RID: 47870
				public static LocString NO_SELECTED_LAUNCH_TARGET = "No destination selected\nOpen the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to set a course";

				// Token: 0x0400BAFF RID: 47871
				public static LocString LAUNCH_TARGET_SELECTED = "Launcher destination {0} set";

				// Token: 0x0400BB00 RID: 47872
				public static LocString OPENSTARMAPBUTTON = "OPEN STARMAP";

				// Token: 0x0400BB01 RID: 47873
				public static LocString LAUNCH_RESOURCES_HEADER = "Launch Resources:";

				// Token: 0x0400BB02 RID: 47874
				public static LocString MINIMUM_PAYLOAD_MASS = "Minimum launch mass:";
			}

			// Token: 0x02002AFC RID: 11004
			public class CLUSTERWORLDSIDESCREEN
			{
				// Token: 0x0400BB03 RID: 47875
				public static LocString TITLE = UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400BB04 RID: 47876
				public static LocString VIEW_WORLD = "Oversee " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400BB05 RID: 47877
				public static LocString VIEW_WORLD_DISABLE_TOOLTIP = "Cannot view " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400BB06 RID: 47878
				public static LocString VIEW_WORLD_TOOLTIP = "View this " + UI.CLUSTERMAP.PLANETOID + "'s surface";
			}

			// Token: 0x02002AFD RID: 11005
			public class ROCKETMODULESIDESCREEN
			{
				// Token: 0x0400BB07 RID: 47879
				public static LocString TITLE = "Rocket Module";

				// Token: 0x0400BB08 RID: 47880
				public static LocString CHANGEMODULEPANEL = "Add or Change Module";

				// Token: 0x0400BB09 RID: 47881
				public static LocString ENGINE_MAX_HEIGHT = "This engine allows a <b>Maximum Rocket Height</b> of {0}";

				// Token: 0x0200371E RID: 14110
				public class MODULESTATCHANGE
				{
					// Token: 0x0400DB9B RID: 56219
					public static LocString TITLE = "Rocket stats on construction:";

					// Token: 0x0400DB9C RID: 56220
					public static LocString BURDEN = "    • " + DUPLICANTS.ATTRIBUTES.ROCKETBURDEN.NAME + ": {0} ({1})";

					// Token: 0x0400DB9D RID: 56221
					public static LocString RANGE = string.Concat(new string[]
					{
						"    • Potential ",
						DUPLICANTS.ATTRIBUTES.FUELRANGEPERKILOGRAM.NAME,
						": {0}/1",
						UI.UNITSUFFIXES.MASS.KILOGRAM,
						" Fuel ({1})"
					});

					// Token: 0x0400DB9E RID: 56222
					public static LocString SPEED = "    • Speed: {0} ({1})";

					// Token: 0x0400DB9F RID: 56223
					public static LocString ENGINEPOWER = "    • " + DUPLICANTS.ATTRIBUTES.ROCKETENGINEPOWER.NAME + ": {0} ({1})";

					// Token: 0x0400DBA0 RID: 56224
					public static LocString HEIGHT = "    • " + DUPLICANTS.ATTRIBUTES.HEIGHT.NAME + ": {0}/{2} ({1})";

					// Token: 0x0400DBA1 RID: 56225
					public static LocString HEIGHT_NOMAX = "    • " + DUPLICANTS.ATTRIBUTES.HEIGHT.NAME + ": {0} ({1})";

					// Token: 0x0400DBA2 RID: 56226
					public static LocString POSITIVEDELTA = UI.FormatAsPositiveModifier("{0}");

					// Token: 0x0400DBA3 RID: 56227
					public static LocString NEGATIVEDELTA = UI.FormatAsNegativeModifier("{0}");
				}

				// Token: 0x0200371F RID: 14111
				public class BUTTONSWAPMODULEUP
				{
					// Token: 0x0400DBA4 RID: 56228
					public static LocString DESC = "Swap this rocket module with the one above";

					// Token: 0x0400DBA5 RID: 56229
					public static LocString INVALID = "No module above may be swapped.\n\n    • A module above may be unable to have modules placed above it.\n    • A module above may be unable to fit into the space below it.\n    • This module may be unable to fit into the space above it.";
				}

				// Token: 0x02003720 RID: 14112
				public class BUTTONVIEWINTERIOR
				{
					// Token: 0x0400DBA6 RID: 56230
					public static LocString LABEL = "View Interior";

					// Token: 0x0400DBA7 RID: 56231
					public static LocString DESC = "What's goin' on in there?";

					// Token: 0x0400DBA8 RID: 56232
					public static LocString INVALID = "This module does not have an interior view";
				}

				// Token: 0x02003721 RID: 14113
				public class BUTTONVIEWEXTERIOR
				{
					// Token: 0x0400DBA9 RID: 56233
					public static LocString LABEL = "View Exterior";

					// Token: 0x0400DBAA RID: 56234
					public static LocString DESC = "Switch to external world view";

					// Token: 0x0400DBAB RID: 56235
					public static LocString INVALID = "Not available in flight";
				}

				// Token: 0x02003722 RID: 14114
				public class BUTTONSWAPMODULEDOWN
				{
					// Token: 0x0400DBAC RID: 56236
					public static LocString DESC = "Swap this rocket module with the one below";

					// Token: 0x0400DBAD RID: 56237
					public static LocString INVALID = "No module below may be swapped.\n\n    • A module below may be unable to have modules placed below it.\n    • A module below may be unable to fit into the space above it.\n    • This module may be unable to fit into the space below it.";
				}

				// Token: 0x02003723 RID: 14115
				public class BUTTONCHANGEMODULE
				{
					// Token: 0x0400DBAE RID: 56238
					public static LocString DESC = "Swap this module for a different module";

					// Token: 0x0400DBAF RID: 56239
					public static LocString INVALID = "This module cannot be changed to a different type";
				}

				// Token: 0x02003724 RID: 14116
				public class BUTTONREMOVEMODULE
				{
					// Token: 0x0400DBB0 RID: 56240
					public static LocString DESC = "Remove this module";

					// Token: 0x0400DBB1 RID: 56241
					public static LocString INVALID = "This module cannot be removed";
				}

				// Token: 0x02003725 RID: 14117
				public class ADDMODULE
				{
					// Token: 0x0400DBB2 RID: 56242
					public static LocString DESC = "Add a new module above this one";

					// Token: 0x0400DBB3 RID: 56243
					public static LocString INVALID = "Modules cannot be added above this module, or there is no room above to add a module";
				}
			}

			// Token: 0x02002AFE RID: 11006
			public class CLUSTERLOCATIONFILTERSIDESCREEN
			{
				// Token: 0x0400BB0A RID: 47882
				public static LocString TITLE = "Location Filter";

				// Token: 0x0400BB0B RID: 47883
				public static LocString HEADER = "Send Green signal at locations";

				// Token: 0x0400BB0C RID: 47884
				public static LocString EMPTY_SPACE_ROW = "In Space";
			}

			// Token: 0x02002AFF RID: 11007
			public class DISPENSERSIDESCREEN
			{
				// Token: 0x0400BB0D RID: 47885
				public static LocString TITLE = "Dispenser";

				// Token: 0x0400BB0E RID: 47886
				public static LocString BUTTON_CANCEL = "Cancel order";

				// Token: 0x0400BB0F RID: 47887
				public static LocString BUTTON_DISPENSE = "Dispense item";
			}

			// Token: 0x02002B00 RID: 11008
			public class ROCKETRESTRICTIONSIDESCREEN
			{
				// Token: 0x0400BB10 RID: 47888
				public static LocString TITLE = "Rocket Restrictions";

				// Token: 0x0400BB11 RID: 47889
				public static LocString BUILDING_RESTRICTIONS_LABEL = "Interior Building Restrictions";

				// Token: 0x0400BB12 RID: 47890
				public static LocString NONE_RESTRICTION_BUTTON = "None";

				// Token: 0x0400BB13 RID: 47891
				public static LocString NONE_RESTRICTION_BUTTON_TOOLTIP = "There are no restrictions on buildings inside this rocket";

				// Token: 0x0400BB14 RID: 47892
				public static LocString GROUNDED_RESTRICTION_BUTTON = "Grounded";

				// Token: 0x0400BB15 RID: 47893
				public static LocString GROUNDED_RESTRICTION_BUTTON_TOOLTIP = "Buildings with their access restricted cannot be operated while grounded, though they can still be filled";

				// Token: 0x0400BB16 RID: 47894
				public static LocString AUTOMATION = "Automation Controlled";

				// Token: 0x0400BB17 RID: 47895
				public static LocString AUTOMATION_TOOLTIP = "Building restrictions are managed by automation\n\nBuildings with their access restricted cannot be operated, though they can still be filled";
			}

			// Token: 0x02002B01 RID: 11009
			public class HABITATMODULESIDESCREEN
			{
				// Token: 0x0400BB18 RID: 47896
				public static LocString TITLE = "Spacefarer Module";

				// Token: 0x0400BB19 RID: 47897
				public static LocString VIEW_BUTTON = "View Interior";

				// Token: 0x0400BB1A RID: 47898
				public static LocString VIEW_BUTTON_TOOLTIP = "What's goin' on in there?";
			}

			// Token: 0x02002B02 RID: 11010
			public class HARVESTMODULESIDESCREEN
			{
				// Token: 0x0400BB1B RID: 47899
				public static LocString TITLE = "Resource Gathering";

				// Token: 0x0400BB1C RID: 47900
				public static LocString MINING_IN_PROGRESS = "Drilling...";

				// Token: 0x0400BB1D RID: 47901
				public static LocString MINING_STOPPED = "Not drilling";

				// Token: 0x0400BB1E RID: 47902
				public static LocString ENABLE = "Enable Drill";

				// Token: 0x0400BB1F RID: 47903
				public static LocString DISABLE = "Disable Drill";
			}

			// Token: 0x02002B03 RID: 11011
			public class SELECTMODULESIDESCREEN
			{
				// Token: 0x0400BB20 RID: 47904
				public static LocString TITLE = "Select Module";

				// Token: 0x0400BB21 RID: 47905
				public static LocString BUILDBUTTON = "Build";

				// Token: 0x02003726 RID: 14118
				public class CONSTRAINTS
				{
					// Token: 0x02003B37 RID: 15159
					public class RESEARCHED
					{
						// Token: 0x0400E591 RID: 58769
						public static LocString COMPLETE = "Research Completed";

						// Token: 0x0400E592 RID: 58770
						public static LocString FAILED = "Research Incomplete";
					}

					// Token: 0x02003B38 RID: 15160
					public class MATERIALS_AVAILABLE
					{
						// Token: 0x0400E593 RID: 58771
						public static LocString COMPLETE = "Materials available";

						// Token: 0x0400E594 RID: 58772
						public static LocString FAILED = "• Materials unavailable";
					}

					// Token: 0x02003B39 RID: 15161
					public class ONE_COMMAND_PER_ROCKET
					{
						// Token: 0x0400E595 RID: 58773
						public static LocString COMPLETE = "";

						// Token: 0x0400E596 RID: 58774
						public static LocString FAILED = "• Command module already installed";
					}

					// Token: 0x02003B3A RID: 15162
					public class ONE_ENGINE_PER_ROCKET
					{
						// Token: 0x0400E597 RID: 58775
						public static LocString COMPLETE = "";

						// Token: 0x0400E598 RID: 58776
						public static LocString FAILED = "• Engine module already installed";
					}

					// Token: 0x02003B3B RID: 15163
					public class ENGINE_AT_BOTTOM
					{
						// Token: 0x0400E599 RID: 58777
						public static LocString COMPLETE = "";

						// Token: 0x0400E59A RID: 58778
						public static LocString FAILED = "• Must install at bottom of rocket";
					}

					// Token: 0x02003B3C RID: 15164
					public class TOP_ONLY
					{
						// Token: 0x0400E59B RID: 58779
						public static LocString COMPLETE = "";

						// Token: 0x0400E59C RID: 58780
						public static LocString FAILED = "• Must install at top of rocket";
					}

					// Token: 0x02003B3D RID: 15165
					public class SPACE_AVAILABLE
					{
						// Token: 0x0400E59D RID: 58781
						public static LocString COMPLETE = "";

						// Token: 0x0400E59E RID: 58782
						public static LocString FAILED = "• Space above rocket blocked";
					}

					// Token: 0x02003B3E RID: 15166
					public class PASSENGER_MODULE_AVAILABLE
					{
						// Token: 0x0400E59F RID: 58783
						public static LocString COMPLETE = "";

						// Token: 0x0400E5A0 RID: 58784
						public static LocString FAILED = "• Max number of passenger modules installed";
					}

					// Token: 0x02003B3F RID: 15167
					public class MAX_MODULES
					{
						// Token: 0x0400E5A1 RID: 58785
						public static LocString COMPLETE = "";

						// Token: 0x0400E5A2 RID: 58786
						public static LocString FAILED = "• Max module limit of engine reached";
					}

					// Token: 0x02003B40 RID: 15168
					public class MAX_HEIGHT
					{
						// Token: 0x0400E5A3 RID: 58787
						public static LocString COMPLETE = "";

						// Token: 0x0400E5A4 RID: 58788
						public static LocString FAILED = "• Engine's height limit reached or exceeded";

						// Token: 0x0400E5A5 RID: 58789
						public static LocString FAILED_NO_ENGINE = "• Rocket requires space for an engine";
					}

					// Token: 0x02003B41 RID: 15169
					public class ONE_ROBOPILOT_PER_ROCKET
					{
						// Token: 0x0400E5A6 RID: 58790
						public static LocString COMPLETE = "";

						// Token: 0x0400E5A7 RID: 58791
						public static LocString FAILED = "• Robo-Pilot module already installed";
					}
				}
			}

			// Token: 0x02002B04 RID: 11012
			public class FILTERSIDESCREEN
			{
				// Token: 0x0400BB22 RID: 47906
				public static LocString TITLE = "Filter Outputs";

				// Token: 0x0400BB23 RID: 47907
				public static LocString NO_SELECTION = "None";

				// Token: 0x0400BB24 RID: 47908
				public static LocString OUTPUTELEMENTHEADER = "Output 1";

				// Token: 0x0400BB25 RID: 47909
				public static LocString SELECTELEMENTHEADER = "Output 2";

				// Token: 0x0400BB26 RID: 47910
				public static LocString OUTPUTRED = "Output Red";

				// Token: 0x0400BB27 RID: 47911
				public static LocString OUTPUTGREEN = "Output Green";

				// Token: 0x0400BB28 RID: 47912
				public static LocString NOELEMENTSELECTED = "No element selected";

				// Token: 0x0400BB29 RID: 47913
				public static LocString DRIEDFOOD = "Dried Food";

				// Token: 0x02003727 RID: 14119
				public static class UNFILTEREDELEMENTS
				{
					// Token: 0x0400DBB4 RID: 56244
					public static LocString GAS = "Gas Output:\nAll";

					// Token: 0x0400DBB5 RID: 56245
					public static LocString LIQUID = "Liquid Output:\nAll";

					// Token: 0x0400DBB6 RID: 56246
					public static LocString SOLID = "Solid Output:\nAll";
				}

				// Token: 0x02003728 RID: 14120
				public static class FILTEREDELEMENT
				{
					// Token: 0x0400DBB7 RID: 56247
					public static LocString GAS = "Filtered Gas Output:\n{0}";

					// Token: 0x0400DBB8 RID: 56248
					public static LocString LIQUID = "Filtered Liquid Output:\n{0}";

					// Token: 0x0400DBB9 RID: 56249
					public static LocString SOLID = "Filtered Solid Output:\n{0}";
				}
			}

			// Token: 0x02002B05 RID: 11013
			public class SINGLEITEMSELECTIONSIDESCREEN
			{
				// Token: 0x0400BB2A RID: 47914
				public static LocString TITLE = "Element Filter";

				// Token: 0x0400BB2B RID: 47915
				public static LocString LIST_TITLE = "Options";

				// Token: 0x0400BB2C RID: 47916
				public static LocString NO_SELECTION = "None";

				// Token: 0x02003729 RID: 14121
				public class CURRENT_ITEM_SELECTED_SECTION
				{
					// Token: 0x0400DBBA RID: 56250
					public static LocString TITLE = "Current Selection";

					// Token: 0x0400DBBB RID: 56251
					public static LocString NO_ITEM_TITLE = "No Item Selected";

					// Token: 0x0400DBBC RID: 56252
					public static LocString NO_ITEM_MESSAGE = "Select an item for storage below.";
				}
			}

			// Token: 0x02002B06 RID: 11014
			public class FEWOPTIONSELECTIONSIDESCREEN
			{
				// Token: 0x0400BB2D RID: 47917
				public static LocString TITLE = "Options";
			}

			// Token: 0x02002B07 RID: 11015
			public class LOGICBROADCASTCHANNELSIDESCREEN
			{
				// Token: 0x0400BB2E RID: 47918
				public static LocString TITLE = "Channel Selector";

				// Token: 0x0400BB2F RID: 47919
				public static LocString HEADER = "Channel Selector";

				// Token: 0x0400BB30 RID: 47920
				public static LocString IN_RANGE = "In Range";

				// Token: 0x0400BB31 RID: 47921
				public static LocString OUT_OF_RANGE = "Out of Range";

				// Token: 0x0400BB32 RID: 47922
				public static LocString NO_SENDERS = "No Channels Available";

				// Token: 0x0400BB33 RID: 47923
				public static LocString NO_SENDERS_DESC = "Build a " + BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.NAME + " to transmit a signal.";
			}

			// Token: 0x02002B08 RID: 11016
			public class CONDITIONLISTSIDESCREEN
			{
				// Token: 0x0400BB34 RID: 47924
				public static LocString TITLE = "Condition List";
			}

			// Token: 0x02002B09 RID: 11017
			public class FABRICATORSIDESCREEN
			{
				// Token: 0x0400BB35 RID: 47925
				public static LocString TITLE = "Production Orders";

				// Token: 0x0400BB36 RID: 47926
				public static LocString SUBTITLE = "Recipes";

				// Token: 0x0400BB37 RID: 47927
				public static LocString NORECIPEDISCOVERED = "No discovered recipes";

				// Token: 0x0400BB38 RID: 47928
				public static LocString NORECIPEDISCOVERED_BODY = "Discover new ingredients or research new technology to unlock some recipes.";

				// Token: 0x0400BB39 RID: 47929
				public static LocString NORECIPESELECTED = "No recipe selected";

				// Token: 0x0400BB3A RID: 47930
				public static LocString SELECTRECIPE = "Select a recipe to fabricate.";

				// Token: 0x0400BB3B RID: 47931
				public static LocString COST = "<b>Ingredients:</b>\n";

				// Token: 0x0400BB3C RID: 47932
				public static LocString RESULTREQUIREMENTS = "<b>Requirements:</b>";

				// Token: 0x0400BB3D RID: 47933
				public static LocString RESULTEFFECTS = "<b>Effects:</b>";

				// Token: 0x0400BB3E RID: 47934
				public static LocString KG = "- {0}: {1}\n";

				// Token: 0x0400BB3F RID: 47935
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400BB40 RID: 47936
				public static LocString CANCEL = "Cancel";

				// Token: 0x0400BB41 RID: 47937
				public static LocString RECIPERQUIREMENT = "{0}: {1} / {2}";

				// Token: 0x0400BB42 RID: 47938
				public static LocString RECIPEPRODUCT = "{0}: {1}";

				// Token: 0x0400BB43 RID: 47939
				public static LocString UNITS_AND_CALS = "{0} [{1}]";

				// Token: 0x0400BB44 RID: 47940
				public static LocString CALS = "{0}";

				// Token: 0x0400BB45 RID: 47941
				public static LocString QUEUED_MISSING_INGREDIENTS_TOOLTIP = "Missing {0} of {1}\n";

				// Token: 0x0400BB46 RID: 47942
				public static LocString CURRENT_ORDER = "Current order: {0}";

				// Token: 0x0400BB47 RID: 47943
				public static LocString NEXT_ORDER = "Next order: {0}";

				// Token: 0x0400BB48 RID: 47944
				public static LocString NO_WORKABLE_ORDER = "No workable order";

				// Token: 0x0400BB49 RID: 47945
				public static LocString RECIPE_DETAILS = "Recipe Details";

				// Token: 0x0400BB4A RID: 47946
				public static LocString RECIPE_QUEUE = "Order Production Quantity:";

				// Token: 0x0400BB4B RID: 47947
				public static LocString RECIPE_FOREVER = "Forever";

				// Token: 0x0400BB4C RID: 47948
				public static LocString CHANGE_RECIPE_ARROW_LABEL = "Change recipe";

				// Token: 0x0400BB4D RID: 47949
				public static LocString RECIPE_RESEARCH_REQUIRED = "Research Required";

				// Token: 0x0400BB4E RID: 47950
				public static LocString INGREDIENTS = "<b>Ingredients:</b>";

				// Token: 0x0400BB4F RID: 47951
				public static LocString RECIPE_EFFECTS = "<b>Effects:</b>";

				// Token: 0x0400BB50 RID: 47952
				public static LocString ALLOW_MUTANT_SEED_INGREDIENTS = "Building accepts mutant seeds";

				// Token: 0x0400BB51 RID: 47953
				public static LocString ALLOW_MUTANT_SEED_INGREDIENTS_TOOLTIP = "Toggle whether Duplicants will deliver mutant seed species to this building as recipe ingredients.";

				// Token: 0x0200372A RID: 14122
				public class TOOLTIPS
				{
					// Token: 0x0400DBBD RID: 56253
					public static LocString RECIPE_WORKTIME = "This recipe takes {0} to complete";

					// Token: 0x0400DBBE RID: 56254
					public static LocString RECIPERQUIREMENT_SUFFICIENT = "This recipe consumes {1} of an available {2} of {0}";

					// Token: 0x0400DBBF RID: 56255
					public static LocString RECIPERQUIREMENT_INSUFFICIENT = "This recipe requires {1} {0}\nAvailable: {2}";

					// Token: 0x0400DBC0 RID: 56256
					public static LocString RECIPEPRODUCT = "This recipe produces {1} {0}";
				}

				// Token: 0x0200372B RID: 14123
				public class EFFECTS
				{
					// Token: 0x0400DBC1 RID: 56257
					public static LocString OXYGEN_TANK = STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK.NAME + " ({0})";

					// Token: 0x0400DBC2 RID: 56258
					public static LocString OXYGEN_TANK_UNDERWATER = STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK_UNDERWATER.NAME + " ({0})";

					// Token: 0x0400DBC3 RID: 56259
					public static LocString JETSUIT_TANK = STRINGS.EQUIPMENT.PREFABS.JET_SUIT.TANK_EFFECT_NAME + " ({0})";

					// Token: 0x0400DBC4 RID: 56260
					public static LocString LEADSUIT_BATTERY = STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.BATTERY_EFFECT_NAME + " ({0})";

					// Token: 0x0400DBC5 RID: 56261
					public static LocString COOL_VEST = STRINGS.EQUIPMENT.PREFABS.COOL_VEST.NAME + " ({0})";

					// Token: 0x0400DBC6 RID: 56262
					public static LocString WARM_VEST = STRINGS.EQUIPMENT.PREFABS.WARM_VEST.NAME + " ({0})";

					// Token: 0x0400DBC7 RID: 56263
					public static LocString FUNKY_VEST = STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.NAME + " ({0})";

					// Token: 0x0400DBC8 RID: 56264
					public static LocString RESEARCHPOINT = "{0}: +1";
				}

				// Token: 0x0200372C RID: 14124
				public class RECIPE_CATEGORIES
				{
					// Token: 0x0400DBC9 RID: 56265
					public static LocString ATMO_SUIT_FACADES = "Atmo Suit Styles";

					// Token: 0x0400DBCA RID: 56266
					public static LocString JET_SUIT_FACADES = "Jet Suit Styles";

					// Token: 0x0400DBCB RID: 56267
					public static LocString LEAD_SUIT_FACADES = "Lead Suit Styles";

					// Token: 0x0400DBCC RID: 56268
					public static LocString PRIMO_GARB_FACADES = "Primo Garb Styles";
				}
			}

			// Token: 0x02002B0A RID: 11018
			public class ASSIGNMENTGROUPCONTROLLER
			{
				// Token: 0x0400BB52 RID: 47954
				public static LocString TITLE = "Duplicant Assignment";

				// Token: 0x0400BB53 RID: 47955
				public static LocString PILOT = "Pilot";

				// Token: 0x0400BB54 RID: 47956
				public static LocString OFFWORLD = "Offworld";

				// Token: 0x0200372D RID: 14125
				public class TOOLTIPS
				{
					// Token: 0x0400DBCD RID: 56269
					public static LocString DIFFERENT_WORLD = "This Duplicant is on a different " + UI.CLUSTERMAP.PLANETOID;

					// Token: 0x0400DBCE RID: 56270
					public static LocString ASSIGN = "<b>Add</b> this Duplicant to rocket crew";

					// Token: 0x0400DBCF RID: 56271
					public static LocString UNASSIGN = "<b>Remove</b> this Duplicant from rocket crew";
				}
			}

			// Token: 0x02002B0B RID: 11019
			public class LAUNCHPADSIDESCREEN
			{
				// Token: 0x0400BB55 RID: 47957
				public static LocString TITLE = "Rocket Platform";

				// Token: 0x0400BB56 RID: 47958
				public static LocString WAITING_TO_LAND_PANEL = "Waiting to land";

				// Token: 0x0400BB57 RID: 47959
				public static LocString NO_ROCKETS_WAITING = "No rockets in orbit";

				// Token: 0x0400BB58 RID: 47960
				public static LocString IN_ORBIT_ABOVE_PANEL = "Rockets in orbit";

				// Token: 0x0400BB59 RID: 47961
				public static LocString NEW_ROCKET_BUTTON = "NEW ROCKET";

				// Token: 0x0400BB5A RID: 47962
				public static LocString LAND_BUTTON = "LAND HERE";

				// Token: 0x0400BB5B RID: 47963
				public static LocString CANCEL_LAND_BUTTON = "CANCEL";

				// Token: 0x0400BB5C RID: 47964
				public static LocString LAUNCH_BUTTON = "BEGIN LAUNCH SEQUENCE";

				// Token: 0x0400BB5D RID: 47965
				public static LocString LAUNCH_BUTTON_DEBUG = "BEGIN LAUNCH SEQUENCE (DEBUG ENABLED)";

				// Token: 0x0400BB5E RID: 47966
				public static LocString LAUNCH_BUTTON_TOOLTIP = "Blast off!";

				// Token: 0x0400BB5F RID: 47967
				public static LocString LAUNCH_BUTTON_NOT_READY_TOOLTIP = "This rocket is <b>not</b> ready to launch\n\n<b>Review the Launch Checklist in the status panel for more detail</b>";

				// Token: 0x0400BB60 RID: 47968
				public static LocString LAUNCH_WARNINGS_BUTTON = "ACKNOWLEDGE WARNINGS";

				// Token: 0x0400BB61 RID: 47969
				public static LocString LAUNCH_WARNINGS_BUTTON_TOOLTIP = "Some items in the Launch Checklist require attention\n\n<b>" + UI.CLICK(UI.ClickType.Click) + " to ignore warnings and proceed with launch</b>";

				// Token: 0x0400BB62 RID: 47970
				public static LocString LAUNCH_REQUESTED_BUTTON = "CANCEL LAUNCH";

				// Token: 0x0400BB63 RID: 47971
				public static LocString LAUNCH_REQUESTED_BUTTON_TOOLTIP = "This rocket will take off as soon as a Duplicant takes the controls\n\n<b>" + UI.CLICK(UI.ClickType.Click) + " to cancel launch</b>";

				// Token: 0x0400BB64 RID: 47972
				public static LocString LAUNCH_AUTOMATION_CONTROLLED = "AUTOMATION CONTROLLED";

				// Token: 0x0400BB65 RID: 47973
				public static LocString LAUNCH_AUTOMATION_CONTROLLED_TOOLTIP = "This " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + "'s launch operation is controlled by automation signals";

				// Token: 0x0200372E RID: 14126
				public class STATUS
				{
					// Token: 0x0400DBD0 RID: 56272
					public static LocString STILL_PREPPING = "Launch Checklist Incomplete";

					// Token: 0x0400DBD1 RID: 56273
					public static LocString READY_FOR_LAUNCH = "Ready to Launch";

					// Token: 0x0400DBD2 RID: 56274
					public static LocString LOADING_CREW = "Loading crew...";

					// Token: 0x0400DBD3 RID: 56275
					public static LocString UNLOADING_PASSENGERS = "Unloading non-crew...";

					// Token: 0x0400DBD4 RID: 56276
					public static LocString WAITING_FOR_PILOT = "Pilot requested at control station...";

					// Token: 0x0400DBD5 RID: 56277
					public static LocString COUNTING_DOWN = "5... 4... 3... 2... 1...";

					// Token: 0x0400DBD6 RID: 56278
					public static LocString TAKING_OFF = "Liftoff!!";
				}
			}

			// Token: 0x02002B0C RID: 11020
			public class AUTOPLUMBERSIDESCREEN
			{
				// Token: 0x0400BB66 RID: 47974
				public static LocString TITLE = "Automatic Building Configuration";

				// Token: 0x0200372F RID: 14127
				public class BUTTONS
				{
					// Token: 0x02003B42 RID: 15170
					public class POWER
					{
						// Token: 0x0400E5A8 RID: 58792
						public static LocString TOOLTIP = "Add Dev Generator and Electrical Wires";
					}

					// Token: 0x02003B43 RID: 15171
					public class PIPES
					{
						// Token: 0x0400E5A9 RID: 58793
						public static LocString TOOLTIP = "Add Dev Pumps and Pipes";
					}

					// Token: 0x02003B44 RID: 15172
					public class SOLIDS
					{
						// Token: 0x0400E5AA RID: 58794
						public static LocString TOOLTIP = "Spawn solid resources for a relevant recipe or conversions";
					}

					// Token: 0x02003B45 RID: 15173
					public class MINION
					{
						// Token: 0x0400E5AB RID: 58795
						public static LocString TOOLTIP = "Spawn a Duplicant in front of the building";
					}

					// Token: 0x02003B46 RID: 15174
					public class FACADE
					{
						// Token: 0x0400E5AC RID: 58796
						public static LocString TOOLTIP = "Toggle the building blueprint";
					}
				}
			}

			// Token: 0x02002B0D RID: 11021
			public class SELFDESTRUCTSIDESCREEN
			{
				// Token: 0x0400BB67 RID: 47975
				public static LocString TITLE = "Self Destruct";

				// Token: 0x0400BB68 RID: 47976
				public static LocString MESSAGE_TEXT = "EMERGENCY PROCEDURES";

				// Token: 0x0400BB69 RID: 47977
				public static LocString BUTTON_TEXT = "ABANDON SHIP!";

				// Token: 0x0400BB6A RID: 47978
				public static LocString BUTTON_TEXT_CONFIRM = "CONFIRM ABANDON SHIP";

				// Token: 0x0400BB6B RID: 47979
				public static LocString BUTTON_TOOLTIP = "This rocket is equipped with an emergency escape system.\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";

				// Token: 0x0400BB6C RID: 47980
				public static LocString BUTTON_TOOLTIP_CONFIRM = "<b>This will eject any passengers and destroy the rocket.<b>\n\nThe rocket's self-destruct sequence can be triggered to destroy it and propel fragments of the ship towards the nearest planetoid.\n\nAny Duplicants on board will be safely delivered in escape pods.";
			}

			// Token: 0x02002B0E RID: 11022
			public class GENESHUFFLERSIDESREEN
			{
				// Token: 0x0400BB6D RID: 47981
				public static LocString TITLE = "Neural Vacillator";

				// Token: 0x0400BB6E RID: 47982
				public static LocString COMPLETE = "Something feels different.";

				// Token: 0x0400BB6F RID: 47983
				public static LocString UNDERWAY = "Neural Vacillation in progress.";

				// Token: 0x0400BB70 RID: 47984
				public static LocString CONSUMED = "There are no charges left in this Vacillator.";

				// Token: 0x0400BB71 RID: 47985
				public static LocString CONSUMED_WAITING = "Recharge requested, awaiting delivery by Duplicant.";

				// Token: 0x0400BB72 RID: 47986
				public static LocString BUTTON = "Complete Neural Process";

				// Token: 0x0400BB73 RID: 47987
				public static LocString BUTTON_RECHARGE = "Recharge";

				// Token: 0x0400BB74 RID: 47988
				public static LocString BUTTON_RECHARGE_CANCEL = "Cancel Recharge";
			}

			// Token: 0x02002B0F RID: 11023
			public class MINIONTODOSIDESCREEN
			{
				// Token: 0x0400BB75 RID: 47989
				public static LocString NAME = "Errands";

				// Token: 0x0400BB76 RID: 47990
				public static LocString TOOLTIP = "<b>Errands</b>\nView current and upcoming errands";

				// Token: 0x0400BB77 RID: 47991
				public static LocString CURRENT_TITLE = "Current Errand";

				// Token: 0x0400BB78 RID: 47992
				public static LocString LIST_TITLE = "Upcoming Errands";

				// Token: 0x0400BB79 RID: 47993
				public static LocString CURRENT_SCHEDULE_BLOCK = "Schedule Block: {0}";

				// Token: 0x0400BB7A RID: 47994
				public static LocString CHORE_TARGET = "{Target}";

				// Token: 0x0400BB7B RID: 47995
				public static LocString CHORE_TARGET_AND_GROUP = "{Target} -- {Groups}";

				// Token: 0x0400BB7C RID: 47996
				public static LocString SELF_LABEL = "Self";

				// Token: 0x0400BB7D RID: 47997
				public static LocString TRUNCATED_CHORES = "{0} more";

				// Token: 0x0400BB7E RID: 47998
				public static LocString TOOLTIP_IDLE = string.Concat(new string[]
				{
					"{IdleDescription}\n\nDuplicants will only <b>{Errand}</b> when there is nothing else for them to do\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.IDLE,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BB7F RID: 47999
				public static LocString TOOLTIP_NORMAL = string.Concat(new string[]
				{
					"{Description}\n\nErrand Type: {Groups}\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • {Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n    • This {Building}'s Priority: {BuildingPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BB80 RID: 48000
				public static LocString TOOLTIP_PERSONAL = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is a ",
					UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS,
					" errand and so will be performed before all Regular errands\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.PERSONAL_NEEDS,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BB81 RID: 48001
				public static LocString TOOLTIP_EMERGENCY = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is an ",
					UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY,
					" errand and so will be performed before all Regular and Personal errands\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.EMERGENCY,
					" : {ClassPriority}\n    • This {Building}'s Priority: {BuildingPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BB82 RID: 48002
				public static LocString TOOLTIP_COMPULSORY = string.Concat(new string[]
				{
					"{Description}\n\n<b>{Errand}</b> is a ",
					UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY,
					" action and so will occur immediately\n\nTotal ",
					UI.PRE_KEYWORD,
					"Priority",
					UI.PST_KEYWORD,
					": {TotalPriority}\n    • ",
					UI.JOBSSCREEN.PRIORITY_CLASS.COMPULSORY,
					": {ClassPriority}\n    • All {BestGroup} Errands: {TypePriority}"
				});

				// Token: 0x0400BB83 RID: 48003
				public static LocString TOOLTIP_DESC_ACTIVE = "{Name}'s Current Errand: <b>{Errand}</b>";

				// Token: 0x0400BB84 RID: 48004
				public static LocString TOOLTIP_DESC_INACTIVE = "{Name} could work on <b>{Errand}</b>, but it's not their top priority right now";

				// Token: 0x0400BB85 RID: 48005
				public static LocString TOOLTIP_IDLEDESC_ACTIVE = "{Name} is currently <b>Idle</b>";

				// Token: 0x0400BB86 RID: 48006
				public static LocString TOOLTIP_IDLEDESC_INACTIVE = "{Name} could become <b>Idle</b> when all other errands are canceled or completed";

				// Token: 0x0400BB87 RID: 48007
				public static LocString TOOLTIP_NA = "--";

				// Token: 0x0400BB88 RID: 48008
				public static LocString CHORE_GROUP_SEPARATOR = " or ";
			}

			// Token: 0x02002B10 RID: 11024
			public class MODULEFLIGHTUTILITYSIDESCREEN
			{
				// Token: 0x0400BB89 RID: 48009
				public static LocString TITLE = "Deployables";

				// Token: 0x0400BB8A RID: 48010
				public static LocString DEPLOY_BUTTON = "Deploy";

				// Token: 0x0400BB8B RID: 48011
				public static LocString DEPLOY_BUTTON_TOOLTIP = "Send this module's contents to the surface of the currently orbited " + UI.CLUSTERMAP.PLANETOID_KEYWORD + "\n\nA specific deploy location may need to be chosen for certain modules";

				// Token: 0x0400BB8C RID: 48012
				public static LocString REPEAT_BUTTON_TOOLTIP = "Automatically deploy this module's contents when a destination orbit is reached";

				// Token: 0x0400BB8D RID: 48013
				public static LocString SELECT_DUPLICANT = "Select Duplicant";

				// Token: 0x0400BB8E RID: 48014
				public static LocString PILOT_FMT = "{0} - Pilot";
			}

			// Token: 0x02002B11 RID: 11025
			public class HIGHENERGYPARTICLEDIRECTIONSIDESCREEN
			{
				// Token: 0x0400BB8F RID: 48015
				public static LocString TITLE = "Emitting Particle Direction";

				// Token: 0x0400BB90 RID: 48016
				public static LocString SELECTED_DIRECTION = "Selected direction: {0}";

				// Token: 0x0400BB91 RID: 48017
				public static LocString DIRECTION_N = "N";

				// Token: 0x0400BB92 RID: 48018
				public static LocString DIRECTION_NE = "NE";

				// Token: 0x0400BB93 RID: 48019
				public static LocString DIRECTION_E = "E";

				// Token: 0x0400BB94 RID: 48020
				public static LocString DIRECTION_SE = "SE";

				// Token: 0x0400BB95 RID: 48021
				public static LocString DIRECTION_S = "S";

				// Token: 0x0400BB96 RID: 48022
				public static LocString DIRECTION_SW = "SW";

				// Token: 0x0400BB97 RID: 48023
				public static LocString DIRECTION_W = "W";

				// Token: 0x0400BB98 RID: 48024
				public static LocString DIRECTION_NW = "NW";
			}

			// Token: 0x02002B12 RID: 11026
			public class MONUMENTSIDESCREEN
			{
				// Token: 0x0400BB99 RID: 48025
				public static LocString TITLE = "Great Monument";

				// Token: 0x0400BB9A RID: 48026
				public static LocString FLIP_FACING_BUTTON = UI.CLICK(UI.ClickType.CLICK) + " TO ROTATE";
			}

			// Token: 0x02002B13 RID: 11027
			public class PLANTERSIDESCREEN
			{
				// Token: 0x0400BB9B RID: 48027
				public static LocString TITLE = "{0} Seeds";

				// Token: 0x0400BB9C RID: 48028
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400BB9D RID: 48029
				public static LocString AWAITINGREQUEST = "PLANT: {0}";

				// Token: 0x0400BB9E RID: 48030
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BB9F RID: 48031
				public static LocString AWAITINGREMOVAL = "AWAITING DIGGING UP: {0}";

				// Token: 0x0400BBA0 RID: 48032
				public static LocString ENTITYDEPOSITED = "PLANTED: {0}";

				// Token: 0x0400BBA1 RID: 48033
				public static LocString MUTATIONS_HEADER = "Mutations";

				// Token: 0x0400BBA2 RID: 48034
				public static LocString DEPOSIT = "Plant";

				// Token: 0x0400BBA3 RID: 48035
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400BBA4 RID: 48036
				public static LocString REMOVE = "Uproot";

				// Token: 0x0400BBA5 RID: 48037
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400BBA6 RID: 48038
				public static LocString SELECT_TITLE = "SELECT";

				// Token: 0x0400BBA7 RID: 48039
				public static LocString SELECT_DESC = "Select a seed to plant.";

				// Token: 0x0400BBA8 RID: 48040
				public static LocString LIFECYCLE = "<b>Life Cycle</b>:";

				// Token: 0x0400BBA9 RID: 48041
				public static LocString PLANTREQUIREMENTS = "<b>Growth Requirements</b>:";

				// Token: 0x0400BBAA RID: 48042
				public static LocString PLANTEFFECTS = "<b>Effects</b>:";

				// Token: 0x0400BBAB RID: 48043
				public static LocString NUMBEROFHARVESTS = "Harvests: {0}";

				// Token: 0x0400BBAC RID: 48044
				public static LocString YIELD = "{0}: {1} ";

				// Token: 0x0400BBAD RID: 48045
				public static LocString YIELD_NONFOOD = "{0}: {1} ";

				// Token: 0x0400BBAE RID: 48046
				public static LocString YIELD_SINGLE = "{0}";

				// Token: 0x0400BBAF RID: 48047
				public static LocString YIELDPERHARVEST = "{0} {1} per harvest";

				// Token: 0x0400BBB0 RID: 48048
				public static LocString TOTALHARVESTCALORIESWITHPERUNIT = "{0} [{1} / unit]";

				// Token: 0x0400BBB1 RID: 48049
				public static LocString TOTALHARVESTCALORIES = "{0}";

				// Token: 0x0400BBB2 RID: 48050
				public static LocString BONUS_SEEDS = "Base " + UI.FormatAsLink("Seed", "PLANTS") + " Harvest Chance: {0}";

				// Token: 0x0400BBB3 RID: 48051
				public static LocString YIELD_SEED = "{1} {0}";

				// Token: 0x0400BBB4 RID: 48052
				public static LocString YIELD_SEED_SINGLE = "{0}";

				// Token: 0x0400BBB5 RID: 48053
				public static LocString YIELD_SEED_FINAL_HARVEST = "{1} {0} - Final harvest only";

				// Token: 0x0400BBB6 RID: 48054
				public static LocString YIELD_SEED_SINGLE_FINAL_HARVEST = "{0} - Final harvest only";

				// Token: 0x0400BBB7 RID: 48055
				public static LocString ROTATION_NEED_FLOOR = "<b>Requires upward plot orientation.</b>";

				// Token: 0x0400BBB8 RID: 48056
				public static LocString ROTATION_NEED_WALL = "<b>Requires sideways plot orientation.</b>";

				// Token: 0x0400BBB9 RID: 48057
				public static LocString ROTATION_NEED_CEILING = "<b>Requires downward plot orientation.</b>";

				// Token: 0x0400BBBA RID: 48058
				public static LocString NO_SPECIES_SELECTED = "Select a seed species above...";

				// Token: 0x0400BBBB RID: 48059
				public static LocString DISEASE_DROPPER_BURST = "{Disease} Burst: {DiseaseAmount}";

				// Token: 0x0400BBBC RID: 48060
				public static LocString DISEASE_DROPPER_CONSTANT = "{Disease}: {DiseaseAmount}";

				// Token: 0x0400BBBD RID: 48061
				public static LocString DISEASE_ON_HARVEST = "{Disease} on crop: {DiseaseAmount}";

				// Token: 0x0400BBBE RID: 48062
				public static LocString AUTO_SELF_HARVEST = "Self-Harvest On Grown";

				// Token: 0x02003730 RID: 14128
				public class TOOLTIPS
				{
					// Token: 0x0400DBD7 RID: 56279
					public static LocString PLANTLIFECYCLE = "Duration and number of harvests produced by this plant in a lifetime";

					// Token: 0x0400DBD8 RID: 56280
					public static LocString PLANTREQUIREMENTS = "Minimum conditions for basic plant growth";

					// Token: 0x0400DBD9 RID: 56281
					public static LocString PLANTEFFECTS = "Additional attributes of this plant";

					// Token: 0x0400DBDA RID: 56282
					public static LocString YIELD = UI.FormatAsLink("{2}", "KCAL") + " produced [" + UI.FormatAsLink("{1}", "KCAL") + " / unit]";

					// Token: 0x0400DBDB RID: 56283
					public static LocString YIELD_NONFOOD = "{0} produced per harvest";

					// Token: 0x0400DBDC RID: 56284
					public static LocString NUMBEROFHARVESTS = "This plant can mature {0} times before the end of its life cycle";

					// Token: 0x0400DBDD RID: 56285
					public static LocString YIELD_SEED = "Sow to grow more of this plant";

					// Token: 0x0400DBDE RID: 56286
					public static LocString YIELD_SEED_FINAL_HARVEST = "{0}\n\nProduced in the final harvest of the plant's life cycle";

					// Token: 0x0400DBDF RID: 56287
					public static LocString BONUS_SEEDS = "This plant has a {0} chance to produce new seeds when harvested";

					// Token: 0x0400DBE0 RID: 56288
					public static LocString DISEASE_DROPPER_BURST = "At certain points in this plant's lifecycle, it will emit a burst of {DiseaseAmount} {Disease}.";

					// Token: 0x0400DBE1 RID: 56289
					public static LocString DISEASE_DROPPER_CONSTANT = "This plant emits {DiseaseAmount} {Disease} while it is alive.";

					// Token: 0x0400DBE2 RID: 56290
					public static LocString DISEASE_ON_HARVEST = "The {Crop} produced by this plant will have {DiseaseAmount} {Disease} on it.";

					// Token: 0x0400DBE3 RID: 56291
					public static LocString AUTO_SELF_HARVEST = "This plant will instantly drop its crop and begin regrowing when it is matured.";

					// Token: 0x0400DBE4 RID: 56292
					public static LocString PLANT_TOGGLE_TOOLTIP = "{0}\n\n{1}\n\n<b>{2}</b> seeds available";
				}
			}

			// Token: 0x02002B14 RID: 11028
			public class EGGINCUBATOR
			{
				// Token: 0x0400BBBF RID: 48063
				public static LocString TITLE = "Critter Eggs";

				// Token: 0x0400BBC0 RID: 48064
				public static LocString AWAITINGREQUEST = "INCUBATE: {0}";

				// Token: 0x0400BBC1 RID: 48065
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BBC2 RID: 48066
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400BBC3 RID: 48067
				public static LocString ENTITYDEPOSITED = "INCUBATING: {0}";

				// Token: 0x0400BBC4 RID: 48068
				public static LocString DEPOSIT = "Incubate";

				// Token: 0x0400BBC5 RID: 48069
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400BBC6 RID: 48070
				public static LocString REMOVE = "Remove";

				// Token: 0x0400BBC7 RID: 48071
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400BBC8 RID: 48072
				public static LocString SELECT_TITLE = "SELECT";

				// Token: 0x0400BBC9 RID: 48073
				public static LocString SELECT_DESC = "Select an egg to incubate.";
			}

			// Token: 0x02002B15 RID: 11029
			public class BASICRECEPTACLE
			{
				// Token: 0x0400BBCA RID: 48074
				public static LocString TITLE = "Displayed Object";

				// Token: 0x0400BBCB RID: 48075
				public static LocString AWAITINGREQUEST = "SELECT: {0}";

				// Token: 0x0400BBCC RID: 48076
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BBCD RID: 48077
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400BBCE RID: 48078
				public static LocString ENTITYDEPOSITED = "DISPLAYING: {0}";

				// Token: 0x0400BBCF RID: 48079
				public static LocString DEPOSIT = "Select";

				// Token: 0x0400BBD0 RID: 48080
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400BBD1 RID: 48081
				public static LocString REMOVE = "Remove";

				// Token: 0x0400BBD2 RID: 48082
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400BBD3 RID: 48083
				public static LocString SELECT_TITLE = "SELECT OBJECT";

				// Token: 0x0400BBD4 RID: 48084
				public static LocString SELECT_DESC = "Select an object to display here.";
			}

			// Token: 0x02002B16 RID: 11030
			public class SPECIALCARGOBAYCLUSTER
			{
				// Token: 0x0400BBD5 RID: 48085
				public static LocString TITLE = "Target Critter";

				// Token: 0x0400BBD6 RID: 48086
				public static LocString AWAITINGREQUEST = "SELECT: {0}";

				// Token: 0x0400BBD7 RID: 48087
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BBD8 RID: 48088
				public static LocString AWAITINGREMOVAL = "AWAITING REMOVAL: {0}";

				// Token: 0x0400BBD9 RID: 48089
				public static LocString ENTITYDEPOSITED = "CONTENTS: {0}";

				// Token: 0x0400BBDA RID: 48090
				public static LocString DEPOSIT = "Select";

				// Token: 0x0400BBDB RID: 48091
				public static LocString CANCELDEPOSIT = "Cancel";

				// Token: 0x0400BBDC RID: 48092
				public static LocString REMOVE = "Remove";

				// Token: 0x0400BBDD RID: 48093
				public static LocString CANCELREMOVAL = "Cancel";

				// Token: 0x0400BBDE RID: 48094
				public static LocString SELECT_TITLE = "SELECT CRITTER";

				// Token: 0x0400BBDF RID: 48095
				public static LocString SELECT_DESC = "Select a critter to store in this module.";
			}

			// Token: 0x02002B17 RID: 11031
			public class LURE
			{
				// Token: 0x0400BBE0 RID: 48096
				public static LocString TITLE = "Select Bait";

				// Token: 0x0400BBE1 RID: 48097
				public static LocString INFORMATION = "INFORMATION";

				// Token: 0x0400BBE2 RID: 48098
				public static LocString AWAITINGREQUEST = "PLANT: {0}";

				// Token: 0x0400BBE3 RID: 48099
				public static LocString AWAITINGDELIVERY = "AWAITING DELIVERY: {0}";

				// Token: 0x0400BBE4 RID: 48100
				public static LocString AWAITINGREMOVAL = "AWAITING DIGGING UP: {0}";

				// Token: 0x0400BBE5 RID: 48101
				public static LocString ENTITYDEPOSITED = "PLANTED: {0}";

				// Token: 0x0400BBE6 RID: 48102
				public static LocString ATTRACTS = "Attract {1}s";
			}

			// Token: 0x02002B18 RID: 11032
			public class ROLESTATION
			{
				// Token: 0x0400BBE7 RID: 48103
				public static LocString TITLE = "Duplicant Skills";

				// Token: 0x0400BBE8 RID: 48104
				public static LocString OPENROLESBUTTON = "SKILLS";
			}

			// Token: 0x02002B19 RID: 11033
			public class RESEARCHSIDESCREEN
			{
				// Token: 0x0400BBE9 RID: 48105
				public static LocString TITLE = "Select Research";

				// Token: 0x0400BBEA RID: 48106
				public static LocString CURRENTLYRESEARCHING = "Currently Researching";

				// Token: 0x0400BBEB RID: 48107
				public static LocString NOSELECTEDRESEARCH = "No Research selected";

				// Token: 0x0400BBEC RID: 48108
				public static LocString OPENRESEARCHBUTTON = "RESEARCH";
			}

			// Token: 0x02002B1A RID: 11034
			public class REFINERYSIDESCREEN
			{
				// Token: 0x0400BBED RID: 48109
				public static LocString RECIPE_FROM_TO = "{0} to {1}";

				// Token: 0x0400BBEE RID: 48110
				public static LocString RECIPE_WITH = "{1} ({0})";

				// Token: 0x0400BBEF RID: 48111
				public static LocString RECIPE_FROM_TO_WITH_NEWLINES = "{0}\nto\n{1}";

				// Token: 0x0400BBF0 RID: 48112
				public static LocString RECIPE_FROM_TO_COMPOSITE = "{0} to {1} and {2}";

				// Token: 0x0400BBF1 RID: 48113
				public static LocString RECIPE_FROM_TO_HEP = "{0} to " + UI.FormatAsLink("Radbolts", "RADIATION") + " and {1}";

				// Token: 0x0400BBF2 RID: 48114
				public static LocString RECIPE_SIMPLE_INCLUDE_AMOUNTS = "{0} {1}";

				// Token: 0x0400BBF3 RID: 48115
				public static LocString RECIPE_FROM_TO_INCLUDE_AMOUNTS = "{2} {0} to {3} {1}";

				// Token: 0x0400BBF4 RID: 48116
				public static LocString RECIPE_WITH_INCLUDE_AMOUNTS = "{3} {1} ({2} {0})";

				// Token: 0x0400BBF5 RID: 48117
				public static LocString RECIPE_FROM_TO_COMPOSITE_INCLUDE_AMOUNTS = "{3} {0} to {4} {1} and {5} {2}";

				// Token: 0x0400BBF6 RID: 48118
				public static LocString RECIPE_FROM_TO_HEP_INCLUDE_AMOUNTS = "{2} {0} to {3} " + UI.FormatAsLink("Radbolts", "RADIATION") + " and {4} {1}";
			}

			// Token: 0x02002B1B RID: 11035
			public class SEALEDDOORSIDESCREEN
			{
				// Token: 0x0400BBF7 RID: 48119
				public static LocString TITLE = "Sealed Door";

				// Token: 0x0400BBF8 RID: 48120
				public static LocString LABEL = "This door requires a sample to unlock.";

				// Token: 0x0400BBF9 RID: 48121
				public static LocString BUTTON = "SUBMIT BIOSCAN";

				// Token: 0x0400BBFA RID: 48122
				public static LocString AWAITINGBUTTON = "AWAITING BIOSCAN";
			}

			// Token: 0x02002B1C RID: 11036
			public class ENCRYPTEDLORESIDESCREEN
			{
				// Token: 0x0400BBFB RID: 48123
				public static LocString TITLE = "Encrypted File";

				// Token: 0x0400BBFC RID: 48124
				public static LocString LABEL = "This computer contains encrypted files.";

				// Token: 0x0400BBFD RID: 48125
				public static LocString BUTTON = "ATTEMPT DECRYPTION";

				// Token: 0x0400BBFE RID: 48126
				public static LocString AWAITINGBUTTON = "AWAITING DECRYPTION";
			}

			// Token: 0x02002B1D RID: 11037
			public class ACCESS_CONTROL_SIDE_SCREEN
			{
				// Token: 0x0400BBFF RID: 48127
				public static LocString TITLE = "Door Access Control";

				// Token: 0x0400BC00 RID: 48128
				public static LocString DOOR_DEFAULT = "Default";

				// Token: 0x0400BC01 RID: 48129
				public static LocString MINION_ACCESS = "Duplicant Access Permissions";

				// Token: 0x0400BC02 RID: 48130
				public static LocString GO_LEFT_ENABLED = "Passing Left through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400BC03 RID: 48131
				public static LocString GO_LEFT_DISABLED = "Passing Left through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400BC04 RID: 48132
				public static LocString GO_RIGHT_ENABLED = "Passing Right through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400BC05 RID: 48133
				public static LocString GO_RIGHT_DISABLED = "Passing Right through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400BC06 RID: 48134
				public static LocString GO_UP_ENABLED = "Passing Up through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400BC07 RID: 48135
				public static LocString GO_UP_DISABLED = "Passing Up through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400BC08 RID: 48136
				public static LocString GO_DOWN_ENABLED = "Passing Down through this door is permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to revoke permission";

				// Token: 0x0400BC09 RID: 48137
				public static LocString GO_DOWN_DISABLED = "Passing Down through this door is not permitted\n\n" + UI.CLICK(UI.ClickType.Click) + " to grant permission";

				// Token: 0x0400BC0A RID: 48138
				public static LocString SET_TO_DEFAULT = UI.CLICK(UI.ClickType.Click) + " to clear custom permissions";

				// Token: 0x0400BC0B RID: 48139
				public static LocString SET_TO_CUSTOM = UI.CLICK(UI.ClickType.Click) + " to assign custom permissions";

				// Token: 0x0400BC0C RID: 48140
				public static LocString USING_DEFAULT = "Default Access";

				// Token: 0x0400BC0D RID: 48141
				public static LocString USING_CUSTOM = "Custom Access";
			}

			// Token: 0x02002B1E RID: 11038
			public class OWNABLESSIDESCREEN
			{
				// Token: 0x0400BC0E RID: 48142
				public static LocString TITLE = "Equipment and Amenities";

				// Token: 0x0400BC0F RID: 48143
				public static LocString NO_ITEM_ASSIGNED = "Assign";

				// Token: 0x0400BC10 RID: 48144
				public static LocString NO_ITEM_FOUND = "None found";

				// Token: 0x0400BC11 RID: 48145
				public static LocString NO_APPLICABLE = "{0}: Ineligible";

				// Token: 0x02003731 RID: 14129
				public static class TOOLTIPS
				{
					// Token: 0x0400DBE5 RID: 56293
					public static LocString NO_APPLICABLE = "This Duplicant cannot be assigned " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

					// Token: 0x0400DBE6 RID: 56294
					public static LocString NO_ITEM_ASSIGNED = string.Concat(new string[]
					{
						"Click to view and assign existing ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" to this Duplicant"
					});

					// Token: 0x0400DBE7 RID: 56295
					public static LocString ITEM_ASSIGNED_GENERIC = "This Duplicant has {0} assigned to them";

					// Token: 0x0400DBE8 RID: 56296
					public static LocString ITEM_ASSIGNED = "{0}\n\n{1}";
				}

				// Token: 0x02003732 RID: 14130
				public class CATEGORIES
				{
					// Token: 0x0400DBE9 RID: 56297
					public static LocString SUITS = "Suits";

					// Token: 0x0400DBEA RID: 56298
					public static LocString AMENITIES = "Amenities";
				}
			}

			// Token: 0x02002B1F RID: 11039
			public class OWNABLESSECONDSIDESCREEN
			{
				// Token: 0x0400BC12 RID: 48146
				public static LocString TITLE = "{0}";

				// Token: 0x0400BC13 RID: 48147
				public static LocString NONE_ROW_LABEL = "Unequip";

				// Token: 0x0400BC14 RID: 48148
				public static LocString NONE_ROW_TOOLTIP = "Click to remove any item currently assigned to the selected slot";

				// Token: 0x0400BC15 RID: 48149
				public static LocString ASSIGNED_TO_OTHER_STATUS = "Assigned to: {0}";

				// Token: 0x0400BC16 RID: 48150
				public static LocString ASSIGNED_TO_SELF_STATUS = "Assigned";

				// Token: 0x0400BC17 RID: 48151
				public static LocString NOT_ASSIGNED = "Unassigned";
			}

			// Token: 0x02002B20 RID: 11040
			public class ASSIGNABLESIDESCREEN
			{
				// Token: 0x0400BC18 RID: 48152
				public static LocString TITLE = "Assign {0}";

				// Token: 0x0400BC19 RID: 48153
				public static LocString ASSIGNED = "Assigned";

				// Token: 0x0400BC1A RID: 48154
				public static LocString UNASSIGNED = "-";

				// Token: 0x0400BC1B RID: 48155
				public static LocString DISABLED = "Ineligible";

				// Token: 0x0400BC1C RID: 48156
				public static LocString SORT_BY_DUPLICANT = "Duplicant";

				// Token: 0x0400BC1D RID: 48157
				public static LocString SORT_BY_ASSIGNMENT = "Assignment";

				// Token: 0x0400BC1E RID: 48158
				public static LocString ASSIGN_TO_TOOLTIP = "Assign to {0}";

				// Token: 0x0400BC1F RID: 48159
				public static LocString UNASSIGN_TOOLTIP = "Assigned to {0}";

				// Token: 0x0400BC20 RID: 48160
				public static LocString DISABLED_TOOLTIP = "{0} is ineligible for this skill assignment";

				// Token: 0x0400BC21 RID: 48161
				public static LocString PUBLIC = "Public";
			}

			// Token: 0x02002B21 RID: 11041
			public class COMETDETECTORSIDESCREEN
			{
				// Token: 0x0400BC22 RID: 48162
				public static LocString TITLE = "Space Scanner";

				// Token: 0x0400BC23 RID: 48163
				public static LocString HEADER = "Sends automation signal when selected object is detected";

				// Token: 0x0400BC24 RID: 48164
				public static LocString ASSIGNED = "Assigned";

				// Token: 0x0400BC25 RID: 48165
				public static LocString UNASSIGNED = "-";

				// Token: 0x0400BC26 RID: 48166
				public static LocString DISABLED = "Ineligible";

				// Token: 0x0400BC27 RID: 48167
				public static LocString SORT_BY_DUPLICANT = "Duplicant";

				// Token: 0x0400BC28 RID: 48168
				public static LocString SORT_BY_ASSIGNMENT = "Assignment";

				// Token: 0x0400BC29 RID: 48169
				public static LocString ASSIGN_TO_TOOLTIP = "Scanning for {0}";

				// Token: 0x0400BC2A RID: 48170
				public static LocString UNASSIGN_TOOLTIP = "Scanning for {0}";

				// Token: 0x0400BC2B RID: 48171
				public static LocString NOTHING = "Nothing";

				// Token: 0x0400BC2C RID: 48172
				public static LocString COMETS = "Meteor Showers";

				// Token: 0x0400BC2D RID: 48173
				public static LocString ROCKETS = "Rocket Landing Ping";

				// Token: 0x0400BC2E RID: 48174
				public static LocString DUPEMADE = "Interplanetary Payloads";
			}

			// Token: 0x02002B22 RID: 11042
			public class GEOTUNERSIDESCREEN
			{
				// Token: 0x0400BC2F RID: 48175
				public static LocString TITLE = "Select Geyser";

				// Token: 0x0400BC30 RID: 48176
				public static LocString DESCRIPTION = "Select an analyzed geyser to transmit amplification data to.";

				// Token: 0x0400BC31 RID: 48177
				public static LocString NOTHING = "No geyser selected";

				// Token: 0x0400BC32 RID: 48178
				public static LocString UNSTUDIED_TOOLTIP = "This geyser must be analyzed before it can be selected\n\nDouble-click to view this geyser";

				// Token: 0x0400BC33 RID: 48179
				public static LocString STUDIED_TOOLTIP = string.Concat(new string[]
				{
					"Increase this geyser's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" and output"
				});

				// Token: 0x0400BC34 RID: 48180
				public static LocString GEOTUNER_LIMIT_TOOLTIP = "This geyser cannot be targeted by more " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;

				// Token: 0x0400BC35 RID: 48181
				public static LocString STUDIED_TOOLTIP_MATERIAL = "Required resource: {MATERIAL}";

				// Token: 0x0400BC36 RID: 48182
				public static LocString STUDIED_TOOLTIP_POTENTIAL_OUTPUT = "Potential Output {POTENTIAL_OUTPUT}";

				// Token: 0x0400BC37 RID: 48183
				public static LocString STUDIED_TOOLTIP_BASE_TEMP = "Base {BASE}";

				// Token: 0x0400BC38 RID: 48184
				public static LocString STUDIED_TOOLTIP_VISIT_GEYSER = "Double-click to view this geyser";

				// Token: 0x0400BC39 RID: 48185
				public static LocString STUDIED_TOOLTIP_GEOTUNER_MODIFIER_ROW_TITLE = "Geotuned ";

				// Token: 0x0400BC3A RID: 48186
				public static LocString STUDIED_TOOLTIP_NUMBER_HOVERED = "This geyser is targeted by {0} Geotuners";
			}

			// Token: 0x02002B23 RID: 11043
			public class REMOTE_WORK_TERMINAL_SIDE_SCREEN
			{
				// Token: 0x0400BC3B RID: 48187
				public static LocString DOCK_TOOLTIP = "Click to assign this dock to this controller\n\nDouble-click to view this dock";
			}

			// Token: 0x02002B24 RID: 11044
			public class COMMAND_MODULE_SIDE_SCREEN
			{
				// Token: 0x0400BC3C RID: 48188
				public static LocString TITLE = "Launch Conditions";

				// Token: 0x0400BC3D RID: 48189
				public static LocString DESTINATION_BUTTON = "Show Starmap";

				// Token: 0x0400BC3E RID: 48190
				public static LocString DESTINATION_BUTTON_EXPANSION = "Show Starmap";
			}

			// Token: 0x02002B25 RID: 11045
			public class CLUSTERDESTINATIONSIDESCREEN
			{
				// Token: 0x0400BC3F RID: 48191
				public static LocString TITLE = "Destination";

				// Token: 0x0400BC40 RID: 48192
				public static LocString FIRSTAVAILABLE = "Any " + BUILDINGS.PREFABS.LAUNCHPAD.NAME;

				// Token: 0x0400BC41 RID: 48193
				public static LocString NONEAVAILABLE = "No landing site";

				// Token: 0x0400BC42 RID: 48194
				public static LocString NO_TALL_SITES_AVAILABLE = "No landing sites fit the height of this rocket";

				// Token: 0x0400BC43 RID: 48195
				public static LocString DROPDOWN_TOOLTIP_VALID_SITE = "Land at {0} when the site is clear";

				// Token: 0x0400BC44 RID: 48196
				public static LocString DROPDOWN_TOOLTIP_FIRST_AVAILABLE = "Select the first available landing site";

				// Token: 0x0400BC45 RID: 48197
				public static LocString DROPDOWN_TOOLTIP_TOO_SHORT = "This rocket's height exceeds the space available in this landing site";

				// Token: 0x0400BC46 RID: 48198
				public static LocString DROPDOWN_TOOLTIP_PATH_OBSTRUCTED = "Landing path obstructed";

				// Token: 0x0400BC47 RID: 48199
				public static LocString DROPDOWN_TOOLTIP_SITE_OBSTRUCTED = "Landing position on the platform is obstructed";

				// Token: 0x0400BC48 RID: 48200
				public static LocString DROPDOWN_TOOLTIP_PAD_DISABLED = BUILDINGS.PREFABS.LAUNCHPAD.NAME + " is disabled";

				// Token: 0x0400BC49 RID: 48201
				public static LocString CHANGE_DESTINATION_BUTTON = "Change";

				// Token: 0x0400BC4A RID: 48202
				public static LocString CHANGE_DESTINATION_BUTTON_TOOLTIP = "Select a new destination for this rocket";

				// Token: 0x0400BC4B RID: 48203
				public static LocString CLEAR_DESTINATION_BUTTON = "Clear";

				// Token: 0x0400BC4C RID: 48204
				public static LocString CLEAR_DESTINATION_BUTTON_TOOLTIP = "Clear this rocket's selected destination";

				// Token: 0x0400BC4D RID: 48205
				public static LocString LOOP_BUTTON_TOOLTIP = "Toggle a roundtrip flight between this rocket's destination and its original takeoff location";

				// Token: 0x02003733 RID: 14131
				public class ASSIGNMENTSTATUS
				{
					// Token: 0x0400DBEB RID: 56299
					public static LocString LOCAL = "Current";

					// Token: 0x0400DBEC RID: 56300
					public static LocString DESTINATION = "Destination";
				}
			}

			// Token: 0x02002B26 RID: 11046
			public class EQUIPPABLESIDESCREEN
			{
				// Token: 0x0400BC4E RID: 48206
				public static LocString TITLE = "Equip {0}";

				// Token: 0x0400BC4F RID: 48207
				public static LocString ASSIGNEDTO = "Assigned to: {Assignee}";

				// Token: 0x0400BC50 RID: 48208
				public static LocString UNASSIGNED = "Unassigned";

				// Token: 0x0400BC51 RID: 48209
				public static LocString GENERAL_CURRENTASSIGNED = "(Owner)";
			}

			// Token: 0x02002B27 RID: 11047
			public class EQUIPPABLE_SIDE_SCREEN
			{
				// Token: 0x0400BC52 RID: 48210
				public static LocString TITLE = "Assign To Duplicant";

				// Token: 0x0400BC53 RID: 48211
				public static LocString CURRENTLY_EQUIPPED = "Currently Equipped:\n{0}";

				// Token: 0x0400BC54 RID: 48212
				public static LocString NONE_EQUIPPED = "None";

				// Token: 0x0400BC55 RID: 48213
				public static LocString EQUIP_BUTTON = "Equip";

				// Token: 0x0400BC56 RID: 48214
				public static LocString DROP_BUTTON = "Drop";

				// Token: 0x0400BC57 RID: 48215
				public static LocString SWAP_BUTTON = "Swap";
			}

			// Token: 0x02002B28 RID: 11048
			public class TELEPADSIDESCREEN
			{
				// Token: 0x0400BC58 RID: 48216
				public static LocString TITLE = "Printables";

				// Token: 0x0400BC59 RID: 48217
				public static LocString NEXTPRODUCTION = "Next Production: {0}";

				// Token: 0x0400BC5A RID: 48218
				public static LocString GAMEOVER = "Colony Lost";

				// Token: 0x0400BC5B RID: 48219
				public static LocString VICTORY_CONDITIONS = "Hardwired Imperatives";

				// Token: 0x0400BC5C RID: 48220
				public static LocString SUMMARY_TITLE = "Colony Summary";

				// Token: 0x0400BC5D RID: 48221
				public static LocString SKILLS_BUTTON = "Duplicant Skills";
			}

			// Token: 0x02002B29 RID: 11049
			public class VALVESIDESCREEN
			{
				// Token: 0x0400BC5E RID: 48222
				public static LocString TITLE = "Flow Control";
			}

			// Token: 0x02002B2A RID: 11050
			public class BIONIC_SIDE_SCREEN
			{
				// Token: 0x0400BC5F RID: 48223
				public static LocString TITLE = "Boosters";

				// Token: 0x0400BC60 RID: 48224
				public static LocString UPGRADE_SLOT_EMPTY = "Empty";

				// Token: 0x0400BC61 RID: 48225
				public static LocString UPGRADE_SLOT_ASSIGNED = "Assigned";

				// Token: 0x0400BC62 RID: 48226
				public static LocString UPGRADE_SLOT_WATTAGE = "{0}";

				// Token: 0x0400BC63 RID: 48227
				public static LocString CURRENT_WATTAGE_LABEL = "Current Wattage: <b>{0}</b>";

				// Token: 0x0400BC64 RID: 48228
				public static LocString CURRENT_WATTAGE_LABEL_BATTERY_SAVE_MODE = "Current Wattage: <color=#0303fc><b>{0}</b> {1}</color>";

				// Token: 0x0400BC65 RID: 48229
				public static LocString CURRENT_WATTAGE_LABEL_OFFLINE = "Current Wattage: <color=#GG2222>Offline {0}</color>";

				// Token: 0x0400BC66 RID: 48230
				public const string OFFLINE_MODE_COLOR = "<color=#GG2222>";

				// Token: 0x0400BC67 RID: 48231
				public const string BATTERY_SAVE_MODE_COLOR = "<color=#0303fc>";

				// Token: 0x0400BC68 RID: 48232
				public const string COLOR_END = "</color>";

				// Token: 0x02003734 RID: 14132
				public class TOOLTIP
				{
					// Token: 0x0400DBED RID: 56301
					public static LocString CURRENT_WATTAGE = "Wattage is the amount of energy that this Duplicant's bionic parts consume per second\n\nInstalled boosters consume wattage while active";

					// Token: 0x0400DBEE RID: 56302
					public static LocString SLOT_EMPTY = "No booster installed\n\nClick to view available boosters";

					// Token: 0x0400DBEF RID: 56303
					public static LocString SLOT_ASSIGNED = string.Concat(new string[]
					{
						"This ",
						UI.PRE_KEYWORD,
						"{0}",
						UI.PST_KEYWORD,
						" will be installed when it is within this Duplicant's reach"
					});

					// Token: 0x0400DBF0 RID: 56304
					public static LocString SLOT_INSTALLED_IN_USE = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " installed\n\nStatus: Active\n\nWattage: {1}\n\n{2}";

					// Token: 0x0400DBF1 RID: 56305
					public static LocString SLOT_INSTALLED_NOT_IN_USE = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " installed\n\nStatus: Idle\n\nPotential Wattage: {1}\n\n{2}";
				}
			}

			// Token: 0x02002B2B RID: 11051
			public class LIMIT_VALVE_SIDE_SCREEN
			{
				// Token: 0x0400BC69 RID: 48233
				public static LocString TITLE = "Meter Control";

				// Token: 0x0400BC6A RID: 48234
				public static LocString AMOUNT = "Amount: {0}";

				// Token: 0x0400BC6B RID: 48235
				public static LocString LIMIT = "Limit:";

				// Token: 0x0400BC6C RID: 48236
				public static LocString RESET_BUTTON = "Reset Amount";

				// Token: 0x0400BC6D RID: 48237
				public static LocString SLIDER_TOOLTIP_UNITS = "The amount of Units or Mass passing through the sensor.";
			}

			// Token: 0x02002B2C RID: 11052
			public class NUCLEAR_REACTOR_SIDE_SCREEN
			{
				// Token: 0x0400BC6E RID: 48238
				public static LocString TITLE = "Reaction Mass Target";

				// Token: 0x0400BC6F RID: 48239
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will attempt to keep the reactor supplied with ",
					UI.PRE_KEYWORD,
					"{0}{1}",
					UI.PST_KEYWORD,
					" of ",
					UI.PRE_KEYWORD,
					"{2}",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002B2D RID: 11053
			public class MANUALGENERATORSIDESCREEN
			{
				// Token: 0x0400BC70 RID: 48240
				public static LocString TITLE = "Battery Recharge Threshold";

				// Token: 0x0400BC71 RID: 48241
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400BC72 RID: 48242
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will be requested to operate this generator when the total charge of the connected ",
					UI.PRE_KEYWORD,
					"Batteries",
					UI.PST_KEYWORD,
					" falls below <b>{0}%</b>"
				});
			}

			// Token: 0x02002B2E RID: 11054
			public class SPACEHEATERSIDESCREEN
			{
				// Token: 0x0400BC73 RID: 48243
				public static LocString TITLE = "Power Consumption";

				// Token: 0x0400BC74 RID: 48244
				public static LocString CURRENT_THRESHOLD = "Current Power Consumption: {0}";

				// Token: 0x0400BC75 RID: 48245
				public static LocString TOOLTIP = "Adjust power consumption to determine how much heat is produced\n\nCurrent heat production: <b>{0}</b>";
			}

			// Token: 0x02002B2F RID: 11055
			public class MANUALDELIVERYGENERATORSIDESCREEN
			{
				// Token: 0x0400BC76 RID: 48246
				public static LocString TITLE = "Fuel Request Threshold";

				// Token: 0x0400BC77 RID: 48247
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400BC78 RID: 48248
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will be requested to deliver ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when the total charge of the connected ",
					UI.PRE_KEYWORD,
					"Batteries",
					UI.PST_KEYWORD,
					" falls below <b>{1}%</b>"
				});
			}

			// Token: 0x02002B30 RID: 11056
			public class TIME_OF_DAY_SIDE_SCREEN
			{
				// Token: 0x0400BC79 RID: 48249
				public static LocString TITLE = "Time-of-Day Sensor";

				// Token: 0x0400BC7A RID: 48250
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" after the selected Turn On time, and a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" after the selected Turn Off time"
				});

				// Token: 0x0400BC7B RID: 48251
				public static LocString START = "Turn On";

				// Token: 0x0400BC7C RID: 48252
				public static LocString STOP = "Turn Off";
			}

			// Token: 0x02002B31 RID: 11057
			public class CRITTER_COUNT_SIDE_SCREEN
			{
				// Token: 0x0400BC7D RID: 48253
				public static LocString TITLE = "Critter Count Sensor";

				// Token: 0x0400BC7E RID: 48254
				public static LocString TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if there are more than <b>{0}</b> ",
					UI.PRE_KEYWORD,
					"Critters",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" in the room"
				});

				// Token: 0x0400BC7F RID: 48255
				public static LocString TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if there are fewer than <b>{0}</b> ",
					UI.PRE_KEYWORD,
					"Critters",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" in the room"
				});

				// Token: 0x0400BC80 RID: 48256
				public static LocString START = "Turn On";

				// Token: 0x0400BC81 RID: 48257
				public static LocString STOP = "Turn Off";

				// Token: 0x0400BC82 RID: 48258
				public static LocString VALUE_NAME = "Count";
			}

			// Token: 0x02002B32 RID: 11058
			public class OIL_WELL_CAP_SIDE_SCREEN
			{
				// Token: 0x0400BC83 RID: 48259
				public static LocString TITLE = "Backpressure Release Threshold";

				// Token: 0x0400BC84 RID: 48260
				public static LocString TOOLTIP = "Duplicants will be requested to release backpressure buildup when it exceeds <b>{0}%</b>";
			}

			// Token: 0x02002B33 RID: 11059
			public class MODULAR_CONDUIT_PORT_SIDE_SCREEN
			{
				// Token: 0x0400BC85 RID: 48261
				public static LocString TITLE = "Pump Control";

				// Token: 0x0400BC86 RID: 48262
				public static LocString LABEL_UNLOAD = "Unload Only";

				// Token: 0x0400BC87 RID: 48263
				public static LocString LABEL_BOTH = "Load/Unload";

				// Token: 0x0400BC88 RID: 48264
				public static LocString LABEL_LOAD = "Load Only";

				// Token: 0x0400BC89 RID: 48265
				public static readonly List<LocString> LABELS = new List<LocString>
				{
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_UNLOAD,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_BOTH,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.LABEL_LOAD
				};

				// Token: 0x0400BC8A RID: 48266
				public static LocString TOOLTIP_UNLOAD = "This pump will attempt to <b>Unload</b> cargo from the landed rocket, but not attempt to load new cargo";

				// Token: 0x0400BC8B RID: 48267
				public static LocString TOOLTIP_BOTH = "This pump will both <b>Load</b> and <b>Unload</b> cargo from the landed rocket";

				// Token: 0x0400BC8C RID: 48268
				public static LocString TOOLTIP_LOAD = "This pump will attempt to <b>Load</b> cargo onto the landed rocket, but will not unload it";

				// Token: 0x0400BC8D RID: 48269
				public static readonly List<LocString> TOOLTIPS = new List<LocString>
				{
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_UNLOAD,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_BOTH,
					UI.UISIDESCREENS.MODULAR_CONDUIT_PORT_SIDE_SCREEN.TOOLTIP_LOAD
				};

				// Token: 0x0400BC8E RID: 48270
				public static LocString DESCRIPTION = "";
			}

			// Token: 0x02002B34 RID: 11060
			public class LOGIC_BUFFER_SIDE_SCREEN
			{
				// Token: 0x0400BC8F RID: 48271
				public static LocString TITLE = "Buffer Time";

				// Token: 0x0400BC90 RID: 48272
				public static LocString TOOLTIP = "Will continue to send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for <b>{0} seconds</b> after receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002B35 RID: 11061
			public class LOGIC_FILTER_SIDE_SCREEN
			{
				// Token: 0x0400BC91 RID: 48273
				public static LocString TITLE = "Filter Time";

				// Token: 0x0400BC92 RID: 48274
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Will only send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if it receives ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" for longer than <b>{0} seconds</b>"
				});
			}

			// Token: 0x02002B36 RID: 11062
			public class TIME_RANGE_SIDE_SCREEN
			{
				// Token: 0x0400BC93 RID: 48275
				public static LocString TITLE = "Time Schedule";

				// Token: 0x0400BC94 RID: 48276
				public static LocString ON = "Activation Time";

				// Token: 0x0400BC95 RID: 48277
				public static LocString ON_TOOLTIP = string.Concat(new string[]
				{
					"Activation time determines the time of day this sensor should begin sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" {0} through the day"
				});

				// Token: 0x0400BC96 RID: 48278
				public static LocString DURATION = "Active Duration";

				// Token: 0x0400BC97 RID: 48279
				public static LocString DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Active duration determines what percentage of the day this sensor will spend sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for {0} of the day"
				});
			}

			// Token: 0x02002B37 RID: 11063
			public class TIMER_SIDE_SCREEN
			{
				// Token: 0x0400BC98 RID: 48280
				public static LocString TITLE = "Timer";

				// Token: 0x0400BC99 RID: 48281
				public static LocString ON = "Green Duration";

				// Token: 0x0400BC9A RID: 48282
				public static LocString GREEN_DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Green duration determines the amount of time this sensor should send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"\n\nThis sensor sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for {0}"
				});

				// Token: 0x0400BC9B RID: 48283
				public static LocString OFF = "Red Duration";

				// Token: 0x0400BC9C RID: 48284
				public static LocString RED_DURATION_TOOLTIP = string.Concat(new string[]
				{
					"Red duration determines the amount of time this sensor should send a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"\n\nThis sensor will send a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" for {0}"
				});

				// Token: 0x0400BC9D RID: 48285
				public static LocString CURRENT_TIME = "{0}/{1}";

				// Token: 0x0400BC9E RID: 48286
				public static LocString MODE_LABEL_SECONDS = "Mode: Seconds";

				// Token: 0x0400BC9F RID: 48287
				public static LocString MODE_LABEL_CYCLES = "Mode: Cycles";

				// Token: 0x0400BCA0 RID: 48288
				public static LocString RESET_BUTTON = "Reset Timer";

				// Token: 0x0400BCA1 RID: 48289
				public static LocString DISABLED = "Timer Disabled";
			}

			// Token: 0x02002B38 RID: 11064
			public class COUNTER_SIDE_SCREEN
			{
				// Token: 0x0400BCA2 RID: 48290
				public static LocString TITLE = "Counter";

				// Token: 0x0400BCA3 RID: 48291
				public static LocString RESET_BUTTON = "Reset Counter";

				// Token: 0x0400BCA4 RID: 48292
				public static LocString DESCRIPTION = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when count is reached:";

				// Token: 0x0400BCA5 RID: 48293
				public static LocString INCREMENT_MODE = "Mode: Increment";

				// Token: 0x0400BCA6 RID: 48294
				public static LocString DECREMENT_MODE = "Mode: Decrement";

				// Token: 0x0400BCA7 RID: 48295
				public static LocString ADVANCED_MODE = "Advanced Mode";

				// Token: 0x0400BCA8 RID: 48296
				public static LocString CURRENT_COUNT_SIMPLE = "{0} of ";

				// Token: 0x0400BCA9 RID: 48297
				public static LocString CURRENT_COUNT_ADVANCED = "{0} % ";

				// Token: 0x02003735 RID: 14133
				public class TOOLTIPS
				{
					// Token: 0x0400DBF2 RID: 56306
					public static LocString ADVANCED_MODE = string.Concat(new string[]
					{
						"In Advanced Mode, the ",
						BUILDINGS.PREFABS.LOGICCOUNTER.NAME,
						" will count from <b>0</b> rather than <b>1</b>. It will reset when the max is reached, and send a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						" as a brief pulse rather than continuously."
					});
				}
			}

			// Token: 0x02002B39 RID: 11065
			public class PASSENGERMODULESIDESCREEN
			{
				// Token: 0x0400BCAA RID: 48298
				public static LocString REQUEST_CREW = "Crew";

				// Token: 0x0400BCAB RID: 48299
				public static LocString REQUEST_CREW_TOOLTIP = "Crew may not leave the module, non crew-must exit";

				// Token: 0x0400BCAC RID: 48300
				public static LocString AUTO_CREW = "Auto";

				// Token: 0x0400BCAD RID: 48301
				public static LocString AUTO_CREW_TOOLTIP = "All Duplicants may enter and exit the module freely until the rocket is ready for launch\n\nBefore launch the crew will automatically be requested";

				// Token: 0x0400BCAE RID: 48302
				public static LocString RELEASE_CREW = "All";

				// Token: 0x0400BCAF RID: 48303
				public static LocString RELEASE_CREW_TOOLTIP = "All Duplicants may enter and exit the module freely";

				// Token: 0x0400BCB0 RID: 48304
				public static LocString REQUIRE_SUIT_LABEL = "Atmosuit Required";

				// Token: 0x0400BCB1 RID: 48305
				public static LocString REQUIRE_SUIT_LABEL_TOOLTIP = "If checked, Duplicants will be required to wear an Atmo Suit when entering this rocket";

				// Token: 0x0400BCB2 RID: 48306
				public static LocString CHANGE_CREW_BUTTON = "Change crew";

				// Token: 0x0400BCB3 RID: 48307
				public static LocString CHANGE_CREW_BUTTON_TOOLTIP = "Assign Duplicants to crew this rocket's missions";

				// Token: 0x0400BCB4 RID: 48308
				public static LocString ASSIGNED_TO_CREW = "Assigned to crew";

				// Token: 0x0400BCB5 RID: 48309
				public static LocString UNASSIGNED = "Unassigned";
			}

			// Token: 0x02002B3A RID: 11066
			public class TIMEDSWITCHSIDESCREEN
			{
				// Token: 0x0400BCB6 RID: 48310
				public static LocString TITLE = "Time Schedule";

				// Token: 0x0400BCB7 RID: 48311
				public static LocString ONTIME = "On Time:";

				// Token: 0x0400BCB8 RID: 48312
				public static LocString OFFTIME = "Off Time:";

				// Token: 0x0400BCB9 RID: 48313
				public static LocString TIMETODEACTIVATE = "Time until deactivation: {0}";

				// Token: 0x0400BCBA RID: 48314
				public static LocString TIMETOACTIVATE = "Time until activation: {0}";

				// Token: 0x0400BCBB RID: 48315
				public static LocString WARNING = "Switch must be connected to a " + UI.FormatAsLink("Power", "POWER") + " grid";

				// Token: 0x0400BCBC RID: 48316
				public static LocString CURRENTSTATE = "Current State:";

				// Token: 0x0400BCBD RID: 48317
				public static LocString ON = "On";

				// Token: 0x0400BCBE RID: 48318
				public static LocString OFF = "Off";
			}

			// Token: 0x02002B3B RID: 11067
			public class CAPTURE_POINT_SIDE_SCREEN
			{
				// Token: 0x0400BCBF RID: 48319
				public static LocString TITLE = "Stable Management";

				// Token: 0x0400BCC0 RID: 48320
				public static LocString AUTOWRANGLE = "Auto-Wrangle Surplus";

				// Token: 0x0400BCC1 RID: 48321
				public static LocString AUTOWRANGLE_TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant will automatically wrangle any critters that exceed the population limit or that do not belong in this stable\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill in order to wrangle critters"
				});

				// Token: 0x0400BCC2 RID: 48322
				public static LocString LIMIT_TOOLTIP = "Critters exceeding this population limit will automatically be wrangled:";

				// Token: 0x0400BCC3 RID: 48323
				public static LocString UNITS_SUFFIX = " Critters";
			}

			// Token: 0x02002B3C RID: 11068
			public class TEMPERATURESWITCHSIDESCREEN
			{
				// Token: 0x0400BCC4 RID: 48324
				public static LocString TITLE = "Temperature Threshold";

				// Token: 0x0400BCC5 RID: 48325
				public static LocString CURRENT_TEMPERATURE = "Current Temperature:\n{0}";

				// Token: 0x0400BCC6 RID: 48326
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BCC7 RID: 48327
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400BCC8 RID: 48328
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002B3D RID: 11069
			public class BRIGHTNESSSWITCHSIDESCREEN
			{
				// Token: 0x0400BCC9 RID: 48329
				public static LocString TITLE = "Brightness Threshold";

				// Token: 0x0400BCCA RID: 48330
				public static LocString CURRENT_TEMPERATURE = "Current Brightness:\n{0}";

				// Token: 0x0400BCCB RID: 48331
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BCCC RID: 48332
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400BCCD RID: 48333
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002B3E RID: 11070
			public class RADIATIONSWITCHSIDESCREEN
			{
				// Token: 0x0400BCCE RID: 48334
				public static LocString TITLE = "Radiation Threshold";

				// Token: 0x0400BCCF RID: 48335
				public static LocString CURRENT_TEMPERATURE = "Current Radiation:\n{0}/cycle";

				// Token: 0x0400BCD0 RID: 48336
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BCD1 RID: 48337
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400BCD2 RID: 48338
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002B3F RID: 11071
			public class WATTAGESWITCHSIDESCREEN
			{
				// Token: 0x0400BCD3 RID: 48339
				public static LocString TITLE = "Wattage Threshold";

				// Token: 0x0400BCD4 RID: 48340
				public static LocString CURRENT_TEMPERATURE = "Current Wattage:\n{0}";

				// Token: 0x0400BCD5 RID: 48341
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BCD6 RID: 48342
				public static LocString COLDER_BUTTON = "Below";

				// Token: 0x0400BCD7 RID: 48343
				public static LocString WARMER_BUTTON = "Above";
			}

			// Token: 0x02002B40 RID: 11072
			public class HEPSWITCHSIDESCREEN
			{
				// Token: 0x0400BCD8 RID: 48344
				public static LocString TITLE = "Radbolt Threshold";
			}

			// Token: 0x02002B41 RID: 11073
			public class THRESHOLD_SWITCH_SIDESCREEN
			{
				// Token: 0x0400BCD9 RID: 48345
				public static LocString TITLE = "Pressure";

				// Token: 0x0400BCDA RID: 48346
				public static LocString THRESHOLD_SUBTITLE = "Threshold:";

				// Token: 0x0400BCDB RID: 48347
				public static LocString CURRENT_VALUE = "Current {0}:\n{1}";

				// Token: 0x0400BCDC RID: 48348
				public static LocString ACTIVATE_IF = "Send " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if:";

				// Token: 0x0400BCDD RID: 48349
				public static LocString ABOVE_BUTTON = "Above";

				// Token: 0x0400BCDE RID: 48350
				public static LocString BELOW_BUTTON = "Below";

				// Token: 0x0400BCDF RID: 48351
				public static LocString STATUS_ACTIVE = "Switch Active";

				// Token: 0x0400BCE0 RID: 48352
				public static LocString STATUS_INACTIVE = "Switch Inactive";

				// Token: 0x0400BCE1 RID: 48353
				public static LocString PRESSURE = "Ambient Pressure";

				// Token: 0x0400BCE2 RID: 48354
				public static LocString PRESSURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BCE3 RID: 48355
				public static LocString PRESSURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Pressure",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BCE4 RID: 48356
				public static LocString TEMPERATURE = "Ambient Temperature";

				// Token: 0x0400BCE5 RID: 48357
				public static LocString TEMPERATURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BCE6 RID: 48358
				public static LocString TEMPERATURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BCE7 RID: 48359
				public static LocString CONTENT_TEMPERATURE = "Internal Temperature";

				// Token: 0x0400BCE8 RID: 48360
				public static LocString CONTENT_TEMPERATURE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of its contents is above <b>{0}</b>"
				});

				// Token: 0x0400BCE9 RID: 48361
				public static LocString CONTENT_TEMPERATURE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of its contents is below <b>{0}</b>"
				});

				// Token: 0x0400BCEA RID: 48362
				public static LocString BRIGHTNESS = "Ambient Brightness";

				// Token: 0x0400BCEB RID: 48363
				public static LocString BRIGHTNESS_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Brightness",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BCEC RID: 48364
				public static LocString BRIGHTNESS_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Brightness",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BCED RID: 48365
				public static LocString WATTAGE = "Wattage Reading";

				// Token: 0x0400BCEE RID: 48366
				public static LocString WATTAGE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Wattage",
					UI.PST_KEYWORD,
					" consumed is above <b>{0}</b>"
				});

				// Token: 0x0400BCEF RID: 48367
				public static LocString WATTAGE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Wattage",
					UI.PST_KEYWORD,
					" consumed is below <b>{0}</b>"
				});

				// Token: 0x0400BCF0 RID: 48368
				public static LocString DISEASE_TITLE = "Germ Threshold";

				// Token: 0x0400BCF1 RID: 48369
				public static LocString DISEASE = "Ambient Germs";

				// Token: 0x0400BCF2 RID: 48370
				public static LocString DISEASE_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the number of ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BCF3 RID: 48371
				public static LocString DISEASE_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the number of ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BCF4 RID: 48372
				public static LocString DISEASE_UNITS = "";

				// Token: 0x0400BCF5 RID: 48373
				public static LocString CONTENT_DISEASE = "Germ Count";

				// Token: 0x0400BCF6 RID: 48374
				public static LocString RADIATION = "Ambient Radiation";

				// Token: 0x0400BCF7 RID: 48375
				public static LocString RADIATION_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BCF8 RID: 48376
				public static LocString RADIATION_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ambient ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});

				// Token: 0x0400BCF9 RID: 48377
				public static LocString HEPS = "Radbolt Reading";

				// Token: 0x0400BCFA RID: 48378
				public static LocString HEPS_TOOLTIP_ABOVE = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BCFB RID: 48379
				public static LocString HEPS_TOOLTIP_BELOW = string.Concat(new string[]
				{
					"Will send a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" is below <b>{0}</b>"
				});
			}

			// Token: 0x02002B42 RID: 11074
			public class CAPACITY_CONTROL_SIDE_SCREEN
			{
				// Token: 0x0400BCFC RID: 48380
				public static LocString TITLE = "Automated Storage Capacity";

				// Token: 0x0400BCFD RID: 48381
				public static LocString MAX_LABEL = "Max:";
			}

			// Token: 0x02002B43 RID: 11075
			public class DOOR_TOGGLE_SIDE_SCREEN
			{
				// Token: 0x0400BCFE RID: 48382
				public static LocString TITLE = "Door Setting";

				// Token: 0x0400BCFF RID: 48383
				public static LocString OPEN = "Door is open.";

				// Token: 0x0400BD00 RID: 48384
				public static LocString AUTO = "Door is on auto.";

				// Token: 0x0400BD01 RID: 48385
				public static LocString CLOSE = "Door is locked.";

				// Token: 0x0400BD02 RID: 48386
				public static LocString PENDING_FORMAT = "{0} {1}";

				// Token: 0x0400BD03 RID: 48387
				public static LocString OPEN_PENDING = "Awaiting Duplicant to open door.";

				// Token: 0x0400BD04 RID: 48388
				public static LocString AUTO_PENDING = "Awaiting Duplicant to automate door.";

				// Token: 0x0400BD05 RID: 48389
				public static LocString CLOSE_PENDING = "Awaiting Duplicant to lock door.";

				// Token: 0x0400BD06 RID: 48390
				public static LocString ACCESS_FORMAT = "{0}\n\n{1}";

				// Token: 0x0400BD07 RID: 48391
				public static LocString ACCESS_OFFLINE = "Emergency Access Permissions:\nAll Duplicants are permitted to use this door until " + UI.FormatAsLink("Power", "POWER") + " is restored.";

				// Token: 0x0400BD08 RID: 48392
				public static LocString POI_INTERNAL = "This door cannot be manually controlled.";
			}

			// Token: 0x02002B44 RID: 11076
			public class ACTIVATION_RANGE_SIDE_SCREEN
			{
				// Token: 0x0400BD09 RID: 48393
				public static LocString NAME = "Breaktime Policy";

				// Token: 0x0400BD0A RID: 48394
				public static LocString ACTIVATE = "Break starts at:";

				// Token: 0x0400BD0B RID: 48395
				public static LocString DEACTIVATE = "Break ends at:";
			}

			// Token: 0x02002B45 RID: 11077
			public class CAPACITY_SIDE_SCREEN
			{
				// Token: 0x0400BD0C RID: 48396
				public static LocString TOOLTIP = "Adjust the maximum amount that can be stored here";
			}

			// Token: 0x02002B46 RID: 11078
			public class SUIT_SIDE_SCREEN
			{
				// Token: 0x0400BD0D RID: 48397
				public static LocString TITLE = "Dock Inventory";

				// Token: 0x0400BD0E RID: 48398
				public static LocString CONFIGURATION_REQUIRED = "Configuration Required:";

				// Token: 0x0400BD0F RID: 48399
				public static LocString CONFIG_REQUEST_SUIT = "Deliver Suit";

				// Token: 0x0400BD10 RID: 48400
				public static LocString CONFIG_REQUEST_SUIT_TOOLTIP = "Duplicants will immediately deliver and dock the nearest unequipped suit";

				// Token: 0x0400BD11 RID: 48401
				public static LocString CONFIG_NO_SUIT = "Leave Empty";

				// Token: 0x0400BD12 RID: 48402
				public static LocString CONFIG_NO_SUIT_TOOLTIP = "The next suited Duplicant to pass by will unequip their suit and dock it here";

				// Token: 0x0400BD13 RID: 48403
				public static LocString CONFIG_CANCEL_REQUEST = "Cancel Request";

				// Token: 0x0400BD14 RID: 48404
				public static LocString CONFIG_CANCEL_REQUEST_TOOLTIP = "Cancel this suit delivery";

				// Token: 0x0400BD15 RID: 48405
				public static LocString CONFIG_DROP_SUIT = "Undock Suit";

				// Token: 0x0400BD16 RID: 48406
				public static LocString CONFIG_DROP_SUIT_TOOLTIP = "Disconnect this suit, dropping it on the ground";

				// Token: 0x0400BD17 RID: 48407
				public static LocString CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP = "There is no suit in this building to undock";
			}

			// Token: 0x02002B47 RID: 11079
			public class AUTOMATABLE_SIDE_SCREEN
			{
				// Token: 0x0400BD18 RID: 48408
				public static LocString TITLE = "Automatable Storage";

				// Token: 0x0400BD19 RID: 48409
				public static LocString ALLOWMANUALBUTTON = "Allow Manual Use";

				// Token: 0x0400BD1A RID: 48410
				public static LocString ALLOWMANUALBUTTONTOOLTIP = "Allow Duplicants to manually manage these storage materials";
			}

			// Token: 0x02002B48 RID: 11080
			public class STUDYABLE_SIDE_SCREEN
			{
				// Token: 0x0400BD1B RID: 48411
				public static LocString TITLE = "Analyze Natural Feature";

				// Token: 0x0400BD1C RID: 48412
				public static LocString STUDIED_STATUS = "Researchers have completed their analysis and compiled their findings.";

				// Token: 0x0400BD1D RID: 48413
				public static LocString STUDIED_BUTTON = "ANALYSIS COMPLETE";

				// Token: 0x0400BD1E RID: 48414
				public static LocString SEND_STATUS = "Send a researcher to gather data here.\n\nAnalyzing a feature takes time, but yields useful results.";

				// Token: 0x0400BD1F RID: 48415
				public static LocString SEND_BUTTON = "ANALYZE";

				// Token: 0x0400BD20 RID: 48416
				public static LocString PENDING_STATUS = "A researcher is in the process of studying this feature.";

				// Token: 0x0400BD21 RID: 48417
				public static LocString PENDING_BUTTON = "CANCEL ANALYSIS";
			}

			// Token: 0x02002B49 RID: 11081
			public class MEDICALCOTSIDESCREEN
			{
				// Token: 0x0400BD22 RID: 48418
				public static LocString TITLE = "Severity Requirement";

				// Token: 0x0400BD23 RID: 48419
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant may not use this cot until their ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" falls below <b>{0}%</b>"
				});
			}

			// Token: 0x02002B4A RID: 11082
			public class WARPPORTALSIDESCREEN
			{
				// Token: 0x0400BD24 RID: 48420
				public static LocString TITLE = "Teleporter";

				// Token: 0x0400BD25 RID: 48421
				public static LocString IDLE = "Teleporter online.\nPlease select a passenger:";

				// Token: 0x0400BD26 RID: 48422
				public static LocString WAITING = "Ready to transmit passenger.";

				// Token: 0x0400BD27 RID: 48423
				public static LocString COMPLETE = "Passenger transmitted!";

				// Token: 0x0400BD28 RID: 48424
				public static LocString UNDERWAY = "Transmitting passenger...";

				// Token: 0x0400BD29 RID: 48425
				public static LocString CONSUMED = "Teleporter recharging:";

				// Token: 0x0400BD2A RID: 48426
				public static LocString BUTTON = "Teleport!";

				// Token: 0x0400BD2B RID: 48427
				public static LocString CANCELBUTTON = "Cancel";
			}

			// Token: 0x02002B4B RID: 11083
			public class RADBOLTTHRESHOLDSIDESCREEN
			{
				// Token: 0x0400BD2C RID: 48428
				public static LocString TITLE = "Radbolt Threshold";

				// Token: 0x0400BD2D RID: 48429
				public static LocString CURRENT_THRESHOLD = "Current Threshold: {0}%";

				// Token: 0x0400BD2E RID: 48430
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Releases a ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" when stored Radbolts exceed <b>{0}</b>"
				});

				// Token: 0x0400BD2F RID: 48431
				public static LocString PROGRESS_BAR_LABEL = "Radbolt Generation";

				// Token: 0x0400BD30 RID: 48432
				public static LocString PROGRESS_BAR_TOOLTIP = string.Concat(new string[]
				{
					"The building will emit a ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" in the chosen direction when fully charged"
				});
			}

			// Token: 0x02002B4C RID: 11084
			public class LOGICBITSELECTORSIDESCREEN
			{
				// Token: 0x0400BD31 RID: 48433
				public static LocString RIBBON_READER_TITLE = "Ribbon Reader";

				// Token: 0x0400BD32 RID: 48434
				public static LocString RIBBON_READER_DESCRIPTION = "Selected <b>Bit's Signal</b> will be read by the <b>Output Port</b>";

				// Token: 0x0400BD33 RID: 48435
				public static LocString RIBBON_WRITER_TITLE = "Ribbon Writer";

				// Token: 0x0400BD34 RID: 48436
				public static LocString RIBBON_WRITER_DESCRIPTION = "Received <b>Signal</b> will be written to selected <b>Bit</b>";

				// Token: 0x0400BD35 RID: 48437
				public static LocString BIT = "Bit {0}";

				// Token: 0x0400BD36 RID: 48438
				public static LocString STATE_ACTIVE = "Green";

				// Token: 0x0400BD37 RID: 48439
				public static LocString STATE_INACTIVE = "Red";
			}

			// Token: 0x02002B4D RID: 11085
			public class LOGICALARMSIDESCREEN
			{
				// Token: 0x0400BD38 RID: 48440
				public static LocString TITLE = "Notification Designer";

				// Token: 0x0400BD39 RID: 48441
				public static LocString DESCRIPTION = "Notification will be sent upon receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + "\n\nMaking modifications will clear any existing notifications being sent by this building.";

				// Token: 0x0400BD3A RID: 48442
				public static LocString NAME = "<b>Name:</b>";

				// Token: 0x0400BD3B RID: 48443
				public static LocString NAME_DEFAULT = "Notification";

				// Token: 0x0400BD3C RID: 48444
				public static LocString TOOLTIP = "<b>Tooltip:</b>";

				// Token: 0x0400BD3D RID: 48445
				public static LocString TOOLTIP_DEFAULT = "Tooltip";

				// Token: 0x0400BD3E RID: 48446
				public static LocString TYPE = "<b>Type:</b>";

				// Token: 0x0400BD3F RID: 48447
				public static LocString PAUSE = "<b>Pause:</b>";

				// Token: 0x0400BD40 RID: 48448
				public static LocString ZOOM = "<b>Zoom:</b>";

				// Token: 0x02003736 RID: 14134
				public class TOOLTIPS
				{
					// Token: 0x0400DBF3 RID: 56307
					public static LocString NAME = "Select notification text";

					// Token: 0x0400DBF4 RID: 56308
					public static LocString TOOLTIP = "Select notification hover text";

					// Token: 0x0400DBF5 RID: 56309
					public static LocString TYPE = "Select the visual and aural style of the notification";

					// Token: 0x0400DBF6 RID: 56310
					public static LocString PAUSE = "Time will pause upon notification when checked";

					// Token: 0x0400DBF7 RID: 56311
					public static LocString ZOOM = "The view will zoom to this building upon notification when checked";

					// Token: 0x0400DBF8 RID: 56312
					public static LocString BAD = "\"Boing boing!\"";

					// Token: 0x0400DBF9 RID: 56313
					public static LocString NEUTRAL = "\"Pop!\"";

					// Token: 0x0400DBFA RID: 56314
					public static LocString DUPLICANT_THREATENING = "AHH!";
				}
			}

			// Token: 0x02002B4E RID: 11086
			public class GENETICANALYSISSIDESCREEN
			{
				// Token: 0x0400BD41 RID: 48449
				public static LocString TITLE = "Genetic Analysis";

				// Token: 0x0400BD42 RID: 48450
				public static LocString NONE_DISCOVERED = "No mutant seeds have been found.";

				// Token: 0x0400BD43 RID: 48451
				public static LocString SELECT_SEEDS = "Select which seed types to analyze:";

				// Token: 0x0400BD44 RID: 48452
				public static LocString SEED_NO_MUTANTS = "</i>No mutants found</i>";

				// Token: 0x0400BD45 RID: 48453
				public static LocString SEED_FORBIDDEN = "</i>Won't analyze</i>";

				// Token: 0x0400BD46 RID: 48454
				public static LocString SEED_ALLOWED = "</i>Will analyze</i>";
			}

			// Token: 0x02002B4F RID: 11087
			public class RELATEDENTITIESSIDESCREEN
			{
				// Token: 0x0400BD47 RID: 48455
				public static LocString TITLE = "Related Objects";
			}
		}

		// Token: 0x0200217E RID: 8574
		public class USERMENUACTIONS
		{
			// Token: 0x02002B50 RID: 11088
			public class TINKER
			{
				// Token: 0x0400BD48 RID: 48456
				public static LocString ALLOW = "Allow Tinker";

				// Token: 0x0400BD49 RID: 48457
				public static LocString DISALLOW = "Disallow Tinker";

				// Token: 0x0400BD4A RID: 48458
				public static LocString TOOLTIP_DISALLOW = "Disallow Tinker Tool application on this building";

				// Token: 0x0400BD4B RID: 48459
				public static LocString TOOLTIP_ALLOW = "Allow Tinker Tool application on this building";
			}

			// Token: 0x02002B51 RID: 11089
			public class TRANSITTUBEWAX
			{
				// Token: 0x0400BD4C RID: 48460
				public static LocString NAME = "Enable Smooth Ride";

				// Token: 0x0400BD4D RID: 48461
				public static LocString TOOLTIP = "Enables the use of " + ELEMENTS.MILKFAT.NAME + " to boost travel speed";
			}

			// Token: 0x02002B52 RID: 11090
			public class CANCELTRANSITTUBEWAX
			{
				// Token: 0x0400BD4E RID: 48462
				public static LocString NAME = "Disable Smooth Ride";

				// Token: 0x0400BD4F RID: 48463
				public static LocString TOOLTIP = "Disables travel speed boost and refunds stored " + ELEMENTS.MILKFAT.NAME;
			}

			// Token: 0x02002B53 RID: 11091
			public class CLEANTOILET
			{
				// Token: 0x0400BD50 RID: 48464
				public static LocString NAME = "Clean Toilet";

				// Token: 0x0400BD51 RID: 48465
				public static LocString TOOLTIP = "Empty waste from this toilet";
			}

			// Token: 0x02002B54 RID: 11092
			public class CANCELCLEANTOILET
			{
				// Token: 0x0400BD52 RID: 48466
				public static LocString NAME = "Cancel Clean";

				// Token: 0x0400BD53 RID: 48467
				public static LocString TOOLTIP = "Cancel this cleaning order";
			}

			// Token: 0x02002B55 RID: 11093
			public class EMPTYBEEHIVE
			{
				// Token: 0x0400BD54 RID: 48468
				public static LocString NAME = "Enable Autoharvest";

				// Token: 0x0400BD55 RID: 48469
				public static LocString TOOLTIP = "Automatically harvest this hive when full";
			}

			// Token: 0x02002B56 RID: 11094
			public class CANCELEMPTYBEEHIVE
			{
				// Token: 0x0400BD56 RID: 48470
				public static LocString NAME = "Disable Autoharvest";

				// Token: 0x0400BD57 RID: 48471
				public static LocString TOOLTIP = "Do not automatically harvest this hive";
			}

			// Token: 0x02002B57 RID: 11095
			public class EMPTYDESALINATOR
			{
				// Token: 0x0400BD58 RID: 48472
				public static LocString NAME = "Empty Desalinator";

				// Token: 0x0400BD59 RID: 48473
				public static LocString TOOLTIP = "Empty salt from this desalinator";
			}

			// Token: 0x02002B58 RID: 11096
			public class CHANGE_ROOM
			{
				// Token: 0x0400BD5A RID: 48474
				public static LocString REQUEST_OUTFIT = "Request Outfit";

				// Token: 0x0400BD5B RID: 48475
				public static LocString REQUEST_OUTFIT_TOOLTIP = "Request outfit to be delivered to this change room";

				// Token: 0x0400BD5C RID: 48476
				public static LocString CANCEL_REQUEST = "Cancel Request";

				// Token: 0x0400BD5D RID: 48477
				public static LocString CANCEL_REQUEST_TOOLTIP = "Cancel outfit request";

				// Token: 0x0400BD5E RID: 48478
				public static LocString DROP_OUTFIT = "Drop Outfit";

				// Token: 0x0400BD5F RID: 48479
				public static LocString DROP_OUTFIT_TOOLTIP = "Drop outfit on floor";
			}

			// Token: 0x02002B59 RID: 11097
			public class DUMP
			{
				// Token: 0x0400BD60 RID: 48480
				public static LocString NAME = "Empty";

				// Token: 0x0400BD61 RID: 48481
				public static LocString TOOLTIP = "Dump bottle contents onto the floor";

				// Token: 0x0400BD62 RID: 48482
				public static LocString NAME_OFF = "Cancel Empty";

				// Token: 0x0400BD63 RID: 48483
				public static LocString TOOLTIP_OFF = "Cancel this empty order";
			}

			// Token: 0x02002B5A RID: 11098
			public class TAGFILTER
			{
				// Token: 0x0400BD64 RID: 48484
				public static LocString NAME = "Filter Settings";

				// Token: 0x0400BD65 RID: 48485
				public static LocString TOOLTIP = "Assign materials to storage";
			}

			// Token: 0x02002B5B RID: 11099
			public class CANCELCONSTRUCTION
			{
				// Token: 0x0400BD66 RID: 48486
				public static LocString NAME = "Cancel Build";

				// Token: 0x0400BD67 RID: 48487
				public static LocString TOOLTIP = "Cancel this build order";
			}

			// Token: 0x02002B5C RID: 11100
			public class DIG
			{
				// Token: 0x0400BD68 RID: 48488
				public static LocString NAME = "Dig";

				// Token: 0x0400BD69 RID: 48489
				public static LocString TOOLTIP = "Dig out this cell";

				// Token: 0x0400BD6A RID: 48490
				public static LocString TOOLTIP_OFF = "Cancel this dig order";
			}

			// Token: 0x02002B5D RID: 11101
			public class CANCELMOP
			{
				// Token: 0x0400BD6B RID: 48491
				public static LocString NAME = "Cancel Mop";

				// Token: 0x0400BD6C RID: 48492
				public static LocString TOOLTIP = "Cancel this mop order";
			}

			// Token: 0x02002B5E RID: 11102
			public class CANCELDIG
			{
				// Token: 0x0400BD6D RID: 48493
				public static LocString NAME = "Cancel Dig";

				// Token: 0x0400BD6E RID: 48494
				public static LocString TOOLTIP = "Cancel this dig order";
			}

			// Token: 0x02002B5F RID: 11103
			public class UPROOT
			{
				// Token: 0x0400BD6F RID: 48495
				public static LocString NAME = "Uproot";

				// Token: 0x0400BD70 RID: 48496
				public static LocString TOOLTIP = "Convert this plant into a seed";
			}

			// Token: 0x02002B60 RID: 11104
			public class CANCELUPROOT
			{
				// Token: 0x0400BD71 RID: 48497
				public static LocString NAME = "Cancel Uproot";

				// Token: 0x0400BD72 RID: 48498
				public static LocString TOOLTIP = "Cancel this uproot order";
			}

			// Token: 0x02002B61 RID: 11105
			public class HARVEST_WHEN_READY
			{
				// Token: 0x0400BD73 RID: 48499
				public static LocString NAME = "Enable Autoharvest";

				// Token: 0x0400BD74 RID: 48500
				public static LocString TOOLTIP = "Automatically harvest this plant when it matures";
			}

			// Token: 0x02002B62 RID: 11106
			public class CANCEL_HARVEST_WHEN_READY
			{
				// Token: 0x0400BD75 RID: 48501
				public static LocString NAME = "Disable Autoharvest";

				// Token: 0x0400BD76 RID: 48502
				public static LocString TOOLTIP = "Do not automatically harvest this plant";
			}

			// Token: 0x02002B63 RID: 11107
			public class HARVEST
			{
				// Token: 0x0400BD77 RID: 48503
				public static LocString NAME = "Harvest";

				// Token: 0x0400BD78 RID: 48504
				public static LocString TOOLTIP = "Harvest materials from this plant";

				// Token: 0x0400BD79 RID: 48505
				public static LocString TOOLTIP_DISABLED = "This plant has nothing to harvest";
			}

			// Token: 0x02002B64 RID: 11108
			public class CANCELHARVEST
			{
				// Token: 0x0400BD7A RID: 48506
				public static LocString NAME = "Cancel Harvest";

				// Token: 0x0400BD7B RID: 48507
				public static LocString TOOLTIP = "Cancel this harvest order";
			}

			// Token: 0x02002B65 RID: 11109
			public class ATTACK
			{
				// Token: 0x0400BD7C RID: 48508
				public static LocString NAME = "Attack";

				// Token: 0x0400BD7D RID: 48509
				public static LocString TOOLTIP = "Attack this critter";
			}

			// Token: 0x02002B66 RID: 11110
			public class CANCELATTACK
			{
				// Token: 0x0400BD7E RID: 48510
				public static LocString NAME = "Cancel Attack";

				// Token: 0x0400BD7F RID: 48511
				public static LocString TOOLTIP = "Cancel this attack order";
			}

			// Token: 0x02002B67 RID: 11111
			public class CAPTURE
			{
				// Token: 0x0400BD80 RID: 48512
				public static LocString NAME = "Wrangle";

				// Token: 0x0400BD81 RID: 48513
				public static LocString TOOLTIP = "Capture this critter alive";
			}

			// Token: 0x02002B68 RID: 11112
			public class CANCELCAPTURE
			{
				// Token: 0x0400BD82 RID: 48514
				public static LocString NAME = "Cancel Wrangle";

				// Token: 0x0400BD83 RID: 48515
				public static LocString TOOLTIP = "Cancel this wrangle order";
			}

			// Token: 0x02002B69 RID: 11113
			public class RELEASEELEMENT
			{
				// Token: 0x0400BD84 RID: 48516
				public static LocString NAME = "Empty Building";

				// Token: 0x0400BD85 RID: 48517
				public static LocString TOOLTIP = "Refund all resources currently in use by this building";
			}

			// Token: 0x02002B6A RID: 11114
			public class DECONSTRUCT
			{
				// Token: 0x0400BD86 RID: 48518
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400BD87 RID: 48519
				public static LocString TOOLTIP = "Deconstruct this building and refund all resources";

				// Token: 0x0400BD88 RID: 48520
				public static LocString NAME_OFF = "Cancel Deconstruct";

				// Token: 0x0400BD89 RID: 48521
				public static LocString TOOLTIP_OFF = "Cancel this deconstruct order";
			}

			// Token: 0x02002B6B RID: 11115
			public class DEMOLISH
			{
				// Token: 0x0400BD8A RID: 48522
				public static LocString NAME = "Demolish";

				// Token: 0x0400BD8B RID: 48523
				public static LocString TOOLTIP = "Demolish this building";

				// Token: 0x0400BD8C RID: 48524
				public static LocString NAME_OFF = "Cancel Demolition";

				// Token: 0x0400BD8D RID: 48525
				public static LocString TOOLTIP_OFF = "Cancel this demolition order";
			}

			// Token: 0x02002B6C RID: 11116
			public class ROCKETUSAGERESTRICTION
			{
				// Token: 0x0400BD8E RID: 48526
				public static LocString NAME_UNCONTROLLED = "Uncontrolled";

				// Token: 0x0400BD8F RID: 48527
				public static LocString TOOLTIP_UNCONTROLLED = "Do not allow this building to be controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;

				// Token: 0x0400BD90 RID: 48528
				public static LocString NAME_CONTROLLED = "Controlled";

				// Token: 0x0400BD91 RID: 48529
				public static LocString TOOLTIP_CONTROLLED = "Allow this building's operation to be controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02002B6D RID: 11117
			public class MANUAL_DELIVERY
			{
				// Token: 0x0400BD92 RID: 48530
				public static LocString NAME = "Disable Delivery";

				// Token: 0x0400BD93 RID: 48531
				public static LocString TOOLTIP = "Do not deliver materials to this building";

				// Token: 0x0400BD94 RID: 48532
				public static LocString NAME_OFF = "Enable Delivery";

				// Token: 0x0400BD95 RID: 48533
				public static LocString TOOLTIP_OFF = "Deliver materials to this building";
			}

			// Token: 0x02002B6E RID: 11118
			public class SELECTRESEARCH
			{
				// Token: 0x0400BD96 RID: 48534
				public static LocString NAME = "Select Research";

				// Token: 0x0400BD97 RID: 48535
				public static LocString TOOLTIP = "Choose a technology from the " + UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch);
			}

			// Token: 0x02002B6F RID: 11119
			public class RECONSTRUCT
			{
				// Token: 0x0400BD98 RID: 48536
				public static LocString REQUEST_RECONSTRUCT = "Order Rebuild";

				// Token: 0x0400BD99 RID: 48537
				public static LocString REQUEST_RECONSTRUCT_TOOLTIP = "Deconstruct this building and rebuild it using the selected material";

				// Token: 0x0400BD9A RID: 48538
				public static LocString CANCEL_RECONSTRUCT = "Cancel Rebuild Order";

				// Token: 0x0400BD9B RID: 48539
				public static LocString CANCEL_RECONSTRUCT_TOOLTIP = "Cancel deconstruction and rebuilding of this building";
			}

			// Token: 0x02002B70 RID: 11120
			public class RELOCATE
			{
				// Token: 0x0400BD9C RID: 48540
				public static LocString NAME = "Relocate";

				// Token: 0x0400BD9D RID: 48541
				public static LocString TOOLTIP = "Move this building to a new location\n\nCosts no additional resources";

				// Token: 0x0400BD9E RID: 48542
				public static LocString NAME_OFF = "Cancel Relocation";

				// Token: 0x0400BD9F RID: 48543
				public static LocString TOOLTIP_OFF = "Cancel this relocation order";
			}

			// Token: 0x02002B71 RID: 11121
			public class ENABLEBUILDING
			{
				// Token: 0x0400BDA0 RID: 48544
				public static LocString NAME = "Disable Building";

				// Token: 0x0400BDA1 RID: 48545
				public static LocString TOOLTIP = "Halt the use of this building {Hotkey}\n\nDisabled buildings consume no energy or resources";

				// Token: 0x0400BDA2 RID: 48546
				public static LocString NAME_OFF = "Enable Building";

				// Token: 0x0400BDA3 RID: 48547
				public static LocString TOOLTIP_OFF = "Resume the use of this building {Hotkey}";
			}

			// Token: 0x02002B72 RID: 11122
			public class READLORE
			{
				// Token: 0x0400BDA4 RID: 48548
				public static LocString NAME = "Inspect";

				// Token: 0x0400BDA5 RID: 48549
				public static LocString ALREADYINSPECTED = "Already inspected";

				// Token: 0x0400BDA6 RID: 48550
				public static LocString TOOLTIP = "Recover files from this structure";

				// Token: 0x0400BDA7 RID: 48551
				public static LocString TOOLTIP_ALREADYINSPECTED = "This structure has already been inspected";

				// Token: 0x0400BDA8 RID: 48552
				public static LocString GOTODATABASE = "View Entry";

				// Token: 0x0400BDA9 RID: 48553
				public static LocString SEARCH_DISPLAY = "The display is still functional. I copy its message into my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BDAA RID: 48554
				public static LocString SEARCH_ELLIESDESK = "All I find on the machine is a curt e-mail from a disgruntled employee.\n\nNew Database Entry discovered.";

				// Token: 0x0400BDAB RID: 48555
				public static LocString SEARCH_POD = "I search my incoming message history and find a single entry. I move the odd message into my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BDAC RID: 48556
				public static LocString ALREADY_SEARCHED = "I already took everything of interest from this. I can check the Database to re-read what I found.";

				// Token: 0x0400BDAD RID: 48557
				public static LocString SEARCH_CABINET = "One intact document remains - an old yellowing newspaper clipping. It won't be of much use, but I add it to my database nonetheless.\n\nNew Database Entry discovered.";

				// Token: 0x0400BDAE RID: 48558
				public static LocString SEARCH_STERNSDESK = "There's an old magazine article from a publication called the \"Nucleoid\" tucked in the top drawer. I add it to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BDAF RID: 48559
				public static LocString ALREADY_SEARCHED_STERNSDESK = "The desk is eerily empty inside.";

				// Token: 0x0400BDB0 RID: 48560
				public static LocString SEARCH_TELEPORTER_SENDER = "While scanning the antiquated computer code of this machine I uncovered some research notes. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BDB1 RID: 48561
				public static LocString SEARCH_TELEPORTER_RECEIVER = "Incongruously placed research notes are hidden within the operating instructions of this device. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BDB2 RID: 48562
				public static LocString SEARCH_CRYO_TANK = "There are some safety instructions included in the operating instructions of this Cryotank. I add them to my database.\n\nNew Database Entry discovered.";

				// Token: 0x0400BDB3 RID: 48563
				public static LocString SEARCH_PROPGRAVITASCREATUREPOSTER = "There's a handwritten note taped to the back of this poster. I add it to my database.\n\nNew Database Entry discovered.";

				// Token: 0x02003737 RID: 14135
				public class SEARCH_COMPUTER_PODIUM
				{
					// Token: 0x0400DBFB RID: 56315
					public static LocString SEARCH1 = "I search through the computer's database and find an unredacted e-mail.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02003738 RID: 14136
				public class SEARCH_COMPUTER_SUCCESS
				{
					// Token: 0x0400DBFC RID: 56316
					public static LocString SEARCH1 = "After searching through the computer's database, I managed to piece together some files that piqued my interest.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DBFD RID: 56317
					public static LocString SEARCH2 = "Searching through the computer, I find some recoverable files that are still readable.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DBFE RID: 56318
					public static LocString SEARCH3 = "The computer looks pristine on the outside, but is corrupted internally. Still, I managed to find one uncorrupted file, and have added it to my database.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DBFF RID: 56319
					public static LocString SEARCH4 = "The computer was wiped almost completely clean, except for one file hidden in the recycle bin.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DC00 RID: 56320
					public static LocString SEARCH5 = "I search the computer, storing what useful data I can find in my own memory.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DC01 RID: 56321
					public static LocString SEARCH6 = "This computer is broken and requires some finessing to get working. Still, I recover a handful of interesting files.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x02003739 RID: 14137
				public class SEARCH_COMPUTER_FAIL
				{
					// Token: 0x0400DC02 RID: 56322
					public static LocString SEARCH1 = "Unfortunately, the computer's hard drive is irreparably corrupted.";

					// Token: 0x0400DC03 RID: 56323
					public static LocString SEARCH2 = "The computer was wiped clean before I got here. There is nothing to recover.";

					// Token: 0x0400DC04 RID: 56324
					public static LocString SEARCH3 = "Some intact files are available on the computer, but nothing I haven't already discovered elsewhere. I find nothing else.";

					// Token: 0x0400DC05 RID: 56325
					public static LocString SEARCH4 = "The computer has nothing of import.";

					// Token: 0x0400DC06 RID: 56326
					public static LocString SEARCH5 = "Someone's left a solitaire game up. There's nothing else of interest on the computer.\n\nAlso, it looks as though they were about to lose.";

					// Token: 0x0400DC07 RID: 56327
					public static LocString SEARCH6 = "The background on this computer depicts two kittens hugging in a field of daisies. There is nothing else of import to be found.";

					// Token: 0x0400DC08 RID: 56328
					public static LocString SEARCH7 = "The user alphabetized the shortcuts on their desktop. There is nothing else of import to be found.";

					// Token: 0x0400DC09 RID: 56329
					public static LocString SEARCH8 = "The background is a picture of a golden retriever in a science lab. It looks very confused. There is nothing else of import to be found.";

					// Token: 0x0400DC0A RID: 56330
					public static LocString SEARCH9 = "This user never changed their default background. There is nothing else of import to be found. How dull.";
				}

				// Token: 0x0200373A RID: 14138
				public class SEARCH_TECHNOLOGY_SUCCESS
				{
					// Token: 0x0400DC0B RID: 56331
					public static LocString SEARCH1 = "I scour the internal systems and find something of interest.\n\nNew Database Entry discovered.";

					// Token: 0x0400DC0C RID: 56332
					public static LocString SEARCH2 = "I see if I can salvage anything from the electronics. I add what I find to my database.\n\nNew Database Entry discovered.";

					// Token: 0x0400DC0D RID: 56333
					public static LocString SEARCH3 = "I look for anything of interest within the abandoned machinery and add what I find to my database.\n\nNew Database Entry discovered.";
				}

				// Token: 0x0200373B RID: 14139
				public class SEARCH_OBJECT_SUCCESS
				{
					// Token: 0x0400DC0E RID: 56334
					public static LocString SEARCH1 = "I look around and recover an old file.\n\nNew Database Entry discovered.";

					// Token: 0x0400DC0F RID: 56335
					public static LocString SEARCH2 = "There's a three-ringed binder inside. I scan the surviving documents.\n\nNew Database Entry discovered.";

					// Token: 0x0400DC10 RID: 56336
					public static LocString SEARCH3 = "A discarded journal inside remains mostly intact. I scan the pages of use.\n\nNew Database Entry discovered.";

					// Token: 0x0400DC11 RID: 56337
					public static LocString SEARCH4 = "A single page of a long printout remains legible. I scan it and add it to my database.\n\nNew Database Entry discovered.";

					// Token: 0x0400DC12 RID: 56338
					public static LocString SEARCH5 = "A few loose papers can be found inside. I scan the ones that look interesting.\n\nNew Database Entry discovered.";

					// Token: 0x0400DC13 RID: 56339
					public static LocString SEARCH6 = "I find a memory stick inside and copy its data into my database.\n\nNew Database Entry discovered.";
				}

				// Token: 0x0200373C RID: 14140
				public class SEARCH_OBJECT_FAIL
				{
					// Token: 0x0400DC14 RID: 56340
					public static LocString SEARCH1 = "I look around but find nothing of interest.";
				}

				// Token: 0x0200373D RID: 14141
				public class SEARCH_SPACEPOI_SUCCESS
				{
					// Token: 0x0400DC15 RID: 56341
					public static LocString SEARCH1 = "A quick analysis of the hardware of this debris has uncovered some searchable files within.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DC16 RID: 56342
					public static LocString SEARCH2 = "There's an archaic interface I can interact with on this device.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DC17 RID: 56343
					public static LocString SEARCH3 = "While investigating the software of this wreckage, a compelling file comes to my attention.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DC18 RID: 56344
					public static LocString SEARCH4 = "Not much remains of the software that once ran this spacecraft except for one file that piques my interest.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DC19 RID: 56345
					public static LocString SEARCH5 = "I find some noteworthy data hidden amongst the system files of this space junk.\n\nNew Database Entry unlocked.";

					// Token: 0x0400DC1A RID: 56346
					public static LocString SEARCH6 = "Despite being subjected to years of degradation, there are still recoverable files in this machinery.\n\nNew Database Entry unlocked.";
				}

				// Token: 0x0200373E RID: 14142
				public class SEARCH_SPACEPOI_FAIL
				{
					// Token: 0x0400DC1B RID: 56347
					public static LocString SEARCH1 = "There's nothing of interest left in this old space junk.";

					// Token: 0x0400DC1C RID: 56348
					public static LocString SEARCH2 = "I've salvaged everything I can from this vehicle.";

					// Token: 0x0400DC1D RID: 56349
					public static LocString SEARCH3 = "Years of neglect and radioactive decay have destroyed all the useful data from this derelict spacecraft.";
				}

				// Token: 0x0200373F RID: 14143
				public class SEARCH_DISPLAY_FAIL
				{
					// Token: 0x0400DC1E RID: 56350
					public static LocString SEARCH1 = "The display is frozen. Whatever information it once contained is long gone.";
				}
			}

			// Token: 0x02002B73 RID: 11123
			public class OPENPOI
			{
				// Token: 0x0400BDB4 RID: 48564
				public static LocString NAME = "Rummage";

				// Token: 0x0400BDB5 RID: 48565
				public static LocString TOOLTIP = "Scrounge for usable materials";

				// Token: 0x0400BDB6 RID: 48566
				public static LocString NAME_OFF = "Cancel Rummage";

				// Token: 0x0400BDB7 RID: 48567
				public static LocString TOOLTIP_OFF = "Cancel this rummage order";

				// Token: 0x0400BDB8 RID: 48568
				public static LocString ALREADY_RUMMAGED = "Already Rummaged";

				// Token: 0x0400BDB9 RID: 48569
				public static LocString TOOLTIP_ALREADYRUMMAGED = "There are no usable materials left to find";
			}

			// Token: 0x02002B74 RID: 11124
			public class OPEN_TECHUNLOCKS
			{
				// Token: 0x0400BDBA RID: 48570
				public static LocString NAME = "Unlock Research";

				// Token: 0x0400BDBB RID: 48571
				public static LocString TOOLTIP = "Retrieve data stored in this building";

				// Token: 0x0400BDBC RID: 48572
				public static LocString NAME_OFF = "Cancel Unlock Research";

				// Token: 0x0400BDBD RID: 48573
				public static LocString TOOLTIP_OFF = "Cancel this research access order";

				// Token: 0x0400BDBE RID: 48574
				public static LocString ALREADY_RUMMAGED = "Already Unlocked";

				// Token: 0x0400BDBF RID: 48575
				public static LocString TOOLTIP_ALREADYRUMMAGED = "All data has been accessed and recorded";
			}

			// Token: 0x02002B75 RID: 11125
			public class EMPTYSTORAGE
			{
				// Token: 0x0400BDC0 RID: 48576
				public static LocString NAME = "Empty Storage";

				// Token: 0x0400BDC1 RID: 48577
				public static LocString TOOLTIP = "Eject all resources from this container";

				// Token: 0x0400BDC2 RID: 48578
				public static LocString NAME_OFF = "Cancel Empty";

				// Token: 0x0400BDC3 RID: 48579
				public static LocString TOOLTIP_OFF = "Cancel this empty order";
			}

			// Token: 0x02002B76 RID: 11126
			public class CLOSESTORAGE
			{
				// Token: 0x0400BDC4 RID: 48580
				public static LocString NAME = "Close Storage";

				// Token: 0x0400BDC5 RID: 48581
				public static LocString TOOLTIP = "Prevent this container from receiving resources for storage";

				// Token: 0x0400BDC6 RID: 48582
				public static LocString NAME_OFF = "Cancel Close";

				// Token: 0x0400BDC7 RID: 48583
				public static LocString TOOLTIP_OFF = "Cancel this close order";
			}

			// Token: 0x02002B77 RID: 11127
			public class COPY_BUILDING_SETTINGS
			{
				// Token: 0x0400BDC8 RID: 48584
				public static LocString NAME = "Copy Settings";

				// Token: 0x0400BDC9 RID: 48585
				public static LocString TOOLTIP = "Apply the settings and priorities of this building to other buildings of the same type {Hotkey}";
			}

			// Token: 0x02002B78 RID: 11128
			public class CLEAR
			{
				// Token: 0x0400BDCA RID: 48586
				public static LocString NAME = "Sweep";

				// Token: 0x0400BDCB RID: 48587
				public static LocString TOOLTIP = "Put this object away in the nearest storage container";

				// Token: 0x0400BDCC RID: 48588
				public static LocString NAME_OFF = "Cancel Sweeping";

				// Token: 0x0400BDCD RID: 48589
				public static LocString TOOLTIP_OFF = "Cancel this sweep order";
			}

			// Token: 0x02002B79 RID: 11129
			public class COMPOST
			{
				// Token: 0x0400BDCE RID: 48590
				public static LocString NAME = "Compost";

				// Token: 0x0400BDCF RID: 48591
				public static LocString TOOLTIP = "Mark this object for compost";

				// Token: 0x0400BDD0 RID: 48592
				public static LocString NAME_OFF = "Cancel Compost";

				// Token: 0x0400BDD1 RID: 48593
				public static LocString TOOLTIP_OFF = "Cancel this compost order";
			}

			// Token: 0x02002B7A RID: 11130
			public class PICKUPABLEMOVE
			{
				// Token: 0x0400BDD2 RID: 48594
				public static LocString NAME = "Relocate To";

				// Token: 0x0400BDD3 RID: 48595
				public static LocString TOOLTIP = "Relocate this object to a specific location";

				// Token: 0x0400BDD4 RID: 48596
				public static LocString NAME_OFF = "Cancel Relocate";

				// Token: 0x0400BDD5 RID: 48597
				public static LocString TOOLTIP_OFF = "Cancel order to relocate this object";
			}

			// Token: 0x02002B7B RID: 11131
			public class UNEQUIP
			{
				// Token: 0x0400BDD6 RID: 48598
				public static LocString NAME = "Unequip {0}";

				// Token: 0x0400BDD7 RID: 48599
				public static LocString TOOLTIP = "Take off and drop this equipment";
			}

			// Token: 0x02002B7C RID: 11132
			public class QUARANTINE
			{
				// Token: 0x0400BDD8 RID: 48600
				public static LocString NAME = "Quarantine";

				// Token: 0x0400BDD9 RID: 48601
				public static LocString TOOLTIP = "Isolate this Duplicant\nThe Duplicant will return to their assigned Cot";

				// Token: 0x0400BDDA RID: 48602
				public static LocString TOOLTIP_DISABLED = "No quarantine zone assigned";

				// Token: 0x0400BDDB RID: 48603
				public static LocString NAME_OFF = "Cancel Quarantine";

				// Token: 0x0400BDDC RID: 48604
				public static LocString TOOLTIP_OFF = "Cancel this quarantine order";
			}

			// Token: 0x02002B7D RID: 11133
			public class DRAWPATHS
			{
				// Token: 0x0400BDDD RID: 48605
				public static LocString NAME = "Show Navigation";

				// Token: 0x0400BDDE RID: 48606
				public static LocString TOOLTIP = "Show all areas within this Duplicant's reach";

				// Token: 0x0400BDDF RID: 48607
				public static LocString NAME_OFF = "Hide Navigation";

				// Token: 0x0400BDE0 RID: 48608
				public static LocString TOOLTIP_OFF = "Hide areas within this Duplicant's reach";
			}

			// Token: 0x02002B7E RID: 11134
			public class MOVETOLOCATION
			{
				// Token: 0x0400BDE1 RID: 48609
				public static LocString NAME = "Move To";

				// Token: 0x0400BDE2 RID: 48610
				public static LocString TOOLTIP = "Move this Duplicant to a specific location";
			}

			// Token: 0x02002B7F RID: 11135
			public class FOLLOWCAM
			{
				// Token: 0x0400BDE3 RID: 48611
				public static LocString NAME = "Follow Cam";

				// Token: 0x0400BDE4 RID: 48612
				public static LocString TOOLTIP = "Track this Duplicant with the camera";
			}

			// Token: 0x02002B80 RID: 11136
			public class WORKABLE_DIRECTION_BOTH
			{
				// Token: 0x0400BDE5 RID: 48613
				public static LocString NAME = "Set Direction: Both";

				// Token: 0x0400BDE6 RID: 48614
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by in either direction";
			}

			// Token: 0x02002B81 RID: 11137
			public class WORKABLE_DIRECTION_LEFT
			{
				// Token: 0x0400BDE7 RID: 48615
				public static LocString NAME = "Set Direction: Left";

				// Token: 0x0400BDE8 RID: 48616
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by from right to left";
			}

			// Token: 0x02002B82 RID: 11138
			public class WORKABLE_DIRECTION_RIGHT
			{
				// Token: 0x0400BDE9 RID: 48617
				public static LocString NAME = "Set Direction: Right";

				// Token: 0x0400BDEA RID: 48618
				public static LocString TOOLTIP = "Select to make Duplicants wash when passing by from left to right";
			}

			// Token: 0x02002B83 RID: 11139
			public class MANUAL_PUMP_DELIVERY
			{
				// Token: 0x02003740 RID: 14144
				public static class ALLOWED
				{
					// Token: 0x0400DC1F RID: 56351
					public static LocString NAME = "Enable Auto-Bottle";

					// Token: 0x0400DC20 RID: 56352
					public static LocString TOOLTIP = "If enabled, Duplicants will deliver bottled liquids to this building directly from these sources:\n";

					// Token: 0x0400DC21 RID: 56353
					public static LocString ITEM = "\n{0}";
				}

				// Token: 0x02003741 RID: 14145
				public static class DENIED
				{
					// Token: 0x0400DC22 RID: 56354
					public static LocString NAME = "Disable Auto-Bottle";

					// Token: 0x0400DC23 RID: 56355
					public static LocString TOOLTIP = "If disabled, Duplicants will no longer deliver bottled liquids directly from Pitcher Pumps";
				}

				// Token: 0x02003742 RID: 14146
				public static class ALLOWED_GAS
				{
					// Token: 0x0400DC24 RID: 56356
					public static LocString NAME = "Enable Auto-Bottle";

					// Token: 0x0400DC25 RID: 56357
					public static LocString TOOLTIP = "If enabled, Duplicants will deliver gas canisters to this building directly from Canister Fillers";
				}

				// Token: 0x02003743 RID: 14147
				public static class DENIED_GAS
				{
					// Token: 0x0400DC26 RID: 56358
					public static LocString NAME = "Disable Auto-Bottle";

					// Token: 0x0400DC27 RID: 56359
					public static LocString TOOLTIP = "If disabled, Duplicants will no longer deliver gas canisters directly from Canister Fillers";
				}
			}

			// Token: 0x02002B84 RID: 11140
			public class SUIT_MARKER_TRAVERSAL
			{
				// Token: 0x02003744 RID: 14148
				public static class ONLY_WHEN_ROOM_AVAILABLE
				{
					// Token: 0x0400DC28 RID: 56360
					public static LocString NAME = "Clearance: Vacancy";

					// Token: 0x0400DC29 RID: 56361
					public static LocString TOOLTIP = "Suited Duplicants may only pass if there is an available dock to store their suit";
				}

				// Token: 0x02003745 RID: 14149
				public static class ALWAYS
				{
					// Token: 0x0400DC2A RID: 56362
					public static LocString NAME = "Clearance: Always";

					// Token: 0x0400DC2B RID: 56363
					public static LocString TOOLTIP = "Suited Duplicants may pass even if there is no room to store their suits\n\nWhen all available docks are full, Duplicants will unequip their suits and drop them on the floor";
				}
			}

			// Token: 0x02002B85 RID: 11141
			public class ACTIVATEBUILDING
			{
				// Token: 0x0400BDEB RID: 48619
				public static LocString ACTIVATE = "Activate";

				// Token: 0x0400BDEC RID: 48620
				public static LocString TOOLTIP_ACTIVATE = "Request a Duplicant to activate this building";

				// Token: 0x0400BDED RID: 48621
				public static LocString TOOLTIP_ACTIVATED = "This building has already been activated";

				// Token: 0x0400BDEE RID: 48622
				public static LocString ACTIVATE_CANCEL = "Cancel Activation";

				// Token: 0x0400BDEF RID: 48623
				public static LocString ACTIVATED = "Activated";

				// Token: 0x0400BDF0 RID: 48624
				public static LocString TOOLTIP_CANCEL = "Cancel activation of this building";
			}

			// Token: 0x02002B86 RID: 11142
			public class ACCEPT_MUTANT_SEEDS
			{
				// Token: 0x0400BDF1 RID: 48625
				public static LocString ACCEPT = "Allow Mutants";

				// Token: 0x0400BDF2 RID: 48626
				public static LocString REJECT = "Forbid Mutants";

				// Token: 0x0400BDF3 RID: 48627
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Toggle whether or not this building will accept ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" for recipes that could use them"
				});

				// Token: 0x0400BDF4 RID: 48628
				public static LocString FISH_FEEDER_TOOLTIP = string.Concat(new string[]
				{
					"Toggle whether or not this feeder will accept ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" for critters who eat them"
				});
			}

			// Token: 0x02002B87 RID: 11143
			public class CARVE
			{
				// Token: 0x0400BDF5 RID: 48629
				public static LocString NAME = "Carve";

				// Token: 0x0400BDF6 RID: 48630
				public static LocString TOOLTIP = "Carve this rock to enhance its positive effects";
			}

			// Token: 0x02002B88 RID: 11144
			public class CANCELCARVE
			{
				// Token: 0x0400BDF7 RID: 48631
				public static LocString NAME = "Cancel Carve";

				// Token: 0x0400BDF8 RID: 48632
				public static LocString TOOLTIP = "Cancel order to carve this rock";
			}
		}

		// Token: 0x0200217F RID: 8575
		public class BUILDCATEGORIES
		{
			// Token: 0x02002B89 RID: 11145
			public static class BASE
			{
				// Token: 0x0400BDF9 RID: 48633
				public static LocString NAME = UI.FormatAsLink("Base", "BUILDCATEGORYBASE");

				// Token: 0x0400BDFA RID: 48634
				public static LocString TOOLTIP = "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
			}

			// Token: 0x02002B8A RID: 11146
			public static class CONVEYANCE
			{
				// Token: 0x0400BDFB RID: 48635
				public static LocString NAME = UI.FormatAsLink("Shipping", "BUILDCATEGORYCONVEYANCE");

				// Token: 0x0400BDFC RID: 48636
				public static LocString TOOLTIP = "Transport ore and solid materials around my base. {Hotkey}";
			}

			// Token: 0x02002B8B RID: 11147
			public static class OXYGEN
			{
				// Token: 0x0400BDFD RID: 48637
				public static LocString NAME = UI.FormatAsLink("Oxygen", "BUILDCATEGORYOXYGEN");

				// Token: 0x0400BDFE RID: 48638
				public static LocString TOOLTIP = "Everything I need to keep the colony breathing. {Hotkey}";
			}

			// Token: 0x02002B8C RID: 11148
			public static class POWER
			{
				// Token: 0x0400BDFF RID: 48639
				public static LocString NAME = UI.FormatAsLink("Power", "BUILDCATEGORYPOWER");

				// Token: 0x0400BE00 RID: 48640
				public static LocString TOOLTIP = "Need to power the colony? Here's how to do it! {Hotkey}";
			}

			// Token: 0x02002B8D RID: 11149
			public static class FOOD
			{
				// Token: 0x0400BE01 RID: 48641
				public static LocString NAME = UI.FormatAsLink("Food", "BUILDCATEGORYFOOD");

				// Token: 0x0400BE02 RID: 48642
				public static LocString TOOLTIP = "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
			}

			// Token: 0x02002B8E RID: 11150
			public static class UTILITIES
			{
				// Token: 0x0400BE03 RID: 48643
				public static LocString NAME = UI.FormatAsLink("Utilities", "BUILDCATEGORYUTILITIES");

				// Token: 0x0400BE04 RID: 48644
				public static LocString TOOLTIP = "Heat up and cool down. {Hotkey}";
			}

			// Token: 0x02002B8F RID: 11151
			public static class PLUMBING
			{
				// Token: 0x0400BE05 RID: 48645
				public static LocString NAME = UI.FormatAsLink("Plumbing", "BUILDCATEGORYPLUMBING");

				// Token: 0x0400BE06 RID: 48646
				public static LocString TOOLTIP = "Get the colony's water running and its sewage flowing. {Hotkey}";
			}

			// Token: 0x02002B90 RID: 11152
			public static class HVAC
			{
				// Token: 0x0400BE07 RID: 48647
				public static LocString NAME = UI.FormatAsLink("Ventilation", "BUILDCATEGORYHVAC");

				// Token: 0x0400BE08 RID: 48648
				public static LocString TOOLTIP = "Control the flow of gas in the base. {Hotkey}";
			}

			// Token: 0x02002B91 RID: 11153
			public static class REFINING
			{
				// Token: 0x0400BE09 RID: 48649
				public static LocString NAME = UI.FormatAsLink("Refinement", "BUILDCATEGORYREFINING");

				// Token: 0x0400BE0A RID: 48650
				public static LocString TOOLTIP = "Use the resources I want, filter the ones I don't. {Hotkey}";
			}

			// Token: 0x02002B92 RID: 11154
			public static class ROCKETRY
			{
				// Token: 0x0400BE0B RID: 48651
				public static LocString NAME = UI.FormatAsLink("Rocketry", "BUILDCATEGORYROCKETRY");

				// Token: 0x0400BE0C RID: 48652
				public static LocString TOOLTIP = "With rockets, the sky's no longer the limit! {Hotkey}";
			}

			// Token: 0x02002B93 RID: 11155
			public static class MEDICAL
			{
				// Token: 0x0400BE0D RID: 48653
				public static LocString NAME = UI.FormatAsLink("Medicine", "BUILDCATEGORYMEDICAL");

				// Token: 0x0400BE0E RID: 48654
				public static LocString TOOLTIP = "A cure for everything but the common cold. {Hotkey}";
			}

			// Token: 0x02002B94 RID: 11156
			public static class FURNITURE
			{
				// Token: 0x0400BE0F RID: 48655
				public static LocString NAME = UI.FormatAsLink("Furniture", "BUILDCATEGORYFURNITURE");

				// Token: 0x0400BE10 RID: 48656
				public static LocString TOOLTIP = "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
			}

			// Token: 0x02002B95 RID: 11157
			public static class EQUIPMENT
			{
				// Token: 0x0400BE11 RID: 48657
				public static LocString NAME = UI.FormatAsLink("Stations", "BUILDCATEGORYEQUIPMENT");

				// Token: 0x0400BE12 RID: 48658
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x02002B96 RID: 11158
			public static class MISC
			{
				// Token: 0x0400BE13 RID: 48659
				public static LocString NAME = UI.FormatAsLink("Decor", "BUILDCATEGORYMISC");

				// Token: 0x0400BE14 RID: 48660
				public static LocString TOOLTIP = "Spruce up my colony with some lovely interior decorating. {Hotkey}";
			}

			// Token: 0x02002B97 RID: 11159
			public static class AUTOMATION
			{
				// Token: 0x0400BE15 RID: 48661
				public static LocString NAME = UI.FormatAsLink("Automation", "BUILDCATEGORYAUTOMATION");

				// Token: 0x0400BE16 RID: 48662
				public static LocString TOOLTIP = "Automate my base with a wide range of sensors. {Hotkey}";
			}

			// Token: 0x02002B98 RID: 11160
			public static class HEP
			{
				// Token: 0x0400BE17 RID: 48663
				public static LocString NAME = UI.FormatAsLink("Radiation", "BUILDCATEGORYHEP");

				// Token: 0x0400BE18 RID: 48664
				public static LocString TOOLTIP = "Here's where things get rad. {Hotkey}";
			}
		}

		// Token: 0x02002180 RID: 8576
		public class NEWBUILDCATEGORIES
		{
			// Token: 0x02002B99 RID: 11161
			public static class BASE
			{
				// Token: 0x0400BE19 RID: 48665
				public static LocString NAME = UI.FormatAsLink("Base", "BUILD_CATEGORY_BASE");

				// Token: 0x0400BE1A RID: 48666
				public static LocString TOOLTIP = "Maintain the colony's infrastructure with these homebase basics. {Hotkey}";
			}

			// Token: 0x02002B9A RID: 11162
			public static class INFRASTRUCTURE
			{
				// Token: 0x0400BE1B RID: 48667
				public static LocString NAME = UI.FormatAsLink("Utilities", "BUILD_CATEGORY_INFRASTRUCTURE");

				// Token: 0x0400BE1C RID: 48668
				public static LocString TOOLTIP = "Power, plumbing, and ventilation can all be found here. {Hotkey}";
			}

			// Token: 0x02002B9B RID: 11163
			public static class FOODANDAGRICULTURE
			{
				// Token: 0x0400BE1D RID: 48669
				public static LocString NAME = UI.FormatAsLink("Food", "BUILD_CATEGORY_FOODANDAGRICULTURE");

				// Token: 0x0400BE1E RID: 48670
				public static LocString TOOLTIP = "Keep my Duplicants' spirits high and their bellies full. {Hotkey}";
			}

			// Token: 0x02002B9C RID: 11164
			public static class LOGISTICS
			{
				// Token: 0x0400BE1F RID: 48671
				public static LocString NAME = UI.FormatAsLink("Logistics", "BUILD_CATEGORY_LOGISTICS");

				// Token: 0x0400BE20 RID: 48672
				public static LocString TOOLTIP = "Devices for base automation and material transport. {Hotkey}";
			}

			// Token: 0x02002B9D RID: 11165
			public static class HEALTHANDHAPPINESS
			{
				// Token: 0x0400BE21 RID: 48673
				public static LocString NAME = UI.FormatAsLink("Accommodation", "BUILD_CATEGORY_HEALTHANDHAPPINESS");

				// Token: 0x0400BE22 RID: 48674
				public static LocString TOOLTIP = "Everything a Duplicant needs to stay happy, healthy, and fulfilled. {Hotkey}";
			}

			// Token: 0x02002B9E RID: 11166
			public static class INDUSTRIAL
			{
				// Token: 0x0400BE23 RID: 48675
				public static LocString NAME = UI.FormatAsLink("Industrials", "BUILD_CATEGORY_INDUSTRIAL");

				// Token: 0x0400BE24 RID: 48676
				public static LocString TOOLTIP = "Machinery for oxygen production, heat management, and material refinement. {Hotkey}";
			}

			// Token: 0x02002B9F RID: 11167
			public static class LADDERS
			{
				// Token: 0x0400BE25 RID: 48677
				public static LocString NAME = "Ladders";

				// Token: 0x0400BE26 RID: 48678
				public static LocString BUILDMENUTITLE = "Ladders";

				// Token: 0x0400BE27 RID: 48679
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA0 RID: 11168
			public static class TILES
			{
				// Token: 0x0400BE28 RID: 48680
				public static LocString NAME = "Tiles and Drywall";

				// Token: 0x0400BE29 RID: 48681
				public static LocString BUILDMENUTITLE = "Tiles and Drywall";

				// Token: 0x0400BE2A RID: 48682
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA1 RID: 11169
			public static class PRINTINGPODS
			{
				// Token: 0x0400BE2B RID: 48683
				public static LocString NAME = "Printing Pods";

				// Token: 0x0400BE2C RID: 48684
				public static LocString BUILDMENUTITLE = "Printing Pods";

				// Token: 0x0400BE2D RID: 48685
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA2 RID: 11170
			public static class DOORS
			{
				// Token: 0x0400BE2E RID: 48686
				public static LocString NAME = "Doors";

				// Token: 0x0400BE2F RID: 48687
				public static LocString BUILDMENUTITLE = "Doors";

				// Token: 0x0400BE30 RID: 48688
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA3 RID: 11171
			public static class STORAGE
			{
				// Token: 0x0400BE31 RID: 48689
				public static LocString NAME = "Storage";

				// Token: 0x0400BE32 RID: 48690
				public static LocString BUILDMENUTITLE = "Storage";

				// Token: 0x0400BE33 RID: 48691
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA4 RID: 11172
			public static class TRANSPORT
			{
				// Token: 0x0400BE34 RID: 48692
				public static LocString NAME = "Transit Tubes";

				// Token: 0x0400BE35 RID: 48693
				public static LocString BUILDMENUTITLE = "Transit Tubes";

				// Token: 0x0400BE36 RID: 48694
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA5 RID: 11173
			public static class OPERATIONS
			{
				// Token: 0x0400BE37 RID: 48695
				public static LocString NAME = "Operations";

				// Token: 0x0400BE38 RID: 48696
				public static LocString BUILDMENUTITLE = "Operations";

				// Token: 0x0400BE39 RID: 48697
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA6 RID: 11174
			public static class PRODUCERS
			{
				// Token: 0x0400BE3A RID: 48698
				public static LocString NAME = "Production";

				// Token: 0x0400BE3B RID: 48699
				public static LocString BUILDMENUTITLE = "Production";

				// Token: 0x0400BE3C RID: 48700
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA7 RID: 11175
			public static class SCRUBBERS
			{
				// Token: 0x0400BE3D RID: 48701
				public static LocString NAME = "Purification";

				// Token: 0x0400BE3E RID: 48702
				public static LocString BUILDMENUTITLE = "Purification";

				// Token: 0x0400BE3F RID: 48703
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA8 RID: 11176
			public static class BATTERIES
			{
				// Token: 0x0400BE40 RID: 48704
				public static LocString NAME = "Batteries";

				// Token: 0x0400BE41 RID: 48705
				public static LocString BUILDMENUTITLE = "Batteries";

				// Token: 0x0400BE42 RID: 48706
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BA9 RID: 11177
			public static class SWITCHES
			{
				// Token: 0x0400BE43 RID: 48707
				public static LocString NAME = "Switches";

				// Token: 0x0400BE44 RID: 48708
				public static LocString BUILDMENUTITLE = "Switches";

				// Token: 0x0400BE45 RID: 48709
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BAA RID: 11178
			public static class COOKING
			{
				// Token: 0x0400BE46 RID: 48710
				public static LocString NAME = "Cooking";

				// Token: 0x0400BE47 RID: 48711
				public static LocString BUILDMENUTITLE = "Cooking";

				// Token: 0x0400BE48 RID: 48712
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BAB RID: 11179
			public static class FARMING
			{
				// Token: 0x0400BE49 RID: 48713
				public static LocString NAME = "Farming";

				// Token: 0x0400BE4A RID: 48714
				public static LocString BUILDMENUTITLE = "Farming";

				// Token: 0x0400BE4B RID: 48715
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BAC RID: 11180
			public static class RANCHING
			{
				// Token: 0x0400BE4C RID: 48716
				public static LocString NAME = "Ranching";

				// Token: 0x0400BE4D RID: 48717
				public static LocString BUILDMENUTITLE = "Ranching";

				// Token: 0x0400BE4E RID: 48718
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BAD RID: 11181
			public static class WASHROOM
			{
				// Token: 0x0400BE4F RID: 48719
				public static LocString NAME = "Washroom";

				// Token: 0x0400BE50 RID: 48720
				public static LocString BUILDMENUTITLE = "Washroom";

				// Token: 0x0400BE51 RID: 48721
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BAE RID: 11182
			public static class VALVES
			{
				// Token: 0x0400BE52 RID: 48722
				public static LocString NAME = "Valves";

				// Token: 0x0400BE53 RID: 48723
				public static LocString BUILDMENUTITLE = "Valves";

				// Token: 0x0400BE54 RID: 48724
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BAF RID: 11183
			public static class PUMPS
			{
				// Token: 0x0400BE55 RID: 48725
				public static LocString NAME = "Pumps";

				// Token: 0x0400BE56 RID: 48726
				public static LocString BUILDMENUTITLE = "Pumps";

				// Token: 0x0400BE57 RID: 48727
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB0 RID: 11184
			public static class SENSORS
			{
				// Token: 0x0400BE58 RID: 48728
				public static LocString NAME = "Sensors";

				// Token: 0x0400BE59 RID: 48729
				public static LocString BUILDMENUTITLE = "Sensors";

				// Token: 0x0400BE5A RID: 48730
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB1 RID: 11185
			public static class PORTS
			{
				// Token: 0x0400BE5B RID: 48731
				public static LocString NAME = "Ports";

				// Token: 0x0400BE5C RID: 48732
				public static LocString BUILDMENUTITLE = "Ports";

				// Token: 0x0400BE5D RID: 48733
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB2 RID: 11186
			public static class MATERIALS
			{
				// Token: 0x0400BE5E RID: 48734
				public static LocString NAME = "Materials";

				// Token: 0x0400BE5F RID: 48735
				public static LocString BUILDMENUTITLE = "Materials";

				// Token: 0x0400BE60 RID: 48736
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB3 RID: 11187
			public static class OIL
			{
				// Token: 0x0400BE61 RID: 48737
				public static LocString NAME = "Oil";

				// Token: 0x0400BE62 RID: 48738
				public static LocString BUILDMENUTITLE = "Oil";

				// Token: 0x0400BE63 RID: 48739
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB4 RID: 11188
			public static class ADVANCED
			{
				// Token: 0x0400BE64 RID: 48740
				public static LocString NAME = "Advanced";

				// Token: 0x0400BE65 RID: 48741
				public static LocString BUILDMENUTITLE = "Advanced";

				// Token: 0x0400BE66 RID: 48742
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB5 RID: 11189
			public static class ORGANIC
			{
				// Token: 0x0400BE67 RID: 48743
				public static LocString NAME = "Organic";

				// Token: 0x0400BE68 RID: 48744
				public static LocString BUILDMENUTITLE = "Organic";

				// Token: 0x0400BE69 RID: 48745
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB6 RID: 11190
			public static class BEDS
			{
				// Token: 0x0400BE6A RID: 48746
				public static LocString NAME = "Beds";

				// Token: 0x0400BE6B RID: 48747
				public static LocString BUILDMENUTITLE = "Beds";

				// Token: 0x0400BE6C RID: 48748
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB7 RID: 11191
			public static class LIGHTS
			{
				// Token: 0x0400BE6D RID: 48749
				public static LocString NAME = "Lights";

				// Token: 0x0400BE6E RID: 48750
				public static LocString BUILDMENUTITLE = "Lights";

				// Token: 0x0400BE6F RID: 48751
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB8 RID: 11192
			public static class DINING
			{
				// Token: 0x0400BE70 RID: 48752
				public static LocString NAME = "Dining";

				// Token: 0x0400BE71 RID: 48753
				public static LocString BUILDMENUTITLE = "Dining";

				// Token: 0x0400BE72 RID: 48754
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BB9 RID: 11193
			public static class MANUFACTURING
			{
				// Token: 0x0400BE73 RID: 48755
				public static LocString NAME = "Manufacturing";

				// Token: 0x0400BE74 RID: 48756
				public static LocString BUILDMENUTITLE = "Manufacturing";

				// Token: 0x0400BE75 RID: 48757
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BBA RID: 11194
			public static class TEMPERATURE
			{
				// Token: 0x0400BE76 RID: 48758
				public static LocString NAME = "Temperature";

				// Token: 0x0400BE77 RID: 48759
				public static LocString BUILDMENUTITLE = "Temperature";

				// Token: 0x0400BE78 RID: 48760
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BBB RID: 11195
			public static class RESEARCH
			{
				// Token: 0x0400BE79 RID: 48761
				public static LocString NAME = "Research";

				// Token: 0x0400BE7A RID: 48762
				public static LocString BUILDMENUTITLE = "Research";

				// Token: 0x0400BE7B RID: 48763
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BBC RID: 11196
			public static class GENERATORS
			{
				// Token: 0x0400BE7C RID: 48764
				public static LocString NAME = "Generators";

				// Token: 0x0400BE7D RID: 48765
				public static LocString BUILDMENUTITLE = "Generators";

				// Token: 0x0400BE7E RID: 48766
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BBD RID: 11197
			public static class WIRES
			{
				// Token: 0x0400BE7F RID: 48767
				public static LocString NAME = "Wires";

				// Token: 0x0400BE80 RID: 48768
				public static LocString BUILDMENUTITLE = "Wires";

				// Token: 0x0400BE81 RID: 48769
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BBE RID: 11198
			public static class ELECTROBANKBUILDINGS
			{
				// Token: 0x0400BE82 RID: 48770
				public static LocString NAME = "Converters";

				// Token: 0x0400BE83 RID: 48771
				public static LocString BUILDMENUTITLE = "Converters";

				// Token: 0x0400BE84 RID: 48772
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BBF RID: 11199
			public static class LOGICGATES
			{
				// Token: 0x0400BE85 RID: 48773
				public static LocString NAME = "Gates";

				// Token: 0x0400BE86 RID: 48774
				public static LocString BUILDMENUTITLE = "Gates";

				// Token: 0x0400BE87 RID: 48775
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BC0 RID: 11200
			public static class TRANSMISSIONS
			{
				// Token: 0x0400BE88 RID: 48776
				public static LocString NAME = "Transmissions";

				// Token: 0x0400BE89 RID: 48777
				public static LocString BUILDMENUTITLE = "Transmissions";

				// Token: 0x0400BE8A RID: 48778
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BC1 RID: 11201
			public static class LOGICMANAGER
			{
				// Token: 0x0400BE8B RID: 48779
				public static LocString NAME = "Monitoring";

				// Token: 0x0400BE8C RID: 48780
				public static LocString BUILDMENUTITLE = "Monitoring";

				// Token: 0x0400BE8D RID: 48781
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BC2 RID: 11202
			public static class LOGICAUDIO
			{
				// Token: 0x0400BE8E RID: 48782
				public static LocString NAME = "Ambience";

				// Token: 0x0400BE8F RID: 48783
				public static LocString BUILDMENUTITLE = "Ambience";

				// Token: 0x0400BE90 RID: 48784
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BC3 RID: 11203
			public static class CONVEYANCESTRUCTURES
			{
				// Token: 0x0400BE91 RID: 48785
				public static LocString NAME = "Structural";

				// Token: 0x0400BE92 RID: 48786
				public static LocString BUILDMENUTITLE = "Structural";

				// Token: 0x0400BE93 RID: 48787
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BC4 RID: 11204
			public static class BUILDMENUPORTS
			{
				// Token: 0x0400BE94 RID: 48788
				public static LocString NAME = "Ports";

				// Token: 0x0400BE95 RID: 48789
				public static LocString BUILDMENUTITLE = "Ports";

				// Token: 0x0400BE96 RID: 48790
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BC5 RID: 11205
			public static class POWERCONTROL
			{
				// Token: 0x0400BE97 RID: 48791
				public static LocString NAME = "Power\nRegulation";

				// Token: 0x0400BE98 RID: 48792
				public static LocString BUILDMENUTITLE = "Power Regulation";

				// Token: 0x0400BE99 RID: 48793
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BC6 RID: 11206
			public static class PLUMBINGSTRUCTURES
			{
				// Token: 0x0400BE9A RID: 48794
				public static LocString NAME = "Plumbing";

				// Token: 0x0400BE9B RID: 48795
				public static LocString BUILDMENUTITLE = "Plumbing";

				// Token: 0x0400BE9C RID: 48796
				public static LocString TOOLTIP = "Get the colony's water running and its sewage flowing. {Hotkey}";
			}

			// Token: 0x02002BC7 RID: 11207
			public static class PIPES
			{
				// Token: 0x0400BE9D RID: 48797
				public static LocString NAME = "Pipes";

				// Token: 0x0400BE9E RID: 48798
				public static LocString BUILDMENUTITLE = "Pipes";

				// Token: 0x0400BE9F RID: 48799
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BC8 RID: 11208
			public static class VENTILATIONSTRUCTURES
			{
				// Token: 0x0400BEA0 RID: 48800
				public static LocString NAME = "Ventilation";

				// Token: 0x0400BEA1 RID: 48801
				public static LocString BUILDMENUTITLE = "Ventilation";

				// Token: 0x0400BEA2 RID: 48802
				public static LocString TOOLTIP = "Control the flow of gas in your base. {Hotkey}";
			}

			// Token: 0x02002BC9 RID: 11209
			public static class CONVEYANCE
			{
				// Token: 0x0400BEA3 RID: 48803
				public static LocString NAME = "Ore\nTransport";

				// Token: 0x0400BEA4 RID: 48804
				public static LocString BUILDMENUTITLE = "Ore Transport";

				// Token: 0x0400BEA5 RID: 48805
				public static LocString TOOLTIP = "Transport ore and solid materials around my base. {Hotkey}";
			}

			// Token: 0x02002BCA RID: 11210
			public static class HYGIENE
			{
				// Token: 0x0400BEA6 RID: 48806
				public static LocString NAME = "Hygiene";

				// Token: 0x0400BEA7 RID: 48807
				public static LocString BUILDMENUTITLE = "Hygiene";

				// Token: 0x0400BEA8 RID: 48808
				public static LocString TOOLTIP = "Keeps my Duplicants clean.";
			}

			// Token: 0x02002BCB RID: 11211
			public static class MEDICAL
			{
				// Token: 0x0400BEA9 RID: 48809
				public static LocString NAME = "Medical";

				// Token: 0x0400BEAA RID: 48810
				public static LocString BUILDMENUTITLE = "Medical";

				// Token: 0x0400BEAB RID: 48811
				public static LocString TOOLTIP = "A cure for everything but the common cold. {Hotkey}";
			}

			// Token: 0x02002BCC RID: 11212
			public static class WELLNESS
			{
				// Token: 0x0400BEAC RID: 48812
				public static LocString NAME = "Wellness";

				// Token: 0x0400BEAD RID: 48813
				public static LocString BUILDMENUTITLE = "Wellness";

				// Token: 0x0400BEAE RID: 48814
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BCD RID: 11213
			public static class RECREATION
			{
				// Token: 0x0400BEAF RID: 48815
				public static LocString NAME = "Recreation";

				// Token: 0x0400BEB0 RID: 48816
				public static LocString BUILDMENUTITLE = "Recreation";

				// Token: 0x0400BEB1 RID: 48817
				public static LocString TOOLTIP = "Everything needed to reduce stress and increase fun.";
			}

			// Token: 0x02002BCE RID: 11214
			public static class FURNITURE
			{
				// Token: 0x0400BEB2 RID: 48818
				public static LocString NAME = "Furniture";

				// Token: 0x0400BEB3 RID: 48819
				public static LocString BUILDMENUTITLE = "Furniture";

				// Token: 0x0400BEB4 RID: 48820
				public static LocString TOOLTIP = "Amenities to keep my Duplicants happy, comfy and efficient. {Hotkey}";
			}

			// Token: 0x02002BCF RID: 11215
			public static class DECOR
			{
				// Token: 0x0400BEB5 RID: 48821
				public static LocString NAME = "Decor";

				// Token: 0x0400BEB6 RID: 48822
				public static LocString BUILDMENUTITLE = "Decor";

				// Token: 0x0400BEB7 RID: 48823
				public static LocString TOOLTIP = "Spruce up your colony with some lovely interior decorating. {Hotkey}";
			}

			// Token: 0x02002BD0 RID: 11216
			public static class OXYGEN
			{
				// Token: 0x0400BEB8 RID: 48824
				public static LocString NAME = "Oxygen";

				// Token: 0x0400BEB9 RID: 48825
				public static LocString BUILDMENUTITLE = "Oxygen";

				// Token: 0x0400BEBA RID: 48826
				public static LocString TOOLTIP = "Everything I need to keep my colony breathing. {Hotkey}";
			}

			// Token: 0x02002BD1 RID: 11217
			public static class UTILITIES
			{
				// Token: 0x0400BEBB RID: 48827
				public static LocString NAME = "Temperature";

				// Token: 0x0400BEBC RID: 48828
				public static LocString BUILDMENUTITLE = "Temperature";

				// Token: 0x0400BEBD RID: 48829
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BD2 RID: 11218
			public static class REFINING
			{
				// Token: 0x0400BEBE RID: 48830
				public static LocString NAME = "Refinement";

				// Token: 0x0400BEBF RID: 48831
				public static LocString BUILDMENUTITLE = "Refinement";

				// Token: 0x0400BEC0 RID: 48832
				public static LocString TOOLTIP = "Use the resources you want, filter the ones you don't. {Hotkey}";
			}

			// Token: 0x02002BD3 RID: 11219
			public static class EQUIPMENT
			{
				// Token: 0x0400BEC1 RID: 48833
				public static LocString NAME = "Equipment";

				// Token: 0x0400BEC2 RID: 48834
				public static LocString BUILDMENUTITLE = "Equipment";

				// Token: 0x0400BEC3 RID: 48835
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x02002BD4 RID: 11220
			public static class ARCHAEOLOGY
			{
				// Token: 0x0400BEC4 RID: 48836
				public static LocString NAME = "Archaeology";

				// Token: 0x0400BEC5 RID: 48837
				public static LocString BUILDMENUTITLE = "Archaeology";

				// Token: 0x0400BEC6 RID: 48838
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BD5 RID: 11221
			public static class METEORDEFENSE
			{
				// Token: 0x0400BEC7 RID: 48839
				public static LocString NAME = "Meteor Defense";

				// Token: 0x0400BEC8 RID: 48840
				public static LocString BUILDMENUTITLE = "Meteor Defense";

				// Token: 0x0400BEC9 RID: 48841
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BD6 RID: 11222
			public static class INDUSTRIALSTATION
			{
				// Token: 0x0400BECA RID: 48842
				public static LocString NAME = "Industrial";

				// Token: 0x0400BECB RID: 48843
				public static LocString BUILDMENUTITLE = "Industrial";

				// Token: 0x0400BECC RID: 48844
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BD7 RID: 11223
			public static class TELESCOPES
			{
				// Token: 0x0400BECD RID: 48845
				public static LocString NAME = "Telescopes";

				// Token: 0x0400BECE RID: 48846
				public static LocString BUILDMENUTITLE = "Telescopes";

				// Token: 0x0400BECF RID: 48847
				public static LocString TOOLTIP = "Unlock new technologies through the power of science! {Hotkey}";
			}

			// Token: 0x02002BD8 RID: 11224
			public static class MISSILES
			{
				// Token: 0x0400BED0 RID: 48848
				public static LocString NAME = "Meteor Defense";

				// Token: 0x0400BED1 RID: 48849
				public static LocString BUILDMENUTITLE = "Meteor Defense";

				// Token: 0x0400BED2 RID: 48850
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BD9 RID: 11225
			public static class FITTINGS
			{
				// Token: 0x0400BED3 RID: 48851
				public static LocString NAME = "Fittings";

				// Token: 0x0400BED4 RID: 48852
				public static LocString BUILDMENUTITLE = "Fittings";

				// Token: 0x0400BED5 RID: 48853
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BDA RID: 11226
			public static class SANITATION
			{
				// Token: 0x0400BED6 RID: 48854
				public static LocString NAME = "Sanitation";

				// Token: 0x0400BED7 RID: 48855
				public static LocString BUILDMENUTITLE = "Sanitation";

				// Token: 0x0400BED8 RID: 48856
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BDB RID: 11227
			public static class AUTOMATED
			{
				// Token: 0x0400BED9 RID: 48857
				public static LocString NAME = "Automated";

				// Token: 0x0400BEDA RID: 48858
				public static LocString BUILDMENUTITLE = "Automated";

				// Token: 0x0400BEDB RID: 48859
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BDC RID: 11228
			public static class ROCKETSTRUCTURES
			{
				// Token: 0x0400BEDC RID: 48860
				public static LocString NAME = "Structural";

				// Token: 0x0400BEDD RID: 48861
				public static LocString BUILDMENUTITLE = "Structural";

				// Token: 0x0400BEDE RID: 48862
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BDD RID: 11229
			public static class ROCKETNAV
			{
				// Token: 0x0400BEDF RID: 48863
				public static LocString NAME = "Navigation";

				// Token: 0x0400BEE0 RID: 48864
				public static LocString BUILDMENUTITLE = "Navigation";

				// Token: 0x0400BEE1 RID: 48865
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BDE RID: 11230
			public static class CONDUITSENSORS
			{
				// Token: 0x0400BEE2 RID: 48866
				public static LocString NAME = "Pipe Sensors";

				// Token: 0x0400BEE3 RID: 48867
				public static LocString BUILDMENUTITLE = "Pipe Sensors";

				// Token: 0x0400BEE4 RID: 48868
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BDF RID: 11231
			public static class ROCKETRY
			{
				// Token: 0x0400BEE5 RID: 48869
				public static LocString NAME = "Rocketry";

				// Token: 0x0400BEE6 RID: 48870
				public static LocString BUILDMENUTITLE = "Rocketry";

				// Token: 0x0400BEE7 RID: 48871
				public static LocString TOOLTIP = "Rocketry {Hotkey}";
			}

			// Token: 0x02002BE0 RID: 11232
			public static class ENGINES
			{
				// Token: 0x0400BEE8 RID: 48872
				public static LocString NAME = "Engines";

				// Token: 0x0400BEE9 RID: 48873
				public static LocString BUILDMENUTITLE = "Engines";

				// Token: 0x0400BEEA RID: 48874
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BE1 RID: 11233
			public static class TANKS
			{
				// Token: 0x0400BEEB RID: 48875
				public static LocString NAME = "Tanks";

				// Token: 0x0400BEEC RID: 48876
				public static LocString BUILDMENUTITLE = "Tanks";

				// Token: 0x0400BEED RID: 48877
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BE2 RID: 11234
			public static class CARGO
			{
				// Token: 0x0400BEEE RID: 48878
				public static LocString NAME = "Cargo";

				// Token: 0x0400BEEF RID: 48879
				public static LocString BUILDMENUTITLE = "Cargo";

				// Token: 0x0400BEF0 RID: 48880
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002BE3 RID: 11235
			public static class MODULE
			{
				// Token: 0x0400BEF1 RID: 48881
				public static LocString NAME = "Modules";

				// Token: 0x0400BEF2 RID: 48882
				public static LocString BUILDMENUTITLE = "Modules";

				// Token: 0x0400BEF3 RID: 48883
				public static LocString TOOLTIP = "";
			}
		}

		// Token: 0x02002181 RID: 8577
		public class TOOLS
		{
			// Token: 0x040097C5 RID: 38853
			public static LocString TOOL_AREA_FMT = "{0} x {1}\n{2} tiles";

			// Token: 0x040097C6 RID: 38854
			public static LocString TOOL_LENGTH_FMT = "{0}";

			// Token: 0x040097C7 RID: 38855
			public static LocString FILTER_HOVERCARD_HEADER = "   <style=\"hovercard_element\">({0})</style>";

			// Token: 0x040097C8 RID: 38856
			public static LocString CAPITALS = "<uppercase>{0}</uppercase>";

			// Token: 0x02002BE4 RID: 11236
			public class SANDBOX
			{
				// Token: 0x02003746 RID: 14150
				public class SANDBOX_TOGGLE
				{
					// Token: 0x0400DC2C RID: 56364
					public static LocString NAME = "SANDBOX";
				}

				// Token: 0x02003747 RID: 14151
				public class BRUSH
				{
					// Token: 0x0400DC2D RID: 56365
					public static LocString NAME = "Brush";

					// Token: 0x0400DC2E RID: 56366
					public static LocString HOVERACTION = "PAINT SIM";
				}

				// Token: 0x02003748 RID: 14152
				public class SPRINKLE
				{
					// Token: 0x0400DC2F RID: 56367
					public static LocString NAME = "Sprinkle";

					// Token: 0x0400DC30 RID: 56368
					public static LocString HOVERACTION = "SPRINKLE SIM";
				}

				// Token: 0x02003749 RID: 14153
				public class FLOOD
				{
					// Token: 0x0400DC31 RID: 56369
					public static LocString NAME = "Fill";

					// Token: 0x0400DC32 RID: 56370
					public static LocString HOVERACTION = "PAINT SECTION";
				}

				// Token: 0x0200374A RID: 14154
				public class MARQUEE
				{
					// Token: 0x0400DC33 RID: 56371
					public static LocString NAME = "Marquee";
				}

				// Token: 0x0200374B RID: 14155
				public class SAMPLE
				{
					// Token: 0x0400DC34 RID: 56372
					public static LocString NAME = "Sample";

					// Token: 0x0400DC35 RID: 56373
					public static LocString HOVERACTION = "COPY SELECTION";
				}

				// Token: 0x0200374C RID: 14156
				public class HEATGUN
				{
					// Token: 0x0400DC36 RID: 56374
					public static LocString NAME = "Heat Gun";

					// Token: 0x0400DC37 RID: 56375
					public static LocString HOVERACTION = "PAINT HEAT";
				}

				// Token: 0x0200374D RID: 14157
				public class RADSTOOL
				{
					// Token: 0x0400DC38 RID: 56376
					public static LocString NAME = "Radiation Tool";

					// Token: 0x0400DC39 RID: 56377
					public static LocString HOVERACTION = "PAINT RADS";
				}

				// Token: 0x0200374E RID: 14158
				public class STRESSTOOL
				{
					// Token: 0x0400DC3A RID: 56378
					public static LocString NAME = "Happy Tool";

					// Token: 0x0400DC3B RID: 56379
					public static LocString HOVERACTION = "PAINT CALM";
				}

				// Token: 0x0200374F RID: 14159
				public class SPAWNER
				{
					// Token: 0x0400DC3C RID: 56380
					public static LocString NAME = "Spawner";

					// Token: 0x0400DC3D RID: 56381
					public static LocString HOVERACTION = "SPAWN";
				}

				// Token: 0x02003750 RID: 14160
				public class CLEAR_FLOOR
				{
					// Token: 0x0400DC3E RID: 56382
					public static LocString NAME = "Clear Floor";

					// Token: 0x0400DC3F RID: 56383
					public static LocString HOVERACTION = "DELETE DEBRIS";
				}

				// Token: 0x02003751 RID: 14161
				public class DESTROY
				{
					// Token: 0x0400DC40 RID: 56384
					public static LocString NAME = "Destroy";

					// Token: 0x0400DC41 RID: 56385
					public static LocString HOVERACTION = "DELETE";
				}

				// Token: 0x02003752 RID: 14162
				public class SPAWN_ENTITY
				{
					// Token: 0x0400DC42 RID: 56386
					public static LocString NAME = "Spawn";
				}

				// Token: 0x02003753 RID: 14163
				public class FOW
				{
					// Token: 0x0400DC43 RID: 56387
					public static LocString NAME = "Reveal";

					// Token: 0x0400DC44 RID: 56388
					public static LocString HOVERACTION = "DE-FOG";
				}

				// Token: 0x02003754 RID: 14164
				public class CRITTER
				{
					// Token: 0x0400DC45 RID: 56389
					public static LocString NAME = "Critter Removal";

					// Token: 0x0400DC46 RID: 56390
					public static LocString HOVERACTION = "DELETE CRITTERS";
				}

				// Token: 0x02003755 RID: 14165
				public class SPAWN_STORY_TRAIT
				{
					// Token: 0x0400DC47 RID: 56391
					public static LocString NAME = "Story Trait";

					// Token: 0x0400DC48 RID: 56392
					public static LocString HOVERACTION = "PLACE";

					// Token: 0x0400DC49 RID: 56393
					public static LocString ERROR_ALREADY_EXISTS = "{StoryName} already exists in this save";

					// Token: 0x0400DC4A RID: 56394
					public static LocString ERROR_INVALID_LOCATION = "Invalid location";

					// Token: 0x0400DC4B RID: 56395
					public static LocString ERROR_DUPE_HAZARD = "One or more Duplicants are in the way";

					// Token: 0x0400DC4C RID: 56396
					public static LocString ERROR_ROBOT_HAZARD = "One or more robots are in the way";

					// Token: 0x0400DC4D RID: 56397
					public static LocString ERROR_CREATURE_HAZARD = "One or more critters are in the way";

					// Token: 0x0400DC4E RID: 56398
					public static LocString ERROR_BUILDING_HAZARD = "One or more buildings are in the way";
				}
			}

			// Token: 0x02002BE5 RID: 11237
			public class GENERIC
			{
				// Token: 0x0400BEF4 RID: 48884
				public static LocString BACK = "Back";

				// Token: 0x0400BEF5 RID: 48885
				public static LocString UNKNOWN = "UNKNOWN";

				// Token: 0x0400BEF6 RID: 48886
				public static LocString BUILDING_HOVER_NAME_FMT = "{Name}    <style=\"hovercard_element\">({Element})</style>";

				// Token: 0x0400BEF7 RID: 48887
				public static LocString LOGIC_INPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400BEF8 RID: 48888
				public static LocString LOGIC_OUTPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400BEF9 RID: 48889
				public static LocString LOGIC_MULTI_INPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";

				// Token: 0x0400BEFA RID: 48890
				public static LocString LOGIC_MULTI_OUTPUT_HOVER_FMT = "{Port}    <style=\"hovercard_element\">({Name})</style>";
			}

			// Token: 0x02002BE6 RID: 11238
			public class ATTACK
			{
				// Token: 0x0400BEFB RID: 48891
				public static LocString NAME = "Attack";

				// Token: 0x0400BEFC RID: 48892
				public static LocString TOOLNAME = "Attack tool";

				// Token: 0x0400BEFD RID: 48893
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BE7 RID: 11239
			public class CAPTURE
			{
				// Token: 0x0400BEFE RID: 48894
				public static LocString NAME = "Wrangle";

				// Token: 0x0400BEFF RID: 48895
				public static LocString TOOLNAME = "Wrangle tool";

				// Token: 0x0400BF00 RID: 48896
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400BF01 RID: 48897
				public static LocString NOT_CAPTURABLE = "Cannot Wrangle";
			}

			// Token: 0x02002BE8 RID: 11240
			public class BUILD
			{
				// Token: 0x0400BF02 RID: 48898
				public static LocString NAME = "Build {0}";

				// Token: 0x0400BF03 RID: 48899
				public static LocString TOOLNAME = "Build tool";

				// Token: 0x0400BF04 RID: 48900
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) + " TO BUILD";

				// Token: 0x0400BF05 RID: 48901
				public static LocString TOOLACTION_DRAG = "DRAG";
			}

			// Token: 0x02002BE9 RID: 11241
			public class PLACE
			{
				// Token: 0x0400BF06 RID: 48902
				public static LocString NAME = "Place {0}";

				// Token: 0x0400BF07 RID: 48903
				public static LocString TOOLNAME = "Place tool";

				// Token: 0x0400BF08 RID: 48904
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) + " TO PLACE";

				// Token: 0x02003756 RID: 14166
				public class REASONS
				{
					// Token: 0x0400DC4F RID: 56399
					public static LocString CAN_OCCUPY_AREA = "Location blocked";

					// Token: 0x0400DC50 RID: 56400
					public static LocString ON_FOUNDATION = "Must place on the ground";

					// Token: 0x0400DC51 RID: 56401
					public static LocString VISIBLE_TO_SPACE = "Must have a clear path to space";

					// Token: 0x0400DC52 RID: 56402
					public static LocString RESTRICT_TO_WORLD = "Incorrect " + UI.CLUSTERMAP.PLANETOID;
				}
			}

			// Token: 0x02002BEA RID: 11242
			public class MOVETOLOCATION
			{
				// Token: 0x0400BF09 RID: 48905
				public static LocString NAME = "Relocate";

				// Token: 0x0400BF0A RID: 48906
				public static LocString TOOLNAME = "Relocate Tool";

				// Token: 0x0400BF0B RID: 48907
				public static LocString TOOLACTION = UI.CLICK(UI.ClickType.CLICK) ?? "";

				// Token: 0x0400BF0C RID: 48908
				public static LocString UNREACHABLE = "UNREACHABLE";
			}

			// Token: 0x02002BEB RID: 11243
			public class COPYSETTINGS
			{
				// Token: 0x0400BF0D RID: 48909
				public static LocString NAME = "Paste Settings";

				// Token: 0x0400BF0E RID: 48910
				public static LocString TOOLNAME = "Paste Settings Tool";

				// Token: 0x0400BF0F RID: 48911
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BEC RID: 11244
			public class DIG
			{
				// Token: 0x0400BF10 RID: 48912
				public static LocString NAME = "Dig";

				// Token: 0x0400BF11 RID: 48913
				public static LocString TOOLNAME = "Dig tool";

				// Token: 0x0400BF12 RID: 48914
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BED RID: 11245
			public class DISINFECT
			{
				// Token: 0x0400BF13 RID: 48915
				public static LocString NAME = "Disinfect";

				// Token: 0x0400BF14 RID: 48916
				public static LocString TOOLNAME = "Disinfect tool";

				// Token: 0x0400BF15 RID: 48917
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BEE RID: 11246
			public class DISCONNECT
			{
				// Token: 0x0400BF16 RID: 48918
				public static LocString NAME = "Disconnect";

				// Token: 0x0400BF17 RID: 48919
				public static LocString TOOLTIP = "Sever conduits and connectors {Hotkey}";

				// Token: 0x0400BF18 RID: 48920
				public static LocString TOOLNAME = "Disconnect tool";

				// Token: 0x0400BF19 RID: 48921
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BEF RID: 11247
			public class CANCEL
			{
				// Token: 0x0400BF1A RID: 48922
				public static LocString NAME = "Cancel";

				// Token: 0x0400BF1B RID: 48923
				public static LocString TOOLNAME = "Cancel tool";

				// Token: 0x0400BF1C RID: 48924
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BF0 RID: 11248
			public class DECONSTRUCT
			{
				// Token: 0x0400BF1D RID: 48925
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400BF1E RID: 48926
				public static LocString TOOLNAME = "Deconstruct tool";

				// Token: 0x0400BF1F RID: 48927
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BF1 RID: 11249
			public class CLEANUPCATEGORY
			{
				// Token: 0x0400BF20 RID: 48928
				public static LocString NAME = "Clean";

				// Token: 0x0400BF21 RID: 48929
				public static LocString TOOLNAME = "Clean Up tools";
			}

			// Token: 0x02002BF2 RID: 11250
			public class PRIORITIESCATEGORY
			{
				// Token: 0x0400BF22 RID: 48930
				public static LocString NAME = "Priority";
			}

			// Token: 0x02002BF3 RID: 11251
			public class MARKFORSTORAGE
			{
				// Token: 0x0400BF23 RID: 48931
				public static LocString NAME = "Sweep";

				// Token: 0x0400BF24 RID: 48932
				public static LocString TOOLNAME = "Sweep tool";

				// Token: 0x0400BF25 RID: 48933
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BF4 RID: 11252
			public class MOP
			{
				// Token: 0x0400BF26 RID: 48934
				public static LocString NAME = "Mop";

				// Token: 0x0400BF27 RID: 48935
				public static LocString TOOLNAME = "Mop tool";

				// Token: 0x0400BF28 RID: 48936
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400BF29 RID: 48937
				public static LocString TOO_MUCH_LIQUID = "Too Much Liquid";

				// Token: 0x0400BF2A RID: 48938
				public static LocString NOT_ON_FLOOR = "Not On Floor";
			}

			// Token: 0x02002BF5 RID: 11253
			public class HARVEST
			{
				// Token: 0x0400BF2B RID: 48939
				public static LocString NAME = "Harvest";

				// Token: 0x0400BF2C RID: 48940
				public static LocString TOOLNAME = "Harvest tool";

				// Token: 0x0400BF2D RID: 48941
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BF6 RID: 11254
			public class PRIORITIZE
			{
				// Token: 0x0400BF2E RID: 48942
				public static LocString NAME = "Priority";

				// Token: 0x0400BF2F RID: 48943
				public static LocString TOOLNAME = "Priority tool";

				// Token: 0x0400BF30 RID: 48944
				public static LocString TOOLACTION = "DRAG";

				// Token: 0x0400BF31 RID: 48945
				public static LocString SPECIFIC_PRIORITY = "Set Priority: {0}";
			}

			// Token: 0x02002BF7 RID: 11255
			public class EMPTY_PIPE
			{
				// Token: 0x0400BF32 RID: 48946
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400BF33 RID: 48947
				public static LocString TOOLTIP = "Extract pipe contents {Hotkey}";

				// Token: 0x0400BF34 RID: 48948
				public static LocString TOOLNAME = "Empty Pipe tool";

				// Token: 0x0400BF35 RID: 48949
				public static LocString TOOLACTION = "DRAG";
			}

			// Token: 0x02002BF8 RID: 11256
			public class FILTERSCREEN
			{
				// Token: 0x0400BF36 RID: 48950
				public static LocString OPTIONS = "Tool Filter";
			}

			// Token: 0x02002BF9 RID: 11257
			public class FILTERLAYERS
			{
				// Token: 0x02003757 RID: 14167
				public class BUILDINGS
				{
					// Token: 0x0400DC53 RID: 56403
					public static LocString NAME = "Buildings";

					// Token: 0x0400DC54 RID: 56404
					public static LocString TOOLTIP = "All buildings";
				}

				// Token: 0x02003758 RID: 14168
				public class TILES
				{
					// Token: 0x0400DC55 RID: 56405
					public static LocString NAME = "Tiles";

					// Token: 0x0400DC56 RID: 56406
					public static LocString TOOLTIP = "Tiles only";
				}

				// Token: 0x02003759 RID: 14169
				public class WIRES
				{
					// Token: 0x0400DC57 RID: 56407
					public static LocString NAME = "Power Wires";

					// Token: 0x0400DC58 RID: 56408
					public static LocString TOOLTIP = "Power wires only";
				}

				// Token: 0x0200375A RID: 14170
				public class SOLIDCONDUITS
				{
					// Token: 0x0400DC59 RID: 56409
					public static LocString NAME = "Conveyor Rails";

					// Token: 0x0400DC5A RID: 56410
					public static LocString TOOLTIP = "Conveyor rails only";
				}

				// Token: 0x0200375B RID: 14171
				public class DIGPLACER
				{
					// Token: 0x0400DC5B RID: 56411
					public static LocString NAME = "Dig Orders";

					// Token: 0x0400DC5C RID: 56412
					public static LocString TOOLTIP = "Dig orders only";
				}

				// Token: 0x0200375C RID: 14172
				public class CLEANANDCLEAR
				{
					// Token: 0x0400DC5D RID: 56413
					public static LocString NAME = "Sweep & Mop Orders";

					// Token: 0x0400DC5E RID: 56414
					public static LocString TOOLTIP = "Sweep and mop orders only";
				}

				// Token: 0x0200375D RID: 14173
				public class HARVEST_WHEN_READY
				{
					// Token: 0x0400DC5F RID: 56415
					public static LocString NAME = "Enable Harvest";

					// Token: 0x0400DC60 RID: 56416
					public static LocString TOOLTIP = "Enable harvest on selected plants";
				}

				// Token: 0x0200375E RID: 14174
				public class DO_NOT_HARVEST
				{
					// Token: 0x0400DC61 RID: 56417
					public static LocString NAME = "Disable Harvest";

					// Token: 0x0400DC62 RID: 56418
					public static LocString TOOLTIP = "Disable harvest on selected plants";
				}

				// Token: 0x0200375F RID: 14175
				public class ATTACK
				{
					// Token: 0x0400DC63 RID: 56419
					public static LocString NAME = "Attack";

					// Token: 0x0400DC64 RID: 56420
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02003760 RID: 14176
				public class LOGIC
				{
					// Token: 0x0400DC65 RID: 56421
					public static LocString NAME = "Automation";

					// Token: 0x0400DC66 RID: 56422
					public static LocString TOOLTIP = "Automation buildings only";
				}

				// Token: 0x02003761 RID: 14177
				public class BACKWALL
				{
					// Token: 0x0400DC67 RID: 56423
					public static LocString NAME = "Background Buildings";

					// Token: 0x0400DC68 RID: 56424
					public static LocString TOOLTIP = "Background buildings only";
				}

				// Token: 0x02003762 RID: 14178
				public class LIQUIDPIPES
				{
					// Token: 0x0400DC69 RID: 56425
					public static LocString NAME = "Liquid Pipes";

					// Token: 0x0400DC6A RID: 56426
					public static LocString TOOLTIP = "Liquid pipes only";
				}

				// Token: 0x02003763 RID: 14179
				public class GASPIPES
				{
					// Token: 0x0400DC6B RID: 56427
					public static LocString NAME = "Gas Pipes";

					// Token: 0x0400DC6C RID: 56428
					public static LocString TOOLTIP = "Gas pipes only";
				}

				// Token: 0x02003764 RID: 14180
				public class ALL
				{
					// Token: 0x0400DC6D RID: 56429
					public static LocString NAME = "All";

					// Token: 0x0400DC6E RID: 56430
					public static LocString TOOLTIP = "Target all";
				}

				// Token: 0x02003765 RID: 14181
				public class ALL_OVERLAY
				{
					// Token: 0x0400DC6F RID: 56431
					public static LocString NAME = "All";

					// Token: 0x0400DC70 RID: 56432
					public static LocString TOOLTIP = "Show all";
				}

				// Token: 0x02003766 RID: 14182
				public class METAL
				{
					// Token: 0x0400DC71 RID: 56433
					public static LocString NAME = "Metal";

					// Token: 0x0400DC72 RID: 56434
					public static LocString TOOLTIP = "Show only metals";
				}

				// Token: 0x02003767 RID: 14183
				public class BUILDABLE
				{
					// Token: 0x0400DC73 RID: 56435
					public static LocString NAME = "Mineral";

					// Token: 0x0400DC74 RID: 56436
					public static LocString TOOLTIP = "Show only minerals";
				}

				// Token: 0x02003768 RID: 14184
				public class FILTER
				{
					// Token: 0x0400DC75 RID: 56437
					public static LocString NAME = "Filtration Medium";

					// Token: 0x0400DC76 RID: 56438
					public static LocString TOOLTIP = "Show only filtration mediums";
				}

				// Token: 0x02003769 RID: 14185
				public class CONSUMABLEORE
				{
					// Token: 0x0400DC77 RID: 56439
					public static LocString NAME = "Consumable Ore";

					// Token: 0x0400DC78 RID: 56440
					public static LocString TOOLTIP = "Show only consumable ore";
				}

				// Token: 0x0200376A RID: 14186
				public class ORGANICS
				{
					// Token: 0x0400DC79 RID: 56441
					public static LocString NAME = "Organic";

					// Token: 0x0400DC7A RID: 56442
					public static LocString TOOLTIP = "Show only organic materials";
				}

				// Token: 0x0200376B RID: 14187
				public class FARMABLE
				{
					// Token: 0x0400DC7B RID: 56443
					public static LocString NAME = "Cultivable Soil";

					// Token: 0x0400DC7C RID: 56444
					public static LocString TOOLTIP = "Show only cultivable soil";
				}

				// Token: 0x0200376C RID: 14188
				public class LIQUIFIABLE
				{
					// Token: 0x0400DC7D RID: 56445
					public static LocString NAME = "Liquefiable";

					// Token: 0x0400DC7E RID: 56446
					public static LocString TOOLTIP = "Show only liquefiable elements";
				}

				// Token: 0x0200376D RID: 14189
				public class GAS
				{
					// Token: 0x0400DC7F RID: 56447
					public static LocString NAME = "Gas";

					// Token: 0x0400DC80 RID: 56448
					public static LocString TOOLTIP = "Show only gases";
				}

				// Token: 0x0200376E RID: 14190
				public class LIQUID
				{
					// Token: 0x0400DC81 RID: 56449
					public static LocString NAME = "Liquid";

					// Token: 0x0400DC82 RID: 56450
					public static LocString TOOLTIP = "Show only liquids";
				}

				// Token: 0x0200376F RID: 14191
				public class MISC
				{
					// Token: 0x0400DC83 RID: 56451
					public static LocString NAME = "Miscellaneous";

					// Token: 0x0400DC84 RID: 56452
					public static LocString TOOLTIP = "Show only miscellaneous elements";
				}

				// Token: 0x02003770 RID: 14192
				public class ABSOLUTETEMPERATURE
				{
					// Token: 0x0400DC85 RID: 56453
					public static LocString NAME = "Absolute Temperature";

					// Token: 0x0400DC86 RID: 56454
					public static LocString TOOLTIP = "<b>Absolute Temperature</b>\nView the default temperature ranges and categories relative to absolute zero";
				}

				// Token: 0x02003771 RID: 14193
				public class RELATIVETEMPERATURE
				{
					// Token: 0x0400DC87 RID: 56455
					public static LocString NAME = "Relative Temperature";

					// Token: 0x0400DC88 RID: 56456
					public static LocString TOOLTIP = "<b>Relative Temperature</b>\nCustomize visual map to identify temperatures relative to a selected midpoint\n\nDrag the slider to adjust the relative temperature range";
				}

				// Token: 0x02003772 RID: 14194
				public class HEATFLOW
				{
					// Token: 0x0400DC89 RID: 56457
					public static LocString NAME = "Thermal Tolerance";

					// Token: 0x0400DC8A RID: 56458
					public static LocString TOOLTIP = "<b>Thermal Tolerance</b>\nView the impact of ambient temperatures on living beings";
				}

				// Token: 0x02003773 RID: 14195
				public class STATECHANGE
				{
					// Token: 0x0400DC8B RID: 56459
					public static LocString NAME = "State Change";

					// Token: 0x0400DC8C RID: 56460
					public static LocString TOOLTIP = "<b>State Change</b>\nView the impact of ambient temperatures on element states";
				}

				// Token: 0x02003774 RID: 14196
				public class BREATHABLE
				{
					// Token: 0x0400DC8D RID: 56461
					public static LocString NAME = "Breathable Gas";

					// Token: 0x0400DC8E RID: 56462
					public static LocString TOOLTIP = "Show only breathable gases";
				}

				// Token: 0x02003775 RID: 14197
				public class UNBREATHABLE
				{
					// Token: 0x0400DC8F RID: 56463
					public static LocString NAME = "Unbreathable Gas";

					// Token: 0x0400DC90 RID: 56464
					public static LocString TOOLTIP = "Show only unbreathable gases";
				}

				// Token: 0x02003776 RID: 14198
				public class AGRICULTURE
				{
					// Token: 0x0400DC91 RID: 56465
					public static LocString NAME = "Agriculture";

					// Token: 0x0400DC92 RID: 56466
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02003777 RID: 14199
				public class ADAPTIVETEMPERATURE
				{
					// Token: 0x0400DC93 RID: 56467
					public static LocString NAME = "Adapt. Temperature";

					// Token: 0x0400DC94 RID: 56468
					public static LocString TOOLTIP = "";
				}

				// Token: 0x02003778 RID: 14200
				public class CONSTRUCTION
				{
					// Token: 0x0400DC95 RID: 56469
					public static LocString NAME = "Construction";

					// Token: 0x0400DC96 RID: 56470
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Target ",
						UI.PRE_KEYWORD,
						"Construction",
						UI.PST_KEYWORD,
						" errands only"
					});
				}

				// Token: 0x02003779 RID: 14201
				public class DIG
				{
					// Token: 0x0400DC97 RID: 56471
					public static LocString NAME = "Digging";

					// Token: 0x0400DC98 RID: 56472
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Target ",
						UI.PRE_KEYWORD,
						"Digging",
						UI.PST_KEYWORD,
						" errands only"
					});
				}

				// Token: 0x0200377A RID: 14202
				public class CLEAN
				{
					// Token: 0x0400DC99 RID: 56473
					public static LocString NAME = "Cleaning";

					// Token: 0x0400DC9A RID: 56474
					public static LocString TOOLTIP = "Target cleaning errands only";
				}

				// Token: 0x0200377B RID: 14203
				public class OPERATE
				{
					// Token: 0x0400DC9B RID: 56475
					public static LocString NAME = "Duties";

					// Token: 0x0400DC9C RID: 56476
					public static LocString TOOLTIP = "Target general duties only";
				}
			}
		}

		// Token: 0x02002182 RID: 8578
		public class DETAILTABS
		{
			// Token: 0x02002BFA RID: 11258
			public class STATS
			{
				// Token: 0x0400BF37 RID: 48951
				public static LocString NAME = "Skills";

				// Token: 0x0400BF38 RID: 48952
				public static LocString TOOLTIP = "<b>Skills</b>\nView this Duplicant's resume and attributes";

				// Token: 0x0400BF39 RID: 48953
				public static LocString GROUPNAME_ATTRIBUTES = "ATTRIBUTES";

				// Token: 0x0400BF3A RID: 48954
				public static LocString GROUPNAME_STRESS = "TODAY'S STRESS";

				// Token: 0x0400BF3B RID: 48955
				public static LocString GROUPNAME_EXPECTATIONS = "EXPECTATIONS";

				// Token: 0x0400BF3C RID: 48956
				public static LocString GROUPNAME_TRAITS = "TRAITS";
			}

			// Token: 0x02002BFB RID: 11259
			public class SIMPLEINFO
			{
				// Token: 0x0400BF3D RID: 48957
				public static LocString NAME = "Status";

				// Token: 0x0400BF3E RID: 48958
				public static LocString TOOLTIP = "<b>Status</b>\nView current status";

				// Token: 0x0400BF3F RID: 48959
				public static LocString GROUPNAME_STATUS = "STATUS";

				// Token: 0x0400BF40 RID: 48960
				public static LocString GROUPNAME_DESCRIPTION = "INFORMATION";

				// Token: 0x0400BF41 RID: 48961
				public static LocString GROUPNAME_CONDITION = "CONDITION";

				// Token: 0x0400BF42 RID: 48962
				public static LocString GROUPNAME_REQUIREMENTS = "REQUIREMENTS";

				// Token: 0x0400BF43 RID: 48963
				public static LocString GROUPNAME_EFFECTS = "EFFECTS";

				// Token: 0x0400BF44 RID: 48964
				public static LocString GROUPNAME_RESEARCH = "RESEARCH";

				// Token: 0x0400BF45 RID: 48965
				public static LocString GROUPNAME_LORE = "RECOVERED FILES";

				// Token: 0x0400BF46 RID: 48966
				public static LocString GROUPNAME_FERTILITY = "EGG CHANCES";

				// Token: 0x0400BF47 RID: 48967
				public static LocString GROUPNAME_ROCKET = "ROCKETRY";

				// Token: 0x0400BF48 RID: 48968
				public static LocString GROUPNAME_CARGOBAY = "CARGO BAYS";

				// Token: 0x0400BF49 RID: 48969
				public static LocString GROUPNAME_ELEMENTS = "RESOURCES";

				// Token: 0x0400BF4A RID: 48970
				public static LocString GROUPNAME_LIFE = "LIFEFORMS";

				// Token: 0x0400BF4B RID: 48971
				public static LocString GROUPNAME_BIOMES = "BIOMES";

				// Token: 0x0400BF4C RID: 48972
				public static LocString GROUPNAME_GEYSERS = "GEYSERS";

				// Token: 0x0400BF4D RID: 48973
				public static LocString GROUPNAME_METEORSHOWERS = "METEOR SHOWERS";

				// Token: 0x0400BF4E RID: 48974
				public static LocString GROUPNAME_WORLDTRAITS = "WORLD TRAITS";

				// Token: 0x0400BF4F RID: 48975
				public static LocString GROUPNAME_CLUSTER_POI = "POINT OF INTEREST";

				// Token: 0x0400BF50 RID: 48976
				public static LocString GROUPNAME_MOVABLE = "MOVING";

				// Token: 0x0400BF51 RID: 48977
				public static LocString NO_METEORSHOWERS = "No meteor showers forecasted";

				// Token: 0x0400BF52 RID: 48978
				public static LocString NO_GEYSERS = "No geysers detected";

				// Token: 0x0400BF53 RID: 48979
				public static LocString UNKNOWN_GEYSERS = "Unknown Geysers ({num})";
			}

			// Token: 0x02002BFC RID: 11260
			public class DETAILS
			{
				// Token: 0x0400BF54 RID: 48980
				public static LocString NAME = "Properties";

				// Token: 0x0400BF55 RID: 48981
				public static LocString MINION_NAME = "About";

				// Token: 0x0400BF56 RID: 48982
				public static LocString TOOLTIP = "<b>Properties</b>\nView elements, temperature, germs and more";

				// Token: 0x0400BF57 RID: 48983
				public static LocString MINION_TOOLTIP = "More information";

				// Token: 0x0400BF58 RID: 48984
				public static LocString GROUPNAME_DETAILS = "DETAILS";

				// Token: 0x0400BF59 RID: 48985
				public static LocString GROUPNAME_CONTENTS = "CONTENTS";

				// Token: 0x0400BF5A RID: 48986
				public static LocString GROUPNAME_MINION_CONTENTS = "CARRIED ITEMS";

				// Token: 0x0400BF5B RID: 48987
				public static LocString STORAGE_EMPTY = "None";

				// Token: 0x0400BF5C RID: 48988
				public static LocString CONTENTS_MASS = "{0}: {1}";

				// Token: 0x0400BF5D RID: 48989
				public static LocString CONTENTS_TEMPERATURE = "{0} at {1}";

				// Token: 0x0400BF5E RID: 48990
				public static LocString CONTENTS_ROTTABLE = "\n • {0}";

				// Token: 0x0400BF5F RID: 48991
				public static LocString CONTENTS_DISEASED = "\n • {0}";

				// Token: 0x0400BF60 RID: 48992
				public static LocString NET_STRESS = "<b>Today's Net Stress: {0}%</b>";

				// Token: 0x0200377C RID: 14204
				public class RADIATIONABSORPTIONFACTOR
				{
					// Token: 0x0400DC9D RID: 56477
					public static LocString NAME = "Radiation Blocking: {0}";

					// Token: 0x0400DC9E RID: 56478
					public static LocString TOOLTIP = "This object will block approximately {0} of radiation.";
				}
			}

			// Token: 0x02002BFD RID: 11261
			public class PERSONALITY
			{
				// Token: 0x0400BF61 RID: 48993
				public static LocString NAME = "Bio";

				// Token: 0x0400BF62 RID: 48994
				public static LocString TOOLTIP = "<b>Bio</b>\nView this Duplicant's personality, skills, traits and amenities";

				// Token: 0x0400BF63 RID: 48995
				public static LocString GROUPNAME_BIO = "ABOUT";

				// Token: 0x0400BF64 RID: 48996
				public static LocString GROUPNAME_RESUME = "{0}'S RESUME";

				// Token: 0x0200377D RID: 14205
				public class RESUME
				{
					// Token: 0x0400DC9F RID: 56479
					public static LocString MASTERED_SKILLS = "<b><size=13>Learned Skills:</size></b>";

					// Token: 0x0400DCA0 RID: 56480
					public static LocString MASTERED_SKILLS_TOOLTIP = string.Concat(new string[]
					{
						"All ",
						UI.PRE_KEYWORD,
						"Traits",
						UI.PST_KEYWORD,
						" and ",
						UI.PRE_KEYWORD,
						"Morale Needs",
						UI.PST_KEYWORD,
						" become permanent once a Duplicant has learned a new ",
						UI.PRE_KEYWORD,
						"Skill",
						UI.PST_KEYWORD,
						"\n\n",
						STRINGS.BUILDINGS.PREFABS.RESETSKILLSSTATION.NAME,
						"s can be built from the ",
						UI.FormatAsBuildMenuTab("Stations Tab", global::Action.Plan10),
						" to completely reset a Duplicant's learned ",
						UI.PRE_KEYWORD,
						"Skills",
						UI.PST_KEYWORD,
						", refunding all ",
						UI.PRE_KEYWORD,
						"Skill Points",
						UI.PST_KEYWORD
					});

					// Token: 0x0400DCA1 RID: 56481
					public static LocString JOBTRAINING_TOOLTIP = string.Concat(new string[]
					{
						"{0} learned this ",
						UI.PRE_KEYWORD,
						"Skill",
						UI.PST_KEYWORD,
						" while working as a {1}"
					});

					// Token: 0x02003B47 RID: 15175
					public class APTITUDES
					{
						// Token: 0x0400E5AD RID: 58797
						public static LocString NAME = "<b><size=13>Personal Interests:</size></b>";

						// Token: 0x0400E5AE RID: 58798
						public static LocString TOOLTIP = "{0} enjoys these types of work";
					}

					// Token: 0x02003B48 RID: 15176
					public class PERKS
					{
						// Token: 0x0400E5AF RID: 58799
						public static LocString NAME = "<b><size=13>Skill Training:</size></b>";

						// Token: 0x0400E5B0 RID: 58800
						public static LocString TOOLTIP = "These are permanent skills {0} gained from learned skills";
					}

					// Token: 0x02003B49 RID: 15177
					public class CURRENT_ROLE
					{
						// Token: 0x0400E5B1 RID: 58801
						public static LocString NAME = "<size=13><b>Current Job:</b> {0}</size>";

						// Token: 0x0400E5B2 RID: 58802
						public static LocString TOOLTIP = "{0} is currently working as a {1}";

						// Token: 0x0400E5B3 RID: 58803
						public static LocString NOJOB_TOOLTIP = "This {0} is... \"between jobs\" at present";
					}

					// Token: 0x02003B4A RID: 15178
					public class NO_MASTERED_SKILLS
					{
						// Token: 0x0400E5B4 RID: 58804
						public static LocString NAME = "None";

						// Token: 0x0400E5B5 RID: 58805
						public static LocString TOOLTIP = string.Concat(new string[]
						{
							"{0} has not learned any ",
							UI.PRE_KEYWORD,
							"Skills",
							UI.PST_KEYWORD,
							" yet"
						});
					}
				}

				// Token: 0x0200377E RID: 14206
				public class EQUIPMENT
				{
					// Token: 0x0400DCA2 RID: 56482
					public static LocString GROUPNAME_ROOMS = "AMENITIES";

					// Token: 0x0400DCA3 RID: 56483
					public static LocString GROUPNAME_OWNABLE = "EQUIPMENT";

					// Token: 0x0400DCA4 RID: 56484
					public static LocString NO_ASSIGNABLES = "None";

					// Token: 0x0400DCA5 RID: 56485
					public static LocString NO_ASSIGNABLES_TOOLTIP = "{0} has not been assigned any buildings of their own";

					// Token: 0x0400DCA6 RID: 56486
					public static LocString UNASSIGNED = "Unassigned";

					// Token: 0x0400DCA7 RID: 56487
					public static LocString UNASSIGNED_TOOLTIP = "This Duplicant has not been assigned a {0}";

					// Token: 0x0400DCA8 RID: 56488
					public static LocString ASSIGNED_TOOLTIP = "{2} has been assigned a {0}\n\nEffects: {1}";

					// Token: 0x0400DCA9 RID: 56489
					public static LocString NOEQUIPMENT = "None";

					// Token: 0x0400DCAA RID: 56490
					public static LocString NOEQUIPMENT_TOOLTIP = "{0}'s wearing their Printday Suit and nothing more";
				}
			}

			// Token: 0x02002BFE RID: 11262
			public class ENERGYCONSUMER
			{
				// Token: 0x0400BF65 RID: 48997
				public static LocString NAME = "Energy";

				// Token: 0x0400BF66 RID: 48998
				public static LocString TOOLTIP = "View how much power this building consumes";
			}

			// Token: 0x02002BFF RID: 11263
			public class ENERGYWIRE
			{
				// Token: 0x0400BF67 RID: 48999
				public static LocString NAME = "Energy";

				// Token: 0x0400BF68 RID: 49000
				public static LocString TOOLTIP = "View this wire's network";
			}

			// Token: 0x02002C00 RID: 11264
			public class ENERGYGENERATOR
			{
				// Token: 0x0400BF69 RID: 49001
				public static LocString NAME = "Energy";

				// Token: 0x0400BF6A RID: 49002
				public static LocString TOOLTIP = "<b>Energy</b>\nMonitor the power this building is generating";

				// Token: 0x0400BF6B RID: 49003
				public static LocString CIRCUITOVERVIEW = "CIRCUIT OVERVIEW";

				// Token: 0x0400BF6C RID: 49004
				public static LocString GENERATORS = "POWER GENERATORS";

				// Token: 0x0400BF6D RID: 49005
				public static LocString CONSUMERS = "POWER CONSUMERS";

				// Token: 0x0400BF6E RID: 49006
				public static LocString BATTERIES = "BATTERIES";

				// Token: 0x0400BF6F RID: 49007
				public static LocString DISCONNECTED = "Not connected to an electrical circuit";

				// Token: 0x0400BF70 RID: 49008
				public static LocString NOGENERATORS = "No generators on this circuit";

				// Token: 0x0400BF71 RID: 49009
				public static LocString NOCONSUMERS = "No consumers on this circuit";

				// Token: 0x0400BF72 RID: 49010
				public static LocString NOBATTERIES = "No batteries on this circuit";

				// Token: 0x0400BF73 RID: 49011
				public static LocString AVAILABLE_JOULES = UI.FormatAsLink("Power", "POWER") + " stored: {0}";

				// Token: 0x0400BF74 RID: 49012
				public static LocString AVAILABLE_JOULES_TOOLTIP = "Amount of power stored in batteries";

				// Token: 0x0400BF75 RID: 49013
				public static LocString WATTAGE_GENERATED = UI.FormatAsLink("Power", "POWER") + " produced: {0}";

				// Token: 0x0400BF76 RID: 49014
				public static LocString WATTAGE_GENERATED_TOOLTIP = "The total amount of power generated by this circuit";

				// Token: 0x0400BF77 RID: 49015
				public static LocString WATTAGE_CONSUMED = UI.FormatAsLink("Power", "POWER") + " consumed: {0}";

				// Token: 0x0400BF78 RID: 49016
				public static LocString WATTAGE_CONSUMED_TOOLTIP = "The total amount of power used by this circuit";

				// Token: 0x0400BF79 RID: 49017
				public static LocString POTENTIAL_WATTAGE_CONSUMED = "Potential power consumed: {0}";

				// Token: 0x0400BF7A RID: 49018
				public static LocString POTENTIAL_WATTAGE_CONSUMED_TOOLTIP = "The total amount of power that can be used by this circuit if all connected buildings are active";

				// Token: 0x0400BF7B RID: 49019
				public static LocString MAX_SAFE_WATTAGE = "Maximum safe wattage: {0}";

				// Token: 0x0400BF7C RID: 49020
				public static LocString MAX_SAFE_WATTAGE_TOOLTIP = "Exceeding this value will overload the circuit and can result in damage to wiring and buildings";
			}

			// Token: 0x02002C01 RID: 11265
			public class DISEASE
			{
				// Token: 0x0400BF7D RID: 49021
				public static LocString NAME = "Germs";

				// Token: 0x0400BF7E RID: 49022
				public static LocString TOOLTIP = "<b>Germs</b>\nView germ resistance and risk of contagion";

				// Token: 0x0400BF7F RID: 49023
				public static LocString DISEASE_SOURCE = "DISEASE SOURCE";

				// Token: 0x0400BF80 RID: 49024
				public static LocString IMMUNE_SYSTEM = "GERM HOST";

				// Token: 0x0400BF81 RID: 49025
				public static LocString CONTRACTION_RATES = "CONTRACTION RATES";

				// Token: 0x0400BF82 RID: 49026
				public static LocString CURRENT_GERMS = "SURFACE GERMS";

				// Token: 0x0400BF83 RID: 49027
				public static LocString NO_CURRENT_GERMS = "SURFACE GERMS";

				// Token: 0x0400BF84 RID: 49028
				public static LocString GERMS_INFO = "GERM LIFE CYCLE";

				// Token: 0x0400BF85 RID: 49029
				public static LocString INFECTION_INFO = "INFECTION DETAILS";

				// Token: 0x0400BF86 RID: 49030
				public static LocString DISEASE_INFO_POPUP_HEADER = "DISEASE INFO: {0}";

				// Token: 0x0400BF87 RID: 49031
				public static LocString DISEASE_INFO_POPUP_BUTTON = "FULL INFO";

				// Token: 0x0400BF88 RID: 49032
				public static LocString DISEASE_INFO_POPUP_TOOLTIP = "View detailed germ and infection info for {0}";

				// Token: 0x0200377F RID: 14207
				public class DETAILS
				{
					// Token: 0x0400DCAB RID: 56491
					public static LocString NODISEASE = "No surface germs";

					// Token: 0x0400DCAC RID: 56492
					public static LocString NODISEASE_TOOLTIP = "There are no germs present on this object";

					// Token: 0x0400DCAD RID: 56493
					public static LocString DISEASE_AMOUNT = "{0}: {1}";

					// Token: 0x0400DCAE RID: 56494
					public static LocString DISEASE_AMOUNT_TOOLTIP = "{0} are present on the surface of the selected object";

					// Token: 0x0400DCAF RID: 56495
					public static LocString DEATH_FORMAT = "{0} dead/cycle";

					// Token: 0x0400DCB0 RID: 56496
					public static LocString DEATH_FORMAT_TOOLTIP = "Germ count is being reduced by {0}/cycle";

					// Token: 0x0400DCB1 RID: 56497
					public static LocString GROWTH_FORMAT = "{0} spawned/cycle";

					// Token: 0x0400DCB2 RID: 56498
					public static LocString GROWTH_FORMAT_TOOLTIP = "Germ count is being increased by {0}/cycle";

					// Token: 0x0400DCB3 RID: 56499
					public static LocString NEUTRAL_FORMAT = "No change";

					// Token: 0x0400DCB4 RID: 56500
					public static LocString NEUTRAL_FORMAT_TOOLTIP = "Germ count is static";

					// Token: 0x02003B4B RID: 15179
					public class GROWTH_FACTORS
					{
						// Token: 0x0400E5B6 RID: 58806
						public static LocString TITLE = "\nGrowth factors:";

						// Token: 0x0400E5B7 RID: 58807
						public static LocString TOOLTIP = "These conditions are contributing to the multiplication of germs";

						// Token: 0x0400E5B8 RID: 58808
						public static LocString RATE_OF_CHANGE = "Change rate: {0}";

						// Token: 0x0400E5B9 RID: 58809
						public static LocString RATE_OF_CHANGE_TOOLTIP = "Germ count is fluctuating at a rate of {0}";

						// Token: 0x0400E5BA RID: 58810
						public static LocString HALF_LIFE_NEG = "Half life: {0}";

						// Token: 0x0400E5BB RID: 58811
						public static LocString HALF_LIFE_NEG_TOOLTIP = "In {0} the germ count on this object will be halved";

						// Token: 0x0400E5BC RID: 58812
						public static LocString HALF_LIFE_POS = "Doubling time: {0}";

						// Token: 0x0400E5BD RID: 58813
						public static LocString HALF_LIFE_POS_TOOLTIP = "In {0} the germ count on this object will be doubled";

						// Token: 0x0400E5BE RID: 58814
						public static LocString HALF_LIFE_NEUTRAL = "Static";

						// Token: 0x0400E5BF RID: 58815
						public static LocString HALF_LIFE_NEUTRAL_TOOLTIP = "The germ count is neither increasing nor decreasing";

						// Token: 0x02003B7B RID: 15227
						public class SUBSTRATE
						{
							// Token: 0x0400E5FA RID: 58874
							public static LocString GROW = "    • Growing on {0}: {1}";

							// Token: 0x0400E5FB RID: 58875
							public static LocString GROW_TOOLTIP = "Contact with this substance is causing germs to multiply";

							// Token: 0x0400E5FC RID: 58876
							public static LocString NEUTRAL = "    • No change on {0}";

							// Token: 0x0400E5FD RID: 58877
							public static LocString NEUTRAL_TOOLTIP = "Contact with this substance has no effect on germ count";

							// Token: 0x0400E5FE RID: 58878
							public static LocString DIE = "    • Dying on {0}: {1}";

							// Token: 0x0400E5FF RID: 58879
							public static LocString DIE_TOOLTIP = "Contact with this substance is causing germs to die off";
						}

						// Token: 0x02003B7C RID: 15228
						public class ENVIRONMENT
						{
							// Token: 0x0400E600 RID: 58880
							public static LocString TITLE = "    • Surrounded by {0}: {1}";

							// Token: 0x0400E601 RID: 58881
							public static LocString GROW_TOOLTIP = "This atmosphere is causing germs to multiply";

							// Token: 0x0400E602 RID: 58882
							public static LocString DIE_TOOLTIP = "This atmosphere is causing germs to die off";
						}

						// Token: 0x02003B7D RID: 15229
						public class TEMPERATURE
						{
							// Token: 0x0400E603 RID: 58883
							public static LocString TITLE = "    • Current temperature {0}: {1}";

							// Token: 0x0400E604 RID: 58884
							public static LocString GROW_TOOLTIP = "This temperature is allowing germs to multiply";

							// Token: 0x0400E605 RID: 58885
							public static LocString DIE_TOOLTIP = "This temperature is causing germs to die off";
						}

						// Token: 0x02003B7E RID: 15230
						public class PRESSURE
						{
							// Token: 0x0400E606 RID: 58886
							public static LocString TITLE = "    • Current pressure {0}: {1}";

							// Token: 0x0400E607 RID: 58887
							public static LocString GROW_TOOLTIP = "Atmospheric pressure is causing germs to multiply";

							// Token: 0x0400E608 RID: 58888
							public static LocString DIE_TOOLTIP = "Atmospheric pressure is causing germs to die off";
						}

						// Token: 0x02003B7F RID: 15231
						public class RADIATION
						{
							// Token: 0x0400E609 RID: 58889
							public static LocString TITLE = "    • Exposed to {0} Rads: {1}";

							// Token: 0x0400E60A RID: 58890
							public static LocString DIE_TOOLTIP = "Radiation exposure is causing germs to die off";
						}

						// Token: 0x02003B80 RID: 15232
						public class DYING_OFF
						{
							// Token: 0x0400E60B RID: 58891
							public static LocString TITLE = "    • <b>Dying off: {0}</b>";

							// Token: 0x0400E60C RID: 58892
							public static LocString TOOLTIP = "Low germ count in this area is causing germs to die rapidly\n\nFewer than {0} are on this {1} of material.\n({2} germs/" + UI.UNITSUFFIXES.MASS.KILOGRAM + ")";
						}

						// Token: 0x02003B81 RID: 15233
						public class OVERPOPULATED
						{
							// Token: 0x0400E60D RID: 58893
							public static LocString TITLE = "    • <b>Overpopulated: {0}</b>";

							// Token: 0x0400E60E RID: 58894
							public static LocString TOOLTIP = "Too many germs are present in this area, resulting in rapid die-off until the population stabilizes\n\nA maximum of {0} can be on this {1} of material.\n({2} germs/" + UI.UNITSUFFIXES.MASS.KILOGRAM + ")";
						}
					}
				}
			}

			// Token: 0x02002C02 RID: 11266
			public class NEEDS
			{
				// Token: 0x0400BF89 RID: 49033
				public static LocString NAME = "Stress";

				// Token: 0x0400BF8A RID: 49034
				public static LocString TOOLTIP = "View this Duplicant's psychological status";

				// Token: 0x0400BF8B RID: 49035
				public static LocString CURRENT_STRESS_LEVEL = "Current " + UI.FormatAsLink("Stress", "STRESS") + " Level: {0}";

				// Token: 0x0400BF8C RID: 49036
				public static LocString OVERVIEW = "Overview";

				// Token: 0x0400BF8D RID: 49037
				public static LocString STRESS_CREATORS = UI.FormatAsLink("Stress", "STRESS") + " Creators";

				// Token: 0x0400BF8E RID: 49038
				public static LocString STRESS_RELIEVERS = UI.FormatAsLink("Stress", "STRESS") + " Relievers";

				// Token: 0x0400BF8F RID: 49039
				public static LocString CURRENT_NEED_LEVEL = "Current Level: {0}";

				// Token: 0x0400BF90 RID: 49040
				public static LocString NEXT_NEED_LEVEL = "Next Level: {0}";
			}

			// Token: 0x02002C03 RID: 11267
			public class EGG_CHANCES
			{
				// Token: 0x0400BF91 RID: 49041
				public static LocString CHANCE_FORMAT = "{0}: {1}";

				// Token: 0x0400BF92 RID: 49042
				public static LocString CHANCE_FORMAT_TOOLTIP = "This critter has a {1} chance of laying {0}s.\n\nThis probability increases when the creature:\n{2}";

				// Token: 0x0400BF93 RID: 49043
				public static LocString CHANCE_MOD_FORMAT = "    • {0}\n";

				// Token: 0x0400BF94 RID: 49044
				public static LocString CHANCE_FORMAT_TOOLTIP_NOMOD = "This critter has a {1} chance of laying {0}s.";
			}

			// Token: 0x02002C04 RID: 11268
			public class BUILDING_CHORES
			{
				// Token: 0x0400BF95 RID: 49045
				public static LocString NAME = "Errands";

				// Token: 0x0400BF96 RID: 49046
				public static LocString TOOLTIP = "<b>Errands</b>\nView available errands and current queue";

				// Token: 0x0400BF97 RID: 49047
				public static LocString CHORE_TYPE_TOOLTIP = "Errand Type: {0}";

				// Token: 0x0400BF98 RID: 49048
				public static LocString AVAILABLE_CHORES = "AVAILABLE ERRANDS";

				// Token: 0x0400BF99 RID: 49049
				public static LocString DUPE_TOOLTIP_FAILED = "{Name} cannot currently {Errand}\n\nReason:\n{FailedPrecondition}";

				// Token: 0x0400BF9A RID: 49050
				public static LocString DUPE_TOOLTIP_SUCCEEDED = "{Description}\n\n{Errand}'s Type: {Groups}\n\n{Name}'s {BestGroup} Priority: {PersonalPriorityValue} ({PersonalPriority})\n{Building} Priority: {BuildingPriority}\nAll {BestGroup} Errands: {TypePriority}\n\nTotal Priority: {TotalPriority}";

				// Token: 0x0400BF9B RID: 49051
				public static LocString DUPE_TOOLTIP_DESC_ACTIVE = "{Name} is currently busy: \"{Errand}\"";

				// Token: 0x0400BF9C RID: 49052
				public static LocString DUPE_TOOLTIP_DESC_INACTIVE = "\"{Errand}\" is #{Rank} on {Name}'s To Do list, after they finish their current errand";
			}

			// Token: 0x02002C05 RID: 11269
			public class PROCESS_CONDITIONS
			{
				// Token: 0x0400BF9D RID: 49053
				public static LocString NAME = "LAUNCH CHECKLIST";

				// Token: 0x0400BF9E RID: 49054
				public static LocString ROCKETPREP = "Rocket Construction";

				// Token: 0x0400BF9F RID: 49055
				public static LocString ROCKETPREP_TOOLTIP = "It is recommended that all boxes on the Rocket Construction checklist be ticked before launching";

				// Token: 0x0400BFA0 RID: 49056
				public static LocString ROCKETSTORAGE = "Cargo Manifest";

				// Token: 0x0400BFA1 RID: 49057
				public static LocString ROCKETSTORAGE_TOOLTIP = "It is recommended that all boxes on the Cargo Manifest checklist be ticked before launching";

				// Token: 0x0400BFA2 RID: 49058
				public static LocString ROCKETFLIGHT = "Flight Route";

				// Token: 0x0400BFA3 RID: 49059
				public static LocString ROCKETFLIGHT_TOOLTIP = "A rocket requires a clear path to a set destination to conduct a mission";

				// Token: 0x0400BFA4 RID: 49060
				public static LocString ROCKETBOARD = "Crew Manifest";

				// Token: 0x0400BFA5 RID: 49061
				public static LocString ROCKETBOARD_TOOLTIP = "It is recommended that all boxes on the Crew Manifest checklist be ticked before launching";

				// Token: 0x0400BFA6 RID: 49062
				public static LocString ALL = "Requirements";

				// Token: 0x0400BFA7 RID: 49063
				public static LocString ALL_TOOLTIP = "These conditions must be fulfilled in order to launch a rocket mission";
			}

			// Token: 0x02002C06 RID: 11270
			public class COSMETICS
			{
				// Token: 0x0400BFA8 RID: 49064
				public static LocString NAME = "Blueprint";

				// Token: 0x0400BFA9 RID: 49065
				public static LocString TOOLTIP = "<b>Blueprint</b>\nView and change assigned blueprints";
			}

			// Token: 0x02002C07 RID: 11271
			public class MATERIAL
			{
				// Token: 0x0400BFAA RID: 49066
				public static LocString NAME = "Material";

				// Token: 0x0400BFAB RID: 49067
				public static LocString TOOLTIP = "<b>Material</b>\nView and change this building's construction material";

				// Token: 0x0400BFAC RID: 49068
				public static LocString SUB_HEADER_CURRENT_MATERIAL = "CURRENT MATERIAL";

				// Token: 0x0400BFAD RID: 49069
				public static LocString BUTTON_CHANGE_MATERIAL = "Change Material";
			}

			// Token: 0x02002C08 RID: 11272
			public class CONFIGURATION
			{
				// Token: 0x0400BFAE RID: 49070
				public static LocString NAME = "Config";

				// Token: 0x0400BFAF RID: 49071
				public static LocString TOOLTIP = "<b>Config</b>\nView and change filters, recipes, production orders and more";
			}
		}

		// Token: 0x02002183 RID: 8579
		public class BUILDMENU
		{
			// Token: 0x040097C9 RID: 38857
			public static LocString GRID_VIEW_TOGGLE_TOOLTIP = "Toggle Grid View";

			// Token: 0x040097CA RID: 38858
			public static LocString LIST_VIEW_TOGGLE_TOOLTIP = "Toggle List View";

			// Token: 0x040097CB RID: 38859
			public static LocString NO_SEARCH_RESULTS = "NO RESULTS FOUND";

			// Token: 0x040097CC RID: 38860
			public static LocString SEARCH_RESULTS_HEADER = "SEARCH RESULTS";

			// Token: 0x040097CD RID: 38861
			public static LocString SEARCH_TEXT_PLACEHOLDER = "Search all buildings...";

			// Token: 0x040097CE RID: 38862
			public static LocString CLEAR_SEARCH_TOOLTIP = "Clear search";
		}

		// Token: 0x02002184 RID: 8580
		public class BUILDINGEFFECTS
		{
			// Token: 0x040097CF RID: 38863
			public static LocString OPERATIONREQUIREMENTS = "<b>Requirements:</b>";

			// Token: 0x040097D0 RID: 38864
			public static LocString REQUIRESPOWER = UI.FormatAsLink("Power", "POWER") + ": {0}";

			// Token: 0x040097D1 RID: 38865
			public static LocString REQUIRESELEMENT = "Supply of {0}";

			// Token: 0x040097D2 RID: 38866
			public static LocString REQUIRESLIQUIDINPUT = UI.FormatAsLink("Liquid Intake Pipe", "LIQUIDPIPING");

			// Token: 0x040097D3 RID: 38867
			public static LocString REQUIRESLIQUIDOUTPUT = UI.FormatAsLink("Liquid Output Pipe", "LIQUIDPIPING");

			// Token: 0x040097D4 RID: 38868
			public static LocString REQUIRESLIQUIDOUTPUTS = "Two " + UI.FormatAsLink("Liquid Output Pipes", "LIQUIDPIPING");

			// Token: 0x040097D5 RID: 38869
			public static LocString REQUIRESGASINPUT = UI.FormatAsLink("Gas Intake Pipe", "GASPIPING");

			// Token: 0x040097D6 RID: 38870
			public static LocString REQUIRESGASOUTPUT = UI.FormatAsLink("Gas Output Pipe", "GASPIPING");

			// Token: 0x040097D7 RID: 38871
			public static LocString REQUIRESGASOUTPUTS = "Two " + UI.FormatAsLink("Gas Output Pipes", "GASPIPING");

			// Token: 0x040097D8 RID: 38872
			public static LocString REQUIRESMANUALOPERATION = "Duplicant operation";

			// Token: 0x040097D9 RID: 38873
			public static LocString REQUIRESCREATIVITY = "Duplicant " + UI.FormatAsLink("Creativity", "ARTIST");

			// Token: 0x040097DA RID: 38874
			public static LocString REQUIRESPOWERGENERATOR = UI.FormatAsLink("Power", "POWER") + " generator";

			// Token: 0x040097DB RID: 38875
			public static LocString REQUIRESSEED = "1 Unplanted " + UI.FormatAsLink("Seed", "PLANTS");

			// Token: 0x040097DC RID: 38876
			public static LocString PREFERS_ROOM = "Preferred Room: {0}";

			// Token: 0x040097DD RID: 38877
			public static LocString REQUIRESROOM = "Dedicated Room: {0}";

			// Token: 0x040097DE RID: 38878
			public static LocString ALLOWS_FERTILIZER = "Plant " + UI.FormatAsLink("Fertilization", "WILTCONDITIONS");

			// Token: 0x040097DF RID: 38879
			public static LocString ALLOWS_IRRIGATION = "Plant " + UI.FormatAsLink("Liquid", "WILTCONDITIONS");

			// Token: 0x040097E0 RID: 38880
			public static LocString ASSIGNEDDUPLICANT = "Duplicant assignment";

			// Token: 0x040097E1 RID: 38881
			public static LocString CONSUMESANYELEMENT = "Any Element";

			// Token: 0x040097E2 RID: 38882
			public static LocString ENABLESDOMESTICGROWTH = "Enables " + UI.FormatAsLink("Plant Domestication", "PLANTS");

			// Token: 0x040097E3 RID: 38883
			public static LocString TRANSFORMER_INPUT_WIRE = "Input " + UI.FormatAsLink("Power Wire", "WIRE");

			// Token: 0x040097E4 RID: 38884
			public static LocString TRANSFORMER_OUTPUT_WIRE = "Output " + UI.FormatAsLink("Power Wire", "WIRE") + " (Limited to {0})";

			// Token: 0x040097E5 RID: 38885
			public static LocString OPERATIONEFFECTS = "<b>Effects:</b>";

			// Token: 0x040097E6 RID: 38886
			public static LocString BATTERYCAPACITY = UI.FormatAsLink("Power", "POWER") + " capacity: {0}";

			// Token: 0x040097E7 RID: 38887
			public static LocString BATTERYLEAK = UI.FormatAsLink("Power", "POWER") + " leak: {0}";

			// Token: 0x040097E8 RID: 38888
			public static LocString STORAGECAPACITY = "Storage capacity: {0}";

			// Token: 0x040097E9 RID: 38889
			public static LocString ELEMENTEMITTED_INPUTTEMP = "{0}: {1}";

			// Token: 0x040097EA RID: 38890
			public static LocString ELEMENTEMITTED_ENTITYTEMP = "{0}: {1}";

			// Token: 0x040097EB RID: 38891
			public static LocString ELEMENTEMITTED_MINORENTITYTEMP = "{0}: {1}";

			// Token: 0x040097EC RID: 38892
			public static LocString ELEMENTEMITTED_MINTEMP = "{0}: {1}";

			// Token: 0x040097ED RID: 38893
			public static LocString ELEMENTEMITTED_FIXEDTEMP = "{0}: {1}";

			// Token: 0x040097EE RID: 38894
			public static LocString ELEMENTCONSUMED = "{0}: {1}";

			// Token: 0x040097EF RID: 38895
			public static LocString ELEMENTEMITTED_TOILET = "{0}: {1} per use";

			// Token: 0x040097F0 RID: 38896
			public static LocString ELEMENTEMITTEDPERUSE = "{0}: {1} per use";

			// Token: 0x040097F1 RID: 38897
			public static LocString DISEASEEMITTEDPERUSE = "{0}: {1} per use";

			// Token: 0x040097F2 RID: 38898
			public static LocString DISEASECONSUMEDPERUSE = "All Diseases: -{0} per use";

			// Token: 0x040097F3 RID: 38899
			public static LocString ELEMENTCONSUMEDPERUSE = "{0}: {1} per use";

			// Token: 0x040097F4 RID: 38900
			public static LocString ENERGYCONSUMED = UI.FormatAsLink("Power", "POWER") + " consumed: {0}";

			// Token: 0x040097F5 RID: 38901
			public static LocString ENERGYGENERATED = UI.FormatAsLink("Power", "POWER") + ": +{0}";

			// Token: 0x040097F6 RID: 38902
			public static LocString HEATGENERATED = UI.FormatAsLink("Heat", "HEAT") + ": +{0}/s";

			// Token: 0x040097F7 RID: 38903
			public static LocString HEATCONSUMED = UI.FormatAsLink("Heat", "HEAT") + ": -{0}/s";

			// Token: 0x040097F8 RID: 38904
			public static LocString HEATER_TARGETTEMPERATURE = "Target " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x040097F9 RID: 38905
			public static LocString HEATGENERATED_AIRCONDITIONER = UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)";

			// Token: 0x040097FA RID: 38906
			public static LocString HEATGENERATED_LIQUIDCONDITIONER = UI.FormatAsLink("Heat", "HEAT") + ": +{0} (Approximate Value)";

			// Token: 0x040097FB RID: 38907
			public static LocString FABRICATES = "Fabricates";

			// Token: 0x040097FC RID: 38908
			public static LocString FABRICATEDITEM = "{1}";

			// Token: 0x040097FD RID: 38909
			public static LocString PROCESSES = "Refines:";

			// Token: 0x040097FE RID: 38910
			public static LocString PROCESSEDITEM = "{1} {0}";

			// Token: 0x040097FF RID: 38911
			public static LocString PLANTERBOX_PENTALTY = "Planter box penalty";

			// Token: 0x04009800 RID: 38912
			public static LocString DECORPROVIDED = UI.FormatAsLink("Decor", "DECOR") + ": {1} (Radius: {2} tiles)";

			// Token: 0x04009801 RID: 38913
			public static LocString OVERHEAT_TEMP = "Overheat " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x04009802 RID: 38914
			public static LocString MINIMUM_TEMP = "Freeze " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x04009803 RID: 38915
			public static LocString OVER_PRESSURE_MASS = "Overpressure: {0}";

			// Token: 0x04009804 RID: 38916
			public static LocString REFILLOXYGENTANK = "Refills Exosuit " + STRINGS.EQUIPMENT.PREFABS.OXYGEN_TANK.NAME;

			// Token: 0x04009805 RID: 38917
			public static LocString DUPLICANTMOVEMENTBOOST = "Runspeed: {0}";

			// Token: 0x04009806 RID: 38918
			public static LocString ELECTROBANKS = UI.FormatAsLink("Charge", "POWER") + ": {0}";

			// Token: 0x04009807 RID: 38919
			public static LocString STRESSREDUCEDPERMINUTE = UI.FormatAsLink("Stress", "STRESS") + ": {0} per minute";

			// Token: 0x04009808 RID: 38920
			public static LocString REMOVESEFFECTSUBTITLE = "Cures";

			// Token: 0x04009809 RID: 38921
			public static LocString REMOVEDEFFECT = "{0}";

			// Token: 0x0400980A RID: 38922
			public static LocString ADDED_EFFECT = "Added Effect: {0}";

			// Token: 0x0400980B RID: 38923
			public static LocString GASCOOLING = UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}";

			// Token: 0x0400980C RID: 38924
			public static LocString LIQUIDCOOLING = UI.FormatAsLink("Cooling factor", "HEAT") + ": {0}";

			// Token: 0x0400980D RID: 38925
			public static LocString MAX_WATTAGE = "Max " + UI.FormatAsLink("Power", "POWER") + ": {0}";

			// Token: 0x0400980E RID: 38926
			public static LocString MAX_BITS = UI.FormatAsLink("Bit", "LOGIC") + " Depth: {0}";

			// Token: 0x0400980F RID: 38927
			public static LocString RESEARCH_MATERIALS = "{0}: {1} per " + UI.FormatAsLink("Research", "RESEARCH") + " point";

			// Token: 0x04009810 RID: 38928
			public static LocString PRODUCES_RESEARCH_POINTS = "{0}";

			// Token: 0x04009811 RID: 38929
			public static LocString HIT_POINTS_PER_CYCLE = UI.FormatAsLink("Health", "Health") + " per cycle: {0}";

			// Token: 0x04009812 RID: 38930
			public static LocString KCAL_PER_CYCLE = UI.FormatAsLink("KCal", "FOOD") + " per cycle: {0}";

			// Token: 0x04009813 RID: 38931
			public static LocString REMOVES_DISEASE = "Kills germs";

			// Token: 0x04009814 RID: 38932
			public static LocString DOCTORING = "Doctoring";

			// Token: 0x04009815 RID: 38933
			public static LocString RECREATION = "Recreation";

			// Token: 0x04009816 RID: 38934
			public static LocString COOLANT = "Coolant: {1} {0}";

			// Token: 0x04009817 RID: 38935
			public static LocString REFINEMENT_ENERGY = "Heat: {0}";

			// Token: 0x04009818 RID: 38936
			public static LocString IMPROVED_BUILDINGS = "Improved Buildings";

			// Token: 0x04009819 RID: 38937
			public static LocString IMPROVED_PLANTS = "Improved Plants";

			// Token: 0x0400981A RID: 38938
			public static LocString IMPROVED_BUILDINGS_ITEM = "{0}";

			// Token: 0x0400981B RID: 38939
			public static LocString IMPROVED_PLANTS_ITEM = "{0}";

			// Token: 0x0400981C RID: 38940
			public static LocString GEYSER_PRODUCTION = "{0}: {1} at {2}";

			// Token: 0x0400981D RID: 38941
			public static LocString GEYSER_DISEASE = "Germs: {0}";

			// Token: 0x0400981E RID: 38942
			public static LocString GEYSER_PERIOD = "Eruption Period: {0} every {1}";

			// Token: 0x0400981F RID: 38943
			public static LocString GEYSER_YEAR_UNSTUDIED = "Active Period: (Requires Analysis)";

			// Token: 0x04009820 RID: 38944
			public static LocString GEYSER_YEAR_PERIOD = "Active Period: {0} every {1}";

			// Token: 0x04009821 RID: 38945
			public static LocString GEYSER_YEAR_NEXT_ACTIVE = "Next Activity: {0}";

			// Token: 0x04009822 RID: 38946
			public static LocString GEYSER_YEAR_NEXT_DORMANT = "Next Dormancy: {0}";

			// Token: 0x04009823 RID: 38947
			public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = "Average Output: (Requires Analysis)";

			// Token: 0x04009824 RID: 38948
			public static LocString GEYSER_YEAR_AVR_OUTPUT = "Average Output: {0}";

			// Token: 0x04009825 RID: 38949
			public static LocString CAPTURE_METHOD_WRANGLE = "Capture Method: Wrangling";

			// Token: 0x04009826 RID: 38950
			public static LocString CAPTURE_METHOD_LURE = "Capture Method: Lures";

			// Token: 0x04009827 RID: 38951
			public static LocString CAPTURE_METHOD_TRAP = "Capture Method: Traps";

			// Token: 0x04009828 RID: 38952
			public static LocString DIET_HEADER = "Digestion:";

			// Token: 0x04009829 RID: 38953
			public static LocString DIET_CONSUMED = "    • Diet: {Foodlist}";

			// Token: 0x0400982A RID: 38954
			public static LocString DIET_STORED = "    • Stores: {Foodlist}";

			// Token: 0x0400982B RID: 38955
			public static LocString DIET_CONSUMED_ITEM = "{Food}: {Amount}";

			// Token: 0x0400982C RID: 38956
			public static LocString DIET_PRODUCED = "    • Excretion: {Items}";

			// Token: 0x0400982D RID: 38957
			public static LocString DIET_PRODUCED_ITEM = "{Item}: {Percent} of consumed mass";

			// Token: 0x0400982E RID: 38958
			public static LocString DIET_PRODUCED_ITEM_FROM_PLANT = "{Item}: {Amount} when properly fed";

			// Token: 0x0400982F RID: 38959
			public static LocString SCALE_GROWTH = "Shearable {Item}: {Amount} per {Time}";

			// Token: 0x04009830 RID: 38960
			public static LocString SCALE_GROWTH_ATMO = "Shearable {Item}: {Amount} per {Time} ({Atmosphere})";

			// Token: 0x04009831 RID: 38961
			public static LocString SCALE_GROWTH_TEMP = "Shearable {Item}: {Amount} per {Time} ({TempMin} - {TempMax})";

			// Token: 0x04009832 RID: 38962
			public static LocString ACCESS_CONTROL = "Duplicant Access Permissions";

			// Token: 0x04009833 RID: 38963
			public static LocString ROCKETRESTRICTION_HEADER = "Restriction Control:";

			// Token: 0x04009834 RID: 38964
			public static LocString ROCKETRESTRICTION_BUILDINGS = "    • Buildings: {buildinglist}";

			// Token: 0x04009835 RID: 38965
			public static LocString UNSTABLEENTOMBDEFENSEREADY = "Entomb Defense: Ready";

			// Token: 0x04009836 RID: 38966
			public static LocString UNSTABLEENTOMBDEFENSETHREATENED = "Entomb Defense: Threatened";

			// Token: 0x04009837 RID: 38967
			public static LocString UNSTABLEENTOMBDEFENSEREACTING = "Entomb Defense: Reacting";

			// Token: 0x04009838 RID: 38968
			public static LocString UNSTABLEENTOMBDEFENSEOFF = "Entomb Defense: Off";

			// Token: 0x04009839 RID: 38969
			public static LocString ITEM_TEMPERATURE_ADJUST = "Stored " + UI.FormatAsLink("Temperature", "HEAT") + ": {0}";

			// Token: 0x0400983A RID: 38970
			public static LocString NOISE_CREATED = UI.FormatAsLink("Noise", "SOUND") + ": {0} dB (Radius: {1} tiles)";

			// Token: 0x0400983B RID: 38971
			public static LocString MESS_TABLE_SALT = "Table Salt: +{0}";

			// Token: 0x0400983C RID: 38972
			public static LocString ACTIVE_PARTICLE_CONSUMPTION = "Radbolts: {Rate}";

			// Token: 0x0400983D RID: 38973
			public static LocString PARTICLE_PORT_INPUT = "Radbolt Input Port";

			// Token: 0x0400983E RID: 38974
			public static LocString PARTICLE_PORT_OUTPUT = "Radbolt Output Port";

			// Token: 0x0400983F RID: 38975
			public static LocString IN_ORBIT_REQUIRED = "Active In Space";

			// Token: 0x04009840 RID: 38976
			public static LocString KETTLE_MELT_RATE = "Melting Rate: {0}";

			// Token: 0x04009841 RID: 38977
			public static LocString FOOD_DEHYDRATOR_WATER_OUTPUT = "Wet Floor";

			// Token: 0x02002C09 RID: 11273
			public class TOOLTIPS
			{
				// Token: 0x0400BFB0 RID: 49072
				public static LocString OPERATIONREQUIREMENTS = "All requirements must be met in order for this building to operate";

				// Token: 0x0400BFB1 RID: 49073
				public static LocString REQUIRESPOWER = string.Concat(new string[]
				{
					"Must be connected to a power grid with at least ",
					UI.FormatAsNegativeRate("{0}"),
					" of available ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});

				// Token: 0x0400BFB2 RID: 49074
				public static LocString REQUIRESELEMENT = string.Concat(new string[]
				{
					"Must receive deliveries of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" to function"
				});

				// Token: 0x0400BFB3 RID: 49075
				public static LocString REQUIRESLIQUIDINPUT = string.Concat(new string[]
				{
					"Must receive ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" from a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400BFB4 RID: 49076
				public static LocString REQUIRESLIQUIDOUTPUT = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400BFB5 RID: 49077
				public static LocString REQUIRESLIQUIDOUTPUTS = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400BFB6 RID: 49078
				public static LocString REQUIRESGASINPUT = string.Concat(new string[]
				{
					"Must receive ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" from a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400BFB7 RID: 49079
				public static LocString REQUIRESGASOUTPUT = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400BFB8 RID: 49080
				public static LocString REQUIRESGASOUTPUTS = string.Concat(new string[]
				{
					"Must expel ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" through a ",
					STRINGS.BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" system"
				});

				// Token: 0x0400BFB9 RID: 49081
				public static LocString REQUIRESMANUALOPERATION = "A Duplicant must be present to run this building";

				// Token: 0x0400BFBA RID: 49082
				public static LocString REQUIRESCREATIVITY = "A Duplicant must work on this object to create " + UI.PRE_KEYWORD + "Art" + UI.PST_KEYWORD;

				// Token: 0x0400BFBB RID: 49083
				public static LocString REQUIRESPOWERGENERATOR = string.Concat(new string[]
				{
					"Must be connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" producing generator to function"
				});

				// Token: 0x0400BFBC RID: 49084
				public static LocString REQUIRESSEED = "Must receive a plant " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;

				// Token: 0x0400BFBD RID: 49085
				public static LocString PREFERS_ROOM = "This building gains additional effects or functionality when built inside a " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

				// Token: 0x0400BFBE RID: 49086
				public static LocString REQUIRESROOM = string.Concat(new string[]
				{
					"Must be built within a dedicated ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					"\n\n",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" will become a ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" after construction"
				});

				// Token: 0x0400BFBF RID: 49087
				public static LocString ALLOWS_FERTILIZER = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Fertilizer",
					UI.PST_KEYWORD,
					" to be delivered to plants"
				});

				// Token: 0x0400BFC0 RID: 49088
				public static LocString ALLOWS_IRRIGATION = string.Concat(new string[]
				{
					"Allows ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to be delivered to plants"
				});

				// Token: 0x0400BFC1 RID: 49089
				public static LocString ALLOWS_IRRIGATION_PIPE = string.Concat(new string[]
				{
					"Allows irrigation ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" connection"
				});

				// Token: 0x0400BFC2 RID: 49090
				public static LocString ASSIGNEDDUPLICANT = "This amenity may only be used by the Duplicant it is assigned to";

				// Token: 0x0400BFC3 RID: 49091
				public static LocString BUILDINGROOMREQUIREMENTCLASS = "This category of building may be required or prohibited in certain " + UI.PRE_KEYWORD + "Rooms" + UI.PST_KEYWORD;

				// Token: 0x0400BFC4 RID: 49092
				public static LocString OPERATIONEFFECTS = "The building will produce these effects when its requirements are met";

				// Token: 0x0400BFC5 RID: 49093
				public static LocString BATTERYCAPACITY = string.Concat(new string[]
				{
					"Can hold <b>{0}</b> of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" when connected to a ",
					UI.PRE_KEYWORD,
					"Generator",
					UI.PST_KEYWORD
				});

				// Token: 0x0400BFC6 RID: 49094
				public static LocString BATTERYLEAK = string.Concat(new string[]
				{
					UI.FormatAsNegativeRate("{0}"),
					" of this battery's charge will be lost as ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD
				});

				// Token: 0x0400BFC7 RID: 49095
				public static LocString STORAGECAPACITY = "Holds up to <b>{0}</b> of material";

				// Token: 0x0400BFC8 RID: 49096
				public static LocString ELEMENTEMITTED_INPUTTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be the combined ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the input materials."
				});

				// Token: 0x0400BFC9 RID: 49097
				public static LocString ELEMENTEMITTED_ENTITYTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the building at the time of production"
				});

				// Token: 0x0400BFCA RID: 49098
				public static LocString ELEMENTEMITTED_MINORENTITYTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be at least <b>{2}</b>, or hotter if the building is hotter."
				});

				// Token: 0x0400BFCB RID: 49099
				public static LocString ELEMENTEMITTED_MINTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be at least <b>{2}</b>, or hotter if the input materials are hotter."
				});

				// Token: 0x0400BFCC RID: 49100
				public static LocString ELEMENTEMITTED_FIXEDTEMP = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use\n\nIt will be produced at <b>{2}</b>."
				});

				// Token: 0x0400BFCD RID: 49101
				public static LocString ELEMENTCONSUMED = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" when in use"
				});

				// Token: 0x0400BFCE RID: 49102
				public static LocString ELEMENTEMITTED_TOILET = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use\n\nDuplicant waste is emitted at <b>{2}</b>."
				});

				// Token: 0x0400BFCF RID: 49103
				public static LocString ELEMENTEMITTEDPERUSE = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use\n\nIt will be the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the input materials."
				});

				// Token: 0x0400BFD0 RID: 49104
				public static LocString DISEASEEMITTEDPERUSE = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use"
				});

				// Token: 0x0400BFD1 RID: 49105
				public static LocString DISEASECONSUMEDPERUSE = "Removes " + UI.FormatAsNegativeRate("{0}") + " per use";

				// Token: 0x0400BFD2 RID: 49106
				public static LocString ELEMENTCONSUMEDPERUSE = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" per use"
				});

				// Token: 0x0400BFD3 RID: 49107
				public static LocString ENERGYCONSUMED = string.Concat(new string[]
				{
					"Draws ",
					UI.FormatAsNegativeRate("{0}"),
					" from the ",
					UI.PRE_KEYWORD,
					"Power Grid",
					UI.PST_KEYWORD,
					" it's connected to"
				});

				// Token: 0x0400BFD4 RID: 49108
				public static LocString ENERGYGENERATED = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsPositiveRate("{0}"),
					" for the ",
					UI.PRE_KEYWORD,
					"Power Grid",
					UI.PST_KEYWORD,
					" it's connected to"
				});

				// Token: 0x0400BFD5 RID: 49109
				public static LocString ENABLESDOMESTICGROWTH = string.Concat(new string[]
				{
					"Accelerates ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" growth and maturation"
				});

				// Token: 0x0400BFD6 RID: 49110
				public static LocString HEATGENERATED = string.Concat(new string[]
				{
					"Generates ",
					UI.FormatAsPositiveRate("{0}"),
					" per second\n\nSum ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" change is affected by the material attributes of the heated substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity"
				});

				// Token: 0x0400BFD7 RID: 49111
				public static LocString HEATCONSUMED = string.Concat(new string[]
				{
					"Dissipates ",
					UI.FormatAsNegativeRate("{0}"),
					" per second\n\nSum ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" change can be affected by the material attributes of the cooled substance:\n    • mass\n    • specific heat capacity\n    • surface area\n    • insulation thickness\n    • thermal conductivity"
				});

				// Token: 0x0400BFD8 RID: 49112
				public static LocString HEATER_TARGETTEMPERATURE = string.Concat(new string[]
				{
					"Stops heating when the surrounding average ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{0}</b>"
				});

				// Token: 0x0400BFD9 RID: 49113
				public static LocString FABRICATES = "Fabrication is the production of items and equipment";

				// Token: 0x0400BFDA RID: 49114
				public static LocString PROCESSES = "Processes raw materials into refined materials";

				// Token: 0x0400BFDB RID: 49115
				public static LocString PROCESSEDITEM = "Refining this material produces " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;

				// Token: 0x0400BFDC RID: 49116
				public static LocString PLANTERBOX_PENTALTY = "Plants grow more slowly when contained within boxes";

				// Token: 0x0400BFDD RID: 49117
				public static LocString DECORPROVIDED = string.Concat(new string[]
				{
					"Improves ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values by ",
					UI.FormatAsPositiveModifier("<b>{0}</b>"),
					" in a <b>{1}</b> tile radius"
				});

				// Token: 0x0400BFDE RID: 49118
				public static LocString DECORDECREASED = string.Concat(new string[]
				{
					"Decreases ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values by ",
					UI.FormatAsNegativeModifier("<b>{0}</b>"),
					" in a <b>{1}</b> tile radius"
				});

				// Token: 0x0400BFDF RID: 49119
				public static LocString OVERHEAT_TEMP = "Begins overheating at <b>{0}</b>";

				// Token: 0x0400BFE0 RID: 49120
				public static LocString MINIMUM_TEMP = "Ceases to function when temperatures fall below <b>{0}</b>";

				// Token: 0x0400BFE1 RID: 49121
				public static LocString OVER_PRESSURE_MASS = "Ceases to function when the surrounding mass is above <b>{0}</b>";

				// Token: 0x0400BFE2 RID: 49122
				public static LocString REFILLOXYGENTANK = string.Concat(new string[]
				{
					"Refills ",
					UI.PRE_KEYWORD,
					"Exosuit",
					UI.PST_KEYWORD,
					" Oxygen tanks with ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" for reuse"
				});

				// Token: 0x0400BFE3 RID: 49123
				public static LocString DUPLICANTMOVEMENTBOOST = "Duplicants walk <b>{0}</b> faster on this tile";

				// Token: 0x0400BFE4 RID: 49124
				public static LocString ELECTROBANKS = string.Concat(new string[]
				{
					"Power Banks store {0} of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nThey can be discharged by circuits, buildings and Bionic Duplicants"
				});

				// Token: 0x0400BFE5 RID: 49125
				public static LocString STRESSREDUCEDPERMINUTE = string.Concat(new string[]
				{
					"Removes <b>{0}</b> of Duplicants' ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" for every uninterrupted minute of use"
				});

				// Token: 0x0400BFE6 RID: 49126
				public static LocString REMOVESEFFECTSUBTITLE = "Use of this building will remove the listed effects";

				// Token: 0x0400BFE7 RID: 49127
				public static LocString REMOVEDEFFECT = "{0}";

				// Token: 0x0400BFE8 RID: 49128
				public static LocString ADDED_EFFECT = "Effect being applied:\n\n{0}\n{1}";

				// Token: 0x0400BFE9 RID: 49129
				public static LocString GASCOOLING = string.Concat(new string[]
				{
					"Reduces the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of piped ",
					UI.PRE_KEYWORD,
					"Gases",
					UI.PST_KEYWORD,
					" by <b>{0}</b>"
				});

				// Token: 0x0400BFEA RID: 49130
				public static LocString LIQUIDCOOLING = string.Concat(new string[]
				{
					"Reduces the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of piped ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" by <b>{0}</b>"
				});

				// Token: 0x0400BFEB RID: 49131
				public static LocString MAX_WATTAGE = string.Concat(new string[]
				{
					"Drawing more than the maximum allowed ",
					UI.PRE_KEYWORD,
					"Watts",
					UI.PST_KEYWORD,
					" can result in damage to the circuit"
				});

				// Token: 0x0400BFEC RID: 49132
				public static LocString MAX_BITS = string.Concat(new string[]
				{
					"Sending an ",
					UI.PRE_KEYWORD,
					"Automation Signal",
					UI.PST_KEYWORD,
					" with a higher ",
					UI.PRE_KEYWORD,
					"Bit Depth",
					UI.PST_KEYWORD,
					" than the connected ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" can result in damage to the circuit"
				});

				// Token: 0x0400BFED RID: 49133
				public static LocString RESEARCH_MATERIALS = string.Concat(new string[]
				{
					"This research station consumes ",
					UI.FormatAsNegativeRate("{1}"),
					" of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for each ",
					UI.PRE_KEYWORD,
					"Research Point",
					UI.PST_KEYWORD,
					" produced"
				});

				// Token: 0x0400BFEE RID: 49134
				public static LocString PRODUCES_RESEARCH_POINTS = string.Concat(new string[]
				{
					"Produces ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" research"
				});

				// Token: 0x0400BFEF RID: 49135
				public static LocString REMOVES_DISEASE = string.Concat(new string[]
				{
					"The cooking process kills all ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" present in the ingredients, removing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" risk when eating the product"
				});

				// Token: 0x0400BFF0 RID: 49136
				public static LocString DOCTORING = "Doctoring increases existing health benefits and can allow the treatment of otherwise stubborn " + UI.PRE_KEYWORD + "Diseases" + UI.PST_KEYWORD;

				// Token: 0x0400BFF1 RID: 49137
				public static LocString RECREATION = string.Concat(new string[]
				{
					"Improves Duplicant ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" during scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD
				});

				// Token: 0x0400BFF2 RID: 49138
				public static LocString HEATGENERATED_AIRCONDITIONER = string.Concat(new string[]
				{
					"Generates ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" based on the ",
					UI.PRE_KEYWORD,
					"Volume",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Specific Heat Capacity",
					UI.PST_KEYWORD,
					" of the pumped ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					"\n\nCooling 1",
					UI.UNITSUFFIXES.MASS.KILOGRAM,
					" of ",
					ELEMENTS.OXYGEN.NAME,
					" the entire <b>{1}</b> will output <b>{0}</b>"
				});

				// Token: 0x0400BFF3 RID: 49139
				public static LocString HEATGENERATED_LIQUIDCONDITIONER = string.Concat(new string[]
				{
					"Generates ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" based on the ",
					UI.PRE_KEYWORD,
					"Volume",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Specific Heat Capacity",
					UI.PST_KEYWORD,
					" of the pumped ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					"\n\nCooling 10",
					UI.UNITSUFFIXES.MASS.KILOGRAM,
					" of ",
					ELEMENTS.WATER.NAME,
					" the entire <b>{1}</b> will output <b>{0}</b>"
				});

				// Token: 0x0400BFF4 RID: 49140
				public static LocString MOVEMENT_BONUS = "Increases the Runspeed of Duplicants";

				// Token: 0x0400BFF5 RID: 49141
				public static LocString COOLANT = string.Concat(new string[]
				{
					"<b>{1}</b> of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" coolant is required to cool off an item produced by this building\n\nCoolant ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" increase is variable and dictated by the amount of energy needed to cool the produced item"
				});

				// Token: 0x0400BFF6 RID: 49142
				public static LocString REFINEMENT_ENERGY_HAS_COOLANT = string.Concat(new string[]
				{
					UI.FormatAsPositiveRate("{0}"),
					" of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" will be produced to cool off the fabricated item\n\nThis will raise the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of the contained ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" by ",
					UI.FormatAsPositiveModifier("{2}"),
					", and heat the containing building"
				});

				// Token: 0x0400BFF7 RID: 49143
				public static LocString REFINEMENT_ENERGY_NO_COOLANT = string.Concat(new string[]
				{
					UI.FormatAsPositiveRate("{0}"),
					" of ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" will be produced to cool off the fabricated item\n\nIf ",
					UI.PRE_KEYWORD,
					"{1}",
					UI.PST_KEYWORD,
					" is used for coolant, its ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will be raised by ",
					UI.FormatAsPositiveModifier("{2}"),
					", and will heat the containing building"
				});

				// Token: 0x0400BFF8 RID: 49144
				public static LocString IMPROVED_BUILDINGS = UI.PRE_KEYWORD + "Tune Ups" + UI.PST_KEYWORD + " will improve these buildings:";

				// Token: 0x0400BFF9 RID: 49145
				public static LocString IMPROVED_BUILDINGS_ITEM = "{0}";

				// Token: 0x0400BFFA RID: 49146
				public static LocString IMPROVED_PLANTS = UI.PRE_KEYWORD + "Crop Tending" + UI.PST_KEYWORD + " will improve growth times for these plants:";

				// Token: 0x0400BFFB RID: 49147
				public static LocString IMPROVED_PLANTS_ITEM = "{0}";

				// Token: 0x0400BFFC RID: 49148
				public static LocString GEYSER_PRODUCTION = string.Concat(new string[]
				{
					"While erupting, this geyser will produce ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" at a rate of ",
					UI.FormatAsPositiveRate("{1}"),
					", and at a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{2}</b>"
				});

				// Token: 0x0400BFFD RID: 49149
				public static LocString GEYSER_PRODUCTION_GEOTUNED = string.Concat(new string[]
				{
					"While erupting, this geyser will produce ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" at a rate of ",
					UI.FormatAsPositiveRate("{1}"),
					", and at a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{2}</b>"
				});

				// Token: 0x0400BFFE RID: 49150
				public static LocString GEYSER_PRODUCTION_GEOTUNED_COUNT = "<b>{0}</b> of <b>{1}</b> Geotuners targeting this geyser are amplifying it";

				// Token: 0x0400BFFF RID: 49151
				public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL = "Total geotuning: {0} {1}";

				// Token: 0x0400C000 RID: 49152
				public static LocString GEYSER_PRODUCTION_GEOTUNED_TOTAL_ROW_TITLE = "Geotuned ";

				// Token: 0x0400C001 RID: 49153
				public static LocString GEYSER_DISEASE = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + " germs are present in the output of this geyser";

				// Token: 0x0400C002 RID: 49154
				public static LocString GEYSER_PERIOD = "This geyser will produce for <b>{0}</b> of every <b>{1}</b>";

				// Token: 0x0400C003 RID: 49155
				public static LocString GEYSER_YEAR_UNSTUDIED = "A researcher must analyze this geyser to determine its geoactive period";

				// Token: 0x0400C004 RID: 49156
				public static LocString GEYSER_YEAR_PERIOD = "This geyser will be active for <b>{0}</b> out of every <b>{1}</b>\n\nIt will be dormant the rest of the time";

				// Token: 0x0400C005 RID: 49157
				public static LocString GEYSER_YEAR_NEXT_ACTIVE = "This geyser will become active in <b>{0}</b>";

				// Token: 0x0400C006 RID: 49158
				public static LocString GEYSER_YEAR_NEXT_DORMANT = "This geyser will become dormant in <b>{0}</b>";

				// Token: 0x0400C007 RID: 49159
				public static LocString GEYSER_YEAR_AVR_OUTPUT_UNSTUDIED = "A researcher must analyze this geyser to determine its average output rate";

				// Token: 0x0400C008 RID: 49160
				public static LocString GEYSER_YEAR_AVR_OUTPUT = "This geyser emits an average of {average} of {element} during its lifetime\n\nThis includes its dormant period";

				// Token: 0x0400C009 RID: 49161
				public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_TITLE = "Total Geotuning ";

				// Token: 0x0400C00A RID: 49162
				public static LocString GEYSER_YEAR_AVR_OUTPUT_BREAKDOWN_ROW = "Geotuned ";

				// Token: 0x0400C00B RID: 49163
				public static LocString CAPTURE_METHOD_WRANGLE = string.Concat(new string[]
				{
					"This critter can be captured\n\nMark critters for capture using the ",
					UI.FormatAsTool("Wrangle Tool", global::Action.Capture),
					"\n\nDuplicants must possess the ",
					UI.PRE_KEYWORD,
					"Critter Ranching",
					UI.PST_KEYWORD,
					" skill in order to wrangle critters"
				});

				// Token: 0x0400C00C RID: 49164
				public static LocString CAPTURE_METHOD_LURE = "This critter can be moved using an " + STRINGS.BUILDINGS.PREFABS.AIRBORNECREATURELURE.NAME;

				// Token: 0x0400C00D RID: 49165
				public static LocString CAPTURE_METHOD_TRAP = "This critter can be captured using a " + STRINGS.BUILDINGS.PREFABS.CREATURETRAP.NAME;

				// Token: 0x0400C00E RID: 49166
				public static LocString NOISE_POLLUTION_INCREASE = "Produces noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";

				// Token: 0x0400C00F RID: 49167
				public static LocString NOISE_POLLUTION_DECREASE = "Dampens noise at <b>{0} dB</b> in a <b>{1}</b> tile radius";

				// Token: 0x0400C010 RID: 49168
				public static LocString ITEM_TEMPERATURE_ADJUST = string.Concat(new string[]
				{
					"Stored items will reach a ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" of <b>{0}</b> over time"
				});

				// Token: 0x0400C011 RID: 49169
				public static LocString DIET_HEADER = "Creatures will eat and digest only specific materials";

				// Token: 0x0400C012 RID: 49170
				public static LocString DIET_CONSUMED = "This critter can typically consume these materials at the following rates:\n\n{Foodlist}";

				// Token: 0x0400C013 RID: 49171
				public static LocString DIET_PRODUCED = "This critter will \"produce\" the following materials:\n\n{Items}";

				// Token: 0x0400C014 RID: 49172
				public static LocString ROCKETRESTRICTION_HEADER = "Controls whether a building is operational within a rocket interior";

				// Token: 0x0400C015 RID: 49173
				public static LocString ROCKETRESTRICTION_BUILDINGS = "This station controls the operational status of the following buildings:\n\n{buildinglist}";

				// Token: 0x0400C016 RID: 49174
				public static LocString UNSTABLEENTOMBDEFENSEREADY = string.Concat(new string[]
				{
					"This plant is ready to shake off ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements that threaten to entomb it"
				});

				// Token: 0x0400C017 RID: 49175
				public static LocString UNSTABLEENTOMBDEFENSETHREATENED = string.Concat(new string[]
				{
					"This plant is preparing to shake off ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements that are entombing it"
				});

				// Token: 0x0400C018 RID: 49176
				public static LocString UNSTABLEENTOMBDEFENSEREACTING = string.Concat(new string[]
				{
					"This plant is currently unentombing itself from ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements"
				});

				// Token: 0x0400C019 RID: 49177
				public static LocString UNSTABLEENTOMBDEFENSEOFF = string.Concat(new string[]
				{
					"This plant's ability to unentomb itself from ",
					UI.PRE_KEYWORD,
					"Unstable",
					UI.PST_KEYWORD,
					" elements is currently disabled"
				});

				// Token: 0x0400C01A RID: 49178
				public static LocString EDIBLE_PLANT_INTERNAL_STORAGE = "{0} of stored {1}";

				// Token: 0x0400C01B RID: 49179
				public static LocString SCALE_GROWTH = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveModifier("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C01C RID: 49180
				public static LocString SCALE_GROWTH_ATMO = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveRate("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must be kept in ",
					UI.PRE_KEYWORD,
					"{Atmosphere}",
					UI.PST_KEYWORD,
					"-rich environments to regrow sheared ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C01D RID: 49181
				public static LocString SCALE_GROWTH_TEMP = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveRate("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must eat food between {TempMin} - {TempMax} to regrow sheared ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C01E RID: 49182
				public static LocString SCALE_GROWTH_FED = string.Concat(new string[]
				{
					"This critter can be sheared every <b>{Time}</b> to produce ",
					UI.FormatAsPositiveModifier("{Amount}"),
					" of ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD,
					"\n\nIt must be well fed to grow shearable ",
					UI.PRE_KEYWORD,
					"{Item}",
					UI.PST_KEYWORD
				});

				// Token: 0x0400C01F RID: 49183
				public static LocString MESS_TABLE_SALT = string.Concat(new string[]
				{
					"Duplicants gain ",
					UI.FormatAsPositiveModifier("+{0}"),
					" ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when using ",
					UI.PRE_KEYWORD,
					"Table Salt",
					UI.PST_KEYWORD,
					" with their food at a ",
					STRINGS.BUILDINGS.PREFABS.DININGTABLE.NAME
				});

				// Token: 0x0400C020 RID: 49184
				public static LocString ACCESS_CONTROL = "Settings to allow or restrict Duplicants from passing through the door.";

				// Token: 0x0400C021 RID: 49185
				public static LocString TRANSFORMER_INPUT_WIRE = string.Concat(new string[]
				{
					"Connect a ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" to the large ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" with any amount of ",
					UI.PRE_KEYWORD,
					"Watts",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400C022 RID: 49186
				public static LocString TRANSFORMER_OUTPUT_WIRE = string.Concat(new string[]
				{
					"The ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" provided by the the small ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" will be limited to {0}."
				});

				// Token: 0x0400C023 RID: 49187
				public static LocString FABRICATOR_INGREDIENTS = "Ingredients:\n{0}";

				// Token: 0x0400C024 RID: 49188
				public static LocString ACTIVE_PARTICLE_CONSUMPTION = string.Concat(new string[]
				{
					"This building requires ",
					UI.PRE_KEYWORD,
					"Radbolts",
					UI.PST_KEYWORD,
					" to function, consuming them at a rate of {Rate} while in use"
				});

				// Token: 0x0400C025 RID: 49189
				public static LocString PARTICLE_PORT_INPUT = "A Radbolt Port on this building allows it to receive " + UI.PRE_KEYWORD + "Radbolts" + UI.PST_KEYWORD;

				// Token: 0x0400C026 RID: 49190
				public static LocString PARTICLE_PORT_OUTPUT = string.Concat(new string[]
				{
					"This building has a configurable Radbolt Port for ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" emission"
				});

				// Token: 0x0400C027 RID: 49191
				public static LocString IN_ORBIT_REQUIRED = "This building is only operational while its parent rocket is in flight";

				// Token: 0x0400C028 RID: 49192
				public static LocString FOOD_DEHYDRATOR_WATER_OUTPUT = string.Concat(new string[]
				{
					"This building dumps ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" on the floor while in use"
				});

				// Token: 0x0400C029 RID: 49193
				public static LocString KETTLE_MELT_RATE = string.Concat(new string[]
				{
					"This building melts {0} of ",
					UI.PRE_KEYWORD,
					"Ice",
					UI.PST_KEYWORD,
					" into {0} of cold ({1}) ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					"\n\n",
					UI.PRE_KEYWORD,
					"Wood",
					UI.PST_KEYWORD,
					" consumption varies depending on the initial temperature of the ",
					UI.PRE_KEYWORD,
					"Ice",
					UI.PST_KEYWORD
				});
			}
		}

		// Token: 0x02002185 RID: 8581
		public class LOGIC_PORTS
		{
			// Token: 0x04009842 RID: 38978
			public static LocString INPUT_PORTS = UI.FormatAsLink("Auto Inputs", "LOGIC");

			// Token: 0x04009843 RID: 38979
			public static LocString INPUT_PORTS_TOOLTIP = "Input ports change a state on this building when a signal is received";

			// Token: 0x04009844 RID: 38980
			public static LocString OUTPUT_PORTS = UI.FormatAsLink("Auto Outputs", "LOGIC");

			// Token: 0x04009845 RID: 38981
			public static LocString OUTPUT_PORTS_TOOLTIP = "Output ports send a signal when this building changes state";

			// Token: 0x04009846 RID: 38982
			public static LocString INPUT_PORT_TOOLTIP = "Input Behavior:\n• {0}\n• {1}";

			// Token: 0x04009847 RID: 38983
			public static LocString OUTPUT_PORT_TOOLTIP = "Output Behavior:\n• {0}\n• {1}";

			// Token: 0x04009848 RID: 38984
			public static LocString CONTROL_OPERATIONAL = "Enable/Disable";

			// Token: 0x04009849 RID: 38985
			public static LocString CONTROL_OPERATIONAL_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable building";

			// Token: 0x0400984A RID: 38986
			public static LocString CONTROL_OPERATIONAL_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable building";

			// Token: 0x0400984B RID: 38987
			public static LocString PORT_INPUT_DEFAULT_NAME = "INPUT";

			// Token: 0x0400984C RID: 38988
			public static LocString PORT_OUTPUT_DEFAULT_NAME = "OUTPUT";

			// Token: 0x0400984D RID: 38989
			public static LocString GATE_MULTI_INPUT_ONE_NAME = "INPUT A";

			// Token: 0x0400984E RID: 38990
			public static LocString GATE_MULTI_INPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400984F RID: 38991
			public static LocString GATE_MULTI_INPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x04009850 RID: 38992
			public static LocString GATE_MULTI_INPUT_TWO_NAME = "INPUT B";

			// Token: 0x04009851 RID: 38993
			public static LocString GATE_MULTI_INPUT_TWO_ACTIVE = "Green Signal";

			// Token: 0x04009852 RID: 38994
			public static LocString GATE_MULTI_INPUT_TWO_INACTIVE = "Red Signal";

			// Token: 0x04009853 RID: 38995
			public static LocString GATE_MULTI_INPUT_THREE_NAME = "INPUT C";

			// Token: 0x04009854 RID: 38996
			public static LocString GATE_MULTI_INPUT_THREE_ACTIVE = "Green Signal";

			// Token: 0x04009855 RID: 38997
			public static LocString GATE_MULTI_INPUT_THREE_INACTIVE = "Red Signal";

			// Token: 0x04009856 RID: 38998
			public static LocString GATE_MULTI_INPUT_FOUR_NAME = "INPUT D";

			// Token: 0x04009857 RID: 38999
			public static LocString GATE_MULTI_INPUT_FOUR_ACTIVE = "Green Signal";

			// Token: 0x04009858 RID: 39000
			public static LocString GATE_MULTI_INPUT_FOUR_INACTIVE = "Red Signal";

			// Token: 0x04009859 RID: 39001
			public static LocString GATE_SINGLE_INPUT_ONE_NAME = "INPUT";

			// Token: 0x0400985A RID: 39002
			public static LocString GATE_SINGLE_INPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400985B RID: 39003
			public static LocString GATE_SINGLE_INPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400985C RID: 39004
			public static LocString GATE_MULTI_OUTPUT_ONE_NAME = "OUTPUT A";

			// Token: 0x0400985D RID: 39005
			public static LocString GATE_MULTI_OUTPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400985E RID: 39006
			public static LocString GATE_MULTI_OUTPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400985F RID: 39007
			public static LocString GATE_MULTI_OUTPUT_TWO_NAME = "OUTPUT B";

			// Token: 0x04009860 RID: 39008
			public static LocString GATE_MULTI_OUTPUT_TWO_ACTIVE = "Green Signal";

			// Token: 0x04009861 RID: 39009
			public static LocString GATE_MULTI_OUTPUT_TWO_INACTIVE = "Red Signal";

			// Token: 0x04009862 RID: 39010
			public static LocString GATE_MULTI_OUTPUT_THREE_NAME = "OUTPUT C";

			// Token: 0x04009863 RID: 39011
			public static LocString GATE_MULTI_OUTPUT_THREE_ACTIVE = "Green Signal";

			// Token: 0x04009864 RID: 39012
			public static LocString GATE_MULTI_OUTPUT_THREE_INACTIVE = "Red Signal";

			// Token: 0x04009865 RID: 39013
			public static LocString GATE_MULTI_OUTPUT_FOUR_NAME = "OUTPUT D";

			// Token: 0x04009866 RID: 39014
			public static LocString GATE_MULTI_OUTPUT_FOUR_ACTIVE = "Green Signal";

			// Token: 0x04009867 RID: 39015
			public static LocString GATE_MULTI_OUTPUT_FOUR_INACTIVE = "Red Signal";

			// Token: 0x04009868 RID: 39016
			public static LocString GATE_SINGLE_OUTPUT_ONE_NAME = "OUTPUT";

			// Token: 0x04009869 RID: 39017
			public static LocString GATE_SINGLE_OUTPUT_ONE_ACTIVE = "Green Signal";

			// Token: 0x0400986A RID: 39018
			public static LocString GATE_SINGLE_OUTPUT_ONE_INACTIVE = "Red Signal";

			// Token: 0x0400986B RID: 39019
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_NAME = "CONTROL A";

			// Token: 0x0400986C RID: 39020
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position";

			// Token: 0x0400986D RID: 39021
			public static LocString GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position";

			// Token: 0x0400986E RID: 39022
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_NAME = "CONTROL B";

			// Token: 0x0400986F RID: 39023
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set signal path to <b>down</b> position";

			// Token: 0x04009870 RID: 39024
			public static LocString GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Set signal path to <b>up</b> position";
		}

		// Token: 0x02002186 RID: 8582
		public class GAMEOBJECTEFFECTS
		{
			// Token: 0x04009871 RID: 39025
			public static LocString CALORIES = "+{0}";

			// Token: 0x04009872 RID: 39026
			public static LocString FOOD_QUALITY = "Quality: {0}";

			// Token: 0x04009873 RID: 39027
			public static LocString FOOD_MORALE = "Morale: {0}";

			// Token: 0x04009874 RID: 39028
			public static LocString FORGAVEATTACKER = "Forgiveness";

			// Token: 0x04009875 RID: 39029
			public static LocString COLDBREATHER = UI.FormatAsLink("Cooling Effect", "HEAT");

			// Token: 0x04009876 RID: 39030
			public static LocString LIFECYCLETITLE = "Growth:";

			// Token: 0x04009877 RID: 39031
			public static LocString GROWTHTIME_SIMPLE = "Life Cycle: {0}";

			// Token: 0x04009878 RID: 39032
			public static LocString GROWTHTIME_REGROWTH = "Domestic growth: {0} / {1}";

			// Token: 0x04009879 RID: 39033
			public static LocString GROWTHTIME = "Growth: {0}";

			// Token: 0x0400987A RID: 39034
			public static LocString INITIALGROWTHTIME = "Initial Growth: {0}";

			// Token: 0x0400987B RID: 39035
			public static LocString REGROWTHTIME = "Regrowth: {0}";

			// Token: 0x0400987C RID: 39036
			public static LocString REQUIRES_LIGHT = UI.FormatAsLink("Light", "LIGHT") + ": {Lux}";

			// Token: 0x0400987D RID: 39037
			public static LocString REQUIRES_DARKNESS = UI.FormatAsLink("Darkness", "LIGHT");

			// Token: 0x0400987E RID: 39038
			public static LocString REQUIRESFERTILIZER = "{0}: {1}";

			// Token: 0x0400987F RID: 39039
			public static LocString IDEAL_FERTILIZER = "{0}: {1}";

			// Token: 0x04009880 RID: 39040
			public static LocString EQUIPMENT_MODS = "{Attribute} {Value}";

			// Token: 0x04009881 RID: 39041
			public static LocString ROTTEN = "Rotten";

			// Token: 0x04009882 RID: 39042
			public static LocString REQUIRES_ATMOSPHERE = UI.FormatAsLink("Atmosphere", "ATMOSPHERE") + ": {0}";

			// Token: 0x04009883 RID: 39043
			public static LocString REQUIRES_PRESSURE = UI.FormatAsLink("Air", "ATMOSPHERE") + " Pressure: {0} minimum";

			// Token: 0x04009884 RID: 39044
			public static LocString IDEAL_PRESSURE = UI.FormatAsLink("Air", "ATMOSPHERE") + " Pressure: {0}";

			// Token: 0x04009885 RID: 39045
			public static LocString REQUIRES_TEMPERATURE = UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}";

			// Token: 0x04009886 RID: 39046
			public static LocString IDEAL_TEMPERATURE = UI.FormatAsLink("Temperature", "HEAT") + ": {0} to {1}";

			// Token: 0x04009887 RID: 39047
			public static LocString REQUIRES_SUBMERSION = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Submersion";

			// Token: 0x04009888 RID: 39048
			public static LocString FOOD_EFFECTS = "Effects:";

			// Token: 0x04009889 RID: 39049
			public static LocString EMITS_LIGHT = UI.FormatAsLink("Light Range", "LIGHT") + ": {0} tiles";

			// Token: 0x0400988A RID: 39050
			public static LocString EMITS_LIGHT_LUX = UI.FormatAsLink("Brightness", "LIGHT") + ": {0} Lux";

			// Token: 0x0400988B RID: 39051
			public static LocString AMBIENT_RADIATION = "Ambient Radiation";

			// Token: 0x0400988C RID: 39052
			public static LocString AMBIENT_RADIATION_FMT = "{minRads} - {maxRads}";

			// Token: 0x0400988D RID: 39053
			public static LocString AMBIENT_NO_MIN_RADIATION_FMT = "Less than {maxRads}";

			// Token: 0x0400988E RID: 39054
			public static LocString REQUIRES_NO_MIN_RADIATION = "Maximum " + UI.FormatAsLink("Radiation", "RADIATION") + ": {MaxRads}";

			// Token: 0x0400988F RID: 39055
			public static LocString REQUIRES_RADIATION = UI.FormatAsLink("Radiation", "RADIATION") + ": {MinRads} to {MaxRads}";

			// Token: 0x04009890 RID: 39056
			public static LocString MUTANT_STERILE = "Doesn't Drop " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x04009891 RID: 39057
			public static LocString DARKNESS = "Darkness";

			// Token: 0x04009892 RID: 39058
			public static LocString LIGHT = "Light";

			// Token: 0x04009893 RID: 39059
			public static LocString SEED_PRODUCTION_DIG_ONLY = "Consumes 1 " + UI.FormatAsLink("Seed", "PLANTS");

			// Token: 0x04009894 RID: 39060
			public static LocString SEED_PRODUCTION_HARVEST = "Harvest yields " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x04009895 RID: 39061
			public static LocString SEED_PRODUCTION_FINAL_HARVEST = "Final harvest yields " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x04009896 RID: 39062
			public static LocString SEED_PRODUCTION_FRUIT = "Fruit produces " + UI.FormatAsLink("Seeds", "PLANTS");

			// Token: 0x04009897 RID: 39063
			public static LocString SEED_REQUIREMENT_CEILING = "Plot Orientation: Downward";

			// Token: 0x04009898 RID: 39064
			public static LocString SEED_REQUIREMENT_WALL = "Plot Orientation: Sideways";

			// Token: 0x04009899 RID: 39065
			public static LocString REQUIRES_RECEPTACLE = "Farm Plot";

			// Token: 0x0400989A RID: 39066
			public static LocString PLANT_MARK_FOR_HARVEST = "Autoharvest Enabled";

			// Token: 0x0400989B RID: 39067
			public static LocString PLANT_DO_NOT_HARVEST = "Autoharvest Disabled";

			// Token: 0x02002C0A RID: 11274
			public class INSULATED
			{
				// Token: 0x0400C02A RID: 49194
				public static LocString NAME = "Insulated";

				// Token: 0x0400C02B RID: 49195
				public static LocString TOOLTIP = "Proper insulation drastically reduces thermal conductivity";
			}

			// Token: 0x02002C0B RID: 11275
			public class TOOLTIPS
			{
				// Token: 0x0400C02C RID: 49196
				public static LocString CALORIES = "+{0}";

				// Token: 0x0400C02D RID: 49197
				public static LocString FOOD_QUALITY = "Quality: {0}";

				// Token: 0x0400C02E RID: 49198
				public static LocString FOOD_MORALE = "Morale: {0}";

				// Token: 0x0400C02F RID: 49199
				public static LocString COLDBREATHER = "Lowers ambient air temperature";

				// Token: 0x0400C030 RID: 49200
				public static LocString GROWTHTIME_SIMPLE = "This plant takes <b>{0}</b> to grow";

				// Token: 0x0400C031 RID: 49201
				public static LocString GROWTHTIME_REGROWTH = "This plant initially takes <b>{0}</b> to grow, but only <b>{1}</b> to mature after first harvest";

				// Token: 0x0400C032 RID: 49202
				public static LocString GROWTHTIME = "This plant takes <b>{0}</b> to grow";

				// Token: 0x0400C033 RID: 49203
				public static LocString INITIALGROWTHTIME = "This plant takes <b>{0}</b> to mature again once replanted";

				// Token: 0x0400C034 RID: 49204
				public static LocString REGROWTHTIME = "This plant takes <b>{0}</b> to mature again once harvested";

				// Token: 0x0400C035 RID: 49205
				public static LocString EQUIPMENT_MODS = "{Attribute} {Value}";

				// Token: 0x0400C036 RID: 49206
				public static LocString REQUIRESFERTILIZER = string.Concat(new string[]
				{
					"This plant requires <b>{1}</b> ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400C037 RID: 49207
				public static LocString IDEAL_FERTILIZER = string.Concat(new string[]
				{
					"This plant requires <b>{1}</b> of ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400C038 RID: 49208
				public static LocString REQUIRES_LIGHT = string.Concat(new string[]
				{
					"This plant requires a ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" source bathing it in at least {Lux}"
				});

				// Token: 0x0400C039 RID: 49209
				public static LocString REQUIRES_DARKNESS = "This plant requires complete darkness";

				// Token: 0x0400C03A RID: 49210
				public static LocString REQUIRES_ATMOSPHERE = "This plant must be submerged in one of the following gases: {0}";

				// Token: 0x0400C03B RID: 49211
				public static LocString REQUIRES_ATMOSPHERE_LIQUID = "This plant must be submerged in one of the following liquids: {0}";

				// Token: 0x0400C03C RID: 49212
				public static LocString REQUIRES_ATMOSPHERE_MIXED = "This plant must be submerged in one of the following gases or liquids: {0}";

				// Token: 0x0400C03D RID: 49213
				public static LocString REQUIRES_PRESSURE = string.Concat(new string[]
				{
					"Ambient ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" pressure must be at least <b>{0}</b> for basic growth"
				});

				// Token: 0x0400C03E RID: 49214
				public static LocString IDEAL_PRESSURE = string.Concat(new string[]
				{
					"This plant requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" pressures above <b>{0}</b> for basic growth"
				});

				// Token: 0x0400C03F RID: 49215
				public static LocString REQUIRES_TEMPERATURE = string.Concat(new string[]
				{
					"Internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" must be between <b>{0}</b> and <b>{1}</b> for basic growth"
				});

				// Token: 0x0400C040 RID: 49216
				public static LocString IDEAL_TEMPERATURE = string.Concat(new string[]
				{
					"This plant requires internal ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" between <b>{0}</b> and <b>{1}</b> for basic growth"
				});

				// Token: 0x0400C041 RID: 49217
				public static LocString REQUIRES_SUBMERSION = string.Concat(new string[]
				{
					"This plant must be fully submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" for basic growth"
				});

				// Token: 0x0400C042 RID: 49218
				public static LocString FOOD_EFFECTS = "Duplicants will gain the following effects from eating this food: {0}";

				// Token: 0x0400C043 RID: 49219
				public static LocString REQUIRES_RECEPTACLE = string.Concat(new string[]
				{
					"This plant must be housed in a ",
					UI.FormatAsLink("Planter Box", "PLANTERBOX"),
					", ",
					UI.FormatAsLink("Farm Tile", "FARMTILE"),
					", or ",
					UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM"),
					" farm to grow domestically"
				});

				// Token: 0x0400C044 RID: 49220
				public static LocString EMITS_LIGHT = string.Concat(new string[]
				{
					"Emits ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					"\n\nDuplicants can operate buildings more quickly when they're well lit"
				});

				// Token: 0x0400C045 RID: 49221
				public static LocString EMITS_LIGHT_LUX = string.Concat(new string[]
				{
					"Emits ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					"\n\nDuplicants can operate buildings more quickly when they're well lit"
				});

				// Token: 0x0400C046 RID: 49222
				public static LocString METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP = "Distribution of meteor types in this shower";

				// Token: 0x0400C047 RID: 49223
				public static LocString SEED_PRODUCTION_DIG_ONLY = "May be replanted, but will produce no further " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400C048 RID: 49224
				public static LocString SEED_PRODUCTION_HARVEST = "Harvesting this plant will yield new " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400C049 RID: 49225
				public static LocString SEED_PRODUCTION_FINAL_HARVEST = string.Concat(new string[]
				{
					"Yields new ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD,
					" on the final harvest of its life cycle"
				});

				// Token: 0x0400C04A RID: 49226
				public static LocString SEED_PRODUCTION_FRUIT = "Consuming this plant's fruit will yield new " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;

				// Token: 0x0400C04B RID: 49227
				public static LocString SEED_REQUIREMENT_CEILING = "This seed must be planted in a downward facing plot\n\nPress " + UI.FormatAsKeyWord("[O]") + " while building farm plots to rotate them";

				// Token: 0x0400C04C RID: 49228
				public static LocString SEED_REQUIREMENT_WALL = "This seed must be planted in a side facing plot\n\nPress " + UI.FormatAsKeyWord("[O]") + " while building farm plots to rotate them";

				// Token: 0x0400C04D RID: 49229
				public static LocString REQUIRES_NO_MIN_RADIATION = "This plant will stop growing if exposed to more than {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400C04E RID: 49230
				public static LocString REQUIRES_RADIATION = "This plant will only grow if it has between {MinRads} and {MaxRads} of " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x0400C04F RID: 49231
				public static LocString MUTANT_SEED_TOOLTIP = "\n\nGrowing near its maximum radiation increases the chance of mutant seeds being produced";

				// Token: 0x0400C050 RID: 49232
				public static LocString MUTANT_STERILE = "This plant will not produce seeds of its own due to changes to its DNA";
			}

			// Token: 0x02002C0C RID: 11276
			public class DAMAGE_POPS
			{
				// Token: 0x0400C051 RID: 49233
				public static LocString OVERHEAT = "Overheat Damage";

				// Token: 0x0400C052 RID: 49234
				public static LocString CORROSIVE_ELEMENT = "Corrosive Element Damage";

				// Token: 0x0400C053 RID: 49235
				public static LocString WRONG_ELEMENT = "Wrong Element Damage";

				// Token: 0x0400C054 RID: 49236
				public static LocString CIRCUIT_OVERLOADED = "Overload Damage";

				// Token: 0x0400C055 RID: 49237
				public static LocString LOGIC_CIRCUIT_OVERLOADED = "Signal Overload Damage";

				// Token: 0x0400C056 RID: 49238
				public static LocString LIQUID_PRESSURE = "Pressure Damage";

				// Token: 0x0400C057 RID: 49239
				public static LocString MINION_DESTRUCTION = "Tantrum Damage";

				// Token: 0x0400C058 RID: 49240
				public static LocString CONDUIT_CONTENTS_FROZE = "Cold Damage";

				// Token: 0x0400C059 RID: 49241
				public static LocString CONDUIT_CONTENTS_BOILED = "Heat Damage";

				// Token: 0x0400C05A RID: 49242
				public static LocString MICROMETEORITE = "Micrometeorite Damage";

				// Token: 0x0400C05B RID: 49243
				public static LocString COMET = "Meteor Damage";

				// Token: 0x0400C05C RID: 49244
				public static LocString ROCKET = "Rocket Thruster Damage";
			}
		}

		// Token: 0x02002187 RID: 8583
		public class ASTEROIDCLOCK
		{
			// Token: 0x0400989C RID: 39068
			public static LocString CYCLE = "Cycle";

			// Token: 0x0400989D RID: 39069
			public static LocString CYCLES_OLD = "This Colony is {0} Cycle(s) Old";

			// Token: 0x0400989E RID: 39070
			public static LocString TIME_PLAYED = "Time Played: {0} hours";

			// Token: 0x0400989F RID: 39071
			public static LocString SCHEDULE_BUTTON_TOOLTIP = "Manage Schedule";

			// Token: 0x040098A0 RID: 39072
			public static LocString MILESTONE_TITLE = "Approaching Milestone";

			// Token: 0x040098A1 RID: 39073
			public static LocString MILESTONE_DESCRIPTION = "This colony is about to hit Cycle {0}!";
		}

		// Token: 0x02002188 RID: 8584
		public class ENDOFDAYREPORT
		{
			// Token: 0x040098A2 RID: 39074
			public static LocString REPORT_TITLE = "DAILY REPORTS";

			// Token: 0x040098A3 RID: 39075
			public static LocString DAY_TITLE = "Cycle {0}";

			// Token: 0x040098A4 RID: 39076
			public static LocString DAY_TITLE_TODAY = "Cycle {0} - Today";

			// Token: 0x040098A5 RID: 39077
			public static LocString DAY_TITLE_YESTERDAY = "Cycle {0} - Yesterday";

			// Token: 0x040098A6 RID: 39078
			public static LocString NOTIFICATION_TITLE = "Cycle {0} report ready";

			// Token: 0x040098A7 RID: 39079
			public static LocString NOTIFICATION_TOOLTIP = "The daily report for Cycle {0} is ready to view";

			// Token: 0x040098A8 RID: 39080
			public static LocString NEXT = "Next";

			// Token: 0x040098A9 RID: 39081
			public static LocString PREV = "Prev";

			// Token: 0x040098AA RID: 39082
			public static LocString ADDED = "Added";

			// Token: 0x040098AB RID: 39083
			public static LocString REMOVED = "Removed";

			// Token: 0x040098AC RID: 39084
			public static LocString NET = "Net";

			// Token: 0x040098AD RID: 39085
			public static LocString DUPLICANT_DETAILS_HEADER = "Duplicant Details:";

			// Token: 0x040098AE RID: 39086
			public static LocString TIME_DETAILS_HEADER = "Total Time Details:";

			// Token: 0x040098AF RID: 39087
			public static LocString BASE_DETAILS_HEADER = "Base Details:";

			// Token: 0x040098B0 RID: 39088
			public static LocString AVERAGE_TIME_DETAILS_HEADER = "Average Time Details:";

			// Token: 0x040098B1 RID: 39089
			public static LocString MY_COLONY = "my colony";

			// Token: 0x040098B2 RID: 39090
			public static LocString NONE = "None";

			// Token: 0x02002C0D RID: 11277
			public class OXYGEN_CREATED
			{
				// Token: 0x0400C05D RID: 49245
				public static LocString NAME = UI.FormatAsLink("Oxygen", "OXYGEN") + " Generation:";

				// Token: 0x0400C05E RID: 49246
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Oxygen", "OXYGEN") + " was produced by {1} over the course of the day";

				// Token: 0x0400C05F RID: 49247
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Oxygen", "OXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002C0E RID: 11278
			public class CALORIES_CREATED
			{
				// Token: 0x0400C060 RID: 49248
				public static LocString NAME = "Calorie Generation:";

				// Token: 0x0400C061 RID: 49249
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Food", "FOOD") + " was produced by {1} over the course of the day";

				// Token: 0x0400C062 RID: 49250
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Food", "FOOD") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002C0F RID: 11279
			public class NUMBER_OF_DOMESTICATED_CRITTERS
			{
				// Token: 0x0400C063 RID: 49251
				public static LocString NAME = "Domesticated Critters:";

				// Token: 0x0400C064 RID: 49252
				public static LocString POSITIVE_TOOLTIP = "{0} domestic critters live in {1}";

				// Token: 0x0400C065 RID: 49253
				public static LocString NEGATIVE_TOOLTIP = "{0} domestic critters live in {1}";
			}

			// Token: 0x02002C10 RID: 11280
			public class NUMBER_OF_WILD_CRITTERS
			{
				// Token: 0x0400C066 RID: 49254
				public static LocString NAME = "Wild Critters:";

				// Token: 0x0400C067 RID: 49255
				public static LocString POSITIVE_TOOLTIP = "{0} wild critters live in {1}";

				// Token: 0x0400C068 RID: 49256
				public static LocString NEGATIVE_TOOLTIP = "{0} wild critters live in {1}";
			}

			// Token: 0x02002C11 RID: 11281
			public class ROCKETS_IN_FLIGHT
			{
				// Token: 0x0400C069 RID: 49257
				public static LocString NAME = "Rocket Missions Underway:";

				// Token: 0x0400C06A RID: 49258
				public static LocString POSITIVE_TOOLTIP = "{0} rockets are currently flying missions for {1}";

				// Token: 0x0400C06B RID: 49259
				public static LocString NEGATIVE_TOOLTIP = "{0} rockets are currently flying missions for {1}";
			}

			// Token: 0x02002C12 RID: 11282
			public class STRESS_DELTA
			{
				// Token: 0x0400C06C RID: 49260
				public static LocString NAME = UI.FormatAsLink("Stress", "STRESS") + " Change:";

				// Token: 0x0400C06D RID: 49261
				public static LocString POSITIVE_TOOLTIP = UI.FormatAsLink("Stress", "STRESS") + " increased by a total of {0} for {1}";

				// Token: 0x0400C06E RID: 49262
				public static LocString NEGATIVE_TOOLTIP = UI.FormatAsLink("Stress", "STRESS") + " decreased by a total of {0} for {1}";
			}

			// Token: 0x02002C13 RID: 11283
			public class TRAVELTIMEWARNING
			{
				// Token: 0x0400C06F RID: 49263
				public static LocString WARNING_TITLE = "Long Commutes";

				// Token: 0x0400C070 RID: 49264
				public static LocString WARNING_MESSAGE = "My Duplicants are spending a significant amount of time traveling between their errands (> {0})";
			}

			// Token: 0x02002C14 RID: 11284
			public class TRAVEL_TIME
			{
				// Token: 0x0400C071 RID: 49265
				public static LocString NAME = "Travel Time:";

				// Token: 0x0400C072 RID: 49266
				public static LocString POSITIVE_TOOLTIP = "On average, {1} spent {0} of their time traveling between tasks";
			}

			// Token: 0x02002C15 RID: 11285
			public class WORK_TIME
			{
				// Token: 0x0400C073 RID: 49267
				public static LocString NAME = "Work Time:";

				// Token: 0x0400C074 RID: 49268
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent working";
			}

			// Token: 0x02002C16 RID: 11286
			public class IDLE_TIME
			{
				// Token: 0x0400C075 RID: 49269
				public static LocString NAME = "Idle Time:";

				// Token: 0x0400C076 RID: 49270
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent idling";
			}

			// Token: 0x02002C17 RID: 11287
			public class PERSONAL_TIME
			{
				// Token: 0x0400C077 RID: 49271
				public static LocString NAME = "Personal Time:";

				// Token: 0x0400C078 RID: 49272
				public static LocString POSITIVE_TOOLTIP = "On average, {0} of {1}'s time was spent tending to personal needs";
			}

			// Token: 0x02002C18 RID: 11288
			public class ENERGY_USAGE
			{
				// Token: 0x0400C079 RID: 49273
				public static LocString NAME = UI.FormatAsLink("Power", "POWER") + " Usage:";

				// Token: 0x0400C07A RID: 49274
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was created by {1} over the course of the day";

				// Token: 0x0400C07B RID: 49275
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002C19 RID: 11289
			public class ENERGY_WASTED
			{
				// Token: 0x0400C07C RID: 49276
				public static LocString NAME = UI.FormatAsLink("Power", "POWER") + " Wasted:";

				// Token: 0x0400C07D RID: 49277
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Power", "POWER") + " was lost today due to battery runoff and overproduction in {1}";
			}

			// Token: 0x02002C1A RID: 11290
			public class LEVEL_UP
			{
				// Token: 0x0400C07E RID: 49278
				public static LocString NAME = "Skill Increases:";

				// Token: 0x0400C07F RID: 49279
				public static LocString TOOLTIP = "Today {1} gained a total of {0} skill levels";
			}

			// Token: 0x02002C1B RID: 11291
			public class TOILET_INCIDENT
			{
				// Token: 0x0400C080 RID: 49280
				public static LocString NAME = "Restroom Accidents:";

				// Token: 0x0400C081 RID: 49281
				public static LocString TOOLTIP = "{0} Duplicants couldn't quite reach the toilet in time today";
			}

			// Token: 0x02002C1C RID: 11292
			public class DISEASE_ADDED
			{
				// Token: 0x0400C082 RID: 49282
				public static LocString NAME = UI.FormatAsLink("Diseases", "DISEASE") + " Contracted:";

				// Token: 0x0400C083 RID: 49283
				public static LocString POSITIVE_TOOLTIP = "{0} " + UI.FormatAsLink("Disease", "DISEASE") + " were contracted by {1}";

				// Token: 0x0400C084 RID: 49284
				public static LocString NEGATIVE_TOOLTIP = "{0} " + UI.FormatAsLink("Disease", "DISEASE") + " were cured by {1}";
			}

			// Token: 0x02002C1D RID: 11293
			public class CONTAMINATED_OXYGEN_FLATULENCE
			{
				// Token: 0x0400C085 RID: 49285
				public static LocString NAME = UI.FormatAsLink("Flatulence", "CONTAMINATEDOXYGEN") + " Generation:";

				// Token: 0x0400C086 RID: 49286
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400C087 RID: 49287
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002C1E RID: 11294
			public class CONTAMINATED_OXYGEN_TOILET
			{
				// Token: 0x0400C088 RID: 49288
				public static LocString NAME = UI.FormatAsLink("Toilet Emissions: ", "CONTAMINATEDOXYGEN");

				// Token: 0x0400C089 RID: 49289
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400C08A RID: 49290
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002C1F RID: 11295
			public class CONTAMINATED_OXYGEN_SUBLIMATION
			{
				// Token: 0x0400C08B RID: 49291
				public static LocString NAME = UI.FormatAsLink("Sublimation", "CONTAMINATEDOXYGEN") + ":";

				// Token: 0x0400C08C RID: 49292
				public static LocString POSITIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was generated by {1} over the course of the day";

				// Token: 0x0400C08D RID: 49293
				public static LocString NEGATIVE_TOOLTIP = "{0} of " + UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN") + " was consumed by {1} over the course of the day";
			}

			// Token: 0x02002C20 RID: 11296
			public class DISEASE_STATUS
			{
				// Token: 0x0400C08E RID: 49294
				public static LocString NAME = "Disease Status:";

				// Token: 0x0400C08F RID: 49295
				public static LocString TOOLTIP = "There are {0} covering {1}";
			}

			// Token: 0x02002C21 RID: 11297
			public class CHORE_STATUS
			{
				// Token: 0x0400C090 RID: 49296
				public static LocString NAME = "Errands:";

				// Token: 0x0400C091 RID: 49297
				public static LocString POSITIVE_TOOLTIP = "{0} errands are queued for {1}";

				// Token: 0x0400C092 RID: 49298
				public static LocString NEGATIVE_TOOLTIP = "{0} errands were completed over the course of the day by {1}";
			}

			// Token: 0x02002C22 RID: 11298
			public class NOTES
			{
				// Token: 0x0400C093 RID: 49299
				public static LocString NOTE_ENTRY_LINE_ITEM = "{0}\n{1}: {2}";

				// Token: 0x0400C094 RID: 49300
				public static LocString BUTCHERED = "Butchered for {0}";

				// Token: 0x0400C095 RID: 49301
				public static LocString BUTCHERED_CONTEXT = "Butchered";

				// Token: 0x0400C096 RID: 49302
				public static LocString CRAFTED = "Crafted a {0}";

				// Token: 0x0400C097 RID: 49303
				public static LocString CRAFTED_USED = "{0} used as ingredient";

				// Token: 0x0400C098 RID: 49304
				public static LocString CRAFTED_CONTEXT = "Crafted";

				// Token: 0x0400C099 RID: 49305
				public static LocString HARVESTED = "Harvested {0}";

				// Token: 0x0400C09A RID: 49306
				public static LocString HARVESTED_CONTEXT = "Harvested";

				// Token: 0x0400C09B RID: 49307
				public static LocString EATEN = "{0} eaten";

				// Token: 0x0400C09C RID: 49308
				public static LocString ROTTED = "Rotten {0}";

				// Token: 0x0400C09D RID: 49309
				public static LocString ROTTED_CONTEXT = "Rotted";

				// Token: 0x0400C09E RID: 49310
				public static LocString GERMS = "On {0}";

				// Token: 0x0400C09F RID: 49311
				public static LocString TIME_SPENT = "{0}";

				// Token: 0x0400C0A0 RID: 49312
				public static LocString WORK_TIME = "{0}";

				// Token: 0x0400C0A1 RID: 49313
				public static LocString PERSONAL_TIME = "{0}";

				// Token: 0x0400C0A2 RID: 49314
				public static LocString FOODFIGHT_CONTEXT = "{0} ingested in food fight";
			}
		}

		// Token: 0x02002189 RID: 8585
		public static class SCHEDULEBLOCKTYPES
		{
			// Token: 0x02002C23 RID: 11299
			public static class EAT
			{
				// Token: 0x0400C0A3 RID: 49315
				public static LocString NAME = "Mealtime";

				// Token: 0x0400C0A4 RID: 49316
				public static LocString DESCRIPTION = "EAT:\nDuring Mealtime Duplicants will head to their assigned mess halls and eat.";
			}

			// Token: 0x02002C24 RID: 11300
			public static class SLEEP
			{
				// Token: 0x0400C0A5 RID: 49317
				public static LocString NAME = "Sleep";

				// Token: 0x0400C0A6 RID: 49318
				public static LocString DESCRIPTION = "SLEEP:\nWhen it's time to sleep, Duplicants will head to their assigned rooms and rest.";
			}

			// Token: 0x02002C25 RID: 11301
			public static class WORK
			{
				// Token: 0x0400C0A7 RID: 49319
				public static LocString NAME = "Work";

				// Token: 0x0400C0A8 RID: 49320
				public static LocString DESCRIPTION = "WORK:\nDuring Work hours Duplicants will perform any pending errands in the colony.";
			}

			// Token: 0x02002C26 RID: 11302
			public static class RECREATION
			{
				// Token: 0x0400C0A9 RID: 49321
				public static LocString NAME = "Recreation";

				// Token: 0x0400C0AA RID: 49322
				public static LocString DESCRIPTION = "HAMMER TIME:\nDuring Hammer Time, Duplicants will relieve their " + UI.FormatAsLink("Stress", "STRESS") + " through dance. Please be aware that no matter how hard my Duplicants try, they will absolutely not be able to touch this.";
			}

			// Token: 0x02002C27 RID: 11303
			public static class HYGIENE
			{
				// Token: 0x0400C0AB RID: 49323
				public static LocString NAME = "Hygiene";

				// Token: 0x0400C0AC RID: 49324
				public static LocString DESCRIPTION = "HYGIENE:\nDuring " + UI.FormatAsLink("Hygiene", "HYGIENE") + " hours Duplicants will head to their assigned washrooms to get cleaned up.";
			}
		}

		// Token: 0x0200218A RID: 8586
		public static class SCHEDULEGROUPS
		{
			// Token: 0x040098B3 RID: 39091
			public static LocString TOOLTIP_FORMAT = "{0}\n\n{1}";

			// Token: 0x040098B4 RID: 39092
			public static LocString MISSINGBLOCKS = "Warning: Scheduling Issues ({0})";

			// Token: 0x040098B5 RID: 39093
			public static LocString NOTIME = "No {0} shifts allotted";

			// Token: 0x02002C28 RID: 11304
			public static class HYGENE
			{
				// Token: 0x0400C0AD RID: 49325
				public static LocString NAME = "Bathtime";

				// Token: 0x0400C0AE RID: 49326
				public static LocString DESCRIPTION = "During Bathtime shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands.\n\nOnce they're all caught up on personal hygiene, Duplicants will head back to work.";

				// Token: 0x0400C0AF RID: 49327
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Bathtime",
					UI.PST_KEYWORD,
					" shifts my Duplicants will take care of their hygienic needs, such as going to the bathroom, using the shower or washing their hands."
				});
			}

			// Token: 0x02002C29 RID: 11305
			public static class WORKTIME
			{
				// Token: 0x0400C0B0 RID: 49328
				public static LocString NAME = "Work";

				// Token: 0x0400C0B1 RID: 49329
				public static LocString DESCRIPTION = "During Work shifts my Duplicants must perform the errands I have placed for them throughout the colony.\n\nIt's important when scheduling to maintain a good work-life balance for my Duplicants to maintain their health and prevent Morale loss.";

				// Token: 0x0400C0B2 RID: 49330
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Work",
					UI.PST_KEYWORD,
					" shifts my Duplicants must perform the errands I've placed for them throughout the colony."
				});
			}

			// Token: 0x02002C2A RID: 11306
			public static class RECREATION
			{
				// Token: 0x0400C0B3 RID: 49331
				public static LocString NAME = "Downtime";

				// Token: 0x0400C0B4 RID: 49332
				public static LocString DESCRIPTION = "During Downtime my Duplicants they may do as they please.\n\nThis may include personal matters like bathroom visits or snacking, or they may choose to engage in leisure activities like socializing with friends.\n\nDowntime increases Duplicant Morale.";

				// Token: 0x0400C0B5 RID: 49333
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"During ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts my Duplicants they may do as they please."
				});
			}

			// Token: 0x02002C2B RID: 11307
			public static class SLEEP
			{
				// Token: 0x0400C0B6 RID: 49334
				public static LocString NAME = "Bedtime";

				// Token: 0x0400C0B7 RID: 49335
				public static LocString DESCRIPTION = "My Duplicants use Bedtime shifts to rest up after a hard day's work.\n\nScheduling too few bedtime shifts may prevent my Duplicants from regaining enough Stamina to make it through the following day.";

				// Token: 0x0400C0B8 RID: 49336
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"My Duplicants use ",
					UI.PRE_KEYWORD,
					"Bedtime",
					UI.PST_KEYWORD,
					" shifts to rest up after a hard day's work."
				});
			}
		}

		// Token: 0x0200218B RID: 8587
		public class ELEMENTAL
		{
			// Token: 0x02002C2C RID: 11308
			public class AGE
			{
				// Token: 0x0400C0B9 RID: 49337
				public static LocString NAME = "Age: {0}";

				// Token: 0x0400C0BA RID: 49338
				public static LocString TOOLTIP = "The selected object is {0} cycles old";

				// Token: 0x0400C0BB RID: 49339
				public static LocString UNKNOWN = "Unknown";

				// Token: 0x0400C0BC RID: 49340
				public static LocString UNKNOWN_TOOLTIP = "The age of the selected object is unknown";
			}

			// Token: 0x02002C2D RID: 11309
			public class UPTIME
			{
				// Token: 0x0400C0BD RID: 49341
				public static LocString NAME = "Uptime:\n{0}{1}: {2}\n{0}{3}: {4}\n{0}{5}: {6}";

				// Token: 0x0400C0BE RID: 49342
				public static LocString THIS_CYCLE = "This Cycle";

				// Token: 0x0400C0BF RID: 49343
				public static LocString LAST_CYCLE = "Last Cycle";

				// Token: 0x0400C0C0 RID: 49344
				public static LocString LAST_X_CYCLES = "Last {0} Cycles";
			}

			// Token: 0x02002C2E RID: 11310
			public class PRIMARYELEMENT
			{
				// Token: 0x0400C0C1 RID: 49345
				public static LocString NAME = "Primary Element: {0}";

				// Token: 0x0400C0C2 RID: 49346
				public static LocString TOOLTIP = "The selected object is primarily composed of {0}";
			}

			// Token: 0x02002C2F RID: 11311
			public class UNITS
			{
				// Token: 0x0400C0C3 RID: 49347
				public static LocString NAME = "Stack Units: {0}";

				// Token: 0x0400C0C4 RID: 49348
				public static LocString TOOLTIP = "This stack contains {0} units of {1}";
			}

			// Token: 0x02002C30 RID: 11312
			public class MASS
			{
				// Token: 0x0400C0C5 RID: 49349
				public static LocString NAME = "Mass: {0}";

				// Token: 0x0400C0C6 RID: 49350
				public static LocString TOOLTIP = "The selected object has a mass of {0}";
			}

			// Token: 0x02002C31 RID: 11313
			public class TEMPERATURE
			{
				// Token: 0x0400C0C7 RID: 49351
				public static LocString NAME = "Temperature: {0}";

				// Token: 0x0400C0C8 RID: 49352
				public static LocString TOOLTIP = "The selected object's current temperature is {0}";
			}

			// Token: 0x02002C32 RID: 11314
			public class DISEASE
			{
				// Token: 0x0400C0C9 RID: 49353
				public static LocString NAME = "Disease: {0}";

				// Token: 0x0400C0CA RID: 49354
				public static LocString TOOLTIP = "There are {0} on the selected object";
			}

			// Token: 0x02002C33 RID: 11315
			public class SHC
			{
				// Token: 0x0400C0CB RID: 49355
				public static LocString NAME = "Specific Heat Capacity: {0}";

				// Token: 0x0400C0CC RID: 49356
				public static LocString TOOLTIP = "{SPECIFIC_HEAT_CAPACITY} is required to heat 1 g of the selected object by 1 {TEMPERATURE_UNIT}";
			}

			// Token: 0x02002C34 RID: 11316
			public class THERMALCONDUCTIVITY
			{
				// Token: 0x0400C0CD RID: 49357
				public static LocString NAME = "Thermal Conductivity: {0}";

				// Token: 0x0400C0CE RID: 49358
				public static LocString TOOLTIP = "This object can conduct heat to other materials at a rate of {THERMAL_CONDUCTIVITY} W for each degree {TEMPERATURE_UNIT} difference\n\nBetween two objects, the rate of heat transfer will be determined by the object with the lowest Thermal Conductivity";

				// Token: 0x02003780 RID: 14208
				public class ADJECTIVES
				{
					// Token: 0x0400DCB5 RID: 56501
					public static LocString VALUE_WITH_ADJECTIVE = "{0} ({1})";

					// Token: 0x0400DCB6 RID: 56502
					public static LocString VERY_LOW_CONDUCTIVITY = "Highly Insulating";

					// Token: 0x0400DCB7 RID: 56503
					public static LocString LOW_CONDUCTIVITY = "Insulating";

					// Token: 0x0400DCB8 RID: 56504
					public static LocString MEDIUM_CONDUCTIVITY = "Conductive";

					// Token: 0x0400DCB9 RID: 56505
					public static LocString HIGH_CONDUCTIVITY = "Highly Conductive";

					// Token: 0x0400DCBA RID: 56506
					public static LocString VERY_HIGH_CONDUCTIVITY = "Extremely Conductive";
				}
			}

			// Token: 0x02002C35 RID: 11317
			public class CONDUCTIVITYBARRIER
			{
				// Token: 0x0400C0CF RID: 49359
				public static LocString NAME = "Insulation Thickness: {0}";

				// Token: 0x0400C0D0 RID: 49360
				public static LocString TOOLTIP = "Thick insulation reduces an object's Thermal Conductivity";
			}

			// Token: 0x02002C36 RID: 11318
			public class VAPOURIZATIONPOINT
			{
				// Token: 0x0400C0D1 RID: 49361
				public static LocString NAME = "Vaporization Point: {0}";

				// Token: 0x0400C0D2 RID: 49362
				public static LocString TOOLTIP = "The selected object will evaporate into a gas at {0}";
			}

			// Token: 0x02002C37 RID: 11319
			public class MELTINGPOINT
			{
				// Token: 0x0400C0D3 RID: 49363
				public static LocString NAME = "Melting Point: {0}";

				// Token: 0x0400C0D4 RID: 49364
				public static LocString TOOLTIP = "The selected object will melt into a liquid at {0}";
			}

			// Token: 0x02002C38 RID: 11320
			public class OVERHEATPOINT
			{
				// Token: 0x0400C0D5 RID: 49365
				public static LocString NAME = "Overheat Modifier: {0}";

				// Token: 0x0400C0D6 RID: 49366
				public static LocString TOOLTIP = "This building will overheat and take damage if its temperature reaches {0}\n\nBuilding with better building materials can increase overheat temperature";
			}

			// Token: 0x02002C39 RID: 11321
			public class FREEZEPOINT
			{
				// Token: 0x0400C0D7 RID: 49367
				public static LocString NAME = "Freeze Point: {0}";

				// Token: 0x0400C0D8 RID: 49368
				public static LocString TOOLTIP = "The selected object will cool into a solid at {0}";
			}

			// Token: 0x02002C3A RID: 11322
			public class DEWPOINT
			{
				// Token: 0x0400C0D9 RID: 49369
				public static LocString NAME = "Condensation Point: {0}";

				// Token: 0x0400C0DA RID: 49370
				public static LocString TOOLTIP = "The selected object will condense into a liquid at {0}";
			}
		}

		// Token: 0x0200218C RID: 8588
		public class IMMIGRANTSCREEN
		{
			// Token: 0x040098B6 RID: 39094
			public static LocString IMMIGRANTSCREENTITLE = "Select a Blueprint";

			// Token: 0x040098B7 RID: 39095
			public static LocString PROCEEDBUTTON = "Print";

			// Token: 0x040098B8 RID: 39096
			public static LocString CANCELBUTTON = "Cancel";

			// Token: 0x040098B9 RID: 39097
			public static LocString REJECTALL = "Reject All";

			// Token: 0x040098BA RID: 39098
			public static LocString EMBARK = "EMBARK";

			// Token: 0x040098BB RID: 39099
			public static LocString SELECTDUPLICANTS = "Select {0} Duplicants";

			// Token: 0x040098BC RID: 39100
			public static LocString SELECTYOURCREW = "CHOOSE THREE DUPLICANTS TO BEGIN";

			// Token: 0x040098BD RID: 39101
			public static LocString SHUFFLE = "REROLL";

			// Token: 0x040098BE RID: 39102
			public static LocString SHUFFLETOOLTIP = "Reroll for a different Duplicant";

			// Token: 0x040098BF RID: 39103
			public static LocString BACK = "BACK";

			// Token: 0x040098C0 RID: 39104
			public static LocString CONFIRMATIONTITLE = "Reject All Printables?";

			// Token: 0x040098C1 RID: 39105
			public static LocString CONFIRMATIONBODY = "The Printing Pod will need time to recharge if I reject these Printables.";

			// Token: 0x040098C2 RID: 39106
			public static LocString NAME_YOUR_COLONY = "NAME THE COLONY";

			// Token: 0x040098C3 RID: 39107
			public static LocString CARE_PACKAGE_ELEMENT_QUANTITY = "{0} of {1}";

			// Token: 0x040098C4 RID: 39108
			public static LocString CARE_PACKAGE_ELEMENT_COUNT = "{0} x {1}";

			// Token: 0x040098C5 RID: 39109
			public static LocString CARE_PACKAGE_ELEMENT_COUNT_ONLY = "x {0}";

			// Token: 0x040098C6 RID: 39110
			public static LocString CARE_PACKAGE_CURRENT_AMOUNT = "Available: {0}";

			// Token: 0x040098C7 RID: 39111
			public static LocString DUPLICATE_COLONY_NAME = "A colony named \"{0}\" already exists";
		}

		// Token: 0x0200218D RID: 8589
		public class METERS
		{
			// Token: 0x02002C3B RID: 11323
			public class HEALTH
			{
				// Token: 0x0400C0DB RID: 49371
				public static LocString TOOLTIP = "Health";
			}

			// Token: 0x02002C3C RID: 11324
			public class BREATH
			{
				// Token: 0x0400C0DC RID: 49372
				public static LocString TOOLTIP = "Oxygen";
			}

			// Token: 0x02002C3D RID: 11325
			public class FUEL
			{
				// Token: 0x0400C0DD RID: 49373
				public static LocString TOOLTIP = "Fuel";
			}

			// Token: 0x02002C3E RID: 11326
			public class BATTERY
			{
				// Token: 0x0400C0DE RID: 49374
				public static LocString TOOLTIP = "Battery Charge";
			}
		}
	}
}
