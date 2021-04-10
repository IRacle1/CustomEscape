using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace CustomEscape
{
    public class EventHandlers
    {
        public static GameObject EscapePos;
        public void OnGenerated()
        {
            EscapePos = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Log.Debug("created a sphere", CustomEscape.Singleton.Config.Debug);
            EscapePos.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f); // stop bumping into that shit. not 0 because unity
            EscapePos.transform.localPosition = new Vector3(170f, 985f, 26f);
            Log.Debug($"modified the sphere: {EscapePos.transform.localScale}, {EscapePos.transform.localPosition}", CustomEscape.Singleton.Config.Debug);
            Component[] comps = EscapePos.GetComponents(typeof(Component));

            SphereCollider collider = EscapePos.GetComponent<SphereCollider>();
            Log.Debug($"got a collider: {collider}", CustomEscape.Singleton.Config.Debug);
            collider.isTrigger = true;
            collider.radius = 10f;
            Log.Debug($"modified the collider: {collider.center}, {collider.radius}, {collider.isTrigger}", CustomEscape.Singleton.Config.Debug);

            EscapePos.AddComponent<CustomEscapeComponent>();
            Log.Debug("attached an escape component", CustomEscape.Singleton.Config.Debug);
        }
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Object.Destroy(EscapePos.gameObject); // lets hope unity handles the components for us
            Log.Debug("destroyed the sphere", CustomEscape.Singleton.Config.Debug);
        }
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsEscaped) return;

            if (!CustomEscape.Singleton.Config.RoleConversions.TryGetValue(ev.Player.Role, out PrettyCuffedConfig value))
            {
                ev.NewRole = RoleType.None;
                return;
            }

            RoleType role = ev.Player.IsCuffed ? value.CuffedRole : value.UncuffedRole;
            Log.Debug($"changingrole: {ev.Player.Role} to {role}, cuffed: {ev.Player.IsCuffed}", CustomEscape.Singleton.Config.Debug);
            ev.NewRole = role;

            // Because SetRole() is called with Player's current role, the items thing is not handled properly and the inventory is changed here, so exiled can change the inventory itself
            ev.Items.Clear();
            ev.Items.AddRange(ev.Player.ReferenceHub.characterClassManager.Classes.SafeGet(ev.NewRole).startItems);
        }
        public void OnEscaping(EscapingEventArgs ev)
        {
            Log.Debug($"RoleType is {ev.NewRole}", CustomEscape.Singleton.Config.Debug);
            if (!ev.IsAllowed) return;
            /*
			 * Those checks are here and not in OnChangingRole() because
			 * 1. I need the IsAllowed property which is not present in ChangingRole
			 * 2. Other plugins can override the NewRole and it will affect the logic
			 */
            if (ev.NewRole == RoleType.None)
            {
                ev.IsAllowed = false;
                Log.Debug($"so we're not allowing the escape", CustomEscape.Singleton.Config.Debug);
            }
            if (ev.NewRole == RoleType.Spectator)
            {
                Timing.CallDelayed(0.1f, () => ev.Player.Position = ev.Player.Role.GetRandomSpawnPoint());
                Log.Debug($"so we're moving spectator out of the way: {ev.Player.Nickname}", CustomEscape.Singleton.Config.Debug);
            }
        }
    }
}
