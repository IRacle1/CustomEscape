using Exiled.API.Features;
using MEC;
using UnityEngine;

namespace CustomEscape
{
    public class CustomEscapeComponent : MonoBehaviour
    {
        private bool Debug { get; set; } = CustomEscape.Singleton.Config.Debug;

        public void OnTriggerEnter(Collider col)
        {
            Log.Debug("triggered", Debug);
            Player Ply = Player.Get(col.gameObject);
            if (Ply == null)
            {
                Log.Debug("it wasn't a player");
                return;
            }

            if (Ply.Role == RoleType.ClassD || Ply.Role == RoleType.Scientist) return;

            Log.Debug($"setting role: {Ply.Nickname}, {Ply.Role}, IsCuffed:{Ply.IsCuffed}", Debug);

            Timing.CallDelayed(0.01f, () => Ply.SetRole(Ply.Role, false, true));
        }
    }
}
