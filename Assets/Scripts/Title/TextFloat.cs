using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class TitleTextFloat : MonoBehaviour
{
    public float AMP, SPD;
    Vector3 defaultPos;
    private void Start()
    {
        defaultPos = transform.position;
    }

    void Update()
    {
        transform.position = defaultPos + new Vector3(0, AMP * Mathf.Sin(SPD * Time.time), 0);
    }
}
