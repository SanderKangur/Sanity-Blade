using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SanityBlade/Weapon")]
public class WeaponData : ScriptableObject
{
    public float Damage;
    public float Speed;
    public Sprite Sprite;
}
