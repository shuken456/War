using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public BattleManager BaManager;

    //���m���U���g �X�N���[���r���[
    public GameObject FighterResultView;
    //���m���U���g�v���n�u
    public GameObject FighterResultUI;

    //�o���������m���X�g
    public List<PlayerFighter> SortieFighterList;

    // Start is called before the first frame update
    void Start()
    {
        //�o���������m���擾
        List<PlayerUnit> SortieUnit = BaManager.PlayerUnitDataBaseAllList.FindAll((n) => n.SoriteFlg);
        foreach (PlayerUnit pu in SortieUnit)
        {
            List<PlayerFighter> SortieFighter = BaManager.PlayerFighterDataBaseAllList.FindAll((n) => n.UnitNum == pu.Num);
            foreach (PlayerFighter pf in SortieFighter)
            {
                SortieFighterList.Add(pf);
            } 
        }

        //�o���������m�����U���g���쐬
        foreach (PlayerFighter pf in SortieFighterList)
        {
            GameObject button = Instantiate(FighterResultUI, FighterResultView.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
