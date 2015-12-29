using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;


public static class Assimp
{
     /// <summary>
        /// Enables time measurements. If enabled the time needed for each
        /// part of the loading process is timed and logged.
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_GLOB_MEASURE_TIME = "GLOB_MEASURE_TIME";

        /// <summary>
        /// Sets Assimp's multithreading policy. This is ignored if Assimp is
        /// built without boost.thread support. Possible values are: -1 to
        /// let Assimp decide, 0 to disable multithreading, and nay number larger than 0
        /// to force a specific number of threads. This is only a hint and may be 
        /// ignored by Assimp.
        /// <para>Type: integer. Default: -1</para>
        /// </summary>
        public const String AI_CONFIG_GLOB_MULTITHREADING = "GLOB_MULTITHREADING";
        
   

        /// <summary>
        /// Specifies the maximum angle that may be between two vertex tangents that their tangents
        /// and bitangents are smoothed during the step to calculate the tangent basis. The angle specified 
        /// is in degrees. The maximum value is 175 degrees.
        /// <para>Type: float. Default: 45 degrees</para>
        /// </summary>
        public const String AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE = "PP_CT_MAX_SMOOTHING_ANGLE";

        /// <summary>
        /// Specifies the maximum angle that may be between two face normals at the same vertex position that
        /// their normals will be smoothed together during the calculate smooth normals step. This is commonly
        /// called the "crease angle". The angle is specified in degrees. Maximum value is 175 degrees (all vertices
        /// smoothed).
        /// <para>Type: float. Default: 175 degrees</para>
        /// </summary>
        public const String AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE = "PP_GSN_MAX_SMOOTHING_ANGLE";

        /// <summary>
        /// Sets the colormap(= palette) to be used to decode embedded textures in MDL (Quake or 3DG5) files.
        /// This must be a valid path to a file. The file is 768 (256 * 3) bytes large and contains
        /// RGB triplets for each of the 256 palette entries. If the file is not found, a default
        /// palette (from Quake 1) is used.
        /// <para>Type: string. Default: "colormap.lmp"</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MDL_COLORMAP = "IMPORT_MDL_COLORMAP";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.RemoveRedundantMaterials"/> step to
        /// keep materials matching a name in a given list. This is a list of
        /// 1 to n strings where whitespace ' ' serves as a delimiter character. Identifiers
        /// containing whitespaces must be enclosed in *single* quotation marks. Tabs or
        /// carriage returns are treated as whitespace.
        /// <para>If a material matches one of these names, it will not be modified
        /// or removed by the post processing step nor will other materials be replaced
        /// by a reference to it.</para>
        /// <para>Default: string. Default: ""</para>
        /// </summary>
        public const String AI_CONFIG_PP_RRM_EXCLUDE_LIST = "PP_RRM_EXCLUDE_LIST";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.PreTransformVertices"/> step
        /// to keep the scene hierarchy. Meshes are moved to worldspace, but no optimization
        /// is performed where meshes with the same materials are not joined.
        /// <para>This option could be of used if you have a scene hierarchy that contains
        /// important additional information which you intend to parse.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_PP_PTV_KEEP_HIERARCHY = "PP_PTV_KEEP_HIERARCHY";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.PreTransformVertices"/> step
        /// to normalize all vertex components into the -1...1 range. That is, a bounding
        /// box for the whole scene is computed where the maximum component is taken
        /// and all meshes are scaled uniformly. This is useful if you don't know the spatial dimension
        /// of the input data.
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_PP_PTV_NORMALIZE = "PP_PTV_NORMALIZE";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.FindDegenerates"/> step
        /// to remove degenerated primitives from the import immediately.
        /// <para>The default behavior converts degenerated triangles to lines and
        /// degenerated lines to points.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_PP_FD_REMOVE = "PP_FD_REMOVE";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.OptimizeGraph"/> step
        /// to preserve nodes matching a name in a given list. This is a list of 1 to n strings, whitespace ' ' serves as a delimter character.
        /// Identifiers containing whitespaces must be enclosed in *single* quotation marks. Carriage returns
        /// and tabs are treated as white space.
        /// <para>If a node matches one of these names, it will not be modified or removed by the
        /// postprocessing step.</para>
        /// <para>Type: string. Default: ""</para>
        /// </summary>
        public const String AI_CONFIG_PP_OG_EXCLUDE_LIST = "PP_OG_EXCLUDE_LIST";

        /// <summary>
        /// Sets the maximum number of triangles a mesh can contain. This is used by the
        /// <see cref="PostProcessSteps.SplitLargeMeshes"/> step to determine
        /// whether a mesh must be split or not.
        /// <para>Type: int. Default: AiDefines.AI_SLM_DEFAULT_MAX_TRIANGLES</para>
        /// </summary>
        public const String AI_CONFIG_PP_SLM_TRIANGLE_LIMIT = "PP_SLM_TRIANGLE_LIMIT";

        /// <summary>
        /// Sets the maximum number of vertices in a mesh. This is used by the
        /// <see cref="PostProcessSteps.SplitLargeMeshes"/> step to determine
        /// whether a mesh must be split or not.
        /// <para>Type: integer. Default: AiDefines.AI_SLM_DEFAULT_MAX_VERTICES</para>
        /// </summary>
        public const String AI_CONFIG_PP_SLM_VERTEX_LIMIT = "PP_SLM_VERTEX_LIMIT";

        /// <summary>
        /// Sets the maximum number of bones that can affect a single vertex. This is used
        /// by the <see cref="PostProcessSteps.LimitBoneWeights"/> step.
        /// <para>Type: integer. Default: AiDefines.AI_LBW_MAX_WEIGHTS</para>
        /// </summary>
        public const String AI_CONFIG_PP_LBW_MAX_WEIGHTS = "PP_LBW_MAX_WEIGHTS";

        /// <summary>
        /// Sets the size of the post-transform vertex cache to optimize vertices for. This is
        /// for the <see cref="PostProcessSteps.ImproveCacheLocality"/> step. The size
        /// is given in vertices. Of course you can't know how the vertex format will exactly look
        /// like after the import returns, but you can still guess what your meshes will
        /// probably have. The default value *has* resulted in slight performance improvements
        /// for most Nvidia/AMD cards since 2002.
        /// <para>Type: integer. Default: AiDefines.PP_ICL_PTCACHE_SIZE</para>
        /// </summary>
        public const String AI_CONFIG_PP_ICL_PTCACHE_SIZE = "PP_ICL_PTCACHE_SIZE";

