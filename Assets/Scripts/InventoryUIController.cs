using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{

    public Image WeaponSprite;
   
    public Image ItemSprite;


    public static InventoryUIController Instance;

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
