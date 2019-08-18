# Mathf-unity-ECS

It's just me learning new Entity Component System in Unity Game Engine with using Entity Package v0.0.12 pre-32 and its dependencies.

![Mathf-unity-ECS](https://i.imgur.com/3sBU4pz.png)

#### About Project

This project is nothing but just a bunch of Cubes Spawned and Positioned using basic Trigonometry's Sin() and Cos() Functions to make Shapes such as Wave grid, Ripple grid, Cylinder, Sphere and Torus. However, you can learn ECS behind the scene such as the simplest way to spawn Entity from GameObject prefab, Creating/Adding Components and the most fun part is Working with C# Jobs to make systems that run across CPU cores. Feel free to Learn, Clone, Modify, Publish, Sell this project as you wish but on your own RISK.

#### known issues:

- On my android device with 10 resolution FPS drops, but with 50 resolution improve FPS....... :tableFlip:
- Entities won't destroy on resolution decrease (not a bug, it's just me being lazy), they just move out of the viewport if not in use, - which is the cause for the above issue.
- GUI you see are all ugly Gameobjects :eww!! yucks:
- And also yeah GUI is hard to select and use.
- Size of builds are large due to disabled Stripping unused systems, upon enabling ECS won't run.
- Currently, using Mono to compile builds, IL2CPP compilation crashes for some unknown reason.

note: i did bugreport and waiting for its response, i'll update soon

#### History of Project (dont read)

I started this project about more than 3 years ago as part of my actual basic unity learning from [Catlikecoding](https://catlikecoding.com/unity/tutorials/) ( <— this tutorials is a rare gemstone I can ever find on the Internet) at that time this project was divided into several sub Projects as followed by tutorial with different trigonometric functions Shapes to position cubes. *But learning isn't just following someone*, so I continued this project and merged all those into one project and added GUI to make switchable between different Shapes, all those cubes were about just an array of 2500 GameObject and all Trig calculations were done on Update() function using UnityEngine.Mathf, that's why the project title is Mathf. 

After two years Off of this project, I was eager to come back and Introduce Unity's new ECS very early preview packages, took me 15 days to research and lean Undocumented ECS features and scripting, after many failures and its billion of bugs, I somehow managed to make it run on Editor (building binary was not possible that time due to unity strip unused file from assets and other bugs), it took about 10 scripts to make it run ECS very manually just to spawn primitive cube, mean not much support from all internal classes and no Monobehavior, so scripts needed to be run with Scene start, now today, its just 4 simplest scripts including Monobehaviour to spawn Entities, and so this way this Project grew Smaller and Smaller each time ECS updated (*pun intended*).

#### Results

With the GameObject method, I used 3 years ago, I used to get 10 FPS on my old laptop with 2500 GameObjects calculated postions on *Update()* for each on Editor play mode.
But today my old laptop can easily handle 10000 entities with 60 FPS and my new Ryzen 2200G PC can handle 250000 entities with 60FPS on Editor play mode. 

##Further Plans

-to make dynamically spawn/despawn entities, so on resolution 10, only 100 entities should stay alive on the scene, so I can increase the max resolution to 500 (*250000 entities*) and improved pullback resolution function to automatically reduce resolution if FPS < 4.

-waiting for unity UI to be implemented on ECS (*idk how they can do this but I think possible*).

**Note: until Unity ECS comes out of beta, it's going to be changed continuously.**
