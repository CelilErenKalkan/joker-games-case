using Items;
using UnityEngine;

namespace Board
{
    public class Grid : MonoBehaviour
    {
        [HideInInspector] public int gridNo;
        [HideInInspector] public int itemAmount;
        [HideInInspector] public ItemType itemType;
    }
}
