
namespace Assets.Scripts.States
{
    public class GameState
    {
        private float timeScore;
        public float TimeScore { get => timeScore; set => timeScore = value > 0 ? value : 0; }
        private int bestTimeScore;
        public int BestTimeScore { get => bestTimeScore; set => bestTimeScore = (int)value > bestTimeScore ? value : bestTimeScore; }
    }
}