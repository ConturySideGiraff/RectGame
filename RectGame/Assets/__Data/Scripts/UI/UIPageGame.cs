using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIPageGame : UIPopup
{
    // layer 0
    [SerializeField] 
    private GridLayoutGroup gridLayoutGroup;
    private RectTransform _gridLayoutGroupRt;

    private void Awake()
    {
        _gridLayoutGroupRt = gridLayoutGroup.GetComponent<RectTransform>();
    }

    // layer get
    public Transform Layer(int layer)
    {
        if (layer == 0)
            return gridLayoutGroup.transform;

        return null;
    }

    // layer 0
    public void GameWait(int width, int height, int xLen, int yLen, Action<int, int> clamp)
    {
        var size = _gridLayoutGroupRt.rect.size;
        var xClampLen = (int)size.x / width;
        var yClampLen = (int)size.y / height;

        xLen = xLen < xClampLen ? xLen : xClampLen;
        yLen = yLen < yClampLen ? yLen : yClampLen;
        
        gridLayoutGroup.constraintCount = xLen;
        gridLayoutGroup.cellSize = new Vector2(width, height);

        clamp.Invoke(xLen, yLen);
    }
}
