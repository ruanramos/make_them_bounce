using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class DrawLine2D : MonoBehaviour
{

    [FormerlySerializedAs("MLineRenderer")] [FormerlySerializedAs("m_LineRenderer")] [SerializeField]
    private LineRenderer _mLineRenderer;
    [FormerlySerializedAs("MAddCollider")] [FormerlySerializedAs("m_AddCollider")] [SerializeField]
    private bool _mAddCollider;
    [FormerlySerializedAs("MEdgeCollider2D")] [FormerlySerializedAs("m_EdgeCollider2D")] [SerializeField]
    private EdgeCollider2D _mEdgeCollider2D;
    [FormerlySerializedAs("MCamera")] [FormerlySerializedAs("m_Camera")] [SerializeField]
    private Camera _mCamera;

    private List<Vector2> _mPoints;

    // this variable ignoreInput exists so one prefab that will make a line renderer
    // can create another prefab and stop receiving inputs, so it will only create one line by prefab
    private bool _ignoreInput;
    private bool _createdChildPrefab;
    private bool _draging;
    
    private Vector2 _startMousePosition;
    private Vector2 _startTouchPosition;
    private float _timeFingerLeftScreen;

    private GlobalInfos _watcherGlobalInfosScript;

    [FormerlySerializedAs("lineTextures")] [SerializeField]
    public Texture2D[] LineTextures;
    private Touch _myTouch;
    [FormerlySerializedAs("lineMaterial")] [SerializeField]
    public Material LineMaterial;

    private const float MagicCost = 0.2f;

    private PlatformEffector2D _ef;

/*
    public virtual LineRenderer LineRenderer
    {
        get
        {
            return MLineRenderer;
        }
    }
*/

/*
    public virtual bool AddCollider
    {
        get
        {
            return MAddCollider;
        }
    }
*/

/*
    public virtual EdgeCollider2D EdgeCollider2D
    {
        get
        {
            return MEdgeCollider2D;
        }
    }
*/

/*
    public virtual List<Vector2> Points
    {
        get
        {
            return _mPoints;
        }
    }
*/

    private void Awake()
    {
        if (_mLineRenderer == null)
        {
            //Debug.LogWarning("DrawLine: Line Renderer not assigned, Adding and Using default Line Renderer.");
            CreateDefaultLineRenderer();
        }
        if (_mEdgeCollider2D == null && _mAddCollider)
        {
            //Debug.LogWarning("DrawLine: Edge Collider 2D not assigned, Adding and Using default Edge Collider 2D.");
            CreateDefaultEdgeCollider2D();
        }
        if (_mCamera == null)
        {
            _mCamera = Camera.main;
        }
        _mPoints = new List<Vector2>();


        // Initial characteristics of the line renderer
        _mLineRenderer.colorGradient.mode = GradientMode.Fixed;
        _mLineRenderer.startColor = new Color(124f/255, 252f/255, 0f);
        _mLineRenderer.endColor = new Color(40f / 255, 255f / 255, 0f);
        _mLineRenderer.startWidth = 0.8f;
        _mLineRenderer.endWidth = 0.8f;
        _mLineRenderer.widthMultiplier = 0.8f;
        _mLineRenderer.numCornerVertices = 0;
        _mLineRenderer.numCapVertices = 0;
        _mEdgeCollider2D.edgeRadius = 0.15f;
        _mEdgeCollider2D.usedByEffector = true;

        _ef = gameObject.AddComponent<PlatformEffector2D>();
        _ef.useOneWay = false;
    }

    private void Start()
    {
        Destroy(gameObject, 8);
        // getting reference for the script
        _watcherGlobalInfosScript = GameObject.Find("Watcher").GetComponent<GlobalInfos>();
    }

    private void Update()
    {
        /*
        // ----------------- mouse input -------------------

        if (Input.GetMouseButtonUp(0))
        {
            if (!ignoreInput)
            {
                timeButtonUp = Time.time;
            }
            ignoreInput = true;
            dragging = false;
        }
        if (Input.GetMouseButtonDown(0) && !ignoreInput)
        {
            Reset();
        }
        else if(Input.GetMouseButtonDown(0) && ignoreInput)
        {
            // creates another line
            if(!createdChildPrefab)
            {
                CreateAnotherLine();
                createdChildPrefab = true;
            }
        }
        if (Input.GetMouseButton(0) && !ignoreInput)
        {
            // add points to current line and decreases magic amount
            if (watcherGlobalInfosScript.magic >= 0)
            {
                Vector2 mousePosition = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                if (!dragging)
                {
                    startMousePosition = mousePosition;
                    dragging = true;
                }
                // use magic only if i click and move move the mouse
                if (Vector2.Distance(startMousePosition, new Vector2(m_Camera.ScreenToWorldPoint(Input.mousePosition).x, m_Camera.ScreenToWorldPoint(Input.mousePosition).y)) > 0.1f)
                {
                    watcherGlobalInfosScript.magic -= CalculateTotalLineDistance(m_Points) * 2;
                }
                // here im holding the mouse clicked
                else
                {

                }
                if (!m_Points.Contains(mousePosition) && watcherGlobalInfosScript.magic >= 0)
                {
                    m_Points.Add(mousePosition);
                    m_LineRenderer.positionCount = m_Points.Count;
                    m_LineRenderer.SetPosition(m_LineRenderer.positionCount - 1, mousePosition);
                    if (m_EdgeCollider2D != null && m_AddCollider && m_Points.Count > 1)
                    {
                        m_EdgeCollider2D.points = m_Points.ToArray();
                        m_EdgeCollider2D.enabled = true;
                    }
                }
            }
        }
        */
        //---------------------- touch input ---------------------------
        var panelRed = GameObject.Find("PanelRed");
        var panelRedColor = panelRed.GetComponent<Image>().color;

        if (Input.touchCount > 0)
        {
            _myTouch = Input.GetTouch(0);
        }
        _mEdgeCollider2D.sharedMaterial = Resources.Load("New Physics Material 2D") as PhysicsMaterial2D;
        GameObject.Find("ChargeText").GetComponent<Text>().text = _watcherGlobalInfosScript.UpgradeCharge >= 100 ? "CHARGE READY!" : "";

        
        // checking chargeUpUpgrade cooldown
        if (_watcherGlobalInfosScript.ChargeUpOnCooldown)
        {
            if (Time.time - _watcherGlobalInfosScript.TimeLastChargeUpHappened >= _watcherGlobalInfosScript.ChargeUpCooldown)
            {
                _watcherGlobalInfosScript.ChargeUpOnCooldown = false;
            }
        }

        if (_watcherGlobalInfosScript.ChargeUpOnCooldown)
        {
            _watcherGlobalInfosScript.UpgradeCharge = 0;
        }
        
        // Just lifted my finger of screen
        if (_myTouch.phase == TouchPhase.Ended)
        {
            // reset upgrade that charges up the circles visual feedback to 0 alpha
            panelRed.GetComponent<Image>().color = new Color(panelRedColor.r, panelRedColor.g, panelRedColor.b, 0);

            // change color to show player feedback when line will start disappearing
            var timeLineExists = Time.time - _timeFingerLeftScreen;
            var lineColor = new Color(124 / 255f + 80 * timeLineExists / 255f, 252 / 255f - 80 * timeLineExists / 255f, 0f);
            _mLineRenderer.startColor = lineColor;
            _mLineRenderer.endColor = lineColor;

            // here we charge the circles up
            if (_watcherGlobalInfosScript.UpgradeCharge >= 100 && !_watcherGlobalInfosScript.ChargeUpOnCooldown)
            {
                var balls = GameObject.FindGameObjectsWithTag("Ball");
                foreach (var ball in balls)
                {
                    ball.GetComponent<Circle>().ChargeUp();
                }

                _watcherGlobalInfosScript.TimeLastChargeUpHappened = Time.time;
                _watcherGlobalInfosScript.ChargeUpOnCooldown = true;
            }
            _watcherGlobalInfosScript.UpgradeCharge = 0;
            if (!_ignoreInput)
            {
                _timeFingerLeftScreen = Time.time;
            }
            _ignoreInput = true;
            _draging = false;
        }
        
        if (_myTouch.phase == TouchPhase.Began && !_ignoreInput)
        {
            Reset();
        }
        else if (_myTouch.phase == TouchPhase.Began && _ignoreInput)
        {
            // creates another line
            if (!_createdChildPrefab)
            {
                CreateAnotherLine();
                _createdChildPrefab = true;
            }
        }

        if (_myTouch.phase == TouchPhase.Moved && !_ignoreInput)
        {
            
            panelRed.GetComponent<Image>().color = new Color(panelRedColor.r, panelRedColor.g, panelRedColor.b, 0);
            _watcherGlobalInfosScript.UpgradeCharge = 0;
            
            // add points to current line and decreases magic amount
            if (_watcherGlobalInfosScript.Charge >= 0)
            {
                Vector2 touchPosition = _mCamera.ScreenToWorldPoint(_myTouch.position);
                if (!_draging)
                {
                    _startTouchPosition = touchPosition;
                    _draging = true;
                }
                
                // use magic only if i click and move move the finger
                if (Vector2.Distance(_startTouchPosition, new Vector2(_mCamera.ScreenToWorldPoint(_myTouch.position).x, _mCamera.ScreenToWorldPoint(_myTouch.position).y)) > 0.1f)
                {
                    // decreases magic
                    // i have to set up the value here, didn't understand what was happening to the value on the other script
                    _watcherGlobalInfosScript.Charge -= MagicCost;
                }
                if (!_mPoints.Contains(touchPosition) && _watcherGlobalInfosScript.Charge >= 0)
                {
                    _mPoints.Add(touchPosition);
                    _mLineRenderer.positionCount = _mPoints.Count;
                    _mLineRenderer.SetPosition(_mLineRenderer.positionCount - 1, touchPosition);
                    if (_mEdgeCollider2D != null && _mAddCollider && _mPoints.Count > 1)
                    {
                        _mEdgeCollider2D.points = _mPoints.ToArray();
                        _mEdgeCollider2D.enabled = true;
                    }
                }
            }
        }

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Stationary)
        {
            if (_watcherGlobalInfosScript.UpgradeCharge < 100 && !_watcherGlobalInfosScript.ChargeUpOnCooldown)
            {
                _watcherGlobalInfosScript.UpgradeCharge += 100 * Time.deltaTime;
            }
        }
        
        //--------------------------- end touch input -------------------------

        // charging ChargeUp upgrade
        if(_watcherGlobalInfosScript.UpgradeCharge > 0 && !_watcherGlobalInfosScript.ChargeUpOnCooldown)
        {
            panelRed.GetComponent<Image>().color = new Color(panelRedColor.r, panelRedColor.g, panelRedColor.b, _watcherGlobalInfosScript.UpgradeCharge/355f);
        }

        _watcherGlobalInfosScript.ChargeUpImageBlue.fillAmount = _watcherGlobalInfosScript.UpgradeCharge / 100.0f;
        if (_watcherGlobalInfosScript.ChargeUpOnCooldown)
        {
            _watcherGlobalInfosScript.ChargeUpImageRed.fillAmount = (Time.time - _watcherGlobalInfosScript.TimeLastChargeUpHappened) /_watcherGlobalInfosScript.ChargeUpCooldown;
        }
        else
        {
            _watcherGlobalInfosScript.ChargeUpImageRed.fillAmount -= Time.deltaTime;
        }

        // Removing the line after the time
        if (Time.time - _timeFingerLeftScreen >= _watcherGlobalInfosScript.TimeToStartVanishing && _ignoreInput)
        {
            RemoveColliderAndDestroy(gameObject);
            
        }
    }

    private IEnumerator WaitSecondsAndRemoveNextPoint(float seconds)
    {
        var oldPos = new Vector3[GetComponent<LineRenderer>().positionCount];
        GetComponent<LineRenderer>().GetPositions(oldPos);
        Vector3[] newPos;
        Vector2[] newPosV2;
        if (GetComponent<LineRenderer>().positionCount - 1 >= 0)
        {
            newPos = new Vector3[GetComponent<LineRenderer>().positionCount - 1];
            newPosV2 = new Vector2[GetComponent<LineRenderer>().positionCount - 1];
        }
        else
        {
            newPos = new Vector3[1];
            newPosV2 = new Vector2[1];
        }
        
        for (var i = 0; i < oldPos.Length-1; i++)
        {
            newPos[i] = oldPos[i + 1];
        }
        for(var i = 0; i < newPos.Length; i++)
        {
            newPosV2[i] = new Vector2(newPos[i].x, newPos[i].y);
        }
        yield return new WaitForSeconds(seconds);
        GetComponent<LineRenderer>().SetPositions(newPos);

        var flag = false;
        foreach (var item in newPos)
        {
            if (item == newPos[0]) continue;
            flag = true;
            break;
        }
        if (!flag && _createdChildPrefab)
        {
            Destroy(gameObject);
        }
        if(newPosV2.Length > 1)
        {
            GetComponent<EdgeCollider2D>().points = newPosV2;
        }
        
    }

    private float CalculateTotalLineDistance(List<Vector2> points)
    {
        float distance = 0;
        for (var i = 0; i < points.Count - 1; i++)
        {
            distance += Vector2.Distance(points[i], points[i + 1]);
        }
        return distance;
    }

    private void CreateAnotherLine()
    {
        Instantiate(Resources.Load("prefab 1", typeof(GameObject)), transform.position, transform.rotation);
    }

    private void Reset()
    {
        if (_mLineRenderer != null)
        {
            _mLineRenderer.positionCount = 0;
        }
        if (_mPoints != null)
        {
            _mPoints.Clear();
        }
        if (_mEdgeCollider2D != null && _mAddCollider)
        {
            _mEdgeCollider2D.Reset();
        }
    }

    private void CreateDefaultLineRenderer()
    {
        _mLineRenderer = gameObject.AddComponent<LineRenderer>();
        _mLineRenderer.positionCount = 0;
        //m_LineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        _mLineRenderer.material = LineMaterial;
        _mLineRenderer.startColor = Color.white;
        _mLineRenderer.endColor = Color.white;
        _mLineRenderer.startWidth = 0.2f;
        _mLineRenderer.endWidth = 0.2f;
        _mLineRenderer.useWorldSpace = true;
        _mLineRenderer.textureMode = LineTextureMode.Tile;
        var num = Random.Range(0, LineTextures.Length - 1);
        _mLineRenderer.material.mainTexture = LineTextures[num];
        _mLineRenderer.material.mainTextureScale = new Vector2(2f, 1);
        _mLineRenderer.sortingOrder = 10000;
        
    }

    private void CreateDefaultEdgeCollider2D()
    {
        _mEdgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
        _mEdgeCollider2D.enabled = false;
        
    }

    private void RemoveColliderAndDestroy(GameObject o)
    {
        GetComponent<EdgeCollider2D>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
        Destroy(gameObject, 15);
    }

}