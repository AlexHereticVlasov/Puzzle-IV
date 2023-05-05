using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class EventSystemsExtentions
{
    public static T GetFirstCompponentOverPointer<T>(this EventSystem system, PointerEventData eventData)where T: MonoBehaviour
    {
        var components = new List<RaycastResult>();
        system.RaycastAll(eventData, components);

        foreach (var raycast in components)
            if (raycast.gameObject.TryGetComponent(out T component))
                return component;

        return null;
    }

    public static bool TryGetFirstComponentOverPointer<T>(this EventSystem system, PointerEventData eventData, out T component) where T: MonoBehaviour
    {
        component = GetFirstCompponentOverPointer<T>(system, eventData);
        return component != null;
    }
}
