using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using TMPro;

namespace MathECS
{
    public class GameObj : MonoBehaviour
    {

        //  ----Unity Inspactor Variables----

        public TextMeshProUGUI[] textNums;
        public GameObject Prefab, LoadingPanel;
        public int resolution = 100;
        public Camera thiscamera;


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
        public static FreqSin freq;
        public static MagnSin magn;
        public static TimeSin time;
        public static PI pi;


        //  ----Private Variables----

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
        private byte fpscount = 0;
        private float fpsAvg = 0;
        private float fpsSum = 0;
        private bool fpsClock = true;
        



        //  ----Init Functions----

        private void Awake()
        {
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            Caching.ClearCache();
            Input.backButtonLeavesApp = true;
        }

        private void Start()
        {
            thiscamera.clearFlags = CameraClearFlags.SolidColor;

            Entity prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
            EntityManager entityManager = World.Active.EntityManager;
            int count = resolution * resolution;

            float3 zero = float3.zero;
            float3 one = new float3(1f, 1f, 1f);

            for (int x = 0; x < count; x++)
            {
                var instance = entityManager.Instantiate(prefab);


                entityManager.SetComponentData(instance, new Translation { Value = zero });
                entityManager.AddComponentData(instance, new Index { index = x });
                entityManager.AddComponentData(instance, new NonUniformScale { Value = one });
            }

            LoadingPanel.SetActive(false);
            PreLoopCalc();
            pi.value = math.PI;
            UIinit();

        }

        private void UIinit()
        {
            //setting up default Slider value
            ResRange = 10;
            TimeXMulti = 1f;
            TimeZMulti = 1f;
            FreqXSine = 1f;
            FreqZSine = 1f;
            MagXSine = 1f;
            MagZSine = 1f;
        }


        //  ----Updating Functions----


        private void Update()
        {
            PreLoopCalc();

            fpscount++;
            fpsSum += 1f / Time.unscaledDeltaTime;
            if ((int)Time.time % 2 != 0 && fpsClock)
            {
                fpsClock = false;
                fpsAvg = fpsSum / fpscount;
                fpscount = 0;
                fpsSum = 0f;
            }
            else if ((int)Time.time % 2 == 0 && !fpsClock)
            {
                fpsClock = true;
                fpsAvg = fpsSum / fpscount;
                fpscount = 0;
                fpsSum = 0f;
            }
        }

        //Call updating fuctions
        private void LateUpdate()
        {
            if (fpsAvg < 5 && !fpsClock)
            {
                ResRange = 10;
                Debug.Log("fallback resoution");
            }
            TextUpdate();
        }

        //Job calculations for next frame
        private void PreLoopCalc()
        {
            baseData.mode = (byte)Mode;                 //set mode about how to draw graph
            baseData.res = (byte)math.clamp(ResRange, 10, 100);      //set resolution
            baseData.size = 2f / baseData.res;                     //helps in calculating postion and scalling of all items based on resolution
            baseData.posBase = (0.5f * baseData.size) - 1f;        //helps in calculating postion of each item
            time.x = TimeXMulti * Time.time;    //Sine wave Speed on X axis
            time.z = TimeZMulti * Time.time;    //Sine wave Speed on Z axis
            freq.x = FreqXSine;                   //Sine wave Frequency on X axis
            freq.z = FreqZSine;                   //Sine wave Frequency on Z axis
            magn.x = MagXSine;                    //Sine wave Magnitude on X axix
            magn.z = MagZSine;                    //Sine wave Magnitude on Z axix
        }

        //UI Texts update
        private void TextUpdate()
        {
            textNums[0].text = baseData.res.ToString("N2");
            textNums[1].text = freq.x.ToString("N2");
            textNums[2].text = freq.z.ToString("N2");
            textNums[3].text = magn.x.ToString("N2");
            textNums[4].text = magn.z.ToString("N2");
            textNums[5].text = TimeXMulti.ToString("N2");
            textNums[6].text = TimeZMulti.ToString("N2");
            textNums[7].text = stringsFrom00To99[math.clamp((int)fpsAvg, 0, 99)];
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
}