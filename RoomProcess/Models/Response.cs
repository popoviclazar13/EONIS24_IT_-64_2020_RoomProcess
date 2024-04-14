namespace RoomProcess.Models
{
    public class Response<T> : ResponseNoData
    {
        public T TransferObject { get; set; }

        public Response()
        {
            Status = 200;
        }
    }
    public class ResponseNoData
    {
        public string Message { get; set; }
        public int Status { get; set; }

        public ResponseNoData()
        {
            Status = 200;
            Message = "";
        }
    }

}
