using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace RollABall.Scripts
{
    public sealed class Camera : MonoBehaviour
    {
        #region private fields

        /// <summary>
        /// Non-Serializable fields
        /// </summary>
        private Vector3 _offset;

        /// <summary>
        /// Serializable fields
        /// </summary>
        [SerializeField] private GameObject _player;

        #endregion


        
        
        
        #region Monobehaviour Events

#if UNITY_EDITOR || UNITY_EDITOR_64 || UNITY_EDITOR_OSX
        private void OnValidate()
        {
            _player = GameObject.FindWithTag(Constants.Tags.PLAYER);
        }
#endif

        // Start is called before the first frame update
        private void Start()
        {
            _offset = transform.position - _player.transform.position;
            this.LateUpdateAsObservable()
                .Subscribe(_ => OnLateUpdateListener())
                .AddTo(this);
        }

        #endregion

        
        
        

        #region UniRx Listeners
        
        private void OnLateUpdateListener()
        {
            transform.position = _player.transform.position + _offset;
        }

        #endregion
    }
}