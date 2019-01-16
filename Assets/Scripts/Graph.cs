using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    //ref var
    public Transform pointPrefab;
    public Transform empty;
    public Dropdown dropdown;
    public Text[] texts;

    //public properties
    public Slider resRange;
    public Slider timeXMulti;
    public Slider timeZMulti;
    public Slider freqXSine;
    public Slider freqZSine;
    public Slider magXSine;
    public Slider magZSine;

    //private var
    private JobHandle xLoopHandle;
    private JobHandle gOHandle;
    private XLoop xLoop;
    private GOPos gOpos;
    private TransformAccessArray points;
    private NativeArray<float3> pos;
    private TransformAccessArray holder;
   // private Transform[] pointsObj;
    private BaseData baseData;
    private SinXData sinX;
    private SinZData sinZ;
    private FpsData fpsData;
    private float steps = 0f;
    private readonly string[] stringsFrom00To99 = {
        "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
        "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
        "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
        "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
        "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
        "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
        "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
        "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
        "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
        "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
    };

    private void Awake()
    {
        Transform temp;
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
        //pointsObj = new Transform[10000];
        holder = new TransformAccessArray(0, -1);
        points = new TransformAccessArray(0, -1);

        for (int i = 0; i < 100; i++)
        {
            holder.Add(Instantiate(empty));
        }

        for (int i = 0; i < 10000; i++)
        {
            temp = Instantiate(pointPrefab);
            temp.SetParent(holder[i / 100], false);
            points.Add(temp);
        }
        
        //some prejob calculations
        steps = 2f / resRange.value;
        baseData.mode = dropdown.value;             //set mode about how to draw graph
        baseData.res = (int)resRange.value;               //set resolution
        baseData.posBase = (0.5f * steps) - 1f;     //helps in calculating postion of each item
        baseData.steps = steps;                     //helps in calculating postion and scalling of all items based on resolution
        sinX.pi = Mathf.PI;                         //just referancing PI constent
        sinX.timeXSine = timeXMulti.value * Time.time;    //Sine wave Speed on X axis
        sinX.freqXSine = freqXSine.value;                 //Sine wave Frequency on X axis
        sinX.magXSine = magXSine.value;                   //Sine wave Magnitude on X axix
        sinZ.timeZSine = timeZMulti.value * Time.time;    //Sine wave Speed on Z axis
        sinZ.freqZSine = freqZSine.value;                 //Sine wave Frequency on Z axis
        sinZ.magZSine = magZSine.value;                   //Sine wave Magnitude on Z axix
        pos = new NativeArray<float3>(10000, Allocator.TempJob);
    }

    private void Update()
    {

        //calculations for Postions of each GameObjects
        xLoop = new XLoop()
        {
            posJ = pos,                                 //position Shared Native Array
            baseDataj = baseData,                          //passing last frame calculated Job variable Data
            sinXj = sinX,
            sinZj = sinZ,
        };
        xLoopHandle = xLoop.Schedule((int)(resRange.value * resRange.value), 64);
        xLoopHandle.Complete();

        //setting up Transform of each Gameobjects AFTER XLoop Job comepleted 
        gOpos = new GOPos()
        {
            scaleJ = new float3(1f, 1f, 1f) * steps,               //Size of all Cubes
            pos = pos                                   //position Shared Native Array
        };
        gOHandle = gOpos.Schedule(points, xLoopHandle);

        JobHandle.ScheduleBatchedJobs();

        fpsData.fpscount++;
        fpsData.fpsSum += 1f / Time.unscaledDeltaTime;
        if ((int)Time.time % 2 == 0 && fpsData.fpsClock)
        {
            fpsData.fpsClock = false;
            fpsData.fpsAvg = math.clamp((int)(fpsData.fpsSum / fpsData.fpscount), 0, 99);
            fpsData.fpscount = 0;
            fpsData.fpsSum = 0f;
        }
        else if ((int)Time.time % 2 != 0 && !fpsData.fpsClock)
        {
            fpsData.fpsClock = true;
        }
    }

    private void LateUpdate()
    {
        //some preloop calculations for next frame
        steps = 2f / resRange.value;
        baseData.mode = dropdown.value;             //set mode about how to draw graph
        baseData.res = (int)resRange.value;               //set resolution
        baseData.posBase = (0.5f * steps) - 1f;     //helps in calculating postion of each item
        baseData.steps = steps;                     //helps in calculating postion and scalling of all items based on resolution
        sinX.timeXSine = timeXMulti.value * Time.time;    //Sine wave Speed on X axis
        sinX.freqXSine = freqXSine.value;                 //Sine wave Frequency on X axis
        sinX.magXSine = magXSine.value;                   //Sine wave Magnitude on X axix
        sinZ.timeZSine = timeZMulti.value * Time.time;    //Sine wave Speed on Z axis
        sinZ.freqZSine = freqZSine.value;                 //Sine wave Frequency on Z axis
        sinZ.magZSine = magZSine.value;                   //Sine wave Magnitude on Z axix

        texts[0].text = resRange.value.ToString("N2");
        texts[1].text = freqXSine.value.ToString("N2");
        texts[2].text = freqZSine.value.ToString("N2");
        texts[3].text = magXSine.value.ToString("N2");
        texts[4].text = magZSine.value.ToString("N2");
        texts[5].text = timeXMulti.value.ToString("N2");
        texts[6].text = timeZMulti.value.ToString("N2");
        texts[7].text = stringsFrom00To99[fpsData.fpsAvg];

        gOHandle.Complete();
        pos.Dispose();
        pos = new NativeArray<float3>(10000, Allocator.TempJob);
    }

    //when this gameObjec Disables/Inactive
    private void OnDisable()
    {
        gOHandle.Complete();
        points.Dispose();
        holder.Dispose();
        pos.Dispose();
    }
}