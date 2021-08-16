using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace RollABall.Scripts
{
    public class Player : MonoBehaviour
    {
        #region private fields

        /// <summary>
        /// Non-Serializable fields
        /// </summary>
        private Rigidbody rigidbody;

        /// <summary>
        /// Serializable fields
        /// </summary>
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpFactor;

        private IObservable<Vector2> Movement
        {
            get
            {
                return Observable.EveryFixedUpdate()
                    .DistinctUntilChanged()
                    .Select(_ =>
                    {
                        var moveHorizontal = Input.GetAxis(Constants.Simulation.HORIZONTAL_MOVE);
                        var moveVertical = Input.GetAxis(Constants.Simulation.VERTICAL_MOVE);
                        return new Vector2(moveHorizontal, moveVertical).normalized;
                    });
            }
        }

        private IObservable<long> Jump
        {
            get
            {
                return Observable.EveryFixedUpdate()
                    .DistinctUntilChanged()
                    .Where(_ => Input.GetKeyDown(KeyCode.Space));
            }
        }

        private IObservable<bool> IsPlayerDead
        {
            get
            {
                return this.OnTriggerEnterAsObservable()
                    .Select(otherCollider =>
                    {
                        Debug.LogFormat($"{otherCollider.name}");
                        return otherCollider.CompareTag(Constants.Tags.FINISH);
                    });
            }
        }

        #endregion


        #region Monobehaviour Events

        // Start is called before the first frame update
        private void Start()
        {
            rigidbody = transform.GetComponent<Rigidbody>();

            Movement.Subscribe(vector =>
            {
                Vector3 movement = new Vector3(vector.x, 0f, vector.y);
                rigidbody.AddForce(movement * _speed);
            }).AddTo(this);

            Jump.Subscribe(_ =>
            {
                Vector3 jumpMove = new Vector3(0f, 2, 0f);
                rigidbody.AddForce(jumpMove * _jumpFactor);
            }).AddTo(this);

            IsPlayerDead.Subscribe(_ => { Debug.LogError("DEAD"); })
                .AddTo(this);
        }

        #endregion
    }
}