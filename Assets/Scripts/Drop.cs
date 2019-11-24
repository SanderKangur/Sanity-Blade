using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public static Drop Instance;
    public string Type;
    public WeaponData[] Weapons;
    public ItemData[] Items;
    public SpellData[] Spells;
    public PotionData[] Potions;

    public WeaponData Weapon;
    public ItemData Item;
    public SpellData Spell;
    public PotionData Potion;



    void Start()
    {
        Instance = this;
        Weapon = Weapons[Random.Range(0, Weapons.Length)];
        Item = Items[Random.Range(0, Items.Length)];
        Spell = Spells[Random.Range(0, Spells.Length)];
        Potion = Potions[Random.Range(0, Potions.Length)];

        if (Type.Equals("Weapon"))
        {
            GetComponent<SpriteRenderer>().sprite = Weapon.Sprite;
        }
        if (Type.Equals("Item"))
        {
            GetComponent<SpriteRenderer>().sprite = Item.Sprite;
        }
        if (Type.Equals("Spell"))
        {
            GetComponent<SpriteRenderer>().sprite = Spell.Sprite;
        }
        if (Type.Equals("Potion"))
        {
            GetComponent<SpriteRenderer>().sprite = Potion.Sprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
