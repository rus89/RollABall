using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace RollABall.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        #region private fields

        // fields
        private Rigidbody rigidbody;

        //NOTE: ako ova promenljiva bude tipa IReadOnlyReactiveProperty<Vector2> onda jednostavno ne dobijamo nikakavo kretanje lopte ako su u jednom trenutku pririsnute dve strelice za kretanje
        //NOTE: kretanje moze da se odvija samo u jednom pravcu
        //NOTE: razlog za ovakvo ponasanje nepoznat
        private IObservable<Vector2> Movement
        {
            get
            {
                return Observable.EveryFixedUpdate()
                    .DistinctUntilChanged()
                    .Select(_ =>
                    {
                        var moveHorizontal = Input.GetAxis(HORIZONTAL_MOVE);
                        var moveVertical = Input.GetAxis(VERTICAL_MOVE);
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

        public IObservable<bool> IsPlayerDead
        {
            get
            {
                return this.OnTriggerEnterAsObservable()
                    .Select(otherCollider => otherCollider.CompareTag("Finish"));
            }
        }

        // serialized
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpFactor;

        // consts
        private const string HORIZONTAL_MOVE = "Horizontal";
        private const string VERTICAL_MOVE = "Vertical";

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