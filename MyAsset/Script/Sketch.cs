using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Line
{
    public List<Vector3> position = new List<Vector3>();
    public Color color = Color.red;
    public float width = 4; // 실제로는 0.02 (1~10 == 0.005~0.05)
}


[System.Serializable]
public enum MyColor { red, orange, yellow, green, blue, black }

public class Sketch : MonoBehaviour
{
    public Material[] MatColors;

    public GameObject linePrefab;
    LineRenderer lineRenderer;
    bool isDrawing = false;

    // Tube Renderer by good man
    TubeRenderer tube;

    // 위치 파악
    public GameObject rightHandAnchor;
    public GameObject CTObj;

    // 현재 선
    public GameObject currentLine;
    public Line currentLineData;

    // 전체 선들
    public List<GameObject> lines = new List<GameObject>();
    public List<Line> linesData = new List<Line>();

    // 옵션
    public Color currentColor;
    public MyColor currentMeshColor;
    public Slider width;
    public TMP_Text widthValue;

    // Start is called before the first frame update
    void Start()
    {
        currentColor = Color.red;
        currentMeshColor = MyColor.red;
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger));

        widthValue.text = width.value.ToString("F2");


        // 그리기 시작
        if(ManipulationMenu.sketchMode && !isDrawing && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5)
        {
            isDrawing = true;
            currentLineData = new Line();
            currentLineData.color = currentColor;
            currentLine = CreateLine();
        }

        // 그리는 중
        if (isDrawing)
        {
            AddPoint();

            // 그리기 종료
            if(isDrawing && OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.5)
            {
                // 그리기 종료시에 추가
                linesData.Add(currentLineData);
                isDrawing = false;

                // Tube
                tube = currentLine.GetComponent<TubeRenderer>();
                tube.SetRadius(width.value / 200);
                tube.SetPositions(currentLineData.position.ToArray());

                currentLine.GetComponent<MeshRenderer>().material = MatColors[getMeshColorIndex(currentMeshColor)];

                Mesh mesh = new Mesh();
                lineRenderer.BakeMesh(mesh);

                //currentLine.GetComponent<MeshCollider>().sharedMesh = mesh; // 선 모양대로 collider
                //currentLine.GetComponent<MeshFilter>().mesh = mesh; // 추후 선 색상을 바꾸거나 특수효과를 주려면 사용 가능

                currentLine.GetComponent<MeshCollider>().sharedMesh = tube.GetMesh();
                currentLine.GetComponent<MeshFilter>().sharedMesh = tube.GetMesh();


            }
        }

    }

    public GameObject CreateLine()
    {
        GameObject line = Instantiate(linePrefab);
        line.transform.SetParent(CTObj.transform);


        lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.startColor = currentColor;
        lineRenderer.endColor = currentColor;
        lineRenderer.startWidth = width.value / 200;
        lineRenderer.endWidth = width.value / 200;




        lines.Add(line);
        return line;
    }


    // Sketch

    // rightHandAnchor.transform.position: World
    // OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch): Local
    // transform.InverseTransformPoint(rightHandAnchor.transform.position): World to Local
    public void AddPoint()
    {
        if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.5)
        {
            //Debug.Log("RTouch");

            // 스케치 world space를 CT 오브젝트의 local space로 변환
            // CT Object와 Center Eye Anchor 둘 다 자체 position이 0,0,0, 회전도 0,0,0이므로 가능

            lineRenderer.SetPosition(lineRenderer.positionCount++, rightHandAnchor.transform.position);
            currentLineData.position.Add(rightHandAnchor.transform.position);

            //Debug.Log("---------------------");
            //Debug.Log(rightHandAnchor.transform.position);
            //Debug.Log(transform.InverseTransformPoint(rightHandAnchor.transform.position));
            //Debug.Log(CTObj.transform.InverseTransformPoint(rightHandAnchor.transform.position));
            //Debug.Log(rightHandAnchor.transform.InverseTransformPoint(rightHandAnchor.transform.position));

        }


    }


    public int getMeshColorIndex(MyColor color)
    {
        switch (currentMeshColor)
        {
            case MyColor.red: return 0;
            case MyColor.orange: return 1;
            case MyColor.yellow: return 2;
            case MyColor.green: return 3;
            case MyColor.blue: return 4;
            default: return 5;
        }


    }

    public void PickColor(int color)
    {

        switch (color)
        {
            case 0:
                currentColor = new Color(1, 0, 0); // NOT 255
                currentMeshColor = MyColor.red;
                break;
            case 1:
                currentColor = new Color(1, 0.5f, 0);
                currentMeshColor= MyColor.orange;
                break;
            case 2:
                currentColor = new Color(1, 1, 0);
                currentMeshColor = MyColor.yellow;
                break;
            case 3:
                currentColor = new Color(0, 1, 0);
                currentMeshColor = MyColor.green;
                break;
            case 4:
                currentColor = new Color(0, 0, 1);
                currentMeshColor = MyColor.blue;
                break;
            case 5:
            default:
                currentColor = new Color(0, 0, 0);
                currentMeshColor = MyColor.black;
                break;

        }

    }


    public void asdf(MyColor color)
    {

    }
}
