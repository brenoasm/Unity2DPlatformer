using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    private AudioSource finishSoundEffect;

    private bool alreadyTouchedFinish = false;

    private void Start()
    {
        finishSoundEffect = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !alreadyTouchedFinish)
        {
            finishSoundEffect.Play();

            alreadyTouchedFinish = true;

            Invoke("CompleteLevel", finishSoundEffect.clip.length);
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
