(*
---------------------------------------------------------------------------
Open Asset Import Library (ASSIMP)
---------------------------------------------------------------------------

Copyright (c) 2006-2010, ASSIMP Development Team

All rights reserved.

Redistribution and use of this software in source and binary forms, 
with or without modification, are permitted provided that the following 
conditions are met:

* Redistributions of source code must retain the above
  copyright notice, this list of conditions and the
  following disclaimer.

* Redistributions in binary form must reproduce the above
  copyright notice, this list of conditions and the
  following disclaimer in the documentation and/or other
  materials provided with the distribution.

* Neither the name of the ASSIMP team, nor the names of its
  contributors may be used to endorse or promote products
  derived from this software without specific prior
  written permission of the ASSIMP Development Team.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
'AS IS' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
---------------------------------------------------------------------------
*)

Unit Assimp;

Interface

Const
  AssimpLib = 'assimp-vc110-mt.dll';

Type

  TaiReturn = Integer;

PSingleArray = ^SingleArray;
SingleArray = Array[0..100000] Of Single;

PCardinalArray = ^CardinalArray;
CardinalArray = Array[0..1000] Of Cardinal;

aiMatrix3x3 = Packed Record
	v:Array[0..8] Of Single;
End;

aiMatrix4x4 = Packed Record
	v:Array[0..15] Of Single;
End;

aiQuaternion = Packed Record
	 w,	x, y, z:Single;
End;

aiVector3D = Packed Record
	x, y, z:Single;
End;

aiVector2D = Packed Record
	x, y:Single;
End;

aiColor4D = Packed Record
	// Red, green, blue and alpha color values 
	r, g, b, a:Single;
End;

Const
	MAXLEN = 1024;

Type
// Represents a plane in a three-dimensional, euclidean space
aiPlane = Packed Record
	//! Plane equation
	a,b,c,d:Single;
End;

// Represents a ray
aiRay = Packed Record
	//! Position and direction of the ray
	pos, dir:aiVector3D;
End;

// Represents a color in Red-Green-Blue space.
aiColor3D = Packed Record
	//! Red, green and blue color values
	r, g, b:Single;
End;  // !struct aiColor3D

(* Represents an UTF-8 string, zero byte terminated.
 *
 *  The character set of an aiString is explicitly defined to be UTF-8. This Unicode
 *  transformation was chosen in the belief that most strings in 3d files are limited
 *  to the ASCII characters, thus the character set needed to be ASCII compatible.
 *  
 *  Most text file loaders provide proper Unicode input file handling, special unicode
 *  characters are correctly transcoded to UTF8 and are kept throughout the libraries'
 *  import pipeline. 
 *
 *  For most applications, it will be absolutely sufficient to interpret the
 *  aiString as ASCII data and work with it as one would work with a plain char*. 
 *  Windows users in need of proper support for i.e asian characters can use the
 *  #MultiByteToWideChar(), #WideCharToMultiByte() WinAPI functionality to convert the
 *  UTF-8 strings to their working character set (i.e. MBCS, WideChar).
 *
 *  We use this representation instead of std::string to be C-compatible. The 
 *  (binary) length of such a string is limited to MAXLEN characters (including the
 *  the terminating zero).
*)
aiString = Packed Record
	(* Binary length of the string excluding the terminal 0. This is NOT the
	 *  logical length of strings containing UTF-8 multibyte sequences! It's
	 *  the number of bytes from the beginning of the string to its end.*)
	length:Integer;

	// String buffer. Size limit is MAXLEN */
	data:Array[0..Pred(MAXLEN)] Of Char;
End ;  // !struct aiString



// --------------------------------------------------------------------------------
///** Describes an file format which Assimp can export to. Use #aiGetExportFormatCount() to
//* learn how many export formats the current Assimp build supports and #aiGetExportFormatDescription()
//* to retrieve a description of an export format option.
 aiExportFormatDesc=packed record

    /// a short string ID to uniquely identify the export format. Use this ID string to
    /// specify which file format you want to export to when calling #aiExportScene().
    /// Example: "dae" or "obj"
     id:pchar;

    /// A short description of the file format to present to users. Useful if you want
    /// to allow the user to select an export format.
    description:pchar;

    /// Recommended file extension for the exported file in lower case.
     fileExtension:pchar;
end;
PaiExportFormatDesc= ^aiExportFormatDesc;


const
   AI_CONFIG_GLOB_MEASURE_TIME = 'GLOB_MEASURE_TIME';

        /// <summary>
        /// Sets Assimp's multithreading policy. This is ignored if Assimp is
        /// built without boost.thread support. Possible values are: -1 to
        /// let Assimp decide, 0 to disable multithreading, and nay number larger than 0
        /// to force a specific number of threads. This is only a hint and may be 
        /// ignored by Assimp.
        /// <para>Type: integer. Default: -1</para>
        /// </summary>
         AI_CONFIG_GLOB_MULTITHREADING = 'GLOB_MULTITHREADING';
        
   

        /// <summary>
        /// Specifies the maximum angle that may be between two vertex tangents that their tangents
        /// and bitangents are smoothed during the step to calculate the tangent basis. The angle specified 
        /// is in degrees. The maximum value is 175 degrees.
        /// <para>Type: float. Default: 45 degrees</para>
        /// </summary>
         AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE = 'PP_CT_MAX_SMOOTHING_ANGLE';

        /// <summary>
        /// Specifies the maximum angle that may be between two face normals at the same vertex position that
        /// their normals will be smoothed together during the calculate smooth normals step. This is commonly
        /// called the 'crease angle'. The angle is specified in degrees. Maximum value is 175 degrees (all vertices
        /// smoothed).
        /// <para>Type: float. Default: 175 degrees</para>
        /// </summary>
         AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE = 'PP_GSN_MAX_SMOOTHING_ANGLE';

        /// <summary>
        /// Sets the colormap(= palette) to be used to decode embedded textures in MDL (Quake or 3DG5) files.
        /// This must be a valid path to a file. The file is 768 (256 * 3) bytes large and contains
        /// RGB triplets for each of the 256 palette entries. If the file is not found, a default
        /// palette (from Quake 1) is used.
        /// <para>Type: string. Default: 'colormap.lmp'</para>
        /// </summary>
         AI_CONFIG_IMPORT_MDL_COLORMAP = 'IMPORT_MDL_COLORMAP';

        /// <summary>
        /// Configures the <see cref='PostProcessSteps.RemoveRedundantMaterials'/> step to
        /// keep materials matching a name in a given list. This is a list of
        /// 1 to n strings where whitespace ' ' serves as a delimiter character. Identifiers
        /// containing whitespaces must be enclosed in *single* quotation marks. Tabs or
        /// carriage returns are treated as whitespace.
        /// <para>If a material matches one of these names, it will not be modified
        /// or removed by the post processing step nor will other materials be replaced
        /// by a reference to it.</para>
        /// <para>Default: string. Default: ''</para>
        /// </summary>
         AI_CONFIG_PP_RRM_EXCLUDE_LIST = 'PP_RRM_EXCLUDE_LIST';

        /// <summary>
        /// Configures the <see cref='PostProcessSteps.PreTransformVertices'/> step
        /// to keep the scene hierarchy. Meshes are moved to worldspace, but no optimization
        /// is performed where meshes with the same materials are not joined.
        /// <para>This option could be of used if you have a scene hierarchy that contains
        /// important additional information which you intend to parse.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
         AI_CONFIG_PP_PTV_KEEP_HIERARCHY = 'PP_PTV_KEEP_HIERARCHY';

        /// <summary>
        /// Configures the <see cref='PostProcessSteps.PreTransformVertices'/> step
        /// to normalize all vertex components into the -1...1 range. That is, a bounding
        /// box for the whole scene is computed where the maximum component is taken
        /// and all meshes are scaled uniformly. This is useful if you don't know the spatial dimension
        /// of the input data.
        /// <para>Type: bool. Default: false</para>
        /// </summary>
         AI_CONFIG_PP_PTV_NORMALIZE = 'PP_PTV_NORMALIZE';

        /// <summary>
        /// Configures the <see cref='PostProcessSteps.FindDegenerates'/> step
        /// to remove degenerated primitives from the import immediately.
        /// <para>The default behavior converts degenerated triangles to lines and
        /// degenerated lines to points.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
         AI_CONFIG_PP_FD_REMOVE = 'PP_FD_REMOVE';

        /// <summary>
        /// Configures the <see cref='PostProcessSteps.OptimizeGraph'/> step
        /// to preserve nodes matching a name in a given list. This is a list of 1 to n strings, whitespace ' ' serves as a delimter character.
        /// Identifiers containing whitespaces must be enclosed in *single* quotation marks. Carriage returns
        /// and tabs are treated as white space.
        /// <para>If a node matches one of these names, it will not be modified or removed by the
        /// postprocessing step.</para>
        /// <para>Type: string. Default: ''</para>
        /// </summary>
         AI_CONFIG_PP_OG_EXCLUDE_LIST = 'PP_OG_EXCLUDE_LIST';

        /// <summary>
        /// Sets the maximum number of triangles a mesh can contain. This is used by the
        /// <see cref='PostProcessSteps.SplitLargeMeshes'/> step to determine
        /// whether a mesh must be split or not.
        /// <para>Type: int. Default: AiDefines.AI_SLM_DEFAULT_MAX_TRIANGLES</para>
        /// </summary>
         AI_CONFIG_PP_SLM_TRIANGLE_LIMIT = 'PP_SLM_TRIANGLE_LIMIT';

        /// <summary>
        /// Sets the maximum number of vertices in a mesh. This is used by the
        /// <see cref='PostProcessSteps.SplitLargeMeshes'/> step to determine
        /// whether a mesh must be split or not.
        /// <para>Type: integer. Default: AiDefines.AI_SLM_DEFAULT_MAX_VERTICES</para>
        /// </summary>
         AI_CONFIG_PP_SLM_VERTEX_LIMIT = 'PP_SLM_VERTEX_LIMIT';

        /// <summary>
        /// Sets the maximum number of bones that can affect a single vertex. This is used
        /// by the <see cref='PostProcessSteps.LimitBoneWeights'/> step.
        /// <para>Type: integer. Default: AiDefines.AI_LBW_MAX_WEIGHTS</para>
        /// </summary>
         AI_CONFIG_PP_LBW_MAX_WEIGHTS = 'PP_LBW_MAX_WEIGHTS';

        /// <summary>
        /// Sets the size of the post-transform vertex cache to optimize vertices for. This is
        /// for the <see cref='PostProcessSteps.ImproveCacheLocality'/> step. The size
        /// is given in vertices. Of course you can't know how the vertex format will exactly look
        /// like after the import returns, but you can still guess what your meshes will
        /// probably have. The default value *has* resulted in slight performance improvements
        /// for most Nvidia/AMD cards since 2002.
        /// <para>Type: integer. Default: AiDefines.PP_ICL_PTCACHE_SIZE</para>
        /// </summary>
         AI_CONFIG_PP_ICL_PTCACHE_SIZE = 'PP_ICL_PTCACHE_SIZE';

        /// <summary>
        /// Input parameter to the <see cref='PostProcessSteps.RemoveComponent'/> step. 
        /// It specifies the parts of the data structure to be removed.
        /// <para>This is a bitwise combination of the <see cref='ExcludeComponent'/> flag. If no valid mesh is remaining after
        /// the step is executed, the import FAILS.</para>
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
         AI_CONFIG_PP_RVC_FLAGS = 'PP_RVC_FLAGS';

        /// <summary>
        /// Input parameter to the <see cref='PostProcessSteps.SortByPrimitiveType'/> step.
        /// It specifies which primitive types are to be removed by the step.
        /// <para>This is a bitwise combination of the <see cref='PrimitiveType'/> flag.
        /// Specifying ALL types is illegal.</para>
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
         AI_CONFIG_PP_SBP_REMOVE = 'PP_SBP_REMOVE';

        /// <summary>
        /// Input parameter to the <see cref='PostProcessSteps.FindInvalidData'/> step.
        /// It specifies the floating point accuracy for animation values, specifically the epislon
        /// during the comparison. The step checks for animation tracks where all frame values are absolutely equal 
        /// and removes them. Two floats are considered equal if the invariant <c>abs(n0-n1) > epislon</c> holds
        /// true for all vector/quaternion components.
        /// <para>Type: float. Default: 0.0f (comparisons are exact)</para>
        /// </summary>
         AI_CONFIG_PP_FID_ANIM_ACCURACY = 'PP_FID_ANIM_ACCURACY';

        /// <summary>
        /// Input parameter to the <see cref='PostProcessSteps.TransformUVCoords'/> step.
        /// It specifies which UV transformations are to be evaluated.
        /// <para>This is bitwise combination of the <see cref='UVTransformFlags'/> flag.</para>
        /// <para>Type: integer. Default: AiDefines.AI_UV_TRAFO_ALL (All combinations)</para>
        /// </summary>
         AI_CONFIG_PP_TUV_EVALUATE = 'PP_TUV_EVALUATE';

        /// <summary>
        /// A hint to Assimp to favour speed against import quality. Enabling this option
        /// may result in faster loading, or it may not. It is just a hint to loaders and post-processing
        /// steps to use faster code paths if possible. A value not equal to zero stands
        /// for true.
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
         AI_CONFIG_FAVOUR_SPEED = 'FAVOUR_SPEED';

        /// <summary>
        /// Maximum bone cone per mesh for the <see cref='PostProcessSteps.SplitByBoneCount'/> step. Meshes
        /// are split until the max number of bones is reached.
        /// <para>Type: integer. Default: 60</para>
        /// </summary>
         AI_CONFIG_PP_SBBC_MAX_BONES = 'PP_SBBC_MAX_BONES';

        /// <summary>
        /// Source UV channel for tangent space computation. The specified channel must exist or an error will be raised.
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
         AI_CONFIG_PP_CT_TEXTURE_CHANNEL_INDEX = 'AI_CONFIG_PP_CT_TEXTURE_CHANNEL_INDEX';

        /// <summary>
        /// Threshold used to determine if a bone is kept or removed during the <see cref='PostProcessSteps.Debone'/> step.
        /// <para>Type: float. Default: 1.0f</para>
        /// </summary>
         AI_CONFIG_PP_DB_THRESHOLD = 'PP_DB_THRESHOLD';

        /// <summary>
        /// Require all bones to qualify for deboning before any are removed.
        /// <para>Type: bool. Default: false</para>
        /// </summary>
         AI_CONFIG_PP_DB_ALL_OR_NONE = 'PP_DB_ALL_OR_NONE';

      
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
         AI_CONFIG_IMPORT_GLOBAL_KEYFRAME = 'IMPORT_GLOBAL_KEYFRAME';

        /// <summary>
        /// See the documentation for <see cref='AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME'/>.
        /// </summary>
         AI_CONFIG_IMPORT_MD3_KEYFRAME = 'IMPORT_MD3_KEYFRAME';

        /// <summary>
        /// See the documentation for <see cref='AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME'/>.
        /// </summary>
         AI_CONFIG_IMPORT_MD2_KEYFRAME = 'IMPORT_MD3_KEYFRAME';

        /// <summary>
        /// See the documentation for <see cref='AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME'/>.
        /// </summary>
         AI_CONFIG_IMPORT_MDL_KEYFRAME = 'IMPORT_MDL_KEYFRAME';

        /// <summary>
        /// See the documentation for <see cref='AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME'/>.
        /// </summary>
         AI_CONFIG_IMPORT_MDC_KEYFRAME = 'IMPORT_MDC_KEYFRAME';

        /// <summary>
        /// See the documentation for <see cref='AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME'/>.
        /// </summary>
         AI_CONFIG_IMPORT_SMD_KEYFRAME = 'IMPORT_SMD_KEYFRAME';

        /// <summary>
        /// See the documentation for <see cref='AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME'/>.
        /// </summary>
         AI_CONFIG_IMPORT_UNREAL_KEYFRAME = 'IMPORT_UNREAL_KEYFRAME';

        /// <summary>
        /// Configures the AC loader to collect all surfaces which have the 'Backface cull' flag set in separate
        /// meshes.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
         AI_CONFIG_IMPORT_AC_SEPARATE_BFCULL = 'IMPORT_AC_SEPARATE_BFCULL';

        /// <summary>
        /// Configures whether the AC loader evaluates subdivision surfaces (indicated by the presence
        /// of the 'subdiv' attribute in the file). By default, Assimp performs
        /// the subdivision using the standard Catmull-Clark algorithm.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
         AI_CONFIG_IMPORT_AC_EVAL_SUBDIVISION = 'IMPORT_AC_EVAL_SUBDIVISION';

        /// <summary>
        /// Configures the UNREAL 3D loader to separate faces with different surface flags (e.g. two-sided vs single-sided).
        /// <para>Type: bool. Default: true</para>
        /// </summary>
         AI_CONFIG_IMPORT_UNREAL_HANDLE_FLAGS = 'UNREAL_HANDLE_FLAGS';

        /// <summary>
        /// Configures the terragen import plugin to compute UV's for terrains, if
        /// they are not given. Furthermore, a default texture is assigned.
        /// <para>UV coordinates for terrains are so simple to compute that you'll usually 
        /// want to compute them on your own, if you need them. This option is intended for model viewers which
        /// want to offer an easy way to apply textures to terrains.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
         AI_CONFIG_IMPORT_TER_MAKE_UVS = 'IMPORT_TER_MAKE_UVS';

        /// <summary>
        /// Configures the ASE loader to always reconstruct normal vectors basing on the smoothing groups
        /// loaded from the file. Some ASE files carry invalid normals, others don't.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
         AI_CONFIG_IMPORT_ASE_RECONSTRUCT_NORMALS = 'IMPORT_ASE_RECONSTRUCT_NORMALS';

        /// <summary>
        /// Configures the M3D loader to detect and process multi-part Quake player models. These models
        /// usually consit of three files, lower.md3, upper.md3 and head.md3. If this propery is
        /// set to true, Assimp will try to load and combine all three files if one of them is loaded.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
         AI_CONFIG_IMPORT_MD3_HANDLE_MULTIPART = 'IMPORT_MD3_HANDLE_MULTIPART';

        /// <summary>
        /// Tells the MD3 loader which skin files to load. When loading MD3 files, Assimp checks
        /// whether a file named 'md3_file_name'_'skin_name'.skin exists. These files are used by
        /// Quake III to be able to assign different skins (e.g. red and blue team) to models. 'default', 'red', 'blue'
        /// are typical skin names.
        /// <para>Type: string. Default: 'default'</para>
        /// </summary>
         AI_CONFIG_IMPORT_MD3_SKIN_NAME = 'IMPORT_MD3_SKIN_NAME';

        /// <summary>
        /// Specifies the Quake 3 shader file to be used for a particular MD3 file. This can be a full path or
        /// relative to where all MD3 shaders reside.
        /// <para>Type: string. Default: ''</para>
        /// </summary>
         AI_CONFIG_IMPORT_MD3_SHADER_SRC = 'IMPORT_MD3_SHADER_SRC';

        /// <summary>
        /// Configures the LWO loader to load just one layer from the model.
        /// <para>LWO files consist of layers and in some cases it could be useful to load only one of them.
        /// This property can be either a string - which specifies the name of the layer - or an integer - the index
        /// of the layer. If the property is not set then the whole LWO model is loaded. Loading fails
        /// if the requested layer is not vailable. The layer index is zero-based and the layer name may not be empty</para>
        /// <para>Type: bool. Default: false (All layers are loaded)</para>
        /// </summary>
         AI_CONFIG_IMPORT_LWO_ONE_LAYER_ONLY = 'IMPORT_LWO_ONE_LAYER_ONLY';

        /// <summary>
        /// Configures the MD5 loader to not load the MD5ANIM file for a MD5MESH file automatically.
        /// <para>The default strategy is to look for a file with the same name but with the MD5ANIm extension
        /// in the same directory. If it is found it is loaded and combined with the MD5MESH file. This configuration
        /// option can be used to disable this behavior.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
         AI_CONFIG_IMPORT_MD5_NO_ANIM_AUTOLOAD = 'IMPORT_MD5_NO_ANIM_AUTOLOAD';

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
         AI_CONFIG_IMPORT_LWS_ANIM_START = 'IMPORT_LWS_ANIM_START';

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
         AI_CONFIG_IMPORT_LWS_ANIM_END = 'IMPORT_LWS_ANIM_END';

        /// <summary>
        /// Defines the output frame rate of the IRR loader.
        /// <para>IRR animations are difficult to convert for Assimp and there will always be
        /// a loss of quality. This setting defines how many keys per second are returned by the converter.</para>
        /// <para>Type: integer. Default: 100</para>
        /// </summary>
         AI_CONFIG_IMPORT_IRR_ANIM_FPS = 'IMPORT_IRR_ANIM_FPS';

        /// <summary>
        /// The Ogre importer will try to load this MaterialFile. If a material file does not
        /// exist with the same name as a material to load, the ogre importer will try to load this file
        /// and searches for the material in it.
        /// <para>Type: string. Default: ''</para>
        /// </summary>
         AI_CONFIG_IMPORT_OGRE_MATERIAL_FILE = 'IMPORT_OGRE_MATERIAL_FILE';

        /// <summary>
        /// The Ogre importer will detect the texture usage from the filename. Normally a texture is loaded as a color map, if no target is specified
        /// in the material file. If this is enabled, texture names ending with _n, _l, _s are used as normal maps, light maps, or specular maps.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
         AI_CONFIG_IMPORT_OGRE_TEXTURETYPE_FROM_FILENAME = 'IMPORT_OGRE_TEXTURETYPE_FROM_FILENAME';

        /// <summary>
        /// Specifies whether the IFC loader skips over shape representations of type 'Curve2D'. A lot of files contain both a faceted mesh representation and a outline 
        /// with a presentation type of 'Curve2D'. Currently Assimp does not convert those, so turning this option off just clutters the log with errors.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
         AI_CONFIG_IMPORT_IFC_SKIP_CURVE_REPRESENTATIONS = 'IMPORT_IFC_SKIP_CURVE_REPRESENTATIONS';

        /// <summary>
        /// Specifies whether the IFC loader will use its own, custom triangulation algorithm to triangulate wall and floor meshes. If this is set to false,
        /// walls will be either triangulated by the post process triangulation or will be passed through as huge polygons with faked holes (e.g. holes that are connected
        /// with the outer boundary using a dummy edge). It is highly recommended to leave this property set to true as the default post process has some known
        /// issues with these kind of polygons.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
         AI_CONFIG_IMPORT_IFC_CUSTOM_TRIANGULATION = 'IMPORT_IFC_CUSTOM_TRIANGULATION';

