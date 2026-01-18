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
    private bool _isDead = false; // Bloqueo de seguridad

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

        if (_photonView != null && _photonView.IsMine)
        {
            CamaraSeguimiento scriptCamara = Camera.main.GetComponent<CamaraSeguimiento>();
            if (scriptCamara != null) scriptCamara.objetivo = this.transform;
        }
        else
        {
            AudioListener listener = GetComponentInChildren<AudioListener>();
            if (listener != null) Destroy(listener);
        }

        _audioSource = GetComponent<AudioSource>();
        var canvas = GameObject.Find("Canvas");
        if (canvas != null) _uiManager = canvas.GetComponent<UIManager>();

        if (_playerModel != null) _anim = _playerModel.GetComponent<Animator>();
        else _anim = GetComponentInChildren<Animator>();

        if (_uiManager != null)
        {
            _uiManager.UpdateLivesDisplay(_lives);
            _uiManager.UpdateCoinDisplay(_coins);
        }
    }

    void Update()
    {
        if (_photonView != null && !_photonView.IsMine) return;
        if (_isDead) return; // Si murió, no hace nada más

        CalculateMovement();

        // RESPAWN SÓLO SI CAE
        if (this.transform.position.y <= -8f)
        {
            Respawn();
        }
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
                _yVelocity = _jumpSpeed;
                _canDoubleJump = true;
                if (_anim != null) _anim.SetBool("estaSaltando", true);
                if (_audioSource != null && _sonidoSalto != null) _audioSource.PlayOneShot(_sonidoSalto);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canDoubleJump)
            {
                _yVelocity = _jumpSpeed;
                _canDoubleJump = false;
                if (_audioSource != null && _sonidoSalto != null) _audioSource.PlayOneShot(_sonidoSalto);
            }
            _yVelocity -= _gravity * Time.deltaTime;
        }

        velocity.y = _yVelocity;
        _controller.Move(velocity * Time.deltaTime);
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
            _lives = 0;
            if (_uiManager != null) _uiManager.MostrarGameOver();

            _controller.enabled = false;

            // SOLUCIÓN AQUÍ: Desactivar el modelo o el objeto completo
            if (_playerModel != null)
                _playerModel.SetActive(false);
            else
                this.gameObject.SetActive(false); // Opcional: Desactiva todo el script/objeto

            Debug.Log("Juego Terminado");
        }
    }

    public void AddCoin()
    {
        if (_isDead) return;
        _coins++;
        if (_uiManager != null) _uiManager.UpdateCoinDisplay(_coins);
    }
}