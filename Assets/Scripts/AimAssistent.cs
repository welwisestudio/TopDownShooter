using System;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistent : MonoBehaviour
{
    private const int _detectionFrameRate = 25;
    [SerializeField] private Player _player;
    [SerializeField] private LayerMask _detectMask;
    [SerializeField] private float _detectRadius;
    [SerializeField] private float _aimOffset = .05f;
    private int _frameCount = 0;
    private Transform _currentTarget;
    private GameObject _crosshairObject;

    private void Start()
    {
        _crosshairObject = _player.Crosshairs.gameObject;
    }

    private void Update()
    {
        if (++_frameCount >= _detectionFrameRate)
        {
            DetectEnemy();
        }

        AimToTarget();
    }

    private void DetectEnemy()
    {
        var enemies = GetNearestEnemies();

        foreach (var enemy in enemies)
        {
            if (ClearWayToEnemy(enemy))
            { 
                _currentTarget = enemy.transform;
                return;
            }
        }
    }

    private void AimToTarget()
    {
        _crosshairObject.SetActive(_currentTarget != null);
        if (_currentTarget == null)
            return;

        _player.Controller.LookAt(_currentTarget.position);
        _player.Crosshairs.transform.position = _currentTarget.position + _currentTarget.forward * _aimOffset;
        _player.Crosshairs.DetectTarget();
        _player.GunControllerScript.Aim(_currentTarget.position);
    }

    private bool ClearWayToEnemy(Enemy enemy)
    {
        if (Physics.Linecast(_player.transform.position, enemy.transform.position, out var hitInfo))
        {
            if (hitInfo.collider.TryGetComponent<Enemy>(out var tenemy))
                return tenemy == enemy;
            else return false;
        }
        else return true;
    }

    private Enemy[] GetNearestEnemies()
    {
        var colliders = Physics.OverlapSphere(_player.transform.position, _detectRadius, _detectMask);
        List<Enemy> enemies = new List<Enemy>();

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Enemy>(out var enemy))
                enemies.Add(enemy);
        }
        var arr = enemies.ToArray();
        Array.Sort(arr, (enemy1, enemy2) =>
        {
            float distanceToPlayer1 = Vector3.Distance(enemy1.transform.position, _player.transform.position);
            float distanceToPlayer2 = Vector3.Distance(enemy2.transform.position, _player.transform.position);
            return distanceToPlayer1.CompareTo(distanceToPlayer2);
        });
        return arr;
    }

    private void OnEnable()
    {
        _player.ControllLook = false;
    }

    private void OnDisable()
    {
        _player.ControllLook = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var tcolor = Gizmos.color;
        Gizmos.color = UnityEngine.Color.red;

        Gizmos.DrawWireSphere(_player.transform.position, _detectRadius);

        Gizmos.color = tcolor;
    }
#endif  
}
