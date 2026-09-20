using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class PlatformArea : MonoBehaviour
{
    public PlatformBlock platformPrefab;
    public int count = 9;

    [Header("Grid")]
    public int rows = 3;
    public int columns = 3;
    public float spacing = 1f;

    public Color areaColor = Color.black;

    private List<PlatformBlock> platformBlocks = new();

    private void Start()
    {
        CreatePlatforms();

        if (Application.isPlaying)
        {
            foreach (PlatformBlock block in platformBlocks)
                block.SetBlockColor(areaColor);

            HideRandomBlocks();
        }
    }

    //[InspectorButton("Create Platforms")]
    void CreatePlatforms()
    {
        if (platformPrefab == null)
            return;

        ClearPlatforms();

        int total = Mathf.Min(count, rows * columns);

        for (int i = 0; i < total; i++)
        {
            int x = i % columns;
            int z = i / columns;

            PlatformBlock block;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                block = PrefabUtility.InstantiatePrefab(platformPrefab, transform) as PlatformBlock;
            else
#endif
                block = Instantiate(platformPrefab, transform);

            block.transform.localPosition = new Vector3(
                x * spacing,
                0,
                z * spacing
            );

            platformBlocks.Add(block);
        }
    }

    void ClearPlatforms()
    {
        platformBlocks.Clear();

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject obj = transform.GetChild(i).gameObject;

            if (Application.isPlaying)
                Destroy(obj);
            else
                DestroyImmediate(obj);
        }
    }

    //[InspectorButton("Hide Random Blocks")]
    void HideRandomBlocks()
    {
        ShowAllBlocks();

        if (platformBlocks.Count == 0)
            return;

        int amount = Random.Range(1, Mathf.Max(2, platformBlocks.Count / 2));

        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, platformBlocks.Count);
            platformBlocks[index].gameObject.SetActive(false);
        }
    }

    //[InspectorButton("Show All Blocks")]
    void ShowAllBlocks()
    {
        foreach (PlatformBlock block in platformBlocks)
            block.gameObject.SetActive(true);
    }
}