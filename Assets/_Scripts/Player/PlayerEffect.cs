using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem runEffect;
    [SerializeField] private ParticleSystem landingEffect;

    [SerializeField] private PlayerStateSO playerState;

    private void FixedUpdate()
    {
        if (!playerState.wasOnTheGround && playerState.isOnTheGround)
        {
            landingEffect.Play();
        }
        else
        {
            landingEffect.Stop();
        }

        if (playerState.IsRunning && playerState.isOnTheGround)
        {
            runEffect.Play();
        } else
        {
            runEffect.Stop();
        }
    }
}
