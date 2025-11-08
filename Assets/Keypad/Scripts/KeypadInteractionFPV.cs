using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class KeypadInteractionFPV : MonoBehaviour
    {
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
            if (cam == null)
                Debug.LogError("Ana kamera bulunamadı! Tag'in 'MainCamera' olduğundan emin ol.");
        }

        private void Update()
        {
            // Sol tık algılandı mı?
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse sol tıklandı.");

                var ray = cam.ScreenPointToRay(Input.mousePosition);

                // Işın bir objeye çarptı mı?
                if (Physics.Raycast(ray, out var hit))
                {
                    Debug.Log("Raycast çarptı -> Obje: " + hit.collider.name);

                    // Obje üzerinde KeypadButton var mı?
                    if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                    {
                        Debug.Log("KeypadButton bulundu, butona basılıyor...");
                        keypadButton.PressButton();
                    }
                    else
                    {
                        Debug.Log("KeypadButton component bulunamadı.");
                    }
                }
                else
                {
                    Debug.Log("Raycast hiçbir objeye çarpmadı.");
                }
            }
        }
    }
}
