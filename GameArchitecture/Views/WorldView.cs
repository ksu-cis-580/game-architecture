using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CIS580.Simulation;
using CIS580.Views;
using GameArchitecture.Simulation;

namespace GameArchitecture.Views;

/// <summary>
/// Draws the game world.
/// </summary>
/// <remarks>
/// This is purely a view: it reads the <see cref="GameSession"/> and draws it, but never changes
/// it. Positions in the session are in world units; the camera turns them into pixels, so the
/// world scales to fit the window. (These are placeholders: replace them with your game's drawing.)
/// </remarks>
public class WorldView : WorldLayer
{
    private static readonly TimeSpan SpawnFlashDuration = TimeSpan.FromSeconds(0.5);
    private static readonly RectangleF WorldArea = new(0, 0, World.Width, World.Height);

    private readonly GameSession _session;
    private readonly SpriteFont _font;

    // When each player last spawned, for drawing a flash as they appear
    private readonly Dictionary<PlayerId, TimeSpan> _spawnTimes = new();
    private TimeSpan _now;

    /// <param name="session">The game to draw</param>
    /// <param name="font">The font to draw with</param>
    public WorldView(GameSession session, SpriteFont font)
    {
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _font = font;
        Transition.OnDuration = TimeSpan.FromSeconds(1.5);
        Transition.OffDuration = TimeSpan.FromSeconds(0.5);
    }

    /// <summary>
    /// Reacts to something that happened in the simulation
    /// </summary>
    public void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent is PlayerJoined joined)
            _spawnTimes[joined.Player] = _now;
    }

    public override void Update(GameTime gameTime)
    {
        _now = gameTime.TotalGameTime;
        base.Update(gameTime);
    }

    protected override void UpdateCameras(GameTime gameTime)
    {
        // Show the whole world, as large as fits
        foreach (var camera in Cameras)
            camera.FitToArea(WorldArea);
    }

    protected override void DrawWorld(Camera2D camera, GameTime gameTime)
    {
        // This game has a blue world. Why? Because!
        SpriteBatch.Draw(Stack.BlankTexture, new Rectangle(0, 0, (int)World.Width, (int)World.Height), Color.CornflowerBlue);

        // Our players and wanderer are both actually just text strings
        var world = _session.World;
        foreach (var player in world.Players)
        {
            SpriteBatch.DrawString(_font, "// TODO", player.Position, PlayerColors.For(player.Owner));
            DrawSpawnFlash(player);
        }

        // The wanderer's text is centered horizontally on its position
        const string wandererText = "Insert Gameplay Here";
        var wandererOrigin = new Vector2(_font.MeasureString(wandererText).X / 2, 0);
        SpriteBatch.DrawString(_font, wandererText, world.Wanderer.Position, Color.DarkRed,
            0, wandererOrigin, 1, SpriteEffects.None, 0);
    }

    /// <summary>
    /// Draws an expanding, fading copy of a player that just spawned
    /// </summary>
    private void DrawSpawnFlash(PlayerAvatar player)
    {
        if (!_spawnTimes.TryGetValue(player.Owner, out var spawned))
            return;

        float t = (float)((_now - spawned) / SpawnFlashDuration);
        if (t >= 1f)
        {
            _spawnTimes.Remove(player.Owner);
            return;
        }

        const string text = "// TODO";
        var origin = _font.MeasureString(text) / 2;
        SpriteBatch.DrawString(_font, text, player.Position + origin, Color.White * (1f - t),
            0, origin, 1f + t * 2f, SpriteEffects.None, 0);
    }
}