        /// <summary>
        /// Input parameter to the <see cref="PostProcessSteps.RemoveComponent"/> step. 
        /// It specifies the parts of the data structure to be removed.
        /// <para>This is a bitwise combination of the <see cref="ExcludeComponent"/> flag. If no valid mesh is remaining after
        /// the step is executed, the import FAILS.</para>
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_PP_RVC_FLAGS = "PP_RVC_FLAGS";

        /// <summary>
        /// Input parameter to the <see cref="PostProcessSteps.SortByPrimitiveType"/> step.
        /// It specifies which primitive types are to be removed by the step.
        /// <para>This is a bitwise combination of the <see cref="PrimitiveType"/> flag.
        /// Specifying ALL types is illegal.</para>
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_PP_SBP_REMOVE = "PP_SBP_REMOVE";

        /// <summary>
        /// Input parameter to the <see cref="PostProcessSteps.FindInvalidData"/> step.
        /// It specifies the floating point accuracy for animation values, specifically the epislon
        /// during the comparison. The step checks for animation tracks where all frame values are absolutely equal 
        /// and removes them. Two floats are considered equal if the invariant <c>abs(n0-n1) > epislon</c> holds
        /// true for all vector/quaternion components.
        /// <para>Type: float. Default: 0.0f (comparisons are exact)</para>
        /// </summary>
        public const String AI_CONFIG_PP_FID_ANIM_ACCURACY = "PP_FID_ANIM_ACCURACY";

        /// <summary>
        /// Input parameter to the <see cref="PostProcessSteps.TransformUVCoords"/> step.
        /// It specifies which UV transformations are to be evaluated.
        /// <para>This is bitwise combination of the <see cref="UVTransformFlags"/> flag.</para>
        /// <para>Type: integer. Default: AiDefines.AI_UV_TRAFO_ALL (All combinations)</para>
        /// </summary>
        public const String AI_CONFIG_PP_TUV_EVALUATE = "PP_TUV_EVALUATE";

        /// <summary>
        /// A hint to Assimp to favour speed against import quality. Enabling this option
        /// may result in faster loading, or it may not. It is just a hint to loaders and post-processing
        /// steps to use faster code paths if possible. A value not equal to zero stands
        /// for true.
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_FAVOUR_SPEED = "FAVOUR_SPEED";

        /// <summary>
        /// Maximum bone cone per mesh for the <see cref="PostProcessSteps.SplitByBoneCount"/> step. Meshes
        /// are split until the max number of bones is reached.
        /// <para>Type: integer. Default: 60</para>
        /// </summary>
        public const String AI_CONFIG_PP_SBBC_MAX_BONES = "PP_SBBC_MAX_BONES";

        /// <summary>
        /// Source UV channel for tangent space computation. The specified channel must exist or an error will be raised.
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_PP_CT_TEXTURE_CHANNEL_INDEX = "AI_CONFIG_PP_CT_TEXTURE_CHANNEL_INDEX";

        /// <summary>
        /// Threshold used to determine if a bone is kept or removed during the <see cref="PostProcessSteps.Debone"/> step.
        /// <para>Type: float. Default: 1.0f</para>
        /// </summary>
        public const String AI_CONFIG_PP_DB_THRESHOLD = "PP_DB_THRESHOLD";

        /// <summary>
        /// Require all bones to qualify for deboning before any are removed.
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_PP_DB_ALL_OR_NONE = "PP_DB_ALL_OR_NONE";

      
        /// <summary>
        /// Sets the vertex animation keyframe to be imported. Assimp does not support
        /// vertex keyframes (only bone animation is supported). The libary reads only one frame of models
        /// with vertex animations. By default this is the first frame.
        /// <para>The default value is 0. This option applies to all importers. However, it is
        /// also possible to override the global setting for a specific loader. You can use the
        /// AI_CONFIG_IMPORT_XXX_KEYFRAME options where XXX is a placeholder for the file format which
        /// you want to override the global setting.</para>
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_GLOBAL_KEYFRAME = "IMPORT_GLOBAL_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD3_KEYFRAME = "IMPORT_MD3_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD2_KEYFRAME = "IMPORT_MD3_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_MDL_KEYFRAME = "IMPORT_MDL_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_MDC_KEYFRAME = "IMPORT_MDC_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_SMD_KEYFRAME = "IMPORT_SMD_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_UNREAL_KEYFRAME = "IMPORT_UNREAL_KEYFRAME";

        /// <summary>
        /// Configures the AC loader to collect all surfaces which have the "Backface cull" flag set in separate
        /// meshes.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_AC_SEPARATE_BFCULL = "IMPORT_AC_SEPARATE_BFCULL";

        /// <summary>
        /// Configures whether the AC loader evaluates subdivision surfaces (indicated by the presence
        /// of the 'subdiv' attribute in the file). By default, Assimp performs
        /// the subdivision using the standard Catmull-Clark algorithm.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_AC_EVAL_SUBDIVISION = "IMPORT_AC_EVAL_SUBDIVISION";

        /// <summary>
        /// Configures the UNREAL 3D loader to separate faces with different surface flags (e.g. two-sided vs single-sided).
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_UNREAL_HANDLE_FLAGS = "UNREAL_HANDLE_FLAGS";

        /// <summary>
        /// Configures the terragen import plugin to compute UV's for terrains, if
        /// they are not given. Furthermore, a default texture is assigned.
        /// <para>UV coordinates for terrains are so simple to compute that you'll usually 
        /// want to compute them on your own, if you need them. This option is intended for model viewers which
        /// want to offer an easy way to apply textures to terrains.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_TER_MAKE_UVS = "IMPORT_TER_MAKE_UVS";

        /// <summary>
        /// Configures the ASE loader to always reconstruct normal vectors basing on the smoothing groups
        /// loaded from the file. Some ASE files carry invalid normals, others don't.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_ASE_RECONSTRUCT_NORMALS = "IMPORT_ASE_RECONSTRUCT_NORMALS";

        /// <summary>
        /// Configures the M3D loader to detect and process multi-part Quake player models. These models
        /// usually consit of three files, lower.md3, upper.md3 and head.md3. If this propery is
        /// set to true, Assimp will try to load and combine all three files if one of them is loaded.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD3_HANDLE_MULTIPART = "IMPORT_MD3_HANDLE_MULTIPART";

        /// <summary>
        /// Tells the MD3 loader which skin files to load. When loading MD3 files, Assimp checks
        /// whether a file named "md3_file_name"_"skin_name".skin exists. These files are used by
        /// Quake III to be able to assign different skins (e.g. red and blue team) to models. 'default', 'red', 'blue'
        /// are typical skin names.
        /// <para>Type: string. Default: "default"</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD3_SKIN_NAME = "IMPORT_MD3_SKIN_NAME";

