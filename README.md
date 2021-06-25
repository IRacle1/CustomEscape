# CustomEscape

Plugin to ultimately manage where will the players escape and what roles will they get.

Requires [EXILED](https://github.com/Exiled-Team/EXILED) and [0Points](https://github.com/arithfeather-scp-sl/Points)
installed.

You can download both the plugin and 0Points.dll [here](https://github.com/4310V343k/CustomEscape/releases).

## Configs

Configs are actually very easy to do. You can find all the `RoleType`s under the example config.

### Points

To add a new escape point you need to
1) Turn `edit_mode` on in Points' config
2) Load escape points fileby using `.pnt load *points_file_name*`, e.g. `.pnt load EscapePoints`
3) Add new escape points by using `.pnt add *id*`, e.g. `.pnt add escape0`

    You can use `.pnt player` and `.pnt crosshair` to change how points will be saved
4) `.pnt save` to save added points in a file
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
          cuffed_role: NtfCadet
          un_cuffed_role: ChaosInsurgency
        Scientist:
          cuffed_role: ChaosInsurgency
          un_cuffed_role: NtfScientist
  # Points file. It contains all escape positions in the form of IDs and raw XYZ data
  ## Change this if you want to have different escape points on multiple servers
  points_file_name: EscapePoints
  debug: true
  # Enables Custom Escape
  is_enabled: true
```

### `RoleType`s

```yaml
None
  ## SCPs
Scp173
Scp106
Scp049
Scp079
Scp096
Scp0492
Scp93953
Scp93989
  ## Humans
NtfScientist
ChaosInsurgency
NtfLieutenant
NtfCommander
NtfCadet
FacilityGuard
Scientist
ClassD
  ## Misc Roles. Yes, they can 'escape' too
Spectator
Tutorial
```
