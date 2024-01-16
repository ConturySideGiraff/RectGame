using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : UIDisplay
{
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private List<Transform> layerList = new List<Transform>();

    private RectTransform _gridRt;
    
    public Transform Layer(int layer) => layerList[layer];

    public void ClampCount(int xLen, int yLen, int width, int height, int xSpace, int ySpace, Action<int, int> clamp)
    {
        _gridRt ??= gridLayoutGroup.GetComponent<RectTransform>();
        
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
