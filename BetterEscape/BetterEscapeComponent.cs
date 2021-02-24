using Exiled.API.Features;
using MEC;
using System;
using UnityEngine;

namespace BetterEscape
{

    public class BetterEscapeComponent : MonoBehaviour
    {
        private Player Ply { get; set; }

        private Vector3 escapePos = new Vector3(170, 984, 26);

        private bool Debug { get; set; } = BetterEscape.singleton.Config.Debug;

        public void Awake()
        {
            Ply = Player.Get(gameObject);
        }

        public void Update()
        {
            if (Ply.Role == RoleType.ClassD || Ply.Role == RoleType.Scientist) return;
            if (Vector3.Distance(Ply.Position, escapePos) > 2) return;

            Log.Debug($"update:{Ply.Role}, IsCuffed:{Ply.IsCuffed}", Debug);

            Timing.CallDelayed(0.01f, () => Ply.SetRole(Ply.Role, false, true));

            this.Destroy();
            Timing.CallDelayed(1f, () =>
            {
                Ply.GameObject.AddComponent<BetterEscapeComponent>();
                Log.Debug($"reattached:{Ply.Nickname}", Debug);
            });
        }
        public void Destroy()
        {
            try
            {
                Destroy(this);
            }
            catch (Exception e)
            {
                Log.Error($"Can't Destroy: {e}");
            }
        }
    }

}
