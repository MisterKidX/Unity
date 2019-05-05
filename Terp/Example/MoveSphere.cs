using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Example
{
    public class MoveSphere : MonoBehaviour
    {
        public Curve[] curves;
        public Transform newPosition;
        public Color newColor;
        public float maxSize;
        public float time = 2;

        private Curve _curve;
        private Vector3 _startPos;
        private Material _startMat;
        private float _startAlpha;
        private float _counterA = 0;
        private float _counterX = 0;
        private float _counterS = 0;
        private int _mode = 0;
        private int _curveMode = 0;

        private void Awake()
        {
            _startPos = transform.position;
            _startMat = GetComponent<Renderer>().material;
            _startAlpha = _startMat.color.a;
            _curve = curves[0];
            _mode = 0;
        }

        void Update ()
        {
            switch (_mode)
            {
                case (0):
                    Move();
                    break;
                case (1):
                    Alpha();
                    break;
                case (2):
                    Resize();
                    break;
                case (3):
                    Move();
                    Alpha();
                    Resize();
                    break;
                default:
                    break;
            }
        }

        public void SetMode(int mode)
        {
            this._mode = mode;
            Reset();
        }

        public void SetCurve(int curve)
        {
            this._curve = curves[curve];
            Reset();
        }

        public void SetTime(float time)
        {
            this.time = time;
            Reset();
        }

        public void SetSize(float size)
        {
            this.maxSize = size;
            Reset();
        }

        public void Replay()
        {
            Reset();
        }

        private void Reset()
        {
            transform.position = _startPos;
            _startMat.color = new Color(_startMat.color.r, _startMat.color.g, _startMat.color.b, 1);
            transform.localScale = new Vector3(1, 1, 1);
            _counterA = 0;
            _counterX = 0;
            _counterS = 0;
        }

        private void Move()
        {
            var x = Terp.Interpolate(_startPos.x, newPosition.position.x, _curve, time, ref _counterX);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        private void Alpha()
        {
            var alpha = Terp.Interpolate(_startAlpha, newColor.a, _curve, time, ref _counterA);
            _startMat.color = new Color(_startMat.color.r, _startMat.color.g, _startMat.color.b, alpha);
        }

        private void Resize()
        {
            var size = Terp.Interpolate(1, maxSize, _curve, time, ref _counterS);
            transform.localScale = new Vector3(size, size, size);
        }
    }
}
