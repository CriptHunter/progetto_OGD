using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IvySpawner : MonoBehaviour
{
    public GameObject ivy;

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
                Vector3 place = tilemap.CellToWorld(localPlace) + new Vector3(0.5f, 0.5f, 0f);
                if (tilemap.HasTile(localPlace))
                {
                    TileBase tile = tilemap.GetTile(localPlace);
                    Sprite s = tilemap.GetSprite(localPlace);
                    int repetitions = Util.Choose(0,1,1,1, 2);//Util.Choose(1, 2, 2, 3, 3, 3, 3, 3, 4, 4);
                    for (int i = 0; i < repetitions; i++)
                    {
                        var sph = Instantiate(ivy);
                        sph.transform.position = place + new Vector3(Util.RandomRange(-0.5f, 0.5f),Util.RandomRange(-0.5f, 0.5f),Util.RandomRange(0,-0.75f));
                        /*if (Util.Choose(true, true, true, true, true, true, false))
                        {
                            var scale = Util.RandomRange(0.4f, 1.2f);
                            sph.transform.GetChild(0).localScale = new Vector3(Util.RandomRange(scale * 0.7f, scale * 1.3f), Util.RandomRange(scale * 0.7f, scale * 1.3f), 1);
                        }
                        else
                        {
                            var scale = Util.RandomRange(1.2f, 2f);
                            sph.transform.GetChild(0).localScale = new Vector3(Util.RandomRange(scale * 0.7f, scale * 1.3f), Util.RandomRange(scale * 0.7f, scale * 1.3f), 1);
                        }

                        if (Util.Choose(true, false))
                        {
                            sph.transform.GetChild(0).localScale = new Vector3(sph.transform.localScale.x, sph.transform.localScale.y, -sph.transform.localScale.z);
                        }
                        BoxCollider2D collider = sph.GetComponent<BoxCollider2D>();
                        if (collider != null)
                        {

                            var stacco = (collider.offset.y + collider.size.y / 2);
                            var finaloffset = stacco - collider.size.y / 2 * sph.transform.GetChild(0).localScale.y;
                            collider.size = new Vector2(collider.size.x, collider.size.y * sph.transform.GetChild(0).localScale.y);
                            collider.offset = new Vector2(collider.offset.x, finaloffset);
                        }
                        Ivy grs = sph.GetComponent<Ivy>();
                        if (grs != null)
                        {
                            grs.SetOscillationOffset(Util.Random(180));
                            var renderer= grs.transform.GetChild(0).GetComponent<SpriteRenderer>();
                            if (renderer != null)
                            {
                                renderer.sortingOrder += Mathf.RoundToInt(-sph.transform.position.z);
                            }
                        }*/
                    }
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }
}
