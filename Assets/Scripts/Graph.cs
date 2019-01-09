using UnityEngine;
using UnityEngine.UI;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Collections;
using System.Collections.Generic;


public class Graph : MonoBehaviour
{
    //ref var
    public Transform pointPrefab;
    public Transform empty;
    [Space]
    public Slider resRange;
    public Slider timeXMulti;
    public Slider timeZMulti;
    public Slider freqXSine;
    public Slider freqZSine;
    public Slider magXSine;
    public Slider magZSine;
    public Dropdown dropdown;
    [Space]
    public Text resNum;
    public Text freq1Num;
    public Text freq2Num;
    public Text mag1Num;
    public Text mag2Num;
    public Text time1Num;
    public Text time2Num;
    public Text fpsNum;

    //private var
    private JobHandle xLoopHandle;
    private XLoop xLoop;
    private JobHandle gOHandle;
    private GOPos gOpos;
    private TransformAccessArray transforms;
    private NativeArray<Vector3> pos;
    private Transform[] points;
    private Transform[] holder;
    private float steps = 0f;
    private JobData jobData;


    private void Awake()
    {
        Input.backButtonLeavesApp = true;
        DontDestroyOnLoad(gameObject);
        //setup Dropdown
        List<string> optionData = new List<string>
        {
            "Wave",
            "Ripple",
            "Cylinder",
            "Sphere",
            "Torus"
        };
        dropdown.AddOptions(optionData);

        //setting up slider type
        resRange.wholeNumbers = true;
        timeXMulti.wholeNumbers = false;
        timeZMulti.wholeNumbers = false;
        freqXSine.wholeNumbers = false;
        freqZSine.wholeNumbers = false;
        magXSine.wholeNumbers = false;
        magZSine.wholeNumbers = false;

        //setting up all Sliders min value
        resRange.minValue = 10;
        timeXMulti.minValue = -5f;
        timeZMulti.minValue = -5f;
        freqXSine.minValue = 0f;
        freqZSine.minValue = 0f;
        magXSine.minValue = 0f;
        magZSine.minValue = 0f;

        //setting up all Sliders Max value
        resRange.maxValue = 100;
        timeXMulti.maxValue = 5f;
        timeZMulti.maxValue = 5f;
        freqXSine.maxValue = 5f;
        freqZSine.maxValue = 5f;
        magXSine.maxValue = 5f;
        magZSine.maxValue = 5f;

        //setting up default Slider value
        resRange.value = 50;
        timeXMulti.value = 1f;
        timeZMulti.value = 1f;
        freqXSine.value = 1f;
        freqZSine.value = 1f;
        magXSine.value = 1f;
        magZSine.value = 1f;

        //instantiating 10000 (ten thousand) gameOjects into array
        points = new Transform[10000];
        holder = new Transform[100];
        for (int i = 0; i < 100; i++)
        {
            holder[i] = Instantiate(empty);
        }

        for (int i = 0; i < 10000; i++)
        {
            points[i] = Instantiate(pointPrefab);
            points[i].SetParent(holder[i / 100], false);
        }
        transforms = new TransformAccessArray(points, -1);

        //some preloop calculations
        steps = 2f / resRange.value;
        jobData.modeJ = dropdown.value;                     //set mode about how to draw graph
        jobData.resJ = (int)resRange.value;                 //set resolution
        jobData.posBaseJ = (0.5f * steps) - 1f;             //helps in calculating postion of each item
        jobData.stepsJ = steps;                             //helps in calculating postion and scalling of all items based on resolution
        jobData.piRefJ = Mathf.PI;                           //just referancing PI constent
        jobData.timeXSineJ = timeXMulti.value * Time.time;  //Sine wave Speed on X axis
        jobData.timeZSineJ = timeZMulti.value * Time.time;  //Sine wave Speed on Z axis
        jobData.freqXSineJ = freqXSine.value;               //Sine wave Frequency on X axis
        jobData.freqZSineJ = freqZSine.value;               //Sine wave Frequency on Z axis
        jobData.magXSineJ = magXSine.value;                 //Sine wave Magnitude on X axix
        jobData.magZSineJ = magZSine.value;                 //Sine wave Magnitude on Z axix
        pos = new NativeArray<Vector3>(10000, Allocator.TempJob);
    }

    private void Update()
    {
        fpsNum.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
        
        //calculations for Postions of each GameObjects
        xLoop = new XLoop()
        {
            posJ = pos,                                 //position Shared Native Array
            jobDataJ = jobData                          //passing last frame calculated Job variable Data
        };
        xLoopHandle = xLoop.Schedule((int)(resRange.value * resRange.value), 64);
        xLoopHandle.Complete();

        //setting up Transform of each Gameobjects AFTER XLoop Job comepleted 
        gOpos = new GOPos()
        {
            scaleJ = Vector3.one * steps,               //Size of all Cubes
            pos = pos                                   //position Shared Native Array
        };
        gOHandle = gOpos.Schedule(transforms, xLoopHandle);

        JobHandle.ScheduleBatchedJobs();
    }

    private void LateUpdate()
    {
        //some preloop calculations for next frame
        steps = 2f / resRange.value;
        jobData.modeJ = dropdown.value;                     //set mode about how to draw graph
        jobData.resJ = (int)resRange.value;                 //set resolution
        jobData.posBaseJ = (0.5f * steps) - 1f;             //helps in calculating postion of each item
        jobData.stepsJ = steps;                             //helps in calculating postion and scalling of all items based on resolution
        jobData.timeXSineJ = timeXMulti.value * Time.time;  //Sine wave Speed on X axis
        jobData.timeZSineJ = timeZMulti.value * Time.time;  //Sine wave Speed on Z axis
        jobData.freqXSineJ = freqXSine.value;               //Sine wave Frequency on X axis
        jobData.freqZSineJ = freqZSine.value;               //Sine wave Frequency on Z axis
        jobData.magXSineJ = magXSine.value;                 //Sine wave Magnitude on X axix
        jobData.magZSineJ = magZSine.value;                 //Sine wave Magnitude on Z axix

        resNum.text = resRange.value.ToString("N2");
        freq1Num.text = freqXSine.value.ToString("N2");
        freq2Num.text = freqZSine.value.ToString("N2");
        mag1Num.text = magXSine.value.ToString("N2");
        mag2Num.text = magZSine.value.ToString("N2");
        time1Num.text = timeXMulti.value.ToString("N2");
        time2Num.text = timeZMulti.value.ToString("N2");
        

        gOHandle.Complete();
        pos.Dispose();
        pos = new NativeArray<Vector3>(10000, Allocator.TempJob);
    }

    //when this gameObjec Disables/Inactive
    private void OnDisable()
    {
        gOHandle.Complete();
        transforms.Dispose();
        pos.Dispose();
    }
}