using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class ImportAssimpSkinnedCombine
{
    public static bool saveAssets = true;
    public static bool useTangents = true;
    public static bool ignoreRotations = false;
    public static bool ignorePositions = true;
    public static bool ignoreScalling = true;

    private static StreamWriter streamWriter;
    private static List<AssimpJoint> listJoints = new List<AssimpJoint>();
    [MenuItem("Assets/Djoker Tools/Assimp/ImportSkinnedOptimize")]
    static void init()
    {

        string filename = Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
/*
        FileStream fileStream = new FileStream("Assets/Log.txt",
                                     FileMode.Create,
                                      FileAccess.Write,
                                      FileShare.ReadWrite);
        streamWriter = new StreamWriter(fileStream);//"Assets/Log.txt",false);
        streamWriter.WriteLine("Import mesh");
*/
        
        readMesh(rootPath, filename, "Textures/");
        /*
        streamWriter.Flush();
        streamWriter.Close();
        */

    }

   



    private static void trace(string msg)
    {
      //  streamWriter.WriteLine("LOG:" + msg);
    }

    public class IBone
    {
        public int BoneIndex;
        public float Weight;
        public IBone()
        {
            BoneIndex = 0;
            Weight = 0;
        }


    }

   

    public class BoneVertex
    {

        public IBone[] bones;
        public int numbones;
        public BoneVertex()
        {
            bones = new IBone[4];
            for (int i = 0; i < 4; i++)
            {
                bones[i] = new IBone();
            }

            numbones = 0;
        }

        public void addBone(int bone, float w)
        {
            for (int i = 0; i < 4; i++)
            {
                if (bones[i].Weight == 0)
                {
                    bones[i].BoneIndex = bone;
                    bones[i].Weight = w;
                    numbones++;
                    return;
                }
            }

           
        }

    }



    public class AssimpJoint
    {
        public string parentName;
        public string Name;
        public string Path;
        public Vector3 Position;
        public Quaternion Orientation;
        public AssimpJoint parent;
        public Transform transform;



        public AssimpJoint()
        {

            Name = "";
            parentName = "";
            parent = null;
            Path = "";

        }

    }

    public class AssimpMesh
    {
        public Mesh geometry;
        public string name;
        public List<AssimpJoint> joints;
        public List<Transform> bones;
        public List<BoneVertex> BoneVertexList;
        public List<BoneWeight> boneWeightsList;
        public Material material;
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Vector4> tangents;
        public List<Vector2> uvcoords;
        public List<int> faces;
        public GameObject meshContainer;
        public SkinnedMeshRenderer meshRenderer;
        public GameObject Root;



        public AssimpMesh(GameObject parent, GameObject root, string name)
        {
            this.name = name;
            meshContainer = new GameObject(name);
            this.Root = root;
            meshContainer.transform.parent = parent.transform;
            meshRenderer = (SkinnedMeshRenderer)meshContainer.AddComponent(typeof(SkinnedMeshRenderer));
            meshRenderer.sharedMesh = new Mesh();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Diffuse"));
            geometry = meshRenderer.sharedMesh;
            joints = new List<AssimpJoint>();
            BoneVertexList = new List<BoneVertex>();
            boneWeightsList = new List<BoneWeight>();
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            uvcoords = new List<Vector2>();
            tangents= new List<Vector4>();
            faces = new List<int>();
            bones = new List<Transform>();

        }
        public void addVertex(Vector3 pos, Vector3 normal,Vector4 tan, Vector2 uv)
        {
            vertices.Add(pos);
            normals.Add(normal);
            uvcoords.Add(uv);
           if(useTangents) tangents.Add(tan);
            BoneVertexList.Add(new BoneVertex());

        }
        public void setmaterial(Material mat)
        {
            meshRenderer.sharedMaterial = mat;
        }

        public void addFace(int a, int b, int c)
        {
            faces.Add(a);
            faces.Add(b);
            faces.Add(c);
        }
        public void addJoint(AssimpJoint joint)
        {
            joints.Add(joint);
            bones.Add(joint.transform);
        }
        public void addBone(int vertexID, int JointId, float force)
        {
            BoneVertex vertex = BoneVertexList[vertexID];
            vertex.addBone(JointId, force);
            BoneVertexList[vertexID] = vertex;
        }
        public void BuilSkin()
        {

             
            
            for (int i = 0; i < BoneVertexList.Count; i++)
            {
                BoneVertex vertex = BoneVertexList[i];
                var wight = new BoneWeight();
                float len=0;


                 var orderdWeights=vertex.bones.OrderByDescending(t=>t.Weight).ToList();
                 switch (orderdWeights.Count)
                 {
                     case 0:

                         break;
                     case 1:


                         wight.boneIndex0 = orderdWeights[0].BoneIndex;
                         wight.weight0 = orderdWeights[0].Weight;

                         break;
                     case 2:

                         wight.boneIndex0 = orderdWeights[0].BoneIndex;

                         wight.boneIndex1 = orderdWeights[1].BoneIndex;

                         len = Mathf.Sqrt(orderdWeights[0].Weight * orderdWeights[0].Weight +
                                          orderdWeights[1].Weight * orderdWeights[1].Weight);
                         wight.weight0 = orderdWeights[0].Weight / len;
                         wight.weight1 = orderdWeights[1].Weight / len;
                         break;
                     case 3:

                         wight.boneIndex0 = orderdWeights[0].BoneIndex;
                         wight.boneIndex1 = orderdWeights[1].BoneIndex;
                         wight.boneIndex2 = orderdWeights[2].BoneIndex;

                         len = Mathf.Sqrt(orderdWeights[0].Weight * orderdWeights[0].Weight +
                                          orderdWeights[1].Weight * orderdWeights[1].Weight +
                                          orderdWeights[2].Weight * orderdWeights[2].Weight);
                         wight.weight0 = orderdWeights[0].Weight / len;
                         wight.weight1 = orderdWeights[1].Weight / len;
                         wight.weight2 = orderdWeights[2].Weight / len;

                         break;
                     default:

                         wight.boneIndex0 = orderdWeights[0].BoneIndex;
                         wight.boneIndex1 = orderdWeights[1].BoneIndex;
                         wight.boneIndex2 = orderdWeights[2].BoneIndex;
                         wight.boneIndex3 = orderdWeights[3].BoneIndex;

                         len = Mathf.Sqrt(orderdWeights[0].Weight * orderdWeights[0].Weight +
                                          orderdWeights[1].Weight * orderdWeights[1].Weight +
                                          orderdWeights[2].Weight * orderdWeights[2].Weight +
                                          orderdWeights[3].Weight * orderdWeights[3].Weight);
                         wight.weight0 = orderdWeights[0].Weight / len;
                         wight.weight1 = orderdWeights[1].Weight / len;
                         wight.weight2 = orderdWeights[2].Weight / len;
                         wight.weight3 = orderdWeights[3].Weight / len;

                         break;
                 }


                 trace("----------");
                 trace(" ");
                 trace(string.Format(" vertex:{0} num Bones:{1} force {2}", i, vertex.numbones,len));

                 trace(string.Format(" bone index :{0}  w:{1}", wight.boneIndex0, wight.weight0));
                 trace(string.Format(" bone index :{0}  w:{1}", wight.boneIndex1, wight.weight1));
                 trace(string.Format(" bone index :{0}  w:{1}", wight.boneIndex2, wight.weight2));
                 trace(string.Format(" bone index :{0}  w:{1}", wight.boneIndex3, wight.weight3));

                 boneWeightsList.Add(wight);

             

            }

      
        }
        public void build()
        {
            List<Matrix4x4> bindposes = new List<Matrix4x4>();

            for (int i = 0; i < joints.Count; i++)
            {
                AssimpJoint joint = joints[i];
                Transform bone = bones[i];
                bindposes.Add(bone.worldToLocalMatrix * Root.transform.localToWorldMatrix);


            }


            geometry.vertices = vertices.ToArray();
            geometry.normals = normals.ToArray();
            geometry.uv = uvcoords.ToArray();
            geometry.triangles = faces.ToArray();
            geometry.bindposes = bindposes.ToArray();
            geometry.boneWeights = boneWeightsList.ToArray();
            if(useTangents)geometry.tangents = tangents.ToArray();
            geometry.RecalculateBounds();
        //    geometry.RecalculateNormals();
            geometry.Optimize();
            meshRenderer.sharedMesh = geometry;
            meshRenderer.bones = bones.ToArray();


        }
        public void dispose()
        {
            joints.Clear();
            boneWeightsList.Clear();
            BoneVertexList.Clear();
            vertices.Clear();
            normals.Clear();
            faces.Clear();
            tangents.Clear();
            uvcoords.Clear();
        }


    }

    private static void readMesh(string path, string filename, string texturepath)
    {
        string importingAssetsDir;

        if (File.Exists(path + "/" + filename))
        {



            Assimp.PostProcessSteps flags = (
         //  Assimp.PostProcessSteps.MakeLeftHanded |
           Assimp.PostProcessSteps.Triangulate |
           Assimp.PostProcessSteps.CalculateTangentSpace |
           Assimp.PostProcessSteps.GenerateUVCoords |
           Assimp.PostProcessSteps.GenerateSmoothNormals |
            Assimp.PostProcessSteps.RemoveComponent |
           Assimp.PostProcessSteps.JoinIdenticalVertices);

            IntPtr config = Assimp.aiCreatePropertyStore();

            Assimp.aiSetImportPropertyFloat(config, Assimp.AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE, 60.0f);
            Assimp.aiSetImportPropertyInteger(config,Assimp.AI_CONFIG_PP_LBW_MAX_WEIGHTS,4);
            // IntPtr scene = Assimp.aiImportFile(path + "/" + filename, (uint)flags);
            IntPtr scene = Assimp.aiImportFileWithProperties(path + "/" + filename, (uint)flags, config);
            Assimp.aiReleasePropertyStore(config);
            if (scene == null)
            {

                Debug.LogWarning("failed to read file: " + path + "/" + filename);
                return;
            }
            else
            {


                string nm = Path.GetFileNameWithoutExtension(filename);


                importingAssetsDir = "Assets/Prefabs/" + nm + "/";

                if (saveAssets)
                {
                    if (!Directory.Exists(importingAssetsDir))
                    {
                        Directory.CreateDirectory(importingAssetsDir);
                    }
                    AssetDatabase.Refresh();
                }


                GameObject ObjectRoot = new GameObject(nm);
                GameObject meshContainer = new GameObject(nm + "_Mesh");
                meshContainer.transform.parent = ObjectRoot.transform;




                //  List<CombineInstance> combineInstances = new List<CombineInstance>();
                List<Material> materials = new List<Material>();
                List<AssimpMesh> MeshList = new List<AssimpMesh>();


           
                for (int i = 0; i < Assimp.aiScene_GetNumMaterials(scene); i++)
                {
                    string matName = Assimp.aiMaterial_GetName(scene, i);
                    matName = nm + "_mat" + i;

                    //  string fname = Path.GetFileNameWithoutExtension(Assimp.aiMaterial_GetTexture(scene, i, (int)Assimp.TextureType.Diffuse));
                    string fname = Path.GetFileName(Assimp.aiMaterial_GetTexture(scene, i, (int)Assimp.TextureType.Diffuse));
                    Debug.Log("texture " + fname + "Material :" + matName);

                    Color ambient = Assimp.aiMaterial_GetAmbient(scene, i);
                    Color diffuse = Assimp.aiMaterial_GetDiffuse(scene, i);
                    Color specular = Assimp.aiMaterial_GetSpecular(scene, i);
                    Color emissive = Assimp.aiMaterial_GetEmissive(scene, i);

                    Material mat = new Material(Shader.Find("Diffuse"));
                    mat.name = matName;

                    string texturename = path + "/" + fname;

                    Texture2D tex = null;
                    if (File.Exists(texturename))
                    {
                        tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturename, typeof(Texture2D));
                    }
                    else
                        if (File.Exists(texturename + ".PNG"))
                        {
                            tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturename + ".PNG", typeof(Texture2D));
                        }
                        else
                            if (File.Exists(texturename + ".JPG"))
                            {
                                tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturename + ".JPG", typeof(Texture2D));
                            }
                            else
                                if (File.Exists(texturename + ".BMP"))
                                {
                                    tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturename + ".BMP", typeof(Texture2D));
                                }
                                else
                                    if (File.Exists(texturename + ".TGA"))
                                    {
                                        tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturename + ".TGA", typeof(Texture2D));
                                    }
                                    else
                                        if (File.Exists(texturename + ".DDS"))
                                        {
                                            tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturename + ".DDS", typeof(Texture2D));
                                        }
                    
                    if (tex != null)
                    {
                        string p = AssetDatabase.GetAssetPath(tex);
                        ConfigureForAtlas(p);


                        Debug.Log("LOAD (" + texturename + ") texture");
                        mat.SetTexture("_MainTex", tex);

                    }
                    else
                    {
                        Debug.LogError("Fail LOAD (" + texturename + ") error");
                    }


                    materials.Add(mat);

                }

                AssetDatabase.Refresh();

                if (Assimp.aiScene_GetRootNode(scene) != null)
                {
                    ObjectRoot.transform.position = Assimp.aiNode_GetPosition(Assimp.aiScene_GetRootNode(scene));

                    //assimp quaternion is w,x,y,z and unity x,y,z,w bu int this lib i fix this for unity
                    Quaternion assQuad = Assimp.aiNode_GetRotation(Assimp.aiScene_GetRootNode(scene));
                    ObjectRoot.transform.rotation = assQuad;

                   
                    GameObject skeleton = new GameObject("Skeleton");
                    skeleton.transform.parent = ObjectRoot.transform;
                    processNodes(scene, Assimp.aiScene_GetRootNode(scene), ref listJoints);

                    for (int i = 0; i < listJoints.Count; i++)
                    {

                        AssimpJoint joint = listJoints[i];
                        //Transform bone = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                        Transform bone = new GameObject(joint.Name).transform;
                     //   DebugBone debug = (DebugBone)bone.gameObject.AddComponent(typeof(DebugBone));



                        bone.name = joint.Name;
                        bone.parent = skeleton.transform;

                        if (getBoneByName(joint.parentName) != null)
                        {
                            int index = findBoneByName(joint.parentName);
                            bone.parent = joint.parent.transform;
                        }
                        bone.localPosition = joint.Position;
                        bone.localRotation = joint.Orientation;

                        joint.transform = bone;
                    }

                }




                if (Assimp.aiScene_HasMeshes(scene))
                {



                    for (int i = 0; i < Assimp.aiScene_GetNumMeshes(scene); i++)
                    {
                        string name = "Mesh_";
                        name += i.ToString();


                        bool HasNormals = Assimp.aiMesh_HasNormals(scene, i);
                        bool HasTexCoord = Assimp.aiMesh_HasTextureCoords(scene, i, 0);
                        bool HasFaces = Assimp.aiMesh_HasFaces(scene, i);


                        AssimpMesh mesh = new AssimpMesh(meshContainer, ObjectRoot, name);
                        mesh.setmaterial(materials[Assimp.aiMesh_GetMaterialIndex(scene, i)]);
                        MeshList.Add(mesh);



                        for (int v = 0; v < Assimp.aiMesh_GetNumVertices(scene, i); v++)
                        {

                            Vector3 vertex = Assimp.aiMesh_Vertex(scene, i, v);
                            Vector3 n = Assimp.aiMesh_Normal(scene, i, v);
                            float x = Assimp.aiMesh_TextureCoordX(scene, i, v, 0);
                            float y = Assimp.aiMesh_TextureCoordY(scene, i, v, 0);

                            Vector3 binormalf = Assimp.aiMesh_Bitangent(scene, i, v);
                            Vector3 tangentf = Assimp.aiMesh_Tangent(scene, i, v);

                            Vector4 outputTangent = new Vector4(tangentf.x, tangentf.y, tangentf.z, 0.0F);

                            float dp = Vector3.Dot(Vector3.Cross(n, tangentf), binormalf);
                            if (dp > 0.0F)
                                outputTangent.w = 1.0F;
                            else
                                outputTangent.w = -1.0F;


                            mesh.addVertex(vertex, n, outputTangent, new Vector2(x, y));
                            

                        }

                        for (int f = 0; f < Assimp.aiMesh_GetNumFaces(scene, i); f++)
                        {
                            int a = Assimp.aiMesh_Indice(scene, i, f, 0);
                            int b = Assimp.aiMesh_Indice(scene, i, f, 1);
                            int c = Assimp.aiMesh_Indice(scene, i, f, 2);
                            mesh.addFace(a, b, c);
                        }




                        //****
                        int numBone = Assimp.aiMesh_GetNumBones(scene, i);


                        for (int b = 0; b < numBone; b++)
                        {
                            string bname = Assimp.aiMesh_GetBoneName(scene, i, b);
                            AssimpJoint joint = getBoneByName(bname);
                            int boneID = findBoneByName(bname);


                            int numWeights = Assimp.aiMesh_GetNumBoneWeights(scene, i, b);
                            for (int w = 0; w < numWeights; w++)
                            {
                                float Weight = Assimp.aiMesh_GetBoneWeight(scene, i, b, w);
                                int VertexId = Assimp.aiMesh_GetBoneVertexId(scene, i, b, w);
                                mesh.addBone(VertexId, boneID, Weight);
                            }
                        }

                        for (int j = 0; j < listJoints.Count; j++)
                        {
                            AssimpJoint joint = listJoints[j];
                            mesh.addJoint(joint);
                        }

                        //**********


                        mesh.BuilSkin();
                        mesh.build();
                        mesh.dispose();
                    }

                   
                }

                Mesh m = CombineInstance(ObjectRoot,meshContainer, importingAssetsDir);
          


                //create key frames
                if (Assimp.aiScene_HasAnimation(scene))
                {
                    Animation anim = (UnityEngine.Animation)ObjectRoot.AddComponent(typeof(Animation));
                  
                    int numAnimation = Assimp.aiScene_GetNumAnimations(scene);

                    Debug.Log("count animation  :" + numAnimation);


                    for (int a = 0; a < numAnimation; a++)
                    {

                        AnimationClip clip = new AnimationClip();
                        string anima = Assimp.aiAnim_GetName(scene, a);
                        clip.name = nm + "_" + anima + "_" + a;


                        clip.wrapMode = WrapMode.Loop;

                        float tinks = (float)Assimp.aiAnim_GetTicksPerSecond(scene, a);
                        if (tinks <= 1f) tinks = 1f;
                        float fps = tinks;
                        clip.frameRate = tinks;


                        Debug.Log("animation fps :" + fps);



                        int numchannels = Assimp.aiAnim_GetNumChannels(scene, a);
                        for (int i = 0; i < numchannels; i++)
                        {
                            string name = Assimp.aiAnim_GetChannelName(scene, a, i);
                            AssimpJoint joint = getBoneByName(name);



                            //  Debug.Log(String.Format("anim channel {0} bone name {1}  poskeys {2}  rotkeys{2}", i, name, Assimp.aiAnim_GetNumPositionKeys(scene, 0, i), Assimp.aiAnim_GetNumRotationKeys(scene, 0, i)));

   //public static bool ignoreRotations = true;
   // public static bool ignorePositions = true;
   // public static bool ignoreScalling = true;
                   
                      if (!ignoreScalling)
                       {
                            if (Assimp.aiAnim_GetNumScalingKeys(scene, a, i) !=0)
                            {
                                AnimationCurve scaleXcurve = new AnimationCurve();
                                AnimationCurve scaleYcurve = new AnimationCurve();
                                AnimationCurve scaleZcurve = new AnimationCurve();

                                for (int j = 0; j < Assimp.aiAnim_GetNumScalingKeys(scene, a, i); j++)
                                {
                                    float time = (float)Assimp.aiAnim_GetScalingFrame(scene, a, i, j) ;/// fps;
                                    Vector3 scale = Assimp.aiAnim_GetScalingKey(scene, a, i, j);
                                //    time = (float)j;
                                    scaleXcurve.AddKey(time, scale.x);
                                    scaleYcurve.AddKey(time, scale.y);
                                    scaleZcurve.AddKey(time, scale.z);
                                }
                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "m_LocalScale.x", scaleXcurve);
                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "m_LocalScale.y", scaleYcurve);
                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "m_LocalScale.z", scaleZcurve);

                            }
                       }

                      if (!ignorePositions)
                      {
                          if (Assimp.aiAnim_GetNumPositionKeys(scene, a, i) != 0)
                          {
                              AnimationCurve posXcurve = new AnimationCurve();
                              AnimationCurve posYcurve = new AnimationCurve();
                              AnimationCurve posZcurve = new AnimationCurve();


                              for (int j = 0; j < Assimp.aiAnim_GetNumPositionKeys(scene, a, i); j++)
                              {
                                  float time = (float)Assimp.aiAnim_GetPositionFrame(scene, a, i, j);// / fps;
                                  Vector3 pos = Assimp.aiAnim_GetPositionKey(scene, a, i, j);
                                  //    time = (float)j;
                                  posXcurve.AddKey(time, pos.x);
                                  posYcurve.AddKey(time, pos.y);
                                  posZcurve.AddKey(time, pos.z);
                              }

                              clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localPosition.x", posXcurve);
                              clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localPosition.y", posYcurve);
                              clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localPosition.z", posZcurve);
                          }
                      }

                      if (!ignoreRotations)
                      {
                          if (Assimp.aiAnim_GetNumRotationKeys(scene, a, i) != 0)
                          {

                              AnimationCurve rotXcurve = new AnimationCurve();
                              AnimationCurve rotYcurve = new AnimationCurve();
                              AnimationCurve rotZcurve = new AnimationCurve();
                              AnimationCurve rotWcurve = new AnimationCurve();

                              for (int j = 0; j < Assimp.aiAnim_GetNumRotationKeys(scene, a, i); j++)
                              {
                                  float time = (float)Assimp.aiAnim_GetRotationFrame(scene, a, i, j);// / fps;

                                  Quaternion rotation = Assimp.aiAnim_GetRotationKey(scene, a, i, j);
                                  //    time = (float)j;
                                  rotXcurve.AddKey(time, rotation.x);
                                  rotYcurve.AddKey(time, rotation.y);
                                  rotZcurve.AddKey(time, rotation.z);
                                  rotWcurve.AddKey(time, rotation.w);

                              }

                              clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localRotation.x", rotXcurve);
                              clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localRotation.y", rotYcurve);
                              clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localRotation.z", rotZcurve);
                              clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localRotation.w", rotWcurve);

                          }
                      }



                        }



                            clip.EnsureQuaternionContinuity();
                            anim.AddClip(clip, clip.name);
                            anim.clip = clip;
                  
                        string clipAssetPath = AssetDatabase.GenerateUniqueAssetPath(importingAssetsDir + clip.name + ".asset");
                        AssetDatabase.CreateAsset(clip, clipAssetPath);
                       }

                    

                }

         
                if (saveAssets)
                {

                    string prefabPath = AssetDatabase.GenerateUniqueAssetPath(importingAssetsDir + filename + ".prefab");
                    var prefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
                    PrefabUtility.ReplacePrefab(ObjectRoot, prefab, ReplacePrefabOptions.ConnectToPrefab);
                    AssetDatabase.Refresh();
                }
         
                MeshList.Clear();
            }

            listJoints.Clear();

            Assimp.aiReleaseImport(scene);
            Debug.LogWarning(path + "/" + filename + " Imported ;) ");
        }

    }
    private static void ConfigureForAtlas(string TexturePath)
    {
        TextureImporter TexImport = AssetImporter.GetAtPath(TexturePath) as TextureImporter;
        TextureImporterSettings tiSettings = new TextureImporterSettings();
        TexImport.textureType = TextureImporterType.Advanced;
        TexImport.ReadTextureSettings(tiSettings);
        tiSettings.readable = true;
        TexImport.SetTextureSettings(tiSettings);
        AssetDatabase.ImportAsset(TexturePath, ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();
    }

    private static Mesh CombineInstance(GameObject root,GameObject obj,string path)
    {
        SkinnedMeshRenderer[] smRenderers = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        List<Transform> bones = new List<Transform>();
        List<BoneWeight> boneWeights = new List<BoneWeight>();
        List<CombineInstance> combineInstances = new List<CombineInstance>();
        List<Texture2D> textures = new List<Texture2D>();
        int numSubs = 0;

        foreach (SkinnedMeshRenderer smr in smRenderers)
            numSubs += smr.sharedMesh.subMeshCount;

       

        int[] meshIndex = new int[numSubs];
        int boneOffset = 0;
        for (int s = 0; s < smRenderers.Length; s++)
        {
            SkinnedMeshRenderer smr = smRenderers[s];

            BoneWeight[] meshBoneweight = smr.sharedMesh.boneWeights;

            // May want to modify this if the renderer shares bones as unnecessary bones will get added.
            foreach (BoneWeight bw in meshBoneweight)
            {
                BoneWeight bWeight = bw;

                bWeight.boneIndex0 += boneOffset;
                bWeight.boneIndex1 += boneOffset;
                bWeight.boneIndex2 += boneOffset;
                bWeight.boneIndex3 += boneOffset;

                boneWeights.Add(bWeight);
            }
            boneOffset += smr.bones.Length;

            Transform[] meshBones = smr.bones;
        
            foreach (Transform bone in meshBones)
                bones.Add(bone);

            if (smr.material.mainTexture != null)
                textures.Add(smr.GetComponent<Renderer>().material.mainTexture as Texture2D);

            CombineInstance ci = new CombineInstance();
            ci.mesh = smr.sharedMesh;
            meshIndex[s] = ci.mesh.vertexCount;
            ci.transform = smr.transform.localToWorldMatrix;
            combineInstances.Add(ci);


            UnityEngine.Object.DestroyImmediate(smr.gameObject);
        }

      //  var children = new List<GameObject>();
    //    foreach (Transform child in obj.transform) children.Add(child.gameObject);
     //   children.ForEach(child => UnityEngine.Object.DestroyImmediate(child));


        List<Matrix4x4> bindposes = new List<Matrix4x4>();

        for (int b = 0; b < bones.Count; b++)
        {
            bindposes.Add(bones[b].worldToLocalMatrix * obj.transform.worldToLocalMatrix);
        }

        SkinnedMeshRenderer r = obj.AddComponent<SkinnedMeshRenderer>();
        r.sharedMesh = new Mesh();
        r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, true);
        //r.rootBone = root.transform;

        Texture2D skinnedMeshAtlas = new Texture2D(128, 128);
        Rect[] packingResult = skinnedMeshAtlas.PackTextures(textures.ToArray(), 0);
        Vector2[] originalUVs = r.sharedMesh.uv;
        Vector2[] atlasUVs = new Vector2[originalUVs.Length];

        int rectIndex = 0;
        int vertTracker = 0;
        for (int i = 0; i < atlasUVs.Length; i++)
        {
            atlasUVs[i].x = Mathf.Lerp(packingResult[rectIndex].xMin, packingResult[rectIndex].xMax, originalUVs[i].x);
            atlasUVs[i].y = Mathf.Lerp(packingResult[rectIndex].yMin, packingResult[rectIndex].yMax, originalUVs[i].y);

            if (i >= meshIndex[rectIndex] + vertTracker)
            {
                vertTracker += meshIndex[rectIndex];
                rectIndex++;
            }
        }

        string assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "texture.asset");
        AssetDatabase.CreateAsset(skinnedMeshAtlas, assetPath);

        Material combinedMat = new Material(Shader.Find("Diffuse"));
        combinedMat.mainTexture = skinnedMeshAtlas;
        r.sharedMesh.uv = atlasUVs;
        r.sharedMaterial = combinedMat;

        assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "material.asset");
        AssetDatabase.CreateAsset(combinedMat, assetPath);
            

        r.bones = bones.ToArray();
        r.sharedMesh.boneWeights = boneWeights.ToArray();
        r.sharedMesh.bindposes = bindposes.ToArray();
        r.sharedMesh.RecalculateBounds();

        assetPath = AssetDatabase.GenerateUniqueAssetPath(path + "mesh.asset");
        AssetDatabase.CreateAsset(r.sharedMesh, assetPath);
        

        return r.sharedMesh;
    }
    private static AssimpJoint getBoneByName(string name)
    {
        for (int i = 0; i < listJoints.Count; i++)
        {

            AssimpJoint bone = listJoints[i];
            if (bone.Name == name)
            {
                return bone;

            }
        }
        return null;
    }
    private static int findBoneByName(string name)
    {
        for (int i = 0; i < listJoints.Count; i++)
        {

            AssimpJoint bone = listJoints[i];
            if (bone.Name == name)
            {
                return i;

            }
        }
        return -1;
    }

    private static void processNodes(IntPtr scene, IntPtr node, ref List<AssimpJoint> listJoints)
    {

        AssimpJoint joint = new AssimpJoint();

        string name = Assimp.aiNode_GetName(node);
       if (name == "") name = "REMOVE-ME";
        joint.Name = name;
        joint.parentName = "NONE";
        joint.Position = Assimp.aiNode_GetPosition(node);
        //assimp quaternion is w,x,y,z and unity x,y,z,w bu int this lib i fix this for unity
        Quaternion quad = Assimp.aiNode_GetRotation(node);

        joint.Orientation = quad;
        


        if (Assimp.aiNode_GetParent(node) != null)
        {
            string parentName = Assimp.aiNode_GetName(Assimp.aiNode_GetParent(node));
            joint.parentName = parentName;

        }


        listJoints.Add(joint);

        for (int i = 0; i < listJoints.Count; i++)
        {

            AssimpJoint parent = listJoints[i];
            if (joint.parentName == parent.Name)
            {
                joint.parent = parent;
                joint.Path += parent.Path + "/";
                break;
            }

        }

        joint.Path += name;

        if (joint.parent != null)
        {
         //   Debug.Log(string.Format(" Joint  name: {0}  ; parent:{1} ;  animation path:{2} ", joint.Name, joint.parent.Name,"Skeleton/" +  joint.Path));
        }
        else
        {
         //   Debug.Log(string.Format(" Joint  name: {0}  ; animation path:{1} ", joint.Name,"Skeleton/" + joint.Path));
        }

        for (int n = 0; n < Assimp.aiNode_GetNumChildren(node); n++)
        {
            processNodes(scene, Assimp.aiNode_GetChild(node, n), ref listJoints);

        }
    }


}
