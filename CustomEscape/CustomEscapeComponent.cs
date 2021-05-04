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
            Player ply = Player.Get(col.gameObject);
            if (ply == null)
            {
                Log.Debug("it wasn't a player");
                return;
            }

            Log.Debug($"setting role: {ply.Nickname}, {ply.Role}, IsCuffed:{ply.IsCuffed}", Debug);

            Timing.CallDelayed(0.01f, () => ply.SetRole(ply.Role, false, true));
        }
    }
}
