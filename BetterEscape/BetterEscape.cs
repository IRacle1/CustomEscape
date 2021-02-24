using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace BetterEscape
{
    public class BetterEscape : Plugin<Configs>
    {
		public override string Author { get; } = "Remindme";
		public override string Name { get; } = "Better Escape";
		public override string Prefix { get; } = "bEscape";
		public override Version Version { get; } = new Version(2, 0, 0);
		public override Version RequiredExiledVersion { get; } = new Version(2, 1, 34);

		private EventHandlers EventHandlers { get; set; }

		public static BetterEscape singleton;

		public override void OnEnabled()
		{
			singleton = this;

			EventHandlers = new EventHandlers();

			PlayerEvents.Verified += EventHandlers.OnVerified;
			PlayerEvents.Left += EventHandlers.OnLeft;
			PlayerEvents.ChangingRole += EventHandlers.OnChangingRole;
			ServerEvents.EndingRound += EventHandlers.EndingRound;

			base.OnEnabled();
		}

		public override void OnDisabled()
		{
			PlayerEvents.Verified -= EventHandlers.OnVerified;
			PlayerEvents.Left -= EventHandlers.OnLeft;
			PlayerEvents.ChangingRole -= EventHandlers.OnChangingRole;
			ServerEvents.EndingRound -= EventHandlers.EndingRound;

			EventHandlers = null;

			singleton = null;

			base.OnDisabled();
		}
	}
	public class EventHandlers
    {
		private CoroutineHandle coroutine = new CoroutineHandle();
		public void OnVerified(VerifiedEventArgs ev)
		{
			ev.Player.GameObject.AddComponent<BetterEscapeComponent>();
			Log.Debug($"attached: {ev.Player.Nickname}", BetterEscape.singleton.Config.Debug);
		}
		public void EndingRound(EndingRoundEventArgs ev)
		{
			foreach (Player pl in Player.List)
			{
				if (pl.GameObject.TryGetComponent(out BetterEscapeComponent betterEscape))
				{
					betterEscape.Destroy();
					Log.Debug($"destroyed: {pl.Nickname}", BetterEscape.singleton.Config.Debug);
				}
			}
			Timing.KillCoroutines(coroutine);
		}
		public void OnLeft(LeftEventArgs ev)
		{
			if (ev.Player.GameObject.TryGetComponent(out BetterEscapeComponent betterEscape))
			{
				betterEscape.Destroy();
				Log.Debug($"destroyed: {ev.Player.Nickname}", BetterEscape.singleton.Config.Debug);
			}
		}
		public void OnChangingRole(ChangingRoleEventArgs ev)
		{
			if (!ev.IsEscaped) return;
			
			foreach (KeyValuePair<RoleType, PrettyCuffedConfig> kvp in BetterEscape.singleton.Config.RoleConversions)
			{
				if (kvp.Key == ev.Player.Role)
				{
					RoleType role = ev.Player.IsCuffed ? kvp.Value.CuffedRole : kvp.Value.UncuffedRole;
					Log.Debug($"changingrole: {ev.Player.Role} to {role}, cuffed: {ev.Player.IsCuffed}", BetterEscape.singleton.Config.Debug);
					if (role != RoleType.None)
					{
						ev.NewRole = role;
					}
				}
			}
		}
	} 
}
