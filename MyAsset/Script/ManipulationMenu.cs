using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityVolumeRendering;
using TMPro;
using Oculus.Interaction;

public class ManipulationMenu : MonoBehaviour
{

    public GameObject crossSectionPlane;
    public GameObject slicingPlane;
    public TMP_Text currentMode;

    public GameObject rightHandAnchor; // 컨트롤러 부착시 위치할 곳
    public GameObject CTObj; // 고정시 위치할 곳

    public GameObject CSMenu, SCMenu, RoMenu, SkMenu, ObMenu; //Mode

    public Slider scale, rotation;
    public TMP_Text scaleValue, rotationValue;




    // Start is called before the first frame update
    void Start()
    {
        
    }


    bool isSCMenu = false;
    bool isROMenu = false;
    public static bool sketchMode = false;
    bool isObMenu = false;

    bool isSCFixed = false;
    bool isCSFixed = false;

    // Update is called once per frame
    void Update()
    {


        slicingPlane.transform.localScale = Vector3.one * 0.1f * scale.value;

        // crossSectionPlane은 크기가 커지거나 회전되지 않는 게 좋음
        // 임시로 가장 밖으로 빼냄
        Transform cparent = crossSectionPlane.transform.parent;
        crossSectionPlane.transform.SetParent(null);
        Transform sparent = slicingPlane.transform.parent;
        slicingPlane.transform.SetParent(null);

        CTObj.transform.rotation = Quaternion.Euler(0, rotation.value, 0); // 기본값 0, 0, 0
        CTObj.transform.localScale = Vector3.one * scale.value;

        // 크기 조정 이후에 Parent 적용
        crossSectionPlane.transform.SetParent(cparent);
        slicingPlane.transform.SetParent(sparent);


        scaleValue.text = scale.value.ToString("F2") + "x";
        rotationValue.text = rotation.value + "'";



        if (isSCMenu)
        {
            //crossSectionPlane.transform.SetPositionAndRotation(slicingPlane.transform.position + new Vector3(0, 0, 0.01f), slicingPlane.transform.rotation);
        }
        
    }

    // Main Menu
    public void CSMode()
    {
        crossSectionPlane.SetActive(true);
        if (!isSCFixed)
        {
            slicingPlane.SetActive(false);
        }
        currentMode.text = "Cross Section Mode";
        CSMenu.SetActive(true);
        SCMenu.SetActive(false);
        isSCMenu = false;
        AttachCS();

        isObMenu = false;
        ObMenu.SetActive(false);

    }

    public void SCMode()
    {
        if (!isCSFixed)
        {
            crossSectionPlane.SetActive(false);
        }
        slicingPlane.SetActive(true);
        currentMode.text = "CT Slicing Mode";
        CSMenu.SetActive(false);
        SCMenu.SetActive(true);
        isSCMenu = true;
        AttachSC();

        isObMenu = false;
        ObMenu.SetActive(false);
    }

    public void RoMode()
    {
        if (!isSCFixed)
        {
            slicingPlane.SetActive(false);
        }
        if (!isCSFixed)
        {
            crossSectionPlane.SetActive(false);
        }
        isROMenu = !isROMenu;
        RoMenu.SetActive(isROMenu);

        //crossSectionPlane.gameObject.SetActive(false);
        //slicingPlane.gameObject.SetActive(false);
        currentMode.text = "Rotation Mode";
        CSMenu.SetActive(false);

        isSCMenu = false;
        SCMenu.SetActive(false);

        sketchMode = false;
        SkMenu.SetActive(false);
    }

    public void SkMode()
    {
        currentMode.text = "Sketch Mode";

        sketchMode = !sketchMode;
        SkMenu.SetActive(sketchMode);

        isROMenu = false;
        RoMenu.SetActive(false);

    }

    public void ObMode()
    {
        currentMode.text = "Object Mode";

        SCMenu.SetActive(false);
        CSMenu.SetActive(false);
        isSCMenu=false;

        isObMenu = !isObMenu;
        ObMenu.SetActive(isObMenu);

    }

    public void ExitScene()
    {
        SceneManager.LoadScene("Mainmenu");
        //JsonUtility
        //PlayerPrefs
    }


    //Cross Section Mode
    public void FixCS() // 판 고정 및 Grab 허용
    {
        crossSectionPlane.GetComponent<GrabInteractable>().enabled = true;
        crossSectionPlane.transform.SetParent(CTObj.transform);
        isCSFixed = true;
    }

    public void AttachCS()
    {
        crossSectionPlane.GetComponent<GrabInteractable>().enabled = false;
        crossSectionPlane.transform.SetParent(rightHandAnchor.transform);
        crossSectionPlane.transform.position = rightHandAnchor.transform.position + new Vector3(0, 0, 0.1f);
        isCSFixed = false;
    }


    //CT Slicing Mode
    public void FixSC() // 판 고정 및 Grab 허용
    {

        slicingPlane.GetComponent<GrabInteractable>().enabled = true;
        slicingPlane.transform.SetParent(CTObj.transform);
        isSCFixed = true;
    }

    public void AttachSC()
    {
        slicingPlane.GetComponent<GrabInteractable>().enabled = false;
        slicingPlane.transform.SetParent(rightHandAnchor.transform);
        slicingPlane.transform.position = rightHandAnchor.transform.position + new Vector3(0, 0, 0.1f);
        isSCFixed = false;
    }





}
