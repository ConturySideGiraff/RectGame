using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonManager<UIManager>
{
    [SerializeField] private List<UIPopup> uiPopupList = new List<UIPopup>();
    [SerializeField] private List<UIDisplay> uiDisplayList = new List<UIDisplay>();
    
    public T GetPopup<T>() where T : UIPopup => uiPopupList.FirstOrDefault(t => t.GetType() == typeof(T)) as T;
    public T GetDisplay<T>() where T : UIDisplay => uiDisplayList.FirstOrDefault(t => t.GetType() == typeof(T)) as T;

    public void OffPopupAll()
    {
        uiPopupList.ForEach(p => p.Off());
    }
}
