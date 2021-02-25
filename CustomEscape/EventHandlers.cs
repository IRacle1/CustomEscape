using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Collections.Generic;

namespace CustomEscape
{
    public class EventHandlers
    {
        public void OnVerified(VerifiedEventArgs ev)
        {
            ev.Player.GameObject.AddComponent<CustomEscapeComponent>();
            Log.Debug($"attached: {ev.Player.Nickname}", CustomEscape.singleton.Config.Debug);
        }
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {

            foreach (Player pl in Player.List)
            {
                if (pl.GameObject.TryGetComponent(out CustomEscapeComponent betterEscape))
                {
                    betterEscape.Destroy();
                    Log.Debug($"destroyed: {pl.Nickname}", CustomEscape.singleton.Config.Debug);
                }
            }
        }
        public void OnDestroying(DestroyingEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent(out CustomEscapeComponent betterEscape))
            {
                betterEscape.Destroy();
                Log.Debug($"destroyed: {ev.Player.Nickname}", CustomEscape.singleton.Config.Debug);
            }
        }
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.IsEscaped) return;

            foreach (KeyValuePair<RoleType, PrettyCuffedConfig> kvp in CustomEscape.singleton.Config.RoleConversions)
            {
                if (kvp.Key == ev.Player.Role)
                {
                    RoleType role = ev.Player.IsCuffed ? kvp.Value.CuffedRole : kvp.Value.UncuffedRole;
                    Log.Debug($"changingrole: {ev.Player.Role} to {role}, cuffed: {ev.Player.IsCuffed}", CustomEscape.singleton.Config.Debug);
                    ev.NewRole = role;
                }
            }

            // Because SetRole() is called with Player's current role, the items thing is not handled properly and the inventory is changed here, so exiled can change the inventory itself
            ev.Items.Clear();
            ev.Items.AddRange(ev.Player.ReferenceHub.characterClassManager.Classes.SafeGet(ev.NewRole).startItems);
        }
        public void OnEscaping(EscapingEventArgs ev)
        {
            if (!ev.IsAllowed) return;
            /*
			 * Those checks are here and not in OnChangingRole() because
			 * 1. I need the IsAllowed property which is not present in ChangingRole
			 * 2. Other plugins can override the NewRole and it will affect the logic
			 */
            if (ev.NewRole == RoleType.None)
            {
                ev.IsAllowed = false;
                Log.Debug($"but not allowed", CustomEscape.singleton.Config.Debug);
            }
            if (ev.NewRole == RoleType.Spectator)
            {
                Timing.CallDelayed(0.1f, () => ev.Player.Position = Map.GetRandomSpawnPoint(ev.Player.Role));
                Log.Debug($"moved spectator out of the way: {ev.Player.Nickname}", CustomEscape.singleton.Config.Debug);
            }
        }
    }
}
