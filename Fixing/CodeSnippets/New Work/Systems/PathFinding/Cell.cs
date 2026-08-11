namespace Systems {
    public class Cell {
        public readonly int x;
        public readonly int y;
        public byte cost;
        public ushort bestCost;
        public GridDirection bestDirection;

        public Cell(int x, int y) {
            this.x = x;
            this.y = y;
            cost = 1;
            bestCost = ushort.MaxValue;
            bestDirection = GridDirection.None;
        }
        public void IncreaseCost(int amount) {
            int newCost = cost + amount;
            cost = newCost >= byte.MaxValue ? byte.MaxValue : (byte)newCost;
        }
    }
}