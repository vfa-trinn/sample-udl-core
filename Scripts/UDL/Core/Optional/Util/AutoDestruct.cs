using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour 
{
    [SerializeField]
    private float _lifeTime = 1.5f;
    [SerializeField]
    private bool _hideOnDestruct = false;

    public void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(DelayAutoDestruct());
    }

    private IEnumerator DelayAutoDestruct()
    {
        yield return new WaitForSeconds(_lifeTime);
        if(_hideOnDestruct)
        {
            gameObject.SetActive(false);
            yield break;
        }
        Destroy(gameObject);
    }
}
