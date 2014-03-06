namespace MultiplayerWebChess.Models
{
    public enum ColorSelect
    {
        Auto, White, Black
    }

    public class CreateGameVM
    {
        public string Description { get; set; }

        public ColorSelect ColorSelect { get; set; }
    }
}