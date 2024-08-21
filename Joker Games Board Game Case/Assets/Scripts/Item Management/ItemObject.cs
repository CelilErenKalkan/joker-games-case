using UnityEngine;

namespace Item_Management
{
    [CreateAssetMenu(fileName = "newItem",menuName = "Item", order = 0)]

    public class ItemObject : ScriptableObject
    {
        public ItemType itemType;
        public Sprite icon;
    }
}