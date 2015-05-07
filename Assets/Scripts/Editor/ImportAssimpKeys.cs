using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public static class ImportAssimpKeys
{
    public static bool saveAssets = true;
    public static bool useTangents = true;

    private static StreamWriter streamWriter;
    private static List<AssimpJoint> listJoints = new List<AssimpJoint>();
    [MenuItem("Assets/Djoker Tools/Assimp/ImportKeyFrames")]
    static void init()
    {

        string filename = Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));
    
        readMesh(rootPath, filename, "Textures/");

    }

   



    private static void trace(string msg)
    {
      //  streamWriter.WriteLine("LOG:" + msg);
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

   

    private static void readMesh(string path, string filename, string texturepath)
    {
        string importingAssetsDir;

        if (File.Exists(path + "/" + filename))
        {



            Assimp.PostProcessSteps flags = (

            Assimp.PostProcessSteps.RemoveComponent );

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
                        Transform bone = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                      //  Transform bone = new GameObject(joint.Name).transform;
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




                //create key frames
                if (Assimp.aiScene_HasAnimation(scene))
                {
                    Animation anim = (UnityEngine.Animation)ObjectRoot.AddComponent(typeof(Animation));

                    int numAnimation = Assimp.aiScene_GetNumAnimations(scene);

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




                            if (Assimp.aiAnim_GetNumScalingKeys(scene, a, i) != 0)
                            {
                                AnimationCurve scaleXcurve = new AnimationCurve();
                                AnimationCurve scaleYcurve = new AnimationCurve();
                                AnimationCurve scaleZcurve = new AnimationCurve();

                                for (int j = 0; j < Assimp.aiAnim_GetNumScalingKeys(scene, a, i); j++)
                                {
                                    float time = (float)Assimp.aiAnim_GetScalingFrame(scene, a, i, j);// *fps;
                                    Vector3 scale = Assimp.aiAnim_GetScalingKey(scene, a, i, j);
                                       //time = (float)j;
                                    scaleXcurve.AddKey(time, scale.x);
                                    scaleYcurve.AddKey(time, scale.y);
                                    scaleZcurve.AddKey(time, scale.z);
                                }
                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "m_LocalScale.x", scaleXcurve);
                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "m_LocalScale.y", scaleYcurve);
                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "m_LocalScale.z", scaleZcurve);

                            }


                            if (Assimp.aiAnim_GetNumPositionKeys(scene, a, i) != 0)
                            {
                                AnimationCurve posXcurve = new AnimationCurve();
                                AnimationCurve posYcurve = new AnimationCurve();
                                AnimationCurve posZcurve = new AnimationCurve();


                                for (int j = 0; j < Assimp.aiAnim_GetNumPositionKeys(scene, a, i); j++)
                                {
                                    float time = (float)Assimp.aiAnim_GetPositionFrame(scene, a, i, j);// *fps;
                                    Vector3 pos = Assimp.aiAnim_GetPositionKey(scene, a, i, j);
                                      //  time = (float)j;
                                    posXcurve.AddKey(time, pos.x);
                                    posYcurve.AddKey(time, pos.y);
                                    posZcurve.AddKey(time, pos.z);
                                }

                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localPosition.x", posXcurve);
                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localPosition.y", posYcurve);
                                clip.SetCurve("Skeleton/" + joint.Path, typeof(Transform), "localPosition.z", posZcurve);
                            }
                            if (Assimp.aiAnim_GetNumRotationKeys(scene, a, i) != 0)
                            {

                                AnimationCurve rotXcurve = new AnimationCurve();
                                AnimationCurve rotYcurve = new AnimationCurve();
                                AnimationCurve rotZcurve = new AnimationCurve();
                                AnimationCurve rotWcurve = new AnimationCurve();

                                for (int j = 0; j < Assimp.aiAnim_GetNumRotationKeys(scene, a, i); j++)
                                {
                                    float time = (float)Assimp.aiAnim_GetRotationFrame(scene, a, i, j);// *fps;

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



                        clip.EnsureQuaternionContinuity();
                        anim.AddClip(clip, clip.name);
                        anim.clip = clip;
                      

                        string clipAssetPath = AssetDatabase.GenerateUniqueAssetPath(importingAssetsDir + clip.name + ".asset");
                        AssetDatabase.CreateAsset(clip, clipAssetPath);

                        //  AssetDatabase.CreateAsset(clip, "Assets/Models/" + nm +"_"+a+ ".anim");
                        //  AssetDatabase.SaveAssets();
                    }

                }


                if (saveAssets)
                {

                    string prefabPath = AssetDatabase.GenerateUniqueAssetPath(importingAssetsDir + filename + ".prefab");
                    var prefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
                    PrefabUtility.ReplacePrefab(ObjectRoot, prefab, ReplacePrefabOptions.ConnectToPrefab);
                    AssetDatabase.Refresh();
                }

              
            }

            listJoints.Clear();

            Assimp.aiReleaseImport(scene);
            Debug.LogWarning(path + "/" + filename + " Imported ;) ");
        }

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
        joint.parentName = "";
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
          //  trace(string.Format(" Joint  name: {0}  ; parent:{1} ;  path:{2} ", joint.Name, joint.parent.Name, joint.Path));
        }
        else
        {
          //  trace(string.Format(" Joint  name: {0}  ;  path:{1} ", joint.Name, joint.Path));
        }

        for (int n = 0; n < Assimp.aiNode_GetNumChildren(node); n++)
        {
            processNodes(scene, Assimp.aiNode_GetChild(node, n), ref listJoints);

        }
    }


}
