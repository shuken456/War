using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FormationListManager : MonoBehaviour
{
    public GameObject[] UnitUI;
    public GameObject[] UnitOblect;

    public GameObject LockButton;

    public GameObject EmptyInfantry;
    public GameObject EmptyArcher;

    private GameObject Fighter;

    //���j�b�gDB�ƕ��mDB
    private List<PlayerUnit> PlayerUnitDataBaseAllList;
    private List<PlayerFighter> PlayerFighterDataBaseAllList;

    // Start is called before the first frame update
    void Start()
    {
        //DB�f�[�^�擾
        PlayerUnitDataBaseAllList = Resources.Load<PlayerUnitDB>("DB/PlayerUnitDB").PlayerUnitDBList.OrderBy((n) => n.Num).ToList(); //���j�b�g�ԍ����ɕ��ёւ�
        PlayerFighterDataBaseAllList = Resources.Load<PlayerFighterDB>("DB/PlayerFighterDB").PlayerFighterDBList.OrderByDescending((n) => n.UnitLeader).ToList(); //���j�b�g���[�_�[�����ɗ���悤�ɂɕ��ёւ�

        int UnitCount = PlayerUnitDataBaseAllList.Count;

        for (int i = 0; i < UnitCount; i++)
        {
            UnitUI[i].transform.Find("Text (UnitName)").GetComponent<Text>().text = PlayerUnitDataBaseAllList[i].Name;
            UnitOblect[i].transform.Find("UnitBackImage").GetComponent<SpriteRenderer>().color = Color.white;

            //���j�b�g���̕��m�����i�[
            List<PlayerFighter> PlayerFighterDataBaseUnitList = PlayerFighterDataBaseAllList.FindAll(n => n.UnitNum == i + 1);
            int FighterCount = PlayerFighterDataBaseUnitList.Count;

            for (int j = 0; j < FighterCount; j++)
            {
                if(PlayerFighterDataBaseUnitList[j].type == 1)
                {
                    Fighter = Instantiate(EmptyInfantry, new Vector3(0, 0, 0), Quaternion.identity);
                }
                else if (PlayerFighterDataBaseUnitList[j].type == 2)
                {
                    Fighter = Instantiate(EmptyArcher, new Vector3(0, 0, 0), Quaternion.identity);
                }

                Fighter.GetComponent<SpriteRenderer>().color = PlayerUnitDataBaseAllList[i].UnitColor;
                Fighter.transform.parent = UnitOblect[i].transform;
                Fighter.transform.localPosition = PlayerFighterDataBaseUnitList[j].Position;
            }
        }

        //����������������ĂȂ��ꍇ�A���b�N�{�^����\��
        if(UnitCount < 10)
        {
            LockButton.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, UnitOblect[UnitCount].transform.position);
            LockButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
