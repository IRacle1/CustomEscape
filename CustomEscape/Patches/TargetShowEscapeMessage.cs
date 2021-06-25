using HarmonyLib;

namespace CustomEscape.Patches
{
    [HarmonyPatch(typeof(Escape), nameof(Escape.TargetShowEscapeMessage))]
    internal static class TargetShowEscapeMessage
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}