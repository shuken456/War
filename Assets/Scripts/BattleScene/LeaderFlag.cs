using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderFlag : MonoBehaviour
{
    //ëŒè€ï∫ém
    public GameObject targetFighter;

    public Vector3 offset;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Image>().color = targetFighter.GetComponent<SpriteRenderer>().color;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetFighter != null)
        {
            Transform targetTransform = targetFighter.GetComponent<Transform>();
            rectTransform.position = targetTransform.position + offset;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
