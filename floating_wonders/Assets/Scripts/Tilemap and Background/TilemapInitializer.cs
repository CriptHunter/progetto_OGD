using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapInitializer : MonoBehaviour
{
    public GameObject edgeChecker;

    void Start()
    {
        Tilemap tilemap = GetComponent<Tilemap>();

        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        /*for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    var sph=Instantiate(debug);
                    sph.transform.position = new Vector2(x,y);
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                }
                else
                {
                    Debug.Log("x:" + x + " y:" + y + " tile: (null)");
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
                    /*var sph = Instantiate(debug);
                    sph.transform.position = place;
                    availablePlaces.Add(place);*/
                    var go = Instantiate(edgeChecker);
                    go.transform.position = place;
                    //edgeGenerators.Add(go.GetComponent<TilemapEdgeGenerator>());

                    go = Instantiate(edgeChecker);
                    go.transform.position = place;
                    go.transform.localScale = new Vector3(-1,1,1);
                    //edgeGenerators.Add(go.GetComponent<TilemapEdgeGenerator>());
                }
                else
                {
                    //No tile at "place"
                }
            }
        }

        /*foreach(TilemapEdgeGenerator e in edgeGenerators)
        {
            //e.GenerateEdge();
        }*/
    }
}
