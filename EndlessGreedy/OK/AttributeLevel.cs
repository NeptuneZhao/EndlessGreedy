using HarmonyLib;
using Klei.AI;
using UnityEngine;

[HarmonyPatch(typeof(AttributeLevel))]
internal class SuperAttributeLevel
{
	[HarmonyPostfix, HarmonyPatch(nameof(AttributeLevel.GetExperienceForNextLevel))]
	public static void Postfix(ref float __result) => __result = 1f;

	[HarmonyPrefix, HarmonyPatch(nameof(AttributeLevel.AddExperience), new System.Type[] { typeof(AttributeLevels), typeof(float) })]
	public static bool Prefix(AttributeLevel __instance, AttributeLevels levels, float experience, ref bool __result)
	{
		__instance.experience += experience;
		__instance.experience = Mathf.Max(0f, __instance.experience);
		if (__instance.experience >= __instance.GetExperienceForNextLevel() && __instance.GetLevel() < 10000)
		{
			__instance.LevelUp(levels);
			__result = true;
			return true;
		}
		__result = false;
		return false;
}
}