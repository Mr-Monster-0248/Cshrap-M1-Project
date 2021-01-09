namespace SharedProject.DTO
{
    public class DirectMessageDto: CommandDto
    {
        public string Receiver { get; set; }
        public string Text { get; set; }
    }
}