using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomBoardControler : MonoBehaviour
{
    private Camera cam;
    private BottomBoard bottomBoard;
    private MidBoard midBoard;
    private GameManager2 gameManager2;


    void Start()
    {
        cam = Camera.main;
        gameManager2 = FindObjectOfType<GameManager2>();
        CreateBoard();
    }

    // Update is called once per frame
    // Hàm này (có th? là Update()) ch? x? lý vi?c b?m chu?t
    void Update()
    {
        // 1. Ch? ki?m tra khi ng??i dùng NH?N chu?t trái xu?ng
        if (Input.GetMouseButtonDown(0))
        {
            // 2. B?n m?t tia raycast t? camera ??n v? trí chu?t
            var hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            // 3. Ki?m tra xem tia có va trúng collider nào không
            if (hit.collider != null)
            {
                // 4. L?y component 'Cell' t? collider ?ó
                Cell clickedCell = hit.collider.GetComponent<Cell>();

                // 5. N?u ?ó th?c s? là m?t 'Cell'
                if (clickedCell != null && !bottomBoard.losed)
                {
                    Item selectionItem = clickedCell.Item;
                    clickedCell.Clear();
                    bottomBoard.FillToBoard(selectionItem);
                    StartCoroutine(CollapseAndRefillBoard());
                }
            }
        }
    }

    private void CreateBoard() {
        bottomBoard = new BottomBoard(this.transform, gameManager2);
        midBoard = new MidBoard(this.transform);
    }

    public IEnumerator CollapseAndRefillBoard()
    {
        // B??C 1: D?N ITEM C? XU?NG
        // (midBoard là bi?n tham chi?u ??n class MidBoard c?a b?n)
        midBoard.ShiftDownItems();

        // B??C 2: ??I ANIMATION R?I XONG
        // ??i 0.3 giây (b?ng th?i gian DOMove)
        yield return new WaitForSeconds(0.3f);

        // B??C 3: L?P ??Y ITEM M?I VÀO Ô TR?NG
        midBoard.FillGapsWithNewItems();
    }
}
