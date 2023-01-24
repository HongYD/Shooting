using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorType
{
    CursorNormal,
    CursorGrab,
    Count,
}

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    private List<CustomCursor> cursors;
    [SerializeField]
    private CustomCursor currentCursor;
    public static CursorManager instance { get; private set; }
    public bool isNeedSetCursor = false;

    private void Awake()
    {
        instance= this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.SetCursor(currentCursor.cursorTexture, currentCursor.Offset, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        if (isNeedSetCursor)
        {
            Cursor.SetCursor(currentCursor.cursorTexture, currentCursor.Offset, CursorMode.Auto);
            isNeedSetCursor= false;
        }
    }

    public void SetCursorType(CursorType type, bool needSetCorsor = true)
    {
        for(int i=0;i<cursors.Count;i++)
        {
            if (cursors[i].cursorType == type)
            {
                currentCursor.cursorTexture = cursors[i].cursorTexture;
                currentCursor.cursorType = cursors[i].cursorType;
                currentCursor.Offset = cursors[i].Offset;
                isNeedSetCursor = needSetCorsor;
                break;
            }
        }
    }

    public void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
    }

    public void SetCursorLockState(CursorLockMode mode)
    {
        Cursor.lockState = mode;
    }
}

[System.Serializable]
public class CustomCursor
{
    public Texture2D cursorTexture;
    public CursorType cursorType;
    public Vector2 Offset;
}
