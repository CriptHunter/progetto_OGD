using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ivy : MonoBehaviour
{
    public int reactToLayer;

    private float oscillationAngle = 15f;
    private float oscillationSpeed = 90;
    private float oscillationOffset = 0;

    private float schiacciamentoProgress = 0;
    private float schiacciamentoSpeed = 0.25f;
    private float schiacciamentoAng = 0;
    private float schiacciamentoAngEffective = 0;
    private float schiacciamentoRotspeed = 202.5f;

    private GameObject spriteObject;
    //private BoxCollider2D collider;
    private new SpriteRenderer renderer;

    private HashSet<GameObject> characters;
    // Start is called before the first frame update
    void Start()
    {
        //collider = GetComponent<BoxCollider2D>();
        spriteObject = gameObject.GetChild("Sprite");
        renderer = spriteObject.GetComponent<SpriteRenderer>();

        //renderer.sprite = Util.ChooseFrom(variants);
        characters = new HashSet<GameObject>();

        //randomize
        var sph = gameObject;
        if (Util.Choose(true, true, true, true, true, true, false))
        {
            var scale = Util.RandomRange(0.4f, 1.2f);
            sph.transform.GetChild(0).localScale = new Vector3(Util.RandomRange(scale * 0.7f, scale * 1.3f) * Util.Choose(-1,1), Util.RandomRange(scale * 0.7f, scale * 1.3f), 1);
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
            var renderer = grs.transform.GetChild(0).GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder += Mathf.RoundToInt(-sph.transform.position.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject nearest = null;
        float neardist = 1000000;
        float dist;

        float ang;

        if (characters != null)
        {
            if (characters.Count > 0)
            {
                foreach (GameObject g in characters)
                {
                    if (g != null)
                    {
                        dist = Vector2.SqrMagnitude(g.transform.position - transform.position);
                        if (dist < neardist)
                        {
                            nearest = g;
                            neardist = dist;
                        }
                    }
                }
            }
        }

        if (nearest != null)
        {
            if (nearest.transform.position.y <= transform.position.y)
            {
                schiacciamentoProgress += Time.deltaTime / schiacciamentoSpeed;
                if (nearest.transform.position.x > transform.position.x)
                {
                    schiacciamentoAng = Mathf.Lerp(-30, 0, nearest.transform.position.x - transform.position.x);
                }
                else
                {
                    schiacciamentoAng = Mathf.Lerp(30, 0, transform.position.x - nearest.transform.position.x);
                }
            }
            else
            {
                schiacciamentoProgress -= Time.deltaTime / schiacciamentoSpeed;
            }
        }
        else
        {
            schiacciamentoProgress -= Time.deltaTime / schiacciamentoSpeed;
        }
        schiacciamentoProgress = Mathf.Clamp(schiacciamentoProgress, 0f, 1f);

        if (schiacciamentoAngEffective < schiacciamentoAng)
        {
            schiacciamentoAngEffective += Time.deltaTime * schiacciamentoRotspeed;
            if (schiacciamentoAngEffective >= schiacciamentoAng)
            {
                schiacciamentoAngEffective = schiacciamentoAng;
            }
        }
        if (schiacciamentoAngEffective > schiacciamentoAng)
        {
            schiacciamentoAngEffective -= Time.deltaTime * schiacciamentoRotspeed;
            if (schiacciamentoAngEffective <= schiacciamentoAng)
            {
                schiacciamentoAngEffective = schiacciamentoAng;
            }
        }

        ang = Util.LengthDirX(oscillationAngle, Time.timeSinceLevelLoad * oscillationSpeed + oscillationOffset);
        spriteObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(ang, schiacciamentoAngEffective, schiacciamentoProgress));
    }

    public void SetOscillationOffset(float offset)
    {
        oscillationOffset = offset;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == reactToLayer)
        {
            characters.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == reactToLayer)
        {
            characters.Remove(collision.gameObject);
        }
    }
}
