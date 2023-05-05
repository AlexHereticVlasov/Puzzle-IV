using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public sealed class Element : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private const int AngleOfRotation = 90;
    private const float SizeChangeSpeed = 4;
    private const float RotationSpeed = 4;
    private const float IncreasedSize = 1.1f;

    private readonly int _sideAmount = Enum.GetNames(typeof(Orientation)).Length;

    [SerializeField] private RectTransform _canvas;
    [SerializeField] private Transform _bean;

    private Orientation _orientation;
    private RectTransform _previousParent;
    private int _index;
    private Coroutine _sizeChangeRotine;

    public event UnityAction Rotation;
    public event UnityAction<Sprite> Inicialized;
    
    public Orientation Orientation => _orientation;
    public int Index => _index;

    public void Init(Sprite sprite, int index, RectTransform canvas, Transform bean)
    {
        Inicialized?.Invoke(sprite);
        _index = index;
        _canvas = canvas;
        _bean = bean;
        RotateRandomly();
    }

    private void OnDisable() => transform.localScale = Vector2.one;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _previousParent = (RectTransform)transform.parent;
        transform.SetParent(_canvas);
    }

    public void OnDrag(PointerEventData eventData) => transform.position = eventData.position;

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.TryGetFirstComponentOverPointer(eventData, out Cell cell))
            TryPlaceToCell(cell);
        else if (EventSystem.current.TryGetFirstComponentOverPointer(eventData, out BeanContent beanContent))
            beanContent.PlaceElement(this);
        else 
            ReturnToBean();
        
        RemoveFromPrevious();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_sizeChangeRotine != null)
            StopCoroutine(_sizeChangeRotine);

        _sizeChangeRotine = StartCoroutine(ChangeSize(Vector3.one * IncreasedSize));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_sizeChangeRotine != null)
            StopCoroutine(_sizeChangeRotine);

        _sizeChangeRotine = StartCoroutine(ChangeSize(Vector3.one));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1))
            Rotate();
    }

    private void TryPlaceToCell(Cell cell)
    {
        if (cell.TrySetElement(this) == false)
            ReturnToBean();
    }

    private void Rotate() => StartCoroutine(RotateSmoothly());

    private IEnumerator RotateSmoothly()
    {
        ChangeOrientation();
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, ((int)_orientation) * AngleOfRotation);

        float t = 0;
        while (t <= 1)
        {
            yield return null;
            t += Time.deltaTime * RotationSpeed;
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
        }

        Rotation?.Invoke();
    }

    private void ChangeOrientation() => _orientation = (Orientation)(((int)_orientation + 1) % _sideAmount);

    private IEnumerator ChangeSize(Vector3 targetSize)
    {
        Vector3 startSize = transform.localScale;
        float t = 0;
        while (t <= 1)
        {
            yield return null;
            t += Time.deltaTime * SizeChangeSpeed;
            transform.localScale = Vector3.Lerp(startSize, targetSize, t);
        }
    }

    private void RemoveFromPrevious()
    {
        if (_previousParent.TryGetComponent(out Cell previousCell))
            previousCell.RemoveElement();
    }

    private void RotateRandomly()
    {
        int multiplier = Random.Range(0, _sideAmount);
        transform.Rotate(0, 0, AngleOfRotation * multiplier);
        _orientation = (Orientation)multiplier;
    }

    private void ReturnToBean()
    {
        transform.SetParent(_bean);
        transform.localPosition = Vector3.zero;
    }
}
