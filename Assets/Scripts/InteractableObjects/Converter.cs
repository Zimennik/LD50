using DG.Tweening;
using UnityEngine;

public class Converter : MonoBehaviour, IInteractable
{
    [SerializeField] private LineRenderer _laser;
    [SerializeField] private Transform _laserStartTransform;
    [SerializeField] private Color[] _colors;
    [SerializeField] private LayerMask _layerMask;

    private PullableObject currentIPullable;


    bool isShot = false;
    private float currentConvertionTime = 0;
    private float targetConvertionTime = 3;

    public bool isInHands = false;


    void Awake()
    {
        _laser.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isInHands) return;
        if (Input.GetMouseButton(0))
        {
            Shoot();
            SetLaser(true);
        }
        else
        {
            SetLaser(false);
        }
    }


    //raycast from camera to IPullable object
    //if object is IPullable increase currentConvertionTime by Time.deltaTime
    //set currentShootable to hit object
    //if object is no longer hit, reset currentConvertionTime to 0 and set currentShootable to new object or null
    //if currentConvertionTime is greater than targetConvertionTime, call ConvertToAntiMatter()
    public void Shoot()
    {
        RaycastHit hit;
        Camera camera = GameManager.Instance.characterController._firstPersonAIO.playerCamera;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100, layerMask: _layerMask))
        {
            var pullable = hit.transform.GetComponent<PullableObject>();
            if (pullable != null)
            {
                if (pullable.IsAntiMatter)
                {
                    currentIPullable = null;
                    return;
                }

                currentConvertionTime += Time.deltaTime;
                currentIPullable = pullable;
            }
            else
            {
                currentConvertionTime = 0;
                currentIPullable = null;
            }
        }
        else
        {
            currentConvertionTime = 0;
            currentIPullable = null;
        }

        if (currentConvertionTime > targetConvertionTime)
        {
            if (currentIPullable != null)
            {
                currentIPullable.ConvertToAntiMatter();
                currentIPullable = null;
                currentConvertionTime = 0;
            }
        }

        //while shooting, change laser color to random color with DOTween
        _laser.material.DOColor(_colors[Random.Range(0, _colors.Length)], "_EmissionColor", 0.2f);
    }


    //Draws a line from the converter to the forward until any object is hit
    public void SetLaser(bool value)
    {
        _laser.gameObject.SetActive(value);

        if (value)
        {
            RaycastHit hit;
            Camera camera = GameManager.Instance.characterController._firstPersonAIO.playerCamera;
            //Debug.DrawRay(_laserStartTransform.position, camera.transform.forward, Color.red);
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100,
                    layerMask: _layerMask))
            {
                _laser.SetPosition(0, _laserStartTransform.position);
                _laser.SetPosition(1, currentIPullable != null ? currentIPullable.transform.position : hit.point);
                Debug.Log(hit.transform.name);
            }
            else
            {
                _laser.SetPosition(0, _laserStartTransform.position);
                _laser.SetPosition(1, _laserStartTransform.position + camera.transform.forward * 1000);
            }
        }
    }


    public void Interact()
    {
        GameManager.Instance.characterController._playerInteraction.PickUpConverter(this);
    }

    public void CursorEnter()
    {
        // throw new System.NotImplementedException();
    }

    public void CursorExit()
    {
        // throw new System.NotImplementedException();
    }

    public void TurnCollider(bool value)
    {
        GetComponent<Collider>().enabled = value;
    }

    public string CustomText => "Pick up anti-matter converter";
    public bool IsInteractable { get; }
}