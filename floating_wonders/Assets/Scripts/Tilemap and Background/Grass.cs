using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public Sprite[] variants;
    public int reactToLayer;

    private float oscillationAngle = 15f;
    private float oscillationSpeed = 90;
    private float oscillationOffset = 0;

    private float schiacciamentoProgress = 0;
    private float schiacciamentoSpeed = 0.25f;
    private float schiacciamentoAng = 0;
    private float schiacciamentoAngEffective = 0;
    private float schiacciamentoRotspeed = 810f;

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

        renderer.sprite = Util.ChooseFrom(variants);
        characters = new HashSet<GameObject>();
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

        if (nearest!=null)
        {
            schiacciamentoProgress += Time.deltaTime / schiacciamentoSpeed;
            if (nearest.transform.position.x > transform.position.x)
            {
                schiacciamentoAng = Mathf.Lerp(70, 0, nearest.transform.position.x - transform.position.x);
            }
            else
            {
                schiacciamentoAng = Mathf.Lerp(-70, 0, transform.position.x - nearest.transform.position.x);
            }
        }
        else
        {
            schiacciamentoProgress -= Time.deltaTime / schiacciamentoSpeed;
        }
        schiacciamentoProgress=Mathf.Clamp(schiacciamentoProgress, 0f, 1f);

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
