using System;

namespace Game_Management
{
    public static class Actions
    {
        public static Action NewGame;
        public static Action LoadGame;
        public static Action GameStart;
        public static Action GameEnd;
        public static Action NextTurn;
        public static Action ButtonTapped;
        public static Action<bool> AudioChanged;
        public static Action<bool> VibrationChanged;
        public static Action<int> RollDice;
        public static Action<int> DiceResult;
        public static Action GridAppeared;
        public static Action GridHasFallen;
        public static Action ItemAmountChanged;
    }
}
