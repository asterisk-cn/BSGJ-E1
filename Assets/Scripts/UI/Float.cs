using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float AMP, SPD;
    Vector3 _defaultPos;

    private void Start()
    {
        _defaultPos = transform.position;
    }

    void Update()
    {
        this.transform.position = _defaultPos + new Vector3(0, AMP * Mathf.Sin(SPD * Time.time), 0);
    }
}
