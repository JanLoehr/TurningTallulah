using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TallulhaControl : MonoBehaviour
{
    [Header("Flight Params")]
    public float Speed = 50;
    public float TurnSpeed = 0.5f;

    public float StartHeight = -20;

    [Header("Spray Params")]
    public float SprayShotDuration = 0.2f;
    public ParticleSystem SprayParticles;

    [Header("Audio")]
    public AudioClip CrashSound;
    public float MinCollisionSoundDelay;

    [Space()]
    [Range(0, 1)]
    public float EngineMenuVol;
    [Range(0, 1)]
    public float EngineGameVol;
    [Range(0, 1)]
    public float WindMenuVol;
    [Range(0, 1)]
    public float WindGameVol;

    [Header("Object Refs")]
    public LevelManager LevelManager;

    public Transform IntroCamPosRef;
    public Transform GameCamPosRef;

    public AudioSource EngineAudioSource;
    public AudioSource WindAudioSource;
    public AudioSource SprayAudioSource;


    [Header("Events")]
    public UnityEvent CrashStart;

    private Transform _trans;
    private Transform _gyroRef;

    private float _touchPrevX;

    private bool _controllable = true;

    private bool _spraying;
    private float _sprayStart;
    private float _sprayTime;

    private Collider[] _colliders;
    private Rigidbody _rigidbody;

    private float _lastCollisionSound;

    private Coroutine _crashRoutine;
    private Coroutine _windSoundRoutine;
    private Coroutine _engineSoundRoutine;

    // Start is called before the first frame update
    void Start()
    {
        if (_trans == null)
        {
            _trans = transform;
            _gyroRef = new GameObject("GyroRef").transform;

            _rigidbody = GetComponent<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();

            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_controllable)
        {
            return;
        }

        _trans.position += _trans.right * Time.deltaTime * Speed;

        if (SystemInfo.supportsGyroscope)
        {
            if (!Input.gyro.enabled)
            {
                Input.gyro.enabled = true;
            }

            Vector3 gravity = Input.gyro.gravity;

            _gyroRef.position = _trans.position + new Vector3(gravity.x, gravity.y, 0) * 5;
            _gyroRef.RotateAround(_trans.position, new Vector3(0, 0, 1), 90);

            _trans.rotation = Quaternion.identity;
            _trans.Rotate(0, 0, Vector2.SignedAngle(Vector2.right, _gyroRef.position - _trans.position));
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touchPrevX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                _trans.Rotate(0, 0, (Input.mousePosition.x - _touchPrevX) * TurnSpeed, Space.Self);
                _touchPrevX = Input.mousePosition.x;
            }
        }
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Equals("Finish") && !other.tag.Equals("NonLethal"))
        {
            if (_controllable)
            {
                _crashRoutine = StartCoroutine(TriggerCrash(false));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time > _lastCollisionSound + MinCollisionSoundDelay)
        {
            EngineAudioSource.PlayOneShot(CrashSound);
            _lastCollisionSound = Time.time;
        }
    }

    public void SetPlaying()
    {
        if (_trans == null)
        {
            Start();
        }

        enabled = true;

        if (!_rigidbody.isKinematic)
        {
            Reset(true);
        }

        SetSoundVolume(_engineSoundRoutine, EngineAudioSource, EngineGameVol);
        SetSoundVolume(_windSoundRoutine, WindAudioSource, WindGameVol);
    }

    public void Reset(bool withPosition = false)
    {
        if (_trans == null)
        {
            Start();
        }

        SetSoundVolume(_engineSoundRoutine, EngineAudioSource, EngineGameVol);
        SetSoundVolume(_windSoundRoutine, WindAudioSource, WindGameVol);

        enabled = true;

        _rigidbody.isKinematic = true;

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].isTrigger = true;
        }

        _controllable = true;

        if (_crashRoutine != null)
        {
            StopCoroutine(_crashRoutine);
        }

        if (withPosition)
        {
            _trans.SetPositionAndRotation(new Vector3(0, StartHeight, 0), Quaternion.identity);
        }
    }

    public void VolumeDown()
    {
        SetSoundVolume(_engineSoundRoutine, EngineAudioSource, EngineMenuVol);
        SetSoundVolume(_windSoundRoutine, WindAudioSource, WindMenuVol);
    }

    public void FinishHit()
    {
        if (_controllable)
        {
            GameStateManager.Instance.SwitchGameState("Finish");

            _crashRoutine = StartCoroutine(TriggerCrash(true));
        }
    }

    public void CroplineHit()
    {
        _sprayTime += SprayShotDuration;

        LevelManager.CropReached();

        if (!_spraying)
        {
            _spraying = true;
            _sprayStart = Time.time;

            StartCoroutine(SprayAsync());
        }
    }

    public void StarCollected()
    {
        LevelManager.StarCollected();
    }

    private void SetSoundVolume(Coroutine routine, AudioSource source, float vol)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }

        routine = StartCoroutine(FadeSoundAsync(source, vol));
    }

    private IEnumerator FadeSoundAsync(AudioSource source, float vol)
    {
        float lerp = 0;
        float start = source.volume;

        while (source.volume != vol)
        {
            lerp += Time.deltaTime;
            source.volume = Mathf.Lerp(start, vol, lerp);

            yield return null;
        }
    }

    private IEnumerator SprayAsync()
    {
        SprayParticles.enableEmission = true;
        SprayAudioSource.mute = false;

        while (_sprayStart + _sprayTime > Time.time)
        {
            yield return null;
        }

        _spraying = false;
        _sprayTime = 0;

        SprayParticles.enableEmission = false;
        SprayAudioSource.mute = true;
    }

    private IEnumerator TriggerCrash(bool finished)
    {
        _controllable = false;

        if (!finished)
        {
            CrashStart.Invoke();
        }

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].isTrigger = false;
        }

        _rigidbody.isKinematic = false;

        _rigidbody.velocity = _trans.right * Speed;

        yield return new WaitForSeconds(3);

        if (GameStateManager.Instance.CurrentState == GameStates.Playing)
        {
            Reset();

            LevelManager.ReloadLevel();
        }
        else
        {
            enabled = false;
        }
    }
}
