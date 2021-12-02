# CustomEscape

Plugin to ultimately manage where will the players escape and what roles will they get.

Requires [EXILED](https://github.com/Exiled-Team/EXILED) and [0Points](https://github.com/4310V343k/Points)
installed.

You can download both the plugin and 0Points.dll [here](https://github.com/4310V343k/CustomEscape/releases).

## Configs

Configs are actually very easy to do. You can find all the `RoleType`s under the example config.

### Points

To add a new escape point you need to
1) Turn `edit_mode` on in Points' config. Warning: everyone with access to RA will be able to execute commands
2) Load escape points file by using `pnt load *points_file_name*`, e.g. `pnt load EscapePoints`
3) Add new escape points by using `pnt add *id*`, e.g. `pnt add escape0`

    You can use `pnt mode` to change how points will be saved
4) `pnt save` to save added points to a file
5) Add new `escape_points` config entry with name you used in 3rd step, e.g. `escape0`

### Example config
Base config doesn't differ from base-game

```yaml
bEscape:
   escape_points:
      ## As defined in *points_file_name*.txt
      escape0:
         # How big will the escape zone be
         escape_radius: 1000
         # Self-explanatory. Wrong configs will lead to role changing to Scp173. You can pass None to not change role. Make sure you follow the example formatting or it *can* possibly break
         ## None to not change role. Spectator to kill a person
         role_conversions:
            ClassD:
               cuffed_role: NtfPrivate
               cuffed_clear_inventory: false
               un_cuffed_role: ChaosConscript
               un_cuffed_clear_inventory: false
            Scientist:
               cuffed_role: ChaosConscript
               cuffed_clear_inventory: false
               un_cuffed_role: NtfSpecialist
               un_cuffed_clear_inventory: false
   # Points file. It contains all escape positions in the form of IDs and raw XYZ data
   points_file_name: EscapePoints
   debug: false
   # Enables Custom Escape
   is_enabled: true
```

### `RoleType`s

```yaml
## SCP
Scp049
Scp0492
Scp096
Scp079
Scp106
Scp173
Scp93953
Scp93989
## FoundationStaff
Scientist
FacilityGuard
NtfPrivate
NtfSergeant
NtfSpecialist
NtfCaptain
## FoundationEnemy
ClassD
ChaosConscript
ChaosRifleman
ChaosRepressor
ChaosMarauder
## Others
None
Spectator
Tutorial
```
