using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomCursorSetter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, cursorMode); // Or reset to custom if you want
    }
}
