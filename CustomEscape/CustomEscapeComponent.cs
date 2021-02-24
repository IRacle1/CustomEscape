using Exiled.API.Features;
using MEC;
using System;
using UnityEngine;

namespace CustomEscape
{

    public class CustomEscapeComponent : MonoBehaviour
    {
        private Player Ply { get; set; }

        private Vector3 EscapePos { get; set; } = new Vector3(170, 984, 26);

        private bool Debug { get; set; } = CustomEscape.singleton.Config.Debug;

        public void Awake()
        {
            Ply = Player.Get(gameObject);
        }

        public void Update()
        {
            if (Ply.Role == RoleType.ClassD || Ply.Role == RoleType.Scientist) return;
            if (Vector3.Distance(Ply.Position, EscapePos) > 2) return;

            Log.Debug($"update:{Ply.Role}, IsCuffed:{Ply.IsCuffed}", Debug);

            Timing.CallDelayed(0.01f, () => Ply.SetRole(Ply.Role, false, true));

            Destroy();
            Timing.CallDelayed(1f, () =>
            {
                Ply.GameObject.AddComponent<CustomEscapeComponent>();
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
