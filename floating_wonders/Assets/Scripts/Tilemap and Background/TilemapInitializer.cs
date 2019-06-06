using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInitializer : MonoBehaviour
{
    public GameObject initializer;
    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        /*TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    if (tile == grassTile)
                    {
                        var sph = Instantiate(grass);
                        sph.transform.position = new Vector2(x, y);
                    }
                }
            }
        }*/

        //var edgeGenerators = new List<TilemapEdgeGenerator>();
        
        for (int n = bounds.xMin; n < bounds.xMax; n++)
        {
            for (int p = bounds.yMin; p < bounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.CellToWorld(localPlace) + new Vector3(0.5f, 1.5f, 0f);
                if (tilemap.HasTile(localPlace))
                {
                    //Tile at "place"
                    var go = Instantiate(initializer);
                    go.transform.position = place;

                    go = Instantiate(initializer);
                    go.transform.position = place;
                    go.transform.localScale = new Vector3(-1,1,1);
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }
}
