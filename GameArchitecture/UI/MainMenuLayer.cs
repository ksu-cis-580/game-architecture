using System;
using CIS580.Input;
using CIS580.UI;
using CIS580.UI.Widgets;

namespace GameArchitecture.UI;

/// <summary>
/// The main menu. It only presents choices: what happens when one is made is up to the
/// state that showed it.
/// </summary>
public class MainMenuLayer : MenuLayer
{
    private readonly Action _onExit;

    /// <param name="onPlay">Called when a player chooses to play, with who chose it</param>
    /// <param name="onLocalMultiplayer">
    /// Called when a player chooses local multiplayer; if null, the menu doesn't offer it
    /// (e.g. for a single player game)
    /// </param>
    /// <param name="onExit">Called when the player confirms they want to exit</param>
    public MainMenuLayer(Action<InputSource> onPlay, Action<InputSource> onLocalMultiplayer, Action onExit)
        : base("Main Menu")
    {
        _onExit = onExit;

        Menu.Add(new ButtonItem("Play Game", onPlay));
        if (onLocalMultiplayer != null)
            Menu.Add(new ButtonItem("Local Multiplayer", onLocalMultiplayer));
        Menu.Add(new ButtonItem("Options", _ => Open(new OptionsLayer())))
            .Add(new ButtonItem("Exit", OnCancel));
    }

    /// <summary>
    /// Cancelling the main menu asks the player to confirm they want to exit
    /// </summary>
    protected override void OnCancel(InputSource source)
    {
        var prompt = new PromptLayer("Are you sure you want to exit?");
        prompt.Accepted += _ => _onExit();
        Open(prompt);
    }
}
