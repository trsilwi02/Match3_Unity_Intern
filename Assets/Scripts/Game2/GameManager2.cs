using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager2 : MonoBehaviour
{
    [SerializeField] public GameObject HomePanel;
    [SerializeField] public GameObject losePanel;
    [SerializeField] public GameObject wonPanel;

    private BottomBoard bottomBoard;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Losed()
    {
        losePanel.SetActive(true);
        wonPanel.SetActive(false);
        HomePanel.SetActive(false);
    }

    public void Home()
    {
        losePanel.SetActive(false);
        wonPanel.SetActive(false);
        HomePanel.SetActive(true);
    }

    public void Won()
    {
        losePanel.SetActive(false);
        wonPanel.SetActive(true);
        HomePanel.SetActive(false);
    }

    public void Play()
    {
        losePanel.SetActive(false);
        wonPanel.SetActive(false);
        HomePanel.SetActive(false);
        CreateBoard();
    }

    public void CreateBoard()
    {
        // 1. T?o m?t GameObject m?i
        // B?n có th? ??t tên cho nó, ví d?: "BottomBoardObject"
        GameObject boardObject = new GameObject("BottomBoardController");

        // 3. ??t v? trí c?a nó th?p h?n 4 ??n v? so v?i cha (this)
        // (0, -4, 0) là v? trí t??ng ??i (localPosition) so v?i 'this.transform'
        boardObject.transform.localPosition = new Vector3(0, -4, 0);

        boardObject.AddComponent<BottomBoardControler>();
    }

    public void DestroyController()
    {
        BottomBoardControler objectToDestroy = FindObjectOfType<BottomBoardControler>();

        // 2. Ki?m tra xem có tìm th?y không (r?t quan tr?ng)
        if (objectToDestroy != null)
        {
            // 3. H?y toàn b? GameObject ch?a component ?ó
            Destroy(objectToDestroy.gameObject);
        }
        else
        {
            Debug.LogWarning("Không tìm th?y ??i t??ng nào có component BR ?? h?y.");
        }
    }

    public void DestroyAllItemsByLayer()
    {
        // 1. Tìm t?t c? các SpriteRenderer trong Scene
        // (N?u b?n dùng 3D/Mesh, hãy ??i 'SpriteRenderer' thành 'Renderer')
        SpriteRenderer[] allRenderers = FindObjectsOfType<SpriteRenderer>();

        Debug.Log("Tìm th?y " + allRenderers.Length + " renderers. ?ang ki?m tra sorting layer...");

        // 2. Duy?t qua t?ng cái
        foreach (SpriteRenderer renderer in allRenderers)
        {
            // 3. Ki?m tra xem tên sorting layer có ph?i là "Item" không
            if (renderer.sortingLayerName == "items")
            {
                // 4. N?u ?úng, H?Y toàn b? GameObject ch?a nó
                Destroy(renderer.gameObject);
            }
        }
    }
}
