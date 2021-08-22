namespace CustomEscape.Patches
{
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using GameCore;
    using HarmonyLib;
    using InventorySystem.Disarming;
    using Respawning;

    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.UserCode_CmdRegisterEscape))]
    internal static class Escaping
    {
        private static bool Prefix(CharacterClassManager __instance)
        {
            var ev = new EscapingEventArgs(Player.Get(__instance._hub));
            EventHandlers.OnEscaping(ev); // i can do that now, neato
            Exiled.Events.Handlers.Player.OnEscaping(ev);

            if (!ev.IsAllowed)
                return false;

            var cuffed = false;
            foreach (var entry in DisarmedPlayers.Entries.Where(entry =>
                (int) entry.DisarmedPlayer == (int) __instance._hub.networkIdentity.netId))
            {
                if (entry.Disarmer == 0U)
                {
                    cuffed = CharacterClassManager.ForceCuffedChangeTeam;
                    break;
                }

                if (ReferenceHub.TryGetHubNetID(entry.Disarmer, out var hub))
                {
                    var characterClassManager = hub.characterClassManager;
                    if (__instance.CurClass == RoleType.Scientist &&
                        characterClassManager.Faction == Faction.FoundationEnemy ||
                        __instance.CurClass == RoleType.ClassD &&
                        characterClassManager.Faction == Faction.FoundationStaff)
                        cuffed = true;
                }
                else
                {
                    break;
                }
            }

            var singleton = RespawnTickets.Singleton;
            switch (__instance.CurRole.team)
            {
                case Team.RSC:
                    if (cuffed)
                    {
                        // __instance.SetPlayersClass(RoleType.ChaosConscript, __instance.gameObject,
                        //     CharacterClassManager.SpawnReason.Escaped);
                        ++RoundSummary.escaped_ds;
                        singleton.GrantTickets(SpawnableTeamType.ChaosInsurgency,
                            ConfigFile.ServerConfig.GetInt("respawn_tickets_ci_scientist_cuffed_count", 2));
                        // __instance.GetComponent<Escape>()
                        //     .TargetShowEscapeMessage(__instance.connectionToClient, false, true);
                        break;
                    }

                    // __instance.SetPlayersClass(RoleType.NtfSpecialist, __instance.gameObject,
                    //     CharacterClassManager.SpawnReason.Escaped);
                    ++RoundSummary.escaped_scientists;
                    singleton.GrantTickets(SpawnableTeamType.NineTailedFox,
                        ConfigFile.ServerConfig.GetInt("respawn_tickets_mtf_scientist_count", 1));
                    // __instance.GetComponent<Escape>()
                    //     .TargetShowEscapeMessage(__instance.connectionToClient, false, false);
                    break;
                case Team.CDP:
                    if (cuffed)
                    {
                        // __instance.SetPlayersClass(RoleType.NtfPrivate, __instance.gameObject,
                        //     CharacterClassManager.SpawnReason.Escaped);
                        ++RoundSummary.escaped_scientists;
                        singleton.GrantTickets(SpawnableTeamType.NineTailedFox,
                            ConfigFile.ServerConfig.GetInt("respawn_tickets_mtf_classd_cuffed_count", 1));
                        // __instance.GetComponent<Escape>()
                        //     .TargetShowEscapeMessage(__instance.connectionToClient, true, true);
                        break;
                    }

                    // __instance.SetPlayersClass(RoleType.ChaosConscript, __instance.gameObject,
                    //     CharacterClassManager.SpawnReason.Escaped);
                    ++RoundSummary.escaped_ds;
                    singleton.GrantTickets(SpawnableTeamType.ChaosInsurgency,
                        ConfigFile.ServerConfig.GetInt("respawn_tickets_ci_classd_count", 1));
                    // __instance.GetComponent<Escape>()
                    //     .TargetShowEscapeMessage(__instance.connectionToClient, true, false);
                    break;
            }

            ev.Player.SetRole(ev.NewRole, SpawnReason.Escaped);

            return false;
        }
    }
}