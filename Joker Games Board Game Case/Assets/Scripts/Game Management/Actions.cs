using System;
using Item_Management;
using UnityEngine;

namespace Game_Management
{
    public static class Actions
    {
        public static Action NewGame;
        public static Action LoadGame;
        public static Action GameStart;
        public static Action GameEnd;
        public static Action NextTurn;
        public static Action GridAppeared;
        public static Action GridHasFallen;
        public static Action PrizeAddedToBag;
        public static Action PlayerStep;
        public static Action DiceToDiceCollision;
        public static Action DiceToFloorCollision;
        public static Action ButtonTapped;
        public static Action<bool> AudioChanged;
        public static Action<bool> VibrationChanged;
        public static Action<int> RollDice;
        public static Action<int> DiceResult;
        public static Action<int> DiceAmountChanged;
        public static Action<int> MoveForward;
        public static Action<Transform, int, ItemType> PrizesAppeared;


    }
}
