using UnityEngine;
using YG;

public class DeviceDefinition : MonoBehaviour
{
    [SerializeField] private GameObject _mobileUI;
    [SerializeField] private ShopTogler[] _togglers;
    [SerializeField] private AimAssistent _aimAssistent;
    [SerializeField] private Player _player;
    public void DefineDevice()
    {
        if (YandexGame.EnvironmentData.isMobile)
        {
            _player.MobileControll = true;
            _mobileUI.SetActive(true);
            _aimAssistent.enabled = true;
            foreach (var togler in _togglers)
            {
                togler.IsMobile = true;
            }
        }
    } 

    private void Start()
    {
        _player.MobileControll = false;
        _mobileUI.SetActive(false);
        _aimAssistent.enabled = false;
        _player.MobileControll = false;
        foreach (var togler in _togglers)
        {
            togler.IsMobile = false;
        }

        if (YandexGame.SDKEnabled)
            DefineDevice();
    }

    private void OnEnable() => YandexGame.GetDataEvent += DefineDevice;

    private void OnDisable() => YandexGame.GetDataEvent -= DefineDevice;
}
