using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Material _stretchLineMaterial;
    [SerializeField] private GameObject _stretchLinePrefab;

    private Player _player;
    private Puck _puck;
    private Rigidbody _puckRigidbody;
    private Arrow _arrow;

    private PoolManager _poolManager;

    private readonly Vector3 _playerStartPosition = new Vector3(0.0f, 0.0f, -6.0f);
    private readonly Vector3 _puckStartPosition = new Vector3(0.0f, 0.15f, -12.0f);

    private bool _isAboveUi;
    private bool _isPuckLocked;
    private bool _isReleased;

    private float _nimDistToLock = 4.0f;
    private float _thrust = 5.0f;

    public TextMeshProUGUI _text1;
    public TextMeshProUGUI _text2;
    public TextMeshProUGUI _text3;
    public TextMeshProUGUI _text4;

    private void Start()
    {
        _poolManager = InjectBox.Get<PoolManager>();

        _player = GetComponent<Player>();
        _puck = GameObject.Find("Puck").GetComponent<Puck>();
        _puckRigidbody = _puck.GetComponent<Rigidbody>();

        if (_player != null) _player.transform.position = _playerStartPosition;
        if (_puck != null) _puck.transform.position = _puckStartPosition;


        _arrow = _poolManager.Create<Arrow>(_stretchLinePrefab, _puck.transform);
        _arrow.gameObject.SetActive(false);
    }

    private void Update()
    {
#if UNITY_EDITOR
        MouseMovementPlayer();
        PullPuck();
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        TouchMovementPlayer();
#endif
    }

    private void FixedUpdate()
    {
        ReleasePuck();
    }

    private void MouseMovementPlayer()
    {
        float deltaX;
        float deltaZ;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) _isAboveUi = true;
            if (!EventSystem.current.IsPointerOverGameObject()) _isAboveUi = false;
        }

        if (Input.GetMouseButtonUp(0)) _isAboveUi = false;

        if (Input.GetMouseButton(0) && !_isAboveUi)
        {
            deltaX = Input.GetAxis("Mouse X");
            deltaZ = Input.GetAxis("Mouse Y");
            _player.transform.position += new Vector3(deltaX, 0.0f, deltaZ);

            SetClampCoordinates();
        }
        else if (!_isAboveUi)
        {
            SetClampCoordinates();
        }
    }

    private void PullPuck()
    {
        if (Input.GetMouseButton(0))
        {
            var puckPosition = _puck.transform.position;
            var playerPosition = _player.transform.position;

            if (Vector3.Distance(puckPosition, playerPosition) <= _nimDistToLock)
            {
                _isPuckLocked = true;
            }

            if (_isPuckLocked)
            {
                _arrow.gameObject.SetActive(true);
                _arrow.transform.LookAt(_player.transform);

                var distance = Vector3.Distance(puckPosition, playerPosition);
                var scale = _arrow.transform.localScale;
                _arrow.transform.localScale = new Vector3(scale.x, scale.y, distance / 10);

                if (!_puck.PuckParticle.isPlaying) _puck.PuckParticle.Play();
            }
        }
    }

    private void ReleasePuck()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var puckPosition = _puck.transform.position;
            var playerPosition = _player.transform.position;
            
            _puckRigidbody.velocity = (playerPosition - puckPosition) * _thrust;
            _arrow.gameObject.SetActive(false);
            if (_puck.PuckParticle.isPlaying) _puck.PuckParticle.Stop();
        }
    }

    private void SetClampCoordinates()
    {
        var pos = _player.transform.position;
        _player.transform.position =
            new Vector3(Mathf.Clamp(pos.x, -8.0f, 8.0f), 0.0f, Mathf.Clamp(pos.z, -14.0f, -3.0f));
    }

    private void TouchMovementPlayer()
    {
        _text4.text = "Touch count: " + Input.touchCount;

        if (Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject())
        {
            var firstTouch = Input.GetTouch(0);

            var deltaX = -firstTouch.deltaPosition.x / 2.0f * Time.deltaTime;
            var deltaZ = -firstTouch.deltaPosition.y / 2.0f * Time.deltaTime;
            var pos = _player.transform.position;
            _player.transform.position += new Vector3(deltaX, 0.0f, deltaZ);

            _text1.text = "Camera position: " + _player.transform.position;
            _text2.text = "Delta X: " + deltaX;
            _text3.text = "Delta Y: " + deltaZ;

            SetClampCoordinates();
        }
    }
}