using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace MathfECS
{
    public class Spawner : MonoBehaviour
    {
        //Spawner is Responsible for creating and Destroying Entities based on Resolution calculated on Last frame in GameObj class.



        //  ▀▄▀▄▀▄ Inspactor Variables ▄▀▄▀▄▀

        public Mesh mesh;
        public Material material;
        public int maxRes = 100;


        //  ▀▄▀▄▀▄ Private Variables ▄▀▄▀▄▀

        private EntityManager entityManager;
        private EntityArchetype archetype;
        private NativeArray<Entity>[] entityArrays;
        private int currentRes, x, lastIndex;
        private bool destroyed;



        //  ▀▄▀▄▀▄ Core Functions ▄▀▄▀▄▀


        private void Start()
        {

            //EntityManager is Big Daddy of Entities
            entityManager = World.Active.EntityManager;
            //Archetype is list of components
            archetype = entityManager.CreateArchetype(
                typeof(Translation),
                typeof(Scale),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(Index)
            );
            //entityArrays is Container of Entities (dead and live, currently all dead)
            //but in this case, entityArrays is an Array of Entity Array (2d Array), each element in entityArrays represents each resolution count
            //for example at 10 resolution, 10 entityArrays of entities get created.
            //but first initializing all 100/500 arrays. (depends on runtime platform)
            entityArrays = new NativeArray<Entity>[maxRes];
            for (int x = 0, y = 1; x < entityArrays.Length; x++, y++)
            {
                //number of elements in each entityArray is based on currentIndex^2 minus lastIndex^2.
                //example: at resolution 10, 100 need to entities spawn.
                //so first entityArrays contains 1 entity
                //second entityArrays contains 3 entities ((second * second = 4) minus (first * first = 1))
                //third entityArrays contains 5 entities ((third * third = 9) minus (second * second = 4))
                //and so on...
                // i could just +2 on each iteration, but i am trying making this logical to understand.
                entityArrays[x] = new NativeArray<Entity>(y * y - x * x, Allocator.Persistent);
            }
            currentRes = lastIndex = 0;
        }

        private void Update()
        {
            //if resolution is increased by user, then create that much Entities 
            if (!destroyed)
            {
                if (currentRes < GameObj.baseData.res)
                {
                    entityManager.CreateEntity(archetype, entityArrays[currentRes]);
                    for (x = 0; x < entityArrays[currentRes].Length; x++)
                    {
                        entityManager.SetSharedComponentData(entityArrays[currentRes][x], new RenderMesh
                        {
                            mesh = mesh,
                            material = material
                        });
                        entityManager.SetComponentData(entityArrays[currentRes][x], new Index
                        {
                            index = lastIndex++
                        });
                    }
                    GameObj.entityCount = lastIndex;
                    currentRes++;
                }
                //else if resolution is decreased by user, then destroy thats much entities 
                else if (currentRes > GameObj.baseData.res)
                {
                    entityManager.DestroyEntity(entityArrays[--currentRes]);
                    GameObj.entityCount = lastIndex -= entityArrays[currentRes].Length;
                }
            }
        }



        //  ▀▄▀▄▀▄ Escaping Functions ▄▀▄▀▄▀


        //at the end we need to delete all array or Entity arrays before application closed
        private void OnDisable()
        {
            if (entityArrays != null && !destroyed)
            {
                foreach (var item in entityArrays)
                {
                    entityManager.DestroyEntity(item);
                    item.Dispose();
                }
                destroyed = true;
            }
        }
        private void OnDestroy()
        {
            if (entityArrays != null && !destroyed)
            {
                foreach (var item in entityArrays)
                {
                    entityManager.DestroyEntity(item);
                    item.Dispose();
                }
                destroyed = true;
            }
        }
        private void OnApplicationQuit()
        {
            if (entityArrays != null && !destroyed)
            {
                foreach (var item in entityArrays)
                {
                    entityManager.DestroyEntity(item);
                    item.Dispose();
                }
                destroyed = true;
            }
        }
    }
}