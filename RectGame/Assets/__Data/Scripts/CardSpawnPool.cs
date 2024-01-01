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
    [SerializeField] private List<T> poolList = new List<T>();

    public int Count => poolList.Count;
    
    public T Add(Transform layerTransform)
    {
        var card = Object.Instantiate(prefab, layerTransform);
        
#if UNITY_EDITOR
        card.name = $"card_{card.transform.GetSiblingIndex()}";
#endif
        card.gameObject.SetActive(false);

        poolList.Add(card);

        return card;
    }

    public T Spawn(Transform layerTransform)
    {
        foreach (var t in poolList.Where(t => !t.ActiveInHierarchy))
        {
            return t;
        }

        return Add(layerTransform);
    }
}