        /// <summary>
        /// Specifies the Quake 3 shader file to be used for a particular MD3 file. This can be a full path or
        /// relative to where all MD3 shaders reside.
        /// <para>Type: string. Default: ""</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD3_SHADER_SRC = "IMPORT_MD3_SHADER_SRC";

        /// <summary>
        /// Configures the LWO loader to load just one layer from the model.
        /// <para>LWO files consist of layers and in some cases it could be useful to load only one of them.
        /// This property can be either a string - which specifies the name of the layer - or an integer - the index
        /// of the layer. If the property is not set then the whole LWO model is loaded. Loading fails
        /// if the requested layer is not vailable. The layer index is zero-based and the layer name may not be empty</para>
        /// <para>Type: bool. Default: false (All layers are loaded)</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_LWO_ONE_LAYER_ONLY = "IMPORT_LWO_ONE_LAYER_ONLY";

        /// <summary>
        /// Configures the MD5 loader to not load the MD5ANIM file for a MD5MESH file automatically.
        /// <para>The default strategy is to look for a file with the same name but with the MD5ANIm extension
        /// in the same directory. If it is found it is loaded and combined with the MD5MESH file. This configuration
        /// option can be used to disable this behavior.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD5_NO_ANIM_AUTOLOAD = "IMPORT_MD5_NO_ANIM_AUTOLOAD";

        /// <summary>
        /// Defines the beginning of the time range for which the LWS loader evaluates animations and computes
        /// AiNodeAnim's.
        /// <para>Assimp provides full conversion of Lightwave's envelope system, including pre and post
        /// conditions. The loader computes linearly subsampled animation channels with the frame rate
        /// given in the LWS file. This property defines the start time.</para>
        /// <para>Animation channels are only generated if a node has at least one envelope with more than one key
        /// assigned. This property is given in frames where '0' is the first. By default,
        /// if this property is not set, the importer takes the animation start from the input LWS
        /// file ('FirstFrame' line)</para>
        /// <para>Type: integer. Default: taken from file</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_LWS_ANIM_START = "IMPORT_LWS_ANIM_START";

        /// <summary>
        /// Defines the ending of the time range for which the LWS loader evaluates animations and computes
        /// AiNodeAnim's.
        /// <para>Assimp provides full conversion of Lightwave's envelope system, including pre and post
        /// conditions. The loader computes linearly subsampled animation channels with the frame rate
        /// given in the LWS file. This property defines the end time.</para>
        /// <para>Animation channels are only generated if a node has at least one envelope with more than one key
        /// assigned. This property is given in frames where '0' is the first. By default,
        /// if this property is not set, the importer takes the animation end from the input LWS
        /// file.</para>
        /// <para>Type: integer. Default: taken from file</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_LWS_ANIM_END = "IMPORT_LWS_ANIM_END";

        /// <summary>
        /// Defines the output frame rate of the IRR loader.
        /// <para>IRR animations are difficult to convert for Assimp and there will always be
        /// a loss of quality. This setting defines how many keys per second are returned by the converter.</para>
        /// <para>Type: integer. Default: 100</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_IRR_ANIM_FPS = "IMPORT_IRR_ANIM_FPS";

        /// <summary>
        /// The Ogre importer will try to load this MaterialFile. If a material file does not
        /// exist with the same name as a material to load, the ogre importer will try to load this file
        /// and searches for the material in it.
        /// <para>Type: string. Default: ""</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_OGRE_MATERIAL_FILE = "IMPORT_OGRE_MATERIAL_FILE";

        /// <summary>
        /// The Ogre importer will detect the texture usage from the filename. Normally a texture is loaded as a color map, if no target is specified
        /// in the material file. If this is enabled, texture names ending with _n, _l, _s are used as normal maps, light maps, or specular maps.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_OGRE_TEXTURETYPE_FROM_FILENAME = "IMPORT_OGRE_TEXTURETYPE_FROM_FILENAME";

        /// <summary>
        /// Specifies whether the IFC loader skips over shape representations of type 'Curve2D'. A lot of files contain both a faceted mesh representation and a outline 
        /// with a presentation type of 'Curve2D'. Currently Assimp does not convert those, so turning this option off just clutters the log with errors.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_IFC_SKIP_CURVE_REPRESENTATIONS = "IMPORT_IFC_SKIP_CURVE_REPRESENTATIONS";

        /// <summary>
        /// Specifies whether the IFC loader will use its own, custom triangulation algorithm to triangulate wall and floor meshes. If this is set to false,
        /// walls will be either triangulated by the post process triangulation or will be passed through as huge polygons with faked holes (e.g. holes that are connected
        /// with the outer boundary using a dummy edge). It is highly recommended to leave this property set to true as the default post process has some known
        /// issues with these kind of polygons.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_IFC_CUSTOM_TRIANGULATION = "IMPORT_IFC_CUSTOM_TRIANGULATION";
        
    /// <summary>
    /// Post processing flag options, specifying a number of steps
    /// that can be run on the data to either generate additional vertex
    /// data or optimize the imported data.
    /// </summary>

    [Flags]
    public enum PostProcessSteps : int
    {
        /// <summary>
        /// No flags enabled.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Calculates the tangents and binormals (bitangents)
        /// for the imported meshes.
        /// <para>
        /// This does nothing if a mesh does not have normals. You might
        /// want this post processing step to be executed if you plan
        /// to use tangent space calculations such as normal mapping. There is a
        /// config setting AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE which
        /// allows you to specify a maximimum smoothing angle for the algorithm.
        /// However, usually you'll want to leave it at the default value.
        /// </para>
        /// </summary>
        CalculateTangentSpace = 0x1,

        /// <summary>
        /// Identifies and joins identical vertex data sets within all
        /// imported meshes.
        /// <para>
        /// After this step is run each mesh does contain only unique vertices
        /// anymore, so a vertex is possibly used by multiple faces. You usually
        /// want to use this post processing step. If your application deals with
        /// indexed geometry, this step is compulsory or you'll just waste rendering
        /// time.</para>
        /// <para>If this flag is not specified, no vertices are referenced by more than one
        /// face and no index buffer is required for rendering.</para>
        /// </summary>
        JoinIdenticalVertices = 0x2,

        /// <summary>
        /// Converts all imported data to a left handed coordinate space.
        /// 
        /// <para>By default the data is returned in a right-handed coordinate space,
        /// where +X points to the right, +Z towards the viewer, and +Y upwards.</para>
        /// </summary>
        MakeLeftHanded = 0x4,

