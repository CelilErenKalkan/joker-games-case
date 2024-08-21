using UnityEngine;
using System;
using UnityEditor;

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
        public string name;
        public ItemObject itemObject;
        public ItemType itemType;
        public Sprite icon;
        public int itemAmount;

        public Item()
        {
            itemObject = Resources.Load<ItemObject>("ScriptableObjects/Items/Empty");
            name = "Empty";
            itemType = ItemType.Empty;
        }
    
        public virtual void Collect()
        {
        
        }
    }
    
    [Serializable]
    public class Apple : Item
    {
        public Apple()
        {
            icon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/2D/Item Icons/item_apple.png");
            name = "Apple";
            itemType = ItemType.Apple;
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
            icon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/2D/Item Icons/item_pear.png");
            name = "Pear";
            itemType = ItemType.Pear;
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
            icon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/2D/Item Icons/item_strawberry.png");
            name = "Strawberry";
            itemType = ItemType.Strawberry;
        }
    
        public override void Collect()
        {
        
        }
    }
}
