using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIPageGame : UIPopup
{
    [SerializeField] 
    private GridLayoutGroup gridLayoutGroup;
    private RectTransform _gridRt;
    
    private void Awake()
    {
        _gridRt = gridLayoutGroup.GetComponent<RectTransform>();
    }

    public Transform Layer(int layer)
    {
        if (layer == 0)
            return gridLayoutGroup.transform;

        return null;
    }

    public void GameWait(int xLen, int yLen, int width, int height, int xSpace, int ySpace, Action<int, int> clamp)
    {
        var size = _gridRt.rect.size;
        var xClampLen = (int)size.x / (width + xSpace);
        var yClampLen = (int)size.y / (height + ySpace);

        xLen = xLen < xClampLen ? xLen : xClampLen;
        yLen = yLen < yClampLen ? yLen : yClampLen;

        if (xLen * yLen % 2 != 0)
        {
            if (xLen >= yLen) xLen -= 1;
            else yLen -= 1;
        }

        gridLayoutGroup.constraintCount = xLen;
        gridLayoutGroup.cellSize = new Vector2(width, height);
        gridLayoutGroup.spacing = new Vector2(xSpace, ySpace);

        clamp.Invoke(xLen, yLen);
    }
}
