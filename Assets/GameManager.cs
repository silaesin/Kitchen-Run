using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    [Header("Oyun Ayarları")]
    public int levelNumarasi;
    public int toplanmasiGerekenSayi = 0; 
    public int hakSayisi = 3; 

    [Header("Süre Ayarları")]
    public float toplamSure = 60f;
    private float kalanSure;
    private bool oyunBittiMi = false;

    [Header("Ekran Bağlantıları")]
    public GameObject puanObjesi;      
    public GameObject sureObjesi;      
    public GameObject hakObjesi;       
    public GameObject levelObjesi;     
    
    [Header("Paneller")]
    public GameObject kaybetmePaneli;  
    public GameObject kaybetmeYazisi;  
    
    public GameObject kazanmaPaneli;   
    public GameObject kazanmaYazisi;   
    public GameObject kekResmi;        

    private int mevcutPuan = 0;

    void Start()
    {
        // HER SAHNE BAŞLADIĞINDA ZAMANI TEKRAR AKIT (Çok Önemli!)
        Time.timeScale = 1f; 

        // Level Tespiti
        int sahneIndex = SceneManager.GetActiveScene().buildIndex;
        levelNumarasi = sahneIndex + 1; 

        HedefSayiyiBelirle();

        kalanSure = toplamSure;
        
        if(kaybetmePaneli != null) kaybetmePaneli.SetActive(false);
        if(kazanmaPaneli != null) kazanmaPaneli.SetActive(false);

        EkranlariGuncelle();
    }

    // --- BUTONLAR İÇİN YENİ FONKSİYONLAR ---
    
    // "Yeniden Dene" butonuna bunu vereceğiz
    public void TekrarOyna()
    {
        // Şu anki sahneyi baştan yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // "Sonraki Level" butonuna bunu vereceğiz
    public void SonrakiLeveleGec()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void HedefSayiyiBelirle()
    {
        toplanmasiGerekenSayi = 0; 
        if (levelNumarasi == 1)
        {
            toplanmasiGerekenSayi += GameObject.FindGameObjectsWithTag("Yumurta").Length;
            toplanmasiGerekenSayi += GameObject.FindGameObjectsWithTag("Seker").Length;
        }
        else if (levelNumarasi == 2)
        {
            toplanmasiGerekenSayi += GameObject.FindGameObjectsWithTag("Yag").Length;
            toplanmasiGerekenSayi += GameObject.FindGameObjectsWithTag("Sut").Length;
        }
        else if (levelNumarasi == 3)
        {
            toplanmasiGerekenSayi += GameObject.FindGameObjectsWithTag("Un").Length;
            toplanmasiGerekenSayi += GameObject.FindGameObjectsWithTag("KabartmaTozu").Length;
        }
    }

    void Update()
    {
        if (oyunBittiMi) return;

        if (kalanSure > 0)
        {
            kalanSure -= Time.deltaTime;
            YaziGuncelle(sureObjesi, "Süre: " + Mathf.CeilToInt(kalanSure).ToString());
        }
        else
        {
            OyunBitti(false, "SÜRE"); 
        }
    }

    public void MalzemeToplandi(string gelenTag)
    {
        if (oyunBittiMi) return;
        bool dogruMalzeme = false;

        if (levelNumarasi == 1 && (gelenTag == "Yumurta" || gelenTag == "Seker")) dogruMalzeme = true;
        else if (levelNumarasi == 2 && (gelenTag == "Yag" || gelenTag == "Sut")) dogruMalzeme = true;
        else if (levelNumarasi == 3 && (gelenTag == "Un" || gelenTag == "KabartmaTozu")) dogruMalzeme = true;

        if (dogruMalzeme)
        {
            mevcutPuan++;
        }
        else
        {
            hakSayisi--; 
            if (hakSayisi <= 0) OyunBitti(false, "HAK"); 
        }
        EkranlariGuncelle();
    }

    public void FirinaGidildi()
    {
        if (mevcutPuan >= toplanmasiGerekenSayi)
        {
            OyunBitti(true, "KAZANDI"); 
        }
    }

    void OyunBitti(bool kazandi, string neden)
    {
        oyunBittiMi = true;
        Time.timeScale = 0f; // Oyunu durdur

        if (kazandi)
        {
            if(kazanmaPaneli != null) kazanmaPaneli.SetActive(true);
            
            if (levelNumarasi == 3)
            {
                YaziGuncelle(kazanmaYazisi, "TEBRİKLER!\nKEKİN PİŞTİ, AFİYET OLSUN!");
                if (kekResmi != null) kekResmi.SetActive(true); 
            }
            else
            {
                YaziGuncelle(kazanmaYazisi, "TEBRİKLER!\nBU AŞAMA TAMAMLANDI.");
                if (kekResmi != null) kekResmi.SetActive(false); 
            }
        }
        else 
        {
            if(kaybetmePaneli != null) kaybetmePaneli.SetActive(true);

            if (neden == "SÜRE")
                YaziGuncelle(kaybetmeYazisi, "SÜRE DOLDU!\nDAHA HIZLI OLMALISIN.");
            else if (neden == "HAK")
                YaziGuncelle(kaybetmeYazisi, "YANLIŞ MALZEME!\nÇOK FAZLA HATA YAPTIN.");
        }
    }

    void EkranlariGuncelle()
    {
        YaziGuncelle(puanObjesi, "Malzemeler: " + mevcutPuan + " / " + toplanmasiGerekenSayi);
        YaziGuncelle(hakObjesi, "Hak: " + hakSayisi);
        YaziGuncelle(levelObjesi, "Level: " + levelNumarasi);
    }

    void YaziGuncelle(GameObject yaziObjesi, string metin)
    {
        if (yaziObjesi == null) return;
        if (yaziObjesi.GetComponent<TextMeshProUGUI>())
            yaziObjesi.GetComponent<TextMeshProUGUI>().text = metin;
        else if (yaziObjesi.GetComponent<Text>())
            yaziObjesi.GetComponent<Text>().text = metin;
    }
}