using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "GameState")]
public class GameStateSO : ScriptableObject
{
    public GameState state = GameState.START_MENU;

    public delegate void HandleEvent(GameState state);

    public event HandleEvent OnEvent;

    public void RaiseEvent(GameState state)
    {
        this.state = state;

        OnEvent?.Invoke(state);
    }
}

public enum GameState
{
    START_MENU,
    START_GAME,
    RESTART_LEVEL,
    COMPLETE_LEVEL,
    RESTART_GAME,
}