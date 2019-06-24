using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GrassSpawner : MonoBehaviour
{
    public GameObject grass;
    public Sprite grassSpriteTile;
    public Sprite grassSpriteTileLeft;
    public Sprite grassSpriteTileRight;

    public TileBase grassMiddle;
    public TileBase grassLeft;
    public TileBase grassRight;

    // Start is called before the first frame update
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
                    if (tile == grassTile)
                    {
                        var sph = Instantiate(grass);
                        sph.transform.position = new Vector2(x, y);
                    }
                }
            }
        }*/
        for (int n = bounds.xMin; n < bounds.xMax; n++)
        {
            for (int p = bounds.yMin; p < bounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.z));
                Vector3 place = tilemap.CellToWorld(localPlace) + new Vector3(0.5f, 1f, 0f);
                if (tilemap.HasTile(localPlace))
                {
                    TileBase tile = tilemap.GetTile(localPlace);
                    Sprite s = tilemap.GetSprite(localPlace);
                    if (s == grassSpriteTile)
                    {
                        int repetitions = Util.Choose(1, 2, 2, 3, 3, 3, 3, 3, 4, 4);
                        for (int i = 0; i < repetitions; i++)
                        {
                            var sph = Instantiate(grass);
                            sph.transform.position = place + Vector3.right * Util.RandomRange(-0.5f, 0.5f);
                            /*if (Util.Choose(true, true, true, true, true, true, false))
                            {
                                sph.transform.GetChild(0).localScale = Util.RandomRange(Vector3.one * 0.3f, Vector3.one * 0.8f);
                            }
                            else
                            {
                                sph.transform.GetChild(0).localScale = Util.RandomRange(Vector3.one * 0.85f, Vector3.one * 2f);
                            }

                            if (Util.Choose(true, false))
                            {
                                sph.transform.GetChild(0).localScale = new Vector3(sph.transform.localScale.x, sph.transform.localScale.y, -sph.transform.localScale.z);
                            }
                            Grass grs = sph.GetComponent<Grass>();
                            if (grs != null)
                            {
                                grs.SetOscillationOffset(Util.Random(180));
                            }*/
                        }

                        tilemap.SetTile(localPlace + Vector3Int.up, grassMiddle);
                    }

                    if (s == grassSpriteTileLeft)
                    {
                        tilemap.SetTile(localPlace + Vector3Int.up, grassLeft);
                    }

                    if (s == grassSpriteTileRight)
                    {
                        tilemap.SetTile(localPlace + Vector3Int.up, grassRight);
                    }

                    //Debug.Log(tile.name);
                    //Tile at "place"
                    //var go = Instantiate(edgeChecker);
                    //go.transform.position = place;
                    //edgeGenerators.Add(go.GetComponent<TilemapEdgeGenerator>());

                    //go = Instantiate(edgeChecker);
                    //go.transform.position = place;
                    //go.transform.localScale = new Vector3(-1, 1, 1);
                    //edgeGenerators.Add(go.GetComponent<TilemapEdgeGenerator>());
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }
}
