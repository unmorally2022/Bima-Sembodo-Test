using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public enum StateBuilding
    {
        Locked, CountingDown, LevelingUp
    }

    [SerializeField]
    GameplayManager gameplayManager;
    [SerializeField]
    GameObject[] BuildingObject;

    [SerializeField]
    public string BuildingName;
    [SerializeField]
    TMPro.TMP_Text[] TMP_TimeLeft;

    public TMPro.TMP_Text TMP_TimeLeftProperty;

    private StateBuilding stateBuilding;

    [SerializeField]
    Building buildingNeedToUnlockThis;
    [SerializeField]
    int buildingLevelNeedToUnlockThis;

    [SerializeField]
    public int MaxLevel;
    public int CurrentLevel;

    [SerializeField]
    public int[] SeccondNeedToUpgrade;    
    public int currentSeccond;
    [SerializeField]
    int SpeedUpSeccond = 300;

    IEnumerator Enum_TimerCountdown;

    // Start is called before the first frame update
    void Start()
    {
        if (buildingNeedToUnlockThis != null) {
            this.gameObject.SetActive(false);            
        }
        else
        {
            UnlockThisBuilding();
        }

        ReShowBuilding();
    }

    IEnumerator IE_TimerCountdown() {
        while (true) {

            if (CurrentLevel < MaxLevel)
            {
                TMP_TimeLeft[CurrentLevel - 1].text = "Time : " + SeccondToTime(currentSeccond);
                if (TMP_TimeLeftProperty != null)
                    TMP_TimeLeftProperty.text = "Time : " + SeccondToTime(currentSeccond);

                switch (stateBuilding)
                {
                    case StateBuilding.CountingDown:
                        currentSeccond -= 1;
                        if (currentSeccond <= 0)
                        {   
                            stateBuilding = StateBuilding.LevelingUp;
                            LevelUp();                           
                        }
                        break;
                    case StateBuilding.LevelingUp:


                        break;
                }
            }
            else {
                if(TMP_TimeLeftProperty!=null)
                    TMP_TimeLeftProperty.text = "Max Level";
                TMP_TimeLeft[CurrentLevel - 1].text = "Max Level";
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void LevelUp() {
        CurrentLevel += 1;
        ReShowBuilding();
        gameplayManager.ForceCheckBuildingToUnlock(this);
        
        currentSeccond = SeccondNeedToUpgrade[CurrentLevel - 1];
        
        stateBuilding = StateBuilding.CountingDown;
    }

    private void ReShowBuilding()
    {
        //Debug.Log(this.gameObject.name);
        for (int i = 0; i < BuildingObject.Length; i++) {
            if (i != CurrentLevel-1)
            {
                //Debug.Log(i);
                BuildingObject[i].SetActive(false);
            }
        }
        if (CurrentLevel < MaxLevel) { }
        else
        {
            if (TMP_TimeLeftProperty != null)
                TMP_TimeLeftProperty.text = "Max Level";
            TMP_TimeLeft[CurrentLevel - 1].text = "Max Level";
        }
        BuildingObject[CurrentLevel-1].SetActive(true);
        
    }

    private void UnlockThisBuilding() {        
        CurrentLevel = 1;
        currentSeccond = SeccondNeedToUpgrade[CurrentLevel - 1];

        stateBuilding = StateBuilding.CountingDown;
        gameObject.SetActive(true);

        Enum_TimerCountdown = IE_TimerCountdown();
        StartCoroutine(Enum_TimerCountdown);
    }

    public void ForceCheckBuildingToUnlock(Building _building) {
        if (buildingNeedToUnlockThis != null) {
            if (buildingNeedToUnlockThis.CurrentLevel >= buildingLevelNeedToUnlockThis && buildingNeedToUnlockThis == _building) {
                UnlockThisBuilding();
            }
        }
    }

    string SeccondToTime(int seccond) {
        System.TimeSpan time = System.TimeSpan.FromSeconds(seccond);

        //here backslash is must to tell that colon is
        //not the part of format, it just a character that we want in output
        string str = time.ToString(@"hh\:mm\:ss");

        return str;
    }

    public void SpeedUp() {
        if (currentSeccond - SpeedUpSeccond <= 0)
        {
            currentSeccond = 0;
        }
        else {
            SpeedUpSeccond -= SpeedUpSeccond;
        }
    }

    public void InstantUpgrade() {
        if (CurrentLevel < MaxLevel)
        {
            stateBuilding = StateBuilding.LevelingUp;
            currentSeccond = 0;
            LevelUp();
            
        }
    }


}
