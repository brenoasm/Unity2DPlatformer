using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;

    public void StartGame()
    {
        gameState.RaiseEvent(GameState.START_GAME);
    }
}
