using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBordersHolder : MonoBehaviour
{
    public float colDepth = 4f;
    public float zPosition = 0f;
    private Vector2 screenSize;
    private Transform topBorder;
    private Transform botBorder;
    private Transform leftBorder;
    private Transform rightBorder;
    private Vector3 cameraPos;

    void Start()
    {
        Invoke("SetUpBorders", 1.5f);
    }

    public void ClearOldBorders()
    {
        // Destroy the old borders
        if (topBorder != null) Destroy(topBorder.gameObject);
        if (botBorder != null) Destroy(botBorder.gameObject);
        if (rightBorder != null) Destroy(rightBorder.gameObject);
        if (leftBorder != null) Destroy(leftBorder.gameObject);
    }

    public void SetUpBorders()
    {
        // Generate our empty objects
        topBorder = new GameObject().transform;
        botBorder = new GameObject().transform;
        rightBorder = new GameObject().transform;
        leftBorder = new GameObject().transform;

        // Adding names and tags
        topBorder.name = "Top Border";
        botBorder.name = "Bot Border";
        rightBorder.name = "Right Border";
        leftBorder.name = "Left Border";

        topBorder.tag = "HBorder";
        botBorder.tag = "HBorder";
        rightBorder.tag = "VBorder";
        leftBorder.tag = "VBorder";

        // Add the colliders
        topBorder.gameObject.AddComponent<BoxCollider2D>();
        botBorder.gameObject.AddComponent<BoxCollider2D>();
        rightBorder.gameObject.AddComponent<BoxCollider2D>();
        leftBorder.gameObject.AddComponent<BoxCollider2D>();
        topBorder.GetComponent<BoxCollider2D>().isTrigger = true;
        botBorder.GetComponent<BoxCollider2D>().isTrigger = true;
        rightBorder.GetComponent<BoxCollider2D>().isTrigger = true;
        leftBorder.GetComponent<BoxCollider2D>().isTrigger = true;

        // Make them children of whatever object this script attached to, better to the Camera so the objects move with the camera without extra scripting
        topBorder.parent = transform;
        botBorder.parent = transform;
        rightBorder.parent = transform;
        leftBorder.parent = transform;

        // Generate world space point informations for position and scale calculations
        cameraPos = Camera.main.transform.position;
        screenSize = GetScreenWorldSize();

        // Change scales and positions to match the edges of the screen 
        rightBorder.localScale = new Vector3(colDepth, screenSize.y * 2, colDepth);
        rightBorder.position = new Vector3(cameraPos.x + screenSize.x + (rightBorder.localScale.x * 0.5f), cameraPos.y, zPosition);
        leftBorder.localScale = new Vector3(colDepth, screenSize.y * 2, colDepth);
        leftBorder.position = new Vector3(cameraPos.x - screenSize.x - (leftBorder.localScale.x * 0.5f), cameraPos.y, zPosition);
        topBorder.localScale = new Vector3(screenSize.x * 2, colDepth, colDepth);
        topBorder.position = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (topBorder.localScale.y * 0.5f), zPosition);
        botBorder.localScale = new Vector3(screenSize.x * 2, colDepth, colDepth);
        botBorder.position = new Vector3(cameraPos.x, cameraPos.y - screenSize.y - (botBorder.localScale.y * 0.5f), zPosition);
    }

    public static Vector2 GetScreenWorldSize()
    {
        Vector2 screenSize = Vector2.zero;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
        return screenSize;
    }
}
