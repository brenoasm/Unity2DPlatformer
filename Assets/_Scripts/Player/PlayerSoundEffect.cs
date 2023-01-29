using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource dyingSoundEffect;
    [SerializeField] private AudioSource collectSoundEffect;

    [HideInInspector] public float currentClipLength = 0f;

    public void PlayCollectEffect()
    {
        Play(collectSoundEffect);
    }

    public void PlayJumpingEffect()
    {
        Play(jumpSoundEffect);
    }

    public void PlayDyingEffect()
    {
        Play(dyingSoundEffect);
    }

    private void Play(AudioSource source)
    {
        source.Play();

        currentClipLength = source.clip.length;

        Invoke(nameof(ResetLength), currentClipLength);
    }

    private void ResetLength()
    {
        currentClipLength = 0f;
    }
}
