using System.Collections.Generic;
using Items;

namespace Data_Management
{
    public struct PlayerData
    {
        public string playerName;
        public List<Item> itemList;
        public int diceRoll;
        public int appleAmount;
        public int pearAmount;
        public int strawberryAmount;

        public bool isMuted;
        public bool isVibrationOff;
    }
    
    public static class PlayerDataManager
    {
        public static PlayerData playerData;

        /// <summary>
        /// Loads all the data from the files.
        /// </summary>
        /// <returns></returns>
        public static void LoadData()
        {
            playerData = FileHandler.ReadFromJson<PlayerData>("PlayerData.json");
        }
        
        /// <summary>
        /// Saves all the data from the files.
        /// </summary>
        /// <returns></returns>
        public static void SaveData()
        {
            FileHandler.SaveToJson(playerData, "PlayerData.json");
        }
        
        /// <summary>
        /// Get the amount of the item you wanted.
        /// </summary>
        /// <returns></returns>
        public static int GetCertainItemAmount(ItemType itemType)
        {
            if (playerData.itemList.Count <= 0) return 0;
            
            int count = 0;
            
            foreach (var item in playerData.itemList)
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
            playerData.itemList.Add(item);
            SaveData();
        }
        
        /// <summary>
        /// Sets the amount of apples collected.
        /// </summary>
        /// <returns></returns>
        public static void CollectApple(int amount)
        {
            playerData.appleAmount++;
            SaveData();
        }

        /// <summary>
        /// Sets the amount of pears collected.
        /// </summary>
        /// <returns></returns>
        public static void CollectPear(int amount)
        {
            playerData.pearAmount++;
            SaveData();
        }
        
        /// <summary>
        /// Sets the amount of strawberries collected.
        /// </summary>
        /// <returns></returns>
        public static void CollectStrawberry(int amount)
        {
            playerData.strawberryAmount++;
            SaveData();
        }
    }
}