Const
	(* <hr>Calculates the tangents and bitangents for the imported meshes.
	 *
	 * Does nothing if a mesh does not have normals. You might want this post
	 * processing step to be executed if you plan to use tangent space calculations
	 * such as normal mapping  applied to the meshes. There's a config setting;
	 * <tt>#AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE</tt>; which allows you to specify
	 * a maximum smoothing angle for the algorithm. However; usually you'll
	 * want to leave it at the default value. Thanks.
	 *)
	aiProcess_CalcTangentSpace = $1;

 (* <hr>Identifies and joins identical vertex data sets within all
	 *  imported meshes.
	 *
	 * After this step is run each mesh does contain only unique vertices anymore;
	 * so a vertex is possibly used by multiple faces. You usually want
	 * to use this post processing step. If your application deals with
	 * indexed geometry; this step is compulsory or you'll just waste rendering
	 * time. <b>If this flag is not specified</b>; no vertices are referenced by
	 * more than one face and <b>no index buffer is required</b> for rendering.
	 *)
	aiProcess_JoinIdenticalVertices = $2;

	(* <hr>Converts all the imported data to a left-handed coordinate space.
	 *
	 * By default the data is returned in a right-handed coordinate space which
	 * for example OpenGL prefers. In this space; +X points to the right;
	 * +Z points towards the viewer and and +Y points upwards. In the DirectX
     * coordinate space +X points to the right; +Y points upwards and +Z points
     * away from the viewer.
	 *
	 * You'll probably want to consider this flag if you use Direct3D for
	 * rendering. The #aiProcess_ConvertToLeftHanded flag supersedes this
	 * setting and bundles all conversions typically required for D3D-based
	 * applications.
	 *)
	aiProcess_MakeLeftHanded = $4;

	(* <hr>Triangulates all faces of all meshes.
	 *
	 * By default the imported mesh data might contain faces with more than 3
	 * indices. For rendering you'll usually want all faces to be triangles.
	 * This post processing step splits up all higher faces to triangles.
	 * Line and point primitives are *not* modified!. If you want
	 * 'triangles only' with no other kinds of primitives; try the following
	 * solution:
	 * <ul>
	 * <li>Specify both #aiProcess_Triangulate and #aiProcess_SortByPType </li>
	 * </li>Ignore all point and line meshes when you process assimp's output</li>
	 * </ul>
	 *)
	aiProcess_Triangulate = $8;

	(* <hr>Removes some parts of the data structure (animations; materials;
	 *  light sources; cameras; textures; vertex components).
	 *
	 * The  components to be removed are specified in a separate
	 * configuration option; <tt>#AI_CONFIG_PP_RVC_FLAGS</tt>. This is quite useful
	 * if you don't need all parts of the output structure. Especially vertex
	 * colors are rarely used today ... . Calling this step to remove unrequired
	 * stuff from the pipeline as early as possible results in an increased
	 * performance and a better optimized output data structure.
	 * This step is also useful if you want to force Assimp to recompute
	 * normals or tangents. The corresponding steps don't recompute them if
	 * they're already there (loaded from the source asset). By using this
	 * step you can make sure they are NOT there.
	 *
	 * This flag is a poor one; mainly because its purpose is usually
     * misunderstood. Consider the following case: a 3d model has been exported
	 * from a CAD app; it has per-face vertex colors. Vertex positions can't be
	 * shared; thus the #aiProcess_JoinIdenticalVertices step fails to
	 * optimize the data. Just because these nasty; little vertex colors.
	 * Most apps don't even process them; so it's all for nothing. By using
	 * this step; unneeded components are excluded as early as possible
	 * thus opening more room for internal optimzations.
	 *)
	aiProcess_RemoveComponent = $10;

	(* <hr>Generates normals for all faces of all meshes.
	 *
	 * This is ignored if normals are already there at the time where this flag
	 * is evaluated. Model importers try to load them from the source file; so
	 * they're usually already there. Face normals are shared between all points
	 * of a single face; so a single point can have multiple normals; which in
	 * other words; enforces the library to duplicate vertices in some cases.
	 * #aiProcess_JoinIdenticalVertices is *senseless* then.
	 *
	 * This flag may not be specified together with #aiProcess_GenSmoothNormals.
	 *)
	aiProcess_GenNormals = $20;

	(* <hr>Generates smooth normals for all vertices in the mesh.
	*
	* This is ignored if normals are already there at the time where this flag
	* is evaluated. Model importers try to load them from the source file; so
	* they're usually already there.
	*
	* This flag may (of course) not be specified together with
	* #aiProcess_GenNormals. There's a configuration option;
	* <tt>#AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE</tt> which allows you to specify
	* an angle maximum for the normal smoothing algorithm. Normals exceeding
	* this limit are not smoothed; resulting in a a 'hard' seam between two faces.
	* Using a decent angle here (e.g. 80°) results in very good visual
	* appearance.
	*)
	aiProcess_GenSmoothNormals = $40;

	(* <hr>Splits large meshes into smaller submeshes
	*
	* This is quite useful for realtime rendering where the number of triangles
	* which can be maximally processed in a single draw-call is usually limited
	* by the video driver/hardware. The maximum vertex buffer is usually limited;
	* too. Both requirements can be met with this step: you may specify both a
	* triangle and vertex limit for a single mesh.
	*
	* The split limits can (and should!) be set through the
	* <tt>#AI_CONFIG_PP_SLM_VERTEX_LIMIT</tt> and <tt>#AI_CONFIG_PP_SLM_TRIANGLE_LIMIT</tt>
	* settings. The default values are <tt>#AI_SLM_DEFAULT_MAX_VERTICES</tt> and
	* <tt>#AI_SLM_DEFAULT_MAX_TRIANGLES</tt>.
	*
	* Note that splitting is generally a time-consuming task; but not if there's
	* nothing to split. The use of this step is recommended for most users.
	*)
	aiProcess_SplitLargeMeshes = $80;

	(* <hr>Removes the node graph and pre-transforms all vertices with
	* the local transformation matrices of their nodes. The output
	* scene does still contain nodes; however; there is only a
	* root node with children; each one referencing only one mesh;
	* each mesh referencing one material. For rendering; you can
	* simply render all meshes in order; you don't need to pay
	* attention to local transformations and the node hierarchy.
	* Animations are removed during this step.
	* This step is intended for applications without a scenegraph.
	* The step CAN cause some problems: if e.g. a mesh of the asset
	* contains normals and another; using the same material index; does not;
	* they will be brought together; but the first meshes's part of
	* the normal list is zeroed. However; these artifacts are rare.
	* @note The <tt>#AI_CONFIG_PP_PTV_NORMALIZE</tt> configuration property
	* can be set to normalize the scene's spatial dimension to the -1...1
	* range.
	*)
	aiProcess_PreTransformVertices = $100;

	(* <hr>Limits the number of bones simultaneously affecting a single vertex
	*  to a maximum value.
	*
	* If any vertex is affected by more than that number of bones; the least
	* important vertex weights are removed and the remaining vertex weights are
	* renormalized so that the weights still sum up to 1.
	* The default bone weight limit is 4 (defined as <tt>#AI_LMW_MAX_WEIGHTS</tt> in
	* aiConfig.h); but you can use the <tt>#AI_CONFIG_PP_LBW_MAX_WEIGHTS</tt> setting to
	* supply your own limit to the post processing step.
	*
	* If you intend to perform the skinning in hardware; this post processing
	* step might be of interest for you.
	*)
	aiProcess_LimitBoneWeights = $200;

	(* <hr>Validates the imported scene data structure
	 * This makes sure that all indices are valid; all animations and
	 * bones are linked correctly; all material references are correct .. etc.
	 *
	 * It is recommended to capture Assimp's log output if you use this flag;
	 * so you can easily find ot what's actually wrong if a file fails the
	 * validation. The validator is quite rude and will find *all*
	 * inconsistencies in the data structure ... plugin developers are
	 * recommended to use it to debug their loaders. There are two types of
	 * validation failures:
	 * <ul>
	 * <li>Error: There's something wrong with the imported data. Further
	 *   postprocessing is not possible and the data is not usable at all.
	 *   The import fails. #Importer::GetErrorString() or #aiGetErrorString()
	 *   carry the error message around.</li>
	 * <li>Warning: There are some minor issues (e.g. 1000000 animation
	 *   keyframes with the same time); but further postprocessing and use
	 *   of the data structure is still safe. Warning details are written
	 *   to the log file; <tt>#AI_SCENE_FLAGS_VALIDATION_WARNING</tt> is set
	 *   in #aiScene::mFlags</li>
	 * </ul>
	 *
	 * This post-processing step is not time-consuming. It's use is not
	 * compulsory; but recommended.
	*)
	aiProcess_ValidateDataStructure = $400;

	(* <hr>Reorders triangles for better vertex cache locality.
	 *
	 * The step tries to improve the ACMR (average post-transform vertex cache
	 * miss ratio) for all meshes. The implementation runs in O(n) and is
	 * roughly based on the 'tipsify' algorithm (see <a href='
	 * http://www.cs.princeton.edu/gfx/pubs/Sander_2007_%3ETR/tipsy.pdf'>this
	 * paper</a>).
	 *
	 * If you intend to render huge models in hardware; this step might
	 * be of interest for you. The <tt>#AI_CONFIG_PP_ICL_PTCACHE_SIZE</tt>config
	 * setting can be used to fine-tune the cache optimization.
	 *)
	aiProcess_ImproveCacheLocality = $800;

	(* <hr>Searches for redundant/unreferenced materials and removes them.
	 *
	 * This is especially useful in combination with the
	 * #aiProcess_PretransformVertices and #aiProcess_OptimizeMeshes flags.
	 * Both join small meshes with equal characteristics; but they can't do
	 * their work if two meshes have different materials. Because several
	 * material settings are always lost during Assimp's import filters;
	 * (and because many exporters don't check for redundant materials); huge
	 * models often have materials which are are defined several times with
	 * exactly the same settings ..
	 *
	 * Several material settings not contributing to the final appearance of
	 * a surface are ignored in all comparisons ... the material name is
	 * one of them. So; if you're passing additional information through the
	 * content pipeline (probably using *magic* material names); don't
	 * specify this flag. Alternatively take a look at the
	 * <tt>#AI_CONFIG_PP_RRM_EXCLUDE_LIST</tt> setting.
	 *)
	aiProcess_RemoveRedundantMaterials = $1000;

	(* <hr>This step tries to determine which meshes have normal vectors
	 * that are facing inwards. The algorithm is simple but effective:
	 * the bounding box of all vertices + their normals is compared against
	 * the volume of the bounding box of all vertices without their normals.
	 * This works well for most objects; problems might occur with planar
	 * surfaces. However; the step tries to filter such cases.
	 * The step inverts all in-facing normals. Generally it is recommended
	 * to enable this step; although the result is not always correct.
	*)
	aiProcess_FixInfacingNormals = $2000;

	(* <hr>This step splits meshes with more than one primitive type in
	 *  homogeneous submeshes.
	 *
	 *  The step is executed after the triangulation step. After the step
	 *  returns; just one bit is set in aiMesh::mPrimitiveTypes. This is
	 *  especially useful for real-time rendering where point and line
	 *  primitives are often ignored or rendered separately.
	 *  You can use the <tt>#AI_CONFIG_PP_SBP_REMOVE</tt> option to specify which
	 *  primitive types you need. This can be used to easily exclude
	 *  lines and points; which are rarely used; from the import.
	*)
	aiProcess_SortByPType = $8000;

	(* <hr>This step searches all meshes for degenerated primitives and
	 *  converts them to proper lines or points.
	 *
	 * A face is 'degenerated' if one or more of its points are identical.
	 * To have the degenerated stuff not only detected and collapsed but
	 * also removed; try one of the following procedures:
	 * <br><b>1.</b> (if you support lines&points for rendering but don't
	 *    want the degenerates)</br>
	 * <ul>
	 *   <li>Specify the #aiProcess_FindDegenerates flag.
	 *   </li>
	 *   <li>Set the <tt>AI_CONFIG_PP_FD_REMOVE</tt> option to 1. This will
	 *       cause the step to remove degenerated triangles from the import
	 *       as soon as they're detected. They won't pass any further
	 *       pipeline steps.
	 *   </li>
	 * </ul>
	 * <br><b>2.</b>(if you don't support lines&points at all ...)</br>
	 * <ul>
	 *   <li>Specify the #aiProcess_FindDegenerates flag.
	 *   </li>
	 *   <li>Specify the #aiProcess_SortByPType flag. This moves line and
	 *     point primitives to separate meshes.
	 *   </li>
	 *   <li>Set the <tt>AI_CONFIG_PP_SBP_REMOVE</tt> option to
	 *       @code aiPrimitiveType_POINTS | aiPrimitiveType_LINES
	 *       @endcode to cause SortByPType to reject point
	 *       and line meshes from the scene.
	 *   </li>
	 * </ul>
	 * @note Degenerated polygons are not necessarily evil and that's why
	 * they're not removed by default. There are several file formats which
	 * don't support lines or points ... some exporters bypass the
	 * format specification and write them as degenerated triangle instead.
	*)
	aiProcess_FindDegenerates = $10000;

	(* <hr>This step searches all meshes for invalid data; such as zeroed
	 *  normal vectors or invalid UV coords and removes/fixes them. This is
	 *  intended to get rid of some common exporter errors.
	 *
	 * This is especially useful for normals. If they are invalid; and
	 * the step recognizes this; they will be removed and can later
	 * be recomputed; i.e. by the #aiProcess_GenSmoothNormals flag.<br>
	 * The step will also remove meshes that are infinitely small and reduce
	 * animation tracks consisting of hundreds if redundant keys to a single
	 * key. The <tt>AI_CONFIG_PP_FID_ANIM_ACCURACY</tt> config property decides
	 * the accuracy of the check for duplicate animation tracks.
	*)
	aiProcess_FindInvalidData = $20000;

	(* <hr>This step converts non-UV mappings (such as spherical or
	 *  cylindrical mapping) to proper texture coordinate channels.
	 *
	 * Most applications will support UV mapping only; so you will
	 * probably want to specify this step in every case. Note tha Assimp is not
	 * always able to match the original mapping implementation of the
	 * 3d app which produced a model perfectly. It's always better to let the
	 * father app compute the UV channels; at least 3ds max; maja; blender;
	 * lightwave; modo; ... are able to achieve this.
	 *
	 * @note If this step is not requested; you'll need to process the
	 * <tt>#AI_MATKEY_MAPPING</tt> material property in order to display all assets
	 * properly.
	 *)
	aiProcess_GenUVCoords = $40000;

	(* <hr>This step applies per-texture UV transformations and bakes
	 *  them to stand-alone vtexture coordinate channelss.
	 *
	 * UV transformations are specified per-texture - see the
	 * <tt>#AI_MATKEY_UVTRANSFORM</tt> material key for more information.
	 * This step processes all textures with
	 * transformed input UV coordinates and generates new (pretransformed) UV channel
	 * which replace the old channel. Most applications won't support UV
	 * transformations; so you will probably want to specify this step.
     *
	 * @note UV transformations are usually implemented in realtime apps by
	 * transforming texture coordinates at vertex shader stage with a 3x3
	 * (homogenous) transformation matrix.
	*)
	aiProcess_TransformUVCoords = $80000;

	(* <hr>This step searches for duplicate meshes and replaces duplicates
	 *  with references to the first mesh.
	 *
	 *  This step takes a while; don't use it if you have no time.
	 *  Its main purpose is to workaround the limitation that many export
	 *  file formats don't support instanced meshes; so exporters need to
	 *  duplicate meshes. This step removes the duplicates again. Please
 	 *  note that Assimp does currently not support per-node material
	 *  assignment to meshes; which means that identical meshes with
	 *  differnent materials are currently *not* joined; although this is
	 *  planned for future versions.
	 *)
	aiProcess_FindInstances = $100000;

	(* <hr>A postprocessing step to reduce the number of meshes.
	 *
	 *  In fact; it will reduce the number of drawcalls.
	 *
	 *  This is a very effective optimization and is recommended to be used
	 *  together with #aiProcess_OptimizeGraph; if possible. The flag is fully
	 *  compatible with both #aiProcess_SplitLargeMeshes and #aiProcess_SortByPType.
	*)
	aiProcess_OptimizeMeshes  = $200000;

	(* <hr>A postprocessing step to optimize the scene hierarchy.
	 *
	 *  Nodes with no animations; bones; lights or cameras assigned are
	 *  collapsed and joined.
	 *
	 *  Node names can be lost during this step. If you use special 'tag nodes'
	 *  to pass additional information through your content pipeline; use the
	 *  <tt>#AI_CONFIG_PP_OG_EXCLUDE_LIST</tt> setting to specify a list of node
	 *  names you want to be kept. Nodes matching one of the names in this list won't
	 *  be touched or modified.
	 *
	 *  Use this flag with caution. Most simple files will be collapsed to a
	 *  single node; complex hierarchies are usually completely lost. That's not
	 *  the right choice for editor environments; but probably a very effective
	 *  optimization if you just want to get the model data; convert it to your
	 *  own format and render it as fast as possible.
	 *
	 *  This flag is designed to be used with #aiProcess_OptimizeMeshes for best
	 *  results.
	 *
	 *  @note 'crappy' scenes with thousands of extremely small meshes packed
	 *  in deeply nested nodes exist for almost all file formats.
	 *  #aiProcess_OptimizeMeshes in combination with #aiProcess_OptimizeGraph
	 *  usually fixes them all and makes them renderable.
	*)
	aiProcess_OptimizeGraph  = $400000;

	(* <hr>This step flips all UV coordinates along the y-axis and adjusts
	 * material settings and bitangents accordingly.
	 * <br><b>Output UV coordinate system:</b>
	 * @code
	 * 0y|0y ---------- 1x|0y
     * |                 |
     * |                 |
     * |                 |
     * 0x|1y ---------- 1x|1y
	 * @endcode
	 *
	 * You'll probably want to consider this flag if you use Direct3D for
	 * rendering. The #aiProcess_ConvertToLeftHanded flag supersedes this
	 * setting and bundles all conversions typically required for D3D-based
	 * applications.
	*)
	aiProcess_FlipUVs = $800000;

	(* <hr>This step adjusts the output face winding order to be cw.
	 *
	 * The default face winding order is counter clockwise.
	 * <br><b>Output face order:</b>
	 * @code
	 *       x2
	 *
	 *                         x0
	 *  x1
	 * @endcode
	*)
	aiProcess_FlipWindingOrder  = $1000000;

	// aiProcess_GenEntityMeshes = 0x100000;
	// aiProcess_OptimizeAnimations = 0x200000
	// aiProcess_FixTexturePaths = 0x200000

(* @def aiProcessPreset_TargetRealtimeUse_Fast
 *  @brief Default postprocess configuration optimizing the data for real-time rendering.
 *
 *  Applications would want to use this preset to load models on end-user PCs;
 *  maybe for direct use in game.
 *
 * If you're using DirectX; don't forget to combine this value with
 * the #aiProcess_ConvertToLeftHanded step. If you don't support UV transformations
 * in your application apply the #aiProcess_TransformUVCoords step; too.
 *  @note Please take the time to read the doc to the steps enabled by this preset.
 *  Some of them offer further configurable properties, some of them might not be of
 *  use for you so it might be better to not specify them.
 *)
{#define aiProcessPreset_TargetRealtime_Fast ( \
	aiProcess_CalcTangentSpace		|  \
	aiProcess_GenNormals			|  \
	aiProcess_JoinIdenticalVertices |  \
	aiProcess_Triangulate			|  \
	aiProcess_GenUVCoords           |  \
	aiProcess_SortByPType           |  \
	0 )
 }

 (** @def aiProcessPreset_TargetRealtime_Quality
  *  @brief Default postprocess configuration optimizing the data for real-time rendering.
  *
  *  Unlike #aiProcessPreset_TargetRealtime_Fast, this configuration
  *  performs some extra optimizations to improve rendering speed and
  *  to minimize memory usage. It could be a good choice for a level editor
  *  environment where import speed is not so important.
  *
  *  If you're using DirectX, don't forget to combine this value with
  *  the #aiProcess_ConvertToLeftHanded step. If you don't support UV transformations
  *  in your application apply the #aiProcess_TransformUVCoords step, too.
  *  @note Please take the time to read the doc for the steps enabled by this preset.
  *  Some of them offer further configurable properties, some of them might not be of
  *  use for you so it might be better to not specify them.
  *)
{#define aiProcessPreset_TargetRealtime_Quality ( \
	aiProcess_CalcTangentSpace				|  \
	aiProcess_GenSmoothNormals				|  \
	aiProcess_JoinIdenticalVertices			|  \
	aiProcess_ImproveCacheLocality			|  \
	aiProcess_LimitBoneWeights				|  \
	aiProcess_RemoveRedundantMaterials      |  \
	aiProcess_SplitLargeMeshes				|  \
	aiProcess_Triangulate					|  \
	aiProcess_GenUVCoords                   |  \
	aiProcess_SortByPType                   |  \
	aiProcess_FindDegenerates               |  \
	aiProcess_FindInvalidData               |  \
	0 )
}

