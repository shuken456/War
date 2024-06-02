using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterNameChaseText : MonoBehaviour
{
    //‘ÎÛ•ºm
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
        //•ºm–¼‚ğ•\¦@•”‘à’·‚Íš‚ğ‚Â‚¯‚é
        if (targetFighter != null)
        {
            if(targetFighter.GetComponent<FighterStatus>().UnitLeader)
            {
                GetComponent<Text>().text = "š" + targetFighter.GetComponent<FighterStatus>().FighterName;
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
