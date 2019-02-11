Hello ! and thank you :)

THIS UPDATE:

- Fixed Lighting problems.
- Optimization of the Shader UI (Now is more compact, easy to understand and to work with).

- New shader and script to mask the water, so you can avoid the water mesh going through objects, like boats for example. 
  The shader path is: "DCG/Masked/Mask", and the script is "WaterCutoutMask.cs"

- HOT FEATURE: physics supported !
- HOT FEATURE: Buoyancy supported !

---------------------
Quick instructions:
---------------------

**You may want to use some of the example meshes depending on the spec target you are doing your game, there are low poly and highpoly planes included :) **

Displacement settings:

- Enable Displacement: Boolean to enable or disable the displacement of the mesh.

- Noise Intensity slider controls the contrast of the generated noise, so 0 is no displacement and 1 is full displacement, 
  this is multiplied by the current slider value. In simple words, it is the intensity of the displacement.

- Noise scale slider controls the tiling of the generated noise, for more natural results you should use low values.


Physics settings:

- Enable Collision: Boolean that controls the mesh collision.

- Collision ignore list: this is an array of int values, so here you should put all the layers index's that you want to avoid collision
   (because the water mesh needs to be non-trigger, so it can't be convex to have a decent calculation of the mesh vertices).
   By default the layer 0 is ignored, so anything in the default layer will have NO collision with the water plane. For example if you have created a
   player layer in the 8th layer, you should add a new value in the collision ignore array with the int "8".

- Refresh Mesh: Press this boolean to apply any changes you've made.


--------------------------------
MATERIAL INSTRUCTIONS:
--------------------------------
Tessellation: Level of the tessellation in the mesh(only in the tessellation shader).

Water Map: This input is made to receive a water texture, which consist in a RGB texture, where R and B channels are used to construct the normal map and G to displace the mesh.

Water Parameters:
	X : This controls the tiling of the water.
	Y : The speed of the water.
	Z : Displacement Intensity.
	W : Offset of the displacement.

Wave Direction: Direction of the waves, "0.5" being neutral direction with both directions being overlaped.

Normal Power: This controls the intensity of the constructed normal.

Normal Detail Power: This controls the intensity of the detailed normal contribution.

Gloss: Controls how glossy will be the water.

Enable Planar Reflection: Enable this to use the "_ReflectionTex" input of the shader, otherwise it will use reflection probes.

Invert Reflection: If you experience flipped reflection enable this bool.

Water Color: Controls the tint of the water. light values works better for a realistic water depth calculation.

Water Density: This is a density factor to affect the water depth.

Foam map: The texture input to be applied as foam.

Foam Parameters:  
	X : This controls the distance of the foam.
	Y : Foam Intensity.
	Z : Foam tiling
	W : Foam Speed

Render Queue: Please try to maintin this at 2600 to avoid overlaping problems unless you know what you are doing, also with the water cut out mask.


********************
***IMPORTANT THINGS***
********************

**This Shader is made to be used with Deferred rendering in Linear Color space, Otherwise you may experience flipped normals.**

**This water shader has been made thinking of modern realistic games, where the rendering method is the physically based rendering. So in order to get the water looks good, you may want to stick to that rendering pipeline.**

**Use HDR on your camera if you experience flipped reflections(with the example MirrorReflection, from the UnifyWiki), or invert it in the material.

**For mobile projects i do NOT recommend you to use the script reflection with a resolution above 64px and a high poly mesh, Otherwise you will experiencie performance issues.**

**Example materials and Prefabs are included in the package**



- Buoyancy script NEED some collider to work, otherwise it will add a meshcollider in the start, i didn't do this automatic because there is a lot of collider options, some may want to use mesh collider, others box, sphere, etc... So you have to add it by yourself.

- To get better results you should work with deffered rendering mode in linear color space, the overall look of the shader is done in a way that looks good on linear color space. But if you are using gamma, just tweak your custom material as needed to get a desired result.


If you have some problem, advice or just want to say something, contact me through my social media:

Facebook:	https://www.facebook.com/deadlycrow.games
Gmail:    	Deadlykrow@gmail.com
Twitter: 	@DeadlyCrowGames

PD: Are you making a game with any of my assets? Please contact me! ;)

Enjoy :) !


Please do not forget you are not allowed to re-distribute this asset in any way, thats an illegal move, also keep in mind this is my work and my source of living, thanks :) !
