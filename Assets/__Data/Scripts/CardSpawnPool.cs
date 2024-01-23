using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class CardSpawnPool<T> where T : CardBehaviour
{
    private T _prefab;
    private List<T> _poolList = new List<T>();
    private Transform _currentLayerTransform;
    
    
    public int Count => _poolList.Count;

    public T AddOnly(Transform layerTransform, T prefab)
    {
        _prefab = prefab;
        _currentLayerTransform = layerTransform;
        
        var card = Object.Instantiate(prefab, layerTransform);
        card.name = $"card_{card.transform.GetSiblingIndex()}";
        card.gameObject.SetActive(false);

        _poolList.Add(card);

        return card;
    }

    public List<T> GetSpawnList(int count)
    {
        var requireCount = Count - count;

        if (requireCount > 0)
        {
            return _poolList.GetRange(0, count);
        }
        
        foreach (var i in Enumerable.Range(0, requireCount))
        {
            AddOnly(_currentLayerTransform, _prefab);
        }

        return _poolList;
    }
}