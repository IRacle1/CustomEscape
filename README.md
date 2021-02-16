# BetterEscape
You can manage a **Player**'s **Role** when he **Escapes** from the **Facility**.

## Configs
Too lazy to make a tab. Enjoy.

        [Description("Enables BetterEscape")]
        public bool IsEnabled { get; set; } = true;

        [Description("Syntax: From Old Role to New Role")]
        public RoleType fromScientistTo { get; set; } = RoleType.ChaosInsurgency;
        public RoleType fromChaosTo { get; set; } = RoleType.NtfLieutenant;
        public RoleType fromCommanderTo { get; set; } = RoleType.ChaosInsurgency;
        public RoleType fromLieutenantTo { get; set; } = RoleType.ChaosInsurgency;
        public RoleType fromCadetTo { get; set; } = RoleType.ChaosInsurgency;
        public RoleType fromGuardTo { get; set; } = RoleType.ChaosInsurgency;
        public RoleType fromNtfScientistTo { get; set; } = RoleType.ChaosInsurgency;
        public RoleType fromClassdTo { get; set; } = RoleType.NtfLieutenant;
