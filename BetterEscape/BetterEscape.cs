using Exiled.API.Features;
using System;
using ServerEvents = Exiled.Events.Handlers.Server;

namespace BetterEscape
{
    public class BetterEscape : Plugin<Configs>
    {
		public override string Author { get; } = "Tomorii";
		public override string Name { get; } = "BetterEscape";
		public override string Prefix { get; } = "BetterEscape";
		public override Version Version { get; } = new Version(1, 0, 0);
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

			ServerEvents.RoundStarted -= EventHandler.OnStart;
		}

		private void UnregisterEvents()
		{
			ServerEvents.RoundStarted -= EventHandler.OnStart;

			EventHandler = null;
		}
	}

	public class EventHandlers
    {
		public void OnStart()
        {
			foreach (Player pl in Player.List)
				pl.GameObject.AddComponent<BetterEscapeComponent>();
        }

		public void EndingRound()
        {
			foreach (Player pl in Player.List)
				if (pl.GameObject.TryGetComponent(out BetterEscapeComponent betterEscape))
					betterEscape.Destroy();
        }
    } 
}
