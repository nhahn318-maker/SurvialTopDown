using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private GameAudioSettings audioSettings;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerBasicAttack basicAttack;
    [SerializeField] private PlayerBombSkill bombSkill;
    [SerializeField] private PlayerDashSkill dashSkill;

    private AudioSource audioSource;
    private Health health;
    private float nextFootstepTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        health = GetComponent<Health>();

        if (audioSettings == null || playerMovement == null ||
            basicAttack == null || bombSkill == null || dashSkill == null || health == null)
        {
            Debug.LogError("PlayerAudioController requires all references.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        if (basicAttack != null)
            basicAttack.Fired += PlayShoot;

        if (bombSkill != null)
            bombSkill.BombExploded += PlayBombExplosion;

        if (dashSkill != null)
            dashSkill.DashStarted += PlayDash;

        if (health != null)
            health.Damaged += PlayPlayerHit;
    }

    private void OnDisable()
    {
        if (basicAttack != null)
            basicAttack.Fired -= PlayShoot;

        if (bombSkill != null)
            bombSkill.BombExploded -= PlayBombExplosion;

        if (dashSkill != null)
            dashSkill.DashStarted -= PlayDash;

        if (health != null)
            health.Damaged -= PlayPlayerHit;
    }

    private void Update()
    {
        if (playerMovement.MovementInput.sqrMagnitude <= Mathf.Epsilon ||
            Time.time < nextFootstepTime)
            return;

        Play(audioSettings.GrassFootstepClip, audioSettings.GrassFootstepVolume);
        nextFootstepTime = Time.time + audioSettings.FootstepInterval;
    }

    private void PlayShoot()
    {
        Play(audioSettings.ShootClip, audioSettings.ShootVolume);
    }

    private void PlayBombExplosion()
    {
        Play(audioSettings.BombExplosionClip, audioSettings.BombExplosionVolume);
    }

    private void PlayDash()
    {
        Play(audioSettings.DashClip, audioSettings.DashVolume);
    }

    private void PlayPlayerHit(float damage, bool playHitAnimation)
    {
        if (playHitAnimation)
            Play(audioSettings.PlayerHitClip, audioSettings.PlayerHitVolume);
    }

    private void Play(AudioClip clip, float volume)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
