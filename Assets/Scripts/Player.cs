using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private Vector3 moveDir = Vector3.zero;

    [Header("Configuración de Movimiento")]
    [SerializeField] private float _speed = 6.0f;
    [SerializeField] private float _gravity = 40.0f;
    [SerializeField] private float _jumpSpeed = 12.0f;

    private float _yVelocity;
    private bool _canDoubleJump = false;
    private bool _isDead = false;

    [Header("Estadísticas y UI")]
    [SerializeField] private int _coins = 0;
    [SerializeField] private int _lives = 3;
    private UIManager _uiManager;

    [Header("Referencias de Animación")]
    private Animator _anim;
    [SerializeField] private GameObject _playerModel;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _sonidoSalto;
    private PhotonView _photonView;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _photonView = GetComponent<PhotonView>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();

        var canvas = GameObject.Find("Canvas");
        if (canvas != null) _uiManager = canvas.GetComponent<UIManager>();
        _anim = GetComponentInChildren<Animator>();

        if (_photonView != null && _photonView.IsMine)
        {
            CamaraSeguimiento scriptCamara = Camera.main.GetComponent<CamaraSeguimiento>();
            if (scriptCamara != null) scriptCamara.objetivo = this.transform;

            if (_uiManager != null)
            {
                _uiManager.UpdateLivesDisplay(_lives);
                _uiManager.UpdateCoinDisplay(_coins);
            }
        }
        else
        {
            AudioListener listener = GetComponentInChildren<AudioListener>();
            if (listener != null) Destroy(listener);
        }
    }

    void Update()
    {
        if (_photonView != null && !_photonView.IsMine) return;
        if (_isDead) return;

        CalculateMovement();

        if (this.transform.position.y <= -8f) Respawn();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDir = new Vector3(horizontalInput, 0, 0);
        Vector3 velocity = moveDir * _speed;

        if (_anim != null) _anim.SetFloat("velocidad", Mathf.Abs(horizontalInput));

        if (horizontalInput != 0 && _playerModel != null)
        {
            _playerModel.transform.localEulerAngles = new Vector3(0, (horizontalInput > 0) ? 90f : 270f, 0);
        }

        if (_controller.isGrounded)
        {
            if (_anim != null) _anim.SetBool("estaSaltando", false);
            _yVelocity = -1.0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
                _canDoubleJump = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump)
            {
                Jump();
                _canDoubleJump = false;
            }
            _yVelocity -= _gravity * Time.deltaTime;
        }

        velocity.y = _yVelocity;
        _controller.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        _yVelocity = _jumpSpeed;
        if (_anim != null) _anim.SetBool("estaSaltando", true);
        if (_audioSource != null && _sonidoSalto != null) _audioSource.PlayOneShot(_sonidoSalto);
    }

    public void Respawn()
    {
        if (_isDead) return;

        _lives--;
        if (_uiManager != null) _uiManager.UpdateLivesDisplay(_lives);

        if (_lives > 0)
        {
            _controller.enabled = false;
            this.transform.position = new Vector3(-4.65f, 3.5f, 0);
            _yVelocity = 0;
            _controller.enabled = true;
        }
        else
        {
            _isDead = true;
            if (_uiManager != null) _uiManager.MostrarGameOver();
            _controller.enabled = false;

            if (_photonView.IsMine)
            {
                // Destrucción inmediata para liberar la red
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }

    public void AddCoin()
    {
        // Solo sumamos si somos el dueño (independencia de puntaje)
        if (_isDead || !_photonView.IsMine) return;

        _coins++;
        if (_uiManager != null) _uiManager.UpdateCoinDisplay(_coins);
    }
}