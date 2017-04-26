using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraController : MonoBehaviour {
    public Button MenuButton, PanOption, FlatOption;
    public bool Pan, Flat, Settings, MenuB;
   
    private Vector3 target;
    public GameObject View;
    public float speedMod;
    enum States
    {
        Menu,
        Settings,
        PanView,
        FlatView,
        AriealView
    }
    private Vector3 startHeight;
    States currentState, GameState;
    private void Start()
    {
        currentState = States.Settings;
        GameState = States.FlatView;
        startHeight = transform.position;
    }
    void Update()
    {
        switch (currentState)
        {
            case States.Settings:
                EditSettings();
                break;
            case States.Menu:
                Menu();
                break;

        }


        //target = ((Obj1.position - obj2.position) * 0.5f) + Obj1.position;
        //DoPanView();

    }
    public void EditSettings()
    {
        switch (GameState)
        {
            case States.PanView:
                DoPanView();
                break;
            case States.FlatView:
                DoFlatView();
                break;
            case States.AriealView:
                DoAirealView();
                break;


        }


    }

    public void Menu() { }
    public void DoPanView()
    {
       
            target = (CenterPos("Knight") + CenterPos("Knight1")) / 2;
        
            transform.LookAt(target);
            //transform.Translate(transform.right);
            transform.RotateAround(target, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * speedMod);
        ActivePan = true;

    }
    public void DoFlatView()
    {
      
            transform.position = FlatLocation.position;
            transform.rotation = FlatLocation.rotation;
        ActivePan = false;

    }
    public void DoAirealView()
    {
      
            target = (CenterPos("Knight") + CenterPos("Knight1"))/ 2;
         
        
        transform.LookAt(target);
        //transform.Translate(transform.right);
        transform.RotateAround(target, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * speedMod);
        ActivePan = false;

    }
    private static bool ActivePan;
    private static bool ActiveFlat = true;
    public Transform FlatLocation;

    public void PanClick()
    {
        if(ActivePan == false)
        {

            transform.position = FlatLocation.position;
        }
        GameState = States.PanView;
    } 
    public void FlatClick()
    {
        GameState = States.FlatView;
    }
    public void ViewClick()
    {
       if (View.activeSelf == false)
        { 
           View.SetActive(true);
        }
       else
        {
            View.SetActive(false);
        }
    }
    public void AirealPanClick()
    {   if (transform.position.y >= (startHeight.y + 45) || transform.position.z >= (startHeight.z + 15))
        {
            GameState = States.AriealView;
        }
        else
        {
            transform.position = transform.position + new Vector3(0, 15, 5);
        }
        GameState = States.AriealView;
       
    }
    private GameObject[] targets;
    public string desiredTarget;
    public Vector3 CenterPos(string desired)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(desired);
        Bounds bound = new Bounds();
        if (gos.Length == 0)
            return Vector3.zero;
        if (gos.Length == 1)
            return gos[0].transform.position;

        for (var i = 1; i < gos.Length; i++)
           
                bound.Encapsulate(gos[i].transform.position);
            
        return bound.center;
    }
}
