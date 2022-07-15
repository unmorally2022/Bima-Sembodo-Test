using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    //TMPro.TMP_Text TMP_Time;
    [SerializeField]
    TMPro.TMP_Text TMP_Coin;
    [SerializeField]
    int coins = 0;
    [SerializeField]
    Building[] building;
    Building SelectedBuilding;

    [SerializeField]
    GameObject PanelBuildingProperty, PanelMessage;
    [SerializeField]
    TMPro.TMP_Text TMP_TextMessage, TMP_TextBuildingName, TMP_TextTimeLeftProperty;
    [SerializeField]
    Button ButtonSpeedUp;

    [SerializeField]
    int SpeedUpAmmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        //coins = 10;
        updateCoinText();

        PanelBuildingProperty.SetActive(false); PanelMessage.SetActive(false);
    }

    public void updateCoinText() {
        TMP_Coin.text = "Coins : " + coins.ToString() + "\n" + "Speed Up : "+SpeedUpAmmount.ToString();
    }

    public void ForceCheckBuildingToUnlock(Building _building) {
        for (int i = 0; i < building.Length; i++) {
            building[i].ForceCheckBuildingToUnlock(_building);
        }
    }

    public void ShowBuildingProperty() {
        SetSpeedUpButton();
        SelectedBuilding.TMP_TimeLeftProperty = TMP_TextTimeLeftProperty;
        TMP_TextBuildingName.text = SelectedBuilding.BuildingName;
        PanelBuildingProperty.SetActive(true);
    }

    private void SetSpeedUpButton() {
        if (SpeedUpAmmount > 0)
        {
            ButtonSpeedUp.enabled = true;
        }
        else
        {
            ButtonSpeedUp.enabled = false;
        }
    }

    public void GUI_5MinuteSpeedUp() {
        if (SelectedBuilding != null)
        {
            if (SelectedBuilding.currentSeccond < SelectedBuilding.SeccondNeedToUpgrade[SelectedBuilding.CurrentLevel - 1])
            {
                SelectedBuilding.SpeedUp();
                SpeedUpAmmount -= 1;
                SetSpeedUpButton();
            }
            else {
                TMP_TextMessage.text = "Max Level reached Mas E, jok ngeyel";
                PanelMessage.SetActive(true);
            }
        }
    }

    public void GUI_InstantUpgrade() {
        if (SelectedBuilding.CurrentLevel < SelectedBuilding.MaxLevel)
        {
            if (coins - 7 >= 0)
            {
                if (SelectedBuilding != null)
                {
                    SelectedBuilding.InstantUpgrade();
                    coins -= 7;
                    updateCoinText();
                }
            }
            else
            {
                TMP_TextMessage.text = "Coin ga cukup Mas E";
                PanelMessage.SetActive(true);
            }
        }
        else {
            TMP_TextMessage.text = "Max Level reached Mas E, jok ngeyel";
            PanelMessage.SetActive(true);
        }
    }

    public void GUI_CloseProperty()
    {
        if (SelectedBuilding != null)
        {
            SelectedBuilding.TMP_TimeLeftProperty = null;
            SelectedBuilding = null;            
        }

        PanelBuildingProperty.SetActive(false);
    }

    public void GUI_CloseMessage()
    {
        if (SelectedBuilding != null)
        {
            PanelMessage.SetActive(false);
        }
    }

    void Update()
    {

        
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
            {
                if (raycastHit.transform.tag == "BuildingObject") {
                    GameObject _building = raycastHit.transform.parent.gameObject;
                    if (_building != null) {
                        SelectedBuilding = _building.GetComponent<Building>();
                        ShowBuildingProperty();
                    }
                    //Debug.Log(_building.name);
                }
                //Transform objecthit = hit.transform;
                
                
            }
        }
#else
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.transform.tag == "BuildingObject") {
                    GameObject _building = raycastHit.transform.parent.gameObject;
                    if (_building != null) {
                        SelectedBuilding = _building.GetComponent<Building>();
                        ShowBuildingProperty();
                    }
                    //Debug.Log(_building.name);
                }
            }
        }
#endif
    }
}
