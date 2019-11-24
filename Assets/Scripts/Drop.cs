using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public static Drop Instance;
    public string Type;
    public Sprite[] Sprites;

    void Start()
    {
        Instance = this;
        SpriteRenderer Sprite = GetComponent<SpriteRenderer>();
        Sprite.sprite = Sprites[Random.Range(0, 4)];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
