using UnityEngine;

[CreateAssetMenu(
    fileName = "GameAudioSettings",
    menuName = "Survival Top-down/Audio/Game Audio Settings")]
public class GameAudioSettings : ScriptableObject
{
    [Header("Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip dashClip;
    [SerializeField] private AudioClip bombExplosionClip;
    [SerializeField] private AudioClip playerHitClip;
    [SerializeField] private AudioClip grassFootstepClip;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)] private float shootVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float dashVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float bombExplosionVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float playerHitVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float grassFootstepVolume = 1f;

    [Header("Footsteps")]
    [SerializeField, Min(0.01f)] private float footstepInterval = 0.4f;

    public AudioClip ShootClip => shootClip;
    public AudioClip DashClip => dashClip;
    public AudioClip BombExplosionClip => bombExplosionClip;
    public AudioClip PlayerHitClip => playerHitClip;
    public AudioClip GrassFootstepClip => grassFootstepClip;
    public float ShootVolume => shootVolume;
    public float DashVolume => dashVolume;
    public float BombExplosionVolume => bombExplosionVolume;
    public float PlayerHitVolume => playerHitVolume;
    public float GrassFootstepVolume => grassFootstepVolume;
    public float FootstepInterval => footstepInterval;
}
