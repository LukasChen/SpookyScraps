using System;
using UnityEngine;

public class GunLaser : MonoBehaviour {
    [SerializeField] private float _laserDistance;
    [SerializeField] private LayerMask _ignoreMask;
    [SerializeField] private BoolDataEventChannelSO _toggleLaser;
    private bool _laserEnabled = false;
    private LineRenderer _lineRenderer;
    

    private void ToggleLaser(bool status) {
        enabled = status;
        _lineRenderer.enabled = status;
        _laserEnabled = status;
    }

    private void Start() {
        _toggleLaser.OnEventRaised += ToggleLaser;
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        ToggleLaser(false);
    }
    
    private void OnDestroy() {
        _toggleLaser.OnEventRaised -= ToggleLaser;
    }

    private void Update() {
        if (_laserEnabled) {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)) {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, hit.point);
            }
            else {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, transform.position + transform.forward * _laserDistance);
            }
        }
    }
}