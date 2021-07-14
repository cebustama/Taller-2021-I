using UnityEngine;

[CreateAssetMenu]
public class FireType : ScriptableObject
{
    public ShootingType shootingType = ShootingType.hold;
    public GameObject projectilePrefab;
    public float rateOfFire = 1f;
}
