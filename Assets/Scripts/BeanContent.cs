using UnityEngine;

public sealed class BeanContent : MonoBehaviour
{
    public void PlaceElement(Element element) => element.transform.SetParent(transform);
}
