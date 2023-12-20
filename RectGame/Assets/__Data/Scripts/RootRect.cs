using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootRect : MonoBehaviour
{
    public List<Rect> _rectList = new List<Rect>();

    //

    public Texture2D _debugDraw;

    private void Start()
    {
        ChildRectList(200, 200, 10, 10, out _rectList);
    }

    private void OnGUI()
    {
        foreach (var v in _rectList)
        {
            GUI.DrawTexture(v, _debugDraw);
        }
    }

    private RectTransform _rt = null;
    private Rect _rect = default;

    public Rect GetRect()
    {
        _rt ??= GetComponent<RectTransform>();

        var point = new Vector2(_rt.offsetMin.x, _rt.offsetMax.y * -1);
        var size = _rt.rect.size;

        _rect = new Rect(point, size);

        return _rect;
    }

    public void ChildRectList(int width, int height, int xLen, int yLen, out List<Rect> childRectList)
    {
        childRectList = new List<Rect>();

        var rootRect = GetRect();
        var childsize = new Vector2(width, height);
        var fullsize = childsize * new Vector2(xLen, yLen);

        for (int y = 0; y < yLen; y++)
        {
            for (int x = 0; x < xLen; x++)
            {
                var inter = rootRect.center - fullsize * 0.5F;
                var point = inter + new Vector2(x, y) * childsize;
                var childRect = new Rect(point, childsize);

                childRectList.Add(childRect);
            }
        }
    }
}