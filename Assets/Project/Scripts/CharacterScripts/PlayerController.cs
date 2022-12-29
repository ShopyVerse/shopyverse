using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vitrin.PlayerController
{
    [RequireComponent(typeof (CharacterController))]
    [DisallowMultipleComponent]
    public class PlayerController : MonoBehaviour
    {
        CharacterController _characterController;

        Animator _animator;

        public Button handButton;

        [SerializeField]
        float verticalSpeed;

        [SerializeField]
        float horizontalSpeed;

        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            // handButton = // for mobile
            //     GameObject
            //         .FindGameObjectWithTag("handButton")
            //         .GetComponent<Button>();
            // handButton.onClick.AddListener(() => HiAnimations());
        }

        void Update()
        {
            PlayerAnimations();
            Input_Controller();

            if (Input.GetMouseButtonDown(0))
            {
                if (gameObject.tag == "bowling")
                {
                    Debug.Log("bowling giriş");
                }
            }
        }

        void PlayerAnimations()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _animator.SetTrigger("RaiseHand");
            }
        }

        void Input_Controller()
        {
            float horizontal = SimpleInput.GetAxis("Horizontal");
            float vertical = SimpleInput.GetAxis("Vertical");
            if (
                vertical < 0 && horizontal < -0.1f ||
                horizontal > 0.1f && vertical < 0
            )
            {
                horizontal = 0.1f;
            }

            Vector3 vector =
                transform.right *
                horizontal *
                horizontalSpeed *
                Time.deltaTime +
                transform.forward * vertical * verticalSpeed * Time.deltaTime;
            Movement (vector);

            if (!Input.GetKey(KeyCode.LeftShift))
            {
                vertical = Mathf.Clamp(vertical, -0.5f, 0.5f);
                verticalSpeed = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftShift) && vertical > 0.1f)
            {
                vertical = Mathf.Clamp(vertical, -0.5f, 1f);
                verticalSpeed = 4f;
            }
            AnimatorMovement (horizontal, vertical);
        }

        void Movement(Vector3 vector)
        {
            _characterController.Move (vector);
        }

        void AnimatorMovement(float horizontal, float vertical)
        {
            _animator.SetFloat("Vertical", vertical, 0.06f, Time.deltaTime);
            _animator.SetFloat("Horizontal", horizontal, 0.06f, Time.deltaTime);
        }

        public void HiAnimations() // For Mobile
        {
            _animator.SetTrigger("RaiseHand");
        }
    }
}
