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
        //Dodato da bi znali koja je uloga!
        public int Role { get; set; }
        //Dodato da bi znali koji korisnik je ulogovan
        public string Name { get; set; }

        public ResponseNoData()
        {
            Status = 200;
            Message = "";
        }
    }

}
