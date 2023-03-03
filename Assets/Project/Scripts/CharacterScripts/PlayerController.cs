using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vitrin.PlayerController
{
    [RequireComponent(typeof(CharacterController))]
    [DisallowMultipleComponent]
    public class PlayerController : MonoBehaviour
    {
        CharacterController _characterController;

        Animator _animator;

        public Button handButton;
        //camera
        private Transform cam;

        [SerializeField]
        float verticalSpeed;

        [SerializeField]
        float horizontalSpeed;
        //turns
        public float turnSmoothTime = 0.1f;
        float turnSmoothVelocity = 0.1f;

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
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

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
            //test
            if (direction.magnitude >= 0.1f)
                Movement(direction);

            AnimatorMovement(horizontal, vertical);
        }

        void Movement(Vector3 direction)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            _characterController.Move(moveDir.normalized * verticalSpeed * Time.deltaTime);

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

        public void SetCam(Transform cam)
        {
            this.cam = cam;
        }
    }
}
