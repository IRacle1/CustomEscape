using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace CustomEscape.Patches
{
    [HarmonyPatch(typeof(PlayerMovementSync), nameof(PlayerMovementSync.AntiFly))]
    internal static class AntiFly
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instr in instructions)
            {
                if (instr.opcode == OpCodes.Call)
                {
                    if ((MethodInfo) instr.operand == AccessTools.Method(
                        typeof(Physics),
                        nameof(Physics.Linecast),
                        new[] {typeof(Vector3), typeof(Vector3), typeof(int)}))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_1); // QueryTriggerInteraction.Ignore
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(
                            typeof(Physics),
                            nameof(Physics.Linecast),
                            new[] {typeof(Vector3), typeof(Vector3), typeof(int), typeof(QueryTriggerInteraction)}));

                        continue;
                    }

                    if ((MethodInfo) instr.operand == AccessTools.Method(
                        typeof(Physics),
                        nameof(Physics.OverlapBoxNonAlloc),
                        new[] {typeof(Vector3), typeof(Vector3), typeof(Collider[]), typeof(Quaternion), typeof(int)}))
                    {
                        yield return new CodeInstruction(OpCodes.Ldc_I4_1); // QueryTriggerInteraction.Ignore
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(
                            typeof(Physics),
                            nameof(Physics.OverlapBoxNonAlloc),
                            new[]
                            {
                                typeof(Vector3), typeof(Vector3), typeof(Collider[]), typeof(Quaternion), typeof(int),
                                typeof(QueryTriggerInteraction)
                            }));

                        continue;
                    }
                }

                yield return instr;
            }
        }
    }
}