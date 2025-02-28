using UnityEngine;

namespace Helpers
{
    public class LevelLoader
    {
        private static string darkCellPath = "Prefabs/CellDark";
        private static string lightCellPath = "Prefabs/CellLight";

        public LevelLoader(Grid grid, Transform parent)
        {
            LoadLevel(grid, parent);
        }

        private void LoadLevel(Grid grid, Transform parent)
        {
            var width = grid.Width;
            var height = grid.Height;
            var lightPrefabInstance = Resources.Load<BaseCell>(lightCellPath);
            var darkPrefabInstance = Resources.Load<BaseCell>(darkCellPath);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var prefabToUse = j % 2 == 0 ? lightPrefabInstance : darkPrefabInstance;
                    var cell = Object.Instantiate(prefabToUse, parent);
                    cell.ConfigureSelf(i, j);
                }
            }
        } 
    }
}
