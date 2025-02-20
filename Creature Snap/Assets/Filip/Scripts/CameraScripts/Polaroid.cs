using UnityEngine;

public class Polaroid : MonoBehaviour
{
    public GameObject photoPrefab = null;
    public MeshRenderer screenRenderer = null;
    public Transform spawnLocation = null;
    private Camera renderCamera = null;

    private bool camOn = false;
    private int grabCount = 0;
    public PlayQuickSound playSound;
    private Photo lastPhoto = null;
    private float timer = 0;

    private void Awake()
    {
        renderCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        CreateRenderTexture();
        TurnOff();
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void CreateRenderTexture()
    {
        RenderTexture newTexture = new RenderTexture(256, 256, 32, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
        newTexture.antiAliasing = 4;

        renderCamera.targetTexture = newTexture;
        screenRenderer.material.mainTexture = newTexture;
    }

    public void TakePhoto()
    {
        if (!camOn || timer < 1.5f)
            return;
        timer = 0;
        if (lastPhoto != null)
        {
            lastPhoto.GrabPhoto();
            lastPhoto.ReleasePhoto();
        }
        playSound.Play();
        Photo newPhoto = CreatePhoto();
        lastPhoto = newPhoto;
        SetPhotoImage(newPhoto);
    }

    private Photo CreatePhoto()
    {
        GameObject photoObject = Instantiate(photoPrefab, spawnLocation.position, spawnLocation.rotation, transform);
        return photoObject.GetComponent<Photo>();
    }

    private void SetPhotoImage(Photo photo)
    {
        Texture2D newTexture = RenderCameraToTexture(renderCamera);
        photo.SetImage(newTexture);
    }

    private Texture2D RenderCameraToTexture(Camera camera)
    {
        camera.Render();
        RenderTexture.active = camera.targetTexture;

        Texture2D photo = new Texture2D(256, 256, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        photo.Apply();

        return photo;
    }
    public void TurnOn()
    {
        grabCount++;
        if (grabCount > 1)
        {
            camOn = true;
            renderCamera.enabled = true;
            screenRenderer.material.color = Color.white;
        }
    }

    public void TurnOff()
    {
        grabCount--;
        if (grabCount < 2)
        {
            camOn = false;
            renderCamera.enabled = false;
            screenRenderer.material.color = Color.black;
        }
    }
}