(* @def aiProcessPreset_TargetRealtime_MaxQuality
  *  @brief Default postprocess configuration optimizing the data for real-time rendering.
  *
  *  This preset enables almost every optimization step to achieve perfectly
  *  optimized data. It's your choice for level editor environments where import speed
  *  is not important.
  *
  *  If you're using DirectX, don't forget to combine this value with
  *  the #aiProcess_ConvertToLeftHanded step. If you don't support UV transformations
  *  in your application, apply the #aiProcess_TransformUVCoords step, too.
  *  @note Please take the time to read the doc for the steps enabled by this preset.
  *  Some of them offer further configurable properties, some of them might not be of
  *  use for you so it might be better to not specify them.
  *)

Type

// ----------------------------------------------------------------------------------
(*	Standard return type for some library functions.
 * Rarely used, and if, mostly in the C API.
 *)
aiReturn = (
	// Indicates that a function was successful
	aiReturn_SUCCESS = 0,

	// Indicates that a function failed
	aiReturn_FAILURE = -1,

	(* Indicates that not enough memory was available
	 * to perform the requested operation
	 *)
	aiReturn_OUTOFMEMORY = -3,

	(* @cond never
	 *  Force 32-bit size enum
	 *)
	_AI_ENFORCE_ENUM_SIZE = $7fffffff
);  // !enum aiReturn

// just for backwards compatibility, don't use these constants anymore
Const
	AI_SUCCESS     = aiReturn_SUCCESS;
	AI_FAILURE     = aiReturn_FAILURE;
	AI_OUTOFMEMORY = aiReturn_OUTOFMEMORY;

// ----------------------------------------------------------------------------------
(* Seek origins (for the virtual file system API).
 *  Much cooler than using SEEK_SET, SEEK_CUR or SEEK_END.
 *)
 Type
aiOrigin = Cardinal;

Const
	// Beginning of the file
	aiOrigin_SET = $0;

	// Current position of the file pointer
	aiOrigin_CUR = $1;

	// End of the file, offsets must be negative
	aiOrigin_END = $2;

(* @brief Enumerates predefined log streaming destinations.
 *  Logging to these streams can be enabled with a single call to
 *   #LogStream::createDefaultStream or #aiAttachPredefinedLogStream(),
 *   respectively.
 *)
Type
 aiDefaultLogStream	 = Cardinal;

Const
	// Stream the log to a file
	aiDefaultLogStream_FILE = $1;

	// Stream the log to std::cout
	aiDefaultLogStream_STDOUT = $2;

	// Stream the log to std::cerr
	aiDefaultLogStream_STDERR = $4;

	(* MSVC only: Stream the log the the debugger
	 * (this relies on OutputDebugString from the Win32 SDK)
	 *)
	aiDefaultLogStream_DEBUGGER = $8;

// just for backwards compatibility, don't use these constants anymore
Const
	DLS_FILE     = aiDefaultLogStream_FILE;
	DLS_STDOUT   = aiDefaultLogStream_STDOUT;
	DLS_STDERR   = aiDefaultLogStream_STDERR;
	DLS_DEBUGGER = aiDefaultLogStream_DEBUGGER;

(** Stores the memory requirements for different components (e.g. meshes, materials,
 *  animations) of an import. All sizes are in bytes.
 *  @see Importer::GetMemoryRequirements()
*)
Type
PaiMemoryInfo = ^aiMemoryInfo;
aiMemoryInfo = Packed Record
	/// Storage allocated for texture data */
	textures:Cardinal;

	/// Storage allocated for material data  */
	materials:Cardinal;

	// Storage allocated for mesh data */
	meshes:Cardinal;

	// Storage allocated for node data */
	nodes:Cardinal;

	// Storage allocated for animation data */
	animations:Cardinal;

	// Storage allocated for camera data */
	cameras:Cardinal;

	// Storage allocated for light data */
	lights:Cardinal;

	// Total storage allocated for the full import. */
	total:Cardinal;
End; 

(* @brief Defines how the Nth texture of a specific type is combined with
 *  the result of all previous layers.
 *
 *  Example (left: key, right: value): <br>
 *  @code
 *  DiffColor0     - gray
 *  DiffTextureOp0 - aiTextureOpMultiply
 *  DiffTexture0   - tex1.png
 *  DiffTextureOp0 - aiTextureOpAdd
 *  DiffTexture1   - tex2.png
 *  @endcode
 *  Written as equation, the final diffuse term for a specific pixel would be: 
 *  @code
 *  diffFinal = DiffColor0 * sampleTex(DiffTexture0,UV0) + 
 *     sampleTex(DiffTexture1,UV0) * diffContrib;
 *  @endcode
 *  where 'diffContrib' is the intensity of the incoming light for that pixel.
 *)
aiTextureOp = Cardinal;

Const
	// T = T1 * T2 */
	aiTextureOp_Multiply = $0;

	// T = T1 + T2 */
	aiTextureOp_Add = $1;

	// T = T1 - T2 */
	aiTextureOp_Subtract = $2;

	// T = T1 / T2 */
	aiTextureOp_Divide = $3;

	// T = (T1 + T2) - (T1 * T2) */
	aiTextureOp_SmoothAdd = $4;

	// T = T1 + (T2-0.5) */
	aiTextureOp_SignedAdd = $5;


(* @brief Defines how UV coordinates outside the [0...1] range are handled.
 *
 *  Commonly refered to as 'wrapping mode'.
 *)
 Type
aiTextureMapMode = Cardinal;

Const
    // A texture coordinate u|v is translated to u%1|v%1
    aiTextureMapMode_Wrap = $0;

    (* Texture coordinates outside [0...1]
     *  are clamped to the nearest valid value.
     *)
    aiTextureMapMode_Clamp = $1;

	(* If the texture coordinates for a pixel are outside [0...1]
	 *  the texture is not applied to that pixel
     *)
    aiTextureMapMode_Decal = $3;

    (* A texture coordinate u|v becomes u%1|v%1 if (u-(u%1))%2 is zero and
     *  1-(u%1)|1-(v%1) otherwise
     *)
    aiTextureMapMode_Mirror = $2;

	 (* @cond never
	  *  This value is not used. It forces the compiler to use at least
	  *  32 Bit integers to represent this enum.
	  *)

(* @brief Defines how the mapping coords for a texture are generated.
 *
 *  Real-time applications typically require full UV coordinates, so the use of
 *  the aiProcess_GenUVCoords step is highly recommended. It generates proper
 *  UV channels for non-UV mapped objects, as long as an accurate description
 *  how the mapping should look like (e.g spherical) is given.
 *  See the #AI_MATKEY_MAPPING property for more details.
 *)
Type
aiTextureMapping = Cardinal;

