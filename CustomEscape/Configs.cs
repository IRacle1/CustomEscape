using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using ArithFeather.Points.DataTypes;
using ArithFeather.Points.Tools;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using UnityEngine;

namespace CustomEscape
{
    public class Configs : IConfig
    {
        public Dictionary<string, EscapePointConfig> EscapePoints { get; set; } =
            new Dictionary<string, EscapePointConfig>
            {
                {
                    "Escape0", new EscapePointConfig
                    {
                        EscapeRadius = 100f
                    }
                }
            };

        [Description(
            "Self-explanatory. Wrong configs will lead to role changing to Scp173. You can pass None to not change role. Make sure you follow the example formatting or it *can* possibly break")]
        public Dictionary<RoleType, PrettyCuffedConfig> RoleConversions { get; set; } =
            new Dictionary<RoleType, PrettyCuffedConfig>
            {
                {
                    RoleType.FacilityGuard,
                    new PrettyCuffedConfig
                        {CuffedRole = RoleType.ChaosInsurgency, UnCuffedRole = RoleType.NtfLieutenant}
                },
                {
                    RoleType.Scientist,
                    new PrettyCuffedConfig {CuffedRole = RoleType.ChaosInsurgency, UnCuffedRole = RoleType.NtfScientist}
                }
            };

        [Description("Hint that will be shown on escape. Possible literals: [newrole], [oldrole] ")]
        public string EscapeHint { get; set; } = "You escaped the complex as [oldrole]!";

        [Description("The time the hint will be shown")]
        public float EscapeHintDuration { get; set; } = 3f;
        
        


        [Description("Points file. It contains all escape positions in the form of IDs and raw XYZ data")]
        public string PointsFileName { get; set; } = "EscapePoints";

        public bool Debug { get; set; } = false;
        [Description("Enables Custom Escape")] public bool IsEnabled { get; set; } = true;

        public void TryCreateFile()
        {
            if (FileManager.FileExists(Path.Combine(PointIO.FolderPath, PointsFileName) + ".txt"))
                return;
            Log.Info("Creating new EscapePoint file using default spawn points.");

            PointIO.Save(new PointList(new List<RawPoint>
            {
                new RawPoint("Escape0", RoomType.Surface, new Vector3(170f, 985f, 26f), new Vector3(0f, 0f, 0f))
            }), PointsFileName);
        }
    }

    public class EscapePointConfig
    {
        [Description("How big will the escape zone be")]
        public float EscapeRadius { get; set; } = 100f;
    }

    public class
        PrettyCuffedConfig // Because of this there will be "cuffed_role" and "uncuffed_role" config entries instead of just dictionaries
    {
        public RoleType CuffedRole { get; set; } = RoleType.ChaosInsurgency;
        public RoleType UnCuffedRole { get; set; } = RoleType.NtfCadet;
    }
}