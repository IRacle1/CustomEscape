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
		public override string Author { get; } = "Tomorii";
		public override string Name { get; } = "BetterEscape";
		public override string Prefix { get; } = "BetterEscape";
		public override Version Version { get; } = new Version(1, 1, 3);
		public override Version RequiredExiledVersion { get; } = new Version(2, 1, 34);

		public EventHandlers EventHandlers { get; set; }

		public static BetterEscape singleton;

		public override void OnEnabled()
		{
			singleton = this;
			if (ParseConfig()) RegisterEvents();
			Log.Info("BetterEscape Loaded!");

			base.OnEnabled();
		}

		public override void OnDisabled()
		{
			UnregisterEvents();
			Log.Info("BetterEscape Disabled!");

			base.OnDisabled();
		}

        private void RegisterEvents()
		{
			EventHandlers = new EventHandlers();

			PlayerEvents.Verified += EventHandlers.OnVerified;
			PlayerEvents.Left += EventHandlers.OnLeft;
			ServerEvents.EndingRound += EventHandlers.EndingRound;
		}

		private void UnregisterEvents()
		{
			PlayerEvents.Verified -= EventHandlers.OnVerified;
			PlayerEvents.Left -= EventHandlers.OnLeft;
			ServerEvents.EndingRound -= EventHandlers.EndingRound;

			EventHandlers = null;
		}
		private bool ParseConfig()
		{
			Dictionary<RoleType, List<RoleType>> dict = Config.RoleConversions;
			foreach (KeyValuePair<RoleType, List<RoleType>> kvp in dict)
			{
				if (kvp.Value.Count != 2) return true;
			}
		}
	}

	public class EventHandlers
    {
		private CoroutineHandle coroutine = new CoroutineHandle();
		public void OnVerified(VerifiedEventArgs ev) => ev.Player.GameObject.AddComponent<BetterEscapeComponent>();

		public void EndingRound(EndingRoundEventArgs ev)
        {
			foreach (Player pl in Player.List)
				if (pl.GameObject.TryGetComponent(out BetterEscapeComponent betterEscape))
					betterEscape.Destroy();

			Timing.KillCoroutines(coroutine);
        }

		public void OnLeft(LeftEventArgs ev)
        {
			if (ev.Player.GameObject.TryGetComponent(out BetterEscapeComponent betterEscape))
				betterEscape.Destroy();
		}

		private static Vector3 escapePos = new Vector3(170, 984, 26);
	} 
}
