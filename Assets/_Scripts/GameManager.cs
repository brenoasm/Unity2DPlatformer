using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameStateSO gameState;

    private static GameManager _instance;

    public static GameManager Instance { get => _instance; }

    private void Awake()
    { 
        if (_instance != null && Instance != this)
        {
            Destroy(this);
        } else
        {
            _instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
    }

    private void Start()
    {
        gameState.OnEvent += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        gameState.OnEvent -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.START_GAME:
                StartGame();
                break;
            case GameState.RESTART_LEVEL:
                RestartLevel();
                break;
            case GameState.COMPLETE_LEVEL:
                CompleteLevel();
                break;
            case GameState.RESTART_GAME:
                RestartGame();
                break;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
}
