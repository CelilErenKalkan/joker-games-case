using System.Collections.Generic;
using Board;
using Items;
using UnityEngine;

namespace Data_Management
{
    public class PlayerData
    {
        public PlayerData(List<Item> itemList)
        {
            this.ItemList = itemList;
        }

        // Public properties with encapsulation for all fields
        public string PlayerName { get; set; }
        public List<Item> ItemList { get; set; }
        public int DiceRoll { get; set; }
        public int DiceAmount { get; set; }
        public int AppleAmount { get; set; }
        public int PearAmount { get; set; }
        public int StrawberryAmount { get; set; }

        public bool IsMuted { get; set; }
        public bool IsVibrationOff { get; set; }
    }

    public static class PlayerDataManager
    {
        public static PlayerData PlayerData;
        public static List<BoardGenerator.Point> mapOrder;


        #region Data Management

        /// <summary>
        /// Loads all the data from the files.
        /// </summary>
        /// <returns></returns>
        public static void LoadData()
        {
            PlayerData = FileHandler.ReadFromJson<PlayerData>("PlayerData.json");
            LoadMapOrder();
        }
        
        /// <summary>
        /// Saves all the data from the files.
        /// </summary>
        /// <returns></returns>
        public static void SaveData()
        {
            FileHandler.SaveToJson(PlayerData, "PlayerData.json");
        }

        #endregion


        #region Inventory Management

        /// <summary>
        /// Get the amount of the item you wanted.
        /// </summary>
        /// <returns></returns>
        public static int GetCertainItemAmount(ItemType itemType)
        {
            if (PlayerData.ItemList.Count <= 0) return 0;
            
            int count = 0;
            
            foreach (var item in PlayerData.ItemList)
            {
                if (item.type == itemType)
                    count++;
            }

            return count;
        }
        
        /// <summary>
        /// Sets the amount of items collected.
        /// </summary>
        /// <returns></returns>
        public static void Collect(int amount, Item item)
        {
            PlayerData.ItemList.Add(item);
            SaveData();
        }

        /// <summary>
        /// Sets the amount of apples collected.
        /// </summary>
        /// <returns></returns>
        public static void CollectApple(int amount)
        {
            PlayerData.AppleAmount++;
            SaveData();
        }
        
        /// <summary>
        /// Sets the amount of pears collected.
        /// </summary>
        /// <returns></returns>
        public static void CollectPear(int amount)
        {
            PlayerData.PearAmount++;
            SaveData();
        }
        
        /// <summary>
        /// Sets the amount of strawberries collected.
        /// </summary>
        /// <returns></returns>
        public static void CollectStrawberry(int amount)
        {
            PlayerData.StrawberryAmount++;
            SaveData();
        }
        
        #endregion

        #region Dice Management

        /// <summary>
        /// Updates the amount of dices that gets thrown simultaneously.
        /// </summary>
        /// <returns></returns>
        public static void UpdateDiceAmount(int amount)
        {
            PlayerData.DiceAmount = amount;
            SaveData();
        }

        #endregion


        #region Map Management

        /// <summary>
        /// Saves the new map.
        /// </summary>
        /// <returns></returns>
        public static void SaveMapOrder(List<BoardGenerator.Point> newMapOrder)
        {
            mapOrder = newMapOrder;
            //Debug.Log(mapOrder.Count);
            FileHandler.SaveListToJson(mapOrder, "MapOrder.json");
        }

        /// <summary>
        /// Loads the new map.
        /// </summary>
        /// <returns></returns>
        public static void LoadMapOrder()
        {
            mapOrder = FileHandler.ReadListFromJson<BoardGenerator.Point>("MapOrder.json");
            Debug.Log(mapOrder.Count);
        }
        
        #endregion
    }
}
