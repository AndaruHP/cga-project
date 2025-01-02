using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;
    [SerializeField] Transform Muzzle;
    public TextMeshProUGUI ammoText;
    float timeSinceLastShot;
    AudioSource m_shootingSound;
    
    private void Awake()
    {
        // Make the Gun persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gunData.currentAmmo = 1; // Reset ammo ke 1 saat memulai scene
        PlayerShoot.shootInput += Shoot;
        UpdateAmmoText();
    }

    private void OnDestroy()
    {
        // Clean up event subscription when destroyed
        PlayerShoot.shootInput -= Shoot;
    }

    private bool CanShoot() => !Pause.paused && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        // Don't shoot if game is paused
        if (Pause.paused) return;

        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(Muzzle.position, Muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);
                }

                gunData.currentAmmo--;
                UpdateAmmoText();
                timeSinceLastShot = 0;
                OnGunShot();

                if (gunData.currentAmmo <= 0)
                {
                    Debug.Log("Out of Ammo!");
                }
            }
        }
        else
        {
            Debug.Log("Cannot shoot. No ammo left.");
        }
    }

    private void Update()
    {
        // Don't update timer if game is paused
        if (!Pause.paused)
        {
            timeSinceLastShot += Time.deltaTime;
            Debug.DrawRay(Muzzle.position, Muzzle.forward);
        }
    }

    private void OnGunShot()
    {
        if (m_shootingSound == null)
        {
            m_shootingSound = GetComponent<AudioSource>();
        }
        m_shootingSound.Play();
    }

    public void AddAmmo(int amount)
    {
        gunData.currentAmmo += amount;
        Debug.Log("Picked up ammo! Current ammo: " + gunData.currentAmmo);
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{gunData.currentAmmo}";
        }
        else
        {
            Debug.LogWarning("Ammo Text is not assigned in the Inspector!");
        }
    }

    public int GetCurrentAmmo()
    {
        return gunData.currentAmmo;
    }
}