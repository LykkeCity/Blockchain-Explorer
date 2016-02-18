using System;
using System.IO;
using System.Threading.Tasks;

namespace TcpSockets
{


    /// <summary>
    /// Даём интерфейс службам, в которых хотим обработать момент коннекта и дисконнекта
    /// </summary>
    public interface ISocketNotifyer
    {
        Task Connect();
        Task Disconnect();
    }

    /// <summary>
    /// Интерфейс, который выдается сериалайзеру данных
    /// </summary>
    public interface ITcpSerializer
    {
        Task<Tuple<object,int>> Deserialize(Stream stream);
        byte[] Serialize(object data);
    }


    /// <summary>
    /// Интерфейс, с помощью которого мы уведомляем класс потребителя информации о поступлении данных 
    /// и даём интерфейс (SendDataToSocket) на обратную отправку данных
    /// </summary>
    public interface ITcpService
    {
        /// <summary>
        /// Когда сокет получил данные и распарсил их в объект, 
        /// вызывается этот метод, в котором мы обрабатываем полученные данные
        /// </summary>
        /// <param name="data">данные, которые получил сокет и распарсил биндер</param>
        Task HandleDataFromSocket(object data);

        /// <summary>
        /// Метод, с помощью которого мы отправляем данные в сокет
        /// </summary>

        Func<object, Task> SendDataToSocket { get; set; }

        /// <summary>
        /// Имя контекста сокета (для логирования)
        /// </summary>
        string ContextName { get; }

    }

    public interface ITcpClientService : ITcpService
    {
        object GetPingData();
    }


}


