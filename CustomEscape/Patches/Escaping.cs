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
        [HarmonyPrefix]
        private static bool Prefix(CharacterClassManager __instance)
        {
            var ev = new ExtendedEscapingEventArgs(Player.Get(__instance._hub));
            EventHandlers.OnEscaping(ev); // i can do that now, neato
            Exiled.Events.Handlers.Player.OnEscaping(ev);

            if (!ev.IsAllowed)
                return false;

            var cuffed = false;
            foreach (DisarmedPlayers.DisarmedEntry entry in DisarmedPlayers.Entries.Where(entry =>
                entry.DisarmedPlayer == __instance._hub.networkIdentity.netId))
            {
                if (entry.Disarmer == 0U)
                {
                    cuffed = CharacterClassManager.ForceCuffedChangeTeam;
                    break;
                }

                if (ReferenceHub.TryGetHubNetID(entry.Disarmer, out ReferenceHub hub))
                {
                    CharacterClassManager characterClassManager = hub.characterClassManager;
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

            RespawnTickets singleton = RespawnTickets.Singleton;
            switch (__instance.CurRole.team)
            {
                case Team.RSC:
                    if (cuffed)
                    {
                        ++RoundSummary.EscapedClassD;
                        singleton.GrantTickets(SpawnableTeamType.ChaosInsurgency,
                            ConfigFile.ServerConfig.GetInt("respawn_tickets_ci_scientist_cuffed_count", 2));
                        break;
                    }

                    ++RoundSummary.EscapedScientists;
                    singleton.GrantTickets(SpawnableTeamType.NineTailedFox,
                        ConfigFile.ServerConfig.GetInt("respawn_tickets_mtf_scientist_count", 1));
                    break;
                case Team.CDP:
                    if (cuffed)
                    {
                        ++RoundSummary.EscapedScientists;
                        singleton.GrantTickets(SpawnableTeamType.NineTailedFox,
                            ConfigFile.ServerConfig.GetInt("respawn_tickets_mtf_classd_cuffed_count", 1));
                        break;
                    }

                    ++RoundSummary.EscapedClassD;
                    singleton.GrantTickets(SpawnableTeamType.ChaosInsurgency,
                        ConfigFile.ServerConfig.GetInt("respawn_tickets_ci_classd_count", 1));
                    break;
            }

            if (ev.ClearInventory)
                ev.Player.ClearInventory();
            ev.Player.SetRole(ev.NewRole, SpawnReason.Escaped);

            return false;
        }
    }

    public class ExtendedEscapingEventArgs : EscapingEventArgs
    {
        public ExtendedEscapingEventArgs(Player player) : base(player)
        {
        }

        public bool ClearInventory { get; set; }
    }
}
