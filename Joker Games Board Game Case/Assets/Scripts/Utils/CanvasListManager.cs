using Data_Management;
using Game_Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasListManager : MonoBehaviour
{
    public Transform referenceTransform; // The transform to align with
    private RectTransform parentRectTransform;

    private void Start()
    {
        parentRectTransform = GetComponent<RectTransform>(); // Assuming this script is attached to the parent

        foreach (Transform child in transform)
        {
            Button button = child.GetComponent<Button>();

            if (button == null)
            {
                button = child.gameObject.AddComponent<Button>();
            }

            // Add listener for each child button
            button.onClick.AddListener(() => OnChildSelected(child));
            var text = child.GetChild(0).GetComponent<TMP_Text>();
            text.text = "x" + (child.GetSiblingIndex() + 1);
        }

        Transform selectedChild = transform.GetChild(PlayerDataManager.PlayerData.diceAmount - 1);
        MoveToSelectedChild(selectedChild);
    }


    private void MoveToSelectedChild(Transform selectedChild)
    {
        Vector3 difference = referenceTransform.position - selectedChild.position;

        // Move the parent by the difference to align the selected child with the referenceTransform
        parentRectTransform.position += new Vector3(difference.x, difference.y, 0);
    }
    
    private void OnChildSelected(Transform selectedChild)
    {
        MoveToSelectedChild(selectedChild);
        PlayerDataManager.UpdateDiceAmount(selectedChild.GetSiblingIndex() + 1);
        Actions.DiceAmountChanged?.Invoke(selectedChild.GetSiblingIndex() + 1);
    }
}