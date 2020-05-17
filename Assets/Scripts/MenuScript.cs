using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuScript : MonoBehaviour
{

    public enum MenuStates { Main, AR };
    public MenuStates currentMenuState;

    public enum ARStates { Vehicle1, Vehicle2, Vehicle3};
    public ARStates currentARState;

    public Canvas mainMenuCanvas;
    public Canvas arCanvas;

    private TextMeshProUGUI placeRemove;
    private ARPlacement arPlaceScript;

    void Start()
    {
        currentMenuState = MenuStates.Main;
        currentARState = ARStates.Vehicle1;
        placeRemove = GameObject.Find("ARCanvas/PlaceRemove/PlaceRemoveText").GetComponent<TextMeshProUGUI>();
        arPlaceScript = GameObject.Find("AR Session Origin").GetComponent<ARPlacement>();
    }

    void Update()
    {
        switch (currentMenuState)
        {
            case MenuStates.Main:
                //display correct buttons
                mainMenuCanvas.gameObject.SetActive(true);
                arCanvas.gameObject.SetActive(false);

                break;

            case MenuStates.AR:
                //display correct buttons
                mainMenuCanvas.gameObject.SetActive(false);
                arCanvas.gameObject.SetActive(true);

                break;
        }
    }

    public MenuStates getMenuState()
    {
        return currentMenuState;
    }

    public ARStates getARState()
    {
        return currentARState;
    }

    //Main Menu
    public void Vehicle1ButtonPressed()
    {
        currentMenuState = MenuStates.AR;
        currentARState = ARStates.Vehicle1;
    }

    public void Vehicle2ButtonPressed()
    {
        currentMenuState = MenuStates.AR;
        currentARState = ARStates.Vehicle2;
    }

    public void Vehicle3ButtonPressed()
    {
        currentMenuState = MenuStates.AR;
        currentARState = ARStates.Vehicle3;
    }

    //AR Vehicle Buttons
    public void PlaceRemovePressed()
    {
        if (arPlaceScript.isVehiclePlaced()){ 
            placeRemove.text = "Place";
        } else
        {
            placeRemove.text = "Remove";
        }

    }

    //back button for both AR session and Dive session
    public void BackToMain()
    {
        currentMenuState = MenuStates.Main;
    }

    public void resetMenu()
    {
        placeRemove.text = "Place";
    }
}
