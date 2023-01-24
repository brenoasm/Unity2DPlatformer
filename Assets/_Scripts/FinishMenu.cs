using UnityEngine;

public class FinishMenu : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;

    public void RestartGame()
    {
        gameState.RaiseEvent(GameState.RESTART_GAME);
    }
}
