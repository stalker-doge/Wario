using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Rigidbody2D rb;
    public float dragLimit = 3f;
    public float forceToAdd = 10f;

    private Camera cam;
    private bool isDragging ;

    private bool isGrounded ;
    public bool shoot;
    public Transform spawnPoint;

    public static DragController Instance { get; private set; }

    Vector3 MousPosition
    {
        get
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 10f; 
            pos = cam.ScreenToWorldPoint(pos);
            pos.z = -1f;
            return pos;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector2.zero);
        lineRenderer.SetPosition(1, Vector2.zero); 
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging)
        {
            DragStart();
        }

        if (isDragging)
        {
            Drag();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            DragEnd();
        }
        
        
    }

    void DragStart()
    {
        lineRenderer.enabled = true;
        isDragging = true;
        lineRenderer.SetPosition(0,MousPosition);
    }

    void Drag()
    {
        Vector3 startpos = lineRenderer.GetPosition(0);
        Vector3 curentpos = MousPosition;
        
        Vector3 distance = curentpos - startpos;

        if (distance.magnitude <= dragLimit)
        {
            lineRenderer.SetPosition(1, curentpos);
        }
        else
        {
            Vector3 limitVector = startpos + (distance.normalized * dragLimit);
            lineRenderer.SetPosition(1, limitVector);
        }

    }

    void DragEnd()
    {
        isDragging = false;
        lineRenderer.enabled = false;
        
        Vector3 startpos = lineRenderer.GetPosition(0);
        Vector3 curentpos = MousPosition;
        
        Vector3 distance = curentpos - startpos;
        Vector3 finalForce = distance * forceToAdd;
        
        rb.AddForce(-finalForce,ForceMode2D.Impulse);

        isGrounded = false;
        shoot = true;
    }


}
