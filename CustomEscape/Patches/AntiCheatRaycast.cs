using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace CustomEscape.Patches
{
    [HarmonyPatch(typeof(PlayerMovementSync), nameof(PlayerMovementSync.AnticheatRaycast), typeof(Vector3),
        typeof(bool))]
    internal static class AnticheatRaycast
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instr in instructions)
            {
                if (instr.opcode == OpCodes.Call && (MethodInfo) instr.operand ==
                    AccessTools.Method(
                        typeof(Physics),
                        nameof(Physics.RaycastNonAlloc),
                        new[] {typeof(Ray), typeof(RaycastHit[]), typeof(float), typeof(int)}))
                {
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1); // QueryTriggerInteraction.Ignore
                    yield return new CodeInstruction(
                        OpCodes.Call,
                        AccessTools.Method(
                            typeof(Physics),
                            nameof(Physics.RaycastNonAlloc),
                            new[]
                            {
                                typeof(Ray), typeof(RaycastHit[]), typeof(float), typeof(int),
                                typeof(QueryTriggerInteraction)
                            }));

                    continue;
                }

                yield return instr;
            }
        }
    }
}