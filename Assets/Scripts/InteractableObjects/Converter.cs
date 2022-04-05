using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Converter : MonoBehaviour, IInteractable
{
    [SerializeField] private LineRenderer _laser;
    [SerializeField] private Transform _laserStartTransform;
    [SerializeField] private Color[] _colors;
    [SerializeField] private LayerMask _layerMask;
    private AudioSource _audioSource; 

    private PullableObject currentIPullable;


    bool isShot = false;
    private float currentConvertionTime = 0;
    private float targetConvertionTime = 3;

    public bool isInHands = false;

    private CrosshairController _crosshairController;
    private bool isBlackhole = false;

    public bool DisableInteraction => false;

    private bool firstObjectConverted = false;

    private bool isShooting = false;

    private Vector3 endingOffset = new Vector3(0, 0, 2);

    void Awake()
    {
        _laser.gameObject.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _crosshairController = GameManager.Instance.uiManager.crosshairController;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        if (!isInHands) return;
        if (GameManager.Instance.IsCutscenePlaying && isShooting)
        {
            SetLaser(false);
            _audioSource.Stop();
            return;
        }

        if (!GameManager.Instance.characterController._playerInteraction.canInteract)
        {
            SetLaser(false);
            _audioSource.Stop();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Shoot();
            SetLaser(true);
            _crosshairController.SetProgressVisibility(true);
            //_crosshairController.SetProgress(0);

            if (!isShooting)
            {
                isShooting = true;
                _audioSource.Play();
            }
        }
        else
        {
            currentConvertionTime = 0;
            SetLaser(false);
            _crosshairController.SetProgressVisibility(false);

            if (isShooting)
            {
                isShooting = false;
                _audioSource.Stop();
            }
        }
    }


    //raycast from camera to IPullable object
    //if object is IPullable increase currentConvertionTime by Time.deltaTime
    //set currentShootable to hit object
    //if object is no longer hit, reset currentConvertionTime to 0 and set currentShootable to new object or null
    //if currentConvertionTime is greater than targetConvertionTime, call ConvertToAntiMatter()
    public void Shoot()
    {
        if (currentConvertionTime > targetConvertionTime / 2)
        {
            GameManager.Instance.uiManager.HideAntiMatterTutorial();
        }

        RaycastHit hit;
        Camera camera = GameManager.Instance.characterController._firstPersonAIO.playerCamera;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 100, layerMask: _layerMask))
        {
            var pullable = hit.transform.GetComponent<PullableObject>();
            if (pullable != null)
            {
                targetConvertionTime = pullable.Mass * 3;

                _audioSource.pitch = 1.2f;
                if (isBlackhole)
                {
                    isBlackhole = false;
                    currentConvertionTime = 0;
                }

                isBlackhole = false;
                if (pullable.IsAntiMatter)
                {
                    currentIPullable = null;
                    return;
                }

                if (currentIPullable != pullable)
                {
                    currentConvertionTime = 0;
                }

                currentConvertionTime += Time.deltaTime;
                currentIPullable = pullable;
                _crosshairController.SetProgress(currentConvertionTime / targetConvertionTime);
            }
            else
            {
                var blackHole = hit.transform.GetComponent<BlackHoleController>();
                if (blackHole != null)
                {
                    targetConvertionTime = 5;
                    _audioSource.pitch = 1.2f;
                    if (!isBlackhole)
                    {
                        isBlackhole = true;
                        currentConvertionTime = 0;
                    }

                    currentConvertionTime += Time.deltaTime;
                    _crosshairController.SetProgressVisibility(true);
                    _crosshairController.SetProgress(currentConvertionTime / targetConvertionTime);
                }
                else
                {
                    _audioSource.pitch = 1;
                    isBlackhole = false;
                    currentConvertionTime = 0;
                    currentIPullable = null;
                    _crosshairController.SetProgress(0);
                }
            }
        }
        else
        {
            _audioSource.pitch = 1;
            isBlackhole = false;
            currentConvertionTime = 0;
            _crosshairController.SetProgress(0);
            _crosshairController.SetProgressVisibility(false);
            currentIPullable = null;
        }

        if (currentConvertionTime > targetConvertionTime)
        {
            if (isBlackhole)
            {
                //Destroy(GameManager.Instance.characterController.gameObject);
                GameManager.Instance.AnihilationEnding();

                return;
            }

            if (currentIPullable != null)
            {
                currentIPullable.ConvertToAntiMatter();
                currentIPullable = null;
                currentConvertionTime = 0;
                _crosshairController.SetProgressVisibility(false);

                if (!firstObjectConverted)
                {
                    firstObjectConverted = true;
                    GameManager.Instance.PlayAntiMatterConvertedCutscene();
                }
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

                if (isBlackhole)
                {
                    _laser.SetPosition(1, GameManager.Instance.blackHoleController.transform.position);
                }
                else
                {
                    if (currentIPullable != null)
                    {
                        _laser.SetPosition(1,
                            currentIPullable.pivotPoint != null
                                ? currentIPullable.pivotPoint.position
                                : currentIPullable.transform.position);
                    }
                    else
                    {
                        _laser.SetPosition(1, hit.point);
                    }
                }


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

    public async UniTask PointAtPlayer()
    {
        await UniTask.Delay(1000);
        GameManager.Instance.ShowWhiteFade();
        //_audioSource.Play();

        //await UniTask.Delay(1000);

        //s_audioSource.Stop();
    }

    public string CustomText => "Press E to pick up anti-matter converter";
    public bool IsInteractable { get; }
}