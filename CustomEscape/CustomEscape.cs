﻿using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using System.Collections.Generic;
using UnityEngine;
using MEC;

namespace CustomEscape
{
    public class CustomEscape : Plugin<Configs>
    {
		public override string Author { get; } = "Remindme";
		public override string Name { get; } = "Custom Escapes";
		public override string Prefix { get; } = "bEscape";
		public override Version Version { get; } = new Version(2, 3, 0);
		public override Version RequiredExiledVersion { get; } = new Version(2, 1, 34);

		private EventHandlers EventHandlers { get; set; }

		public static CustomEscape singleton;

		public override void OnEnabled()
		{
			singleton = this;

			EventHandlers = new EventHandlers();

			PlayerEvents.Verified += EventHandlers.OnVerified;
			PlayerEvents.Left += EventHandlers.OnLeft;
			PlayerEvents.Escaping += EventHandlers.OnEscaping;
			ServerEvents.RoundEnded += EventHandlers.OnRoundEnded;

			base.OnEnabled();
		}

		public override void OnDisabled()
		{
			PlayerEvents.Verified -= EventHandlers.OnVerified;
			PlayerEvents.Left -= EventHandlers.OnLeft;
			PlayerEvents.Escaping -= EventHandlers.OnEscaping;
			ServerEvents.RoundEnded -= EventHandlers.OnRoundEnded;

			EventHandlers = null;

			singleton = null;

			base.OnDisabled();
		}
	}
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
		public void OnLeft(LeftEventArgs ev)
		{
			if (ev.Player.GameObject.TryGetComponent(out CustomEscapeComponent betterEscape))
			{
				betterEscape.Destroy();
				Log.Debug($"destroyed: {ev.Player.Nickname}", CustomEscape.singleton.Config.Debug);
			}
		}
		public void OnEscaping(EscapingEventArgs ev)
		{
			if (!ev.IsAllowed) return;
			
			foreach (KeyValuePair<RoleType, PrettyCuffedConfig> kvp in CustomEscape.singleton.Config.RoleConversions)
			{
				if (kvp.Key == ev.Player.Role)
				{
					RoleType role = ev.Player.IsCuffed ? kvp.Value.CuffedRole : kvp.Value.UncuffedRole;
					Log.Debug($"escaping: {ev.Player.Role} to {role}, cuffed: {ev.Player.IsCuffed}", CustomEscape.singleton.Config.Debug);
					ev.NewRole = role;
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
			ev.Player.ReferenceHub.characterClassManager.KeepItemsAfterEscaping = true; // This is needed to properly give players items respectively to their NEW role instead of the OLD one. don't ask me, ask northwood
		}
	} 
}