using UnityEngine;
using Unity.Mathematics;
using TMPro;
using UnityEngine.UI;

namespace MathfECS
{
    public class GameObj : MonoBehaviour
    {
        //GameObj is responsible for all Gui Controls.
        //GameObj also pre calculates data variables for ShapeJobs (IComponentData) class.
        //GameObj also calculates screen fps.


        //  ▀▄▀▄▀▄ Inspactor Variables ▄▀▄▀▄▀

        public TextMeshProUGUI[] textNums;
        public TextMeshProUGUI infoText;
        public GameObject LoadingPanel;
        public Slider slider;
        public int resolution = 100;


        //  ▀▄▀▄▀▄ Public properties ▄▀▄▀▄▀

        public int Mode { get; set; }
        public float ResRange { get; set; }
        public float TimeXMulti { get; set; }
        public float TimeZMulti { get; set; }
        public float FreqXSine { get; set; }
        public float FreqZSine { get; set; }
        public float MagXSine { get; set; }
        public float MagZSine { get; set; }
        public static BaseData baseData;
        public static TransformData transformData;
        public static FreqSin freq;
        public static MagnSin magn;
        public static TimeSin time;
        public static int entityCount;


        //  ▀▄▀▄▀▄ Private Variables ▄▀▄▀▄▀

        private readonly string[] fpsStringsFrom00To99 = {
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
        private byte fpscount, fpsFallbackCount;
        private float fpsAvg, fpsSum, fpsTimer, infoTextTimer;
        private readonly string lowFpsAlert =
        @"<!!> Alert Low FPS <!!>
         Fallback to Resolution 10";
        private bool infoTextDispayed;



        //  ▀▄▀▄▀▄ Core Func ▄▀▄▀▄▀


        private void Awake() => Input.backButtonLeavesApp = true;

        private void Start()
        {
            slider.maxValue = resolution;

            //setting up default Slider value
            slider.value = 40;
            TimeXMulti = 1f;
            TimeZMulti = 1f;
            FreqXSine = 1f;
            FreqZSine = 1f;
            MagXSine = 1f;
            MagZSine = 1f;

            fpsAvg = 0f;
            fpsSum = 0f;
            fpsTimer = 0f;

            PreLoopCalc();
        }



        //  ▀▄▀▄▀▄ Updating Func ▄▀▄▀▄▀


        private void Update()
        {
            //counts screen Fps based on average of one second
            fpscount++;
            fpsSum += 1f / Time.unscaledDeltaTime;
            if (fpsTimer > 0f) fpsTimer -= Time.unscaledDeltaTime;
            else
            {
                fpsAvg = fpsSum / fpscount;
                fpscount = 0;
                fpsSum = 0f;
                fpsTimer = 1f;
            }

            //counts if Fps is low for 5 frames, Fallback to resolution 10
            if (fpsAvg < 5)
            {
                if (fpsFallbackCount++ > 5)
                {
                    if (baseData.res > 10) slider.value--;
                    fpsFallbackCount = 0;
                    if (!infoTextDispayed)
                    {
                        infoText.text = lowFpsAlert;
                        infoTextDispayed = true;
                        infoTextTimer = 5f;
                    }
                }
            }
            else
            {
                fpsFallbackCount = 0;
            }
        }

        private void LateUpdate()
        {
            PreLoopCalc();
            TextUpdate();
        }

        //Job calculations for next frame based of user input
        private void PreLoopCalc()
        {
            baseData.mode = Mode;                                   //mode of shape to draw graph
            baseData.res = math.clamp((int)ResRange, 10, resolution);    //set current resolution
            transformData.size = 2f / baseData.res;                 //scaling based on resolution
            transformData.posBase = (0.5f * transformData.size) - 1f;   //postion based on resolution
            time.x = TimeXMulti * Time.time;                        //Sine wave Speed on X axis
            time.z = TimeZMulti * Time.time;                        //Sine wave Speed on Z axis
            freq.x = FreqXSine;                                     //Sine wave Frequency on X axis
            freq.z = FreqZSine;                                     //Sine wave Frequency on Z axis
            magn.x = MagXSine;                                      //Sine wave Magnitude on X axis
            magn.z = MagZSine;                                      //Sine wave Magnitude on Z axis
        }

        //UI Texts update with atleast two decimal
        private void TextUpdate()
        {
            textNums[0].text = baseData.res.ToString("N2");
            textNums[1].text = freq.x.ToString("N2");
            textNums[2].text = freq.z.ToString("N2");
            textNums[3].text = magn.x.ToString("N2");
            textNums[4].text = magn.z.ToString("N2");
            textNums[5].text = TimeXMulti.ToString("N2");
            textNums[6].text = TimeZMulti.ToString("N2");
            textNums[7].text = fpsStringsFrom00To99[math.clamp((int)fpsAvg, 0, 99)];
            textNums[8].text = entityCount.ToString();

            //if any infotext displayed, text get wiped out in 5 seconds
            if (infoTextDispayed)
            {
                if (infoTextTimer > 0f) infoTextTimer -= Time.unscaledDeltaTime;
                else
                {
                    infoText.text = null;
                    infoTextDispayed = false;
                }
            }
        }
    }
}