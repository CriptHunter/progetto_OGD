using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MinimapGenerator : MonoBehaviour
{
    public Grid worldgrid;
    public Tilemap[] inputTilemaps;

    private Texture2D minimap;
    private Image image;
    void Awake()
    {
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


        minimap = new Texture2D(cellTo.x-cellFrom.x, cellTo.y-cellFrom.y);
        minimap.filterMode = FilterMode.Point;
        minimap.wrapMode = TextureWrapMode.Clamp;
        print("texture is "+minimap.width + " x " + minimap.height);

        Color[] minimapCol = minimap.GetPixels();
        for(int i = 0; i < minimapCol.Length; i++)
        {
            minimapCol[i] = Color.white;
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
                    minimapCol[index] = Color.black;
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
}
