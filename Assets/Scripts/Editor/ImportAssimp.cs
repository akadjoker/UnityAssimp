using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public static class ImportAssimp
{
    public static bool saveAssets = true;
    
    [MenuItem("Assets/Djoker Tools/Assimp/ImportStatic")]
    static void init()
    {

        string filename = Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject));
        string rootPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));

        readMesh(rootPath, filename, "");

    }
    

    private static void trace(string msg)
    {

    }




    private class AssimpMesh
    {
        public Mesh geometry;
        public string name;
        public Material material;
        public List<Vector3> vertices;
        public List<Vector3> normals;
        public List<Vector4> tangents;
        public List<Vector2> uvcoords;
        public List<int> faces;
        public GameObject meshContainer;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public GameObject Root;



        public AssimpMesh(GameObject parent, GameObject root, string name)
        {
            this.name = name;
            meshContainer = new GameObject(name);
          
            this.Root = root;
            meshContainer.transform.parent = parent.transform;

            meshContainer.AddComponent<MeshFilter>();
            meshContainer.AddComponent<MeshRenderer>();
            meshFilter = meshContainer.GetComponent<MeshFilter>();
            meshRenderer = meshContainer.GetComponent<MeshRenderer>();

            meshFilter.sharedMesh = new Mesh();
            geometry = meshFilter.sharedMesh;
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            tangents = new List<Vector4>();
            uvcoords = new List<Vector2>();
            faces = new List<int>();


        }
        public void addVertex(Vector3 pos, Vector3 normal, Vector2 uv ,Vector4 tan)
        {
            vertices.Add(pos);
            normals.Add(normal);
            uvcoords.Add(uv);
            tangents.Add(tan);

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


        public void build()
        {
            
            geometry.vertices = vertices.ToArray();
            geometry.normals = normals.ToArray();
            geometry.uv = uvcoords.ToArray();
            //geometry.u = uvcoords.ToArray();
            geometry.triangles = faces.ToArray();
            geometry.tangents = tangents.ToArray();
            Unwrapping.GenerateSecondaryUVSet(geometry);
    //        geometry.RecalculateNormals();
            geometry.RecalculateBounds();
          //  TangentSolver(geometry);

            geometry.Optimize();
        }
        public void dispose()
        {
            vertices.Clear();
            normals.Clear();
            faces.Clear();
            uvcoords.Clear();
            tangents.Clear();
        }

         public void TangentSolver(Mesh mesh)
        {
            Vector3[] tan2 = new Vector3[mesh.vertices.Length];
            Vector3[] tan1 = new Vector3[mesh.vertices.Length];
            Vector4[] tangents = new Vector4[mesh.vertices.Length];
            //Vector3[] binormal = new Vector3[mesh.vertices.Length];
            for (int a = 0; a < (mesh.triangles.Length); a += 3)
            {
                long i1 = mesh.triangles[a + 0];
                long i2 = mesh.triangles[a + 1];
                long i3 = mesh.triangles[a + 2];

                Vector3 v1 = mesh.vertices[i1];
                Vector3 v2 = mesh.vertices[i2];
                Vector3 v3 = mesh.vertices[i3];

                Vector2 w1 = mesh.uv[i1];
                Vector2 w2 = mesh.uv[i2];
                Vector2 w3 = mesh.uv[i3];

                float x1 = v2.x - v1.x;
                float x2 = v3.x - v1.x;
                float y1 = v2.y - v1.y;
                float y2 = v3.y - v1.y;
                float z1 = v2.z - v1.z;
                float z2 = v3.z - v1.z;

                float s1 = w2.x - w1.x;
                float s2 = w3.x - w1.x;
                float t1 = w2.y - w1.y;
                float t2 = w3.y - w1.y;

                float r = 1.0F / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;

                tan2[i1] += tdir;
                tan2[i2] += tdir;
                tan2[i3] += tdir;
            }

            for (int a = 0; a < mesh.vertices.Length; a++)
            {
                Vector3 n = mesh.normals[a];
                Vector3 t = tan1[a];

                Vector3.OrthoNormalize(ref n, ref t);
                tangents[a].x = t.x;
                tangents[a].y = t.y;
                tangents[a].z = t.z;

                // Calculate handedness
                tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;

                //To calculate binormals if required as vector3 try one of below:-
                //Vector3 binormal[a] = (Vector3.Cross(n, t) * tangents[a].w).normalized;
                //Vector3 binormal[a] = Vector3.Normalize(Vector3.Cross(n, t) * tangents[a].w)
            }
            mesh.tangents = tangents;
        }

    }

    public static void readMesh(string path, string filename, string texturepath)
    {
        string importingAssetsDir;

        if (File.Exists(path + "/" + filename))
        {




            Assimp.PostProcessSteps flags = (
        //        Assimp.PostProcessSteps.MakeLeftHanded |
            
                Assimp.PostProcessSteps.OptimizeMeshes |
                 Assimp.PostProcessSteps.OptimizeGraph |
                Assimp.PostProcessSteps.RemoveRedundantMaterials |
                Assimp.PostProcessSteps.SortByPrimitiveType |
                Assimp.PostProcessSteps.SplitLargeMeshes |
                Assimp.PostProcessSteps.Triangulate |
                Assimp.PostProcessSteps.CalculateTangentSpace |
                Assimp.PostProcessSteps.GenerateUVCoords |
                Assimp.PostProcessSteps.GenerateSmoothNormals |
                 Assimp.PostProcessSteps.RemoveComponent |
                Assimp.PostProcessSteps.JoinIdenticalVertices );

            IntPtr config = Assimp.aiCreatePropertyStore();

            Assimp.aiSetImportPropertyFloat(config, Assimp.AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE, 60.0f);
            
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

                    string texturename = path +"/"+ fname;

                    Texture2D tex = null;
                    if (File.Exists(texturename ))
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
                        } else
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
                    
                    
                    
                    //Texture2D tex = Resources.Load(texturename) as Texture2D;

                  //  Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texturename, typeof(Texture2D));


                    if (tex != null)
                    {
                        Debug.Log("LOAD (" + texturename + ") texture");
                        mat.SetTexture("_MainTex", tex);

                    }
                    else
                    {
                        Debug.LogError("Fail LOAD (" + texturename + ") error");
                    }


                    if (saveAssets)
                    {
                        string materialAssetPath = AssetDatabase.GenerateUniqueAssetPath(importingAssetsDir + mat.name + ".asset");
                        AssetDatabase.CreateAsset(mat, materialAssetPath);
                    }
                    materials.Add(mat);

                }

                AssetDatabase.Refresh();




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

                            Vector4 outputTangent =new Vector4(tangentf.x, tangentf.y, tangentf.z, 0.0F);

                            float dp = Vector3.Dot(Vector3.Cross(n, tangentf), binormalf);
                            if (dp > 0.0F)
                                outputTangent.w = 1.0F;
                            else
                                outputTangent.w = -1.0F;






                            mesh.addVertex(vertex, n, new Vector2(x, y), outputTangent);


                            //mesh.addVertex(vertex, new Vector3(1 * -n.x, n.y, n.z), new Vector2(x, y), outputTangent);

                           

                        }

                        for (int f = 0; f < Assimp.aiMesh_GetNumFaces(scene, i); f++)
                        {
                            int a = Assimp.aiMesh_Indice(scene, i, f, 0);
                            int b = Assimp.aiMesh_Indice(scene, i, f, 1);
                            int c = Assimp.aiMesh_Indice(scene, i, f, 2);
                            mesh.addFace(a, b, c);
                        }






                        //**********



                        mesh.build();


                        if (saveAssets)
                        {

                            string meshAssetPath = AssetDatabase.GenerateUniqueAssetPath(importingAssetsDir + mesh.name + ".asset");
                            AssetDatabase.CreateAsset(mesh.geometry, meshAssetPath);
                        }

                        mesh.dispose();
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



            Assimp.aiReleaseImport(scene);
            Debug.LogWarning(path + "/" + filename + " Imported ;) ");
        }

    }


}
