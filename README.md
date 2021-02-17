# BetterEscape
You can manage a **Player**'s **Role** when he **Escapes** from the **Facility**.

## Configs
Too lazy to make a tab. Enjoy.

        [Description("Enables BetterEscape")]
        public bool IsEnabled { get; set; } = true;

        [Description("Syntax: From Old Role to New Role")]
        public Dictionary<RoleType, RoleType> RoleConversions = new Dictionary<RoleType, RoleType>()
        {
            { RoleType.Scientist, RoleType.ChaosInsurgency },
            { RoleType.ChaosInsurgency, RoleType.NtfLieutenant },
            { RoleType.NtfCommander, RoleType.ChaosInsurgency },
            { RoleType.NtfLieutenant, RoleType.ChaosInsurgency },
            { RoleType.NtfCadet, RoleType.ChaosInsurgency },
            { RoleType.FacilityGuard, RoleType.ChaosInsurgency },
            { RoleType.NtfScientist, RoleType.ChaosInsurgency },
            { RoleType.ClassD, RoleType.NtfLieutenant }
        };
