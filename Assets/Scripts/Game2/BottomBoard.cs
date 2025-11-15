using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BottomBoard
{
    // Kích thước cố định của bảng
    private int boardSizeX = 5;
    private int boardSizeY = 1;

    // Mảng 2D để chứa các ô, vẫn dùng 2D để tương thích
    private Cell[,] m_cells;

    // Transform gốc để chứa các ô
    private Transform m_root;

    public bool losed;

    private GameManager2 my_gamemanager;

    public BottomBoard(Transform transform, GameManager2 gameManager2)
    {
        m_root = transform;

        // Khởi tạo mảng m_cells với kích thước cố định
        m_cells = new Cell[boardSizeX, boardSizeY];
        my_gamemanager =  gameManager2;

        // Gọi hàm tạo bảng
        CreateBoard();
    }

    private void CreateBoard()
    {
        // Tính toán vị trí gốc để căn giữa bảng
        // Với 5x1, origin sẽ là (-2.0, 0, 0)
        Vector3 origin = new Vector3(-boardSizeX * 0.5f + 0.5f, -boardSizeY * 0.5f + 0.5f, 0f);

        // Tải prefab (giả định Constants.PREFAB_CELL_BACKGROUND tồn tại)
        GameObject prefabBG = Resources.Load<GameObject>(Constants.PREFAB_CELL_BACKGROUND);

        // Vòng 1: Tạo và đặt vị trí các ô
        for (int x = 0; x < boardSizeX; x++) // x chạy từ 0 đến 4
        {
            for (int y = 0; y < boardSizeY; y++) // y chỉ chạy giá trị 0
            {
                // Tạo GameObject từ prefab
                GameObject go = GameObject.Instantiate(prefabBG);

                // Đặt vị trí (sẽ tạo thành một hàng ngang)
                go.transform.SetParent(m_root);
                go.transform.localPosition = origin + new Vector3(x, y, 0f); 

                // Lấy component Cell và gán tọa độ (index)
                Cell cell = go.GetComponent<Cell>();
                cell.Setup(x, y);

                // Lưu cell vào mảng
                m_cells[x, y] = cell;
            }
        }
    }

    public void FillToBoard(Item selectedItem)
    {
        if (losed) return;

        int insertPosition = FindInsertPosition(selectedItem);

        if (insertPosition == -1)
        {
            // Board đầy, game over
            Debug.Log("Thua - Board đã đầy");
            losed = true;
            return;
        }

        // Dồn các item sang phải để tạo chỗ trống tại insertPosition
        ShiftItemsRight(insertPosition);

        // Đặt item mới vào vị trí tìm được
        PlaceItemAt(selectedItem, insertPosition);

        // Kiểm tra match-3
        CheckAndMatchItems(selectedItem);

        // Kiểm tra điều kiện thua (ô cuối cùng có item)
        if (m_cells[boardSizeX - 1, 0].Item != null)
        {
            losed = true;
            my_gamemanager.DestroyController();
            my_gamemanager.DestroyAllItemsByLayer();
            my_gamemanager.Losed();
        }
    }

    private int FindInsertPosition(Item selectedItem)
    {
        // Vị trí của item CÙNG LOẠI cuối cùng tìm thấy
        int lastSameTypePos = -1;

        // Vị trí của ô TRỐNG đầu tiên tìm thấy
        int firstEmptyPos = -1;

        for (int x = 0; x < boardSizeX; x++)
        {
            Cell cell = m_cells[x, 0];

            if (cell.Item == null)
            {
                // Tìm thấy ô trống
                if (firstEmptyPos == -1) // Chỉ lưu ô trống ĐẦU TIÊN
                {
                    firstEmptyPos = x;
                }
            }
            else if (cell.Item.IsSameType(selectedItem))
            {
                // Tìm thấy item cùng loại,
                // cập nhật vị trí "cuối cùng" tìm thấy là vị trí NÀY
                lastSameTypePos = x;
            }
        }

        // --- Quyết định vị trí chèn ---

        // ƯU TIÊN 1: Đã tìm thấy ít nhất 1 item cùng loại
        if (lastSameTypePos != -1)
        {
            // Vị trí chèn mong muốn là "ngay sau" item cuối cùng
            int desiredPos = lastSameTypePos + 1;

            // Kiểm tra xem vị trí "ngay sau" có vượt quá mảng không
            if (desiredPos < boardSizeX)
            {
                return desiredPos;
            }
            // Nếu vượt quá (tức là item cùng loại nằm ở ô cuối cùng),
            // chúng ta không thể chèn "ngay sau" nó.
        }

        if (firstEmptyPos != -1)
        {
            return firstEmptyPos;
        }

        // Không tìm thấy item cùng loại VÀ không có ô trống
        return -1; // Board đầy
    }

    private void ShiftItemsRight(int fromPosition)
    {
        // Dồn tất cả items từ vị trí fromPosition trở đi sang phải 1 ô
        for (int x = boardSizeX - 1; x > fromPosition; x--)
        {
            Cell currentCell = m_cells[x - 1, 0];
            Cell nextCell = m_cells[x, 0];

            if (currentCell.Item != null)
            {
                nextCell.Assign(currentCell.Item);
                nextCell.Item.View.DOMove(nextCell.transform.position, 0.2f);
                currentCell.Free(); // Xóa item khỏi ô cũ
            }
        }
    }

    private void PlaceItemAt(Item item, int position)
    {
        Cell targetCell = m_cells[position, 0];
        targetCell.Assign(item);
        item.SetView();
        item.View.DOMove(targetCell.transform.position, 0.2f);
    }

    private void CheckAndMatchItems(Item selectedItem)
    {
        // Đếm số lượng item cùng loại liên tiếp
        int matchCount = 0;

        for (int x = 0; x < boardSizeX; x++)
        {
            Cell cell = m_cells[x, 0];
            if (cell.Item != null && cell.Item.IsSameType(selectedItem))
                matchCount++;
        }

        // Nếu có bội số của 3, kích hoạt match
        if (matchCount > 0 && matchCount % 3 == 0)
        {
            MatchedItem(selectedItem);
        }
    }

    private void MatchedItem(Item itemToMatch)
    {
        if (itemToMatch == null)
        {
            Debug.LogError("itemToMatch is null!");
            return;
        }

        // --- GIAI ĐOẠN 1: XÓA CÁC ITEM KHỚP ---
        List<Item> itemsToExplode = new List<Item>();

        for (int x = 0; x < boardSizeX; x++)
        {
            Cell currentCell = m_cells[x, 0];

            if (currentCell.Item != null && currentCell.Item.IsSameType(itemToMatch))
            {
                itemsToExplode.Add(currentCell.Item);
                currentCell.Free();
            }
        }

        // Explode SAU khi đã Free() tất cả
        foreach (Item item in itemsToExplode)
        {
            if (item != null)
            {
                item.ExplodeView(); // Animation + Destroy
            }
        }

        // --- GIAI ĐOẠN 2: DỒN CÁC ITEM CÒN LẠI ---
        int writeIndex = 0;

        for (int readIndex = 0; readIndex < boardSizeX; readIndex++)
        {
            Cell sourceCell = m_cells[readIndex, 0];

            if (sourceCell.Item != null)
            {
                if (writeIndex != readIndex)
                {
                    Cell destinationCell = m_cells[writeIndex, 0];
                    Item itemToMove = sourceCell.Item;

                    // Di chuyển item
                    destinationCell.Assign(itemToMove);
                    sourceCell.Free(); 

                    // Animation với vị trí đích rõ ràng
                    itemToMove.View.DOMove(destinationCell.transform.position, 0.2f);
                }
                writeIndex++;
            }
        }
    }

    private IEnumerator WaitTwoSeconds()
    {
        // Dòng này sẽ tạm dừng coroutine trong 2.0 giây
        yield return new WaitForSeconds(2.0f);
    }
}
