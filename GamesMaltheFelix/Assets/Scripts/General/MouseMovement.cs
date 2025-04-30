using UnityEngine;
using UnityEngine.UI;

public class MouseMovement : MonoBehaviour
{
    public RectTransform customCursorUI; // Assign this in the inspector

    void Start()
    {
        Cursor.visible = false; // Hide the system cursor
    }

    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            customCursorUI.parent as RectTransform,
            Input.mousePosition,
            null, // Use null for Screen Space - Overlay canvas
            out pos
        );

        customCursorUI.anchoredPosition = pos;
    }
}
