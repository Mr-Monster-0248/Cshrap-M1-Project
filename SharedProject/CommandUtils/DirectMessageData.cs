namespace SharedProject.CommandUtils
{
    public class DirectMessageData: ICommandData
    {
        public string Receiver { get; set; }
        public string Text { get; set; }
    }
}