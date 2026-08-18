using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "CameraShakeSettings",
    menuName = "Survival Top-down/Camera Shake Settings")]
public class CameraShakeSettings : ScriptableObject
{
    [SerializeField] private CameraShakeProfile shot;
    [SerializeField] private CameraShakeProfile dashExplosion;
    [SerializeField] private CameraShakeProfile playerDamage;

    public CameraShakeProfile Shot => shot;
    public CameraShakeProfile DashExplosion => dashExplosion;
    public CameraShakeProfile PlayerDamage => playerDamage;
}

[Serializable]
public struct CameraShakeProfile
{
    [SerializeField, Min(0f)] private float duration;
    [SerializeField, Min(0f)] private float magnitude;

    public float Duration => duration;
    public float Magnitude => magnitude;
}
