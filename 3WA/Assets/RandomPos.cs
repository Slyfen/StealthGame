using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 randomCircle = Random.insideUnitCircle * 5 ;

        transform.position = new Vector3(randomCircle.x, transform.position.y, randomCircle.y);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
