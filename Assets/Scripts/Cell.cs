using UnityEngine;
using UnityEngine.Events;

public sealed class Cell : MonoBehaviour
{
    private Element _element;
    private int _index;

    public event UnityAction ElementPlaced;

    public Element Element => _element;

    public bool TrySetElement(Element element)
    {
        if (_element == null)
        {
            _element = element;
            _element.transform.SetParent(transform);
            _element.transform.localPosition = Vector3.zero;
            ElementPlaced?.Invoke();
            _element.Rotation += OnRotation;
            return true;
        }

        return false;
    }

    public void RemoveElement()
    {
        _element.Rotation -= OnRotation;
        _element = null;
    }

    private void OnRotation() => ElementPlaced?.Invoke();

    public bool CheackedIsCorrect => _index == _element.Index && _element.Orientation == Orientation.Up;

    public void Init(int index) => _index = index;
}
