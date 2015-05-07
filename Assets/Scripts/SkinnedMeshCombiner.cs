using UnityEngine;
using System.Collections.Generic;

public class SkinnedMeshCombiner : MonoBehaviour 
{

    void Start()
    {
        
        SkinnedMeshRenderer[] smRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
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

            Object.Destroy(smr.gameObject);
        }

        List<Matrix4x4> bindposes = new List<Matrix4x4>();

        for (int b = 0; b < bones.Count; b++)
        {
            bindposes.Add(bones[b].worldToLocalMatrix * transform.worldToLocalMatrix);
        }

        SkinnedMeshRenderer r = gameObject.AddComponent<SkinnedMeshRenderer>();
        r.sharedMesh = new Mesh();
        r.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, true);

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

        Material combinedMat = new Material(Shader.Find("Diffuse"));
        combinedMat.mainTexture = skinnedMeshAtlas;
        r.sharedMesh.uv = atlasUVs;
        r.sharedMaterial = combinedMat;

        r.bones = bones.ToArray();
        r.sharedMesh.boneWeights = boneWeights.ToArray();
        r.sharedMesh.bindposes = bindposes.ToArray();
        r.sharedMesh.RecalculateBounds();
    }
}