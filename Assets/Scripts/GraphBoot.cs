//using Unity.Entities;
//using Unity.Mathematics;
//using Unity.Rendering;
//using Unity.Transforms;
//using UnityEngine;

//public class GraphBoot
//{
//    //  ----Private Variables----


//    private static EntityArchetype CubeArchetype;
//    private static RenderMesh cubeRenderer;
//    private static EntityManager entityManager;
//    private static Entity cube;


//    //  ----initial functions----


//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//    public static void Awake()
//    {
//        //create entities
//        entityManager = World.Active.GetOrCreateManager<EntityManager>();

//        CubeArchetype = entityManager.CreateArchetype(
//            typeof(Translation),
//            typeof(Index),
//            typeof(NonUniformScale),
//            typeof(Rotation)
//            );

//        //other game startup peram
//        Input.backButtonLeavesApp = true;
//    }

//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//    public static void Start()
//    {
//        cubeRenderer = Object.FindObjectOfType<RenderMeshProxy>().Value;
//        PopulateCubles();
//    }

//    private static void PopulateCubles()
//    {
//        for (int i = 0; i < 40000; i++)
//        {
//            cube = entityManager.CreateEntity(CubeArchetype);
//            entityManager.SetComponentData(cube, new Translation { Value = float3.zero });
//            entityManager.SetComponentData(cube, new Index { index = i });
//            entityManager.SetComponentData(cube, new NonUniformScale { Value = new float3(1f, 1f, 1f) });
//            entityManager.AddSharedComponentData(cube, cubeRenderer);
//        }
//    }
//}
