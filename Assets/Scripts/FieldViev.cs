using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FieldViev : MonoBehaviour
{
    [SerializeField] private Field _field;
    [SerializeField] private GridLayoutGroup _layoutGroup;

    private void OnEnable() => _field.Victory += OnVictory;

    private void OnDisable() => _field.Victory -= OnVictory;

    private void OnVictory() => StartCoroutine(RemoveSpasing());

    private IEnumerator RemoveSpasing()
    {
        while (_layoutGroup.spacing.x > 0 && _layoutGroup.spacing.y > 0)
        {
            _layoutGroup.spacing = new Vector2(Mathf.Clamp(_layoutGroup.spacing.x - 1, 0, int.MaxValue),
                                               Mathf.Clamp(_layoutGroup.spacing.y - 1, 0, int.MaxValue));
            yield return null;
        }
    }
}
