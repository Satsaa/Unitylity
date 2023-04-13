# RenderImages

## Purpose

Using the tools provided under `Unitylity.Systems.RenderImages` you can create UI images which display a rendered view produced by a RenderObject (player character, inventory item, etc.). RenderObjects can be pooled and shared automatically, which will reduce GPU load. 

The system handles multiple images using the same RenderObject automatically, using highest resolution and widest aspect ratio, and configures the images to fit to the aspect ratio without strecthing.

Note that having many Cameras rendering during a frame will have a big performance hit even when rendering nothing due to culling and other processes I don't have control of.

## RenderImage

RenderImages are RawImages which you can add to the UI like other Images.  

For a RenderImage to show rendered content, a RenderObject must be set for the image. No other values need to be set, but a placeholder texture might be useful as the rendered content is only shown in play mode. Also, a custom material can be set if necessary.

RenderImages do not themself control Unitylityh of the properties of the rendered content. Resolution is determined by the size of the image on screen multiplied by the resolution scale, and is automatically updated when the image size changes.

When multiple RenderImages are sharing the same RenderObject (the RenderObject is set to share instances) a resolution is used that gives all RenderImages at least 1:1 pixel to screen ratio (or ResolutionScale:1). Different ratios are also taken in to consideration. To prevent crashes or performance degradation due to large textures, a maximum resolution setting is available.

<details>
  <summary>Resolution Examples (click to expand)</summary>  

```
  (Size of RenderImage on screen)

  A: 100x100  
  B: 100x500  

               (100/100 vs 100/500)
  Max Vert Ratio: 1 vs 0.2  -> 1
  Max height: 100 vs 500    -> 500
    (500*1, 500)
  Out: 500x500


  A: 100x100  
  B: 10x50  

  Max Vert Ratio: 1 vs 0.2  -> 1
  Max height: 100 vs 50     -> 100
  Out: 100x100


  A: 100x100  
  B: 50x10 (cannot be 50x10 because A needs higher resolution)

  Max Vert Ratio: 1 vs 5  -> 5
  Max height: 100 vs 50   -> 100
  Out: 500x100


  A: 500x500 (resolution scale = 0.5)  
  B: 100x100  

  Max Vert Ratio: 1 vs 1  -> 1
  Max height: 500 vs 10   -> 500
  Out: 500x500


  A: 100x100 
  B: 1000x1  

  Max Vert Ratio: 1 vs 100  -> 1000
  Max height: 100 vs 1      -> 100
  Out: 100000x100 (be wary of thin images!)

```

</details>

<details>
  <summary>Example images</summary>  

![RenderImageComponent](/Res/RenderImages/RenderImageComponent.png)
  
![RenderImageUIExample](/Res/RenderImages/RenderImageUIExample.png)
  
</details>



## RenderObject

RenderObject is a Component handling the rendering for RenderImage(s). RenderObjects are to be created as prefabs. A Camera in the prefab will be used for rendering. The Camera will render everything in view, so a dedicated layer for RenderObjects are recommended.

When any RenderImage becomes active (is created) an instance for it is created based on the RenderObject. In the case of sharing being enabled, an already existing instance is used if possible. If pooling is enabled, a pooled instance is used if possible.

Depending on the update setting, rendering happens only once to initialize the texture (and when image dimensions change), on each frame, or manually in code.

The Camera within the prefab will dictate how the image is rendered:
  * To have a transparent background you need to set clear color to transparent.
  * A culling distance below the spacing distance of objects will prevent seeing other objects.
  * A dedicated layer for RenderObjects is recommended so other world objects are not possibly rendered.
  * Disabling the opaque and depth texture and other settings will reduce render overhead massively.

<details>
  <summary>Example image</summary>  

![RenderObject](/Res/RenderImages/RenderObject.png)
  
</details>

## RenderObjects

RenderObjects is a Singleton which is needed for RenderImages and RenderPrefabs to work. Put a GameObject with this component anywhere in a persistent scene (preferrably far away from the main areas to avoid any issues).

The distance value will be the minimum distance between instantiated RenderObjects, which are placed in a spiral around the GameObject as they are created. 

<details>
  <summary>Example image</summary>  

![RenderObjects](/Res/RenderImages/RenderObjects.png)
  
</details>
