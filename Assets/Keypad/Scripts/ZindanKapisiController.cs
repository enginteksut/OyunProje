using UnityEngine;
using System.Collections;

namespace NavKeypad
{
    public class ZindanKapisiController : MonoBehaviour
    {
        [Header("Kapı Objesi")]
        public GameObject door;

        [Header("Açılma Ayarları")]
        public float openHeight = 3f;
        public float openSpeed = 2f;

        [Header("Kapanma Gecikmesi")]
        public float stayOpenDuration = 3f; // Kapı açıldıktan sonra kaç saniye açık kalsın

        private Vector3 closedPosition;
        private Vector3 openPosition;
        private Vector3 originalScale;
        private Collider doorCollider;
        private bool isOpen = false;
        private bool isMoving = false;

        void Start()
        {
            // Başlangıç pozisyonunu ve ölçeğini kaydet
            closedPosition = door.transform.position;
            openPosition = closedPosition + Vector3.up * openHeight;
            originalScale = door.transform.localScale;

            // Door objesinden Collider’ı al
            doorCollider = door.GetComponent<Collider>();
            if (doorCollider != null)
            {
                doorCollider.isTrigger = false; // Kapı başlangıçta fiziksel engel
            }
        }

        // Bu metodu Keypad'in OnAccessGranted event'ine atayacaksın
        public void OpenDoor()
        {
            if (isOpen || isMoving) return;
            StartCoroutine(OpenRoutine());
        }

        private IEnumerator OpenRoutine()
        {
            isMoving = true;

            // Collider'ı trigger yap: oyuncu içinden geçebilsin
            if (doorCollider != null)
                doorCollider.isTrigger = true;

            // Kapı yukarı hareket ederken ve ölçek küçültülürken
            while (Vector3.Distance(door.transform.position, openPosition) > 0.01f)
            {
                door.transform.position = Vector3.MoveTowards(
                    door.transform.position,
                    openPosition,
                    openSpeed * Time.deltaTime
                );
                door.transform.localScale = Vector3.MoveTowards(
                    door.transform.localScale,
                    new Vector3(originalScale.x, originalScale.y * 0.4f, originalScale.z),
                    openSpeed * Time.deltaTime
                );
                yield return null;
            }

            // Kapı tamamen açıldı
            isOpen = true;
            isMoving = false;

            // Belirli süre açık kalması gerekiyorsa, sadece bekle
            // yield return new WaitForSeconds(stayOpenDuration);

            // Bu satırı kaldırarak kapanmasını engelliyoruz:
            // StartCoroutine(CloseRoutine());
        }


        private IEnumerator CloseRoutine()
        {
            isMoving = true;

            // Kapı aşağı inerken ve ölçek eski haline getirilirken
            while (Vector3.Distance(door.transform.position, closedPosition) > 0.01f)
            {
                door.transform.position = Vector3.MoveTowards(
                    door.transform.position,
                    closedPosition,
                    openSpeed * Time.deltaTime
                );
                door.transform.localScale = Vector3.MoveTowards(
                    door.transform.localScale,
                    originalScale,
                    openSpeed * Time.deltaTime
                );
                yield return null;
            }

            // Kapı tamamen kapandı: tekrar fiziksel engel olsun
            if (doorCollider != null)
                doorCollider.isTrigger = false;

            isOpen = false;
            isMoving = false;
        }
    }
}