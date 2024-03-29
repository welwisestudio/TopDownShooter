using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class PlusModal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private int _reward;
    [SerializeField] private RectTransform _modalRect;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private float _movingDuration = .1f;
    [SerializeField] private float _hidePosition;
    [SerializeField] private float _showPosition;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private string _adId = "1";

    private bool _isShowing;
    private float _master;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Hide);
        _rewardButton.onClick.AddListener( () => CombinedSDK.OpenRewardAd(_adId));
        _textMesh.text = _reward.ToString();
    }

    private void OnEnable() => CombinedSDK.OnCombinedSDKCloseRewardVideo += Rewarded;
    private void OnDisable() => CombinedSDK.OnCombinedSDKCloseRewardVideo -= Rewarded;

    private void Rewarded(string id)
    {
        if (id == _adId)
        {
            _wallet.Add(_reward);
            //Hide();
        }
    }

    public void Show()
    {
        _master = PlayerPrefs.GetFloat("Master Volume", 0.5f);
        AudioManager.Instance.SetVolume(0, AudioManager.AudioChannel.Master);
        if (_isShowing) return;
        _isShowing = true;
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        StartCoroutine(MoveRoutine(_hidePosition, _showPosition,1, .000000003f));
    }

    public void Hide()
    {
        AudioManager.Instance.SetVolume(_master, AudioManager.AudioChannel.Master);
        if (!_isShowing) return;
        _isShowing = false;
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        StartCoroutine(MoveRoutine(_showPosition, _hidePosition, 1,1));
    }

    private IEnumerator MoveRoutine(float from, float to, float inScale, float outScale)
    {
        Time.timeScale = inScale;
        var destination = new Vector3(_modalRect.anchoredPosition.x, to);
        var start = new Vector3(_modalRect.anchoredPosition.x, from);
        for (float t = 0; t <= _movingDuration; t += Time.deltaTime)
        {
            _modalRect.anchoredPosition = Vector2.Lerp(start, destination, Mathf.SmoothStep(0, 1, t / _movingDuration));
            yield return new WaitForEndOfFrame();
        }
        _modalRect.anchoredPosition = destination;
        Time.timeScale = outScale;
    }
}
