using UnityEngine;

public enum ShootingType
{
    click,
    hold,
    release,
    charge
}

[CreateAssetMenu]
public class FireType : ScriptableObject
{
    public ShootingType shootingType = ShootingType.hold;
    public GameObject projectilePrefab;
    public float rateOfFire = 1f;
    public int ammoCost = 0;

    [Header("Multi")]
    public int projectileAmount = 1;
    public float projectileSpreadAngle;

    [Header("UI")]
    public Sprite projectileSprite;
}
