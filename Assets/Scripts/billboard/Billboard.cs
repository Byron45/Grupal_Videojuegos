using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    private Transform _camTransform;
    [SerializeField] private Text _nombreTexto;
    private PhotonView _pv;

    void Start()
    {
        _camTransform = Camera.main.transform;
        _pv = GetComponentInParent<PhotonView>();

        if (_pv != null && _nombreTexto != null)
        {
            // Esto asigna el nombre que Photon ya conoce de ese jugador
            _nombreTexto.text = _pv.Owner.NickName;

            // Opcional: El tuyo en verde, los demás en blanco
            _nombreTexto.color = _pv.IsMine ? Color.green : Color.white;
        }
    }

    void LateUpdate()
    {
        // Si por alguna razón la cámara se pierde al reiniciar, la buscamos de nuevo
        if (_camTransform == null && Camera.main != null)
            _camTransform = Camera.main.transform;

        if (_camTransform != null)
            transform.LookAt(transform.position + _camTransform.forward);
    }
}