namespace ERPServices.MessageBus
{
    public class BaseMessage
    {
        public string UUId { get; set; }
        public DateTime MessageCreated { get; set; }
        public BaseMessage()
        {
            MessageCreated = DateTime.Now;
            UUId = Guid.NewGuid().ToString();

        }
    }
}