        /// <summary>
        /// Triangulates all faces of all meshes.
        /// <para>
        /// By default the imported mesh data might contain faces with more than 
        /// three indices. For rendering you'll usually want all faces to
        /// be triangles. This post processing step splits up all
        /// higher faces to triangles. Line and point primitives are *not*
        /// modified. If you want 'triangles only' with no other kinds of primitives,
        /// try the following:
        /// </para>
        /// <list type="number">
        /// <item>
        /// <description>Specify both <see cref="PostProcessSteps.Triangulate"/> and <see cref="PostProcessSteps.SortByPrimitiveType"/>.</description>
        /// </item>
        /// <item>
        /// <description>Ignore all point and line meshes when you process Assimp's output</description>
        /// </item>
        /// </list>
        /// </summary>
        Triangulate = 0x8,

        /// <summary>
        /// Removes some parts of the data structure (animations, materials,
        /// light sources, cameras, textures, vertex components).
        /// <para>
        /// The components to be removed are specified in a separate configuration
        /// option, AI_CONFIG_PP_RVC_FLAGS. This is quite useful if you don't
        /// need all parts of the output structure. Especially vertex colors are rarely used today...calling this step to remove
        /// unrequired stuff from the pipeline as early as possible results in an increased
        /// performance and a better optimized output data structure.
        /// </para>
        /// <para>
        /// This step is also useful if you want to force Assimp to recompute normals
        /// or tangents. the corresponding steps don't recompute them if they're already
        /// there (loaded from the source asset). By using this step you can make sure
        /// they are NOT there.</para>
        /// </summary>
        RemoveComponent = 0x10,

        /// <summary>
        /// Generates normals for all faces of all meshes. It may not be
        /// specified together with <see cref="PostProcessSteps.GenerateSmoothNormals"/>.
        /// <para>
        /// This is ignored if normals are already there at the time where this
        /// flag is evaluated. Model importers try to load them from the source file,
        /// so they're usually already there. Face normals are shared between all
        /// points of a single face, so a single point can have multiple normals,
        /// which in other words, forces the library to duplicate vertices in
        /// some cases. This makes <see cref="PostProcessSteps.JoinIdenticalVertices"/> senseless then.
        /// </para>
        /// </summary>
        GenerateNormals = 0x20,

        /// <summary>
        /// Generates smooth normals for all vertices of all meshes. It
        /// may not be specified together with <see cref="PostProcessSteps.GenerateNormals"/>.
        /// <para>
        /// This is ignored if normals are already there at the time where
        /// this flag is evaluated. Model importers try to load them from the
        /// source file, so they're usually already there.
        /// </para>
        /// <para>The configuration option AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE
        /// allows you to specify an angle maximum for the normal smoothing algorithm.
        /// Normals exceeding this limit are not smoothed, resulting in a 'hard' seam
        /// between two faces. using a decent angle here (e.g. 80 degrees) results in a very good visual
        /// appearance.</para>
        /// </summary>
        GenerateSmoothNormals = 0x40,

        /// <summary>
        /// Splits large meshes into smaller submeshes.
        /// <para>
        /// This is useful for realtime rendering where the number
        /// of triangles which can be maximally processed in a single draw call is
        /// usually limited by the video driver/hardware. The maximum vertex buffer
        /// is usually limited, too. Both requirements can be met with this step:
        /// you may specify both a triangle and a vertex limit for a single mesh.
        /// </para>
        /// <para>The split limits can be set through the AI_CONFIG_PP_SLM_VERTEX_LIMIT
        /// and AI_CONFIG_PP_SLM_TRIANGLE_LIMIT config settings. The default
        /// values are 1,000,000.</para>
        /// 
        /// <para>Warning: This can be a time consuming task.</para>
        /// </summary>
        SplitLargeMeshes = 0x80,

        /// <summary>
        /// Removes the node graph and "bakes" (pre-transforms) all
        /// vertices with the local transformation matrices of their nodes.
        /// The output scene does still contain nodes, however, there is only
        /// a root node with children, each one referencing only one mesh. 
        /// Each mesh referencing one material. For rendering, you can simply render
        /// all meshes in order, you don't need to pay attention to local transformations
        /// and the node hierarchy.
        /// 
        /// <para>Warning: Animations are removed during this step.</para>
        /// </summary>
        PreTransformVertices = 0x100,

        /// <summary>
        /// Limits the number of bones simultaneously affecting a single
        /// vertex to a maximum value.
        /// <para>
        /// If any vertex is affected by more than that number of bones,
        /// the least important vertex weights are removed and the remaining vertex
        /// weights are re-normalized so that the weights still sum up to 1.
        /// </para>
        /// <para>The default bone weight limit is 4 and uses the
        /// AI_LMW_MAX_WEIGHTS config. If you intend to perform the skinning in hardware, this post processing
        /// step might be of interest for you.</para>
        /// </summary>
        LimitBoneWeights = 0x200,

        /// <summary>
        /// Validates the imported scene data structure.
        /// <para>
        /// This makes sure that all indices are valid, all animations
        /// and bones are linked correctly, all material references are
        /// correct, etc.
        /// </para>
        /// It is recommended to capture Assimp's log output if you use this flag,
        /// so you can easily find out what's actually wrong if a file fails the
        /// validation. The validator is quite rude and will find *all* inconsistencies
        /// in the data structure. There are two types of failures:
        /// <list type="bullet">
        /// <item>
        /// <description>Error: There's something wrong with the imported data. Further
        /// postprocessing is not possible and the data is not usable at all. The import
        /// fails.</description>
        /// </item>
        /// <item>
        /// <description>Warning: There are some minor issues (e.g. 1000000 animation keyframes
        /// with the same time), but further postprocessing and use of the data structure is still
        /// safe. Warning details are written to the log file.</description>
        /// </item>
        /// </list>
        /// </summary>
        ValidateDataStructure = 0x400,

        /// <summary>
        /// Re-orders triangles for better vertex cache locality.
        /// 
        /// <para>This step tries to improve the ACMR (average post-transform vertex cache
        /// miss ratio) for all meshes. The implementation runs in O(n) time 
        /// and is roughly based on the <a href="http://www.cs.princeton.edu/gfx/pubs/Sander_2007_%3ETR/tipsy.pdf">'tipsify' algorithm</a>.</para>
        /// 
        /// <para>If you intend to render huge models in hardware, this step might be of interest for you.
        /// The AI_CONFIG_PP_ICL_PTCACHE_SIZE config setting can be used to fine tune
        /// the cache optimization.</para>
        /// </summary>
        ImproveCacheLocality = 0x800,

