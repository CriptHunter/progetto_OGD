using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MinimapGenerator : MonoBehaviour
{
    public Color backColor;
    public Color frontColor;
    public Color playerColor;
    public float playerScale;

    public Grid worldgrid;
    public Tilemap[] inputTilemaps;

    private Texture2D minimap;
    private Image image;

    private bool minimapActive;
    public bool MinimapActive
    {
        get
        {
            return minimapActive;
        }
        set
        {
            minimapActive = value;
            if (!minimapActive)
            {
                minimapActive = false;
                image.enabled = false;
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            else
            {
                minimapActive = true;
                image.enabled = true;

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }
    void Awake()
    {
        minimapActive = true;

        for(int i = 0; i < transform.childCount; i += 1)
        {
            Image img = transform.GetChild(i).GetComponent<Image>();
            img.color = playerColor;
            img.transform.localScale = Vector3.one * playerScale;
        }

        image = GetComponent<Image>();

        Bounds bounds = new Bounds();
        foreach (Tilemap tm in inputTilemaps)
        {
            tm.CompressBounds();
            bounds.Encapsulate(tm.localBounds);
        }

        Vector2 cellSize = worldgrid.cellSize;
        Vector2 worldFrom = new Vector3(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y, 0);
        Vector2 worldTo = new Vector3(bounds.center.x + bounds.extents.x, bounds.center.y + bounds.extents.y, 0);
        Vector3Int cellFrom = worldgrid.WorldToCell(worldFrom);
        Vector3Int cellTo = worldgrid.WorldToCell(worldTo);

        foreach (PlayerMinimapIndicator pmi in GetComponentsInChildren<PlayerMinimapIndicator>())
        {
            pmi.SetBounds(worldFrom.x, worldFrom.y, worldTo.x, worldTo.y);
        }


        minimap = new Texture2D(cellTo.x-cellFrom.x, cellTo.y-cellFrom.y);
        minimap.filterMode = FilterMode.Point;
        minimap.wrapMode = TextureWrapMode.Clamp;
        print("texture is "+minimap.width + " x " + minimap.height);

        Color[] minimapCol = minimap.GetPixels();
        for(int i = 0; i < minimapCol.Length; i++)
        {
            minimapCol[i] = backColor;
        }

        int yy = 0;
        for (float wy = worldFrom.y; wy < worldTo.y; wy += cellSize.y)
        {
            int xx = 0;
            for (float wx = worldFrom.x; wx < worldTo.x; wx += cellSize.x)
            {
                int index = (yy * minimap.width) + xx;

                bool hasTile = false;
                foreach (Tilemap tm in inputTilemaps)
                {
                    if (tm.HasTile(tm.WorldToCell(new Vector3(wx, wy, 0)))){
                        hasTile = true;
                    }
                }

                if (hasTile)
                {
                    minimapCol[index] = frontColor;
                }
                xx++;
            }
            yy++;
        }

        minimap.SetPixels(minimapCol);
        minimap.Apply();

        //byte[] bytes = minimap.EncodeToPNG();
        //File.WriteAllBytes("TESSITURA.png", bytes);

        image.sprite = Sprite.Create(minimap, new Rect(0, 0, minimap.width, minimap.height), new Vector2(1,0),64);
        image.SetNativeSize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MinimapActive = !MinimapActive;
        }
    }
}
