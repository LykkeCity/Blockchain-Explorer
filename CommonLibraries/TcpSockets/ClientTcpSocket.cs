using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpSockets
{
    public interface IClientSocketConsumer<out TService> 
        where TService :ITcpService
    {
        TService GetConnection();
    }

    public class ClientTcpSocket<TTcpSerializer, TService> : IClientSocketConsumer<TService>
        where TTcpSerializer : ITcpSerializer, new() 
        where TService : class, ITcpClientService
    {
        private int _id;

        private readonly ISocketLog _log;
        private readonly IPEndPoint _ipEndPoint;
        private readonly int _reconnectTimeOut;

        private readonly Func<TService> _srvFactory;

        public const int PingInterval = 5;

        private TService _service;

        public ClientTcpSocket(ISocketLog log, IPEndPoint ipEndPoint, int reconnectTimeOut, Func<TService> srvFactory)
        {
            SocketStatistic = new SocketStatistic();
            _log = log;
            _ipEndPoint = ipEndPoint;
            _reconnectTimeOut = reconnectTimeOut;

            _srvFactory = srvFactory;

        }


        private bool _working;

        // Метод, который пытается сделать соединение с сервером
        private async Task<TcpConnection> Connect()
        {
            _log?.Add("Attempt To Connect:" + _ipEndPoint.Address + ":" + _ipEndPoint.Port);

            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(_ipEndPoint.Address, _ipEndPoint.Port);
            _service = _srvFactory();
            SocketStatistic.Init();
            var connection = new TcpConnection(_service, new TTcpSerializer(), tcpClient, SocketStatistic, _log, _id++);

            _log?.Add("Connected. Id=" + connection.Id);

            return connection;

        }

        private async Task SocketPingProcess(TcpConnection connection)
        {


            var lastSendPingTime = DateTime.UtcNow;
            try
            {
                while (!connection.Disconnected)
                {
                    await Task.Delay(500);

                    if ((DateTime.UtcNow - SocketStatistic.LastRecieveTime).TotalSeconds > PingInterval * 2)
                    {
                        Debug.WriteLine("Disconnect becouse of Ping");
                        _log?.Add("Long time [" + PingInterval * 2 + "] no recieve activity. Disconnect...");
                        await connection.Disconnect();
                    }
                    else
                        if ((DateTime.UtcNow - lastSendPingTime).TotalSeconds > PingInterval)
                        {
                            var pingData = _service.GetPingData();
                            Debug.WriteLine("Send Ping from Client");
                            await connection.SendDataToSocket(pingData);
                            lastSendPingTime = DateTime.UtcNow;
                        }
                }
            }
            catch (Exception exception)
            {
                await connection.Disconnect();
                _log?.Add("Ping Thread Exception: " + exception.Message);
            }
        }

        public async void SocketThread()
        {
            var makeSleep = false;

            while (_working)
            {

                try
                {

                    if (makeSleep)
                    {
                        _log?.Add("Connection Timeout...");
                        await Task.Delay(_reconnectTimeOut);
                    }

                    makeSleep = false;

                    // Пытаемся создать соединение с сервером
                    var connection = await Connect();

                    // Запускаем процесс чтения данных в другом потоке
                    await Task.WhenAny(
                        connection.StartReadDataAsync(),
                        SocketPingProcess(connection)
                        );

                }
                catch (SocketException se)
                {
                    if (se.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        _log?.Add("Connection support exception: " + se.Message);
                        makeSleep = true;
                    }

                }
                catch (Exception ex)
                {
                     _log?.Add("Connection support fatal exception:"+ex.Message);
                }
                finally
                {
                    _service = default(TService);
                }
            }

        }

        public void Start()
        {
            if (_working)
                throw new Exception("Client socket has already started");
            _working = true;
            SocketThread();
        }

        public bool Connected => _service != null;

        public SocketStatistic SocketStatistic { get; }

        TService IClientSocketConsumer<TService>.GetConnection()
        {
            return _service;
        }
    }
}