        /// <summary>
        /// Searches for redundant/unreferenced materials and removes them.
        /// <para>
        /// This is especially useful in combination with the  PreTransformVertices
        /// and OptimizeMeshes flags. Both join small meshes with equal characteristics, but
        /// they can't do their work if two meshes have different materials. Because several
        /// material settings are always lost during Assimp's import filders and because many
        /// exporters don't check for redundant materials, huge models often have materials which
        /// are defined several times with exactly the same settings.
        /// </para>
        /// <para>Several material settings not contributing to the final appearance of a surface
        /// are ignored in all comparisons ... the material name is one of them. So, if you're passing
        /// additional information through the content pipeline (probably using *magic* material names),
        /// don't specify this flag. Alternatively, take a look at the AI_CONFIG_PP_RRM_EXCLUDE_LIST
        /// setting.</para>
        /// </summary>
        RemoveRedundantMaterials = 0x1000,

        /// <summary>
        /// This step tries to determine which meshes have normal vectors
        /// that are facing inwards. 
        /// <para>
        /// The algorithm is simple but effective:
        /// </para>
        /// <para>The bounding box of all vertices and their normals are compared
        /// against the volume of the bounding box of all vertices without their normals.
        /// This works well for most objects, problems might occur with planar surfaces. However,
        /// the step tries to filter such cases. The step inverts all in-facing normals.
        /// Generally, it is recommended to enable this step, although the result is not
        /// always correct.</para>
        /// </summary>
        FixInFacingNormals = 0x2000,

        /// <summary>
        /// This step splits meshes with more than one primitive type in homogeneous submeshes.
        /// <para>
        /// This step is executed after triangulation and after it returns, just one
        /// bit is set in aiMesh:mPrimitiveTypes. This is especially useful for real-time
        /// rendering where point and line primitives are often ignored or rendered separately.
        /// </para>
        /// <para>
        /// You can use AI_CONFIG_PP_SBP_REMOVE option to specify which primitive types you need.
        /// This can be used to easily exclude lines and points, which are rarely used,
        /// from the import.
        /// </para>
        /// </summary>
        SortByPrimitiveType = 0x8000,

        /// <summary>
        /// This step searches all meshes for degenerated primitives and
        /// converts them to proper lines or points. A face is 'degenerated' if one or more of its points are identical.
        /// <para>
        /// To have degenerated primitives removed, specify the <see cref="PostProcessSteps.FindDegenerates"/> flag
        /// try one of the following procedures:
        /// </para>
        /// <list type="numbers">
        /// <item>
        /// <description>To support lines and points: Set the
        /// AI_CONFIG_PP_FD_REMOVE option to one. This will cause the step to remove degenerated triangles as
        /// soon as they are detected. They won't pass any further pipeline steps.</description>
        /// </item>
        /// <item>
        /// <description>If you don't support lines and points: Specify <see cref="PostProcessSteps.SortByPrimitiveType"/> flag, which
        /// will move line and point primitives to separate meshes.  Then set the AI_CONFIG_PP_SBP_REMOVE
        /// option to <see cref="PrimitiveType.Point"/> and <see cref="PrimitiveType.Line"/> to cause <see cref="PostProcessSteps.SortByPrimitiveType"/> step
        /// to reject point and line meshes from the scene.</description>
        /// </item>
        /// </list>
        /// <para>
        /// Degenerated polygons are not necessarily evil and that's why they are not removed by default. There are several
        /// file formats which do not support lines or points where exporters bypass the format specification and write
        /// them as degenerated triangles instead.
        /// </para>
        /// </summary>
        FindDegenerates = 0x10000,

        /// <summary>
        /// This step searches all meshes for invalid data, such as zeroed
        /// normal vectors or invalid UV coordinates and removes or fixes them.
        /// This is intended to get rid of some common exporter rrors.
        /// <para>
        /// This is especially useful for normals. If they are invalid,
        /// and the step recognizes this, they will be removed and can later
        /// be recomputed, e.g. by the GenerateSmoothNormals flag. The step
        /// will also remove meshes that are infinitely small and reduce animation
        /// tracks consisting of hundreds of redundant keys to a single key. The
        /// AI_CONFIG_PP_FID_ANIM_ACCURACY config property decides the accuracy of the check
        /// for duplicate animation tracks.</para>
        /// </summary>
        FindInvalidData = 0x20000,

        /// <summary>
        /// This step converts non-UV mappings (such as spherical or
        /// cylindrical mapping) to proper texture coordinate channels.
        /// 
        /// <para>Most applications will support UV mapping only, so you will
        /// probably want to specify this step in every case. Note that Assimp
        /// is not always able to match the original mapping implementation of the 3D
        /// app which produced a model perfectly. It's always better
        /// to let the father app compute the UV channels, at least 3DS max, maya, blender,
        /// lightwave, modo, .... are able to achieve this.</para>
        /// 
        /// <para>If this step is not requested, you'll need to process the MATKEY_MAPPING
        /// material property in order to display all assets properly.</para>
        /// </summary>
        GenerateUVCoords = 0x40000,

        /// <summary>
        /// Applies per-texture UV transformations and bakes them to stand-alone vtexture
        /// coordinate channels.
        /// 
        /// <para>UV Transformations are specified per-texture - see the MATKEY_UVTRANSFORM material
        /// key for more information. This step processes all textures with transformed input UV coordinates
        /// and generates new (pretransformed) UV channel transformations, so you will probably
        /// want to specify this step.</para>
        /// 
        /// <para>UV transformations are usually implemented in realtime apps by
        /// transforming texture coordinates in a vertex shader stage with a 3x3 (homogenous)
        /// transformation matrix.</para>
        /// </summary>
        TransformUVCoords = 0x80000,

        /// <summary>
        /// Searches for duplicated meshes and replaces them with a reference
        /// to the first mesh.
        /// <para>
        /// This is time consuming, so don't use it if you have no time. Its
        /// main purpose is to work around the limitation with some
        /// file formats that don't support instanced meshes, so exporters
        /// duplicate meshes.
        /// </para>
        /// </summary>
        FindInstances = 0x100000,

        /// <summary>
        /// Attempts to reduce the number of meshes (and draw calls). 
        /// <para>
        /// This is recommended to be used together with <see cref="PostProcessSteps.OptimizeGraph"/>
        /// and is fully compatible with both <see cref="PostProcessSteps.SplitLargeMeshes"/> and <see cref="PostProcessSteps.SortByPrimitiveType"/>.
        /// </para>
        /// </summary>
        OptimizeMeshes = 0x200000,

