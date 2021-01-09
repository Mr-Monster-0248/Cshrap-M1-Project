namespace SharedProject.DTO
{
    public class DirectMessageDto: ICommandDto
    {
        public string Receiver { get; set; }
        public string Text { get; set; }
    }
}