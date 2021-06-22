using System.Collections.Generic;
using System.Linq;
using ArithFeather.Points;
using ArithFeather.Points.DataTypes;
using Exiled.Events.EventArgs;
using GameCore;
using MEC;
using Respawning;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace CustomEscape
{
    public static class EventHandlers
    {
        public const string SessionVariable = "plugin_escaping_collider_name";

        private static PointList _pointsPointList;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private static Dictionary<string, GameObject> _escapePosDict = new Dictionary<string, GameObject>();

        public static void OnGenerated()
        {
            Timing.CallDelayed(.5f, () =>
            {
                foreach (var kvp in _escapePosDict)
                    Object.Destroy(kvp.Value);

                _escapePosDict.Clear();

                _pointsPointList = Points.GetPointList(CustomEscape.Singleton.Config.PointsFileName);
                _pointsPointList.FixData();

                Log.Debug(
                    "Raw points: " + _pointsPointList.RawPoints.Count + " " +
                    string.Join(",", _pointsPointList.RawPoints.Select(x => x.RoomType)),
                    CustomEscape.Singleton.Config.Debug);
                Log.Debug(
                    "Fixed points: " + _pointsPointList.FixedPoints.Count + " " +
                    string.Join(",", _pointsPointList.FixedPoints.Select(x => x.Room)),
                    CustomEscape.Singleton.Config.Debug);
                Log.Debug(
                    "Escape points: " + CustomEscape.Singleton.Config.EscapePoints.Count + " " +
                    string.Join(",",
                        CustomEscape.Singleton.Config.EscapePoints.Select(x => x.Key)),
                    CustomEscape.Singleton.Config.Debug);

                foreach (var escapePoint in CustomEscape.Singleton.Config.EscapePoints)
                {
                    var fixedPoint = _pointsPointList.FixedPoints.FirstOrDefault(x => x.Id == escapePoint.Key);
                    if (fixedPoint == null)
                    {
                        Log.Error("Unknown Id while trying to create escape point: " + escapePoint.Key);
                        continue;
                    }

                    var escapePos = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    _escapePosDict.Add(fixedPoint.Id, escapePos);
                    escapePos.name = fixedPoint.Id;

                    Log.Debug("created a sphere " + fixedPoint.Id, CustomEscape.Singleton.Config.Debug);
                    escapePos.transform.localScale =
                        new Vector3(0.001f, 0.001f, 0.001f); // stop bumping into that shit
                    escapePos.transform.localPosition = fixedPoint.Position;
                    Log.Debug(
                        $"modified the sphere '{fixedPoint.Id}': {escapePos.transform.localScale}, {escapePos.transform.localPosition}",
                        CustomEscape.Singleton.Config.Debug);

                    var collider = escapePos.GetComponent<SphereCollider>();
                    Log.Debug($"got a collider of '{fixedPoint.Id}': '{collider}'",
                        CustomEscape.Singleton.Config.Debug);
                    collider.isTrigger = true;
                    collider.radius = escapePoint.Value.EscapeRadius;
                    Log.Debug($"modified the collider: {collider.center}, {collider.radius}, {collider.isTrigger}",
                        CustomEscape.Singleton.Config.Debug);

                    escapePos.AddComponent<CustomEscapeComponent>();
                    Log.Debug($"attached an escape component to '{fixedPoint.Id}'",
                        CustomEscape.Singleton.Config.Debug);
                }
            });
        }

        public static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            foreach (var kvp in _escapePosDict)
            {
                Object.Destroy(kvp.Value);
                Log.Debug($"destroyed the '{kvp.Key}'", CustomEscape.Singleton.Config.Debug);
            }

            _escapePosDict.Clear();
        }

        public static void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsEscaped) return;
            if (!ev.Player.SessionVariables.TryGetValue(SessionVariable, out var objValue) ||
                !(objValue is string sValue) ||
                !CustomEscape.Singleton.Config.EscapePoints.TryGetValue(sValue, out var epc))
            {
                Log.Debug("the escape is not performed by a custom collider or we don't have a role to change to");
                ev.NewRole = RoleType.None;
                return;
            }

            Log.Debug($"got session variable and escape point config: '{sValue}'");

            if (!epc.RoleConversions.TryGetValue(ev.Player.Role,
                out var pcc))
            {
                ev.NewRole = RoleType.None;
                return;
            }

            var role = ev.Player.IsCuffed ? pcc.CuffedRole : pcc.UnCuffedRole;
            Log.Debug($"changing role: '{ev.Player.Role}' to '{role}', cuffed: '{ev.Player.IsCuffed}'",
                CustomEscape.Singleton.Config.Debug);
            ev.NewRole = role;

            // Because SetRole() is called with Player's current role, the items thing is not handled properly and the inventory is changed here, so exiled can change the inventory itself
            ev.Items.Clear();
            ev.Items.AddRange(ev.Player.ReferenceHub.characterClassManager.Classes.SafeGet(ev.NewRole).startItems);
        }

        public static void OnEscaping(EscapingEventArgs ev)
        {
            Log.Debug($"RoleType is '{ev.NewRole}'", CustomEscape.Singleton.Config.Debug);

            ev.Player.SessionVariables[SessionVariable] = null;
            Log.Debug($"set '{SessionVariable}' back to 'null'");

            if (!ev.IsAllowed) return;
            /*
             * Those checks are here and not in OnChangingRole() because
             * 1. I need the IsAllowed property which is not present in ChangingRole
             * 2. Other plugins can override the NewRole and it will affect the logic
             */
            switch (ev.NewRole)
            {
                case RoleType.None:
                    ev.IsAllowed = false;
                    Log.Debug("so we're not allowing the escape", CustomEscape.Singleton.Config.Debug);
                    return;
                case RoleType.Spectator:
                    Timing.CallDelayed(0.1f,
                        () => ev.Player.Position = Exiled.API.Extensions.Role.GetRandomSpawnPoint(ev.Player.Role));
                    Log.Debug($"so we're moving spectator out of the way: {ev.Player.Nickname}",
                        CustomEscape.Singleton.Config.Debug);
                    break;
            }

            ev.Player.ShowHint(
                CustomEscape.Singleton.Config.EscapeHint
                    .Replace("[oldrole]", ev.Player.Role.ToString())
                    .Replace("[newrole]", ev.NewRole.ToString()), 
                CustomEscape.Singleton.Config.EscapeHintDuration);

            if (ev.Player.Team == Team.CDP)
            {
                if (ev.Player.IsCuffed)
                {
                    RoundSummary.escaped_scientists++;
                    RespawnTickets.Singleton.GrantTickets(SpawnableTeamType.NineTailedFox,
                        ConfigFile.ServerConfig.GetInt("respawn_tickets_mtf_classd_cuffed_count", 1));
                    Log.Debug("so we're adding tickets to NTF", CustomEscape.Singleton.Config.Debug);
                }
                else
                {
                    RoundSummary.escaped_ds++;
                    RespawnTickets.Singleton.GrantTickets(SpawnableTeamType.ChaosInsurgency,
                        ConfigFile.ServerConfig.GetInt("respawn_tickets_ci_classd_count", 1));
                    Log.Debug("so we're adding tickets to CI", CustomEscape.Singleton.Config.Debug);
                }
            }
            else if (ev.Player.Team == Team.RSC)
            {
                if (ev.Player.IsCuffed)
                {
                    RoundSummary.escaped_ds++;
                    RespawnTickets.Singleton.GrantTickets(SpawnableTeamType.ChaosInsurgency,
                        ConfigFile.ServerConfig.GetInt("respawn_tickets_ci_scientist_cuffed_count", 2));
                    Log.Debug("so we're adding tickets to CI", CustomEscape.Singleton.Config.Debug);
                }
                else
                {
                    RoundSummary.escaped_scientists++;
                    RespawnTickets.Singleton.GrantTickets(SpawnableTeamType.NineTailedFox,
                        ConfigFile.ServerConfig.GetInt("respawn_tickets_mtf_scientist_count", 1));
                    Log.Debug("so we're adding tickets to NTF", CustomEscape.Singleton.Config.Debug);
                }
            }
        }
    }
}