using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextResizer : MonoBehaviour
{
    RectTransform rt;
    Text txt;

    void Start()
    {
        rt = gameObject.GetComponent<RectTransform>(); // Acessing the RectTransform 
        txt = gameObject.GetComponent<Text>(); // Accessing the text component
    }

    void Update()
    {
        rt.sizeDelta = new Vector2(txt.preferredWidth, rt.rect.height); // Setting the height to equal the height of text
    }
}
