using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        Empty = 0,
        Apple = 1,
        Pear = 2,
        Strawberry = 3
    }

    [CreateAssetMenu(fileName = "newItem",menuName = "Item", order = 0)]

    public class Item : ScriptableObject
    {
        public ItemType type = 0;
        public GameObject prefab;
        public Sprite icon;
        public string itemName;
    
        public void Collect()
        {
        
        }
    }
}