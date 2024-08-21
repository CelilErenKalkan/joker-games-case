using UnityEngine;
using System;

namespace Item_Management
{
    public enum ItemType
    {
        Empty = 0,
        Apple = 1,
        Pear = 2,
        Strawberry = 3
    }
    
    public static class ItemFactory
    {
        public static Item CreateItem(ItemType itemType)
        {
            return itemType switch
            {
                ItemType.Apple => new Apple(),
                ItemType.Pear => new Pear(),
                ItemType.Strawberry => new Strawberry(),
                _ => null,
            };
        }
    }
    
    [Serializable]
    public class Item
    {
        public ItemObject itemObject;
        public int itemAmount;

        public Item()
        {
            itemObject = Resources.Load<ItemObject>("Items/Empty");
        }

        public string GetName() => itemObject.name;
        public ItemType GetItemType => itemObject.itemType;
        public Sprite GetIcon => itemObject.icon;

        public virtual void Collect()
        {
        
        }
    }
    
    [Serializable]
    public class Apple : Item
    {
        public Apple()
        {
            itemObject = Resources.Load<ItemObject>("Items/Apple");
        }

        public override void Collect()
        {
            
        }
    }
    
    [Serializable]
    public class Pear : Item
    {
        public Pear()
        {
            itemObject = Resources.Load<ItemObject>("Items/Pear");
        }

        public override void Collect()
        {
        
        }
    }
    
    [Serializable]
    public class Strawberry : Item
    {
        public Strawberry()
        {
            itemObject = Resources.Load<ItemObject>("Items/Strawberry");
        }

        public override void Collect()
        {
        
        }
    }
}
