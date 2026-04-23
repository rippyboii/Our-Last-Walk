using System.Collections;
using UnityEngine;

public class TurntableController : MonoBehaviour
{
    [Header("Parts")]
    public Transform platter;
    public Transform btnPower;
    public Transform btnPlayPause;
    public Transform btnNext;
    public Transform btnPrev;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] tracks;

    [Header("References")]
    public Camera_movement cameraMovement;
    public Player player;
    public Vector3 cameraOffset   = new Vector3(0f, 0.35f, 0.1f);
    public Vector3 cameraRotation = new Vector3(60f, 180f, 0f);

    [Header("Button Feel")]
    public float pressDepth    = 0.005f;
    public float pressDuration = 0.06f;
    public float popDuration   = 0.09f;

    private bool isPoweredOn  = false;
    private bool isPlaying    = false;
    private int  currentTrack = 0;
    private bool animating    = false;

    private Camera     cam;
    private Vector3    camOriginalPos;
    private Quaternion camOriginalRot;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (isPoweredOn && isPlaying && platter != null)
            platter.Rotate(Vector3.up, 198f * Time.deltaTime, Space.Self); // 33 RPM

        if (Input.GetKeyDown(KeyCode.Escape))
            Exit();

        if (!animating && Input.GetMouseButtonDown(0))
            HandleClick();
    }

    public void Enter()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        camOriginalPos = cam.transform.position;
        camOriginalRot = cam.transform.rotation;
        cam.transform.position = transform.TransformPoint(cameraOffset);
        cam.transform.rotation = Quaternion.Euler(cameraRotation);

        cameraMovement.enabled = false;
        player.enabled         = false;
    }

    void Exit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        cam.transform.position = camOriginalPos;
        cam.transform.rotation = camOriginalRot;

        cameraMovement.enabled = true;
        player.enabled         = true;
    }

    void HandleClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 5f)) return;

        switch (hit.transform.name.ToLower())
        {
            case "powerbtn":   StartCoroutine(PressButton(btnPower,     OnPower));     break;
            case "play/pause": StartCoroutine(PressButton(btnPlayPause, OnPlayPause)); break;
            case "next":       StartCoroutine(PressButton(btnNext,      OnNext));      break;
            case "prev":       StartCoroutine(PressButton(btnPrev,      OnPrev));      break;
        }
    }

    IEnumerator PressButton(Transform btn, System.Action callback)
    {
        if (btn == null) yield break;
        animating = true;

        Vector3 origin  = btn.localPosition;
        Vector3 pressed = origin - new Vector3(0f, pressDepth, 0f);

        float t = 0f;
        while (t < 1f) { t += Time.deltaTime / pressDuration; btn.localPosition = Vector3.Lerp(origin, pressed, Mathf.Clamp01(t)); yield return null; }

        callback?.Invoke();

        t = 0f;
        while (t < 1f) { t += Time.deltaTime / popDuration;   btn.localPosition = Vector3.Lerp(pressed, origin, Mathf.Clamp01(t)); yield return null; }

        btn.localPosition = origin;
        animating = false;
    }

    void OnPower()
    {
        isPoweredOn = !isPoweredOn;
        if (!isPoweredOn) { isPlaying = false; audioSource?.Stop(); }
        else              { isPlaying = true;  PlayTrack(); }
    }

    void OnPlayPause()
    {
        if (!isPoweredOn) return;
        isPlaying = !isPlaying;
        if (isPlaying) audioSource?.UnPause();
        else           audioSource?.Pause();
    }

    void OnNext()
    {
        if (!isPoweredOn) return;
        currentTrack = (currentTrack + 1) % tracks.Length;
        PlayTrack();
    }

    void OnPrev()
    {
        if (!isPoweredOn) return;
        currentTrack = (currentTrack - 1 + tracks.Length) % tracks.Length;
        PlayTrack();
    }

    void PlayTrack()
    {
        if (audioSource == null || tracks == null || tracks.Length == 0) return;
        audioSource.clip = tracks[currentTrack];
        audioSource.Play();
    }
}