Const
    (* The mapping coordinates are taken from an UV channel.
	 *
	 *  The #AI_MATKEY_UVWSRC key specifies from which UV channel
	 *  the texture coordinates are to be taken from (remember,
	 *  meshes can have more than one UV channel).
    *)
    aiTextureMapping_UV = $0;

	 // Spherical mapping
    aiTextureMapping_SPHERE = $1;

	 // Cylindrical mapping
    aiTextureMapping_CYLINDER = $2;

	// Cubic mapping
    aiTextureMapping_BOX = $3;

	 // Planar mapping
    aiTextureMapping_PLANE = $4;

	 // Undefined mapping. Have fun.
    aiTextureMapping_OTHER = $5;

(** @brief Defines the purpose of a texture
 *
 *  This is a very difficult topic. Different 3D packages support different
 *  kinds of textures. For very common texture types, such as bumpmaps, the
 *  rendering results depend on implementation details in the rendering
 *  pipelines of these applications. Assimp loads all texture references from
 *  the model file and tries to determine which of the predefined texture
 *  types below is the best choice to match the original use of the texture
 *  as closely as possible.<br>
 *
 *  In content pipelines you'll usually define how textures have to be handled,
 *  and the artists working on models have to conform to this specification,
 *  regardless which 3D tool they're using.
 *)
 Type
aiTextureType = Cardinal;
	(* Dummy value.
	 *
	 *  No texture, but the value to be used as 'texture semantic'
	 *  (#aiMaterialProperty::mSemantic) for all material properties
	 *  *not* related to textures.
	 *)

  Const
	aiTextureType_NONE = $0;

    (* The texture is combined with the result of the diffuse
	 *  lighting equation.
     *)
    aiTextureType_DIFFUSE = $1;

	(* The texture is combined with the result of the specular
	 *  lighting equation.
     *)
    aiTextureType_SPECULAR = $2;

	(* The texture is combined with the result of the ambient
	 *  lighting equation.
     *)
    aiTextureType_AMBIENT = $3;

	(* The texture is added to the result of the lighting
	 *  calculation. It isn't influenced by incoming light.
     *)
    aiTextureType_EMISSIVE = $4;

	(* The texture is a height map.
	 *
	 *  By convention, higher gray-scale values stand for
	 *  higher elevations from the base height.
     *)
    aiTextureType_HEIGHT = $5;

	(* The texture is a (tangent space) normal-map.
	 *
	 *  Again, there are several conventions for tangent-space
	 *  normal maps. Assimp does (intentionally) not
	 *  distinguish here.
     *)
    aiTextureType_NORMALS = $6;

	(* The texture defines the glossiness of the material.
	 *
	 *  The glossiness is in fact the exponent of the specular
	 *  (phong) lighting equation. Usually there is a conversion
	 *  function defined to map the linear color values in the
	 *  texture to a suitable exponent. Have fun.
    *)
    aiTextureType_SHININESS = $7;

	(* The texture defines per-pixel opacity.
	 *
	 *  Usually 'white' means opaque and 'black' means
	 *  'transparency'. Or quite the opposite. Have fun.
    *)
    aiTextureType_OPACITY = $8;

	(* Displacement texture
	 *
	 *  The exact purpose and format is application-dependent.
     *  Higher color values stand for higher vertex displacements.
    *)
    aiTextureType_DISPLACEMENT = $9;

	(* Lightmap texture (aka Ambient Occlusion)
	 *
	 *  Both 'Lightmaps' and dedicated 'ambient occlusion maps' are
	 *  covered by this material property. The texture contains a
	 *  scaling value for the final color value of a pixel. Its
	 *  intensity is not affected by incoming light.
    *)
    aiTextureType_LIGHTMAP = $A;

	(* Reflection texture
	 *
	 * Contains the color of a perfect mirror reflection.
	 * Rarely used, almost never for real-time applications.
    *)
    aiTextureType_REFLECTION = $B;

	(* Unknown texture
	 *
	 *  A texture reference that does not match any of the definitions
	 *  above is considered to be 'unknown'. It is still imported,
	 *  but is excluded from any further postprocessing.
    *)
    aiTextureType_UNKNOWN = $C;


Const
 AI_TEXTURE_TYPE_MAX  = aiTextureType_UNKNOWN;

(** @brief Defines all shading models supported by the library
 *
 *  The list of shading modes has been taken from Blender.
 *  See Blender documentation for more information. The API does
 *  not distinguish between 'specular' and 'diffuse' shaders (thus the
 *  specular term for diffuse shading models like Oren-Nayar remains
 *  undefined). <br>
 *  Again, this value is just a hint. Assimp tries to select the shader whose
 *  most common implementation matches the original rendering results of the
 *  3D modeller which wrote a particular model as closely as possible.
 *)
 Type
aiShadingMode = (
    (** Flat shading. Shading is done on per-face base,
     *  diffuse only. Also known as 'faceted shading'.
     *)
    aiShadingMode_Flat = $1,

    // Simple Gouraud shading. 
    aiShadingMode_Gouraud = $2,

    // Phong-Shading
    aiShadingMode_Phong = $3,

    // Phong-Blinn-Shading
    aiShadingMode_Blinn	= $4,

    (** Toon-Shading per pixel
     *
	 *  Also known as 'comic' shader.
     *)
    aiShadingMode_Toon = $5,

    (** OrenNayar-Shading per pixel
     *
     *  Extension to standard Lambertian shading, taking the
     *  roughness of the material into account
     *)
    aiShadingMode_OrenNayar = $6,

    (** Minnaert-Shading per pixel
     *
     *  Extension to standard Lambertian shading, taking the
     *  'darkness' of the material into account
     *)
    aiShadingMode_Minnaert = $7,

    (** CookTorrance-Shading per pixel
	 *
	 *  Special shader for metallic surfaces.
     *)
    aiShadingMode_CookTorrance = $8,

    (** No shading at all. Constant light influence of 1.0.
    *)
    aiShadingMode_NoShading = $9,

	 (** Fresnel shading
     *)
    aiShadingMode_Fresnel = $a,


	 (** @cond never 
	  *  This value is not used. It forces the compiler to use at least
	  *  32 Bit integers to represent this enum.
	  *)
	_aiShadingMode_Force32Bit = $9fffffff
);


(* @brief Defines some mixed flags for a particular texture.
 *
 *  Usually you'll instruct your cg artists how textures have to look like ...
 *  and how they will be processed in your application. However, if you use
 *  Assimp for completely generic loading purposes you might also need to 
 *  process these flags in order to display as many 'unknown' 3D models as 
 *  possible correctly.
 *
 *  This corresponds to the #AI_MATKEY_TEXFLAGS property.
*)
aiTextureFlags = (
	(* The texture's color values have to be inverted (componentwise 1-n)
	 *)
	aiTextureFlags_Invert = $1,

	(* Explicit request to the application to process the alpha channel
	 *  of the texture.
	 *
	 *  Mutually exclusive with #aiTextureFlags_IgnoreAlpha. These
	 *  flags are set if the library can say for sure that the alpha
	 *  channel is used/is not used. If the model format does not
	 *  define this, it is left to the application to decide whether
	 *  the texture alpha channel - if any - is evaluated or not.
	 *)
	aiTextureFlags_UseAlpha = $2,

	(* Explicit request to the application to ignore the alpha channel
	 *  of the texture.
	 *
	 *  Mutually exclusive with #aiTextureFlags_UseAlpha. 
	 *)
	aiTextureFlags_IgnoreAlpha = $4,
	
	 (* @cond never 
	  *  This value is not used. It forces the compiler to use at least
	  *  32 Bit integers to represent this enum.
	  *)
	  _aiTextureFlags_Force32Bit = $9fffffff
);


(* @brief Defines alpha-blend flags.
 *
 *  If you're familiar with OpenGL or D3D, these flags aren't new to you.
 *  They define *how* the final color value of a pixel is computed, basing
 *  on the previous color at that pixel and the new color value from the
 *  material.
 *  The blend formula is:
 *  @code
 *    SourceColor * SourceBlend + DestColor * DestBlend
 *  @endcode
 *  where <DestColor> is the previous color in the framebuffer at this
 *  position and <SourceColor> is the material colro before the transparency
 *  calculation.<br>
 *  This corresponds to the #AI_MATKEY_BLEND_FUNC property.
*)
aiBlendMode = (
	(* 
	 *  Formula:
	 *  @code
	 *  SourceColor*SourceAlpha + DestColor*(1-SourceAlpha)
	 *  @endcode
	 *)
	aiBlendMode_Default = $0,

	(* Additive blending
	 *
	 *  Formula:
	 *  @code
	 *  SourceColor*1 + DestColor*1
	 *  @endcode
	 *)
	aiBlendMode_Additive = $1,

	// we don't need more for the moment, but we might need them
	// in future versions ...

	 (* @cond never 
	  *  This value is not used. It forces the compiler to use at least
	  *  32 Bit integers to represent this enum.
	  *)
	_aiBlendMode_Force32Bit = $9fffffff
	//! @endcond
);

(* @brief Defines how an UV channel is transformed.
 *
 *  This is just a helper structure for the #AI_MATKEY_UVTRANSFORM key.
 *  See its documentation for more details. 
 *
 *  Typically you'll want to build a matrix of this information. However,
 *  we keep separate scaling/translation/rotation values to make it
 *  easier to process and optimize UV transformations internally.
 *)
aiUVTransform = Packed Record
	(* Translation on the u and v axes. 
	 *
	 *  The default value is (0|0).
	 *)
	mTranslation:aiVector2D;

	(* Scaling on the u and v axes. 
	 *
	 *  The default value is (1|1).
	 *)
	mScaling:aiVector2D;

	(* Rotation - in counter-clockwise direction.
	 *
	 *  The rotation angle is specified in radians. The
	 *  rotation center is 0.5f|0.5f. The default value
     *  0.f.
	 *)
	mRotation:Single;
End;


// ---------------------------------------------------------------------------
(** @brief A very primitive RTTI system for the contents of material
 *  properties.
 *)
aiPropertyTypeInfo = Cardinal;
    (* Array of single-precision (32 Bit) floats
	 *
	 *  It is possible to use aiGetMaterialInteger[Array]() (or the C++-API
	 *  aiMaterial::Get()) to query properties stored in floating-point format.
	 *  The material system performs the type conversion automatically.
    *)
Const
    aiPTI_Float   = $1;

    (* The material property is an aiString.
	 *
	 *  Arrays of strings aren't possible, aiGetMaterialString() (or the 
	 *  C++-API aiMaterial::Get()) *must* be used to query a string property.
    *)
    aiPTI_String  = $3;

    (* Array of (32 Bit) integers
	 *
	 *  It is possible to use aiGetMaterialFloat[Array]() (or the C++-API
	 *  aiMaterial::Get()) to query properties stored in integer format.
	 *  The material system performs the type conversion automatically.
    *)
    aiPTI_Integer = $4;


    (* Simple binary buffer, content undefined. Not convertible to anything.
    *)
    aiPTI_Buffer  = $5;

(* @brief Data structure for a single material property
 *
 *  As an user, you'll probably never need to deal with this data structure.
 *  Just use the provided aiGetMaterialXXX() or aiMaterial::Get() family
 *  of functions to query material properties easily. Processing them 
 *  manually is faster, but it is not the recommended way. It isn't worth
 *  the effort. <br>
 *  Material property names follow a simple scheme:
 *  @code
 *    $<name>
 *    ?<name>
 *       A public property, there must be corresponding AI_MATKEY_XXX define
 *       2nd: Public, but ignored by the #aiProcess_RemoveRedundantMaterials 
 *       post-processing step.
 *    ~<name>
 *       A temporary property for internal use. 
 *  @endcode
 *  @see aiMaterial
 *)

