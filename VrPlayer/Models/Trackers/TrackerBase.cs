﻿using System.Windows.Media.Media3D;

using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Trackers
{
    public abstract class TrackerBase: ViewModelBase
    {
        protected Vector3D RawPosition;
        protected Quaternion RawRotation;

        private Vector3D _position;
        public Vector3D Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged("Position");
            }
        }

        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                OnPropertyChanged("Rotation");
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }

        private double _positionScaleFactor;
        public double PositionScaleFactor
        {
            get
            {
                return _positionScaleFactor;
            }
            set
            {
                _positionScaleFactor = value;
                OnPropertyChanged("PositionScaleFactor");
            }
        }

        private Vector3D _basePosition;
        public Vector3D BasePosition
        {
            get
            {
                return _basePosition;
            }
            set
            {
                _basePosition = value;
                OnPropertyChanged("BasePosition");
            }
        }

        private Vector3D _positionOffset;
        public Vector3D PositionOffset
        {
            get
            {
                return _positionOffset;
            }
            set
            {
                _positionOffset = value;
                OnPropertyChanged("PositionOffset");
            }
        }

        private Quaternion _baseRotation;
        public Quaternion BaseRotation
        {
            get
            {
                return _baseRotation;
            }
            set
            {
                _baseRotation = value;
                OnPropertyChanged("BaseRotation");
            }
        }

        private Quaternion _rotationOffset;
        public Quaternion RotationOffset
        {
            get
            {
                return _rotationOffset;
            }
            set
            {
                _rotationOffset = value;
                OnPropertyChanged("RotationOffset");
            }
        }

        protected void UpdatePositionAndRotation()
        {
            Rotation = BaseRotation * RawRotation * _rotationOffset;
            Vector3D relativePos = BasePosition + RawPosition;
            Matrix3D m = Matrix3D.Identity;
            m.Rotate(BaseRotation);
            m.Translate(relativePos);
            m.Rotate(BaseRotation);
            this.Position = new Vector3D(m.OffsetX, m.OffsetY, m.OffsetZ);
        }

        public void Calibrate()
        {
            Quaternion conjugate = new Quaternion(RawRotation.X, RawRotation.Y, RawRotation.Z, RawRotation.W) * _rotationOffset;
            conjugate.Conjugate();
            BaseRotation = conjugate;
            BasePosition = -RawPosition + _positionOffset;
        }

        public abstract void Dispose();
    }
}
