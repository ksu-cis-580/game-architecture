# Placeholder audio

These sounds are procedurally generated placeholders. Replace them with your own game's audio,
and update the provenance and license entries in the top-level `ASSETS.md`.

| File | Use |
|---|---|
| `Sfx/ui_move.wav` | Menu selection moved |
| `Sfx/ui_confirm.wav` | Menu item confirmed |
| `Sfx/ui_back.wav` | Menu cancelled / back |
| `Sfx/player_join.wav` | A player joined the game |
| `Sfx/player_leave.wav` | A player left the game |
| `Music/menu_loop.wav` | Menu music (seamless loop, 90 BPM) |
| `Music/game_loop.wav` | Gameplay music (seamless loop, 124 BPM) |

Music is stored as `.wav` and built as `SoundEffect` content (see `Builder.cs`), which lets the
music player loop and cross-fade tracks. Content is loaded by its path relative to `Assets/`,
without the extension, e.g. `Content.Load<SoundEffect>("Audio/Sfx/ui_move")`.
