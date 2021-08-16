using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace RollABall.Scripts
{
    public class CameraController : MonoBehaviour
    {
        #region private fields

        // fields
        private Vector3 offset;

        // serialized
        [SerializeField] private GameObject player;

        #endregion


        #region Monobehaviour Events

        // Start is called before the first frame update
        private void Start()
        {
            offset = transform.position - player.transform.position;
            Observable.EveryLateUpdate()
                .Subscribe(OnLateUpdateListener)
                .AddTo(this);
        }

        #endregion


        #region UniRx Listeners
        
        private void OnLateUpdateListener(long obj)
        {
            transform.position = player.transform.position + offset;
        }

        #endregion
    }
}