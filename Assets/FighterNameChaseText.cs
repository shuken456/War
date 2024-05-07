using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterNameChaseText : MonoBehaviour
{
    //ëŒè€ï∫ém
    public GameObject targetFighter;

    public Vector3 offset;

    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetFighter != null)
        {
            if(targetFighter.GetComponent<FighterStatus>().UnitLeader)
            {
                GetComponent<Text>().text = "Åö" + targetFighter.GetComponent<FighterStatus>().FighterName;
            }
            else
            {
                GetComponent<Text>().text = targetFighter.GetComponent<FighterStatus>().FighterName;
            }
            
            Transform targetTransform = targetFighter.GetComponent<Transform>();
            rectTransform.position = targetTransform.position + offset;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
