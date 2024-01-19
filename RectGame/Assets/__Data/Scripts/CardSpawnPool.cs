using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class CardSpawnPool<T> where T : CardBehaviour
{
    [SerializeField] private T prefab;
    [SerializeField, ReadOnly] private List<T> poolList = new List<T>();

    public int Count => poolList.Count;

    private Transform _currentLayerTransform;
    
    public T AddOnly(Transform layerTransform)
    {
        _currentLayerTransform = layerTransform;
        
        var card = Object.Instantiate(prefab, layerTransform);
        card.name = $"card_{card.transform.GetSiblingIndex()}";
        card.gameObject.SetActive(false);

        poolList.Add(card);

        return card;
    }

    public List<T> GetSpawnList(int count)
    {
        var requireCount = Count - count;

        if (requireCount > 0)
        {
            return poolList.GetRange(0, count);
        }
        
        foreach (var i in Enumerable.Range(0, requireCount))
        {
            AddOnly(_currentLayerTransform);
        }

        return poolList;
    }
}