        /// <summary>
        /// Optimizes scene hierarchy. Nodes with no animations, bones,
        /// lights, or cameras assigned are collapsed and joined.
        /// 
        /// <para>Node names can be lost during this step, you can specify
        /// names of nodes that should'nt be touched or modified
        /// with AI_CONFIG_PP_OG_EXCLUDE_LIST.</para>
        /// 
        /// <para>Use this flag with caution. Most simple files will be collapsed to a 
        /// single node, complex hierarchies are usually completely lost. That's not
        /// the right choice for editor environments, but probably a very effective
        /// optimization if you just want to get the model data, convert it to your
        /// own format and render it as fast as possible. </para>
        /// 
        /// <para>This flag is designed to be used with <see cref="PostProcessSteps.OptimizeMeshes"/> for best
        /// results.</para>
        /// 
        /// <para>Scenes with thousands of extremely small meshes packed
        /// in deeply nested nodes exist for almost all file formats.
        /// Usage of this and <see cref="PostProcessSteps.OptimizeMeshes"/> usually fixes them all and
        /// makes them renderable.</para>
        /// </summary>
        OptimizeGraph = 0x400000,

        /// <summary>
        /// Flips all UV coordinates along the y-axis
        /// and adjusts material settings/bitangents accordingly.
        /// </summary>
        FlipUVs = 0x800000,

        /// <summary>
        /// Flips face winding order from CCW (default) to CW.
        /// </summary>
        FlipWindingOrder = 0x1000000,

        /// <summary>
        /// Splits meshes with many bones into submeshes so that each submesh has fewer or as many bones as a given limit.
        /// </summary>
        SplitByBoneCount = 0x2000000,

        /// <summary>
        /// <para>Removes bones losslessly or according to some threshold. In some cases (e.g. formats that require it) exporters
        /// are faced to assign dummy bone weights to otherwise static meshes assigned to animated meshes. Full, weight-based skinning is expensive while
        /// animating nodes is extremely cheap, so this step is offered to clean up the data in that regard. 
        /// </para>
        /// <para>Usage of the configuration AI_CONFIG_PP_DB_THRESHOLD to control the threshold and AI_CONFIG_PP_DB_ALL_OR_NONE if you want bones
        /// removed if and only if all bones within the scene qualify for removal.</para>
        /// </summary>
        Debone = 0x4000000
    }
    public enum TextureType : int
    {
        /// <summary>
        /// No texture, but the value can be used as a 'texture semantic'.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// A diffuse texture that is combined with the result of the diffuse lighting equation.
        /// </summary>
        Diffuse = 0x1,

        /// <summary>
        /// A specular texture that is combined with the result of the specular lighting equation.
        /// </summary>
        Specular = 0x2,

        /// <summary>
        /// An ambient texture that is combined with the ambient lighting equation.
        /// </summary>
        Ambient = 0x3,

        /// <summary>
        /// An emissive texture that is added to the result of the lighting calculation. It is not influenced
        /// by incoming light, instead it represents the light that the object is naturally emitting.
        /// </summary>
        Emissive = 0x4,

        /// <summary>
        /// A height map texture. by convention, higher gray-scale values stand for
        /// higher elevations from some base height.
        /// </summary>
        Height = 0x5,

        /// <summary>
        /// A tangent-space normal map. There are several conventions for normal maps
        /// and Assimp does (intentionally) not distinguish here.
        /// </summary>
        Normals = 0x6,

        /// <summary>
        /// A texture that defines the glossiness of the material. This is the exponent of the specular (phong)
        /// lighting equation. Usually there is a conversion function defined to map the linear color values
        /// in the texture to a suitable exponent.
        /// </summary>
        Shininess = 0x7,

        /// <summary>
        /// The texture defines per-pixel opacity. usually 'white' means opaque and 'black' means 'transparency. Or quite
        /// the opposite.
        /// </summary>
        Opacity = 0x8,

        /// <summary>
        /// A displacement texture. The exact purpose and format is application-dependent. Higher color values stand for higher vertex displacements.
        /// </summary>
        Displacement = 0x9,

        /// <summary>
        /// A lightmap texture (aka Ambient occlusion). Both 'lightmaps' and dedicated 'ambient occlusion maps' are covered by this material property. The
        /// texture contains a scaling value for the final color value of a pixel. Its intensity is not affected by incoming light.
        /// </summary>
        Lightmap = 0xA,

        /// <summary>
        /// A reflection texture. Contains the color of a perfect mirror reflection. This is rarely used, almost never for real-time applications.
        /// </summary>
        Reflection = 0xB,

        /// <summary>
        /// An unknown texture that does not mention any of the defined texture type definitions. It is still imported, but is excluded from any
        /// further postprocessing.
        /// </summary>
        Unknown = 0xC
    }

