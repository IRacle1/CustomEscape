namespace CustomEscape
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using Patches;
    using Points;
    using Points.DataTypes;
    using UnityEngine;

    public static class EventHandlers
    {
        public const string SessionVariable = "plugin_escaping_collider_name";

        private static PointList _pointsPointList;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private static Dictionary<string, GameObject> _escapePosDict = new Dictionary<string, GameObject>();

        public static void OnLoadedSpawnPoints()
        {
            Log.Debug("OnLoadedSpawnPoints", CustomEscape.Singleton.Config.Debug);
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

        public static void OnEscaping(ExtendedEscapingEventArgs ev)
        {
            if (!ev.Player.SessionVariables.TryGetValue(SessionVariable, out var objValue) ||
                !(objValue is string sValue) ||
                !CustomEscape.Singleton.Config.EscapePoints.TryGetValue(sValue, out var epc))
            {
                Log.Debug("the escape is not performed by a custom collider or we don't have a role to change to",
                    CustomEscape.Singleton.Config.Debug);
                ev.IsAllowed = false;
                return;
            }

            Log.Debug($"got session variable and escape point config: '{sValue}'", CustomEscape.Singleton.Config.Debug);

            if (!epc.RoleConversions.TryGetValue(ev.Player.Role,
                out var pcc))
            {
                ev.IsAllowed = false;
                return;
            }

            ev.Player.SessionVariables[SessionVariable] = null;
            Log.Debug($"set '{SessionVariable}' back to 'null'", CustomEscape.Singleton.Config.Debug);

            var role = ev.Player.IsCuffed ? pcc.CuffedRole : pcc.UnCuffedRole;
            ev.ClearInventory = ev.Player.IsCuffed ? pcc.CuffedClearInventory : pcc.UnCuffedClearInventory;
            Log.Debug($"changing role: '{ev.Player.Role}' to '{role}', cuffed: '{ev.Player.IsCuffed}'",
                CustomEscape.Singleton.Config.Debug);
            ev.NewRole = role;

            switch (ev.NewRole)
            {
                case RoleType.None:
                    ev.IsAllowed = false;
                    Log.Debug("role is None, so we're not allowing the escape", CustomEscape.Singleton.Config.Debug);
                    break;
                case RoleType.Spectator:
                    Timing.CallDelayed(0.1f,
                        () => ev.Player.Position = ev.Player.Role.GetRandomSpawnProperties().Item1);
                    Log.Debug($"role is Spectator, so we're moving them out of the way: {ev.Player.Nickname}",
                        CustomEscape.Singleton.Config.Debug);
                    break;
            }
        }
    }
}