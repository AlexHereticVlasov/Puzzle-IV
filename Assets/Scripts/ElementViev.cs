using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class ElementViev : MonoBehaviour
{
    [SerializeField] private Element _element;
    [SerializeField] private Image _image;

    private void OnEnable() => _element.Inicialized += OnInicialize;

    private void OnInicialize(Sprite sprite) => _image.sprite = sprite;

    private void OnDisable() => _element.Inicialized -= OnInicialize;
}
