using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFowardAndDestroy : MonoBehaviour
{
    [SerializeField]
    float _speed;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }
}
