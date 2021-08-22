namespace CustomEscape
{
    using Exiled.API.Features;
    using MEC;
    using UnityEngine;

    public class CustomEscapeComponent : MonoBehaviour
    {
        private bool Debug { get; } = CustomEscape.Singleton.Config.Debug;
        private string SessionVariable { get; } = EventHandlers.SessionVariable;

        public void OnTriggerEnter(Collider col)
        {
            Log.Debug("triggered", Debug);
            var ply = Player.Get(col.gameObject);
            if (ply == null)
            {
                Log.Debug("it wasn't a player");
                return;
            }

            Log.Debug($"setting role: '{ply.Nickname}', '{ply.Role}', IsCuffed:'{ply.IsCuffed}'", Debug);
            Log.Debug($"setting session variable '{EventHandlers.SessionVariable}': '{gameObject.name}'", Debug);

            ply.SessionVariables[SessionVariable] = gameObject.name;

            Timing.CallDelayed(0.01f, () => ply.ReferenceHub.characterClassManager.UserCode_CmdRegisterEscape());
        }
    }
}