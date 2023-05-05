using UnityEngine;

public sealed class Bean : MonoBehaviour
{
    private float minX;

    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Element _template;
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private RectTransform _beanContent;

    private void Awake()
    {
    }

    private void Start()
    {
        for(int i = 0; i < _sprites.Length; i++)
        {
            var element = Instantiate(_template, _content);
            element.Init(_sprites[i], i, _canvas, _beanContent);
            Shuffle(element);
        }
    }

    private void Shuffle(Element element)
    {
        element.transform.localPosition = GetRandomPosition();
    }

    private Vector2 GetRandomPosition() => new Vector3(Random.Range(-210, 210), Random.Range(-290, 290));

}
