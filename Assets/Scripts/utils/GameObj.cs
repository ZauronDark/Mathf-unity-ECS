using UnityEngine.UI;
using UnityEngine;
using Unity.Mathematics;

public class GameObj : MonoBehaviour
{

    //  ----Unity Inspactor Variables----


    public Text[] texts;


    //  ----Public Variables----


    public int Mode { get; set; }
    public float ResRange { get; set; }
    public float TimeXMulti { get; set; }
    public float TimeZMulti { get; set; }
    public float FreqXSine { get; set; }
    public float FreqZSine { get; set; }
    public float MagXSine { get; set; }
    public float MagZSine { get; set; }
    public static BaseData baseData;
    public static SinXData sinX;
    public static SinZData sinZ;


    //  ----Private Variables----


    private FpsData fpsData;
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
    

    private void Start()
    {
        PreLoopCalc();
        sinX.pi = Mathf.PI;
        UIinit();
    }

    private void UIinit()
    {
        //setting up default Slider value
        ResRange = 50;
        TimeXMulti = 1f;
        TimeZMulti = 1f;
        FreqXSine = 1f;
        FreqZSine = 1f;
        MagXSine = 1f;
        MagZSine = 1f;
    }

    //  ----Updating Functions----


    //Call updating fuctions
    private void LateUpdate()
    {
        FpsUpdate();
        PreLoopCalc();
        TextUpdate();
    }

    //Calculate FPS every 2 sec
    private void FpsUpdate()
    {
        fpsData.fpscount++;
        fpsData.fpsSum += 1f / Time.unscaledDeltaTime;
        if ((int)Time.time % 2 == 0 && fpsData.fpsClock)
        {
            fpsData.fpsClock = false;
            fpsData.fpsAvg = math.clamp((int)(fpsData.fpsSum / fpsData.fpscount), 0, 99);
            fpsData.fpscount = 0;
            fpsData.fpsSum = 0f;
            if (fpsData.fpsAvg < 5)
            {
                ResRange = 10;
                Debug.Log("fallback resoution");
            }
        }
        else if ((int)Time.time % 2 != 0 && !fpsData.fpsClock)
        {
            fpsData.fpsClock = true;
        }


    }

    //Job calculations for next frame
    private void PreLoopCalc()
    {
        baseData.mode = Mode;                 //set mode about how to draw graph
        baseData.res = (int)math.clamp(ResRange, 10, 200);             //set resolution
        baseData.steps = 2f / baseData.res;                         //helps in calculating postion and scalling of all items based on resolution
        baseData.posBase = (0.5f * baseData.steps) - 1f;         //helps in calculating postion of each item
        sinX.timeXSine = TimeXMulti * Time.time;  //Sine wave Speed on X axis
        sinX.freqXSine = FreqXSine;               //Sine wave Frequency on X axis
        sinX.magXSine = MagXSine;                 //Sine wave Magnitude on X axix
        sinZ.timeZSine = TimeZMulti * Time.time;  //Sine wave Speed on Z axis
        sinZ.freqZSine = FreqZSine;               //Sine wave Frequency on Z axis
        sinZ.magZSine = MagZSine;                 //Sine wave Magnitude on Z axix
    }

    //UI Texts update
    private void TextUpdate()
    {
        texts[0].text = baseData.res.ToString("N2");
        texts[1].text = sinX.freqXSine.ToString("N2");
        texts[3].text = sinX.magXSine.ToString("N2");
        texts[2].text = sinZ.freqZSine.ToString("N2");
        texts[4].text = sinZ.magZSine.ToString("N2");
        texts[5].text = TimeXMulti.ToString("N2");
        texts[6].text = TimeZMulti.ToString("N2");
        texts[7].text = stringsFrom00To99[fpsData.fpsAvg];
    }


    //  ----Escaping Functions----


    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
