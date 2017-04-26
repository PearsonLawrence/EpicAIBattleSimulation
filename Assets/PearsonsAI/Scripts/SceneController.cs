using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    private GameObject[] Spawner1, Spawner2;
    public GameObject[] tools;// 0 = MenuTool, 1 = EditTool, 2 = PlayTool, 3 = SelectTool, 4 = ClassSelectTool, 5 = SceneEditTool
    private string selectClick, Class1, Class2, KnightClass, DragonClass;
    public GameObject DragonPic1, KnightPic1, DragonPic2, KnightPic2;
    public InputField NumClass1, NumClass2, HealthClass2, HealthClass1;
    public int ClassAmount1, ClassAmount2,HealthAmount1, HealthAmount2;
    public Material SkyOne, SkyTwo, SkyThree;
    public bool toggleState1, toggleState2;
    public GameObject SunLight1, sunlight2, sunlight3;
    public GameObject Map1, Map2, Map3;
    public AudioClip[] music;
    public AudioSource musicPlayer;
    public Slider Vol;

    enum States
    {
        SceneIdle,
        SceneEnter,
        SceneEdit,
        ScenePlay,
        SceneExit,
        SelectionState,
        AIspawnState,
        MapEditState,
        SelectClassState,
        SelectEditState,
    }


    States currentState;
    States EditState;
    void Start()
    {
        Map1.SetActive(true);
        SunLight1.SetActive(true);
        RenderSettings.skybox = SkyOne;
        currentState = States.SceneEnter;
        EditState = States.SelectionState;
        KnightClass = "Knight";
        DragonClass = "Dragon";
        Class1 = KnightClass;
        Class2 = KnightClass;
        ClassAmount1 = 1;
        ClassAmount2 = 1;
        HealthAmount1 = 50;
        HealthAmount2 = 50;
        MusicToggle = true;
        musicPlayer.enabled = true;
        musicPlayer.clip = music[0];
        musicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            
            case States.SceneEnter:
                Enter();
                break;
            case States.SceneIdle:
                Idle();
                break;
            case States.SceneEdit:
                Edit();
                break;
            case States.ScenePlay:
                Play();
                break;
            case States.SceneExit:
                Exit();
                break;

        }

        if (currentState != States.ScenePlay)
        {
            SceneClear();

        }

        musicPlayer.volume = Vol.value;
    }
    public void Idle()
    {
        
    }
    public void Enter()
    {
        tools[0].SetActive(true);
        for(int i = 0; i < tools.Length; i++)
        {
            if (i != 0)
            {
                tools[i].SetActive(false);
            }
        }
    }
    public void Edit()
    {
        tools[1].SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            if (i != 1)
            {
                tools[i].SetActive(false);
            }
        }
        switch (EditState)
        {
            case States.SelectionState:
               Select();
               break;
            case States.AIspawnState:
                Spawn();
                break;
            case States.MapEditState:
                Editor();
                break;
            case States.SelectClassState:
                SelectClass();
                break;
            case States.SelectEditState:
                SelectEdit();
                break;
        }
        
    }

    private void Play()
    {
        tools[2].SetActive(true);
        for (int i = 0; i < tools.Length; i++)
        {
            if (i != 2)
            {
                tools[i].SetActive(false);
            }
        }
    }

    private void Exit()
    {

    }

    private void Select()
    {
        tools[1].SetActive(true);
        for (int i = 0; i < 2; i++)
        {
            if (i != 1)
            {
                tools[i].SetActive(false);
            }
        }

    }

    private void SelectEdit()
    {
       
    }
    private void SceneClear()
    {
        GameObject[] temp1 = null;
        GameObject[] temp2 = null;
        if (currentState == States.ScenePlay)
        {

        }
        else
        {
            if (Class1 == KnightClass)
            {
                temp1 = GameObject.FindGameObjectsWithTag("Knight");
            }
            else if (Class1 == DragonClass)
            {

                temp1 = GameObject.FindGameObjectsWithTag("Dragon");
            }

            if (Class2 == KnightClass)
            {
                temp2 = GameObject.FindGameObjectsWithTag("Knight1");
            }
            else if (Class2 == DragonClass)
            {

                temp2 = GameObject.FindGameObjectsWithTag("Dragon1");
            }

            for (int i = 0; i < temp1.Length; i++)
            {
                Destroy(temp1[i]);

            }
            for (int i = 0; i < temp2.Length; i++)
            {
                Destroy(temp2[i]);

            }

        }
    }
    private void SelectClass()
    {
        tools[3].SetActive(true);
        for (int i = 0; i < tools.Length; i++)
        {
            if (i != 3)
            {
                tools[i].SetActive(false);
            }
        }

    }
    private void Spawn()
    {

    }
    private void Editor()
    {

        tools[4].SetActive(true);
        for (int i = 0; i < tools.Length; i++)
        {
            if (i != 4)
            {
                tools[i].SetActive(false);
            }
        }
    }
    public void LeftClick()
    {
        selectClick = "left";
    }
    public void RightClick()
    {
        selectClick = "right";
    }
    public void StartClick()
    {

    }
    public void EditClick()
    {
        currentState = States.SceneEdit;
    }
    public void SceneEditClick()
    {
        EditState = States.MapEditState;

    }
    public void PlayClick()
    {
        currentState = States.ScenePlay;
        GameObject[] temp1 = GameObject.FindGameObjectsWithTag("Spawn");
        GameObject[] temp2 = GameObject.FindGameObjectsWithTag("Spawn1");

        musicPlayer.Play();


            for (int i = 0; i < ClassAmount1; i++)
            {

           
                temp1[i].GetComponent<AIspawner>().DesiredClass = Class1;
             

            
            temp1[i].GetComponent<AIspawner>().health = HealthAmount1;
            temp1[i].GetComponent<AIspawner>().spawn = true;

            if (toggleState1)
            {

                temp1[i].GetComponent<AIspawner>().CanUseMagic = true;
            }
            else
            {

                temp1[i].GetComponent<AIspawner>().CanUseMagic = false;
            }

            if (KnightPic2.activeSelf)
            {

                temp1[i].GetComponent<AIspawner>().DesiredTarget = "KnightClass";
            }
            else if (DragonPic2.activeSelf)
            {

                temp1[i].GetComponent<AIspawner>().DesiredTarget = "DragonClass";
            }

        }
        
        for (int i = 0; i < ClassAmount2; i++)
        {

           
            temp2[i].GetComponent<AIspawner>().DesiredClass = Class2;
            
            temp2[i].GetComponent<AIspawner>().health = HealthAmount2;
            temp2[i].GetComponent<AIspawner>().spawn = true;
            if(toggleState2)
            {

                temp2[i].GetComponent<AIspawner>().CanUseMagic = true;
            }
            else
            {

                temp2[i].GetComponent<AIspawner>().CanUseMagic = false;
            }
            if (KnightPic1.activeSelf)
            {

                temp2[i].GetComponent<AIspawner>().DesiredTarget = "KnightClass";
            }
            else if (DragonPic1.activeSelf)
            {

                temp2[i].GetComponent<AIspawner>().DesiredTarget = "DragonClass";
            }

        }

    }
    public void ExitClick()
    {

    }
    public void BackClick()
    {
        GetComponent<CameraController>().FlatClick();

        if (currentState == States.SceneEnter || currentState == States.SceneIdle)
        {
            EditState = States.SelectionState;
        }
        else if (currentState == States.SceneEdit)
        {
            
            if (EditState == States.SelectClassState || EditState == States.MapEditState)
            {
                EditState = States.SelectionState;
                tools[1].SetActive(true);
                for (int i = 0; i < tools.Length; i++)
                {
                    if (i != 1)
                    {
                        tools[i].SetActive(false);
                    }
                }
            }
            else if (EditState == States.SelectionState)
            {
                currentState = States.SceneEnter;
                EditState = States.SelectionState;
            }
            

        }
        else if(currentState == States.ScenePlay)
        {
            currentState = States.SceneEnter;
        }

    }
    public void ClassSelectClick1()
    {
        if(Class1 == KnightClass)
        {
            Class1 = DragonClass;
            DragonPic1.SetActive(true);
            KnightPic1.SetActive(false);
        }
        else if(Class1 == DragonClass)
        {
            Class1 = KnightClass;
            DragonPic1.SetActive(false);
            KnightPic1.SetActive(true);
        }
    }
    public void ClassSelectClick2()
    {
        if (Class2 == KnightClass)
        {
            Class2 = DragonClass;
            DragonPic2.SetActive(true);
            KnightPic2.SetActive(false);
        }
        else if (Class2 == DragonClass)
        {
            Class2 = KnightClass;
            DragonPic2.SetActive(false);
            KnightPic2.SetActive(true);
        }
    }
    public void ClassEditClick()
    {
        EditState = States.SelectClassState;
    }
    public void NumClass()
    {
        bool temptest = int.TryParse(NumClass1.text, out ClassAmount1);
        if(ClassAmount1 == 0)
        {
            ClassAmount1 = 1;
        }
     
        if (ClassAmount1 > 50)
        {
            ClassAmount1 = 50;
        }
        bool temptest2 = int.TryParse(NumClass2.text, out ClassAmount2);
        if (ClassAmount2 > 50)
        {
            ClassAmount2 = 50;
        }
        if (ClassAmount2 == 0)
        {
            ClassAmount2 = 1;
        }

    }
    public void healthClass()
    {

        // string temp = HealthClass1.text;
        bool temptest = int.TryParse(HealthClass1.text, out HealthAmount1);
        //HealthAmount1 = int.Parse(temp);

        bool temptest2 = int.TryParse(HealthClass2.text, out HealthAmount2);
            //HealthAmount2 = int.Parse(temp2);
            
    }

    public void ToggleCast()
    {
        if(toggleState1)
        {
            toggleState1 = false;
        }
        else
        {
            toggleState1 = true;

        }
    }
    public void ToggleCast2()
    {
        if (toggleState2)
        {
            toggleState2 = false;
        }
        else
        {
            toggleState2 = true;

        }
    }

    public void SkyClick()
    {
        if(RenderSettings.skybox == SkyOne)
        {
            RenderSettings.skybox = SkyTwo;
            SunLight1.SetActive(false);
            sunlight2.SetActive(true);
            sunlight3.SetActive(false);
        }
        else if(RenderSettings.skybox == SkyTwo)
        {
            RenderSettings.skybox = SkyThree;
            SunLight1.SetActive(false);
            sunlight2.SetActive(false);
            sunlight3.SetActive(true);

        }
        else if (RenderSettings.skybox == SkyThree)
        {
            RenderSettings.skybox = SkyOne;
            SunLight1.SetActive(true);
            sunlight2.SetActive(false);
            sunlight3.SetActive(false);
        }
    }
    public void MapClick()
    {
        if(Map1.activeSelf)
        {

            Map1.SetActive(false);

            Map2.SetActive(true);

            Map3.SetActive(false);
        }
        else if (Map2.activeSelf)
        {

            Map1.SetActive(false);

            Map2.SetActive(false);

            Map3.SetActive(true);
        }
        else if (Map3.activeSelf)
        {

            Map1.SetActive(true);

            Map2.SetActive(false);

            Map3.SetActive(false);
        }
    }
    private bool MusicToggle;
    public void musicToggle()
    {
        if(MusicToggle == true)
        {
            MusicToggle = false;
            musicPlayer.enabled = false;
        }
        else
        {

            MusicToggle = true;
            musicPlayer.enabled = true;
        }

    }
    public void MusicClick()
    {
       for(int i = 0; i < music.Length; i++)
        {
            if(musicPlayer.clip == music[i] && i != music.Length - 1)
            {
                musicPlayer.clip = music[i + 1];
                musicPlayer.Play();
                break;
            }
            else if(musicPlayer.clip == music[i] && i == music.Length - 1)
            {
                musicPlayer.clip = music[0];
                musicPlayer.Play();
                break;
            }
            
        }
    }
}
