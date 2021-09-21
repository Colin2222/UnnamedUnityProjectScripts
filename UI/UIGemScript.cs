using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGemScript : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(image.rectTransform.rect.y);
        //Debug.Log(rigidbody2d.position.y);
        Vector3 screenPos = cam.WorldToScreenPoint(rigidbody2d.position);
        Debug.Log(screenPos);

        if(screenPos.y < 10000)
        {
            Debug.Log("UI GEM DESTROYED");
            Destroy(gameObject);
        }


    }
}
