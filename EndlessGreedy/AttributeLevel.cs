using HarmonyLib;
using Klei.AI;

[HarmonyPatch(typeof(AttributeLevel))]
internal class SuperAttributeLevel
{
	[HarmonyPostfix]
	[HarmonyPatch("GetExperienceForNextLevel")]
	public static void Postfix(ref float __result) => __result = 10f;
}