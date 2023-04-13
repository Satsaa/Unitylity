
# Unitylity  

<p>
    <a href="https://github.com/satsaa/Unitylity/LICENSE" alt="License">
        <img src="https://img.shields.io/github/license/satsaa/unitylity" />
    </a>
    <a href="https://github.com/Satsaa/Unitylity/releases" alt="Releases">
        <img src="https://img.shields.io/github/v/release/satsaa/unitylity" />
    </a>
    <a href="https://github.com/Satsaa/Unitylity/commits/master" alt="Commits">
        <img src="https://img.shields.io/github/last-commit/satsaa/unitylity" />
    </a>
    <a href="https://github.com/satsaa/Unitylity" alt="Lines of code">
        <img src="https://img.shields.io/tokei/lines/github/satsaa/Unitylity" />
    </a>
</p>


A library of utilities, tools, classes etc. for Unity.  

This library is developed alongside the main projects. Later this repository will be updated with those changes.  

You can expect massive breaking changes, aggressive removal of code deemed unworthy, lack of versioning, all the good stuff.  

As the library evolves it will become more stable in structure.

## Features  

###### You can hide Components in the project settings.

#### See the dedicated Readmes for systems

[Unitylity.Systems.RenderImages](../master/Unitylity/Scripts/Unitylity.Systems/Unitylity.Systems.RenderImages)  
[Unitylity.Systems.Input](../master/Unitylity/Scripts/Unitylity.Systems/Unitylity.Systems.Input)  
[Unitylity.Systems.Interaction](../master/Unitylity/Scripts/Unitylity.Systems/Unitylity.Systems.Interaction)  
[Unitylity.Systems.Camera](../master/Unitylity/Scripts/Unitylity.Systems/Unitylity.Systems.Camera)  
[Unitylity.Systems.Lang](../master/Unitylity/Scripts/Unitylity.Systems/Unitylity.Systems.Lang)  
[Unitylity.Systems.Menus](../master/Unitylity/Scripts/Unitylity.Systems/Unitylity.Systems.Menus)  
[Unitylity.Systems.Popups](../master/Unitylity/Scripts/Unitylity.Systems/Unitylity.Systems.Popups)  

#### Short descriptions of items

