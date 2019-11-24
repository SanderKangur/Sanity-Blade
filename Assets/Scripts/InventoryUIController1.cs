using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController1 : MonoBehaviour
{

    
    public Image SpellSprite;
    public Image PotionSprite;


    public static InventoryUIController1 Instance;

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateUI()
    {

    }

}