 #if UNITY_EDITOR_64

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiImportFile(string filename, uint flags);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiReleaseImport(IntPtr scene);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiImportFileWithProperties(string filename, uint flags,IntPtr pProps);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiCreatePropertyStore();
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiReleasePropertyStore(IntPtr pProps);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiSetImportPropertyInteger(IntPtr pProps,string name,int value);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiSetImportPropertyFloat(IntPtr pProps, string name, float value);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiSetImportPropertyString(IntPtr pProps, string name, string value);


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiScene_HasMaterials(IntPtr pScene);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiScene_GetNumMaterials(IntPtr pScene);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiScene_GetNumMeshes(IntPtr pScene);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiScene_GetNumAnimations(IntPtr pScene);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiScene_HasMeshes(IntPtr pScene);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiScene_HasAnimation(IntPtr pScene);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiScene_GetRootNode(IntPtr pScene);


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMaterial_GetTextureCount(IntPtr pScene, int Layer, int type);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMaterial_GetShininess(IntPtr pScene, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMaterial_GetTransparency(IntPtr pScene, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiMaterial_GetTexture", ExactSpelling = false)]
	private static extern IntPtr _aiMaterial_GetTexture(IntPtr pScene, int Layer, int type);
	public static string aiMaterial_GetTexture(IntPtr pScene, int Layer, int type)
	{
		return getString(_aiMaterial_GetTexture(pScene, Layer, type));
	}

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMaterial_GetAmbient(IntPtr pScene, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMaterial_GetDiffuse(IntPtr pScene, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMaterial_GetSpecular(IntPtr pScene, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMaterial_GetEmissive(IntPtr pScene, int Layer);


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiMaterial_GetTexture", ExactSpelling = false)]
	private static extern IntPtr _aiMaterial_GetName(IntPtr pScene, int Layer);
	public static string aiMaterial_GetName(IntPtr pScene, int Layer)
	{
		return getString(_aiMaterial_GetName(pScene, Layer));
	}


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumVertices(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumFaces(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumBones(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetMaterialIndex(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumUVChannels(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumColorChannels(IntPtr pScene, int mesh);


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiMesh_GetName", ExactSpelling = false)]
	private static extern IntPtr _aiMesh_GetName(IntPtr pScene, int mesh);
	public static string aiMesh_GetName(IntPtr pScene, int mesh)
	{
		return getString(_aiMesh_GetName(pScene, mesh));
	}


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumIndicesPerFace(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_Indice(IntPtr pScene, int mesh, int Index, int Face);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasPositions(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasFaces(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasNormals(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasTangentsAndBitangents(IntPtr pScene, int mesh);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasVertexColors(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasTextureCoords(IntPtr pScene, int mesh, int Index);


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_Vertex(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_Normal(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_Tangent(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_Bitangent(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMesh_Color(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
	public static extern Vector3 aiMesh_TextureCoord(IntPtr pScene, int mesh, int Index, int Layer);



	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexX(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexY(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexZ(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexNX(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexNY(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexNZ(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TangentX(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TangentY(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TangentZ(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_BitangentX(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_BitangentY(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_BitangentZ(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TextureCoordX(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TextureCoordY(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_ColorRed(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_ColorGreen(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_ColorBlue(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_ColorAlpha(IntPtr pScene, int mesh, int Index, int Layer);




	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiMesh_GetBoneName", ExactSpelling = false)]
	private static extern IntPtr _aiMesh_GetBoneName(IntPtr pScene, int mesh, int Index);
	public static string aiMesh_GetBoneName(IntPtr pScene, int mesh, int Index)
	{
		return getString(_aiMesh_GetBoneName(pScene, mesh,Index));
	}
	//! Matrix that transforms from mesh space to bone space in bind pose(bone->mOffsetMatrix)
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Matrix4x4 aiMesh_GetBoneTransform(IntPtr pNode, int mesh, int Index);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Quaternion aiMesh_GetBoneRotation(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneRotationX(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneRotationY(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneRotationZ(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneRotationW(IntPtr pNode, int mesh, int Index);


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_GetBoneEulerRotation(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneEulerRotationX(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneEulerRotationY(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneEulerRotationZ(IntPtr pNode, int mesh, int Index);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_GetBonePosition(IntPtr pNode, int mesh, int Index);




	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumBoneWeights(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneWeight(IntPtr pScene, int mesh, int Index, int Weight);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetBoneVertexId(IntPtr pScene, int mesh, int Index, int Weight);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetDuration(IntPtr pScene, int anim);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetTicksPerSecond(IntPtr pScene, int anim);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetNumChannels(IntPtr pScene, int anim);


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiAnim_GetName", ExactSpelling = false)]
	private static extern IntPtr _aiAnim_GetName(IntPtr pScene, int anim);
	public static string aiAnim_GetName(IntPtr pScene, int anim)
	{
		return getString(_aiAnim_GetName(pScene, anim));
	}

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiAnim_GetChannelName", ExactSpelling = false)]
	private static extern IntPtr _aiAnim_GetChannelName(IntPtr pScene, int anim, int Index);
	public static string aiAnim_GetChannelName(IntPtr pScene, int anim, int Index)
	{
		return getString(_aiAnim_GetChannelName(pScene, anim, Index));
	}


	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiAnim_GetPositionKey(IntPtr pScene, int anim, int Index, int key);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiAnim_GetScalingKey(IntPtr pScene, int anim, int Index, int key);

	//assimp quaternion is w,x,y,z by i fix this and the quaternion is x,y,z,w
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Quaternion aiAnim_GetRotationKey(IntPtr pScene, int anim, int Index, int key);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiAnim_GetEurlRotationKey(IntPtr pScene, int anim, int Index, int key);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetChannelIndex(IntPtr pScene, int anim, string name);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetNumPositionKeys(IntPtr pScene, int anim, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetNumRotationKeys(IntPtr pScene, int anim, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetNumScalingKeys(IntPtr pScene, int anim, int Index);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetPositionKeyX(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetPositionKeyY(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetPositionKeyZ(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetPositionFrame(IntPtr pScene, int anim, int Index, int Key);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetScalingKeyX(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetScalingKeyY(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetScalingKeyZ(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetScalingFrame(IntPtr pScene, int anim, int Index, int Key);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetRotationKeyX(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetRotationKeyY(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetRotationKeyZ(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetRotationKeyW(IntPtr pScene, int anim, int Index, int Key);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetRotationFrame(IntPtr pScene, int anim, int Index, int Key);



	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiNode_GetName", ExactSpelling = false)]
	private static extern IntPtr _aiNode_GetName(IntPtr pNode);
	public static string aiNode_GetName(IntPtr pNode)
	{
		return getString(_aiNode_GetName(pNode));
	}

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiNode_GetNumChildren(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiNode_GetChild(IntPtr pNode, int Index);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiNode_GetParent(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiNode_FindChild(IntPtr pNode, string name);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Matrix4x4 aiNode_GetTransformation(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]

	//assimp quaternion is w,x,y,z by i fix this and the quaternion is x,y,z,w
	public static extern Quaternion aiNode_GetRotation(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetRotationX(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetRotationY(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetRotationZ(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetRotationW(IntPtr pNode);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiNode_GetPosition(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetPositionX(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetPositionY(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetPositionZ(IntPtr pNode);

	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiNode_GetEurlRotation(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetEurlRotationX(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetEurlRotationY(IntPtr pNode);
	[DllImport("assimp64.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetEurlRotationZ(IntPtr pNode);

 #else

	 	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiImportFile(string filename, uint flags);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiReleaseImport(IntPtr scene);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiImportFileWithProperties(string filename, uint flags,IntPtr pProps);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiCreatePropertyStore();
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiReleasePropertyStore(IntPtr pProps);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiSetImportPropertyInteger(IntPtr pProps,string name,int value);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiSetImportPropertyFloat(IntPtr pProps, string name, float value);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern void aiSetImportPropertyString(IntPtr pProps, string name, string value);


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiScene_HasMaterials(IntPtr pScene);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiScene_GetNumMaterials(IntPtr pScene);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiScene_GetNumMeshes(IntPtr pScene);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiScene_GetNumAnimations(IntPtr pScene);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiScene_HasMeshes(IntPtr pScene);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiScene_HasAnimation(IntPtr pScene);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiScene_GetRootNode(IntPtr pScene);


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMaterial_GetTextureCount(IntPtr pScene, int Layer, int type);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMaterial_GetShininess(IntPtr pScene, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMaterial_GetTransparency(IntPtr pScene, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiMaterial_GetTexture", ExactSpelling = false)]
	private static extern IntPtr _aiMaterial_GetTexture(IntPtr pScene, int Layer, int type);
	public static string aiMaterial_GetTexture(IntPtr pScene, int Layer, int type)
	{
		return getString(_aiMaterial_GetTexture(pScene, Layer, type));
	}

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMaterial_GetAmbient(IntPtr pScene, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMaterial_GetDiffuse(IntPtr pScene, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMaterial_GetSpecular(IntPtr pScene, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMaterial_GetEmissive(IntPtr pScene, int Layer);


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiMaterial_GetTexture", ExactSpelling = false)]
	private static extern IntPtr _aiMaterial_GetName(IntPtr pScene, int Layer);
	public static string aiMaterial_GetName(IntPtr pScene, int Layer)
	{
		return getString(_aiMaterial_GetName(pScene, Layer));
	}


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumVertices(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumFaces(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumBones(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetMaterialIndex(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumUVChannels(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumColorChannels(IntPtr pScene, int mesh);


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiMesh_GetName", ExactSpelling = false)]
	private static extern IntPtr _aiMesh_GetName(IntPtr pScene, int mesh);
	public static string aiMesh_GetName(IntPtr pScene, int mesh)
	{
		return getString(_aiMesh_GetName(pScene, mesh));
	}


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumIndicesPerFace(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_Indice(IntPtr pScene, int mesh, int Index, int Face);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasPositions(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasFaces(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasNormals(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasTangentsAndBitangents(IntPtr pScene, int mesh);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasVertexColors(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern bool aiMesh_HasTextureCoords(IntPtr pScene, int mesh, int Index);


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_Vertex(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_Normal(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_Tangent(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_Bitangent(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Color aiMesh_Color(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
	public static extern Vector3 aiMesh_TextureCoord(IntPtr pScene, int mesh, int Index, int Layer);



	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexX(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexY(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexZ(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexNX(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexNY(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_VertexNZ(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TangentX(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TangentY(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TangentZ(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_BitangentX(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_BitangentY(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_BitangentZ(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TextureCoordX(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_TextureCoordY(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_ColorRed(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_ColorGreen(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_ColorBlue(IntPtr pScene, int mesh, int Index, int Layer);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_ColorAlpha(IntPtr pScene, int mesh, int Index, int Layer);




	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiMesh_GetBoneName", ExactSpelling = false)]
	private static extern IntPtr _aiMesh_GetBoneName(IntPtr pScene, int mesh, int Index);
	public static string aiMesh_GetBoneName(IntPtr pScene, int mesh, int Index)
	{
		return getString(_aiMesh_GetBoneName(pScene, mesh,Index));
	}
	//! Matrix that transforms from mesh space to bone space in bind pose(bone->mOffsetMatrix)
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Matrix4x4 aiMesh_GetBoneTransform(IntPtr pNode, int mesh, int Index);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Quaternion aiMesh_GetBoneRotation(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneRotationX(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneRotationY(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneRotationZ(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneRotationW(IntPtr pNode, int mesh, int Index);


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_GetBoneEulerRotation(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneEulerRotationX(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneEulerRotationY(IntPtr pNode, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneEulerRotationZ(IntPtr pNode, int mesh, int Index);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiMesh_GetBonePosition(IntPtr pNode, int mesh, int Index);




	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetNumBoneWeights(IntPtr pScene, int mesh, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiMesh_GetBoneWeight(IntPtr pScene, int mesh, int Index, int Weight);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiMesh_GetBoneVertexId(IntPtr pScene, int mesh, int Index, int Weight);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetDuration(IntPtr pScene, int anim);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetTicksPerSecond(IntPtr pScene, int anim);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetNumChannels(IntPtr pScene, int anim);


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiAnim_GetName", ExactSpelling = false)]
	private static extern IntPtr _aiAnim_GetName(IntPtr pScene, int anim);
	public static string aiAnim_GetName(IntPtr pScene, int anim)
	{
		return getString(_aiAnim_GetName(pScene, anim));
	}

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiAnim_GetChannelName", ExactSpelling = false)]
	private static extern IntPtr _aiAnim_GetChannelName(IntPtr pScene, int anim, int Index);
	public static string aiAnim_GetChannelName(IntPtr pScene, int anim, int Index)
	{
		return getString(_aiAnim_GetChannelName(pScene, anim, Index));
	}


	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiAnim_GetPositionKey(IntPtr pScene, int anim, int Index, int key);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiAnim_GetScalingKey(IntPtr pScene, int anim, int Index, int key);

	//assimp quaternion is w,x,y,z by i fix this and the quaternion is x,y,z,w
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Quaternion aiAnim_GetRotationKey(IntPtr pScene, int anim, int Index, int key);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiAnim_GetEurlRotationKey(IntPtr pScene, int anim, int Index, int key);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetChannelIndex(IntPtr pScene, int anim, string name);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetNumPositionKeys(IntPtr pScene, int anim, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetNumRotationKeys(IntPtr pScene, int anim, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiAnim_GetNumScalingKeys(IntPtr pScene, int anim, int Index);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetPositionKeyX(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetPositionKeyY(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetPositionKeyZ(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetPositionFrame(IntPtr pScene, int anim, int Index, int Key);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetScalingKeyX(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetScalingKeyY(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetScalingKeyZ(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetScalingFrame(IntPtr pScene, int anim, int Index, int Key);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetRotationKeyX(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetRotationKeyY(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetRotationKeyZ(IntPtr pScene, int anim, int Index, int Key);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiAnim_GetRotationKeyW(IntPtr pScene, int anim, int Index, int Key);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern double aiAnim_GetRotationFrame(IntPtr pScene, int anim, int Index, int Key);



	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "aiNode_GetName", ExactSpelling = false)]
	private static extern IntPtr _aiNode_GetName(IntPtr pNode);
	public static string aiNode_GetName(IntPtr pNode)
	{
		return getString(_aiNode_GetName(pNode));
	}

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern int aiNode_GetNumChildren(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiNode_GetChild(IntPtr pNode, int Index);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiNode_GetParent(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern IntPtr aiNode_FindChild(IntPtr pNode, string name);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Matrix4x4 aiNode_GetTransformation(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]

	//assimp quaternion is w,x,y,z by i fix this and the quaternion is x,y,z,w
	public static extern Quaternion aiNode_GetRotation(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetRotationX(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetRotationY(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetRotationZ(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetRotationW(IntPtr pNode);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiNode_GetPosition(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetPositionX(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetPositionY(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetPositionZ(IntPtr pNode);

	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern Vector3 aiNode_GetEurlRotation(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetEurlRotationX(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetEurlRotationY(IntPtr pNode);
	[DllImport("assimp32.dll", CallingConvention = CallingConvention.StdCall)]
	public static extern float aiNode_GetEurlRotationZ(IntPtr pNode);
	 
 #endif
	
   

    public static string getString(IntPtr inString)
    {
        string str = Marshal.PtrToStringAnsi(inString);
        return str;
    }

}