```cs
> Graphics
  > Shaders
    > Functions
      CalculateLevelOfDetail.hlsl // Texture LOD (mipmap level) calculator
      SimplexNoise3D.hlsl         // 3D Simplex noise generator
  > ShaderGraph
    > SubGraphs
      Grid                        // Grid generator node
      LOD                         // Texture LOD (mipmap level) calculation node
      SimplexNoise3D              // 3D Simplex noise generator node
> Scripts
  > Unitylity
    . Deferred                    // Structure used for execution of function in using statements
    . Stopwatch                   // IDisposable stopwatch for measuring execution time in a using statement
    > Unitylity.Collections
      . CircularArray             // Simple array implementation of a circular buffer
      . OrderedList               // Automatically sorted list
      . SafeList                  // Equivalent of List<T> but allows reliable enumeration of lists which may change during the enumeration
      . SerializedStack           // Implementation of Stack<T> which supports Unity serialization
    > Unitylity.Components
      . CollisionTracker          // Stores information about objects that are currently colliding with the GameObject
      . OnCollision               // UnityEvents fired when a collision starts, remains and ends
      . OnCollision2D             // 2D equivalent of OnCollision
      . OnInput                   // UnityEvents for various user inputs
      . OnTrigger                 // UnityEvents fired when an object enters, stays or leaves the trigger
      . OnTrigger2D               // 2D equivalent of OnTrigger
      . Tags                      // Experimental multi-tag system for GameObjects
      . TransformHistory          // Stores Transform info each physics update
      . VirtualLayoutGroup        // Layout group which automatically removes items outside the rect of the list. Useful for large lists.
      > Unitylity.Components.Extended
        . ExtendedUIBehaviour     // UIBehaviour with cached rectTransform property
        . Singleton               // Basic singleton implementation based on MonoBehaviour
        . UISingleton             // Basic singleton implementation based on ExtendedUIBehaviour
    > Unitylity.Data
      . Event                     // Class implementation of an event
      . SceneReference            // Allows easy referencing of scenes
      . SerializedDictionary      // Dictionary which supports Serialization by Unity
      . SerializedFieldInfo       // FieldInfo which supports Serialization by Unity
      . SerializedMemberInfo      // MemberInfo which supports Serialization by Unity
      . SerializedPropertyInfo    // PropertyInfo which supports Serialization by Unity
      . SerializedType            // Type which supports Serialization by Unity
      . ValueWrapper              // Wraps a single value in an object
      > Unitylity.Data.Trees
        . IBranch                 // Base Interface for branches of a tree
        . ICell                   // Base Interface for cells of a tree
        . ITree                   // Base Interface for a tree
        . ITreeEnumerator         // Base Interface for enumerating a tree
        > Octree
          . Octree_Enumerator     // Enumerator for Octree
          . Octree                // 8 branched tree data structure of unlimited depth
          . OctreeCell            // Cell of Octree
        > Quadtree
          . Quadtree_Enumerator   // Enumerator for Quadtree
          . Quadtree              // 4 branched tree data structure of unlimited depth
          . QuadtreeCell          // Cell of Quadtree
        > VoxelTree
          . VoxelTree_Enumerator  // Enumerator for VoxelTree
          . VoxelTree             // 8 branched tree data structure of predetermined depth specialized for voxel structures
          . VoxCell               // Cell of VoxelTree
    > Unitylity.Editor
      . BuiltInResourcesWindow    // Finds and lists styles and textures in the project
      . UnitylitySettings         // Project settings tab
      . ShowEditorAttribute       // Shows an inline editor in the inspector for a specific field
      > Util
        . GizmosUtil              // Utilities for Gizmos
        . HandlesUtil             // Utilities for Handles
        . PropertyUtil            // Utilities for SerializedProperties
        > EditorUtil
          . EditorUtil            // Utilities for custom Editors
          . EditorUtil_Controls   // Essential replacement for the horrible EditorGUI and EditorGUILayout
          . EditorUtil_TypeSelectMenu // Context menus for selecting Types
    > Unitylity.Extensions
      .ComponentExtensions        // Extension Methods for Component
      .FloatExtensions            // Extension Methods for Float
      .RectExtensions             // Extension Methods for Rect
      .StringExtensions           // Extension Methods for String
      .TransformExtensions        // Extension Methods for Transform
      .TypeExtensions             // Extension Methods for Type
      .VectorExtensions           // Extension Methods for Vector (swizzles, math ops, and more)
    > Unitylity.Geometry
      . Line                      // A line from point A to B
      . Spline                    // A smoothed path through points
    > Unitylity.Numerics
      . CircularDecimal           // Decimal which wraps back to 0 at a specific value
      . CircularDouble            // Double which wraps back to 0 at a specific value
      . CircularFloat             // Single which wraps back to 0 at a specific value
      . CircularInt               // Int32 which wraps back to 0 at a specific value
      . CircularLong              // Int64 which wraps back to 0 at a specific value
    > Unitylity.Time
      . FrameInterval             // Frame based interval which accumulates one use when an amount of frames pass
      . FrameTimeout              // Frame based timeout which can be used when an amount of frames pass
      . Interval                  // Time based interval which accumulates one use when an amount of time passes
      . Timeout                   // Time based timeout which can be used when an amount of time passes
    > Unitylity.Systems
      > Unitylity.Systems.Camera
        . MyUnityCamera           // Component based Camera with smoothing, targeting, etc.
        . MyUnityCameraDrag       // Drag feature for MyUnityCamera which can be plane or collider based
        . MyUnityCameraMove       // Move feature for MyUnityCamera e.g. with a controller
        . MyUnityCameraRotate     // Rotate feature for MyUnityCamera e.g. with a controller
        . MyUnityCameraZoom       // Zoom feature for MyUnityCamera e.g. with scroll wheel
      > Unitylity.Systems.Input
        . Input                   // Base class for inputs utilizing the "New Input System" to fire UnityEvents
        . AxisInput               // Reacts to changes in an AxisControl (float)
        . ButtonInput             // Reacts to changes in a ButtonControl (float)
        . DoubleInput             // Reacts to changes in a DoubleControl
        . IntegerInput            // Reacts to changes in an IntegerControl
        . QuaternionInput         // Reacts to changes in a QuaternionControl
        . TouchInput              // Reacts to changes in a TouchControl
        . TouchPhaseInput         // Reacts to changes in a TouchPhaseControl
        . Vector2Input            // Reacts to changes in a Vector2Control
        . Vector3Input            // Reacts to changes in a Vector3Control
      > Unitylity.Systems.Interaction
        . Interactable            // Fires UnityEvents when an Interactor interacts with it
        . Interactor              // Able to interact with Interactables
        . Movable                 // Implements moving of the object by Interactors
        . Throwable               // Implements throwing of the object by Interactors
      > Unitylity.Systems.RenderImages
        . RenderImage             // RawImage which displays a RenderObject
        . RenderObject            // Objects that can be rendered to RenderImages
        . RenderObjects           // Manages the usage, sharing, and pooling of RenderObjects
      > Unitylity.Systems.Lang
        . Lang                    // Singleton that facilitates dynamic text with translations 
        . LangText                // TMPro UI text implementation 
        . LangFormatText          // TMPro UI text implementation with values for formatting
      > Unitylity.Systems.Menus
        . Menus                   // Singleton that handles menus that can be stacked and more 
        . Menu                    // Base class for a menu 
      > Unitylity.Systems.Popups
        . Popups                  // Singleton that handles popups (e.g. a message box)
        . Popup                   // Base class for a popup
        . PopupOption             // Option shown in a popup (yes, no, etc.) 
        . PopupPreset             // Combines the Popup prefab and PopupOptions so it can be shown by Popups 
```