Type
PaiMaterialProperty = ^aiMaterialProperty;
aiMaterialProperty = Packed Record
    (** Specifies the name of the property (key)
     *  Keys are generally case insensitive.
     *)
    mKey:aiString;

	(* Textures: Specifies their exact usage semantic.
	 * For non-texture properties, this member is always 0
	 * (or, better-said, #aiTextureType_NONE).
	 *)
	mSemantic:Cardinal;

	(* Textures: Specifies the index of the texture.
	 *  For non-texture properties, this member is always 0.
	 *)
	mIndex:Cardinal;

    (*	Size of the buffer mData is pointing to, in bytes.
	 *  This value may not be 0.
     *)
    mDataLength:Cardinal;

    (* Type information for the property.
     *
     * Defines the data layout inside the data buffer. This is used
	 * by the library internally to perform debug checks and to 
	 * utilize proper type conversions. 
	 * (It's probably a hacky solution, but it works.)
     *)
    mType:aiPropertyTypeInfo;

    (*	Binary buffer to hold the property's value.
     * The size of the buffer is always mDataLength.
     *)
    mData:PChar;
End;


(* @brief Data structure for a material
*
*  Material data is stored using a key-value structure. A single key-value
*  pair is called a 'material property'. C++ users should use the provided
*  member functions of aiMaterial to process material properties, C users
*  have to stick with the aiMaterialGetXXX family of unbound functions.
*  The library defines a set of standard keys (AI_MATKEY_XXX).
*)

PaiMaterialPropertyArray = ^aiMaterialPropertyArray;
aiMaterialPropertyArray = Array[0..9] Of PaiMaterialProperty;

PaiMaterial = ^aiMaterial;
aiMaterial = Packed Record
    // List of all material properties loaded. 
    mProperties:PaiMaterialPropertyArray;

    // Number of properties in the data base 
	mNumProperties:Cardinal;

	 // Storage allocated 
    mNumAllocated:Cardinal;
End;


Const
 AI_MATKEY_NAME  = '?mat.name';
 AI_MATKEY_TWOSIDED = '$mat.twosided';
 AI_MATKEY_SHADING_MODEL = '$mat.shadingm';
 AI_MATKEY_ENABLE_WIREFRAME ='$mat.wireframe';
 AI_MATKEY_BLEND_FUNC = '$mat.blend';
 AI_MATKEY_OPACITY ='$mat.opacity';
 AI_MATKEY_BUMPSCALING ='$mat.bumpscaling';
 AI_MATKEY_SHININESS = '$mat.shininess';
 AI_MATKEY_REFLECTIVITY = '$mat.reflectivity';
 AI_MATKEY_SHININESS_STRENGTH ='$mat.shinpercent';
 AI_MATKEY_REFRACTI = '$mat.refracti';
 AI_MATKEY_COLOR_DIFFUSE = '$clr.diffuse';
 AI_MATKEY_COLOR_AMBIENT = '$clr.ambient';
 AI_MATKEY_COLOR_SPECULAR = '$clr.specular';
 AI_MATKEY_COLOR_EMISSIVE = '$clr.emissive';
 AI_MATKEY_COLOR_TRANSPARENT = '$clr.transparent';
 AI_MATKEY_COLOR_REFLECTIVE = '$clr.reflective';
 AI_MATKEY_GLOBAL_BACKGROUND_IMAGE = '?bg.global';

// Pure key names for all texture-related properties
 _AI_MATKEY_TEXTURE_BASE		=	'$tex.file';
 _AI_MATKEY_UVWSRC_BASE			= '$tex.uvwsrc';
 _AI_MATKEY_TEXOP_BASE			= '$tex.op';
 _AI_MATKEY_MAPPING_BASE			= '$tex.mapping';
 _AI_MATKEY_TEXBLEND_BASE		= '$tex.blend';
 _AI_MATKEY_MAPPINGMODE_U_BASE =	'$tex.mapmodeu';
 _AI_MATKEY_MAPPINGMODE_V_BASE =	'$tex.mapmodev';
 _AI_MATKEY_TEXMAP_AXIS_BASE	 =	'$tex.mapaxis';
 _AI_MATKEY_UVTRANSFORM_BASE	 =	'$tex.uvtrafo';
 _AI_MATKEY_TEXFLAGS_BASE		= '$tex.flags';

(* @brief Retrieve a material property with a specific key from the material
 *
 * @param pMat Pointer to the input material. May not be NULL
 * @param pKey Key to search for. One of the AI_MATKEY_XXX constants.
 * @param type Specifies the type of the texture to be retrieved (
 *    e.g. diffuse, specular, height map ...)
 * @param index Index of the texture to be retrieved.
 * @param pPropOut Pointer to receive a pointer to a valid aiMaterialProperty
 *        structure or NULL if the key has not been found. *)

Function aiGetMaterialProperty(pMat:PaiMaterial; pKey:PChar; _type:aiTextureType; index:Cardinal;Var pPropOut:PaiMaterialProperty):aiReturn;  cdecl; external AssimpLib;

(* @brief Retrieve an array of float values with a specific key 
 *  from the material
 *
 * Pass one of the AI_MATKEY_XXX constants for the last three parameters (the
 * example reads the #AI_MATKEY_UVTRANSFORM property of the first diffuse texture)
 * @code
 * aiUVTransform trafo;
 * unsigned int max = sizeof(aiUVTransform);
 * if (AI_SUCCESS != aiGetMaterialFloatArray(mat, AI_MATKEY_UVTRANSFORM(aiTextureType_DIFFUSE,0),
 *    (float * )&trafo, &max) || sizeof(aiUVTransform) != max)
 * {
 *   // error handling
 * }
 * @endcode
 *
 * @param pMat Pointer to the input material. May not be NULL
 * @param pKey Key to search for. One of the AI_MATKEY_XXX constants.
 * @param pOut Pointer to a buffer to receive the result.
 * @param pMax Specifies the size of the given buffer, in float's.
 *        Receives the number of values (not bytes!) read.
 * @param type (see the code sample above)
 * @param index (see the code sample above)
 * @return Specifies whether the key has been found. If not, the output
 *   arrays remains unmodified and pMax is set to 0.*)
// ---------------------------------------------------------------------------
Function aiGetMaterialFloatArray(pMat:PaiMaterial; pKey:PChar;
	 _type, index:Cardinal; pOut:PSingleArray;
    Var pMax:Integer):aiReturn;  cdecl; external AssimpLib;


(* @brief Retrieve a single float property with a specific key from the material.
*
* Pass one of the AI_MATKEY_XXX constants for the last three parameters (the
* example reads the #AI_MATKEY_SHININESS_STRENGTH property of the first diffuse texture)
* @code
* float specStrength = 1.f; // default value, remains unmodified if we fail.
* aiGetMaterialFloat(mat, AI_MATKEY_SHININESS_STRENGTH,
*    (float* )&specStrength);
* @endcode
*
* @param pMat Pointer to the input material. May not be NULL
* @param pKey Key to search for. One of the AI_MATKEY_XXX constants.
* @param pOut Receives the output float.
* @param type (see the code sample above)
* @param index (see the code sample above)
* @return Specifies whether the key has been found. If not, the output
*   float remains unmodified.*)

(* @brief Retrieve an array of integer values with a specific key
 *  from a material
 *
 * See the sample for aiGetMaterialFloatArray for more information.*)
Function  aiGetMaterialIntegerArray(pMat:PaiMaterial; pKey:PChar; _type, index:Cardinal;
   pOut:PIntegerArray; Var pMax:Integer):aiReturn;  cdecl; external AssimpLib;


(* @brief Retrieve a color value from the material property table
*
* See the sample for aiGetMaterialFloat for more information*)
Function aiGetMaterialColor(pMat:PaiMaterial; pKey:PChar; _type, index:Cardinal; Var pOut:aiColor4D):aiReturn; cdecl; external AssimpLib;


(* @brief Retrieve a string from the material property table
* See the sample for aiGetMaterialFloat for more information.*)

Function aiGetMaterialString(pMat:PaiMaterial; pKey:PChar; _type:Cardinal; index:Cardinal; var pOut:aiString):aiReturn; cdecl; external AssimpLib;

(* Get the number of textures for a particular texture type.
 *  @param[in] pMat Pointer to the input material. May not be NULL
 *  @param type Texture type to check for
 *  @return Number of textures for this type.
 *  @note A texture can be easily queried using #aiGetMaterialTexture() *)
Function aiGetMaterialTextureCount(pMat:PaiMaterial; textype:aiTextureType):Cardinal; cdecl; external AssimpLib;



(* Helper structure to describe a virtual camera. 
 *
 * Cameras have a representation in the node graph and can be animated.
 * An important aspect is that the camera itself is also part of the
 * scenegraph. This means, any values such as the look-at vector are not 
 * *absolute*, they're <b>relative</b> to the coordinate system defined
 * by the node which corresponds to the camera. This allows for camera
 * animations. For static cameras parameters like the 'look-at' or 'up' vectors
 * are usually specified directly in aiCamera, but beware, they could also
 * be encoded in the node transformation. The following (pseudo)code sample 
 * shows how to do it: <br><br>
 * @code
 * // Get the camera matrix for a camera at a specific time
 * // if the node hierarchy for the camera does not contain
 * // at least one animated node this is a static computation
 * get-camera-matrix (node sceneRoot, camera cam) : matrix
 * {
 *    node   cnd = find-node-for-camera(cam)
 *    matrix cmt = identity()
 *
 *    // as usual - get the absolute camera transformation for this frame
 *    for each node nd in hierarchy from sceneRoot to cnd
 *      matrix cur
 *      if (is-animated(nd))
 *         cur = eval-animation(nd)
 *      else cur = nd->mTransformation;
 *      cmt = mult-matrices( cmt, cur )
 *    end for
 *
 *    // now multiply with the camera's own local transform
 *    cam = mult-matrices (cam, get-camera-matrix(cmt) )
 * }
 * @endcode
 *
 * @note some file formats (such as 3DS, ASE) export a 'target point' -
 * the point the camera is looking at (it can even be animated). Assimp
 * writes the target point as a subnode of the camera's main node,
 * called '<camName>.Target'. However this is just additional information
 * then the transformation tracks of the camera main node make the
 * camera already look in the right direction.
 * 
*)

Type
PaiCamera = ^aiCamera;
aiCamera = Packed Record
	(* The name of the camera.
	 *
	 *  There must be a node in the scenegraph with the same name.
	 *  This node specifies the position of the camera in the scene
	 *  hierarchy and can be animated.
	 *)
	mName:aiString;

	(* Position of the camera relative to the coordinate space
	 *  defined by the corresponding node.
	 *
	 *  The default value is 0|0|0.
	 *)
	mPosition:aiVector3D;


	(* 'Up' - vector of the camera coordinate system relative to
	 *  the coordinate space defined by the corresponding node.
	 *
	 *  The 'right' vector of the camera coordinate system is
	 *  the cross product of  the up and lookAt vectors.
	 *  The default value is 0|1|0. The vector
	 *  may be normalized, but it needn't.
	 *)
	mUp:aiVector3D;


	(* 'LookAt' - vector of the camera coordinate system relative to
	 *  the coordinate space defined by the corresponding node.
	 *
	 *  This is the viewing direction of the user.
	 *  The default value is 0|0|1. The vector
	 *  may be normalized, but it needn't.
	 *)
	mLookAt:aiVector3D;


	(* Half horizontal field of view angle, in radians. 
	 *
	 *  The field of view angle is the angle between the center
	 *  line of the screen and the left or right border.
	 *  The default value is 1/4PI.
	 *)
	mHorizontalFOV:Single;

	(* Distance of the near clipping plane from the camera.
	 *
	 * The value may not be 0.f (for arithmetic reasons to prevent
	 * a division through zero). The default value is 0.1f.
	 *)
	mClipPlaneNear:Single;

	(* Distance of the far clipping plane from the camera.
	 *
	 * The far clipping plane must, of course, be further away than the
	 * near clipping plane. The default value is 1000.f. The ratio
	 * between the near and the far plane should not be too
	 * large (between 1000-10000 should be ok) to avoid floating-point
	 * inaccuracies which could lead to z-fighting.
	 *)
	mClipPlaneFar:Single;


	(* Screen aspect ratio.
	 *
	 * This is the ration between the width and the height of the
	 * screen. Typical values are 4/3, 1/2 or 1/1. This value is
	 * 0 if the aspect ratio is not defined in the source file.
	 * 0 is also the default value.
	 *)
	mAspect:Single;
End;


// A time-value pair specifying a certain 3D vector for the given time. */
PaiVectorKey  = ^aiVectorKey ;
aiVectorKey = Packed Record
	mTime:double;     	// The time of this key
	mValue:aiVector3D; 	// The value of this key
End;

// A time-value pair specifying a rotation for the given time.
// Rotations are expressed with quaternions.
PaiQuatKey =^aiQuatKey;
aiQuatKey = Packed Record
	mTime:double;	// The time of this key
	mValue:aiQuaternion; // The value of this key
End;

//  Binds a anim mesh to a specific point in time. */
PaiMeshKey = ^aiMeshKey;
aiMeshKey = Packed Record
	mTime:Double;	// The time of this key

	(** Index into the aiMesh::mAnimMeshes array of the 
	 *  mesh coresponding to the #aiMeshAnim hosting this
	 *  key frame. The referenced anim mesh is evaluated
	 *  according to the rules defined in the docs for #aiAnimMesh.*)
	mValue:Cardinal;
End;

(* Defines how an animation channel behaves outside the defined time
 *  range. This corresponds to aiNodeAnim::mPreState and 
 *  aiNodeAnim::mPostState.*)
aiAnimBehaviour = (
	// The value from the default node transformation is taken*/
	aiAnimBehaviour_DEFAULT  = $0,  

	// The nearest key value is used without interpolation 
	aiAnimBehaviour_CONSTANT = $1,

	(* The value of the nearest two keys is linearly
	 *  extrapolated for the current time value.*)
	aiAnimBehaviour_LINEAR   = $2,

	(* The animation is repeated.
	 *
	 *  If the animation key go from n to m and the current
	 *  time is t, use the value at (t-n) % (|m-n|).*)
	aiAnimBehaviour_REPEAT   = $3,



	(* This value is not used, it is just here to force the
	 *  the compiler to map this enum to a 32 Bit integer  *)
	_aiAnimBehaviour_Force32Bit = $8fffffff
);

(* Describes the animation of a single node. The name specifies the 
 *  bone/node which is affected by this animation channel. The keyframes
 *  are given in three separate series of values, one each for position, 
 *  rotation and scaling. The transformation matrix computed from these
 *  values replaces the node's original transformation matrix at a
 *  specific time.
 *  This means all keys are absolute and not relative to the bone default pose.
 *  The order in which the transformations are applied is
 *  - as usual - scaling, rotation, translation.
 *
 *  @note All keys are returned in their correct, chronological order.
 *  Duplicate keys don't pass the validation step. Most likely there
 *  will be no negative time values, but they are not forbidden also ( so 
 *  implementations need to cope with them! ) *)
 
PaiVectorKeyArray = ^aiVectorKeyArray;
aiVectorKeyArray = Array[0..100000] Of aiVectorKey;

PaiQuatKeyArray = ^aiQuatKeyArray;
aiQuatKeyArray = Array[0..100000] Of aiQuatKey;

PaiNodeAnim = ^aiNodeAnim;
aiNodeAnim = Packed Record
	// The name of the node affected by this animation. The node must exist and it must be unique.
	mNodeName:aiString;

	// The number of position keys
	mNumPositionKeys:Cardinal;

	(** The position keys of this animation channel. Positions are 
	 * specified as 3D vector. The array is mNumPositionKeys in size.
	 *
	 * If there are position keys, there will also be at least one
	 * scaling and one rotation key.*)
	mPositionKeys:PaiVectorKeyArray;

	// The number of rotation keys 
	mNumRotationKeys:Cardinal;

	(** The rotation keys of this animation channel. Rotations are 
	 *  given as quaternions,  which are 4D vectors. The array is 
	 *  mNumRotationKeys in size.
	 *
	 * If there are rotation keys, there will also be at least one
	 * scaling and one position key. *)
	mRotationKeys:PaiQuatKeyArray;


	// The number of scaling keys 
	mNumScalingKeys:Cardinal;

	(* The scaling keys of this animation channel. Scalings are 
	 *  specified as 3D vector. The array is mNumScalingKeys in size.
	 *
	 * If there are scaling keys, there will also be at least one
	 * position and one rotation key.*)
	mScalingKeys:PaiVectorKeyArray;


	(** Defines how the animation behaves before the first
	 *  key is encountered.
	 *
	 *  The default value is aiAnimBehaviour_DEFAULT (the original
	 *  transformation matrix of the affected node is used).*)
	mPreState:aiAnimBehaviour;

	(** Defines how the animation behaves after the last
	 *  key was processed.
	 *
	 *  The default value is aiAnimBehaviour_DEFAULT (the original
	 *  transformation matrix of the affected node is taken).*)
	mPostState:aiAnimBehaviour;
End;

(* Describes vertex-based animations for a single mesh or a group of
 *  meshes. Meshes carry the animation data for each frame in their
 *  aiMesh::mAnimMeshes array. The purpose of aiMeshAnim is to 
 *  define keyframes linking each mesh attachment to a particular
 *  point in time. *)
 
PaiMeshKeyArray = ^ aiMeshKeyArray;
aiMeshKeyArray = Array[0..0] Of PaiMeshKey;

PaiMeshAnim = ^aiMeshAnim;
aiMeshAnim = Packed Record
	(* Name of the mesh to be animated. An empty string is not allowed,
	 *  animated meshes need to be named (not necessarily uniquely,
	 *  the name can basically serve as wildcard to select a group
	 *  of meshes with similar animation setup)*)
	mName:aiString;

	// Size of the #mKeys array. Must be 1, at least. */
	mNumKeys:Cardinal;

	// Key frames of the animation. May not be NULL. 
	mKeys:PaiMeshKeyArray;
End;

PaiNodeAnimArray = ^aiNodeAnimArray;
aiNodeAnimArray = Array[0..0] Of PaiNodeAnim;

PaiMeshAnimArray = ^aiMeshAnimArray;
aiMeshAnimArray = Array[0..0] Of PaiMeshAnim;

// An animation consists of keyframe data for a number of nodes.
// For  each node affected by the animation a separate series of data is given.
PaiAnimation = ^aiAnimation;
aiAnimation = Packed Record
	(** The name of the animation. If the modeling package this data was
	 *  exported from does support only a single animation channel, this
	 *  name is usually empty (length is zero). *)
	mName:aiString;

	// Duration of the animation in ticks.
	mDuration:double;

	// Ticks per second. 0 if not specified in the imported file
	mTicksPerSecond:double;

  pad:Cardinal; //????

	//The number of bone animation channels. Each channel affects a single node.
	mNumChannels:Cardinal;

	(** The node animation channels. Each channel affects a single node.
	 *  The array is mNumChannels in size. *)
	mChannels:PaiNodeAnimArray;


	(** The number of mesh animation channels. Each channel affects
	 *  a single mesh and defines vertex-based animation. *)
	mNumMeshChannels:Cardinal;

	(** The mesh animation channels. Each channel affects a single mesh.
	 *  The array is mNumMeshChannels in size. *)
	mMeshChannels:PaiMeshAnimArray;
End;

(** A node in the imported hierarchy. 
 *
 * Each node has name, a parent node (except for the root node), 
 * a transformation relative to its parent and possibly several child nodes.
 * Simple file formats don't support hierarchical structures - for these formats 
 * the imported scene does consist of only a single root node without children.
 *)

PaiNode = ^aiNode;
PaiNodeArray = ^aiNodeArray;
aiNodeArray = Array[0..0]Of paiNode;

aiNode = Packed Record
	(** The name of the node. 
	 *
	 * The name might be empty (length of zero) but all nodes which 
	 * need to be accessed afterwards by bones or anims are usually named.
	 * Multiple nodes may have the same name, but nodes which are accessed
	 * by bones (see #aiBone and #aiMesh::mBones) *must* be unique.
	 * 
	 * Cameras and lights are assigned to a specific node name - if there
	 * are multiple nodes with this name, they're assigned to each of them.
	 * <br>
	 * There are no limitations regarding the characters contained in
	 * this text. You should be able to handle stuff like whitespace, tabs,
	 * linefeeds, quotation marks, ampersands, ... .
	 *)
	mName:aiString;

	// The transformation relative to the node's parent. 
	mTransformation:aiMatrix4x4;

	// Parent node. NULL if this node is the root node. 
	mParent:PaiNode;

	// The number of child nodes of this node. 
	mNumChildren:Cardinal;

	// The child nodes of this node. NULL if mNumChildren is 0. 
	mChildren:PaiNodeArray;

	// The number of meshes of this node. 
	mNumMeshes:Cardinal;

	// The meshes of this node. Each entry is an index into the mesh 
	mMeshes:PIntegerArray;
End;


Const
  //  Maximum number of indices per face (polygon).
  AI_MAX_FACE_INDICES = $7fff;


  // Maximum number of indices per face (polygon).
  AI_MAX_BONE_WEIGHTS = $7fffffff;

  // Maximum number of vertices per mesh.
  AI_MAX_VERTICES = $7fffffff;


//  Maximum number of faces per mesh.
  AI_MAX_FACES = $7fffffff;

// Supported number of vertex color sets per mesh.
  AI_MAX_NUMBER_OF_COLOR_SETS = 4;

// Supported number of texture coord sets (UV(W) channels) per mesh */
  AI_MAX_NUMBER_OF_TEXTURECOORDS = 4;

(* @brief A single face in a mesh, referring to multiple vertices.
 *
 * If mNumIndices is 3, we call the face 'triangle', for mNumIndices > 3
 * it's called 'polygon' (hey, that's just a definition!).
 * <br>
 * aiMesh::mPrimitiveTypes can be queried to quickly examine which types of
 * primitive are actually present in a mesh. The #aiProcess_SortByPType flag
 * executes a special post-processing algorithm which splits meshes with
 * *different* primitive types mixed up (e.g. lines and triangles) in several
 * 'clean' submeshes. Furthermore there is a configuration option (
 * #AI_CONFIG_PP_SBP_REMOVE) to force #aiProcess_SortByPType to remove
 * specific kinds of primitives from the imported scene, completely and forever.
 * In many cases you'll probably want to set this setting to
 * @code
 * aiPrimitiveType_LINE|aiPrimitiveType_POINT
 * @endcode
 * Together with the #aiProcess_Triangulate flag you can then be sure that
 * #aiFace::mNumIndices is always 3.
 * @note Take a look at the @link data Data Structures page @endlink for
 * more information on the layout and winding order of a face.
 *)
Type
aiFace = Packed Record
	//! Number of indices defining this face.
	//! The maximum value for this member is #AI_MAX_FACE_INDICES.
	mNumIndices:Cardinal;

	//! Pointer to the indices array. Size of the array is given in numIndices.
	mIndices:PCardinalArray;
End; // struct aiFace


(* @brief A single influence of a bone on a vertex.
 *)
 PaiVertexWeight = ^aiVertexWeight;
aiVertexWeight = Packed Record
	//! Index of the vertex which is influenced by the bone.
	mVertexId:Cardinal;

	//! The strength of the influence in the range (0...1).
	//! The influence from all bones at one vertex amounts to 1.
	mWeight:Single;
End;

PaiVertexWeightArray = ^aiVertexWeightArray;
aiVertexWeightArray = Array[0..100000] Of aiVertexWeight;


(* @brief A single bone of a mesh.
 *
 *  A bone has a name by which it can be found in the frame hierarchy and by
 *  which it can be addressed by animations. In addition it has a number of
 *  influences on vertices.
 *)
PaiBone = ^aiBone;
aiBone = Packed Record
	//! The name of the bone.
	mName:aiString;

	//! The number of vertices affected by this bone
	//! The maximum value for this member is #AI_MAX_BONE_WEIGHTS.
	mNumWeights:Cardinal;

	//! The vertices affected by this bone
	mWeights:PaiVertexWeightArray;

	//! Matrix that transforms from mesh space to bone space in bind pose
	mOffsetMatrix:aiMatrix4x4;
End;


(* @brief Enumerates the types of geometric primitives supported by Assimp.
 *
 *  @see aiFace Face data structure
 *  @see aiProcess_SortByPType Per-primitive sorting of meshes
 *  @see aiProcess_Triangulate Automatic triangulation
 *  @see AI_CONFIG_PP_SBP_REMOVE Removal of specific primitive types.
 *)
aiPrimitiveType = (
	(* A point primitive.
	 *
	 * This is just a single vertex in the virtual world,
	 * #aiFace contains just one index for such a primitive.
	 *)
	aiPrimitiveType_POINT       = $1,

	(* A line primitive.
	 *
	 * This is a line defined through a start and an end position.
	 * #aiFace contains exactly two indices for such a primitive.
	 *)
	aiPrimitiveType_LINE        = $2,

	(* A triangular primitive.
	 *
	 * A triangle consists of three indices.
	 *)
	aiPrimitiveType_TRIANGLE    = $4,

	(* A higher-level polygon with more than 3 edges.
	 *
	 * A triangle is a polygon, but polygon in this context means
	 * 'all polygons that are not triangles'. The 'Triangulate'-Step
	 * is provided for your convenience, it splits all polygons in
	 * triangles (which are much easier to handle).
	 *)
	aiPrimitiveType_POLYGON     = $8,


	(* This value is not used. It is just here to force the
	 *  compiler to map this enum to a 32 Bit integer.
	 *)
	_aiPrimitiveType_Force32Bit = $9fffffff
  ); //! enum aiPrimitiveType

// Get the #aiPrimitiveType flag for a specific number of face indices
//#define AI_PRIMITIVE_TYPE_FOR_N_INDICES(n) \
//	((n) > 3 ? aiPrimitiveType_POLYGON : (aiPrimitiveType)(1u << ((n)-1)))

PaiVector3DArray = ^aiVector3DArray;
aiVector3DArray = Array[0..100000] Of aiVector3D;

PaiColor4DArray = ^aiColor4DArray;
aiColor4DArray = Array[0..100000] Of aiColor4D;

(* @brief NOT CURRENTLY IN USE. An AnimMesh is an attachment to an #aiMesh stores per-vertex
 *  animations for a particular frame.
 *
 *  You may think of an #aiAnimMesh as a `patch` for the host mesh, which
 *  replaces only certain vertex data streams at a particular time.
 *  Each mesh stores n attached attached meshes (#aiMesh::mAnimMeshes).
 *  The actual relationship between the time line and anim meshes is
 *  established by #aiMeshAnim, which references singular mesh attachments
 *  by their ID and binds them to a time offset.
*)
PaiAnimMesh =^aiAnimMesh;
aiAnimMesh = Packed Record
	(** Replacement for aiMesh::mVertices. If this array is non-NULL,
	 *  it *must* contain mNumVertices entries. The corresponding
	 *  array in the host mesh must be non-NULL as well - animation
	 *  meshes may neither add or nor remove vertex components (if
	 *  a replacement array is NULL and the corresponding source
	 *  array is not, the source data is taken instead)*)
	mVertices:PaiVector3DArray;

	// Replacement for aiMesh::mNormals.
	mNormals:PaiVector3DArray;

	// Replacement for aiMesh::mTangents.
	mTangents:PaiVector3DArray;

	// Replacement for aiMesh::mBitangents.
	mBitangents:PaiVector3DArray;

	// Replacement for aiMesh::mColors
	mColors:Array[0..Pred(AI_MAX_NUMBER_OF_COLOR_SETS)] Of PaiColor4DArray;

	// Replacement for aiMesh::mTextureCoords
	mTextureCoords:Array[0..Pred(AI_MAX_NUMBER_OF_TEXTURECOORDS)] Of aiVector3D;

	(* The number of vertices in the aiAnimMesh, and thus the length of all
	 * the member arrays.
	 *
	 * This has always the same value as the mNumVertices property in the
	 * corresponding aiMesh. It is duplicated here merely to make the length
	 * of the member arrays accessible even if the aiMesh is not known, e.g.
	 * from language bindings.
	 *)
	mNumVertices:Cardinal;
End;

PaiFaceArray = ^aiFaceArray;
aiFaceArray = Array[0..0] Of aiFace;

PaiBoneArray = ^aiBoneArray;
aiBoneArray = Array[0..0] Of PaiBone;

PaiAnimMeshArray = ^aiAnimMeshArray;
aiAnimMeshArray = Array[0..0] Of PaiAnimMesh;

(* @brief A mesh represents a geometry or model with a single material.
*
* It usually consists of a number of vertices and a series of primitives/faces
* referencing the vertices. In addition there might be a series of bones, each
* of them addressing a number of vertices with a certain weight. Vertex data
* is presented in channels with each channel containing a single per-vertex
* information such as a set of texture coords or a normal vector.
* If a data pointer is non-null, the corresponding data stream is present.
* From C++-programs you can also use the comfort functions Has*() to
* test for the presence of various data streams.
*
* A Mesh uses only a single material which is referenced by a material ID.
* @note The mPositions member is usually not optional. However, vertex positions
* *could* be missing if the #AI_SCENE_FLAGS_INCOMPLETE flag is set in
* @code
* aiScene::mFlags
* @endcode
*)
PaiMesh = ^aiMesh;
aiMesh = Packed Record
	(* Bitwise combination of the members of the #aiPrimitiveType enum.
	 * This specifies which types of primitives are present in the mesh.
	 * The 'SortByPrimitiveType'-Step can be used to make sure the
	 * output meshes consist of one primitive type each.
	 *)
	mPrimitiveTypes:Cardinal;

	(* The number of vertices in this mesh.
	* This is also the size of all of the per-vertex data arrays.
	* The maximum value for this member is #AI_MAX_VERTICES.
	*)
	mNumVertices:Cardinal;

	(* The number of primitives (triangles, polygons, lines) in this  mesh.
	* This is also the size of the mFaces array.
	* The maximum value for this member is #AI_MAX_FACES.
	*)
	mNumFaces:Cardinal;

	(* Vertex positions.
	* This array is always present in a mesh. The array is
	* mNumVertices in size.
	*)
	mVertices:PaiVector3DArray;

	(* Vertex normals.
	* The array contains normalized vectors, NULL if not present.
	* The array is mNumVertices in size. Normals are undefined for
	* point and line primitives. A mesh consisting of points and
	* lines only may not have normal vectors. Meshes with mixed
	* primitive types (i.e. lines and triangles) may have normals,
	* but the normals for vertices that are only referenced by
	* point or line primitives are undefined and set to QNaN (WARN:
	* qNaN compares to inequal to *everything*, even to qNaN itself.
	* Using code like this to check whether a field is qnan is:
	* @code
	* #define IS_QNAN(f) (f != f)
	* @endcode
	* still dangerous because even 1.f == 1.f could evaluate to false! (
	* remember the subtleties of IEEE754 artithmetics). Use stuff like
	* @c fpclassify instead.
	* @note Normal vectors computed by Assimp are always unit-length.
	* However, this needn't apply for normals that have been taken
	*   directly from the model file.
	*)
	mNormals:PaiVector3DArray;

	(** Vertex tangents.
	* The tangent of a vertex points in the direction of the positive
	* X texture axis. The array contains normalized vectors, NULL if
	* not present. The array is mNumVertices in size. A mesh consisting
	* of points and lines only may not have normal vectors. Meshes with
	* mixed primitive types (i.e. lines and triangles) may have
	* normals, but the normals for vertices that are only referenced by
	* point or line primitives are undefined and set to qNaN.  See
	* the #mNormals member for a detailled discussion of qNaNs.
	* @note If the mesh contains tangents, it automatically also
	* contains bitangents (the bitangent is just the cross product of
	* tangent and normal vectors).
	*)
	mTangents:PaiVector3DArray;

	(* Vertex bitangents.
	* The bitangent of a vertex points in the direction of the positive
	* Y texture axis. The array contains normalized vectors, NULL if not
	* present. The array is mNumVertices in size.
	* @note If the mesh contains tangents, it automatically also contains
	* bitangents.
	*)
	mBitangents:PaiVector3DArray;

	(* Vertex color sets.
	* A mesh may contain 0 to #AI_MAX_NUMBER_OF_COLOR_SETS vertex
	* colors per vertex. NULL if not present. Each array is
	* mNumVertices in size if present.
	*)
	mColors:Array[0..Pred(AI_MAX_NUMBER_OF_COLOR_SETS)] Of PaiColor4DArray;

	(* Vertex texture coords, also known as UV channels.
	* A mesh may contain 0 to AI_MAX_NUMBER_OF_TEXTURECOORDS per
	* vertex. NULL if not present. The array is mNumVertices in size.
	*)
	mTextureCoords:Array[0..Pred(AI_MAX_NUMBER_OF_TEXTURECOORDS)] Of PaiVector3DArray;

	(* Specifies the number of components for a given UV channel.
	* Up to three channels are supported (UVW, for accessing volume
	* or cube maps). If the value is 2 for a given channel n, the
	* component p.z of mTextureCoords[n][p] is set to 0.0f.
	* If the value is 1 for a given channel, p.y is set to 0.0f, too.
	* @note 4D coords are not supported
	*)
	mNumUVComponents:Array[0..Pred(AI_MAX_NUMBER_OF_TEXTURECOORDS)] Of Cardinal;

	(* The faces the mesh is constructed from.
	* Each face refers to a number of vertices by their indices.
	* This array is always present in a mesh, its size is given
	* in mNumFaces. If the #AI_SCENE_FLAGS_NON_VERBOSE_FORMAT
	* is NOT set each face references an unique set of vertices.
	*)
	mFaces:PaiFaceArray;

	(* The number of bones this mesh contains.
	* Can be 0, in which case the mBones array is NULL.
	*)
	mNumBones:Cardinal;

	(* The bones of this mesh.
	* A bone consists of a name by which it can be found in the
	* frame hierarchy and a set of vertex weights.
	*)
	mBones:PaiBoneArray;

	(** The material used by this mesh.
	 * A mesh does use only a single material. If an imported model uses
	 * multiple materials, the import splits up the mesh. Use this value
	 * as index into the scene's material list.
	 *)
	mMaterialIndex:Cardinal;

	(* Name of the mesh. Meshes can be named, but this is not a
	 *  requirement and leaving this field empty is totally fine.
	 *  There are mainly three uses for mesh names:
	 *   - some formats name nodes and meshes independently.
	 *   - importers tend to split meshes up to meet the
	 *      one-material-per-mesh requirement. Assigning
	 *      the same (dummy) name to each of the result meshes
	 *      aids the caller at recovering the original mesh
	 *      partitioning.
	 *   - Vertex animations refer to meshes by their names.
	 *)
	mName:aiString;


	// NOT CURRENTLY IN USE. The number of attachment meshes
	mNumAnimMeshes:Cardinal;

	(* NOT CURRENTLY IN USE. Attachment meshes for this mesh, for vertex-based animation.
	 *  Attachment meshes carry replacement data for some of the
	 *  mesh'es vertex components (usually positions, normals). *)
	mAnimMeshes:PaiAnimMeshArray;
End;


Const
// -------------------------------------------------------------------------------
(* Specifies that the scene data structure that was imported is not complete.
 * This flag bypasses some internal validations and allows the import
 * of animation skeletons, material libraries or camera animation paths
 * using Assimp. Most applications won't support such data.
 *)
	AI_SCENE_FLAGS_INCOMPLETE	= $1;

(** @def AI_SCENE_FLAGS_VALIDATED
 * This flag is set by the validation postprocess-step (aiPostProcess_ValidateDS)
 * if the validation is successful. In a validated scene you can be sure that
 * any cross references in the data structure (e.g. vertex indices) are valid.
 *)
	AI_SCENE_FLAGS_VALIDATED =	$2;

(** @def AI_SCENE_FLAGS_VALIDATION_WARNING
 * This flag is set by the validation postprocess-step (aiPostProcess_ValidateDS)
 * if the validation is successful but some issues have been found.
 * This can for example mean that a texture that does not exist is referenced 
 * by a material or that the bone weights for a vertex don't sum to 1.0 ... .
 * In most cases you should still be able to use the import. This flag could
 * be useful for applications which don't capture Assimp's log output.
 *)
	AI_SCENE_FLAGS_VALIDATION_WARNING =	$4;

(** @def AI_SCENE_FLAGS_NON_VERBOSE_FORMAT
 * This flag is currently only set by the aiProcess_JoinIdenticalVertices step.
 * It indicates that the vertices of the output meshes aren't in the internal
 * verbose format anymore. In the verbose format all vertices are unique,
 * no vertex is ever referenced by more than one face.
 *)
	AI_SCENE_FLAGS_NON_VERBOSE_FORMAT  =	$8;

(** @def AI_SCENE_FLAGS_TERRAIN
 * Denotes pure height-map terrain data. Pure terrains usually consist of quads,
 * sometimes triangles, in a regular grid. The x,y coordinates of all vertex 
 * positions refer to the x,y coordinates on the terrain height map, the z-axis
 * stores the elevation at a specific point.
 *
 * TER (Terragen) and HMP (3D Game Studio) are height map formats.
 * @note Assimp is probably not the best choice for loading *huge* terrains -
 * fully triangulated data takes extremely much free store and should be avoided
 * as long as possible (typically you'll do the triangulation when you actually
 * need to render it).
 *)
 	AI_SCENE_FLAGS_TERRAIN = $10;


(* The root structure of the imported data.
 *
 *  Everything that was imported from the given file can be accessed from here.
 *  Objects of this class are generally maintained and owned by Assimp, not
 *  by the caller. You shouldn't want to instance it, nor should you ever try to
 *  delete a given scene on your own.
 *)

Type
aiTexel = Packed Record
	b,g,r,a:Byte;
End;

PaiTexelArray = ^aiTexelArray;
aiTexelArray = Array[0..100000] Of aiTexel;

(* Helper structure to describe an embedded texture
 *
 * Normally textures are contained in external files but some file formats embed
 * them directly in the model file. There are two types of embedded textures:
 * 1. Uncompressed textures. The color data is given in an uncompressed format.
 * 2. Compressed textures stored in a file format like png or jpg. The raw file
 * bytes are given so the application must utilize an image decoder (e.g. DevIL) to
 * get access to the actual color data.
 *)
 PaiTexture = ^aiTexture;
aiTexture = Packed Record
	(* Width of the texture, in pixels
	 *
	 * If mHeight is zero the texture is compressed in a format
	 * like JPEG. In this case mWidth specifies the size of the
	 * memory area pcData is pointing to, in bytes.
	 *)
	mWidth:Cardinal;

	(* Height of the texture, in pixels
	 *
	 * If this value is zero, pcData points to an compressed texture
	 * in any format (e.g. JPEG).
	 *)
	mHeight:Cardinal;

	(* A hint from the loader to make it easier for applications
	 *  to determine the type of embedded compressed textures.
	 *
	 * If mHeight != 0 this member is undefined. Otherwise it
	 * is set set to '\\0\\0\\0\\0' if the loader has no additional
	 * information about the texture file format used OR the
	 * file extension of the format without a trailing dot. If there
	 * are multiple file extensions for a format, the shortest
	 * extension is chosen (JPEG maps to 'jpg', not to 'jpeg').
	 * E.g. 'dds\\0', 'pcx\\0', 'jpg\\0'.  All characters are lower-case.
	 * The fourth character will always be '\\0'.
	 *)
	achFormatHint:Array[0..3] Of Char;

	(* Data of the texture.
	 *
	 * Points to an array of mWidth * mHeight aiTexel's.
	 * The format of the texture data is always ARGB8888 to
	 * make the implementation for user of the library as easy
	 * as possible. If mHeight = 0 this is a pointer to a memory
	 * buffer of size mWidth containing the compressed texture
	 * data. Good luck, have fun!
	 *)
	pcData:PaiTexelArray;
End;


// Enumerates all supported types of light sources.
aiLightSourceType = (
	aiLightSource_UNDEFINED     = $0,

	//! A directional light source has a well-defined direction
	//! but is infinitely far away. That's quite a good
	//! approximation for sun light.
	aiLightSource_DIRECTIONAL   = $1,

	//! A point light source has a well-defined position
	//! in space but no direction - it emits light in all
	//! directions. A normal bulb is a point light.
	aiLightSource_POINT         = $2,

	//! A spot light source emits light in a specific
	//! angle. It has a position and a direction it is pointing to.
	//! A good example for a spot light is a light spot in
	//! sport arenas.
	aiLightSource_SPOT          = $3,


	_aiLightSource_Force32Bit = $9fffffff
);

(* Helper structure to describe a light source.
 *
 *  Assimp supports multiple sorts of light sources, including
 *  directional, point and spot lights. All of them are defined with just
 *  a single structure and distinguished by their parameters.
 *  Note - some file formats (such as 3DS, ASE) export a 'target point' -
 *  the point a spot light is looking at (it can even be animated). Assimp
 *  writes the target point as a subnode of a spotlights's main node,
 *  called '<spotName>.Target'. However, this is just additional information
 *  then, the transformation tracks of the main node make the
 *  spot light already point in the right direction.
*)
PaiLight = ^aiLight;
aiLight = Packed Record
	(* The name of the light source.
	 *
	 *  There must be a node in the scenegraph with the same name.
	 *  This node specifies the position of the light in the scene
	 *  hierarchy and can be animated.
	 *)
	mName:aiString;

	(* The type of the light source.
 	 *
	 * aiLightSource_UNDEFINED is not a valid value for this member.
	 *)
	mType:aiLightSourceType;

	(* Position of the light source in space. Relative to the
	 *  transformation of the node corresponding to the light.
	 *
	 *  The position is undefined for directional lights.
	 *)
	mPosition:aiVector3D;

	(* Direction of the light source in space. Relative to the
	 *  transformation of the node corresponding to the light.
	 *
	 *  The direction is undefined for point lights. The vector
	 *  may be normalized, but it needn't.
	 *)
	mDirection:aiVector3D;

	(* Constant light attenuation factor.
	 *
	 *  The intensity of the light source at a given distance 'd' from
	 *  the light's position is
	 *  @code
	 *  Atten = 1/( att0 + att1 * d + att2 * d*d)
	 *  @endcode
	 *  This member corresponds to the att0 variable in the equation.
	 *  Naturally undefined for directional lights.
	 *)
	mAttenuationConstant:Single;

	(* Linear light attenuation factor.
	 *
	 *  The intensity of the light source at a given distance 'd' from
	 *  the light's position is
	 *  @code
	 *  Atten = 1/( att0 + att1 * d + att2 * d*d)
	 *  @endcode
	 *  This member corresponds to the att1 variable in the equation.
	 *  Naturally undefined for directional lights.
	 *)
	mAttenuationLinear:Single;

	(* Quadratic light attenuation factor.
	 *
	 *  The intensity of the light source at a given distance 'd' from
	 *  the light's position is
	 *  @code
	 *  Atten = 1/( att0 + att1 * d + att2 * d*d)
	 *  @endcode
	 *  This member corresponds to the att2 variable in the equation.
	 *  Naturally undefined for directional lights.
	 *)
	mAttenuationQuadratic:Single;

	(* Diffuse color of the light source
	 *
	 *  The diffuse light color is multiplied with the diffuse
	 *  material color to obtain the final color that contributes
	 *  to the diffuse shading term.
	 *)
	mColorDiffuse:aiColor3D;

	(* Specular color of the light source
	 *
	 *  The specular light color is multiplied with the specular
	 *  material color to obtain the final color that contributes
	 *  to the specular shading term.
	 *)
	mColorSpecular:aiColor3D;

	(* Ambient color of the light source
	 *
	 *  The ambient light color is multiplied with the ambient
	 *  material color to obtain the final color that contributes
	 *  to the ambient shading term. Most renderers will ignore
	 *  this value it, is just a remaining of the fixed-function pipeline
	 *  that is still supported by quite many file formats.
	 *)
	mColorAmbient:aiColor3D;

	(* Inner angle of a spot light's light cone.
	 *
	 *  The spot light has maximum influence on objects inside this
	 *  angle. The angle is given in radians. It is 2PI for point
	 *  lights and undefined for directional lights.
	 *)
	mAngleInnerCone:Single;

	(* Outer angle of a spot light's light cone.
	 *
	 *  The spot light does not affect objects outside this angle.
	 *  The angle is given in radians. It is 2PI for point lights and
	 *  undefined for directional lights. The outer angle must be
	 *  greater than or equal to the inner angle.
	 *  It is assumed that the application uses a smooth
	 *  interpolation between the inner and the outer cone of the
	 *  spot light.
	 *)
	mAngleOuterCone:Single;
End;

PaiMeshArray = ^aiMeshArray;
aiMeshArray = Array[0..0] Of PaiMesh;

PaiMaterialArray = ^aiMaterialArray;
aiMaterialArray = Array[0..0] Of PaiMaterial;

PaiAnimationArray = ^aiAnimationArray;
aiAnimationArray = Array[0..0] Of PaiAnimation;

PaiTextureArray = ^aiTextureArray;
aiTextureArray = Array[0..0] Of PaiTexture;

PaiLightArray = ^aiLightArray;
aiLightArray = Array[0..0] Of PaiLight;

PaiCameraArray = ^aiCameraArray;
aiCameraArray = Array[0..0] Of PaiCamera;

PaiScene = ^aiScene;
aiScene = Packed Record
	(** Any combination of the AI_SCENE_FLAGS_XXX flags. By default
	* this value is 0, no flags are set. Most applications will
	* want to reject all scenes with the AI_SCENE_FLAGS_INCOMPLETE
	* bit set.
	*)
	mFlags:Cardinal;


	(* The root node of the hierarchy.
	*
	* There will always be at least the root node if the import
	* was successful (and no special flags have been set).
	* Presence of further nodes depends on the format and content
	* of the imported file.
	*)
	mRootNode:PaiNode;



	// The number of meshes in the scene.
	mNumMeshes:Cardinal;

	(* The array of meshes.
	*
	* Use the indices given in the aiNode structure to access
	* this array. The array is mNumMeshes in size. If the
	* AI_SCENE_FLAGS_INCOMPLETE flag is not set there will always
	* be at least ONE material.
	*)
	mMeshes:PaiMeshArray;



	// The number of materials in the scene.
	mNumMaterials:Cardinal;

	(* The array of materials.
	*
	* Use the index given in each aiMesh structure to access this
	* array. The array is mNumMaterials in size. If the
	* AI_SCENE_FLAGS_INCOMPLETE flag is not set there will always
	* be at least ONE material.
	*)
	mMaterials:PaiMaterialArray;



	// The number of animations in the scene. */
	mNumAnimations:Cardinal;

	(* The array of animations.
	*
	* All animations imported from the given file are listed here.
	* The array is mNumAnimations in size.
	*)
	mAnimations:PaiAnimationArray;



	// The number of textures embedded into the file
	mNumTextures:Cardinal;

	(* The array of embedded textures.
	*
	* Not many file formats embed their textures into the file.
	* An example is Quake's MDL format (which is also used by
	* some GameStudio versions)
	*)
	mTextures:PaiTextureArray;


	(* The number of light sources in the scene. Light sources
	* are fully optional, in most cases this attribute will be 0
        *)
	mNumLights:Cardinal;

	(* The array of light sources.
	*
	* All light sources imported from the given file are
	* listed here. The array is mNumLights in size.
	*)
	mLights:PaiLightArray;


	(* The number of cameras in the scene. Cameras
	* are fully optional, in most cases this attribute will be 0
        *)
	mNumCameras:Cardinal;

	(* The array of cameras.
	*
	* All cameras imported from the given file are listed here.
	* The array is mNumCameras in size. The first camera in the
	* array (if existing) is the default camera view into
	* the scene.
	*)
	mCameras:PaiCameraArray;
End;


//assimp.h
Type
	aiLogStreamCallback = Procedure (message, user:PChar); Cdecl;

  PaiLogStream= ^aiLogStream;
	aiLogStream = Packed Record
	// callback to be called 
		 callback:aiLogStreamCallback;

	// user data to be passed to the callback 
		user:PChar;
	End;

(* Reads the given file and returns its content.
 * 
 * If the call succeeds, the imported data is returned in an aiScene structure. 
 * The data is intended to be read-only, it stays property of the ASSIMP 
 * library and will be stable until aiReleaseImport() is called. After you're 
 * done with it, call aiReleaseImport() to free the resources associated with 
 * this file. If the import fails, NULL is returned instead. Call 
 * aiGetErrorString() to retrieve a human-readable error text.
 * @param pFile Path and filename of the file to be imported, 
 *   expected to be a null-terminated c-string. NULL is not a valid value.
 * @param pFlags Optional post processing steps to be executed after
 *   a successful import. Provide a bitwise combination of the 
 *   #aiPostProcessSteps flags.
 * @return Pointer to the imported data or NULL if the import failed. 
 *)
Function aiImportFile(pFile:PChar; pFlags:Cardinal):PaiScene; cdecl; external AssimpLib;

// --------------------------------------------------------------------------------
{/** Exports the given scene to a chosen file format and writes the result file(s) to disk.
* @param pScene The scene to export. Stays in possession of the caller, is not changed by the function.
*   The scene is expected to conform to Assimp's Importer output format as specified
*   in the @link data Data Structures Page @endlink. In short, this means the model data
*   should use a right-handed coordinate systems, face winding should be counter-clockwise
*   and the UV coordinate origin is assumed to be in the upper left. If your input data
*   uses different conventions, have a look at the last parameter.
* @param pFormatId ID string to specify to which format you want to export to. Use
* aiGetExportFormatCount() / aiGetExportFormatDescription() to learn which export formats are available.
* @param pFileName Output file to write
* @param pPreprocessing Accepts any choice of the #aiPostProcessSteps enumerated
*   flags, but in reality only a subset of them makes sense here. Specifying
*   'preprocessing' flags is useful if the input scene does not conform to
*   Assimp's default conventions as specified in the @link data Data Structures Page @endlink.
*   In short, this means the geometry data should use a right-handed coordinate systems, face
*   winding should be counter-clockwise and the UV coordinate origin is assumed to be in
*   the upper left. The #aiProcess_MakeLeftHanded, #aiProcess_FlipUVs and
*   #aiProcess_FlipWindingOrder flags are used in the import side to allow users
*   to have those defaults automatically adapted to their conventions. Specifying those flags
*   for exporting has the opposite effect, respectively. Some other of the
*   #aiPostProcessSteps enumerated values may be useful as well, but you'll need
*   to try out what their effect on the exported file is. Many formats impose
*   their own restrictions on the structure of the geometry stored therein,
*   so some preprocessing may have little or no effect at all, or may be
*   redundant as exporters would apply them anyhow. A good example
*   is triangulation - whilst you can enforce it by specifying
*   the #aiProcess_Triangulate flag, most export formats support only
*   triangulate data so they would run the step anyway.
*
*   If assimp detects that the input scene was directly taken from the importer side of
*   the library (i.e. not copied using aiCopyScene and potetially modified afterwards),
*   any postprocessing steps already applied to the scene will not be applied again, unless
*   they show non-idempotent behaviour (#aiProcess_MakeLeftHanded, #aiProcess_FlipUVs and
*   #aiProcess_FlipWindingOrder).
* @return a status code indicating the result of the export
* @note Use aiCopyScene() to get a modifiable copy of a previously
*   imported scene.
*/}

function aiExportScene(const aiScene:PaiScene; const  pFormatId:PChar;const pFileName:PChar;pPreprocessing:Cardinal):TaiReturn; cdecl; external AssimpLib;


// --------------------------------------------------------------------------------
///** Returns the number of export file formats available in the current Assimp build.
// * Use aiGetExportFormatDescription() to retrieve infos of a specific export format.

function aiGetExportFormatCount:Integer; cdecl; external AssimpLib;


// --------------------------------------------------------------------------------
{/** Returns a description of the nth export file format. Use #aiGetExportFormatCount()
 * to learn how many export formats are supported.
 * @param pIndex Index of the export format to retrieve information for. Valid range is
 *    0 to #aiGetExportFormatCount()
 * @return A description of that specific export format. NULL if pIndex is out of range.
}
function aiGetExportFormatDescription(pIndex:integer):paiExportFormatDesc;cdecl; external AssimpLib;

function aiCreatePropertyStore():pointer; cdecl; external AssimpLib;

procedure aiReleasePropertyStore(props:pointer); cdecl; external AssimpLib;



(* Reads the given file using user-defined I/O functions and returns 
 *   its content.
 * 
 * If the call succeeds, the imported data is returned in an aiScene structure. 
 * The data is intended to be read-only, it stays property of the ASSIMP 
 * library and will be stable until aiReleaseImport() is called. After you're 
 * done with it, call aiReleaseImport() to free the resources associated with 
 * this file. If the import fails, NULL is returned instead. Call 
 * aiGetErrorString() to retrieve a human-readable error text.
 * @param pFile Path and filename of the file to be imported, 
 *   expected to be a null-terminated c-string. NULL is not a valid value.
 * @param pFlags Optional post processing steps to be executed after 
 *   a successful import. Provide a bitwise combination of the
 *   #aiPostProcessSteps flags.
 * @param pFS aiFileIO structure. Will be used to open the model file itself
 *   and any other files the loader needs to open.
 * @return Pointer to the imported data or NULL if the import failed.  
 * @note Include <aiFileIO.h> for the definition of #aiFileIO.
 *)
Function aiImportFileWithProperties( pFile:PChar; pFlags:Cardinal; props:pointer):PaiScene; cdecl; external AssimpLib;

(* Reads the given file from a given memory buffer,
 * 
 * If the call succeeds, the contents of the file are returned as a pointer to an
 * aiScene object. The returned data is intended to be read-only, the importer keeps 
 * ownership of the data and will destroy it upon destruction. If the import fails, 
 * NULL is returned.
 * A human-readable error description can be retrieved by calling aiGetErrorString(). 
 * @param pBuffer Pointer to the file data
 * @param pLength Length of pBuffer, in bytes
 * @param pFlags Optional post processing steps to be executed after 
 *   a successful import. Provide a bitwise combination of the 
 *   #aiPostProcessSteps flags. If you wish to inspect the imported
 *   scene first in order to fine-tune your post-processing setup,
 *   consider to use #aiApplyPostProcessing().
 * @param pHint An additional hint to the library. If this is a non empty string,
 *   the library looks for a loader to support the file extension specified by pHint
 *   and passes the file to the first matching loader. If this loader is unable to 
 *   completely the request, the library continues and tries to determine the file
 *   format on its own, a task that may or may not be successful. 
 *   Check the return value, and you'll know ...
 * @return A pointer to the imported data, NULL if the import failed.
 *
 * @note This is a straightforward way to decode models from memory buffers, but it 
 * doesn't handle model formats spreading their data across multiple files or even
 * directories. Examples include OBJ or MD3, which outsource parts of their material
 * stuff into external scripts. If you need the full functionality, provide a custom 
 * IOSystem to make Assimp find these files.
 *)
Function aiImportFileFromMemory(pBuffer:Pointer; pLength:Cardinal; pFlags:Cardinal; pHint:PChar):PAiScene; cdecl; external AssimpLib;

Procedure aiReleaseImport(pScene:PaiScene); cdecl; external AssimpLib;

(* Returns the error text of the last failed import process.
 *
 * @return A textual description of the error that occurred at the last
 * import process. NULL if there was no error. There can't be an error if you
 * got a non-NULL #aiScene from #aiImportFile/#aiImportFileEx/#aiApplyPostProcessing.
 *)
Function aiGetErrorString():PChar; cdecl; external AssimpLib;

(* Returns whether a given file extension is supported by ASSIMP
 *
 * @param szExtension Extension for which the function queries support for.
 * Must include a leading dot '.'. Example: '.3ds', '.md3'
 * @return AI_TRUE if the file extension is supported.
 *)
Function aiIsExtensionSupported(szExtension:PChar):Boolean; cdecl; external AssimpLib;

(* Get a list of all file extensions supported by ASSIMP.
 *
 * If a file extension is contained in the list this does, of course, not
 * mean that ASSIMP is able to load all files with this extension.
 * @param szOut String to receive the extension list.
 * Format of the list: '*.3ds;*.obj;*.dae'. NULL is not a valid parameter.
 *)
Procedure aiGetExtensionList(Var szOut:aiString); cdecl; external AssimpLib;

(* Get the storage required by an imported asset
 * @param pIn Input asset.
 * @param in Data structure to be filled. 
 *)
Procedure aiGetMemoryRequirements( pIn:PaiScene; info:PaiMemoryInfo); cdecl; external AssimpLib;


(** Set an integer property. 
 *
 *  This is the C-version of #Assimp::Importer::SetPropertyInteger(). In the C 
 *  interface, properties are always shared by all imports. It is not possible to 
 *  specify them per import.
 *
 * @param szName Name of the configuration property to be set. All supported 
 *   public properties are defined in the aiConfig.h header file (#AI_CONFIG_XXX).
 * @param value New value for the property
 *)
Procedure aiSetImportPropertyInteger(prop:Pointer;szName:PChar; value:Integer);cdecl; external AssimpLib;

(* Set a floating-point property. 
 *
 *  This is the C-version of #Assimp::Importer::SetPropertyFloat(). In the C
 *  interface, properties are always shared by all imports. It is not possible to 
 *  specify them per import.
 *
 * @param szName Name of the configuration property to be set. All supported 
 *   public properties are defined in the aiConfig.h header file (#AI_CONFIG_XXX).
 * @param value New value for the property
 *)
Procedure aiSetImportPropertyFloat(prop:Pointer;szName:PChar; value:Single); cdecl; external AssimpLib;

(* Set a string property. 
 *
 *  This is the C-version of #Assimp::Importer::SetPropertyString(). In the C 
 *  interface, properties are always shared by all imports. It is not possible to 
 *  specify them per import.
 *
 * @param szName Name of the configuration property to be set. All supported 
 *   public properties are defined in the aiConfig.h header file (#AI_CONFIG_XXX).
 * @param value New value for the property
 *)
Procedure aiSetImportPropertyString(prop:Pointer;szName:PChar; Var st:aiString); cdecl; external AssimpLib;




Function aiStringGetValue(Str:aiString): String;
function aiAnimationGetChannel(Anim:PAiAnimation; Name: String): Integer;


//
 function aiScene_GetNumMaterials( scene:PaiScene):integer; cdecl; external AssimpLib;
 function aiScene_GetNumMeshes( scene:PaiScene):integer; cdecl; external AssimpLib;
 function aiScene_GetNumAnimations( scene:PaiScene):integer; cdecl; external AssimpLib;
 function aiScene_GetRootNode( scene:PaiScene):pAiNode;cdecl; external AssimpLib;

 function aiScene_HasMaterials( scene:PaiScene):Boolean; cdecl; external AssimpLib;
 function aiScene_HasMeshes( scene:PaiScene):Boolean; cdecl; external AssimpLib;
 function aiScene_HasAnimation( scene:PaiScene):Boolean; cdecl; external AssimpLib;

 function aiMaterial_GetTextureCount( scene:PaiScene;Layer:integer; _type:integer):integer; cdecl; external AssimpLib;
 function aiMaterial_GetTexture( scene:PaiScene;Layer:integer;_type:integer):pchar; cdecl; external AssimpLib;
 function aiMaterial_GetName( scene:PaiScene;Layer:integer):PChar; cdecl; external AssimpLib;
 function aiMaterial_GetAmbient( scene:PaiScene;Layer:integer):aiColor4D;cdecl; external AssimpLib;
 function aiMaterial_GetDiffuse( scene:PaiScene;Layer:integer):aiColor4D;cdecl; external AssimpLib;
 function aiMaterial_GetSpecular( scene:PaiScene;Layer:integer):aiColor4D;cdecl; external AssimpLib;
 function aiMaterial_GetEmissive( scene:PaiScene;Layer:integer):aiColor4D;cdecl; external AssimpLib;
 function aiMaterial_GetShininess( scene:PaiScene;Layer:integer):Single;cdecl; external AssimpLib;
 function aiMaterial_GetTransparency( scene:PaiScene;Layer:integer):Single;cdecl; external AssimpLib;

function aiMesh_GetNumVertices(scene:PaiScene;Mesh:integer):Integer;cdecl; external AssimpLib;
function aiMesh_GetNumFaces(scene:PaiScene;Mesh:integer):Integer;cdecl; external AssimpLib;
function aiMesh_GetNumBones(scene:PaiScene;Mesh:integer):Integer;cdecl; external AssimpLib;
function aiMesh_GetMaterialIndex(scene:PaiScene;Mesh:integer):Integer;cdecl; external AssimpLib;
function aiMesh_GetNumUVChannels(scene:PaiScene;Mesh:integer):Integer;cdecl; external AssimpLib;
function aiMesh_GetNumColorChannels(scene:PaiScene;Mesh:integer):Integer;cdecl; external AssimpLib;
function aiMesh_GetName(scene:PaiScene;Mesh:integer):pchar;cdecl; external AssimpLib;

 function  aiMesh_HasPositions(scene:PaiScene;Mesh:integer):Boolean;cdecl; external AssimpLib;
 function  aiMesh_HasFaces(scene:PaiScene;Mesh:integer):Boolean;cdecl; external AssimpLib;
 function  aiMesh_HasNormals(scene:PaiScene;Mesh:integer):Boolean;cdecl; external AssimpLib;
 function  aiMesh_HasTangentsAndBitangents(scene:PaiScene;Mesh:integer):Boolean;cdecl; external AssimpLib;
 function  aiMesh_HasVertexColors(scene:PaiScene;Mesh:integer;Index:integer):Boolean;cdecl; external AssimpLib;
 function  aiMesh_HasTextureCoords(scene:PaiScene;Mesh:integer;Index:integer):Boolean;cdecl; external AssimpLib;

 function  aiMesh_GetNumIndicesPerFace(scene:PaiScene;Mesh:integer;Index:Integer):Integer;cdecl; external AssimpLib;
 function  aiMesh_Indice(scene:PaiScene;Mesh:integer;Index:integer; Face:integer):Integer;cdecl; external AssimpLib;

 function  aiMesh_Normal(scene:PaiScene;Mesh:integer;Index:integer):aiVector3D;cdecl; external AssimpLib;
 function  aiMesh_Vertex(scene:PaiScene;Mesh:integer;Index:integer):aiVector3D;cdecl; external AssimpLib;
 function  aiMesh_Tangent(scene:PaiScene;Mesh:integer;Index:integer):aiVector3D;cdecl; external AssimpLib;
 function  aiMesh_Bitangent(scene:PaiScene;Mesh:integer;Index:integer):aiVector3D;cdecl; external AssimpLib;
 function  aiMesh_TextureCoord(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):aiVector3D;cdecl; external AssimpLib;
 function  aiMesh_Color(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):aiColor4D;cdecl; external AssimpLib;

 function aiMesh_VertexX(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_VertexY(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_VertexZ(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_VertexNX(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_VertexNY(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_VertexNZ(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_TangentX(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_TangentY(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_TangentZ(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_BitangentX(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_BitangentY(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_BitangentZ(scene:PaiScene;Mesh:integer;Index:integer):single;cdecl; external AssimpLib;
 function aiMesh_TextureCoordX(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):single;cdecl; external AssimpLib;
 function aiMesh_TextureCoordY(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):single;cdecl; external AssimpLib;
  function aiMesh_TextureCoordW(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):single;cdecl; external AssimpLib;
 function aiMesh_ColorRed(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):single;cdecl; external AssimpLib;
 function aiMesh_ColorGreen(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):single;cdecl; external AssimpLib;
 function aiMesh_ColorBlue(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):single;cdecl; external AssimpLib;
 function aiMesh_ColorAlpha(scene:PaiScene;Mesh:integer;Index:integer;Layer:integer):single;cdecl; external AssimpLib;

function  aiMesh_GetBoneName(scene:PaiScene;Mesh:integer;Index:integer):PChar;cdecl; external AssimpLib;
function  aiMesh_GetBoneTransform(scene:PaiScene;Mesh:integer;Index:integer):aiMatrix4x4;cdecl; external AssimpLib;
function  aiMesh_GetNumBoneWeights(scene:PaiScene;Mesh:integer;Index:integer):Integer;cdecl; external AssimpLib;
function  aiMesh_GetBoneWeight(scene:PaiScene;Mesh:integer;Index:integer;Weight:integer):Single;cdecl; external AssimpLib;
function  aiMesh_GetBoneVertexId(scene:PaiScene;Mesh:integer;Index:integer;Weight:integer):Integer;cdecl; external AssimpLib;
function  aiMesh_GetBoneEulerRotation(scene:PaiScene;Mesh:integer;Index:integer):aiVector3D;cdecl; external AssimpLib;
function  aiMesh_GetBonePosition( scene:PaiScene;Mesh:integer;Index:integer):aiVector3D;cdecl; external AssimpLib;
function  aiMesh_GetBoneRotation(scene:PaiScene;Mesh:integer;Index:integer):aiQuaternion;cdecl; external AssimpLib;




 function aiAnim_GetDuration( scene:PaiScene;Animation:Integer):Double;cdecl; external AssimpLib;
 function aiAnim_GetTicksPerSecond( scene:PaiScene;Animation:Integer):Double;cdecl; external AssimpLib;
 function aiAnim_GetNumChannels( scene:PaiScene;Animation:Integer):Integer;cdecl; external AssimpLib;
 function aiAnim_GetChannelName( scene:PaiScene;Animation,Channel:integer):PChar;cdecl; external AssimpLib;
 function aiAnim_GetChannelIndex(scene:PaiScene;Animation:Integer;const  name:pchar):Integer;cdecl; external AssimpLib;
 function aiAnim_GetNumPositionKeys( scene:PaiScene;Animation,Channel:integer):Integer;cdecl; external AssimpLib;
 function aiAnim_GetNumRotationKeys( scene:PaiScene;Animation,Channel:integer):Integer;cdecl; external AssimpLib;
 function aiAnim_GetNumScalingKeys( scene:PaiScene;Animation,Channel:integer):Integer;cdecl; external AssimpLib;
 function aiAnim_GetPositionKey( scene:PaiScene;Animation,Channel:integer;Key:integer):aiVector3D;cdecl; external AssimpLib;
 function aiAnim_GetPositionFrame( scene:PaiScene;Animation,Channel:integer;Key:integer):Double;cdecl; external AssimpLib;
 function aiAnim_GetScalingKey( scene:PaiScene;Animation,Channel:integer;Key:integer):aiVector3D;cdecl; external AssimpLib;
 function aiAnim_GetScalingFrame( scene:PaiScene;Animation,Channel:integer;Key:integer):Double;cdecl; external AssimpLib;
 function aiAnim_GetRotationKey( scene:PaiScene;Animation,Channel:integer;Key:integer):aiQuaternion;cdecl; external AssimpLib;
 function aiAnim_GetRotationFrame( scene:PaiScene;Animation,Channel:integer;Key:integer):Double;cdecl; external AssimpLib;

 function aiNode_GetName( node:PaiNode):PChar;cdecl; external AssimpLib;
 function aiNode_GetNumChildren( node:PaiNode):Integer;cdecl; external AssimpLib;
 function aiNode_GetChild( node:PaiNode;Index:integer):pAiNode;cdecl; external AssimpLib;
 function aiNode_GetParent( node:PaiNode):pAiNode;cdecl; external AssimpLib;
 function aiNode_FindChild( node:PaiNode;const name:pchar):pAiNode;cdecl; external AssimpLib;
 function aiNode_GetTransformation( node:PaiNode):aiMatrix4x4;cdecl; external AssimpLib;
 function aiNode_GetEurlRotation(node:PaiNode):aiVector3D;cdecl; external AssimpLib;
 function aiNode_GetPosition( node:PaiNode):aiVector3D;cdecl; external AssimpLib;
 function aiNode_GetRotation(node:PaiNode):aiQuaternion;cdecl; external AssimpLib;


 // --------------------------------------------------------------------------------
{** Construct a quaternion from a 3x3 rotation matrix.
 *  @param quat Receives the output quaternion.
 *  @param mat Matrix to 'quaternionize'.
 *  @see aiQuaternion(const aiMatrix3x3& pRotMatrix)
 *}
procedure aiCreateQuaternionFromMatrix(quat: aiQuaternion; const mat: aiMatrix3x3) cdecl; external AssimpLib  ;

// --------------------------------------------------------------------------------
{** Decompose a transformation matrix into its rotational, translational and
 *  scaling components.
 * 
 * @param mat Matrix to decompose
 * @param scaling Receives the scaling component
 * @param rotation Receives the rotational component
 * @param position Receives the translational component.
 * @see aiMatrix4x4::Decompose (aiVector3D&, aiQuaternion&, aiVector3D&) const;
 *}
procedure aiDecomposeMatrix(const mat: aiMatrix4x4; var scaling: aiVector3D; var rotation: aiQuaternion;var  position: aiVector3D) cdecl; external AssimpLib ;
procedure aiDecomposeMatrixNoScaling(const mat: aiMatrix4x4;  var rotation: aiQuaternion;var  position: aiVector3D) cdecl; external AssimpLib ;


// --------------------------------------------------------------------------------
{** Transpose a 4x4 matrix.
 *  @param mat Pointer to the matrix to be transposed
 *}
procedure aiTransposeMatrix4(mat: aiMatrix4x4) cdecl; external AssimpLib ;

// --------------------------------------------------------------------------------
{** Transpose a 3x3 matrix.
 *  @param mat Pointer to the matrix to be transposed
 *}
procedure aiTransposeMatrix3(mat: aiMatrix3x3) cdecl; external AssimpLib  ;

// --------------------------------------------------------------------------------
{** Transform a vector by a 3x3 matrix
 *  @param vec Vector to be transformed.
 *  @param mat Matrix to transform the vector with.
 *}
procedure aiTransformVecByMatrix3(var vec: aiVector3D; const mat: aiMatrix3x3) cdecl; external AssimpLib ;

// --------------------------------------------------------------------------------
{** Transform a vector by a 4x4 matrix
 *  @param vec Vector to be transformed.
 *  @param mat Matrix to transform the vector with.
 *}
procedure aiTransformVecByMatrix4(var vec: aiVector3D; const mat: aiMatrix4x4) cdecl; external AssimpLib  ;

// --------------------------------------------------------------------------------
{** Multiply two 4x4 matrices.
 *  @param dst First factor, receives result.
 *  @param src Matrix to be multiplied with 'dst'.
 *}
procedure aiMultiplyMatrix4(var dst: aiMatrix4x4; const src: aiMatrix4x4) cdecl; external AssimpLib ;

// --------------------------------------------------------------------------------
{** Multiply two 3x3 matrices.
 *  @param dst First factor, receives result.
 *  @param src Matrix to be multiplied with 'dst'.
 *}
procedure aiMultiplyMatrix3(var dst: aiMatrix3x3; const src: aiMatrix3x3) cdecl; external AssimpLib  ;

// --------------------------------------------------------------------------------
{** Get a 3x3 identity matrix.
 *  @param mat Matrix to receive its personal identity
 *}
procedure aiIdentityMatrix3(var mat: aiMatrix3x3) cdecl; external AssimpLib  ;

// --------------------------------------------------------------------------------
{** Get a 4x4 identity matrix.
 *  @param mat Matrix to receive its personal identity
 *}
procedure aiIdentityMatrix4(var mat: aiMatrix4x4) cdecl; external AssimpLib  ;


procedure aiCreateLogger;cdecl; external AssimpLib  ;
procedure aiDestroyLogger;cdecl; external AssimpLib  ;
procedure aiLogInfo(const msg:PChar);cdecl; external AssimpLib  ;
procedure aiLogDebug(const msg:PChar);cdecl; external AssimpLib  ;
procedure aiLogError(const msg:PChar);cdecl; external AssimpLib  ;
procedure aiLogWarn(const msg:PChar);cdecl; external AssimpLib  ;




Implementation


{ aiString }

Function aiStringGetValue(Str:aiString): String;
Begin
  SetLength(Result, str.length);
  If str.Length>0 Then
    Move(str.Data[0], Result[1], str.length);
End;

{ aiAnimation }

function aiAnimationGetChannel(Anim:PAiAnimation; Name: String): Integer;
Var
  I:Integer;
begin
  Result := -1;

  For I:=0 To Pred(Anim.mNumChannels) Do
  If (aiStringGetValue(Anim.mChannels[I].mNodeName) = Name) Then
  Begin
    Result := I;
    Exit;
  End;
end;

End.