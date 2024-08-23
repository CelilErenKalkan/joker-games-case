using System;
using System.Collections.Generic;
using Board;
using Item_Management;
using UnityEngine;

namespace Data_Management
{
    [Serializable] // Required for JSON serialization
    public struct PlayerData
    {
        // Public fields or properties to be serialized
        public string playerName;
        public List<Item> itemList;
        public int currentGrid;
        public int diceAmount;

        public bool isMuted;
        public bool isVibrationOff;

        // Constructor with default values
        public PlayerData(List<Item> itemList)
        {
            playerName = "Player";
            this.itemList = itemList ?? new List<Item>();
            currentGrid = 0;
            diceAmount = 2;
            isMuted = false;
            isVibrationOff = false;
        }
    }

    public static class PlayerDataManager
    {
        public static PlayerData PlayerData;
        public static List<BoardGenerator.Point> MapOrder;

        #region Data Management

        /// <summary>
        /// Loads all the data from the files with error handling.
        /// </summary>
        public static void LoadData()
        {
            try
            {
                PlayerData = FileHandler.ReadFromJson<PlayerData>("PlayerData.json");
                if (PlayerData.diceAmount <= 0) PlayerData.diceAmount = 2;
                SaveData();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data: {ex.Message}");
                PlayerData = new PlayerData(new List<Item>());
                SaveData();
            }
        }

        /// <summary>
        /// Saves all the data to the files with error handling.
        /// </summary>
        public static void SaveData()
        {
            try
            {
                FileHandler.SaveToJson(PlayerData, "PlayerData.json");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data: {ex.Message}");
            }
        }

        #endregion

        #region Inventory Management

        public static int GetCertainItemAmount(ItemType itemType)
        {
            if (PlayerData.itemList == null || PlayerData.itemList.Count <= 0) return 0;

            int count = 0;
            foreach (var item in PlayerData.itemList)
            {
                if (item.GetItemType == itemType)
                    count++;
            }

            return count;
        }

        public static Sprite GetCertainItemSprite(ItemType itemType)
        {
            foreach (var item in PlayerData.itemList)
            {
                if (item.GetItemType == itemType)
                    return item.GetIcon;
            }

            var newItem = ItemFactory.CreateItem(itemType);
            return newItem.GetIcon;
        }

        public static void Collect()
        {
            var amount = MapOrder[PlayerData.currentGrid].itemAmount;
            var itemType = MapOrder[PlayerData.currentGrid].itemType;
            if (amount <= 0)
            {
                Debug.LogWarning("Amount to collect must be greater than zero.");
                return;
            }
            
            PlayerData.itemList ??= new List<Item>();

            foreach (var item in PlayerData.itemList)
            {
                if (item.GetItemType == itemType)
                {
                    item.itemAmount += amount;
                    return;
                }
            }
            
            var newItem = ItemFactory.CreateItem(itemType);
            PlayerData.itemList.Add(newItem);
            newItem.itemAmount = 1;
        }
        
        #endregion

        #region Dice Management

        public static void UpdateDiceAmount(int amount)
        {
            if (amount <= 0)
            {
                Debug.LogWarning("Dice amount must be greater than zero.");
                return;
            }

            PlayerData.diceAmount = amount;
            SaveData();
        }

        #endregion

        #region Map Management

        public static void SaveMapOrder(List<BoardGenerator.Point> newMapOrder)
        {
            MapOrder = newMapOrder;
            try
            {
                FileHandler.SaveListToJson(MapOrder, "MapOrder.json");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save map order: {ex.Message}");
            }
        }

        public static void LoadMapOrder()
        {
            try
            {
                MapOrder = FileHandler.ReadListFromJson<BoardGenerator.Point>("MapOrder.json");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load map order: {ex.Message}");
                MapOrder = new List<BoardGenerator.Point>(); // Initialize empty list in case of failure.
            }
        }

        #endregion
    }
}
