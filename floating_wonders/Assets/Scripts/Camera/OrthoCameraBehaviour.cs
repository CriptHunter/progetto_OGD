using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrthoCameraBehaviour : MonoBehaviour
{
    private new Camera camera;
    private float initialOrthoCameraSize;
    private float initialZ;
    private bool isOrtho;

    [SerializeField]
    private float shakeReductionSpeed = 1;//35f;
    [SerializeField]
    private float shakeAmplification = 0.08f;

    private float desiredX;
    private float desiredY;
    private float desiredZoom;

    private float x;
    private float y;
    private float zoom;
    private float shake;

    private float precision = 0.001f;

    private float boh = 0.0f;


    public Vector2 GetFocus()
    {
        return new Vector2(desiredX, desiredY);
    }

    public float GetFocusX()
    {
        return desiredX;
    }
    public float GetFocusY()
    {
        return desiredY;
    }

    public float GetZoom()
    {
        return desiredZoom;
    }

    public Vector2 GetFocusCurrent()
    {
        return new Vector2(x, y);
    }

    public float GetFocusXCurrent()
    {
        return x;
    }
    public float GetFocusYCurrent()
    {
        return y;
    }

    public float GetZoomCurrent()
    {
        return zoom;
    }


    public void SetFocus(Vector2 focus, bool instantaneous = false)
    {
        SetFocusX(focus.x, instantaneous);
        SetFocusY(focus.y, instantaneous);
    }

    public void SetFocusX(float focusX, bool instantaneous = false)
    {
        desiredX = focusX;
        if (instantaneous)
        {
            x = focusX;
        }
    }

    public void SetFocusY(float focusY, bool instantaneous = false)
    {
        desiredY = focusY;
        if (instantaneous)
        {
            y = focusY;
        }
    }

    public void SetZoom(float zoom, bool instantaneous = false)
    {
        desiredZoom = zoom;
        if (instantaneous)
        {
            this.zoom = zoom;
        }
    }

    public void ApplyShake(float amount, bool absoluteAmount = false)
    {
        if (absoluteAmount)
        {
            shake = amount;
        }
        else
        {
            shake = Mathf.Max(shake, amount);
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        camera = GetComponent<Camera>();
        initialOrthoCameraSize = camera.orthographicSize;
        initialZ = transform.position.z;
        isOrtho = camera.orthographic;

        x = 0;
        desiredX = 0;
        y = 0;
        desiredY = 0;
        zoom = 1;
        desiredZoom = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //float dt = Time.deltaTime;
        //var height = 2 * Camera.main.orthographicSize;
        //var width = height * Camera.main.aspect;
        //print("smoothdamping x " + x);
        //x = Mathf.SmoothDamp(x, desiredX, ref boh, 1f);
        
    }

    private void FixedUpdate()
    {
        ExpDamp(ref x, desiredX, 10f, precision);
        ExpDamp(ref y, desiredY, 10f, precision);
        ExpDamp(ref zoom, desiredZoom, 50f, precision);

        if (shake > 0)
        {
            shake -= shakeReductionSpeed;
            if (shake < 0)
                shake = 0;
        }

        if (isOrtho)
        {
            transform.position = new Vector3(x + Util.RandomRange(-shake * shakeAmplification, shake * shakeAmplification) * zoom, y + Util.RandomRange(-shake * shakeAmplification, shake * shakeAmplification) * zoom, initialZ);
            camera.orthographicSize = initialOrthoCameraSize * zoom;
        }
        else
        {
            transform.position = new Vector3(x + Util.RandomRange(-shake * shakeAmplification, shake * shakeAmplification) * zoom, y + Util.RandomRange(-shake * shakeAmplification, shake * shakeAmplification) * zoom, initialZ * zoom);
        }
    }

    void ExpDampDeltaTime(ref float value, float targetValue, float dampSpeed, float epsilon)
    {
        if (Mathf.Abs(value - targetValue) > epsilon)
        {
            value -= ((value - targetValue) / (dampSpeed))* Time.deltaTime;
        }
        else
        {
            value = targetValue;
        }
    }

    void ExpDamp(ref float value, float targetValue, float dampSpeed, float epsilon)
    {
        if (Mathf.Abs(value - targetValue) > epsilon)
        {
            value -= ((value - targetValue) / (dampSpeed));
        }
        else
        {
            value = targetValue;
        }
    }
}
