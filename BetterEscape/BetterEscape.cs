using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using System.Collections.Generic;

namespace BetterEscape
{
    public class BetterEscape : Plugin<Configs>
    {
		public override string Author { get; } = "Tomorii";
		public override string Name { get; } = "BetterEscape";
		public override string Prefix { get; } = "BetterEscape";
		public override Version Version { get; } = new Version(1, 1, 0);
		public override Version RequiredExiledVersion { get; } = new Version(2, 1, 34);

		public EventHandlers EventHandler { get; set; }

		public static BetterEscape singleton;

		public override void OnEnabled()
		{
			singleton = this;
			RegisterEvents();
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
			EventHandler = new EventHandlers();

			PlayerEvents.Verified += EventHandler.OnVerified;
			ServerEvents.EndingRound += EventHandler.EndingRound;
		}

		private void UnregisterEvents()
		{
			PlayerEvents.Verified -= EventHandler.OnVerified;
			ServerEvents.EndingRound -= EventHandler.EndingRound;

			EventHandler = null;
		}
	}

	public class EventHandlers
    {
		public static Dictionary<RoleType, RoleType> RoleConversions = new Dictionary<RoleType, RoleType>()
		{
			{ RoleType.Scientist, BetterEscape.singleton.Config.ScientistTo },
			{ RoleType.ChaosInsurgency, BetterEscape.singleton.Config.ChaosInsurgencyTo },
			{ RoleType.NtfCommander, BetterEscape.singleton.Config.NtfCommanderTo },
			{ RoleType.NtfLieutenant, BetterEscape.singleton.Config.NtfLieutenantTo },
			{ RoleType.NtfCadet, BetterEscape.singleton.Config.NtfCadetTo },
			{ RoleType.FacilityGuard, BetterEscape.singleton.Config.FacilityGuardTo },
			{ RoleType.NtfScientist, BetterEscape.singleton.Config.NtfScientistTo },
			{ RoleType.ClassD, BetterEscape.singleton.Config.ClassDTo }
		};

		public void OnVerified(VerifiedEventArgs ev)
        {
			ev.Player.GameObject.AddComponent<BetterEscapeComponent>();
			Log.Debug("BetterComponent");
		}

		public void EndingRound(EndingRoundEventArgs ev)
        {
			foreach (Player pl in Player.List)
				if (pl.GameObject.TryGetComponent(out BetterEscapeComponent betterEscape))
					betterEscape.Destroy();
        }
    } 
}
