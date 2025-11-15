## 🚀 Update Contents

* **New Game Level:** Built in the `scene/game2` scene.
* **Skin Change:** Changed item skins to a "Fish" theme (see `prefabs`).
* **Main Scripts Written:**
    * `Game2/GameManager2.cs`: Manages UI (Win/Lose Panel) and scene navigation in Game 2.
    * `MidBoard.cs`: Main script (similar to `Board.cs`) to create the 5x5 game board and initialize items/cells.
    * `BottomBoard.cs`: Creates the 5x1 item holding row at the bottom. Used to hold items interacted with from the `MidBoard`.
    * `BottomBoardControler.cs`: Manages the initialization and spawning of the boards (MidBoard and BottomBoard).

---

## 📊 Task Status

### ✅ Task 1: Skin Change
* **Status:** Completed.
* **Description:** Changed the `Sprite` of item prefabs to apply the new "fish" skin.

### ✅ Task 2: Basic Logic (5x1 Row)
* **Status:** Completed.
* **Description:**
    * Items can now be added from the 5x5 board to the 5x1 row below.
    * Implemented basic scoring mechanic (when `count % 3 == 0`).
    * Added **Auto-Win** and **Auto-Lose** buttons to test scene transitions to `WinPanel` and `LosePanel`.

### ❌ Task 3: Gameplay Improvements
* **Status:** Not completed.
* **Description:** Due to time constraints, gameplay improvements and optimization have not been implemented.

---

## 📝 Lessons Learned

* The project took longer than expected. Due to lack of experience and time pressure, some tasks could not be fully completed.
* Approaching a "perfect" project (complex structure, already optimized) was challenging. Faced difficulties in compiling and understanding the entire existing script structure of the original project.
* **Note:** Due to not fully understanding 100% of the specific requirements, the implementation and gameplay structure may not be exactly as expected.

---

## ℹ️ Additional Information

Feel free to ask for a demo (video/gif) or further explanation of the code structure if needed.
