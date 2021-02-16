using Exiled.API.Features;
using System;
using UnityEngine;

namespace BetterEscape
{
    public class BetterEscapeComponent : MonoBehaviour
    {
        public Player ply { get; private set; }
        private static Vector3 playerPos = Vector3.zero;

        public void Awake() => ply = Player.Get(gameObject);
        public void FixedUpdate()
        {
            if (ply.Position == GetWorldPosition(ply.GameObject) || Vector3.Distance(ply.Position, GetWorldPosition(ply.GameObject)) <= 2 && ply.IsCuffed)
                SwitchSide(ply);
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

        public Vector3 GetWorldPosition(GameObject obj)
        {
            if (playerPos == Vector3.zero)
                playerPos = obj.GetComponent<Escape>().worldPosition;
            return playerPos;
        }       

        public void SwitchSide(Player plr)
        {
            switch (plr.Role)
            {
                case RoleType.Scientist:
                    plr.Role = BetterEscape.singleton.Config.fromScientistTo;
                    break;
                case RoleType.ChaosInsurgency:
                    plr.Role = BetterEscape.singleton.Config.fromChaosTo;
                    break;
                case RoleType.NtfCommander:
                    plr.Role = BetterEscape.singleton.Config.fromCommanderTo;
                    break;
                case RoleType.NtfLieutenant:
                    plr.Role = BetterEscape.singleton.Config.fromLieutenantTo;
                    break;
                case RoleType.NtfCadet:
                    plr.Role = BetterEscape.singleton.Config.fromCadetTo;
                    break;
                case RoleType.FacilityGuard:
                    plr.Role = BetterEscape.singleton.Config.fromGuardTo;
                    break;
                case RoleType.NtfScientist:
                    plr.Role = BetterEscape.singleton.Config.fromNtfScientistTo;
                    break;
                default:
                    plr.Role = BetterEscape.singleton.Config.fromClassdTo;
                    break;
            }
        }
    }
}
