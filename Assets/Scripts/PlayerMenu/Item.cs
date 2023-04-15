using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")] 
public class Item :ScriptableObject
{
    [Header("On Cell")]
    public Sprite image;
    public string name;
    public int price;
    public string description;
    public bool consumable;

    public bool Equals(Item item)
    {
        return name == item.name;
    }
    
    
}


